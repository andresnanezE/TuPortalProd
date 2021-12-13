namespace Datos.Administracion.UnidadDeTrabajo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("adm.SiteMap")]
    public partial class SiteMap
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? IdFuncionalidad { get; set; }

        [StringLength(32)]
        public string Title { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        [StringLength(512)]
        public string Url { get; set; }

        [StringLength(512)]
        public string Roles { get; set; }

        public int? Parent { get; set; }

        public bool? Estado { get; set; }
    }
}