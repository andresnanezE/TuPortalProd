using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloProcesos
{
    using System;
    using System.Collections.Generic;

    public partial class EMA_USUARIO
    {
        //public EMA_USUARIO()
        //{
        //    this.EMA_ROLXUSUARIO = new List<EMA_ROLXUSUARIO>();
        //}

        [Key]
        public int USUARIOID { get; set; }
        public string USUARIO { get; set; }
        public string TIPODOCUMENTO { get; set; }
        public string DOCUMENTO { get; set; }
        public string NOMBREUSUARIO { get; set; }
        public string CORREO { get; set; }
        public bool ACTIVO { get; set; }
        public Nullable<System.DateTime> FECHAULTIMASESION { get; set; }
        public System.DateTime FECHAREGISTRO { get; set; }
        public Nullable<System.DateTime> FECHAMODIFICACION { get; set; }
        public string USUARIOCREACION { get; set; }
        public string USUARIOMODIFICACION { get; set; }

        //public virtual IEnumerable<EMA_ROLXUSUARIO> EMA_ROLXUSUARIO { get; set; }
    }
}