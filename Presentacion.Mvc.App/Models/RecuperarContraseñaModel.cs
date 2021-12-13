using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    public class RecuperarContraseñaModel
    {
        [Display(Name = "Usuario")]
        //[Required]
        public string Usuario { get; set; }

        [Display(Name = "Cédula")]
        //[Required]
        public string documento { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nueva clave")]
        public string Clave { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repite clave")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmacionClave { get; set; }

        [Display(Name = "Mensaje")]
        public string Mensaje { get; set; }

        public string tokenGog { get; set; }
    }
}