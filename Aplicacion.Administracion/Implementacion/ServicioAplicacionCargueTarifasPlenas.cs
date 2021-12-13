using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionCargueTarifasPlenas : IServicioAplicacionCargueTarifasPlenas
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioCargueTarifasPlenas _repositorioCargueTarifasPlenas;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionCargueTarifasPlenas(IRepositorioCargueTarifasPlenas repositoCargueTarifasPlenas, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioCargueTarifasPlenas = repositoCargueTarifasPlenas;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion C'tors

        public void ActualizarEstadoCargueTarifasPlenas()
        {
            _repositorioCargueTarifasPlenas.ActualizarEstadoCargueTarifasPlenas();
        }

        public void InsertaCargueTarifasPlenas(CTB_TARIFAS_PLENAS _datos)
        {
            _repositorioCargueTarifasPlenas.InsertaCargueTarifasPlenas(_datos);
        }

        public IEnumerable<CTB_TARIFAS_PLENAS> ListaTarifasPlenas()
        {
            return _repositorioCargueTarifasPlenas.ListaTarifasPlenas();
        }
    }
}