using System;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class ReclutamientoInfo
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public Int64 NumeroDocumento { get; set; }
        public string CorreoElectronico { get; set; }
        public bool Recovery { get; set; }
        public string UrlRecovery { get; set; }
    }
}
