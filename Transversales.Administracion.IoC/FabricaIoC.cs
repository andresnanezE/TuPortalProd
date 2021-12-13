// ----------------------------------------------------------------------------------------------
// <copyright file="FabricaIoC.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Implementacion;
using Datos.Administracion.Repositorios;
using Dominio.Administracion.Repositorios;
using Microsoft.Practices.Unity;
using Transversales.Administracion.Correos;
using Transversales.Administracion.IoC.Adaptadores;

namespace Transversales.Administracion.IoC
{
    public static class FabricaIoC
    {
        #region Readonly & Static Fields

        private static readonly IUnityContainer Contenedor;

        #endregion Readonly & Static Fields

        #region C'tors

        static FabricaIoC()
        {
            Contenedor = new UnityContainer();

            // Registrar Servicios de Aplicación
            Contenedor.RegisterType<IServicioAplicacionDestacado, ServicioAplicacionDestacado>();
            Contenedor.RegisterType<IServicioAplicacionBanner, ServicioAplicacionBanner>();
            Contenedor.RegisterType<IServicioAplicacionNoticia, ServicioAplicacionNoticia>();
            Contenedor.RegisterType<IServicioAplicacionUsuarios, ServicioAplicacionUsuarios>();
            Contenedor.RegisterType<IServicioAplicacionTablasBase, ServicioAplicacionTablasBase>();
            Contenedor.RegisterType<IServicioAplicacionRoles, ServicioAplicacionRoles>();
            Contenedor.RegisterType<IServicioAplicacionMenu, ServicioAplicacionMenu>();
            Contenedor.RegisterType<IServicioAplicacionReporteVentasAfiliaciones, ServicioAplicacionReporteVentasAfiliaciones>();
            Contenedor.RegisterType<IServicioAplicacionTarjetasTransmilenio, ServicioAplicacionRteTarjetasTransmilenio>();
            Contenedor.RegisterType<IServicioAplicacionApoyoRodamiento, ServicioAplicacionRteApoyoRodamiento>();
            Contenedor.RegisterType<IServicioAplicacionComoVoy, ServicioAplicacionComoVoy>();
            Contenedor.RegisterType<IServicioAplicacionMotivoSegSocial, ServicioAplicacionMotivoSoporteSegSocial>();
            Contenedor.RegisterType<IServicioAplicacionContratos, ServicioAplicacionContratos>();
            Contenedor.RegisterType<IServicioAplicacionMedioPago, ServicioAplicacionMedioPago>();
            Contenedor.RegisterType<IServicioAplicacionLogs, ServicioAplicacionLogs>();
            Contenedor.RegisterType<IServicioAplicacionProcesosAsesor, ServicioAplicacionProcesosAsesor>();
            Contenedor.RegisterType<IServicioAplicacionReporteSabanaAsesores, ServicioAplicacionReporteSabanaAsesores>();
            Contenedor.RegisterType<IServicioAplicacionCargueTarifasCampañas, ServicioAplicacionCargueTarifasCampañas>();
            Contenedor.RegisterType<IServicioAplicacionCotizadorTarifas, ServicioAplicacionCotizadorTarifas>();
            Contenedor.RegisterType<IServicioAplicacionCargueTarifasPlenas, ServicioAplicacionCargueTarifasPlenas>();
            Contenedor.RegisterType<IServicioAplicacionCaracterizacionCampañas, ServicioAplicacionCaracterizacionCampañas>();
            Contenedor.RegisterType<IServicioAplicacionCalcularInclusiones, ServicioAplicacionCalcularInclusiones>();
            Contenedor.RegisterType<IServicioAplicacionSolicitudesInternas, ServicioAplicacionSolicitudesInternas>();
            Contenedor.RegisterType<IServicioAplicacionSesionEnVezDe, ServicioAplicacionSesionEnVezDe>();
            Contenedor.RegisterType<IServicioAplicacionSesionEnVezDeAdmUsuarios, ServicioAplicacionSesionEnVezDeAdmUsuarios>();
            Contenedor.RegisterType<IServicioAplicacionEsCadenaSupervision, ServicioAplicacionEsCadenaSupervision>();
            Contenedor.RegisterType<IServicioCotizacion, ServicioCotizacion>();
            Contenedor.RegisterType<IServicioAplicacionGanometro, ServicioAplicacionGanometro>();
            Contenedor.RegisterType<IServicioAplicacionContratoUsuario, ServicioAplicacionContratoUsuario>();
            Contenedor.RegisterType<IServicioReclutamiento, ServicioReclutamiento>();
            Contenedor.RegisterType<IServicioArchivosVentas, ServicioArchivosVentas>();

            // Registrar Repositorios
            Contenedor.RegisterType<IRepositorioDestacado, RepositorioDestacado>();
            Contenedor.RegisterType<IRepositorioBanner, RepositorioBanner>();
            Contenedor.RegisterType<IRepositorioNoticia, RepositorioNoticia>();
            Contenedor.RegisterType<IRepositorioUsuarios, RepositorioUsuarios>();
            Contenedor.RegisterType<IRepositorioTablasBase, RepositorioTablasBasicas>();
            Contenedor.RegisterType<IRepositorioRoles, RepositorioRoles>();
            Contenedor.RegisterType<IRepositorioMenu, RepositorioMenu>();
            Contenedor.RegisterType<IRepositorioLogs, RepositorioLogs>();
            Contenedor.RegisterType<IRepositorioContratos, RepositorioContratos>();
            Contenedor.RegisterType<IRepositorioReporteVentasAfiliaciones, RepositorioReporteVentasAfiliaciones>();
            Contenedor.RegisterType<IRepositorioRteTarjetasTransmilenio, RepositorioRteTarjetasTransmilenio>();
            Contenedor.RegisterType<IRepositorioRteApoyoRodamiento, RepositorioRteApoyoRodamiento>();
            Contenedor.RegisterType<IRepositorioComoVoy, RepositorioComoVoy>();
            Contenedor.RegisterType<IRepositorioMotivoSoporteSegSocial, RepositorioMotivoSoporteSegSocial>();
            Contenedor.RegisterType<IRepositorioProcesosAsesor, RepositorioProceasosAsesor>();
            Contenedor.RegisterType<IRepositorioContratos, RepositorioContratos>();
            Contenedor.RegisterType<IRepositorioReporteSabanaAsesores, RepositorioReporteSabanaAsesores>();
            Contenedor.RegisterType<IRepositorioCargueTarifasCampañas, RepositorioCargueTarifasCampañas>();
            Contenedor.RegisterType<IRepositorioCotizadorTarifas, RepositorioCotizadorTarifas>();
            Contenedor.RegisterType<IRepositorioCargueTarifasPlenas, RepositorioCargueTarifasPlenas>();
            Contenedor.RegisterType<IRepositorioCaracterizacionCampañas, RepositorioCaracterizacionCampañas>();
            Contenedor.RegisterType<IRepositorioCalcularInclusiones, RepositorioCalcularInclusiones>();
            Contenedor.RegisterType<IRepositorioSolicitudesInternas, RepositorioSolicitudesInternas>();
            Contenedor.RegisterType<IRepositorioSesionEnVezDe, RepositorioSesionEnVezDe>();
            Contenedor.RegisterType<IRepositorioSesionEnVezDeAdmUsuarios, RepositorioSesionEnVezDeAdmUsuarios>();
            Contenedor.RegisterType<IRepositorioEsCadenaSupervision, RepositorioEsCadenaSupervision>();
            Contenedor.RegisterType<IRepositorioEmpresasClientes, RepositorioEmpresasClientes>();
            Contenedor.RegisterType<IRepositorioGanometro, RepositorioGanometro>();
            Contenedor.RegisterType<IRepositorioContratosUsuarios, RepositorioContratosUsuarios>();
            Contenedor.RegisterType<IRepositorioMedioPago, RepositorioMedioPago>();
            Contenedor.RegisterType<IRepositorioReclutamiento, RepositorioReclutamiento>();
            Contenedor.RegisterType<IRepositorioArchivosVentas, RepositorioArchivosVentas>();

            // Registrar Frameworks Transversales
            Contenedor.RegisterType<IManejadorCorreos, ManejadorCorreos>();

            //Adaptadores
            Contenedor.RegisterType<IAdaptadorDeObjetos, AdaptadorDeObjetosAplicacion>();

            Contenedor.RegisterType<IRepositorioCotizacion, RepositorioCotizacion>();
        }

        #endregion C'tors

        #region Class Methods

        public static TServicio Resolver<TServicio>()
        {
            return Contenedor.Resolve<TServicio>();
        }

        #endregion Class Methods
    }
}