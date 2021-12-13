namespace Datos.Administracion.UnidadDeTrabajo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("adm.RolFuncionalidad")]
    public partial class RolFuncionalidad
    {
        [Key]
        public int IdRolFuncionalidad { get; set; }

        public int? IdFuncionalidad { get; set; }

        public int? IdRol { get; set; }
    }
}