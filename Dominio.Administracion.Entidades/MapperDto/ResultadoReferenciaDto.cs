using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class ResultadoReferenciaDto
    {
        public bool IsInclusion { get; set; }
        public bool ContratoValido { get; set; }
        public bool Exitoso { get; set; }
        public string ReferenciaPago { get; set; }
        public string Mensaje { get; set; }
    }
}
