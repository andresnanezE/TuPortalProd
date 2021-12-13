#region Importaciones

using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using EmermedicaReservaNit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Transversales.Administracion;
using Transversales.Administracion.IoC;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

#endregion Importaciones

namespace EmermedicaReservaNit.Controllers
{
    public class ReservaController : ApiController
    {
        private IServicioCotizacion _servicioCotizacion;

        private IServicioCotizacion ServicioCotizacion
        {
            get { return _servicioCotizacion ?? (_servicioCotizacion = FabricaIoC.Resolver<IServicioCotizacion>()); }
        }

        [HttpGet]
        public IHttpActionResult ConsultarNIT(Request objRequest)
        {
            Result response = new Result();
            response.Information = new InformationBusinness();
            List<string> lstMensajeReserva = new List<string>();

            try
            {
                List<Productos> lstProductosDisponibles = new List<Productos>();
                var productos = ServicioCotizacion.ObtenerProductos();
                string mensaje;
                int contad = 0;

                if (objRequest.Nit != null)
                {
                    var auxNIT = Utils.RemoverCaracteresEspeciales(objRequest.Nit);

                    if (auxNIT.Length == 9 || auxNIT.Length <= 9)
                    {
                        decimal NIT = Convert.ToDecimal(auxNIT);
                        var reciClient = ServicioCotizacion.ObtenerNombreXNIT(NIT);

                        if (reciClient != null)
                        {
                            if (reciClient.NOM_TERC != null)
                            {
                                response.Information.NombreEmpresa = reciClient.NOM_TERC;
                                response.Information.DigitoVerificacion = reciClient.DIG_VERI.ToString();

                                var permiNIT = Convert.ToDecimal(auxNIT);
                                response.Information.NombreCiudad = ServicioCotizacion.ObtenerCiudadAsesor(objRequest.UserId);
                                response.Information.NombreCanal = ServicioCotizacion.ObtenerCanalAsesor(objRequest.UserId);

                                if (response.Information.NombreCiudad == null && response.Information.NombreCanal == null)
                                {
                                    response.Information.NombreCanal = "";
                                    response.Information.NombreCiudad = "";
                                    lstMensajeReserva.Add("Ciudad y canal no disponible. Usuario inactivo");
                                }

                                var existenContrat = ServicioCotizacion.validarPermiContrato(permiNIT, objRequest.UserId);
                                if (existenContrat != null)
                                {
                                    if (existenContrat.canalAsesor != null && existenContrat.ciudadAsesor != null)
                                    {
                                        foreach (var t in existenContrat.productos)
                                        {
                                            lstMensajeReserva.Add("El NIT ingresado ya tiene un contrato vigente en Emermédica de tipo: " + t.ToString() +
                                                ", bajo el número de contrato: " + existenContrat.idContrato.ToString() + ", registrado por el asesor " + existenContrat.nombreAsesor +
                                                " cuyo director es " + existenContrat.nombreDirector + " de la ciudad de " + existenContrat.ciudadAsesor + " y el canal " + existenContrat.canalAsesor + ". Con una fecha de expiración del contrato, hasta el día " + existenContrat.fechaExpiracion.ToString());
                                        }
                                    }
                                    else
                                    {
                                        foreach (var t in existenContrat.productos)
                                        {
                                            lstMensajeReserva.Add("El NIT ingresado ya tiene un contrato vigente en Emermédica de tipo: " + t.ToString() +
                                                ", bajo el número de contrato: " + existenContrat.idContrato.ToString() + ", registrado por el asesor " + existenContrat.nombreAsesor +
                                                " cuyo director es " + existenContrat.nombreDirector + ". Con una fecha de expiración del contrato, hasta el día " + existenContrat.fechaExpiracion.ToString());
                                        }
                                    }

                                    //Validar adicional de si  hay reservas vigentes
                                    var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(auxNIT, objRequest.UserId);

                                    //Validar Si tiene productos disponibles.
                                    var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                    var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                    if (productosDisponibles.Count > 0)
                                    {
                                        if (nitClient.Count > 0)
                                        {
                                            lstProductosDisponibles = productosDisponibles;
                                            foreach (var n in nitClient)
                                            {
                                                var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                                if (n.AsesorID != objRequest.UserId)
                                                {
                                                    lstMensajeReserva.Add(string.Format("El NIT {0} ya se encuentra bloqueado en la reserva con ID {1} por el asesor {2}  cuyo director es {3}, hasta el día {4} con el producto {5} en la ciudad de {6}.", objRequest.Nit, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad));
                                                    contad++;
                                                }
                                                else
                                                {
                                                    lstMensajeReserva.Add(string.Format("El NIT {0} ya se encuentra bloqueado en la reserva con ID {1} por usted hasta el día {2} con el producto {3}.", objRequest.Nit, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                    contad++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lstMensajeReserva.Add(string.Format("El NIT {0} no cuenta con productos disponibles para reservar", objRequest.Nit));
                                    }
                                }
                                else
                                {
                                    lstMensajeReserva.Add("El NIT ingresado no tiene contratos ni bloqueos asociados. Por favor continúe ingresando la información del formulario.");

                                    var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(auxNIT, objRequest.UserId);
                                    if (nitClient.Count > 0)
                                    {
                                        mensaje = null;

                                        //Validar Si tiene productos disponibles.
                                        var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                        var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                        if (productosDisponibles.Count > 0)
                                        {
                                            lstProductosDisponibles = productosDisponibles;
                                            foreach (var n in nitClient)
                                            {
                                                var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);
                                                if (n.AsesorID != objRequest.UserId)
                                                {
                                                    lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {5} con el producto {6} en la ciudad de {7}.", mensaje, objRequest.Nit, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad));
                                                    contad++;
                                                }
                                                else
                                                {
                                                    lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", mensaje, objRequest.Nit, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                    contad++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lstMensajeReserva.Add(string.Format("El NIT {0} no cuenta con productos disponibles para reservar", objRequest.Nit));
                                        }
                                    }
                                }
                            }
                            if (reciClient.NOM_TERC == null)
                            {
                                response.Information.NombreCiudad = ServicioCotizacion.ObtenerCiudadAsesor(objRequest.UserId);
                                response.Information.NombreCanal = ServicioCotizacion.ObtenerCanalAsesor(objRequest.UserId);

                                var permiNIT = Convert.ToDecimal(auxNIT);
                                var existenContrat = ServicioCotizacion.validarPermiContrato(permiNIT, objRequest.UserId);

                                if (response.Information.NombreCiudad == null && response.Information.NombreCanal == null)
                                {
                                    response.Information.NombreCanal = "";
                                    response.Information.NombreCiudad = "";
                                    lstMensajeReserva.Add("Ciudad y canal no disponible. Usuario inactivo");
                                }

                                var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(auxNIT, objRequest.UserId);

                                if (reciClient.DIG_VERI != -1 && existenContrat == null && nitClient.Count <= 0)
                                {
                                    mensaje = null;
                                    response.Information.DigitoVerificacion = reciClient.DIG_VERI.ToString();
                                    lstMensajeReserva.Add("No se encontró registros ni bloqueos con el NIT digitado, por favor complete toda la información del siguiente formulario");
                                }
                                if (reciClient.DIG_VERI == -1)
                                {
                                    mensaje = null;
                                    lstMensajeReserva.Add("El NIT ingresado no es válido, por favor verifica e intenta nuevamente");
                                    ModelState.AddModelError(string.Empty, "El NIT ingresado no es válido, por favor verifica e intenta nuevamente");
                                }

                                //Validar NIT, que tenga contratos vigentes y reservas vigentes
                                if (reciClient.DIG_VERI != -1 && existenContrat != null && nitClient.Count > 0)
                                {
                                    mensaje = null;
                                    response.Information.DigitoVerificacion = reciClient.DIG_VERI.ToString();
                                    foreach (var t in existenContrat.productos)
                                    {
                                        lstMensajeReserva.Add("El NIT ingresado ya tiene un contrato vigente en Emermédica de tipo: " + t.ToString() +
                                            ", bajo el número de contrato: " + existenContrat.idContrato.ToString() + ", registrado por el asesor " + existenContrat.nombreAsesor +
                                            " cuyo director es " + existenContrat.nombreDirector + " de la ciudad de " + existenContrat.ciudadAsesor + " y el canal " + existenContrat.canalAsesor + " Con una fecha de expiración del contrato, hasta el día " + existenContrat.fechaExpiracion.ToString());
                                    }

                                    //Validar Si tiene productos disponibles.
                                    var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                    var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                    if (productosDisponibles.Count > 0)
                                    {
                                        lstProductosDisponibles = productosDisponibles;
                                        response.Information.NombreEmpresa = ServicioCotizacion.ObtenerNombreClienteXNIT(Convert.ToDecimal(auxNIT));

                                        foreach (var n in nitClient)
                                        {
                                            //Nombre del producto
                                            var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                            //Validar si ya existe el producto en el NIT a reservar.
                                            if (prodsClient == n.NombreProducto.ToString())
                                            {
                                                if (n.AsesorID != Convert.ToInt32(objRequest.Nit))
                                                {
                                                    lstMensajeReserva.Add(string.Format("El NIT {0} ya se encuentra bloqueado en la reserva con ID {1} por el asesor {2} cuyo director es {3}, hasta el día {4} con el producto {5} en la ciudad de {6}.", objRequest.Nit, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad));
                                                    contad++;
                                                }
                                                else
                                                {
                                                    lstMensajeReserva.Add(string.Format("El NIT {0} ya se encuentra bloqueado en la reserva con ID {1} por usted hasta el día {2} con el producto {3}.", objRequest.Nit, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                    contad++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lstMensajeReserva.Add(string.Format("El NIT {0} no cuenta con productos disponibles para reservar", objRequest.Nit));
                                    }
                                }
                                if (reciClient.DIG_VERI != -1 && existenContrat == null && nitClient.Count > 0)
                                {
                                    mensaje = null;

                                    //Validar Si tiene productos disponibles.
                                    var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                    var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                    if (productosDisponibles.Count > 0)
                                    {
                                        lstProductosDisponibles = productosDisponibles;
                                        response.Information.NombreEmpresa = ServicioCotizacion.ObtenerNombreClienteXNIT(Convert.ToDecimal(auxNIT));

                                        foreach (var n in nitClient)
                                        {
                                            response.Information.DigitoVerificacion = reciClient.DIG_VERI.ToString();
                                            var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                            //Validar si ya existe el producto en el NIT a reservar.
                                            if (prodsClient == n.NombreProducto.ToString())
                                            {
                                                if (n.AsesorID != objRequest.UserId)
                                                {
                                                    if (prodsClient != null)
                                                    {
                                                        lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {5} con el producto {6} en la ciudad de {7}.", mensaje, objRequest.Nit, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad));
                                                        contad++;
                                                    }
                                                    else
                                                    {
                                                        lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {3} en la ciudad de {4}.", mensaje, objRequest.Nit, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, n.ciudad));
                                                        contad++;
                                                    }
                                                }
                                                if (n.AsesorID == objRequest.UserId)
                                                {
                                                    if (prodsClient != null)
                                                    {
                                                        lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", mensaje, objRequest.Nit, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                        contad++;
                                                    }
                                                    else
                                                    {
                                                        lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3}.", mensaje, objRequest.Nit, n.CotizacionID, n.fechaExpiracion));
                                                        contad++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lstMensajeReserva.Add(string.Format("El NIT {0} no cuenta con productos disponibles para reservar", objRequest.Nit));
                                    }
                                }
                            }
                        }
                        else
                        {
                            lstMensajeReserva.Add("El NIT ingresado no se encuentra registrado, por favor ingresa el nombre de la empresa cliente");
                        }
                    }
                    else
                    {
                        if (auxNIT.Length > 9)
                        {
                            var permiNIT = Convert.ToDecimal(auxNIT);
                            var existenContrat = ServicioCotizacion.validarPermiContrato(permiNIT, objRequest.UserId);

                            response.Information.NombreCiudad = ServicioCotizacion.ObtenerCiudadAsesor(objRequest.UserId);
                            response.Information.NombreCanal = ServicioCotizacion.ObtenerCanalAsesor(objRequest.UserId);

                            response.Information.DigitoVerificacion = "0";
                            if (response.Information.NombreCiudad != null && response.Information.NombreCanal != null)
                            {
                                response.Information.NombreCanal = "";
                                response.Information.NombreCiudad = "";
                                lstMensajeReserva.Add("Ciudad y canal no disponible. Usuario inactivo");
                            }

                            var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(auxNIT, objRequest.UserId);
                            if (existenContrat == null && nitClient.Count <= 0)
                            {
                                mensaje = null;
                                lstMensajeReserva.Add("No se encontró registros ni bloqueos con el NIT digitado por favor complete toda la información del formulario");
                            }
                            if (existenContrat != null && nitClient.Count > 0)
                            {
                                mensaje = null;

                                //Validar Si tiene productos disponibles.
                                var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                if (productosDisponibles.Count > 0)
                                {
                                    lstProductosDisponibles = productosDisponibles;
                                    response.Information.NombreEmpresa = ServicioCotizacion.ObtenerNombreClienteXNIT(Convert.ToDecimal(auxNIT));

                                    foreach (var t in existenContrat.productos)
                                    {
                                        lstMensajeReserva.Add("El NIT ingresado ya tiene un contrato vigente en Emermédica de tipo: " + t.ToString() +
                                            ", bajo el número de contrato: " + existenContrat.idContrato.ToString() + ", registrado por el asesor " + existenContrat.nombreAsesor +
                                            " cuyo director es " + existenContrat.nombreDirector + " de la ciudad de " + existenContrat.ciudadAsesor + " y el canal " + existenContrat.canalAsesor + " Con una fecha de expiración del contrato, hasta el día " + existenContrat.fechaExpiracion.ToString());
                                    }
                                    foreach (var n in nitClient)
                                    {
                                        var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                        //Validar si ya existe el producto en el NIT a reservar.
                                        if (prodsClient == n.NombreProducto.ToString())
                                        {
                                            if (n.AsesorID != objRequest.UserId)
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {5} con los productos {6} en la ciudad de {7}.", mensaje, objRequest.Nit, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad));
                                                contad++;
                                            }
                                            else
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con los productos {4}.", mensaje, objRequest.Nit, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                contad++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lstMensajeReserva.Add(string.Format("El NIT {0} no cuenta con productos disponibles para reservar", objRequest.Nit));
                                }
                            }
                            if (existenContrat == null && nitClient.Count > 0)
                            {
                                mensaje = null;

                                //Validar Si tiene productos disponibles.
                                var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                if (productosDisponibles.Count > 0)
                                {
                                    lstProductosDisponibles = productosDisponibles;
                                    response.Information.NombreEmpresa = ServicioCotizacion.ObtenerNombreClienteXNIT(Convert.ToDecimal(auxNIT));

                                    foreach (var n in nitClient)
                                    {
                                        //model.mensaje = null;
                                        var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                        //Validar si ya existe el producto en el NIT a reservar.
                                        if (prodsClient == n.NombreProducto.ToString())
                                        {
                                            if (n.AsesorID != objRequest.UserId)
                                            {
                                                if (prodsClient != null)
                                                {
                                                    lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {5} con el producto {6} en la ciudad de {7}.", mensaje, objRequest.Nit, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad));
                                                    contad++;
                                                }
                                                else
                                                {
                                                    lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {3} en la ciudad de {4}.", mensaje, objRequest.Nit, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, n.ciudad));
                                                    contad++;
                                                }
                                            }
                                            if (n.AsesorID == objRequest.UserId)
                                            {
                                                if (prodsClient != null)
                                                {
                                                    lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", mensaje, objRequest.Nit, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                    contad++;
                                                }
                                                else
                                                {
                                                    lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3}.", mensaje, objRequest.Nit, n.CotizacionID, n.fechaExpiracion));
                                                    contad++;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lstMensajeReserva.Add(string.Format("El NIT {0} no cuenta con productos disponibles para reservar", objRequest.Nit));
                                }
                            }
                        }
                    }
                }

                response.Information.ProductosDisponibles = lstProductosDisponibles;
                response.IsSuccess = true;
                response.ValidationList = lstMensajeReserva;

                return Ok(response);
            }
            catch (Exception e)
            {
                // string msjRet = "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.";

                response = new Result
                {
                    IsSuccess = false,
                    Message = e.Message.ToString()
                };

                return Content(HttpStatusCode.BadRequest, response);
            }
        }

        [HttpPost]
        public IHttpActionResult GuardarReserva(DataClient objRequest)
        {
            Result response = new Result();
            Cotizaciones objCotizacion = new Cotizaciones();

            try
            {
                if (objRequest != null)
                {
                    objCotizacion.NIT = objRequest.Nit;
                    objCotizacion.DV = objRequest.ValidationDigit;
                    objCotizacion.nombreEmpresa = objRequest.CompanyName;
                    objCotizacion.contacto = objRequest.ContactName;
                    objCotizacion.telefono = objRequest.NumberPhone;
                    objCotizacion.celular = objRequest.MobilePhone;
                    objCotizacion.cargo = objRequest.ContactCharge;
                    objCotizacion.correoElectronico = objRequest.Email;
                    objCotizacion.ciudad = objRequest.CityName;
                    objCotizacion.canal = objRequest.ChannelName;
                    objCotizacion.TelefonoExt = objRequest.Extension;
                    objCotizacion.ObservacionReserva = objRequest.Annotation;

                    if (objRequest.ChannelName == null) objRequest.ChannelName = "";
                    if (objRequest.CityName == null) objRequest.CityName = "";

                    var productos = ServicioCotizacion.ObtenerProductos();

                    List<int> listIdProd = new List<int>();
                    listIdProd.Add(objRequest.ProductId);

                    var auxiNIT = Utils.RemoverCaracteresEspeciales(objRequest.Nit.ToString());
                    var produsClient = ServicioCotizacion.ValidarPermisoRegistroPorProducto(auxiNIT, objRequest.UserId, listIdProd.ToArray(), objRequest.CityName);

                    if (produsClient.Count <= 0 || produsClient == null)
                    {
                        var auxNIT = Convert.ToInt32(auxiNIT);
                        var contrat = ServicioCotizacion.validarPermisoContrato(auxNIT, objRequest.UserId, objRequest.ProductId, objRequest.CityName);

                        if (contrat == false)
                        {
                            if (objRequest.ProductId != 0)
                            {
                                response.IsSuccess = ServicioCotizacion.RegistrarCotizacion(objCotizacion, listIdProd.ToArray(), objRequest.UserId);
                                if (response.IsSuccess)
                                {
                                    response.Message = "Tu cliente ha sido creado exitosamente y ahora podrás generar una cotización o consultarlo  después en la sección Mis Reservas";
                                }
                                else
                                {
                                    response.IsSuccess = false;
                                    response.Message = "Error en el registro. Verifica la información digitada e intentalo nuevamente";
                                }
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.Message = "Seleccione al menos un producto por favor";
                            }
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "¡ATENCIÓN! El cliente " + objRequest.CompanyName + " tiene un contrato vigente con el producto seleccionado";
                        }
                    }
                    else
                    {
                        foreach (var index in produsClient)
                        {
                            response.IsSuccess = false;
                            response.Message = string.Format("¡ATENCIÓN! El NIT digitado, en la reserva {0} ya ha sido bloqueado previamente con el producto {1} en la  ciudad de {2}", index.CotizacionID, index.nombreProducto, index.ciudadCotiz);
                        }
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Digita la información requerida";
                }

                return Ok(response);
            }
            catch (Exception e)
            {
                response = new Result
                {
                    IsSuccess = false,
                    Message = e.Message.ToString()
                };

                return Content(HttpStatusCode.BadRequest, response);
            }
        }
    }
}