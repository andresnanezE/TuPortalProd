using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioRteApoyoRodamiento : IRepositorioRteApoyoRodamiento
    {
        public IEnumerable<ApoyoRodamientoPeriodo> PeriodosNoDefinitivos()
        {
            IEnumerable<ApoyoRodamientoPeriodo> lista = null;
            using (var modelo = new ContextEMERMEDICA_AreaETL())
            {
                lista = modelo.ConsultaSql<ApoyoRodamientoPeriodo>("EMSP_PERIODO_NO_CERRADO_APOYO_RODAMIENTO");
            }
            return lista;
        }
    }
}