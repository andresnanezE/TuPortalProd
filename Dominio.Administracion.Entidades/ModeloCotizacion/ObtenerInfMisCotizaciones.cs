using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class ObtenerInfMisCotizaciones
    {
        public List<string> LstRoles { get; set; }
        public List<EMA_Cotizacion> LstMisCotizaciones { get; set; }
        public List<Usp_ObtenerCotizaciones> LstMisCotizacionesRol { get; set; }
        public List<string> LstProximasVencer { get; set; }
        public List<string> LstMsgProximasVencer { get; set; }
        public string Mensaje { get; set; }
        public bool Respuesta { get; set; }
    }
}
