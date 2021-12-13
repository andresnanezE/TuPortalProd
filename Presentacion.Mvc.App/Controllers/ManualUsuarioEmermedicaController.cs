using Presentacion.Mvc.App.Models;
using System.IO;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Controllers
{
    public class ManualUsuarioEmermedicaController : Controller
    {
        // GET: ManualUsuarioEmermedica
        public ActionResult Index()
        {
            var model = new ClausuladoContratoModel();
            model.Archivo = ConfiguracionesGlobales.ManualUsuario;
            return View(model);
        }

        public ActionResult DescargarArchivo(ClausuladoContratoModel model)
        {
            string fileName = Path.GetFileName(model.Archivo);
            return File(model.Archivo, "text/plain", fileName);
        }
    }
}