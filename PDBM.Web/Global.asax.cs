using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using PDBM.Infrastructure.Utils;
using PDBM.Web.Authorization;

namespace PDBM.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            LogHelper.InitConfigure();
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
            if (formsIdentity != null && formsIdentity.IsAuthenticated && formsIdentity.AuthenticationType == "Forms")
            {
                HttpContext.Current.User = FormsAuthentication<UserDataPrincipal>.TryParsePrincipal(HttpContext.Current.Request);
            }
        }
    }
}
