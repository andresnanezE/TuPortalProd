using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Implementacion;
using Dominio.Administracion.Entidades.Enumeraciones;
using Dominio.Administracion.Entidades.ReportesDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReportesController : Controller
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
        public ActionResult Index()
        {
            if (RolesUsuario().Contains("7"))
                return View();
            else
                return RedirectToHome();
        }
        public ActionResult ReporteAsesor()
        {
            if (RolesUsuario().Contains("8"))
                return View();
            else
                return RedirectToHome();
        }
        public ActionResult ReportePricing()
        {
            if (RolesUsuario().Contains("28"))
                return View();
            else
                return RedirectToHome();
        }
        public ActionResult ReporteGerenteNacional()
        {
            if (RolesUsuario().Contains("30"))
                return View();
            else
                return RedirectToHome();
        }
        public ActionResult ReporteGerenteRegional()
        {
            if (RolesUsuario().Contains("29"))
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                ViewBag.Ciudad = ServicioCotizacion.ObtenerCiudadAsesor(userId);
                return View();
            }
            else
                return RedirectToHome();
           
        }


        [System.Web.Mvc.HttpGet]
        public String ObtenerReporteGlobal()
        {
            var dataReporteCotizacionGlobal = ServicioCotizacion.ObtenerReporteGlobal();
            return JsonConvert.SerializeObject(dataReporteCotizacionGlobal);
        }
        [HttpPost]
        public string GetData(FiltrosReporte filtros)
        {

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var roles = identity.FindFirst(ClaimTypes.Role).Value;
                var lstRoles = roles.Split(',');
                var LstRoles = lstRoles.Where(l => l != "").ToList();

                if ((LstRoles.Any(x => x == "8") && LstRoles.Count() == 1) || filtros.DescripcionRol.Equals("Asesor"))
                {
                    var dataReporte = ServicioCotizacion.DataReporte(filtros, userId.ToString(), filtros.DescripcionRol);
                    return JsonConvert.SerializeObject(dataReporte);
                }
                else
                {
                    var noDocumentoDirector = ServicioCotizacion.ObtenerNoDocumentoDirector(userId);
                    var dataReporte = ServicioCotizacion.DataReporte(filtros, noDocumentoDirector.ToString(), filtros.DescripcionRol);
                    return JsonConvert.SerializeObject(dataReporte);
                }
            }
            catch (Exception e)
            {

                throw e;
            }


        }
        [System.Web.Mvc.HttpGet]
        public String ObtenerListadosMultiselect(string nombreListado)
        {
            var seriarlizer = new JavaScriptSerializer();
            var identity = (ClaimsIdentity)User.Identity;
            var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

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

                case "Ciudades":
                    var ciudades = ServicioCotizacion.ObtenerCiudades();
                    var serializadoCiudades = seriarlizer.Serialize(ciudades);
                    return serializadoCiudades;

                case "Canales":
                    var canales = ServicioCotizacion.ObtenerCanales();
                    var serializadoCanales = seriarlizer.Serialize(canales);
                    return serializadoCanales;

                case "SectorEconomico":
                    var sectores = ServicioCotizacion.ObtenerSector();
                    var serializadoSector = seriarlizer.Serialize(sectores);
                    return serializadoSector;

                case "Directores":
                    var directores = ServicioCotizacion.ObtenerDirector();
                    var serializadoDirector = seriarlizer.Serialize(directores);
                    return serializadoDirector;
                case "Asesores":
                    var noDocumentoDirector = ServicioCotizacion.ObtenerNoDocumentoDirector(userId);
                    var directoresId = new List<decimal>() { noDocumentoDirector };
                    var asesores = ServicioCotizacion.ObtenerAsesoresXDirectores(directoresId);
                    return seriarlizer.Serialize(asesores);
                case "DirectoresRegion":
                    var ciudadUsuar = ServicioCotizacion.ObtenerCiudadAsesor(userId);
                    var directoresRegion = ServicioCotizacion.ObtenerDirector(ciudadUsuar);
                    return seriarlizer.Serialize(directoresRegion);
                default:
                    break;
            }

            return "";
        }

        private static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        private ActionResult RedirectToHome()
        {
            return View("../Home/Index");
        }
        private List<string> RolesUsuario()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.FindFirst(ClaimTypes.Role).Value;
            var lstRoles = roles.Split(',');
            return lstRoles.Where(l => l != "").ToList();
        }
    }
}