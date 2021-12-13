using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloUsuariosCentralizado
{
    public class UsuarioXappXrol
    {
        [Key]
        public int id_usuario { get; set; }

        public int id_aplicacion { get; set; }
        public int id_rol { get; set; }
    }
}