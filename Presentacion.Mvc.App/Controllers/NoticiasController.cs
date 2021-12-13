using Aplicacion.Administracion.Contratos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;
using Dominio.Administracion.Entidades.MapperDto;
using Dominio.Administracion.Entidades.MapperDto.DtoProcesos;
using Presentacion.Mvc.App.Helpers;
using Presentacion.Mvc.App.Models;
using Presentacion.Mvc.App.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Transversales.Administracion.IoC;
using Utilidades.Configuration;
using Utilidades.Enums;
using Utilidades.ImageManager;

namespace Presentacion.Mvc.App.Controllers
{
    public class NoticiasController : Controller
    {
        #region Fields

        private IServicioAplicacionNoticia _servicioAplicacionNoticia;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion Fields

        #region Instance Properties

        private IServicioAplicacionNoticia ServicioAplicacionNoticia
        {
            get { return _servicioAplicacionNoticia ?? (_servicioAplicacionNoticia = FabricaIoC.Resolver<IServicioAplicacionNoticia>()); }
        }

        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        #endregion Instance Properties

        #region Admin

        /// <summary>
        /// Administrador de noticias
        /// </summary>
        /// <returns></returns>
        public ActionResult ListaAdmin()
        {
            //actualiza log actividades
            var identity = (ClaimsIdentity)User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var log = new EmbLogActividadesDto
            {
                UsuarioId = usuarioIdentity,
                fecha = DateTime.Now,
                idTipoLog = (int)Administracion.Noticias,
                ip = ip,
                MenuId = (int)Menus.Noticias
            };
            ServicioAplicacionLogs.AgregarLog(log);
            //
            return View();
        }

        /// <summary>
        /// Pagina para gregar o editar una noticia
        /// </summary>
        /// <param name="id">Identificador de la noticia</param>
        /// <returns></returns>
        public ActionResult Editar(Guid? id)
        {
            //actualiza log actividades
            var identity = (ClaimsIdentity)User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);
            var tipoLog = id.HasValue ? TipoLog.EditarNoticia : TipoLog.NuevaNoticia;

            var log = new EmbLogActividadesDto
            {
                UsuarioId = usuarioIdentity,
                fecha = DateTime.Now,
                idTipoLog = (int)tipoLog,
                ip = ip,
                MenuId = (int)Menus.Noticias
            };
            ServicioAplicacionLogs.AgregarLog(log);
            //

            return View(id);
        }

