using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.WorkFlow
{
    /// <summary>
    /// 工作流过程实体
    /// </summary>
    public class WFProcess : AggregateRoot
    {
        protected WFProcess()
        {
        }

        /// <summary>
        /// 构造工作流过程实体
        /// </summary>
        /// <param name="wfCategoryId">工作流类型Id</param>
        /// <param name="wfProcessCode">工作流过程编码</param>
        /// <param name="wfProcessName">工作流过程名称</param>
        /// <param name="isApprovedByManager">是否部门经理审批</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">工作流过程状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal WFProcess(Guid wfCategoryId, string wfProcessCode, string wfProcessName, Bool isApprovedByManager, string remarks, State state, Guid createUserId)
        {
            wfCategoryId.IsEmpty("流程类型Id");
            wfProcessCode.IsNullOrEmptyOrTooLong("流程编码", true, 50);
            wfProcessName.IsNullOrEmptyOrTooLong("流程名称", true, 100);
            isApprovedByManager.IsInvalid("是否部门经理审批");
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("流程状态");

            this.Id = Guid.NewGuid();
            this.WFCategoryId = wfCategoryId;
            this.WFProcessCode = wfProcessCode;
            this.WFProcessName = wfProcessName;
            this.IsApprovedByManager = isApprovedByManager;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 工作流类型Id
        /// </summary>
        public Guid WFCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程编码
        /// </summary>
        public string WFProcessCode
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程名称
        /// </summary>
        public string WFProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否部门经理审批
        /// </summary>
        public Bool IsApprovedByManager
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
        /// 工作流过程状态
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
        /// 工作流类型实体
        /// </summary>
        protected virtual WFCategory WFCategory
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
        /// 工作流过程实例实体列表
        /// </summary>
        protected virtual ICollection<WFProcessInstance> WFProcessInstances
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改工作流过程实体
        /// </summary>
        /// <param name="wfProcessCode">工作流过程编码</param>
        /// <param name="wfProcessName">工作流过程名称</param>
        /// <param name="isApprovedByManager">是否部门经理审批</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">工作流过程状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string wfProcessCode, string wfProcessName, Bool isApprovedByManager, string remarks, State state, Guid modifyUserId)
        {
            wfProcessCode.IsNullOrEmptyOrTooLong("流程编码", true, 50);
            wfProcessName.IsNullOrEmptyOrTooLong("流程名称", true, 100);
            isApprovedByManager.IsInvalid("是否部门经理审批");
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("流程状态");

            this.WFProcessCode = wfProcessCode;
            this.WFProcessName = wfProcessName;
            this.IsApprovedByManager = isApprovedByManager;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
