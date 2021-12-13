namespace Datos.Administracion.UnidadDeTrabajo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("adm.Funcionalidad")]
    public partial class Funcionalidad
    {
        [Key]
        public int IdFuncionalidad { get; set; }

        [StringLength(500)]
        public string Nombre { get; set; }
    }
}