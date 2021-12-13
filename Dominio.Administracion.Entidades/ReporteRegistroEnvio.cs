using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades
{
    public class ReporteRegistroEnvio
    {
        public string NumeroDocumento { get; set; }
        public string TipoContrato { get; set; }
        public string NombreContratante { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string EnvioEmail { get; set; }
        public string Email { get; set; }
        public string DireccionFisica { get; set; }
        public string ErrorEnvio { get; set; }
    }
}
