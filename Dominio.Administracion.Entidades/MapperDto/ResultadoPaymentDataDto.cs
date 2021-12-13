using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades.MapperDto
{
    public class ResultadoPaymentDataDto
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public object Resultado { get; set; }

    }
}
