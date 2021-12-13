using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Newtonsoft.Json;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class CotizarReservaController : Controller
    {
        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioCotizacion _servicioCotizacion;
        private IServicioAplicacionUsuarios _servicioUsuarios;

        private IServicioAplicacionRoles ServicioAplicacionRoles
        {
            get { return _servicioAplicacionRoles ?? (_servicioAplicacionRoles = FabricaIoC.Resolver<IServicioAplicacionRoles>()); }
        }

        private IServicioCotizacion ServicioCotizacion
        {
            get { return _servicioCotizacion ?? (_servicioCotizacion = FabricaIoC.Resolver<IServicioCotizacion>()); }
        }

        private IServicioAplicacionUsuarios ServicioUsuarios
        {
            get { return _servicioUsuarios ?? (_servicioUsuarios = FabricaIoC.Resolver<IServicioAplicacionUsuarios>()); }
        }

        #region 3). Cotización

        /// <summary>
        /// Metodo creado para cargar valores campos y checklist destinados a cotizar una reserva
        /// </summary>
        /// <param name="reservID"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public ActionResult Index(int cotID)
        {
            var model = new RegistroVisitaModel();
            List<Productos> product = new List<Productos>();

            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            try
            {
                //Información del cliente.
                if (cotID != 0) model.IdCotizacion = cotID.ToString();

                Session["IdCotizacion"] = cotID;
                model.validacionInfo = ServicioCotizacion.ObtenerValidacion();
                var infoReserva = ServicioCotizacion.ObtenerDatosReserva(cotID, userId);
                if (infoReserva != null)
                {
                    foreach (var reser in infoReserva)
                    {
                        model.NIT = reser.NIT.ToString();
                        model.nombreEmpresa = reser.nombreEmpresa;
                        model.nombreCliente = reser.contacto;
                        model.Cargo = reser.cargo;
                        model.telefono = reser.telefono;
                        model.correoElectronico = reser.correoElectronico;
                        model.celular = reser.celular;
                        model.ciudad = reser.ciudad;
                    }
                }

                product = ServicioCotizacion.productosCotizacion(cotID).ToList();
                if (product.Count > 0)
                {
                    model.NombreProductoCotizacion = product[0].NombreProducto;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }

            return View("Index", model);
        }

        [System.Web.Mvc.HttpGet]
        public String ObtenerListados(string nombreListado)
        {
            var retornar = new ObtenerData();
            var seriarlizer = new JavaScriptSerializer();

            if (Session["IdCotizacion"] != null)
            {
                retornar.IdCotizacion = Session["IdCotizacion"].ToString();
            }

            switch (nombreListado)
            {
                case "SectorEconomico":
                    var sectores = ServicioCotizacion.ObtenerSectores();
                    var serializadoSector = seriarlizer.Serialize(sectores);
                    return serializadoSector;

                case "TipoAP":
                    var tipoAreasProtegidas = ServicioCotizacion.ObtenerTipoAreasProtegidas();
                    var serializadoTipoAP = seriarlizer.Serialize(tipoAreasProtegidas);
                    return serializadoTipoAP;

                case "CiudadesFactor":
                    var CiudadesFactor = ServicioCotizacion.ObtenerCiudadesFactor();
                    var serializadoCiudadesFactor = seriarlizer.Serialize(CiudadesFactor);
                    return serializadoCiudadesFactor;

                case "TipoRiesgo":
                    var TipoRiesgo = ServicioCotizacion.ObtenerTipoRiesgo();
                    var serializadoTipoRiesgo = seriarlizer.Serialize(TipoRiesgo);
                    return serializadoTipoRiesgo;

                default:
                    break;
            }

            return "";
        }

        [System.Web.Mvc.HttpGet]
        public String ObtenerIdCotizacion()
        {
            var seriarlizer = new JavaScriptSerializer();
            var idCotizacion = seriarlizer.Serialize(Session["IdCotizacion"]);

            return idCotizacion;
        }

        [System.Web.Mvc.HttpGet]
        public String ObtenerInformacionReserva()
        {
            List<InformacionReserva> lstRetornar = new List<InformacionReserva>();
            InformacionReserva objRetornar = new InformacionReserva();

            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var infoReserva = ServicioCotizacion.ObtenerDatosReserva(Convert.ToInt32(Session["IdCotizacion"]), userId).ToList();
            var product = ServicioCotizacion.productosCotizacion(Convert.ToInt32(Session["IdCotizacion"])).ToList();

            foreach (var item in infoReserva)
            {
                objRetornar.id_cotizacion = item.id_cotizacion;
                objRetornar.DV = item.DV;
                objRetornar.NIT = item.NIT;
                objRetornar.nombreEmpresa = item.nombreEmpresa;
                objRetornar.contacto = item.contacto;
                objRetornar.telefono = item.telefono;
                objRetornar.cargo = item.cargo;
                objRetornar.bloqueo = item.bloqueo;
                objRetornar.motivoVisita = item.motivoVisita;
                objRetornar.celular = item.celular;
                objRetornar.correoElectronico = item.correoElectronico;
                objRetornar.numeroRenovaciones = item.numeroRenovaciones;
                objRetornar.TipoAP = item.TipoAP;
                objRetornar.fechaVisita = item.fechaVisita;
                objRetornar.fechaExpiracion = item.fechaExpiracion;
                objRetornar.ciudad = item.ciudad;
                objRetornar.canal = item.canal;
                objRetornar.estado = item.estado;
                objRetornar.SectorEconomico = item.SectorEconomico;
                objRetornar.NumeroExpuestos = item.NumeroExpuestos;
                objRetornar.NumeroCapacitaciones = item.NumeroCapacitaciones;
                objRetornar.NumeroEventos = item.NumeroEventos;
                objRetornar.NumeroSedes = item.NumeroSedes;
                objRetornar.ArchivoCotizacion = item.ArchivoCotizacion;
                objRetornar.VeracidadInformacion = item.VeracidadInformacion;
                objRetornar.Total = item.Total;
                objRetornar.EstadoPricing = item.EstadoPricing;
                objRetornar.ObservacionesPricing = item.ObservacionesPricing;
                objRetornar.ObservacionReserva = item.ObservacionReserva;
                objRetornar.TelefonoExt = item.TelefonoExt;
                objRetornar.EstadoCotizacion = item.EstadoCotizacion;
                objRetornar.NombreProducto = product[0].NombreProducto;
                objRetornar.NumeroReconsideraciones = item.NumeroReconsideraciones ?? 0;

                lstRetornar.Add(objRetornar);
                objRetornar = new InformacionReserva();
            }

            return JsonConvert.SerializeObject(lstRetornar);
        }

        [System.Web.Mvc.HttpGet]
        public String ObtenerNombreDirector()
        {
            var seriarlizer = new JavaScriptSerializer();
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var userAseso = ServicioCotizacion.ObtenerDocumentoAsesor(Session["IdCotizacion"].ToString());
            var nombreDirector = ServicioCotizacion.ObtenerNombreDirector(userAseso);

            return seriarlizer.Serialize(nombreDirector);
        }

        [System.Web.Mvc.HttpPost]
        public String GuardarCotizacion(GuardarCotizacion objGuardar)
        {
            var obtenerTarifaSede = new List<decimal>();
            var seriarlizer = new JavaScriptSerializer();
            RespuestaCotizacion respuesta = new RespuestaCotizacion();
            CalcularTarifaSede objCalcularTarifaSede = new CalcularTarifaSede();

            if (objGuardar.ListadoSedes.Count > 0)
            {
                //Calcular el valor de la tarifa de cada sede
                foreach (var item in objGuardar.ListadoSedes)
                {
                    objCalcularTarifaSede = new CalcularTarifaSede();
                    objCalcularTarifaSede.Id_Ciudad = item.Id_Ciudad;
                    objCalcularTarifaSede.Id_Sector = objGuardar.Id_Sector;
                    objCalcularTarifaSede.Id_TipoRiesgo = item.Id_TipoRiesgo;
                    objCalcularTarifaSede.Id_TipoAP = objGuardar.Id_TipoAP;
                    objCalcularTarifaSede.NumeroEmpleados = item.NoPersonalPermanente;
                    objCalcularTarifaSede.NumeroVisitantes = item.NoPersonalVisitantes;

                    try
                    {
                        obtenerTarifaSede = ServicioCotizacion.CalcularTarifaSedeSCI(objCalcularTarifaSede);
                    }
                    catch (Exception ex)
                    {
                        if (((SqlException)ex).Number.Equals(8134))
                        {
                            respuesta.RespCotizacion = false;
                            respuesta.RespCotizacionSedes = false;
                            respuesta.RespCotizacionDivisor = false;
                            respuesta.MensajeCotizacion = "No se guardo correctamente la cotización";
                            respuesta.MensajeCotizacionSedes = "No se guardo correctamente las sedes de la cotización";
                            respuesta.MensajeCotizacionDivisor = "El divisor de la formula es cero, no se puede realizar la cotización.";

                            return seriarlizer.Serialize(respuesta);
                        }
                    }

                    if (obtenerTarifaSede.Count > 0)
                    {
                        item.Valor = Convert.ToDecimal(obtenerTarifaSede[0]);
                    }

                    //Valor Total de la cotización
                    objGuardar.ValorTarifa += item.Valor;
                }
            }

            if (objGuardar != null)
            {
                var respuestaActualizarCoti = ServicioCotizacion.GuardarCotizacionSCI(objGuardar);
                if (respuestaActualizarCoti == true)
                {
                    respuesta.RespCotizacion = true;
                    respuesta.MensajeCotizacion = "Se guardo correctamente la cotización";

                    var respuestaCrearSedes = ServicioCotizacion.GuardarCotizacionSedesSCI(objGuardar);
                    if (respuestaCrearSedes == true)
                    {
                        respuesta.RespCotizacionSedes = true;
                        respuesta.MensajeCotizacionSedes = "Se guardo correctamente las sedes de la cotización";
                    }
                    else
                    {
                        respuesta.RespCotizacionSedes = false;
                        respuesta.MensajeCotizacionSedes = "No se guardo correctamente las sedes de la cotización";
                    }
                }
                else
                {
                    respuesta.RespCotizacion = false;
                    respuesta.MensajeCotizacion = "No se guardo correctamente la cotización";
                }
            }

            return seriarlizer.Serialize(respuesta);
        }

        [System.Web.Mvc.HttpGet]
        public String ObtenerInfReporteCotizacion()
        {
            var dataReporteCotizacion = ServicioCotizacion.ObtenerInfReporteCotizacion(Convert.ToInt32(Session["IdCotizacion"]));
            return JsonConvert.SerializeObject(dataReporteCotizacion);
        }
        [System.Web.Mvc.HttpGet]
        public String ObtenerReporteGlobal()
        {
            var dataReporteCotizacionGlobal = ServicioCotizacion.ObtenerReporteGlobal();
            return JsonConvert.SerializeObject(dataReporteCotizacionGlobal);
        }

        [System.Web.Mvc.HttpPost]
        public String EnviarEmailCotizacion(EnvioEmailCotizacion objEmailCotizacion)
        {
            RespuestaPost respuesta = new RespuestaPost();
            try
            {
                if (objEmailCotizacion.PlantillaCotizacion != null)
                {
                    objEmailCotizacion.PlantillaCotizacion.Base64 = objEmailCotizacion.PlantillaCotizacion.Base64.Replace("data:application/pdf;base64,", "");

                    var numeroDocumentoDirector = ServicioCotizacion.ObtenerDocumentoAsesor(Session["IdCotizacion"].ToString());
                    var correoDirector = ServicioCotizacion.ObtenerCorreoDirector(numeroDocumentoDirector);

                    var aplicacion = string.Format(@"{0}\{1}", ConfiguracionesGlobales.AplicacionWsUsuarios, "Cotizacion");
                    var rutaPlantillas = ConfigurationManager.AppSettings["RutaPlantillasEnvioCorreos"];
                    var nombrePlantilla = ConfigurationManager.AppSettings["NombrePlantillaCotizacion"];
                    var rutaPlantillaEmail = string.Format(@"{0}\{1}\{2}", rutaPlantillas, aplicacion, nombrePlantilla);

                    string body = Transversales.Administracion.Plantillas.GestionPlantillas.ObtenerHtmlPlantilla(rutaPlantillaEmail);
                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        body = body.Replace("@@IDRESERVA@@", Session["IdCotizacion"].ToString());
                        body = body.Replace("@@NOMBRECLIENTE@@", objEmailCotizacion.NombreEmpresa);
                        body = body.Replace("@@NIT@@", objEmailCotizacion.Nit);
                    }
                    else
                    {
                        body = string.Format("Se ha asociado a la reserva {0} correspondiente al cliente {1} de NIT {2} por parte de un asesor en Tu Portal. Ingrese por favor a tu portal para mayor detalle", Session["IdCotizacion"].ToString(), objEmailCotizacion.NombreEmpresa, objEmailCotizacion.Nit);
                    }

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtp = new SmtpClient
                    {
                        Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]),
                        Host = ConfigurationManager.AppSettings["SmtpHost"],
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SmtpCorreo"], ConfigurationManager.AppSettings["SmtpPassword"]),
                        Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpTimeout"])
                    };

                    var email = ConfigurationManager.AppSettings["CorreoPruebas"].ToString() == "1" ? ConfigurationManager.AppSettings["CorreoPrueba"] : correoDirector;

                    MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SmtpCorreo"], ConfigurationManager.AppSettings["NombreFromNameEmail"]);
                    MailAddress to = new MailAddress(email, "ToName");
                    MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                    myMail.Subject = ConfigurationManager.AppSettings["SubjectEmail"];
               
                    myMail.SubjectEncoding = System.Text.Encoding.UTF8;
                    myMail.IsBodyHtml = true;
                    myMail.Body = body;
                    myMail.BodyEncoding = System.Text.Encoding.UTF8;
                    myMail.Attachments.Add(new Attachment(new MemoryStream(Convert.FromBase64String(objEmailCotizacion.PlantillaCotizacion.Base64)), ConfigurationManager.AppSettings["NombreArchivoAdjunto"]));

                    smtp.Send(myMail);

                    respuesta.Respuesta = true;
                    respuesta.Mensaje = null;
                }
                else
                {
                    respuesta.Respuesta = false;
                    respuesta.Mensaje = "No se envio la información para el envió del correo eléctronico";
                }
            }
            catch (Exception e)
            {
                respuesta.Respuesta = false;
                respuesta.Mensaje = e.Message;
                //ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }

            return JsonConvert.SerializeObject(respuesta);
        }

        public ActionResult VisualizarCotizacion(int cotID)
        {
            var modelo = new RegistroVisitaModel();
            if (cotID != 0) Session["IdCotizacion"] = cotID;

            return View("VisualizarCotizacion", modelo);
        }

        #endregion 3). Cotización

        [System.Web.Mvc.HttpGet]
        public String ObtenerInfSedesCotizacion()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            var infSedes = ServicioCotizacion.ObtenerInfSedesCotizacion(Convert.ToInt32(Session["IdCotizacion"]));

            return JsonConvert.SerializeObject(infSedes);
        }
    }
}