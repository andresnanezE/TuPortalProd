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
    public class ReporteTarjetasTrasmilenioController : Controller
    {
        readonly string nosuccessuse = "Surgio algo ineperado generando el reporte.";
        readonly string mensajegeneral = "Intentalo nuevamente, si el problema persiste comunicate con el árear de TI.";

        private IServicioAplicacionTarjetasTransmilenio _servicioAplicacionTTransmilenio;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private IServicioAplicacionReporteVentasAfiliaciones _servicioAplicacionAfiliaciones;

         private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

         private IServicioAplicacionTarjetasTransmilenio ServicioAplicacionTTransmilenio
         {
             get
             {
                 return _servicioAplicacionTTransmilenio ??
                        (_servicioAplicacionTTransmilenio =
                            FabricaIoC.Resolver<IServicioAplicacionTarjetasTransmilenio>());
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
                idTipoLog = (int)Consulta.TarjetasTransmilenio,
                ip = ip,
                MenuId = (int)Menus.Tarjetas_transmilenio
            };

            ServicioAplicacionLogs.AgregarLog(log);
            //

            var model = new TarjetasTransmilenioModel();
            return View(model);
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

        public ActionResult ExportarRteTarjetasTransmilenio(TarjetasTransmilenioModel _modelRequest)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var parametros = new List<KeyValuePair<string, object>>();

                DateTime dtIni = DateTime.Parse(_modelRequest.PeriodoIni);
                DateTime dtFin = DateTime.Parse(_modelRequest.PeriodoFin);
                
                if((dtFin.Month - dtIni.Month) == 1)
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

                // agrega actividad
                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioId,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Descarga.GeneracionDeReporteTarjetasTransmilenio,
                    ip = ip,
                    MenuId = (int) Menus.Tarjetas_transmilenio
                };
                //

                ServicioAplicacionLogs.AgregarLog(log);

                FileStreamResult reporte = null;
                reporte = this.Report(
                           ReportFormat.Excel,
                           ConfiguracionesGlobales.ReportPathTarjetasTrasmilenio,
                           ConfiguracionesGlobales.ReportesReportServerUrl,
                           parametros,
                           ConfiguracionesGlobales.ReportesUsername,
                           ConfiguracionesGlobales.ReportesPassword
                           );

                reporte.FileDownloadName = "ReporteTarjetasTransmilenio.xls";
                return reporte;

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0} {1}", nosuccessuse, mensajegeneral));
                return View("Index");
            }
        }

        [HttpPost]
         public JsonResult PeriodosNoDefinitivos()
        {
            var dateFormatString = new List<Tuple<string,string>>();
            Tuple<string, string> tp = null;

            var lista = ServicioAplicacionAfiliaciones.Obtener_Periodos(); 
            

            foreach(var date in lista)
            {
                tp = new Tuple<string, string>(String.Format("{0:dd/MM/yyyy}", date.FEC_INI), String.Format("{0:dd/MM/yyyy}", date.FECHA_CORTE));
                dateFormatString.Add(tp);
            }

            return Json(dateFormatString);
        }
    }
}