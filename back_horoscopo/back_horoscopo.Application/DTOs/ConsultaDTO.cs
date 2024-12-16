using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Application.DTOs
{
    public class ConsultaDTO
    {
        public int Id { get; set; }

        public int Nombre { get; set; }

        public string Signo { get; set; }

        public DateTime FechaConsulta { get; set; }
    }
}
