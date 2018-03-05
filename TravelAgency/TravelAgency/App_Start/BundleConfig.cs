using System.Web;
using System.Web.Optimization;

namespace TravelAgency
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
					  "~/Content/bootstrap-lumen.css",
					  "~/Content/site.css"));

            //HomePage
            bundles.Add(new StyleBundle("~/Content/assets/css").Include(
                     "~/assets/css/bootstrap.min.css", "~/assets/css/font-awesome.min.css",
                     "~/assets/css/font.css", "~/assets/css/animate.css", "~/assets/css/dodatno.css",
                     "~/assets/css/structure.css"));

            bundles.Add(new ScriptBundle("~/bundles/assets/js").Include(
                        "~/assets/js/jquery.min.js", "~/assets/js/bootstrap.min.js",
                        "~/assets/js/wow.min.js", "~/assets/js/custom.js"));			
		}
	}
}
