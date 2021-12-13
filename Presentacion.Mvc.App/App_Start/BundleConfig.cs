using System.Web.Optimization;

namespace Presentacion.Mvc.App
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            //Style
            bundles.Add(new StyleBundle("~/Content/bootstrap.css").Include("~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/kui.css").Include("~/Content/kui/kendo.common.min.css", "~/Content/kui/kendo.bootstrap.min.css"));

            //Emermedica Styles
            bundles.Add(new StyleBundle("~/Styles/home.css").Include("~/Content/comovoy.css", "~/Content/flickity.css"));
            bundles.Add(new StyleBundle("~/Styles/filer").Include("~/Content/jquery.filer.css"));
            bundles.Add(new StyleBundle("~/Styles/site.css").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Styles/spinner.css").Include("~/Content/sci_spinner.css"));

            //Reportes
            bundles.Add(new ScriptBundle("~/bundles/reportes").Include(
                       "~/Scripts/sci_descargarreporte.js", "~/Scripts/sci_reporte.js"));

            //Scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include("~/Scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/filer").Include("~/Scripts/jquery.filer.js"));
            bundles.Add(new ScriptBundle("~/bundles/soporteseguridadsocial").Include("~/Scripts/SoporteSeguridadSocial/SoporteSeguridadSocial.js"));
            bundles.Add(new ScriptBundle("~/bundles/cargacomisiones").Include("~/Scripts/CargaComisionesAsesroColpatria/cargacomisiones.js"));
            bundles.Add(new ScriptBundle("~/bundles/adminenvezde").Include("~/Scripts/AdminEnVezDe/AdminEnVezDe.js"));

            bundles.Add(new ScriptBundle("~/bundles/SolicitudesInternas").Include("~/Scripts/SolicitudesInternas/SolicitudesInternas.js"));
            bundles.Add(new ScriptBundle("~/bundles/DetalleSolicitud").Include("~/Scripts/SolicitudesInternas/DetalleSolicitud.js"));
            bundles.Add(new ScriptBundle("~/bundles/CargueCondicionesPIM").Include("~/Scripts/CargueCondicionesPIM/pim.js"));
            bundles.Add(new ScriptBundle("~/bundles/CargueBanner").Include("~/Scripts/CargueImagenBanner/carguebanner.js"));

            bundles.Add(new ScriptBundle("~/bundles/rtes").Include("~/js/rtestransmilenio_apoyo_rto.js"));
            bundles.Add(new ScriptBundle("~/bundles/popup").Include("~/js/popup.js"));
            bundles.Add(new ScriptBundle("~/bundles/modal").Include("~/js/modal.js"));
            bundles.Add(new ScriptBundle("~/bundles/kendoui").Include("~/Scripts/kendo 2016.3.1118/kendo.web.min.js", "~/Scripts/kendo 2016.3.1118/cultures/kendo.culture.es-CO.min.js", "~/Scripts/kendo/2014.1.318/cultures/kendo.culture.en-US.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/layout").Include("~/Scripts/_Layout/cotizadorInternoExterno.js", "~/Scripts/_Layout/manejoBanner.js"));
            bundles.Add(new ScriptBundle("~/bundles/ui").Include("~/Scripts/kui/kendo.web.min.js", "~/Scripts/kui/cultures/kendo.culture.es-CO.min.js", "~/Scripts/kui/cultures/kendo.culture.en-US.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/validaciones").Include("~/Scripts/validaciones.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/flickity").Include("~/Scripts/flickity.pkgd.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include("~/Scripts/jquery.unobtrusive-ajax.js"));

            //Emermedica Scripts
            bundles.Add(new ScriptBundle("~/bundles/emermedica.base").Include("~/Scripts/Emermedica/base.core.js"));
            bundles.Add(new ScriptBundle("~/bundles/emermedica.home").Include("~/Scripts/Emermedica/home.js"));
            bundles.Add(new ScriptBundle("~/bundles/emermedica.destacados").Include("~/Scripts/jquery.form.min.js", "~/Scripts/Emermedica/base.core.js", "~/Scripts/Emermedica/destacados.js", "~/Scripts/underscore-min.js"));
            bundles.Add(new ScriptBundle("~/bundles/emermedica.adminListaNoticias").Include("~/Scripts/Emermedica/base.core.js", "~/Scripts/Emermedica/adminListaNoticias.js"));
            bundles.Add(new ScriptBundle("~/bundles/emermedica.editarnoticia").Include("~/Scripts/jquery.form.min.js", "~/Scripts/Emermedica/base.core.js", "~/Scripts/Emermedica/editarnoticia.js"));
            bundles.Add(new ScriptBundle("~/bundles/emermedica.noticias").Include("~/Scripts/Emermedica/noticias.js"));
            bundles.Add(new ScriptBundle("~/bundles/emermedica.noticia").Include("~/Scripts/Emermedica/noticia.js"));
        }
    }
}