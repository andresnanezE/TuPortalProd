using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Presentacion.Mvc.App.Models
{
    public class GeoZonificadorModel
    {
        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        [DisplayName("Dirección Normalizada")]
        public string DireccionNormalizada { get; set; }

        [DisplayName("Zona Operación")]
        public string ZonaOperacion { get; set; }

        [DisplayName("Barrio")]
        public string Barrio { get; set; }

        [DisplayName("Estado")]
        public string Estado { get; set; }

        [DisplayName("Ciudad")]
        public string Ciudad { get; set; }

        [DisplayName("Longitud")]
        public double Longitud { get; set; }

        [DisplayName("Latitud")]
        public double Latitud { get; set; }

        public List<ListItem> Ciudades { get; set; }
    }
}