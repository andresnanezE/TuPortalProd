using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.MapperDto.DtoSitioWeb;
using Dominio.Administracion.Entidades.MapperDto.DtoStone.DtoSitioWeb;
using Dominio.Administracion.Entidades.ModeloMedioPago;
using Newtonsoft.Json;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class MedioPagoController : Controller
    {
        private IServicioAplicacionMedioPago _servicioAplicacionMedioPago;
        private IServicioAplicacionContratos _servicioAplicacionContratos;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        private IServicioAplicacionMedioPago ServicioAplicacionMedioPago
        {
            get { return _servicioAplicacionMedioPago ?? (_servicioAplicacionMedioPago = FabricaIoC.Resolver<IServicioAplicacionMedioPago>()); }
        }

        private IServicioAplicacionContratos ServicioAplicacionContratos
        {
            get { return _servicioAplicacionContratos ?? (_servicioAplicacionContratos = FabricaIoC.Resolver<IServicioAplicacionContratos>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        public ActionResult Index()
        {
            ViewBag.busquedaPorTexto = false;
            return View();
        }

        public ActionResult Response()
        {
            return View();
        }        

        public async Task<ActionResult> GetBancos()
        {
            return Json(new
            {
                res = await ServicioAplicacionMedioPago.GetBancosAsyc()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string ObtenerPlanesVigentes(string filtro)
        {            
            //Obtener datos usuario
            var lstAfiliacion = new List<SPEM_CONSULTACONTRATOSDto>();
            var user = UserClaims.GetCurrentUserClaims((ClaimsIdentity)HttpContext.User.Identity);
            var rolComercial = UserClaims.EsRolAsesor(user.Rol) ? Int32.Parse(user.Rol) : 0;
            ViewBag.busquedaPorTexto = false;

            try
            {
                decimal contrato;
                if (decimal.TryParse(filtro, out contrato))
                {
                    lstAfiliacion = ServicioAplicacionContratos.ObtenerConsultaContratos(contrato, decimal.Parse(user.Document), rolComercial).ToList();
                }
                else
                {
                    ViewBag.busquedaPorTexto = true;
                    lstAfiliacion = ServicioAplicacionContratos.ObtenerConsultaContratosPorNombre(filtro, decimal.Parse(user.Document), rolComercial).ToList();
                }
                //lstAfiliacion = ServicioAplicacionMedioPago.ObtenerMedioPagoActualPorUsuario(numeroIdentidad).ToList();   
                //lstAfiliacion = ServicioAplicacionContratos.ObtenerConsultaContratos(numeroIdentidad).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

            return JsonConvert.SerializeObject(lstAfiliacion);
        }

        [HttpGet]
        public string DetalleContrato(decimal? busqueda, int rmt, decimal contratante, string tipoContrato, string tipoBusqueda, string nombreContratante, string numCont, string prefijo = "", string mensaje = "")
        {
            var model = new ContratoMedioPagoModel();

            model.NumCont = string.Format("{0}-{1}", prefijo, numCont);
            model.viewBene = false;
            model.viewPMP = false;
            model.viewAP = false;
            var codigo = busqueda ?? 0;
            model.Busqueda = codigo;
            if (tipoContrato == null || tipoContrato == "null")
            {
                model.Titulo = "Contrato";
                model.Mensaje = "Bienvenido";                
                model.FechaFin = DateTime.Now.AddDays(-5);
                model.Rmt_Cont = rmt.ToString();
                model.Identificacion = contratante.ToString();

                return JsonConvert.SerializeObject(model);
            }
            if (numCont != null) if (busqueda != null) codigo = numCont.Contains(busqueda.ToString()) ? rmt : busqueda.Value;
            var contratoPorRmt = new SPEM_CONSULTACONTRATOSDto();

            #region RMT

            if (tipoBusqueda.ToLower() == "rmt")
            {
                contratoPorRmt = ServicioAplicacionContratos.ObtenerContratos(codigo).FirstOrDefault(x => x.RmtCont == rmt);
                model.Identificacion = contratante.ToString("0");
                model.Rmt_Cont = rmt.ToString("0");
                model.TipoContrato = tipoContrato;
                model.Contratante = contratoPorRmt.Nombre;
                model.Telefono = contratoPorRmt.Telefono;
                model.Estado = contratoPorRmt.Estado;
                model.FechaInicio = contratoPorRmt.FechaI;
                model.FechaFin = contratoPorRmt.FechaV;
                model.Asesor = contratoPorRmt.nom_comp;
                model.Cantidad = contratoPorRmt.num_pers.ToString("0");
                model.IdentificacionAse = contratoPorRmt.cod_ases.ToString("C0");
                model.TelefonoAse = contratoPorRmt.TelefonoAse; model.EstadoAse = contratoPorRmt.EstadoAse;
                model.ModoPago = contratoPorRmt.ModoPago;
                model.FormaPago = contratoPorRmt.FormaPago;
                model.ValorContrato = contratoPorRmt.ValorContrato.ToString("C0");
                model.ValorCartera = contratoPorRmt.Cartera != null ? contratoPorRmt.Cartera.Value.ToString("C0") : "0";
                model.CuotaMensual = contratoPorRmt.CuotaMensual.ToString("C0");                
            }

            #endregion RMT

            #region BENEFICIARIO

            if (tipoBusqueda.ToLower() == "cc")
            {
                contratoPorRmt = ServicioAplicacionContratos.ObtenerContratosBeneficiario(codigo, rmt, contratante, nombreContratante.TrimEnd(' '))
                    .FirstOrDefault();
                var bene = ServicioAplicacionContratos.ObtenerBeneficiario(codigo.ToString()).FirstOrDefault();
                model.Titulo = "Beneficiario";
                model.Identificacion = contratante.ToString("0");
                model.Rmt_Cont = rmt.ToString("0");
                model.TipoContrato = tipoContrato;
                model.Contratante = contratoPorRmt.Nombre;
                model.Telefono = contratoPorRmt.Telefono;
                model.Estado = contratoPorRmt.Estado;
                model.FechaInicio = contratoPorRmt.FechaI.Date;
                model.FechaFin = contratoPorRmt.FechaV.Date;
                model.Asesor = contratoPorRmt.nom_comp;
                model.Cantidad = contratoPorRmt.num_pers.ToString("0");
                model.IdentificacionAse = contratoPorRmt.cod_ases.ToString("C0");
                model.TelefonoAse = contratoPorRmt.TelefonoAse;
                model.EstadoAse = contratoPorRmt.EstadoAse;
                model.ModoPago = contratoPorRmt.ModoPago;
                model.FormaPago = contratoPorRmt.FormaPago;
                model.ValorContrato = contratoPorRmt.ValorContrato.ToString("C0");
                model.ValorCartera = contratoPorRmt.Cartera != null ? contratoPorRmt.Cartera.Value.ToString("C0") : "0";
                model.CuotaMensual = contratoPorRmt.CuotaMensual.ToString("C0");
                model.Num_idenBen = bene.Num_iden.ToString("0");
                model.Nom_bene = bene.Nom_bene + " " + bene.Ape_bene;
                model.Fec_naciBen = bene.Fec_naci.Date;
                model.Tel_bene = bene.Tel_bene;
                model.viewBene = true;
                
            }

            #endregion BENEFICIARIO

            #region Contratante

            if (tipoBusqueda.ToLower() == "terc")
            {
                model.Titulo = "Contratante";
                contratoPorRmt = ServicioAplicacionContratos.ObtenerContratosContratante(contratante, rmt).FirstOrDefault();
                model.Identificacion = contratante.ToString("0");
                model.Rmt_Cont = rmt.ToString("0");
                model.TipoContrato = tipoContrato;
                model.Contratante = contratoPorRmt.Nombre;
                model.Telefono = contratoPorRmt.Telefono;
                model.Estado = contratoPorRmt.Estado;
                model.FechaInicio = contratoPorRmt.FechaI.Date;
                model.FechaFin = contratoPorRmt.FechaV.Date;
                model.Asesor = contratoPorRmt.nom_comp;
                model.Cantidad = contratoPorRmt.num_pers.ToString("0");
                model.IdentificacionAse = contratoPorRmt.cod_ases.ToString("C0");
                model.TelefonoAse = contratoPorRmt.TelefonoAse;
                model.EstadoAse = contratoPorRmt.EstadoAse;
                model.ModoPago = contratoPorRmt.ModoPago;
                model.FormaPago = contratoPorRmt.FormaPago;
                model.ValorContrato = contratoPorRmt.ValorContrato.ToString("C0");
                model.ValorCartera = contratoPorRmt.Cartera != null ? contratoPorRmt.Cartera.Value.ToString("C0") : "0";
                model.CuotaMensual = contratoPorRmt.CuotaMensual.ToString("C0");
                //Beneficiarios y Factura
            }

            #endregion Contratante

            switch (tipoContrato)
            {
                case "PPE":
                    model.Titulo = "PPE";
                    model.viewPMP = true;
                    model.fCorte = contratoPorRmt.fCorte;
                    model.diasFaltantes = contratoPorRmt.diasFaltantes;
                    model.diasProrrateo = contratoPorRmt.diasProrrateo;
                    //Afiliados, Factura y Estado de cuenta.
                    break;

                case "AP":
                    model.Titulo = "AP";
                    model.viewAP = true;
                    //Factura
                    break;

                case "FAM":
                    model.Titulo = "Familiar";
                    model.viewPMP = true;
                    model.fCorte = contratoPorRmt.fCorte;
                    model.diasFaltantes = contratoPorRmt.diasFaltantes;
                    model.diasProrrateo = contratoPorRmt.diasProrrateo;
                    break;

                default:
                    break;
                    //Afiliados, Factura y Estado de cuenta.
            }

            if (!string.IsNullOrEmpty(mensaje))
            {
                ViewBag.Mensaje = mensaje;
            }           

            //agrega actividad log
            //var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            //var identity = (ClaimsIdentity)HttpContext.User.Identity;
            //var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            //var log = new EmbLogActividadesDto();
            //log.UsuarioId = usuarioId;
            //log.fecha = DateTime.Now;
            //log.idTipoLog = (int)Consulta.DetalleContrato;
            //log.ip = ip;
            //log.MenuId = (int)Menus.Contratos;

            //ServicioAplicacionLogs.AgregarLog(log);
            

            return JsonConvert.SerializeObject(model);
        }

        [HttpPost]
        public async Task<string> UpdateMedioPago(DatosActualizacionMedioPagoDto model)
        {
            var respuesta = await ServicioAplicacionMedioPago.UpdateMedioPagoAsyc(model);

            if (respuesta.Identifiquer > 0) {
                //agrega actividad log
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var identity = (ClaimsIdentity)HttpContext.User.Identity;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.DetalleContrato;
                log.ip = ip;
                log.MenuId = (int)Menus.Contratos;
                log.Detalle = JsonConvert.SerializeObject(model);
                log.Respuesta = JsonConvert.SerializeObject(respuesta);

                ServicioAplicacionLogs.AgregarLog(log);
            }

            return JsonConvert.SerializeObject(respuesta);
        }

        [HttpPost]
        public async Task<string> UpdateMedioPagoIvr(DatosActualizacionMedioPagoIvrDto model)
        {
           var respuesta = await ServicioAplicacionMedioPago.UpdateMedioPagoIvrAsyc(model);           

           return JsonConvert.SerializeObject(respuesta);
        }

    }
}