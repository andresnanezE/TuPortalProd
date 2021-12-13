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
    public class ServicioAplicacionRteApoyoRodamiento : IServicioAplicacionApoyoRodamiento
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioRteApoyoRodamiento _repositorioApoyoRodamiento;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionRteApoyoRodamiento(IRepositorioRteApoyoRodamiento repositorioApoyoRodamiento, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioApoyoRodamiento = repositorioApoyoRodamiento;
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

        public IEnumerable<ApoyoRodamientoPeriodoDto> PeriodosNoDefinitivos()
        {
            try
            {
                List<ApoyoRodamientoPeriodoDto> lst = new List<ApoyoRodamientoPeriodoDto>();
                var lista = _repositorioApoyoRodamiento.PeriodosNoDefinitivos();

                foreach (ApoyoRodamientoPeriodo item in lista)
                {
                    var tt = new ApoyoRodamientoPeriodoDto();
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