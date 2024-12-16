using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace back_horoscopo.Infrastructure.Models;

public partial class Estadisticassigno
{
    [Key]
    public string? Signo { get; set; }

    public int? CantidadConsultas { get; set; }
}
