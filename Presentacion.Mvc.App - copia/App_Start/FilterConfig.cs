using Presentacion.Mvc.App.Handler;
using System.Web;
using System.Web.Mvc;

namespace Presentacion.Mvc.App
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
        }

        

    }
}