        /// <summary>
        /// Elimina una noticia
        /// </summary>
        [HttpPost]
        public JsonResult EliminarNoticia(Guid id)
        {
            try
            {
                ServicioAplicacionNoticia.EliminarNoticia(id);

                //Guarda el log
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var identity = (ClaimsIdentity)User.Identity;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)TipoLog.EliminarNoticia;
                log.ip = ip;
                log.MenuId = (int)Menus.Noticias;
                ServicioAplicacionLogs.AgregarLog(log);

                return Json(new { Success = true });
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return Json(new { Success = false });
            }
        }

        /// <summary>
        /// Sube una imagen temporal de un destacado
        /// </summary>
        /// <param name="fileKey">Llave del destacado</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubirImagen(string fileKey, ImageType imageType)
        {
            try
            {
                var file = this.Request.Files[fileKey];
                var filePath = FileUploaderHelper.UploadFile(file);
                IResourceManager appSettings = new AppSettings();

                var imageSize = new ImageSize();
                if (imageType == ImageType.NoticiaImagen)
                {
                    imageSize.Width = 263;
                    imageSize.Height = 200;
                }
                else
                {
                    imageSize.Width = 1140;
                    imageSize.Height = 312;
                }

                var imageName = LocalImageService.GenerateImage(filePath, imageSize, FileUploaderHelper.UploadTempFolder);
                var imagePath = FileUploaderHelper.UploadTempFolder + imageName;

                var fi = new FileInfo(filePath);
                var mimetype = MimeTypeHelper.GetMimeTypeFromExtension(fi.Extension);
                var prefix = String.Format("data:{0};base64,", mimetype);
                var original = prefix + Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath));
                var preview = prefix + Convert.ToBase64String(System.IO.File.ReadAllBytes(imagePath));

                //Delete files
                //try
                //{
                //    System.IO.File.Delete(filePath);
                //}
                //catch
                //{
                //}

                //try
                //{
                //    System.IO.File.Delete(imagePath);
                //}
                //catch
                //{
                //}

                var model = new { Success = true, Original = original, Preview = preview, Key = fileKey };
                var result = Json(model, "text/plain");

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var output = serializer.Serialize(model);
                result.MaxJsonLength = output.Length;

                return result;
            }
            catch (Exception ex)
            {
                var t = ex.Message;
                var model = new { Success = false };
                return Request.IsAjaxRequest() ? Json(model) : new FileJsonResult(model);
            }
        }

        /// <summary>
        /// Saves the Board Message
        /// </summary>
        [HttpPost]
        public JsonResult GuardarNoticia(Guid? id, string titulo, string descripcion, string contenido, DateTime fecha, bool activa, string imagenRaw, string bannerRaw)
        {
            var imagePath = "";
            var bannerPath = "";

            #region Imagen

            if (!String.IsNullOrEmpty(imagenRaw))
            {
                var iId = Guid.NewGuid();
                var ipath = FileUploaderHelper.UploadTempFolder;
                var imparts = imagenRaw.Split(',');
                if (imparts.Length > 1)
                {
                    var mimetype = imparts[0].Split(':')[1].Split(';')[0];
                    var extension = MimeTypeHelper.GetDefaultExtension(mimetype);
                    var buffer = Convert.FromBase64String(imparts[1]);
                    var file = ipath + iId.ToString() + extension;
                    System.IO.File.WriteAllBytes(file, buffer);

                    imagePath = file;
                }
            }

            #endregion Imagen

            #region Banner

            if (!String.IsNullOrEmpty(bannerRaw))
            {
                var iId = Guid.NewGuid();
                var ipath = FileUploaderHelper.UploadTempFolder;
                var imparts = bannerRaw.Split(',');
                if (imparts.Length > 1)
                {
                    var mimetype = imparts[0].Split(':')[1].Split(';')[0];
                    var extension = MimeTypeHelper.GetDefaultExtension(mimetype);
                    var buffer = Convert.FromBase64String(imparts[1]);
                    var file = ipath + iId.ToString() + extension;
                    System.IO.File.WriteAllBytes(file, buffer);

                    bannerPath = file;
                }
            }

            #endregion Banner

            DYK_NOTICIADto noticia = new DYK_NOTICIADto()
            {
                NOTICIAID = id.HasValue ? id.Value : Guid.NewGuid(),
                TITULO = titulo,
                DESCRIPCION = descripcion,
                FECHA = fecha,
                ACTIVO = activa,
                CONTENIDO = contenido
            };

            //Traer el usuario
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

            TipoLog tipoLog = TipoLog.NuevaNoticia;
            var virtualPath = ConfigurationManager.AppSettings["Assets.Uploads.Path"];
            var pathImagenDestino = virtualPath + ConfigurationManager.AppSettings["Imagen.Path.Noticias.Imagen"].Replace("/", "\\");
            var pathBannerDestino = virtualPath + ConfigurationManager.AppSettings["Imagen.Path.Noticias.Banner"].Replace("/", "\\");
            if (id.HasValue)
            {
                tipoLog = TipoLog.EditarNoticia;
                ServicioAplicacionNoticia.ModificarNoticia(noticia, imagePath, pathImagenDestino, bannerPath, pathBannerDestino);
            }
            else
            {
                ServicioAplicacionNoticia.AgregarNoticia(noticia, imagePath, pathImagenDestino, bannerPath, pathBannerDestino);
            }

            //Guarda el log
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var log = new EmbLogActividadesDto();
            log.UsuarioId = usuarioId;
            log.fecha = DateTime.Now;
            log.idTipoLog = (int)tipoLog;
            log.ip = ip;
            log.MenuId = (int)Menus.Noticias;
            ServicioAplicacionLogs.AgregarLog(log);

            return Json(new { Success = true });
        }

        #endregion Admin

        #region Front

        //
        // GET: /News/
        public ActionResult Index(string titulo)
        {
            var noticia = ServicioAplicacionNoticia.TraerNoticiaPorTituloQS(titulo);
            noticia.CONTENIDO = Server.HtmlDecode(noticia.CONTENIDO);

            return View(noticia);
        }

        //
        // GET: /News/
        public ActionResult Preview(Guid id)
        {
            var noticia = ServicioAplicacionNoticia.TraerNoticiaPorId(id);
            noticia.CONTENIDO = Server.HtmlDecode(noticia.CONTENIDO);

            return View(noticia);
        }

        /// <summary>
        /// Trae la lista de noticias
        /// </summary>
        /// <returns></returns>
        public ActionResult Lista()
        {
            return View();
        }

        //
        // GET: /News/
        public ActionResult Noticia1()
        {
            return View();
        }

        //
        // GET: /News/
        public ActionResult Noticia2()
        {
            return View();
        }

        //
        // GET: /News/
        public ActionResult Noticia3()
        {
            return View();
        }

        //
        // GET: /News/
        public ActionResult Noticia4()
        {
            return View();
        }

        #endregion Front

        #region Core

        /// <summary>
        /// Trae la lista de noticias paginadas
        /// </summary>
        /// <param name="soloActivas">Indica si solo trae las noticia activas</param>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="page">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TraerNoticias(bool soloActivas, int pageSize, int page, int take, int skip, string order)
        {
            if (string.IsNullOrWhiteSpace(order))
            {
                order = "FECHA";
            }

            var count = ServicioAplicacionNoticia.TraerNumeroNoticias(soloActivas);
            var noticias = ServicioAplicacionNoticia.TraerNoticias(soloActivas, pageSize, page, order);

            var model = new
            {
                data = noticias,
                total = count
            };

            return new JsonNetResult() { Data = model };
        }

        /// <summary>
        /// Trae una noticia
        /// </summary>
        /// <param name="soloActivas">Indica si solo trae las noticia activas</param>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="page">Número de página</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TraerNoticia(Guid id)
        {
            var noticia = ServicioAplicacionNoticia.TraerNoticiaPorId(id);
            noticia.CONTENIDO = Server.HtmlDecode(noticia.CONTENIDO);

            return new JsonNetResult() { Data = noticia };
        }

        #endregion Core

    }
}