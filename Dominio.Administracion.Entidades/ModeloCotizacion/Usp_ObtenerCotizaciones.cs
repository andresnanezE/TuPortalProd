using System;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Usp_ObtenerCotizaciones
    {
        public int IdCotizacion { get; set; }
        public string NombreDirector { get; set; }
        public string DocumentoDirector { get; set; }
        public string NombreAsesor { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string Nit { get; set; }
        public string NombreEmpresa { get; set; }
        public string EstadoCotizacion { get; set; }
        public string Producto { get; set; }
        public bool ValidarVencida { get; set; }
        public string Alerta { get; set; }
        public int NumeroRenovaciones { get; set; }
        public int? NumeroReconsideraciones { get; set; }
    }
}