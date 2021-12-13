using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioCargueTarifasPlenas
    {
        void InsertaCargueTarifasPlenas(CTB_TARIFAS_PLENAS _datos);

        void ActualizarEstadoCargueTarifasPlenas();

        IEnumerable<CTB_TARIFAS_PLENAS> ListaTarifasPlenas();
    }
}