using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.MapperDto;
using Presentacion.Mvc.App.Models;
using System;
using System.Security.Claims;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class GanometroController : Controller
    {
        private static IServicioAplicacionGanometro _servicioAplicacionGanometro;

        private static IServicioAplicacionGanometro ServicioAplicacionGanometro
        {
            get
            {
                return _servicioAplicacionGanometro ??
                       (_servicioAplicacionGanometro =
                           FabricaIoC.Resolver<IServicioAplicacionGanometro>());
            }
        }

        [HttpGet]
        public ActionResult RegistroVentas()
        {
            var model = new GanometroModel();
            model.FechaVenta = DateTime.Now;
            model = CargarModelo(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult RegistroVentas(GanometroModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserClaims.GetCurrentUserClaims((ClaimsIdentity)HttpContext.User.Identity);
                var rolComercial = UserClaims.EsRolDirector(user.Rol) ? Int32.Parse(user.Rol) : 0;
                int idUsuario;
                int.TryParse(user.UserId, out idUsuario);

                var periodo = ServicioAplicacionGanometro.ObtenerPeriodo(model.FechaVenta);
                var anio = periodo.Anio;
                var mesPeriodo = periodo.Periodo;
#pragma warning disable CS0219 // The variable 'idCiudad' is assigned but its value is never used
                int idCiudad = 100142;
#pragma warning restore CS0219 // The variable 'idCiudad' is assigned but its value is never used

                var p = ServicioAplicacionGanometro.ObtenerPeriodo(model.FechaVenta);

                var v = new VentasGanometroDto
                {
                    Anio = p.Anio,
                    Periodo = p.Periodo,
                    Semana = p.Semana,
                    Dia = model.Dia,
                    CantidadVentas = model.CantidadVentas,
                    FechaVenta = model.FechaVenta,
                    IdCiudadHomologada = model.IdCiudadHomologada,
                    IdDirector = model.IdDirector,
                    EsHabil = p.EsHabil,
                    FechaIngreso = DateTime.Now
                };
                ServicioAplicacionGanometro.InsertarVenta(v);
                model = CargarModelo(model);
            }
            return View(model);
        }

        private GanometroModel CargarModelo(GanometroModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserClaims.GetCurrentUserClaims((ClaimsIdentity)HttpContext.User.Identity);
                var rolComercial = UserClaims.EsRolDirector(user.Rol) ? Int32.Parse(user.Rol) : 0;

                int idUsuario;
                int.TryParse(user.UserId, out idUsuario);

                var periodo = ServicioAplicacionGanometro.ObtenerPeriodo(model.FechaVenta);
                var anio = periodo.Anio;
                var mesPeriodo = periodo.Periodo;
#pragma warning disable CS0219 // The variable 'idCiudad' is assigned but its value is never used
                int idCiudad = 100142;
#pragma warning restore CS0219 // The variable 'idCiudad' is assigned but its value is never used

                /*
                var metas = ServicioAplicacionGanometro.ObtenerMetas(periodo, idCiudad);

                var t = ServicioAplicacionGanometro.ObtenerVentasXperiodoXsemanaXdia(idUsuario, anio, mesPeriodo, idCiudad);

                var ventasActuales = t.Select(x => (
                    new VentasDirectorPeriodoDto
                    {
                        SEMANA = x.SEMANA,
                        DIA1 = x.DIA1,
                        DIA2 = x.DIA2,
                        DIA3 = x.DIA3,
                        DIA4 = x.DIA4,
                        DIA5 = x.DIA5,
                        DIA6 = x.DIA6,
                        DIA7 = x.DIA7,
                        FDIA1 = x.FDIA1,
                        FDIA2 = x.FDIA2,
                        FDIA3 = x.FDIA3,
                        FDIA4 = x.FDIA4,
                        FDIA5 = x.FDIA5,
                        FDIA6 = x.FDIA6,
                        FDIA7 = x.FDIA7
                    })

                );
                var metasConVentas = new List<VentasDirectorPeriodoDto>();
                foreach (var m in metas)
                {
                    var meta = new VentasDirectorPeriodoDto
                    {
                        SEMANA = m.Semana,
                    };
                    metasConVentas.Add(
                        meta
                       );
                }*/

                model.IdDirector = idUsuario;
            }
            return model;
        }

        [HttpGet]
        public ActionResult HistoricoVentas()
        {
            var model = new GanometroModel();
            model.FechaVenta = DateTime.Now;
            model = CargarModelo(model);

            return View(model);
        }
    }
}