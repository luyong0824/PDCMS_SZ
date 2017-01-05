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

namespace PDBM.Domain.Models.WorkFlow
{
    /// <summary>
    /// 工作流活动实例实体
    /// </summary>
    public class WFActivityInstance : AggregateRoot
    {
        protected WFActivityInstance()
        {
        }

        /// <summary>
        /// 构造工作流活动实例实体
        /// </summary>
        /// <param name="wfProcessInstanceId">工作流过程实例Id</param>
        /// <param name="wfActivityInstanceName">工作流活动实例名称</param>
        /// <param name="wfActivityOperate">工作流活动操作类型</param>
        /// <param name="wfActivityEditorId">工作流活动编辑器Id</param>
        /// <param name="wfActivityOrder">工作流活动顺序类型</param>
        /// <param name="serialId">序号</param>
        /// <param name="rowId">行号</param>
        /// <param name="timelimit">期限(小时)</param>
        /// <param name="userId">批阅用户Id</param>
        /// <param name="isMustEdit">是否必须编辑</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal WFActivityInstance(Guid wfProcessInstanceId, string wfActivityInstanceName, WFActivityOperate wfActivityOperate, Guid wfActivityEditorId,
            WFActivityOrder wfActivityOrder, int serialId, int rowId, int timelimit, Guid userId, Bool isMustEdit, Guid createUserId)
        {
            wfProcessInstanceId.IsEmpty("公文Id");
            wfActivityInstanceName.IsNullOrTooLong("步骤名称", true, 100);
            wfActivityOperate.IsInvalid("操作类型");
            if (wfActivityOperate == WFActivityOperate.单据编辑)
            {
                wfActivityEditorId.IsEmpty("编辑类型");
            }
            wfActivityOrder.IsInvalid("排序方式");
            timelimit.IsPositive("时限");
            userId.IsEmpty("用户Id");

            this.Id = Guid.NewGuid();
            this.WFProcessInstanceId = wfProcessInstanceId;
            this.WFActivityInstanceName = wfActivityInstanceName;
            this.WFActivityInstanceState = WFActivityInstanceState.未处理;
            this.WFActivityInstanceFlow = WFActivityInstanceFlow.未处理;
            this.WFActivityInstanceResult = WFActivityInstanceResult.未处理;
            this.WFActivityOperate = wfActivityOperate;
            if (wfActivityOperate != WFActivityOperate.单据编辑)
            {
                this.WFActivityEditorId = null;
                this.IsMustEdit = Bool.否;
            }
            else
            {
                this.WFActivityEditorId = wfActivityEditorId;
                this.IsMustEdit = isMustEdit;
            }
            this.WFActivityOrder = wfActivityOrder;
            this.SerialId = serialId;
            this.RowId = rowId;
            this.Timelimit = timelimit;
            this.UserId = userId;
            this.Content = "";
            this.ReceivedDate = DateTime.Now;
            this.OperateDate = DateTime.Now;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 工作流过程实例Id
        /// </summary>
        public Guid WFProcessInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例名称
        /// </summary>
        public string WFActivityInstanceName
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例状态
        /// </summary>
        public WFActivityInstanceState WFActivityInstanceState
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例流转类型
        /// </summary>
        public WFActivityInstanceFlow WFActivityInstanceFlow
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例操作结果
        /// </summary>
        public WFActivityInstanceResult WFActivityInstanceResult
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动操作类型
        /// </summary>
        public WFActivityOperate WFActivityOperate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动编辑器Id
        /// </summary>
        public Guid? WFActivityEditorId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否必须编辑
        /// </summary>
        public Bool IsMustEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动顺序类型
        /// </summary>
        public WFActivityOrder WFActivityOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int SerialId
        {
            get;
            set;
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int RowId
        {
            get;
            set;
        }

        /// <summary>
        /// 期限(小时)
        /// </summary>
        public int Timelimit
        {
            get;
            set;
        }

        /// <summary>
        /// 批阅用户Id
        /// </summary>
        public Guid UserId
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
        /// 接收日期
        /// </summary>
        public DateTime ReceivedDate
        {
            get;
            set;
        }

        /// <summary>
        /// 批阅日期
        /// </summary>
        public DateTime OperateDate
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
        /// 工作流过程实例实体
        /// </summary>
        protected virtual WFProcessInstance WFProcessInstance
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动编辑器实体
        /// </summary>
        protected virtual WFActivityEditor WFActivityEditor
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

        /// <summary>
        /// 启动工作流活动实例
        /// </summary>
        public void Start()
        {
            this.WFActivityInstanceState = WFActivityInstanceState.待处理;
        }

        /// <summary>
        /// 处理工作流活动实例
        /// </summary>
        /// <param name="wfActivityInstanceFlow">工作流活动实例流转类型</param>
        /// <param name="content">内容</param>
        /// <param name="forwardWFActivityInstances">要转发工作流活动实例列表</param>
        public void Do(WFActivityInstanceFlow wfActivityInstanceFlow, string content, IList<WFActivityInstance> forwardWFActivityInstances)
        {
            content.IsNullOrTooLong("内容", true, 200);

            if (this.WFActivityInstanceState == WFActivityInstanceState.待处理)
            {
                if (this.WFActivityOperate == WFActivityOperate.阅)
                {
                    this.WFActivityInstanceResult = WFActivityInstanceResult.已阅;
                }
                else
                {
                    wfActivityInstanceFlow.IsInvalid("流转选项");

                    this.WFActivityInstanceFlow = wfActivityInstanceFlow;
                    if (this.WFActivityInstanceFlow == WFActivityInstanceFlow.正常流转 || this.WFActivityInstanceFlow == WFActivityInstanceFlow.转发他人)
                    {
                        this.WFActivityInstanceResult = WFActivityInstanceResult.同意;
                    }
                    else
                    {
                        this.WFActivityInstanceResult = WFActivityInstanceResult.不同意;
                    }
                    DomainEvent.Publish<WFActivityInstanceDoingEvent>(new WFActivityInstanceDoingEvent(this, forwardWFActivityInstances));
                }
                this.WFActivityInstanceState = WFActivityInstanceState.已处理;
                this.Content = content;
                this.OperateDate = DateTime.Now;
                this.ModifyUserId = this.UserId;
            }
            else
            {
                throw new DomainFault("该流程步骤已经处理过");
            }
        }
    }
}
