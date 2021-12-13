using System;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class ReclutamientoDirector
    {
        public int id_usr { get; set; }
        public string log_usr { get; set; }
        public string nom_usr { get; set; }
        public string tip_doc { get; set; }
        public string num_doc { get; set; }
        public string correo { get; set; }
        public int id_rol { get; set; }
        public string nom_rol { get; set; }
    }
}
