using System;

namespace Dominio.Administracion.Entidades.ReportesDto
{
    public class Reporte
    {
        public int id_cotizacion { get; set; }
        public string nombreEmpresa { get; set; }
        public string nombreDirector { get; set; }
        public string nombreAsesor { get; set; }
        public string NIT { get; set; }
        public int DV { get; set; }
        public Nullable<System.DateTime> fechaVencimiento { get; set; }
        public string estadoReserva { get; set; }
        public string productos { get; set; }
        public int valorCotizacion { get; set; }
        public string estadoCotizacion { get; set; }
        public string canal { get; set; }
        public string ciudad { get; set; }
    }
}