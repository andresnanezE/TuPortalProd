using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Cotizacion_Producto
    {
        [Key]
        public int CotizXProductID { get; set; }

        public int id_cotizacion { get; set; }
        public int id_producto { get; set; }
    }
}