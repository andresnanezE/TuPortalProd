using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioCaracterizacionCampañas
    {
        IEnumerable<TIPO_TARIFAtb> ListaTipoTarifa();

        IEnumerable<CAMPAÑA> ListaCampañaXTipoTarifa(string tipoTarifa);

        IEnumerable<CTB_CAMPANA_CARACTERIZACION> ObtenerCampañaCaracterizacionFiltros(string tipoTarifa, string campaña);

        void ModificarCampañaCaracterizacion(CTB_CAMPANA_CARACTERIZACION campañaModificar);
    }
}