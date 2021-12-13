using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class GuardarCotizacion
    {
        public int Id_Cotizacion { get; set; }
        public int Id_Sector { get; set; }
        public int Id_TipoAP { get; set; }
        public string SectorEconomico { get; set; }
        public string TipoAP { get; set; }
        public decimal ValorTarifa { get; set; }
        public List<Cotizacion_Sedes> ListadoSedes { get; set; }
    }
}