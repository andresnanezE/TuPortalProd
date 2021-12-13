using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ApiCertificadoTributario.Data.Entities
{
    public partial class EnvioCertificadoTributario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProcesoId { get; set; }
        public string NumeroDocumento { get; set; }
        public string RmtCont { get; set; }
        public string Email { get; set; }
        public bool EstadoEnvio { get; set; }
        public bool EstadoError { get; set; }
        public string Mensaje { get; set; }
        public string TipoContrato { get; set; }
        public string NombreContratante { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string DireccionFisica { get; set; }
    }
}
