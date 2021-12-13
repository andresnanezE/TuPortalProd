using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;

//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
//using Dominio.Administracion.Entidades.MapperDto.DtoSitioWeb;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoSitioWeb;
using MvcReportViewer;
using Newtonsoft.Json;
using PagedList;
using Presentacion.Mvc.App.Helpers;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

//using Dominio.Administracion.Entidades.MapperDto;
using Transversales.Administracion;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class ContratosController : Controller
    {
        private const string NoResultados = "No se encontraron resultados";
        private readonly string MENSAJE = $"{ConfiguracionesGlobales.MsgErrorRte1} {ConfiguracionesGlobales.MsgErrorRte2}";
        private readonly string MENSAJECARGAPREVIA = $"{ConfiguracionesGlobales.MsgErrorDataPrev}";

        #region Fields

        private IAdaptadorDeObjetos _adaptadorDeObjetos;
        private IServicioAplicacionContratos _servicioAplicacion;
        private IServicioAplicacionCalcularInclusiones _saCalcularInclusiones;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion Fields

        #region Instance Properties

        private IServicioAplicacionContratos ServicioAplicacion
        {
            get { return _servicioAplicacion ?? (_servicioAplicacion = FabricaIoC.Resolver<IServicioAplicacionContratos>()); }
        }

        private IServicioAplicacionCalcularInclusiones SA_CalcularInclusiones
        {
            get { return _saCalcularInclusiones ?? (_saCalcularInclusiones = FabricaIoC.Resolver<IServicioAplicacionCalcularInclusiones>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        private IAdaptadorDeObjetos ServicioAdaptadorDeObjetos
        {
            get { return _adaptadorDeObjetos ?? (_adaptadorDeObjetos = FabricaIoC.Resolver<IAdaptadorDeObjetos>()); }
        }

        #endregion Instance Properties

        public ActionResult Index(ContratosModel model)
        {
            //Hallar rol para permitir o no ver detalle de contratos
            ViewBag.busquedaPorTexto = false;

            var user = UserClaims.GetCurrentUserClaims((ClaimsIdentity)HttpContext.User.Identity);

            var rolComercial = UserClaims.EsRolAsesor(user.Rol) ? Int32.Parse(user.Rol) : 0;
            int itmp = 0;
            if (int.TryParse(model.Criterio, out itmp))
            {
                model.ListaContratos = ServicioAplicacion.ObtenerConsultaContratos(Convert.ToDecimal(model.Criterio), decimal.Parse(user.Document), rolComercial).ToPagedList(1, 10);
            }
            else
            {
                ViewBag.busquedaPorTexto = true;
                model.ListaContratos = ServicioAplicacion.ObtenerConsultaContratosPorNombre(model.Criterio, decimal.Parse(user.Document), rolComercial).ToPagedList(1, 10);
            }

            //ViewBag.Roll = roles[0];
            ViewBag.Roll = user.Rol;

            if (!model.ListaContratos.Any() && model.Codigo != 0)
            {
                model.Mensaje = "a";
            }

            if (model.ListaContratos.Any())
            {
                model.Nom_bene = (model.ListaContratos.FirstOrDefault().NOM_BENE != "") ? model.ListaContratos.FirstOrDefault().NOM_BENE + " - " + model.Criterio : "";
                foreach (var contrato in model.ListaContratos)
                {
                    contrato.MostrarBoton = (contrato.Ver_Detalle) ? true : false;
                }
            }

            //Agrega Actividad
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            if ((Session["esSesionEnVezDe"] != null) && (Boolean.Parse(Session["esSesionEnVezDe"].ToString())))
            {
                var documentoPadre = (identity.FindFirst(ClaimTypes.Anonymous).Value).ToString();

                var nombrePadre = (identity.FindFirst(ClaimTypes.Authentication).Value).ToString();

                var documentoHijo = (identity.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
                var nombreHijo = (identity.FindFirst(ClaimTypes.Actor).Value).ToString();

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioId,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Consulta.ConsultaDeContratos,
                    ip = ip,
                    MenuId = (int)Menus.Contratos,
                    DocUsuario1 = documentoPadre,
                    NombreUsuario1 = nombrePadre,
                    DocUsuario2 = documentoHijo,
                    NombreUsuario2 = nombreHijo,
                    FechaHoraIni = DateTime.Now,
                    FechaHoraFin = DateTime.Now,
                    TiempoSesion = TimeSpan.Zero,
                    EsSesionEnVezDe = true
                };

                ServicioAplicacionLogs.AgregarLog(log);
            }
            else
            {
                EmbLogActividadesDto log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaDeContratos;
                log.ip = ip;
                log.MenuId = (int)Menus.Contratos;

                ServicioAplicacionLogs.AgregarLog(log);
            }
            //
            return View(model);
        }

        public ActionResult Contratos(decimal busqueda)
        {
            var model = new ContratosModel();
            ////Hallar rol para permitir o no ver detalle de contratos

            var user = UserClaims.GetCurrentUserClaims((ClaimsIdentity)HttpContext.User.Identity);

            var rolComercial = UserClaims.EsRolAsesor(user.Rol) ? Int32.Parse(UserClaims.RolesAsesor.First()) :
                UserClaims.EsRolDirector(user.Rol) ? Int32.Parse(UserClaims.RolesDirector.First()) : 0;

            model.ListaContratos = ServicioAplicacion.ObtenerConsultaContratos(model.Codigo, decimal.Parse(user.Document), rolComercial).ToPagedList(1, 10);

            model.Busqueda = busqueda;
            if (model.ListaContratos.Count == 0)
            {
                model.Mensaje = "a";
            }
            if (model.ListaContratos.Count == 0 && UserClaims.EsRolAsesor(user.Rol))
            {
                model.Mensaje = user.Rol;
            }
            if (model.ListaContratos.Count == 0 && UserClaims.EsRolDirector(user.Rol))
            {
                model.Mensaje = user.Rol;
            }

            //agrega actividad log
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            if ((Session["esSesionEnVezDe"] != null) && (Boolean.Parse(Session["esSesionEnVezDe"].ToString())))
            {
                var documentoPadre = (identity.FindFirst(ClaimTypes.Anonymous).Value).ToString();

                var nombrePadre = (identity.FindFirst(ClaimTypes.Authentication).Value).ToString();

                var documentoHijo = (identity.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
                var nombreHijo = (identity.FindFirst(ClaimTypes.Actor).Value).ToString();

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioId,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Consulta.ConsultaDeContratos,
                    ip = ip,
                    MenuId = (int)Menus.Contratos,
                    DocUsuario1 = documentoPadre,
                    NombreUsuario1 = nombrePadre,
                    DocUsuario2 = documentoHijo,
                    NombreUsuario2 = nombreHijo,
                    FechaHoraIni = DateTime.Now,
                    FechaHoraFin = DateTime.Now,
                    TiempoSesion = TimeSpan.Zero,
                    EsSesionEnVezDe = true
                };

                ServicioAplicacionLogs.AgregarLog(log);
            }
            else
            {
                EmbLogActividadesDto log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaDeContratos;
                log.ip = ip;
                log.MenuId = (int)Menus.Contratos;

                ServicioAplicacionLogs.AgregarLog(log);
            }
            //

            return View("Index", model);
        }

        public ActionResult Fosyga()
        {
            //agrega actividad log
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            if ((Session["esSesionEnVezDe"] != null) && (Boolean.Parse(Session["esSesionEnVezDe"].ToString())))
            {
                var documentoPadre = (identity.FindFirst(ClaimTypes.Anonymous).Value).ToString();

                var nombrePadre = (identity.FindFirst(ClaimTypes.Authentication).Value).ToString();

                var documentoHijo = (identity.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
                var nombreHijo = (identity.FindFirst(ClaimTypes.Actor).Value).ToString();

                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioId,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Consulta.ConsultaFosyga,
                    ip = ip,
                    MenuId = (int)Menus.Contratos,
                    DocUsuario1 = documentoPadre,
                    NombreUsuario1 = nombrePadre,
                    DocUsuario2 = documentoHijo,
                    NombreUsuario2 = nombreHijo,
                    FechaHoraIni = DateTime.Now,
                    FechaHoraFin = DateTime.Now,
                    TiempoSesion = TimeSpan.Zero,
                    EsSesionEnVezDe = true
                };

                ServicioAplicacionLogs.AgregarLog(log);
            }
            else
            {
                EmbLogActividadesDto log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.ConsultaFosyga;
                log.ip = ip;
                log.MenuId = (int)Menus.Contratos;

                ServicioAplicacionLogs.AgregarLog(log);
            }
            //

            //
            //return JavaScript("window.setTimeout(window.location.href='http://www.fosyga.gov.co/Consultas/BDUABasedeDatosUnicadeAfiliados/AfiliadosBDUA/tabid/436/Default.aspx';,1000)");
            // return Redirect("http://www.fosyga.gov.co/Consultas/BDUABasedeDatosUnicadeAfiliados/AfiliadosBDUA/tabid/436/Default.aspx");
            return View("");
        }

        public ActionResult Paginador(int page, decimal codigo)
        {
            var model = new ContratosModel();

            //model.Codigo = codigo;
            //var identity = (ClaimsIdentity)HttpContext.User.Identity;
            //var documento = decimal.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            //var claimRol = identity.FindFirst(ClaimTypes.Role).Value;
            //var roles = claimRol.Split(',');
            //var listaRoles = (from role in roles where !string.IsNullOrWhiteSpace(role) select int.Parse(role)).Where(x => x.ToString() == "8").ToList();
            //var rolComercial = listaRoles.FirstOrDefault().ToString() == "8" ? 8 : 0;
            //model.ListaContratos = ServicioAplicacion.ObtenerConsultaContratos(codigo, documento, rolComercial).ToPagedList(page, 10);

            var user = UserClaims.GetCurrentUserClaims((ClaimsIdentity)HttpContext.User.Identity);

            var rolComercial = UserClaims.EsRolAsesor(user.Rol) ? Int32.Parse(UserClaims.RolesAsesor.First()) : 0;

            model.ListaContratos = ServicioAplicacion.ObtenerConsultaContratos(model.Codigo, decimal.Parse(user.Document), rolComercial).ToPagedList(1, 10);

            return View("Index", model);
        }

        /// <summary>
        /// Modificado por JohnNelsonRodriguex
        /// </summary>
        /// <param name="busqueda"></param>
        /// <param name="rmt"></param>
        /// <param name="contratante"></param>
        /// <param name="tipoContrato"></param>
        /// <param name="tipoBusqueda"></param>
        /// <param name="nombreContratante"></param>
        /// <param name="numCont"></param>
        /// <param name="prefijo"></param>
        /// <returns></returns>
        public ActionResult DetalleContrato(decimal? busqueda, int rmt, decimal contratante, string tipoContrato, string tipoBusqueda, string nombreContratante, string numCont, string prefijo = "", string mensaje = "")
        {
            try
            {
                var detalleContrato = new DetalleContrato()
                {
                    Busqueda = busqueda,
                    Rmt = rmt,
                    Contratante = contratante,
                    TipoContrato = tipoContrato,
                    TipoBusqueda = tipoBusqueda,
                    NombreContratante = nombreContratante,
                    NumCont = numCont,
                    Prefijo = prefijo
                };

                Session["DetalleContrato"] = detalleContrato;

                var model = new ContratosModel();

                model.NumCont = string.Format("{0}-{1}", prefijo, numCont);
                model.viewBene = "none";
                model.viewPMP = "none";
                model.viewAP = "none";
                var codigo = busqueda ?? 0;
                model.Busqueda = codigo;
                if (tipoContrato == null || tipoContrato == "null")
                {
                    ViewBag.Title = "Contrato";
                    model.Mensaje = "Bienvenido";
                    ViewBag.Facturas = new List<ConsultaFacturaDto>();
                    model.FechaFin = DateTime.Now.AddDays(-5);

                    return View(model);
                }
                if (numCont != null) if (busqueda != null) codigo = numCont.Contains(busqueda.ToString()) ? rmt : busqueda.Value;
                var contratoPorRmt = new SPEM_CONSULTACONTRATOSDto();

                #region RMT

                if (tipoBusqueda.ToLower() == "rmt")
                {
                    contratoPorRmt = ServicioAplicacion.ObtenerContratos(codigo).FirstOrDefault(x => x.RmtCont == rmt);
                    model.Identificacion = contratante.ToString("0");
                    model.Rmt_Cont = rmt.ToString("0");
                    model.TipoContrato = tipoContrato;
                    model.Contratante = contratoPorRmt.Nombre;
                    model.Telefono = contratoPorRmt.Telefono;
                    model.Estado = contratoPorRmt.Estado;
                    model.FechaInicio = contratoPorRmt.FechaI.Date;
                    model.FechaFin = contratoPorRmt.FechaV.Date;
                    model.Asesor = contratoPorRmt.nom_comp;
                    model.Cantidad = contratoPorRmt.num_pers.ToString("0");
                    model.IdentificacionAse = contratoPorRmt.cod_ases.ToString("C0");
                    model.TelefonoAse = contratoPorRmt.TelefonoAse; model.EstadoAse = contratoPorRmt.EstadoAse;
                    model.ModoPago = contratoPorRmt.ModoPago;
                    model.FormaPago = contratoPorRmt.FormaPago;
                    model.ValorContrato = contratoPorRmt.ValorContrato.ToString("C0");
                    model.ValorCartera = contratoPorRmt.Cartera != null ? contratoPorRmt.Cartera.Value.ToString("C0") : "0";
                    model.CuotaMensual = contratoPorRmt.CuotaMensual.ToString("C0");

                    //switch (tipoContrato)
                    //{
                    //    case "PPE":
                    //        model.Titulo = "PPE";
                    //        model.viewPMP = "visible";
                    //        model.fCorte = contratoPorRmt.fCorte;
                    //        model.diasFaltantes = contratoPorRmt.diasFaltantes;
                    //        model.diasProrrateo = contratoPorRmt.diasProrrateo;
                    //        //Afiliados, Factura y Estado de cuenta.
                    //        break;

                    //    case "AP":
                    //        model.Titulo = "AP";
                    //        model.viewAP = "visible";
                    //        //Factura
                    //        break;

                    //    case "FAM":
                    //        model.Titulo = "Familiar";
                    //        model.viewPMP = "visible";
                    //        //Afiliados, Factura y Estado de cuenta.
                    //        break;
                    //}
                }

                #endregion RMT

                #region BENEFICIARIO

                if (tipoBusqueda.ToLower() == "cc")
                {
                    contratoPorRmt = ServicioAplicacion.ObtenerContratosBeneficiario(codigo, rmt, contratante, nombreContratante.TrimEnd(' '))
                        .FirstOrDefault();
                    var bene = ServicioAplicacion.ObtenerBeneficiario(codigo.ToString()).FirstOrDefault();
                    model.Titulo = "Beneficiario";
                    model.Identificacion = contratante.ToString("0");
                    model.Rmt_Cont = rmt.ToString("0");
                    model.TipoContrato = tipoContrato;
                    model.Contratante = contratoPorRmt.Nombre;
                    model.Telefono = contratoPorRmt.Telefono;
                    model.Estado = contratoPorRmt.Estado;
                    model.FechaInicio = contratoPorRmt.FechaI.Date;
                    model.FechaFin = contratoPorRmt.FechaV.Date;
                    model.Asesor = contratoPorRmt.nom_comp;
                    model.Cantidad = contratoPorRmt.num_pers.ToString("0");
                    model.IdentificacionAse = contratoPorRmt.cod_ases.ToString("C0");
                    model.TelefonoAse = contratoPorRmt.TelefonoAse;
                    model.EstadoAse = contratoPorRmt.EstadoAse;
                    model.ModoPago = contratoPorRmt.ModoPago;
                    model.FormaPago = contratoPorRmt.FormaPago;
                    model.ValorContrato = contratoPorRmt.ValorContrato.ToString("C0");
                    model.ValorCartera = contratoPorRmt.Cartera != null ? contratoPorRmt.Cartera.Value.ToString("C0") : "0";
                    model.CuotaMensual = contratoPorRmt.CuotaMensual.ToString("C0");
                    model.Num_idenBen = bene.Num_iden.ToString("0");
                    model.Nom_bene = bene.Nom_bene + " " + bene.Ape_bene;
                    model.Fec_naciBen = bene.Fec_naci.Date;
                    model.Tel_bene = bene.Tel_bene;
                    model.viewBene = "visible";
                    //return View(model);
                }

                #endregion BENEFICIARIO

                #region Contratante

                if (tipoBusqueda.ToLower() == "terc")
                {
                    model.Titulo = "Contratante";
                    contratoPorRmt = ServicioAplicacion.ObtenerContratosContratante(contratante, rmt).FirstOrDefault();
                    model.Identificacion = contratante.ToString("0");
                    model.Rmt_Cont = rmt.ToString("0");
                    model.TipoContrato = tipoContrato;
                    model.Contratante = contratoPorRmt.Nombre;
                    model.Telefono = contratoPorRmt.Telefono;
                    model.Estado = contratoPorRmt.Estado;
                    model.FechaInicio = contratoPorRmt.FechaI.Date;
                    model.FechaFin = contratoPorRmt.FechaV.Date;
                    model.Asesor = contratoPorRmt.nom_comp;
                    model.Cantidad = contratoPorRmt.num_pers.ToString("0");
                    model.IdentificacionAse = contratoPorRmt.cod_ases.ToString("C0");
                    model.TelefonoAse = contratoPorRmt.TelefonoAse;
                    model.EstadoAse = contratoPorRmt.EstadoAse;
                    model.ModoPago = contratoPorRmt.ModoPago;
                    model.FormaPago = contratoPorRmt.FormaPago;
                    model.ValorContrato = contratoPorRmt.ValorContrato.ToString("C0");
                    model.ValorCartera = contratoPorRmt.Cartera != null ? contratoPorRmt.Cartera.Value.ToString("C0") : "0";
                    model.CuotaMensual = contratoPorRmt.CuotaMensual.ToString("C0");
                    //Beneficiarios y Factura
                }

                #endregion Contratante

                switch (tipoContrato)
                {
                    case "PPE":
                        model.Titulo = "PPE";
                        model.viewPMP = "visible";
                        model.fCorte = contratoPorRmt.fCorte;
                        model.diasFaltantes = contratoPorRmt.diasFaltantes;
                        model.diasProrrateo = contratoPorRmt.diasProrrateo;
                        //Afiliados, Factura y Estado de cuenta.
                        break;

                    case "AP":
                        model.Titulo = "AP";
                        model.viewAP = "visible";
                        //Factura
                        break;

                    case "FAM":
                        model.Titulo = "Familiar";
                        model.viewPMP = "visible";
                        model.fCorte = contratoPorRmt.fCorte;
                        model.diasFaltantes = contratoPorRmt.diasFaltantes;
                        model.diasProrrateo = contratoPorRmt.diasProrrateo;
                        break;

                    default:
                        break;
                        //Afiliados, Factura y Estado de cuenta.
                }

                if (!string.IsNullOrEmpty(mensaje))
                {
                    ViewBag.Mensaje = mensaje;
                }

                //tomo las facturas existente, las ordeno decendentemente, y tomo las ultimas 5
                List<ConsultaFacturaDto> facturas = ServicioAplicacion.ObtenerFacturas(model.Identificacion).OrderByDescending(x => x.FECHA_INICIAL).Take(5).ToList();

                //Esto es para obtener el nombre del mes
                DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
                foreach (var factura in facturas)
                {
                    factura.PERIODO_FACTURA = dtinfo.GetMonthName(Convert.ToDateTime(factura.FECHA_INICIAL).Month);
                }

                ViewBag.Facturas = facturas.OrderBy(x => x.FECHA_INICIAL).ToList();

                //agrega actividad log
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var identity = (ClaimsIdentity)HttpContext.User.Identity;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.DetalleContrato;
                log.ip = ip;
                log.MenuId = (int)Menus.Contratos;

                ServicioAplicacionLogs.AgregarLog(log);
                //

                return View(model);
            }
            catch (SystemException ex)
            {
                ServicioAplicacion.LogExepxion(ex, "Error", "DetalleContrato");
                return null;
            }
        }

        /// <summary>
        /// JohnNelsonRodriguex
        /// </summary>
        /// <param name="rmt"></param>
        /// <param name="nPersonas"></param>
        /// <param name="fInclusion"></param>
        /// <param name="tarifa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CalcularCuotaInclusiones(int rmt, int nPersonas, DateTime fInclusion, decimal tarifa)
        {
            try
            {
                var lista = SA_CalcularInclusiones.CalcularInclusiones(rmt, nPersonas, fInclusion, tarifa);

                return Json(lista);
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        /// <summary>
        /// JohnNelsonRodriguex
        /// </summary>
        /// <param name="rmt"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TarifasContrato(int rmt)
        {
            try
            {
                //var lista = SA_CalcularInclusiones.TarifasContrato(rmt);
                var lista = SA_CalcularInclusiones.TarifasInclusiones(rmt);

                if (!lista.Any())
                {
                    return Json(new { mensaje = "No existen tarifas para este contrato.</br>Este valor se puede ingresar manualmente en el campo Tarifas." });
                }

                return Json(lista);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                return Json(new { mensaje = "Ha surgido algo inesperado recuperando las tarifas." });
            }
        }

        public ActionResult DescargarAnexoFactura_Old(int Rmt_Cont, string Identificacion)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            EmbLogActividadesDto log = new EmbLogActividadesDto();
            log.UsuarioId = usuarioId;
            log.fecha = DateTime.Now;
            log.idTipoLog = (int)Descarga.GeneracionDeAnexoFactura;
            log.ip = ip;
            log.MenuId = (int)Menus.Contratos;
            ServicioAplicacionLogs.AgregarLog(log);

            var inicio = DateTime.Now.AddMonths(-1);

            var facts = ServicioAplicacion.ObtenerFacturas(Identificacion);

            if (facts.Any())
            {
                var filtro = facts.OrderByDescending(x => x.FECHA_INICIAL).FirstOrDefault();
                try
                {
                    return this.Report(
                              ReportFormat.PDF,
                              ConfiguracionesGlobales.ReportPathAnexoFacturaContrato,
                              ConfiguracionesGlobales.ReportesReportServerUrl,
                              new List<KeyValuePair<string, object>>
                              {
                                    new KeyValuePair<string, object>("NUM_DOCU", filtro.NUM_DOCU),
                                    new KeyValuePair<string, object>("COD_DOCU", filtro.COD_DOCU)
                              },
                              ConfiguracionesGlobales.ReportesUsername,
                              ConfiguracionesGlobales.ReportesPassword
                    );
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                }
            }
            return Json("No se encuentra factura pendiente");
        }

        /// <summary>
        /// Modificado por John Nelson Rodrigiguez.
        /// Marzo 2017.
        /// No se manejaba excepcion. Ahora en caso de excepcion se
        /// invoca el action DetalleContrato, enviando un mensaje de error.
        /// </summary>
        /// <param name="Rmt_Cont"></param>
        /// <param name="Identificacion"></param>
        /// <returns></returns>
        public ActionResult DescargarAnexoFactura(int Rmt_Cont, string Identificacion)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            string mensaje = string.Empty;
            EmbLogActividadesDto log = new EmbLogActividadesDto();

            try
            {
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Descarga.GeneracionDeAnexoFactura;
                log.ip = ip;
                log.MenuId = (int)Menus.Contratos;
                ServicioAplicacionLogs.AgregarLog(log);

                var inicio = DateTime.Now.AddMonths(-1);

                var facts = ServicioAplicacion.ObtenerFacturas(Identificacion);

                if (facts.Any())
                {
                    var filtro = facts.OrderByDescending(x => x.FECHA_INICIAL).FirstOrDefault();

                    return this.Report(
                              ReportFormat.PDF,
                              ConfiguracionesGlobales.ReportPathAnexoFacturaContrato,
                              ConfiguracionesGlobales.ReportesReportServerUrl,
                              new List<KeyValuePair<string, object>>
                              {
                                    new KeyValuePair<string, object>("NUM_DOCU", filtro.NUM_DOCU),
                                    new KeyValuePair<string, object>("COD_DOCU", filtro.COD_DOCU)
                              },
                              ConfiguracionesGlobales.ReportesUsername,
                              ConfiguracionesGlobales.ReportesPassword
                    );
                }
                else
                {
                    mensaje = "No se encuentran facturas pendientes";
                    throw new Exception(mensaje);
                }
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                if (Session["DetalleContrato"] != null)
                {
                    var detalleContrato = Session["DetalleContrato"] as DetalleContrato;

                    if (string.IsNullOrEmpty(mensaje))
                    {
                        mensaje = "Ha ocurrido algo inesperado recuperando el Anexo factura.";
                    }

                    return RedirectToAction("DetalleContrato", new
                    {
                        busqueda = detalleContrato.Busqueda,
                        rmt = detalleContrato.Rmt,
                        contratante = detalleContrato.Contratante,
                        tipoContrato = detalleContrato.TipoContrato,
                        tipoBusqueda = detalleContrato.TipoBusqueda,
                        nombreContratante = detalleContrato.NombreContratante,
                        numCont = detalleContrato.NumCont,
                        prefijo = detalleContrato.Prefijo,
                        mensaje = mensaje
                    });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }

        /// <summary>
        /// Modificado por John Nelson Rodrigiguez.
        /// Marzo 2017.
        /// No se manejaba excepcion. Ahora en caso de excepcion se
        /// invoca el action DetalleContrato, enviando un mensaje de error.
        /// </summary>
        /// <param name="Rmt_Cont"></param>
        /// <param name="Identificacion"></param>
        /// <param name="numFactura">el inidce en la lista de facturas (de 1 a 5 )</param>
        /// <returns></returns>
        public ActionResult DescargarFactura(int Rmt_Cont, string Identificacion, string periodo)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            string mensaje = string.Empty;
            EmbLogActividadesDto log = new EmbLogActividadesDto();

            try
            {
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Descarga.GeneracionDeFactura;
                log.ip = ip;
                log.MenuId = (int)Menus.Contratos;
                ServicioAplicacionLogs.AgregarLog(log);

                var inicio = DateTime.Now.AddMonths(-1);
                var fin = DateTime.Now;

                //tomo las facturas existentes
                var facts = ServicioAplicacion.ObtenerFacturas(Identificacion);
                //las ordeno de manera decendente (La mas reciente primero) y tomo las ultimas deacuerdo al periodo que la persona haya seleccionado
                var facts2 = ServicioAplicacion.ObtenerFacturas(Identificacion).OrderByDescending(x => x.FECHA_INICIAL).Take(5);

                if (facts.Any())
                {
                    var filtro = facts2.OrderBy(x => x.FECHA_INICIAL).FirstOrDefault(x => x.FECHA_INICIAL == Convert.ToDateTime(periodo));

                    return this.Report(
                                       ReportFormat.PDF,
                        ConfiguracionesGlobales.ReportPathFacturaContrato,
                        ConfiguracionesGlobales.ReportesReportServerUrl,
                        new List<KeyValuePair<string, object>>
                    {
                    new KeyValuePair<string, object>("NUM_DOCU", filtro.NUM_DOCU),
                    new KeyValuePair<string, object>("COD_DOCU", filtro.COD_DOCU),
                    new KeyValuePair<string, object>("USUARIO", usuarioId)
                    },
                        ConfiguracionesGlobales.ReportesUsername,
                        ConfiguracionesGlobales.ReportesPassword
                        );
                }
                else
                {
                    mensaje = "No se encuentran facturas pendientes";
                    throw new Exception(mensaje);
                }
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                if (Session["DetalleContrato"] != null)
                {
                    var detalleContrato = Session["DetalleContrato"] as DetalleContrato;

                    if (string.IsNullOrEmpty(mensaje))
                    {
                        mensaje = "Ha ocurrido algo inesperado recuperando la factura.";
                    }

                    return RedirectToAction("DetalleContrato", new
                    {
                        busqueda = detalleContrato.Busqueda,
                        rmt = detalleContrato.Rmt,
                        contratante = detalleContrato.Contratante,
                        tipoContrato = detalleContrato.TipoContrato,
                        tipoBusqueda = detalleContrato.TipoBusqueda,
                        nombreContratante = detalleContrato.NombreContratante,
                        numCont = detalleContrato.NumCont,
                        prefijo = detalleContrato.Prefijo,
                        mensaje = mensaje
                    });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult EstadoCuentaExcel(int Rmt_Cont)
        {
            InsertarLogAccionDescarga(Descarga.GeneracionDeEstadoDeCuenta, Menus.Contratos);

            return this.Report(
                ReportFormat.Excel,
                ConfiguracionesGlobales.ReportPathEstadoCuenta,
                ConfiguracionesGlobales.ReportesReportServerUrl,
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("RMT_CONT", Rmt_Cont)
                },
                ConfiguracionesGlobales.ReportesUsername,
                ConfiguracionesGlobales.ReportesPassword
                );
        }

        public ActionResult RelacionAfiliados(int Rmt_Cont)
        {
            InsertarLogAccionDescarga(Descarga.GeneracionDeReporteAfiliados, Menus.Contratos);
            return this.Report(
                               ReportFormat.Excel,
                ConfiguracionesGlobales.ReportesReportPathRelacionAfiliados,
                ConfiguracionesGlobales.ReportesReportServerUrl,
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("RMT_CONT", Rmt_Cont)
                },
                ConfiguracionesGlobales.ReportesUsername,
                ConfiguracionesGlobales.ReportesPassword
                );
        }

        /// <summary>
        /// Obtiene los usuarios cancelados para un contrato determinado.
        /// </summary>
        /// <param name="Rmt_Cont">Numero del contrato</param>
        /// <returns>reporte de los usuario cancelados para ese contrato en formato exel</returns>
        public ActionResult RelacionCancelados(int Rmt_Cont)
        {
            InsertarLogAccionDescarga(Descarga.GeneracionDeReporteAfiliados, Menus.Contratos);
            FileStreamResult fsr;
            fsr = this.Report(
                               ReportFormat.Excel,
                ConfiguracionesGlobales.ReportesReportPathRelacionCancelados,
                ConfiguracionesGlobales.ReportesReportServerUrl,
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("NUM_CONT", Rmt_Cont)
                },
                ConfiguracionesGlobales.ReportesUsername,
                ConfiguracionesGlobales.ReportesPassword
                );
            return fsr;
        }

        public ActionResult Atenciones(ContratosModel modelo)
        {
            FileStreamResult reporte = null;

            string ahora = string.Empty;
            string handle = string.Empty;
            string nomRte = string.Empty;

            try
            {
                InsertarLogAccionDescarga(Descarga.GeneracionDeReporteAfiliados, Menus.Contratos);

                reporte = this.Report(
                                   ReportFormat.Excel,
                    ConfiguracionesGlobales.ReportesReportPathAtenciones,
                    ConfiguracionesGlobales.ReportesReportServerUrl,
                    new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("RmtCont", modelo.Rmt_Cont)
                },
                    ConfiguracionesGlobales.ReportesUsername,
                    ConfiguracionesGlobales.ReportesPassword
                    );

                nomRte = modelo.NOMBRE_RTE_ATENCIONES;
                reporte.FileDownloadName = nomRte;

                ahora = DateTime.Now.ToString("s");
                handle = Guid.NewGuid().ToString();

                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();

                return Json(new { FileGuid = handle, FileName = "REPORTE ATENCIONES.xls" });
            }
            catch (Exception exception)
            {
                return Json(new
                {
                    msgError = MENSAJE,
                    msgErrorException =
                    $"{exception.Source}</br>{exception.Message}</br>{exception.StackTrace}"
                });
            }
        }

        [HttpGet]
        public ActionResult ValidarInclusion()
        {
            var model = new ValidarInclusionModel();
            model.TipoConsulta = TipoConsultaList(model.TipoConsultaValor);

            return View(model);
        }

        [HttpPost]
        public ActionResult ValidarInclusion(ValidarInclusionModel ViewModel)
        {
            var model = new ValidarInclusionModel();
            var inclusion = ServicioAplicacion.ValidacionBeneficiarioInclusion(ViewModel.RmtCont, ViewModel.CodTerc, ViewModel.CodBene);

            var contratanteList = ServicioAdaptadorDeObjetos.Adaptar<IEnumerable<ValidarContratante>>(inclusion);
            var beneficiarioList = ServicioAdaptadorDeObjetos.Adaptar<IEnumerable<ValidarBeneficiario>>(inclusion);

            model.RmtCont = ViewModel.RmtCont;
            model.CodTerc = ViewModel.CodTerc;
            model.CodBene = ViewModel.CodBene;
            switch (ViewModel.TipoConsultaValor)
            {
                case "cont":
                    {
                        model.ContratanteList = contratanteList.DistinctBy(e => e.Rmt_Cont).DistinctBy(e => e.Cod_Contra).ToList();
                        model.BeneficiarioList = new List<ValidarBeneficiario>();
                        break;
                    }

                case "bene":
                    {
                        model.ContratanteList = new List<ValidarContratante>();
                        model.BeneficiarioList = beneficiarioList.DistinctBy(e => e.Cod_bene).ToList();
                        break;
                    }

                default:
                    {
                        model.ContratanteList = contratanteList.DistinctBy(e => e.Rmt_Cont).DistinctBy(e => e.Cod_Contra).ToList();
                        model.BeneficiarioList = beneficiarioList.DistinctBy(e => e.Cod_bene).ToList();
                        break;
                    }
            }

            model.TipoConsulta = TipoConsultaList(ViewModel.TipoConsultaValor);
            if (inclusion.Count() <= 0)
            {
                model.Mensaje = NoResultados;
            }
            return View(model);
        }

        public List<SelectListItem> TipoConsultaList(string selected)
        {
            var resultado = new List<SelectListItem>();
            if (string.IsNullOrEmpty(selected))
            {
                resultado.Add(new SelectListItem { Text = "Contratante", Value = "cont" });
                resultado.Add(new SelectListItem { Text = "Beneficiario", Value = "bene" });
            }
            else
            {
                resultado.Add(new SelectListItem { Text = "Contratante", Value = "cont", Selected = selected == "cont" ? true : false });
                resultado.Add(new SelectListItem { Text = "Beneficiario", Value = "bene", Selected = selected == "bene" ? true : false });
            }

            return resultado;
        }

        public ActionResult RelacionBeneficiarios(int Rmt_Cont)
        {
            InsertarLogAccionDescarga(Descarga.GeneracionDeRelacionDeBeneficiarios, Menus.Contratos);

            return this.Report(
                               ReportFormat.Excel,
                ConfiguracionesGlobales.ReportPathRelacionBeneficiarios,
                ConfiguracionesGlobales.ReportesReportServerUrl,
                new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("RMT_CONT", Rmt_Cont)
                },
                ConfiguracionesGlobales.ReportesUsername,
                ConfiguracionesGlobales.ReportesPassword
                );
        }

        /// <summary>
        /// inserta un log en la bd con la accion efectuada
        /// </summary>
        /// <param name="idTipoLog">elemento de la enumeracion de tipo de log</param>
        /// <param name="MenuId">elemento de la enumeracion de menus</param>
        public void InsertarLogAccionDescarga(Descarga idTipoLog, Menus MenuId)
        {
            try
            {
                var identity = (ClaimsIdentity)HttpContext.User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                EmbLogActividadesDto log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioId,
                    fecha = DateTime.Now,
                    idTipoLog = (int)idTipoLog,
                    ip = ip,
                    MenuId = (int)MenuId
                };
                ServicioAplicacionLogs.AgregarLog(log);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> ReporteFactura(FacturaAnexosViewModel viewModel)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfiguracionesGlobales.ApiUrlFacturacion);
                string json = JsonConvert.SerializeObject(new
                {
                    ConsecutivoFactura = viewModel.NUM_DOCU,
                    Prefijo = viewModel.PREFIJO.Trim(),
                    TipoDocumento = viewModel.TIPO_DOCUMENTO
                });
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.Headers.Add("api_key", ConfiguracionesGlobales.ApiTokenFacturacion);
                request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var http = new HttpClient();
                var response = await http.SendAsync(request);

                byte[] bytes = await response.Content.ReadAsByteArrayAsync();
                if (bytes.Length > 0)
                {
                    var archivo = File(bytes, "application/zip", $"{viewModel.NUM_DOCU}_{viewModel.RMT_CONT}.zip");
                    return archivo;
                }
                else
                {
                    var result = new JsonResult
                    {
                        Data = new
                        {
                            Message = "No se encontró factura.",
                            ExitosoFactura = false
                        }
                    };
                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = new JsonResult
                {
                    Data = new
                    {
                        Message = "Error al consultar factura. Comuníquese con el administrador.",
                        ExitosoFactura = false
                    }
                };
                return result;
            }
        }
    }
}