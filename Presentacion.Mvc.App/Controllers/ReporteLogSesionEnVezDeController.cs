using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using MvcReportViewer;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReporteLogSesionEnVezDeController : Controller
    {
        private readonly string nosuccessuse = "Surgio algo ineperado generando el reporte.";
        private readonly string mensajegeneral = "Intentalo nuevamente, si el problema persiste comunicate con el árear de TI.";

        private IServicioAplicacionLogs _servicioAplicacionLogs;

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        // GET: ReporteLogSesionEnVezDe
        public ActionResult Index()
        {
            //actualiza log actividades
            var identity = (ClaimsIdentity)User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var log = new EmbLogActividadesDto
            {
                UsuarioId = usuarioIdentity,
                fecha = DateTime.Now,
                idTipoLog = (int)Consulta.ReporteLogSesionEnVezDe,
                ip = ip,
                MenuId = (int)Menus.Reporte_Sesion_en_vez_de
            };

            ServicioAplicacionLogs.AgregarLog(log);
            //

            return View();
        }

        [HttpPost]
        public ActionResult Index(LogSesionEnVezDe model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, string.Format("{0} {1}", nosuccessuse, mensajegeneral));
                    return View("Index");
                }

                var parametros = new List<KeyValuePair<string, object>>();

                parametros.Add(new KeyValuePair<string, object>("FECHA_INICIAL", string.Format("{0:s}", model.FechaInicial.ToString().Substring(0, 10))));
                parametros.Add(new KeyValuePair<string, object>("FECHA_FIN", string.Format("{0:s}", model.FechaFinal.ToString().Substring(0, 10))));

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;

                var identity = (ClaimsIdentity)User.Identity;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                //parametros.Add(new KeyValuePair<string, object>("idUsuario", usuarioId.ToString()));

                //agrega log actividades
                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioId,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Consulta.DescargarReporteLogSesionEnVezDe,
                    ip = ip,
                    MenuId = (int)Menus.Reporte_Sesion_en_vez_de
                };
                //

                ServicioAplicacionLogs.AgregarLog(log);

                FileStreamResult reporte = null;
                reporte = this.Report(
                           ReportFormat.Excel,
                           ConfiguracionesGlobales.ReporteLogSesionEnVezDe,
                           ConfiguracionesGlobales.ReportesReportServerUrl,
                           parametros,
                           ConfiguracionesGlobales.ReportesUsername,
                           ConfiguracionesGlobales.ReportesPassword
                           );

                reporte.FileDownloadName = "ReporteLogSesionEnVezDe.xls";
                return reporte;
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                ModelState.AddModelError(string.Empty, string.Format("{0} {1}", nosuccessuse, mensajegeneral));
                return View("Index");
            }
        }
    }
}