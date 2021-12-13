using ServiceStack.Text;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using Utilidades.Configuration;

namespace Presentacion.Mvc.App.Helpers
{
    /// <summary>
    /// FileUploaderHelper: Provides functionality to uploads files
    /// </summary>
    public class FileUploaderHelper
    {
        /// <summary>
        /// Temporal folder
        /// </summary>
        public static readonly string UploadTempFolder = ConfigurationManager.AppSettings["Assets.Temp.Path"];

        /// <summary>
        /// Uploads a files into the uploads folder
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <returns>Returns the file full path of the uploaded file</returns>
        public static string UploadFile(HttpPostedFileBase file)
        {
            var fileId = Guid.NewGuid();
            var filePath = UploadTempFolder + fileId + Path.GetExtension(file.FileName);
            if (!Directory.Exists(UploadTempFolder))
                Directory.CreateDirectory(UploadTempFolder);
            file.SaveAs(filePath);
            file.InputStream.Close();
            file.InputStream.Dispose();
            return filePath;
        }

        /// <summary>
        /// Uploads a files into the given upload folder
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="uploadFolder"></param>
        /// <returns>Returns the file full path of the uploaded file</returns>
        public static string UploadCopy(HttpPostedFileBase file, string uploadFolder)
        {
            var fileId = Guid.NewGuid();
            var filePath = uploadFolder + "Datos" + Path.GetExtension(file.FileName);
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);
            file.SaveAs(filePath);
            return filePath;
        }

        /// <summary>
        /// Gets an object of T type by name
        /// </summary>
        public T Get<T>(string name, T defaultValue)
        {
            var stringValue = ConfigUtils.GetNullableAppSetting(name);

            return stringValue != null ? TypeSerializer.DeserializeFromString<T>(stringValue) : defaultValue;
        }
    }
}
