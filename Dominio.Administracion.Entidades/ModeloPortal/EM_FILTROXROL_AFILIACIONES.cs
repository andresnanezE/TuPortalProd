using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class EM_FILTROXROL_AFILIACIONES
    {
        [Key]
        public int ID_FILTROXROL { get; set; }

        public int ID_ROL { get; set; }
        public int ID_FILTRO { get; set; }
        public string OPCION_DEFAULT { get; set; }
        public string MOSTRAR { get; set; }
    }
}