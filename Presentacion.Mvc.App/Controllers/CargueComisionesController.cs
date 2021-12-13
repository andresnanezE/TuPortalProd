using Presentacion.Mvc.App.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Controllers
{
    public class CargueComisionesController : Controller
    {
        // GET: CargueComisiones
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase f)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    string fileName = Request.Files[0].FileName;
                    string path = Path.Combine(ConfiguracionesGlobales.PathArchivoPlanoComisionesAsesorColpatria, fileName);
                    var inputStream = Request.Files[0];

                    inputStream.SaveAs(path);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch (Exception exception)
            {
                ViewBag.Message = exception;
            }
            return View();
        }
    }
}