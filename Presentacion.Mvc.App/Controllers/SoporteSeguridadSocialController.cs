using Aplicacion.Administracion.Contratos;

//using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Presentacion.Mvc.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Transversales.Administracion.IoC;

namespace Presentacion.Mvc.App.Controllers
{
    public class SoporteSeguridadSocialController : Controller
    {
        private IServicioAplicacionMotivoSegSocial _servicioAplicacionMotivos;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private readonly string MENSAJE = "Hay un problema enviando el mensaje.";
        private readonly string MENSAJEENVIADO = "Mensaje enviado.";
        private readonly string MENSAJEGENERAL = "Intentalo nuevamente, si el problema persiste comunicate con el árear de tecnología.";
        private readonly string ERROEGETMOTIVOS = "Error consultando la lista de motivos.";

        //private string script = "";

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        private IServicioAplicacionMotivoSegSocial ServicioAplicacionMotivoSegSocial
        {
            get
            {
                return _servicioAplicacionMotivos ??
                       (_servicioAplicacionMotivos =
                           FabricaIoC.Resolver<IServicioAplicacionMotivoSegSocial>());
            }
        }

        // GET: SoporteSeguridadSocial
        public ActionResult Index(string mensaje)
        {
            var model = new SoportesSeguridadSocial();

            if (!string.IsNullOrEmpty(mensaje))
            {
                ViewBag.Message = mensaje;
            }

            try
            {
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
                        idTipoLog = (int)Consulta.SoportesSeguridadSocial,
                        ip = ip,
                        MenuId = (int)Menus.Soportes_seguridad_social,
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
                        idTipoLog = (int)Consulta.SoportesSeguridadSocial,
                        ip = ip,
                        MenuId = (int)Menus.Soportes_seguridad_social
                    };

                    ServicioAplicacionLogs.AgregarLog(log);
                }
                //

                var list = ServicioAplicacionMotivoSegSocial.MotivosSoporteSegSocial();
                ViewBag.Motivos = new SelectList(list.OrderBy(g => g.NOMBRE_MOTIVO), "ID", "NOMBRE_MOTIVO");

                Session["NombresMotivo"] = list;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                ViewBag.Message = string.Format("{0} {1}", ERROEGETMOTIVOS, MENSAJEGENERAL); ;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(SoportesSeguridadSocial model)
        {
            try
            {
                var motivo = new MotivoSoporteSegSocialDto();
                var doc = string.Empty;
                var nombre = string.Empty;

                string nombreArchivo = string.Empty;

                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                motivo.EMAIL = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault().Replace(",", "");
                motivo.USUARIO = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

                doc = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
                nombre = claims.Where(c => c.Type == ClaimTypes.Actor).Select(c => c.Value).SingleOrDefault();

                var list = (IEnumerable<MotivoSoporteSegSocialDto>)Session["NombresMotivo"];

                motivo.NOMBRE_MOTIVO = list.First(s => s.ID == model.ID_MOTIVO).NOMBRE_MOTIVO;
                var sendTo = list.First(s => s.ID == model.ID_MOTIVO).EMAIL;

                if (ModelState.IsValid)
                {
                    string firma = Server.MapPath(ConfiguracionesGlobales.FirmaSoporteSeguridadSocialRuta);

                    var body = "<html>" +
                                "<head> </head> " +
                                "<body> " +
                                    "</div> " +
                                    "<h4>Buen día, Se envian datos adjuntos de soportes de pagos</h4> " +
                                    "<br />" + model.OBSERVACION +
                                    "<br /><br />  " +
                                    //" <br /> Ingreso:  <a href=  \"http://portalempresas.emermedica.com.co/Login\" target=\"_blank\" style=\"color:blue\"> Portal Empresarial </a><br />" +
                                    " Le informamos este es un correo de envío automático. Agradecemos no responder a este email.<br />" +
                                    "<img style='width: 40%; height: 20%;' src=\"cid:Firma\">" +
                                "</body> " +
                            "</html>";

                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                    LinkedResource pic1 = new LinkedResource(firma, MediaTypeNames.Image.Jpeg);
                    pic1.ContentId = "Firma";
                    avHtml.LinkedResources.Add(pic1);

                    var message = new MailMessage();
                    message.AlternateViews.Add(avHtml);
                    message.To.Add(new MailAddress(sendTo));
                    message.Subject = string.Format("{0} [{1}] {2}", motivo.NOMBRE_MOTIVO, doc, nombre);
                    message.Body = string.Format(body, "emermedica", "portal_empresarial@emermedica.com.co", model.OBSERVACION);
                    message.IsBodyHtml = true;

                    HttpFileCollectionBase files = Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            message.Attachments.Add(new Attachment(file.InputStream, Path.GetFileName(file.FileName)));
                            nombreArchivo += file.FileName + ";";
                        }
                    }

                    IEnumerable<MotivoSoporteSegSocialDto> lst = null;

                    if (Session["NombresMotivo"] != null)
                    {
                        lst = Session["NombresMotivo"] as IEnumerable<MotivoSoporteSegSocialDto>;
                        ViewBag.Motivos = new SelectList(lst.OrderBy(g => g.NOMBRE_MOTIVO), "ID", "NOMBRE_MOTIVO");
                    }
                    else
                    {
                        lst = ServicioAplicacionMotivoSegSocial.MotivosSoporteSegSocial();
                        ViewBag.Motivos = new SelectList(lst.OrderBy(g => g.NOMBRE_MOTIVO), "ID", "NOMBRE_MOTIVO");
                    }

                    using (var smtp = new SmtpClient())
                    {
                        smtp.Send(message);

                        motivo.ARCHIVOSTXT = nombreArchivo;
                        motivo.ID_MOTIVO = model.ID_MOTIVO;
                        motivo.OBSERVACION = model.OBSERVACION;
                        ServicioAplicacionMotivoSegSocial.AgregarLog(motivo);

                        return RedirectToAction("Index", new { mensaje = MENSAJEENVIADO });
                    }
                }

                return View("Index");
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return RedirectToAction("Index", new { mensaje = string.Format("{0} {1}", MENSAJE, MENSAJEGENERAL) });
            }
        }
    }
}