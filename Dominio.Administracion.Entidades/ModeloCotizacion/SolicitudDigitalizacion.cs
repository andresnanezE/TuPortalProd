using System;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class SolicitudDigitalizacion
    {
        public int Id { get; set; }
        public int IdUsr { get; set; }
        public int NumeroFormulario { get; set; }
        public int CedulaAsesor { get; set; }
        public string NombreAsesor { get; set; }
        public string Director { get; set; }
        public int CedulaContratante { get; set; }
        public int NumeroAfiliados { get; set; }
        public string Link { get; set; }
        public string TipoArchivo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaCargue { get; set; }
        public DateTime FechaUltimoCargue { get; set; }
        public string ClaveTipoContrato { get; set; }
        public string TipoContrato { get; set; }
        public string ClaveMedioPago { get; set; }
        public string MedioPago { get; set; }
        public string NumeroContrato_Inclusion { get; set; }
        public int ArchivoId { get; set; }
        public string NombreOriginal { get; set; }
        public string RutaArchivo { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
    }

    public class SolicitudDigital
    {
        public int Id { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public int IdUsr { get; set; }
        public string NumeroFormulario { get; set; }
        public int CedulaAsesor { get; set; }
        public string NombreAsesor { get; set; }
        public string Director { get; set; }
        public int CedulaContratante { get; set; }
        public int NumeroAfiliados { get; set; }
        public string Link { get; set; }
        public string TipoArchivo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaCargue { get; set; }
        public DateTime FechaUltimoCargue { get; set; }
        public string ClaveTipoContrato { get; set; }
        public string TipoContrato { get; set; }
        public string ClaveMedioPago { get; set; }
        public string MedioPago { get; set; }
        public string NumeroContrato_Inclusion { get; set; }
        public string Observaciones { get; set; }
        public List<ArchivosSolicitud> Archivos { get; set; }
    }

    public class ArchivosSolicitud
    {
        public int Id { get; set; }
        public string NombreOriginal { get; set; }
        public string RutaArchivo { get; set; }
    }
}
