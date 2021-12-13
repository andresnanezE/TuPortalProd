// ----------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="SCI Software">
//     Copyright (c) SCI Software 2014. Todos los derechos reservados.
// </copyright>
// <project>Domain.Administrator</project>
// ----------------------------------------------------------------------------------------------

using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Presentacion.Mvc.App
{
    public class RouteConfig
    {
        #region Class Methods

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            OmitirRutasExtensionesComoControlador(ref routes);

            routes.MapRoute(
                name: "Noticia",
                url: "Noticias/Articulo/{titulo}",
                defaults: new
                {
                    controller = "Noticias",
                    action = "Index",
                    titulo = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Login", action = "Index", id = UrlParameter.Optional
                }
            );

        }
        /// <summary>
        /// Corregido error: "The controller for path ... was not found or does not implement IController.". problema generado por lectura e interpretacion de archivos como controladores relacionados a su ruta.
        /// </summary>
        /// <param name="routes">RouteCollection para agregar los filtros a omitir para no tratarlos como controladores</param>
        private static void OmitirRutasExtensionesComoControlador(ref RouteCollection routes)
        {
            try
            {
                string strExtensions = Transversales.Administracion.Utils.getKey<string>("skipRoutersOnRouteConfigExtensions");
                string strPaths = Transversales.Administracion.Utils.getKey<string>("skipRoutersOnRouteConfigPaths");

                if (!string.IsNullOrWhiteSpace(strExtensions))
                {
                    var vExtensionsSkip = strExtensions.Split('|');
                    if (vExtensionsSkip != null && vExtensionsSkip.Length > 0)
                    {
                        routes.IgnoreRoute("{*allCustomExtensionsConfig}", new { allCustomExtensionsConfig = @".*\.(" + strExtensions + ")(/.*)?" });
                    }
                }

                if (!string.IsNullOrWhiteSpace(strPaths))
                {
                    var vPathsSkip = strPaths.Split('|').ToList();
                    if (vPathsSkip != null)
                    {
                        foreach (var item in vPathsSkip)
                        {
                            routes.IgnoreRoute(item);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                new System.Threading.Tasks.Task(() => { Transversales.Administracion.Utils.RegistrarLogWindows("RouteConfig_skipRoutesMod", ex); }).Start();
            }

        }
        #endregion captura errores

    }
}
