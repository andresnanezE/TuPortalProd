using Aplicacion.Administracion.Contratos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio.Administracion.Repositorios;
using Dominio.Administracion.Entidades;
using System.ServiceModel;
using System.Runtime.CompilerServices;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioArchivosVentas : IServicioArchivosVentas
    {
        private const string ErrorProcesandoPeticion = "Ha ocurrido un error procesando la petición. Comuníquese con su administrador";
        private readonly IRepositorioArchivosVentas _repositorioVentas;
        private readonly IRepositorioLogs _manejadorLogs;


        public ServicioArchivosVentas(IRepositorioArchivosVentas repositorioArchivos, IRepositorioLogs manejadorLogs)
        {
            _repositorioVentas = repositorioArchivos;
            _manejadorLogs = manejadorLogs;

        }

        public async Task<List<string>> GetArchivosVentas(string documento)
        {
            try
            {
                var Archivos = new List<string>();
                Archivos = await _repositorioVentas.GetArchivosVentas(documento);
                return Archivos;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }

        }

        public async Task<ArchivoBlob> GetArchivoBlob(string fileName)
        {
            try
            {
                return await _repositorioVentas.GetArchivoBlob(fileName);

            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, ErrorProcesandoPeticion);
            }

        }
        private FaultException<ErrorServicio> LogExepxion(SystemException exepcion, string mensajeError, [CallerMemberName] string metodo = "")
        {
            var razon = new FaultReason(mensajeError);
            _manejadorLogs.LogAplicacion(metodo, null, exepcion);
            return new FaultException<ErrorServicio>(new ErrorServicio(mensajeError), razon);
        }
    }
}
