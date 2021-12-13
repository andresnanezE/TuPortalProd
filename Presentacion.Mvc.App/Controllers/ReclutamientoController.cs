using System;
using System.IO;
using System.Web;
using System.Net;
using System.Web.Mvc;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Configuration;
using System.Collections.Generic;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion.IoC;
using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Reclutamiento;
using System.Security.Claims;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using System.Linq;
using MvcReportViewer;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReclutamientoController : Controller
    {
        private IServicioReclutamiento _servicioReclutamiento;
        private IServicioAplicacionUsuarios _servicioUsuarios;

        private IServicioReclutamiento ServicioReclutamiento
        {
            get { return _servicioReclutamiento ?? (_servicioReclutamiento = FabricaIoC.Resolver<IServicioReclutamiento>()); }
        }

        private IServicioAplicacionUsuarios ServicioUsuarios
        {
            get { return _servicioUsuarios ?? (_servicioUsuarios = FabricaIoC.Resolver<IServicioAplicacionUsuarios>()); }
        }

        #region Views

        public ActionResult Index()
        {
            ViewBag.ActivateCaptcha = ConfigurationManager.AppSettings["ActivateCaptcha"];
            ViewBag.PublicSecurityKey = ConfigurationManager.AppSettings["PublicSecurityKey"];
            return View();
        }

        public ActionResult ProcesoRegistro(int nDoc)
        {
            try
            {
                var ingreso = ServicioReclutamiento.ValidarIngresoProspecto(nDoc, true);

                if (!ingreso)
                    return RedirectToAction("Index", "Reclutamiento");
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
            }

            return View();
        }

        public ActionResult Solicitudes()
        {
            return View();
        }

        public ActionResult Capacitacion()
        {
            return View();
        }

        public ActionResult Contratacion()
        {
            return View();
        }

        public ActionResult Sucursales()
        {
            return View();
        }

        public ActionResult Reclutadores()
        {
            return View();
        }

        public ActionResult Capacitadores()
        {
            return View();
        }

        public ActionResult Reportes()
        {
            return View();
        }

        public ActionResult ReporteDirector()
        {
            return View();
        }

        public ActionResult PasswordReset(int nDoc, string rcvr)
        {
            ViewBag.ActivateCaptcha = ConfigurationManager.AppSettings["ActivateCaptcha"];
            ViewBag.PublicSecurityKey = ConfigurationManager.AppSettings["PublicSecurityKey"];

            var recovery = ServicioReclutamiento.ValidarRecovery(nDoc, rcvr);

            if (!recovery)
                return RedirectToAction("Index", "Reclutamiento");

            return View();
        }

        #endregion

        #region Administrador

        [HttpGet]
        public string ObtenerSucursales(int? id)
        {
            try
            {
                var sucursales = ServicioReclutamiento.ObtenerSucursales(id);
                return JsonConvert.SerializeObject(sucursales);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerReclutadores(int? id)
        {
            try
            {
                var reclutadores = ServicioReclutamiento.ObtenerReclutadores(id);
                return JsonConvert.SerializeObject(reclutadores);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult GestionarSucursal(int id, string nombre, bool activo, string tipo)
        {
            try
            {
                var result = ServicioReclutamiento.GestionarSucursal(id, nombre, activo, tipo);

                if (!result)
                    return Json(false);

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpPost]
        public ActionResult GestionarReclutamiento(int id, int? idUsr, string nombres, string apellidos, int? ciudadId, string[] directores, string tipo)
        {
            try
            {
                var result = ServicioReclutamiento.GestionarReclutadores(id, idUsr, nombres, apellidos, ciudadId, directores, tipo);

                if (!result)
                    return Json(false);

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpGet]
        public string ObtenerReclutadorPorId(int id)
        {
            try
            {
                var result = ServicioReclutamiento.ObtenerReclutadorPorId(id);
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerCapacitadoresAdmin(int? id)
        {
            try
            {
                var sucursales = ServicioReclutamiento.ObtenerCapacitadores(id);
                return JsonConvert.SerializeObject(sucursales);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult GestionarCapacitador(int id, int idUsr, string nombres, string apellidos, bool activo, string tipo)
        {
            try
            {
                var result = ServicioReclutamiento.GestionarCapacitador(id, idUsr, nombres, apellidos, activo, tipo);

                if (!result)
                    return Json(false);

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpPost]
        public ActionResult ExportarReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            List<KeyValuePair<string, object>> parametros = null;
            try
            {
                string handle = Guid.NewGuid().ToString();
                parametros = new List<KeyValuePair<string, object>>();

                parametros.Add(new KeyValuePair<string, object>("FECHA_INICIO", fechaInicio.ToString("s")));
                parametros.Add(new KeyValuePair<string, object>("FECHA_FIN", fechaFin.ToString("s")));

                FileStreamResult reporte = this.Report(
                ReportFormat.Excel,
                ConfigurationManager.AppSettings["ReporteAdministrador"],
                ConfiguracionesGlobales.ReportesReportServerUrl,
                parametros,
                ConfiguracionesGlobales.ReportesUsername,
                ConfiguracionesGlobales.ReportesPassword
                );

                string nomRte = ConfigurationManager.AppSettings["NReporteAdministrador"];
                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();

                return Json(new { FileGuid = handle, FileName = nomRte });
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        public ActionResult ExportarReporteDirector(DateTime fechaInicio, DateTime fechaFin)
        {
            List<KeyValuePair<string, object>> parametros = null;
            try
            {
                string handle = Guid.NewGuid().ToString();
                parametros = new List<KeyValuePair<string, object>>();

                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                parametros.Add(new KeyValuePair<string, object>("FECHA_INICIO", fechaInicio.ToString("s")));
                parametros.Add(new KeyValuePair<string, object>("FECHA_FIN", fechaFin.ToString("s")));
                parametros.Add(new KeyValuePair<string, object>("ID_DIRECTOR", userId));

                FileStreamResult reporte = this.Report(
                ReportFormat.Excel,
                ConfigurationManager.AppSettings["ReporteDirector"],
                ConfiguracionesGlobales.ReportesReportServerUrl,
                parametros,
                ConfiguracionesGlobales.ReportesUsername,
                ConfiguracionesGlobales.ReportesPassword
                );

                string nomRte = ConfigurationManager.AppSettings["NReporteDirector"];
                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();

                return Json(new { FileGuid = handle, FileName = nomRte });
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        #endregion

        #region Perfil Reclutamiento

        [HttpGet]
        public String ObtenerSolicitudes(int id, bool first, string proceso, string estado, string director, DateTime? fechaInicio, DateTime? fechaFin, string numeroDocumento)
        {
            UserClaims.ResetUserClaimsSession();
            try
            {
                var rolDescripcion = string.Empty;
                var identity = (ClaimsIdentity)User.Identity;

                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var roles = identity.FindFirst(ClaimTypes.Role).Value;
                var lstRoles = roles.TrimEnd(',').Split(',');

                if (first)
                    userId = id;

                var solicitudes = ServicioReclutamiento.ObtenerSolicitudesProspecto(userId, first, proceso, estado, director,fechaInicio, fechaFin, numeroDocumento);
                return JsonConvert.SerializeObject(solicitudes);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject("Error");
            }
        }

        [HttpGet]
        public String ObtenerContrato(int id)
        {

            UserClaims.ResetUserClaimsSession();
            try
            {
                var userId = id;

                var solicitudesContrato = ServicioReclutamiento.ObtenerGestionContratoProspecto(userId);
                return JsonConvert.SerializeObject(solicitudesContrato);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject("Error");
            }
        }

        [HttpGet]
        public string ObtenerNotasReclutamiento(Int64 numeroDocumento)
        {
            try
            {
                var notas = ServicioReclutamiento.ObtenerNotaReclutamiento(numeroDocumento);
                return JsonConvert.SerializeObject(notas);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult ActualizarSolicitud(GestionSolicitud gestion)
        {
            var tipoEmail = string.Empty;
            var email = new ReclutamientoEmail();

            UserClaims.ResetUserClaimsSession();
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;

            try
            {
                var result = ServicioReclutamiento.ActualizarSolicitudProspecto(gestion);
                var notas = ServicioReclutamiento.ObtenerNotaReclutamiento(gestion.NumeroDocumento);

                if (gestion.Estado == "Devuelto")
                {
                    tipoEmail = "Cargue";
                    email.Sujeto = "Notificación de solicitud";
                    email.Observaciones = gestion.Observaciones;
                }

                if (gestion.Estado == "Aprobado")
                {
                    tipoEmail = "Aprobacion";
                    email.Sujeto = "Notificación de aprobación";
                }

                if (result)
                {
                    email.Password = "";
                    email.Correo = notas.First().CorreoElectronico;
                    email.Nombres = notas.First().Nombres;
                    email.Apellidos = notas.First().Apellidos;
                    email.NumeroDocumento = gestion.NumeroDocumento;
                    email.Url = ConfigurationManager.AppSettings["UrlLoginReclutameinto"];

                    EnviarEmailCotizacion(tipoEmail, email);
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpPost]
        public ActionResult ExportarContrato(Int64 numeroIdentificacion)
        {
            List<KeyValuePair<string, object>> parametros = null;
            try
            {
                string handle = Guid.NewGuid().ToString();
                parametros = new List<KeyValuePair<string, object>>();
                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO_IDENTIFICACION", numeroIdentificacion));

                FileStreamResult reporte = this.Report(
                ReportFormat.PDF,
                ConfigurationManager.AppSettings["ReporteContrato"],
                ConfiguracionesGlobales.ReportesReportServerUrl,
                parametros,
                ConfiguracionesGlobales.ReportesUsername,
                ConfiguracionesGlobales.ReportesPassword
                );

                string nomRte = ConfigurationManager.AppSettings["NReporteContrato"];
                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();

                return Json(new { FileGuid = handle, FileName = nomRte });
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpPost]
        public ActionResult DatosSolicitud(Int64 numeroIdentificacion)
        {
            List<KeyValuePair<string, object>> parametros = null;
            try
            {
                string handle = Guid.NewGuid().ToString();
                parametros = new List<KeyValuePair<string, object>>();
                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO_IDENTIFICACION", numeroIdentificacion));

                FileStreamResult reporte = this.Report(
                ReportFormat.PDF,
                ConfigurationManager.AppSettings["ReporteDatosSolicitud"],
                ConfiguracionesGlobales.ReportesReportServerUrl,
                parametros,
                ConfiguracionesGlobales.ReportesUsername,
                ConfiguracionesGlobales.ReportesPassword
                );

                string nomRte = ConfigurationManager.AppSettings["NReporteDatosSolicitud"];
                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();

                return Json(new { FileGuid = handle, FileName = nomRte });
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpPost]
        public ActionResult ExportarInformeGestion(DateTime fechaInicio, DateTime fechaFin, int? director, int? usuario)
        {
            List<KeyValuePair<string, object>> parametros = null;
            UserClaims.ResetUserClaimsSession();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                string handle = Guid.NewGuid().ToString();
                parametros = new List<KeyValuePair<string, object>>();
                parametros.Add(new KeyValuePair<string, object>("FECHA_INICIO", fechaInicio.ToString("s")));
                parametros.Add(new KeyValuePair<string, object>("FECHA_FIN", fechaFin.ToString("s")));
                
                if(director != null)
                    parametros.Add(new KeyValuePair<string, object>("ID_DIRECTOR", director));
                if (usuario != null)
                    parametros.Add(new KeyValuePair<string, object>("USUARIO", usuario));

                parametros.Add(new KeyValuePair<string, object>("USUARIO_RECLUTADOR", userId.ToString()));

                FileStreamResult reporte = this.Report(
                ReportFormat.Excel,
                ConfigurationManager.AppSettings["ReporteInformeGestion"],
                ConfiguracionesGlobales.ReportesReportServerUrl,
                parametros,
                ConfiguracionesGlobales.ReportesUsername,
                ConfiguracionesGlobales.ReportesPassword
                );

                string nomRte = ConfigurationManager.AppSettings["NReporteInformeGestion"];
                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();

                return Json(new { FileGuid = handle, FileName = nomRte });
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        public ActionResult ActualizarSolicitudContrato(GestionSolicitudContrato gestion)
        {
            var tipoEmail = string.Empty;
            var email = new ReclutamientoEmail();

            UserClaims.ResetUserClaimsSession();
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;

            try
            {
                var result = ServicioReclutamiento.ActualizarSolicitudContrato(gestion);
                var notas = ServicioReclutamiento.ObtenerNotaReclutamiento(gestion.NumeroDocumento);

                if (gestion.Estado == "Devuelto")
                {
                    tipoEmail = "DevolucionRegistro";
                    email.Sujeto = "Notificación de solicitud";
                    email.Observaciones = gestion.Observaciones;
                }

                if (gestion.Estado == "Aprobado")
                {
                    tipoEmail = "AprobacionContrato";
                    email.Sujeto = "Notificación de aprobación";
                }

                if (result)
                {
                    email.Password = "";
                    email.Correo = notas.First().CorreoElectronico;
                    email.Nombres = notas.First().Nombres;
                    email.Apellidos = notas.First().Apellidos;
                    email.NumeroDocumento = gestion.NumeroDocumento;
                    email.Url = ConfigurationManager.AppSettings["UrlLoginReclutameinto"];

                    EnviarEmailCotizacion(tipoEmail, email);
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        #endregion

        #region Perfil Capacitador

        [HttpGet]
        public String ObtenerCapacitaciones(int id, bool fist)
        {
            UserClaims.ResetUserClaimsSession();
            try
            {
                string estado = string.Empty;
                var rolDescripcion = string.Empty;
                var identity = (ClaimsIdentity)User.Identity;

                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var roles = identity.FindFirst(ClaimTypes.Role).Value;
                var lstRoles = roles.TrimEnd(',').Split(',');


                if (lstRoles.Any(x => x == "32"))
                    estado = "Capacitación";

                if (fist)
                    userId = id;

                var solicitudes = ServicioReclutamiento.ObtenerSolicitudesCapacitacion(userId, estado, fist);
                return JsonConvert.SerializeObject(solicitudes);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject("Error");
            }
        }

        [HttpGet]
        public string ObtenerCapacitadores()
        {
            try
            {
                var capacitadores = ServicioReclutamiento.ObtenerCapacitadores();
                return JsonConvert.SerializeObject(capacitadores);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult ActualizarCapacitacion(GestionCapacitacion gestion)
        {
            var tipoEmail = string.Empty;
            var email = new ReclutamientoEmail();

            UserClaims.ResetUserClaimsSession();
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;

            try
            {
                var result = ServicioReclutamiento.ActualizarSolicitudCapacitacion(gestion);
                var notas = ServicioReclutamiento.ObtenerNotaReclutamiento(gestion.NumeroDocumento);
                var notaCapacitacion = ServicioReclutamiento.ObtenerNotasCapacitacion(gestion.NumeroDocumento);

                if (gestion.Estado == "Aprobado")
                {
                    tipoEmail = "CapacitaciónAprobada";
                    email.Sujeto = "Proceso de inducción comercial";
                }

                if (gestion.Estado == "Devuelto")
                {
                    tipoEmail = "CapacitaciónDevuelto";
                    email.Sujeto = "Capacitación Devuelta";
                }

                if (result)
                {
                    email.Password = "";
                    email.Correo = notas.First().CorreoElectronico;
                    email.Nombres = notas.First().Nombres;
                    email.Apellidos = notas.First().Apellidos;
                    email.NumeroDocumento = gestion.NumeroDocumento;
                    email.Url = ConfigurationManager.AppSettings["UrlLoginReclutameinto"];

                    EnviarEmailCotizacion(tipoEmail, email);
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpPost]
        public ActionResult ActualizarCapacitacionReclutador(CapacitacionReclutador gestion)
        {
            var tipoEmail = string.Empty;
            var email = new ReclutamientoEmail();

            UserClaims.ResetUserClaimsSession();
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;

            try
            {
                var result = ServicioReclutamiento.ActualizarCapacitacionReclutador(gestion);
                //var notas = ServicioReclutamiento.ObtenerNotaReclutamiento(gestion.NumeroDocumento);
                //var notaCapacitacion = ServicioReclutamiento.ObtenerNotasCapacitacion(gestion.NumeroDocumento);

                //if (gestion.Estado == "Aprobado")
                //{
                //    tipoEmail = "CapacitaciónAprobada";
                //    email.Sujeto = "Proceso de inducción comercial";
                //}

                //if (gestion.Estado == "Devuelto")
                //{
                //    tipoEmail = "CapacitaciónDevuelto";
                //    email.Sujeto = "Capacitación Devuelta";
                //}

                //if (result)
                //{
                //    email.Password = "";
                //    email.Correo = notas.First().CorreoElectronico;
                //    email.Nombres = notas.First().Nombres;
                //    email.Apellidos = notas.First().Apellidos;
                //    email.NumeroDocumento = gestion.NumeroDocumento;
                //    email.Url = ConfigurationManager.AppSettings["UrlLoginReclutameinto"];

                //    EnviarEmailCotizacion(tipoEmail, email);
                //}

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        #endregion

        #region Perfil Prospecto

        [HttpPost]
        public ActionResult RegistrarUsuario(RegistroReclutamiento reclutamiento)
        {
            var email = new ReclutamientoEmail();

            UserClaims.ResetUserClaimsSession();
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;

            try
            {
                var validacion = ServicioReclutamiento.ValidarListaRestrictiva(reclutamiento.NumeroCedula.ToString());

                if (validacion.ID_CONDICION == 1)
                {
                    var result = ServicioUsuarios.RegistrarUsuario(reclutamiento);

                    if (result.Item1 == "Success")
                    {
                        email.Password = result.Item2;
                        email.Sujeto = "Proceso Contratacion – Candidato Registrado como Asesor";
                        email.Correo = reclutamiento.Correo;
                        email.Nombres = reclutamiento.Nombres;
                        email.Apellidos = reclutamiento.Apellidos;
                        email.NumeroDocumento = reclutamiento.NumeroCedula;
                        email.Url = ConfigurationManager.AppSettings["UrlLoginReclutameinto"];

                        EnviarEmailCotizacion("Registro", email);
                    }

                    return Json(result.Item1 + "$$" + result.Item3);
                }
                else
                {
                    email.Sujeto = "Lista Restrictiva";
                    email.Correo = ConfigurationManager.AppSettings["CorreoListaRestrictiva"];
                    email.TipoIdentificacion = reclutamiento.TipoIdentificacion;
                    email.NumeroDocumento = reclutamiento.NumeroCedula;
                    email.Nombres = reclutamiento.Nombres;
                    email.Apellidos = reclutamiento.Apellidos;
                    email.CorreoProspecto = reclutamiento.Correo;
                    email.Telefono = reclutamiento.Telefono;
                    email.CiudadVinculacion = reclutamiento.CiudadVinculacion;
                    email.Gestionado = reclutamiento.Gestionado;

                    EnviarEmailCotizacion("ListaRestrictiva", email);

                    return Json("Restrictivo$$" + ConfigurationManager.AppSettings["CorreoListaRestrictiva"]);
                }
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpGet]
        public string ObtenerCiudadesReclutamiento()
        {
            try
            {
                var ciudades = ServicioReclutamiento.ObtenerCiudadReclutamiento();
                return JsonConvert.SerializeObject(ciudades);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerTipoIdentificacion()
        {
            try
            {
                var tipoIdentificacion = ServicioReclutamiento.ObtenerTipoIdentificacion();
                return JsonConvert.SerializeObject(tipoIdentificacion);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerReferidosPorCiudad(int ciudadId)
        {
            try
            {
                var referidos = ServicioReclutamiento.ObtenerReferidosPorCiudad(ciudadId);
                return JsonConvert.SerializeObject(referidos);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerDirectoresPorReferido(int referidoId)
        {
            try
            {
                var tipo = "";

                if (referidoId == 0)
                {
                    tipo = "Solicitud";
                    var identity = (ClaimsIdentity)User.Identity;
                    var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                    referidoId = userId;
                }

                var directores = ServicioReclutamiento.ObtenerDirectoresPorReferido(referidoId, tipo);
                return JsonConvert.SerializeObject(directores);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerDirectores()
        {
            try
            {
                var directores = ServicioReclutamiento.ObtenerDirectores();
                return JsonConvert.SerializeObject(directores);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerRepresentanteLegal()
        {
            try
            {
                var representante = ServicioReclutamiento.ObtenerRepresentanteLegal();
                return JsonConvert.SerializeObject(representante);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
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
            var infoFile = new List<FileReclutamiento>();

            UserClaims.ResetUserClaimsSession();
            try
            {
                var numeroDocumento = Request.QueryString["nd"];
                var estadoCargue = Request.QueryString["ea"];

                string path = ConfigurationManager.AppSettings["RutaReclutamiento"]
                            + numeroDocumento
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

                        infoFile.Add(new FileReclutamiento
                        {
                            NombreOriginal = fileName,
                            NombreArchivo = fileNameBd,
                            ContentType = files[i].ContentType,
                            RutaArchivo = pathFile,
                            Input = files.AllKeys[i]
                        });

                        file.SaveAs(pathFile);
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    }
                }

                var result = false;

                if (estadoCargue == "certf")
                    result = ServicioReclutamiento.ActualizarCertificacion(Int64.Parse(numeroDocumento), infoFile[0].NombreOriginal, infoFile[0].NombreArchivo, infoFile[0].RutaArchivo);
                else
                    result = ServicioReclutamiento.CargueReclutamiento(infoFile, int.Parse(numeroDocumento), int.Parse(estadoCargue));

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult UploadContrato()
        {
            var fileNameBd = "";
            var pathFile = "";
            long ans, ticks;
            var uniqueId = "";
            var infoFile = new List<FileReclutamiento>();

            UserClaims.ResetUserClaimsSession();
            try
            {
                var numeroDocumento = Request.QueryString["nd"];
                var estadoCargue = Request.QueryString["ea"];

                string path = ConfigurationManager.AppSettings["RutaReclutamiento"]
                            + numeroDocumento
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

                        infoFile.Add(new FileReclutamiento
                        {
                            NombreOriginal = fileName,
                            NombreArchivo = fileNameBd,
                            ContentType = files[i].ContentType,
                            RutaArchivo = pathFile,
                            Input = files.AllKeys[i]
                        });

                        file.SaveAs(pathFile);
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    }
                }

                var result = false;

                result = ServicioReclutamiento.CargueReclutamiento(infoFile, int.Parse(numeroDocumento), int.Parse(estadoCargue));

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerEstadoArchivo(int numeroDocumento)
        {
            try
            {
                var notas = ServicioReclutamiento.ObtenerEstadArchivoReclutamiento(numeroDocumento);
                return JsonConvert.SerializeObject(notas);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerNotasCapacitacion(int numeroDocumento)
        {
            try
            {
                var notas = ServicioReclutamiento.ObtenerNotasCapacitacion(numeroDocumento);
                return JsonConvert.SerializeObject(notas);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerProcesoReclutamiento(int numeroDocumento)
        {
            try
            {
                var procesos = ServicioReclutamiento.ObtenerProcesoNumeroIdentificacion(numeroDocumento);
                return JsonConvert.SerializeObject(procesos);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string IngresoProspecto(Int64 numeroDocumento, string pass)
        {
            try
            {
                var ingreso = ServicioReclutamiento.IngresoProspecto(numeroDocumento, pass);
                return JsonConvert.SerializeObject(ingreso);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ValidarIngresoProspecto(Int64 numeroDocumento, bool validate)
        {
            try
            {
                var ingreso = ServicioReclutamiento.ValidarIngresoProspecto(numeroDocumento, validate);

                return JsonConvert.SerializeObject(ingreso);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerPaisesReclutamiento()
        {
            try
            {
                var paises = ServicioReclutamiento.ObtenerPaisesReclutamiento();
                return JsonConvert.SerializeObject(paises);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerEpsReclutamiento()
        {
            try
            {
                var eps = ServicioReclutamiento.ObtenerEpsReclutamiento();
                return JsonConvert.SerializeObject(eps);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerPensionesReclutamiento()
        {
            try
            {
                var pensiones = ServicioReclutamiento.ObtenerPensionesReclutamiento();
                return JsonConvert.SerializeObject(pensiones);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerNivelEducativoReclutamiento()
        {
            try
            {
                var nivelEducativo = ServicioReclutamiento.ObtenerNivelEducativoReclutamiento();
                return JsonConvert.SerializeObject(nivelEducativo);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpGet]
        public string ObtenerProfesionesReclutamiento()
        {
            try
            {
                var profesiones = ServicioReclutamiento.ObtenerNivelEducativoReclutamiento();
                return JsonConvert.SerializeObject(profesiones);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult CompletarRegistro(ReclutamientoRegistro registro)
        {
            var email = new ReclutamientoEmail();

            UserClaims.ResetUserClaimsSession();
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;

            try
            {
                var result = ServicioReclutamiento.CompletarRegistro(registro);
                var notas = ServicioReclutamiento.ObtenerNotaReclutamiento(registro.NumeroDocumento);

                if (result)
                {
                    email.Password = "";
                    email.Correo = notas.First().CorreoElectronico;
                    email.Nombres = notas.First().Nombres;
                    email.Apellidos = notas.First().Apellidos;
                    email.NumeroDocumento = registro.NumeroDocumento;
                    email.Url = ConfigurationManager.AppSettings["UrlLoginReclutameinto"];
                    email.Sujeto = "Finalización de Registro";

                    EnviarEmailCotizacion("ProcesoContratacion", email);
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpPost]
        public ActionResult ActualizarRegistro(ReclutamientoRegistro registro)
        {
            var email = new ReclutamientoEmail();

            UserClaims.ResetUserClaimsSession();
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;

            try
            {
                var result = ServicioReclutamiento.ActualizarRegistro(registro);
                var notas = ServicioReclutamiento.ObtenerNotaReclutamiento(registro.NumeroDocumento);

                if (result)
                {
                    email.Password = "";
                    email.Correo = notas.First().CorreoElectronico;
                    email.Nombres = notas.First().Nombres;
                    email.Apellidos = notas.First().Apellidos;
                    email.NumeroDocumento = registro.NumeroDocumento;
                    email.Url = ConfigurationManager.AppSettings["UrlLoginReclutameinto"];
                    email.Sujeto = "Actualizacion de Registro";

                    EnviarEmailCotizacion("ActualizacionContratacion", email);
                }

                return Json(true);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpGet]
        public string RecuperarContrasenia(int numeroDocumento)
        {
            var email = new ReclutamientoEmail();

            UserClaims.ResetUserClaimsSession();
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;

            try
            {
                var info = ServicioReclutamiento.ObtenerInfoReclutamiento(numeroDocumento, true);

                if (info.Count() == 0)
                    return JsonConvert.SerializeObject(info);

                if (info.First().UrlRecovery != "Process")
                {
                    email.Password = "";
                    email.Correo = info.First().CorreoElectronico;
                    email.Nombres = info.First().Nombres;
                    email.Apellidos = info.First().Apellidos;
                    email.NumeroDocumento = info.First().NumeroDocumento;
                    email.Url = ConfigurationManager.AppSettings["UrlRecoveryReclutameinto"]
                                + "?nDoc=" + info.First().NumeroDocumento
                                + "&rcvr=" + info.First().UrlRecovery;
                    email.Sujeto = "Cambio de contraseña";

                    EnviarEmailCotizacion("CambioContrasenia", email);
                }

                return JsonConvert.SerializeObject(info);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult ActualizarContrasenia(Int64 numeroDocumento, string password, string rcvr)
        {
            var email = new ReclutamientoEmail();

            try
            {
                var result = ServicioReclutamiento.ActualizarContrasenia(numeroDocumento, password, rcvr);
                var info = ServicioReclutamiento.ObtenerInfoReclutamiento(numeroDocumento, false);

                if (result)
                {
                    email.Password = "";
                    email.Correo = info.First().CorreoElectronico;
                    email.Nombres = info.First().Nombres;
                    email.Apellidos = info.First().Apellidos;
                    email.NumeroDocumento = info.First().NumeroDocumento;
                    email.Url = ConfigurationManager.AppSettings["UrlLoginReclutameinto"];
                    email.Sujeto = "Cambio de contraseña";

                    EnviarEmailCotizacion("RecoveryOk", email);
                }

                if (!result)
                    return Json(false);

                return Json(info);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        [HttpPost]
        public ActionResult GuardarTrazaDeclaro(Int64 numeroDocumento, string ip)
        {
            try
            {
                var result = ServicioReclutamiento.GuardarTrazaContrato(numeroDocumento, ip);

                if (!result)
                    return Json(false);

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }
        #endregion

        [HttpGet]
        public string ObtenerInfoCompletaReclutamiento(Int64 numeroDocumento)
        {
            try
            {
                var info = ServicioReclutamiento.ObtenerInformacionCompleta(numeroDocumento);
                return JsonConvert.SerializeObject(info);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject(ex.InnerException + "###" + ex.Message + "###" + ex.StackTrace);
            }
        }

        #region Perfil Reclutamiento

        [HttpGet]
        public String ObtenerContrataciones(int id, bool fist)
        {

            UserClaims.ResetUserClaimsSession();
            try
            {
                string estado = string.Empty;
                var rolDescripcion = string.Empty;
                var identity = (ClaimsIdentity)User.Identity;

                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var roles = identity.FindFirst(ClaimTypes.Role).Value;
                var lstRoles = roles.TrimEnd(',').Split(',');

                if (lstRoles.Any(x => x == "33"))
                    estado = "Proceso Contratación";

                if (fist)
                    userId = id;

                var solicitudes = ServicioReclutamiento.ObtenerProcesoContrataciones(userId, estado, fist);
                return JsonConvert.SerializeObject(solicitudes);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return JsonConvert.SerializeObject("Error");
            }
        }

        [HttpPost]
        public ActionResult ActualizarContratacion(GestionContratacion gestion)
        {
            var tipoEmail = string.Empty;
            var email = new ReclutamientoEmail();

            UserClaims.ResetUserClaimsSession();
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;

            try
            {
                var result = ServicioReclutamiento.ActualizarSolicitudContratacion(gestion);
                var notas = ServicioReclutamiento.ObtenerNotaReclutamiento(gestion.NumeroDocumento);

                if (gestion.Estado == "Devuelto")
                {
                    tipoEmail = "ContratacionDevuelto";
                    email.Sujeto = "Notificación de contratación";
                }

                if (gestion.Estado == "Aprobado")
                {
                    tipoEmail = "ContratacionAprobado";
                    email.Sujeto = "Notificación de contratación";
                }

                if (result)
                {
                    email.Nombres = notas.First().Nombres;
                    email.Apellidos = notas.First().Apellidos;
                    email.NumeroDocumento = gestion.NumeroDocumento;
                    email.Correo = notas.First().CorreoElectronico;
                    email.Url = ConfigurationManager.AppSettings["UrlLoginReclutameinto"];

                    EnviarEmailCotizacion(tipoEmail, email);
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
                return Json("Error");
            }
        }

        #endregion

        public void EnviarEmailCotizacion(string tipo, ReclutamientoEmail rEmail)
        {
            try
            {
                var nombrePlantilla = string.Empty;

                if (tipo == "Registro")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP1"];

                if (tipo == "Cargue")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP2"];

                if (tipo == "Aprobacion")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP3"];

                if (tipo == "CapacitaciónAprobada")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP4"];

                if (tipo == "Finalizada")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP5"];

                if (tipo == "ProcesoContratacion")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP6"];

                if (tipo == "ContratacionAprobado")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP7"];

                if (tipo == "ContratacionDevuelto")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP8"];

                if (tipo == "CambioContrasenia")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP9"];

                if (tipo == "RecoveryOk")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP10"];

                if (tipo == "ListaRestrictiva")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP11"];

                if (tipo == "AprobacionContrato")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP12"];

                if (tipo == "DevolucionRegistro")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP13"];

                if (tipo == "ActualizacionContratacion")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP14"];

                if (tipo == "CapacitaciónDevuelto")
                    nombrePlantilla = ConfigurationManager.AppSettings["ReclutamientoP15"];


                var aplicacion = string.Format(@"{0}\{1}", ConfiguracionesGlobales.AplicacionWsUsuarios, "Reclutamiento");
                var rutaPlantillas = ConfigurationManager.AppSettings["RutaPlantillasEnvioCorreos"];
                var rutaPlantillaEmail = string.Format(@"{0}\{1}\{2}", rutaPlantillas, aplicacion, nombrePlantilla);

                string body = Transversales.Administracion.Plantillas.GestionPlantillas.ObtenerHtmlPlantilla(rutaPlantillaEmail);
                if (!string.IsNullOrWhiteSpace(body))
                {
                    var nombresApellidos = rEmail.Nombres + ' ' + rEmail.Apellidos;

                    if (tipo == "Registro")
                    {
                        body = body.Replace("@@nombresapellidos@@", nombresApellidos);
                        body = body.Replace("@@usuario@@", rEmail.NumeroDocumento.ToString());
                        body = body.Replace("@@pass@@", rEmail.Password);
                        body = body.Replace("@@url@@", rEmail.Url);
                    }

                    if (tipo == "Cargue"
                      || tipo == "DevolucionRegistro")
                    {
                        body = body.Replace("@@nombresapellidos@@", nombresApellidos);
                        body = body.Replace("@@observaciones@@", rEmail.Observaciones);
                        body = body.Replace("@@url@@", rEmail.Url);
                    }

                    if (tipo == "Aprobacion"
                        || tipo == "AprobacionContrato"
                        || tipo == "ContratacionAprobado" || tipo == "ContratacionDevuelto"
                        || tipo == "ProcesoContratacion"
                        || tipo == "ActualizacionContratacion"
                        || tipo == "CapacitaciónDevuelto"
                        || tipo == "CapacitaciónAprobada")
                    {
                        body = body.Replace("@@nombresapellidos@@", nombresApellidos);
                        body = body.Replace("@@url@@", rEmail.Url);
                    }

                    if (tipo == "CambioContrasenia")
                    {
                        body = body.Replace("@@nombresapellidos@@", nombresApellidos);
                        body = body.Replace("@@cedula@@", rEmail.NumeroDocumento.ToString());
                        body = body.Replace("@@url@@", rEmail.Url);
                    }

                    if (tipo == "RecoveryOk")
                    {
                        body = body.Replace("@@nombresapellidos@@", nombresApellidos);
                        body = body.Replace("@@cedula@@", rEmail.NumeroDocumento.ToString());
                        body = body.Replace("@@url@@", rEmail.Url);
                    }

                    if (tipo == "ListaRestrictiva")
                    {
                        body = body.Replace("@@rg1@@", rEmail.TipoIdentificacion);
                        body = body.Replace("@@rg2@@", rEmail.NumeroDocumento.ToString());
                        body = body.Replace("@@rg3@@", rEmail.Nombres);
                        body = body.Replace("@@rg4@@", rEmail.Apellidos);
                        body = body.Replace("@@rg5@@", rEmail.CorreoProspecto);
                        body = body.Replace("@@rg6@@", rEmail.Telefono.ToString());
                        body = body.Replace("@@rg7@@", rEmail.CiudadVinculacion);
                        body = body.Replace("@@rg8@@", rEmail.Gestionado);
                    }
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtp = new SmtpClient
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Host = ConfigurationManager.AppSettings["SmtpHost"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]),
                    Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpTimeout"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SmtpCorreo"], ConfigurationManager.AppSettings["SmtpPassword"])
                };

                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SmtpCorreo"], ConfigurationManager.AppSettings["NombreFromNameEmail"]);
                MailAddress to = new MailAddress(rEmail.Correo, "ToName");
                MailMessage myMail = new MailMessage(from, to);

                myMail.Body = body;
                myMail.IsBodyHtml = true;
                myMail.Subject = rEmail.Sujeto;
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                if (tipo == "ContratacionAprobado")
                {
                    List<KeyValuePair<string, object>> parametros = null;
                    string handle = Guid.NewGuid().ToString();
                    parametros = new List<KeyValuePair<string, object>>();
                    parametros.Add(new KeyValuePair<string, object>("DOCUMENTO_IDENTIFICACION", rEmail.NumeroDocumento));

                    FileStreamResult reporte = this.Report(
                    ReportFormat.PDF,
                    ConfigurationManager.AppSettings["ReporteContrato"],
                    ConfiguracionesGlobales.ReportesReportServerUrl,
                    parametros,
                    ConfiguracionesGlobales.ReportesUsername,
                    ConfiguracionesGlobales.ReportesPassword
                    );

                    string nomRte = ConfigurationManager.AppSettings["NReporteContrato"];
                    var byteArray = ((MemoryStream)reporte.FileStream).ToArray();
                    myMail.Attachments.Add(new Attachment(new MemoryStream(byteArray), nomRte));
                }


                smtp.Send(myMail);
            }
            catch (Exception ex)
            {
                RegistrarErrorCargue(ex);
            }
        }

        private static void RegistrarErrorCargue(Exception ex)
        {
            long ans, ticks;
            var uniqueId = "";

            ticks = new DateTime().Ticks;
            ans = DateTime.Now.Ticks - ticks;
            uniqueId = ans.ToString("x");

            string filePath = ConfigurationManager.AppSettings["ErrorDigital"] + uniqueId;

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            filePath = filePath + "_ErrorDIGITAL.txt";

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
    
    }
}