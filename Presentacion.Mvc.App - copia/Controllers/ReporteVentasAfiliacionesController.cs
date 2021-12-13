using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion.IoC;
using MvcReportViewer;
using System.IO;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;

namespace Presentacion.Mvc.App.Controllers
{
    public class ReporteVentasAfiliacionesController : Controller
    {

        private readonly string MENSAJE = $"{ConfiguracionesGlobales.MsgErrorRte1} {ConfiguracionesGlobales.MsgErrorRte2}";
        private readonly string MENSAJECARGAPREVIA = $"{ConfiguracionesGlobales.MsgErrorDataPrev}";
        private readonly string MOSTRARTRACE = $"{ConfiguracionesGlobales.MostrarTrace}";

        #region Fields

        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioAplicacionTablasBase _servicioAplicacionTablasBase;
        private IServicioAplicacionReporteVentasAfiliaciones _servicioAplicacionAfiliaciones;
        private IServicioAplicacionEsCadenaSupervision _servicioAplicacionEsCadenaSupervision;
        private IServicioAplicacionUsuarios _servicioAplicacionUsuarios;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        
        #endregion

        #region Instance Properties

        private IServicioAplicacionReporteVentasAfiliaciones ServicioAplicacionAfiliaciones
        {
            get
            {
                return _servicioAplicacionAfiliaciones ??
                       (_servicioAplicacionAfiliaciones =
                           FabricaIoC.Resolver<IServicioAplicacionReporteVentasAfiliaciones>());
            }
        }
        private IServicioAplicacionEsCadenaSupervision ServicioAplicacionEsCadenaSupervision
        {
            get
            {
                return _servicioAplicacionEsCadenaSupervision ??
                       (_servicioAplicacionEsCadenaSupervision =
                           FabricaIoC.Resolver<IServicioAplicacionEsCadenaSupervision>());
            }
        }
        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }
        private IServicioAplicacionUsuarios ServicioAplicacionUsuarios
        {
            get
            {
                return _servicioAplicacionUsuarios ??
                       (_servicioAplicacionUsuarios = FabricaIoC.Resolver<IServicioAplicacionUsuarios>());
            }
        }

        private IServicioAplicacionRoles ServicioAplicacionRoles
        {
            get
            {
                return _servicioAplicacionRoles ??
                       (_servicioAplicacionRoles = FabricaIoC.Resolver<IServicioAplicacionRoles>());
            }
        }

        #endregion

        public ActionResult Index(string msg)
        {
            var model = new AfiliacionesModel();
            var usuarioTuPortal = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

            try
            {

                model.ListaPeriodos = new List<AfiliacionesPeriodoDto>();
                model.ListaFiltros = new List<AfiliacionesFiltroDto>();

                model.ListaFiltros = ServicioAplicacionAfiliaciones.ObtenerFiltro_x_Rol(int.Parse(usuarioTuPortal.Rol));
                model.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("Novedad"))
                    .FirstOrDefault().ListaNovedadesHomologadas =
                    ServicioAplicacionAfiliaciones.Obtener_Novedades_Homologadas(new List<string>() { "S", "N" });

                model.ListaPeriodos = ServicioAplicacionAfiliaciones.Obtener_Periodos();

                //actualiza log actividades
                var identity = (ClaimsIdentity)User.Identity;
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);


