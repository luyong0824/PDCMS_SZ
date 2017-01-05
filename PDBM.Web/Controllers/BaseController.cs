using PDBM.Web.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        protected Guid UserId
        {
            get
            {
                return ((FormsPrincipal<UserDataPrincipal>)this.User).UserData.UserId;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        protected string UserName
        {
            get
            {
                return ((FormsPrincipal<UserDataPrincipal>)this.User).UserData.UserName;
            }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        protected string FullName
        {
            get
            {
                return ((FormsPrincipal<UserDataPrincipal>)this.User).UserData.FullName;
            }
        }

        /// <summary>
        /// 联系电话
        /// </summary>
        protected string PhoneNumber
        {
            get
            {
                return ((FormsPrincipal<UserDataPrincipal>)this.User).UserData.PhoneNumber;
            }
        }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        protected string Email
        {
            get
            {
                return ((FormsPrincipal<UserDataPrincipal>)this.User).UserData.Email;
            }
        }

        /// <summary>
        /// 部门Id
        /// </summary>
        protected Guid DepartmentId
        {
            get
            {
                return ((FormsPrincipal<UserDataPrincipal>)this.User).UserData.DepartmentId;
            }
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        protected string DepartmentName
        {
            get
            {
                return ((FormsPrincipal<UserDataPrincipal>)this.User).UserData.DepartmentName;
            }
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        protected Guid CompanyId
        {
            get
            {
                return ((FormsPrincipal<UserDataPrincipal>)this.User).UserData.CompanyId;
            }
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        protected string CompanyName
        {
            get
            {
                return ((FormsPrincipal<UserDataPrincipal>)this.User).UserData.CompanyName;
            }
        }

        /// <summary>
        /// 公司性质
        /// </summary>
        protected int CompanyNature
        {
            get
            {
                return ((FormsPrincipal<UserDataPrincipal>)this.User).UserData.CompanyNature;
            }
        }

        protected ActionResult Sucess(string message)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(2);
            result["Code"] = "0";
            result["Message"] = message;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult Sucess(string message, string[] keys, string[] values)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(keys.Length + 2);
            result["Code"] = "0";
            result["Message"] = message;
            for (int i = 0; i < keys.Length; i++)
            {
                result[keys[i]] = values[i];
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult Error(string message)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(2);
            result["Code"] = "-1";
            result["Message"] = message;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult Error(string code, string message)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(2);
            result["Code"] = code;
            result["Message"] = message;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}