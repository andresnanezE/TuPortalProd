using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dominio.Administracion.Entidades;
using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Repositorios;
using Dominio.Administracion.Entidades.Reclutamiento;
using Dominio.Administracion.Entidades.ModeloCotizacion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioReclutamiento : IServicioReclutamiento
    {
        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";

        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioReclutamiento _repositorioReclutamiento;

        public ServicioReclutamiento(IRepositorioLogs manejadorLogs, IRepositorioReclutamiento repositioReclutamiento)
        {
            _manejadorLogs = manejadorLogs;
            _repositorioReclutamiento = repositioReclutamiento;
        }

        private FaultException<ErrorServicio> RegiExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        public bool CargueReclutamiento(List<FileReclutamiento> infoFile, int numeroDocumento, int estadoCargue)
        {
            try
            {
                var result = _repositorioReclutamiento.CargueReclutamiento(infoFile, numeroDocumento, estadoCargue);
                return result;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool ActualizarCertificacion(Int64 numeroDocumento, string nombreOriginal, string nombreArchivo, string rutaArchivo)
        {
            try
            {
                var result = _repositorioReclutamiento.ActualizarCertificacion(numeroDocumento, nombreOriginal, nombreArchivo, rutaArchivo);
                return result;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<NotaReclutamiento> ObtenerNotaReclutamiento(Int64 numeroDocumento)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerNotaReclutamiento(numeroDocumento);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<EstadoArchivoReclutamiento> ObtenerEstadArchivoReclutamiento(int numeroDocumento)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerEstadArchivoReclutamiento(numeroDocumento);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoCiudad> ObtenerCiudadReclutamiento()
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerCiudadReclutamiento();
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoReferido> ObtenerReferidosPorCiudad(int ciudadId)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerReferidosPorCiudad(ciudadId);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SolicitudProspecto> ObtenerSolicitudesProspecto(int userId, bool first, string proceso, string estado, string director, DateTime? fechaInicio, DateTime? fechaFin, string numeroDocumento)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerSolicitudesProspecto(userId, first, proceso, estado, director, fechaInicio, fechaFin, numeroDocumento);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<GestionContrato> ObtenerGestionContratoProspecto(int userId)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerGestionContratoProspecto(userId);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool ActualizarSolicitudProspecto(GestionSolicitud gestion)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ActualizarSolicitudProspecto(gestion);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool ActualizarSolicitudContrato(GestionSolicitudContrato gestion)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ActualizarSolicitudContrato(gestion);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool ActualizarSolicitudCapacitacion(GestionCapacitacion gestion)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ActualizarSolicitudCapacitacion(gestion);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SolicitudesCapacitacion> ObtenerSolicitudesCapacitacion(int userId, string estado, bool first)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerSolicitudesCapacitacion(userId, estado, first);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoCapacitador> ObtenerCapacitadores()
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerCapacitadores();
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<NotaCapacitacion> ObtenerNotasCapacitacion(int numeroDocumento)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerNotasCapacitacion(numeroDocumento);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool IngresoProspecto(Int64 numeroDocumento, string pass)
        {
            try
            {
                var entidad = _repositorioReclutamiento.IngresoProspecto(numeroDocumento, pass);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool ValidarIngresoProspecto(Int64 numeroDocumento, bool validate)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ValidarIngresoProspecto(numeroDocumento, validate);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoPais> ObtenerPaisesReclutamiento()
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerPaisesReclutamiento();
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoEps> ObtenerEpsReclutamiento()
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerEpsReclutamiento();
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoPensiones> ObtenerPensionesReclutamiento()
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerPensionesReclutamiento();
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoNivelEducativo> ObtenerNivelEducativoReclutamiento()
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerNivelEducativoReclutamiento();
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoProfesiones> ObtenerProfesionesReclutamiento()
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerProfesionesReclutamiento();
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool CompletarRegistro(ReclutamientoRegistro registro)
        {
            try
            {
                var entidad = _repositorioReclutamiento.CompletarRegistro(registro);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SolicitudContratacion> ObtenerProcesoContrataciones(int userId, string estado, bool first)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerProcesoContrataciones(userId, estado, first);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool ActualizarSolicitudContratacion(GestionContratacion gestion)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ActualizarSolicitudContratacion(gestion);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoInfo> ObtenerInfoReclutamiento(Int64 numeroDocumento, bool recovery)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerInfoReclutamiento(numeroDocumento, recovery);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool ValidarRecovery(Int64 numeroDocumento, string recovery)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ValidarRecovery(numeroDocumento, recovery);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool ActualizarContrasenia(Int64 numeroDocumento, string passwordI, string rcvr)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ActualizarContrasenia(numeroDocumento, passwordI, rcvr);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoTipoIdentificacion> ObtenerTipoIdentificacion()
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerTipoIdentificacion();
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public ReclutamientoListaRestrictiva ValidarListaRestrictiva(string numeroDocumento)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ValidarListaRestrictiva(numeroDocumento);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool GuardarTrazaContrato(Int64 numeroDocumento, string ip)
        {
            try
            {
                var entidad = _repositorioReclutamiento.GuardarTrazaContrato(numeroDocumento, ip);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool GestionarSucursal(int? id, string nombre, bool activo, string tipo)
        {
            try
            {
                var entidad = _repositorioReclutamiento.GestionarSucursal(id, nombre, activo, tipo);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoCiudad> ObtenerSucursales(int? id)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerSucursales(id);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool GestionarCapacitador(int? id, int? idUsr, string nombres, string apellidos, bool activo, string tipo)
        {
            try
            {
                var entidad = _repositorioReclutamiento.GestionarCapacitador(id, idUsr, nombres, apellidos, activo, tipo);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoCapacitador> ObtenerCapacitadores(int? id)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerCapacitadores(id);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoInfoCompleta> ObtenerInformacionCompleta(Int64 numeroDocumento)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerInformacionCompleta(numeroDocumento);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool ActualizarRegistro(ReclutamientoRegistro registro)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ActualizarRegistro(registro);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoProceso> ObtenerProcesoNumeroIdentificacion(Int64 numeroDocumento)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerProcesoNumeroIdentificacion(numeroDocumento);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoDirector> ObtenerDirectoresPorReferido(int referidoId, string tipo)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerDirectoresPorReferido(referidoId, tipo);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoReclutadores> ObtenerReclutadores(int? id)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerReclutadores(id);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool GestionarReclutadores(int id, int? idUsr, string nombres, string apellidos, int? ciudadId, string[] directores, string tipo)
        {
            try
            {
                var entidad = _repositorioReclutamiento.GestionarReclutadores(id, idUsr, nombres, apellidos, ciudadId, directores, tipo);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoDirectores> ObtenerDirectores()
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerDirectores();
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<ReclutamientoReclutadores> ObtenerReclutadorPorId(int id)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerReclutadorPorId(id);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public ReclutamientoRepresentante ObtenerRepresentanteLegal()
        {
            try
            {
                var entidad = _repositorioReclutamiento.ObtenerRepresentanteLegal();
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public bool ActualizarCapacitacionReclutador(CapacitacionReclutador gestion)
        {
            try
            {
                var entidad = _repositorioReclutamiento.ActualizarCapacitacionReclutador(gestion);
                return entidad;
            }
            catch (SystemException exception)
            {
                throw RegiExepxion(exception, ErrorProcesandoPeticion);
            }
        }
    }
}