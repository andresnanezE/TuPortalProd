using ApiCertificadoTributario.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ApiCertificadoTributario.Data.Configuration
{
    public class EnvioCertificadoTributarioConfiguration:EntityTypeConfiguration<EnvioCertificadoTributario>
    {
        public EnvioCertificadoTributarioConfiguration()
        {
            ToTable("EnvioCertificadoTributario");
            Property(e => e.Id);
            Property(e => e.ProcesoId);
            Property(e => e.NumeroDocumento);
            Property(e => e.RmtCont);
            Property(e => e.Email);
            Property(e => e.EstadoEnvio);
            Property(e => e.EstadoError);
            Property(e => e.Mensaje);
            Property(e => e.TipoContrato);
            Property(e => e.NombreContratante);
            Property(e => e.FechaEnvio);
            Property(e => e.DireccionFisica);

            HasKey(e => e.Id);
        }
    }
}