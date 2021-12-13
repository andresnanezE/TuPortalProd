using System.Web;

namespace Presentacion.Mvc.App.Models
{
    public class UploadFiles
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string FileData { get; set; }
        public string Ruta { get; set; }
        public string KeyConfig { get; set; }
        public string Extension { get; set; }
        public string labels { get; set; }
        public bool BorrarActuales { get; set; }
        private HttpPostedFileBase Archivo { get; set; }
    }
}