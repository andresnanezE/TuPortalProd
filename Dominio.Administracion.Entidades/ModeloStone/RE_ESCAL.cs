using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloStone
{
    public class RE_ESCAL
    {
        [Key]
        public int COD_ESCA { get; set; }

        public string NOM_ESCA { get; set; }
    }
}