using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using ExcelDataReader;
using MvcReportViewer;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class CertificadoTributarioController : Controller
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

        // GET: CertificadoTributario
        public ActionResult Index()
        {
            ReportesModel viewModel = new ReportesModel();
            viewModel.Contratos = new List<SelectListItem>();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ReporteCertificadoTributario(ReportesModel viewModel)
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

                var tieneFacturacion = TieneFacturacionAnioAnterior(viewModel.ContratoId);
                if (!tieneFacturacion)
                {
                    ModelState.AddModelError(string.Empty, "Apreciado afiliado, La fecha de tu contrato aún no permite generar este certificado, ya que nuestro sistema no registra pagos del año inmediatamente anterior. Por favor comunícate con nuestras líneas de atención del afiliado en Bogotá, Chía y Soacha(1) 3077089.O a la Línea Nacional gratuita 018000117098.E - mail: servicioalcliente@emermedica.com.co");
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
                        parametros.Add(new KeyValuePair<string, object>("anio", ""));
                        return this.Report(
                            ReportFormat.PDF,
                            ConfiguracionesGlobales.ReportPathCertificadoTributario,
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
            catch (Exception ex)
            {
                //result = excepcion.Message;
            }
            return Json(result);
        }

        public bool TieneFacturacionAnioAnterior(string rmtCont)
        {
            try
            {
                var resultado = ServicioAplicacionContratoUsuario.TieneFacturacionAnioAnterior(rmtCont);
                return resultado;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ActionResult Parametrizacion()
        {
            var model = new EnvioCertificadoTributarioModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult GetRmtCont(string blobCsv)
        {
            try
            {
                blobCsv = blobCsv.Replace("data:application/vnd.openxmlformats-officedocument." +
                    "spreadsheetml.sheet;base64,", String.Empty);
                byte[] cvsBytes = Convert.FromBase64String(blobCsv);

                var url = Path.GetTempPath() + "RmtContCertificado_tmp.xlsx";
                System.IO.File.WriteAllBytes(url, cvsBytes);

                var listaRmtCont = new List<RmtContSolicitudCertificado>();

                using (var stream = System.IO.File.Open(url, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            while (reader.Read())
                            {
                                listaRmtCont.Add(new RmtContSolicitudCertificado
                                {
                                    NumeroDocumento = reader.GetValue(0).ToString(),
                                    RmtCont = reader.GetValue(1).ToString()
                                });
                            }
                        } while (reader.NextResult());
                    }
                }
                listaRmtCont.RemoveAt(0);
                return new JsonResult { Data = new { List = listaRmtCont, Message = "" } };
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new
                    {
                        Message = "Error al obtener RMT-CONT del archivo subido. Verifique " +
                    "la escritura del archivo"
                    }
                };
            }
        }

        [HttpPost]
        public ActionResult SolicitarCertificados(EnvioCertificadoTributarioModel model)
        {
            if (model.FechaMaximaEnvio.Year == 1)
            {
                return new JsonResult
                {
                    Data = new
                    {
                        Exitoso = false,
                        Mensaje = "Seleccione una fecha válida"
                    }
                };
            }
            var resultado = ServicioAplicacionContratoUsuario.SolicitarEnvioCertificadoTributario(Convert.ToInt32(model.Año),
                model.TipoContrato,
                model.FechaMaximaEnvio,
                model.TipoEnvio,
                model.RmtContRecurrentes);
            return new JsonResult
            {
                Data = new
                {
                    Exitoso = resultado,
                    Mensaje = resultado ?
                            "La solicitud de generación de certificado ha sido registrada" :
                            "Error al generar la solicitud de certificado"
                }
            };
        }

        public ActionResult Reporte()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerReporteEnvios()
        {

            try
            {
                var result = ServicioAplicacionContratoUsuario.ConsultaReporteEnvios().ToList();
                return new JsonResult { Data = new { Exitoso = true, Result = result } };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { Exitoso = false } };
            }
        }
    }
}