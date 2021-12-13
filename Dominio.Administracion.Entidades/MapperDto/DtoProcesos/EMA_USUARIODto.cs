namespace Dominio.Administracion.Entidades.MapperDto.DtoProcesos
{
    using System;

    public class EMA_USUARIODto
    {
        //public EMA_USUARIODto()
        //{
        //    this.EMA_ROLXUSUARIO = new List<EMA_ROLXUSUARIODto>();
        //}

        public int USUARIOID { get; set; }
        public string USUARIO { get; set; }
        public string TIPODOCUMENTO { get; set; }
        public string DOCUMENTO { get; set; }
        public string NOMBREUSUARIO { get; set; }
        public string CORREO { get; set; }
        public string CLAVE { get; set; }
        public bool ACTIVO { get; set; }
        public Nullable<System.DateTime> FECHAULTIMASESION { get; set; }
        public Nullable<System.DateTime> FECHAEXPIRACLAVE { get; set; }
        public System.DateTime FECHAREGISTRO { get; set; }
        public Nullable<System.DateTime> FECHAMODIFICACION { get; set; }
        public string USUARIOCREACION { get; set; }
        public string USUARIOMODIFICACION { get; set; }
        public int ROL { get; set; }

        //public virtual IEnumerable<EMA_ROLXUSUARIODto> EMA_ROLXUSUARIO { get; set; }
    }
}