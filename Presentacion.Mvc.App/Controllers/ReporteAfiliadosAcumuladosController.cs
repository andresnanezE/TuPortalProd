using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto;
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
    public class ReporteAfiliadosAcumuladosController : Controller
    {
        private readonly string MENSAJE = $"{ConfiguracionesGlobales.MsgErrorRte1} {ConfiguracionesGlobales.MsgErrorRte2}";
        private readonly string MENSAJECARGAPREVIA = $"{ConfiguracionesGlobales.MsgErrorDataPrev}";
        private readonly string MOSTRARTRACE = $"{ConfiguracionesGlobales.MostrarTrace}";

        #region Fields

        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioAplicacionReporteVentasAfiliaciones _servicioAplicacionAfiliaciones;
        private IServicioAplicacionUsuarios _servicioAplicacionUsuarios;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion Fields

        #region Instance Properties

        private IServicioAplicacionReporteVentasAfiliaciones ServicioAplicacionAfiliaciones
        {
            get { return _servicioAplicacionAfiliaciones ?? (_servicioAplicacionAfiliaciones = FabricaIoC.Resolver<IServicioAplicacionReporteVentasAfiliaciones>()); }
        }

        private IServicioAplicacionUsuarios ServicioAplicacionUsuarios
        {
            get { return _servicioAplicacionUsuarios ?? (_servicioAplicacionUsuarios = FabricaIoC.Resolver<IServicioAplicacionUsuarios>()); }
        }

        private IServicioAplicacionRoles ServicioAplicacionRoles
        {
            get { return _servicioAplicacionRoles ?? (_servicioAplicacionRoles = FabricaIoC.Resolver<IServicioAplicacionRoles>()); }
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
                model.ListaFiltros = new List<AfiliacionesFiltroDto>();
                usuarioTuPortal = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

                ViewBag.CS = UserClaims.EsRolCadenaSupervision(usuarioTuPortal.Rol);
                ViewBag.DTOR = UserClaims.EsRolDirector(usuarioTuPortal.Rol);

                if (!usuarioTuPortal.VerReporte)
                {
                    throw new Exception(ConfiguracionesGlobales.MensajeRteCadenaSupervision);
                }

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                if ((Session["esSesionEnVezDe"] != null) && (Boolean.Parse(Session["esSesionEnVezDe"].ToString())))
                {
                    var documentoPadre = (identity.FindFirst(ClaimTypes.Anonymous).Value).ToString();

                    var nombrePadre = (identity.FindFirst(ClaimTypes.Authentication).Value).ToString();

                    var documentoHijo = (identity.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
                    var nombreHijo = (identity.FindFirst(ClaimTypes.Actor).Value).ToString();

                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = usuarioId,
                        fecha = DateTime.Now,
                        idTipoLog = (int)Consulta.AfiliadosAcumuladosNetos,
                        ip = ip,
                        MenuId = (int)Menus.ReportesAfiliadosAcumulados,
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
                        UsuarioId = usuarioId,
                        fecha = DateTime.Now,
                        idTipoLog = (int)Consulta.AfiliadosAcumuladosNetos,
                        ip = ip,
                        MenuId = (int)Menus.ReportesAfiliadosAcumulados
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
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;

                string mensaje = string.Empty;

                for (var i = 0; i < contratos.Length; i++)
                {
                    if (i < (contratos.Length - 1))
                        contrato = contrato + contratos[i] + ",";
                    else
                        contrato = contrato + contratos[i];
                }

                // usuarioActual = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);
                usuarioActual = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);
                parametros = new List<KeyValuePair<string, object>>();

                parametros.Add(new KeyValuePair<string, object>("TIP_CONTR", contrato));
                parametros.Add(new KeyValuePair<string, object>("ROL", usuarioActual.Rol));
                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO", usuarioActual.Document));

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
                        idTipoLog = (int)Descarga.GeneracionDeAfiliadosAcumuladosDetallado,
                        ip = ip,
                        MenuId = (int)Menus.ReportesAfiliadosAcumulados,
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
                        idTipoLog = (int)Descarga.GeneracionDeAfiliadosAcumuladosDetallado,
                        ip = ip,
                        MenuId = (int)Menus.ReportesAfiliadosAcumulados
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                }
                //

                reporte = this.Report(
                               ReportFormat.Excel,
                   ConfiguracionesGlobales.ReportPathResumenAfiliadosAcumulados,
                   ConfiguracionesGlobales.ReportesReportServerUrl,
                   parametros,
                   ConfiguracionesGlobales.ReportesUsername,
                   ConfiguracionesGlobales.ReportesPassword
                   );

                nomRte = model.NOMBRE_REPORTE_RESUMEN;
                reporte.FileDownloadName = model.NOMBRE_REPORTE_RESUMEN;

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
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;

                string mensaje = string.Empty;

                for (var i = 0; i < contratos.Length; i++)
                {
                    if (i < (contratos.Length - 1))
                        contrato = contrato + contratos[i] + ",";
                    else
                        contrato = contrato + contratos[i];
                }

                usuarioActual = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);
                parametros = new List<KeyValuePair<string, object>>();

                parametros.Add(new KeyValuePair<string, object>("TIP_CONTR", contrato));
                parametros.Add(new KeyValuePair<string, object>("ROL", usuarioActual.Rol));
                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO", usuarioActual.Document));

                //log actividades

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
                        idTipoLog = (int)Descarga.GeneracionDeafiliadosAcumuladosConsolidado,
                        ip = ip,
                        MenuId = (int)Menus.ReportesAfiliadosAcumulados,
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
                        idTipoLog = (int)Descarga.GeneracionDeafiliadosAcumuladosConsolidado,
                        ip = ip,
                        MenuId = (int)Menus.ReportesAfiliadosAcumulados
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                }
                //

                reporte = this.Report(
                                        ReportFormat.Excel,
                            ConfiguracionesGlobales.ReportPathDetallesAfiliadosAcumulados,
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

        public JsonResult MensajeCantidadRegistrosNetos(string contrato)
        {
            string mensaje = string.Empty;
            var usuarioTuPortal = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

            mensaje = ServicioAplicacionAfiliaciones.MensajeCantidadRegistrosNetos(new DatosConsultaAfiliacionDto()
            {
                Documento = usuarioTuPortal.Document,
                Rol = Int32.Parse(usuarioTuPortal.Rol),
                TipContr = contrato
            });

            return Json(mensaje);
        }
    }
}