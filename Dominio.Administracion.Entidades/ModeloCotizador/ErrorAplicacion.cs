namespace Datos.Administracion.UnidadDeTrabajo
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("adm.ErrorAplicacion")]
    public partial class ErrorAplicacion
    {
        [Key]
        public int IdErrorAplicacion { get; set; }

        [Column(TypeName = "text")]
        public string Mensaje { get; set; }

        [StringLength(50)]
        public string Tipo { get; set; }

        public DateTime? Fecha { get; set; }
    }
}