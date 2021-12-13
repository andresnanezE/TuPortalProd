using Aplicacion.Administracion.Contratos;

//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
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
    public class CargueTarifasCampañaController : Controller
    {
        #region fields

        private IServicioAplicacionCargueTarifasCampañas _servicioAplicacionCargueTarifasCampaña;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion fields

        #region Intance properties

        private IServicioAplicacionCargueTarifasCampañas ServicioAplicacionCargueTarifasCampañas
        {
            get { return _servicioAplicacionCargueTarifasCampaña ?? (_servicioAplicacionCargueTarifasCampaña = FabricaIoC.Resolver<IServicioAplicacionCargueTarifasCampañas>()); }
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

        // GET: CargueTarifasCampaña
        public ActionResult Index(int usuario = 0)
        {
            try
            {
                var model = new CargueTarifaCampañaModel();
                var tarifasCargadas = ServicioAplicacionCargueTarifasCampañas.ListaTarifasCargue().ToPagedList(1, 20);
                model.ListaCargueTarifasCampaña = tarifasCargadas;

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioIdentity,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Administracion.CargueTarifasCampanas,
                    ip = ip,
                    MenuId = (int)Menus.Cargue_Tarifas_Campana
                };
                ServicioAplicacionLogs.AgregarLog(log);
                //

                return View("Index", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(new CargueTarifaCampañaModel());
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
                idTipoLog = (int)Administracion.CargueTarifasCampanasBoton,
                ip = ip,
                MenuId = (int)Menus.Cargue_Tarifas_Campana
            };
            ServicioAplicacionLogs.AgregarLog(log);
            //

            var model = new CargueTarifaCampañaModel();
            var tarifasCargadas = ServicioAplicacionCargueTarifasCampañas.ListaTarifasCargue().ToPagedList(1, 20);
            model.ListaCargueTarifasCampaña = tarifasCargadas;
            return View(model);
        }

        public string ValidarArchivo(List<string[]> lstDatosArchivo)
        {
            string resultado = string.Empty;
            List<CTB_TARIFAS_CAMPANA> lstarifaCapaña = new List<CTB_TARIFAS_CAMPANA>();
            int contadorRow = 0;
            foreach (string[] row in lstDatosArchivo)
            {
                string validacion = string.Empty;
                contadorRow++;
                DateTime fechaVencimientoTarifa = new DateTime();
                // se valida la cantidad de datos: deben ser 10
                if (row.Length != 10)
                {
                    validacion += "Número de datos no valido, deben ser 10;";
                }
                // validamos el columna 4 que debe ser de tipo int
                int rangoInicial = 0;
                if (!int.TryParse(row[3], out rangoInicial))
                {
                    validacion += "Valor de columna 4 no válido;";
                }
                // validamos el campo 5 que debe ser de tipo int
                int rangoFinal = 0;
                if (!int.TryParse(row[4], out rangoFinal))
                {
                    validacion += "Valor de columna 5 no válido;";
                }
                // validamos el campo 8 que debe ser de tipo double
                decimal valorTarifa = 0;
                if (!decimal.TryParse(row[7], out valorTarifa))
                {
                    validacion += "Valor de columna 8 no válido;";
                }
                // validamos el campo 9 que debe ser de tipo double
                decimal valorIVATarifa = 0;
                if (!decimal.TryParse(row[8], out valorIVATarifa))
                {
                    validacion += "Valor de columna 9 no válido;";
                }
                //validamos el campo 10 que debe ser de tipo fecha
                try
                {
                    string[] fecha = row[9].Split('/');
                    if (fecha.Length == 3)
                    {
                        int dia = 0, mes = 0, año = 0;
                        if (int.TryParse(fecha[0], out dia) && (dia <= 31 && dia >= 1))
                        {
                            if (int.TryParse(fecha[1], out mes) && (mes <= 12 && mes >= 1))
                            {
                                if (int.TryParse(fecha[2], out año) && (año <= 3000 && año >= 2000))
                                {
                                    try
                                    {
                                        fechaVencimientoTarifa = new DateTime(año, mes, dia);
                                    }
                                    catch (Exception)
                                    {
                                        throw new Exception("formato de fecha no valido dd/mm/yyyy.");
                                    }
                                }
                                else
                                {
                                    throw new Exception("formato de fecha no valido dd/mm/yyyy.");
                                }
                            }
                            else
                            {
                                throw new Exception("formato de fecha no valido dd/mm/yyyy.");
                            }
                        }
                        else
                        {
                            throw new Exception("formato de fecha no valido dd/mm/yyyy.");
                        }
                    }
                    else
                    {
                        throw new Exception("formato de fecha no valido dd/mm/yyyy.");
                    }
                }
                catch (Exception ex)
                {
                    validacion += string.Format("Valor de columna 10 no válido: {0};", ex.Message);
                }

                if (validacion != string.Empty)
                {
                    validacion = string.Format("Inconsistencias en la línea {0}:{1} <br /> ", contadorRow, validacion);
                }
                else
                {
                    lstarifaCapaña.Add(new CTB_TARIFAS_CAMPANA
                    {
                        TIPO_TARIFA = row[0],
                        CAMPANA_TARIFA = row[1],
                        CIUDAD = row[2],
                        RANGO_INICIAL_PERSONA = rangoInicial,
                        RANGO_FINAL_PERSONA = rangoFinal,
                        MODALIDAD_PAGO = row[5],
                        FORMA_PAGO = row[6],
                        VALOR_TARIFA = valorTarifa,
                        VALOR_IVA_TARIFA = valorIVATarifa,
                        FECHA_VENCIMIENTO_TARIFA = fechaVencimientoTarifa,
                        ID_ESTADO = 1
                    });
                }
                resultado = resultado + validacion;
            }
            if (resultado == string.Empty)
            {
                ServicioAplicacionCargueTarifasCampañas.ActualizarEstadoCargueTarifaCampañas();
                //foreach (CTB_TARIFAS_CAMPANA tarifacampaña in lstarifaCapaña)
                //{
                //    ServicioAplicacionCargueTarifasCampañas.InsertaCargueTarifaCampañas(tarifacampaña);
                //}
                ServicioAplicacionCargueTarifasCampañas.AgregarTarifaCampañas(lstarifaCapaña);
                resultado = "OK";
            }
            return resultado;
        }

        //[HttpPost]
        //public ActionResult Index(CargueTarifaCampañaModel model)
        //{
        //    try
        //    {
        //        model.ListaCargueTarifasCampaña = ServicioAplicacionCargueTarifasCampañas.ListaTarifasCargue().ToPagedList(1, 20);
        //    }
        //    catch (Exception exception)
        //    {
        //        ModelState.AddModelError(string.Empty, exception.Message);
        //    }
        //    return View(model);
        //}
        public ActionResult Paginador(int page)
        {
            try
            {
                var model = new CargueTarifaCampañaModel();
                var usuarios = ServicioAplicacionCargueTarifasCampañas.ListaTarifasCargue().ToPagedList(page, 20);
                model.ListaCargueTarifasCampaña = usuarios;
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