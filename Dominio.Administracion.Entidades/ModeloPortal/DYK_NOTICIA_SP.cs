using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public partial class DYK_NOTICIA_SP
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid NOTICIAID { get; set; }

        public string TITULO { get; set; }
        public string TITULOQS { get; set; }
        public string DESCRIPCION { get; set; }
        public string IMAGEN { get; set; }
        public DateTime FECHA { get; set; }
        public bool ACTIVO { get; set; }
    }
}