using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiCertificadoTributario.Models
{
    public class RmtContConsultados
    {
        public string NumeroDocumento { get; set; }
        public int Año { get; set; }
        public string Email { get; set; }
        public int RmtCont { get; set; }
    }
}