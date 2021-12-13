using System;

namespace Dominio.Administracion.Entidades.ReportesDto
{
    public class FiltrosReporte
    {
        public string DescripcionRol { get; set; }
        public string ProductosId { get; set; }
        public string EstadosId { get; set; }
        public string EstadosCotizacionId { get; set; }
        public string CiudadesId { get; set; }
        public string CanalesId { get; set; }
        public string SectoresId { get; set; }
        public string DirectoresId { get; set; }
        public string AsesoresId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

    }
}
