using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
     public class EMB_Canal
    {
        [Key]
        public Int16 Id_Canal { get; set; }
        public string Canal { get; set; }
    }
}
