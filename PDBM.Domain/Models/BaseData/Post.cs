using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 岗位实体
    /// </summary>
    public class Post : AggregateRoot
    {
        protected Post()
        {
        }

        /// <summary>
        /// 构造岗位实体
        /// </summary>
        /// <param name="postCode">岗位编码</param>
        /// <param name="postName">岗位名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">岗位状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Post(string postCode, string postName, string remarks, State state, Guid createUserId)
        {
            postCode.IsNullOrEmptyOrTooLong("岗位编码", true, 50);
            postName.IsNullOrEmptyOrTooLong("岗位名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("岗位状态");

            this.Id = Guid.NewGuid();
            this.PostCode = postCode;
            this.PostName = postName;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 岗位编码
        /// </summary>
        public string PostCode
        {
            get;
            set;
        }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public string PostName
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
        /// 岗位状态
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
        /// 岗位用户实体列表
        /// </summary>
        protected virtual ICollection<PostUser> PostUsers
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
        #endregion

        /// <summary>
        /// 修改岗位实体
        /// </summary>
        /// <param name="postCode">岗位编码</param>
        /// <param name="postName">岗位名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">岗位状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string postCode, string postName, string remarks, State state, Guid modifyUserId)
        {
            postCode.IsNullOrEmptyOrTooLong("岗位编码", true, 50);
            postName.IsNullOrEmptyOrTooLong("岗位名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("岗位状态");

            this.PostCode = postCode;
            this.PostName = postName;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
