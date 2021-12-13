using Aplicacion.Administracion.Contratos;

//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;

//using Dominio.Administracion.Entidades.MapperDto;
using PagedList;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class AdministracionEnVezController : Controller
    {
        private IServicioAplicacionSesionEnVezDeAdmUsuarios _servicioAplicacionAdmin;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private readonly string RECUPERARUSUARIOERROR = "Surgio un problema recuperando la información del usuario.";
        private readonly string USUARIOYAEXISTE = "El usuario ya existe.";
        private readonly string USUARIONOEXISTE = "El usuario no existe.";
        private readonly string MSGDEFAULT = "Intentalo nuevamente, si el problema persiste comunicate con el árear de TI.";
        private readonly string NUEVOUSUARIO = "El nuevo usuario se creo con éxito.";
        private readonly string USUARIOACTUALIZADO = "El usuario se actualizo con éxito.";
        private readonly string MODELONOVALIDO = "Los datos ingresados no son validos";
        private readonly string CREARMODFICARUSUARIOEXCEPTION = "Surgio un problema procesando la información del usuario.";

        private IServicioAplicacionSesionEnVezDeAdmUsuarios ServicioAplicacionAdminusuarios
        {
            get
            {
                return _servicioAplicacionAdmin ??
                       (_servicioAplicacionAdmin =
                           FabricaIoC.Resolver<IServicioAplicacionSesionEnVezDeAdmUsuarios>());
            }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get
            {
                return _servicioAplicacionLogs ??
                    (_servicioAplicacionLogs =
                        FabricaIoC.Resolver<IServicioAplicacionLogs>());
            }
        }

        // GET: AdministracionEnVez
        public ActionResult Index(int? page)
        {
            var model = new SesionEnVezDeUsuarioViewModel();
            model.ListaUsuarios = ListaUsuarios().ToList();
            int pageNumber = (page ?? 1);
            model.ListaPageUsuarios = ListaUsuarios().ToPagedList(pageNumber, 10);

            //actualiza log actividades
            var identity = (ClaimsIdentity)User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var log = new EmbLogActividadesDto
            {
                UsuarioId = usuarioId,
                fecha = DateTime.Now,
                idTipoLog = (int)Administracion.AdministracionSesionEnVez,
                ip = ip,
                MenuId = (int)Menus.Administracion_sesion_en_vez_de
            };

            ServicioAplicacionLogs.AgregarLog(log);
            //
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SesionEnVezDeUsuarioViewModel model)
        {
            int countReg = 0;
            string modificar = string.Empty;
            try
            {
                if (!ModelState.IsValid)
                {
                    model = new SesionEnVezDeUsuarioViewModel();
                    ModelState.AddModelError(string.Empty, string.Format("{0} {1}", MODELONOVALIDO, MSGDEFAULT));
                    model.ListaPageUsuarios = ListaUsuarios().ToPagedList(1, 10);
                    model.ListaUsuarios = ListaUsuarios().ToList();
                    return View(model);
                }

                var usuario = new SesionEnVezDeNuevoUsuarioDto()
                {
                    Id = model.Id,
                    CodUsuario = model.Codusuario,
                    Ciudad = model.Ciudad,
                    Canal = model.Canal,
                    Perfil = model.Perfil,
                    Segmento = string.IsNullOrEmpty(model.Segmento) ? "" : model.Segmento
                };

                modificar = model.Modificar;

                if (modificar.Equals("Si"))
                {
                    countReg = ServicioAplicacionAdminusuarios.ActUsuario(usuario);

                    //actualiza log actividades modifica usuario
                    var identity = (ClaimsIdentity)User.Identity;
                    var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                    var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = usuarioId,
                        fecha = DateTime.Now,
                        idTipoLog = (int)Administracion.ModificacionUsuarioSesionEnVez,
                        ip = ip,
                        MenuId = (int)Menus.Administracion_sesion_en_vez_de
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                    //
                }
                else
                {
                    countReg = ServicioAplicacionAdminusuarios.CrearUsuario(usuario);

                    //actualiza log actividades crea usuario
                    var identity = (ClaimsIdentity)User.Identity;
                    var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                    var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = usuarioId,
                        fecha = DateTime.Now,
                        idTipoLog = (int)Administracion.CreacionUsuarioSesionEnVez,
                        ip = ip,
                        MenuId = (int)Menus.Administracion_sesion_en_vez_de
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                    //
                }

                model = new SesionEnVezDeUsuarioViewModel();

                if (countReg > 0)
                {
                    model.Mensaje = modificar.Equals("Si") ? USUARIOACTUALIZADO : NUEVOUSUARIO;
                }
                else
                {
                    model.Mensaje = USUARIOYAEXISTE;
                }

                model.ListaPageUsuarios = ListaUsuarios().ToPagedList(1, 10);
                model.ListaUsuarios = ListaUsuarios().ToList();

                return View(model);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                ModelState.AddModelError(string.Empty, string.Format("{0} {1}", CREARMODFICARUSUARIOEXCEPTION, MSGDEFAULT));
                model = new SesionEnVezDeUsuarioViewModel();
                model.ListaPageUsuarios = ListaUsuarios().ToPagedList(1, 10);
                model.ListaUsuarios = ListaUsuarios().ToList();
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult ObtValoresCamposFormulario(string doc, int id)
        {
            var model = new SesionEnVezDeUsuarioViewModel();

            try
            {
                int _doc;

                if (int.TryParse(doc, out _doc))
                {
                    var parametros = ServicioAplicacionAdminusuarios.ObtenerParametros(Convert.ToDecimal(doc), id);

                    if (!parametros.Any())
                    {
                        model.Error = USUARIONOEXISTE;
                    }
                    else
                    {
                        model = new SesionEnVezDeUsuarioViewModel(parametros);

                        if (!string.IsNullOrEmpty(model.Error))
                        {
                            return Json(model);
                        }
                    }
                }
                else
                {
                    model.Error = string.Format("{0} {1}", "Valor no valido para Documento", MSGDEFAULT);
                }

                return Json(model);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                model.Error = string.Format("{0} {1}", RECUPERARUSUARIOERROR, MSGDEFAULT);
                return Json(model);
            }
        }

        [HttpPost]
        public JsonResult ActEstadoUsuario(int id, char estado)
        {
            try
            {
                int actualizado = ServicioAplicacionAdminusuarios.ActEstadoUsuario(id, estado);
                return Json(actualizado);

                //return Json("{\"msgError\":\"Ha surgido un problema actualizando el estado del usuario. Intentalo de nuevo, si el problema persiste comunicate con el área de TI.\"}");
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                return Json("{\"msgError\":\"Ha surgido un problema actualizando el estado del usuario. Intentalo de nuevo, si el problema persiste comunicate con el área de TI.\"}");
            }
        }

        private IEnumerable<Usuario> ListaUsuarios()
        {
            var ListaUsuariosModel = new List<Usuario>();

            var listaUsuarios = ServicioAplicacionAdminusuarios.ObtUsuarios();

            foreach (var usuarioDto in listaUsuarios)
            {
                Usuario u = new Usuario()
                {
                    Id = usuarioDto.Id,
                    Canal = usuarioDto.Canal,
                    Ciudad = usuarioDto.Ciudad,
                    Documento = usuarioDto.Documento,
                    Estado = usuarioDto.Estado,
                    Nombre = usuarioDto.Nombre,
                    Perfil = usuarioDto.Perfil,
                    Segmento = usuarioDto.Segmento
                };

                ListaUsuariosModel.Add(u);
            }

            return ListaUsuariosModel;
        }

        public ActionResult Paginador(int page, int codigo)
        {
            var usuario = new SesionEnVezDeUsuarioViewModel();
            usuario.ListaPageUsuarios = ListaUsuarios().ToPagedList(page, codigo);

            return View("Index", usuario);
        }
    }
}