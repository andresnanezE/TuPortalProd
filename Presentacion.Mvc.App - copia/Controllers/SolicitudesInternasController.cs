using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion.IoC;
using WebGrease.Css.Extensions;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;

namespace Presentacion.Mvc.App.Controllers
{
    /// <summary>
    /// John Nelson Rodriguez:
    /// Manejo de tickets a ttraves de WebClient
    /// realizando solicitudes al API REst para
    /// https://emermedica.freshservice.com
    /// </summary>
    public class SolicitudesInternasController : Controller
    {

        private SolicitudesInternas solInternas;

        private readonly string domain = ConfiguracionesGlobales.domainFresh;
        private readonly string token = ConfiguracionesGlobales.tokenUser;
        private readonly string account_id = ConfiguracionesGlobales.accountId;
        private readonly string subject = ConfiguracionesGlobales.Subject;

        private readonly string VALORESNOVALIDOS = "Los valores para los campos solicitados no son valudos.";
        private readonly string TICKETCREADO = "El ticket se ha creado con éxito.";
        private readonly string NOTACREADA = "La nota se ha creado con éxito.";
        private readonly string ERRORCREANDOTICKET = "Ha ocurrido algo inesperado creando el ticket.";
        private readonly string ERRORRECUPERANDOTICKET = "Ha ocurrido algo inesperado recuperando información del ticket.";
        private readonly string ERRORRECUPERANDONOTAS = "Ha ocurrido algo inesperado recuperando información de las notas.";
        private readonly string ERRORCREANDONOTA = "Ha ocurrido algo inesperado creando la nota.";
        private readonly string ERROROBTENIENDOLISTATICKETS = "Ha ocurrido algo inesperado recuperando el listado.";
        private readonly string ERRORRECUPERANDODATOSINICIALES = "Ha ocurrido algo inesperado recuperando valores iniciales.";
        private readonly string MENSAJEGENERAL = "Intentalo nuevamente, si el problema persiste comunicate con el área de TI.";
        private readonly string ERRORCREANDOTICKETENBD = "El ticket se ha creado con éxito, pero ocurrio un problema generando el registro en EMERMEDICA.";


        private IServicioAplicacionSolicitudesInternas _servicioAplicacionSolicitudesInternas;
        private IServicioAplicacionLogs _servicioAplicacionLogs;
        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }
        private IServicioAplicacionSolicitudesInternas ServicioAplicacionSolicitudesInternas
        {
            get
            {
                return _servicioAplicacionSolicitudesInternas ??
                       (_servicioAplicacionSolicitudesInternas =
                           FabricaIoC.Resolver<IServicioAplicacionSolicitudesInternas>());
            }
        }


        // 
        /// <summary>
        /// GET: SolicitudesInternas 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                solInternas = new SolicitudesInternas();
                
                //agrega actividad log
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var identity = (ClaimsIdentity)HttpContext.User.Identity;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.SolicitudesEnviar;
                log.ip = ip;
                log.MenuId = (int)Menus.Solicitudes_Internas;

