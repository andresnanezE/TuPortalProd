using Dominio.Administracion.Entidades.ModeloStone;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;

namespace Datos.Administracion.UnidadDeTrabajo
{
    public class ContextoStone : DbContext
    {
        public ContextoStone()
            : base("name=AccesoStone")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<CC_CONTR> CC_CONTR { get; set; }
        public virtual DbSet<GN_TERCE> GN_TERCE { get; set; }
        public virtual DbSet<CC_ASESO> CC_ASESO { get; set; }
        public virtual DbSet<CC_TIPCO> CC_TIPCO { get; set; }
        public virtual DbSet<CC_DIREC> CC_DIREC { get; set; }
        public virtual DbSet<RE_ESCAL> RE_ESCAL { get; set; }

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