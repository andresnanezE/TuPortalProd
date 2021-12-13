using System;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class GestionSolicitud
    {
        public Int64 NumeroDocumento { get; set; }
        public int IdProspecto { get; set; }
        public bool File1 { get; set; }
        public bool File2 { get; set; }
        public bool File3 { get; set; }
        public bool File4 { get; set; }
        public bool File5 { get; set; }
        public bool File6 { get; set; }
        public bool File7 { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
    }
}
