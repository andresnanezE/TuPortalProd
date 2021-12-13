using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class NivelesInteres
    {
        [Key]
        public int id { get; set; }

        public string interes { get; set; }
    }
}