using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace PDBM.Web.Authorization
{
    /// <summary>
    /// 表单用户对象
    /// </summary>
    /// <typeparam name="TUserData">用户数据类型</typeparam>
    public class FormsPrincipal<TUserData> : IPrincipal
        where TUserData : class, new()
    {
        public FormsPrincipal(FormsAuthenticationTicket ticket, TUserData userData)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");
            if (userData == null)
                throw new ArgumentNullException("userData");

            Identity = new FormsIdentity(ticket);
            UserData = userData;
        }

        public TUserData UserData
        {
            get;
            private set;
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}