using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Aplicacion.Administracion.Contratos;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion.IoC;
using MvcReportViewer;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReporteApoyoRodamientoController : Controller
    {

        private IServicioAplicacionApoyoRodamiento _servicioAplicacionARodamiento;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private IServicioAplicacionReporteVentasAfiliaciones _servicioAplicacionAfiliaciones;

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        private IServicioAplicacionReporteVentasAfiliaciones ServicioAplicacionAfiliaciones
        {
            get
            {
                return _servicioAplicacionAfiliaciones ??
                       (_servicioAplicacionAfiliaciones =
                           FabricaIoC.Resolver<IServicioAplicacionReporteVentasAfiliaciones>());
            }
        }

        private IServicioAplicacionApoyoRodamiento ServicioAplicacionRodamiento
        {
            get
            {
                return _servicioAplicacionARodamiento ??
                       (_servicioAplicacionARodamiento =
                           FabricaIoC.Resolver<IServicioAplicacionApoyoRodamiento>());
            }
        }

        // GET: ReporteTarjetasTrasmilenio
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
                idTipoLog = (int)Consulta.ApoyoRodamiento,
                ip = ip,
                MenuId = (int)Menus.ReporteApoyoRodamiento
            };

            ServicioAplicacionLogs.AgregarLog(log);
            //

            var model = new ApoyoRodamientoModel();
            return View(model);
        }

        public ActionResult ExportarRteApoyoRodamiento(ApoyoRodamientoModel _modelRequest)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var parametros = new List<KeyValuePair<string, object>>();

                DateTime dtIni = DateTime.Parse(_modelRequest.PeriodoIni);
                DateTime dtFin = DateTime.Parse(_modelRequest.PeriodoFin);
                
                if ((dtFin.Month - dtIni.Month) == 1)
                {
                    dtIni = dtFin;
                }
                else
                {
                    dtIni = dtIni.AddMonths(1);
                }

                parametros.Add(new KeyValuePair<string, object>("fechaPeriodoIni", string.Format("{0:s}", dtIni.ToString().Substring(0, 10))));
                parametros.Add(new KeyValuePair<string, object>("fechaPeriodoFin", string.Format("{0:s}", dtFin.ToString().Substring(0, 10))));
                parametros.Add(new KeyValuePair<string, object>("definitivo", _modelRequest.Definitivo));

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                parametros.Add(new KeyValuePair<string, object>("idUsuario", usuarioId.ToString()));

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioId,
                    fecha = DateTime.Now,
                    idTipoLog =(int)Consulta.DescargarInformeApoyoRodamiento,
                    ip = ip,
                    MenuId = (int)Menus.ReporteApoyoRodamiento
                };

                ServicioAplicacionLogs.AgregarLog(log);

                FileStreamResult reporte = null;
                reporte = this.Report(
                           ReportFormat.Excel,
                           ConfiguracionesGlobales.ReportPathApoyoRodamiento,
                           ConfiguracionesGlobales.ReportesReportServerUrl,
                           parametros,
                           ConfiguracionesGlobales.ReportesUsername,
                           ConfiguracionesGlobales.ReportesPassword
                           );

                reporte.FileDownloadName = "ReporteApoyoRodamiento.xls";
                return reporte;

            }
            catch (Exception e)
            {
                _modelRequest.MensajeError = "Error realizando la exportación del informe, comuníquese con el administrador. " + e.Message;
                return PartialView("Error");
            }
        }

        [HttpPost]
        public JsonResult PeriodosNoDefinitivos()
        {
            var dateFormatString = new List<Tuple<string, string>>();
            Tuple<string, string> tp = null;

            var lista = ServicioAplicacionAfiliaciones.Obtener_Periodos();


            foreach (var date in lista)
            {
                tp = new Tuple<string, string>(String.Format("{0:dd/MM/yyyy}", date.FEC_INI), String.Format("{0:dd/MM/yyyy}", date.FECHA_CORTE));
                dateFormatString.Add(tp);
            }

            return Json(dateFormatString);
        }
    }
}