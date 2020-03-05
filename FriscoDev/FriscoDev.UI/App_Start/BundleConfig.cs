using System.Web;
using System.Web.Optimization;

namespace FriscoDev.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            /******************************开始**********************************/
            bundles.Add(new StyleBundle("~/library/styles").Include(
                      "~/library/assets/css/bootstrap.css",
                      "~/library/assets/css/bootstrap-responsive.css",
                      "~/library/css/styles.css",
                      "~/library/js/layui/css/layui.css"));

            bundles.Add(new ScriptBundle("~/library/mainjs").Include(
                      "~/library/assets/js/jquery.js",
                      "~/library/js/layui/layui.js",
                      "~/library/assets/js/bootstrap.min.js",
                      "~/library/plugins/jquery.uniform.js",
                      "~/library/js/cookie.js",
                      "~/library/js/main.js",
                      "~/library/js/index.js"));
            /******************************结束**********************************/


        }
    }
}
