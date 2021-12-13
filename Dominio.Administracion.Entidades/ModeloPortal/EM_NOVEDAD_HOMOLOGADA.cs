using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class EM_NOVEDAD_HOMOLOGADA
    {
        [Key]
        public int ID_NOVE_HOMOLOG { get; set; }

        public int COD_NOVE { get; set; }
        public string DESCRIPCION { get; set; }
        public string APLI_COMISION { get; set; }
        public string NOMB_HOMOLOG { get; set; }
    }
}