using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioCargueTarifasPlenas : IRepositorioCargueTarifasPlenas
    {
        public void InsertaCargueTarifasPlenas(CTB_TARIFAS_PLENAS _datos)
        {
            using (var modelo = new ContextoProcesos())
            {
                modelo.EjecutaStoreProcedure("CTSP_INSERTA_TARIFA_PLENA",
                    new SqlParameter("@I_CIUDAD", _datos.CIUDAD),
                    new SqlParameter("@I_TARIFA_PLENA", _datos.TARIFA_PLENA),
                    new SqlParameter("@I_ID_ESTADO", _datos.ID_ESTADO)
                    );
            }
        }

        public void ActualizarEstadoCargueTarifasPlenas()
        {
            using (var modelo = new ContextoProcesos())
            {
                modelo.EjecutaStoreProcedure("CTSP_ACTUALIZA_ESTADAO_TARIFA_PLENA");
            }
        }

        public IEnumerable<CTB_TARIFAS_PLENAS> ListaTarifasPlenas()
        {
            var res = new ConsultaDoble();
            IEnumerable<CTB_TARIFAS_PLENAS> lista = null;
            using (var modelo = new ContextoProcesos())
            {
                lista = modelo.Database.SqlQuery<CTB_TARIFAS_PLENAS>("CTSP_CONSULTA_CTB_TARIFA_PLENA").ToList();
            }
            return lista;
        }
    }
}