using System.Web.Mvc;
using Newtonsoft.Json;
using SCI.GoogleAnalytic;
using System.Configuration;

namespace Presentacion.Mvc.App.Controllers
{
    public class GoogleAnalyticsController : Controller
    {
        [HttpPost]
        public string RegistrarEventoGoogleAnalytic(InformacionGoogle data)
        {
            string ip;
            var useragent = string.Empty;
            var dispositivo = string.Empty;

            if (data.Ip == null)
                ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            else
                ip = data.Ip;

            if(data.Dispositivo == null)
                dispositivo = Request.UserAgent.Contains("iPhone")
                              || Request.UserAgent.Contains("Windows Phone")
                              || Request.UserAgent.Contains("Android")
                              || Request.UserAgent.Contains("webOS")
                              || Request.UserAgent.Contains("iPad")
                              || Request.UserAgent.Contains("iPod")
                              || Request.UserAgent.Contains("BlackBerry") ? "Smartphone" : "PC";
            else
                dispositivo = data.Dispositivo;

            if (data.Navegador == null)
                useragent = Request.UserAgent;
            else
                useragent = data.Navegador;

            data.Ip = ip;
            data.Value = 1;
            data.Description = "";
            data.Navegador = useragent;
            data.Dispositivo = dispositivo;
            data.User = ConfigurationManager.AppSettings["UserGA"];

            var marcarProduccion = bool.Parse(ConfigurationManager.AppSettings["MarcacionDigitalPRD"]);

            var information = JsonConvert.SerializeObject(data);
            var resutl = GoogleAnalytic.TrackEvent(information, marcarProduccion ? SCI.GoogleAnalytic.Enum.EnvironmentEnum.Production : SCI.GoogleAnalytic.Enum.EnvironmentEnum.Developing);

            return resutl;
        }
    }

    public class InformacionGoogle
    {
        public int RmtCount { get; set; }
        public string Ip { get; set; }
        public string Navegador { get; set; }
        public string Dispositivo { get; set; }
        public string User { get; set; }
        public string Category { get; set; }
        public string Action { get; set; }
        public string Label { get; set; }
        public int? Value { get; set; }
        public string Description { get; set; }
    }
}