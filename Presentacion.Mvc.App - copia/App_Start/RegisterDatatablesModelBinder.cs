using System.Web;
using System.Web.Mvc;
using Mvc.Bootstrap.Datatables;
using Presentacion.Mvc.App;

[assembly: PreApplicationStartMethod(typeof(RegisterDatatablesModelBinder), "Start")]

namespace Presentacion.Mvc.App
{
    public static class RegisterDatatablesModelBinder {
        public static void Start() {
            ModelBinders.Binders.Add(typeof(DataTablesParam), new NullableDataTablesModelBinder());
        }
    }
}
