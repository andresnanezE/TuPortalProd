using System.Collections.Generic;

namespace Presentacion.Mvc.App.Models
{
    public class MessageModel
    {
        public string Titulo { get; set; }
        public string Cuerpo { get; set; }
        public string aceptar { get; set; }

        public List<string> ValorMostrar { get; set; }
    }
}