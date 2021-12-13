using Dominio.Administracion.Entidades.ModeloCotizacion;
using Dominio.Administracion.Entidades.ModeloPortal;
using Dominio.Administracion.Entidades.ModeloProcesos;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Datos.Administracion.UnidadDeTrabajo
{
    public partial class ContextoPortal : DbContext
    {
        public ContextoPortal()
            : base("name=AccesoPortal")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 200;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CiudadesFactor>().Property(x => x.Valor).HasPrecision(18, 9);
            modelBuilder.Entity<Prima>().Property(x => x.Valor).HasPrecision(18, 9);
            modelBuilder.Entity<TipoRiesgo>().Property(x => x.Valor).HasPrecision(18, 9);
            modelBuilder.Entity<SECTOR>().Property(x => x.Valor).HasPrecision(18, 9);
            modelBuilder.Entity<Tipos_AP>().Property(x => x.Valor).HasPrecision(18, 9);
            modelBuilder.Entity<FactorAdicionalUno>().Property(x => x.Valor).HasPrecision(18, 9);
            modelBuilder.Entity<FactorAdicionalDos>().Property(x => x.Valor).HasPrecision(18, 9);
            modelBuilder.Entity<FactorAdicionalTres>().Property(x => x.Valor).HasPrecision(18, 9);
            modelBuilder.Entity<FactorAdicionalCuatro>().Property(x => x.Valor).HasPrecision(18, 9);
            modelBuilder.Entity<FactorAdicionalCinco>().Property(x => x.Valor).HasPrecision(18, 9);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<EMB_LogActividades> EMB_LogActividades { get; set; }
        public virtual DbSet<EMA_LOG> EMA_LOG { get; set; }
        public virtual DbSet<EMA_MENU> EMA_MENU { get; set; }

        //public virtual DbSet<EMA_ROL> EMA_ROL { get; set; }
        public virtual DbSet<EMA_ROLXMENU> EMA_ROLXMENU { get; set; }

        //public virtual DbSet<EMA_ROLXUSUARIO> EMA_ROLXUSUARIO { get; set; }
        //public virtual DbSet<EMA_USUARIO> EMA_USUARIO { get; set; }
        public virtual DbSet<DYK_BANNER> DYK_BANNER { get; set; }

        public virtual DbSet<DYK_DESTACADO> DYK_DESTACADO { get; set; }
        public virtual DbSet<DYK_NOTICIA> DYK_NOTICIA { get; set; }
        public virtual DbSet<EME_METAS_VENTAS> EME_METAS_VENTAS { get; set; }
        public virtual DbSet<EME_REGISTRO_VENTAS> EME_REGISTRO_VENTAS { get; set; }

        //Entidades basadas en tablas de BD y necesarias para la funcionalidad de bloqueo NIT y calculo cotizaciones
        public virtual DbSet<Cotizaciones> Cotizaciones { get; set; }
        public virtual DbSet<Cotizacion_NotaTracking> Cotizaciones_Notas { get; set; }

        public virtual DbSet<Cotizacion_Asesor> Cotizacion_Asesor { get; set; }
        public virtual DbSet<Cotizacion_Producto> Cotizacion_Producto { get; set; }
        public virtual DbSet<NivelesInteres> NivelesInteres { get; set; }
        public virtual DbSet<EstadosCotizacions> EstadosCotizacion { get; set; }
        public virtual DbSet<EstadosCotizacion> EstadosCotizacionSCI { get; set; }
        public virtual DbSet<PoliticaNITS> PoliticaNITS { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<Tipos_AP> Tipos_AP { get; set; }
        public virtual DbSet<SECTOR> SECTORES { get; set; }
        public virtual DbSet<Info_Pricing> Datos_Pricing { get; set; }
        public virtual DbSet<ESTADOS_COTI_PRIC> EstadosPricing { get; set; }
        public virtual DbSet<Factores> Factores { get; set; }
        public virtual DbSet<CiudadesFactor> CiudadesFactor { get; set; }
        public virtual DbSet<TipoRiesgo> TipoRiesgo { get; set; }
        public virtual DbSet<Prima> Prima { get; set; }
        public virtual DbSet<FactorAdicionalUno> FactorAdicional1 { get; set; }
        public virtual DbSet<FactorAdicionalDos> FactorAdicional2 { get; set; }
        public virtual DbSet<FactorAdicionalTres> FactorAdicional3 { get; set; }
        public virtual DbSet<FactorAdicionalCuatro> FactorAdicional4 { get; set; }
        public virtual DbSet<FactorAdicionalCinco> FactorAdicional5 { get; set; }
        public virtual DbSet<Detalle_Factor> Detalle_Factor { get; set; }
        public virtual DbSet<Cotizacion_Sedes> Cotizacion_Sedes { get; set; }

        public IEnumerable<T> ConsultaSql<T>(string sp, params object[] parametros) where T : class
        {
            return Database.SqlQuery<T>("exec " + sp, parametros);
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
            Database.Connection.Close();
            return resDt;
        }
    }
}