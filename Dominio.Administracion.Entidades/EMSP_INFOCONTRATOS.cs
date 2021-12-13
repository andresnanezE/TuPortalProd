using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades
{
    public class EMSP_INFOCONTRATOS
    {
        public string idContrato { get; set; }
        public IEnumerable<String> productos { get; set; }
        public DateTime fechaExpiracion { get; set; }
        public string nombreAsesor { get; set; }
        public string nombreDirector { get; set; }
        public string ciudadAsesor { get; set; }
        public string canalAsesor { get; set; }
    }
}