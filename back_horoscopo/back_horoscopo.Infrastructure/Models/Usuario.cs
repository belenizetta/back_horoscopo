using System;
using System.Collections.Generic;

namespace back_horoscopo.Infrastructure.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }
}
