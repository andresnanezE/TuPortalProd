using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Models
{
    public class ContratosFacturasViewModel
    {
        #region Instance Properties

        public DateTime FECHA_INICIAL { get; set; }
        public DateTime FECHA_FINAL { get; set; }
        public DateTime FEC_INIC { get; set; }
        public string NOM_TIPC { get; set; }
        public int NUM_CONT { get; set; }
        public Int16 NUM_FACT { get; set; }
        public string PERIODO_FACTURA { get; set; }
        public int RMT_CONT { get; set; }
        public decimal VAL_SALD { get; set; }
        public decimal VAL_TOTA { get; set; }
        public int NUM_DOCU { get; set; }
        public int RMT_CAFA { get; set; }
        public int COD_DOCU { get; set; }

        public static implicit operator List<object>(ContratosFacturasViewModel v)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> Contratos { get; set; }
        public string PREFIJO { get; set; }
        public int TIPO_DOCUMENTO { get; set; }

        #endregion Instance Properties
    }
}