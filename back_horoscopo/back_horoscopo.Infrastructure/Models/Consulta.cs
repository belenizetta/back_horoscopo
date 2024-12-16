using System;
using System.Collections.Generic;

namespace back_horoscopo.Infrastructure.Models;

public partial class Consulta
{
    public int Id { get; set; }

    public int UduarioId { get; set; }

    public string Signo { get; set; } = null!;

    public DateTime FechaConsulta { get; set; }
}
