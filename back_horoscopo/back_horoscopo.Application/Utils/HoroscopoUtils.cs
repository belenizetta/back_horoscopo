using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Application.Utils
{
    public static class HoroscopoUtils
    {
        public static string CalcularSignoZodiacal(DateTime fechaNacimiento)
        {
            int day = fechaNacimiento.Day;
            int month = fechaNacimiento.Month;

            return month switch
            {
                1 => day <= 19 ? "capricorn" : "aquarius",
                2 => day <= 18 ? "aquarius" : "pisces",
                3 => day <= 20 ? "pisces" : "aries",
                4 => day <= 19 ? "aries" : "taurus",
                5 => day <= 20 ? "taurus" : "gemini",
                6 => day <= 20 ? "gemini" : "cancer",
                7 => day <= 22 ? "cancer" : "leo",
                8 => day <= 22 ? "leo" : "virgo",
                9 => day <= 22 ? "virgo" : "libra",
                10 => day <= 22 ? "libra" : "scorpio",
                11 => day <= 21 ? "scorpio" : "sagittarius",
                12 => day <= 21 ? "sagittarius" : "capricorn",
                _ => throw new ArgumentException("Fecha de nacimiento inválida")
            };
        }

        public static int CalcularDiasParaCumple(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            var proximoCumple = new DateTime(hoy.Year, fechaNacimiento.Month, fechaNacimiento.Day);

            if (proximoCumple < hoy)
            {
                proximoCumple = proximoCumple.AddYears(1);
            }

            return (proximoCumple - hoy).Days;
        }

    }
}
