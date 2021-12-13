using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloStone
{
    public class CC_ASESO
    {
        [Key]
        public decimal COD_ASES { get; set; }

        public decimal COD_DIRE { get; set; }
        public string EST_ASES { get; set; }
    }
}