// ----------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Presentacion.Mvc.App;
using System.Web.Services.Description;

[assembly: OwinStartup(typeof(Startup))]

namespace Presentacion.Mvc.App
{
    public class Startup
    {
        #region Instance Methods

        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "PortalComercialCookie",
                LoginPath = new PathString("/Usuarios/index")
            });
        }

        public void ConfigureServices(ServiceCollection services)
        {
            //services.Remove(typeof(IExceptionHandler), new EmerErrHandler());
            //services.Add(new EmerErrHandler());
        }

        #endregion Instance Methods
    }
}