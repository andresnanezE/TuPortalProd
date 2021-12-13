using System;
using System.Collections.Generic;

namespace Presentacion.Mvc.App.Models
{
    public class ListaSolicitudesViewModel
    {
        public List<item> Solicitudes { get; set; }
        public String Mensaje { get; set; }
    }
}