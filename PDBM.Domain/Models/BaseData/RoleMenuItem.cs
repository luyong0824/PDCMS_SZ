using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 角色菜单实体
    /// </summary>
    public class RoleMenuItem : AggregateRoot
    {
        protected RoleMenuItem()
        {
        }

        /// <summary>
        /// 构造角色菜单实体
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="menuItemId">菜单项Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal RoleMenuItem(Guid roleId, Guid menuItemId, Guid createUserId)
        {
            roleId.IsEmpty("角色Id");
            menuItemId.IsEmpty("菜单项Id");

            this.Id = Guid.NewGuid();
            this.RoleId = roleId;
            this.MenuItemId = menuItemId;
            this.CreateUserId = createUserId;
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId
        {
            get;
            set;
        }

        /// <summary>
        /// 菜单项Id
        /// </summary>
        public Guid MenuItemId
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
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        #region Relations
        /// <summary>
        /// 角色实体
        /// </summary>
        protected virtual Role Role
        {
            get;
            set;
        }

        /// <summary>
        /// 菜单项实体
        /// </summary>
        protected virtual MenuItem MenuItem
        {
            get;
            set;
        }
        #endregion
    }
}
