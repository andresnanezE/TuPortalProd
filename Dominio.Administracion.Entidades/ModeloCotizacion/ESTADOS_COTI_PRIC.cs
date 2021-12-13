using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class ESTADOS_COTI_PRIC
    {
        [Key]
        public int Id_Estado { get; set; }

        public string Descripcion { get; set; }
    }
}