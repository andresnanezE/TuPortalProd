// ----------------------------------------------------------------------------------------------
// <copyright file="MenuController.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Aplicacion.Administracion.Contratos;
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

namespace Presentacion.Mvc.App.Controllers
{
    public class MenuController : Controller
    {
        #region Fields

        private IServicioAplicacionMenu _servicioAplicacionMenu;
        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion Fields

        #region Instance Properties

        private IServicioAplicacionMenu ServicioAplicacionMenu
        {
            get { return _servicioAplicacionMenu ?? (_servicioAplicacionMenu = FabricaIoC.Resolver<IServicioAplicacionMenu>()); }
        }

        private IServicioAplicacionRoles ServicioAplicacionRoles
        {
            get { return _servicioAplicacionRoles ?? (_servicioAplicacionRoles = FabricaIoC.Resolver<IServicioAplicacionRoles>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        #endregion Instance Properties

        #region Instance Methods

        public ActionResult Agregar()
        {
            var model = new MenuModel();

            try
            {
                var roles = ServicioAplicacionRoles.ObtenerRolesActivos();
                Mapper.CreateMap<EMA_ROLDto, RolModel>().ReverseMap();
                var rolModel = Mapper.Map<List<RolModel>>(roles);
                model.ListadoRoles = rolModel;
                model.ListadoMenuPadres = ServicioAplicacionMenu.ObtenerMenuPadres();

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.CrearMenu,
                    ip = ip,
                    MenuId = (int)Menus.Menu
                };
                ServicioAplicacionLogs.AgregarLog(log);
                //

                return View(model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Agregar(MenuModel model)
        {
            try
            {
                var roles = ServicioAplicacionRoles.ObtenerRolesActivos();
                Mapper.CreateMap<EMA_ROLDto, RolModel>().ReverseMap();
                var rolModel = Mapper.Map<List<RolModel>>(roles);
                if (model.RolesIds != null)
                {
                    foreach (var rol in from rol in rolModel from rolId in model.RolesIds.Where(rolId => rol.RolId == rolId) select rol)
                    {
                        rol.Seleccionado = true;
                    }
                }
                model.ListadoRoles = rolModel;
                model.ListadoMenuPadres = ServicioAplicacionMenu.ObtenerMenuPadres();

                if (ModelState.IsValid)
                {
                    Mapper.CreateMap<EMA_MENUDto, MenuModel>().ReverseMap();
                    var menu = Mapper.Map<EMA_MENUDto>(model);
                    var userName = ViewData["CurrentUser"] == null ? null : ViewData["CurrentUser"].ToString();
                    menu.FECHAMODIFICACION = null;
                    menu.FECHACREACION = DateTime.Now;
                    menu.USUARIOCREACION = userName;
                    var menuId = ServicioAplicacionMenu.AgregarMenu(menu, model.RolesIds);
                    return RedirectToAction("Index", new
                    {
                        usuarioId = menuId
                    });
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(model);
        }

        public ActionResult Index(int menuId = 0)
        {
            try
            {
                var model = new MenuModel
                {
                    ListadoComboRoles = ServicioAplicacionRoles.ObtenerRoles(),
                    ListadoMenuPadres = ServicioAplicacionMenu.ObtenerMenuPadres()
                };

                if (menuId > 0)
                {
                    model.ListadoMenu = ServicioAplicacionMenu.ObtenerMenuFiltros(menuId, string.Empty, 0, 0).ToPagedList(1, 20);
                    model.Mensaje = "Petición realizada exitosa";
                }
                else
                {
                    model.ListadoMenu = ServicioAplicacionMenu.ObtenerMenu().ToPagedList(1, 20);
                }

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.Menu,
                    ip = ip,
                    MenuId = (int)Menus.Menu
                };
                ServicioAplicacionLogs.AgregarLog(log);
                //
                return View(model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(new List<UsuarioModel>());
        }

        [HttpPost]
        public ActionResult Index(MenuModel model)
        {
            try
            {
                model.ListadoMenuPadres = ServicioAplicacionMenu.ObtenerMenuPadres();
                model.ListadoComboRoles = ServicioAplicacionRoles.ObtenerRoles();
                model.ListadoMenu = ServicioAplicacionMenu.ObtenerMenuFiltros(0, model.DescripcionFiltro, model.NodoPadreId, model.RoleId).ToPagedList(1, 20);

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.BuscarMenu,
                    ip = ip,
                    MenuId = (int)Menus.Menu
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

        public ActionResult Modificar(int menuId = 0)
        {
            try
            {
                var menu = ServicioAplicacionMenu.ObtenerMenuFiltros(menuId, string.Empty, 0, 0).FirstOrDefault();
                Mapper.CreateMap<MenuAplicacionDto, MenuModel>();
                Mapper.CreateMap<MenuAplicacionDto, MenuModel>().ReverseMap();
                var menuModel = Mapper.Map<MenuModel>(menu);
                var roles = ServicioAplicacionRoles.ObtenerRolesActivos();
                Mapper.CreateMap<EMA_ROLDto, RolModel>();
                Mapper.CreateMap<EMA_ROLDto, RolModel>().ReverseMap();
                var rolModel = Mapper.Map<List<RolModel>>(roles);
                var rolesMenu = ServicioAplicacionRoles.ObtenerRolMenu(menuId);
                foreach (var model in rolesMenu.SelectMany(rolMenu => rolModel.Where(model => rolMenu.ROLID == model.RolId)))
                {
                    model.Seleccionado = true;
                }
                menuModel.ListadoRoles = rolModel;
                menuModel.ListadoMenuPadres = ServicioAplicacionMenu.ObtenerMenuPadres();

                return View(menuModel);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Modificar(MenuModel model)
        {
            try
            {
                var roles = ServicioAplicacionRoles.ObtenerRolesActivos();
                Mapper.CreateMap<EMA_ROLDto, RolModel>().ReverseMap();
                var rolModel = Mapper.Map<List<RolModel>>(roles);
                if (model.RolesIds != null)
                {
                    foreach (var rol in from rol in rolModel from rolId in model.RolesIds.Where(rolId => rol.RolId == rolId) select rol)
                    {
                        rol.Seleccionado = true;
                    }
                }
                model.ListadoRoles = rolModel;
                model.ListadoMenuPadres = ServicioAplicacionMenu.ObtenerMenuPadres();
                if (ModelState.IsValid)
                {
                    Mapper.CreateMap<EMA_MENUDto, MenuModel>().ReverseMap();
                    var menu = Mapper.Map<EMA_MENUDto>(model);
                    var userName = ViewData["CurrentUser"] == null ? null : ViewData["CurrentUser"].ToString();

                    menu.USUARIOMODIFICACION = userName;
                    ServicioAplicacionMenu.ModificarMenu(menu, model.RolesIds);
                    return RedirectToAction("Index", new
                    {
                        menu.MENUID
                    });
                }

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.ModificarMenu,
                    ip = ip,
                    MenuId = (int)Menus.Menu
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

        public ActionResult Paginador(int page, string descripcionFiltro, int nodoPadreId = 0, int rolId = 0)
        {
            try
            {
                var model = new MenuModel
                {
                    ListadoComboRoles = ServicioAplicacionRoles.ObtenerRoles(),
                    ListadoMenuPadres = ServicioAplicacionMenu.ObtenerMenuPadres(),
                    DescripcionFiltro = descripcionFiltro,
                    NodoPadreId = nodoPadreId,
                    RoleId = rolId,
                    ListadoMenu = ServicioAplicacionMenu.ObtenerMenuFiltros(0, descripcionFiltro, nodoPadreId, rolId).ToPagedList(page, 20),
                };

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