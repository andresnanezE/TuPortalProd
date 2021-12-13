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

namespace Presentacion.Mvc.App.Controllers
{
    public class ReporteSabanaAsesoresController : Controller
    {
        #region Fields

        private IServicioAplicacionRoles _servicioAplicacionRoles;
        private IServicioAplicacionTablasBase _servicioAplicacionTablaBase;
        private IServicioAplicacionReporteSabanaAsesores _servicioAplicacionAfiliaciones;
        private IServicioAplicacionUsuarios _servicioAplicacionUsuarios;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        #endregion

        #region Instance Properties

        private IServicioAplicacionReporteSabanaAsesores ServicioAplicacionAfiliaciones
        {
            get
            {
                return _servicioAplicacionAfiliaciones ??
                       (_servicioAplicacionAfiliaciones =
                           FabricaIoC.Resolver<IServicioAplicacionReporteSabanaAsesores>());
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


        public ActionResult Index()
        {
            var model = new AfiliacionesModel();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var rol =
                    claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");
                model.ListaFiltros = ServicioAplicacionAfiliaciones.ObtenerFiltro_x_Rol(int.Parse(rol));
                model.ListaFiltros.Where(x => x.DatosFiltro.ID_CODIGO.Equals("Novedad")).FirstOrDefault()
                    .ListaNovedadesHomologadas = ServicioAplicacionAfiliaciones.Obtener_Novedades_Homologadas(new List<string>() { "S", "N" });
                ;
                model.ListaPeriodos = ServicioAplicacionAfiliaciones.Obtener_Periodos(); 
            }
            catch (Exception e)
            {
                if (model.ListaPeriodos == null) model.ListaPeriodos = new List<AfiliacionesPeriodoDto>();
                if (model.ListaFiltros == null) model.ListaFiltros = new List<AfiliacionesFiltroDto>();
                ModelState.AddModelError(string.Empty, "No exisiten registros para el usuario actual.");
            }

            if(Session["sabanaError"] != null)
            {
                ViewBag.Mensaje = Session["sabanaError"].ToString();
            }

            Session["sabanaError"] = null;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(AfiliacionesModel _modelRequest)
        {
            try
            {
                Session["sabanaError"] = null;

                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var rol =
                    claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");
                var userName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                var usuario = ServicioAplicacionUsuarios.ObtenerUsuarioUserName(userName);
                _modelRequest.ListaFiltros = ServicioAplicacionAfiliaciones.ObtenerFiltro_x_Rol(int.Parse(rol));

                string view = UserClaims.EsRolAsesor(rol) ? "ResumenAsesor" : "ResumenDirector";

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = 33;
                log.ip = ip;
                log.MenuId = 34;
                ServicioAplicacionLogs.AgregarLog(log);
                if (!rol.Equals("9"))
                {
                    var dto = new DatosConsultaAfiliacionDto();

                    dto.Documento = usuario.DOCUMENTO;
                    // asesor 43865135  director 43627956
                    dto.Rol = int.Parse(rol);
                    // asesor 8  director  7
                    dto.Periodo = DateTime.Parse(_modelRequest.Periodo);

                    dto.Periodo2 = DateTime.Parse(_modelRequest.Periodo2);

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

                var objetoDto = ServicioAplicacionAfiliaciones.Consultar_Detalle_Afiliacion_Resumen(dto);
                _modelRequest.ListaResultadoResumen = objetoDto.listaResumenAfiliaciones;
                _modelRequest.ListaResultadoEstatus = objetoDto.listaResumenAfiliacionesEstatus;
                }


                if (rol.Equals("9"))
                {
                    view = "ResumenTabla";
                    _modelRequest.ListaResultadoResumenTabla =
                        ServicioAplicacionAfiliaciones.Consultar_Detalle_Afiliacion_ResumenTabla(new DatosConsultaAfiliacionDto()
                                                                                                {
                                                                                                    Documento = usuario.DOCUMENTO,
                                                                                                    // asesor 43865135  director 43627956
                                                                                                    Rol = int.Parse(rol.Replace(",", "")),
                                                                                                    // asesor 8  director  7
                                                                                                    Periodo = DateTime.Parse(_modelRequest.Periodo),Periodo2 = DateTime.Parse(_modelRequest.Periodo2),
                                                                                                    Comisiona = _modelRequest.ListaFiltros.FirstOrDefault(x => x.DatosFiltro.ID_CODIGO.Equals("Comisiona")).Mostar.Equals("N")? _modelRequest.ListaFiltros.FirstOrDefault
                                                                                                        (x => x.DatosFiltro.ID_CODIGO.Equals("Comisiona")).FiltroDefault.Split(',').ToList(): _modelRequest.Comisiona,EstBenef = _modelRequest.ListaFiltros.FirstOrDefault
                                                                                                        (x => x.DatosFiltro.ID_CODIGO.Equals("EstBenef")).Mostar.Equals("N")? _modelRequest.ListaFiltros.FirstOrDefault
                                                                                                        (x => x.DatosFiltro.ID_CODIGO.Equals("EstBenef")).FiltroDefault.Split(',').ToList(): _modelRequest.EstBenef,TipContrato = _modelRequest.ListaFiltros.FirstOrDefault
                                                                                                        (x => x.DatosFiltro.ID_CODIGO.Equals("TipContrato")).Mostar.Equals("N")? _modelRequest.ListaFiltros.FirstOrDefault
                                                                                                        (x => x.DatosFiltro.ID_CODIGO.Equals("TipContrato")).FiltroDefault.Split(',').ToList(): _modelRequest.TipContrato,Novedad =_modelRequest.ListaFiltros.FirstOrDefault
                                                                                                        (x => x.DatosFiltro.ID_CODIGO.Equals("Novedad")).Mostar.Equals("N")? !string.IsNullOrWhiteSpace(_modelRequest.ListaFiltros.FirstOrDefault
                                                                                                        (x => x.DatosFiltro.ID_CODIGO.Equals("Novedad")).FiltroDefault)? _modelRequest.ListaFiltros.FirstOrDefault
                                                                                                        (x => x.DatosFiltro.ID_CODIGO.Equals("Novedad")).FiltroDefault.Split(',').ToList(): new List<string>(): _modelRequest.Novedad
                                                                                                })
                            .OrderByDescending(x => x.PERIODO_INICIO);

                    var agrupado = new string[] { "CANAL", "SEGMENTO", "CIUDAD", "TOTAL" };
                    _modelRequest.ListaGroup = agrupado.ToList();
                }


                if (rol.Equals("7"))
                {
                    var resultados = _modelRequest.ListaResultadoResumen.GroupBy(x => new { x.ESTATUS });
                    _modelRequest.ListaEstatusAsesor =
                        resultados.Select(x => new string(x.Key.ESTATUS.ToCharArray())).ToList();
                }

                Session["sabanaError"] = null;

                return PartialView(view, _modelRequest);
            }
            catch (Exception e)
            {
                var msg = "No se encontraron afiliaciones para esa combinación de filtros.";
                Session["sabanaError"] = msg;
                ViewBag.Error = msg;
                return View();
                
            }
        }


        public ActionResult ExportarInforme(AfiliacionesModel _modelRequest)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var rol =
                    claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");
                var userName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                var nombre_completo =
                    claims.Where(c => c.Type == ClaimTypes.Actor).Select(c => c.Value).SingleOrDefault();
                var usuario = ServicioAplicacionUsuarios.ObtenerUsuarioUserName(userName);
                var parametros = new List<KeyValuePair<string, object>>();
                _modelRequest.ListaFiltros = ServicioAplicacionAfiliaciones.ObtenerFiltro_x_Rol(int.Parse(rol));

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);


                
                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO", usuario.DOCUMENTO));
                parametros.Add(new KeyValuePair<string, object>("ROL", int.Parse(rol)));
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
                FileStreamResult reporte = null;
                switch (_modelRequest.TipoInforme)
                {
                    case "R":
                        var log = new EmbLogActividadesDto();
                        log.UsuarioId = usuarioId;
                        log.fecha = DateTime.Now;
                        log.idTipoLog = 16;//4;
                        log.ip = ip;
                        log.MenuId = 5;
                        ServicioAplicacionLogs.AgregarLog(log);
                        if (int.Parse(rol) == 8)
                        {
                            reporte = this.Report(
                                _modelRequest.ExportarFormato.Equals("Pdf") ? ReportFormat.PDF : ReportFormat.Excel,
                                ConfiguracionesGlobales.ReportPathAfiliacionesResumenAsesores,
                                ConfiguracionesGlobales.ReportesReportServerUrl,
                                parametros,
                                ConfiguracionesGlobales.ReportesUsername,
                                ConfiguracionesGlobales.ReportesPassword
                                );
                            reporte.FileDownloadName = "ResumenVentasAsesor - " + nombre_completo +
                                                       (_modelRequest.ExportarFormato.Equals("Pdf") ? ".pdf" : ".xls");
                            return reporte;
                        }



                        if (int.Parse(rol) == 7)
                        {
                            reporte = this.Report(
                                _modelRequest.ExportarFormato.Equals("Pdf") ? ReportFormat.PDF : ReportFormat.Excel,
                                ConfiguracionesGlobales.ReportPathAfiliacionesResumenDirectores,
                                ConfiguracionesGlobales.ReportesReportServerUrl,
                                parametros,
                                ConfiguracionesGlobales.ReportesUsername,
                                ConfiguracionesGlobales.ReportesPassword
                                );
                            reporte.FileDownloadName = "ResumenVentasDirector - " + nombre_completo +
                                                       (_modelRequest.ExportarFormato.Equals("Pdf") ? ".pdf" : ".xls");
                            ;
                            return reporte;
                        }


                        if (int.Parse(rol) == 9)
                        {
                            reporte = this.Report(
                                _modelRequest.ExportarFormato.Equals("Pdf") ? ReportFormat.PDF : ReportFormat.Excel,
                                ConfiguracionesGlobales.ReportPathAfiliacionesResumenCadenaSupervision,
                                ConfiguracionesGlobales.ReportesReportServerUrl,
                                parametros,
                                ConfiguracionesGlobales.ReportesUsername,
                                ConfiguracionesGlobales.ReportesPassword
                                );
                            reporte.FileDownloadName = "ResumenVentasDirector - " + nombre_completo +
                                                       (_modelRequest.ExportarFormato.Equals("Pdf") ? ".pdf" : ".xls");
                            ;
                            return reporte;
                        }

                        break;
                    default:

                        log = new EmbLogActividadesDto();
                        log.UsuarioId = usuarioId;
                        log.fecha = DateTime.Now;
                        log.idTipoLog = 15;//4;
                        log.ip = ip;
                        log.MenuId = 5;
                        ServicioAplicacionLogs.AgregarLog(log);

                        reporte = this.Report(
                            ReportFormat.Excel,
                            ConfiguracionesGlobales.ReportPathDetallesAfiliaciones,
                            ConfiguracionesGlobales.ReportesReportServerUrl,
                            parametros,
                            ConfiguracionesGlobales.ReportesUsername,
                            ConfiguracionesGlobales.ReportesPassword
                            );
                        var datosRol = ServicioAplicacionRoles.ObtenerRolId(int.Parse(rol));
                        reporte.FileDownloadName = "ReporteDetalladoVentas" + datosRol.ROL + " - " +
                                                   nombre_completo + ".xls";
                        return reporte;
                        break;
                }
                return null;
            }
            catch (Exception e)
            {
                _modelRequest.MensajeError =
                    "Error realizando la exportación del informe, comuníquese con el administrador.";
                return PartialView("Error");
            }

        }



