using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionCargueTarifasCampañas : IServicioAplicacionCargueTarifasCampañas
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioCargueTarifasCampañas _repositorioCargueTarifasCampaña;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionCargueTarifasCampañas(IRepositorioCargueTarifasCampañas repositorioCargueTarifasCampaña, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioCargueTarifasCampaña = repositorioCargueTarifasCampaña;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion C'tors

        public void InsertaCargueTarifaCampañas(CTB_TARIFAS_CAMPANA _datos)
        {
            _repositorioCargueTarifasCampaña.InsertaCargueTarifaCampañas(_datos);
        }

        public void ActualizarEstadoCargueTarifaCampañas()
        {
            _repositorioCargueTarifasCampaña.ActualizarEstadoCargueTarifaCampañas();
        }

        public IEnumerable<CTB_TARIFAS_CAMPANA> ListaTarifasCargue()
        {
            return _repositorioCargueTarifasCampaña.ListaTarifasCargue();
        }

        public void AgregarTarifaCampañas(IEnumerable<CTB_TARIFAS_CAMPANA> _datos)
        {
            _repositorioCargueTarifasCampaña.AgregarTarifaCampañas(_datos);
        }
    }
}