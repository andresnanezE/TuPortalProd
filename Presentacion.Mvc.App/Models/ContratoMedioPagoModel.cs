using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentacion.Mvc.App.Models
{
    public class ContratoMedioPagoModel

    {
        public string NumCont { get; set; }

        public decimal Codigo { get; set; }
        public string Criterio { get; set; }
        public string Mensaje { get; set; }

        public string TipoContrato { get; set; }
        public string Rmt_Cont { get; set; }
        public string Identificacion { get; set; }
        public string Contratante { get; set; }
        public string Telefono { get; set; }

        public string Estado { get; set; }
        public string Cantidad { get; set; }
        public string Asesor { get; set; }
        public string IdentificacionAse { get; set; }

        public string FormaPago { get; set; }

        public string ModoPago { get; set; }

        public string TelefonoAse { get; set; }
        public string EstadoAse { get; set; }

        public string CuotaMensual { get; set; }
        public string ValorContrato { get; set; }
        public string ValorCartera { get; set; }

        #region Instance Properties

        public DateTime? FechaFin { get; set; }

        public DateTime? FechaInicio { get; set; }

        #endregion Instance Properties

        public string Num_idenBen { get; set; }

        public DateTime Fec_naciBen { get; set; }

        public string Nom_bene { get; set; }
        public string Tel_bene { get; set; }
        public string Titulo { get; set; }
        public decimal Busqueda { get; set; }

        public bool viewBene { get; set; }
        public bool viewPMP { get; set; }
        public bool viewAP { get; set; }

        public DateTime? fCorte { get; set; }

        public decimal valIvaRecurrente { get; set; }
        public decimal @valIvaProrrateo { get; set; }
        public decimal @valIvaProxCuota { get; set; }

        public int? diasFaltantes { get; set; }

        public int? diasProrrateo { get; set; }

        public string NOMBRE_RTE_ATENCIONES { get; set; }
    }
}