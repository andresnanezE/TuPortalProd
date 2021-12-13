using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Presentacion.Mvc.App.Models;
using Aplicacion.Administracion.Implementacion;
using Aplicacion.Administracion.Contratos;
using Transversales.Administracion.IoC;
using AutoMapper;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Datos.Administracion.UnidadDeTrabajo;
using System.Security.Claims;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using PagedList;
using Dominio.Administracion.Entidades;
using Presentacion.Mvc.App.Utilidades;

namespace Presentacion.Mvc.App.Controllers
{
    /// <summary>
    /// Clase controladora de metodos relacionados con la funcionalidad de bloqueo de NIT
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

        /// <summary>
        /// Metodo donde se inicia y se traen las opciones de selección multiple como check box y drop down list
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
                var prods = ServicioCotizacion.ObtenerProductos();
                Mapper.CreateMap<ProductosSelector, Dominio.Administracion.Entidades.ModeloCotizacion.Productos>().ReverseMap();
                var productosSelect = Mapper.Map<List<ProductosSelector>>(prods);

                modelo.productos = productosSelect;
                //var nivInteres = ServicioCotizacion.ObtenerNivelesInteres();
                //modelo.nivelesInteres = nivInteres;
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
                //var nivInteres = ServicioCotizacion.ObtenerNivelesInteres();
                //model.nivelesInteres = nivInteres;

