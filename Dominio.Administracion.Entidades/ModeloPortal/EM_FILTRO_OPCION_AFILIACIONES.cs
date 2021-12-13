using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class EM_FILTRO_OPCION_AFILIACIONES
    {
        [Key]
        public int ID_OPCION { get; set; }

        public int ID_FILTRO { get; set; }
        public string DESCRIPCION { get; set; }
        public string VALOR { get; set; }
        public string ACTIVO { get; set; }
    }
}