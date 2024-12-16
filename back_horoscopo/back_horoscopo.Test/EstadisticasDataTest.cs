using back_horoscopo.Application.Services;
using back_horoscopo.Infrastructure.Implementation;
using back_horoscopo.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Test
{
    public class EstadisticasServiceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<HoroscopoContext> _contextMock;
        private readonly Mock<ILogger<EstadisticaData>> _loggerMock;
        private readonly EstadisticaData _service;

        public EstadisticasServiceTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public HoroscopoContext GetTestDbContext()
        {
            var options = new DbContextOptionsBuilder<HoroscopoContext>().UseMySql("Server=localhost;Database=TestDB;User=root;Password=;",
                  new MySqlServerVersion(new Version(8, 0, 33))) // Cambia según la versión de MySQL
        .Options;

            var context = new HoroscopoContext(options);
            context.Database.EnsureCreated(); // Crea la base de datos si no existe
            return context;
        }

        [Fact]
        public async Task SaveEstadisticas_ShouldCreateNewRecord_WhenSignDoesNotExist()
        {
            // Arrange
            var context = GetTestDbContext();
            var service = new EstadisticaData(context, Mock.Of<ILogger<EstadisticaData>>());

            string signo = "Scorpio";

            // Act
            var result = await service.SaveEstadisticas(signo);

            // Assert
            var estadistica = await context.Estadisticassignos.FirstOrDefaultAsync(e => e.Signo == signo);
            Assert.NotNull(estadistica);
            Assert.Equal(signo, estadistica.Signo);
            Assert.Equal(1, estadistica.CantidadConsultas);
        }


        [Fact]
        public async Task SaveEstadisticas_ShouldIncrementCount_WhenSignExists()
        {
            // Arrange
            var context = GetTestDbContext();
            var service = new EstadisticaData(context, Mock.Of<ILogger<EstadisticaData>>());

            string signo = "Piscis";
            await service.SaveEstadisticas(signo); // Primero guardamos el signo

            // Act
            var result = await service.SaveEstadisticas(signo); // Intentamos guardarlo de nuevo

            // Assert
            var estadistica = await context.Estadisticassignos.FirstOrDefaultAsync(e => e.Signo == signo);
            Assert.NotNull(estadistica);
            Assert.Equal(signo, estadistica.Signo);
            Assert.Equal(2, estadistica.CantidadConsultas); // Debería haber incrementado
        }


        [Fact]
        public async Task SaveEstadisticas_ShouldThrowApplicationException_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            var context = GetTestDbContext();
            var service = new EstadisticaData(context, Mock.Of<ILogger<EstadisticaData>>());

            string signo = "Leo";

            // Aquí simularás una falla de base de datos, como una violación de restricción
            // Puedes insertar datos que violen una restricción de la base de datos, por ejemplo, un valor único en un campo que lo restrinja.
            // También puedes lanzar manualmente una excepción en el contexto de base de datos.

            // Simulando una violación de base de datos (esto puede variar según tu contexto de base de datos)
            context.Database.BeginTransaction();
            try
            {
                var estadistica = new Estadisticassigno { Signo = signo, CantidadConsultas = 1 };
                context.Add(estadistica);
                context.SaveChanges();
                throw new DbUpdateException("Simulated database error"); // Lanza la excepción simulada aquí
            }
            catch (DbUpdateException ex)
            {
                // Assert
                var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.SaveEstadisticas(signo));
                Assert.Contains("An error occurred while updating the database", exception.Message);
            }
            finally
            {
                context.Database.RollbackTransaction();
            }
        }


        [Fact]
        public async Task SaveEstadisticas_ShouldThrowApplicationException_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var context = GetTestDbContext();
            var service = new EstadisticaData(context, Mock.Of<ILogger<EstadisticaData>>());

            // Simulamos una excepción inesperada lanzando una excepción directamente
            string signo = "Sagitario";
            await context.SaveChangesAsync(); // Forzamos el estado del contexto

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.SaveEstadisticas(signo));
            Assert.Contains("An unexpected error occurred", exception.Message);
        }


    }

}
