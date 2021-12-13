using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.ComponentModel;
using System.Security.Claims;
using System.Collections.Generic;
using Presentacion.Mvc.App.Models;
using System.Web.Script.Serialization;
using Transversales.Administracion.IoC;
using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Enumeraciones;
using Dominio.Administracion.Entidades.ModeloCotizacion;

namespace Presentacion.Mvc.App.Controllers
{
    public class MisReservasController : Controller
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
            return View();
        }

        [HttpGet]
        public String ObtenerMisCotizaciones(string estadoCotizacion, string producto, string estadoReserva, string fechaInicio, string fechaFin)
        {
            ObtenerInfMisCotizaciones objRetornar = new ObtenerInfMisCotizaciones();
            List<string> lstMsg = new List<string>();
            objRetornar.LstProximasVencer = new List<string>();

            UserClaims.ResetUserClaimsSession();
            try
            {
                var rolDescripcion = string.Empty;
                var identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var roles = identity.FindFirst(ClaimTypes.Role).Value;
                var lstRoles = roles.Split(',');
                objRetornar.LstRoles = lstRoles.Where(l => l != "").ToList();

                var rolId = objRetornar.LstRoles.Select(x => x).FirstOrDefault();

                if(objRetornar.LstRoles.Count() == 2 && objRetornar.LstRoles.Any(x => x == "28"))
                    rolDescripcion = GetEnumDescription((RolPortal)int.Parse("28"));
                else
                    rolDescripcion = GetEnumDescription((RolPortal)int.Parse(rolId));

                if (objRetornar.LstRoles.Any(x => x == "8") && objRetornar.LstRoles.Count() == 1)
                {
                    objRetornar.LstMisCotizacionesRol = ServicioCotizacion.ObtenerJerarquiaCotizaciones(userId.ToString(), estadoCotizacion, producto, estadoReserva, fechaInicio, fechaFin, rolDescripcion).ToList();
                }
                else
                {
                    var noDocumentoDirector = ServicioCotizacion.ObtenerNoDocumentoDirector(userId);
                    objRetornar.LstMisCotizacionesRol = ServicioCotizacion.ObtenerJerarquiaCotizaciones(noDocumentoDirector.ToString(), estadoCotizacion, producto, estadoReserva, fechaInicio, fechaFin, rolDescripcion).ToList();
                    
                }
            }
            catch (Exception ex)
            {
                objRetornar.Respuesta = false;
                objRetornar.Mensaje = ex.Message;//"Ha ocurrido algo inesperado con la solicitud. Intente de nuevo si el error persiste contacte al administrador.";
            }

            return JsonConvert.SerializeObject(objRetornar);
        }

        [HttpGet]
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
    }
}