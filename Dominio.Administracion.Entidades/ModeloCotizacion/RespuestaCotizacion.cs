using System;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class RespuestaCotizacion
    {
        public bool RespCotizacion { get; set; }
        public bool RespCotizacionSedes { get; set; }
        public bool RespCotizacionDivisor { get; set; }
        public String MensajeCotizacion { get; set; }
        public String MensajeCotizacionSedes { get; set; }
        public String MensajeCotizacionDivisor { get; set; }
    }
}