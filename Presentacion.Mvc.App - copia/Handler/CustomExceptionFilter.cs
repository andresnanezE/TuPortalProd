using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Threading.Tasks;
using Presentacion.Mvc.App.Utilidades;

namespace Presentacion.Mvc.App.Handler
{
    public class CustomExceptionFilter: ExceptionFilterAttribute
    {
        /// <summary>
        /// Captura excepcion Controladores si esta incluido el filtro dentro de la configuracion del WebApi
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string exceptionMessage = string.Empty;
            if (actionExecutedContext.Exception.InnerException == null)
            {
                exceptionMessage = actionExecutedContext.Exception.Message;
            }
            else
            {
                exceptionMessage = actionExecutedContext.Exception.InnerException.Message;
            }

            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Error en el servicio."),
                ReasonPhrase = "Error en el Servicio. Por favor contacte al Administrador."
            };

            string metName = actionExecutedContext.ActionContext.Request.RequestUri.ToString();
            Uri urTest = actionExecutedContext.ActionContext.Request.RequestUri;

            if (metName.ToUpper().Contains("API"))
            {
                metName = string.Format("{0}{1}", urTest.Segments[2], urTest.Segments[3]);
            }
            else
            {
                metName = string.Format("{0}{1}", urTest.Segments[1], urTest.Segments[2]);
            }

            metName = metName.Replace('/', '_').Trim();

            new Task(() => { Utils.RegistrarLogWindows(metName, actionExecutedContext.Exception); }).Start();

            actionExecutedContext.Response = response;

        }
    }
}