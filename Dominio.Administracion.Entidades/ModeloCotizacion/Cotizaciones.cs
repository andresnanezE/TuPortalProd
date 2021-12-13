using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Cotizaciones
    {
        [Key]
        public int id_cotizacion { get; set; }

        public decimal NIT { get; set; }
        public Nullable<int> DV { get; set; }
        public string nombreEmpresa { get; set; }
        public string contacto { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string cargo { get; set; }
        public Nullable<System.DateTime> fechaVisita { get; set; }
        public string motivoVisita { get; set; }
        public Nullable<bool> bloqueo { get; set; }
        public Nullable<System.DateTime> fechaExpiracion { get; set; }
        public Nullable<int> numeroRenovaciones { get; set; }
        public string estado { get; set; }
        public string correoElectronico { get; set; }
        public string ciudad { get; set; }
        public string canal { get; set; }

        public string SectorEconomico { get; set; }
        public string NumeroExpuestos { get; set; }
        public string NumeroCapacitaciones { get; set; }
        public string NumeroEventos { get; set; }
        public string NumeroSedes { get; set; }
        public string TipoAP { get; set; }
        public string ArchivoCotizacion { get; set; }
        public Nullable<bool> VeracidadInformacion { get; set; }
        public string EstadoPricing { get; set; }
        public string ObservacionesPricing { get; set; }
        public string ObservacionReserva { get; set; }
        public int TelefonoExt { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string EstadoCotizacion { get; set; }

        public Nullable<decimal> valorReconsideracion { get; set; }
        public Nullable<int> NumeroReconsideraciones { get; set; }
    }
}