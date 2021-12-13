using Dominio.Administracion.Entidades.MapperDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Mvc.App.Models
{
    public class GanometroModel
    {
        public int Id { get; set; }

        [Display(Name = "Año")]
        public int Anio { get; set; }

        public int Periodo { get; set; }
        public int Semana { get; set; }
        public int Dia { get; set; }

        [Display(Name = "Fecha Venta")]
        public DateTime FechaVenta { get; set; }

        public DateTime FechaVent0a { get; set; }
        public bool EsHabil { get; set; }
        public int IdCiudadHomologada { get; set; }
        public int IdDirector { get; set; }

        [Display(Name = "Cantidad Ventas")]
        public int CantidadVentas { get; set; }

        public DateTime FechaIngreso { get; set; }

        //public System.Web.Mvc.WebViewPage<IEnumerable<GanometroModel>> Ventas { get; set; }
        public IEnumerable<VentasDirectorPeriodoDto> Ventas { get; set; }
    }
}