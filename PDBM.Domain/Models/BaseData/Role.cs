using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class Role : AggregateRoot
    {
        protected Role()
        {
        }

        /// <summary>
        /// 构造角色实体
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="roleName">角色名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">角色状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Role(string roleCode, string roleName, string remarks, State state, Guid createUserId)
        {
            roleCode.IsNullOrEmptyOrTooLong("角色编码", true, 50);
            roleName.IsNullOrEmptyOrTooLong("角色名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("角色状态");

            this.Id = Guid.NewGuid();
            this.RoleCode = roleCode;
            this.RoleName = roleName;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode
        {
            get;
            set;
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 角色状态
        /// </summary>
        public State State
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
        /// 角色用户实体列表
        /// </summary>
        protected virtual ICollection<RoleUser> RoleUsers
        {
            get;
            set;
        }

        /// <summary>
        /// 角色菜单实体列表
        /// </summary>
        protected virtual ICollection<RoleMenuItem> RoleMenuItems
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改角色实体
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="roleName">角色名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">角色状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string roleCode, string roleName, string remarks, State state, Guid modifyUserId)
        {
            roleCode.IsNullOrEmptyOrTooLong("角色编码", true, 50);
            roleName.IsNullOrEmptyOrTooLong("角色名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("角色状态");

            this.RoleCode = roleCode;
            this.RoleName = roleName;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
