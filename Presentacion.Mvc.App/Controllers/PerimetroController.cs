using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Presentacion.Mvc.App.Models;
using System;
using System.Security.Claims;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class PerimetroController : Controller
    {
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        public ActionResult Bogota()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroBogota,
                    ip = ip,
                    MenuId = (int)Menus.Bogota,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroBogota;
                log.ip = ip;
                log.MenuId = (int)Menus.Bogota;
                ServicioAplicacionLogs.AgregarLog(log);
            }
            return View();
        }

        public ActionResult Chia()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroChia,
                    ip = ip,
                    MenuId = (int)Menus.Chia,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroChia;
                log.ip = ip;
                log.MenuId = (int)Menus.Chia;
                ServicioAplicacionLogs.AgregarLog(log);
            }
            return View();
        }

        public ActionResult Soacha()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroSoacha,
                    ip = ip,
                    MenuId = (int)Menus.Soacha,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroSoacha;
                log.ip = ip;
                log.MenuId = (int)Menus.Soacha;
                ServicioAplicacionLogs.AgregarLog(log);
            }

            return View();
        }

        public ActionResult Medellin()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroMedellin,
                    ip = ip,
                    MenuId = (int)Menus.Medellin,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroMedellin;
                log.ip = ip;
                log.MenuId = (int)Menus.Medellin;
                ServicioAplicacionLogs.AgregarLog(log);
            }

            return View();
        }

        public ActionResult Cali()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroMedellin,
                    ip = ip,
                    MenuId = (int)Menus.Medellin,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroCali;
                log.ip = ip;
                log.MenuId = (int)Menus.Cali;
                ServicioAplicacionLogs.AgregarLog(log);
            }
            return View();
        }

        public ActionResult Neiva()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroNeiva,
                    ip = ip,
                    MenuId = (int)Menus.Neiva,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroNeiva;
                log.ip = ip;
                log.MenuId = (int)Menus.Neiva;
                ServicioAplicacionLogs.AgregarLog(log);
            }
            return View();
        }

        public ActionResult Villavicencio()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroVillavicencio,
                    ip = ip,
                    MenuId = (int)Menus.Villavicencio,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroVillavicencio;
                log.ip = ip;
                log.MenuId = (int)Menus.Villavicencio;
                ServicioAplicacionLogs.AgregarLog(log);
            }
            return View();
        }

        public ActionResult Bucaramanga()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroBucaramanga,
                    ip = ip,
                    MenuId = (int)Menus.Bucaramanga,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroBucaramanga;
                log.ip = ip;
                log.MenuId = (int)Menus.Bucaramanga;
                ServicioAplicacionLogs.AgregarLog(log);
            }
            return View();
        }

        public ActionResult Floridablanca()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroFloridablanca,
                    ip = ip,
                    MenuId = 25,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroFloridablanca;
                log.ip = ip;
                log.MenuId = 25;
                ServicioAplicacionLogs.AgregarLog(log);
            }
            return View();
        }

        public ActionResult Piedecuesta()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroPiedecuesta,
                    ip = ip,
                    MenuId = 26,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroPiedecuesta;
                log.ip = ip;
                log.MenuId = 26;
                ServicioAplicacionLogs.AgregarLog(log);
            }
            return View();
        }

        public ActionResult Giron()
        {
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
                    idTipoLog = (int)Consulta.ConsultaPerimetroPiedecuesta,
                    ip = ip,
                    MenuId = 26,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaPerimetroGirón;
                log.ip = ip;
                log.MenuId = 27;
                ServicioAplicacionLogs.AgregarLog(log);
            }
            return View();
        }
    }
}