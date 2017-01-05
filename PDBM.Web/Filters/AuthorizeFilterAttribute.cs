using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PDBM.Web.Authorization;

namespace PDBM.Web.Filters
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthorizeFilterAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User as FormsPrincipal<UserDataPrincipal>;
            if (user != null)
            {
                return true;
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            UrlHelper url = new UrlHelper(filterContext.RequestContext);
            filterContext.Result = filterContext.HttpContext.Request.IsAjaxRequest() ?
                (ActionResult)new JsonResult()
                {
                    Data = new
                    {
                        Code = "-2",
                        Message = "由于您长时间未操作，您的登录信息已过期，请重新登录系统。",
                        ReturnUrl = url.RouteUrl(new { controller = "", action = "" })
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                } :
                new RedirectResult("~/Home/Index?ReturnUrl=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.RawUrl));
        }
    }
}