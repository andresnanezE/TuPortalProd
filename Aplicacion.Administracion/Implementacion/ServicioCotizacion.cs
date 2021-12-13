using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModelEMERMEDICA_AreaETL;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Entidades.ReportesDto;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading.Tasks;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioCotizacion : IServicioCotizacion
    {
        #region Constants

        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";
        private const string ErrorCotizacionExistente = "El usuario ya existe en la aplicación.";

        #endregion Constants

        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioRoles _repositorioRoles;
        private readonly IRepositorioCotizacion _repositorioCotizaciones;
        private readonly IRepositorioEmpresasClientes _repositorioEmpresasCliente;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioCotizacion(IAdaptadorDeObjetos adaptadorDeObjetos, IRepositorioLogs manejadorLogs, IRepositorioRoles repositorioRoles, IRepositorioCotizacion repositorioCotizaciones, IRepositorioEmpresasClientes repositorioEmpresasCliente)
        {
            _manejadorLogs = manejadorLogs;
            _repositorioRoles = repositorioRoles;
            _adaptadorDeObjetos = adaptadorDeObjetos;
            _repositorioCotizaciones = repositorioCotizaciones;
            _repositorioEmpresasCliente = repositorioEmpresasCliente;
        }

        #endregion C'tors

        private FaultException<ErrorServicio> RegiExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        public List<EMA_COTIZACIONXPRODUCTODto> ValidarPermisoRegistroPorProducto(string NIT, int userId, int[] productosIds, string ciudad)
        {
            try
            {
                List<EMA_COTIZACIONXPRODUCTODto> pro = new List<EMA_COTIZACIONXPRODUCTODto>();
                var resultadoProducto = _repositorioCotizaciones.ValidarPermisoRegistroPorProducto(NIT, userId, productosIds, ciudad);

                if (resultadoProducto != null)
                {
                    foreach (var i in resultadoProducto)
                    {
                        var prod = new EMA_COTIZACIONXPRODUCTODto()
                        {
                            CotizacionID = i.CotizacionID,
                            ProductoID = i.ProductoID,
                            nombreProducto = i.nombreProducto,
                            ciudadCotiz = i.ciudadCotiz
                        };
                        pro.Add(prod);
                    }
                    return pro;
                }
                return null;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public string ObtenerProductosCliente(int CotizacionID)
        {
            try
            {
                var resultadoProdus = _repositorioCotizaciones.ObtenerProductosCliente(CotizacionID);
                if (resultadoProdus != null)
                {
                    return resultadoProdus;
                }
                return null;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public List<EMA_COTIZACIONXASESORDto> ValidarPermisoRegistroCotizacion(string NIT, int userID)
        {
            try
            {
                List<EMA_COTIZACIONXASESORDto> CotiAsesor = new List<EMA_COTIZACIONXASESORDto>();

                var resultadoEntidad = _repositorioCotizaciones.ValidarPermisoRegistroPorNIT(NIT, userID);
                if (resultadoEntidad.Count > 0)
                {
                    foreach (var r in resultadoEntidad)
                    {
                        var NitXCoti = new EMA_COTIZACIONXASESORDto
                        {
                            CotizacionID = r.cotizacionID,
                            AsesorID = r.asesorID,
                            fechaExpiracion = r.fechaExpiracion,
                            ciudad = r.ciudad,
                            nombreAsesor = r.nombreAsesor,
                            nombreDirector = r.nombreDirector,
                            PRODUCTOID = r.PRODUCTOID,
                            NombreProducto = r.NombreProducto
                        };
                        CotiAsesor.Add(NitXCoti);
                    }
                    return CotiAsesor;
                }
                if (resultadoEntidad.Count <= 0)
                    CotiAsesor = new List<EMA_COTIZACIONXASESORDto>();

                return CotiAsesor;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool RegistrarCotizacion(Cotizaciones cotizacion, int[] productosID, int userID)
        {
            try
            {
                cotizacion.EstadoCotizacion = "Sin Cotizar";
                bool registro = _repositorioCotizaciones.RegistrarCotizacion(cotizacion, productosID, userID);
                if (registro)
                {
                    return registro = true;
                }
                else
                {
                    return registro = false;
                }
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool Cotizar(string IdCotizacion, string MotivoVisita, string numeroCapacitaciones, string NumeroExpuestos, string numeroEventos, string numeroSedes, int? sectorID, int? areasProtegID, int[] validacionIds, int userId)
        {
            try
            {
                bool regiCoti = _repositorioCotizaciones.Cotizar(IdCotizacion, MotivoVisita, numeroCapacitaciones, NumeroExpuestos, numeroEventos, numeroSedes, sectorID, areasProtegID, validacionIds, userId);

                if (regiCoti)
                {
                    return regiCoti = true;
                }
                else
                {
                    return regiCoti = false;
                }
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public string ObtenerCiudadAsesor(int userId)
        {
            return _repositorioCotizaciones.ObtenerCiudadAsesor(userId);
        }

        public string ObtenerCanalAsesor(int userId)
        {
            return _repositorioCotizaciones.ObtenerCanalAsesor(userId);
        }

        public string ObtenerInfoPrincing()
        {
            return _repositorioCotizaciones.ObtenerInfoPricing();
        }

        public string ObtenerPassPricing()
        {
            return _repositorioCotizaciones.ObtenerPassPricing();
        }

        public List<Productos> ObtenerProductos()
        {
            return _repositorioCotizaciones.ObtenerProductos();
        }
        public List<ItemSelect> ObtenerCiudades()
        {
            return _repositorioCotizaciones.ObtenerCiudades();
        }
        public List<EMB_Canal> ObtenerCanales()
        {
            return _repositorioCotizaciones.ObtenerCanales();
        }

        public List<Validacion> ObtenerValidacion()
        {
            return _repositorioCotizaciones.ObtenerValidacion();
        }

        public List<ValidacionCopiaDirector> ObtenerInfoCopiaDirector()
        {
            return _repositorioCotizaciones.ObtenerInfoCopiaDirector();
        }

        public List<NivelesInteres> ObtenerNivelesInteres()
        {
            return _repositorioCotizaciones.ObtenerNivelesInteres();
        }

        public List<EstadosCotizacions> ObtenerEstados()
        {
            return _repositorioCotizaciones.ObtenerEstados();
        }

        public List<EstadosCotizacion> ObtenerEstadosCotizacion()
        {
            return _repositorioCotizaciones.ObtenerEstadosCotizacion();
        }

        public int ObtenerIDAsesor(int cotID)
        {
            return _repositorioCotizaciones.ObtenerIDAsesor(cotID);
        }

        public string ObtenerNombreAsesor(int asesorID)
        {
            return _repositorioCotizaciones.ObtenerNombreAsesor(asesorID);
        }

        public List<ESTADOS_COTI_PRIC> ObtenerEstadosPricing()
        {
            return _repositorioCotizaciones.ObtenerEstadosPricing();
        }

        public List<SECTOR> ObtenerSectores()
        {
            return _repositorioCotizaciones.ObtenerSectores();
        }

        public List<Factores> ObtenerFactores(int? tipoFactor = 0)
        {
            return _repositorioCotizaciones.ObtenerFactores(tipoFactor);
        }

        public List<Tipos_AP> ObtenerTipoAreasProtegidas()
        {
            return _repositorioCotizaciones.ObtenerTipoAreasProtegidas();
        }

        public List<CiudadesFactor> ObtenerCiudadesFactor()
        {
            return _repositorioCotizaciones.ObtenerCiudadesFactor();
        }

        public List<TipoRiesgo> ObtenerTipoRiesgo()
        {
            return _repositorioCotizaciones.ObtenerTipoRiesgo();
        }

        public string ObtenerNombreClienteXNIT(decimal auxNIT)
        {
            try
            {
                var nombre = _repositorioEmpresasCliente.ObtenerNombreClienteXNIT(auxNIT);
                if (nombre != null)
                {
                    return nombre;
                }
                return null;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public EMSP_EmpresasCliente ObtenerNombreXNIT(decimal NIT)
        {
            try
            {
                var existe = _repositorioEmpresasCliente.ObtenerNombreXNIT(NIT);
                if (existe != null)
                {
                    if (existe.DIG_VERI != -1)
                    {
                        return new EMSP_EmpresasCliente()
                        {
                            DIG_VERI = existe.DIG_VERI,
                            NOM_TERC = existe.NOM_TERC
                        };
                    }
                    else
                    {
                        return new EMSP_EmpresasCliente()
                        {
                            DIG_VERI = -1,
                            NOM_TERC = null
                        };
                    }
                }
                return null;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool validarPermisoContrato(int NIT, int userId, int productoId, string ciudad)
        {
            try
            {
                var contrat = _repositorioCotizaciones.validarPermisoContrato(NIT, userId, productoId, ciudad);
                if (contrat == false)
                {
                    return false;
                }
                if (contrat == true)
                {
                    return true;
                }
                return false;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public EMSP_INFOCONTRATOS validarPermiContrato(decimal auxNIT, int userId)
        {
            try
            {
                var contrat = _repositorioCotizaciones.validarPermiContrato(auxNIT, userId);
                if (contrat != null)
                {
                    return new EMSP_INFOCONTRATOS()
                    {
                        idContrato = contrat.idContrato,
                        fechaExpiracion = contrat.fechaExpiracion,
                        nombreAsesor = contrat.nombreAsesor,
                        nombreDirector = contrat.nombreDirector,
                        productos = contrat.productos,
                        ciudadAsesor = contrat.ciudadAsesor,
                        canalAsesor = contrat.canalAsesor
                    };
                }
                return null;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EMA_Cotizacion> ObtenerMisCotizaciones(int idUser, string estadoCotizacion)
        {
            try
            {
                var entidad = _repositorioCotizaciones.ObtenerMisCotizaciones(idUser, estadoCotizacion);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<Usp_ObtenerCotizaciones> ObtenerJerarquiaCotizaciones(string noDocumento, string estadoCotizacion, string producto, string estadoReserva, string fechaInicio, string fechaFin, string rol)
        {
            try
            {
                var entidad = _repositorioCotizaciones.ObtenerJerarquiaCotizaciones(noDocumento, estadoCotizacion, producto, estadoReserva, fechaInicio, fechaFin, rol);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public decimal buscarDirector(decimal asesorID)
        {
            try
            {
                var entidad = _repositorioCotizaciones.buscarDirector(asesorID);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public decimal ObtenerNoDocumentoDirector(int asesorID)
        {
            try
            {
                var entidad = _repositorioCotizaciones.ObtenerNoDocumentoDirector(asesorID);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EMA_Cotizacion> ObtenerMisReservasACotizar(int idUser)
        {
            try
            {
                var entidad = _repositorioCotizaciones.ObtenerMisReservasACotizar(idUser);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EMA_CotizacionXAsesor> ObtenerMisAsesores(int userId)
        {
            try
            {
                var asesores = _repositorioCotizaciones.ObtenerMisAsesores(userId);
                return asesores;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EMA_Cotizacion> ObtenerReservasActivas()
        {
            try
            {
                var reservas = _repositorioCotizaciones.ObtenerReservasActivas();
                return reservas;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EMA_Cotizacion> ObtenerCotizacionFiltros(int idProducto, int idEstadoReserva, int idEstadoCotizacion, int aseID, string nitNombre, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                var entidad = _repositorioCotizaciones.ObtenerCotizacionFiltros(idProducto, idEstadoReserva, idEstadoCotizacion, aseID, nitNombre, fechaInicio, fechaFin);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EMA_Cotizacion> ObtenerCotizacionPricingFiltros(int menuId, string descripcion, int? estadoPricID, string cotiNIT, string nombreCliente, int pricID)
        {
            try
            {
                var entidad = _repositorioCotizaciones.ObtenerCotizacionPricingFiltros(menuId, descripcion, estadoPricID, cotiNIT, nombreCliente, pricID);

                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EMA_Cotizacion> FiltroReservasCotizaciones(int menuId, string descripcion, int? estadoPricID, string cotiNIT, string nombreCliente, int pricID)
        {
            try
            {
                var entidad = _repositorioCotizaciones.FiltroReservasCotizaciones(menuId, descripcion, estadoPricID, cotiNIT, nombreCliente, pricID);

                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EMA_CotizacionXAsesor> ObtenerAsesorFiltro(int menuId, string descripcion, string numeroDocumento, string nombreAsesor, int aseID)
        {
            try
            {
                var asesor = _repositorioCotizaciones.ObtenerAsesorFiltro(menuId, descripcion, numeroDocumento, nombreAsesor, aseID);
                return asesor;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<Cotizaciones> EditarCotizacion(int IdCotizacion, int userId)
        {
            try
            {
                var entidad = _repositorioCotizaciones.EditarCotizacion(IdCotizacion, userId);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<Cotizaciones> ObtenerDatosReserva(int cotID, int userId)
        {
            try
            {
                var reserva = _repositorioCotizaciones.ObtenerDatosReserva(cotID, userId);
                return reserva;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SedesCotizacionDto> ObtenerInfSedesCotizacion(int idCotizacion)
        {
            try
            {
                var infSedes = _repositorioCotizaciones.ObtenerInfSedesCotizacion(idCotizacion);

                foreach (var sede in infSedes)
                {
                    if (sede.ValorReconsideracion == null)
                        sede.ValorReconsideracion = sede.Valor;
                }
                return infSedes;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<Cotizaciones> EditarCotizacionPricing(int cotID, int userId)
        {
            var entidPric = _repositorioCotizaciones.EditarCotizacionPricing(cotID, userId);
            return entidPric;
        }

        public IEnumerable<Productos> productosCotizacion(int IdCotizacion)
        {
            try
            {
                var prods = _repositorioCotizaciones.productosCotizacion(IdCotizacion);
                return prods;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool liberarReserva(string IdCotizacion, string motivo, int estadoCotiID, int IdUsuario)
        {
            try
            {
                var libReser = _repositorioCotizaciones.liberarReserva(IdCotizacion, motivo, estadoCotiID, IdUsuario);

                return libReser;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool renovarReserva(string IdCotizacion, string MotivoVisita, int estadoCotiID, int IdUsuario)
        {
            try
            {
                var renovReser = _repositorioCotizaciones.renovarReserva(IdCotizacion, MotivoVisita, estadoCotiID, IdUsuario);
                return renovReser;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool GuardarCotizacion(string IdCotizacion, string ObservacionesPricing, string total, string nombreArchivo)
        {
            try
            {
                var guardCoti = _repositorioCotizaciones.GuardarCotizacion(IdCotizacion, ObservacionesPricing, total, nombreArchivo);
                return guardCoti;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool GuardarCotizacionSCI(GuardarCotizacion objData)
        {
            try
            {
                var guardarCotizacion = _repositorioCotizaciones.GuardarCotizacionSCI(objData);
                return guardarCotizacion;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool GuardarCotizacionSedesSCI(GuardarCotizacion objData)
        {
            try
            {
                var guardarCotizacionSedes = _repositorioCotizaciones.GuardarCotizacionSedesSCI(objData);
                return guardarCotizacionSedes;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public List<decimal> CalcularTarifaSedeSCI(CalcularTarifaSede objData)
        {
            //try
            //{
            var valorTarifaSede = _repositorioCotizaciones.CalcularTarifaSedeSCI(objData);
            return valorTarifaSede;
            //}
            //catch (SystemException exception)
            //{
            //    throw RegiExepxion(exception, ErrorProcesandoPeticion);
            //}
        }

        public string ObtenerCorreoAsesor(string IdCotizacion)
        {
            try
            {
                var correoAseso = _repositorioCotizaciones.ObtenerCorreoAsesor(IdCotizacion);

                return correoAseso;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public string ObtenerDocumentoAsesor(string IdCotizacion)
        {
            try
            {
                var documentoAseso = _repositorioCotizaciones.ObtenerDocumentoAsesor(IdCotizacion);
                return documentoAseso;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public string ObtenerCorreoDirector(string Document)
        {
            try
            {
                var correoDirector = _repositorioCotizaciones.ObtenerCorreoDirector(Document);
                return correoDirector;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public string ObtenerNombreDirector(string userAseso)
        {
            try
            {
                var nombreDirect = _repositorioCotizaciones.ObtenerNombreDirector(userAseso);
                return nombreDirect;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public string ObtenerEmpresaCliente(string IdCotizacion)
        {
            try
            {
                var empreCliente = _repositorioCotizaciones.ObtenerEmpresaCliente(IdCotizacion);

                return empreCliente;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public List<ReporteCotizacion> ObtenerInfReporteCotizacion(int idCotizacion)
        {
            try
            {
                var infReporteCotizacion = _repositorioCotizaciones.ObtenerInfReporteCotizacion(idCotizacion);
                return infReporteCotizacion;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }
        public List<GlobalReporte> ObtenerReporteGlobal()
        {
            try
            {
                var infReporteGlobal = _repositorioCotizaciones.ObtenerReporteGlobal();
                return infReporteGlobal;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool EnviarCotizacionAReconsideracion(ReconsideracionCotizacionDto reconsideracion)
        {
            try
            {
                return _repositorioCotizaciones.EnviarCotizacionAReconsideracion(reconsideracion);
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool AprobarReconsideracion(ReconsideracionCotizacionDto reconsideracion)
        {
            try
            {
                return _repositorioCotizaciones.AprobarReconsideracion(reconsideracion);
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool RechazarReconsideracion(ReconsideracionCotizacionDto reconsideracion)
        {
            try
            {
                return _repositorioCotizaciones.RechazarReconsideracion(reconsideracion);
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public List<NotaCotizacionDto> ObtenerNotasCotizacion(int cotizacionId)
        {
            try
            {
                return _repositorioCotizaciones.ObtenerNotasCotizacion(cotizacionId);
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public List<GlobalReporte> DataReporte(FiltrosReporte filtros, string noDocumento, string rol)
        {
            try
            {
                return _repositorioCotizaciones.ObtenerDataReporte(filtros, noDocumento, rol);
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public List<EME_CIUDAD_HOMOLOG> ObtenerListaCiudades()
        {
            throw new NotImplementedException();
        }

        public List<EMB_Canal> ObtenerListaCanales()
        {
            throw new NotImplementedException();
        }

        public List<SECTOR> ObtenerSector()
        {
            return _repositorioCotizaciones.ObtenerSector();
        }


        public List<Director> ObtenerDirector(string ciudad = null)
        {
            return _repositorioCotizaciones.ObtenerDirector(ciudad);
        }

        public List<Productos> ObtenerProductosDisponiblesParaNit(decimal nit, string ciudadUsuario)
        {
            return _repositorioCotizaciones.ObtenerProductosDisponiblesParaNit(nit, ciudadUsuario);
        }

        public int ObtenerMaximoRenovaciones()
        {
            return _repositorioCotizaciones.ObtenerMaximoRenovaciones();
        }

        public List<ItemUsuario> ObtenerAsesoresXDirectores(List<decimal> directorIds)
        {
            return _repositorioCotizaciones.ObtenerAsesoresXDirectores(directorIds);
        }

        public Respuesta RegistrarSolicitudDigitalizacion(InfoSolicitud info, int userId)
        {
            try
            {
                var result = _repositorioCotizaciones.RegistrarSolicitudDigitalizacion(info, userId);
                return result;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool EditarSolicitudDigitalizacion(InfoSolicitud info, int userId)
        {
            try
            {
                var result = _repositorioCotizaciones.EditarSolicitudDigitalizacion(info, userId);
                return result;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public string ObtenerCedulaAsesor(int userId)
        {
            try
            {
                return _repositorioCotizaciones.ObtenerCedulaAsesor(userId);
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }

        }

        public IEnumerable<SolicitudDigital> ObtenerSolicitudesDigitalizacion(string documento, string userId, string fechaInicio, string fechaFin, bool esDirector)
        {
            try
            {
                var entidad = _repositorioCotizaciones.ObtenerSolicitudesDigitalizacion(documento, userId, fechaInicio, fechaFin, esDirector);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }
        public List<CausalesDigitalizacion> ObtenerCausales(int idEstado)
        {
            try
            {
                var causales = _repositorioCotizaciones.ObtenerCausales(idEstado);
                return causales;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }
        public List<CausalesSolicitud> ObtenerCausalesSolicitud(int idSolicitud)
        {
            try
            {
                var causalesSolicitud = _repositorioCotizaciones.GetCausalesSolicitud(idSolicitud);
                return causalesSolicitud;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public List<HistoricoSolicitud> ObtenerHistoricoSolicitud(int idSolicitud)
        {
            try
            {
                var historico = _repositorioCotizaciones.GetHistoricoSolicitud(idSolicitud);
                return historico;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }
        public bool VerificarContrato(string numeroContrato)
        {
            try
            {
                var contratoValido = _repositorioCotizaciones.VerificarContrato(numeroContrato);
                return contratoValido;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }
        public async Task<ResultadoReferenciaDto> GetReferenciaPago(GenerarReferenciaDto model)
        {
            try
            {
                var resultadoReferencia = await _repositorioCotizaciones.GetReferenciaPago(model);
                return resultadoReferencia;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }
        public async Task<ResultadoPaymentDataDto> GetPaymentData(string refPago)
        {
            try
            {
                var resultadoData = await _repositorioCotizaciones.GetPaymentData(refPago);
                return resultadoData;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }
    }
}