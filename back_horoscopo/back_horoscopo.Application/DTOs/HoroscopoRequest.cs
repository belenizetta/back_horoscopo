using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Application.DTOs
{
    public class HoroscopoRequest
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public DateTime Fecha_nacimiento { get; set; }
    }
}
