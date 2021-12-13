using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModelKheiron
{
    using System.Collections.Generic;

    public partial class EMB_TIPO_IDENTIFICACION
    {
        public EMB_TIPO_IDENTIFICACION()
        {
            this.EMB_PERSONA = new HashSet<EMB_PERSONA>();
        }

        [Key]
        public string ID_TIPO_IDENTIFICACION { get; set; }

        public string TIPO_IDENTIFICACION { get; set; }
        public string TIP_DOC_STONE { get; set; }

        public virtual ICollection<EMB_PERSONA> EMB_PERSONA { get; set; }
    }
}