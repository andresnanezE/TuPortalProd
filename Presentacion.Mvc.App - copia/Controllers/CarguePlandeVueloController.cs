using System.Collections.Generic;
using System.Web.Mvc;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion.IoC;
using Aplicacion.Administracion.Contratos;

namespace Presentacion.Mvc.App.Controllers
{
    public class CarguePlandeVueloController : Controller
    {

        private IServicioAplicacionComoVoy _servicioAplicacionComoVoy;

        private IServicioAplicacionComoVoy ServicioAplicacionComoVoy
        {
            get
            {
                return _servicioAplicacionComoVoy ??
                       (_servicioAplicacionComoVoy =
                           FabricaIoC.Resolver<IServicioAplicacionComoVoy>());
            }
        }
        // GET: CarguePlandeVuelo
        public ActionResult Index()
        {

            var planesdevuelo = ServicioAplicacionComoVoy.ObtenerPlanesDeVuelo().Split(',');

            List<UploadFiles> lstUploadFiles = new List<UploadFiles>
            {
                 new UploadFiles() {
                Extension = ".pdf",
                KeyConfig = "PathPlanVueloCorretaje",
                Nombre = planesdevuelo[0],
                FileData = string.Empty,
                labels = "Corretaje",
                Id = "PlanVuelo1"
            },

                 new UploadFiles() {
                Extension = ".pdf",
                KeyConfig = "PathPlanVueloCorretaje",
                Nombre = planesdevuelo[1],
                FileData = string.Empty,
                labels = "Corretaje",
                Id = "PlanVuelo2"
            },

                 new UploadFiles() {
                Extension = ".pdf",
                KeyConfig = "PathPlanVueloDirecto",
                Nombre = string.Empty,
                FileData = string.Empty,
                labels = "Directo",
                Id = "PlanVuelo3"
            },

                 new UploadFiles() {
                Extension = ".pdf",
                KeyConfig = "PathCondicionesDirecto",
                Nombre = string.Empty,
                FileData = string.Empty,
                labels = "Directo",
                Id = "PlanVuelo4"
            },

            //     new UploadFiles() {
            //    Extension = ".pdf",
            //    KeyConfig = "PathCondicionesDirecto",
            //    Nombre = string.Empty,
            //    FileData = string.Empty,
            //    labels = "Directo",
            //    Id = "PlanVuelo4"
            //},


        };


            return View(lstUploadFiles);
        }
    }
}