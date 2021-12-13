using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioCargueTarifasCampañas
    {
        void InsertaCargueTarifaCampañas(CTB_TARIFAS_CAMPANA _datos);

        void ActualizarEstadoCargueTarifaCampañas();

        IEnumerable<CTB_TARIFAS_CAMPANA> ListaTarifasCargue();

        void AgregarTarifaCampañas(IEnumerable<CTB_TARIFAS_CAMPANA> _datos);
    }
}