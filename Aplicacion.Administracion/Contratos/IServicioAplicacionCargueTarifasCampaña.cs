using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionCargueTarifasCampañas
    {
        void InsertaCargueTarifaCampañas(CTB_TARIFAS_CAMPANA _datos);

        void ActualizarEstadoCargueTarifaCampañas();

        IEnumerable<CTB_TARIFAS_CAMPANA> ListaTarifasCargue();

        void AgregarTarifaCampañas(IEnumerable<CTB_TARIFAS_CAMPANA> _datos);
    }
}