using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Aplicacion.Administracion.Contratos
{
    public interface IServicioAplicacionCargueTarifasPlenas
    {
        void InsertaCargueTarifasPlenas(CTB_TARIFAS_PLENAS _datos);

        void ActualizarEstadoCargueTarifasPlenas();

        IEnumerable<CTB_TARIFAS_PLENAS> ListaTarifasPlenas();
    }
}