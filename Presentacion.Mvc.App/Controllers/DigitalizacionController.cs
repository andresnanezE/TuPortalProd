using Aplicacion.Administracion.Contratos;
using AutoMapper;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Dominio.Administracion.Entidades.ModeloPortal;
using Newtonsoft.Json;
using Presentacion.Mvc.App.Models;
using Presentacion.Mvc.App.Models.Enum;
using Presentacion.Mvc.App.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class DigitalizacionController : Controller
    {
        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioCotizacion _servicioCotizacion;
        private IServicioAplicacionUsuarios _servicioUsuarios;

        private IServicioAplicacionRoles ServicioAplicacionRoles
        {
            get { return _servicioAplicacionRoles ?? (_servicioAplicacionRoles = FabricaIoC.Resolver<IServicioAplicacionRoles>()); }
        }

        private IServicioCotizacion ServicioCotizacion
        {
            get { return _servicioCotizacion ?? (_servicioCotizacion = FabricaIoC.Resolver<IServicioCotizacion>()); }
        }

        private IServicioAplicacionUsuarios ServicioUsuarios
        {
            get { return _servicioUsuarios ?? (_servicioUsuarios = FabricaIoC.Resolver<IServicioAplicacionUsuarios>()); }
        }

        public ActionResult Index()
        {
            UserClaims.ResetUserClaimsSession();

            try
            {
                ViewBag.IsAsesor = false;
                ViewBag.IsSac = false;
                ViewBag.IsDirector = false;
                ViewBag.VisualizerBtn = false;

                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.FindFirst(ClaimTypes.Role).Value;
                var lstRoles = roles.TrimEnd(',').Split(',');

                if (!(lstRoles.Any(x => x == "8") || lstRoles.Any(x => x == "20") || lstRoles.Any(x => x == "7")))
                    return RedirectToAction("Index", "Home");

                if (lstRoles.Any(x => x == "7"))
                {
                    ViewBag.IsDirector = true;
                }
                if (lstRoles.Any(x => x == "8"))
                {
                    ViewBag.IsAsesor = true;
                    ViewBag.VisualizerBtn = true;
                }
                if (lstRoles.Any(x => x == "20"))
                {
                    ViewBag.IsSac = true;
                }

                return View();
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        public string ObtenerSolicitudesDigitalizacion(string fechaInicio, string fechaFin)
        {
            UserClaims.ResetUserClaimsSession();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = identity.FindFirst(ClaimTypes.UserData).Value;

                var roles = identity.FindFirst(ClaimTypes.Role).Value;
                var lstRoles = roles.Split(',');
                var documento = ServicioCotizacion.ObtenerCedulaAsesor(int.Parse(userId));

                if (lstRoles.Any(x => x == "8"))
                {
                    var solicitudes = ServicioCotizacion.ObtenerSolicitudesDigitalizacion(documento, userId, fechaInicio, fechaFin, false);
                    return JsonConvert.SerializeObject(solicitudes);
                }
                if (lstRoles.Any(x => x == "7"))
                {                    
                    var solicitudes = ServicioCotizacion.ObtenerSolicitudesDigitalizacion(documento, userId, fechaInicio, fechaFin, true);
                    return JsonConvert.SerializeObject(solicitudes);
                }
                else
                {
                    var solicitudes = ServicioCotizacion.ObtenerSolicitudesDigitalizacion(documento, "", fechaInicio, fechaFin, false);
                    return JsonConvert.SerializeObject(solicitudes);
                }
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerCedulaAsesor()
        {
            var noDocumentoAsesor = string.Empty;

            UserClaims.ResetUserClaimsSession();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                noDocumentoAsesor = ServicioCotizacion.ObtenerCedulaAsesor(userId);

                return JsonConvert.SerializeObject(noDocumentoAsesor);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerCausales(int IdEstado)
        {
            try
            {
                var causalesDigitalizacion = ServicioCotizacion.ObtenerCausales(IdEstado);
                return JsonConvert.SerializeObject(causalesDigitalizacion);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            var fileNameBd = "";
            var pathFile = "";
            long ans, ticks;
            var uniqueId = "";
            var infoFile = new List<InfoFile>();

            UserClaims.ResetUserClaimsSession();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var noDocumentoAsesor = ServicioCotizacion.ObtenerCedulaAsesor(userId);

                string path = ConfigurationManager.AppSettings["RutaSolicitudDigital"]
                            + noDocumentoAsesor
                            + "\\" + DateTime.Now.ToString("MM-dd-yyy") + "\\";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                HttpFileCollectionBase files = Request.Files;

                for (int i = 0; i < files.Count; i++)
                {
                    ticks = new DateTime().Ticks;
                    ans = DateTime.Now.Ticks - ticks;
                    uniqueId = ans.ToString("x");

                    HttpPostedFileBase file = files[i];

                    if (file != null)
                    {
                        string fileName = Path.GetFileName(file.FileName);

                        fileNameBd = uniqueId + "_" + fileName;
                        pathFile = path + fileNameBd;

                        infoFile.Add(new InfoFile
                        {
                            NombreOriginal = fileName,
                            NombreArchivo = fileNameBd,
                            ContentType = files[i].ContentType,
                            RutaArchivo = pathFile
                        });

                        file.SaveAs(pathFile);
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    }
                }
                return Json(infoFile);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }


        public string DeleteFiles(List<InfoFile> files)
        {
            UserClaims.ResetUserClaimsSession();
            try
            {
                foreach (var file in files)
                {

                    if (System.IO.File.Exists(file.NombreArchivo))
                        System.IO.File.Delete(file.NombreArchivo);
                }

                return "Ok";
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace;
            }
        }

        [HttpGet]
        public ActionResult DownloadFiles(string rutaArchivo, string nombreOriginal)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(rutaArchivo);
            string fileName = nombreOriginal;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public ActionResult RegistrarSolicitud(InfoSolicitud info)
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            UserClaims.ResetUserClaimsSession();

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var result = ServicioCotizacion.RegistrarSolicitudDigitalizacion(info, userId);

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json(new Respuesta { 
                    Exitoso = false,
                    Mensaje = "Error al procesar la solicitud."
                });
            }
        }

        [HttpPost]
        public ActionResult EditarSolicitud(InfoSolicitud info)
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            UserClaims.ResetUserClaimsSession();

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                if (info.FileInfoEliminar != null && info.FileInfoEliminar.Any())
                {
                    var respuestaEliminarFiles = DeleteFiles(info.FileInfoEliminar);
                    if (respuestaEliminarFiles != "Ok")
                        return Json(respuestaEliminarFiles);
                }

                info.EstadoId = (int)EstadosEnum.Actualizado;
                var result = ServicioCotizacion.EditarSolicitudDigitalizacion(info, userId);

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }
        [HttpPost]
        public ActionResult GestionarSolicitudDigitalizacion(InfoSolicitud info)
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            UserClaims.ResetUserClaimsSession();

            try
            {

                if (info.FileInfoEliminar != null && info.FileInfoEliminar.Any())
                {
                    var respuestaEliminarFiles = DeleteFiles(info.FileInfoEliminar);
                    if (respuestaEliminarFiles != "Ok")
                        return Json(respuestaEliminarFiles);
                }

                info.EstadoId = info.Rechazar ? (int)EstadosEnum.Rechazado : (int)EstadosEnum.Radicado;
                var result = ServicioCotizacion.EditarSolicitudDigitalizacion(info, info.IdUsuario);

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        private static void RegistrarErrorCargue(Exception ex)
        {

            long ans, ticks;
            var uniqueId = "";

            ticks = new DateTime().Ticks;
            ans = DateTime.Now.Ticks - ticks;
            uniqueId = ans.ToString("x");

            string filePath = ConfigurationManager.AppSettings["ErrorDigital"] + uniqueId + "_ErrorDIGITAL.txt";
            if (!Directory.Exists(ConfigurationManager.AppSettings["ErrorDigital"]))
                Directory.CreateDirectory(ConfigurationManager.AppSettings["ErrorDigital"]);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();

                while (ex != null)
                {
                    writer.WriteLine(ex.GetType().FullName);
                    writer.WriteLine("Message : " + ex.InnerException);
                    writer.WriteLine("Message : " + ex.Message);
                    writer.WriteLine("StackTrace : " + ex.StackTrace);

                    ex = ex.InnerException;
                }
            }
        }

        [HttpGet]
        public string ObtenerCausalesSolicitud(int idSolicitud)
        {
            try
            {
                var resultado = ServicioCotizacion.ObtenerCausalesSolicitud(idSolicitud);
                return JsonConvert.SerializeObject(resultado);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerHistoricoSolicitud(int idSolicitud)
        {
            try
            {
                var resultado = ServicioCotizacion.ObtenerHistoricoSolicitud(idSolicitud);
                return JsonConvert.SerializeObject(resultado);
            }catch(Exception ex)
            {
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }
        [HttpGet]
        public string VerificarContrato(string numeroContrato)
        {
            try
            {
                var resultado = ServicioCotizacion.VerificarContrato(numeroContrato);
                return JsonConvert.SerializeObject(resultado);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }
    }
}