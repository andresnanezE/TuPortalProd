using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class EME_REGISTRO_VENTAS
    {
        [Key]
        public int ID { get; set; }

        public int ANIO { get; set; }
        public int PERIODO { get; set; }
        public int SEMANA { get; set; }
        public int DIA { get; set; }
        public DateTime FECHA { get; set; }
        public bool HABIL { get; set; }
        public int CIUD_HOMOL { get; set; }
        public int ID_DIRECTOR { get; set; }
        public int CANT_VENTAS { get; set; }
        public DateTime ACT_FECHA { get; set; }
    }
}