using Aplicacion.Administracion.Contratos;
using AutoMapper;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using Presentacion.Mvc.App.Models;
using Presentacion.Mvc.App.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReservaNitController : Controller
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

        public ActionResult Index(string msg)
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;

            ViewBag.Mensaje = msg;
            UserClaims.ResetUserClaimsSession();

            var modelo = new RegistroVisitaModel();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var prods = ServicioCotizacion.ObtenerProductos();
                Mapper.CreateMap<ProductosSelector, Dominio.Administracion.Entidades.ModeloCotizacion.Productos>().ReverseMap();
                var productosSelect = Mapper.Map<List<ProductosSelector>>(prods);

                modelo.productos = productosSelect;
                modelo.productosIds = modelo.productoSelectID.ToString().Select(o => Convert.ToInt32(o)).ToArray();
                var productos = ServicioCotizacion.ObtenerProductos();
                modelo.listadoProductos = productos;
                modelo.lstMensajeReserva = new List<string>();
                modelo.validarModalReserva = false;
                modelo.validarFormularioReserva = false;

                var ciudadUsuar = ServicioCotizacion.ObtenerCiudadAsesor(userId);
                var canalUsuar = ServicioCotizacion.ObtenerCanalAsesor(userId);
                if (ciudadUsuar != null && canalUsuar != null)
                {
                    modelo.ciudad = ciudadUsuar;
                    modelo.canal = canalUsuar;
                }
                else
                {
                    modelo.ciudad = "Ciudad no disponible.";
                    modelo.canal = "Canal no disponible.";
                    modelo.mensaje = "Ciudad y canal no disponible. Usuario inactivo";
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(modelo);
        }

        [HttpPost]
        public ActionResult Index(RegistroVisitaModel model)
        {
            try
            {
                bool registro = false;
                var prods = ServicioCotizacion.ObtenerProductos();
                Mapper.CreateMap<ProductosSelector, Productos>().ReverseMap();

                var productosSelect = Mapper.Map<List<ProductosSelector>>(prods);
                model.productos = productosSelect;
                model.lstMensajeReserva = new List<string>();

                //Obtener el idProducto, ya que se esta guardo es el nombre en el modelo.
                foreach (var item in productosSelect)
                {
                    if (item.NombreProducto == model.productoSelectName)
                    {
                        model.productoSelectID = item.COD_SERV;
                    }
                }

                List<int> listIdProd = new List<int>();
                listIdProd.Add(model.productoSelectID);

                model.productosIds = listIdProd.ToArray();

                var modelProducto = productosSelect.Where(l => l.id_producto == model.productoSelectID).FirstOrDefault();

                var productos = ServicioCotizacion.ObtenerProductos();
                model.listadoProductos = productos;
                if (model.celular.Length > 10)
                {
                    model.msgErrorReservaNit = "Los  números celulares no pueden superar 10 dígitos.";
                    model.validMsgErrorReservaNit = 1;
                }
                if (ModelState.IsValid)
                {
                    var userName = ViewData["CurrentUser"] == null ? null : ViewData["CurrentUser"].ToString();

                    //int userID = 0;
                    var identity = (ClaimsIdentity)User.Identity;
                    var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                    if (model.ciudad == null) model.ciudad = "";

                    var auxiNIT = Utils.RemoverCaracteresEspeciales(model.NIT);
                    var produsClient = ServicioCotizacion.ValidarPermisoRegistroPorProducto(auxiNIT, userId, model.productosIds, model.ciudad);
                    model.NIT = auxiNIT;

                    if (produsClient.Count <= 0 || produsClient == null)
                    {
                        var auxNIT = Convert.ToInt32(auxiNIT);
                        var contrat = ServicioCotizacion.validarPermisoContrato(auxNIT, userId, model.productoSelectID, model.ciudad);

                        if (contrat == false)
                        {
                            Mapper.CreateMap<Cotizaciones, RegistroVisitaModel>().ReverseMap();
                            var cotizacion = Mapper.Map<Cotizaciones>(model);

                            if (cotizacion.canal == null) cotizacion.canal = "";
                            if (cotizacion.ciudad == null) cotizacion.ciudad = "";

                            if (model.productosIds != null)
                            {
                                registro = ServicioCotizacion.RegistrarCotizacion(cotizacion, model.productosIds, userId);
                                if (registro)
                                {
                                    model.mensaje = "Tu cliente ha sido creado exitosamente y ahora podrás generar una cotización o consultarlo  después en la sección Mis Reservas.";
                                    ViewBag.ModalOpen = true;
                                    return View(model);
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Error en el registro. Verifica la información digitada e intentalo nuevamente.");
                                }
                            }
                            else
                            {
                                model.msgErrorReservaNit = "Seleccione al menos un producto por favor.";
                                model.validMsgErrorReservaNit = 1;
                            }
                        }
                        else
                        {
                            model.validMsgErrorReservaNit = 1;
                            model.msgErrorReservaNit = "¡ATENCIÓN! El cliente " + model.nombreEmpresa + " tiene un contrato vigente con el producto seleccionado.";
                        }
                    }
                    else
                    {
                        foreach (var index in produsClient)
                        {
                            model.validMsgErrorReservaNit = 1;
                            model.mensaje = string.Format("{0}|¡ATENCIÓN! El NIT digitado, en la reserva {1} ya ha sido bloqueado previamente con el (los) producto(s) {2} en la  ciudad de {3}", model.mensaje, index.CotizacionID, index.nombreProducto, index.ciudadCotiz);
                        }
                    }

                    model.validarModalReserva = false;
                }
                else
                {
                    model.validMsgErrorReservaNit = 1;
                    model.msgErrorReservaNit = "Error. Verifique que todos los campos obligatorios hayan sido completados correctamente.";

                    //ModelState.AddModelError(string.Empty, "Error. Verifique que todos los campos obligatorios hayan sido completados correctamente.");
                }
            }
            catch (Exception e)
            {
                model.validMsgErrorReservaNit = 1;
                model.msgErrorReservaNit = "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.";
            }
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult ConsultarNIT(RegistroVisitaModel model)
        {
            List<string> lstMensajeReserva = new List<string>();

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var ciudadUsuar = ServicioCotizacion.ObtenerCiudadAsesor(userId);

                var prods = ServicioCotizacion.ObtenerProductos();
                int contad = 0;
                var nitEsValido = true;

                Mapper.CreateMap<ProductosSelector, Dominio.Administracion.Entidades.ModeloCotizacion.Productos>().ReverseMap();
                var productosSelect = Mapper.Map<List<ProductosSelector>>(prods);
                model.productos = productosSelect;

                var productos = ServicioCotizacion.ObtenerProductos();
                model.listadoProductos = productos;

                if (model.NIT != null)
                {
                    var auxNIT = Utils.RemoverCaracteresEspeciales(model.NIT);
                    model.NIT = auxNIT;
                    if (auxNIT.Length == 9 || auxNIT.Length <= 9)
                    {
                        decimal NIT = Convert.ToDecimal(auxNIT);
                        var reciClient = ServicioCotizacion.ObtenerNombreXNIT(NIT);
                        if (reciClient != null)
                        {
                            if (reciClient.NOM_TERC != null)
                            {
                                model.nombreEmpresa = reciClient.NOM_TERC;
                                model.DV = reciClient.DIG_VERI;
                                var permiNIT = Convert.ToDecimal(auxNIT);

                                var canalUsuar = ServicioCotizacion.ObtenerCanalAsesor(userId);
                                if (ciudadUsuar != null && canalUsuar != null)
                                {
                                    model.ciudad = ciudadUsuar;
                                    model.canal = canalUsuar;
                                }
                                else
                                {
                                    model.canal = "";
                                    model.ciudad = "";
                                    model.mensaje = "Ciudad y canal no disponible. Usuario inactivo";
                                }

                                var existenContrat = ServicioCotizacion.validarPermiContrato(permiNIT, userId);

                                if (existenContrat != null)
                                {
                                    if (existenContrat.canalAsesor != null && existenContrat.ciudadAsesor != null)
                                    {
                                        foreach (var t in existenContrat.productos)
                                        {
                                            lstMensajeReserva.Add("El NIT ingresado ya tiene un contrato vigente en Emermédica de tipo: " + t.ToString() +
                                                ", bajo el número de contrato: " + existenContrat.idContrato.ToString() + ", registrado por el asesor " + existenContrat.nombreAsesor +
                                                " cuyo director es " + existenContrat.nombreDirector + " de la ciudad de " + existenContrat.ciudadAsesor + " y el canal " + existenContrat.canalAsesor + ". Con una fecha de expiración del contrato, hasta el día " + existenContrat.fechaExpiracion.ToString() + ".");
                                        }
                                    }
                                    else
                                    {
                                        foreach (var t in existenContrat.productos)
                                        {
                                            lstMensajeReserva.Add("El NIT ingresado ya tiene un contrato vigente en Emermédica de tipo: " + t.ToString() +
                                                ", bajo el número de contrato: " + existenContrat.idContrato.ToString() + ", registrado por el asesor " + existenContrat.nombreAsesor +
                                                " cuyo director es " + existenContrat.nombreDirector + ". Con una fecha de expiración del contrato, hasta el día " + existenContrat.fechaExpiracion.ToString() + ".");
                                        }
                                    }

                                    //Validar adicional de si  hay reservas vigentes
                                    var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(auxNIT, userId);

                                    //Validar Si tiene productos disponibles.
                                    var productosDisponibles = ServicioCotizacion.ObtenerProductosDisponiblesParaNit(permiNIT, model.ciudad);

                                    if (nitClient.Count > 0)
                                    {
                                        model.validarFormularioReserva = false;
                                        model.listadoProductos = productosDisponibles;

                                        foreach (var n in nitClient)
                                        {
                                            var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                            if (n.AsesorID != userId)
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3}  cuyo director es {4}, hasta el día {5} con el producto {6} en la ciudad de {7}.", model.mensaje, model.NIT, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad, "."));
                                                contad++;
                                            }
                                            else
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient, "."));
                                                contad++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(auxNIT, userId);
                                    if (nitClient.Count > 0)
                                    {
                                        model.mensaje = null;

                                        //Validar Si tiene productos disponibles.
                                        var productosDisponibles = ServicioCotizacion.ObtenerProductosDisponiblesParaNit(permiNIT, model.ciudad);

                                        model.validarFormularioReserva = false;
                                        model.listadoProductos = productosDisponibles;
                                        foreach (var n in nitClient)
                                        {
                                            var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                            if (n.AsesorID != userId)
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {5} con el producto {6} en la ciudad de {7}.", model.mensaje, model.NIT, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad, "."));
                                                contad++;
                                            }
                                            else
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient, "."));
                                                contad++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (reciClient.NOM_TERC == null)
                        {
                            var canalUsuar = ServicioCotizacion.ObtenerCanalAsesor(userId);
                            var permiNIT = Convert.ToDecimal(auxNIT);
                            var existenContrat = ServicioCotizacion.validarPermiContrato(permiNIT, userId);

                            if (ciudadUsuar != null && canalUsuar != null)
                            {
                                model.ciudad = ciudadUsuar;
                                model.canal = canalUsuar;
                            }
                            else
                            {
                                model.canal = "";
                                model.ciudad = "";
                                lstMensajeReserva.Add("Ciudad y canal no disponible. Usuario inactivo");
                                nitEsValido = false;
                            }

                            var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(auxNIT, userId);

                            if (reciClient.DIG_VERI != -1 && existenContrat == null && nitClient.Count <= 0)
                            {
                                model.mensaje = null;
                                model.DV = reciClient.DIG_VERI;
                            }
                            if (reciClient.DIG_VERI == -1)
                            {
                                model.mensaje = null;
                                lstMensajeReserva.Add("El NIT ingresado no es válido, por favor verifica e intenta nuevamente.");
                                ModelState.AddModelError(string.Empty, "El NIT ingresado no es válido, por favor verifica e intenta nuevamente.");
                                nitEsValido = false;
                            }

                            //Validar NIT, que tenga contratos vigentes y reservas vigentes
                            if (reciClient.DIG_VERI != -1 && existenContrat != null && nitClient.Count > 0)
                            {
                                model.mensaje = null;
                                model.DV = reciClient.DIG_VERI;
                                foreach (var t in existenContrat.productos)
                                {
                                    lstMensajeReserva.Add("El NIT ingresado ya tiene un contrato vigente en Emermédica de tipo: " + t.ToString() +
                                        ", bajo el número de contrato: " + existenContrat.idContrato.ToString() + ", registrado por el asesor " + existenContrat.nombreAsesor +
                                        " cuyo director es " + existenContrat.nombreDirector + " de la ciudad de " + existenContrat.ciudadAsesor + " y el canal " + existenContrat.canalAsesor + " Con una fecha de expiración del contrato, hasta el día " + existenContrat.fechaExpiracion.ToString() + ".");
                                }

                                //Validar Si tiene productos disponibles.
                                var productosDisponibles = ServicioCotizacion.ObtenerProductosDisponiblesParaNit(permiNIT, model.ciudad);

                                model.listadoProductos = productosDisponibles;
                                model.validarFormularioReserva = false;
                                model.nombreEmpresa = ServicioCotizacion.ObtenerNombreClienteXNIT(Convert.ToDecimal(auxNIT));

                                foreach (var n in nitClient)
                                {
                                    //Nombre del producto
                                    var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                    //Validar si ya existe el producto en el NIT a reservar.
                                    if (prodsClient == n.NombreProducto.ToString())
                                    {
                                        if (n.AsesorID != userId)
                                        {
                                            lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {5} con el producto {6} en la ciudad de {7}.", model.mensaje, model.NIT, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad));
                                            contad++;
                                        }
                                        else
                                        {
                                            lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                            contad++;
                                        }
                                    }
                                }
                            }


                            if (reciClient.DIG_VERI != -1 && existenContrat == null && nitClient.Count > 0)
                            {
                                model.mensaje = null;

                                //Validar Si tiene productos disponibles.
                                var productosDisponibles = ServicioCotizacion.ObtenerProductosDisponiblesParaNit(permiNIT, model.ciudad);

                                model.listadoProductos = productosDisponibles;
                                model.validarFormularioReserva = false;
                                model.nombreEmpresa = ServicioCotizacion.ObtenerNombreClienteXNIT(Convert.ToDecimal(auxNIT));

                                foreach (var n in nitClient)
                                {
                                    model.DV = reciClient.DIG_VERI;
                                    var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                    //Validar si ya existe el producto en el NIT a reservar.
                                    if (prodsClient == n.NombreProducto.ToString())
                                    {
                                        if (n.AsesorID != userId)
                                        {
                                            if (prodsClient != null)
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {5} con el producto {6} en la ciudad de {7}.", model.mensaje, model.NIT, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad));
                                                contad++;
                                            }
                                            else
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {3} en la ciudad de {4}.", model.mensaje, model.NIT, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, n.ciudad));
                                                contad++;
                                            }
                                        }
                                        if (n.AsesorID == userId)
                                        {
                                            if (prodsClient != null)
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                contad++;
                                            }
                                            else
                                            {
                                                model.validarFormularioReserva = true;
                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion));
                                                contad++; ;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (auxNIT.Length > 9)
                    {
                        var canalUsuar = ServicioCotizacion.ObtenerCanalAsesor(userId);

                        //var permiNIT = Convert.ToInt32(auxNIT);

                        int valitNitInt;
                        bool resValitNit = int.TryParse(auxNIT, out valitNitInt);

                        if (resValitNit == false)
                        {
                            model.validarFormularioReserva = true;
                            lstMensajeReserva.Add("El número de identificación no es válido. Tenga en cuenta que si es un NIT no debe ingresar el dígito de verificación.");
                            nitEsValido = false;
                        }
                        else
                        {

                            model.DV = 0;
                            var existenContrat = ServicioCotizacion.validarPermiContrato(1, userId);
                            if (ciudadUsuar != null && canalUsuar != null)
                            {
                                model.ciudad = ciudadUsuar;
                                model.canal = canalUsuar;
                            }
                            else
                            {
                                model.canal = "";
                                model.ciudad = "";
                                model.mensaje = "Ciudad y canal no disponible. Usuario inactivo";
                            }

                            var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(auxNIT, userId);
                            if (existenContrat == null && nitClient.Count <= 0)
                            {
                                model.mensaje = null;
                                model.validarFormularioReserva = false;
                            }
                            if (existenContrat != null && nitClient.Count > 0)
                            {
                                model.mensaje = null;

                                //Validar Si tiene productos disponibles.
                                var productosDisponibles = ServicioCotizacion.ObtenerProductosDisponiblesParaNit(decimal.Parse(model.NIT), model.ciudad);

                                model.validarFormularioReserva = false;
                                model.listadoProductos = productosDisponibles;
                                model.nombreEmpresa = ServicioCotizacion.ObtenerNombreClienteXNIT(Convert.ToDecimal(auxNIT));

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
                                        if (n.AsesorID != userId)
                                        {
                                            lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {5} con los productos {6} en la ciudad de {7}.", model.mensaje, model.NIT, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad));
                                            contad++;
                                        }
                                        else
                                        {
                                            lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con los productos {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                            contad++;
                                        }
                                    }
                                }
                            }

                            if (existenContrat == null && nitClient.Count > 0)
                            {
                                model.mensaje = null;

                                //Validar Si tiene productos disponibles.
                                var productosDisponibles = ServicioCotizacion.ObtenerProductosDisponiblesParaNit(Convert.ToDecimal(model.NIT), model.ciudad);

                                model.validarFormularioReserva = false;
                                model.listadoProductos = productosDisponibles;
                                model.nombreEmpresa = ServicioCotizacion.ObtenerNombreClienteXNIT(Convert.ToDecimal(auxNIT));

                                foreach (var n in nitClient)
                                {
                                    //model.mensaje = null;
                                    var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                    //Validar si ya existe el producto en el NIT a reservar.
                                    if (prodsClient == n.NombreProducto.ToString())
                                    {
                                        if (n.AsesorID != userId)
                                        {
                                            if (prodsClient != null)
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {5} con el producto {6} en la ciudad de {7}.", model.mensaje, model.NIT, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, prodsClient, n.ciudad));
                                                contad++;
                                            }
                                            else
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por el asesor {3} cuyo director es {4}, hasta el día {3} en la ciudad de {4}.", model.mensaje, model.NIT, n.CotizacionID, n.nombreAsesor, n.nombreDirector, n.fechaExpiracion, n.ciudad));
                                                contad++;
                                            }
                                        }
                                        if (n.AsesorID == userId)
                                        {
                                            if (prodsClient != null)
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                contad++;
                                            }
                                            else
                                            {
                                                lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con los productos {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                contad++;
                                            }
                                        }
                                    }
                                }

                            }
                            
                        }
                    }

                }
                if (nitEsValido)
                {
                    if (!ServicioCotizacion.ObtenerProductosDisponiblesParaNit(Convert.ToDecimal(model.NIT), ciudadUsuar).Any())
                    {
                        model.validarFormularioReserva = true;
                        ViewBag.AlertaSinProductosCiudad = $"No existen productos disponibles para reservar el Nit {model.NIT} en la ciudad {ciudadUsuar}.";
                    }
                    else
                    {
                        model.validarFormularioReserva = false;
                        lstMensajeReserva.Add("El NIT ingresado no tiene contratos ni bloqueos asociados. Por favor continúe ingresando la información del formulario.");
                    }
                }
                model.lstMensajeReserva = lstMensajeReserva;
                model.validarModalReserva = true;

            }
            catch (Exception e)
            {
                //string msjRet = e.Message;
                string msjRet = "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.";
                ModelState.AddModelError(string.Empty, msjRet);
                lstMensajeReserva.Add(msjRet);
                model.lstMensajeReserva = lstMensajeReserva;
            }

            return View("Index", model);
        }

        public ActionResult Reservar()
        {
            return View();
        }
    }
}