// ----------------------------------------------------------------------------------------------
// <copyright file="LoginModel.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    public class LoginModel
    {
        #region Instance Properties

        [Required(ErrorMessage = "Por favor ingrese su contraseña.")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "Por favor ingrese su nombre de usuario.")]
        public string Usuario { get; set; }

        public string Mensaje { get; set; }

        public int banderaCaptcha { get; set; }
        public string tokenGog { get; set; }

        #endregion Instance Properties
    }
}