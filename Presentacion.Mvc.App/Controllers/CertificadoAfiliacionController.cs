using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
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
    public class CertificadoAfiliacionController : Controller
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

        public ActionResult Index()
        {
            try
            {
                var viewModel = new ReportesModel
                {
                    NumeroDoc = "",
                    ContratoId = "",
                    cod_bene = "",
                    Contratos = new List<SelectListItem>(),
                    BeneficiariosList = new List<SelectListItem>()
                };
                return View(viewModel);
            }
            catch (Exception e)
            {
                var viewModel = new ReportesModel
                {
                    NumeroDoc = "",
                    ContratoId = "",
                    cod_bene = "",
                    Contratos = new List<SelectListItem>(),
                    BeneficiariosList = new List<SelectListItem>()
                };
                ModelState.AddModelError(string.Empty, e.Message);
                return View(viewModel);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> ObtenerContratos(string NumDoc)
        {
            var contratos = ServicioAplicacionContratoUsuario.ObtenerContratos(NumDoc);

            if (contratos.Count() <= 0)
            {
                contratos = ServicioAplicacionContratoUsuario.ObtenerContratosBeneficiario(NumDoc).ToList();
            }

            var list = contratos.Select(d => new SelectListItem
            {
                Text = d.CONTRATOCOMBO,
                Value = d.RmtCont.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            return list;
        }

        [NonAction]
        public IEnumerable<SelectListItem> ObtenerBeneficiarios(string rmtCont, string NumDoc)
        {
            var beneficiarios = ServicioAplicacionContratoUsuario.ObtenerBeneficiariosCartaViajero(rmtCont, NumDoc);
            var list = beneficiarios.Select(d => new SelectListItem
            {
                Text = d.Cod_Bene + " - " + d.Nom_Bene + " " + d.Ape_Bene,
                Value = d.Cod_Bene.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Ninguno",
                Value = "-1",
                Selected = true
            });

            return list;
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
                    result = ServicioAplicacionContratoUsuario.ObtenerContratosBeneficiario(numeroDoc).ToList();
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

        [HttpPost]
        public ActionResult ObtenerBeneficiariosList(string rmtCont, string NumDoc)
        {
            var result = new List<SelectListItem>();
            try
            {
                result = ObtenerBeneficiarios(rmtCont, NumDoc).ToList();
            }
#pragma warning disable CS0168 // The variable 'excepcion' is declared but never used
            catch (SystemException excepcion)
#pragma warning restore CS0168 // The variable 'excepcion' is declared but never used
            {
                //result = excepcion.Message;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult ReporteCertificadoAfiliacion(ReportesModel viewModel)
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

                if (ModelState.IsValid)
                {
                    if (viewModel.NumeroDoc != "" && viewModel.cod_bene == null)
                    {
                        var parametros = new List<KeyValuePair<string, object>>();
                        var condicion = "";
                        var documento = "";
                        if (viewModel.BeneficiarioSeleccionados.FirstOrDefault() == "-1")
                        {
                            documento = viewModel.NumeroDoc;
                            condicion = "CONTRATANTE";
                        }
                        else
                        {
                            documento = viewModel.BeneficiarioSeleccionados.FirstOrDefault();
                            condicion = "BENEFICIARIO";
                        }
                        parametros.Add(new KeyValuePair<string, object>("NumIdentificacion", documento));
                        parametros.Add(new KeyValuePair<string, object>("Contrato", viewModel.ContratoId));
                        parametros.Add(new KeyValuePair<string, object>("Condicion", condicion));
                        return this.Report(
                            ReportFormat.PDF,
                            ConfiguracionesGlobales.ReportPathReporteCertificadoAfiliacion,
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
            viewModel.BeneficiariosList = new List<SelectListItem>();
            return View("index", viewModel);
        }
    }
}