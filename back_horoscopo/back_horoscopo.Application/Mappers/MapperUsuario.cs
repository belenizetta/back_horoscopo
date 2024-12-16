using back_horoscopo.Application.DTOs;
using back_horoscopo.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Application.Mappers
{
    public class MapperUsuario
    {
        public virtual Usuario MappingUsuarioDTO(HoroscopoRequest request)
        {
            var Usuario = new Usuario()
            {
                Nombre = request.Nombre,
                Email = request.Email,
                FechaNacimiento = request.Fecha_nacimiento,
            };

            return Usuario;
        }
    }
}
