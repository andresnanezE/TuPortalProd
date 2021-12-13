// ----------------------------------------------------------------------------------------------
// <copyright file="RolController.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using System.Web.Routing;
using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto.DtoProcesos;
using AutoMapper;
using PagedList;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class RolController : Controller
    {
        #region Fields

        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IAdaptadorDeObjetos _adaptadorDeObjetos;

        #endregion

        #region Instance Properties

        private IServicioAplicacionRoles ServicioAplicacionRoles
        {
            get { return _servicioAplicacionRoles ?? (_servicioAplicacionRoles = FabricaIoC.Resolver<IServicioAplicacionRoles>()); }
        }

        private IAdaptadorDeObjetos AdaptadorDeObjetos
        {
            get { return _adaptadorDeObjetos ?? (_adaptadorDeObjetos = FabricaIoC.Resolver<IAdaptadorDeObjetos>()); }
        }
        #endregion

        #region Instance Methods

        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(RolModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Mapper.CreateMap<EMA_ROLDto, RolModel>().ReverseMap();
                    var rol = Mapper.Map<EMA_ROLDto>(model);
                    var userName = ViewData["CurrentUser"] == null ? null : ViewData["CurrentUser"].ToString();
                    rol.FECHACREACION = DateTime.Now;
                    rol.USUARIOCREACION = userName;
                    var rolId = ServicioAplicacionRoles.AgregarRol(rol);
                    return RedirectToAction("Index", new
                                                     {
                                                         rolId
                                                     });
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(model);
        }


        public ActionResult Index(int rolId = 0)
        {
            try
            {
                var model = new RolModel();
                if (rolId > 0)
                {
                    model.Mensaje = "Petición realizada exitosa";
                }

                model.ListadoRoles = ServicioAplicacionRoles.ObtenerRolesFiltros(rolId, string.Empty).ToPagedList(1, 20);

                return View(model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(RolModel model)
        {
            try
            {
                model.ListadoRoles = ServicioAplicacionRoles.ObtenerRolesFiltros(0, model.RolFiltro).ToPagedList(1, 20);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(model);
        }

        public ActionResult Modificar(int rolId = 0)
        {
            try
            {
                var rol = ServicioAplicacionRoles.ObtenerRolId(rolId);
                var model = new RolModel
                            {
                                RolId = rol.ROLID,
                                Rol = rol.ROL,
                                Activo = rol.ACTIVO
                            };
                return View(model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Modificar(RolModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Mapper.CreateMap<EMA_ROLDto, RolModel>().ReverseMap();
                    var rol = Mapper.Map<EMA_ROLDto>(model);
                    var userName = ViewData["CurrentUser"] == null ? null : ViewData["CurrentUser"].ToString();
                    rol.USUARIOMODIFICACION = userName;
                    ServicioAplicacionRoles.ModificarRol(rol);
                    return RedirectToAction("Index", new
                                                     {
                                                         rol.ROLID
                                                     });
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(model);
        }

        public ActionResult Paginador(int page, string rol)
        {
            try
            {
                var model = new RolModel
                            {
                                RolFiltro = rol, ListadoRoles = ServicioAplicacionRoles.ObtenerRolesFiltros(0, rol).ToPagedList(page, 20)
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

        #endregion
    }
}
