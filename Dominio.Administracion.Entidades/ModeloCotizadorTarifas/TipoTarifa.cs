using System.Collections.Generic;

namespace Dominio.Administracion.Entidades
{
    public class TipoTarifa
    {
        public string CODIGO_TARIFA { get; set; }
        public string TIPO_TARIFA { get; set; }
        public IEnumerable<Campana> CAMPANAS { get; set; }
    }
}