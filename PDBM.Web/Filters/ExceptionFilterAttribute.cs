using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using PDBM.DataTransferObjects;
using PDBM.Infrastructure.Utils;

namespace PDBM.Web.Filters
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ExceptionFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                FaultException<FaultObject> fault = filterContext.Exception as FaultException<FaultObject>;
                if (filterContext.HttpContext.Request.IsAjaxRequest() &&
                    fault != null &&
                    fault.Detail.FaultType == "Bisuness")
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = new
                        {
                            Code = "-1",
                            Message = filterContext.Exception.Message
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    filterContext.HttpContext.Response.StatusCode = 200;
                    filterContext.ExceptionHandled = true;
                }
                else
                {
                    LogHelper.Log(filterContext.Exception);
                    filterContext.HttpContext.Response.StatusCode = 500;
                    base.OnException(filterContext);
                }
            }
        }
    }
}