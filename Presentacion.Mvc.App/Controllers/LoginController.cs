// ----------------------------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Web.Mvc;
using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion.IoC;
using System.Net;
using Newtonsoft.Json.Linq;
using Dominio.Administracion.Entidades.MapperDto;
using System.Web.Script.Serialization;

#pragma warning disable CS0105 // The using directive for 'System.IO' appeared previously in this namespace
#pragma warning restore CS0105 // The using directive for 'System.IO' appeared previously in this namespace
#pragma warning disable CS0105 // The using directive for 'System.Diagnostics' appeared previously in this namespace
#pragma warning restore CS0105 // The using directive for 'System.Diagnostics' appeared previously in this namespace

using Dominio.Administracion.Entidades.ModeloCotizacion;
using SCI.GoogleAnalytic;

namespace Presentacion.Mvc.App.Controllers
{
    public class LoginController : Controller
    {
        #region Fields

        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioAplicacionUsuarios _servicioAplicacionUsuarios;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private IServicioCotizacion _servicioCotizacion;

        #endregion Fields

        #region Instance Properties

        private IServicioAplicacionRoles ServicioAplicacionRoles
        {
            get { return _servicioAplicacionRoles ?? (_servicioAplicacionRoles = FabricaIoC.Resolver<IServicioAplicacionRoles>()); }
        }

