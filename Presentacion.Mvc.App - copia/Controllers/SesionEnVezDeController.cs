using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Transversales.Administracion.IoC;


namespace Presentacion.Mvc.App.Controllers
{
    public class SesionEnVezDeController : Controller
    {
        private IServicioAplicacionSesionEnVezDe _servicioAplicacionSesionEnVezDe;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private IServicioAplicacionUsuarios _servicioAplicacionUsuarios;
        private IServicioAplicacionRoles _servicioAplicacionRoles;

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }
        private IServicioAplicacionSesionEnVezDe ServicioAplicacionSesionEnVezDe
        {
            get
            {
                return _servicioAplicacionSesionEnVezDe ??
                       (_servicioAplicacionSesionEnVezDe =
                           FabricaIoC.Resolver<IServicioAplicacionSesionEnVezDe>());
            }
        }
        private IServicioAplicacionUsuarios ServicioAplicacionUsuarios
        {
            get
            {
                return _servicioAplicacionUsuarios ??
                       (_servicioAplicacionUsuarios = FabricaIoC.Resolver<IServicioAplicacionUsuarios>());
            }
        }
        private IServicioAplicacionRoles ServicioAplicacionRoles
        {
            get { return _servicioAplicacionRoles ?? (_servicioAplicacionRoles = FabricaIoC.Resolver<IServicioAplicacionRoles>()); }
        }

        public ActionResult Index()
        {
            var model = new SesionEnVezDeModel();
            model.Terminos = ServicioAplicacionSesionEnVezDe.CargaTerminosCondiciones();

            //actualiza log actividades
            var identity = (ClaimsIdentity)User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var log = new EmbLogActividadesDto
            {
                UsuarioId = usuarioIdentity,
                fecha = DateTime.Now,
                idTipoLog = (int)Administracion.Sesion_en_vez_de,
                ip = ip,
                MenuId = (int)Menus.Sesion_en_vez_de
            };
            ServicioAplicacionLogs.AgregarLog(log);
            //

            return View(model);
        }

        [HttpPost]
        public JsonResult validarDocumento(string documento)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var username = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            var usuario = ServicioAplicacionUsuarios.ObtenerUsuarioUserName(username);
            var Documento = ServicioAplicacionSesionEnVezDe.ValidarDocumento(documento, usuario.DOCUMENTO);

            return Json(Documento);
        }

        [HttpPost]
        public ActionResult iniciarSesion(string documento)
        {
            var identityPadre = (ClaimsIdentity)User.Identity;
            var documentoPadre = (identityPadre.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
            var nombrePadre = (identityPadre.FindFirst(ClaimTypes.Actor).Value).ToString();

            Session["esSesionEnVezDe"] = true;
            Session["idLogActividades"] = 0;

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
            var identity = new ClaimsIdentity(new[]{
                                                        new Claim(ClaimTypes.UserData, user.Id.ToString()),
                                                        new Claim(ClaimTypes.Name, user.logIn),
                                                        new Claim(ClaimTypes.Email, user.correo),
                                                        new Claim(ClaimTypes.Actor, user.Nombre),
                                                        new Claim(ClaimTypes.Role, roleString),
                                                        new Claim(ClaimTypes.NameIdentifier, user.documento),
                                                        new Claim(ClaimTypes.Anonymous, documentoPadre ),
                                                        new Claim(ClaimTypes.Authentication, nombrePadre )
                                                   },
                        "PortalComercialCookie");
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);
            //
            var resultado = rolesIds.Any(rolId => rolId == 2) ? RedirectToAction("Index", "Home") : RedirectToAction("Index", "Home");

            //actualiza log actividades
            var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var log = new EmbLogActividadesDto
            {
                UsuarioId = usuarioIdentity,
                fecha = DateTime.Now,
                idTipoLog = (int)Administracion.InicioSesionEnVezDe,
                ip = ip,
                MenuId = (int)Menus.Sesion_en_vez_de,
                DocUsuario1 = documentoPadre,
                NombreUsuario1 = nombrePadre,
                DocUsuario2 = user.documento,
                NombreUsuario2 = user.Nombre,
                FechaHoraIni = DateTime.Now,
                FechaHoraFin = DateTime.Now,
                TiempoSesion = TimeSpan.Zero,
                EsSesionEnVezDe = true
            };

            var resultLog = ServicioAplicacionLogs.AgregarLog(log);
            Session["idLogActividades"] = resultLog.idLog;
            //

            return resultado;
        }
    }
}