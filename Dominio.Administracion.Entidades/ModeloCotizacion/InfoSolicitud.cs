
using System.Collections.Generic;
namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class InfoSolicitud
    {
        public int IdUsuario { get; set; }
        public int Id { get; set; }
        public int NumeroFormulario { get; set; }
        public string Observaciones { get; set; }
        public int CedulaAsesor { get; set; }
        public int CedulaContratante { get; set; }
        public int NumeroAfiliados { get; set; }
        public string TipoArchivo { get; set; }
        public string Link { get; set; }
        public bool Radicar { get; set; }
        public bool Rechazar { get; set; }
        public int EstadoId { get; set; }
        public List<InfoFile> FileInfo { get; set; }
        public List<InfoFile> FileInfoEliminar { get; set; }
        public List<CausalDigitalizacion> Causales { get; set; }
        public string TipoContrato { get; set; }
        public string NumeroContrato { get; set; }
        public string MedioPago { get; set; }
    }
}
