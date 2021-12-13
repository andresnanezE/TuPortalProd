using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class RegistroReclutamiento
    {
        public int IdTipoIdentificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public Int64 NumeroCedula { get; set; }
        public string Correo { get; set; }
        public Int64 Telefono { get; set; }
        public int IdReclutador { get; set; }
        public int IdDirector { get; set; }
        public bool TerminosCondiciones { get; set; }
        public string Ip { get; set; }
        public string TipoIdentificacion { get; set; }
        public string CiudadVinculacion { get; set; }
        public string Gestionado { get; set; }
    }
}
