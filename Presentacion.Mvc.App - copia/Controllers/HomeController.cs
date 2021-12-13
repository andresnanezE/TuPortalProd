// ----------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Security.Claims;
using Aplicacion.Administracion.Contratos;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion.IoC;
using Aplicacion.Administracion.Dto;
using MvcReportViewer;

namespace Presentacion.Mvc.App.Controllers
{
    public class HomeController : Controller
    {
        private IServicioAplicacionComoVoy _servicioAplicacionComoVoy;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private IServicioAplicacionUsuarios _servicioAplicacionUsuarios;
        private IServicioAplicacionReporteVentasAfiliaciones _servicioAplicacionAfiliaciones;
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

        private IServicioAplicacionReporteVentasAfiliaciones ServicioAplicacionAfiliaciones
        {
            get
            {
                return _servicioAplicacionAfiliaciones ??
                       (_servicioAplicacionAfiliaciones =
                           FabricaIoC.Resolver<IServicioAplicacionReporteVentasAfiliaciones>());
            }
        }


        private IServicioAplicacionComoVoy ServicioAplicacionComoVoy
        {
            get
            {
                return _servicioAplicacionComoVoy ??
                       (_servicioAplicacionComoVoy =
                           FabricaIoC.Resolver<IServicioAplicacionComoVoy>());
            }
        }

        public IEnumerable<AfiliacionesFiltroDto> ListaFiltros { get; set; }

        #region Instance Methods

        public ActionResult Index(ComoVoyModel model)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                UserClaims usuarioActual = null;

                IEnumerable<Claim> claims = identity.Claims;
                var rol =
                    claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");
                var userName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                ViewBag.rol = "";

                if (UserClaims.EsRolAsesor(rol))
                {
                    //TODO: Se comentara var usuario donde es el origen de la falla y no se esta usando, validar esto
                    //var usuario = ServicioAplicacionUsuarios.ObtenerUsuarioUserName(userName);
                    var parametros = new List<KeyValuePair<string, object>>();
                    ListaFiltros = ServicioAplicacionAfiliaciones.ObtenerFiltro_x_Rol(int.Parse(rol));


                    var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                    var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                    model.CC_ASESOR =
                        Convert.ToDecimal(
                            claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                .Select(c => c.Value)
                                .SingleOrDefault());

                    try
                    {
                        /* if (Session["ComoVoy"] == null)
                   {*/

                        /*
                         * Se detecto que si la base de datos de remuneraciones estaba abajo, impedia el ingreso a tuportal
                         * al producirse una Exception, ya que el EMSP_INFO_VENTAS_ASESOR usa REMUNERACIONES..RE_NIVEL para 
                         * obtener el NOM_NIVE, por esa razon se anida este bloque try-catch y se devuelve un ew List<ComoVoyDto>() vacio.
                         */
                        var comovoy = ServicioAplicacionComoVoy
                                .CantidadVentasAsesorMesActual(model.CC_ASESOR, rol, ConfiguracionesGlobales.pathImagenesComoVoy,
                                    ConfiguracionesGlobales.urlActionDownloadPdf)
                                .OrderBy(c => c.POS);

                        Session["ComoVoy"] = comovoy;
                        /*}*/
                    }
                    catch (Exception)
                    {
                        Session["ComoVoy"] = new List<ComoVoyDto>();
                        ViewBag.rol = "Asesor";
                        return View(new List<ComoVoyDto>());
                    }



                    ViewBag.rol = "Asesor";
                    return View((Session["ComoVoy"] as IEnumerable<ComoVoyDto>).ToList());
                }

