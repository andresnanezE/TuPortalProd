using System;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class ReclutamientoRepresentante
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public Int64 NumeroDocumento { get; set; }
        public string CiudadExpedicion { get; set; }
    }
}
