using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Application.DTOs
{
    public class HoroscopoResponse
    {
        public string Signo {  get; set; }
        public int Cantidad_dias { get; set; }

        public string Horoscopo {  get; set; }
    }
}
