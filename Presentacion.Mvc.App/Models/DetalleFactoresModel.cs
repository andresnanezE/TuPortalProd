using Dominio.Administracion.Entidades.ModeloCotizacion;
using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Models
{
    public class DetalleFactoresModel
    {
        public int IdDetalleFactor { get; set; }
        public int IdFactor { get; set; }
        public int IdTipoFactor { get; set; }
        public int ValidarMsgDetalleFactor { get; set; }
        public string NombreDetalleFactor { get; set; }
        public string DescripcionDetalleFactor { get; set; }
        public string Factor { get; set; }
        public string EstadoDetalleFactor { get; set; }
        public string Mensaje { get; set; }
        public bool ValidarMdDetalleFactor { get; set; }

        public decimal ValorIndicadorFuga { get; set; }
        public decimal GastosAdministrativos { get; set; }
        public decimal GastosComerciales { get; set; }
        public decimal FactorUtilidad { get; set; }

        public decimal ValorMinimoCotizacion { get; set; }

        [DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public int? EstadoDetalleFactorID { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public string ValorConstanteFactorString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorConstanteFactor => decimal.TryParse(ValorConstanteFactorString, out decimal var) ? decimal.Parse(ValorConstanteFactorString) : 0;

        [RegularExpression("^[+-]?[0-9]{0,15}(?:/,[0-9]{0,2})?$", ErrorMessage = "Este campo acepta números enteros y dígitos con máximo 2 decimales después de la coma!")]
        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public string ValorExponenteFactorString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorExponenteFactor => decimal.TryParse(ValorExponenteFactorString, out decimal var) ? decimal.Parse(ValorExponenteFactorString) : 0;

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public string ValorFactor_AjusteString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal Factor_Ajuste => decimal.TryParse(ValorFactor_AjusteString, out decimal var) ? decimal.Parse(ValorFactor_AjusteString) : 0;

        //[DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public string ValorDetalleFactorString { get; set; }

        //[DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public decimal ValorDetalleFactor => decimal.TryParse(ValorDetalleFactorString, out decimal var) ? decimal.Parse(ValorDetalleFactorString) : 0;

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public string ValorDetalleConstanteString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorDetalleConstante => decimal.TryParse(ValorDetalleConstanteString, out decimal var) ? decimal.Parse(ValorDetalleConstanteString) : 0;

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public string ValorDetalleExponencialString { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorDetalleExponencial => decimal.TryParse(ValorDetalleExponencialString, out decimal var) ? decimal.Parse(ValorDetalleExponencialString) : 0;

        public IEnumerable<SelectListItem> ListEstadosDetalleFactor { get; set; }
        public IPagedList<Usp_ObtenerDetalleValorFactor> ListadoFactores { get; set; }
        public IEnumerable<Factores> Factores { get; set; }
    }
}