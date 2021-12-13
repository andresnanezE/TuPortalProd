using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCentralizada
{
    public class Roles
    {
        [Key]
        public int id_rol { get; set; }

        public int id_apl { get; set; }
        public string nom_rol { get; set; }
        public bool activo { get; set; }
    }
}