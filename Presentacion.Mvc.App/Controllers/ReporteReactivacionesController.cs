using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using MvcReportViewer;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    /// <summary>
    /// JohnNelsonRodriguex
    /// </summary>
    public class ReporteReactivacionesController : Controller
    {
        private readonly string MENSAJE = $"{ConfiguracionesGlobales.MsgErrorRte1} {ConfiguracionesGlobales.MsgErrorRte2}";
        private readonly string MENSAJECARGAPREVIA = $"{ConfiguracionesGlobales.MsgErrorDataPrev}";
        private readonly string MOSTRARTRACE = $"{ConfiguracionesGlobales.MostrarTrace}";

        #region Fields

        private IServicioAplicacionReporteVentasAfiliaciones _servicioAplicacionAfiliaciones;

        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion Fields

        #region Instance Properties

        private IServicioAplicacionReporteVentasAfiliaciones ServicioAplicacionAfiliaciones
        {
            get { return _servicioAplicacionAfiliaciones ?? (_servicioAplicacionAfiliaciones = FabricaIoC.Resolver<IServicioAplicacionReporteVentasAfiliaciones>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        #endregion Instance Properties

        public ActionResult Index()
        {
            var model = new AfiliacionesModel();
            UserClaims usuarioTuPortal;

            try
            {
                usuarioTuPortal = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

                if (model.ListaFiltros == null) model.ListaFiltros = new List<AfiliacionesFiltroDto>();

                model.ListaPeriodos = ServicioAplicacionAfiliaciones.Obtener_Periodos();

                if (!usuarioTuPortal.VerReporte)
                {
                    throw new Exception(ConfiguracionesGlobales.MensajeRteCadenaSupervision);
                }

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                if ((Session["esSesionEnVezDe"] != null) && (Boolean.Parse(Session["esSesionEnVezDe"].ToString())))
                {
                    var documentoPadre = (identity.FindFirst(ClaimTypes.Anonymous).Value).ToString();

                    var nombrePadre = (identity.FindFirst(ClaimTypes.Authentication).Value).ToString();

                    var documentoHijo = (identity.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
                    var nombreHijo = (identity.FindFirst(ClaimTypes.Actor).Value).ToString();

                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = usuarioIdentity,
                        fecha = DateTime.Now,
                        idTipoLog = (int)Consulta.ReporteReactivaciones,
                        ip = ip,
                        MenuId = (int)Menus.Reporte_reactivaciones,
                        DocUsuario1 = documentoPadre,
                        NombreUsuario1 = nombrePadre,
                        DocUsuario2 = documentoHijo,
                        NombreUsuario2 = nombreHijo,
                        FechaHoraIni = DateTime.Now,
                        FechaHoraFin = DateTime.Now,
                        TiempoSesion = TimeSpan.Zero,
                        EsSesionEnVezDe = true
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                }
                else
                {
                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = usuarioIdentity,
                        fecha = DateTime.Now,
                        idTipoLog = (int)Consulta.ReporteReactivaciones,
                        ip = ip,
                        MenuId = (int)Menus.Reporte_reactivaciones
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                }
                //
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }

            return View(model);
        }

        public ActionResult ExportarInformeDetalle(AfiliacionesModel model)
        {
            UserClaims usuarioActual = null;
            List<KeyValuePair<string, object>> parametros = null;
            FileStreamResult reporte = null;

            string ahora = string.Empty;
            string handle = string.Empty;
            string nomRte = string.Empty;

            try
            {
                var contrato = "";
                var contratos = model.TipContrato.ToArray();

                for (var i = 0; i < contratos.Length; i++)
                {
                    if (i < (contratos.Length - 1))
                        contrato = contrato + contratos[i] + ",";
                    else
                        contrato = contrato + contratos[i];
                }

                parametros = new List<KeyValuePair<string, object>>();
                usuarioActual = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

                parametros.Add(new KeyValuePair<string, object>("FECH_PERIODO", DateTime.Parse(model.Periodo).ToString("s"))); //
                parametros.Add(new KeyValuePair<string, object>("TIP_CONTR", contrato));
                parametros.Add(new KeyValuePair<string, object>("ROL", usuarioActual.Rol));
                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO", usuarioActual.Document));
                parametros.Add(new KeyValuePair<string, object>("FECH_PERIODO2", DateTime.Parse(model.Periodo2).ToString("s")));

                //agregar log
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = usuarioActual.UserId;

                var identity = (ClaimsIdentity)User.Identity;

                if ((Session["esSesionEnVezDe"] != null) && (Boolean.Parse(Session["esSesionEnVezDe"].ToString())))
                {
                    var documentoPadre = (identity.FindFirst(ClaimTypes.Anonymous).Value).ToString();

                    var nombrePadre = (identity.FindFirst(ClaimTypes.Authentication).Value).ToString();

                    var documentoHijo = (identity.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
                    var nombreHijo = (identity.FindFirst(ClaimTypes.Actor).Value).ToString();

                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = Int32.Parse(usuarioActual.UserId),
                        fecha = DateTime.Now,
                        idTipoLog = (int)12,
                        ip = ip,
                        MenuId = (int)14,
                        DocUsuario1 = documentoPadre,
                        NombreUsuario1 = nombrePadre,
                        DocUsuario2 = documentoHijo,
                        NombreUsuario2 = nombreHijo,
                        FechaHoraIni = DateTime.Now,
                        FechaHoraFin = DateTime.Now,
                        TiempoSesion = TimeSpan.Zero,
                        EsSesionEnVezDe = true
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                }
                else
                {
                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = Int32.Parse(usuarioActual.UserId),
                        fecha = DateTime.Now,
                        idTipoLog = 12,
                        ip = ip,
                        MenuId = 14
                    };
                    ServicioAplicacionLogs.AgregarLog(log);
                }

                //
                reporte = this.Report(
                                      ReportFormat.Excel,
                          ConfiguracionesGlobales.ReportPathDetalleReactivaciones,
                          ConfiguracionesGlobales.ReportesReportServerUrl,
                          parametros,
                          ConfiguracionesGlobales.ReportesUsername,
                          ConfiguracionesGlobales.ReportesPassword
                          );

                nomRte = model.NOMBRE_REPORTE_DETALLE;
                reporte.FileDownloadName = nomRte;

                ahora = DateTime.Now.ToString("s");
                handle = Guid.NewGuid().ToString();

                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();

                return Json(new { FileGuid = handle, FileName = nomRte });
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

        public ActionResult ExportarInformeConsolidado(AfiliacionesModel model)
        {
            UserClaims usuarioActual = null;
            List<KeyValuePair<string, object>> parametros = null;
            FileStreamResult reporte = null;

            string ahora = string.Empty;
            string handle = string.Empty;
            string nomRte = string.Empty;

            try
            {
                var contrato = "";
                var contratos = model.TipContrato.ToArray();

                for (var i = 0; i < contratos.Length; i++)
                {
                    if (i < (contratos.Length - 1))
                        contrato = contrato + contratos[i] + ",";
                    else
                        contrato = contrato + contratos[i];
                }

                parametros = new List<KeyValuePair<string, object>>();
                usuarioActual = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

                parametros.Add(new KeyValuePair<string, object>("FECH_PERIODO", DateTime.Parse(model.Periodo).ToString("s"))); //
                parametros.Add(new KeyValuePair<string, object>("TIP_CONTR", contrato));
                parametros.Add(new KeyValuePair<string, object>("ROL", usuarioActual.Rol));
                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO", usuarioActual.Document));
                parametros.Add(new KeyValuePair<string, object>("FECH_PERIODO2", DateTime.Parse(model.Periodo2).ToString("s")));

                //reporte log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = usuarioActual.UserId;

                if ((Session["esSesionEnVezDe"] != null) && (Boolean.Parse(Session["esSesionEnVezDe"].ToString())))
                {
                    var documentoPadre = (identity.FindFirst(ClaimTypes.Anonymous).Value).ToString();

                    var nombrePadre = (identity.FindFirst(ClaimTypes.Authentication).Value).ToString();

                    var documentoHijo = (identity.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
                    var nombreHijo = (identity.FindFirst(ClaimTypes.Actor).Value).ToString();

                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = Int32.Parse(usuarioActual.UserId),
                        fecha = DateTime.Now,
                        idTipoLog = (int)Descarga.GeneracionDeReactivacionesDetallado,
                        ip = ip,
                        MenuId = (int)Menus.Reporte_reactivaciones,
                        DocUsuario1 = documentoPadre,
                        NombreUsuario1 = nombrePadre,
                        DocUsuario2 = documentoHijo,
                        NombreUsuario2 = nombreHijo,
                        FechaHoraIni = DateTime.Now,
                        FechaHoraFin = DateTime.Now,
                        TiempoSesion = TimeSpan.Zero,
                        EsSesionEnVezDe = true
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                }
                else
                {
                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = Int32.Parse(usuarioActual.UserId),
                        fecha = DateTime.Now,
                        idTipoLog = (int)Descarga.GeneracionDeReactivacionesDetallado,
                        ip = ip,
                        MenuId = (int)Menus.Reporte_reactivaciones
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                }
                //

                reporte = this.Report(
                                   ReportFormat.Excel,
                       ConfiguracionesGlobales.ReportPathResumenReactivaciones,
                       ConfiguracionesGlobales.ReportesReportServerUrl,
                       parametros,
                       ConfiguracionesGlobales.ReportesUsername,
                       ConfiguracionesGlobales.ReportesPassword
                       );

                nomRte = model.NOMBRE_REPORTE_DETALLE;
                reporte.FileDownloadName = nomRte;
                ahora = DateTime.Now.ToString("s");
                handle = Guid.NewGuid().ToString();
                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();
                return Json(new { FileGuid = handle, FileName = nomRte });
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

        public JsonResult ObtenerPeriodoPorId(DateTime periodoId)
        {
            var result = new JsonResult();
            var periodos = ServicioAplicacionAfiliaciones.Obtener_Periodos().Where(x => x.FECHA_CORTE >= periodoId).OrderBy(x => x.FECHA_CORTE);
            result.Data = periodos;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}