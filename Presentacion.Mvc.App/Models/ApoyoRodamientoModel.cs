using Dominio.Administracion.Entidades.MapperDto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    public class ApoyoRodamientoModel
    {
        public IEnumerable<AfiliacionesPeriodoDto> ListaPeriodos { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un periodo inicial")]
        public string PeriodoIni { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un periodo final")]
        public string PeriodoFin { get; set; }

        public string Definitivo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar Si o No.")]
        public string Mensaje { get; set; }

        public string MensajeError { get; set; }
    }
}