using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 角色用户实体
    /// </summary>
    public class RoleUser : AggregateRoot
    {
        protected RoleUser()
        {
        }

        /// <summary>
        /// 构造角色用户实体
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal RoleUser(Guid roleId, Guid userId, Guid createUserId)
        {
            roleId.IsEmpty("角色Id");
            userId.IsEmpty("用户Id");

            this.Id = Guid.NewGuid();
            this.RoleId = roleId;
            this.UserId = userId;
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
        /// 用户Id
        /// </summary>
        public Guid UserId
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
        /// 用户实体
        /// </summary>
        protected virtual User User
        {
            get;
            set;
        }
        #endregion
    }
}
