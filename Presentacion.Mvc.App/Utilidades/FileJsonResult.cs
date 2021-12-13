using System.Web.Mvc;

namespace Presentacion.Mvc.App.Utilidades
{
    /// <summary>
    /// FileJsonResult: Enables a response for no AJAX requests when XMLHTTPRequests v2.0 are not enabled
    /// </summary>
    public class FileJsonResult : JsonResult
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="data"></param>
        public FileJsonResult(object data)
        {
            Data = data;
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        /// <summary>
        /// Executes the result
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Write("<textarea>");
            base.ExecuteResult(context);
            context.HttpContext.Response.Write("</textarea>");
            context.HttpContext.Response.ContentType = "text/html";
        }
    }
}