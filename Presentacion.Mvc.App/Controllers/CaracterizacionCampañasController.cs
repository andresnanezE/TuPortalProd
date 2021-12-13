using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;

//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using PagedList;
using Presentacion.Mvc.App.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class CaracterizacionCampañasController : Controller
    {
        #region fields

        private IServicioAplicacionCaracterizacionCampañas _servicioAplicacionCaracterizacionCampañas;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion fields

        #region Intance properties

        private IServicioAplicacionCaracterizacionCampañas ServicioAplicacionCaracterizacionCampañas
        {
            get { return _servicioAplicacionCaracterizacionCampañas ?? (_servicioAplicacionCaracterizacionCampañas = FabricaIoC.Resolver<IServicioAplicacionCaracterizacionCampañas>()); }
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

        #endregion Intance properties

        // GET: CaracterizacionCampañas
        public ActionResult Index(int usuarioId = 0)
        {
            try
            {
                var model = new CaracterizacionCampañasModel();
                if (usuarioId > 0)
                {
                    model.Mensaje = "Petición realizada exitosa";
                }
                //var usuarios = ServicioAplicacionUsuarios.ObtenerUsuarios(usuarioId).ToPagedList(1, 20);
                var tipostarifas = ServicioAplicacionCaracterizacionCampañas.ListaTipoTarifa();
                //model.ListaUsuarios = usuarios;
                model.TipoTarifas = tipostarifas;
                model.ListaCampanaCaracterizacion = ServicioAplicacionCaracterizacionCampañas.ObtenerCampañaCaracterizacionFiltros(model.TIPO_TARIFA, model.CAMPANA_TARIFA).ToPagedList(1, 20);

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.CaracterizacionCampanas,
                    ip = ip,
                    MenuId = (int)Menus.Caracterizacion_Campañas
                };

                ServicioAplicacionLogs.AgregarLog(log);
                //
                return View(model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(new CaracterizacionCampañasModel());
        }

        public ActionResult Modificar(string TIPO_TARIFA, string CAMPANA_TARIFA)
        {
            try
            {
                var caracterizacionCampaña = ServicioAplicacionCaracterizacionCampañas.ObtenerCampañaCaracterizacionFiltros(TIPO_TARIFA, CAMPANA_TARIFA).FirstOrDefault();
                CaracterizacionCampañasModel model = new CaracterizacionCampañasModel()
                {
                    TIPO_TARIFA = caracterizacionCampaña.TIPO_TARIFA,
                    CAMPANA_TARIFA = caracterizacionCampaña.CAMPANA_TARIFA,
                    RUTA_IMAGEN = caracterizacionCampaña.RUTA_IMAGEN,
                    CARACTERIZACION = caracterizacionCampaña.CARACTERIZACION
                };
                return View(model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View();
        }

        public ActionResult Paginador(int page, string tipoTarifa, string campaña)
        {
            try
            {
                var model = new CaracterizacionCampañasModel
                {
                    TIPO_TARIFA = tipoTarifa,
                    CAMPANA_TARIFA = campaña
                };

                if (string.IsNullOrEmpty(model.TIPO_TARIFA) && string.IsNullOrEmpty(model.CAMPANA_TARIFA))
                {
                    var campañasCaracterizacion = ServicioAplicacionCaracterizacionCampañas.ObtenerCampañaCaracterizacionFiltros(null, null).ToPagedList(page, 20);
                    model.ListaCampanaCaracterizacion = campañasCaracterizacion;
                }
                else
                {
                    var listaResultado = ServicioAplicacionCaracterizacionCampañas.ObtenerCampañaCaracterizacionFiltros(model.TIPO_TARIFA, model.CAMPANA_TARIFA).ToPagedList(page, 20);
                    model.ListaCampanaCaracterizacion = listaResultado;
                }

                return View("Index", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Campañas(string TIPO_TARIFA)
        {
            var tipoTarifaList = ServicioAplicacionCaracterizacionCampañas.ListaTipoTarifa();

            var campañasList = ServicioAplicacionCaracterizacionCampañas.ListaCampañaXTipoTarifa(TIPO_TARIFA);

            CaracterizacionCampañasModel model = new CaracterizacionCampañasModel()
            {
                TIPO_TARIFA = TIPO_TARIFA,
                TipoTarifas = tipoTarifaList,
                Campañas = campañasList
            };
            var lstCampanaCaracterizacion = ServicioAplicacionCaracterizacionCampañas.ObtenerCampañaCaracterizacionFiltros(model.TIPO_TARIFA, model.CAMPANA_TARIFA).ToPagedList(1, 20);
            model.ListaCampanaCaracterizacion = lstCampanaCaracterizacion;
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Resultado(string TIPO_TARIFA, string CAMPANA_TARIFA)
        {
            var tipoTarifaList = ServicioAplicacionCaracterizacionCampañas.ListaTipoTarifa();

            var campañasList = ServicioAplicacionCaracterizacionCampañas.ListaCampañaXTipoTarifa(TIPO_TARIFA);

            CaracterizacionCampañasModel model = new CaracterizacionCampañasModel()
            {
                TIPO_TARIFA = TIPO_TARIFA,
                TipoTarifas = tipoTarifaList,
                CAMPANA_TARIFA = CAMPANA_TARIFA,
                Campañas = campañasList
            };
            model.ListaCampanaCaracterizacion = ServicioAplicacionCaracterizacionCampañas.ObtenerCampañaCaracterizacionFiltros(model.TIPO_TARIFA, model.CAMPANA_TARIFA).ToPagedList(1, 20);
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Modificar(CaracterizacionCampañasModel model, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CTB_CAMPANA_CARACTERIZACION campaña = new CTB_CAMPANA_CARACTERIZACION();
                    campaña.TIPO_TARIFA = model.TIPO_TARIFA;
                    campaña.CAMPANA_TARIFA = model.CAMPANA_TARIFA;
                    if (file != null && file.ContentLength > 0)
                    {
                        var tipo = Path.GetExtension(file.FileName).ToUpper();
                        if (tipo != ".PNG" && tipo != ".JPG")
                        {
                            ViewBag.Message = "Debe seleccionar un archivo de tipo imagen";
                            return View(model);
                        }
                        file.SaveAs(Server.MapPath("~\\Image\\campanas\\") + file.FileName);
                        campaña.RUTA_IMAGEN = "../Image/campanas/" + file.FileName;
                    }
                    else
                    {
                        campaña.RUTA_IMAGEN = model.RUTA_IMAGEN;
                    }
                    campaña.CARACTERIZACION = model.CARACTERIZACION;
                    ServicioAplicacionCaracterizacionCampañas.ModificarCampañaCaracterizacion(campaña);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(model);
        }
    }
}