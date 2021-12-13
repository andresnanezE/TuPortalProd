using Aplicacion.Administracion.Contratos;
using Presentacion.Mvc.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Transversales.Administracion.IoC;


namespace Presentacion.Mvc.App.Controllers
{
    public class AdmonSesionEnVezDeController : Controller
    {
        #region fields
        private IServicioAplicacionSesionEnVezDe _servicioAplicacionSesionEnVezDe;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private IServicioAplicacionUsuarios _servicioAplicacionUsuarios;
        private IServicioAplicacionRoles _servicioAplicacionRoles;
        //private string script = "";
        #endregion

        #region instance properties
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
        #endregion

        // GET: SesionEnVezDe
        public ActionResult Index()
        {
            return View();
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
            var user = ServicioAplicacionUsuarios.ObtenerCredencialesUsuario(documento);
            var roles = ServicioAplicacionRoles.ObtenerRolUsuario(user.USUARIOID);
            var rolesIds = roles.Select(r => r.ROLID).ToList();
            var roleString = rolesIds.Aggregate(string.Empty, (current, rolId) => string.Format("{0}{1},", current, rolId));
            var identity = new ClaimsIdentity(new[]{
                                                        new Claim(ClaimTypes.UserData, user.USUARIOID.ToString()),
                                                        new Claim(ClaimTypes.Name, user.USUARIO),
                                                        new Claim(ClaimTypes.Email, user.CORREO),
                                                        new Claim(ClaimTypes.Actor, user.NOMBREUSUARIO),
                                                        new Claim(ClaimTypes.Role, roleString),
                                                        new Claim(ClaimTypes.NameIdentifier, user.DOCUMENTO),
                                                   },
                        "PortalComercialCookie");
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);
            
            var resultado = rolesIds.Any(rolId => rolId == 3) ? RedirectToAction("Index", "Home") : RedirectToAction("Index", "Home");
            return resultado;

        }

    }
}