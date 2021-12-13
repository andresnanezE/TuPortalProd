using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using MvcReportViewer;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReporteExtractoAnticiposController : Controller
    {
        private readonly string MENSAJE = $"{ConfiguracionesGlobales.MsgErrorRte1} {ConfiguracionesGlobales.MsgErrorRte2}";
        private readonly string MENSAJECARGAPREVIA = $"{ConfiguracionesGlobales.MsgErrorDataPrev}";
        private readonly string MOSTRARTRACE = $"{ConfiguracionesGlobales.MostrarTrace}";

        #region Fields

        private IServicioAplicacionUsuarios _servicioAplicacionUsuarios;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private IServicioAplicacionReporteVentasAfiliaciones _servicioAplicacionAfiliaciones;

        #endregion Fields

        #region Instance Properties

        private IServicioAplicacionUsuarios ServicioAplicacionUsuarios
        {
            get { return _servicioAplicacionUsuarios ?? (_servicioAplicacionUsuarios = FabricaIoC.Resolver<IServicioAplicacionUsuarios>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        private IServicioAplicacionReporteVentasAfiliaciones ServicioAplicacionAfiliaciones
        {
            get { return _servicioAplicacionAfiliaciones ?? (_servicioAplicacionAfiliaciones = FabricaIoC.Resolver<IServicioAplicacionReporteVentasAfiliaciones>()); }
        }

        #endregion Instance Properties

        public ActionResult Index()
        {
            var model = new ExtractoAnticiposModel();
            var identity = (ClaimsIdentity)User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var usuarioActual = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

            model.CodAses = usuarioActual.Document;

            if ((Session["esSesionEnVezDe"] != null) && (Boolean.Parse(Session["esSesionEnVezDe"].ToString())))
            {
                var documentoPadre = (identity.FindFirst(ClaimTypes.Anonymous).Value).ToString();

                var nombrePadre = (identity.FindFirst(ClaimTypes.Authentication).Value).ToString();

                var documentoHijo = (identity.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
                var nombreHijo = (identity.FindFirst(ClaimTypes.Actor).Value).ToString();

                //var log = new EmbLogActividadesDto
                //{
                //    UsuarioId = usuarioId,
                //    fecha = DateTime.Now,
                //    idTipoLog = (int)Consulta.ReporteComisionesPendientes,
                //    ip = ip,
                //    MenuId = (int)Menus.Extracto_de_comisiones,
                //    DocUsuario1 = documentoPadre,
                //    NombreUsuario1 = nombrePadre,
                //    DocUsuario2 = documentoHijo,
                //    NombreUsuario2 = nombreHijo,
                //    FechaHoraIni = DateTime.Now,
                //    FechaHoraFin = DateTime.Now,
                //    TiempoSesion = TimeSpan.Zero,
                //    EsSesionEnVezDe = true
                //};

                //ServicioAplicacionLogs.AgregarLog(log);
            }
            else
            {
                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioId,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Consulta.ReporteComisionesPendientes,
                    ip = ip,
                    MenuId = (int)Menus.Extracto_de_comisiones
                };

                ServicioAplicacionLogs.AgregarLog(log);
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult GenerarReporte(ExtractoAnticiposModel model)
        {
            model.RmtProc = 0;
            var UsuarioActual = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);
            var parametros = new List<KeyValuePair<string, object>>();
            FileStreamResult reporte = null;

            string ahora = string.Empty;
            string handle = string.Empty;
            string nonbrerte = string.Empty;

            try
            {
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var user = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);
                var log = new EmbLogActividadesDto
                {
                    UsuarioId = Int32.Parse(user.UserId),
                    fecha = DateTime.Now,
                    idTipoLog = 10,//6,
                    ip = ip,
                    MenuId = 9
                };

                ServicioAplicacionLogs.AgregarLog(log);

                //parametros.Add(new KeyValuePair<string, object>("USUARIO", UsuarioActual.UserName));
                //parametros.Add(new KeyValuePair<string, object>("FUNCIONARIO", model.CodAses));
                //parametros.Add(new KeyValuePair<string, object>("COD_CCOS", model.CodSucursal));
                //parametros.Add(new KeyValuePair<string, object>("FECHA_INICIO", DateTime.Parse(model.Periodo).ToString("s")));
                //parametros.Add(new KeyValuePair<string, object>("FECHA_FIN", DateTime.Parse(model.Periodo2).ToString("s")));

                parametros.Add(new KeyValuePair<string, object>("COD_ASES", model.CodAses));
                parametros.Add(new KeyValuePair<string, object>("RMT_PROC", model.RmtProc));

                reporte = this.Report(
                    ReportFormat.Excel,
                    ConfiguracionesGlobales.ReportPathExtractoAnticiposAsesor,
                    ConfiguracionesGlobales.ReportesReportServerUrl,
                    parametros,
                    ConfiguracionesGlobales.ReportesUsername,
                    ConfiguracionesGlobales.ReportesPassword
                    );

                nonbrerte = "ReporteExtractoAnticipos - " + model.CodAses + " " + DateTime.Now.ToShortDateString() + ".xls";

                ahora = DateTime.Now.ToString("s");
                handle = Guid.NewGuid().ToString();

                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();

                return Json(new { FileGuid = handle, FileName = nonbrerte });
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