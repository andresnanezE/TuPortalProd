using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

//
// j0HNn3LS0N r0DRIGU3Z
// 2016-10-25
namespace Presentacion.Mvc.App.Models
{
    public class SoportesSeguridadSocial
    {
        [DisplayName("Observación")]
        [DataType(DataType.MultilineText)]
        public string OBSERVACION { get; set; }

        [DisplayName("Motivo")]
        public int ID_MOTIVO { get; set; }

        public string NOMBRE_MOTIVO { get; set; }

        public IEnumerable<HttpPostedFileBase> ARCHIVOS { get; set; }

        public int CC_ASESOR { get; set; }

        public string ARCHIVOSTXT { get; set; }

        public string EMAIL { get; set; }

        public string MensajeError { get; set; }
    }
}