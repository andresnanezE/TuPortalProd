//using Aplicacion.Administracion.Dto.DtoSitioWeb;
//using Aplicacion.Administracion.Dto.DtoStone.DtoSitioWeb;
using AutoMapper;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;

//using Aplicacion.Administracion.Dto.DtoKheiron;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos.DtoKheiron;
using Dominio.Administracion.Entidades.MapperDto.DtoSitioWeb;
using Dominio.Administracion.Entidades.MapperDto.DtoStone.DtoSitioWeb;
using Dominio.Administracion.Entidades.ModelKheiron;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Entidades.ModeloProcesos;
using Dominio.Administracion.Entidades.ModeloSitioWeb;

namespace Transversales.Administracion.IoC.Adaptadores
{
    public class AdministracionDominioAAplicacion : Profile
    {
        #region Instance Properties

        public override string ProfileName
        {
            get { return "AdministracionDominioAplicacion"; }
        }

        #endregion Instance Properties

        #region Instance Methods

        protected override void Configure()
        {
            //                fuente => detino
            //Mapper.CreateMap< ENTIDAD , DTO >();

            //Kheiron
            Mapper.CreateMap<EMB_PERSONA, EMB_PERSONADto>();
            Mapper.CreateMap<EMB_TIPO_IDENTIFICACION, EMB_TIPO_IDENTIFICACIONDto>();
            Mapper.CreateMap<EMB_USUARIO, EMB_USUARIODto>();

            //Procesos
            Mapper.CreateMap<EMA_LOG, EMA_LOGDto>();
            Mapper.CreateMap<EMA_MENU, EMA_MENUDto>();

            Mapper.CreateMap<Dominio.Administracion.Entidades.ModeloCentralizada.Roles, EMA_ROLDto>()
                .ForMember(x => x.ACTIVO, opt => opt.ResolveUsing(entity => entity.activo))
                .ForMember(x => x.ROL, opt => opt.ResolveUsing(entity => entity.nom_rol))
                .ForMember(x => x.ROLID, opt => opt.ResolveUsing(entity => entity.id_rol));
            //.ForMember(x => 1, opt => opt.ResolveUsing(entity => entity.id_apl));
            //TODO:nsarmiento colocar el valor del id de la aplicacion para ambos casos ↑↓
            Mapper.CreateMap<EMA_ROLXMENU, EMA_ROLXMENUDto>();
            Mapper.CreateMap<Dominio.Administracion.Entidades.ModeloCentralizada.USR_APL_ROL, EMA_ROLXUSUARIODto>()
                .ForMember(x => x.USUARIOID, opt => opt.ResolveUsing(entity => entity.id_usr))
                .ForMember(x => x.ROLID, opt => opt.ResolveUsing(entity => entity.id_rol));
            //.ForMember(x => 1, opt => opt.ResolveUsing(entity => entity.id_aplicacion));

            Mapper.CreateMap<Dominio.Administracion.Entidades.ModeloCentralizada.usrCentral, EMA_USUARIODto>()
                .ForMember(x => x.USUARIOID, opt => opt.ResolveUsing(entity => entity.id_usr))
                .ForMember(x => x.USUARIO, opt => opt.ResolveUsing(entity => entity.log_usr))
                .ForMember(x => x.NOMBREUSUARIO, opt => opt.ResolveUsing(entity => entity.nom_usr))
                .ForMember(x => x.TIPODOCUMENTO, opt => opt.ResolveUsing(entity => entity.tip_doc))
                .ForMember(x => x.DOCUMENTO, opt => opt.ResolveUsing(entity => entity.num_doc))
                .ForMember(x => x.CORREO, opt => opt.ResolveUsing(entity => entity.correo))
                .ForMember(x => x.CLAVE, opt => opt.ResolveUsing(entity => entity.clave))
                .ForMember(x => x.FECHAULTIMASESION, opt => opt.ResolveUsing(entity => entity.fec_ultima_sesion))
                .ForMember(x => x.FECHAEXPIRACLAVE, opt => opt.ResolveUsing(entity => entity.fec_expira_clave))
                .ForMember(x => x.FECHAREGISTRO, opt => opt.ResolveUsing(entity => entity.fec_regis))
                .ForMember(x => x.ACTIVO, opt => opt.ResolveUsing(entity => entity.activo));

            Mapper.CreateMap<EME_REGISTRO_VENTAS, VentasGanometroDto>()
                .ForMember(x => x.Id, opt => opt.ResolveUsing(ent => ent.ID))
                .ForMember(x => x.Anio, opt => opt.ResolveUsing(ent => ent.ANIO))
                .ForMember(x => x.Periodo, opt => opt.ResolveUsing(ent => ent.PERIODO))
                .ForMember(x => x.Semana, opt => opt.ResolveUsing(ent => ent.SEMANA))
                .ForMember(x => x.Dia, opt => opt.ResolveUsing(ent => ent.DIA))
                .ForMember(x => x.FechaVenta, opt => opt.ResolveUsing(ent => ent.FECHA))
                .ForMember(x => x.EsHabil, opt => opt.ResolveUsing(ent => ent.HABIL))
                .ForMember(x => x.IdCiudadHomologada, opt => opt.ResolveUsing(ent => ent.CIUD_HOMOL))
                .ForMember(x => x.IdDirector, opt => opt.ResolveUsing(ent => ent.ID_DIRECTOR))
                .ForMember(x => x.CantidadVentas, opt => opt.ResolveUsing(ent => ent.CANT_VENTAS))
                .ForMember(x => x.FechaIngreso, opt => opt.ResolveUsing(ent => ent.ACT_FECHA));
            Mapper.CreateMap<SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA, VentasDirectorPeriodoDto>();
            Mapper.CreateMap<PERIODO_VENTA, PeriodoVentaDto>();

            Mapper.CreateMap<EM_NOVEDAD_HOMOLOGADA, EM_NOVEDAD_HOMOLOGADADto>();
            Mapper.CreateMap<EM_FILTRO_AFILIACIONES, EM_FILTRO_AFILIACIONESDto>();
            Mapper.CreateMap<EM_FILTRO_OPCION_AFILIACIONES, EM_FILTRO_OPCION_AFILIACIONESDto>();
            Mapper.CreateMap<SPEM_CONSULTACONTRATOS, SPEM_CONSULTACONTRATOSDto>();
            Mapper.CreateMap<ConsultaBeneficiario, ConsultaBeneficiarioDto>();
            Mapper.CreateMap<ConsultaFactura, ConsultaFacturaDto>();
            Mapper.CreateMap<EMB_LogActividades, EmbLogActividadesDto>();
            Mapper.CreateMap<EMB_TipoLog, EMB_TipoLogDto>();
            Mapper.CreateMap<CadenaSupervision, CadenaSupervisionDto>();
            //Stone

            //General
            Mapper.CreateMap<UsuarioExterno, UsuarioExternoDto>();
            Mapper.CreateMap<MenuAplicacion, MenuAplicacionDto>();
            Mapper.CreateMap<CTB_TARIFAS_PLENAS, CTB_TARIFAS_PLENAS>();
            Mapper.CreateMap<AfiliacionesPeriodo, AfiliacionesPeriodoDto>();
            Mapper.CreateMap<resultadosConsultaAfiliacionResumen, resultadosConsultaAfiliacionResumenDto>();
            Mapper.CreateMap<ResultadosConsultaAfiliacionResumenTabla, ResultadosConsultaAfiliacionResumenTablaDto>();
            Mapper.CreateMap<AfiliacionesFiltro, AfiliacionesFiltroDto>();
            Mapper.CreateMap<DatosGeneralesFiltroAfiliaciones, DatosGeneralesFiltroAfiliacionesDto>();
            Mapper.CreateMap<NovedadesHomologadas, NovedadesHomologadasDto>();
            Mapper.CreateMap<Canales, CanalesDto>();
            Mapper.CreateMap<Ciudades, CiudadesDto>();
            Mapper.CreateMap<DYK_BANNER, DYK_BANNERDto>();
            Mapper.CreateMap<DYK_DESTACADO, DYK_DESTACADODto>();
            Mapper.CreateMap<DYK_NOTICIA, DYK_NOTICIADto>();
            Mapper.CreateMap<DYK_NOTICIA_SP, DYK_NOTICIA_SPDto>();

            Mapper.CreateMap<resultadosConsultaAfiliacionEstatus, resultadosConsultaAfiliacionEstatusDto>();
            Mapper.CreateMap<ConsultaDoble, ConsultaDobleDto>();

            Mapper.CreateMap<ComoVoy, ComoVoyDto>();
            Mapper.CreateMap<CalcularInclusiones, CalcularInclusionesDto>();
            Mapper.CreateMap<SolicitudesInternas, SolicitudesInternasDto>();
            Mapper.CreateMap<SolicitudesInternasNotas, SolicitudesInternasNotasDto>()
               .ForMember(x => x.Attach, opt => opt.Ignore());
            Mapper.CreateMap<SesionEnVezDeParametrosCrearUsuario, SesionEnVezDeParametrosCrearUsuarioDto>();
            Mapper.CreateMap<SesionEnVezDeUsuarioRecuperado, SesionEnVezDeUsuarioRecuperadoDto>();
            Mapper.CreateMap<SesionEnVezDe, SesionEnVezDeDto>();
            Mapper.CreateMap<SPCCBENEFINCLUSIONTUPORTAL, SPCCBENEFINCLUSIONTUPORTALDto>();
            Mapper.CreateMap<ValidarContratante, SPCCBENEFINCLUSIONTUPORTALDto>();
            Mapper.CreateMap<ValidarBeneficiario, SPCCBENEFINCLUSIONTUPORTALDto>();
        }

        #endregion Instance Methods
    }
}