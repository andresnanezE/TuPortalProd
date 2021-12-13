using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Presentacion.Mvc.App.Models;
using System.Configuration;

namespace Presentacion.Mvc.App.Controllers
{
    public class UploadFileController : Controller
    {

        private readonly string MENSAJE = @"Ha ocurrido algo inesperado en la carga de los archivos.</br>
                                            Inténtalo nuevamente, si el problema persiste, comunícate con en área de tecnología.";
        private readonly string MOSTRARTRACE = $"{ConfiguracionesGlobales.MostrarTrace}";

        //GET: UploadFile
        public JsonResult UploadFile(List<UploadFiles> data)
        {

            string path = string.Empty;

            try
            {


                if (data.Any())
                {

                    foreach (UploadFiles file in data)
                    {
                        path = ConfigurationManager.AppSettings[file.KeyConfig];

                        if (!Directory.Exists(path))
                        {
                            throw new Exception("Directorio destino no encontrado.");
                        }
                    }

                    foreach (UploadFiles file in data)
                    {
                        if (file.BorrarActuales)
                        {

                            path = ConfigurationManager.AppSettings[file.KeyConfig];


                            Directory.GetFiles(path).ToList().ForEach(delegate (string f)
                            {
                                System.IO.File.Delete(f);
                            });

                        }
                    }


                    foreach (UploadFiles file in data)
                    {


                        if (!(string.IsNullOrEmpty(file.Nombre)))
                        {

                            path = ConfigurationManager.AppSettings[file.KeyConfig];


                            using (FileStream stream = System.IO.File.Create(Path.Combine(path, file.Nombre)))
                            {
                                byte[] imageData = Convert.FromBase64String(file.FileData.Replace("data:image/jpeg;base64,", "")
                                    .Replace("data:application/pdf;base64,", "")
                                    .Replace("data:application/vnd.ms-excel;base64,", ""));
                                stream.Write(imageData, 0, imageData.Length);
                            }

                        }
                    }
                }

                return Json(new
                {
                    msg = "Ok",
                });


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


        public JsonResult UploadFileImg(UploadFiles data)
        {

            string path = string.Empty;

            try
            {


                path = ConfigurationManager.AppSettings[data.KeyConfig];
                path = path.IndexOf("~") >= 0 ? Server.MapPath(path) : path;


                if (!Directory.Exists(path))
                {
                    throw new Exception("Directorio destino no encontrado.");
                }


                if (data.BorrarActuales)
                {


                    Directory.GetFiles(path).ToList().ForEach(delegate (string f)
                    {
                        System.IO.File.Delete(f);
                    });

                }


                if (!(string.IsNullOrEmpty(data.Nombre) && string.IsNullOrEmpty(data.Nombre)))
                {


                    using (FileStream stream = System.IO.File.Create(Path.Combine(path, data.Nombre)))
                    {
                        byte[] imageData = Convert.FromBase64String(data.FileData.Replace("data:image/jpeg;base64,", "")
                            .Replace("data:application/pdf;base64,", ""));
                        stream.Write(imageData, 0, imageData.Length);
                    }

                }



                return Json(new
                {
                    msg = "Ok",
                });


            }
            catch (Exception exception)
            {
                return Json(new
                {
                    msgError = MENSAJE,
                    fileName = data.Nombre,
                    msgErrorException = MOSTRARTRACE.Equals("1") ?
                     $"{exception.Source}</br>{exception.Message}</br>{exception.StackTrace}" : string.Empty
                });
            }


        }



    }
}