using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCentralizada
{
    public class USR_APL_ROL
    {
        [Key]
        public int id_usr { get; set; }

        public int id_apl { get; set; }
        public int id_rol { get; set; }
    }
}