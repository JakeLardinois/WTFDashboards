using System.Web;
using System.Web.Optimization;

namespace WTFDashboards
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true; // when configuring the bundles, if there are minified or CDN versions found then the files don't show up on the page; This forces them to display...
            bundles.UseCdn = true; //Enables CDN Support.


            bundles.Add(new ScriptBundle("~/bundles/jquery", "http://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui", "http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.3/jquery-ui.js")); //Note that you must put your version of JqueryUI in jquery.themeswitcher.js or jquery.themeswitcher.min.js...
            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                        "~/Scripts/layout.js"));
            bundles.Add(new ScriptBundle("~/bundles/themeswitcher").Include(
                        "~/Scripts/jquery.themeswitcher.js"));


            bundles.Add(new StyleBundle("~/Content/reset").Include(
                "~/Content/reset.css",
                "~/Content/html5-reset.css"));
            bundles.Add(new StyleBundle("~/Content/layout").Include(
                "~/Content/site.css"));
        }
    }
}