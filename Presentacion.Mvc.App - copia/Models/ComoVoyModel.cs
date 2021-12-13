using Aplicacion.Administracion.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
// 
// j0HNn3LS0N r0DRIGU3Z
// 2016-10-25
namespace Presentacion.Mvc.App.Models
{
    public class ComoVoyModel
    {
        public decimal CC_ASESOR { get; set; }
        public int VENTAS { get; set; }
        public int META_VENTAS { get; set; }

        public int CANCELADOS { get; set; }
        
        public int REACTIVADOS { get; set; }


        public string Mensaje { get; set; }

        public string MensajeError { get; set; }

        public string AFILIACIONES { get; set; }


    }
}