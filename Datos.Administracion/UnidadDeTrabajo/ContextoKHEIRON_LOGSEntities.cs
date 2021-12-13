using Dominio.Administracion.Entidades.ModelKheiron_Logs;
using System.Data.Entity;

namespace Datos.Administracion.UnidadDeTrabajo
{
    public partial class ContextoKHEIRON_LOGSEntities : DbContext
    {
        public ContextoKHEIRON_LOGSEntities()
            : base("name=KHEIRON_LOGSEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<EMH_ENVIO_CORREO> EMH_ENVIO_CORREO { get; set; }
        public virtual DbSet<EMH_ENVIO_SMS> EMH_ENVIO_SMS { get; set; }
    }
}