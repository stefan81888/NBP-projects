using System.Web;
using System.Web.Optimization;

namespace FacePlace
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Main layout
            bundles.Add(new StyleBundle("~/Content/assets/css").Include(
                     "~/Content/bootstrap.css",
                     "~/assets/css/main.css"));

            bundles.Add(new ScriptBundle("~/bundles/assets/js").Include(
                        "~/assets/js/jquery.min.js", "~/assets/js/skel.min.js",
                        "~/assets/js/util.js", "~/assets/js/main.js"));

            //Login layout
            bundles.Add(new StyleBundle("~/Content/login/css").Include(
                    "~/Content/bootstrap.css",
                    "~/login/css/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/login/js").Include(
                        "~/login/js/index.js"));
        }
    }
}
