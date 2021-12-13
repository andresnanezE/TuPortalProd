using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloPortal
{
    public class EMB_TipoLog
    {
        [Key]
        public int idTipoLog { get; set; }

        public string tipo { get; set; }
        public string descripcion { get; set; }
    }
}