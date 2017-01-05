using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class User : AggregateRoot
    {
        protected User()
        {
        }

        /// <summary>
        /// 构造用户实体
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="fullName">姓名</param>
        /// <param name="email">邮箱</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="state">用户状态</param>
        /// <param name="uniqueCode">唯一标识</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal User(Guid departmentId, string userName, string fullName, string email, string phoneNumber, State state, Guid uniqueCode, Guid createUserId)
        {
            departmentId.IsEmpty("部门Id");
            userName.IsNullOrEmptyOrTooLong("用户名", true, 50);
            fullName.IsNullOrEmptyOrTooLong("姓名", true, 50);
            email.IsNullOrTooLong("邮箱", true, 100);
            phoneNumber.IsNullOrTooLong("手机号", true, 50);
            state.IsInvalid("用户状态");

            this.Id = Guid.NewGuid();
            this.DepartmentId = departmentId;
            this.UserName = userName;
            this.UserPassword = StringHelper.Encrypto("123456");
            this.FullName = fullName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.State = state;
            this.IsCurrentUsed = Bool.是;
            this.UniqueCode = uniqueCode;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
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
        /// 用户名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword
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
        /// 邮箱
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 用户状态
        /// </summary>
        public State State
        {
            get;
            set;
        }

        /// <summary>
        /// 是否当前使用
        /// </summary>
        public Bool IsCurrentUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid UniqueCode
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModifyDate
        {
            get;
            set;
        }

        #region Relations
        /// <summary>
        /// 部门实体
        /// </summary>
        protected virtual Department Department
        {
            get;
            set;
        }

        /// <summary>
        /// 角色用户实体列表
        /// </summary>
        protected virtual ICollection<RoleUser> RoleUsers
        {
            get;
            set;
        }

        /// <summary>
        /// 岗位用户实体列表
        /// </summary>
        protected virtual ICollection<PostUser> PostUsers
        {
            get;
            set;
        }

        /// <summary>
        /// 部门经理部门实体列表
        /// </summary>
        public virtual ICollection<Department> ManagerUserDepartments
        {
            get;
            set;
        }

        /// <summary>
        /// 分管总经理项目实体列表
        /// </summary>
        public virtual ICollection<Project> ManagerUserProjects
        {
            get;
            set;
        }

        /// <summary>
        /// 项目负责人项目实体列表
        /// </summary>
        public virtual ICollection<Project> ResponsibleUserProjects
        {
            get;
            set;
        }

        /// <summary>
        /// 租赁人规划实体列表
        /// </summary>
        public virtual ICollection<Planning> AddressingUserPlannings
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实体列表
        /// </summary>
        protected virtual ICollection<WFActivity> WFActivitys
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例实体列表
        /// </summary>
        protected virtual ICollection<WFActivityInstance> WFActivityInstances
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改用户实体
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="fullName">姓名</param>
        /// <param name="email">邮箱</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="state">用户状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string userName, string fullName, string email, string phoneNumber, State state, Guid modifyUserId)
        {
            userName.IsNullOrEmptyOrTooLong("用户名", true, 50);
            fullName.IsNullOrEmptyOrTooLong("姓名", true, 50);
            email.IsNullOrTooLong("邮箱", true, 100);
            phoneNumber.IsNullOrTooLong("手机号", true, 50);
            state.IsInvalid("用户状态");

            this.UserName = userName;
            this.FullName = fullName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="fullName">姓名</param>
        /// <param name="email">邮箱</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="oldUserPassword">当前密码</param>
        /// <param name="newUserPassword">新密码，为空则不修改密码</param>
        /// <param name="confirmNewUserPassword">确认密码</param>
        public void ModifyUserInfo(string fullName, string email, string phoneNumber, string oldUserPassword, string newUserPassword, string confirmNewUserPassword)
        {
            fullName.IsNullOrEmptyOrTooLong("姓名", true, 50);
            email.IsNullOrTooLong("邮箱", true, 100);
            phoneNumber.IsNullOrTooLong("手机号", true, 50);

            this.FullName = fullName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            if (newUserPassword != null && newUserPassword.Length > 0)
            {
                if (oldUserPassword == null || oldUserPassword.Length == 0)
                {
                    throw new DomainFault("当前密码为空");
                }
                if (!this.UserPassword.Equals(StringHelper.Encrypto(oldUserPassword)))
                {
                    throw new DomainFault("当前密码不正确");
                }
                if (confirmNewUserPassword != null && confirmNewUserPassword.Length == 0)
                {
                    throw new DomainFault("确认密码为空");
                }
                if (!newUserPassword.Equals(confirmNewUserPassword))
                {
                    throw new DomainFault("两次输入的密码不正确");
                }
                if (Encoding.Default.GetBytes(newUserPassword).Length > 50)
                {
                    throw new DomainFault("新密码长度过长");
                }
                this.UserPassword = StringHelper.Encrypto(newUserPassword);
            }
        }

        /// <summary>
        /// 修改用户检查
        /// </summary>
        public void CheckByUpdate()
        {
            if (this.UserName == "admin")
            {
                throw new DomainFault("{0}<br>禁止修改系统管理员账号", this.UserName);
            }
        }

        /// <summary>
        /// 删除用户检查
        /// </summary>
        public void CheckByRemove()
        {
            if (this.UserName == "admin")
            {
                throw new DomainFault("{0}<br>禁止删除系统管理员账号", this.UserName);
            }
        }

        /// <summary>
        /// 用户登录检查
        /// </summary>
        public void CheckByLogin()
        {
            if (this.State == State.停用)
            {
                throw new DomainFault("用户账号已被停用");
            }
        }
    }
}
