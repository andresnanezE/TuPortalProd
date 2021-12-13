using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using NormalizacionDirecciones;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class GeoZonificadorController : Controller
    {
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private IServicioAplicacionProcesosAsesor _servicioAplicacionAfiliaciones;

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        private IServicioAplicacionProcesosAsesor ServicioAplicacionAfiliaciones
        {
            get { return _servicioAplicacionAfiliaciones ?? (_servicioAplicacionAfiliaciones = FabricaIoC.Resolver<IServicioAplicacionProcesosAsesor>()); }
        }

        // GET: GeoZonificador
        public ActionResult Index()
        {
            GeoZonificadorModel model = new GeoZonificadorModel();
            model.Ciudades = ConsultarCiudades();
            ViewBag.pintarTabla = false;
            ViewBag.EsError = false;
            //ActualizarLog();
            return View(model);
        }

        /// <summary>
        /// regresa valores quemados mientras se cambia el proveedor del servicio
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(GeoZonificadorModel model)
        {
            ViewBag.pintarTabla = false;
            ViewBag.EsError = false;
            model.Ciudades = ConsultarCiudades();
            EMB_DIRECCIONES objResultado = new EMB_DIRECCIONES();
            var normalizador = new Methods();

            objResultado = normalizador.ObtenerDireccionWS(model.Direccion, model.Ciudad, "0", "0", "Geozonificador");

            if (objResultado != null && objResultado.zona != "")
            {
                try
                {
                    model.ZonaOperacion = "";
                    if (objResultado.zona.ToUpper().Contains("PERIMETRO") || objResultado.zona.ToUpper().Contains("PERÍMETRO"))
                    {
                        model.Estado = "Dirección encontrada con cobertura";
                        model.ZonaOperacion = objResultado.zona.ToUpper().Replace("PERIMETRO", "").Replace("PERÍMETRO", "").Replace("_", " ").Trim();
                    }
                    else
                    {
                        model.Estado = "Dirección fuera de cobertura o no encontrada";
                    }
                    model.DireccionNormalizada = objResultado.dirtrad;
                    model.Barrio = objResultado.barrio;
                    model.Longitud = Convert.ToDouble(objResultado.longitude, CultureInfo.InvariantCulture);
                    model.Latitud = Convert.ToDouble(objResultado.latitude, CultureInfo.InvariantCulture);
                    ViewBag.pintarTabla = true;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    ViewBag.EsError = true;
                    ViewBag.msgError = "Se ha presentado un problema interno. Intente nuevamente.";
                }
            }
            else
            {
                ViewBag.EsError = true;
                ViewBag.msgError = "Se ha presentado un problema interno. Intente nuevamente.";
            }
            return View(model);
        }

        /// <summary>
        /// Retorna un listItem de Ciudades
        /// (por ahora esta quemado por que se esta cambiando el proveedor de los datos)
        /// </summary>
        /// <returns>Lista de cuidades en obj LisItem</returns>
        private List<ListItem> ConsultarCiudades()
        {
            var lst = ServicioAplicacionAfiliaciones.ObtenerCiudades();
            List<ListItem> lstCiudades = new List<ListItem>();

            foreach (var ciud in lst)
            {
                lstCiudades.Add(new ListItem { Text = ciud.CIUDAD, Value = ciud.CIUDAD });
            }

            var t = new List<ListItem>
                        {
                            new ListItem { Text = "Bogota", Value = "0" },
                        };

            return lstCiudades;
        }

        private void ActualizarLog(string DatosIngreso, string Respuesta, string Detalle)
        {
            //actualiza log actividades
            var identity = (ClaimsIdentity)User.Identity;
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
                    idTipoLog = (int)Consulta.GeoZonificador,
                    ip = ip,
                    MenuId = (int)Menus.GeoZonificador,
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
                var log = new EmbLogActividadesDto
                {
                    UsuarioId = usuarioId,
                    fecha = DateTime.Now,
                    idTipoLog = (int)Consulta.GeoZonificador,
                    ip = ip,
                    MenuId = (int)Menus.GeoZonificador,
                    DatosIngreso = DatosIngreso,
                    Respuesta = Respuesta,
                    Detalle = Detalle,
                };

                ServicioAplicacionLogs.AgregarLog(log);
            }
        }
    }
}