using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class ManualUsuarioController : Controller
    {
        #region Fields

        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion Fields

        #region Instance Properties

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        #endregion Instance Properties

        // GET: ManualUsuario
        public ActionResult Index()
        {
            string root = string.Empty;

            try
            {
                switch (obtRol())
                {
                    case "7":
                        root = "director";
                        break;

                    case "8":
                    case "13":
                        root = "asesor";
                        break;

                    case "9":
                        root = "gerente";
                        break;

                    case "11":
                        root = "administrativo";
                        break;
                    //case "6":
                    //    root = "administrador";
                    //    break;
                    case "10":
                        root = "contratos";
                        break;

                    default:
                        root = "";
                        break;
                }
                ViewBag.Mensaje = "No existe un manual para el usuario actual.";
                ViewBag.archivo = root;

                //agrega actividad log
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var identity = (ClaimsIdentity)HttpContext.User.Identity;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.AyudaManualUsuario;
                log.ip = ip;
                log.MenuId = (int)Menus.Manual_usuario;

                ServicioAplicacionLogs.AgregarLog(log);
                //

                return View();
            }
            catch (Exception)
            {
                return View("_Error");
            }
        }

        private string obtRol()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return
                claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");
        }

        private byte[] GetFile(string s)
        {
            using (System.IO.FileStream fs = System.IO.File.OpenRead(s))
            {
                byte[] data = new byte[fs.Length];
                int br = fs.Read(data, 0, data.Length);
                if (br != fs.Length)
                    throw new System.IO.IOException(s);
                return data;
            }
        }
    }
}