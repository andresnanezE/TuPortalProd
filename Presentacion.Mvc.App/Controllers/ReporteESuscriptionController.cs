using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using MvcReportViewer;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReporteESuscriptionController : Controller
    {        
        private readonly string MENSAJE = $"{ConfiguracionesGlobales.MsgErrorRte1} {ConfiguracionesGlobales.MsgErrorRte2}";
        private readonly string MENSAJECARGAPREVIA = $"{ConfiguracionesGlobales.MsgErrorDataPrev}";
        private readonly string MOSTRARTRACE = $"{ConfiguracionesGlobales.MostrarTrace}";

        private IServicioAplicacionLogs _servicioAplicacionLogs;

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        // GET: ReporteESuscription
        public ActionResult Index()
        {   
            var viewModel = new ESuscriptionViewModel();
            //viewModel.FechaInicial = Convert.ToDateTime( DateTime.Now.ToString("MM/dd/yyyy"));
            //viewModel.FechaFinal = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult GenerarReporte(ESuscriptionViewModel viewModel)
        {
            ViewBag.mensaje = "";            
            try
            {
                FileStreamResult reporte = null;
                var user = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var log = new EmbLogActividadesDto
                {
                    UsuarioId = Int32.Parse(user.UserId),
                    fecha = DateTime.Now,
                    idTipoLog = 120,//6,
                    ip = ip,
                    MenuId = 9
                };

                ServicioAplicacionLogs.AgregarLog(log);

                viewModel.Cedula = user.Document;

                if (ModelState.IsValid)
                {
                    var parametros = new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("FECHA_INICIAL", viewModel.FechaInicial.ToString("dd/MM/yyyy")),
                        new KeyValuePair<string, object>("FECHA_FINAL", viewModel.FechaFinal.ToString("dd/MM/yyyy")),
                        new KeyValuePair<string, object>("EVENTO", viewModel.IdEvento),
                        new KeyValuePair<string, object>("CEDULA", viewModel.Cedula)
                    };
                    
                    reporte = this.Report(
                                    ReportFormat.Excel,
                        ConfiguracionesGlobales.ReportPathESuscription,
                        ConfiguracionesGlobales.ReportesReportServerUrl,
                        parametros,
                        ConfiguracionesGlobales.ReportesUsername,
                        ConfiguracionesGlobales.ReportesPassword
                        );

                    reporte.FileDownloadName = "ReporteESuscription_"+ viewModel.Cedula + ".xls";

                    var ahora = DateTime.Now.ToString("s");
                    var handle = Guid.NewGuid().ToString();

                    TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();

                    return Json(new { FileGuid = handle, FileName = reporte.FileDownloadName });
                }
                else
                {
                    return Json(new
                    {
                        msgError = "Error con los filtros seleccionados, por favor revise la información ingresada"
                    });
                    
                }
            }

            catch (Exception exception)
            {
                return Json(new
                {
                    msgError = MENSAJE,
                    msgErrorException = MOSTRARTRACE.Equals("1") ?
                  $"{exception.Source}</br>{exception.Message}</br>{exception.StackTrace}" : string.Empty
                });
            }
        }
    }
}