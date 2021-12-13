using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades
{
    public class GenerarReferenciaDto
    {
        public string Cedula { get; set; }
        public bool isInclusion { get; set; }
        public string NumeroContratoInclusion { get; set; }
        public string CedulaContratante { get; set; }
        public string CedulaBeneficiario { get; set; }

    }
}
