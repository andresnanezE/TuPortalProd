// ----------------------------------------------------------------------------------------------
// <copyright file="MenuController.cs" company="SCI Software">
//     Copyright (c) SCI Software 2015. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using Aplicacion.Administracion.Contratos;
using Aplicacion.Administracion.Dto;
using AutoMapper;
using PagedList;
using Presentacion.Mvc.App.Models;
using Transversales.Administracion.IoC;
using Presentacion.Mvc.App.Utilidades;
using Presentacion.Mvc.App.Helpers;
using Utilidades.Enums;
using Utilidades.Configuration;
using Utilidades.ImageManager;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Security.Claims;
using Aplicacion.Administracion.Dto.DtoProcesos;
using Dominio.Administracion.Entidades.Enumeraciones.LogActividades;

namespace Presentacion.Mvc.App.Controllers
{
    public class DestacadosController : Controller
    {
        #region Fields

        private IServicioAplicacionBanner _servicioAplicacionBanner;
        private IServicioAplicacionDestacado _servicioAplicacionDestacado;
        private IServicioAplicacionLogs _servicioAplicacionLogs;

        #endregion

        #region Instance Properties

        private IServicioAplicacionBanner ServicioAplicacionBanner
        {
            get { return _servicioAplicacionBanner ?? (_servicioAplicacionBanner = FabricaIoC.Resolver<IServicioAplicacionBanner>()); }
        }
        private IServicioAplicacionDestacado ServicioAplicacionDestacado
        {
            get { return _servicioAplicacionDestacado ?? (_servicioAplicacionDestacado = FabricaIoC.Resolver<IServicioAplicacionDestacado>()); }
        }
        private IServicioAplicacionLogs ServicioAplicacionLogs
        {
            get { return _servicioAplicacionLogs ?? (_servicioAplicacionLogs = FabricaIoC.Resolver<IServicioAplicacionLogs>()); }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Acción por defecto
        /// </summary>
        /// <returns>Abre la vista para administrar menus y banners</returns>
        public ActionResult Index()
        {

            //actualiza log actividades
            var identity = (ClaimsIdentity)User.Identity;
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioIdentity = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var log = new EmbLogActividadesDto
            {
                UsuarioId = usuarioIdentity,
                fecha = DateTime.Now,
                idTipoLog = (int)Administracion.Destacados,
                ip = ip,
                MenuId = (int)Menus.Destacados
            };
            ServicioAplicacionLogs.AgregarLog(log);
            //
            return View();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ViewData["CurrentUser"] = requestContext.HttpContext.User.Identity.Name;
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
                var imageSizePreview = new ImageSize();
                if (imageType == ImageType.Destacado)
                {
                    imageSize.Width = 360;
                    imageSize.Height = 130;

                    imageSizePreview.Width = 150;
                    imageSizePreview.Height = 50;

                }
                else // Banner
                {
                    imageSize.Width = 1015;
                    imageSize.Height = 359;

                    imageSizePreview.Width = 350;
                    imageSizePreview.Height = 100;
                }

                //Original
                var imageName = LocalImageService.GenerateImage(filePath, imageSize, FileUploaderHelper.UploadTempFolder);
                var imagePath = FileUploaderHelper.UploadTempFolder + imageName;

                var fi = new FileInfo(filePath);
                var mimetype = MimeTypeHelper.GetMimeTypeFromExtension(fi.Extension);
                var prefix = String.Format("data:{0};base64,", mimetype);
                var original = prefix + Convert.ToBase64String(System.IO.File.ReadAllBytes(imagePath));

                //Preview
                var imageNamePreview = LocalImageService.GenerateImage(filePath, imageSizePreview, FileUploaderHelper.UploadTempFolder);
                var imagePathPreview = FileUploaderHelper.UploadTempFolder + imageNamePreview;

                var fiPreview = new FileInfo(filePath);
                var mimetypePreview = MimeTypeHelper.GetMimeTypeFromExtension(fiPreview.Extension);
                var prefixPreview = String.Format("data:{0};base64,", mimetypePreview);
                var preview = prefix + Convert.ToBase64String(System.IO.File.ReadAllBytes(imagePathPreview));

                //Delete files
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch
                {
                }

                try
                {
                    System.IO.File.Delete(imagePath);
                    System.IO.File.Delete(imagePathPreview);
                }
                catch
                {
                }

                var model = new { Success = true, Original = original, Preview = preview, Key = fileKey };
                var result = Json(model, "text/plain");

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var output = serializer.Serialize(model);
                result.MaxJsonLength = output.Length;

                return result;
            }
            catch (Exception)
            {
                var model = new { Success = false };
                return Request.IsAjaxRequest() ? Json(model) : new FileJsonResult(model);
            }
        }

