using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using PagedList;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class CargueTarifasPlenasController : Controller
    {
        #region fields

        private IServicioAplicacionCargueTarifasPlenas _servicioAplicacionCargueTarifasPlenas;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion fields

        #region Intance properties

        private IServicioAplicacionCargueTarifasPlenas ServicioAplicacionCargueTarifasPlenas
        {
            get { return _servicioAplicacionCargueTarifasPlenas ?? (_servicioAplicacionCargueTarifasPlenas = FabricaIoC.Resolver<IServicioAplicacionCargueTarifasPlenas>()); }
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

        // GET: CargueTarifasPlenas
        public ActionResult Index(int usuario = 0)
        {
            try
            {
                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.CargueTarifasPlenas,
                    ip = ip,
                    MenuId = (int)Menus.Cargue_Tarifas_Plenas
                };
                ServicioAplicacionLogs.AgregarLog(log);
                //

                var model = new CargueTarifaPlenaModel();
                var tarifasCargadas = ServicioAplicacionCargueTarifasPlenas.ListaTarifasPlenas().ToPagedList(1, 20);
                model.ListaCargueTarifasPlenas = tarifasCargadas;
                return View("Index", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(new List<CargueTarifaPlenaModel>());
        }

        #region Instance Methods

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ViewData["CurrentUser"] = requestContext.HttpContext.User.Identity.Name;
            }
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    // validamos la extension del archivo
                    var tipo = file.GetType().ToString();
                    List<string[]> parsedData = new List<string[]>();
                    using (StreamReader readFile = new StreamReader(file.InputStream, Encoding.Default))
                    {
                        string line;
                        string[] row;

                        while ((line = readFile.ReadLine()) != null)
                        {
                            int indexSeparador = line.LastIndexOf(';');
                            if (indexSeparador == line.Length - 1)
                            {
                                line = line.Remove(indexSeparador, 1);
                            }
                            row = line.Split(';');
                            parsedData.Add(row);
                        }
                    }

                    var resultado = ValidarArchivo(parsedData);
                    if (resultado == "OK")
                    {
                        //View("Index", new CargueTarifaCampañaModel());
                        return RedirectToAction("Index");
                    }
                    else
                    { ViewBag.Message = ValidarArchivo(parsedData); }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "Debe seleccionar un archivo.";
            }

            //actualiza log actividades
            var identity = (ClaimsIdentity)User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var log = new EmbLogActividadesDto
            {
                UsuarioId = usuarioIdentity,
                fecha = DateTime.Now,
                idTipoLog = (int)Administracion.CargueTarifasPlenasBotón,
                ip = ip,
                MenuId = (int)Menus.Cargue_Tarifas_Plenas
            };
            ServicioAplicacionLogs.AgregarLog(log);
            //

            var model = new CargueTarifaPlenaModel();
            var tarifasCargadas = ServicioAplicacionCargueTarifasPlenas.ListaTarifasPlenas().ToPagedList(1, 20);
            model.ListaCargueTarifasPlenas = tarifasCargadas;
            return View(model);
        }

        public string ValidarArchivo(List<string[]> lstDatosArchivo)
        {
            string resultado = string.Empty;
            List<CTB_TARIFAS_PLENAS> lstarifaPlena = new List<CTB_TARIFAS_PLENAS>();
            int contadorRow = 0;
            foreach (string[] row in lstDatosArchivo)
            {
                string validacion = string.Empty;
                contadorRow++;
                // se valida la cantidad de datos: deben ser 2
                if (row.Length != 2)
                {
                    validacion += "Número de datos no valido, deben ser 2;";
                }
                // validamos el campo 8 que debe ser de tipo double
                decimal tarifaPlena = 0;
                if (!decimal.TryParse(row[1], out tarifaPlena))
                {
                    validacion += "Valor de columna 2 no válido;";
                }

                if (validacion != string.Empty)
                {
                    validacion = string.Format("Inconsistencias en la línea {0}:{1} <br /> ", contadorRow, validacion);
                }
                else
                {
                    lstarifaPlena.Add(new CTB_TARIFAS_PLENAS
                    {
                        CIUDAD = row[0],
                        TARIFA_PLENA = tarifaPlena,
                        ID_ESTADO = 1
                    });
                }
                resultado = resultado + validacion;
            }
            if (resultado == string.Empty)
            {
                ServicioAplicacionCargueTarifasPlenas.ActualizarEstadoCargueTarifasPlenas();
                foreach (CTB_TARIFAS_PLENAS tarifacampaña in lstarifaPlena)
                {
                    ServicioAplicacionCargueTarifasPlenas.InsertaCargueTarifasPlenas(tarifacampaña);
                }
                resultado = "OK";
            }
            return resultado;
        }

        public ActionResult Paginador(int page)
        {
            try
            {
                var model = new CargueTarifaPlenaModel();
                var usuarios = ServicioAplicacionCargueTarifasPlenas.ListaTarifasPlenas().ToPagedList(page, 20);
                model.ListaCargueTarifasPlenas = usuarios;
                return View("Index", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View("Index");
        }

        #endregion Instance Methods
    }
}