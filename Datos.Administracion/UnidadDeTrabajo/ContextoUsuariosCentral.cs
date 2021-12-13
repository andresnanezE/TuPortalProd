using Dominio.Administracion.Entidades.ModeloCentralizada;
using System.Data.Entity;

namespace Datos.Administracion.UnidadDeTrabajo
{
    public class ContextoUsuariosCentral : DbContext
    {
        public ContextoUsuariosCentral()
            : base("name=AccesoCotizador")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<usrCentral> usrCentral { get; set; }
    }
}