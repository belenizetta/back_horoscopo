namespace back_horoscopo.Models
{
    public class HoroscopoRequest
    {
        string Nombre {  get; set; }
        string Email { get; set; }
        DateTime Fecha_nacimiento { get; set; }
    }
}
