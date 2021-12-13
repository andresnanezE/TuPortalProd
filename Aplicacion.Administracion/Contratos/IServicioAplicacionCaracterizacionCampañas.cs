using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionCaracterizacionCampañas
    {
        IEnumerable<TIPO_TARIFAtb> ListaTipoTarifa();

        IEnumerable<CAMPAÑA> ListaCampañaXTipoTarifa(string tipoTarifa);

        IEnumerable<CTB_CAMPANA_CARACTERIZACION> ObtenerCampañaCaracterizacionFiltros(string tipoTarifa, string campaña);

        void ModificarCampañaCaracterizacion(CTB_CAMPANA_CARACTERIZACION campañaModificar);
    }
}