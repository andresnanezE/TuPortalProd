using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class PoliticaNITS
    {
        [Key]
        public int id { get; set; }

        public string descripcion { get; set; }
        public int numero_dias_validez { get; set; }
        public int numero_maximo_renovacion { get; set; }
    }
}