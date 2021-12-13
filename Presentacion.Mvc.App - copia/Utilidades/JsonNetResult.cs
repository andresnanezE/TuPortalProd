using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Presentacion.Mvc.App.Utilidades
{
    /// <summary>
    /// Custom JsonResult
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        /// <summary>
        /// Executes the result
        /// </summary>
        /// <param name="context">Controller context</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            // If you need special handling, you can call another form of SerializeObject below
            var serializedObject = JsonConvert.SerializeObject
            (
                Data,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                }
            );
            response.Write(serializedObject);
        }
    }
}