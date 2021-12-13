using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Productos
    {
        [Key]
        public int id_producto { get; set; }

        public string Abreviatura { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Grupo { get; set; }
        public int COD_SERV { get; set; }
    }
}