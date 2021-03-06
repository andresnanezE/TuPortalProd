using Aplicacion.Administracion.Contratos;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class CargueImagenBannerController : Controller
    {
        private IServicioAplicacionMenu _servicioAplicacionMenu;

        private IServicioAplicacionMenu ServicioAplicacionMenu
        {
            get { return _servicioAplicacionMenu ?? (_servicioAplicacionMenu = FabricaIoC.Resolver<IServicioAplicacionMenu>()); }
        }

        // GET: CargueImagenBanner
        public ActionResult Index()
        {
            List<string> menus =
            ServicioAplicacionMenu.ObtenerMenuPadres().Select(m => m.DESCRIPCION).ToList();

            return View(menus);
        }
    }
}