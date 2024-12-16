using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_horoscopo.Application.Excepciones
{
    public class ExternalApiException : Exception
    {
        public ExternalApiException(string message) : base(message) { }
        public ExternalApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}
