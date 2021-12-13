using Aplicacion.Administracion.Contratos;
using AutoMapper;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.CorreoDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using MvcReportViewer;
using Newtonsoft.Json;
using PagedList;
using Presentacion.Mvc.App.Models;
using Presentacion.Mvc.App.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Transversales.Administracion.IoC;
using Utilidades.GestionCorreos;

namespace Presentacion.Mvc.App.Controllers
{
    /// <summary>
    /// Clase controladora de metodos relacionados con la funcionalidad de bloqueo de NIT y calculo de cotizaciones
    /// </summary>
    public class BloqueoNitController : Controller
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

        #region 1). Reserva a Cotizar

        /// <summary>
        /// Metodo donde se inicia y se traen valores y las opciones de selección multiple como check box y drop down list
        /// </summary>
        /// <param name="msg"> mensaje de exito o fracaso al momento de registrar una visita</param>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo para realizar el registro de una cotizacion por parte de un asesor
        /// </summary>
        /// <param name="model">modelo con los datos para regitrar una cotización</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult Index(RegistroVisitaModel model)
        {
            try
            {
                bool registro = false;
                var prods = ServicioCotizacion.ObtenerProductos();
                Mapper.CreateMap<ProductosSelector, Dominio.Administracion.Entidades.ModeloCotizacion.Productos>().ReverseMap();

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
                    //ModelState.AddModelError(string.Empty, "Los  números celulares no pueden superar  10 dígitos.");
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
                                //ViewBag.Mensaje = "Seleccione al menos un producto por favor.";
                            }
                        }
                        else
                        {
                            model.validMsgErrorReservaNit = 1;
                            model.msgErrorReservaNit = "¡ATENCIÓN! El cliente " + model.nombreEmpresa + " tiene un contrato vigente con el producto seleccionado.";

                            //ModelState.AddModelError(string.Empty, "¡ATENCIÓN! El cliente " + model.nombreEmpresa + " tiene un contrato vigente con el producto seleccionado");
                        }
                    }
                    else
                    {
                        foreach (var index in produsClient)
                        {
                            model.validMsgErrorReservaNit = 1;
                            model.mensaje = string.Format("{0}|¡ATENCIÓN! El NIT digitado, en la reserva {1} ya ha sido bloqueado previamente con el (los) producto(s) {2} en la  ciudad de {3}", model.mensaje, index.CotizacionID, index.nombreProducto, index.ciudadCotiz);

                            //ModelState.AddModelError(string.Empty, string.Format("{0} Error en el registro. El NIT digitado, en la reserva {1} ya ha sido bloqueado previamente con el (los) producto(s) {2} en la  ciudad de {3}", model.mensaje, index.CotizacionID, index.nombreProducto, index.ciudadCotiz));
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

                //ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("Index", model);
        }

        /// <summary>
        /// Metodo para mostrar el nombre de la empresa cliente y el digito de validacion a partir de la validación del NIT y contratos vigentes
        /// </summary>
        /// <param name="model">modelo con los datos para calcular el DV y encontrar el nombre de una empresa cliente</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult ConsultarNIT(RegistroVisitaModel model)
        {
            List<string> lstMensajeReserva = new List<string>();
            try
            {
                var prods = ServicioCotizacion.ObtenerProductos();
                int contad = 0;

                List<string> lstProductosDisponibles = new List<string>();

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
                                var identity = (ClaimsIdentity)User.Identity;
                                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                                var ciudadUsuar = ServicioCotizacion.ObtenerCiudadAsesor(userId);
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
                                    var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                    var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                    if (productosDisponibles.Count > 0)
                                    {
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
                                        // Si no tiene productos disponibles
                                        if (productosDisponibles.Count == 0)
                                    {


                                        foreach (var n in nitClient)
                                        {
                                            model.DV = reciClient.DIG_VERI;
                                            var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                            //Validar si ya existe el producto en el NIT a reservar.
                                            if (prodsClient == n.NombreProducto.ToString() && ciudadUsuar != n.ciudad)
                                            {

                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                contad++;
                                            }
                                            else
                                            {
                                                model.validarFormularioReserva = true;
                                                lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2}  hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient, "."));

                                            }
                                        }
                                    }
                                    else
                                    {
                                        model.validarFormularioReserva = false;
                                        lstMensajeReserva.Add("El NIT ingresado no tiene contratos ni bloqueos asociados. Por favor continúe ingresando la información del formulario.");

                                    }
                                }
                                else
                                {
                                    lstMensajeReserva.Add("El NIT ingresado no tiene contratos ni bloqueos asociados. Por favor continúe ingresando la información del formulario.");

                                    var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(auxNIT, userId);
                                    if (nitClient.Count > 0)
                                    {
                                        model.mensaje = null;

                                        //Validar Si tiene productos disponibles.
                                        var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                        var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                        if (productosDisponibles.Count > 0)
                                        {
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
                                        // Si no tiene productos disponibles
                                        if (productosDisponibles.Count == 0 )
                                        {
                                            foreach (var n in nitClient)
                                            {
                                                model.DV = reciClient.DIG_VERI;
                                                var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                                    //Validar si ya existe el producto en el NIT a reservar.
                                                    if (prodsClient == n.NombreProducto.ToString() && ciudadUsuar != n.ciudad)
                                                    {

                                                        lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                        contad++;
                                                    }
                                                    else
                                                    {
                                                        model.validarFormularioReserva = true;
                                                        lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2}  hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient, "."));

                                                    }
                                            }

                                        }
                                        else
                                        {
                                            model.validarFormularioReserva = false;
                                            lstMensajeReserva.Add("El NIT ingresado no tiene contratos ni bloqueos asociados.Por favor continúe ingresando la información del formulario.");

                                        }
                                    }
                                }
                            }
                        }
                        if (reciClient.NOM_TERC == null)
                        {
                            var identity = (ClaimsIdentity)User.Identity;
                            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                            var ciudadUsuar = ServicioCotizacion.ObtenerCiudadAsesor(userId);
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
                            }

                            var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(auxNIT, userId);

                            if (reciClient.DIG_VERI != -1 && existenContrat == null && nitClient.Count <= 0)
                            {
                                model.mensaje = null;
                                model.DV = reciClient.DIG_VERI;
                                lstMensajeReserva.Add("No se encontró registros ni bloqueos con el NIT digitado, por favor complete toda la información del siguiente formulario.");
                            }
                            if (reciClient.DIG_VERI == -1)
                            {
                                model.mensaje = null;
                                lstMensajeReserva.Add("El NIT ingresado no es válido, por favor verifica e intenta nuevamente.");
                                ModelState.AddModelError(string.Empty, "El NIT ingresado no es válido, por favor verifica e intenta nuevamente.");
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
                                var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                if (productosDisponibles.Count > 0)
                                {
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
                                // Si no tiene productos disponibles
                                if (productosDisponibles.Count == 0)
                                {
                                    foreach (var n in nitClient)
                                    {
                                        model.DV = reciClient.DIG_VERI;
                                        var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                        //Validar si ya existe el producto en el NIT a reservar.
                                        if (prodsClient == n.NombreProducto.ToString() && ciudadUsuar != n.ciudad)
                                        {

                                            lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                            contad++;
                                        }
                                        else
                                        {
                                            model.validarFormularioReserva = true;
                                            lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2}  hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient, "."));

                                        }
                                    }
                                }
                                else
                                {
                                    model.validarFormularioReserva = false;
                                    lstMensajeReserva.Add("El NIT ingresado no tiene contratos ni bloqueos asociados. Por favor continúe ingresando la información del formulario.");

                                }

                            }


                            if (reciClient.DIG_VERI != -1 && existenContrat == null && nitClient.Count > 0)
                            {
                                model.mensaje = null;

                                //Validar Si tiene productos disponibles.
                                var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                if (productosDisponibles.Count > 0)
                                {
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
                                // Si no tiene productos disponibles
                                if (productosDisponibles.Count == 0)
                                {


                                    foreach (var n in nitClient)
                                    {
                                        model.DV = reciClient.DIG_VERI;
                                        var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                        //Validar si ya existe el producto en el NIT a reservar.
                                        if (prodsClient == n.NombreProducto.ToString() && ciudadUsuar != n.ciudad)
                                        {

                                            lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                            contad++;
                                        }
                                        else
                                        {
                                            model.validarFormularioReserva = true;
                                            lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2}  hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient, "."));

                                        }
                                    }
                                }
                                else
                                {
                                    model.validarFormularioReserva = false;
                                    lstMensajeReserva.Add("El NIT ingresado no tiene contratos ni bloqueos asociados. Por favor continúe ingresando la información del formulario.");

                                }
                            }
                        }
                    }
                    if (auxNIT.Length > 9)
                    {
                        var identity = (ClaimsIdentity)User.Identity;
                        var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                        var ciudadUsuar = ServicioCotizacion.ObtenerCiudadAsesor(userId);
                        var canalUsuar = ServicioCotizacion.ObtenerCanalAsesor(userId);

                        //var permiNIT = Convert.ToInt32(auxNIT);

                        int valitNitInt;
                        bool resValitNit = int.TryParse(auxNIT, out valitNitInt);

                        if (resValitNit == false)
                        {
                            model.validarFormularioReserva = true;
                            lstMensajeReserva.Add("El número de identificación no es válido. Tenga en cuenta que si es un NIT no debe ingresar el dígito de verificación");
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
                                lstMensajeReserva.Add("No se encontró registros ni bloqueos con el NIT digitado por favor complete toda la información del formulario");
                            }
                            if (existenContrat != null && nitClient.Count > 0)
                            {
                                model.mensaje = null;

                                //Validar Si tiene productos disponibles.
                                var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                if (productosDisponibles.Count > 0)
                                {
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
                                // Si no tiene productos disponibles
                                if (productosDisponibles.Count <= 0)
                                    // Si no tiene productos disponibles
                                    if (productosDisponibles.Count == 0)
                                    {


                                        foreach (var n in nitClient)
                                        {

                                            var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                            //Validar si ya existe el producto en el NIT a reservar.
                                            if (prodsClient == n.NombreProducto.ToString() && ciudadUsuar != n.ciudad)
                                            {

                                                lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                                contad++;
                                            }
                                            else
                                            {
                                                model.validarFormularioReserva = true;
                                                lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2}  hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient, "."));

                                            }
                                        }
                                    }
                                    else
                                    {
                                        model.validarFormularioReserva = false;
                                        lstMensajeReserva.Add("El NIT ingresado no tiene contratos ni bloqueos asociados. Por favor continúe ingresando la información del formulario.");

                                    }

                            }

                            if (existenContrat == null && nitClient.Count > 0)
                            {
                                model.mensaje = null;

                                //Validar Si tiene productos disponibles.
                                var productosNoDisponibles = from a in productos join b in nitClient on a.NombreProducto equals b.NombreProducto select a;
                                var productosDisponibles = productos.Except(productosNoDisponibles).ToList();

                                if (productosDisponibles.Count > 0)
                                {
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
                                // Si no tiene productos disponibles
                                if (productosDisponibles.Count == 0)
                                {


                                    foreach (var n in nitClient)
                                    {

                                        var prodsClient = ServicioCotizacion.ObtenerProductosCliente(n.CotizacionID);

                                        //Validar si ya existe el producto en el NIT a reservar.
                                        if (prodsClient == n.NombreProducto.ToString() && ciudadUsuar != n.ciudad)
                                        {

                                            lstMensajeReserva.Add(string.Format("{0}El NIT ingresado no tiene contratos asociados. El NIT {1} ya se encuentra bloqueado en la reserva con ID {2} por usted hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient));
                                            contad++;
                                        }
                                        else
                                        {
                                            model.validarFormularioReserva = true;
                                            lstMensajeReserva.Add(string.Format("{0}El NIT {1} ya se encuentra bloqueado en la reserva con ID {2}  hasta el día {3} con el producto {4}.", model.mensaje, model.NIT, n.CotizacionID, n.fechaExpiracion, prodsClient, "."));

                                        }
                                    }
                                }
                                else
                                {
                                    model.validarFormularioReserva = false;
                                    lstMensajeReserva.Add("El NIT ingresado no tiene contratos ni bloqueos asociados. Por favor continúe ingresando la información del formulario.");

                                }
                            }
                            else
                            {
                                model.validarFormularioReserva = false;
                                lstMensajeReserva.Add("El NIT ingresado no se encuentra registrado, por favor diligenciar el siguiente formulario");

                            }
                        }
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



        private bool ValidarNit(RegistroVisitaModel model)
        {
            return true;
        }



        #endregion 1). Reserva a Cotizar

        #region 2). Mis Reservas

        public ActionResult MisCotizaciones()
        {
            var modelo = new RegistroVisitaModel();
            return View("MisCotizaciones", modelo);
        }

        //[System.Web.Mvc.HttpGet]
        //public String ObtenerInfMisCotizaciones(string estadoReserva)
        //{
        //    ObtenerInfMisCotizaciones objRetornar = new ObtenerInfMisCotizaciones();
        //    List<string> lstMsg = new List<string>();
        //    objRetornar.LstProximasVencer = new List<string>();

        //    UserClaims.ResetUserClaimsSession();
        //    try
        //    {
        //        var identity = (ClaimsIdentity)User.Identity;
        //        var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

        //        var roles = identity.FindFirst(ClaimTypes.Role).Value;
        //        var lstRoles = roles.Split(',');
        //        objRetornar.LstRoles = lstRoles.Where(l => l != "").ToList();

        //        //Director = 7, Pricing = 28
        //        if (objRetornar.LstRoles.Any(x => x == "7" || x == "28"))
        //        {
        //            var noDocumentoDirector = ServicioCotizacion.ObtenerNoDocumentoDirector(userId);
        //            objRetornar.LstMisCotizacionesRol = ServicioCotizacion.ObtenerJerarquiaCotizaciones(noDocumentoDirector.ToString(), estadoReserva).ToList();

        //            //foreach (var coti in objRetornar.LstMisCotizacionesRol)
        //            //{
        //            //    if (coti.FechaVencimiento != null)
        //            //    {
        //            //        DateTime auxVencimiento = Convert.ToDateTime(coti.FechaVencimiento);
        //            //        var numDays = (auxVencimiento - DateTime.Now);

        //            //        if (numDays.TotalDays <= 8 && numDays.TotalDays >= 0)
        //            //        {
        //            //            objRetornar.LstProximasVencer.Add(coti.Nit);
        //            //            coti.ValidarVencida = true;
        //            //        }
        //            //        else
        //            //        {
        //            //            coti.ValidarVencida = false;
        //            //        }
        //            //        if (numDays.TotalDays < 0)
        //            //        {
        //            //            var libReser = ServicioCotizacion.liberarReserva(coti.IdCotizacion.ToString(), "Se venció", 5);
        //            //            coti.ValidarVencida = false;
        //            //        }
        //            //    }
        //            //    else
        //            //    {
        //            //        var libReser = ServicioCotizacion.liberarReserva(coti.IdCotizacion.ToString(), "Se venció", 5);
        //            //    }
        //            //}
        //        }
        //        else
        //        {
        //            objRetornar.LstMisCotizaciones = ServicioCotizacion.ObtenerMisCotizaciones(userId, estadoReserva).ToList();

        //            foreach (var coti in objRetornar.LstMisCotizaciones)
        //            {
        //                if (coti.fechaVencimiento != null)
        //                {
        //                    DateTime auxVencimiento = Convert.ToDateTime(coti.fechaVencimiento);
        //                    var numDays = (auxVencimiento - DateTime.Now);

        //                    if (numDays.TotalDays <= 8 && numDays.TotalDays >= 0)
        //                    {
        //                        objRetornar.LstProximasVencer.Add(coti.NIT);
        //                        coti.validarVencida = true;
        //                    }
        //                    else
        //                    {
        //                        objRetornar.Mensaje = "No tiene reservas próximas a vencer.";
        //                        coti.validarVencida = false;
        //                    }
        //                    if (numDays.TotalDays < 0)
        //                    {
        //                        var libReser = ServicioCotizacion.liberarReserva(coti.id_cotizacion.ToString(), "Se venció", 5);
        //                        coti.validarVencida = false;
        //                    }
        //                }
        //                else
        //                {
        //                    objRetornar.Mensaje = "No tiene reservas próximas a vencer.";
        //                    coti.validarVencida = false;
        //                    var libReser = ServicioCotizacion.liberarReserva(coti.id_cotizacion.ToString(), "Se venció", 5);
        //                }
        //            }
        //        }

        //        //if (objRetornar.LstProximasVencer.Count > 0)
        //        //{
        //        //    foreach (var index in objRetornar.LstProximasVencer)
        //        //    {
        //        //        decimal auxNIT = Convert.ToDecimal(index);
        //        //        var noClie = ServicioCotizacion.ObtenerNombreClienteXNIT(auxNIT);

        //        //        if (noClie != null)
        //        //        {
        //        //            var mensaje = string.Format("La reserva del NIT {0} perteneciente al cliente {1} esta próxima a vencerse.", index, noClie);
        //        //            lstMsg.Add(mensaje);
        //        //        }
        //        //        else
        //        //        {
        //        //            var mensaje = string.Format("La reserva del NIT {0} esta próxima a vencerse.", index);
        //        //            lstMsg.Add(mensaje);
        //        //        }
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    var mensaje = "No tiene reservas próximas a vencer";
        //        //    lstMsg.Add(mensaje);
        //        //}

        //        objRetornar.Respuesta = true;
        //        objRetornar.LstMsgProximasVencer = lstMsg;
        //    }
        //    catch (Exception)
        //    {
        //        objRetornar.Respuesta = false;
        //        objRetornar.Mensaje = "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.";
        //    }

        //    return JsonConvert.SerializeObject(objRetornar);
        //}

        [System.Web.Mvc.HttpGet]
        public String ObtenerListadosMisCotizaciones(string nombreListado)
        {
            var seriarlizer = new JavaScriptSerializer();
            switch (nombreListado)
            {
                case "EstadosReserva":
                    var estadosReserva = ServicioCotizacion.ObtenerEstados();
                    var serializadoEstadosReserva = seriarlizer.Serialize(estadosReserva);
                    return serializadoEstadosReserva;

                case "EstadosCotizacion":
                    var estadosCotizacionSCI = ServicioCotizacion.ObtenerEstadosCotizacion();
                    var serializadoEstadosCotizacionSCI = seriarlizer.Serialize(estadosCotizacionSCI);
                    return serializadoEstadosCotizacionSCI;

                case "Productos":
                    var productos = ServicioCotizacion.ObtenerProductos();
                    var serializadoProductos = seriarlizer.Serialize(productos);
                    return serializadoProductos;

                default:
                    break;
            }

            return "";
        }

        /// <summary>
        /// Metodo para filtrar las cotizaciones de un asesor por NIT o estado
        /// </summary>
        /// <param name="modelo">modelo con los criterios para filtrar</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult filtroCotizaciones(RegistroVisitaModel modelo)
        {
            List<EMA_Cotizacion> listMisCoti = new List<EMA_Cotizacion>();
            try
            {
                //Estados  de reserva
                var estados = ServicioCotizacion.ObtenerEstados();
                modelo.estadosCotizacion = estados;

                //Estados  de Cotización
                var estadosCotizacionSCI = ServicioCotizacion.ObtenerEstadosCotizacion();
                modelo.estadosCotizacionSCI = estadosCotizacionSCI;

                //Listado de Productos
                var productos = ServicioCotizacion.ObtenerProductos();
                modelo.listadoProductos = productos;

                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                modelo.misCotizaciones = ServicioCotizacion.ObtenerCotizacionFiltros(modelo.productoFiltroCotiSelectID, modelo.estadoReserFiltroCotiSelectID, modelo.estadoCotiFiltroCotiSelectID, userId, modelo.nitFiltroCoti, modelo.fechaInicioFiltroCoti, modelo.fechaFinFiltroCoti).ToPagedList(1, 20);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("MisCotizaciones", modelo);
        }

        #endregion 2). Mis Reservas

        #region 3). Cotización

        /// <summary>
        /// Metodo creado para cargar valores campos y checklist destinados a cotizar una reserva
        /// </summary>
        /// <param name="reservID"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public ActionResult CotizarReserva(int cotID)
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

            return View("CotizarReserva", model);
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

                    var obtenerTarifaSede = ServicioCotizacion.CalcularTarifaSedeSCI(objCalcularTarifaSede);
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

                    var smtp = new SmtpClient
                    {
                        Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]),
                        Host = ConfigurationManager.AppSettings["SmtpHost"],
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SmtpCorreo"], ConfigurationManager.AppSettings["SmtpPassword"]),
                        Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpTimeout"])
                    };

                    MailAddress from = new MailAddress(ConfigurationManager.AppSettings["SmtpCorreo"], ConfigurationManager.AppSettings["NombreFromNameEmail"]);
                    MailAddress to = new MailAddress(ConfigurationManager.AppSettings["CorreoPrueba"], "ToName");
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
        [System.Web.Mvc.HttpGet]

        #endregion 3). Cotización

        #region 4). Reconcideración / Liberar Reserva


        public ActionResult ReconsiderarReserva(int cotID)
        {
            var model = new RegistroVisitaModel();
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            var roles = identity.FindFirst(ClaimTypes.Role).Value;

            try
            {
                model.lstIdRol = roles.Split(',').Where(r=> !string.IsNullOrEmpty(r)).ToList();
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
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }

            return View("ReconsiderarReserva", model);
        }

        [HttpPost]
        public String ReconsiderarReserva(GuardarLiberarRenovar reconsideracion)
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
                DetalleReconsideracion = JsonConvert.SerializeObject(sedes.Where(s=> s.Valor != s.ValorReconsideracion))
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
        public String AprobarRechazarReconsideracion(GuardarLiberarRenovar reconsideracion)
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

        [System.Web.Mvc.HttpGet]
        public String ObtenerInfSedesCotizacion()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            var infSedes = ServicioCotizacion.ObtenerInfSedesCotizacion(Convert.ToInt32(Session["IdCotizacion"]));

            return JsonConvert.SerializeObject(infSedes);
        }

        [System.Web.Mvc.HttpPost]
        public String LiberarReserva(GuardarLiberarRenovar objLiberarRenovar)
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

        [System.Web.Mvc.HttpPost]
        public String RenovarReservas(GuardarLiberarRenovar objLiberarRenovar)
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
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }

            return seriarlizer.Serialize(objRespuesta);
        }

        #endregion 4). Reconcideración / Liberar Reserva

        #region 5). Reporte Cotizaciones y reservas

        public ActionResult ReporteCotizaciones(string value)
        {
            var modelo = new RegistroVisitaModel();
            //  ViewBag.Mensaje = value;

            List<EMA_Cotizacion> listMisCoti = new List<EMA_Cotizacion>();

            UserClaims.ResetUserClaimsSession();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                //Estados  de reserva
                var estados = ServicioCotizacion.ObtenerEstados();
                modelo.estadosCotizacion = estados;

                //Estados  de Cotización
                var estadosCotizacionSCI = ServicioCotizacion.ObtenerEstadosCotizacion();
                modelo.estadosCotizacionSCI = estadosCotizacionSCI;

                //Listado de Productos
                var productos = ServicioCotizacion.ObtenerProductos();
                modelo.listadoProductos = productos;

                //Asignar los valores iniciales a los filtros
                modelo.estadoReserFiltroCotiSelectID = 4; // Vigente
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("ReporteCotizaciones", modelo);
        }

        /// <summary>
        /// Metodo para filtrar las cotizaciones de un asesor por NIT o estado
        /// </summary>
        /// <param name="modelo">modelo con los criterios para filtrar</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public ActionResult FiltrosReporteCotizaciones(RegistroVisitaModel modelo)
        {
            List<EMA_Cotizacion> listMisCoti = new List<EMA_Cotizacion>();
            try
            {
                //Estados  de reserva
                var estados = ServicioCotizacion.ObtenerEstados();
                modelo.estadosCotizacion = estados;

                //Estados  de Cotización
                var estadosCotizacionSCI = ServicioCotizacion.ObtenerEstadosCotizacion();
                modelo.estadosCotizacionSCI = estadosCotizacionSCI;

                //Listado de Productos
                var productos = ServicioCotizacion.ObtenerProductos();
                modelo.listadoProductos = productos;

                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                modelo.misCotizaciones = ServicioCotizacion.ObtenerCotizacionFiltros(modelo.productoFiltroCotiSelectID, modelo.estadoReserFiltroCotiSelectID, modelo.estadoCotiFiltroCotiSelectID, userId, modelo.nitFiltroCoti, modelo.fechaInicioFiltroCoti, modelo.fechaFinFiltroCoti).ToPagedList(1, 20);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("ReporteCotizaciones", modelo);
        }

        #endregion 5). Reporte Cotizaciones y reservas

        /// <summary>
        /// Metodo creado para asociar la información rquerida para cotizar con la de una reserva previamente ya almacenada.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        ///
        [System.Web.Mvc.HttpPost]
        public ActionResult Cotizar(RegistroVisitaModel modelo)
        {
            try
            {
                bool bandera = false;
                List<Productos> produs = new List<Productos>();
                var estPric = ServicioCotizacion.ObtenerEstadosPricing();
                modelo.estadosPricing = estPric;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var prods = ServicioCotizacion.ObtenerProductos();
                Mapper.CreateMap<ProductosSelector, Dominio.Administracion.Entidades.ModeloCotizacion.Productos>().ReverseMap();
                var productosSelect = Mapper.Map<List<ProductosSelector>>(prods);
                produs = ServicioCotizacion.productosCotizacion(Convert.ToInt32(modelo.IdCotizacion)).ToList();
                modelo.productosIds = new int[produs.Count];
                var prodCoti = (from a in prods
                                join b in produs on a.COD_SERV equals b.COD_SERV
                                select b.COD_SERV).ToList();

                for (int i = 0; i < prodCoti.Count; i++)
                {
                    if (prodCoti.ElementAt(i) == 2010)
                    {
                        modelo.banderaAreasProtegidas += 1;
                    }
                    else
                    {
                        modelo.banderaAreasProtegidas = 0;
                    }
                }

                modelo.validacionInfo = ServicioCotizacion.ObtenerValidacion();
                var sectores = ServicioCotizacion.ObtenerSectores();
                modelo.sectores = sectores;
                var tipoProtegida = ServicioCotizacion.ObtenerTipoAreasProtegidas();
                modelo.areas_protegidas = tipoProtegida;

                var ciudadUsuar = ServicioCotizacion.ObtenerCiudadAsesor(userId);
                var canalUsuar = ServicioCotizacion.ObtenerCanalAsesor(userId);
                if (ciudadUsuar != null && canalUsuar != null)
                {
                    modelo.ciudad = ciudadUsuar;
                    modelo.canal = canalUsuar;
                }
                else
                {
                    modelo.canal = "";
                    modelo.ciudad = "";
                    modelo.mensaje = "Ciudad y canal no disponible. Usuario inactivo";
                }
                if (modelo.NumeroExpuestos.Contains("-") || modelo.NumeroExpuestos.Contains("+"))
                {
                    ModelState.AddModelError(string.Empty, "Número expuestos no valido.");
                    bandera = true;
                }
                if (modelo.numeroCapacitaciones.Contains("-") || modelo.numeroCapacitaciones.Contains("+"))
                {
                    ModelState.AddModelError(string.Empty, "Número capacitaciones no valido.");
                    bandera = true;
                }
                if (modelo.numeroEventos.Contains("-") || modelo.numeroEventos.Contains("+"))
                {
                    ModelState.AddModelError(string.Empty, "Número eventos no valido.");
                    bandera = true;
                }
                if (modelo.numeroSedes.Contains("-") || modelo.numeroSedes.Contains("+"))
                {
                    ModelState.AddModelError(string.Empty, "Número sedes no valido.");
                    bandera = true;
                }
                if (bandera == false)
                {
                    var rutaPlantillas = ConfigurationManager.AppSettings["RutaPlantillasEnvioCorreos"];
                    var nombrePlantilla = ConfigurationManager.AppSettings["NombrePlantillaRegistro"];
                    var aplicacion = ConfiguracionesGlobales.AplicacionWsUsuarios;
                    var rutaPlantillaEmail = string.Format(@"{0}\{1}\{2}", rutaPlantillas, aplicacion, nombrePlantilla);

                    if (modelo.validacionIds != null)
                    {
                        if (modelo.sectorID != null)
                        {
                            var regiCoti = ServicioCotizacion.Cotizar(modelo.IdCotizacion, modelo.MotivoVisita, modelo.numeroCapacitaciones, modelo.NumeroExpuestos, modelo.numeroEventos, modelo.numeroSedes, modelo.sectorID, modelo.areasProtegID, modelo.validacionIds, userId);

                            if (regiCoti == true)
                            {
                                var correoPricing = ServicioCotizacion.ObtenerInfoPrincing();
                                if (correoPricing != null)
                                {
                                    string body = Transversales.Administracion.Plantillas.GestionPlantillas.ObtenerHtmlPlantilla(rutaPlantillaEmail);

                                    if (!string.IsNullOrWhiteSpace(body))
                                    {
                                        body = body.Replace("@@NOMBRECLIENTE@@", modelo.nombreEmpresa);
                                        body = body.Replace("@@NIT@@", modelo.NIT);
                                        body = body.Replace("@@OBSERVACIONESASESOR@@", modelo.MotivoVisita);
                                    }
                                    else
                                    {
                                        body = string.Format("Se ha registrado una nueva reserva del cliente {0} de NIT {1} por parte de un asesor en Tu Portal. Ingrese por favor a tu portal para mayor detalle", modelo.nombreEmpresa, modelo.NIT);
                                    }
                                    CorreoDTO correoEnvio = new CorreoDTO
                                    {
                                        Asunto = "Registro de visita",
                                        Body = body,
                                        Adjuntos = null,
                                        Destinos = correoPricing,
                                        DestinosCopia = null,
                                        DestinosCopiaOculta = null,
                                        Tipo = "Registro de visita"
                                    };

                                    GestionCorreos.InsertarEnviarCorreo(correoEnvio);
                                    return RedirectToAction("ReservasACotizar", new { msg = "Se ha registrado la información de la cotización." });
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Correo de pricing no disponible");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "No se ha podido almacenar la información de la reserva. La reserva ya se encuentra en proceso de cotización.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Debe seleccionar un sector por favor.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Valide la información por favor.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Verifique que todos los campos obligatorios hayan sido completados correctamente.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("CotizarReserva", modelo);
        }

        /// <summary>
        /// Metodo para obtener una cotizacion con el fin de editarla en una vista pertinente
        /// </summary>
        /// <param name="cotID">id de una cotizacion</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public ActionResult EditarCotizacion(int cotID)
        {
            var modelo = new RegistroVisitaModel();
            decimal tot = 0;
            decimal auxTot = 0;
            List<Productos> produs = new List<Productos>();
            try
            {
                var estados = ServicioCotizacion.ObtenerEstados();

                var estadofilt = (from a in estados
                                  where a.nombreEstado.Equals("Perdida")
                                  select a).ToList();
                modelo.estadosCotizacion = estadofilt;

                //var identity = (ClaimsIdentity)User.Identity;
                //var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var userId = ServicioCotizacion.ObtenerIDAsesor(cotID);

                if (userId >= 0)
                {
                    var editarCoti = ServicioCotizacion.EditarCotizacion(cotID, userId);
                    if (editarCoti != null)
                    {
                        foreach (var edit in editarCoti)
                        {
                            if (edit.ObservacionesPricing != null)
                            {
                                tot = Convert.ToDecimal(edit.Total);
                                auxTot = Math.Round(tot, 0);
                                modelo.IdCotizacion = cotID.ToString();
                                modelo.NIT = edit.NIT.ToString();
                                modelo.nombreEmpresa = edit.nombreEmpresa;
                                modelo.Contacto = edit.contacto;
                                modelo.telefono = edit.telefono.ToString();
                                modelo.Cargo = edit.cargo;
                                modelo.tipoAreaProt = edit.TipoAP;
                                //modelo.nivelInteres = edit.nivelInteres;
                                modelo.celular = edit.celular.ToString();
                                modelo.correoElectronico = edit.correoElectronico;
                                modelo.FechaVisita = Convert.ToDateTime(edit.fechaVisita);
                                modelo.ciudad = edit.ciudad;
                                modelo.canal = edit.canal;
                                modelo.estado = edit.estado;
                                modelo.Total = "$ " + auxTot.ToString() + " + IVA";
                                modelo.ObservacionesPricing = edit.ObservacionesPricing;
                                modelo.ObservacionReserva = edit.ObservacionReserva;
                                modelo.TelefonoExt = edit.TelefonoExt;
                            }
                            else
                            {
                                tot = Convert.ToDecimal(edit.Total);
                                auxTot = Math.Round(tot, 0);
                                modelo.IdCotizacion = cotID.ToString();
                                modelo.NIT = edit.NIT.ToString();
                                modelo.nombreEmpresa = edit.nombreEmpresa;
                                modelo.Contacto = edit.contacto;
                                modelo.telefono = edit.telefono.ToString();
                                modelo.Cargo = edit.cargo;
                                modelo.tipoAreaProt = edit.TipoAP;
                                //modelo.nivelInteres = edit.nivelInteres;
                                modelo.celular = edit.celular.ToString();
                                modelo.correoElectronico = edit.correoElectronico;
                                modelo.FechaVisita = Convert.ToDateTime(edit.fechaVisita);
                                modelo.ciudad = edit.ciudad;
                                modelo.canal = edit.canal;
                                modelo.estado = edit.estado;
                                modelo.Total = "$ " + auxTot.ToString() + " + IVA";
                                modelo.ObservacionesPricing = "No aplica";
                                modelo.ObservacionReserva = edit.ObservacionReserva;
                                modelo.TelefonoExt = edit.TelefonoExt;
                            }
                        }

                        var prods = ServicioCotizacion.ObtenerProductos();
                        Mapper.CreateMap<ProductosSelector, Dominio.Administracion.Entidades.ModeloCotizacion.Productos>().ReverseMap();
                        var productosSelect = Mapper.Map<List<ProductosSelector>>(prods);
                        produs = ServicioCotizacion.productosCotizacion(cotID).ToList();
                        modelo.productosIds = new int[produs.Count];
                        var prodCoti = (from a in prods
                                        join b in produs on a.COD_SERV equals b.COD_SERV
                                        select b.COD_SERV).ToList();

                        for (int i = 0; i < prodCoti.Count; i++)
                        {
                            modelo.productosIds[i] = prodCoti.ElementAt(i);
                        }

                        modelo.productos = productosSelect;
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("EditarCotizacion", modelo);
        }

        /// <summary>
        /// Metodo creado para almacenar el valor final de la cotización, actulizar el estado del pricing y observaciones realizadas por el pricing
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        ///
        [System.Web.Mvc.HttpPost]
        [MultipleButton(Name = "action", Argument = "GuardarCotizacion")]
        public ActionResult GuardarCotizacion_(RegistroVisitaModel modelo)
        {
            try
            {
                if (modelo.File != null && modelo.File.ContentLength > 0)
                {
                    string ext = Path.GetExtension(modelo.File.FileName);

                    bool isPDF = IsPDFHeader(modelo.File);

                    if ((ext == ".pdf" || ext == ".PDF") && isPDF)
                    {
                        var identity = (ClaimsIdentity)User.Identity;
                        var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                        var rutaPlantillas = ConfigurationManager.AppSettings["RutaPlantillasEnvioCorreos"];
                        var nombrePlantilla = ConfigurationManager.AppSettings["NombrePlantillaCotizacion"];
                        var aplicacion = ConfiguracionesGlobales.AplicacionWsUsuarios;
                        var rutaPlantillaEmail = string.Format(@"{0}\{1}\{2}", rutaPlantillas, aplicacion, nombrePlantilla);

                        if (modelo.IdCotizacion != null && modelo.ObservacionesPricing != null && modelo.Total != null)
                        {
                            modelo.rutaArchivo = modelo.File.FileName;
                            var correoAsesor = ServicioCotizacion.ObtenerCorreoAsesor(modelo.IdCotizacion);
                            string valorCoti = Utils.RemoverCaracteresEspeciales(modelo.Total);
                            var guardCoti = ServicioCotizacion.GuardarCotizacion(modelo.IdCotizacion, modelo.ObservacionesPricing, valorCoti, modelo.rutaArchivo);
                            var nombreEmpreClient = ServicioCotizacion.ObtenerEmpresaCliente(modelo.IdCotizacion);
                            var correoPricing = ServicioCotizacion.ObtenerInfoPrincing();
                            var passPricing = ServicioCotizacion.ObtenerPassPricing();

                            if (guardCoti == true)
                            {
                                string body = Transversales.Administracion.Plantillas.GestionPlantillas.ObtenerHtmlPlantilla(rutaPlantillaEmail);

                                if (!string.IsNullOrWhiteSpace(body))
                                {
                                    body = body.Replace("@@IDRESERVA@@", modelo.IdCotizacion);
                                    body = body.Replace("@@NOMBRECLIENTE@@", nombreEmpreClient);
                                    body = body.Replace("@@NIT@@", modelo.NIT);
                                    body = body.Replace("@@OBSERVACIONESPRICING@@", modelo.ObservacionesPricing);
                                }
                                else
                                {
                                    body = string.Format("Se ha asociado a la reserva {0} correspondiente al cliente {1} de NIT {2} por parte de un asesor en Tu Portal. Ingrese por favor a tu portal para mayor detalle", modelo.IdCotizacion, nombreEmpreClient, modelo.NIT);
                                }

                                //long numBytes = modelo.File.ContentLength;

                                BinaryReader b = new BinaryReader(modelo.File.InputStream);
                                b.BaseStream.Position = 0;
                                byte[] binData = b.ReadBytes(modelo.File.ContentLength);
                                //string result = System.Text.Encoding.UTF8.GetString(binData);

                                MemoryStream stre = new MemoryStream(binData);

                                Attachment oAttch = new Attachment(stre, modelo.File.FileName);

                                //Attachment oAttch = new Attachment(modelo.File.InputStream, modelo.File.FileName);
                                //Attachment oAttch = Attachment.CreateAttachmentFromString(result, modelo.File.FileName);

                                SmtpClient mySmtpClient = new SmtpClient("my.smtp.exampleserver.net");

                                mySmtpClient.UseDefaultCredentials = false;
                                mySmtpClient.Host = "outlook.office365.com";
                                //mySmtpClient.Host = "smtp.live.com";
                                mySmtpClient.Port = 587;
                                mySmtpClient.EnableSsl = true;
                                mySmtpClient.Timeout = 100000000;
                                mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                                System.Net.NetworkCredential basicAuthenticationInfo = new
                                   System.Net.NetworkCredential(correoPricing, passPricing);
                                mySmtpClient.Credentials = basicAuthenticationInfo;

                                MailAddress from = new MailAddress(correoPricing, "FromName");
                                MailAddress to = new MailAddress(correoAsesor, "ToName");
                                MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                                if (modelo.copiaDirectIds != null)
                                {
                                    var user = ServicioCotizacion.ObtenerDocumentoAsesor(modelo.IdCotizacion);
                                    var obtCorreoDirect = ServicioCotizacion.ObtenerCorreoDirector(user);

                                    if (obtCorreoDirect != null)
                                    {
                                        MailAddress copy = new MailAddress(obtCorreoDirect);
                                        myMail.CC.Add(copy);
                                    }
                                }

                                ContentDisposition disposition = oAttch.ContentDisposition;

                                disposition.DispositionType = DispositionTypeNames.Attachment;

                                myMail.Attachments.Add(oAttch);
                                //myMail.Attachments.Add(adju);
                                myMail.Subject = "Registro cotización";
                                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                                myMail.IsBodyHtml = true;
                                myMail.Body = body;
                                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                                modelo.File.InputStream.Close();
                                mySmtpClient.Send(myMail);

                                return RedirectToAction("ReservasActivas", new { msg = "Se ha cotizado la reserva." });
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Error. La reserva ya tiene una cotización asociada.");
                                //return RedirectToAction("EditarCotizacionPricing", new { msg = "Error. La reserva ya tiene una cotización asociada." });
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Información faltante o invalida.");
                            //return RedirectToAction("EditarCotizacionPricing", new { msg = "Ha ocurrido algo inesperado con la solicitud. Información faltante o invalida." });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error. El archivo seleccionado no tiene una extensión valida.");
                        //return RedirectToAction("EditarCotizacionPricing", new { msg = "Error. El archivo seleccionado no tiene una extensión valida." });
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Por favor seleccione un archivo.");
                    //return RedirectToAction("EditarCotizacionPricing", new { cotID = Convert.ToInt32(modelo.IdCotizacion) });
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }

            return View("EditarPricing", modelo);
        }

        /// <summary>
        /// Metodo creado para mostrar en un cuadro el listado de asesores con el número de reservas o cotizaciones de cada uno
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult MisAsesores(string msg)
        {
            var modelo = new RegistroVisitaModel();
            ViewBag.Mensaje = msg;
            UserClaims.ResetUserClaimsSession();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                modelo.cotizaAsesor = ServicioCotizacion.ObtenerMisAsesores(userId).ToPagedList(1, 20);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "No se ha podido mostrar la información requerida. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("CotizacionesAsesor", modelo);
        }

        /// <summary>
        /// Metodo creado para mostrar en un cuadro las reservas vigentes para ser cotizadas
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult ReservasACotizar(string msg)
        {
            var modelo = new RegistroVisitaModel();
            ViewBag.Mensaje = msg;
            UserClaims.ResetUserClaimsSession();
            try
            {
                var estPric = ServicioCotizacion.ObtenerEstadosPricing();
                modelo.estadosPricing = estPric;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                modelo.misCotizaciones = ServicioCotizacion.ObtenerMisReservasACotizar(userId).ToPagedList(1, 20);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "No se ha podido mostrar la información requerida. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("ReservasACotizar", modelo);
        }

        /// <summary>
        /// Metodo encargado de mostrar las reservas activas al Princing
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult ReservasActivas(string msg)
        {
            var modelo = new RegistroVisitaModel();
            ViewBag.Mensaje = msg;
            UserClaims.ResetUserClaimsSession();

            try
            {
                var estPric = ServicioCotizacion.ObtenerEstadosPricing();
                modelo.estadosPricing = estPric;
                //TODO: Cuando se defina el numero de rol para el princing, realizar validación de identidad (un if)
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                if (userId >= 0)
                {
                    modelo.misCotizaciones = ServicioCotizacion.ObtenerReservasActivas().ToPagedList(1, 20);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se ha podido mostrar la información requerida. Usuario invalido para realizar esta acción.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "No se ha podido mostrar la información requerida. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("CotizacionesPricing", modelo);
        }

        /// <summary>
        /// Metodo creado para filtrar por estado, NIT, nombre de cliente las reservas mostradas al pricing
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult FiltroCotizacionesPricing(RegistroVisitaModel model)
        {
            try
            {
                var estPric = ServicioCotizacion.ObtenerEstadosPricing();
                model.estadosPricing = estPric;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                model.misCotizaciones = ServicioCotizacion.ObtenerCotizacionPricingFiltros(0, model.DescripcionFiltro, model.estadoPricingID, model.NIT, model.nombreCliente, userId).ToPagedList(1, 20);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("CotizacionesPricing", model);
        }

        /// <summary>
        /// Metodo para filtrar las reservas a las cuales se va a ingresar la info para ser cotizada
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult FiltroReservasCotizaciones(RegistroVisitaModel model)
        {
            try
            {
                var estPric = ServicioCotizacion.ObtenerEstadosPricing();
                model.estadosPricing = estPric;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                model.misCotizaciones = ServicioCotizacion.FiltroReservasCotizaciones(0, model.DescripcionFiltro, model.estadoPricingID, model.NIT, model.nombreCliente, userId).ToPagedList(1, 20);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("ReservasACotizar", model);
        }

        /// <summary>
        /// Metodo creado para que un director comercial filtre por nombre y documento de asesor las visitas
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public ActionResult filtroAsesores(RegistroVisitaModel modelo)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                modelo.cotizaAsesor = ServicioCotizacion.ObtenerAsesorFiltro(0, modelo.DescripcionFiltro, modelo.numeroDocumento, modelo.nombreAsesor, userId).ToPagedList(1, 20);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("CotizacionesAsesor", modelo);
        }

        /// <summary>
        /// Metodo para el manejo del numero de paginas de mis cotizaciones
        /// </summary>
        /// <param name="page"></param>
        /// <param name="descripcionFiltro"></param>
        /// <param name="estadoCotID"></param>
        /// <param name="cotizID"></param>
        /// <returns></returns>
        public ActionResult Paginador(int page, string descripcionFiltro, int estadoCotID = 0, string cotizID = "0", int asesoID = 0)
        {
            //var modelo = new RegistroVisitaModel();
            try
            {
                var estados = ServicioCotizacion.ObtenerEstados();
                //modelo.estadosCotizacion = estados;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                if (userId != asesoID && asesoID == 0)
                {
                    var model = new RegistroVisitaModel
                    {
                        DescripcionFiltro = descripcionFiltro,
                        estadoCotiID = estadoCotID,
                        IdCotizacion = cotizID,
                        estadosCotizacion = estados,
                        misCotizaciones = ServicioCotizacion.ObtenerMisCotizaciones(userId, null).ToPagedList(page, 20),
                    };
                    return View("MisCotizaciones", model);
                }
                else
                {
                    if (userId != asesoID && asesoID != 0)
                    {
                        var model = new RegistroVisitaModel
                        {
                            DescripcionFiltro = descripcionFiltro,
                            estadoCotiID = estadoCotID,
                            IdCotizacion = cotizID,
                            estadosCotizacion = estados,
                            misCotizaciones = ServicioCotizacion.ObtenerMisCotizaciones(asesoID, null).ToPagedList(page, 20),
                        };
                        return View("MisCotizaciones", model);
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View("MisCotizaciones");
        }

        /// <summary>
        /// Metodo para el manejo del numero de paginas de mis cotizaciones, para el rol director y pricyng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="descripcionFiltro"></param>
        /// <param name="estadoCotID"></param>
        /// <param name="cotizID"></param>
        /// <returns></returns>
        //public ActionResult PaginadorJerarquia(int page, string descripcionFiltro, int estadoCotID = 0, string cotizID = "0", int asesoID = 0)
        //{
        //    try
        //    {
        //        var estados = ServicioCotizacion.ObtenerEstados();
        //        //modelo.estadosCotizacion = estados;
        //        var identity = (ClaimsIdentity)User.Identity;
        //        var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
        //        var noDocumentoDirector = ServicioCotizacion.ObtenerNoDocumentoDirector(userId);
        //        if (userId != asesoID && asesoID == 0)
        //        {
        //            var model = new RegistroVisitaModel
        //            {
        //                DescripcionFiltro = descripcionFiltro,
        //                estadoCotiID = estadoCotID,
        //                IdCotizacion = cotizID,
        //                estadosCotizacion = estados,
        //                lstCotizaciones = ServicioCotizacion.ObtenerJerarquiaCotizaciones(noDocumentoDirector.ToString(), null).ToPagedList(page, 20),
        //            };
        //            return View("MisCotizaciones", model);
        //        }
        //        else
        //        {
        //            if (userId != asesoID && asesoID != 0)
        //            {
        //                var model = new RegistroVisitaModel
        //                {
        //                    DescripcionFiltro = descripcionFiltro,
        //                    estadoCotiID = estadoCotID,
        //                    IdCotizacion = cotizID,
        //                    estadosCotizacion = estados,
        //                    lstCotizaciones = ServicioCotizacion.ObtenerJerarquiaCotizaciones(noDocumentoDirector.ToString(), null).ToPagedList(page, 20),
        //                };
        //                return View("MisCotizaciones", model);
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        ModelState.AddModelError(string.Empty, exception.Message);
        //    }
        //    return View("MisCotizaciones");
        //}

        /// <summary>
        /// Metodo para controlar las paginas de la vista de reservasACotizar
        /// </summary>
        /// <param name="page"></param>
        /// <param name="descripcionFiltro"></param>
        /// <param name="estadoCotID"></param>
        /// <param name="cotizID"></param>
        /// <returns></returns>
        public ActionResult PaginadorReservasACotizar(int page, string descripcionFiltro, int estadoCotID = 0, string cotizID = "0")
        {
            try
            {
                UserClaims.ResetUserClaimsSession();
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var estPric = ServicioCotizacion.ObtenerEstadosPricing();
                var modelo = new RegistroVisitaModel
                {
                    DescripcionFiltro = descripcionFiltro,
                    estadoCotiID = estadoCotID,
                    IdCotizacion = cotizID,
                    estadosPricing = estPric,
                    misCotizaciones = ServicioCotizacion.ObtenerMisReservasACotizar(userId).ToPagedList(page, 20),
                };
                return View("ReservasACotizar", modelo);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View("ReservasACotizar");
        }

        /// <summary>
        /// Metodo para listar en diferentes paginas las reservas vigentes para el pricing
        /// </summary>
        /// <param name="page"></param>
        /// <param name="descripcionFiltro"></param>
        /// <param name="estadoCotID"></param>
        /// <param name="cotizID"></param>
        /// <returns></returns>
        public ActionResult PaginadorPricing(int page, string descripcionFiltro, int estadoCotID = 0, string cotizID = "0")
        {
            try
            {
                var estadosPricing = ServicioCotizacion.ObtenerEstadosPricing();

                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var model = new RegistroVisitaModel
                {
                    DescripcionFiltro = descripcionFiltro,
                    estadoCotiID = estadoCotID,
                    IdCotizacion = cotizID,
                    estadosPricing = estadosPricing,
                    misCotizaciones = ServicioCotizacion.ObtenerReservasActivas().ToPagedList(page, 20)
                };
                return View("CotizacionesPricing", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View("CotizacionesPricing");
        }

        /// <summary>
        /// Metodo para el manejo del numero de paginas de los asesores de un director
        /// </summary>
        /// <param name="page"></param>
        /// <param name="descripcionFiltro"></param>
        /// <param name="estadoCotID"></param>
        /// <param name="cotizID"></param>
        /// <returns></returns>
        public ActionResult PaginadorCotizacionAsesor(int page, string descripcionFiltro, string docuAsesor, int estadoCotiID = 0, string idCotiz = "0")
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var model = new RegistroVisitaModel
                {
                    DescripcionFiltro = descripcionFiltro,
                    estadoCotiID = estadoCotiID,
                    IdCotizacion = idCotiz,
                    cotizaAsesor = ServicioCotizacion.ObtenerMisAsesores(userId).ToPagedList(page, 20),
                };

                return View("CotizacionesAsesor", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View("CotizacionesAsesor");
        }

        /// <summary>
        /// Metodo para obtener la información de una reserva seleccionada por el pricing
        /// </summary>
        /// <param name="cotID"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public ActionResult EditarCotizacionPricing(int cotID)
        {
            var modelo = new RegistroVisitaModel();
            List<Productos> produs = new List<Productos>();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                if (identity != null && cotID >= 0)
                {
                    var editPric = ServicioCotizacion.EditarCotizacionPricing(cotID, userId);
                    var userAseso = ServicioCotizacion.ObtenerDocumentoAsesor(cotID.ToString());
                    var nombreDirector = ServicioCotizacion.ObtenerNombreDirector(userAseso);
                    modelo.validacionCopiaDirector = ServicioCotizacion.ObtenerInfoCopiaDirector();
                    if (editPric != null)
                    {
                        foreach (var pric in editPric)
                        {
                            modelo.IdCotizacion = cotID.ToString();
                            modelo.NIT = pric.NIT.ToString();
                            modelo.nombreEmpresa = pric.nombreEmpresa;
                            modelo.Contacto = pric.contacto;
                            modelo.telefono = pric.telefono;
                            modelo.celular = pric.celular;
                            modelo.Cargo = pric.cargo;
                            modelo.nombreDirectorAsesor = nombreDirector;
                            modelo.correoElectronico = pric.correoElectronico;
                            modelo.MotivoVisita = pric.motivoVisita;
                            modelo.tipoAreaProt = pric.TipoAP;
                            modelo.ciudad = pric.ciudad;
                            modelo.canal = pric.canal;
                            modelo.sectorSelec = pric.SectorEconomico;
                            modelo.NumeroExpuestos = pric.NumeroExpuestos;
                            modelo.numeroCapacitaciones = pric.NumeroCapacitaciones;
                            modelo.numeroEventos = pric.NumeroEventos;
                            modelo.numeroSedes = pric.NumeroSedes;
                        }

                        var prods = ServicioCotizacion.ObtenerProductos();
                        Mapper.CreateMap<ProductosSelector, Dominio.Administracion.Entidades.ModeloCotizacion.Productos>().ReverseMap();
                        var productosSelect = Mapper.Map<List<ProductosSelector>>(prods);
                        produs = ServicioCotizacion.productosCotizacion(cotID).ToList();
                        modelo.productosIds = new int[produs.Count];
                        var prodCoti = (from a in prods
                                        join b in produs on a.COD_SERV equals b.COD_SERV
                                        select b.COD_SERV).ToList();

                        for (int i = 0; i < prodCoti.Count; i++)
                        {
                            modelo.productosIds[i] = prodCoti.ElementAt(i);
                        }

                        modelo.productos = productosSelect;
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la información requerida. Intente de nuevo si el error persiste contacte al administrador.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("EditarPricing", modelo);
        }

        /// <summary>
        /// Metodo para visualizar el listado de cotizaciones de cada asesor por parte de un director comercial
        /// </summary>
        /// <param name="asesoID"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public ActionResult CotizacionAsesor(int asesoID)
        {
            var modelo = new RegistroVisitaModel();
            List<EMA_Cotizacion> auxMisCoti = new List<EMA_Cotizacion>();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var estados = ServicioCotizacion.ObtenerEstados();
                modelo.estadosCotizacion = estados;
                modelo.misCotizaciones = ServicioCotizacion.ObtenerMisCotizaciones(asesoID, null).ToPagedList(1, 20);
                auxMisCoti = ServicioCotizacion.ObtenerMisCotizaciones(asesoID, null).ToList();
                modelo.asesoID = asesoID;
                List<string> proximosV = new List<string>();
                foreach (var coti in modelo.misCotizaciones)
                {
                    if (coti.fechaVencimiento != null)
                    {
                        DateTime auxVencimiento = Convert.ToDateTime(coti.fechaVencimiento);
                        TimeSpan numDias = DateTime.Now - auxVencimiento;

                        int diasDif = Math.Abs(numDias.Days);

                        if (diasDif <= 8 && diasDif > 0)
                        {
                            proximosV.Add(coti.NIT);
                        }
                        else
                        {
                            modelo.mensaje = "No tiene reservas próximas a vencer.";
                        }
                    }
                    else
                    {
                        modelo.mensaje = "No tiene reservas próximas a vencer.";
                    }
                }

                if (proximosV != null)
                {
                    foreach (var index in proximosV)
                    {
                        modelo.mensaje = null;
                        modelo.mensaje = string.Format("{0}|La reserva del NIT {1} esta próxima a vencerse.", modelo.mensaje, index);
                    }
                }

                foreach (var venc in auxMisCoti)
                {
                    DateTime auxVencimiento = Convert.ToDateTime(venc.fechaVencimiento);
                    double numDays = (auxVencimiento - DateTime.Now).TotalDays;

                    if (numDays <= 0)
                    {
                        var libReser = ServicioCotizacion.liberarReserva(venc.id_cotizacion.ToString(), "Se venció", 5, userId);
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("MisCotizaciones", modelo);
        }

        /// <summary>
        /// Metodo para realizar la liberación de una reserva con un cliente
        /// </summary>
        /// <param name="modelo">modelo con datos de una cotizacion especifica</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [MultipleButton(Name = "action", Argument = "LiberarReserva")]
        public ActionResult LiberarReserva_(RegistroVisitaModel modelo)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;

                var estados = ServicioCotizacion.ObtenerEstados();

                var estadofilt = (from a in estados
                                  where a.nombreEstado.Equals("Ganada") || a.nombreEstado.Equals("Perdida") || a.nombreEstado.Equals("Cerrada") || a.nombreEstado.Equals("En proceso")
                                  select a).ToList();
                modelo.estadosCotizacion = estadofilt;

                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var asesorID = ServicioCotizacion.ObtenerIDAsesor(Convert.ToInt32(modelo.IdCotizacion));
                var nombreAseor = ServicioCotizacion.ObtenerNombreAsesor(asesorID);

                var libReser = ServicioCotizacion.liberarReserva(modelo.IdCotizacion, modelo.MotivoVisita, Convert.ToInt32(modelo.estadoCotiID), userId);
                if (libReser == true)
                {
                    if (userId == asesorID)
                    {
                        return RedirectToAction("MisCotizaciones", new { msg = "Se ha liberado la reserva" });
                    }
                    else if (userId != asesorID)
                    {
                        return RedirectToAction("MisCotizaciones", new { msg = "Se ha liberado la reserva del cliente " + modelo.NIT + " perteneciente al asesor " + nombreAseor });
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La reserva ya se ha liberado previamente");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("EditarCotizacion", modelo);
        }

        /// <summary>
        /// Metodo para renovar una reservación por parte de un asesor
        /// </summary>
        /// <param name="modelo">modelo con datos de una cotizacion en especifico</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [MultipleButton(Name = "action", Argument = "RenovarReservas")]
        public ActionResult RenovarReservas_(RegistroVisitaModel modelo)
        {
            try
            {
                var estados = ServicioCotizacion.ObtenerEstados();

                var estadofilt = (from a in estados
                                  where a.nombreEstado.Equals("Ganada") || a.nombreEstado.Equals("Perdida") || a.nombreEstado.Equals("Cerrada") || a.nombreEstado.Equals("En proceso")
                                  select a).ToList();
                modelo.estadosCotizacion = estadofilt;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var asesorID = ServicioCotizacion.ObtenerIDAsesor(Convert.ToInt32(modelo.IdCotizacion));
                var nombreAseor = ServicioCotizacion.ObtenerNombreAsesor(asesorID);

                var renovReser = ServicioCotizacion.renovarReserva(modelo.IdCotizacion, modelo.MotivoVisita, Convert.ToInt32(modelo.estadoCotiID), userId);
                if (renovReser == true)
                {
                    if (userId == asesorID)
                    {
                        return RedirectToAction("MisCotizaciones", new { msg = "Se ha renovado la reserva" });
                    }
                    else if (userId != asesorID)
                    {
                        return RedirectToAction("MisCotizaciones", new { msg = "Se ha renovado la reserva del cliente " + modelo.NIT + " perteneciente al asesor " + nombreAseor });
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se ha podido renovar la reserva. La reserva no se encuentra disponible para ser renovada");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("EditarCotizacion", modelo);
        }

        /// <summary>
        /// Metodo creado para inicializar la vista de descarga de reporte de cotizaciones y reservas de asesores por director.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult CargarVistaCotizacionesYReservas(string msg)
        {
            var modelo = new RegistroVisitaModel();
            modelo.FechaInicial = DateTime.Now;
            modelo.FechaFinal = DateTime.Now;

            ViewBag.Mensaje = msg;

            return View("ReporteCotizacionesYReservas", modelo);
        }

        /// <summary>
        /// Metodo creado para generar un reporte de cotizaciones y reservas de asesores por director.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult ReporteCotizacionesAsesor(RegistroVisitaModel viewModel)
        {
            ViewBag.mensaje = "";
            try
            {
                var user = UserClaims.GetCurrentUserClaims((ClaimsIdentity)HttpContext.User.Identity);
                if (user != null)
                {
                    if (viewModel.FechaInicial != null && viewModel.FechaFinal != null)
                    {
                        string auxFechaInicial = viewModel.FechaInicial.ToString("dd/MM/yyyy");
                        string auxFechaFinal = viewModel.FechaFinal.ToString("dd/MM/yyyy");
                        Decimal auxCedula = Convert.ToDecimal(user.Document);
                        //DateTime datFechaIni = Convert.ToDateTime(auxFechaInicial);
                        //DateTime datFechaFin = Convert.ToDateTime(auxFechaFinal);

                        var parametros = new List<KeyValuePair<string, object>>();
                        parametros.Add(new KeyValuePair<string, object>("FechaInicio", (auxFechaInicial)));
                        parametros.Add(new KeyValuePair<string, object>("FechaFin", (auxFechaFinal)));
                        parametros.Add(new KeyValuePair<string, object>("DocumentoDirec", auxCedula));

                        FileStreamResult reporte = null;
                        reporte = this.Report(
                                        ReportFormat.Excel,
                            ConfiguracionesGlobales.ReportPthReporteCotizacionesAsesor,
                            ConfiguracionesGlobales.ReportesReportServerUrl,
                            parametros,
                            ConfiguracionesGlobales.ReportesUsername,
                            ConfiguracionesGlobales.ReportesPassword
                            );

                        reporte.FileDownloadName = "ReporteCotizacionesYReservasXAsesores.xls";
                        return reporte;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error.Seleccione un intervalo de fechas.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error.Usuario invalido.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al generar el informe, comuníquese con el administrador.");
            }

            return View(viewModel);
        }

        /// <summary>
        /// Metodo creado para validar el encabezado de un archivo comprobando si es pdf o no
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool IsPDFHeader(HttpPostedFileBase fileName)
        {
            byte[] buffer = null;
            BinaryReader br = new BinaryReader(fileName.InputStream);
            long numBytes = fileName.ContentLength;

            buffer = br.ReadBytes(5);

            var enc = new ASCIIEncoding();
            var header = enc.GetString(buffer);

            //%PDF−1.0
            if (buffer[0] == 0x25 && buffer[1] == 0x50
                && buffer[2] == 0x44 && buffer[3] == 0x46)
            {
                return header.StartsWith("%PDF-");
            }
            return false;
        }
    }
}