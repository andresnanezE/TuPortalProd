#region Importaciones

using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.ModeloCotizacion;
using PagedList;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

#endregion Importaciones

namespace Presentacion.Mvc.App.Controllers
{
    /// <summary>
    /// Clase controladora de metodos relacionados con la funcionalidad de Listar y editar factores para la tarifa de cotización.
    /// </summary>
    public class FactoresController : Controller
    {
        #region Atributos

        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioCotizacion _servicioCotizacion;
        private IServicioAplicacionUsuarios _servicioUsuarios;
        private IServicioAplicacionCotizadorTarifas _servicioAplicacionCotizadorTarifas;

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

        private IServicioAplicacionCotizadorTarifas ServicioAplicacionCotizadorTarifas
        {
            get { return _servicioAplicacionCotizadorTarifas ?? (_servicioAplicacionCotizadorTarifas = FabricaIoC.Resolver<IServicioAplicacionCotizadorTarifas>()); }
        }

        #endregion Atributos

        /// <summary>
        /// Metodo donde se inicia y se traen valores y las opciones de factores
        /// </summary>
        /// <param name="msg"> mensaje de exito o fracaso al momento de Listar los factores</param>
        /// <returns></returns>
        public ActionResult Index(string msg)
        {
            ViewBag.PublicSecurityKey = ConfiguracionesGlobales.PublicSecurityKey;
            ViewBag.Mensaje = msg;
            UserClaims.ResetUserClaimsSession();

            var modelo = new RegistroVisitaModel();
            try
            {
                List<Factores> listFactores = new List<Factores>();

                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                modelo.validarMdDetalleFactor = false;

                listFactores = ServicioCotizacion.ObtenerFactores();
                modelo.factores = listFactores;
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View(modelo);
        }

        /// <summary>
        ///  Método para mostrar los detalles de cada factor
        /// </summary>
        /// <param name="idFactor">Id del factor padre</param>
        /// <param name="nombreFactor">Nombre del factor padre</param>
        ///
        public ActionResult DetalleFactores(int idFactor, string nombreFactor, int idTipoFactor)
        {
            var modelo = new DetalleFactoresModel();
            modelo.IdFactor = idFactor;
            modelo.IdTipoFactor = idTipoFactor;
            modelo.Factor = nombreFactor;

            List<Usp_ObtenerDetalleValorFactor> listDetalleFactores = new List<Usp_ObtenerDetalleValorFactor>();
            UserClaims.ResetUserClaimsSession();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                modelo.ValidarMdDetalleFactor = false;

                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem
                {
                    Text = "Activo",
                    Value = "1"
                });
                listItems.Add(new SelectListItem
                {
                    Text = "Inactivo",
                    Value = "0"
                });

                modelo.ListEstadosDetalleFactor = listItems;
                modelo.ListadoFactores = ServicioAplicacionCotizadorTarifas.getDetalleFactores(idFactor).ToPagedList(1, 8);

                //Obtener Valores de Detalle del Factor
                var obtenerValorDetalleFactor = ServicioAplicacionCotizadorTarifas.getValorDetalle_Factor(modelo.IdFactor);
                if (obtenerValorDetalleFactor != null)
                {
                    modelo.ValorConstanteFactorString = obtenerValorDetalleFactor.ValorConstante.ToString();
                    modelo.ValorExponenteFactorString = obtenerValorDetalleFactor.ValorExponente.ToString();
                    modelo.ValorFactor_AjusteString = obtenerValorDetalleFactor.Factor_Ajuste.ToString();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("DetalleFactores", modelo);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult DetalleFactores(DetalleFactoresModel model)
        {
            List<Usp_ObtenerDetalleValorFactor> listDetalleFactores = new List<Usp_ObtenerDetalleValorFactor>();
            UserClaims.ResetUserClaimsSession();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                model.ValidarMdDetalleFactor = false;

                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem
                {
                    Text = "Activo",
                    Value = "1"
                });
                listItems.Add(new SelectListItem
                {
                    Text = "Inactivo",
                    Value = "0"
                });

                model.ListEstadosDetalleFactor = listItems;
                var respuestaUpdate = ServicioAplicacionCotizadorTarifas.updateValoresFactor(model.IdFactor, model.IdTipoFactor, Convert.ToDecimal(model.ValorConstanteFactorString), Convert.ToDecimal(model.ValorExponenteFactorString));

                // Validar Respuesta del update en la base de datos.
                if (respuestaUpdate == true)
                {
                    model.ValidarMsgDetalleFactor = 1;
                }
                else
                {
                    model.ValidarMsgDetalleFactor = 2;
                }

                //Obtener Valores de Detalle del Factor
                var obtenerValorDetalleFactor = ServicioAplicacionCotizadorTarifas.getValorDetalle_Factor(model.IdFactor);
                if (obtenerValorDetalleFactor != null)
                {
                    model.ValorConstanteFactorString = obtenerValorDetalleFactor.ValorConstante.ToString();
                    model.ValorExponenteFactorString = obtenerValorDetalleFactor.ValorExponente.ToString();
                }

                //Obtener Listado de Detalle del Factor
                model.ListadoFactores = ServicioAplicacionCotizadorTarifas.getDetalleFactores(model.IdFactor).ToPagedList(1, 8);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("DetalleFactores", model);
        }

        /// <summary>
        ///  Método para mostrar los detalles de cada factor
        /// </summary>
        /// <param name="idFactor">Id del factor padre</param>
        /// <param name="nombreFactor">Nombre del factor padre</param>
        /// <param name="idDetalleFactor">Id del detalle que se desea modificar</param>
        /// <param name="estadoDetalleFactor">Estado del detalle que se desea modificar</param>
        /// <param name="nombreDetalleFactor">Nombre del detalle que se desea modificar</param>
        /// <param name="descripcionDetalleFactor">Descripción detalle que se desea modificar</param>
        /// <param name="valorDetalleFactor">Valor del detalle que se desea modificar</param>
        ///
        public ActionResult DetalleFactor(int idFactor, string nombreFactor, int idDetalleFactor, string estadoDetalleFactor, string NombreDetalleFactor, string descripcionDetalleFactor, decimal valorDetalleFactor, decimal valorDetalleConstante, decimal valorDetalleExponencial, decimal valorDetalleEC)
        {
            var modelo = new DetalleFactoresModel();
            modelo.Factor = nombreFactor;

            UserClaims.ResetUserClaimsSession();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                modelo.ValidarMdDetalleFactor = true;

                if (descripcionDetalleFactor == null)
                {
                    descripcionDetalleFactor = "Sin Descripción";
                }

                modelo.IdFactor = idFactor;
                modelo.Factor = nombreFactor;
                modelo.IdDetalleFactor = idDetalleFactor;
                modelo.NombreDetalleFactor = NombreDetalleFactor;
                modelo.EstadoDetalleFactor = estadoDetalleFactor;
                modelo.DescripcionDetalleFactor = descripcionDetalleFactor;
                modelo.ValorDetalleFactorString = valorDetalleFactor.ToString();
                modelo.ValorDetalleConstanteString = valorDetalleConstante.ToString();
                modelo.ValorDetalleExponencialString = valorDetalleExponencial.ToString();

                modelo.ListadoFactores = ServicioAplicacionCotizadorTarifas.getDetalleFactores(idFactor).ToPagedList(1, 8);

                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem
                {
                    Text = "Activo",
                    Value = "1"
                });
                listItems.Add(new SelectListItem
                {
                    Text = "Inactivo",
                    Value = "0"
                });

                modelo.ListEstadosDetalleFactor = listItems;
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.");
            }
            return View("DetalleFactores", modelo);
        }