        private IServicioAplicacionUsuarios ServicioAplicacionUsuarios
        {
            get { return _servicioAplicacionUsuarios ?? (_servicioAplicacionUsuarios = FabricaIoC.Resolver<IServicioAplicacionUsuarios>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        private IServicioCotizacion ServicioCotizacion
        {
            get { return _servicioCotizacion ?? (_servicioCotizacion = FabricaIoC.Resolver<IServicioCotizacion>()); }
        }

        #endregion Instance Properties

        #region Instance Methods

        public ActionResult Index(string msg)
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;
            var activCapt = new LoginModel();
            ViewBag.Mensaje = msg;
            UserClaims.ResetUserClaimsSession();

            activCapt.banderaCaptcha = 0;
            if (Request.QueryString.AllKeys.Any())
            {
                var vFind = Request.QueryString.AllKeys.ToList().Find(s => s == "valores");
                if (!string.IsNullOrEmpty(vFind))
                {
                    string vLlave = Request.QueryString.Get(vFind);
                    if (!string.IsNullOrEmpty(vLlave))
                        return RedirectToAction("RestablecerClave", new { valores = vLlave });
                }
            }

            return View(activCapt);
        }

        /*public ActionResult Index()
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;

            UserClaims.ResetUserClaimsSession();
            return View();
        }*/

        private bool ValidarCaptcha()
        {
            #region validacion captcha

            var response = Request["g-recaptcha-response"];

            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            string secretKey = ConfiguracionesGlobales.SecurityKey;
            var client = new WebClient();

            var ip1 = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}", secretKey, response, ip1));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            return status;

            #endregion validacion captcha
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            try
            {
                ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
                if (ModelState.IsValid)
                {
                    string correoEncript = null;
                    string iniciaCorreoEncript = null;
                    string iniciaCorreo = null;
                    ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;

                    //if (ViewBag.ActivateCaptcha == "1" && model.banderaCaptcha >= 1 && !ValidarCaptcha())
                    //{
                    //    TempData["mostrarModal"] = true;
                    //    ModelState.AddModelError(string.Empty, "Por favor vuelva a digitar sus credenciales");
                    //    return View(model);
                    //}

                    #region Acceso a WS validación Usuario

                    var ws = new Utilidades.WSUsuariosCentralizadosManager();
                    var responseMessage = ws.ValidarAcesoUsuario(model.Usuario, model.Contrasena);
                    UsuarioDto usu2 = new UsuarioDto();
                    string strResponse = responseMessage.Content.ReadAsStringAsync().Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        //el servidor validó la petición y respondio correctamente
                        usu2 = new JavaScriptSerializer().Deserialize<UsuarioDto>(strResponse);
                        model.banderaCaptcha = 0;
                    }
                    else if (responseMessage.StatusCode == HttpStatusCode.RedirectMethod)
                    {
                        var correoUsr = ServicioAplicacionUsuarios.ObtenerCorreoUsr(model.Usuario);
                        if (string.IsNullOrEmpty(correoUsr))
                        {
                            correoEncript = "";
                        }
                        else
                        {
                            int charLocation = correoUsr.IndexOf("@", StringComparison.Ordinal);

                            if (charLocation > 0)
                            {
                                iniciaCorreo = correoUsr.Substring(0, charLocation);
                                iniciaCorreoEncript = iniciaCorreo.Replace(iniciaCorreo, "*****");

                                var result = correoUsr.Substring(correoUsr.LastIndexOf('@') - 2);

                                correoEncript = iniciaCorreoEncript + result;
                            }
                        }

                        var urlSitio = ((HttpRequestWrapper)((HttpContextWrapper)HttpContext).Request).Url.OriginalString;
                        urlSitio = urlSitio.Replace("/Login/Index", "/Login/RestablecerClave");
                        ws.SolicitarRecuperacionDeClave(model.Usuario, urlSitio);
                        ModelState.AddModelError(string.Empty, "La clave ha caducado. En unos momentos le llegará al correo " + correoEncript + " la informacion para reestablecerla");
                        return View(model);
                    }
                    else
                    {
                        var res = new JavaScriptSerializer().Deserialize<WsRespuestaOperacionDto>(strResponse);
                        ModelState.AddModelError(string.Empty, res.Mensajes.FirstOrDefault());
                        model.banderaCaptcha = model.banderaCaptcha + 1;
                        return View(model);
                    }

                    #endregion Acceso a WS validacion Usuario

                    var rolesIds = usu2.Roles;
                    var roleString = rolesIds.Aggregate(string.Empty, (current, rolId) => string.Format("{0}{1},", current, rolId));


                    var identity = new ClaimsIdentity(new[]
                                                      {
                                                          new Claim(ClaimTypes.UserData, usu2.Id.ToString()),
                                                          new Claim(ClaimTypes.Name, usu2.logIn),
                                                          new Claim(ClaimTypes.Email, usu2.correo),
                                                          new Claim(ClaimTypes.Actor, usu2.Nombre),
                                                          new Claim(ClaimTypes.Role, roleString),
                                                          new Claim(ClaimTypes.NameIdentifier, usu2.documento),
                                                          new Claim(ClaimTypes.Anonymous, ""),
                                                          new Claim(ClaimTypes.Authentication, "")
                                                      },
                        "PortalComercialCookie");

                    var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;

                    EmbLogActividadesDto log = new EmbLogActividadesDto();
                    log.UsuarioId = usu2.Id;
                    log.fecha = DateTime.Now;
                    log.idTipoLog = (int)Login.InicioSesion;
                    log.ip = ip;
                    log.MenuId = (int)Menus.SinMenu;

                    ServicioAplicacionLogs.AgregarLog(log);

                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;
                    authManager.SignIn(identity);
                    //TODO:nsarmiento REVISAR ESTE RELINK QUE SE CAMBIO
                    //return rolesIds.Any(rolId => rolId == 3) ? RedirectToAction("Index", "Home") : RedirectToAction("Index", "Home");

                    var infoGoogle = new InformacionGoogle();

                    var useragent = Request.UserAgent;
                    var dispositivo = Request.UserAgent.Contains("iPhone")
                              || Request.UserAgent.Contains("Windows Phone")
                              || Request.UserAgent.Contains("Android")
                              || Request.UserAgent.Contains("webOS")
                              || Request.UserAgent.Contains("iPad")
                              || Request.UserAgent.Contains("iPod")
                              || Request.UserAgent.Contains("BlackBerry") ? "Smartphone" : "PC";


                    infoGoogle.Ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                    infoGoogle.Dispositivo = dispositivo;
                    infoGoogle.Navegador = useragent;
                    infoGoogle.Category = "Login";
                    infoGoogle.Action = "Ingreso";
                    infoGoogle.Label = "Inicia sesión en TuPortal";

                    var ga = new GoogleAnalyticsController().RegistrarEventoGoogleAnalytic(infoGoogle);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                // Qué ha sucedido
                var mensaje = "Error message: " + ex.Message;

                // Información sobre la excepción interna
                if (ex.InnerException != null)
                {
                    mensaje = mensaje + " Inner exception: " + ex.InnerException.Message;
                }

                // Dónde ha sucedido
                mensaje = mensaje + " Stack trace: " + ex.StackTrace;

                ModelState.AddModelError(string.Empty, mensaje);
            }

            return View(model);
        }

        public ActionResult SignOut()
        {
            try
            {
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var documento = "";
                //sesion en vez de
                if (!identity.FindFirst(ClaimTypes.Authentication).Value.ToString().Equals(""))
                {
                    documento = identity.FindFirst(ClaimTypes.Anonymous).Value.ToString();
                }
                //

                if (documento.Equals(""))
                {
                    var log = new EmbLogActividadesDto();
                    log.UsuarioId = userId;
                    log.fecha = DateTime.Now;
                    log.idTipoLog = (int)Login.CierreSesion;
                    log.ip = ip;
                    log.MenuId = (int)Menus.SinMenu;
                    ServicioAplicacionLogs.AgregarLog(log);
                }
                else
                {
                    var log = new EmbLogActividadesDto();
                    log = ServicioAplicacionLogs.ObtenerObtenerLogSesionEnVezDe(int.Parse(Session["idLogActividades"].ToString()));
                    //log.UsuarioId = userId;
                    //log.fecha = DateTime.Now;
                    //log.idTipoLog = (int) Administracion.CerrarSesionEnVezDe;
                    //log.ip = ip;
                    //log.MenuId = (int)Menus.Sesion_en_vez_de;
                    //log.DocUsuario1 = documento;
                    //log.NombreUsuario1 = identity.FindFirst(ClaimTypes.Authentication).Value.ToString();
                    //log.DocUsuario2 =

                    log.FechaHoraFin = DateTime.Now;
                    log.TiempoSesion = TimeSpan.Parse((log.FechaHoraFin.Value - log.FechaHoraIni.Value).ToString());
                    ServicioAplicacionLogs.modificaLogSesionEnVezDe(log);

                    Session["esSesionEnVezDe"] = false;
                }

                UserClaims.ResetUserClaimsSession();
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignOut();

                if (!documento.Equals(""))
                {
                    var credenciales = ServicioAplicacionUsuarios.ObtenerCredencialesUsuario(documento);

                    #region Acceso a WS validacion Usuario

                    var ws = new Utilidades.WSUsuariosCentralizadosManager();
                    var responseMessage = ws.ValidarAcesoUsuario(credenciales.USUARIO, credenciales.CLAVE);
                    UsuarioDto user = new UsuarioDto();
                    string strResponse = responseMessage.Content.ReadAsStringAsync().Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        //el servidor validó la petición y respondio correctamente
                        user = new JavaScriptSerializer().Deserialize<UsuarioDto>(strResponse);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }

                    #endregion Acceso a WS validacion Usuario

                    var roles = ServicioAplicacionRoles.ObtenerRolUsuario(user.Id);
                    var rolesIds = roles.Select(r => r.ROLID).ToList();
                    var roleString = rolesIds.Aggregate(string.Empty, (current, rolId) => string.Format("{0}{1},", current, rolId));
                    identity = new ClaimsIdentity(new[]{
                                                        new Claim(ClaimTypes.UserData, user.Id.ToString()),
                                                        new Claim(ClaimTypes.Name, user.logIn),
                                                        new Claim(ClaimTypes.Email, user.correo),
                                                        new Claim(ClaimTypes.Actor, user.Nombre),
                                                        new Claim(ClaimTypes.Role, roleString),
                                                        new Claim(ClaimTypes.NameIdentifier, user.documento),
                                                        new Claim(ClaimTypes.Anonymous, "" ),
                                                        new Claim(ClaimTypes.Authentication, "" )
                                                   },
                                "PortalComercialCookie");

                    ctx = Request.GetOwinContext();
                    authManager = ctx.Authentication;
                    authManager.SignIn(identity);
                    var resultado = rolesIds.Any(rolId => rolId == 3) ? RedirectToAction("Index", "Home") : RedirectToAction("Index", "Home");
                    return resultado;
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }


            var infoGoogle = new InformacionGoogle();

            var useragent = Request.UserAgent;
            var dispositivo = Request.UserAgent.Contains("iPhone")
                      || Request.UserAgent.Contains("Windows Phone")
                      || Request.UserAgent.Contains("Android")
                      || Request.UserAgent.Contains("webOS")
                      || Request.UserAgent.Contains("iPad")
                      || Request.UserAgent.Contains("iPod")
                      || Request.UserAgent.Contains("BlackBerry") ? "Smartphone" : "PC";


            infoGoogle.Ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            infoGoogle.Dispositivo = dispositivo;
            infoGoogle.Navegador = useragent;
            infoGoogle.Category = "Cerrar Sesión";
            infoGoogle.Action = "Salida";
            infoGoogle.Label = "Cerrar sesión activa del usuario";

            var ga = new GoogleAnalyticsController().RegistrarEventoGoogleAnalytic(infoGoogle);

            return RedirectToAction("Index", "Login");
        }

        public ActionResult Registrate()
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;
            return View();
        }

