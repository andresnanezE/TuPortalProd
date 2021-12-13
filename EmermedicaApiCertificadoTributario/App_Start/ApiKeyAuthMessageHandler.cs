using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace ApiCertificadoTributario.App_Start
{
    public class ApiKeyAuthMessageHandler: DelegatingHandler
    {
        private string[] ApiKeys = ConfigurationManager.AppSettings["keyAuthentication"].Split(',');
        private const string API_KEY = "API_KEY";
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
        {
            // Para obtener la API Key
            var existeCabeceraApiKey = request.Headers.TryGetValues(API_KEY, out IEnumerable<string> listaCabeceras);
            if (existeCabeceraApiKey && listaCabeceras.Any())
            {
                var config = GlobalConfiguration.Configuration;
                var controllerSelector = new DefaultHttpControllerSelector(config);
                var controller = controllerSelector.SelectController(request);

                if (Array.Exists(ApiKeys, ak => ak.Equals(listaCabeceras.First())))
                {
                    var principal = new GenericPrincipal(new GenericIdentity("Auth_" + listaCabeceras.First().ToLower()), null);
                    AutorizarAccesoApi(principal);
                }
            }

            return base.SendAsync(request, cancellationToken);
        }

        private void AutorizarAccesoApi(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }
}