        /// <summary>
        /// Post para guardar los datos modificados del modal del detalle de factor
        /// </summary>
        /// <param name="model">RegistroVisitaModel</param>
        /// <returns>RegistroVisitaModel</returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult ModalDetalleValor(DetalleFactoresModel model)
        {
            List<Factores> listFactors = new List<Factores>();
            try
            {

                var respuestaUpdate = ServicioAplicacionCotizadorTarifas.updateDetalleFactor(
                    new Dominio.Administracion.Entidades.MapperDto.ModificarFactorDto()
                    {
                        IdFactor = model.IdFactor,
                        IdDetalleFactor = model.IdDetalleFactor,
                        NombreDetalleFactor = model.NombreDetalleFactor,
                        ValorDetalleFactor = model.ValorDetalleFactor,
                        ValorDetalleConstante = model.ValorDetalleConstante,
                        ValorDetalleExponencial = model.ValorDetalleExponencial,
                        FactorAjuste = model.Factor_Ajuste,
                        Estado = model.EstadoDetalleFactorID == 1,
                        //factor ciudad
                        ValorIndicadorFuga = model.ValorIndicadorFuga,
                        GastosAdministrativos = model.GastosAdministrativos,
                        GastosComerciales = model.GastosComerciales,
                        FactorUtilidad = model.FactorUtilidad,
                        ValorMinimoCotizacion = model.ValorMinimoCotizacion
                    }
                    );

                listFactors = ServicioCotizacion.ObtenerFactores();
                model.Factores = listFactors;
                model.Factor = model.Factor;
                model.ValidarMdDetalleFactor = false;
                model.ListadoFactores = ServicioAplicacionCotizadorTarifas.getDetalleFactores(model.IdFactor).ToPagedList(1, 8);

                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem
                {
                    Text = "Activo",
                    Value = "1"
                });
                listItems.Add(new SelectListItem
                {
                    Text = "Inactivo",
                    Value = "0"
                });
                model.ListEstadosDetalleFactor = listItems;

                // Validar Respuesta del update en la base de datos.
                if (respuestaUpdate == true)
                {
                    model.ValidarMsgDetalleFactor = 1;
                }
                else
                {
                    model.ValidarMsgDetalleFactor = 2;
                }
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                string msjRet = "Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.";
                ModelState.AddModelError(string.Empty, msjRet);
                model.Mensaje = msjRet;
            }

            return View("DetalleFactores", model);
        }

        /// <summary>
        /// Metodo para el manejo del numero de paginas de mis cotizaciones
        /// </summary>
        /// <param name="page"></param>
        /// <param name="descripcionFiltro"></param>
        /// <param name="estadoCotID"></param>
        /// <param name="cotizID"></param>
        /// <returns></returns>
        public ActionResult Paginador(int page, int idFactor)
        {
            try
            {
                var estados = ServicioCotizacion.ObtenerEstados();
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem
                {
                    Text = "Activo",
                    Value = "1"
                });
                listItems.Add(new SelectListItem
                {
                    Text = "Inactivo",
                    Value = "0"
                });

                var model = new DetalleFactoresModel
                {
                    IdFactor = idFactor,
                    ListadoFactores = ServicioAplicacionCotizadorTarifas.getDetalleFactores(idFactor).ToPagedList(page, 8),
                    ListEstadosDetalleFactor = listItems
                };

                return View("DetalleFactores", model);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }
            return View("DetalleFactores");
        }
    }
}