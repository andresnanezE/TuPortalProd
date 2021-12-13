using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ReportesDto
{
    public class GlobalReporte
    {
        [Key]
        public int IdCotizacion { get; set; }
        public string NombreDirector { get; set; }
        public string DocumentoDirector { get; set; }
        public string NombreAsesor { get; set; }
        public decimal NIT { get; set; }
        public int DV { get; set; }
        public string NombreEmpresa { get; set; }
        public DateTime FechaReserva { get; set; }
        public string NombreContacto { get; set; }
        public string Cargo { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Ciudad { get; set; }
        public string Canal { get; set; }
        public string EstadoReserva { get; set; }
        public string NombreProducto { get; set; }
        public Int64 PersonalPermanente { get; set; }
        public Int64 PromedioVisitantes { get; set; }
        public string SedesCotizadas { get; set; }
        public decimal ValorXSede { get; set; }
        public decimal ValorCotizacionFinal { get; set; }
        public string EstadoCotizacion { get; set; }
        public string SectorEconomico { get; set; }
        public int NumeroReconsideraciones { get; set; }
        public int NumeroRenovaciones { get; set; }
        public decimal ValorUltimaReconsideracion { get; set; }
        public DateTime FechaUltimaReconsideracion { get; set; }

    }
}
