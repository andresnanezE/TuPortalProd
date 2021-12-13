using System;

namespace Dominio.Administracion.Entidades
{
    public class TarifaBase
    {
        public string TIPO_TARIFA { get; set; }
        public string CAMPANA_TARIFA { get; set; }
        public string CIUDAD { get; set; }
        public int RANGO_INICIAL_PERSONA { get; set; }
        public int RANGO_FINAL_PERSONA { get; set; }
        public string MODALIDAD_PAGO { get; set; }
        public string FORMA_PAGO { get; set; }
        public decimal VALOR_TARIFA { get; set; }
        public decimal VALOR_IVA_TARIFA { get; set; }
        public DateTime FECHA_VENCIMIENTO_TARIFA { get; set; }
        public double VALOR_AHORRO { get; set; }
        public double VALOR_comision { get; set; }
        public double PORCENTAJE_AHORRO { get; set; }
        public int VALOR_CUOTA_PERIODICA { get; set; }
        public int VALOR_AHORRO_LIQUIDACION { get; set; }
        public int TOTAL_TARIFA_MES { get; set; }
        public int ID_ESTADO { get; set; }
    }
}