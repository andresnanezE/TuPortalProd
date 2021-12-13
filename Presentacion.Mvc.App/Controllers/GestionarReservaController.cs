using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Collections.Generic;
using Presentacion.Mvc.App.Models;
using System.Web.Script.Serialization;
using Transversales.Administracion.IoC;
using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using System.Configuration;

namespace Presentacion.Mvc.App.Controllers
{
    public class GestionarReservaController : Controller
    {
        private IServicioCotizacion _servicioCotizacion;

        private IServicioCotizacion ServicioCotizacion
        {
            get { return _servicioCotizacion ?? (_servicioCotizacion = FabricaIoC.Resolver<IServicioCotizacion>()); }
        }

        public ActionResult Index(int cotID)
        {
            var model = new GestionarReservaModel();
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            var roles = identity.FindFirst(ClaimTypes.Role).Value;
            ViewBag.MaxReconsideraciones = ConfigurationManager.AppSettings["MaximoReconsideraciones"];
            ViewBag.MaxRenovaciones = ServicioCotizacion.ObtenerMaximoRenovaciones();

            try
            {
                model.lstIdRol = roles.Split(',').Where(r => !string.IsNullOrEmpty(r)).ToList();
                //Información del cliente.
                if (cotID != 0) model.IdCotizacion = cotID.ToString();
                Session["IdCotizacion"] = cotID;

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
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }

            return View("Index", model);
        }

        [HttpPost]
        public string Index(GuardarLiberarRenovar reconsideracion)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            var roles = identity.FindFirst(ClaimTypes.Role).Value;
            var lstRoles = roles.Split(',');
            var respuesta = new RespuestaLiberarReserva();
            
            var sedes = reconsideracion.Sedes.Select(s => new Dominio.Administracion.Entidades.MapperDto.SedeReconsideracionDto()
            {
                SedeId = s.Id,
                Valor = s.Valor,
                ValorReconsideracion = s.ValorReconsideracion.Value,
                Nombre = s.NombreSede
            });

            var reconsideracionDto = new Dominio.Administracion.Entidades.MapperDto.ReconsideracionCotizacionDto()
            {
                UserId = userId,
                NotaReconsideracion = reconsideracion.NotaLiberarRenovar,
                CotizacionId = reconsideracion.Sedes.FirstOrDefault().Id_Cotizacion,
                Sedes = sedes,
                RolId = lstRoles.Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray(),
                DetalleReconsideracion = JsonConvert.SerializeObject(sedes.Where(s => s.Valor != s.ValorReconsideracion))
            };
            if (ServicioCotizacion.EnviarCotizacionAReconsideracion(reconsideracionDto))
            {
                respuesta.Respuesta = true;
            }
            else
            {
                respuesta.Respuesta = false;
                respuesta.Mensaje = "La reserva no ha podido ser enviada a reconsideración. Revisa los datos y vuelve a intentarlo.";
            }

            return JsonConvert.SerializeObject(respuesta);
        }

        [HttpPost]
        public string AprobarRechazarReconsideracion(GuardarLiberarRenovar reconsideracion)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            var roles = identity.FindFirst(ClaimTypes.Role).Value;
            var lstRoles = roles.Split(',');
            var respuesta = new RespuestaLiberarReserva();

            var reconsideracionDto = new Dominio.Administracion.Entidades.MapperDto.ReconsideracionCotizacionDto()
            {
                UserId = userId,
                NotaReconsideracion = reconsideracion.NotaLiberarRenovar,
                CotizacionId = reconsideracion.Sedes.FirstOrDefault().Id_Cotizacion,
                RolId = lstRoles.Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray()
            };
            var exitoso = reconsideracion.Aprobar ?
                ServicioCotizacion.AprobarReconsideracion(reconsideracionDto) :
                ServicioCotizacion.RechazarReconsideracion(reconsideracionDto);

            if (exitoso)
            {
                respuesta.Respuesta = true;
            }
            else
            {
                respuesta.Respuesta = false;
                respuesta.Mensaje = "No se ha realizado la operación.";
            }