        #region Destacados

        /// <summary>
        /// Trae la lista de destacados
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TraerDestacados()
        {
            try
            {
                var destacados = ServicioAplicacionDestacado.TraerDestacados();
                return new JsonNetResult() { Data = destacados };
            }
            catch (Exception ex)
            {
                return new JsonNetResult() { Data = null };
            }
        }

        /// <summary>
        /// Actualiza la información de los destacados
        /// </summary>
        /// <param name="url1">Url para actualizar el destacado 1</param>
        /// <param name="url2">Url para actualizar el destacado 2</param>
        /// <param name="url3">Url para actualizar el destacado 3</param>
        /// <param name="imageRaw1">Imagen del destacado 1</param>
        /// <param name="imageRaw2">Imagen del destacado 2</param>
        /// <param name="imageRaw3">Imagen del destacado 3</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarDestacados(string url1, string url2, string url3, string imageRaw1, string imageRaw2, string imageRaw3,
            bool abrirensitio1, bool abrirensitio2, bool abrirensitio3)
        {
            List<DYK_DESTACADODto> destacados = new List<DYK_DESTACADODto>();
            List<string> imagePaths = new List<string>();
            try
            {
                //Traer el usuario
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var userName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

                #region Destacado 1

                var imagePath1 = "";

                DYK_DESTACADODto destacado1 = new DYK_DESTACADODto();
                destacado1.DESTACADOID = 1;
                destacado1.URL = url1;
                destacado1.ABRIRENSITIO = abrirensitio1;

                if (!String.IsNullOrEmpty(imageRaw1))
                {
                    var iId = Guid.NewGuid(); 
                    var ipath = FileUploaderHelper.UploadTempFolder;
                    var imparts = imageRaw1.Split(',');
                    if (imparts.Length > 1)
                    {
                        var mimetype = imparts[0].Split(':')[1].Split(';')[0];
                        var extension = MimeTypeHelper.GetDefaultExtension(mimetype);
                        var buffer = Convert.FromBase64String(imparts[1]);
                        var file = ipath + iId.ToString() + extension;
                        System.IO.File.WriteAllBytes(file, buffer);

                        imagePath1 = file;
                    }
                }

                destacados.Add(destacado1);
                imagePaths.Add(imagePath1);

                #endregion

                #region Destacado 2

                var imagePath2 = "";

                DYK_DESTACADODto destacado2 = new DYK_DESTACADODto();
                destacado2.DESTACADOID = 2;
                destacado2.URL = url2;
                destacado2.ABRIRENSITIO = abrirensitio2;

                if (!String.IsNullOrEmpty(imageRaw2))
                {
                    var iId = Guid.NewGuid();
                    var ipath = FileUploaderHelper.UploadTempFolder;
                    var imparts = imageRaw2.Split(',');
                    if (imparts.Length > 1)
                    {
                        var mimetype = imparts[0].Split(':')[1].Split(';')[0];
                        var extension = MimeTypeHelper.GetDefaultExtension(mimetype);
                        var buffer = Convert.FromBase64String(imparts[1]);
                        var file = ipath + iId.ToString() + extension;
                        System.IO.File.WriteAllBytes(file, buffer);

                        imagePath2 = file;
                    }
                }

                destacados.Add(destacado2);
                imagePaths.Add(imagePath2);

                #endregion

                #region Destacado 3

                var imagePath3 = "";

                DYK_DESTACADODto destacado3 = new DYK_DESTACADODto();
                destacado3.DESTACADOID = 3;
                destacado3.URL = url3;
                destacado3.ABRIRENSITIO = abrirensitio3;

                if (!String.IsNullOrEmpty(imageRaw3))
                {
                    var iId = Guid.NewGuid();
                    var ipath = FileUploaderHelper.UploadTempFolder;
                    var imparts = imageRaw3.Split(',');
                    if (imparts.Length > 1)
                    {
                        var mimetype = imparts[0].Split(':')[1].Split(';')[0];
                        var extension = MimeTypeHelper.GetDefaultExtension(mimetype);
                        var buffer = Convert.FromBase64String(imparts[1]);
                        var file = ipath + iId.ToString() + extension;
                        System.IO.File.WriteAllBytes(file, buffer);

                        imagePath3 = file;
                    }
                }

                destacados.Add(destacado3);
                imagePaths.Add(imagePath3);

                #endregion

                var pathDestino = ConfigurationManager.AppSettings["Assets.Uploads.Path"] + ConfigurationManager.AppSettings["Imagen.Path.Destacados"].Replace("/", "\\");
                ServicioAplicacionDestacado.ModificarDestacados(destacados, imagePaths, pathDestino);

                //Delete files
                if (!String.IsNullOrEmpty(imagePath1))
                {
                    try
                    {
                        System.IO.File.Delete(imagePath1);
                        System.IO.File.Delete(imagePath2);
                        System.IO.File.Delete(imagePath3);
                    }
                    catch
                    {
                    }
                }

                //Guarda el log
                var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
                var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

                var log = new EmbLogActividadesDto();
                log.UsuarioId = usuarioId;
                log.fecha = DateTime.Now;
                log.idTipoLog = (int)TipoLog.EditarDestacado;
                log.ip = ip;
                log.MenuId = (int)Menus.Destacados;
                ServicioAplicacionLogs.AgregarLog(log);

                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false });
            }
        }

        #endregion

        #region Banners

        /// <summary>
        /// Trae la lista de banners
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TraerBanners()
        {
            var banners = ServicioAplicacionBanner.TraerBanners().ToList();
            var model = new
            {
                data = banners,
                total = banners.Count
            };

            return new JsonNetResult() { Data = model };
        }

        /// <summary>
        /// Trae un banner
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TraerBanner(Guid bannerId)
        {
            var banner = ServicioAplicacionBanner.TraerBannerPorId(bannerId);
            return new JsonNetResult() { Data = banner };
        }

        [HttpPost]
        public JsonResult GuardarBanner(Guid? bannerId, string bannerUrl, int bannerPosicion, string bannerImageRaw)
        {
            var imagePath = "";

            #region Imagen
            if (!String.IsNullOrEmpty(bannerImageRaw))
            {
                var iId = Guid.NewGuid();
                var ipath = FileUploaderHelper.UploadTempFolder;
                var imparts = bannerImageRaw.Split(',');
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
            #endregion

            DYK_BANNERDto banner = new DYK_BANNERDto()
            {
                BANNERID = bannerId.HasValue ? bannerId.Value : Guid.NewGuid(),
                URL = bannerUrl,
                POSICION = bannerPosicion
            };

            //Traer el usuario
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userName = claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

            var tipoLog = TipoLog.NuevoBanner;
            var virtualPath = ConfigurationManager.AppSettings["Assets.Uploads.Path"];
            var pathImagenDestino = virtualPath + ConfigurationManager.AppSettings["Imagen.Path.Banners"].Replace("/", "\\");
            if (bannerId.HasValue)
            {
                tipoLog = TipoLog.EditarBanner;
                ServicioAplicacionBanner.ModificarBanner(banner, imagePath, pathImagenDestino);
            }
            else
            {
                ServicioAplicacionBanner.AgregarBanner(banner, imagePath, pathImagenDestino);
            }

            //Guarda el log
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var log = new EmbLogActividadesDto();
            log.UsuarioId = usuarioId;
            log.fecha = DateTime.Now;
            log.idTipoLog = (int)tipoLog;
            log.ip = ip;
            log.MenuId = (int)Menus.Destacados;
            ServicioAplicacionLogs.AgregarLog(log);

            return Json(new { Success = true });
        }

        /// <summary>
        /// Elimina Banner
        /// </summary>
        /// <param name="id">Identificador del banner</param>
        /// <returns>Result operation</returns>
        [HttpPost]
        public JsonResult EliminarBanner(Guid id)
        {
            ServicioAplicacionBanner.EliminarBanner(id);

            //Guarda el log
            var ip = HttpContext.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Request.UserHostAddress;
            var identity = (ClaimsIdentity)User.Identity;
            var usuarioId = int.Parse(identity.FindFirst(ClaimTypes.UserData).Value);

            var log = new EmbLogActividadesDto();
            log.UsuarioId = usuarioId;
            log.fecha = DateTime.Now;
            log.idTipoLog = (int)TipoLog.EliminarBanner;
            log.ip = ip;
            log.MenuId = (int)Menus.Destacados;
            ServicioAplicacionLogs.AgregarLog(log);

            return Json(new { Success = true });
        }

        #endregion

        #endregion
    }
}
