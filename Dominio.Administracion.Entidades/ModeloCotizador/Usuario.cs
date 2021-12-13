namespace Datos.Administracion.UnidadDeTrabajo
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("adm.Usuario")]
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            RolUsuario = new HashSet<RolUsuario>();
        }

        [Key]
        public int IdUsuario { get; set; }

        public short? EmpresaId { get; set; }

        public decimal? DirectorId { get; set; }

        public decimal? AsesorId { get; set; }

        [StringLength(100)]
        public string Login { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Password { get; set; }

        public bool? EstadoUsuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RolUsuario> RolUsuario { get; set; }
    }
}