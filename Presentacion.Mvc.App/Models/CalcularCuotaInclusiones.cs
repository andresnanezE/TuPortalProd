using System;

namespace Presentacion.Mvc.App.Models
{
    public class CalcularCuotaInclusiones
    {
        //Ingreso primera cuota SI
        public decimal vProrrateo { get; set; }

        public decimal vIvaProrrateo { get; set; }

        //Ingreso primera cuota NO
        public decimal vCuota { get; set; }

        public decimal vIvaCuota { get; set; }

        public DateTime fInclusion { get; set; }
        public int nPersonas { get; set; }
        public decimal tProrrateo { get; set; }
        public decimal vRecurrente { get; set; }
        public decimal vIvaRecurrente { get; set; }
        public decimal tRecurrente { get; set; }
    }
}