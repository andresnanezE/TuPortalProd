using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Cotizacion_Asesor
    {
        [Key]
        public int CotizXAsesorID { get; set; }

        public int id_cotizacion { get; set; }
        public int id_asesor { get; set; }
    }
}