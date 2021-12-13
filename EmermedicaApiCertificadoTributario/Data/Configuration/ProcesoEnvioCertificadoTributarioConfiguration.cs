using ApiCertificadoTributario.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ApiCertificadoTributario.Data.Configuration
{
    public class ProcesoEnvioCertificadoTributarioConfiguration : EntityTypeConfiguration<ProcesoEnvioCertificadoTributario>
    {
        public ProcesoEnvioCertificadoTributarioConfiguration()
        {
            ToTable("ProcesoEnvioCertificadoTributario");
            Property(p => p.Id);
            Property(p => p.Año);
            Property(p => p.TipoContrato);
            Property(p => p.TodosRmtCont);
            Property(p => p.FechaMaximaEnvio);
            Property(p => p.FechaRegistro);
            Property(p => p.Estado);
            Property(p => p.Mensaje);

            HasKey(p => p.Id);
        }
    }
}