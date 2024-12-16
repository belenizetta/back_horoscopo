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
    public class ConsultaData : IConsultaData
    {
        private readonly HoroscopoContext _context;
        private readonly ILogger<ConsultaData> _logger;

        public ConsultaData(HoroscopoContext context, ILogger<ConsultaData> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> SaveConsulta(int usuario, string signo)
        {
            try
            {
                _logger.LogInformation("Starting to save consultation for user ID: {UsuarioId} and sign: {Signo}", usuario, signo);

                var con = new Consulta
                {
                    UduarioId = usuario,
                    Signo = signo,
                    FechaConsulta = DateTime.Now
                };

                _context.Consultas.Add(con);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Consultation saved successfully for user ID: {UsuarioId} and sign: {Signo}", usuario, signo);

                return con.Id;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "A database error occurred while saving the consultation for user ID: {UsuarioId} and sign: {Signo}", usuario, signo);
                throw new ApplicationException("An error occurred while saving the consultation. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while saving the consultation for user ID: {UsuarioId} and sign: {Signo}", usuario, signo);
                throw new ApplicationException("An unexpected error occurred while saving the consultation.", ex);
            }
        }

        public async Task<List<Consulta>> GetConsultas ()
        {
            try
            {
                _logger.LogInformation("Fetching consult from the database.");

                var estadisticas = await _context.Consultas.ToListAsync();

                _logger.LogInformation("Successfully fetched {Count} records from the consult table.", estadisticas.Count);

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
