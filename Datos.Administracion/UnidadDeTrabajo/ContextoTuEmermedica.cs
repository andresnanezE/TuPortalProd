using System.Collections.Generic;
using System.Data.Entity;

namespace Datos.Administracion.UnidadDeTrabajo
{
    public partial class ContextoTuEmermedica : DbContext
    {
        public ContextoTuEmermedica()
            : base("name=TuEmermedica")
        {
        }

        public IEnumerable<T> ConsultaSql<T>(string sp, params object[] parametros) where T : class
        {
            return Database.SqlQuery<T>("exec " + sp, parametros);
        }

        public void ComandoSql(string sp, params object[] parametros)
        {
            Database.SqlQuery<int>("exec " + sp, parametros);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}