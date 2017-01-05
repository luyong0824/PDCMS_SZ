using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace PDBM.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/pdbm_js").Include(
                "~/Scripts/jquery-1.11.2.min.js",
                "~/Scripts/miniui/miniui.js",
                "~/Scripts/common.js",
                "~/Scripts/utils.js"));

            bundles.Add(new ScriptBundle("~/Scripts/pdbmwithwebuploader_js").Include(
                "~/Scripts/jquery-1.11.2.min.js",
                "~/Scripts/miniui/miniui.js",
                "~/Scripts/common.js",
                "~/Scripts/utils.js",
                "~/Scripts/webuploader/webuploader.min.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap/skin").Include(
                "~/Content/bootstrap/skin.css"));

            bundles.Add(new StyleBundle("~/Content/pdbm_css").Include(
                "~/Content/icons.css",
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/pdbmwithwebuploader_css").Include(
                "~/Content/icons.css",
                "~/Content/site.css",
                "~/Content/webuploader.css"));

            bundles.Add(new StyleBundle("~/Content/print_css").Include(
                "~/Content/print.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap_index_js").Include(
                "~/Scripts/jquery-1.11.2.min.js",
                "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap_index_css").Include(
                "~/Content/bootstrap.min.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}