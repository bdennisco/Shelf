using System.Web;
using System.Web.Optimization;

namespace Shelf
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        //"~/Scripts/jquery.validate.unobtrusive.min.js", 
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                      "~/Scripts/handlebars-v1.3.0.js",
                      "~/Scripts/jquery.ui.touch-punch.js",
                      "~/Scripts/jquery.shapeshift.js",
                      "~/Scripts/material.js",
                      "~/Scripts/ripples.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/material.css",
                      "~/Content/ripples.css",
                      "~/Content/content.css",
                      "~/Content/layout.css",
                      "~/Content/site.css"));
        }
    }
}
