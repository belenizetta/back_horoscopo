using back_horoscopo.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Infrastructure.Interface
{
    public interface IConsultaData
    {
        Task<int> SaveConsulta(int usuario, string signo);

        Task<List<Consulta>> GetConsultas();
    }
}
