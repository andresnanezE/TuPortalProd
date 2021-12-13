using Dominio.Administracion.Entidades.ModelEMERMEDICA_AreaETL;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;

namespace Datos.Administracion.UnidadDeTrabajo
{
    public class ContextEMERMEDICA_AreaETL : DbContext
    {
        public ContextEMERMEDICA_AreaETL()
            : base("name=AccesoAreaETL")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<EME_CIUDAD_HOMOLOG> EME_CIUDAD_HOMOLOG { get; set; }
        public virtual DbSet<EMB_Canal> EME_Canal { get; set; }

        public IEnumerable<T> ConsultaSql<T>(string sp, params object[] parametros) where T : class
        {
            return Database.SqlQuery<T>("exec " + sp, parametros).ToList();
        }

        public void ComandoSql(string sp, params object[] parametros)
        {
            Database.SqlQuery<int>("exec " + sp, parametros);
        }

        public DataTable ConsultaSqlDataTable(string sp, params object[] parametros)
        {
            DataTable resDt = new DataTable();
            Database.Connection.Open();
            DbCommand cmd = Database.Connection.CreateCommand();
            cmd.CommandText = sp;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var param in parametros)
            {
                cmd.Parameters.Add(param);
            }
            using (var reader = cmd.ExecuteReader())
            {
                resDt.Load(reader);
            }
            return resDt;
        }
    }
}