using Aplicacion.Administracion.Contratos;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    [Authorize]
    public class ArchivosVentasController : Controller
    {
        private IServicioArchivosVentas _ServicioArchivos;


        private IServicioArchivosVentas ServicioArchivos
        {
            get { return _ServicioArchivos ?? (_ServicioArchivos = FabricaIoC.Resolver<IServicioArchivosVentas>()); }
        }
        // GET: ArchivosVentas

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetArchivosVentas(string documento)
        {

            var archivos = await ServicioArchivos.GetArchivosVentas(documento);
            return new JsonResult { Data = archivos };

        }

        public async Task<FileResult> DownloadFile(string name)
        {
            var message = new HttpResponseMessage(HttpStatusCode.OK);
            var blob = await ServicioArchivos.GetArchivoBlob(name);

            return File(blob.File, MimeMapping.GetMimeMapping(name), name);

        }


    }
}