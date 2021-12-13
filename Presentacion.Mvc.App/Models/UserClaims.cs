/*

    Author       :      j0HNn3LS0N r0DRIGU3Z.
    Create date  :      Abril 2017.
    Description  :      Manejo e información de identidad del usuario actual.

 */

using Aplicacion.Administracion.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Models
{
    public class UserClaims : Controller
    {
        public string Rol { get; set; }
        public string Document { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Actor { get; set; }
        public bool VerReporte { get; set; }

        private static IServicioAplicacionEsCadenaSupervision _servicioAplicacionEsCadenaSupervision;

        private static IServicioAplicacionEsCadenaSupervision ServicioAplicacionEsCadenaSupervision
        {
            get
            {
                return _servicioAplicacionEsCadenaSupervision ??
                       (_servicioAplicacionEsCadenaSupervision =
                           FabricaIoC.Resolver<IServicioAplicacionEsCadenaSupervision>());
            }
        }

        /// <summary>
        ///  Devuelve un objeto UserClaims.
        ///  El rol puede cambia dependiendo  asi es interno (6,10,11,12)
        ///  o si se encuentra dentro de la tabla de CS, [EM_FILTRO_ROL_VENTAS].
        ///  Si el rol se cambia a CS (9), el usuario puede ver el reporte.
        /// </summary>
        /// <param name="rol">Devuelve el rol asignado</param>
        /// <returns></returns>
        public static UserClaims GetUserClaims(ClaimsIdentity identity)
        {
            try
            {
                var UserClaims = new UserClaims();
                var claims = identity.Claims;

                UserClaims.Rol = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");
                UserClaims.UserName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                UserClaims.Name = claims.Where(c => c.Type == ClaimTypes.Actor).Select(c => c.Value).SingleOrDefault();

                UserClaims.Document = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                UserClaims.UserId = identity.FindFirst(ClaimTypes.UserData).Value;
                UserClaims.VerReporte = false;

                /// Si el rol (asesor, asesor colpatria || rol cadena supervision)
                if (!EsRolUsuariosInternos(UserClaims.Rol))
                {
                    UserClaims.VerReporte = true;

                    return UserClaims;
                }

                /// Si el usuario actual (rol:6,10,11,12) esta en la tabla EM_FILTRO_ROL_VENTAS, puede ver el reporte.
                var lista = ServicioAplicacionEsCadenaSupervision.EsCadenaSupervision(UserClaims.Document);

                if (lista.Any())
                {
                    UserClaims.Rol = RolesCadenaSupervision.First();

                    UserClaims.VerReporte = true;

                    return UserClaims;
                }

                UserClaims.VerReporte = false;

                return UserClaims;
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene un objeto que representa la identidad del usuario actual
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static UserClaims GetCurrentUserClaims(ClaimsIdentity identity)
        {
            try
            {
                var UserClaims = new UserClaims();
                var claims = identity.Claims;

                UserClaims.Rol = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");
                UserClaims.UserName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                UserClaims.Name = claims.Where(c => c.Type == ClaimTypes.Actor).Select(c => c.Value).SingleOrDefault();

                UserClaims.Document = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                UserClaims.UserId = identity.FindFirst(ClaimTypes.UserData).Value;
                UserClaims.Actor = identity.FindFirst(ClaimTypes.Actor).Value;

                return UserClaims;
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene un objeto que representa la identidad del usuario actual,
        /// y se encuentra en la variable de sesion [userClaims]
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static UserClaims UserClaimsSession(ClaimsIdentity user)
        {
            if (System.Web.HttpContext.Current.Session["userClaims"] == null)
            {
                var userClaims = GetUserClaims(user);
                System.Web.HttpContext.Current.Session["userClaims"] = userClaims;
            }

            return System.Web.HttpContext.Current.Session["userClaims"] as UserClaims;
        }

        /// <summary>
        /// Al cerrar y/o iniciar sesión se deberia invocar este método.
        /// con el fin de setear la variable de sesión [userClaims]
        /// </summary>
        public static void ResetUserClaimsSession()
        {
            System.Web.HttpContext.Current.Session["userClaims"] = null;
        }

        public static List<string> RolesAsesor { get { return ConfiguracionesGlobales.RolAsesor.Split(',').ToList(); } }
        public static List<string> RolesDirector { get { return ConfiguracionesGlobales.RolDirector.Split(',').ToList(); } }
        public static List<string> RolesCadenaSupervision { get { return ConfiguracionesGlobales.RolCadenaSupervision.Split(',').ToList(); } }
        public static List<string> RolesUsuariosInternos { get { return ConfiguracionesGlobales.RolUsuariosInternos.Split(',').ToList(); } }

        public static bool EsRolDirector(string rol)
        {
            return ConfiguracionesGlobales.RolDirector.Split(',').Any(r => r == rol);
        }

        public static bool ValidarRolDirector(string[] roles)
        {
            return ConfiguracionesGlobales.RolDirector.Split(',').Any(r => roles.Contains(r));
        }

        public static bool ValidarRolAsesor(string[] roles)
        {
            return ConfiguracionesGlobales.RolAsesor.Split(',').Any(r => roles.Contains(r));
        }

        public static bool EsRolAsesor(string rol)
        {
            return ConfiguracionesGlobales.RolAsesor.Split(',').Any(r => r == rol);
        }

        public static bool EsRolCadenaSupervision(string rol)
        {
            return ConfiguracionesGlobales.RolCadenaSupervision.Split(',').Any(r => r == rol);
        }

        public static bool ValidarRolCadenaSupervision(string[] roles)
        {
            return ConfiguracionesGlobales.RolCadenaSupervision.Split(',').Any(r => roles.Contains(r));
        }

        public static bool EsRolUsuariosInternos(string rol)
        {
            return ConfiguracionesGlobales.RolUsuariosInternos.Split(',').Any(r => r == rol);
        }
    }
}