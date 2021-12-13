using Dominio.Administracion.Entidades.ModeloPortal;
using System.Collections.Generic;

namespace Dominio.Administracion.Entidades
{
    public class AfiliacionesFiltro
    {
        public DatosGeneralesFiltroAfiliaciones DatosFiltro { get; set; }
        public IEnumerable<EM_FILTRO_OPCION_AFILIACIONES> ListaOpciones { get; set; }

        public string FiltroDefault { get; set; }
        public string Mostar { get; set; }
    }
}