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
    public class ServicioAplicacionEsCadenaSupervision : IServicioAplicacionEsCadenaSupervision
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioEsCadenaSupervision _repositorioEsCadenaSupervision;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionEsCadenaSupervision(IRepositorioEsCadenaSupervision repositorioEsCadenaSupervision, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioEsCadenaSupervision = repositorioEsCadenaSupervision;
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

        public IEnumerable<CadenaSupervisionDto> EsCadenaSupervision(string cc)
        {
            try
            {
                //List<CadenaSupervisionDto> lst = new List<CadenaSupervisionDto>();
                var lista = _repositorioEsCadenaSupervision.EsCadenaSupervision(cc);

                var lista2 = _adaptadorDeObjetos.Adaptar<IEnumerable<CadenaSupervisionDto>>(lista);

                return lista2;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error invocando el proceso.");
            }
        }
    }
}