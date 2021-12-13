using Aplicacion.Administracion.Contratos;

//using Dominio.Administracion.Entidades.MapperDto;
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
    public class ServicioAplicacionSolicitudesInternas : IServicioAplicacionSolicitudesInternas
    {
        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioSolicitudesInternas _repositorioSolicitudesInternas;

        public ServicioAplicacionSolicitudesInternas(IRepositorioSolicitudesInternas repositorioSolicitudesInternas, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioSolicitudesInternas = repositorioSolicitudesInternas;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }

        public void AgregarNota(SolicitudesInternasNotasDto notas)
        {
            try
            {
                //var dto = _adaptadorDeObjetos.Adaptar<SolicitudesInternasNotas>(notas);
                var dto = new SolicitudesInternasNotas()
                {
                    archivo = notas.archivo,
                    url_archivo = notas.url_archivo,
                    Fecha = DateTime.Now,
                    IdNota = notas.IdNota,
                    IdTicket = notas.IdTicket,
                    nombre_archivo = notas.nombre_archivo,
                    Nota = notas.Nota
                };

                _repositorioSolicitudesInternas.AgregarNota(dto);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SolicitudesInternasNotasDto> ObtenerNotas(int IdTicket)
        {
            try
            {
                var resultado = _repositorioSolicitudesInternas.ObtenerNotas(IdTicket);

                var notas = _adaptadorDeObjetos.Adaptar<IEnumerable<SolicitudesInternasNotasDto>>(resultado);

                return notas;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public void AgregarSolicitusInterna(SolicitudesInternasDto solicitud)
        {
            try
            {
                //var dto = _adaptadorDeObjetos.Adaptar<SolicitudesInternas>(solicitud);

                var dto = new SolicitudesInternas()
                {
                    id_solicitud = solicitud.id_solicitud,
                    cod_ases = solicitud.cod_ases,
                    asunto = solicitud.asunto,
                    area = solicitud.area,
                    tipo_requerimiento = solicitud.tipo_requerimiento,
                    ciudad = solicitud.ciudad,
                    fecha_ini = solicitud.fecha_ini,
                    fecha_cierre = solicitud.fecha_cierre,
                    estado = solicitud.estado,
                    archivo = solicitud.archivo,
                    Descripcion = solicitud.Descripcion,
                    url_archivo = string.IsNullOrEmpty(solicitud.url_archivo) ? String.Empty : solicitud.url_archivo,
                    nombre_archivo = string.IsNullOrEmpty(solicitud.nombre_archivo) ? String.Empty : solicitud.nombre_archivo
                };
                _repositorioSolicitudesInternas.AgregarSolicitudInterna(dto);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }

        public IEnumerable<SolicitudesInternasDto> ObtenerSolicitudesInternas(decimal cod_ases)
        {
            try
            {
                var resultado = _repositorioSolicitudesInternas.ObtenerSolicitudesInternas(cod_ases);
                return _adaptadorDeObjetos.Adaptar<IEnumerable<SolicitudesInternasDto>>(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }
        }
    }
}