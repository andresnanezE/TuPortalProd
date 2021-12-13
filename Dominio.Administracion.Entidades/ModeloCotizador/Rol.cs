namespace Datos.Administracion.UnidadDeTrabajo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("adm.Rol")]
    public partial class Rol
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rol()
        {
            RolUsuario = new HashSet<RolUsuario>();
        }

        [Key]
        public int IdRol { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        public bool? Estado { get; set; }

        public DateTime? Fecha { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RolUsuario> RolUsuario { get; set; }
    }
}