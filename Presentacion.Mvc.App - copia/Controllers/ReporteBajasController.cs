using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion.IoC;
using MvcReportViewer;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using System.IO;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReporteBajasController : Controller
    {

        private readonly string MENSAJE = $"{ConfiguracionesGlobales.MsgErrorRte1} {ConfiguracionesGlobales.MsgErrorRte2}";
        private readonly string MENSAJECARGAPREVIA = $"{ConfiguracionesGlobales.MsgErrorDataPrev}";
        private readonly string MOSTRARTRACE = $"{ConfiguracionesGlobales.MostrarTrace}";

        #region Fields


        private IServicioAplicacionTablasBase _servicioAplicacionTablasBase;
        private IServicioAplicacionReporteVentasAfiliaciones _servicioAplicacionAfiliaciones;
       
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        
        #endregion

        #region Instance Properties

        private IServicioAplicacionReporteVentasAfiliaciones ServicioAplicacionAfiliaciones
        {
            get { return _servicioAplicacionAfiliaciones ?? (_servicioAplicacionAfiliaciones = FabricaIoC.Resolver<IServicioAplicacionReporteVentasAfiliaciones>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

      

        #endregion


        public ActionResult Index()
        {

            var model = new AfiliacionesModel();
            UserClaims usuarioTuPortal;

            try
            {
                if (model.ListaFiltros == null) model.ListaFiltros = new List<AfiliacionesFiltroDto>();

                model.ListaPeriodos = ServicioAplicacionAfiliaciones.Obtener_Periodos();
                usuarioTuPortal = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

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
                        idTipoLog = (int)Consulta.ReporteBajas,
                        ip = ip,
                        MenuId = (int)Menus.Reporte_bajas,
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
                        idTipoLog = (int)Consulta.ReporteBajas,
                        ip = ip,
                        MenuId = (int)Menus.Reporte_bajas
                    };
                    ServicioAplicacionLogs.AgregarLog(log);
                }                
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, MENSAJECARGAPREVIA);
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

                var cantBaja = model.BajasComerciales.ToArray().Length;
                var baja = model.BajasComerciales.ToArray()[0];
                var bajas = cantBaja == 1 ? baja.ToString() : "";
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;


                parametros = new List<KeyValuePair<string, object>>();

                for (var i = 0; i < contratos.Length; i++)
                {
                    if (i < (contratos.Length - 1))
                        contrato = contrato + contratos[i] + ",";
                    else
                        contrato = contrato + contratos[i];
                }

                usuarioActual = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

                parametros.Add(new KeyValuePair<string, object>("COMERCIAL", bajas));
                parametros.Add(new KeyValuePair<string, object>("FECH_PERIODO", DateTime.Parse(model.Periodo).ToString("s"))); 
                parametros.Add(new KeyValuePair<string, object>("TIP_CONTR", contrato));
                parametros.Add(new KeyValuePair<string, object>("ROL", usuarioActual.Rol));
                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO", usuarioActual.Document));
                parametros.Add(new KeyValuePair<string, object>("FECH_PERIODO2", DateTime.Parse(model.Periodo2).ToString("s")));

                //reporte de actividades log
                var identity = (ClaimsIdentity)HttpContext.User.Identity;
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
                        idTipoLog = (int)Consulta.ReporteBajas,
                        ip = ip,
                        MenuId = (int)Menus.Reporte_bajas,
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
                        idTipoLog = (int)Consulta.ReporteBajas,
                        ip = ip,
                        MenuId = (int)Menus.Reporte_bajas
                    };
                    ServicioAplicacionLogs.AgregarLog(log);
                }
                //

                reporte = this.Report(
                            ReportFormat.Excel,
                            ConfiguracionesGlobales.ReportPathDetallesBajas, 
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


                return Json(new { FileGuid = handle, FileName =nomRte});

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

                var cantBaja = model.BajasComerciales.ToArray().Length;
                var baja = model.BajasComerciales.ToArray()[0];
                var bajas = cantBaja == 1 ? baja.ToString() : "";
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                

                parametros = new List<KeyValuePair<string, object>>();

                for (var i = 0; i < contratos.Length; i++)
                {
                    if (i < (contratos.Length - 1))
                        contrato = contrato + contratos[i] + ",";
                    else
                        contrato = contrato + contratos[i];
                }

                usuarioActual = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

                parametros.Add(new KeyValuePair<string, object>("COMERCIAL", bajas));
                parametros.Add(new KeyValuePair<string, object>("FECH_PERIODO", DateTime.Parse(model.Periodo).ToString("s")));
                parametros.Add(new KeyValuePair<string, object>("TIP_CONTR", contrato));
                parametros.Add(new KeyValuePair<string, object>("ROL", usuarioActual.Rol));
                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO", usuarioActual.Document));
                parametros.Add(new KeyValuePair<string, object>("FECH_PERIODO2", DateTime.Parse(model.Periodo2).ToString("s")));


                //reporte log actividades 
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
                        idTipoLog = (int)Descarga.GeneracionDeBajasDetallado,
                        ip = ip,
                        MenuId = (int)Menus.Detalle_de_afiliaciones,
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
                        idTipoLog = (int)Descarga.GeneracionDeBajasDetallado,
                        ip = ip,
                        MenuId = (int)Menus.Reporte_bajas
                    };
                    ServicioAplicacionLogs.AgregarLog(log);
                }
                //

                reporte = this.Report(
                           ReportFormat.Excel,
                           ConfiguracionesGlobales.ReportPathResumenBajas,
                           ConfiguracionesGlobales.ReportesReportServerUrl,
                           parametros,
                           ConfiguracionesGlobales.ReportesUsername,
                           ConfiguracionesGlobales.ReportesPassword
                           );

                nomRte = model.NOMBRE_REPORTE_RESUMEN;
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