using Dominio.Administracion.Entidades.ModeloCentralizada;
using Dominio.Administracion.Entidades.ModeloProcesos;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Datos.Administracion.UnidadDeTrabajo
{
    public class ContextoUsuarios : DbContext
    {
        public ContextoUsuarios()
            : base("name=CentralizadaEntities1")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<usrCentral> usrCentral { get; set; }
        public virtual DbSet<USR_APL_ROL> UsuarioXappXrol { get; set; }
        public virtual DbSet<Dominio.Administracion.Entidades.ModeloCentralizada.Roles> Roles { get; set; }
        public virtual DbSet<EMA_MENU> EMA_MENU { get; set; }

        //public virtual DbSet<EMA_ROL> EMA_ROL { get; set; }
        public virtual DbSet<EMA_ROLXMENU> EMA_ROLXMENU { get; set; }

        public IEnumerable<T> ConsultaSqlSp<T>(string sp, params object[] parametros) where T : class
        {
            return Database.SqlQuery<T>("exec " + sp, parametros).ToList();
        }

        public void ComandoSql(string sp, params object[] parametros)
        {
            Database.SqlQuery<int>("exec " + sp, parametros);
        }

        public DataTable ConsultaSqlDataTableSp(string sp, params object[] parametros)
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

        public DataTable ConsultaSqlDataTable(string sp, params object[] parametros)
        {
            DataTable resDt = new DataTable();
            Database.Connection.Open();
            DbCommand cmd = Database.Connection.CreateCommand();
            cmd.CommandText = sp;
            cmd.CommandType = CommandType.Text;
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

        public IEnumerable<T> ConsultaSql<T>(string consulta, params object[] parametros) where T : class
        {
            return Database.SqlQuery<T>(consulta, parametros).ToList();
        }
    }
}