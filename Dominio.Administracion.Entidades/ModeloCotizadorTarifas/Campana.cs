using System;

namespace Dominio.Administracion.Entidades
{
    public class Campana
    {
        public string CODIGO_TARIFA { get; set; }
        public string RUTA_IMAGEN { get; set; }
        public string CAMPANA_TARIFA { get; set; }
        public string CARACTERIZACION { get; set; }

        public DateTime FECHA_VENCIMIENTO_TARIFA { get; set; }
    }
}