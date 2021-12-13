using ApiCertificadoTributario.Data.Configuration;
using ApiCertificadoTributario.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace ApiCertificadoTributario.Data
{
    public class DatabaseCertificado : DbContext
    {
        private IDbSet<EnvioCertificadoTributario> _envioCertificadoTributario;
        private IDbSet<ProcesoEnvioCertificadoTributario> _procesoEnvioCertificadoTributario;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EnvioCertificadoTributarioConfiguration());
            modelBuilder.Configurations.Add(new ProcesoEnvioCertificadoTributarioConfiguration());
        }
        static DatabaseCertificado()
        {
            Database.SetInitializer(new NullDatabaseInitializer<DatabaseCertificado>());
        }

        public DatabaseCertificado() : base("name=DatabaseCertificado")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
            Database.CommandTimeout = 60000;
        }

        public IDbSet<EnvioCertificadoTributario> EnvioCertificadoTributario
        {
            get { return _envioCertificadoTributario ?? (_envioCertificadoTributario = Set<EnvioCertificadoTributario>()); }
        }
        public IDbSet<ProcesoEnvioCertificadoTributario> ProcesoEnvioCertificadoTributario
        {
            get { return _procesoEnvioCertificadoTributario ?? (_procesoEnvioCertificadoTributario = Set<ProcesoEnvioCertificadoTributario>()); }
        }
    }
}
