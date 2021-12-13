using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public partial class DYK_BANNER
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid BANNERID { get; set; }

        public string IMAGEN { get; set; }
        public string URL { get; set; }
        public int POSICION { get; set; }
    }
}