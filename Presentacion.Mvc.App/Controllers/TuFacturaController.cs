using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades;
using MvcReportViewer;
using Newtonsoft.Json;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class TuFacturaController : Controller
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

        //Metodo Factura Dispapeles



        [HttpPost]
        public async Task<ActionResult> ReporteFactura(FacturaAnexosViewModel viewModel)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfiguracionesGlobales.ApiUrlFacturacion);
                string json = JsonConvert.SerializeObject(new
                {
                    ConsecutivoFactura = viewModel.NUM_DOCU,
                    Prefijo = viewModel.PREFIJO,
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

        // GET: TuFactura
        public ActionResult Index()
        {
            var viewModel = new FacturaAnexosViewModel();

            viewModel.Contratos = new List<SelectListItem>();
            viewModel.ContratosFacturas = new List<ContratosFacturasViewModel>();
            return View(viewModel);
        }
         
        [NonAction]
        public IEnumerable<SelectListItem> ObtenerContratos(string NumDoc)
        {
            var contratos = ServicioAplicacionContratoUsuario.ObtenerContratos(NumDoc);
            var list = contratos.Select(d => new SelectListItem
            {
                Text = d.CONTRATOCOMBO,
                Value = d.RmtCont.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            return list;
        }

        [HttpPost]
        public ActionResult Index(FacturaAnexosViewModel viewModel)
        {
            var contratosFacturas = ServicioAplicacionContratoUsuario.ObtenerContratosFacturasUsuarios(viewModel.NumeroDocumento, viewModel.ContratoId).ToList();

            // se toman las facturas de los ultimos tres periodos
            var result = contratosFacturas.Select(n =>
            new ContratosFacturasViewModel
            {
                FECHA_INICIAL = n.FECHA_INICIAL,
                FECHA_FINAL = n.FECHA_FINAL,
                PERIODO_FACTURA = n.PERIODO_FACTURA,
                NUM_DOCU = n.NUM_DOCU,
                VAL_TOTA = n.VAL_TOTA,
                VAL_SALD = n.VAL_SALD,
                RMT_CAFA = n.RMT_CAFA,
                COD_DOCU = n.COD_DOCU, 
                PREFIJO = n.PREFIJO,
                TIPO_DOCUMENTO = n.TipoDocumento

            })
            .OrderByDescending(n => n.FECHA_FINAL)
            //.Take(1)
            .ToList();

            var facturas = new List<ContratosFacturasViewModel>();
            var factura = new ContratosFacturasViewModel();
            if (!result.Any())
            {
                TempData["mostrarModal"] = true;
                ModelState.AddModelError(string.Empty, "El usuario no tiene facturas asignadas.");
            }
            else
            {
                factura.VAL_TOTA = result.Sum(n => n.VAL_TOTA);
                factura.VAL_SALD = result.Sum(n => n.VAL_SALD);
                factura.FECHA_INICIAL = result.FirstOrDefault().FECHA_INICIAL;
                factura.FECHA_FINAL = result.FirstOrDefault().FECHA_FINAL;
                factura.PERIODO_FACTURA = result.FirstOrDefault().PERIODO_FACTURA;
                factura.RMT_CAFA = result.FirstOrDefault().RMT_CAFA;
                factura.COD_DOCU = result.FirstOrDefault().COD_DOCU;
                factura.NUM_DOCU = result.FirstOrDefault().NUM_DOCU;
                factura.PREFIJO = result.FirstOrDefault().PREFIJO;
                factura.TIPO_DOCUMENTO = result.FirstOrDefault().TIPO_DOCUMENTO;
                facturas.Add(factura);
            }

            var contratos = ObtenerContratos(viewModel.NumeroDocumento);

            viewModel = new FacturaAnexosViewModel
            {
                Contratos = contratos,
                ContratosFacturas = facturas
            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult ObtenerContratosNit(string numeroDoc)
        {
            var result = new List<EmbUsuarioContrato>();
            try
            {
                result = ServicioAplicacionContratoUsuario.ObtenerContratos(numeroDoc).Where(n => n.TipoContrato == "FAM").ToList();

                if (result.Count() <= 0)
                {
                    result.AddRange(ServicioAplicacionContratoUsuario.ObtenerContratosBeneficiario(numeroDoc).Where(n => n.TipoContrato == "FAM").ToList());
                }
            }
            catch (SystemException ex)
            {
                throw new Exception(ex.ToString());
            }
            return Json(result);
        }
    }
}