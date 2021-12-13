using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionReporteVentasAfiliaciones : IServicioAplicacionReporteVentasAfiliaciones
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioReporteVentasAfiliaciones _repositorioAfiliaciones;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionReporteVentasAfiliaciones(IRepositorioReporteVentasAfiliaciones repositorioAfiliaciones, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioAfiliaciones = repositorioAfiliaciones;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion C'tors

        #region Instance Methods

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        #endregion Instance Methods

        public IEnumerable<AfiliacionesPeriodoDto> Obtener_Periodos()
        {
            try
            {
                var lista = _repositorioAfiliaciones.Obtener_Periodos();
                var listaDto = _adaptadorDeObjetos.Adaptar<IEnumerable<AfiliacionesPeriodoDto>>(lista);
                return listaDto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de periodos");
            }
        }

        public IEnumerable<resultadosConsultaAfiliacionResumenDto> Consultar_Detalle_Afiliacion_Resumen(
            DatosAfiliacionDto datos)
        {
            try
            {
                DatosAfiliacion datosEntidad = new DatosAfiliacion()
                {
                    DOCUMENTO = datos.Documento,
                    FECH_PERIODO = datos.Periodo,
                    FECH_PERIODO2 = datos.Periodo2,
                    ROL = datos.Rol,
                    COMISIONA = datos.Comisiona != null ? string.Join(",", datos.Comisiona.ToArray()) : "",
                    EST_BENEF = datos.EstBenef != null ? string.Join(",", datos.EstBenef.ToArray()) : "",
                    TIP_CONTR = datos.TipContrato != null ? string.Join(",", datos.TipContrato.ToArray()) : "",
                    TIP_NOVEDAD = datos.Novedad != null ? string.Join(",", datos.Novedad.ToArray()) : "",
                    TIPO = "R"
                };
                var lista = _repositorioAfiliaciones.Consultar_Detalle_Afiliacion_Resumen(datosEntidad);
                var listaDto = _adaptadorDeObjetos.Adaptar<IEnumerable<resultadosConsultaAfiliacionResumenDto>>(lista);
                return listaDto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo resumen de afiliaciones");
            }
        }

        public IEnumerable<ResultadosConsultaAfiliacionResumenTablaDto> Consultar_Detalle_Afiliacion_ResumenTabla(
           DatosConsultaAfiliacionDto datos)
        {
            try
            {
                var datosEntidad = new DatosConsultaAfiliacion()
                {
                    DOCUMENTO = datos.Documento,
                    FECH_PERIODO = datos.Periodo,
                    FECH_PERIODO2 = datos.Periodo2,
                    ROL = datos.Rol,
                    COMISIONA = datos.Comisiona != null ? string.Join(",", datos.Comisiona.ToArray()) : "",
                    EST_BENEF = datos.EstBenef != null ? string.Join(",", datos.EstBenef.ToArray()) : "",
                    TIP_CONTR = datos.TipContrato != null ? string.Join(",", datos.TipContrato.ToArray()) : "",
                    TIP_NOVEDAD = datos.Novedad != null ? string.Join(",", datos.Novedad.ToArray()) : "",
                    TIPO = "R"
                };
                var lista = _repositorioAfiliaciones.Consultar_Detalle_Afiliacion_ResumenTabla(datosEntidad);
                var listaDto = _adaptadorDeObjetos.Adaptar<IEnumerable<ResultadosConsultaAfiliacionResumenTablaDto>>(lista);
                return listaDto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo resumen de afiliaciones");
            }
        }


        public IEnumerable<AfiliacionesFiltroDto> ObtenerFiltro_x_Rol(int _rol)
        {
            try
            {
                var lista = _repositorioAfiliaciones.ObtenerFiltro_x_Rol(_rol);
                var listaDto = _adaptadorDeObjetos.Adaptar<IEnumerable<AfiliacionesFiltroDto>>(lista);
                return listaDto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de periodos");
            }
        }


        public IEnumerable<AfiliacionesFiltroDto> ObtenerFiltroRoles(string[] roles)
        {
            try
            {
                var lista = _repositorioAfiliaciones.ObtenerFiltroRoles(roles);
                var listaDto = _adaptadorDeObjetos.Adaptar<IEnumerable<AfiliacionesFiltroDto>>(lista);
                return listaDto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de periodos");
            }
        }


        public IEnumerable<NovedadesHomologadasDto> Obtener_Novedades_Homologadas(List<string> comisiona)
        {
            try
            {
                var lista = _repositorioAfiliaciones.Obtener_Novedades_Homologadas(comisiona);
                var listaDto = _adaptadorDeObjetos.Adaptar<IEnumerable<NovedadesHomologadasDto>>(lista);
                return listaDto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de periodos");
            }
        }

        public IEnumerable<CiudadesDto> Obtener_Ciudades_Homologadas(string user)
        {
            try
            {
                var lista = _repositorioAfiliaciones.Obtener_Ciudades_Homologadas(user);
                var listaDto = _adaptadorDeObjetos.Adaptar<IEnumerable<CiudadesDto>>(lista);
                return listaDto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de periodos");
            }
        }

        public IEnumerable<CanalesDto> Obtener_Canales(string user)
        {
            try
            {
                var lista = _repositorioAfiliaciones.Obtener_Canales(user);
                var listaDto = _adaptadorDeObjetos.Adaptar<IEnumerable<CanalesDto>>(lista);
                return listaDto;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de periodos");
            }
        }

        public string MensajeCantidadRegistrosNetos(DatosConsultaAfiliacionDto datos)
        {
            try
            {
                var datosConsultaAfiliacion = new DatosConsultaAfiliacion()
                {
                    DOCUMENTO = datos.Documento,
                    ROL = datos.Rol,
                    TIP_CONTR = datos.TipContr
                };

                return _repositorioAfiliaciones.MensajeCantidadRegistrosNetos(datosConsultaAfiliacion);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo la cantidad de registros.");
            }
        }

        public List<CentrosCosto> ObtenerCentrosCosto()
        {
            try
            {
                return _repositorioAfiliaciones.ObtenerCentrosCosto();
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo centros de costo.");
            }
        }

        public List<Escalera> ObtenerEscaleras()
        {
            try
            {
                return _repositorioAfiliaciones.ObtenerEscaleras();
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo centros de costo.");
            }
        }
    }
}