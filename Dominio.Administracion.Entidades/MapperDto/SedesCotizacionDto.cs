using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades.MapperDto
{
  public class SedesCotizacionDto
    {
        public int Id { get; set; }
        public int Id_Cotizacion { get; set; }
        public int Id_Ciudad { get; set; }
        public int Id_TipoRiesgo { get; set; }
        public string NombreSede { get; set; }
        public int NoPersonalPermanente { get; set; }
        public int NoPersonalVisitantes { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }        
        public decimal Valor { get; set; }
        public decimal? ValorReconsideracion { get; set; }
        public string NombreCiudad { get; set; } = null;
        public string NombreTipoRiesgo { get; set; } = null;
        public string SectorEconomico { get; set; } = null;
        public string TipoAP { get; set; } = null;

    }
}
