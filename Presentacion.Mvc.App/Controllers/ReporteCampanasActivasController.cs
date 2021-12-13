using Presentacion.Mvc.App.Models;
using System;
using System.Security.Claims;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReporteCampanasActivasController : Controller
    {
        private readonly string MENSAJE = $"{ConfiguracionesGlobales.MsgErrorRte1} {ConfiguracionesGlobales.MsgErrorRte2}";
        private readonly string MENSAJECARGAPREVIA = $"{ConfiguracionesGlobales.MsgErrorDataPrev}";
        private readonly string MOSTRARTRACE = $"{ConfiguracionesGlobales.MostrarTrace}";

        /// <summary>
        /// Metodo encargado de iniciar las opciones de seleccion multiple como drop down list
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string msg)
        {
            var modelo = new CampanasActivasModel();
            ViewBag.Mensaje = msg;
            try
            {
#pragma warning disable CS0219 // The variable 'reporte' is assigned but its value is never used
                FileStreamResult reporte = null;
#pragma warning restore CS0219 // The variable 'reporte' is assigned but its value is never used
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(modelo);
        }
    }
}