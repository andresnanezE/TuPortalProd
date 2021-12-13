using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModelKheiron
{
    public class EMB_CIUDAD
    {
        [Key]
        public string ID_CIUDAD { get; set; }

        public string ID_AREA { get; set; }
        public string CIUDAD { get; set; }
    }
}