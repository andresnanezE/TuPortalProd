namespace Dominio.Administracion.Entidades.MapperDto.DtoProcesos
{
    using System;

    public class EMA_MENUDto
    {
        //public EMA_MENUDto()
        //{
        //    this.EMA_MENU1 = new HashSet<EMA_MENUDto>();
        //    this.EMA_ROLXMENU = new HashSet<EMA_ROLXMENUDto>();
        //}

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
        public string ICONO { get; set; }
        //public virtual ICollection<EMA_MENUDto> EMA_MENU1 { get; set; }
        //public virtual EMA_MENUDto EMA_MENU2 { get; set; }
        //public virtual ICollection<EMA_ROLXMENUDto> EMA_ROLXMENU { get; set; }
    }
}