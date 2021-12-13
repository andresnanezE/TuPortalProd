using System.Web.Mvc;

namespace Presentacion.Mvc.App.Helpers
{
    public static class UrlHelper
    {
        /// <summary>
        /// Gets an image
        /// </summary>
        public static string TraerPathImage(this HtmlHelper helper, string imagePath, string img)
        {
            //var virtualPath = ConfigurationManager.AppSettings["Assets.Uploads.Virtual.Path"];
            //var url = virtualPath + imagePath + img;
            //url = path.Replace("\\\\", "/");

            //return url;

            return "";
        }
    }
}