                if ((Session["esSesionEnVezDe"] != null) && (Boolean.Parse(Session["esSesionEnVezDe"].ToString())))
                {
                    var documentoPadre = (identity.FindFirst(ClaimTypes.Anonymous).Value).ToString();

                    var nombrePadre = (identity.FindFirst(ClaimTypes.Authentication).Value).ToString();

                    var documentoHijo = (identity.FindFirst(ClaimTypes.NameIdentifier).Value).ToString();
                    var nombreHijo = (identity.FindFirst(ClaimTypes.Actor).Value).ToString();


                    var log = new EmbLogActividadesDto
                    {
                        UsuarioId = usuarioIdentity,
                        fecha = DateTime.Now,
                        idTipoLog = (int)Consulta.ConsultaDeVentaAfiliaciones,
                        ip = ip,
                        MenuId = (int)Menus.Detalle_de_afiliaciones,
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
                        UsuarioId = usuarioIdentity,
                        fecha = DateTime.Now,
                        idTipoLog = (int)Consulta.ConsultaDeVentaAfiliaciones,
                        ip = ip,
                        MenuId = (int)Menus.Detalle_de_afiliaciones
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                }
                //

            
                if(!string.IsNullOrEmpty(msg))
                {
                    throw new Exception(MENSAJE);
                }


                if (!usuarioTuPortal.VerReporte)
                {
                    throw new Exception(ConfiguracionesGlobales.MensajeRteCadenaSupervision);
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, MENSAJE);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(AfiliacionesModel _modelRequest)
        {
            var usuarioTuPortal = UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);

            try
            {

                _modelRequest.ListaFiltros = ServicioAplicacionAfiliaciones.ObtenerFiltro_x_Rol(int.Parse(usuarioTuPortal.Rol));
                string view = UserClaims.EsRolAsesor(usuarioTuPortal.Rol) ? "ResumenAsesor" : "ResumenDirector";

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;

                var log = new EmbLogActividadesDto();
                log.UsuarioId = int.Parse(usuarioTuPortal.UserId);
                log.fecha = DateTime.Now;
                log.idTipoLog = 4;
                log.ip = ip;
                log.MenuId = (int) Menus.Detalle_de_afiliaciones;
                ServicioAplicacionLogs.AgregarLog(log);
                if (!UserClaims.EsRolCadenaSupervision(usuarioTuPortal.Rol))
                {
                    var dto = new DatosConsultaAfiliacionDto();

                    dto.Documento = usuarioTuPortal.Document;
                    dto.Rol = int.Parse(usuarioTuPortal.Rol);
                    dto.Periodo = DateTime.Parse(_modelRequest.Periodo);
                    dto.Periodo2 = DateTime.Parse(_modelRequest.Periodo2);
                    dto.Periodo = new DateTime(dto.Periodo.Year, dto.Periodo.Month, dto.Periodo.Day);
                    dto.Periodo2 = new DateTime(dto.Periodo2.Year, dto.Periodo2.Month, dto.Periodo2.Day);


                    dto.Comisiona = _modelRequest.ListaFiltros
                            .FirstOrDefault(x => x.DatosFiltro.ID_CODIGO.Equals("Comisiona"))
                            .Mostar.Equals("N")
                            ? _modelRequest.ListaFiltros
                                .FirstOrDefault(x => x.DatosFiltro.ID_CODIGO.Equals("Comisiona"))
                                .FiltroDefault.Split(',')
                                .ToList()
                            : _modelRequest.Comisiona;

                    dto.EstBenef = _modelRequest.ListaFiltros
                            .FirstOrDefault(x => x.DatosFiltro.ID_CODIGO.Equals("EstBenef"))
                            .Mostar.Equals("N")
                            ? _modelRequest.ListaFiltros
                                .FirstOrDefault(x => x.DatosFiltro.ID_CODIGO.Equals("EstBenef"))
                                .FiltroDefault.Split(',')
                                .ToList()
                            : _modelRequest.EstBenef;

                    dto.TipContrato =
                        _modelRequest.ListaFiltros
                            .FirstOrDefault(x => x.DatosFiltro.ID_CODIGO.Equals("TipContrato"))
                            .Mostar.Equals("N")
                            ? _modelRequest.ListaFiltros
                                .FirstOrDefault(x => x.DatosFiltro.ID_CODIGO.Equals("TipContrato"))
                                .FiltroDefault.Split(',')
                                .ToList()
                            : _modelRequest.TipContrato;

                    dto.Novedad =
                        _modelRequest.ListaFiltros
                            .FirstOrDefault(x => x.DatosFiltro.ID_CODIGO.Equals("Novedad"))
                            .Mostar.Equals("N")
                            ? !string.IsNullOrWhiteSpace(
                                _modelRequest.ListaFiltros
                                    .FirstOrDefault(x => x.DatosFiltro.ID_CODIGO.Equals("Novedad"))
                                    .FiltroDefault)
                                ? _modelRequest.ListaFiltros
                                    .FirstOrDefault(x => x.DatosFiltro.ID_CODIGO.Equals("Novedad"))
                                    .FiltroDefault.Split(',')
                                    .ToList()
                                : new List<string>()
                            : _modelRequest.Novedad;

                    _modelRequest.ListaResultadoResumen =
                            ServicioAplicacionAfiliaciones.Consultar_Detalle_Afiliacion_Resumen(dto);
                }


                if (UserClaims.EsRolCadenaSupervision(usuarioTuPortal.Rol))
                {
                    view = "ResumenTabla";
                    _modelRequest.ListaResultadoResumenTabla =
                        ServicioAplicacionAfiliaciones.Consultar_Detalle_Afiliacion_ResumenTabla(new DatosConsultaAfiliacionDto
                            ()
                        {
                            Documento =
                                                                                                        usuarioTuPortal.Document,
                            Rol =
                                                                                                        int.Parse(
                                                                                                            usuarioTuPortal.Rol.Replace
                                                                                                        (",", "")),
                            Periodo = DateTime.Parse(_modelRequest.Periodo),
                            Periodo2 =
                                                                                                            DateTime.Parse(_modelRequest.Periodo2),
                            Comisiona =
                                                                                                        _modelRequest
                                                                                                        .ListaFiltros
                                                                                                        .FirstOrDefault
                                                                                                        (x =>
                                                                                                        x.DatosFiltro
                                                                                                        .ID_CODIGO
                                                                                                        .Equals(
                                                                                                            "Comisiona"))
                                                                                                        .Mostar.Equals(
                                                                                                            "N")
                                                                                                            ? _modelRequest
                                                                                                        .ListaFiltros
                                                                                                        .FirstOrDefault
                                                                                                        (x =>
                                                                                                        x.DatosFiltro
                                                                                                        .ID_CODIGO
                                                                                                        .Equals(
                                                                                                            "Comisiona"))
                                                                                                        .FiltroDefault
                                                                                                        .Split(',')
                                                                                                        .ToList()
                                                                                                            : _modelRequest
                                                                                                        .Comisiona,
                            EstBenef =
                                                                                                        _modelRequest
                                                                                                        .ListaFiltros
                                                                                                        .FirstOrDefault
                                                                                                        (x =>
                                                                                                        x.DatosFiltro
                                                                                                        .ID_CODIGO
                                                                                                        .Equals(
                                                                                                            "EstBenef"))
                                                                                                        .Mostar.Equals(
                                                                                                            "N")
                                                                                                            ? _modelRequest
                                                                                                        .ListaFiltros
                                                                                                        .FirstOrDefault
                                                                                                        (x =>
                                                                                                        x.DatosFiltro
                                                                                                        .ID_CODIGO
                                                                                                        .Equals(
                                                                                                            "EstBenef"))
                                                                                                        .FiltroDefault
                                                                                                        .Split(',')
                                                                                                        .ToList()
                                                                                                            : _modelRequest
                                                                                                        .EstBenef,
                            TipContrato =
                                                                                                        _modelRequest
                                                                                                        .ListaFiltros
                                                                                                        .FirstOrDefault
                                                                                                        (x =>
                                                                                                        x.DatosFiltro
                                                                                                        .ID_CODIGO
                                                                                                        .Equals(
                                                                                                            "TipContrato"))
                                                                                                        .Mostar.Equals(
                                                                                                            "N")
                                                                                                            ? _modelRequest
                                                                                                        .ListaFiltros
                                                                                                        .FirstOrDefault
                                                                                                        (x =>
                                                                                                        x.DatosFiltro
                                                                                                        .ID_CODIGO
                                                                                                        .Equals(
                                                                                                            "TipContrato"))
                                                                                                        .FiltroDefault
                                                                                                        .Split(',')
                                                                                                        .ToList()
                                                                                                            : _modelRequest
                                                                                                        .TipContrato,
                            Novedad =
                                                                                                        _modelRequest
                                                                                                        .ListaFiltros
                                                                                                        .FirstOrDefault
                                                                                                        (x =>
                                                                                                        x.DatosFiltro
                                                                                                        .ID_CODIGO
                                                                                                        .Equals(
                                                                                                            "Novedad"))
                                                                                                        .Mostar.Equals(
                                                                                                            "N")
                                                                                                            ? !string
                                                                                                        .IsNullOrWhiteSpace
                                                                                                        (_modelRequest
                                                                                                        .ListaFiltros
                                                                                                        .FirstOrDefault
                                                                                                        (x =>
                                                                                                        x.DatosFiltro
                                                                                                        .ID_CODIGO
                                                                                                        .Equals(
                                                                                                            "Novedad"))
                                                                                                        .FiltroDefault)
                                                                                                                ? _modelRequest
                                                                                                        .ListaFiltros
                                                                                                        .FirstOrDefault
                                                                                                        (x =>
                                                                                                        x.DatosFiltro
                                                                                                        .ID_CODIGO
                                                                                                        .Equals(
                                                                                                            "Novedad"))
                                                                                                        .FiltroDefault
                                                                                                        .Split(',')
                                                                                                        .ToList()
                                                                                                                : new List
                                                                                                        <string>()
                                                                                                            : _modelRequest
                                                                                                        .Novedad
                        })
                            .OrderByDescending(x => x.PERIODO_INICIO);

                    var agrupado = new string[] { "CANAL", "SEGMENTO", "CIUDAD", "TOTAL" };
                    _modelRequest.ListaGroup = agrupado.ToList();
                }

                if (UserClaims.EsRolDirector(usuarioTuPortal.Rol))
                {
                    var resultados = _modelRequest.ListaResultadoResumen.GroupBy(x => new { x.ESTATUS });
                    _modelRequest.ListaEstatusAsesor =
                        resultados.Select(x => new string(x.Key.ESTATUS.ToCharArray())).ToList();
                }

                return PartialView(view, _modelRequest);
            }
            catch (Exception e)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, MENSAJE);
            }


            return RedirectToAction("Index", new {msg = MENSAJE });
        }

        public ActionResult ExportarInforme(AfiliacionesModel _modelRequest)
        {


            UserClaims usuarioActual = null;
            List<KeyValuePair<string, object>> parametros = null;
            FileStreamResult reporte = null;

            string ahora = string.Empty;
            string handle = string.Empty;
            string nomRte = string.Empty;

            try
            {

                //usuarioActual = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);
                usuarioActual =  UserClaims.UserClaimsSession((ClaimsIdentity)User.Identity);
                parametros = new List<KeyValuePair<string, object>>();

                _modelRequest.ListaFiltros = ServicioAplicacionAfiliaciones.ObtenerFiltro_x_Rol(int.Parse(usuarioActual.Rol));

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;


                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO", usuarioActual.Document));
                parametros.Add(new KeyValuePair<string, object>("ROL", int.Parse(usuarioActual.Rol)));
                parametros.Add(new KeyValuePair<string, object>("TIPO", _modelRequest.TipoInforme));
                parametros.Add(new KeyValuePair<string, object>("FECH_PERIODO",
             DateTime.Parse(_modelRequest.Periodo).ToString("s")));//   
                parametros.Add(new KeyValuePair<string, object>("COMISIONA",
                    string.Join(",",
                        _modelRequest.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("Comisiona"))
                            .FirstOrDefault()
                            .Mostar.Equals("N")
                            ? _modelRequest.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("Comisiona"))
                                .FirstOrDefault()
                                .FiltroDefault.Split(',')
                                .ToList()
                            : _modelRequest.Comisiona)));
                parametros.Add(new KeyValuePair<string, object>("TIP_CONTR",
                    string.Join(",",
                        _modelRequest.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("TipContrato"))
                            .FirstOrDefault()
                            .Mostar.Equals("N")
                            ? _modelRequest.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("TipContrato"))
                                .FirstOrDefault()
                                .FiltroDefault.Split(',')
                                .ToList()
                            : _modelRequest.TipContrato)));
                parametros.Add(new KeyValuePair<string, object>("EST_BENEF",
                    string.Join(",",
                        _modelRequest.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("EstBenef"))
                            .FirstOrDefault()
                            .Mostar.Equals("N")
                            ? _modelRequest.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("EstBenef"))
                                .FirstOrDefault()
                                .FiltroDefault.Split(',')
                                .ToList()
                            : _modelRequest.EstBenef)));
                parametros.Add(new KeyValuePair<string, object>("TIP_NOVEDAD",
                    string.Join(",", _modelRequest.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("Novedad"))
                        .FirstOrDefault().Mostar.Equals("N")
                        ? !string.IsNullOrWhiteSpace(
                            _modelRequest.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("Novedad"))
                                .FirstOrDefault().FiltroDefault)
                            ? _modelRequest.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("Novedad"))
                                .FirstOrDefault()
                                .FiltroDefault.Split(',')
                                .ToList()
                            : new List<string>()
                        : _modelRequest.Novedad)));
                parametros.Add(new KeyValuePair<string, object>("FECH_PERIODO2",
             DateTime.Parse(_modelRequest.Periodo2).ToString("s")));

                switch (_modelRequest.TipoInforme)
                {
                    case "R":
                        var log = new EmbLogActividadesDto();
                        log.UsuarioId = Int32.Parse(usuarioActual.UserId);
                        log.fecha = DateTime.Now;
                        log.idTipoLog = (int)Descarga.GeneracionDeReporteConsolidadoVentaDeAfiliaciones;//4;
                        log.ip = ip;
                        log.MenuId = (int) Menus.Detalle_de_afiliaciones;
                        ServicioAplicacionLogs.AgregarLog(log);
                        if (UserClaims.EsRolAsesor(usuarioActual.Rol))
                        {
                            reporte = this.Report(
                                _modelRequest.ExportarFormato.Equals("Pdf") ? ReportFormat.PDF : ReportFormat.Excel,
                                ConfiguracionesGlobales.ReportPathAfiliacionesResumenAsesores,
                                ConfiguracionesGlobales.ReportesReportServerUrl,
                                parametros,
                                ConfiguracionesGlobales.ReportesUsername,
                                ConfiguracionesGlobales.ReportesPassword
                                );
                            reporte.FileDownloadName = "ResumenVentasAsesor - " + usuarioActual.Name +
                                                       (_modelRequest.ExportarFormato.Equals("Pdf") ? ".pdf" : ".xls");
                            break;
                        }

                        if (UserClaims.EsRolDirector(usuarioActual.Rol))
                        {
                            reporte = this.Report(
                                _modelRequest.ExportarFormato.Equals("Pdf") ? ReportFormat.PDF : ReportFormat.Excel,
                                ConfiguracionesGlobales.ReportPathAfiliacionesResumenDirectores,
                                ConfiguracionesGlobales.ReportesReportServerUrl,
                                parametros,
                                ConfiguracionesGlobales.ReportesUsername,
                                ConfiguracionesGlobales.ReportesPassword
                                );
                            reporte.FileDownloadName = "ResumenVentasDirector - " + usuarioActual.Name +
                                                       (_modelRequest.ExportarFormato.Equals("Pdf") ? ".pdf" : ".xls");
                            break;
                        }


                        if (UserClaims.EsRolCadenaSupervision(usuarioActual.Rol))
                        {
                            reporte = this.Report(
                                _modelRequest.ExportarFormato.Equals("Pdf") ? ReportFormat.PDF : ReportFormat.Excel,
                                ConfiguracionesGlobales.ReportPathAfiliacionesResumenCadenaSupervision,
                                ConfiguracionesGlobales.ReportesReportServerUrl,
                                parametros,
                                ConfiguracionesGlobales.ReportesUsername,
                                ConfiguracionesGlobales.ReportesPassword
                                );
                            reporte.FileDownloadName = "ResumenVentasDirector - " + usuarioActual.Name +
                                                       (_modelRequest.ExportarFormato.Equals("Pdf") ? ".pdf" : ".xls");
                            break;
                        }

                        break;
                    default:

                        log = new EmbLogActividadesDto();
                        log.UsuarioId = Int32.Parse(usuarioActual.UserId);
                        log.fecha = DateTime.Now;
                        log.idTipoLog = (int)Descarga.GeneracionDeReporteDetalleVentaDeAfiliaciones;//4;
                        log.ip = ip;
                        log.MenuId = (int) Menus.Detalle_de_afiliaciones;
                        ServicioAplicacionLogs.AgregarLog(log);

                        reporte = this.Report(
                            ReportFormat.Excel,
                            ConfiguracionesGlobales.ReportPathDetallesAfiliaciones,
                            ConfiguracionesGlobales.ReportesReportServerUrl,
                            parametros,
                            ConfiguracionesGlobales.ReportesUsername,
                            ConfiguracionesGlobales.ReportesPassword
                            );
                        var datosRol = ServicioAplicacionRoles.ObtenerRolId(int.Parse(usuarioActual.Rol));
                        reporte.FileDownloadName = "ReporteDetalladoVentas" + datosRol.ROL + " - " +
                                                   usuarioActual.Name + ".xls";
                        break;
                }

                ahora = DateTime.Now.ToString("s");
                handle = Guid.NewGuid().ToString();

                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();


                return Json(new { FileGuid = handle, FileName = reporte.FileDownloadName });

            }
            catch (Exception exception)
            {
                return Json(new
                {
                    msgError = MENSAJE,
                    msgErrorException = MOSTRARTRACE.Equals("1") ?
                    $"{exception.Source}</br>{exception.Message}</br>{exception.StackTrace}" : string.Empty
                });
            }



        }

        public JsonResult ObtenerPeriodoPorId(DateTime periodoId)
        {
            var result = new JsonResult();
            var periodos = ServicioAplicacionAfiliaciones.Obtener_Periodos().Where(x => x.FECHA_CORTE >= periodoId);
            result.Data = periodos;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

