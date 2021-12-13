using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioCaracterizacionCampañas : IRepositorioCaracterizacionCampañas
    {
        public IEnumerable<TIPO_TARIFAtb> ListaTipoTarifa()
        {
            IEnumerable<TIPO_TARIFAtb> lista = null;
            using (var modelo = new ContextoProcesos())
            {
                lista = modelo.Database.SqlQuery<TIPO_TARIFAtb>("CTSP_CONSULTA_TIPO_TARIFA").ToList();
            }
            return lista;
        }

        public IEnumerable<CAMPAÑA> ListaCampañaXTipoTarifa(string tipoTarifa)
        {
            IEnumerable<CAMPAÑA> lista = null;
            using (var modelo = new ContextoProcesos())
            {
                lista = modelo.ConsultaSql<CAMPAÑA>("CTSP_CONSULTA_CAMPANA @CODIGO_TARIFA",
                    new SqlParameter("@CODIGO_TARIFA", tipoTarifa)
                    ).ToList();
            }
            return lista;
        }

        public IEnumerable<CTB_CAMPANA_CARACTERIZACION> ObtenerCampañaCaracterizacionFiltros(string tipoTarifa, string campaña)
        {
            IEnumerable<CTB_CAMPANA_CARACTERIZACION> lista = null;
            using (var modelo = new ContextoProcesos())
            {
                lista = modelo.ConsultaSql<CTB_CAMPANA_CARACTERIZACION>("CTSP_CONSULTA_CAMPANA_CARACTERIZACION @TIPO_TARIFA,@CAMPANA_TARIFA",
                    new SqlParameter("@TIPO_TARIFA", tipoTarifa),
                    new SqlParameter("@CAMPANA_TARIFA", campaña)
                    ).ToList();
            }
            return lista;
        }

        public void ModificarCampañaCaracterizacion(CTB_CAMPANA_CARACTERIZACION campañaModificar)
        {
            using (var modelo = new ContextoProcesos())
            {
                modelo.EjecutaStoreProcedure("CTSP_INSERTA_CAMPANA_CARACTERIZACION",
                    new SqlParameter("@I_TIPO_TARIFA", campañaModificar.TIPO_TARIFA),
                    new SqlParameter("@I_CAMPANA_TARIFA", campañaModificar.CAMPANA_TARIFA),
                    new SqlParameter("@I_RUTA_IMAGEN", campañaModificar.RUTA_IMAGEN),
                    new SqlParameter("@I_CARACTERIZACION", campañaModificar.CARACTERIZACION)
                    );
            }
        }
    }
}