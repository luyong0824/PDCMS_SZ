using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;

namespace PDBM.Web.Authorization
{
    /// <summary>
    /// 用户数据
    /// </summary>
    public class UserDataPrincipal : IPrincipal
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DepartmentId
        {
            get;
            set;
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get;
            set;
        }

        /// <summary>
        /// 公司性质
        /// </summary>
        public int CompanyNature
        {
            get;
            set;
        }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNumber
        {
            get;
            set;
        }

        [ScriptIgnore]
        public IIdentity Identity
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}