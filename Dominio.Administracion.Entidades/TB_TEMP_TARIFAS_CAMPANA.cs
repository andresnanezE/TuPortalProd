using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Administracion.Entidades
{
    public class TB_TEMP_TARIFAS_CAMPANA
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
        public int ID_ESTADO { get; set; }
    }
}
