using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModelEMERMEDICA_AreaETL
{
    public class EME_CIUDAD_HOMOLOG
    {
        [Key]
        public short COD_CCOS { get; set; }

        public string CIUDAD { get; set; }
        public string CIUDAD_HOMOLOG { get; set; }
    }
}