        public ActionResult ExportarResumenEstatus(AfiliacionesModel _modelRequest)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var rol = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");
                var userName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                var nombre_completo = claims.Where(c => c.Type == ClaimTypes.Actor).Select(c => c.Value).SingleOrDefault();
                var usuario = ServicioAplicacionUsuarios.ObtenerUsuarioUserName(userName);
                var parametros = new List<KeyValuePair<string, object>>();
                _modelRequest.ListaFiltros = ServicioAplicacionAfiliaciones.ObtenerFiltro_x_Rol(int.Parse(rol));

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
                
                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO", usuario.DOCUMENTO)); 
                FileStreamResult reporte = null;
                if (int.Parse(rol) == 7)
                {
                        var log = new EmbLogActividadesDto();
                        log.UsuarioId = usuarioId;
                        log.fecha = DateTime.Now;
                        log.idTipoLog = 34;//4;
                        log.ip = ip;
                        log.MenuId = 34;
                        ServicioAplicacionLogs.AgregarLog(log);
                       
                        reporte = this.Report(
                            _modelRequest.ExportarFormato.Equals("Pdf") ? ReportFormat.PDF : ReportFormat.Excel,
                            ConfiguracionesGlobales.ReportPathSabanaAsesoresEstatus,
                            ConfiguracionesGlobales.ReportesReportServerUrl,
                            parametros,
                            ConfiguracionesGlobales.ReportesUsername,
                            ConfiguracionesGlobales.ReportesPassword
                            );
                        reporte.FileDownloadName = "ResumenEstatus - " + nombre_completo +
                                                    (_modelRequest.ExportarFormato.Equals("Pdf") ? ".pdf" : ".xls");
                        return reporte;                         
                }
                return null;
            }
            catch (Exception e)
            {
                _modelRequest.MensajeError =
                    "Error realizando la exportación del informe, comuníquese con el administrador.";
                return PartialView("Error");
            }

        }
        public ActionResult ExportarResumenDetallado(AfiliacionesModel _modelRequest)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var rol = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");
                var userName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                var nombre_completo = claims.Where(c => c.Type == ClaimTypes.Actor).Select(c => c.Value).SingleOrDefault();
                var usuario = ServicioAplicacionUsuarios.ObtenerUsuarioUserName(userName);
                var parametros = new List<KeyValuePair<string, object>>();
                _modelRequest.ListaFiltros = ServicioAplicacionAfiliaciones.ObtenerFiltro_x_Rol(int.Parse(rol));

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                parametros.Add(new KeyValuePair<string, object>("DOCUMENTO", usuario.DOCUMENTO));
                FileStreamResult reporte = null;
                if (int.Parse(rol) == 7)
                {
                    var log = new EmbLogActividadesDto();
                    log.UsuarioId = usuarioId;
                    log.fecha = DateTime.Now;
                    log.idTipoLog = 35;//4;
                    log.ip = ip;
                    log.MenuId = 5;
                    ServicioAplicacionLogs.AgregarLog(log);

                    reporte = this.Report(
                        _modelRequest.ExportarFormato.Equals("Pdf") ? ReportFormat.PDF : ReportFormat.Excel,
                        ConfiguracionesGlobales.ReportPathSabanaAsesoresDetallado,
                        ConfiguracionesGlobales.ReportesReportServerUrl,
                        parametros,
                        ConfiguracionesGlobales.ReportesUsername,
                        ConfiguracionesGlobales.ReportesPassword
                        );
                    reporte.FileDownloadName = "ResumenEstatus - " + nombre_completo +
                                                (_modelRequest.ExportarFormato.Equals("Pdf") ? ".pdf" : ".xls");
                    return reporte;
                }
                return null;
            }
            catch (Exception e)
            {
                _modelRequest.MensajeError =
                    "Error realizando la exportación del informe, comuníquese con el administrador.";
                return PartialView("Error");
            }

        }
        public ActionResult ExportarInformeAsesor(AfiliacionesModel _modelRequest, string CedulaAsesor, string NombreAsesor
            , string Estatus, int AreasProtegidas, int Pmp, int Familiar, string PerInicio, string PerFin, int Productividad
            , decimal Cumplimiento, int Metas)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var rol =
                    claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");
                var userName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                var nombre_completo =
                    claims.Where(c => c.Type == ClaimTypes.Actor).Select(c => c.Value).SingleOrDefault();
                var usuario = ServicioAplicacionUsuarios.ObtenerUsuarioUserName(userName);
                var parametros = new List<KeyValuePair<string, object>>();
                _modelRequest.ListaFiltros = ServicioAplicacionAfiliaciones.ObtenerFiltro_x_Rol(int.Parse(rol));

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                parametros.Add(new KeyValuePair<string, object>("CC_ASESOR", CedulaAsesor));
                parametros.Add(new KeyValuePair<string, object>("ASESOR", NombreAsesor));
                parametros.Add(new KeyValuePair<string, object>("ESTATUS", Estatus));
                parametros.Add(new KeyValuePair<string, object>("AREAS_PROTEGIDAS", AreasProtegidas));
                parametros.Add(new KeyValuePair<string, object>("PMP", Pmp));
                parametros.Add(new KeyValuePair<string, object>("FAMILIAR", Familiar));
                parametros.Add(new KeyValuePair<string, object>("PER_INICIO", PerInicio));
                parametros.Add(new KeyValuePair<string, object>("PER_FIN", PerFin));
                parametros.Add(new KeyValuePair<string, object>("PRODUCTIVIDAD", Productividad));
                parametros.Add(new KeyValuePair<string, object>("CUMPLIMIENTO", Cumplimiento));
                parametros.Add(new KeyValuePair<string, object>("METAS", Metas));
                
                //_modelRequest.TipoInforme
                FileStreamResult reporte = null;

                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = 36;//4;
                log.ip = ip;
                log.MenuId = 5;
                ServicioAplicacionLogs.AgregarLog(log);

                if (int.Parse(rol) == 7)
                {
                    reporte = this.Report(
                        _modelRequest.ExportarFormato.Equals("Pdf") ? ReportFormat.PDF : ReportFormat.Excel,
                        ConfiguracionesGlobales.ReportPathSabanaAsesores,
                        ConfiguracionesGlobales.ReportesReportServerUrl,
                        parametros,
                        ConfiguracionesGlobales.ReportesUsername,
                        ConfiguracionesGlobales.ReportesPassword
                        );
                    reporte.FileDownloadName = "ResumenVentasDirector - " + nombre_completo +
                                                (_modelRequest.ExportarFormato.Equals("Pdf") ? ".pdf" : ".xls");
                    ;
                    return reporte;
                }
                return null;
            }
            catch (Exception e)
            {
                _modelRequest.MensajeError =
                    "Error realizando la exportación del informe, comuníquese con el administrador.";
                return PartialView("Error");
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

