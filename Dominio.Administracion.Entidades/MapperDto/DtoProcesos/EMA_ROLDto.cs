namespace Dominio.Administracion.Entidades.MapperDto.DtoProcesos
{
    public class EMA_ROLDto
    {
        //public EMA_ROLDto()
        //{
        //    this.EMA_ROLXMENU = new HashSet<EMA_ROLXMENUDto>();
        //    this.EMA_ROLXUSUARIO = new HashSet<EMA_ROLXUSUARIODto>();
        //}

        public int ROLID { get; set; }
        public string ROL { get; set; }
        public System.DateTime FECHACREACION { get; set; }
        public bool ACTIVO { get; set; }
        public string USUARIOCREACION { get; set; }
        public string USUARIOMODIFICACION { get; set; }

        //public virtual ICollection<EMA_ROLXMENUDto> EMA_ROLXMENU { get; set; }
        //public virtual ICollection<EMA_ROLXUSUARIODto> EMA_ROLXUSUARIO { get; set; }
    }
}