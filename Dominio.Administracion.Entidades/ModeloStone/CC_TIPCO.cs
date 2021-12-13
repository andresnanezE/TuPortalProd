using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloStone
{
    public class CC_TIPCO
    {
        [Key]
        public int COD_TIPC { get; set; }

        public string NOM_TIPC { get; set; }
    }
}