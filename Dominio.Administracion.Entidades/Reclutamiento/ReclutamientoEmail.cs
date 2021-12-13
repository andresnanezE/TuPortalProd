using System;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class ReclutamientoEmail
    {
        public string Sujeto { get; set; }

        public string Correo { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public Int64 NumeroDocumento { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        //public string Estado1 { get; set; }
        //public string Estado2 { get; set; }
        //public string Estado3 { get; set; }
        public string Observaciones { get; set; }

        //Lista Restrictiva
        public string CorreoProspecto { get; set; }
        public Int64 Telefono { get; set; }
        public string TipoIdentificacion { get; set; }
        public string CiudadVinculacion { get; set; }
        public string Gestionado { get; set; }
    }
}
