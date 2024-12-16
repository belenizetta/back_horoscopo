
using back_horoscopo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Application.Interface
{
    public interface IHoroscopoService
    {
        Task<HoroscopoResponse> GetHoroscopo (HoroscopoRequest request);

        Task<EstadisticasDTO> GetEstadisticas();

        Task<List<ConsultaDTO>> GetHistorialConsultas();
    }
}
