using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using MvcReportViewer;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class EstadoCuentaController : Controller
    {
        #region Field

        private IServicioAplicacionContratoUsuario _servicioAplicacionContratoUsuario;

        #endregion Field

        #region Instance Properties

        private IServicioAplicacionContratoUsuario ServicioAplicacionContratoUsuario
        {
            get { return _servicioAplicacionContratoUsuario ?? (_servicioAplicacionContratoUsuario = FabricaIoC.Resolver<IServicioAplicacionContratoUsuario>()); }
        }

        #endregion Instance Properties

        // GET: EstadoCuenta
        public ActionResult Index()
        {
            ReportesModel viewModel = new ReportesModel();
            viewModel.Contratos = new List<SelectListItem>();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ReporteEstadoCuenta(ReportesModel viewModel)
        {
            try
            {
                if (viewModel.NumeroDoc == "" | viewModel.NumeroDoc == null)
                {
                    ModelState.AddModelError(string.Empty, "Por favor digite el Nro. de documento");
                }
                else if (viewModel.ContratoId == "" | viewModel.ContratoId == null)
                {
                    ModelState.AddModelError(string.Empty, "Por favor seleccione el Contrato");
                }
                else
                {
                    var result = new List<EmbUsuarioContrato>();
                    result = ServicioAplicacionContratoUsuario.ObtenerContratos(viewModel.NumeroDoc).ToList();

                    if (result.Count() <= 0)
                    {
                        result.AddRange(ServicioAplicacionContratoUsuario.ObtenerContratosBeneficiario(viewModel.NumeroDoc).ToList());
                    }

                    var contratoIdInt = int.Parse(viewModel.ContratoId);

                    if (result.FirstOrDefault(n => n.RmtCont == contratoIdInt).TipoContrato == "PMP")
                    {
                        ModelState.AddModelError(string.Empty, "Apreciado afiliado, El tipo de contrato con el que te encuentras afiliado no permite generar este certificado. Por favor comunícate con nuestras líneas de atención del afiliado en Bogotá, Chía y Soacha (1) 3077089. O a la Línea Nacional gratuita 018000117098. E-mail: servicioalcliente@emermedica.com.co");
                    }
                }

                if (ModelState.IsValid)
                {
                    if (viewModel.NumeroDoc != "")
                    {
                        var parametros = new List<KeyValuePair<string, object>>();
                        parametros.Add(new KeyValuePair<string, object>("NumIdentificacion", viewModel.NumeroDoc));
                        parametros.Add(new KeyValuePair<string, object>("Contrato", viewModel.ContratoId));
                        return this.Report(
                            ReportFormat.PDF,
                            ConfiguracionesGlobales.ReportPathReporteEstadoCuenta,
                            ConfiguracionesGlobales.ReportesReportServerUrl,
                            parametros,
                            ConfiguracionesGlobales.ReportesUsername,
                            ConfiguracionesGlobales.ReportesPassword
                       );
                    }
                }
            }
            catch (Exception excepcion)
            {
                ModelState.AddModelError(string.Empty, excepcion.Message);
            }

            viewModel.Contratos = new List<SelectListItem>();
            return View("index", viewModel);
        }

        [HttpPost]
        public ActionResult ObtenerContratosNit(string numeroDoc)
        {
            var result = new List<EmbUsuarioContrato>();
            try
            {
                result = ServicioAplicacionContratoUsuario.ObtenerContratos(numeroDoc).ToList();

                if (result.Count() <= 0)
                {
                    result.AddRange(ServicioAplicacionContratoUsuario.ObtenerContratosBeneficiario(numeroDoc).ToList());
                }
            }
#pragma warning disable CS0168 // The variable 'excepcion' is declared but never used
            catch (SystemException excepcion)
#pragma warning restore CS0168 // The variable 'excepcion' is declared but never used
            {
                //result = excepcion.Message;
            }
            return Json(result);
        }
    }
}