

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

    public class AdministracionAplicacionADominio : Profile
    {
        #region Instance Properties

        public override string ProfileName
        {
            get { return "AdministracionAplicacionADominio"; }
        }

        #endregion

        #region Instance Methods

        protected override void Configure()
        {
            //Mapper.CreateMap< DTO , ENTIDAD >();
            
            //Kheiron
            Mapper.CreateMap<EMB_PERSONADto, EMB_PERSONA>();
            Mapper.CreateMap<EMB_TIPO_IDENTIFICACIONDto,EMB_TIPO_IDENTIFICACION>();
            Mapper.CreateMap<EMB_USUARIODto, EMB_USUARIO>();

            //Proceos
            Mapper.CreateMap<EMA_LOGDto, EMA_LOG>();
            Mapper.CreateMap<EMA_MENUDto, EMA_MENU>();
            Mapper.CreateMap<EMA_ROLDto, EMA_ROL>();
            Mapper.CreateMap<EMA_ROLXMENUDto, EMA_ROLXMENU>();
            Mapper.CreateMap<EMA_ROLXUSUARIODto, EMA_ROLXUSUARIO>();
            Mapper.CreateMap<EMA_USUARIODto, EMA_USUARIO>();

            //General
            Mapper.CreateMap<UsuarioExternoDto, UsuarioExterno>();
            Mapper.CreateMap<MenuAplicacionDto, MenuAplicacion>();
        }

        #endregion
    }
}