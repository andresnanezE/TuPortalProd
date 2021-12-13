using Presentacion.Mvc.App.Models;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Controllers
{
    public class MensajeController : Controller
    {
        // GET: Mensaje
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Emergente(string title, string mess)
        {
            MessageModel em = new MessageModel() { Titulo = title, Cuerpo = mess, aceptar = "Aceptar" };
            return PartialView("_EmergenteMes", em);
        }
    }
}