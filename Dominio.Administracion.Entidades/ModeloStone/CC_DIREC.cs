using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloStone
{
    public class CC_DIREC
    {
        [Key]
        public decimal COD_DIRE { get; set; }

        public int COD_ESCA { get; set; }
        public short COD_CCOS { get; set; }
        public string EST_DIRE { get; set; }
        public decimal COD_GERE { get; set; }
    }
}