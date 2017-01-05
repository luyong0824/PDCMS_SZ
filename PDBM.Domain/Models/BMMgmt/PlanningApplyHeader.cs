using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Events;
using PDBM.Domain.Events.WorkFlow;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 建设申请单实体
    /// </summary>
    public class PlanningApplyHeader : AggregateRoot
    {
        protected PlanningApplyHeader()
        {
        }

        /// <summary>
        /// 构造建设申请单实体
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal PlanningApplyHeader(string title, Guid createUserId)
        {
            title.IsNullOrEmptyOrTooLong("标题", true, 150);

            this.Id = Guid.NewGuid();
            this.OrderCode = string.Empty;
            this.Title = title;
            this.OrderState = WFProcessInstanceState.未发送;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        public WFProcessInstanceState OrderState
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

        /// <summary>
        /// 修改建设申请单实体
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string title, Guid modifyUserId)
        {
            title.IsNullOrEmptyOrTooLong("标题", true, 150);

            this.Title = title;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改建设申请单检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByUpdate(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能修改别人创建的建设申请单");
            }
            if (this.OrderState == WFProcessInstanceState.流程通过 || this.OrderState == WFProcessInstanceState.流程终止)
            {
                throw new DomainFault("不能修改结束流程的建设申请单");
            }
        }
    }
}
