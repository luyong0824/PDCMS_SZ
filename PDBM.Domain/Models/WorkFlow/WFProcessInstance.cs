using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Events;
using PDBM.Domain.Events.WorkFlow;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.WorkFlow
{
    /// <summary>
    /// 工作流过程实例实体
    /// </summary>
    public class WFProcessInstance : AggregateRoot
    {
        protected WFProcessInstance()
        {
        }

        /// <summary>
        /// 构造工作流过程实例实体
        /// </summary>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="entityId">实体Id</param>
        /// <param name="wfProcessInstanceCode">工作流过程实例编码</param>
        /// <param name="wfProcessInstanceName">工作流过程实例名称</param>
        /// <param name="content">内容</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal WFProcessInstance(Guid wfProcessId, Guid entityId, string wfProcessInstanceCode, string wfProcessInstanceName, string content, Guid createUserId)
        {
            wfProcessId.IsEmpty("流程Id");
            entityId.IsEmpty("实体Id");
            wfProcessInstanceCode.IsNullOrEmptyOrTooLong("单据编码", true, 50);
            wfProcessInstanceName.IsNullOrEmptyOrTooLong("标题", true, 100);
            content.IsNullOrTooLong("内容", true, 200);

            this.Id = Guid.NewGuid();
            this.WFProcessId = wfProcessId;
            this.EntityId = entityId;
            this.WFProcessInstanceCode = wfProcessInstanceCode;
            this.WFProcessInstanceName = wfProcessInstanceName;
            this.WFProcessInstanceState = WFProcessInstanceState.未发送;
            this.Content = content;
            this.CreateUserId = createUserId;
            this.CreateDate = DateTime.Now;
            DomainEvent.Publish<WFProcessInstanceSendingEvent>(new WFProcessInstanceSendingEvent(this));
        }

        /// <summary>
        /// 工作流过程Id
        /// </summary>
        public Guid WFProcessId
        {
            get;
            set;
        }

        /// <summary>
        /// 实体Id
        /// </summary>
        public Guid EntityId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程实例编码
        /// </summary>
        public string WFProcessInstanceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程实例名称
        /// </summary>
        public string WFProcessInstanceName
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程实例状态
        /// </summary>
        public WFProcessInstanceState WFProcessInstanceState
        {
            get;
            set;
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content
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

        /// <summary>
        /// 行版本号
        /// </summary>
        public byte[] RowVersion
        {
            get;
            set;
        }

        #region Relations
        /// <summary>
        /// 工作流过程实体
        /// </summary>
        protected virtual WFProcess WFProcess
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
        /// 启动工作流过程实例
        /// </summary>
        public void Start()
        {
            this.WFProcessInstanceState = WFProcessInstanceState.流转中;
        }
    }
}
