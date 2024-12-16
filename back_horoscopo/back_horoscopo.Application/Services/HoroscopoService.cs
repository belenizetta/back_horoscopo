using back_horoscopo.Application.DTOs;
using back_horoscopo.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using back_horoscopo.Infrastructure.Interface;
using back_horoscopo.Application.Mappers;
using AutoMapper;
using back_horoscopo.Application.Utils;
using Microsoft.Extensions.Logging;
using back_horoscopo.Application.Excepciones;

namespace back_horoscopo.Application.Services
{

    public class HoroscopoService : IHoroscopoService
    {
        private readonly IUsuarioData _usuarioData;
        private readonly IConsultaData _consultaData;
        private readonly IEstadisticaData _estadisticaData;
        private readonly MapperUsuario _mapperUsuario;
        private readonly IMapper _mapper;
        private readonly ILogger<HoroscopoService> _logger;

        public HoroscopoService (IEstadisticaData estadisticaData, IConsultaData consultaData, IUsuarioData usuarioData, MapperUsuario mapperUsuario, IMapper mapper, ILogger<HoroscopoService> logger)
        {
             _usuarioData = usuarioData;
            _consultaData = consultaData;
            _estadisticaData = estadisticaData;
            _mapperUsuario = mapperUsuario;
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<HoroscopoResponse> GetHoroscopo(HoroscopoRequest request)
        {
            try
            {
                _logger.LogInformation("Processing horoscope request for email: {Email}, birthdate: {Birthdate}", request.Email, request.Fecha_nacimiento);

                string signo = HoroscopoUtils.CalcularSignoZodiacal(request.Fecha_nacimiento);
                _logger.LogInformation("Zodiac sign calculated: {Signo}", signo);

                var usuario = _mapperUsuario.MappingUsuarioDTO(request);
                var usuarioId = await _usuarioData.SaveUsuario(usuario);
                _logger.LogInformation("User saved successfully with ID: {UsuarioId}", usuarioId);

                await _consultaData.SaveConsulta(usuarioId, signo);
                _logger.LogInformation("Consulta saved successfully for user ID: {UsuarioId} and sign: {Signo}", usuarioId, signo);

                await _estadisticaData.SaveEstadisticas(signo);
                _logger.LogInformation("Statistics updated successfully for sign: {Signo}", signo);

                var requestBody = new
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd"),
                    lang = "es",
                    sign = signo
                };

                var jsonBody = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                _logger.LogInformation("Request body prepared for external API: {RequestBody}", jsonBody);

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync("https://newastro.vercel.app", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to fetch horoscope. Status code: {StatusCode}, Reason: {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
                        throw new Exception("Error al obtener el horóscopo desde la API externa.");
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Response received from external API: {ResponseContent}", responseContent);
                    var externalResponse = JsonConvert.DeserializeObject<ExternalResponse>(responseContent);
                    if (externalResponse == null)
                    {
                        _logger.LogError("Failed to deserialize external API response.");
                        throw new ExternalApiException("Invalid response from external API.");
                    }

                    int diasParaCumple = HoroscopoUtils.CalcularDiasParaCumple(request.Fecha_nacimiento);
                    _logger.LogInformation("Days until birthday calculated: {DiasParaCumple}", diasParaCumple);

                    return new HoroscopoResponse
                    {
                        Signo = signo,
                        Horoscopo = externalResponse.Horoscope, 
                        Cantidad_dias = diasParaCumple
                    };
                }
            }
            catch (ExternalApiException ex)
            {
                _logger.LogError(ex, "An external API exception occurred.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing the horoscope request.");
                throw new ApplicationException("An error occurred while processing the horoscope request. Please try again later.", ex);
            }
        }




        public async Task<EstadisticasDTO> GetEstadisticas()
        {
            try
            {
                _logger.LogInformation("Starting to fetch statistics from the database.");

                var estadisticas = await _estadisticaData.GetEstadisticas();


                return _mapper.Map<EstadisticasDTO>(estadisticas);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error occurred while converting statistics to DTO.");
                throw new ApplicationException("An error occurred while mapping statistics to DTO.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching statistics.");
                throw new ApplicationException("An error occurred while retrieving statistics. Please try again later.", ex);
            }

        }

        public async Task<List<ConsultaDTO>> GetHistorialConsultas()
        {
            try
            {
                _logger.LogInformation("Iniciando la obtención del historial de consultas.");

                var consultas = await _consultaData.GetConsultas();
                if (consultas == null || !consultas.Any())
                {
                    _logger.LogWarning("No se encontraron registros de consultas en la base de datos.");
                    return new List<ConsultaDTO>();
                }

                var consultaDTOs = consultas.Select(c => new ConsultaDTO
                {
                    Nombre = c.UduarioId,
                    Signo = c.Signo,
                    FechaConsulta = c.FechaConsulta
                }).ToList();

                _logger.LogInformation($"Se obtuvieron {consultaDTOs.Count} registros de historial de consultas.");
                return consultaDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el historial de consultas: {ex.Message}", ex);
                throw new ApplicationException("Hubo un error al obtener el historial de consultas.", ex);
            }
        }
    }
}
