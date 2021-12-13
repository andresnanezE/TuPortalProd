using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioRteTarjetasTransmilenio : IRepositorioRteTarjetasTransmilenio
    {
        public IEnumerable<TarjetasTransmilenioPeriodo> PeriodosNoDefinitivos()
        {
            IEnumerable<TarjetasTransmilenioPeriodo> lista = null;
            using (var modelo = new ContextEMERMEDICA_AreaETL())
            {
                lista = modelo.ConsultaSql<TarjetasTransmilenioPeriodo>("EMSP_PERIODO_NO_CERRADO");
            }
            return lista;
        }
    }
}