        [HttpPost]
        public ActionResult Registrate(RegistroModel model)
        {
            
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;

            try
            {
                if (ModelState.IsValid)
                {
                    //if (ViewBag.ActivateCaptcha == "1" && !ValidarCaptcha())
                    //{
                    //    TempData["mostrarModal"] = true;
                    //    ModelState.AddModelError(string.Empty, "Por favor vuelva a digitar sus credenciales");
                    //    return View(model);
                    //}
                    
                    var user = ServicioAplicacionUsuarios.ValidarPermisoRegistro(model.Documento);
                    if (user != null)
                    {
                        
                        model.NombreUsuario = user.NOMBREUSUARIO; //una vez verificado que user sea empleado se trae el nombre
                        model.Rol = user.ROL;//user me trae el id del rol de la persona
                        model.TipoDocumento = "CC";//Se asigna siempre CC
                        var urlSitio = ((HttpRequestWrapper)((HttpContextWrapper)HttpContext).Request).Url.OriginalString;
                        var queryString = ((HttpRequestWrapper)HttpContext.Request).Path.ToString();
                        urlSitio = urlSitio.Replace("/Login/Registrate", "/Login/index");
                        
                        var ws = new Utilidades.WSUsuariosCentralizadosManager();
                        var responseMessage = ws.RegistrarUsuario(model, urlSitio);
                        string strResponse = responseMessage.Content.ReadAsStringAsync().Result;
                        
                        var resp = new JavaScriptSerializer().Deserialize<WsRespuestaOperacionDto>(strResponse);

                        if (responseMessage.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index", new { msg = "El usuario se ha registrado exitosamente, se ha enviado a su correo mayor información" });
                        }
                        else
                        {
                            
                            foreach (var ms in resp.Mensajes)
                            {
                                ModelState.AddModelError(string.Empty, ms);
                            }
                            //return View(model);
                        }
                    }
                    else
                    {
                        
                        ModelState.AddModelError(string.Empty, "Verifique el documento. El Documento ingresado no es de un asesor o funcionario de Emermedica");
                        //return View(model);
                    }
                }
            }
            catch (Exception e)
            {
                //ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. intente de nuevo si el error persiste contacte al administrador.");
                Utilidades.Utils.RegistrarLogWindows("", e);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult OlvideMiClave()
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;
            return View();
        }

        [HttpPost]
        public ActionResult OlvideMiClave(RecuperarContraseñaModel model)
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;

            var urlSitio = ((HttpRequestWrapper)((HttpContextWrapper)HttpContext).Request).Url.OriginalString;
            urlSitio = urlSitio.Replace("/Login/OlvideMiClave", "/Login/RestablecerClave");

            try
            {
                if (ModelState.IsValid)
                {
                    //if (ViewBag.ActivateCaptcha == "1" && !ValidarCaptcha())
                    //{
                    //    TempData["mostrarModal"] = true;
                    //    ModelState.AddModelError(string.Empty, "Por favor vuelva a digitar sus credenciales");
                    //    return View(model);
                    //}
                    var ws = new Utilidades.WSUsuariosCentralizadosManager();
                    //var envioCorrecto = ws.SolicitarRecuperacionDeClave(model.Usuario, urlSitio);
                    var envioCorrecto = ws.SolicitarRecuperacionDeClave(model.documento, urlSitio);
                    if (envioCorrecto)
                    {
                        //model.Mensaje = "Se ha enviado un correo electronico con la información para restablecer la contraseña";
                        return RedirectToAction("Index", new { msg = "Hemos enviado un correo electrónico con la información para restablecer tu contraseña." });
                        //model.Usuario = string.Empty;
                    }
                    else
                    {
                        ModelState.AddModelError("Name",
                            "<b> Se ha presentado un error.</b>" +
                            "<br>" +
                            "El nombre de usuario no existe. Verifica la información ingresada" +
                            "<br>" +
                            "Si el problema persiste comuniquese con un administrador."
                        );
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult RestablecerClave(string valores)
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;

            var vKeys = Request.Form.AllKeys;

            ViewBag.PermisoCambioContraseña = false;
            var ws = new Utilidades.WSUsuariosCentralizadosManager();
            string usuarioRespuesta = ws.ValidarSolicitudRestablecimiento(valores);
            var model = new RecuperarContraseñaModel();
            if (!string.IsNullOrWhiteSpace(usuarioRespuesta))
            {
                model.Usuario = usuarioRespuesta;
                ViewBag.PermisoCambioContraseña = true;
            }
            else
            {
                ModelState.AddModelError("Error", new Exception("El link ha caducado o es incorrecto. Por favor solicite el restablecimiento de nuevo"));
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult RestablecerClave(RecuperarContraseñaModel model)
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;

            ViewBag.PermisoCambioContraseña = true;
            if (ModelState.IsValid)
            {
                if (ViewBag.ActivateCaptcha == "1" && !ValidarCaptcha())
                {
                    TempData["mostrarModal"] = true;
                    ModelState.AddModelError(string.Empty, "Por favor vuelva a digitar sus credenciales");
                    return View(model);
                }
                var ws = new Utilidades.WSUsuariosCentralizadosManager();
                var urlSitio = ((HttpRequestWrapper)((HttpContextWrapper)HttpContext).Request).Url.OriginalString;
                var queryString = HttpContext.Request.QueryString.ToString();
                urlSitio = urlSitio.Replace(queryString, string.Empty);

                urlSitio = urlSitio.Replace("/Login/OlvideMiClave", "/Login/index");
                var cambioClave = ws.CambiarClave(model.Usuario, model.Clave, urlSitio);
                string strResponse = cambioClave.Content.ReadAsStringAsync().Result;
                var resp = new JavaScriptSerializer().Deserialize<WsRespuestaOperacionDto>(strResponse);

                if (cambioClave.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var ms in resp.Mensajes)
                    {
                        ModelState.AddModelError(string.Empty, ms);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Test()
        {
            var modelo = new RegistroVisitaModel();
            try
            {
                List<Factores> listFactores = new List<Factores>();

                listFactores = ServicioCotizacion.ObtenerFactores();
                modelo.factores_ = listFactores;

                return View(modelo);
            }
            catch (Exception ex)
            {
                return View(modelo.nombreAsesor = ex.Message.ToString());
            }
        }

        #endregion Instance Methods
    }
}