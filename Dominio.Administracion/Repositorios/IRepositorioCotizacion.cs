using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.ModelEMERMEDICA_AreaETL;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Entidades.ReportesDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioCotizacion
    {
        List<EMSP_ValidarRegistroCotizacionPorProducto> ValidarPermisoRegistroPorProducto(string NIT, int userId, int[] productosIds, string ciudad);

        string ObtenerProductosCliente(int CotizacionID);

        List<EMSP_ValidarRegistroCotizacion> ValidarPermisoRegistroPorNIT(string NIT, int userID);

        bool RegistrarCotizacion(Cotizaciones cotizacion, int[] productosID, int userID);

        bool Cotizar(string IdCotizacion, string MotivoVisita, string numeroCapacitaciones, string NumeroExpuestos, string numeroEventos, string numeroSedes, int? sectorID, int? areasProtegID, int[] validacionIds, int userId);

        string ObtenerCiudadAsesor(int userId);

        string ObtenerCanalAsesor(int userId);

        string ObtenerInfoPricing();

        string ObtenerPassPricing();

        List<Productos> ObtenerProductos();

        List<Validacion> ObtenerValidacion();

        List<ValidacionCopiaDirector> ObtenerInfoCopiaDirector();

        List<NivelesInteres> ObtenerNivelesInteres();

        bool validarPermisoContrato(int NIT, int userId, int productoId, string ciudad);

        EMSP_INFOCONTRATOS validarPermiContrato(decimal auxNIT, int userId);

        IEnumerable<EMA_Cotizacion> ObtenerMisCotizaciones(int idUser, string estadoCotizacion);

        IEnumerable<EMA_Cotizacion> ObtenerMisReservasACotizar(int idUser);

        IEnumerable<Usp_ObtenerCotizaciones> ObtenerJerarquiaCotizaciones(string noDocumento, string estadoCotizacion, string producto, string estadoReserva, string fechaInicio, string fechaFin, string rol);

        decimal buscarDirector(decimal asesorID);

        decimal ObtenerNoDocumentoDirector(int asesorID);

        IEnumerable<EMA_CotizacionXAsesor> ObtenerMisAsesores(int userId);

        IEnumerable<EMA_Cotizacion> ObtenerReservasActivas();

        List<EstadosCotizacions> ObtenerEstados();

        List<EstadosCotizacion> ObtenerEstadosCotizacion();

        int ObtenerIDAsesor(int cotID);

        string ObtenerNombreAsesor(int asesorID);

        List<ESTADOS_COTI_PRIC> ObtenerEstadosPricing();

        List<SECTOR> ObtenerSectores();

        List<Factores> ObtenerFactores(int? tipoFactor = 0);

        List<Tipos_AP> ObtenerTipoAreasProtegidas();

        List<CiudadesFactor> ObtenerCiudadesFactor();

        List<TipoRiesgo> ObtenerTipoRiesgo();

        IEnumerable<EMA_Cotizacion> ObtenerCotizacionFiltros(int idProducto, int idEstadoReserva, int idEstadoCotizacion, int aseID, string nitNombre, DateTime? fechaInicio, DateTime? fechaFin);

        IEnumerable<EMA_Cotizacion> ObtenerCotizacionPricingFiltros(int menuId, string descripcion, int? estadoPricID, string cotiNIT, string nombreCliente, int pricID);

        IEnumerable<EMA_Cotizacion> FiltroReservasCotizaciones(int menuId, string descripcion, int? estadoPricID, string cotiNIT, string nombreCliente, int pricID);

        IEnumerable<EMA_CotizacionXAsesor> ObtenerAsesorFiltro(int menuId, string descripcion, string numeroDocumento, string nombreAsesor, int aseID);

        IEnumerable<Cotizaciones> EditarCotizacion(int IdCotizacion, int userId);

        IEnumerable<SedesCotizacionDto> ObtenerInfSedesCotizacion(int idCotizacion);

        IEnumerable<Cotizaciones> ObtenerDatosReserva(int cotID, int userId);

        IEnumerable<Cotizaciones> EditarCotizacionPricing(int cotID, int userId);

        IEnumerable<Productos> productosCotizacion(int IdCotizacion);

        bool liberarReserva(string IdCotizacion, string motivo, int estadoCotiID, int IdUsuario);

        bool renovarReserva(string IdCotizacion, string MotivoVisita, int estadoCotiID, int IdUsuario);

        bool GuardarCotizacion(string IdCotizacion, string ObservacionesPricing, string total, string nombreArchivo);

        bool GuardarCotizacionSCI(GuardarCotizacion objData);

        bool GuardarCotizacionSedesSCI(GuardarCotizacion objData);

        List<decimal> CalcularTarifaSedeSCI(CalcularTarifaSede objData);

        string ObtenerCorreoAsesor(string IdCotizacion);

        string ObtenerDocumentoAsesor(string IdCotizacion);

        string ObtenerCorreoDirector(string Document);

        string ObtenerNombreDirector(string userAseso);

        string ObtenerEmpresaCliente(string IdCotizacion);

        List<ReporteCotizacion> ObtenerInfReporteCotizacion(int idCotizacion);
        List<GlobalReporte> ObtenerReporteGlobal();

        bool EnviarCotizacionAReconsideracion(ReconsideracionCotizacionDto reconsideracion);

        bool AprobarReconsideracion(ReconsideracionCotizacionDto reconsideracion);
        bool RechazarReconsideracion(ReconsideracionCotizacionDto reconsideracion);

        List<NotaCotizacionDto> ObtenerNotasCotizacion(int cotizacionId);
        List<GlobalReporte> ObtenerDataReporte(FiltrosReporte filtros, string noDocumento, string rol);
        List<ItemSelect> ObtenerCiudades();
        List<EMB_Canal> ObtenerCanales();
        List<SECTOR> ObtenerSector();
        List<Director> ObtenerDirector(string ciudad = null);

        /// <summary>
        /// Obtiene los productos disponibles para reservar según el NIT consultado y la ciudad del usuario en sesión.
        /// </summary>
        /// <param name="nit">Nit consultado</param>
        /// <param name="ciudadUsuario">Ciudad del usuario que consulta el nit</param>
        /// <returns></returns>
        List<Productos> ObtenerProductosDisponiblesParaNit(decimal nit, string ciudadUsuario);

        int ObtenerMaximoRenovaciones();

        List<ItemUsuario> ObtenerAsesoresXDirectores(List<decimal> directorIds);

        Respuesta RegistrarSolicitudDigitalizacion(InfoSolicitud info, int userId);

        string ObtenerCedulaAsesor(int userId);

        IEnumerable<SolicitudDigital> ObtenerSolicitudesDigitalizacion(string documento, string userId, string fechaInicio, string fechaFin, bool esDirector);


        bool EditarSolicitudDigitalizacion(InfoSolicitud info, int userId);
        List<CausalesDigitalizacion> ObtenerCausales(int idEstado);
        List<CausalesSolicitud> GetCausalesSolicitud(int idSolicitud);
        List<HistoricoSolicitud> GetHistoricoSolicitud(int idSolicitud);
        bool VerificarContrato(string numeroContrato);
        Task<TokenReferenciaPago> GetTokenRefPago();
        Task<ResultadoReferenciaDto> GetReferenciaPago(GenerarReferenciaDto model);
        Task<ResultadoPaymentDataDto> GetPaymentData(string refPago);
    }
}