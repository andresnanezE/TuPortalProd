using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class EstadosCotizacions
    {
        [Key]
        public int Id_Estado { get; set; }

        public string nombreEstado { get; set; }
    }
}