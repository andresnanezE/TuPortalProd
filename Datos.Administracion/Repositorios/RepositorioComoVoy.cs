using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioComoVoy : IRepositorioComoVoy
    {
        public IEnumerable<ComoVoy> CantidadVentasAsesorMesActual(decimal ccAsesor, string rol, string pathImagenesComoVoy, string urlActionDownloadPdf)
        {
            using (var modelo = new ContextoPortal())
            {
                return modelo.ConsultaSql<ComoVoy>("EMSP_INFO_VENTAS_ASESOR " + ccAsesor.ToString() + " ," + rol + ",'" + pathImagenesComoVoy + "'" + ",'" + urlActionDownloadPdf + "'").ToList();
            }
        }

        public string ObtenerPlanesDeVuelo()
        {
            using (var modelo = new ContextoProcesos())
            {
                return modelo.ExecuteScalar("EMESP_PLANES_DE_VUELO",
                   new SqlParameter("@PLANVUELO1", ""),
                   new SqlParameter("@PLANVUELO2", ""),
                   new SqlParameter("@TIP_OP", 1)
                   ).ToString();
            }
        }

        public void ModificarPlanesDeVuelo(string plan1, string plan2)
        {
            using (var modelo = new ContextoProcesos())
            {
                modelo.EjecutaStoreProcedure("EMESP_PLANES_DE_VUELO",
                   new SqlParameter("@PLANVUELO1", plan1),
                   new SqlParameter("@PLANVUELO2", plan2),
                   new SqlParameter("@TIP_OP", 2)
                   );
            }
        }
    }
}