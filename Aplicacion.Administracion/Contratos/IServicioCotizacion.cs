using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModelEMERMEDICA_AreaETL;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Entidades.ReportesDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioCotizacion
    {
        #region Instance Methods

        List<EMA_COTIZACIONXASESORDto> ValidarPermisoRegistroCotizacion(string NIT, int userID);

        string ObtenerProductosCliente(int CotizacionID);

        List<EMA_COTIZACIONXPRODUCTODto> ValidarPermisoRegistroPorProducto(string NIT, int userId, int[] productosIds, string ciudad);

        bool RegistrarCotizacion(Cotizaciones cotizacion, int[] productosID, int userID);

        bool Cotizar(string IdCotizacion, string MotivoVisita, string numeroCapacitaciones, string NumeroExpuestos, string numeroEventos, string numeroSedes, int? sectorID, int? areasProtegID, int[] validacionIds, int userId);

        List<Productos> ObtenerProductos();
        List<ItemSelect> ObtenerCiudades();
        List<EMB_Canal> ObtenerCanales();
        List<Validacion> ObtenerValidacion();

        List<ValidacionCopiaDirector> ObtenerInfoCopiaDirector();

        string ObtenerCiudadAsesor(int userId);

        string ObtenerCanalAsesor(int userId);

        string ObtenerInfoPrincing();

        string ObtenerPassPricing();

        List<NivelesInteres> ObtenerNivelesInteres();

        IEnumerable<EMA_Cotizacion> ObtenerMisCotizaciones(int idUser, string estadoCotizacion);

        IEnumerable<Usp_ObtenerCotizaciones> ObtenerJerarquiaCotizaciones(string noDocumento, string estadoCotizacion, string producto, string estadoReserva, string fechaInicio, string fechaFin, string rol);

        decimal buscarDirector(decimal asesorID);

        decimal ObtenerNoDocumentoDirector(int asesorID);

        IEnumerable<EMA_Cotizacion> ObtenerMisReservasACotizar(int idUser);

        IEnumerable<EMA_CotizacionXAsesor> ObtenerMisAsesores(int userId);

        IEnumerable<EMA_Cotizacion> ObtenerReservasActivas();

        EMSP_EmpresasCliente ObtenerNombreXNIT(decimal NIT);

        string ObtenerNombreClienteXNIT(decimal auxNIT);

        bool validarPermisoContrato(int auxNIT, int userId, int productoId, string ciudad);

        EMSP_INFOCONTRATOS validarPermiContrato(decimal auxNIT, int userId);

        IEnumerable<EMA_Cotizacion> ObtenerCotizacionFiltros(int idProducto, int idEstadoReserva, int idEstadoCotizacion, int aseID, string nitNombre, DateTime? fechaInicio, DateTime? fechaFin);

        IEnumerable<EMA_Cotizacion> ObtenerCotizacionPricingFiltros(int menuId, string descripcion, int? estadoPricID, string cotiNIT, string nombreCliente, int pricID);

        IEnumerable<EMA_Cotizacion> FiltroReservasCotizaciones(int menuId, string descripcion, int? estadoPricID, string cotiNIT, string nombreCliente, int pricID);

        IEnumerable<EMA_CotizacionXAsesor> ObtenerAsesorFiltro(int menuId, string descripcion, string numeroDocumento, string nombreAsesor, int aseID);

        IEnumerable<Cotizaciones> EditarCotizacion(int IdCotizacion, int userId);

        IEnumerable<Cotizaciones> ObtenerDatosReserva(int cotID, int userId);

        IEnumerable<SedesCotizacionDto> ObtenerInfSedesCotizacion(int idCotizacion);

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

        List<EstadosCotizacions> ObtenerEstados();
        List<EME_CIUDAD_HOMOLOG> ObtenerListaCiudades();
        List<EMB_Canal> ObtenerListaCanales();

        List<EstadosCotizacion> ObtenerEstadosCotizacion();

        int ObtenerIDAsesor(int cotID);

        string ObtenerNombreAsesor(int asesorID);

        List<ESTADOS_COTI_PRIC> ObtenerEstadosPricing();

        List<SECTOR> ObtenerSectores();

        List<Tipos_AP> ObtenerTipoAreasProtegidas();

        List<CiudadesFactor> ObtenerCiudadesFactor();

        List<TipoRiesgo> ObtenerTipoRiesgo();

        List<Factores> ObtenerFactores(int? tipoFactor = 0);

        List<ReporteCotizacion> ObtenerInfReporteCotizacion(int idCotizacion);
        List<GlobalReporte> ObtenerReporteGlobal();
        List<GlobalReporte> DataReporte(FiltrosReporte filtros, string noDocumento, string rol);
        bool EnviarCotizacionAReconsideracion(ReconsideracionCotizacionDto reconsideracion);
        bool AprobarReconsideracion(ReconsideracionCotizacionDto reconsideracion);
        bool RechazarReconsideracion(ReconsideracionCotizacionDto reconsideracion);
        List<NotaCotizacionDto> ObtenerNotasCotizacion(int cotizacionId);
        List<SECTOR> ObtenerSector();
        List<Director> ObtenerDirector(string ciudad = null);

        List<Productos> ObtenerProductosDisponiblesParaNit(decimal nit, string ciudadUsuario);
        int ObtenerMaximoRenovaciones();

        List<ItemUsuario> ObtenerAsesoresXDirectores(List<decimal> directorIds);

        Respuesta RegistrarSolicitudDigitalizacion(InfoSolicitud info, int userId);

        string ObtenerCedulaAsesor(int userId);

        IEnumerable<SolicitudDigital> ObtenerSolicitudesDigitalizacion(string documento, string userId, string fechaInicio, string fechaFin, bool esDirector);

        List<CausalesDigitalizacion> ObtenerCausales(int idEstado);
        bool EditarSolicitudDigitalizacion(InfoSolicitud info, int userId);
        List<CausalesSolicitud> ObtenerCausalesSolicitud(int idSolicitud);
        List<HistoricoSolicitud> ObtenerHistoricoSolicitud(int idSolicitud);
        bool VerificarContrato(string numeroContrato);
        Task<ResultadoReferenciaDto> GetReferenciaPago(GenerarReferenciaDto model);
        Task<ResultadoPaymentDataDto> GetPaymentData(string refPago);

        #endregion Instance Methods
    }
}