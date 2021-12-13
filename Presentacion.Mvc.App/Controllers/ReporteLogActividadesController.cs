using Aplicacion.Administracion.Contratos;
using MvcReportViewer;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReporteLogActividadesController : Controller
    {
        #region Field

        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion Field

        #region Instance Properties

        private IServicioAplicacionRoles ServicioAplicacionRoles
        {
            get { return _servicioAplicacionRoles ?? (_servicioAplicacionRoles = FabricaIoC.Resolver<IServicioAplicacionRoles>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        #endregion Instance Properties

        // GET: ReporteLogActividades
        public ActionResult Index()
        {
            var viewModel = new LogActividadesViewModel();

            viewModel.Tipos = ObtenerTipos();
            viewModel.Roles = ObtenerRoles();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(LogActividadesViewModel viewModel)
        {
            ViewBag.mensaje = "";
            try
            {
                viewModel.Tipos = ObtenerTipos();
                viewModel.Roles = ObtenerRoles();

                if (ModelState.IsValid)
                {
                    var parametros = new List<KeyValuePair<string, object>>();
                    parametros.Add(new KeyValuePair<string, object>("FechaIni", (viewModel.FechaInicial)));
                    parametros.Add(new KeyValuePair<string, object>("FechaFin", (viewModel.FechaFinal)));
                    parametros.Add(new KeyValuePair<string, object>("TIPO", viewModel.IdTipo));
                    parametros.Add(new KeyValuePair<string, object>("ROL", viewModel.IdRol));

                    FileStreamResult reporte = null;
                    reporte = this.Report(
                                    ReportFormat.Excel,
                        ConfiguracionesGlobales.ReporteLogActividades,
                        ConfiguracionesGlobales.ReportesReportServerUrl,
                        parametros,
                        ConfiguracionesGlobales.ReportesUsername,
                        ConfiguracionesGlobales.ReportesPassword
                        );

                    reporte.FileDownloadName = "ReporteLogActividades.xls";
                    return reporte;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error con los filtros seleccionados, por favor validelos");
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                ModelState.AddModelError(string.Empty, "Error al exportar el informe, comuníquese con el administrador.");
            }

            return View(viewModel);
        }

        [NonAction]
        public IEnumerable<SelectListItem> ObtenerTipos()
        {
            var tiposLog = ServicioAplicacionLogs.ObtenerTiposLog().ToList();
            var list = tiposLog.Select(d => new
            {
                Text = d.tipo,
                Value = d.tipo.ToString(CultureInfo.InvariantCulture)
            })
            .GroupBy(n => n.Text)
            .ToList()
            .Select(d => new SelectListItem
            {
                Text = d.Key,
                Value = d.Key
            })
            .OrderBy(n => n.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Ninguno",
                Value = "-1",
                Selected = true
            });

            return list;
        }

        [NonAction]
        public IEnumerable<SelectListItem> ObtenerRoles()
        {
            var contratos = ServicioAplicacionRoles.ObtenerRoles().ToList();

            var list = contratos.Select(d => new SelectListItem
            {
                Text = d.ROL,
                Value = d.ROLID.ToString(CultureInfo.InvariantCulture)
            })
            .OrderBy(n => n.Text)
            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Ninguno",
                Value = "-1",
                Selected = true
            });

            return list;
        }
    }
}