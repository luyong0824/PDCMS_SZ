using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 岗位用户实体
    /// </summary>
    public class PostUser : AggregateRoot
    {
        protected PostUser()
        {
        }

        /// <summary>
        /// 构造岗位用户实体
        /// </summary>
        /// <param name="postId">岗位Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal PostUser(Guid postId, Guid userId, Guid createUserId)
        {
            postId.IsEmpty("岗位Id");
            userId.IsEmpty("用户Id");

            this.Id = Guid.NewGuid();
            this.PostId = postId;
            this.UserId = userId;
            this.CreateUserId = createUserId;
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 岗位Id
        /// </summary>
        public Guid PostId
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
        /// 岗位实体
        /// </summary>
        protected virtual Post Post
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
