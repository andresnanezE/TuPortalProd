using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiCertificadoTributario.Models
{
    public class ResultadoConsulta
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}