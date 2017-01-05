using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Communication;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using PDBM.Web.Authorization;
using PDBM.Web.Filters;
using Newtonsoft.Json;

namespace PDBM.Web.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 系统首页
        /// </summary>
        /// <param name="returnUrl">跳转地址</param>
        /// <returns></returns>
        public ActionResult Index(string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                ViewData["ReturnUrl"] = returnUrl.Trim();
            }
            return View();
        }

        /// <summary>
        /// 系统主页
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public ActionResult Main()
        {
            ViewData["CompanyName"] = this.CompanyName;
            ViewData["DepartmentName"] = this.DepartmentName;
            ViewData["FullName"] = this.FullName;
            return View();
        }

        /// <summary>
        /// 我的桌面
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public ActionResult MyDesktop()
        {
            return View();
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        public ActionResult Exit()
        {
            FormsAuthentication.SignOut();
            ClearClientPageCache();
            return RedirectToAction("");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UserLogin()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> data = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                UserLoginObject userLoginObject = await Task.Factory.StartNew(() => proxy.Channel.UserLogin(data["UserName"].ToString().Trim(), data["UserPassword"].ToString().Trim()));
                UserDataPrincipal userData = new UserDataPrincipal()
                {
                    UserId = userLoginObject.Id,
                    UserName = userLoginObject.UserName,
                    FullName = userLoginObject.FullName,
                    PhoneNumber = userLoginObject.PhoneNumber,
                    Email = userLoginObject.Email,
                    DepartmentId = userLoginObject.DepartmentId,
                    DepartmentName = userLoginObject.DepartmentName,
                    CompanyId = userLoginObject.CompanyId,
                    CompanyName = userLoginObject.CompanyName,
                    CompanyNature = userLoginObject.CompanyNature
                };
                FormsAuthentication<UserDataPrincipal>.SetAuthCookie(userLoginObject.Id, userData);
            }
            return this.Sucess("登录成功");
        }

        /// <summary>
        /// 获取登录用户菜单列表的Json字符串
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<string> GetMenuInfo()
        {
            using (ServiceProxy<IMenuService> proxy = new ServiceProxy<IMenuService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetMenuInfo(this.UserId));
            }
        }

        /// <summary>
        /// 清空客户端缓存
        /// </summary>
        private void ClearClientPageCache()
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.Cache.SetNoStore();
        }

        //获取用户信息
        public ActionResult userInfo()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            string result = JsonConvert.SerializeObject(new { CompanyName = this.CompanyName, DepartmentName = this.DepartmentName, FullName = this.FullName }, Formatting.Indented, settings);//需要注意的是，如果返回的是一个集合，那么还要在它的上面再封装一个类。否则客户端收到会出错的。
            string[] keys = new string[] { "CompanyName", "DepartmentName", "FullName" };

            string[] values = new string[] { this.CompanyName, this.DepartmentName, this.FullName };
            return this.Sucess("获取用户信息成功", keys, values);
        }

        /// <summary>
        /// 手机端退出
        /// </summary>
        /// <returns></returns>
        public ActionResult MExit()
        {
            FormsAuthentication.SignOut();
            ClearClientPageCache();
            return RedirectToAction("/build/views/login.html");
        }
    }
}