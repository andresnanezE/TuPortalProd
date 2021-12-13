using System;

namespace Datos.Administracion.Repositorios
{
    public class CTSP_CONSULTA_TARIFAS_BASE
    {
        public string TIPO_TARIFA { get; set; }
        public string CAMPANA_TARIFA { get; set; }
        public string CIUDAD { get; set; }
        public int RANGO_INICIAL_PERSONA { get; set; }
        public int RANGO_FINAL_PERSONA { get; set; }
        public string MODALIDAD_PAGO { get; set; }
        public string FORMA_PAGO { get; set; }
        public decimal VALOR_TARIFA { get; set; }
        public Nullable<decimal> VALOR_IVA_TARIFA { get; set; }
        public System.DateTime FECHA_VENCIMIENTO_TARIFA { get; set; }
        public Nullable<int> VALOR_AHORRO { get; set; }
        public Nullable<int> VALOR_COMISION { get; set; }
        public Nullable<decimal> PORCENTAJE_AHORRO { get; set; }
        public Nullable<int> VALOR_CUOTA_PERIODICA { get; set; }
        public Nullable<int> VALOR_AHORRO_LIQUIDACION { get; set; }
        public Nullable<int> TOTAL_TARIFA_MES { get; set; }
        public int ID_ESTADO { get; set; }
    }
}