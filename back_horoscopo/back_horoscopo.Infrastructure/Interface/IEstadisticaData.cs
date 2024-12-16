﻿using back_horoscopo.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Infrastructure.Interface
{
    public interface IEstadisticaData
    {
        Task<string> SaveEstadisticas(string signo);

        Task<Estadisticassigno> GetEstadisticas();    
    }
}