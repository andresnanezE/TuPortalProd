using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class HistoricoSolicitud
    {
        public decimal? IdHistorico { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public List<CausalesSolicitud> Causales { get; set; }
        public string Observaciones { get; set; }
        public string Analista { get; set; }
    }
}
