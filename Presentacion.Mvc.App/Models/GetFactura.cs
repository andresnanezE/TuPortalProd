using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentacion.Mvc.App.Models
{
    public class GetFactura
    {
        public string NumeroDocumento2 { get; set; }
        public string NumeroContrato { get; set; }
        public string ConsecutivoFactura { get; set; }
        public string Prefijo { get; set; }
        public string TipoDocumento { get; set; }
    }
}