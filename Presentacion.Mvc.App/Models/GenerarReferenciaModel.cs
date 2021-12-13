using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentacion.Mvc.App.Models
{
    public class GenerarReferenciaModel
    {
        public string Cedula { get; set; }
        public bool isInclusion { get; set; }
        public string NumeroContratoInclusion { get; set; }
        public string CedulaContratante { get; set; }
        public string CedulaBeneficiario { get; set; }
    }
}