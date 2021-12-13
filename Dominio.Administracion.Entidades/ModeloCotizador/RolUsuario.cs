namespace Datos.Administracion.UnidadDeTrabajo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("adm.RolUsuario")]
    public partial class RolUsuario
    {
        [Key]
        public int IdRolUsuario { get; set; }

        public int? IdUsuario { get; set; }

        public int? IdRol { get; set; }

        public virtual Rol Rol { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}