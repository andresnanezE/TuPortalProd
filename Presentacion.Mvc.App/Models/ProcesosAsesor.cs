using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Presentacion.Mvc.App.Models
{
    public class ProcesosAsesor
    {
        [DisplayName("Proceso")]
        public int ID_PROC { get; set; }

        public int CC_ASESOR { get; set; }

        [DisplayName("Año")]
        public int ANIO { get; set; }

        [DisplayName("Mes")]
        public int MES { get; set; }

        public DateTime FECHA_INI { get; set; }
        public DateTime FECHA_FIN { get; set; }

        public IEnumerable<String> FECHA_INI_FIN { get; set; }

        public String NOMBRE_REPORTE { get; set; }
    }
}