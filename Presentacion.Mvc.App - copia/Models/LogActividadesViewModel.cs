using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Models
{
    public class LogActividadesViewModel
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string IdTipo { get; set; }
        public IEnumerable<SelectListItem> Tipos { get; set; }
        public int IdRol { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}