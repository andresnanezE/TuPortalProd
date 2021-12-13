using System;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class GestionCapacitacion
    {
        public int NumeroDocumento { get; set; }
        public int IdProspecto { get; set; }
        public string Estado { get; set; }
        public int CapacitadorId{ get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Observaciones { get; set; }
    }
}