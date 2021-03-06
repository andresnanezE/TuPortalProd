using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class CiudadesFactor
    {
        [Key]
        public int Id { get; set; }

        public int Id_Factor { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        [DisplayFormat(DataFormatString = "{0:D9}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal ValorIndicadorFuga { get; set; }

        public decimal GastosAdministrativos { get; set; }
        public decimal GastosComerciales { get; set; }
        public decimal FactorUtilidad { get; set; }
        public decimal ValorMinimoCotizacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal Valor_Constante { get; set; }

        [DisplayFormat(DataFormatString = "{0:D2}", ApplyFormatInEditMode = true)]
        public decimal Valor_Exponencial { get; set; }

        public Boolean Estado { get; set; }
    }
}