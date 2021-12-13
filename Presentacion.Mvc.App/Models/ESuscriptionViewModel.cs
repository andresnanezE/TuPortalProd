using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Models
{
    public class ESuscriptionViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Fecha Inicio")]
        [DataType(DataType.Date)] // if HTML5 is used you can apply this attribute
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FechaInicial { get; set; }
        [Required]
        [DisplayName("Fecha Final")]
        [DataType(DataType.Date)] // if HTML5 is used you can apply this attribute
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy")]
        public DateTime FechaFinal { get; set; }
        public string Cedula { get; set; }
        public int IdEvento { get; set; }
        public List<SelectListItem> ListaEvento
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem(){Text = "Todos",Value = "0"},
                    new SelectListItem(){Text = "Aprobados",Value = "1"},
                    new SelectListItem(){Text = "Rechazados",Value = "2"}
                };
            }
        }
    }
}