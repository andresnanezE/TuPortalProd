using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    public class ComisionesAsesorMes
    {
        [DisplayName("Proceso")]
        [Required(ErrorMessage = "El numero de proceso es requerido.")]
        public int ID_PROC { get; set; }

        [DisplayName("Año")]
        public int ANIO { get; set; }

        [DisplayName("Mes")]
        public int MES { get; set; }

        [Required(ErrorMessage = "El nombre del reporte es requerido.")]
        public String NOMBRE_REPORTE { get; set; }

        public List<string> ANIOS { get; set; }
    }
}