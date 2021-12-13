using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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