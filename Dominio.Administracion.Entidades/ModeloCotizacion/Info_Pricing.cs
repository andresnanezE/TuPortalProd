using System.ComponentModel.DataAnnotations;

namespace Dominio.Administracion.Entidades.ModeloCotizacion
{
    public class Info_Pricing
    {
        [Key]
        public int Id_funcionario { get; set; }

        public string Correo { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
    }
}