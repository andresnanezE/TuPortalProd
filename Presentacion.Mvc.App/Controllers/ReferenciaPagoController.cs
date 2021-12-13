using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Implementacion;
using Dominio.Administracion.Entidades;
using Dominio.Administracion.Entidades.MapperDto;
using Newtonsoft.Json;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{

    public class ReferenciaPagoController : Controller
    {
        private IServicioCotizacion _servicioCotizacion;
        private IServicioCotizacion ServicioCotizacion
        {
            get { return _servicioCotizacion ?? (_servicioCotizacion = FabricaIoC.Resolver<IServicioCotizacion>()); }
        }
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<ActionResult> GenerarReferencia(GenerarReferenciaDto model)
        {
            var resultadoReferencia = await ServicioCotizacion.GetReferenciaPago(model);
            var result = new JsonResult();
            result.Data = resultadoReferencia;
            return result;
        }
        [HttpPost]
        public async Task<ActionResult> PaymentData(string referencia)
        {
            var resultado = new JsonResult();
            var resultadoPaymentData = await ServicioCotizacion.GetPaymentData(referencia);
            resultado.Data = resultadoPaymentData;
            return resultado;
        }
    }
}