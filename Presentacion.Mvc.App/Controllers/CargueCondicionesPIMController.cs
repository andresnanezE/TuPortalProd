using Presentacion.Mvc.App.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Controllers
{
    public class CargueCondicionesPIMController : Controller
    {
        private readonly string MOSTRARTRACE = $"{ConfiguracionesGlobales.MostrarTrace}";

        // GET: CargueCondicionesPIM
        public ActionResult Index()
        {
            List<UploadFiles> lstUploadFiles = new List<UploadFiles>
            {
                 new UploadFiles() {
                Extension = ".pdf",
                KeyConfig = "PathCondicionesCorretaje",
                Nombre = string.Empty,
                FileData = string.Empty,
                labels = "PIM Condiciones corretaje"
            },

                 new UploadFiles() {
                Extension = ".pdf",
                KeyConfig = "PathCondicionesDirecto",
                Nombre = string.Empty,
                FileData = string.Empty,
                labels = "PIM Condiciones Directo"
            },
        };

            return View(lstUploadFiles);
        }
    }
}