using System;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    public class LogSesionEnVezDe
    {
        [Display(Name = "Fecha Inicial")]
        [Required(ErrorMessage = "El campo es obligatorio")]
        public DateTime FechaInicial { get; set; }

        [Display(Name = "Fecha Final")]
        [Required(ErrorMessage = "El campo es obligatorio")]
        public DateTime FechaFinal { get; set; }
    }
}