                ServicioAplicacionLogs.AgregarLog(log);
                //
                return View(solInternas);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, ERRORRECUPERANDODATOSINICIALES);
                return View(solInternas);
            }
        }

        /// <summary>
        /// Enviar solicitud.
        /// </summary>
        /// <param name="model">SolicitudesInternas</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult Index(SolicitudesInternas model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", VALORESNOVALIDOS);

                return View(model);
            }

            string email = string.IsNullOrEmpty(ConfiguracionesGlobales.SolicitudesInternasEmailPruebas)
            ? GetUserInfo().Item1 : ConfiguracionesGlobales.SolicitudesInternasEmailPruebas;

            decimal documento = Convert.ToDecimal(GetUserInfo().Item2);

            // Crear un mensaje
            string ticketCreado = string.Empty;

            // Crear una marca:
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            // Web Request:
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(domain + "/helpdesk/tickets/[id].json");

            wr.Headers.Clear();

            // Method y headers:
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;

            // Auth:
            string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token + ":X"));
            wr.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            // Body:
            using (var rs = wr.GetRequestStream())
            {
                try
                {
                    string content = string.Empty;
                    byte[] data = new byte[0];

                    // Email:
                    writeBoundaryBytes(rs, boundary, false);
                    writeContentDispositionFormDataHeader(rs, "helpdesk_ticket[email]");
                    writeString(rs, email);
                    writeCRLF(rs);

                    // Subject:
                    writeBoundaryBytes(rs, boundary, false);
                    writeContentDispositionFormDataHeader(rs, "helpdesk_ticket[subject]");
                    writeString(rs, string.Format("{0}-[{1}]", model.Asunto, subject));
                    writeCRLF(rs);

                    // Description:
                    writeBoundaryBytes(rs, boundary, false);
                    writeContentDispositionFormDataHeader(rs, "helpdesk_ticket[description]");
                    writeString(rs, model.Descripcion);
                    writeCRLF(rs);

                    //Ciudad:
                    string ciudad = model.CiudadesItems.Single(c => c.Value == model.Ciudad.idCiudad.ToString()).Text;
                    writeBoundaryBytes(rs, boundary, false);
                    writeContentDispositionFormDataHeader(rs, string.Format("helpdesk_ticket[custom_field][ciudad_{0}]", account_id));
                    writeString(rs, ciudad);
                    writeCRLF(rs);

                    //Area:
                    string area = model.AreasItems.Single(a => a.Value == model.Area.idArea.ToString()).Text;
                    writeBoundaryBytes(rs, boundary, false);
                    writeContentDispositionFormDataHeader(rs, string.Format("helpdesk_ticket[custom_field][area_{0}]", account_id));
                    writeString(rs, area);
                    writeCRLF(rs);

                    //TRequerimiento:
                    string tipo =
                        model.TRequerimientosItems.Single(
                            r => r.Value == model.TipoRequerimiento.idTRequerimiento.ToString()).Text;
                    writeBoundaryBytes(rs, boundary, false);
                    writeContentDispositionFormDataHeader(rs, string.Format("helpdesk_ticket[custom_field][trequerimiento_{0}]", account_id));
                    writeString(rs, tipo);
                    writeCRLF(rs);


                    //Attach:
                    HttpFileCollectionBase files = Request.Files;
                    string fileName = string.Empty;
                    if (files.Count > 0)
                    {

                        for (int i = 0; i < files.Count; i++)
                        {

                            HttpPostedFileBase file = files[i];


                            if (string.IsNullOrEmpty(file.FileName))
                            {
                                continue;
                            }

                            fileName += file.FileName + ";";
                            string extension = Path.GetExtension(fileName);
                            string contentType = file.ContentType;

                            writeBoundaryBytes(rs, boundary, false);
                            writeContentDispositionFileHeader(rs, "helpdesk_ticket[attachments][][resource]", fileName, contentType);
                            //writeContentDispositionFileHeader(rs, "helpdesk_note[attachments][][resource]", fileName, contentType);
                            var stream = file.InputStream;
                            data = new byte[stream.Length];
                            int br = stream.Read(data, 0, data.Length);
                            if (br != stream.Length)
                                throw new System.IO.IOException(fileName);

                            rs.Write(data, 0, data.Length);
                            writeCRLF(rs);
                        }
                    }


                    // Fin de la marca:
                    writeBoundaryBytes(rs, boundary, true);

                    rs.Close();


                    // Response :
                    using (WebResponse w = wr.GetResponse())
                    {
                        using (var inStream = w.GetResponseStream())
                        {
                            using (var reader = new StreamReader(inStream, Encoding.UTF8))
                            {
                                content = reader.ReadToEnd();
                            }
                        }
                    }

                    var ser = new JavaScriptSerializer();
                    var records = ser.Deserialize<dynamic>(content);

                    ticketCreado = TICKETCREADO;


                    //Agregar el ticket a base de datos:


                    var solicitud = new SolicitudesInternasDto()
                    {
                        area = area,
                        asunto = model.Asunto,
                        ciudad = ciudad,
                        cod_ases = documento,
                        estado = null,
                        fecha_cierre = null,
                        fecha_ini = DateTime.Now,
                        tipo_requerimiento = tipo,
                        Descripcion = model.Descripcion,
                        content_file_name = fileName,
                        nombre_archivo = fileName,
                        id_solicitud = records["item"]["helpdesk_ticket"]["display_id"].ToString()
                    };


                    if (!string.IsNullOrEmpty(fileName))
                    {
                        for (int i = 0; i < fileName.Split(';').Count() - 1; ++i)
                        {
                            solicitud.url_archivo += records["item"]["helpdesk_ticket"]["attachments"][i]["attachment_url"] + ";";
                        }
                    }


                    ServicioAplicacionSolicitudesInternas.AgregarSolicitusInterna(solicitud);


                    ViewBag.Message = TICKETCREADO;

                    //agrega actividad log
                    var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                    var identity = (ClaimsIdentity)HttpContext.User.Identity;
                    var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                    var log = new EmbLogActividadesDto();
                    log.UsuarioId = usuarioId;
                    log.fecha = DateTime.Now;
                    log.idTipoLog = (int)Consulta.SolicitudesEnviarBoton;
                    log.ip = ip;
                    log.MenuId = (int)Menus.Solicitudes_Internas;

                    ServicioAplicacionLogs.AgregarLog(log);
                    //

                    return View(model);
                }
                catch (Exception ex)
                {
                    if (!string.IsNullOrEmpty(ticketCreado))
                    {
                        ModelState.AddModelError(string.Empty, ERRORCREANDOTICKETENBD);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, string.Format("{0}. {1}", ERRORCREANDOTICKET, MENSAJEGENERAL));
                    }


                    return View(model);
                }

            }

        }

        /// <summary>
        /// Agregar una nota al ticket.
        /// jOH nELSON rODRIGUEZ GARZON
        /// </summary>
        /// <param name="model">SolicitudesInternasNotas</param>
        /// <returns>ActionResult</returns>
        public ActionResult AddNoteToTicket(SolicitudesInternasNotas model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", VALORESNOVALIDOS);

                return RedirectToAction("DetalleSolicitudes", new { msg = ERRORCREANDONOTA });
            }

            string email = string.IsNullOrEmpty(ConfiguracionesGlobales.SolicitudesInternasEmailPruebas)
           ? GetUserInfo().Item1 : ConfiguracionesGlobales.SolicitudesInternasEmailPruebas;

            decimal documento = Convert.ToDecimal(GetUserInfo().Item2);

            // Boundary:
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            // Web Request:
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(string.Format("{0}/helpdesk/tickets/{1}/conversations/note.json", domain, model.IdTicket));

            wr.Headers.Clear();

            // Method and headers:
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;

            // Basic auth:
            string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token + ":X"));
            wr.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            // Body:
            using (var rs = wr.GetRequestStream())
            {
                try
                {
                    string content = string.Empty;
                    byte[] data = new byte[0];

                    // Email:
                    writeBoundaryBytes(rs, boundary, false);
                    writeContentDispositionFormDataHeader(rs, "helpdesk_ticket[email]");
                    writeString(rs, email);
                    writeCRLF(rs);

                    // Is Private:
                    writeBoundaryBytes(rs, boundary, false);
                    writeContentDispositionFormDataHeader(rs, "helpdesk_note[private]");
                    writeString(rs, model.EsPrivada.ToString());
                    writeCRLF(rs);

                    // Note:
                    writeBoundaryBytes(rs, boundary, false);
                    writeContentDispositionFormDataHeader(rs, "helpdesk_note[body]");
                    writeString(rs, model.Nota);
                    writeCRLF(rs);


                    //// Attachment:
                    //string fileName = string.Empty;
                    //if (model.Attach != null && model.Attach.Any())
                    //{
                    //    if (model.Attach.ToArray()[0] != null)
                    //    {
                    //        fileName = model.Attach.ToArray()[0].FileName;
                    //        string extension = Path.GetExtension(fileName);
                    //        string contentType = model.Attach.ToArray()[0].ContentType;


                    //        writeBoundaryBytes(rs, boundary, false);
                    //        writeContentDispositionFileHeader(rs, "helpdesk_note[attachments][][resource]", fileName, contentType);

                    //        var stream = model.Attach.ToArray()[0].InputStream;
                    //        data = new byte[stream.Length];
                    //        int br = stream.Read(data, 0, data.Length);
                    //        if (br != stream.Length)
                    //            throw new System.IO.IOException(fileName);

                    //        rs.Write(data, 0, data.Length);
                    //        writeCRLF(rs);
                    //    }

                    //Attach:
                    HttpFileCollectionBase files = Request.Files;
                    string fileName = string.Empty;
                    if (files.Count > 0)
                    {

                        for (int i = 0; i < files.Count; i++)
                        {

                            HttpPostedFileBase file = files[i];


                            if (string.IsNullOrEmpty(file.FileName))
                            {
                                continue;
                            }

                            fileName += file.FileName + ";";
                            string extension = Path.GetExtension(fileName);
                            string contentType = file.ContentType;

                            writeBoundaryBytes(rs, boundary, false);
                            writeContentDispositionFileHeader(rs, "helpdesk_note[attachments][][resource]", fileName, contentType);
                            var stream = file.InputStream;
                            data = new byte[stream.Length];
                            int br = stream.Read(data, 0, data.Length);
                            if (br != stream.Length)
                                throw new System.IO.IOException(fileName);

                            rs.Write(data, 0, data.Length);
                            writeCRLF(rs);
                        }
                    }

                    // marca final:
                    writeBoundaryBytes(rs, boundary, true);

                    rs.Close();

                    // Response :

                    using (WebResponse w = wr.GetResponse())
                    {
                        using (var inStream = w.GetResponseStream())
                        {
                            using (var reader = new StreamReader(inStream, Encoding.UTF8))
                            {
                                content = reader.ReadToEnd();
                            }
                        }
                    }

                    var ser = new JavaScriptSerializer();
                    var records = ser.Deserialize<dynamic>(content);


                    //Agregar la nota a base de datos:

                    var nota = new SolicitudesInternasNotasDto()
                    {
                        Nota = model.Nota, //records["note"]["body"]
                        nombre_archivo = fileName, // records["note"]["attachments"][0]["content_file_name"]
                        url_archivo = records["note"]["attachments"].Length > 0 ? records["note"]["attachments"][0]["attachment_url"] : "",
                        IdNota = records["note"]["id"].ToString(),
                        archivo = data,
                        IdTicket = model.IdTicket,
                        Fecha = DateTime.Now
                    };
                    ServicioAplicacionSolicitudesInternas.AgregarNota(nota);

                    return RedirectToAction("DetalleSolicitudes", new { msg = NOTACREADA });

                }
                catch (Exception exception)
                {

                    return RedirectToAction("DetalleSolicitudes", new { msg = ERRORCREANDONOTA });
                }

            }


        }


        /// <summary>
        /// Recuperar la lista de tickets filtrada por email del usuario actual.
        /// </summary>
        /// <param name="msg">Mensaje ha desplegar en las vista, se asigna al
        /// modelo ListaSolicitudesViewModel.Mensaje</param>
        /// <returns>ActionResult</returns>
        public ActionResult DetalleSolicitudes(string msg)
        {
            string post = string.Empty;
            string content = string.Empty;

            var solicitudesModel = new ListaSolicitudesViewModel();

            try
            {
                string email = string.IsNullOrEmpty(ConfiguracionesGlobales.SolicitudesInternasEmailPruebas)
                ? GetUserInfo().Item1 : ConfiguracionesGlobales.SolicitudesInternasEmailPruebas;

                decimal documento = Convert.ToDecimal(GetUserInfo().Item2);

                post = "/helpdesk/tickets.json?email=[email]&filter_name=all_tickets";


                post = post.Replace("[email]", email);
                var request = WebRequest.Create(string.Format("{0}{1}", domain, post));

                request.Method = "GET";
                request.ContentType = "application/json; charset=UTF-8";
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(token)));

                using (WebResponse wr = request.GetResponse())
                {
                    using (var inStream = wr.GetResponseStream())
                    {
                        using (var reader = new StreamReader(inStream, Encoding.UTF8))
                        {
                            content = reader.ReadToEnd();
                        }
                    }
                }

                //Para recuperar la sulicitudes almacenadas en PortalComercial_Pruebas..EME_SOLICITUDES_INTERNAS
                var solicitudes = new List<SolicitudesInternasDto>();

                try
                {
                    solicitudes = ServicioAplicacionSolicitudesInternas.ObtenerSolicitudesInternas(documento).ToList();
                }
                catch (Exception e)
                {
                    //Si ocurre una exception aqui, no afecta el resto del proceso.
                    Console.Write(e.Message);
                }

                // Deserializar el JSON regresado por el API.

                var ser = new JavaScriptSerializer();
                var records = ser.Deserialize<List<item>>(content);

                if (records.Any())
                {
                    foreach (var r in records.Where(s => string.IsNullOrEmpty(s.attachment_url)))
                    {
                        var match = solicitudes.FirstOrDefault(s => s.id_solicitud == r.display_id);
                        if (match != null)
                        {
                            r.attachment_url = match.url_archivo;
                            r.content_file_name = match.nombre_archivo;
                        }
                    }
                }
                solicitudesModel.Solicitudes = records;
                solicitudesModel.Mensaje = string.IsNullOrEmpty(msg) ? "" : Server.HtmlEncode(msg);
                
                //agrega actividad log
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var identity = (ClaimsIdentity)HttpContext.User.Identity;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)Consulta.AyudaManualUsuario;
                log.ip = ip;
                log.MenuId = (int)Menus.Manual_usuario;

                ServicioAplicacionLogs.AgregarLog(log);
                //

                return View(solicitudesModel);

            }
            catch (Exception e)
            {

                solicitudesModel.Solicitudes = new List<item>();
                solicitudesModel.Mensaje = string.Format("{0}. {1}", ERROROBTENIENDOLISTATICKETS, MENSAJEGENERAL);
                return View(solicitudesModel);
            }
        }

        private List<item> ViewTicket(string idTicket)
        {
            string post = string.Empty;
            string content = string.Empty;

            var solicitudesModel = new ListaSolicitudesViewModel();

            try
            {
                string email = string.IsNullOrEmpty(ConfiguracionesGlobales.SolicitudesInternasEmailPruebas)
                ? GetUserInfo().Item1 : ConfiguracionesGlobales.SolicitudesInternasEmailPruebas;

                decimal documento = Convert.ToDecimal(GetUserInfo().Item2);

                post = "/helpdesk/tickets/[id].json";

                post = post.Replace("[id]", idTicket);
                var request = WebRequest.Create(string.Format("{0}{1}", domain, post));

                request.Method = "GET";
                request.ContentType = "application/json; charset=UTF-8";
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(token)));

                using (WebResponse wr = request.GetResponse())
                {
                    using (var inStream = wr.GetResponseStream())
                    {
                        using (var reader = new StreamReader(inStream, Encoding.UTF8))
                        {
                            content = reader.ReadToEnd();
                        }
                    }
                }

                var ser = new JavaScriptSerializer();
                var records = ser.Deserialize<dynamic>(content);
                var listaNotas = new List<item>();

                //foreach (var item in records) records["helpdesk_ticket"]["notes"]
                foreach (var nota in records["helpdesk_ticket"]["notes"])
                {
                    //var i = new item() {
                    //    url_attach = nota["note"]["attachments"][0].Count > 0 ? nota["note"]["attachments"][0]["attachment_url"] : "{}",
                    //    content_file_name = nota["note"]["attachments"][0]["content_file_name"],
                    //    created_at = DateTime.Parse(nota["note"]["created_at"]),
                    //    body = nota["note"]["body"]
                    //};

                    var i = new item();

                    try
                    {
                        i.created_at = DateTime.Parse(nota["note"]["created_at"]);
                        i.body = nota["note"]["body"];
                        i.url_attach = nota["note"]["attachments"][0]["attachment_url"];
                        i.content_file_name = nota["note"]["attachments"][0]["content_file_name"];

                    }
                    catch (Exception ex)
                    {
                        Console.Write(i.url_attach);
                    }
                    finally
                    {
                        listaNotas.Add(i);
                    }



                }

                listaNotas.Reverse();
                return listaNotas;

            }
            catch (Exception ex)
            {

                solicitudesModel.Solicitudes = new List<item>();
                solicitudesModel.Mensaje = string.Format("{0}. {1}", ERRORRECUPERANDOTICKET, MENSAJEGENERAL);
                //return View(solicitudesModel);
                return new List<item>();
            }

        }

        /// <summary>
        /// Obtiene las notas creadas filtradas por el idTicket
        /// </summary>
        /// <param name="idTicket">Filtro para recuperar las notas.</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GetNotes(int idTicket)
        {
            try
            {
                return Json(ViewTicket(idTicket.ToString()));
                //return Json(ServicioAplicacionSolicitudesInternas.ObtenerNotas(idTicket));
            }
            catch (Exception ex)
            {
                return Json("{\"msgError\":\"" + string.Format("{0}. {1}", ERRORRECUPERANDONOTAS, MENSAJEGENERAL) + "}");
            }
        }

        /// <summary>
        /// Recupera el email del usuario actual.
        /// </summary>
        /// <returns>string</returns>
        private Tuple<string, decimal> GetUserInfo()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var email = string.Empty;
            decimal doc = 0M;

            try
            {
                IEnumerable<Claim> claims = identity.Claims;

                email = claims.Where(c => c.Type == ClaimTypes.Email)
                    .Select(c => c.Value)
                    .SingleOrDefault()
                    .Replace(",", "");

                doc = decimal.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value)
                   .SingleOrDefault());

                return new Tuple<string, decimal>(email, doc);
            }
            catch (Exception)
            {

                throw;
            }


        }
        private static void writeCRLF(Stream o)
        {
            byte[] crLf = Encoding.UTF8.GetBytes("\r\n");
            o.Write(crLf, 0, crLf.Length);
        }

        /// <summary>
        /// Escribe un separador en el header del request.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="b"></param>
        /// <param name="isFinalBoundary"></param>
        private static void writeBoundaryBytes(Stream o, string b, bool isFinalBoundary)
        {
            string boundary = isFinalBoundary == true ? "--" + b + "--" : "--" + b + "\r\n";
            byte[] d = Encoding.UTF8.GetBytes(boundary);
            o.Write(d, 0, d.Length);
        }

        /// <summary>
        /// Escribel el cuerpo del header del request.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        private static void writeContentDispositionFormDataHeader(Stream o, string name)
        {
            string data = "Content-Disposition: form-data; name=\"" + name + "\"\r\n\r\n";
            byte[] b = Encoding.UTF8.GetBytes(data);
            o.Write(b, 0, b.Length);
        }

        /// <summary>
        /// Escribe el adjunto en el request.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        private static void writeContentDispositionFileHeader(Stream o, string name, string fileName, string contentType)
        {

            //text/plain
            //image/png, image/jpeg
            //application/pdf

            string data = "Content-Disposition: form-data; name=\"" + name + "\"; filename=\"" + fileName + "\"\r\n";
            data += "Content-Type: " + contentType + "\r\n\r\n";
            byte[] b = Encoding.UTF8.GetBytes(data);
            o.Write(b, 0, b.Length);
        }

        /// <summary>
        /// Escribe el contenido del request.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="data"></param>
        private static void writeString(Stream o, string data)
        {
            byte[] b = Encoding.UTF8.GetBytes(data);
            o.Write(b, 0, b.Length);
        }



    }
}
