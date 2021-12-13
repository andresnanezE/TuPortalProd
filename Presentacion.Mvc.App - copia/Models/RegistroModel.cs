using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    public class RegistroModel
    {
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el documento.")]
        public int Documento { get; set; }

        [Required(ErrorMessage = "Por favor ingrese un nombre de usuario")]
        public string Usuario { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Por favor ingrese el correo.")]
        [EmailAddress(ErrorMessage = "Por favor ingrese un correo valido.")]
        public string Correo { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirma tu correo")]
        [Required(ErrorMessage = "Por favor ingrese el correo.")]
        [Compare("Correo", ErrorMessage = "Los correos no coinciden.")]
        [EmailAddress(ErrorMessage = "Por favor ingrese un correo valido.")]
        public string ConfirmacionCorreo { get; set; }

        [DataType(DataType.Password)]
        //[RegularExpression(@"^ ((?=.*[a - z])(?=.*[A - Z])(?=.*\d)).+$", ErrorMessage = "El password debe incluir una mayuscula, una minuscula y un numero.")]
        [Display(Name = "Nueva clave")]
        [Required(ErrorMessage = "Por favor ingrese la contrasena")]
        public string Clave { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repite clave")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmacionClave { get; set; }

        public DateTime FechaModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaUltimaSesion { get; set; }
        public string Mensaje { get; set; }
        public string TipoDocumento { get; set; }
        public string NombreUsuario { get; set; }
        public int Rol { get; set; }
    }
}