using AutoMapper;
using back_horoscopo.Application.DTOs;
using back_horoscopo.Application.Excepciones;
using back_horoscopo.Application.Mappers;
using back_horoscopo.Application.Services;
using back_horoscopo.Infrastructure.Interface;
using back_horoscopo.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Test
{
    public class HoroscopoServiceTests
    {
        private readonly Mock<IUsuarioData> _usuarioDataMock;
        private readonly Mock<IConsultaData> _consultaDataMock;
        private readonly Mock<IEstadisticaData> _estadisticaDataMock;
        private readonly Mock<MapperUsuario> _mapperUsuario;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<HoroscopoService>> _loggerMock;
        private readonly HoroscopoService _service;

        public HoroscopoServiceTests()
        {
            _usuarioDataMock = new Mock<IUsuarioData>();
            _consultaDataMock = new Mock<IConsultaData>();
            _estadisticaDataMock = new Mock<IEstadisticaData>();
            _mapperUsuario = new Mock<MapperUsuario>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<HoroscopoService>>();

            _service = new HoroscopoService(
                _estadisticaDataMock.Object,
                _consultaDataMock.Object,
                _usuarioDataMock.Object,
                _mapperUsuario.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetHoroscopo_SuccessfulRequest_ReturnsHoroscopoResponse()
        {
            // Arrange
            var request = new HoroscopoRequest { Email = "test@example.com", Fecha_nacimiento = DateTime.Now.AddYears(-25) };
            var signo = "Sagittarius";
            var usuarioId = 1;
            var externalResponse = new ExternalResponse { Horoscope = "You will have a great day!" };

            _mapperUsuario.Setup(m => m.MappingUsuarioDTO(It.IsAny<HoroscopoRequest>())).Returns(new Usuario());
            _usuarioDataMock.Setup(u => u.SaveUsuario(It.IsAny<Usuario>())).ReturnsAsync(usuarioId);
            _consultaDataMock.Setup(c => c.SaveConsulta(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(1);
            _estadisticaDataMock.Setup(e => e.SaveEstadisticas(It.IsAny<string>())).ReturnsAsync("Success");

            var httpClientMock = new Mock<HttpClient>();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(externalResponse))
            };
            httpClientMock.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(response);

            // Act
            var result = await _service.GetHoroscopo(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Sagittarius", result.Signo);
            Assert.Equal("You will have a great day!", result.Horoscopo);
            Assert.Equal(25, result.Cantidad_dias); // Assuming this calculation logic is valid.
        }

        [Fact]
        public async Task GetHoroscopo_ExternalApiFailure_ThrowsException()
        {
            // Arrange
            var request = new HoroscopoRequest { Email = "test@example.com", Fecha_nacimiento = DateTime.Now.AddYears(-25) };
            var signo = "Sagittarius";

            _mapperUsuario.Setup(m => m.MappingUsuarioDTO(It.IsAny<HoroscopoRequest>())).Returns(new Usuario());
            _usuarioDataMock.Setup(u => u.SaveUsuario(It.IsAny<Usuario>())).ReturnsAsync(1);
            _consultaDataMock.Setup(c => c.SaveConsulta(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(1);
            _estadisticaDataMock.Setup(e => e.SaveEstadisticas(It.IsAny<string>())).ReturnsAsync("Success");

            var httpClientMock = new Mock<HttpClient>();
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            httpClientMock.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(response);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetHoroscopo(request));
            Assert.Equal("Error al obtener el horóscopo desde la API externa.", exception.Message);
        }

        [Fact]
        public async Task GetHoroscopo_DeserializationFailure_ThrowsExternalApiException()
        {
            // Arrange
            var request = new HoroscopoRequest { Email = "test@example.com", Fecha_nacimiento = DateTime.Now.AddYears(-25) };
            var signo = "Sagittarius";

            _mapperUsuario.Setup(m => m.MappingUsuarioDTO(It.IsAny<HoroscopoRequest>())).Returns(new Usuario());
            _usuarioDataMock.Setup(u => u.SaveUsuario(It.IsAny<Usuario>())).ReturnsAsync(1);
            _consultaDataMock.Setup(c => c.SaveConsulta(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(1);
            _estadisticaDataMock.Setup(e => e.SaveEstadisticas(It.IsAny<string>())).ReturnsAsync("Success");

            var httpClientMock = new Mock<HttpClient>();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Invalid JSON Response")
            };
            httpClientMock.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(response);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ExternalApiException>(() => _service.GetHoroscopo(request));
            Assert.Equal("Invalid response from external API.", exception.Message);
        }

        [Fact]
        public async Task GetHoroscopo_UnhandledException_ThrowsApplicationException()
        {
            // Arrange
            var request = new HoroscopoRequest { Email = "test@example.com", Fecha_nacimiento = DateTime.Now.AddYears(-25) };

            _mapperUsuario.Setup(m => m.MappingUsuarioDTO(It.IsAny<HoroscopoRequest>())).Throws(new Exception("Unexpected error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.GetHoroscopo(request));
            Assert.Equal("An error occurred while processing the horoscope request. Please try again later.", exception.Message);
        }
    }
}
