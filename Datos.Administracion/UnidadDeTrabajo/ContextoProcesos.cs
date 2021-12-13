using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Entidades.ModeloProcesos;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;

namespace Datos.Administracion.UnidadDeTrabajo
{
    public partial class ContextoProcesos : DbContext
    {
        public ContextoProcesos()
            : base("name=AccesoProcesos")
        {
            Database.CommandTimeout = 50000;
            Database.SetInitializer<ContextoProcesos>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<EMB_LogActividades> EMB_LogActividades { get; set; }
        public virtual DbSet<EMB_TipoLog> EMB_TipoLog { get; set; }
        public virtual DbSet<EMA_LOG> EMA_LOG { get; set; }

        //public virtual DbSet<EMA_MENU> EMA_MENU { get; set; }
        //public virtual DbSet<EMA_ROL> EMA_ROL { get; set; }
        //public virtual DbSet<EMA_ROLXMENU> EMA_ROLXMENU { get; set; }
        //public virtual DbSet<EMA_ROLXUSUARIO> EMA_ROLXUSUARIO { get; set; }
        //public virtual DbSet<EMA_USUARIO> EMA_USUARIO { get; set; }
        public virtual DbSet<EM_NOVEDAD_HOMOLOGADA> EM_NOVEDAD_HOMOLOGADA { get; set; }

        public virtual DbSet<EM_FILTROXROL_AFILIACIONES> EM_FILTROXROL_AFILIACIONES { get; set; }
        public virtual DbSet<EM_FILTRO_AFILIACIONES> EM_FILTRO_AFILIACIONES { get; set; }
        public virtual DbSet<EM_FILTRO_OPCION_AFILIACIONES> EM_FILTRO_OPCION_AFILIACIONES { get; set; }

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

        public DataSet ConsultaSqlDataSet(string sp, params object[] parametros)
        {
            DataSet resDt = new DataSet();
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
                do
                {
                    var tb = new DataTable();
                    tb.Load(reader);
                    resDt.Tables.Add(tb);
                } while (!reader.IsClosed);
            }

            return resDt;
        }

        public void EjecutaStoreProcedure(string storeProcedure, params object[] parametros)
        {
            DataSet resDt = new DataSet();
            Database.Connection.Open();
            DbCommand cmd = Database.Connection.CreateCommand();
            cmd.CommandText = storeProcedure;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var param in parametros)
            {
                cmd.Parameters.Add(param);
            }
            cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string storeProcedure, params object[] parametros)
        {
            using (DbCommand cmd = Database.Connection.CreateCommand())
            {
                cmd.CommandText = storeProcedure;
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var param in parametros)
                {
                    cmd.Parameters.Add(param);
                }

                Database.Connection.Open();

                return cmd.ExecuteScalar();
            }
        }
    }
}