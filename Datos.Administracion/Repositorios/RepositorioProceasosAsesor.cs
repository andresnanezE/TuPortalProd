using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.ModelKheiron;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioProceasosAsesor : IRepositorioProcesosAsesor
    {
        public IEnumerable<ProcesosAsesor> ProcesosAsesor(int mes, int anio, int ccAsesor)
        {
            IEnumerable<ProcesosAsesor> lista = null;
            using (var modelo = new ContextoStone())
            {
                lista = modelo.ConsultaSql<ProcesosAsesor>("EMSP_ProcesoAsesorMes @COD_ASES,@ANIO,@MES",
                    new SqlParameter("@COD_ASES", ccAsesor.ToString()),
                    new SqlParameter("@ANIO", anio.ToString()),
                    new SqlParameter("@MES", mes.ToString())
                    ).ToList();
            }
            return lista;
        }

        public IEnumerable<EMB_CIUDAD> ObtenerCiudades()
        {
            IEnumerable<EMB_CIUDAD> lista = new List<EMB_CIUDAD>();
            using (var modelo = new ContextoKheiron())
            {
                lista = modelo.ConsultaSql<EMB_CIUDAD>("SPEMB_CONSULTARCIUDADES").ToList();
            }
            return lista.ToList().OrderBy(x => x.CIUDAD);
        }
    }
}