                return View();
            }
            catch (Exception e)
            {

                model.MensajeError = "Error realizando la operación, comuníquese con el administrador. " + e.Message;
                return RedirectToAction("Index", "Login");
            }
        }

        #endregion

        public ActionResult Download(string tipoMeta)
        {
            string escalera = string.Empty;
            List<string> paths = null;
            string archivo = string.Empty;
            string estatus = string.Empty;
            string nombreArchivo = string.Empty;
            string root = string.Empty;

            try
            {
                if (Session["ComoVoy"] == null)
                {
                    obtVarSesions();
                }

                if (tipoMeta.Equals("3"))
                {
                    return GenerarReporteComisionesPendientes();
                }
                else
                {
                    var comovoy = Session["ComoVoy"] as IEnumerable<ComoVoyDto>;

                    estatus = comovoy.Select(C => C.ESTATUS_HOMOLOG).First();

                    paths = new List<string>
                    {
                        comovoy.Select(C => C.PATHMETA0).First(),
                        comovoy.Select(C => C.PATHMETA1).First(),
                        comovoy.Select(C => C.PATHMETA2).First()
                    };

                }

                if (paths[0].IndexOf("directo") > 0)
                {
                    if (tipoMeta.Equals("2"))
                    {
                        archivo = ConfiguracionesGlobales.PathCondicionesDirecto;
                    }
                    else if(tipoMeta.Equals("0"))
                    {
                        archivo = ConfiguracionesGlobales.PathPlanVueloDirectoMeta0;
                    }
                    else if (tipoMeta.Equals("1"))
                    {
                        archivo = ConfiguracionesGlobales.PathPlanVueloDirectoMeta1;
                    }                    

                }
                else
                {
                    if (tipoMeta.Equals("2"))
                    {
                        archivo = ConfiguracionesGlobales.PathCondicionesCorretaje;
                    }
                    else if (tipoMeta.Equals("0"))
                    {
                        archivo = ConfiguracionesGlobales.PathPlanVueloCorretajeMeta0;
                    }
                    else if (tipoMeta.Equals("1"))
                    {
                        archivo = ConfiguracionesGlobales.PathPlanVueloCorretajeMeta1;
                    }                    
                }

                var files = Directory.GetFiles(archivo);

                if (!files.Any())
                    throw new Exception("El directorio no tiene archivos");

                byte[] fileBytes = GetFile(files[0]);
                return File(
                    fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(files[0]));

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Puede que el archivo no exista o sea un problema con tu conexión. Intentalo nuevamente, si el problema persiste comunicate con el área de TI.";
                ViewBag.rol = UserClaims.RolesAsesor.First();
                return View("Index", (Session["ComoVoy"] as IEnumerable<ComoVoyDto>).ToList());
            }

        }

        private void obtVarSesions()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var rol =
                   claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault().Replace(",", "");

            var CC_ASESOR =
                  Convert.ToDecimal(
                      claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                          .Select(c => c.Value)
                          .SingleOrDefault());


            var comovoy = ServicioAplicacionComoVoy
                 .CantidadVentasAsesorMesActual(CC_ASESOR, rol, ConfiguracionesGlobales.pathImagenesComoVoy, ConfiguracionesGlobales.urlActionDownloadPdf)
                 .OrderBy(c => c.POS);

            Session["ComoVoy"] = comovoy;
        }

        private string nombreMes()
        {
            var meses = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("enero",1),
                new Tuple<string, int>("febrero",2),
                new Tuple<string, int>("marzo",3),
                new Tuple<string, int>("abril",4),
                new Tuple<string, int>("mayo",5),
                new Tuple<string, int>("junio",6),
                new Tuple<string, int>("julio",7),
                new Tuple<string, int>("agosto",8),
                new Tuple<string, int>("septiembre",9),
                new Tuple<string, int>("octibre",10),
                new Tuple<string, int>("noviembre",11),
                new Tuple<string, int>("diciembre",12),
            };

            return meses.Where(m => m.Item2 == DateTime.Now.Month).Select(c => c.Item1).ToList()[0];

        }

        byte[] GetFile(string s)
        {
            using (FileStream fs = System.IO.File.OpenRead(s))
            {
                byte[] data = new byte[fs.Length];
                int br = fs.Read(data, 0, data.Length);
                if (br != fs.Length)
                    throw new System.IO.IOException(s);
                return data;
            }

        }

        [HttpPost]
        public JsonResult GenerarReporteComisionesPendientes()
        {

            var UsuarioActual = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);
            var parametros = new List<KeyValuePair<string, object>>();
            FileStreamResult reporte = null;

            string ahora = string.Empty;
            string handle = string.Empty;
            string nonbrerte = string.Empty;

            try
            {

                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var user = UserClaims.GetCurrentUserClaims((ClaimsIdentity)User.Identity);
                //var log = new EmbLogActividadesDto
                //{
                //    UsuarioId = Int32.Parse(user.UserId),
                //    fecha = DateTime.Now,
                //    idTipoLog = 10,//6,
                //    ip = ip,
                //    MenuId = 9
                //};

                //ServicioAplicacionLogs.AgregarLog(log);

                parametros.Add(new KeyValuePair<string, object>("Funcionario", user.Document));
                parametros.Add(new KeyValuePair<string, object>("COD_ESCA", 1));                


                reporte = this.Report(
                    ReportFormat.Excel,
                    ConfiguracionesGlobales.ReportPathAfiliacionesComisionesPendientesAsesor,
                    ConfiguracionesGlobales.ReportesReportServerUrl,
                    parametros,
                    ConfiguracionesGlobales.ReportesUsername,
                    ConfiguracionesGlobales.ReportesPassword
                    );

                nonbrerte = string.Format("ReporteComisionesPendientes-" + DateTime.Now.ToShortDateString() + ".xls");

                ahora = DateTime.Now.ToString("s");
                handle = Guid.NewGuid().ToString();

                TempData[handle] = ((MemoryStream)reporte.FileStream).ToArray();


                return Json(new { FileGuid = handle, FileName = nonbrerte }, JsonRequestBehavior.AllowGet);
                //return File(((MemoryStream)reporte.FileStream).ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, nonbrerte);

            }
            catch (Exception exception)
            {
                return Json(new
                {
                    msgError = string.Format("error"),
                    msgErrorException = $"{exception.Source}</br>{exception.Message}</br>{exception.StackTrace}"
                }, JsonRequestBehavior.AllowGet);
                //ViewBag.Error = "Puede que el archivo no exista o sea un problema con tu conexión. Intentalo nuevamente, si el problema persiste comunicate con el área de TI.";
                //ViewBag.rol = UserClaims.RolesAsesor.First();
                //return View("Index", (Session["ComoVoy"] as IEnumerable<ComoVoyDto>).ToList());
            }
        }
    
        public ActionResult Emergente(string title, string mess)
        {
            MessageModel em = new MessageModel() { Titulo = title, Cuerpo = mess, aceptar = "Aceptar" };
            return PartialView("_EmergenteMes", em);
        }

    }
}
