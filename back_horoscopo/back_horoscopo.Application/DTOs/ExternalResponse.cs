using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Application.DTOs
{
    public class ExternalResponse
    {
        public DateTime Date { get; set; }
        public string Horoscope { get; set; }
        public string Icon { get; set; }
        public int Id { get; set; }
        public string Sign  { get; set; }
    }
}
