using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Security.Claims;
using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto.DtoProcesos;
using MvcReportViewer;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion.IoC;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using System.IO;

namespace Presentacion.Mvc.App.Controllers
{
    public class ComisionesAsesorMesController : Controller
    {
        private readonly string MENSAJE = $"{ConfiguracionesGlobales.MsgErrorRte1} {ConfiguracionesGlobales.MsgErrorRte2}";
        private readonly string MENSAJECARGAPREVIA = $"{ConfiguracionesGlobales.MsgErrorDataPrev}";

        private IServicioAplicacionProcesosAsesor _servicioAplicacionProcesosAsesor;
        private IServicioAplicacionLogs _servicioAplicacionLogs;


        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        private IServicioAplicacionProcesosAsesor ServicioAplicacionProcesosAsesor
        {
            get
            {
                return _servicioAplicacionProcesosAsesor ??
                       (_servicioAplicacionProcesosAsesor =
                           FabricaIoC.Resolver<IServicioAplicacionProcesosAsesor>());
            }
        }

        // GET: ComicionesAsesorMes
        public ActionResult Index()
        {
            var UsuarioActual = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);

            try
            {
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
                var anios = ServicioAplicacionProcesosAsesor.ProcesosAsesor(0, 0, Int32.Parse(UsuarioActual.Document));

                if (anios != null)
                {
                    ViewBag.Anios = anios;
                }
                else
                {
                    throw new Exception(MENSAJECARGAPREVIA);
                }

                return View();
            }
            catch (Exception exception)
            {

                ModelState.AddModelError(string.Empty, MENSAJECARGAPREVIA);
                return View();
            }
        }

        public JsonResult GenerarReporte(ComisionesAsesorMes model)
        {

            UserClaims UsuarioActual;
            var parametros = new List<KeyValuePair<string, object>>();
            FileStreamResult reporte = null;

            string ahora = string.Empty;
            string handle = string.Empty;

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { msgError = MENSAJE, msgErrorException = "El modelo no es valido." });
                }

                UsuarioActual = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);

                parametros.Add(new KeyValuePair<string, object>("RMT_PROP", model.ID_PROC));
                parametros.Add(new KeyValuePair<string, object>("COD_ASES", UsuarioActual.Document));
                parametros.Add(new KeyValuePair<string, object>("ROL", UsuarioActual.Rol));

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var log = new EmbLogActividadesDto
                {
                    UsuarioId = Int32.Parse(UsuarioActual.UserId),
                    fecha = DateTime.Now,
                    idTipoLog = (int)Consulta.TarjetasTransmilenio,
                    ip = ip,
                    MenuId = (int)Menus.Afiliados_acumulados
                };

                ServicioAplicacionLogs.AgregarLog(log);

                
                reporte = this.Report(
                           ReportFormat.Excel,
                           ConfiguracionesGlobales.ComisionesAsesorProceso,
                           ConfiguracionesGlobales.ReportesReportServerUrl,
                           parametros,
                           ConfiguracionesGlobales.ReportesUsername,
                           ConfiguracionesGlobales.ReportesPassword
                           );

                ahora = DateTime.Now.ToString("s");
                handle = Guid.NewGuid().ToString();

                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();


                return Json(new { FileGuid = handle, FileName = model.NOMBRE_REPORTE });

            }
            catch (Exception exception)
            {

                return Json(new { msgError = MENSAJE, msgErrorException = exception.Message });
            }
        }

        [HttpPost]
        public JsonResult Procesos(string mes, string anio)
        {
            var UsuarioActual = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);


            try
            {

                var lista = ServicioAplicacionProcesosAsesor.ProcesosAsesor(Convert.ToInt32(mes)
                    , Convert.ToInt32(anio)
                    , Convert.ToInt32(UsuarioActual.Document));
                var dateFormatString = new List<string>();

                return Json(lista.ToList());
            }
            catch (Exception e)
            {

                return Json(null);
            }

        }


    }
}