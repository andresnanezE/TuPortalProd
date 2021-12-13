using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Presentacion.Mvc.App.Controllers
{
    /// <summary>
    /// Esto de ir en el modelo
    /// </summary>
    public class SolicitudesInternasNotas
    {

        public string IdNota { get; set; }
        public string Nota { get; set; }
        public string url_archivo { get; set; }
        public string IdTicket { get; set; }
        public DateTime Fecha { get; set; }
        public byte[] archivo { get; set; }
        public string nombre_archivo { get; set; }
        public IEnumerable<HttpPostedFileBase> Attach { get; set; }
        public bool EsPrivada { get; set; }
        public string Mensaje { get; set; }
    }
}