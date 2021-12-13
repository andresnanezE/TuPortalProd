using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Newtonsoft.Json;

namespace Presentacion.Mvc.App.Models
{

    public class item
    {
        public string display_id { get; set; }
        public string subject { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string status_name { get; set; }
        public custom_field custom_field { get; set; }
        public string id { get; set; }
        public string mensaje { get; set; }
        public string url_attach { get; set; }
        public string nombre_archivo { get; set; }
        public string body { get; set; }
        public string attachment_url { get; set; }
        public string content_file_name { get; set; }


    }
    public class Solicitud
    {
        public item item { get; set; }
    }

    public class custom_field
    {
        public string ciudad_110623 { get; set; }
        public string area_110623 { get; set; }
        public string trequerimiento_110623 { get; set; }
    }
}