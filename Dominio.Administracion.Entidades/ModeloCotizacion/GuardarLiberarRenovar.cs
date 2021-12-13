using System.Collections.Generic;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class GuardarLiberarRenovar
    {
        public bool Aprobar { get; set; }
        public string NotaLiberarRenovar { get; set; }

        public IEnumerable<Cotizacion_Sedes> Sedes {get;set;}
    }
}