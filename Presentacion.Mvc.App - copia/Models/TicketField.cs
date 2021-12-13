using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Presentacion.Mvc.App.Models
{
    public class FiledsTicket
    {
        public string Label;
        public string Id;
        public List<List<string>> Choices;
        public string Status;
    }
    public class Ticket
    {
        public FiledsTicket ticket_field;
    }

    //public class TicketEmermedica
    //{
    //    public string Ciudad { get; set; }
    //    public string TipoRequerimiento { get; set; }
    //    public string Asunto { get; set; }
    //    public string Area { get; set; }
    //    public IEnumerable<HttpPostedFileBase> Adjunto { get; set; }
    //    public string Descripcion { get; set; }


    //}
   
}