using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloProcesos
{
    using System;
    using System.Collections.Generic;

    public partial class EMA_ROL
    {
        //public EMA_ROL()
        //{
        //    this.EMA_ROLXMENU = new HashSet<EMA_ROLXMENU>();
        //    this.EMA_ROLXUSUARIO = new HashSet<EMA_ROLXUSUARIO>();
        //}

        [Key]
        public int ROLID { get; set; }
        public string ROL { get; set; }
        public System.DateTime FECHACREACION { get; set; }
        public bool ACTIVO { get; set; }
        public string USUARIOCREACION { get; set; }
        public string USUARIOMODIFICACION { get; set; }

        //public virtual ICollection<EMA_ROLXMENU> EMA_ROLXMENU { get; set; }
        //public virtual ICollection<EMA_ROLXUSUARIO> EMA_ROLXUSUARIO { get; set; }
    }
}