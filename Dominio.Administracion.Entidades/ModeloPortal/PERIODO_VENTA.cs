using System;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class PERIODO_VENTA
    {
        public int ANIO { get; set; }
        public int PERIODO { get; set; }
        public int SEMANA { get; set; }
        public int DIA { get; set; }
        public DateTime FECHA { get; set; }
        public bool ESHABIL { get; set; }
    }
}