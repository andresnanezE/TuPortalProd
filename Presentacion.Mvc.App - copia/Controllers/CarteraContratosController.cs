using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Presentacion.Mvc.App.Models;
using MvcReportViewer;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using System.IO;
using Aplicacion.Administracion.Implementacion;
using Aplicacion.Administracion.Contratos;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class CarteraContratosController : Controller
    {

        private readonly string MENSAJE = $"{ConfiguracionesGlobales.MsgErrorRte1} {ConfiguracionesGlobales.MsgErrorRte2}";
        private readonly string MENSAJECARGAPREVIA = $"{ConfiguracionesGlobales.MsgErrorDataPrev}";
        private readonly string MOSTRARTRACE = $"{ConfiguracionesGlobales.MostrarTrace}";
        private readonly string MENSAJEMODEL = "El valor del campo Rmt es obligatorio y debe ser un número.";


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


        // GET: CarteraContratos
        public ActionResult CarteraContratos()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CarteraContratos(FormCollection form)
        {


            UserClaims usuarioActual = null;
            List<KeyValuePair<string, object>> parametros = null;
            FileStreamResult reporte = null;
            

            string ahora = string.Empty;
            string handle = string.Empty;
            string nomRte = string.Empty;
            string msg = string.Empty;
            int testRmt;

            string rmt = form["rmt_text"];
            nomRte = form["nomReporte"];
                        
            try
            {
                
                if (!Int32.TryParse(rmt, out testRmt))
                {
                    msg = MENSAJEMODEL;
                    ModelState.AddModelError("", MENSAJEMODEL);
                    throw new Exception(MENSAJEMODEL);
                }

                //if (string.IsNullOrEmpty(rmt))
                //{
                //    msg = MENSAJEMODEL;
                //    ModelState.AddModelError("", MENSAJEMODEL);
                //    throw new Exception(MENSAJEMODEL);
                //}

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;

                parametros = new List<KeyValuePair<string, object>>();

                usuarioActual = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

                parametros.Add(new KeyValuePair<string, object>("RMT_CONT", rmt));

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
               

                reporte = this.Report(
                           ReportFormat.Excel,
                           ConfiguracionesGlobales.ReportPathCarteraContratos,
                           ConfiguracionesGlobales.ReportesReportServerUrl,
                           parametros,
                           ConfiguracionesGlobales.ReportesUsername,
                           ConfiguracionesGlobales.ReportesPassword
                           );

                
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
                    msgError = string.IsNullOrEmpty(msg) ? MENSAJE : msg,
                    msgErrorException = MOSTRARTRACE.Equals("1") ?
                    $"{exception.Source}</br>{exception.Message}</br>{exception.StackTrace}" : string.Empty
                });
            }
        }
             
    }
}