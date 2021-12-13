using System;

namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class GestionContratacion
    {
        public int NumeroDocumento { get; set; }
        public int IdProspecto { get; set; }
        public string Estado { get; set; }
        public bool File1 { get; set; }
        public bool File2 { get; set; }
        public bool File3 { get; set; }
        public bool File4 { get; set; }
        public bool File5 { get; set; }
        public bool File6 { get; set; }
        public bool File7 { get; set; }
        public bool InformacionPersonal { get; set; }
        public bool Sarlaft { get; set; }
        public bool ExperienciaComercial { get; set; }
        public bool Contrato { get; set; }
        public bool CertificacionTributaria { get; set; }
        public string Observaciones { get; set; }
    }
}