            return JsonConvert.SerializeObject(respuesta);

        }
        [HttpGet]
        public String ObtenerNotasCotizacion()
        {
            var notas = ServicioCotizacion.ObtenerNotasCotizacion(Convert.ToInt32(Session["IdCotizacion"]));

            return JsonConvert.SerializeObject(notas);
        }

        [HttpGet]
        public string ObtenerInfSedesCotizacion()
        {
            var infSedes = ServicioCotizacion.ObtenerInfSedesCotizacion(Convert.ToInt32(Session["IdCotizacion"]));
            return JsonConvert.SerializeObject(infSedes);
        }

        [HttpPost]
        public string LiberarReserva(GuardarLiberarRenovar objLiberarRenovar)
        {
            var seriarlizer = new JavaScriptSerializer();
            var nit = "";
            RespuestaLiberarReserva objRespuesta = new RespuestaLiberarReserva();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var asesorID = ServicioCotizacion.ObtenerIDAsesor(Convert.ToInt32(Session["IdCotizacion"]));
                var nombreAseor = ServicioCotizacion.ObtenerNombreAsesor(asesorID);

                var infoReserva = ServicioCotizacion.ObtenerDatosReserva(Convert.ToInt32(Session["IdCotizacion"]), userId);
                if (infoReserva != null)
                {
                    foreach (var reser in infoReserva)
                    {
                        nit = reser.NIT.ToString();
                    }
                }

                var libReser = ServicioCotizacion.liberarReserva(Session["IdCotizacion"].ToString(), objLiberarRenovar.NotaLiberarRenovar, 0, userId);
                if (libReser == true)
                {
                    objRespuesta.Respuesta = true;
                    if (userId == asesorID)
                    {
                        objRespuesta.Mensaje = "Se ha liberado la reserva";
                    }
                    else if (userId != asesorID)
                    {
                        objRespuesta.Mensaje = "Se ha liberado la reserva del cliente " + nit + " perteneciente al asesor " + nombreAseor;
                    }
                }
                else
                {
                    objRespuesta.Respuesta = false;
                    objRespuesta.Mensaje = "La reserva ya se ha liberado previamente";
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }

            return seriarlizer.Serialize(objRespuesta);
        }

        [HttpPost]
        public string RenovarReservas(GuardarLiberarRenovar objLiberarRenovar)
        {
            var seriarlizer = new JavaScriptSerializer();
            var nit = "";
            RespuestaLiberarReserva objRespuesta = new RespuestaLiberarReserva();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var asesorID = ServicioCotizacion.ObtenerIDAsesor(Convert.ToInt32(Convert.ToInt32(Session["IdCotizacion"])));
                var nombreAseor = ServicioCotizacion.ObtenerNombreAsesor(asesorID);
                var renovReser = ServicioCotizacion.renovarReserva(Session["IdCotizacion"].ToString(), objLiberarRenovar.NotaLiberarRenovar, 0, userId);
                var infoReserva = ServicioCotizacion.ObtenerDatosReserva(Convert.ToInt32(Session["IdCotizacion"]), userId);

                if (infoReserva != null)
                {
                    foreach (var reser in infoReserva)
                    {
                        nit = reser.NIT.ToString();
                    }
                }
                if (renovReser == true)
                {
                    objRespuesta.Respuesta = true;
                    if (userId == asesorID)
                    {
                        objRespuesta.Mensaje = "Se ha renovado la reserva";
                    }
                    else if (userId != asesorID)
                    {
                        objRespuesta.Mensaje = "Se ha renovado la reserva del cliente " + nit + " perteneciente al asesor " + nombreAseor;
                    }
                }
                else
                {
                    objRespuesta.Respuesta = true;
                    objRespuesta.Mensaje = "No se ha podido renovar la reserva.La reserva no se encuentra disponible para ser renovada";
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }

            return seriarlizer.Serialize(objRespuesta);
        }

        [HttpGet]
        public string ObtenerIdCotizacion()
        {
            var seriarlizer = new JavaScriptSerializer();
            var idCotizacion = seriarlizer.Serialize(Session["IdCotizacion"]);

            return idCotizacion;
        }

        [HttpGet]
        public string ObtenerInformacionReserva()
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

        //[System.Web.Mvc.HttpGet]
        //public String ObtenerInfSedesCotizacion()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
        //    var infSedes = ServicioCotizacion.ObtenerInfSedesCotizacion(Convert.ToInt32(Session["IdCotizacion"]));

        //    return JsonConvert.SerializeObject(infSedes);
        //}

        //[HttpGet]
        //public String ObtenerNotasCotizacion()
        //{
        //    var notas = ServicioCotizacion.ObtenerNotasCotizacion(Convert.ToInt32(Session["IdCotizacion"]));

        //    return JsonConvert.SerializeObject(notas);
        //}
    }
}