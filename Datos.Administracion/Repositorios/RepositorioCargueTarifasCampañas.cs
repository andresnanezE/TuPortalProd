using Datos.Administracion.UnidadDeTrabajo;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Datos.Administracion.Repositorios
{
    public class RepositorioCargueTarifasCampañas : IRepositorioCargueTarifasCampañas
    {
        public void AgregarTarifaCampañas(IEnumerable<CTB_TARIFAS_CAMPANA> _datos)
        {
            try
            {
                DataTable dtTarifas = new DataTable("Tarifas");

                dtTarifas.Columns.Add("ID", Type.GetType("System.Int32"));

                dtTarifas.Columns[0].AutoIncrement = true;
                dtTarifas.Columns[0].AutoIncrementSeed = 1;
                dtTarifas.Columns[0].AutoIncrementStep = 1;

                dtTarifas.Columns.Add("TIPO_TARIFA", Type.GetType("System.String"));
                dtTarifas.Columns.Add("CAMPANA_TARIFA", Type.GetType("System.String"));
                dtTarifas.Columns.Add("CIUDAD", Type.GetType("System.String"));
                dtTarifas.Columns.Add("RANGO_INICIAL_PERSONA", Type.GetType("System.Int32"));
                dtTarifas.Columns.Add("RANGO_FINAL_PERSONA", Type.GetType("System.Int32"));
                dtTarifas.Columns.Add("MODALIDAD_PAGO", Type.GetType("System.String"));
                dtTarifas.Columns.Add("FORMA_PAGO", Type.GetType("System.String"));
                dtTarifas.Columns.Add("VALOR_TARIFA", Type.GetType("System.Decimal"));
                dtTarifas.Columns.Add("VALOR_IVA_TARIFA", Type.GetType("System.Decimal"));
                dtTarifas.Columns.Add("FECHA_VENCIMIENTO_TARIFA", Type.GetType("System.DateTime"));
                dtTarifas.Columns.Add("ID_ESTADO", Type.GetType("System.Int32"));

                _datos.ToList<CTB_TARIFAS_CAMPANA>().ForEach(delegate (CTB_TARIFAS_CAMPANA t)
                {
                    DataRow d = dtTarifas.NewRow();

                    d["TIPO_TARIFA"] = t.TIPO_TARIFA;
                    d["CAMPANA_TARIFA"] = t.CAMPANA_TARIFA;
                    d["CIUDAD"] = t.CIUDAD;
                    d["RANGO_INICIAL_PERSONA"] = t.RANGO_INICIAL_PERSONA;
                    d["RANGO_FINAL_PERSONA"] = t.RANGO_FINAL_PERSONA;
                    d["MODALIDAD_PAGO"] = t.MODALIDAD_PAGO;
                    d["FORMA_PAGO"] = t.FORMA_PAGO;
                    d["VALOR_TARIFA"] = t.VALOR_TARIFA;
                    d["VALOR_IVA_TARIFA"] = t.VALOR_IVA_TARIFA;
                    d["FECHA_VENCIMIENTO_TARIFA"] = t.FECHA_VENCIMIENTO_TARIFA;
                    d["ID_ESTADO"] = t.ID_ESTADO;

                    dtTarifas.Rows.Add(d);
                });

                using (var context = new ContextoProcesos())
                {
                    string cnn = ((System.Data.SqlClient.SqlConnection)context.Database.Connection).ConnectionString;
                    context.Database.ExecuteSqlCommand("TRUNCATE TABLE TB_TARIFAS_CAMPANA_TMP");

                    SqlBulkCopy sbc = new SqlBulkCopy(cnn);
                    sbc.DestinationTableName = "TB_TARIFAS_CAMPANA_TMP";
                    sbc.WriteToServer(dtTarifas);

                    context.EjecutaStoreProcedure("CTSP_CREAR_TARIFAS_CAMPANA");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void InsertaCargueTarifaCampañas(CTB_TARIFAS_CAMPANA _datos)
        {
            using (var modelo = new ContextoProcesos())
            {
                modelo.EjecutaStoreProcedure("CTSP_INSERTA_TARIFAS_CAMPANA",
                    new SqlParameter("@TIPO_TARIFA", _datos.TIPO_TARIFA),
                    new SqlParameter("@CAMPANA_TARIFA", _datos.CAMPANA_TARIFA),
                    new SqlParameter("@CIUDAD", _datos.CIUDAD),
                    new SqlParameter("@RANGO_INICIAL_PERSONA", _datos.RANGO_INICIAL_PERSONA),
                    new SqlParameter("@RANGO_FINAL_PERSONA", _datos.RANGO_FINAL_PERSONA),
                    new SqlParameter("@MODALIDAD_PAGO", _datos.MODALIDAD_PAGO),
                    new SqlParameter("@FORMA_PAGO", _datos.FORMA_PAGO),
                    new SqlParameter("@VALOR_TARIFA", _datos.VALOR_TARIFA),
                    new SqlParameter("@VALOR_IVA_TARIFA", _datos.VALOR_IVA_TARIFA),
                    new SqlParameter("@FECHA_VENCIMIENTO_TARIFA", _datos.FECHA_VENCIMIENTO_TARIFA.ToString("s")),
                    new SqlParameter("@ID_ESTADO", _datos.ID_ESTADO)
                    );
            }
        }

        public void ActualizarEstadoCargueTarifaCampañas()
        {
            using (var modelo = new ContextoProcesos())
            {
                modelo.EjecutaStoreProcedure("CTSP_ACTUALIZA_ESTADAO_TARIFAS_CAMPANA");
            }
        }

        public IEnumerable<CTB_TARIFAS_CAMPANA> ListaTarifasCargue()
        {
            var res = new ConsultaDoble();
            IEnumerable<CTB_TARIFAS_CAMPANA> lista = null;
            using (var modelo = new ContextoProcesos())
            {
                lista = modelo.Database.SqlQuery<CTB_TARIFAS_CAMPANA>("CTSP_CONSULTA_CTB_TARIFAS_CAMPANA").ToList();
            }
            return lista;
        }
    }
}