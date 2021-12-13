using System.Collections.Generic;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Models
{
    public class FacturaAnexosViewModel
    {
        public string NumeroDocumento { get; set; }
        public string NumeroDocumento2 { get; set; }
        public int ContratoId { get; set; }
        public IEnumerable<SelectListItem> Contratos { get; set; }
        public List<ContratosFacturasViewModel> ContratosFacturas { get; set; }
        public int NUM_DOCU { get; set; }
        public int COD_DOCU { get; set; }
        public string PREFIJO { get; set; }
        public int TIPO_DOCUMENTO { get; set; }
        public string RMT_CONT { get; set; }

    }
}