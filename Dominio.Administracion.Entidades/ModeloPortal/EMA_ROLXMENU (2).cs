using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloProcesos
{
    using System;
    using System.Collections.Generic;

    public partial class EMA_ROLXMENU
    {
        [Key]
        public int ROLXMENUID { get; set; }
        public int ROLID { get; set; }
        public int MENUID { get; set; }

        //public virtual EMA_MENU EMA_MENU { get; set; }
        //public virtual EMA_ROL EMA_ROL { get; set; }
    }
}