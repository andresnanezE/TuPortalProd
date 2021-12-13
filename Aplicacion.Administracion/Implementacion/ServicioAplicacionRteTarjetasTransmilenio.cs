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
    public class ServicioAplicacionRteTarjetasTransmilenio : IServicioAplicacionTarjetasTransmilenio
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioRteTarjetasTransmilenio _repositorioTarjetasTransmilenio;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionRteTarjetasTransmilenio(IRepositorioRteTarjetasTransmilenio repositorioTarjetasTransmilenio, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioTarjetasTransmilenio = repositorioTarjetasTransmilenio;
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

        public IEnumerable<TarjetasTransmilenioPeriodoDto> PeriodosNoDefinitivos()
        {
            try
            {
                List<TarjetasTransmilenioPeriodoDto> lst = new List<TarjetasTransmilenioPeriodoDto>();
                var lista = _repositorioTarjetasTransmilenio.PeriodosNoDefinitivos();

                foreach (TarjetasTransmilenioPeriodo item in lista)
                {
                    var tt = new TarjetasTransmilenioPeriodoDto();
                    tt.PERIODO = item.PERIODO;

                    lst.Add(tt);
                }

                return lst;
            }
            catch (SystemException exception)
            {
                throw LogExepxion(exception, "Error obteniendo lista de periodos");
            }
        }
    }
}