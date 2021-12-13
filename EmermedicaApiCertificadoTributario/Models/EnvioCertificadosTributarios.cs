using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiCertificadoTributario.Models
{
    public class EnvioCertificadosTributarios
    {
        public int IdEnvio { get; set; }
        public int ProcesoId { get; set; }
        public int AñoCertificado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string NumeroDocumento { get; set; }
        public string RmtCont { get; set; }
        public string TipoContrato { get; set; }
        public string Email { get; set; }

        public bool EstadoEnvio { get; set; }
        public bool EstadoError { get; set; }
        public DateTime FechaEnvio { get; set; }
        public string Mensaje { get; set; }
    }
}