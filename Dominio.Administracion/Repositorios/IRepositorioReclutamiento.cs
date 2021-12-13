using System;
using System.Collections.Generic;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Dominio.Administracion.Entidades.Reclutamiento;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioReclutamiento
    {
        bool CargueReclutamiento(List<FileReclutamiento> infoFile, int numeroDocumento, int estadoCargue);

        bool ActualizarCertificacion(Int64 numeroDocumento, string nombreOriginal, string nombreArchivo, string rutaArchivo);

        IEnumerable<NotaReclutamiento> ObtenerNotaReclutamiento(Int64 numeroDocumento);

        IEnumerable<EstadoArchivoReclutamiento> ObtenerEstadArchivoReclutamiento(int numeroDocumento);

        IEnumerable<ReclutamientoCiudad> ObtenerCiudadReclutamiento();

        IEnumerable<ReclutamientoReferido> ObtenerReferidosPorCiudad(int ciudadId);

        IEnumerable<SolicitudProspecto> ObtenerSolicitudesProspecto(int userId, bool first, string proceso, string estado, string director, DateTime? fechaInicio, DateTime? fechaFin, string numeroDocumento);

        IEnumerable<GestionContrato> ObtenerGestionContratoProspecto(int userId);

        bool ActualizarSolicitudProspecto(GestionSolicitud gestion);

        bool ActualizarSolicitudContrato(GestionSolicitudContrato gestion);

        bool ActualizarSolicitudCapacitacion(GestionCapacitacion gestion);

        IEnumerable<SolicitudesCapacitacion> ObtenerSolicitudesCapacitacion(int userId, string estado, bool first);

        IEnumerable<ReclutamientoCapacitador> ObtenerCapacitadores();

        IEnumerable<NotaCapacitacion> ObtenerNotasCapacitacion(int numeroDocumento);

        bool IngresoProspecto(Int64 numeroDocumento, string pass);

        bool ValidarIngresoProspecto(Int64 numeroDocumento, bool validate);

        IEnumerable<ReclutamientoPais> ObtenerPaisesReclutamiento();

        IEnumerable<ReclutamientoEps> ObtenerEpsReclutamiento();

        IEnumerable<ReclutamientoPensiones> ObtenerPensionesReclutamiento();

        IEnumerable<ReclutamientoNivelEducativo> ObtenerNivelEducativoReclutamiento();

        IEnumerable<ReclutamientoProfesiones> ObtenerProfesionesReclutamiento();

        bool CompletarRegistro(ReclutamientoRegistro registro);

        IEnumerable<SolicitudContratacion> ObtenerProcesoContrataciones(int userId, string estado, bool first);

        bool ActualizarSolicitudContratacion(GestionContratacion gestion);

        IEnumerable<ReclutamientoInfo> ObtenerInfoReclutamiento(Int64 numeroDocumento, bool recovery);

        bool ValidarRecovery(Int64 numeroDocumento, string recovery);

        bool ActualizarContrasenia(Int64 numeroDocumento, string passwordI, string rcvr);

        IEnumerable<ReclutamientoTipoIdentificacion> ObtenerTipoIdentificacion();

        ReclutamientoListaRestrictiva ValidarListaRestrictiva(string numeroDocumento);

        bool GuardarTrazaContrato(Int64 numeroDocumento, string ip);

        bool GestionarSucursal(int? id, string nombre, bool activo, string tipo);

        IEnumerable<ReclutamientoCiudad> ObtenerSucursales(int? id);

        bool GestionarCapacitador(int? id, int? idUsr, string nombres, string apellidos, bool activo, string tipo);

        IEnumerable<ReclutamientoCapacitador> ObtenerCapacitadores(int? id);

        IEnumerable<ReclutamientoInfoCompleta> ObtenerInformacionCompleta(Int64 numeroDocumento);

        bool ActualizarRegistro(ReclutamientoRegistro registro);

        IEnumerable<ReclutamientoProceso> ObtenerProcesoNumeroIdentificacion(Int64 numeroDocumento);

        IEnumerable<ReclutamientoDirector> ObtenerDirectoresPorReferido(int referidoId, string tipo);

        IEnumerable<ReclutamientoReclutadores> ObtenerReclutadores(int? id);

        bool GestionarReclutadores(int id, int? idUsr, string nombres, string apellidos, int? ciudadId, string[] directores, string tipo);

        IEnumerable<ReclutamientoDirectores> ObtenerDirectores();

        IEnumerable<ReclutamientoReclutadores> ObtenerReclutadorPorId(int id);

        ReclutamientoRepresentante ObtenerRepresentanteLegal();

        bool ActualizarCapacitacionReclutador(CapacitacionReclutador gestion);
    }
}