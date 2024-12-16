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
    public class UsuarioData : IUsuarioData
    {
        private readonly HoroscopoContext _context;
        private readonly ILogger<UsuarioData> _logger;

        public UsuarioData(HoroscopoContext context, ILogger<UsuarioData> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> SaveUsuario(Usuario usuarioReq)
        {
            try
            {
                _logger.LogInformation("Starting to save or update user with email: {Email}", usuarioReq.Email);

                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == usuarioReq.Email);

                if (user == null)
                {
                    user = new Usuario
                    {
                        Nombre = usuarioReq.Nombre,
                        Email = usuarioReq.Email,
                        FechaNacimiento = usuarioReq.FechaNacimiento
                    };

                    _context.Usuarios.Add(user);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("User with email: {Email} successfully created.", usuarioReq.Email);
                }
                else
                {
                    _logger.LogInformation("User with email: {Email} already exists. Updating user information.", usuarioReq.Email);
                }

                return user.Id;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "A database error occurred while saving or updating the user.");
                throw new ApplicationException("An error occurred while saving the user. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while saving or updating the user.");
                throw new ApplicationException("An unexpected error occurred while saving the user.", ex);
            }
        }
    }
}
