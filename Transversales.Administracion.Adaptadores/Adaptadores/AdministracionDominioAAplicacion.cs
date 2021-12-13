namespace Transversales.Administracion.Adaptadores
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Dominio.Administracion.Entidades;
    using Dominio.Administracion.Entidades.ModelKheiron;
    using Dominio.Administracion.Entidades.ModeloProcesos;
    using Aplicacion.Administracion.Dto;
    using Aplicacion.Administracion.Dto.DtoKheiron;
    using Aplicacion.Administracion.Dto.DtoProcesos;
    public class AdministracionDominioAAplicacion : Profile
    {
        #region Instance Properties

        public override string ProfileName
        {
            get { return "AdministracionDominioAplicacion"; }
        }

        #endregion

        #region Instance Methods

        protected override void Configure()
        {
            //Mapper.CreateMap< ENTIDAD , DTO >();
            
            //Kheiron
            Mapper.CreateMap<EMB_PERSONA,EMB_PERSONADto>();
            Mapper.CreateMap<EMB_TIPO_IDENTIFICACION,EMB_TIPO_IDENTIFICACIONDto>();
            Mapper.CreateMap<EMB_USUARIO,EMB_USUARIODto>();

            //Proceos
            Mapper.CreateMap<EMA_LOG,EMA_LOGDto>();
            Mapper.CreateMap<EMA_MENU,EMA_MENUDto >();
            Mapper.CreateMap<EMA_ROL,EMA_ROLDto>();
            Mapper.CreateMap<EMA_ROLXMENU,EMA_ROLXMENUDto>();
            Mapper.CreateMap<EMA_ROLXUSUARIO,EMA_ROLXUSUARIODto>();
            Mapper.CreateMap<EMA_USUARIO,EMA_USUARIODto>();

            //General
            Mapper.CreateMap<UsuarioExterno,UsuarioExternoDto>();
            Mapper.CreateMap<MenuAplicacion,MenuAplicacionDto>();
            
        }

        #endregion
    }
}