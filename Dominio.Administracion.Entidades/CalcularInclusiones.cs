using System;

namespace Dominio.Administracion.Entidades
{
    /// <summary>
    /// JohnNelsonRodriguex
    /// </summary>
    public class CalcularInclusiones
    {
        //Ingreso primera cuota SI
        public decimal vProrrateo { get; set; }

        public decimal tProrrateo { get; set; }

        //Ingreso primera cuota NO
        public decimal vProximaCuota { get; set; }

        public decimal tProximaCuota { get; set; }

        public decimal vRecurrente { get; set; }
        public decimal tRecurrente { get; set; }

        public decimal valIvaRecurrente { get; set; }
        public decimal @valIvaProrrateo { get; set; }
        public decimal @valIvaProxCuota { get; set; }
        public DateTime fInclusion { get; set; }
        public DateTime fCorte { get; set; }
        public int nPersonas { get; set; }

        public int diasProrrateo { get; set; }

        public decimal tarifas { get; set; }
    }
}