// ----------------------------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion;
using Transversales.Administracion.IoC;
using System.Net;
using Newtonsoft.Json.Linq;
using Presentacion.Mvc.App.Helpers;
using Newtonsoft.Json;
using Aplicacion.Administracion.Dto;
using System.Web.Script.Serialization;

namespace Presentacion.Mvc.App.Controllers
{

    public class LoginController : Controller
    {
        #region Fields

        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioAplicacionUsuarios _servicioAplicacionUsuarios;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion

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
        #endregion

        #region Instance Methods
        public ActionResult Index(string msg)
        {
            
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;

            ViewBag.Mensaje = msg;
            UserClaims.ResetUserClaimsSession();
            
            
            return View();  

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
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            var ip1 = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            return status;

            #endregion

        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ActivateCaptcha = ConfiguracionesGlobales.ActivateCaptcha;

                    if (ViewBag.ActivateCaptcha == "1" && !ValidarCaptcha())
                    {
                        TempData["mostrarModal"] = true;
                        ModelState.AddModelError(string.Empty, "Por favor debe validar el componente reCaptcha");
                        return View(model);
                    }

                    #region Acceso a WS validacion Usuario
                    var ws = new Utilidades.WSUsuariosCentralizadosManager();
                    var responseMessage = ws.ValidarAcesoUsuario(model.Usuario, model.Contrasena);
                    UsuarioDto usu2 = new UsuarioDto();
                    string strResponse = responseMessage.Content.ReadAsStringAsync().Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        //el servidor validó la petición y respondio correctamente
                        usu2 = new JavaScriptSerializer().Deserialize<UsuarioDto>(strResponse);
                    }
                    else if (responseMessage.StatusCode == HttpStatusCode.RedirectMethod)
                    {
                        var urlSitio = ((HttpRequestWrapper)((HttpContextWrapper)HttpContext).Request).Url.OriginalString;
                        urlSitio = urlSitio.Replace("/Login/Index", "/Login/RestablecerClave");
                        ws.SolicitarRecuperacionDeClave(model.Usuario, urlSitio);
                        ModelState.AddModelError(string.Empty, "La clave ha caducado. En unos momentos le llegará un correo con la informacion para reestablecerla");
                        return View(model);
                    }
                    else
                    {
                        var res = new JavaScriptSerializer().Deserialize<WsRespuestaOperacionDto>(strResponse);
                        ModelState.AddModelError(string.Empty, res.Mensajes.FirstOrDefault());
                        return View(model);
                    }
                    #endregion

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

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud.");
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
                    #endregion

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
                    if (ViewBag.ActivateCaptcha == "1" && !ValidarCaptcha())
                    {
                        TempData["mostrarModal"] = true;
                        ModelState.AddModelError(string.Empty, "Por favor debe validar el componente reCaptcha");
                        return View(model);
                    }

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
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. intente de nuevo si el error persiste contacte al administrador.");
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
                    if (ViewBag.ActivateCaptcha == "1" && !ValidarCaptcha())
                    {
                        TempData["mostrarModal"] = true;
                        ModelState.AddModelError(string.Empty, "Por favor debe validar el componente reCaptcha");
                        return View(model);
                    }
                    var ws = new Utilidades.WSUsuariosCentralizadosManager();
                    var envioCorrecto = ws.SolicitarRecuperacionDeClave(model.Usuario, urlSitio);

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
                    ModelState.AddModelError(string.Empty, "Por favor debe validar el componente reCaptcha");
                    return View(model);
                }
                var ws = new Utilidades.WSUsuariosCentralizadosManager();
                var urlSitio = ((HttpRequestWrapper)((HttpContextWrapper)HttpContext).Request).Url.OriginalString;
                var queryString = HttpContext.Request.QueryString.ToString();
                urlSitio = urlSitio.Replace(queryString, string.Empty);

                urlSitio = urlSitio.Replace("/Login/RestablecerClave", "/Login/index");
                var cambioClave = ws.CambiarClave(model.Usuario, model.Clave, urlSitio);
                if (cambioClave)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Se ha presentado un error actualizando la clave");
            }
            return View(model);
        }
        #endregion
    }
}
