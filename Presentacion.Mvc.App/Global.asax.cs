// ----------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using Presentacion.Mvc.App.Handler;
using Presentacion.Mvc.App.Utilidades;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Presentacion.Mvc.App
{
    public class MvcApplication : HttpApplication
    {
        
        //protected void Application_BeginRequest()
        //{
        //    HttpContext httpContext = HttpContext.Current;
        //    if (httpContext != null && httpContext.CurrentHandler != null)
        //    {
        //        var urlSitio = ((HttpRequestWrapper)((HttpContextWrapper)HttpContext).Request).Url.OriginalString;
        //        var queryString = ((HttpRequestWrapper)HttpContext.Request).Path.ToString();
        //    }

        //}
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true; //this line is to hide mvc header
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
            }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomExceptionFilter());
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context != null && context.Session != null)
            {
                context.Session["foo"] = "foo";
            }
        }

        protected void Application_Error()
        {
            string controllerName = "";
            string actionName = "";

            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null && httpContext.CurrentHandler != null)
            {
                try
                {
                    RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
                    /* When the request is ajax the system can automatically handle a mistake with a JSON response.
                       Then overwrites the default response */
                    if (requestContext.HttpContext.Request != null)
                    {
                        httpContext.Response.Clear();
                        controllerName = requestContext.RouteData.GetRequiredString("controller");
                        actionName = requestContext.RouteData.GetRequiredString("action");
                    }
                }
                catch
                {
                }
            }

            var Exc = this.Server.GetLastError();

            if (Exc != null)
            {
                string metName = "";
                if (!string.IsNullOrWhiteSpace(controllerName) && !string.IsNullOrWhiteSpace(actionName))
                    metName = string.Format("{0}_{1}", controllerName, actionName);
                else
                    metName = Utils.ObtenerNombreAplicacion() + "_Application_Error";

                new Task(() => { Utils.RegistrarLogWindows(metName, Exc); }).Start();

                this.Server.ClearError();
                Response.Redirect("../error/index");
                //Response.StatusDescription = "Error en el Servicio. Por favor contacte al Administrador.";
            }
        }
    }
}