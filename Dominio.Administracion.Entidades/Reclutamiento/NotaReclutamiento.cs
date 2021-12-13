using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class NotaReclutamiento
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public Int64 Telefono { get; set; }
        public string Ciudad { get; set; }
        public string EstadoProspecto { get; set; }
        public string Proceso { get; set; }
        public string Nota { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
