using back_horoscopo.Infrastructure.Interface;
using back_horoscopo.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Infrastructure.Implementation
{
    public class EstadisticaData : IEstadisticaData
    {
        private readonly HoroscopoContext _context;
        private readonly ILogger<EstadisticaData> _logger;

        public EstadisticaData(HoroscopoContext context, ILogger<EstadisticaData> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<string> SaveEstadisticas(string signo)
        {
            try
            {
                _logger.LogInformation("Starting to save or update statistics for the sign: {Signo}", signo);

                var estadisticas = await _context.Estadisticassignos
                    .FirstOrDefaultAsync(e => e.Signo == signo);

                if (estadisticas == null)
                {
                    _logger.LogInformation("No existing statistics found for the sign: {Signo}. Creating a new record.", signo);

                    estadisticas = new Estadisticassigno
                    {
                        Signo = signo,
                        CantidadConsultas = 1
                    };

                    await _context.Estadisticassignos.AddAsync(estadisticas);
                }
                else
                {
                    _logger.LogInformation("Existing statistics found for the sign: {Signo}. Incrementing the count.", signo);
                    estadisticas.CantidadConsultas++;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Statistics for the sign: {Signo} saved successfully. Total count: {CantidadConsultas}", signo, estadisticas.CantidadConsultas);

                return estadisticas.Signo;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "A database error occurred while saving statistics for the sign: {Signo}.", signo);
                throw new ApplicationException("An error occurred while updating the database. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while saving statistics for the sign: {Signo}.", signo);
                throw new ApplicationException("An unexpected error occurred while saving statistics.", ex);
            }
        }

        public async Task<Estadisticassigno> GetEstadisticas()
        {
            try
            {
                _logger.LogInformation("Fetching statistics from the database.");

                var estadisticas = await _context.Estadisticassignos.OrderByDescending(e => e.CantidadConsultas).FirstOrDefaultAsync();

                if (estadisticas == null)
                {
                    // Si no se encuentra ninguna estadística, retornamos un valor predeterminado
                    return new Estadisticassigno
                    {
                        Signo = "N/A",
                        CantidadConsultas = 0
                    };
                }

                return estadisticas;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "A database error occurred while fetching statistics.");
                throw new ApplicationException("An error occurred while fetching the statistics. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching statistics.");
                throw new ApplicationException("An unexpected error occurred while fetching the statistics.", ex);
            }
        }
    }

}
