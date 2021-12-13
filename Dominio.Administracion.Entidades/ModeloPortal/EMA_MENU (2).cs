using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloProcesos
{
    using System;
    using System.Collections.Generic;

    public partial class EMA_MENU
    {
        //public EMA_MENU()
        //{
        //    this.EMA_MENU1 = new HashSet<EMA_MENU>();
        //    this.EMA_ROLXMENU = new HashSet<EMA_ROLXMENU>();
        //}

        [Key]
        public int MENUID { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<int> NODOPADREID { get; set; }
        public string CONTROLLER { get; set; }
        public string ACTION { get; set; }
        public string URL { get; set; }
        public System.DateTime FECHACREACION { get; set; }
        public Nullable<System.DateTime> FECHAMODIFICACION { get; set; }
        public string USUARIOCREACION { get; set; }
        public string USUARIOMODIFICACION { get; set; }

        //public virtual ICollection<EMA_MENU> EMA_MENU1 { get; set; }
        //public virtual EMA_MENU EMA_MENU2 { get; set; }
        //public virtual ICollection<EMA_ROLXMENU> EMA_ROLXMENU { get; set; }
    }
}