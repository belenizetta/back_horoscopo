using AutoMapper;
using back_horoscopo.Application.DTOs;
using back_horoscopo.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Estadisticassigno, EstadisticasDTO>();
        }
    }

}
