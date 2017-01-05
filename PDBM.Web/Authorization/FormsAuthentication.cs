using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace PDBM.Web.Authorization
{
    /// <summary>
    /// 表单授权
    /// </summary>
    /// <typeparam name="TUserData">用户数据类型</typeparam>
    public class FormsAuthentication<TUserData>
        where TUserData : class, new()
    {
        private const int expiration = 60;

        public static void SetAuthCookie(Guid userId, TUserData userData)
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException();
            }
            if (userData == null)
            {
                throw new ArgumentNullException("userData");
            }

            var data = (new JavaScriptSerializer()).Serialize(userData);
            var ticket = new FormsAuthenticationTicket(2, userId.ToString(), DateTime.Now, DateTime.Now.AddDays(1), false, data);
            var cookieValue = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Domain = FormsAuthentication.CookieDomain,
                Path = FormsAuthentication.FormsCookiePath,
            };
            if (expiration > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(expiration);
            }
            HttpContext.Current.Response.Cookies.Remove(cookie.Name);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static FormsPrincipal<TUserData> TryParsePrincipal(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var cookie = request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                return null;
            }

            try
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && !string.IsNullOrWhiteSpace(ticket.UserData))
                {
                    var userData = (new JavaScriptSerializer()).Deserialize<TUserData>(ticket.UserData);
                    if (userData != null)
                    {
                        return new FormsPrincipal<TUserData>(ticket, userData);
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}