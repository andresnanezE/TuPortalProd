namespace Dominio.Administracion.Entidades.Reclutamiento
{
    public class ReclutamientoProceso
    {
        public int Id { get; set; }
        public bool InformacionPersonal { get; set; }
        public bool Sarlaft { get; set; }
        public bool ExperienciaComercial { get; set; }
        public bool Contrato { get; set; }
        public bool CertificacionTributaria { get; set; }
        public int EstadoArchivos { get; set; }
    }
}