                if (ModelState.IsValid)
                {
                    var userName = ViewData["CurrentUser"] == null ? null : ViewData["CurrentUser"].ToString();

                    //int userID = 0;
                    var identity = (ClaimsIdentity)User.Identity;
                    var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                    var nitClient = ServicioCotizacion.ValidarPermisoRegistroCotizacion(model.NIT, userId);
                    if (nitClient == null)
                    {
                        var auxNIT = Convert.ToInt32(model.NIT);
                        var contrat = ServicioCotizacion.validarPermisoContrato(auxNIT, userId);
                        //bool contrat = false;
                        if (contrat == false)
                        {
                            Mapper.CreateMap<Cotizaciones, RegistroVisitaModel>().ReverseMap();
                            var cotizacion = Mapper.Map<Cotizaciones>(model);

                            registro = ServicioCotizacion.RegistrarCotizacion(cotizacion, model.productosIds, userId);
                            if (registro)
                            {
                                return RedirectToAction("Index", new { msg = "La reserva ha sido exitosa" });
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Error en el registro. Verifica la información ingresada e intentalo nuevamente");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "El cliente " + model.nombreEmpresa + " tiene un contrato vigente con Emermédica");
                        }
                    }
                    else
                    {
                        if (nitClient.AsesorID != userId)
                        {
                            ModelState.AddModelError(string.Empty, "El NIT " + model.NIT + " ya se encuentra bloqueado en la cotización " + nitClient.CotizacionID + " por el asesor con ID" + nitClient.AsesorID);
                        }
                        if (nitClient.AsesorID == userId)
                        {
                            ModelState.AddModelError(string.Empty, "El NIT " + model.NIT + " ya se encuentra bloqueado en la cotización " + nitClient.CotizacionID + " por usted");
                        }
                    }


                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View(model);
        }

        /// <summary>
        ///  Método para mostrar en un cuadro el listado de cotizaciones registradas por un asesor.
        /// </summary>
        /// <param name="msg">Exito o fallida para renovar o liberar visita</param>
        public ActionResult MisCotizaciones(string msg)
        {
            var modelo = new RegistroVisitaModel();
            ViewBag.Mensaje = msg;
            UserClaims.ResetUserClaimsSession();
            try
            {
                var estados = ServicioCotizacion.ObtenerEstados();
                modelo.estadosCotizacion = estados;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                modelo.misCotizaciones = ServicioCotizacion.ObtenerMisCotizaciones(userId).ToPagedList(1, 20);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("MisCotizaciones", modelo);
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
                modelo.cotizaAsesor = ServicioCotizacion.ObtenerMisAsesores(userId).ToPagedList(1,20);
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("CotizacionesAsesor", modelo);
        }

        /// <summary>
        /// Metodo para filtrar las cotizaciones de un asesor por NIT o estado
        /// </summary>
        /// <param name="modelo">modelo con los criterios para filtrar</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult filtroCotizaciones(RegistroVisitaModel modelo)
        {

            try
            {
                var estados = ServicioCotizacion.ObtenerEstados();
                modelo.estadosCotizacion = estados;
                modelo.estadosCotizacion = estados;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                modelo.misCotizaciones = ServicioCotizacion.ObtenerCotizacionFiltros(0, modelo.DescripcionFiltro, modelo.estadoCotiID, modelo.NIT, userId).ToPagedList(1, 20);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("MisCotizaciones", modelo);
        }
        /// <summary>
        /// Metodo creado para que un director comercial filtre por nombre de asesor las visitas
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public ActionResult filtroAsesores(RegistroVisitaModel modelo)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                modelo.cotizaAsesor = ServicioCotizacion.ObtenerAsesorFiltro(0, modelo.DescripcionFiltro, modelo.numeroDocumento, userId).ToPagedList(1, 20);
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("CotizacionesAsesor", modelo);
        }

        /// <summary>
        /// Metodo para mostrar el nombre de la empresa cliente y el digito de validacion a partir de la validación del NIT y contratos vigentes
        /// </summary>
        /// <param name="model">modelo con los datos para calcular el DV y encontrar el nombre de una empresa cliente</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult ConsultarNIT(RegistroVisitaModel model)
        {
            try
            {
                var prods = ServicioCotizacion.ObtenerProductos();
                Mapper.CreateMap<ProductosSelector, Dominio.Administracion.Entidades.ModeloCotizacion.Productos>().ReverseMap();
                var productosSelect = Mapper.Map<List<ProductosSelector>>(prods);
                model.productos = productosSelect;
                //var nivInteres = ServicioCotizacion.ObtenerNivelesInteres();
                //model.nivelesInteres = nivInteres;

                if (model.NIT != null)
                {
                    var auxNIT = model.NIT;
                    if (auxNIT.Length == 9 || auxNIT.Length <= 9 || auxNIT.Length == 10)
                    {
                        decimal NIT = Convert.ToDecimal(auxNIT);
                        var reciClient = ServicioCotizacion.ObtenerNombreXNIT(NIT);
                        if (reciClient != null)
                        {
                            if (reciClient.NOM_TERC != null)
                            {
                                model.nombreEmpresa = reciClient.NOM_TERC;
                                model.DV = reciClient.DIG_VERI;
                                var permiNIT = Convert.ToInt32(auxNIT);
                                var identity = (ClaimsIdentity)User.Identity;
                                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                                var existenContrat = ServicioCotizacion.validarPermiContrato(permiNIT, userId);
                                if (existenContrat != null)
                                {
                                    foreach (var t in existenContrat.productos)
                                    {
                                        model.mensaje="El NIT ingresado ya tiene un contrato vigente en Emermédica de tipo: "+t.ToString()+
                                            ", bajo el código: "+existenContrat.idContrato.ToString()+" con vigencia hasta el día "
                                            +existenContrat.fechaExpiracion.ToString()+", registrado por el asesor "+existenContrat.nombreAsesor+
                                            " cuyo director es "+existenContrat.nombreDirector;
                                    }
                                }else
                                {
                                    model.mensaje="El NIT ingresado no tiene contratos asociados. Por favor continúe ingresando la información del formulario.";
                                }
                            }
                            if (reciClient.NOM_TERC == null)
                            {
                                if (reciClient.DIG_VERI != -1)
                                {
                                    model.DV = reciClient.DIG_VERI;
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "El NIT ingresado no es valido por favor verifica e intentalo nuevamente");
                                }
                            }
                        }
                        else
                        {
                            //ModelState.AddModelError(string.Empty, "El NIT ingresado no se encuentra registrado, por favor ingresa el nombre de la empresa cliente");

                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "El NIT ingresado no tiene una longitud valida, por favor verifica e intentalo nuevamente");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("Index", model);
        }

        /// <summary>
        /// Metodo para el manejo del numero de paginas de mis cotizaciones
        /// </summary>
        /// <param name="page"></param>
        /// <param name="descripcionFiltro"></param>
        /// <param name="estadoCotID"></param>
        /// <param name="cotizID"></param>
        /// <returns></returns>
        public ActionResult Paginador(int page, string descripcionFiltro, int estadoCotID = 0, string cotizID = "0")
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var model = new RegistroVisitaModel
                {
                    DescripcionFiltro = descripcionFiltro,
                    estadoCotiID = estadoCotID,
                    IdCotizacion = cotizID,
                    misCotizaciones = ServicioCotizacion.ObtenerCotizacionFiltros(0, descripcionFiltro, estadoCotID, cotizID, userId).ToPagedList(page, 20),

                };

                return View("MisCotizaciones", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View("MisCotizaciones");
        }

        /// <summary>
        /// Metodo para el manejo del numero de paginas de los asesores de un director
        /// </summary>
        /// <param name="page"></param>
        /// <param name="descripcionFiltro"></param>
        /// <param name="estadoCotID"></param>
        /// <param name="cotizID"></param>
        /// <returns></returns>
        public ActionResult PaginadorCotizacionAsesor(int page, string descripcionFiltro, int estadoCotID = 0, string cotizID = "0")
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var model = new RegistroVisitaModel
                {
                    DescripcionFiltro = descripcionFiltro,
                    estadoCotiID = estadoCotID,
                    IdCotizacion = cotizID,
                    misCotizaciones = ServicioCotizacion.ObtenerCotizacionFiltros(0, descripcionFiltro, estadoCotID, cotizID, userId).ToPagedList(page, 20),

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
        /// Metodo para obtener una cotizacion con el fin de editarla en una vista pertinente
        /// </summary>
        /// <param name="cotID">id de una cotizacion</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public ActionResult EditarCotizacion(int cotID)
        {
            var modelo = new RegistroVisitaModel();
            List<Productos> produs = new List<Productos>();
            try
            {
                var estados = ServicioCotizacion.ObtenerEstados();

                var estadofilt = (from a in estados
                                  where a.nombreEstado.Equals("Ganada") || a.nombreEstado.Equals("Perdida") || a.nombreEstado.Equals("Cerrada")
                                  select a).ToList();
                modelo.estadosCotizacion = estadofilt;

                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                if (identity != null)
                {
                    var editarCoti = ServicioCotizacion.EditarCotizacion(cotID, userId);
                    if (editarCoti != null)
                    {
                        foreach (var edit in editarCoti)
                        {
                            modelo.IdCotizacion = cotID.ToString();
                            modelo.NIT = edit.NIT.ToString();
                            modelo.nombreEmpresa = edit.nombreEmpresa;
                            modelo.Contacto = edit.contacto;
                            modelo.telefono = edit.telefono.ToString();
                            modelo.Cargo = edit.cargo;
                            //modelo.nivelInteres = edit.nivelInteres;
                            modelo.celular = edit.celular.ToString();
                            modelo.correoElectronico = edit.correoElectronico;
                            modelo.FechaVisita = Convert.ToDateTime(edit.fechaVisita);
                        }

                        var prods = ServicioCotizacion.ObtenerProductos();
                        Mapper.CreateMap<ProductosSelector, Dominio.Administracion.Entidades.ModeloCotizacion.Productos>().ReverseMap();
                        var productosSelect = Mapper.Map<List<ProductosSelector>>(prods);
                        produs = ServicioCotizacion.productosCotizacion(cotID).ToList();
                        modelo.productosIds = new int[produs.Count];
                        var prodCoti = (from a in prods
                                        join b in produs on a.id_producto equals b.id_producto
                                        select b.id_producto).ToList();

                        for (int i = 0; i < prodCoti.Count; i++)
                        {
                            modelo.productosIds[i] = prodCoti.ElementAt(i);
                        }

                        if (modelo.productosIds != null)
                        {
                            foreach (var prod in from prod in productosSelect from prodId in modelo.productosIds.Where(prodId => prod.id_producto == prodId) select prod)
                            {
                                prod.seleccionado = true;
                            }
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
        /// Metodo para visualizar el listado de cotizaciones de cada asesor por parte de un director comercial
        /// </summary>
        /// <param name="asesoID"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public ActionResult CotizacionAsesor(int asesoID)
        {
            var modelo = new RegistroVisitaModel();
            try
            {
                var estados = ServicioCotizacion.ObtenerEstados();
                modelo.estadosCotizacion = estados;
               
                modelo.misCotizaciones = ServicioCotizacion.ObtenerMisCotizaciones(asesoID).ToPagedList(1, 20);
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
        public ActionResult LiberarReserva(RegistroVisitaModel modelo)
        {
            try
            {
                var estados = ServicioCotizacion.ObtenerEstados();

                var estadofilt = (from a in estados
                                  where a.nombreEstado.Equals("Ganada") || a.nombreEstado.Equals("Perdida") || a.nombreEstado.Equals("Cerrada")
                                  select a).ToList();
                modelo.estadosCotizacion = estadofilt;

                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var libReser = ServicioCotizacion.liberarReserva(modelo.IdCotizacion, modelo.MotivoVisita, Convert.ToInt32(modelo.estadoCotiID));
                if (libReser == true)
                {
                    return RedirectToAction("MisCotizaciones", new { msg = "Se ha liberado la reserva" });
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
        public ActionResult RenovarReservas(RegistroVisitaModel modelo)
        {
            try
            {
                var estados = ServicioCotizacion.ObtenerEstados();

                var estadofilt = (from a in estados
                                  where a.nombreEstado.Equals("Ganada") || a.nombreEstado.Equals("Perdida") || a.nombreEstado.Equals("Cerrada")
                                  select a).ToList();
                modelo.estadosCotizacion = estadofilt;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var renovReser = ServicioCotizacion.renovarReserva(modelo.IdCotizacion, modelo.MotivoVisita, Convert.ToInt32(modelo.estadoCotiID));
                if (renovReser == true)
                {
                    return RedirectToAction("MisCotizaciones", new { msg = "Se ha renovado la reserva" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se ha podido renovar la reserva. La reserva se encuentra vencida o cerrada");
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("EditarCotizacion", modelo);
        }


    }
}