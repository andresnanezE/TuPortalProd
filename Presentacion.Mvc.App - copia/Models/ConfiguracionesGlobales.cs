using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Presentacion.Mvc.App.Models
{
    public static class ConfiguracionesGlobales
    {

        public static string Archivos
        {
            get { return ConfigurationManager.AppSettings["Archivos"]; }
        }

        public static string FirmaSoporteSeguridadSocialRuta
        {
            get { return ConfigurationManager.AppSettings["LOGS:FirmaSSS"]; }
        }

        public static string ReportesReportServerUrl
        {
            get { return ConfigurationManager.AppSettings["MvcReportViewer.ReportServerUrl"]; }
        }

        public static string ReportesUsername
        {
            get { return ConfigurationManager.AppSettings["MvcReportViewer.Username"]; }
        }

        public static string ReportesPassword
        {
            get { return ConfigurationManager.AppSettings["MvcReportViewer.Password"]; }
        }

        public static string ReportPathDetallesAfiliaciones
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathDetallesAfiliaciones"]; }
        }
        public static string ReportPathAfiliacionesResumenAsesores
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathAfiliacionesResumenAsesores"]; }
        }
        public static string ReportPathAfiliacionesResumenDirectores
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathAfiliacionesResumenDirectores"]; }
        }
        public static string ReportPathAfiliacionesResumenCadenaSupervision
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathAfiliacionesResumenCadenaSupervision"]; }
        }
        public static string ReportPathAfiliacionesComisionesAsesor
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathAfiliacionesComisionesAsesor"]; }
        }

        public static string ReportPathAfiliacionesComisionesPendientesAsesor
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathAfiliacionesComisionesPendientesAsesor"]; }
        }

        public static string ReportPathExtractoAnticiposAsesor
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathExtractoAnticiposAsesor"]; }
        }

        public static string ReportPathDetallesBajas
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathDetallesBajas"]; }
        }


        public static string ReportPathDetalleReactivaciones
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathDetalleReactivaciones"]; }
        }

        public static string ReportPathResumenReactivaciones
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathResumenReactivaciones"]; }
        }

        public static string ReportPathResumenBajas
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathResumenBajas"]; }
        }

        public static string ReportPathResumenAfiliadosAcumulados
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathResumenAfiliadosAcumulados"]; }
        }
        public static string ReportPathDetallesAfiliadosAcumulados
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathDetallesAfiliadosAcumulados"]; }
        }
        public static string ReportPathAfiliacionesComisionesAsesorExcel
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathAfiliacionesComisionesAsesorExcel"]; }
        }

        public static string ReportPathAnexoFacturaContrato
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathAnexoFacturaContrato"]; }
        }

        public static string ReportPathFacturaContrato
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathFacturaContrato"]; }
        }


        public static string ReportesReportPathRelacionAfiliados
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathRelacionAfiliados"]; }
        }

        public static string ReportesReportPathRelacionCancelados
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportesReportPathRelacionCancelados"]; }
        }

        public static string ReportesReportPathAtenciones
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportesReportPathAtenciones"]; }
        }

        public static string ReportPathRelacionBeneficiarios
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathRelacionBeneficiarios"]; }
        }

        public static string ReportPathEstadoCuenta
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathEstadoCuenta"]; }
        }

        public static string ReportPathTarjetasTrasmilenio
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathTarjetasTrasmilenio"]; }
        }

        public static string ReportPathApoyoRodamiento
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathApoyoRodamiento"]; }
        }

        public static string ReportPathSabanaAsesores
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathSabanaAsesores"]; }
        }

        public static string ReportPathSabanaAsesoresEstatus
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathSabanaAsesoresEstatus"]; }
        }

        public static string ReportPathSabanaAsesoresDetallado
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathSabanaAsesoresDetallado"]; }
        }

        public static string SolicitudesIntrernasPathXmls
        {
            get { return ConfigurationManager.AppSettings["SolicitudesIntrernasPathXmls"]; }
        }

        // Solicitudesd Internas FreshService :
        public static string domainFresh
        {
            get { return ConfigurationManager.AppSettings["domainFresh"]; }
        }

        public static string domainFreshAttach
        {
            get { return ConfigurationManager.AppSettings["domainFreshAttach"]; }
        }

        public static string tokenUser
        {
            get { return ConfigurationManager.AppSettings["tokenUser"]; }
        }
        public static string accountId
        {
            get { return ConfigurationManager.AppSettings["accountId"]; }
        }
        public static string Subject
        {
            get { return ConfigurationManager.AppSettings["subject"]; }
        }
        public static string SolicitudesInternasEmailPruebas
        {
            get { return ConfigurationManager.AppSettings["emailpruebas"]; }
        }

        // Fin Solicitudesd Internas FreshService

        // Como Voy :
        public static string pathImagenesComoVoy
        {
            get { return ConfigurationManager.AppSettings["pathImagenesComoVoy"]; }
        }

        public static string urlActionDownloadPdf
        {
            get { return ConfigurationManager.AppSettings["urlActionDownloadPdf"]; }
        }
        public static string LetraDiscoPdfs
        {
            get { return ConfigurationManager.AppSettings["letraDiscoPdfs"]; }
        }

        public static string urlCotizadorInterna
        {
            get { return ConfigurationManager.AppSettings["urlCotizadorInterna"]; }
        }

        public static string urlCotizadorExterna
        {
            get { return ConfigurationManager.AppSettings["urlCotizadorExterna"]; }
        }

        public static string descMenuInterno
        {
            get { return ConfigurationManager.AppSettings["descMenuInterno"]; }
        }

        public static string descMenuExterno
        {
            get { return ConfigurationManager.AppSettings["descMenuExterno"]; }
        }

        public static string urlPortalPruebas
        {
            get { return ConfigurationManager.AppSettings["urlPortalPruebas"]; }
        }

        public static string urlPortalProduccion
        {
            get { return ConfigurationManager.AppSettings["urlPortalProduccion"]; }
        }

        public static string urlPortalDesarrollo
        {
            get { return ConfigurationManager.AppSettings["urlPortalDesarrollo"]; }
        }

        // Fin Cotizador Juridicos

        public static string urlFOSYGA
        {
            get { return ConfigurationManager.AppSettings["urlFOSYGA"]; }
        }

        public static string ComisionesAsesorProceso
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathComisionesAsesorProceso"]; }
        }


        //No eliminar y mantener.
        public static string EmermedicaWsUrl
        {
            get { return ConfigurationManager.AppSettings["EmermedicaWsUrl"]; }
        }

        public static string EmermedicaWsUsr
        {
            get { return ConfigurationManager.AppSettings["EmermedicaWsUsr"]; }
        }

        public static string EmermedicaWsPwd
        {
            get { return ConfigurationManager.AppSettings["EmermedicaWsPwd"]; }
        }

        //REPORTES:ReportPathComisionesAsesores
        public static string ReportPathComisionesAsesores { get { return ConfigurationManager.AppSettings["REPORTES:ReportPathComisionesAsesores"]; } }

        public static string ReporteLogSesionEnVezDe { get { return ConfigurationManager.AppSettings["REPORTES:ReporteLogSesionEnVezDe"]; } }

       

        // Roles:
        public static string RolAsesor { get { return ConfigurationManager.AppSettings["RolAsesor"]; } }
        public static string RolDirector { get { return ConfigurationManager.AppSettings["RolDirector"]; } }
        public static string RolCadenaSupervision { get { return ConfigurationManager.AppSettings["RolCadenaSupervision"]; } }
        public static string RolUsuariosInternos { get { return ConfigurationManager.AppSettings["RolUsuariosInternos"]; } }
        public static string MensajeRteCadenaSupervision { get { return ConfigurationManager.AppSettings["MensajeRteCadenaSupervision"]; } }

        // Mensajes de error para reportes:
        public static string MsgErrorRte1 { get { return ConfigurationManager.AppSettings["mensajeErrorRtes1"]; } }
        public static string MsgErrorRte2 { get { return ConfigurationManager.AppSettings["mensajeErrorRtes2"]; } }
        public static string MsgErrorDataPrev { get { return ConfigurationManager.AppSettings["mensajeErrorCargaDatosPrevios"]; } }


         
        public static string ReporteLogActividades
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathLogActividades"]; }
        }

        //REPORTES:ReportPathRenovaciones
        public static string ReportPathRenovacionesDetalle { get { return ConfigurationManager.AppSettings["REPORTES:ReportPathRenovacionesDetalle"]; } }
        public static string ReportPathRenovacionesResumen { get { return ConfigurationManager.AppSettings["REPORTES:ReportPathRenovacionesResumen"]; } }

        /*
            Muestra si o no el trace de mensaje de errores atrapados en los catch de los Action de reportes
            Se debe activar unicamente para pruebas.
         */
        public static string MostrarTrace { get { return ConfigurationManager.AppSettings["mostrarTraceErroresEnNavegador"]; } }

        //Ruta archivo plano de carga tarifas asesor colaptria
        public static string PathArchivoPlanoComisionesAsesorColpatria { get { return ConfigurationManager.AppSettings["pathArchivoPlanoComisionesAsesorColpatria"]; } }

        //Ruta archivos pdf GANA TU PIM
        public static string PathCondicionesCorretaje { get { return ConfigurationManager.AppSettings["PathCondicionesCorretaje"]; } }
        public static string PathCondicionesDirecto { get { return ConfigurationManager.AppSettings["PathCondicionesDirecto"]; } }

        //Ruta archivos pdf Plande vuelo
        public static string PathPlanVueloCorretajeMeta0 { get { return ConfigurationManager.AppSettings["PathPlanVueloCorretajeMeta0"]; } }
        public static string PathPlanVueloCorretajeMeta1 { get { return ConfigurationManager.AppSettings["PathPlanVueloCorretajeMeta1"]; } }

        public static string PathPlanVueloDirectoMeta0 { get { return ConfigurationManager.AppSettings["PathPlanVueloDirectoMeta0"]; } }
        public static string PathPlanVueloDirectoMeta1 { get { return ConfigurationManager.AppSettings["PathPlanVueloDirectoMeta1"]; } }

        public static string HeadBanners { get { return ConfigurationManager.AppSettings["HeadBanners"]; } }


        /*
         *  Perímetros:
         */

        public static string PerimetroUrlBogota { get { return ConfigurationManager.AppSettings["PerimetroUrlBogota"]; } }
        public static string PerimetroUrlChia { get { return ConfigurationManager.AppSettings["PerimetroUrlChia"]; } }
        public static string PerimetroUrlCajica { get { return ConfigurationManager.AppSettings["PerimetroUrlCajica"]; } }
        public static string PerimetroUrlMedellin { get { return ConfigurationManager.AppSettings["PerimetroUrlMedellin"]; } }
        public static string PerimetroUrlCali { get { return ConfigurationManager.AppSettings["PerimetroUrlCali"]; } }
        public static string PerimetroUrlNeiva { get { return ConfigurationManager.AppSettings["PerimetroUrlNeiva"]; } }
        public static string PerimetroUrlBucaramanga { get { return ConfigurationManager.AppSettings["PerimetroUrlBucaramanga"]; } }
        public static string PerimetroUrlVillavicencio { get { return ConfigurationManager.AppSettings["PerimetroUrlVillavicencio"]; } }
        public static string PerimetroUrlFloridablanca { get { return ConfigurationManager.AppSettings["PerimetroUrlFloridablanca"]; } }
        public static string PerimetroUrlGiron { get { return ConfigurationManager.AppSettings["PerimetroUrlGiron"]; } }
        public static string PerimetroUrlPiedecuesta { get { return ConfigurationManager.AppSettings["PerimetroUrlPiedecuesta"]; } }
        public static string PerimetroUrlSoacha1 { get { return ConfigurationManager.AppSettings["PerimetroUrlSoacha1"]; } }

        //configuraciones captcha
        public static string SecurityKey => ConfigurationManager.AppSettings["SecurityKey"];
        public static string PublicSecurityKey => ConfigurationManager.AppSettings["PublicSecurityKey"];
        public static string ActivateCaptcha => ConfigurationManager.AppSettings["ActivateCaptcha"];

        //Configuracoin servicio de usuarios centralizado
        public static string URLWsUsuarios
        {
            get { return ConfigurationManager.AppSettings["URLWsUsuarios"]; }
        }
        public static string ClaveWsUsuarios
        {
            get { return ConfigurationManager.AppSettings["ClaveWsUsuarios"]; }
        }
        public static string AplicacionWsUsuarios
        {
            get { return ConfigurationManager.AppSettings["AplicacionWsUsuarios"]; }
        }

        public static string ReportPathCarteraContratos
        {
            get { return ConfigurationManager.AppSettings["REPORTES:ReportPathCarteraContratos"]; }
        }

        /*
         *  Fin de Perímetros:
         */



    }
}