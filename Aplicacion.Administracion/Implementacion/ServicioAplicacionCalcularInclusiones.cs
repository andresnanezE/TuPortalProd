using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    /// <summary>
    /// JohnNelsonRodriguex
    /// </summary>
    public class ServicioAplicacionCalcularInclusiones : IServicioAplicacionCalcularInclusiones

    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioCalcularInclusiones _repositorio;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionCalcularInclusiones(IRepositorioCalcularInclusiones repositorio, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorio = repositorio;
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

        public IEnumerable<CalcularInclusionesDto> CalcularInclusiones(int rmt, int nPersonas, DateTime fechaInclusion, decimal tarifa)
        {
            try
            {
                var resultado = _repositorio.CalcularInclusiones(rmt, nPersonas, fechaInclusion, tarifa);
                return _adaptadorDeObjetos.Adaptar<IEnumerable<CalcularInclusionesDto>>(resultado);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo datos para : CalcularInclusiones " + exception.Message);
            }
        }

        public IEnumerable<decimal> TarifasContrato(int rmt)
        {
            try
            {
                return _repositorio.TarifasContrato(rmt).ToList();
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo datos para : CalcularInclusiones " + exception.Message);
            }
        }

        public IEnumerable<TarifasInclusiones> TarifasInclusiones(int rmt)
        {
            try
            {
                return _repositorio.TarifasInclusiones(rmt);
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo datos para : CalcularInclusiones " + exception.Message); ;
            }
        }
    }
}