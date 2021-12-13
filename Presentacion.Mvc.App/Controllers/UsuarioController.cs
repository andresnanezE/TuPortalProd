// ----------------------------------------------------------------------------------------------
// <copyright file="UsuarioController.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;

//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using AutoMapper;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using PagedList;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Routing;
using Transversales.Administracion.IoC;

//using Dominio.Administracion.Entidades.MapperDto;

namespace Presentacion.Mvc.App.Controllers
{
    public class UsuarioController : Controller
    {
        #region Fields

        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioAplicacionTablasBase _servicioAplicacionTablasBase;
        private IServicioAplicacionUsuarios _servicioAplicacionUsuarios;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion Fields

        #region Instance Properties

        private IServicioAplicacionRoles ServicioAplicacionRoles
        {
            get { return _servicioAplicacionRoles ?? (_servicioAplicacionRoles = FabricaIoC.Resolver<IServicioAplicacionRoles>()); }
        }

        private IServicioAplicacionTablasBase ServicioAplicacionTablasBase
        {
            get { return _servicioAplicacionTablasBase ?? (_servicioAplicacionTablasBase = FabricaIoC.Resolver<IServicioAplicacionTablasBase>()); }
        }

        private IServicioAplicacionUsuarios ServicioAplicacionUsuarios
        {
            get { return _servicioAplicacionUsuarios ?? (_servicioAplicacionUsuarios = FabricaIoC.Resolver<IServicioAplicacionUsuarios>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        #endregion Instance Properties

        #region Instance Methods

        public ActionResult ActivarUsuario(int usuarioId = 0)
        {
            try
            {
                #region Acceso a WS validacion Usuario

                var u = ServicioAplicacionUsuarios.ObtenerUsuarioId(usuarioId);
                //var userName = ViewData["CurrentUser"] == null ? null : ViewData["CurrentUser"].ToString();
                var usrDto = new UsuarioDto()
                {
                    activo = true,
                    Id = u.USUARIOID,
                    correo = u.CORREO,
                    Nombre = u.NOMBREUSUARIO,
                };
                var ws = new Utilidades.WSUsuariosCentralizadosManager();
                var responseMessage = ws.ActualizarUsuario(usrDto);

                #endregion Acceso a WS validacion Usuario

                #region actualiza log actividades

                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.ActivaUsuario,
                    ip = ip,
                    MenuId = (int)Menus.Usuarios
                };
                ServicioAplicacionLogs.AgregarLog(log);

                #endregion actualiza log actividades
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return RedirectToAction("Index", new { usuarioId });
        }

        //public ActionResult Agregar()
        //{
        //    var model = new UsuarioModel();

        //    try
        //    {
        //        model.TipoDocumentos = ServicioAplicacionTablasBase.ObtenerTipoDocumentos();
        //        var roles = ServicioAplicacionRoles.ObtenerRolesActivos();
        //        Mapper.CreateMap<EMA_ROLDto, RolModel>().ReverseMap();
        //        var rolModel = Mapper.Map<List<RolModel>>(roles);
        //        model.ListadoRoles = rolModel;

        //        return View(model);
        //    }
        //    catch (Exception exception)
        //    {
        //        ModelState.AddModelError(string.Empty, exception.Message);
        //    }
        //    return View(model);
        ////}

        //[HttpPost]
        //public ActionResult Agregar(UsuarioModel model)
        //{
        //    try
        //    {
        //        model.TipoDocumentos = ServicioAplicacionTablasBase.ObtenerTipoDocumentos();
        //        var roles = ServicioAplicacionRoles.ObtenerRolesActivos();
        //        Mapper.CreateMap<EMA_ROLDto, RolModel>().ReverseMap();
        //        var rolModel = Mapper.Map<List<RolModel>>(roles);
        //        if (model.RolesIds != null)
        //        {
        //            foreach (var rol in from rol in rolModel from rolId in model.RolesIds.Where(rolId => rol.RolId == rolId) select rol)
        //            {
        //                rol.Seleccionado = true;
        //            }
        //        }
        //        model.ListadoRoles = rolModel;

        //        if (ModelState.IsValid)
        //        {
        //            if (string.IsNullOrEmpty(model.Usuario))
        //            {
        //                var usuarioExterno = ServicioAplicacionUsuarios.ObtenerUsuarioExterno(model.TipoDocumento, model.Documento);
        //                model.Documento = model.Documento;
        //                model.TipoDocumento = model.TipoDocumento;
        //                model.NombreUsuario = usuarioExterno.NombreUsuario;
        //                model.Usuario = usuarioExterno.Usuario;
        //                model.Contrasena = usuarioExterno.Contrasena;
        //            }
        //            var identity = (ClaimsIdentity)User.Identity;
        //            IEnumerable<Claim> claims = identity.Claims;
        //            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
        //            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
        //            var log = new EmbLogActividadesDto
        //            {
        //                UsuarioId = userId,
        //                fecha = DateTime.Now,
        //                idTipoLog = (int) Administracion.CreacionUsuarios,
        //                ip = ip,
        //                MenuId = (int)Menus.Usuarios
        //            };
        //            ServicioAplicacionLogs.AgregarLog(log);

        //            Mapper.CreateMap<EMA_USUARIODto, UsuarioModel>().ReverseMap();
        //            var usuario = Mapper.Map<EMA_USUARIODto>(model);
        //            var userName = ViewData["CurrentUser"] == null ? null : ViewData["CurrentUser"].ToString();
        //            usuario.FECHAMODIFICACION = null;
        //            usuario.FECHAREGISTRO = DateTime.Now;
        //            usuario.FECHAULTIMASESION = null;

        //            usuario.USUARIOCREACION = userName;
        //            var usuarioId = ServicioAplicacionUsuarios.AgregarUsuario(usuario, model.RolesIds);

        //            return RedirectToAction("Index", new
        //                                             {
        //                                                 usuarioId
        //                                             });
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        ModelState.AddModelError(string.Empty, exception.Message);
        //    }
        //    return View(model);
        //}

        public ActionResult DesactivarUsuario(int usuarioId = 0)
        {
            try
            {
                List<int> listRol = new List<int>();

                #region Acceso a WS validacion Usuario

                var u = ServicioAplicacionUsuarios.ObtenerUsuarioId(usuarioId);
                var ro = ServicioAplicacionUsuarios.ObtenerRolUsuario(usuarioId).ToList();
                foreach (var r in ro)
                {
                    listRol.Add(r.ROLID);
                }

                var usrDto = new UsuarioDto()
                {
                    activo = false,
                    Id = u.USUARIOID,
                    correo = u.CORREO,
                    Nombre = u.NOMBREUSUARIO,
                    Roles = listRol
                };
                var ws = new Utilidades.WSUsuariosCentralizadosManager();
                var responseMessage = ws.ActualizarUsuario(usrDto);

                #endregion Acceso a WS validacion Usuario

                #region actualiza log actividades

                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.ActivaUsuario,
                    ip = ip,
                    MenuId = (int)Menus.Usuarios
                };
                ServicioAplicacionLogs.AgregarLog(log);

                #endregion actualiza log actividades
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return RedirectToAction("Index", new { usuarioId });
        }

        public ActionResult Index(int usuarioId = 0)
        {
            try
            {
                var model = new UsuarioModel();
                if (usuarioId > 0)
                {
                    model.Mensaje = "Petición realizada exitosa";
                }
                var usuarios = ServicioAplicacionUsuarios.ObtenerUsuarios(usuarioId).ToPagedList(1, 20);
                var tipoDocumentos = ServicioAplicacionTablasBase.ObtenerTipoDocumentos();
                model.ListaUsuarios = usuarios;
                model.TipoDocumentos = tipoDocumentos;

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.Usuarios,
                    ip = ip,
                    MenuId = (int)Menus.Usuarios
                };
                ServicioAplicacionLogs.AgregarLog(log);
                //
                return View(model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(new UsuarioModel());
        }

        [HttpPost]
        public ActionResult Index(UsuarioModel model)
        {
            try
            {
                string tipDocu = "CC";

                model.TipoDocumentos = ServicioAplicacionTablasBase.ObtenerTipoDocumentos();
                model.ListaUsuarios = ServicioAplicacionUsuarios.ObtenerUsuarioFiltros(model.UsuarioFiltro, model.NombreUsuarioFiltro, tipDocu, model.DocumentoFiltro, model.CorreoFiltro).ToPagedList(1, 20);

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.BuscarUsuarios,
                    ip = ip,
                    MenuId = (int)Menus.Usuarios
                };
                ServicioAplicacionLogs.AgregarLog(log);
                //
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(model);
        }

        public ActionResult Modificar(int usuarioId = 0)
        {
            try
            {
                var usuario = ServicioAplicacionUsuarios.ObtenerUsuarioId(usuarioId);
                Mapper.CreateMap<EMA_USUARIODto, UsuarioModel>().ReverseMap();
                var usuarioModel = Mapper.Map<UsuarioModel>(usuario);
                var roles = ServicioAplicacionRoles.ObtenerRolesActivos();
                Mapper.CreateMap<EMA_ROLDto, RolModel>().ReverseMap();
                var rolModel = Mapper.Map<List<RolModel>>(roles);
                var rolesUsuario = ServicioAplicacionRoles.ObtenerRolUsuario(usuarioId);
                foreach (var model in rolesUsuario.SelectMany(rolUsuario => rolModel.Where(model => rolUsuario.ROLID == model.RolId)))
                {
                    model.Seleccionado = true;
                }
                usuarioModel.ListadoRoles = rolModel;
                return View(usuarioModel);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Modificar(UsuarioModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usua = new UsuarioDto
                    {
                        Nombre = model.NombreUsuario,
                        activo = model.Activo,
                        correo = model.Correo,
                        Roles = model.RolesIds.ToList(),
                        logIn = model.Usuario,
                        Id = model.UsuarioId,
                    };

                    var ws = new Utilidades.WSUsuariosCentralizadosManager();
                    var respuestaJObject = ws.ActualizarUsuario(usua);

                    #region actualiza log actividades

                    var identity = (ClaimsIdentity)User.Identity;
                    var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                    var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = usuarioIdentity,
                        fecha = DateTime.Now,
                        idTipoLog = (int)Administracion.ModificacionUsuarios,
                        ip = ip,
                        MenuId = (int)Menus.Usuarios
                    };
                    ServicioAplicacionLogs.AgregarLog(log);

                    #endregion actualiza log actividades

                    return RedirectToAction("Index", new { usua.Id });
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(model);
        }

        //[HttpPost]
        //public ActionResult ObtenerUsuarioExterno(UsuarioModel model)
        //{
        //    try
        //    {
        //        model.TipoDocumentos = ServicioAplicacionTablasBase.ObtenerTipoDocumentos();
        //        var roles = ServicioAplicacionRoles.ObtenerRolesActivos();
        //        Mapper.CreateMap<EMA_ROLDto, RolModel>().ReverseMap();
        //        var rolModel = Mapper.Map<List<RolModel>>(roles);

        //        if (!string.IsNullOrWhiteSpace(model.Documento))
        //        {
        //            var usuarioExterno = ServicioAplicacionUsuarios.ObtenerUsuarioExterno(model.TipoDocumento, model.Documento);

        //            if (usuarioExterno != null)
        //            {
        //                model.TipoDocumentos = ServicioAplicacionTablasBase.ObtenerTipoDocumentos();
        //                model.Documento = model.Documento;
        //                model.TipoDocumento = model.TipoDocumento;
        //                model.NombreUsuario = usuarioExterno.NombreUsuario;
        //                model.Usuario = usuarioExterno.Usuario;
        //                model.Contrasena = usuarioExterno.Contrasena;
        //                model.Activo = true;
        //                model.Correo = usuarioExterno.Email;
        //                rolModel.Where(r => r.RolId == usuarioExterno.Rol).FirstOrDefault().Seleccionado = true;
        //            }
        //        }
        //        model.ListadoRoles = rolModel;
        //    }
        //    catch (Exception exception)
        //    {
        //        ModelState.AddModelError(string.Empty, exception.Message);
        //    }
        //    return View("Agregar", model);
        //}

        public ActionResult Paginador(int page, string usuarioFiltro, string tipoDocumento, string documentoFiltro, string nombreUsuarioFiltro, string correoFiltro)
        {
            try
            {
                var model = new UsuarioModel
                {
                    UsuarioFiltro = usuarioFiltro,
                    TipoDocumentos = ServicioAplicacionTablasBase.ObtenerTipoDocumentos(),
                    TipoDocumento = tipoDocumento,
                    DocumentoFiltro = documentoFiltro,
                    NombreUsuarioFiltro = nombreUsuarioFiltro,
                    CorreoFiltro = correoFiltro
                };

                if (string.IsNullOrEmpty(model.UsuarioFiltro) && string.IsNullOrEmpty(model.TipoDocumento) && string.IsNullOrEmpty(model.DocumentoFiltro) && string.IsNullOrEmpty(model.NombreUsuarioFiltro) && string.IsNullOrEmpty(model.CorreoFiltro))
                {
                    var usuarios = ServicioAplicacionUsuarios.ObtenerUsuarios(0).ToPagedList(page, 20);
                    model.ListaUsuarios = usuarios;
                }
                else
                {
                    var listaConvenciones = ServicioAplicacionUsuarios.ObtenerUsuarioFiltros(model.UsuarioFiltro, model.NombreUsuarioFiltro, model.TipoDocumento, model.DocumentoFiltro, model.CorreoFiltro).ToPagedList(page, 20);
                    model.ListaUsuarios = listaConvenciones;
                }

                return View("Index", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View("Index");
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ViewData["CurrentUser"] = requestContext.HttpContext.User.Identity.Name;
            }
        }

        #endregion Instance Methods
    }
}