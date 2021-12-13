using System;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class EME_METAS_VENTAS
    {
        public int ID { get; set; }
        public int ANIO { get; set; }
        public int PERIODO { get; set; }
        public int SEMANA { get; set; }
        public int DIA { get; set; }
        public DateTime FECHA { get; set; }
        public bool HABIL { get; set; }
        public int CIUD_HOMOL { get; set; }
        public int META { get; set; }
        public string ACT_USUA { get; set; }
        public DateTime ACT_FECHA { get; set; }
    }
}