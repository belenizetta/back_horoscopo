using back_horoscopo.Application.DTOs;
using back_horoscopo.Application.Interface;
using back_horoscopo.Application.Services;
using back_horoscopo.Infrastructure.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace back_horoscopo.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("/")]
    public class HoroscopoController : Controller
    {
        private readonly IHoroscopoService _horoscopoService;
        private readonly ILogger<HoroscopoController> _logger;

        public HoroscopoController(IHoroscopoService horoscopoService, ILogger<HoroscopoController> logger) 
        { 
            _horoscopoService = horoscopoService;
            _logger = logger;
        }
        [HttpPost("horoscopo")]
        public async Task<ActionResult> GetHoroscopo([FromBody] HoroscopoRequest request)
        {
            try
            {
                _logger.LogInformation("Received request for horoscope with birth date: {FechaNacimiento}", request.Fecha_nacimiento);

                var response = await _horoscopoService.GetHoroscopo(request);

                _logger.LogInformation("Horoscope successfully fetched for sign: {Signo} and user: {Nombre}", response.Signo, request.Nombre);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing horoscope request for user: {Nombre} with birth date: {FechaNacimiento}", request.Nombre, request.Fecha_nacimiento);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpGet("estadisticas-signo")]
        public async Task<ActionResult> ObtenerEstadisticasSigno()
        {
            try
            {
                _logger.LogInformation("Fetching statistics for zodiac signs.");

                var estadisticas = await _horoscopoService.GetEstadisticas();

                _logger.LogInformation("Successfully fetched statistics for zodiac signs.");

                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching statistics for zodiac signs.");
                return StatusCode(500, "An unexpected error occurred while fetching statistics.");
            }
        }

        [HttpGet("historial-consultas")]
        public async Task<ActionResult<List<ConsultaDTO>>> ObtenerHistorialConsultas()
        {
            try
            {
                var historial = await _horoscopoService.GetHistorialConsultas();
                return Ok(historial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo historial de consultas.");
                return StatusCode(500, "Error al obtener historial de consultas.");
            }
        }

    }
}
