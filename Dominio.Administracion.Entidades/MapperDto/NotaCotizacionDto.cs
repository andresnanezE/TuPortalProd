
namespace Dominio.Administracion.Entidades.MapperDto
{
    public class NotaCotizacionDto
    {
        public int Id { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Rol { get; set; }
        public int OrderId { get; set; }
        public string NombreUsuario { get; set; }
        public string Nota { get; set; }
    }
}
