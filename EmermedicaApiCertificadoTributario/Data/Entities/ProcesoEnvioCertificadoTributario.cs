using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiCertificadoTributario.Data.Entities
{
    public class ProcesoEnvioCertificadoTributario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Año { get; set; }
        public int TipoContrato { get; set; }
        public bool TodosRmtCont { get; set; }
        public DateTime FechaMaximaEnvio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
    }
}