using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using Transversales.Administracion;

namespace Aplicacion.Administracion.Implementacion
{
    public class ServicioAplicacionCaracterizacionCampañas : IServicioAplicacionCaracterizacionCampañas
    {
        #region Readonly & Static Fields

        private readonly IAdaptadorDeObjetos _adaptadorDeObjetos;
        private readonly IRepositorioLogs _manejadorLogs;
        private readonly IRepositorioCaracterizacionCampañas _repositorioCaracterizacionCampañas;

        #endregion Readonly & Static Fields

        #region C'tors

        public ServicioAplicacionCaracterizacionCampañas(IRepositorioCaracterizacionCampañas repositorioCaracterizacionCampañas, IRepositorioLogs manejadorLogs, IAdaptadorDeObjetos adaptadorDeObjetos)
        {
            _repositorioCaracterizacionCampañas = repositorioCaracterizacionCampañas;
            _manejadorLogs = manejadorLogs;
            _adaptadorDeObjetos = adaptadorDeObjetos;
        }

        #endregion C'tors

        public IEnumerable<TIPO_TARIFAtb> ListaTipoTarifa()
        {
            return _repositorioCaracterizacionCampañas.ListaTipoTarifa();
        }

        public IEnumerable<CAMPAÑA> ListaCampañaXTipoTarifa(string tipoTarifa)
        {
            return _repositorioCaracterizacionCampañas.ListaCampañaXTipoTarifa(tipoTarifa);
        }

        public IEnumerable<CTB_CAMPANA_CARACTERIZACION> ObtenerCampañaCaracterizacionFiltros(string tipoTarifa, string campaña)
        {
            return _repositorioCaracterizacionCampañas.ObtenerCampañaCaracterizacionFiltros(tipoTarifa, campaña);
        }

        public void ModificarCampañaCaracterizacion(CTB_CAMPANA_CARACTERIZACION campañaModificar)
        {
            _repositorioCaracterizacionCampañas.ModificarCampañaCaracterizacion(campañaModificar);
        }
    }
}