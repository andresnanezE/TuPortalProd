using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public partial class DYK_DESTACADO
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DESTACADOID { get; set; }

        public string IMAGEN { get; set; }
        public string URL { get; set; }
        public bool ABRIRENSITIO { get; set; }
    }
}