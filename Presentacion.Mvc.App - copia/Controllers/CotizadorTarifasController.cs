using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Presentacion.Mvc.App.Models;
using System;
using System.Security.Claims;
using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class CotizadorTarifasController : Controller
    {

        private String urlWs = ConfiguracionesGlobales.EmermedicaWsUrl;
        private String usrWs = ConfiguracionesGlobales.EmermedicaWsUsr;
        private String pwdWs = ConfiguracionesGlobales.EmermedicaWsPwd;

        private IServicioAplicacionCotizadorTarifas _servicioAplicacionCotizadorTarifas;
        private IServicioAplicacionLogs _servicioAplicacionLogs;


        private IServicioAplicacionCotizadorTarifas ServicioAplicacionCotizadorTarifas
        {
            get { return _servicioAplicacionCotizadorTarifas ?? (_servicioAplicacionCotizadorTarifas = FabricaIoC.Resolver<IServicioAplicacionCotizadorTarifas>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ViewData["CurrentUser"] = requestContext.HttpContext.User.Identity.Name;
            }
        }


        // GET: CotizadorTarifas
        public ActionResult Index()
        {
            var model = new CotizadorTarifasModel();
            String sUsuario = ViewData["CurrentUser"].ToString();

            var oUsuarioCiudad = ServicioAplicacionCotizadorTarifas.getUsuarioCiudad(sUsuario, urlWs, usrWs, pwdWs);
            model.UsuarioCiudad = oUsuarioCiudad;
            model.TiposTarifas = ServicioAplicacionCotizadorTarifas.getTiposTarifas(urlWs, usrWs, pwdWs);

            foreach (var tipoTarifa in model.TiposTarifas)
            {
                tipoTarifa.CAMPANAS = ServicioAplicacionCotizadorTarifas.getCampanas(tipoTarifa.CODIGO_TARIFA, urlWs, usrWs, pwdWs);
            }
            model.Serializador = new JavaScriptSerializer();
            
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
                    idTipoLog = (int)Consulta.CotizadorFamiliares,
                    ip = ip,
                    MenuId = (int)Menus.Familiares,
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
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.CotizadorFamiliares;
                log.ip = ip;
                log.MenuId = (int)Menus.Familiares;

                ServicioAplicacionLogs.AgregarLog(log);
            }
            //

            return View(model);
        }

        // GET: CotizadorTarifas/Json
        public String Json(String sCiudad, int iPersonas, String sTipoTarifa, String sCampana)
        {
            var model = new CotizadorTarifasModel();
            var seriarlizer = new JavaScriptSerializer();
            String sUsuario = ViewData["CurrentUser"].ToString();

            var ieTarifas = ServicioAplicacionCotizadorTarifas.getTarifasBase(sCiudad, iPersonas, sTipoTarifa, sCampana, sUsuario, urlWs, usrWs, pwdWs);
            var serializado = seriarlizer.Serialize(ieTarifas);
            return serializado;
        }

    }

}