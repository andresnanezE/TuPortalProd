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
    public class AdministracionAplicacionADominio : Profile
    {
        #region Instance Properties

        public override string ProfileName
        {
            get { return "AdministracionAplicacionADominio"; }
        }

        #endregion Instance Properties

        #region Instance Methods

        protected override void Configure()
        {
            //                fuente => detino
            //Mapper.CreateMap< DTO , ENTIDAD >();

            //Kheiron
            Mapper.CreateMap<EMB_PERSONADto, EMB_PERSONA>();
            Mapper.CreateMap<EMB_TIPO_IDENTIFICACIONDto, EMB_TIPO_IDENTIFICACION>();
            Mapper.CreateMap<EMB_USUARIODto, EMB_USUARIO>();

            //Procesos
            Mapper.CreateMap<EMA_LOGDto, EMA_LOG>();
            Mapper.CreateMap<EMA_MENUDto, EMA_MENU>();
            Mapper.CreateMap<EMA_ROLDto, Dominio.Administracion.Entidades.ModeloCentralizada.Roles>()
                .ForMember(x => x.activo, opt => opt.ResolveUsing(entity => entity.ACTIVO))
                .ForMember(x => x.nom_rol, opt => opt.ResolveUsing(entity => entity.ROL))
                .ForMember(x => x.id_rol, opt => opt.ResolveUsing(entity => entity.ROLID))
                .ForMember(x => x.id_apl, opt => opt.Ignore());

            Mapper.CreateMap<EMA_ROLXMENUDto, EMA_ROLXMENU>();

            Mapper.CreateMap<EMA_ROLXUSUARIODto, Dominio.Administracion.Entidades.ModeloCentralizada.USR_APL_ROL>()
                .ForMember(x => x.id_usr, opt => opt.ResolveUsing(entity => entity.USUARIOID))
                .ForMember(x => x.id_rol, opt => opt.ResolveUsing(entity => entity.ROLID))
                .ForMember(x => x.id_apl, opt => opt.Ignore());
            //se cambia
            Mapper.CreateMap<EMA_USUARIODto, Dominio.Administracion.Entidades.ModeloCentralizada.usrCentral>()
                .ForMember(x => x.id_usr, opt => opt.ResolveUsing(entity => entity.USUARIOID))
                .ForMember(x => x.log_usr, opt => opt.ResolveUsing(entity => entity.USUARIO))
                .ForMember(x => x.nom_usr, opt => opt.ResolveUsing(entity => entity.NOMBREUSUARIO))
                .ForMember(x => x.tip_doc, opt => opt.ResolveUsing(entity => entity.TIPODOCUMENTO))
                .ForMember(x => x.num_doc, opt => opt.ResolveUsing(entity => entity.DOCUMENTO))
                .ForMember(x => x.correo, opt => opt.ResolveUsing(entity => entity.CORREO))
                .ForMember(x => x.clave, opt => opt.ResolveUsing(entity => entity.CLAVE))
                .ForMember(x => x.fec_ultima_sesion, opt => opt.ResolveUsing(entity => entity.FECHAULTIMASESION))
                .ForMember(x => x.fec_expira_clave, opt => opt.ResolveUsing(entity => entity.FECHAEXPIRACLAVE))
                .ForMember(x => x.fec_regis, opt => opt.ResolveUsing(entity => entity.FECHAREGISTRO))
                .ForMember(x => x.activo, opt => opt.ResolveUsing(entity => entity.ACTIVO));

            Mapper.CreateMap<VentasGanometroDto, EME_REGISTRO_VENTAS>()
                .ForMember(x => x.ID, opt => opt.ResolveUsing(ent => ent.Id))
                .ForMember(x => x.ANIO, opt => opt.ResolveUsing(ent => ent.Anio))
                .ForMember(x => x.PERIODO, opt => opt.ResolveUsing(ent => ent.Periodo))
                .ForMember(x => x.SEMANA, opt => opt.ResolveUsing(ent => ent.Semana))
                .ForMember(x => x.DIA, opt => opt.ResolveUsing(ent => ent.Dia))
                .ForMember(x => x.FECHA, opt => opt.ResolveUsing(ent => ent.FechaVenta))
                .ForMember(x => x.HABIL, opt => opt.ResolveUsing(ent => ent.EsHabil))
                .ForMember(x => x.CIUD_HOMOL, opt => opt.ResolveUsing(ent => ent.IdCiudadHomologada))
                .ForMember(x => x.ID_DIRECTOR, opt => opt.ResolveUsing(ent => ent.IdDirector))
                .ForMember(x => x.CANT_VENTAS, opt => opt.ResolveUsing(ent => ent.CantidadVentas))
                .ForMember(x => x.ACT_FECHA, opt => opt.ResolveUsing(ent => ent.FechaIngreso));
            Mapper.CreateMap<VentasDirectorPeriodoDto, SPEM_CONSULTARVENTASXPERIODOXSEMANAXDIA>();
            Mapper.CreateMap<PeriodoVentaDto, PERIODO_VENTA>();

            Mapper.CreateMap<EM_NOVEDAD_HOMOLOGADADto, EM_NOVEDAD_HOMOLOGADA>();
            Mapper.CreateMap<EM_FILTRO_AFILIACIONESDto, EM_FILTRO_AFILIACIONES>();
            Mapper.CreateMap<EM_FILTRO_OPCION_AFILIACIONESDto, EM_FILTRO_OPCION_AFILIACIONES>();
            Mapper.CreateMap<SPEM_CONSULTACONTRATOSDto, SPEM_CONSULTACONTRATOS>();
            Mapper.CreateMap<ConsultaBeneficiarioDto, ConsultaBeneficiario>();
            Mapper.CreateMap<ConsultaFacturaDto, ConsultaFactura>();
            Mapper.CreateMap<EmbLogActividadesDto, EMB_LogActividades>();
            Mapper.CreateMap<EMB_TipoLogDto, EMB_TipoLog>();
            Mapper.CreateMap<CadenaSupervisionDto, CadenaSupervision>();

            //Stone

            Mapper.CreateMap<CTB_TARIFAS_PLENAS, CTB_TARIFAS_PLENAS>();
            //General
            Mapper.CreateMap<UsuarioExternoDto, UsuarioExterno>();
            Mapper.CreateMap<MenuAplicacionDto, MenuAplicacion>();
            Mapper.CreateMap<AfiliacionesPeriodoDto, AfiliacionesPeriodo>();
            Mapper.CreateMap<resultadosConsultaAfiliacionResumenDto, resultadosConsultaAfiliacionResumen>();
            Mapper.CreateMap<ResultadosConsultaAfiliacionResumenTablaDto, ResultadosConsultaAfiliacionResumenTabla>();
            Mapper.CreateMap<AfiliacionesFiltroDto, AfiliacionesFiltro>();
            Mapper.CreateMap<DatosGeneralesFiltroAfiliacionesDto, DatosGeneralesFiltroAfiliaciones>();
            Mapper.CreateMap<NovedadesHomologadasDto, NovedadesHomologadas>();
            Mapper.CreateMap<ConsultaBeneficiarioDto, ConsultaBeneficiario>();
            Mapper.CreateMap<CanalesDto, Canales>();
            Mapper.CreateMap<CiudadesDto, Ciudades>();
            Mapper.CreateMap<ConsultaDobleDto, ConsultaDoble>();
            Mapper.CreateMap<ComoVoyDto, ComoVoy>();
            Mapper.CreateMap<CalcularInclusiones, CalcularInclusionesDto>();
            Mapper.CreateMap<SolicitudesInternas, SolicitudesInternasDto>();
            Mapper.CreateMap<SolicitudesInternasNotas, SolicitudesInternasNotasDto>()
                .ForMember(x => x.Attach, opt => opt.Ignore()); ;
            Mapper.CreateMap<SesionEnVezDeParametrosCrearUsuario, SesionEnVezDeParametrosCrearUsuarioDto>();
            Mapper.CreateMap<SesionEnVezDeUsuarioRecuperadoDto, SesionEnVezDeUsuarioRecuperado>();
            Mapper.CreateMap<SesionEnVezDeDto, SesionEnVezDe>();
            Mapper.CreateMap<DYK_BANNERDto, DYK_BANNER>();
            Mapper.CreateMap<DYK_DESTACADODto, DYK_DESTACADO>();
            Mapper.CreateMap<DYK_NOTICIADto, DYK_NOTICIA>();
            Mapper.CreateMap<DYK_NOTICIA_SPDto, DYK_NOTICIA_SP>();
            Mapper.CreateMap<SPCCBENEFINCLUSIONTUPORTALDto, SPCCBENEFINCLUSIONTUPORTAL>();
            //Mapper.CreateMap<SPCCBENEFINCLUSIONTUPORTALDto, ValidarContratante>()
            //    .ForMember(x => x.Cod_Empr, opt => opt.ResolveUsing(entity => entity.Cod_Empr))
            //    .ForMember(x => x.Rmt_Cont, opt => opt.ResolveUsing(entity => entity.Rmt_Cont))
            //    .ForMember(x => x.Num_Cont, opt => opt.ResolveUsing(entity => entity.Num_Cont))
            //    .ForMember(x => x.Est_Cont, opt => opt.ResolveUsing(entity => entity.Est_Cont))
            //    .ForMember(x => x.Fec_Inic, opt => opt.ResolveUsing(entity => entity.Fec_Inic))
            //    .ForMember(x => x.Fec_Venc, opt => opt.ResolveUsing(entity => entity.Fec_Venc))
            //    .ForMember(x => x.Doc_Admi, opt => opt.ResolveUsing(entity => entity.Doc_Admi))
            //    .ForMember(x => x.Tip_Deau, opt => opt.ResolveUsing(entity => entity.Tip_Deau))
            //    .ForMember(x => x.Mod_Pago, opt => opt.ResolveUsing(entity => entity.Mod_Pago))
            //    .ForMember(x => x.Vig_Cont, opt => opt.ResolveUsing(entity => entity.Vig_Cont))
            //    .ForMember(x => x.Mod_PagoT, opt => opt.ResolveUsing(entity => entity.Mod_PagoT))
            //    .ForMember(x => x.Tip_Pago, opt => opt.ResolveUsing(entity => entity.Tip_Pago))
            //    .ForMember(x => x.Tar_Cont, opt => opt.ResolveUsing(entity => entity.Tar_Cont))
            //    .ForMember(x => x.Cod_Contra, opt => opt.ResolveUsing(entity => entity.Cod_Contra))
            //    .ForMember(x => x.Nom_Contra, opt => opt.ResolveUsing(entity => entity.Nom_Contra))
            //    .ForMember(x => x.Tel_Terc, opt => opt.ResolveUsing(entity => entity.Tel_Terc))
            //    .ForMember(x => x.Nro_Celu, opt => opt.ResolveUsing(entity => entity.Nro_Celu))
            //    .ForMember(x => x.Saldo_Cartera, opt => opt.ResolveUsing(entity => entity.Saldo_Cartera))
            //    .ForMember(x => x.Con_Mora, opt => opt.ResolveUsing(entity => entity.Con_Mora))
            //    .ForMember(x => x.Fec_Mora, opt => opt.ResolveUsing(entity => entity.Fec_Mora))
            //    .ForMember(x => x.Cal_Terc, opt => opt.ResolveUsing(entity => entity.Cal_Terc));
            Mapper.CreateMap<SPCCBENEFINCLUSIONTUPORTALDto, ValidarContratante>();

            Mapper.CreateMap<SPCCBENEFINCLUSIONTUPORTALDto, ValidarBeneficiario>();
            //Mapper.CreateMap<SPCCBENEFINCLUSIONTUPORTALDto, ValidarBeneficiario>()
            //    .ForMember(x => x.Tar_Bene, opt => opt.ResolveUsing(entity => entity.Tar_Bene))
            //    .ForMember(x => x.Rmt_Rccc, opt => opt.ResolveUsing(entity => entity.Rmt_Rccc))
            //    .ForMember(x => x.Rmt_Rccb, opt => opt.ResolveUsing(entity => entity.Rmt_Rccb))
            //    .ForMember(x => x.Est_Bene, opt => opt.ResolveUsing(entity => entity.Est_Bene))
            //    .ForMember(x => x.Nro_Bene, opt => opt.ResolveUsing(entity => entity.Nro_Bene))
            //    .ForMember(x => x.Nom_Bene, opt => opt.ResolveUsing(entity => entity.Nom_Bene))
            //    .ForMember(x => x.Ape_Bene, opt => opt.ResolveUsing(entity => entity.Ape_Bene))
            //    .ForMember(x => x.Tel_Bene, opt => opt.ResolveUsing(entity => entity.Tel_Bene))
            //    .ForMember(x => x.Cal_Bene, opt => opt.ResolveUsing(entity => entity.Cal_Bene));
        }

        #endregion Instance Methods
    }
}