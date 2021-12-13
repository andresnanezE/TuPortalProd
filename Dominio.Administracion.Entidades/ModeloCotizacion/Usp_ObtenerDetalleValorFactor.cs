using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Usp_ObtenerDetalleValorFactor
    {
        public int Id { get; set; }
        public int IdFactor { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        [Required()]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N9}")]
        public decimal Valor { get; set; }

        [Required()]
        public decimal Valor_Constante { get; set; }

        [Required()]
        public decimal Valor_Exponencial { get; set; }

        public decimal Factor_Ajuste { get; set; }
        public string Estado { get; set; }
        public int Id_TipoFactor { get; set; }

        public decimal ValorIndicadorFuga { get; set; }
        public decimal GastosAdministrativos { get; set; }
        public decimal GastosComerciales { get; set; }
        public decimal FactorUtilidad { get; set; }
        public decimal ValorMinimoCotizacion { get; set; }
    }
}