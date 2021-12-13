using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloProcesos
{
    using System;
    using System.Collections.Generic;

    public partial class EMA_ROLXUSUARIO
    {
        [Key]
        public int ROLXUSUARIOID { get; set; }
        public int ROLID { get; set; }
        public int USUARIOID { get; set; }

        //public virtual EMA_ROL EMA_ROL { get; set; }
        //public virtual EMA_USUARIO EMA_USUARIO { get; set; }
    }
}