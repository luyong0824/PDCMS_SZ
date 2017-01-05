using PDBM.Domain.Events;
using PDBM.Domain.Events.WorkFlow;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 隐患上报实体
    /// </summary>
    public class WorkApply : AggregateRoot
    {
        protected WorkApply()
        {
        }

        /// <summary>
        /// 构造隐患上报实体
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="customerId">申请单位Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="reseauManagerId">网格经理Id</param>
        /// <param name="applyReason">申请事由</param>
        /// <param name="sceneContactMan">现场联系人</param>
        /// <param name="sceneContactTel">现场联系电话</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal WorkApply(string title, Guid customerId, Guid reseauId, Guid reseauManagerId, string applyReason, string sceneContactMan, string sceneContactTel, Guid createUserId)
        {
            title.IsNullOrTooLong("标题", true, 50);
            applyReason.IsNullOrTooLong("申请事由", true, 500);

            this.Id = Guid.NewGuid();
            this.OrderCode = "";
            this.Title = title;
            this.CustomerId = customerId;
            this.ReseauId = reseauId;
            this.ReseauManagerId = reseauManagerId;
            this.ApplyReason = applyReason;
            this.IsSoved = Bool.否;
            this.WorkOrderId = Guid.Empty;
            this.OrderState = WFProcessInstanceState.未发送;
            this.SceneContactMan = sceneContactMan;
            this.SceneContactTel = sceneContactTel;
            this.ReturnReason = "";
            this.ProjectCode = "";
            this.IsProject = Bool.否;
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
        /// 申请单位Id
        /// </summary>
        public Guid CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格经理Id
        /// </summary>
        public Guid ReseauManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 申请事由
        /// </summary>
        public string ApplyReason
        {
            get;
            set;
        }

        /// <summary>
        /// 是否解决
        /// </summary>
        public Bool IsSoved
        {
            get;
            set;
        }

        /// <summary>
        /// 派工单Id
        /// </summary>
        public Guid? WorkOrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 申请单审批状态
        /// </summary>
        public WFProcessInstanceState OrderState
        {
            get;
            set;
        }

        /// <summary>
        /// 现场联系人
        /// </summary>
        public string SceneContactMan
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string SceneContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 退回原因
        /// </summary>
        public string ReturnReason
        {
            get;
            set;
        }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否立项
        /// </summary>
        public Bool IsProject
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
        /// 修改隐患上报实体
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="customerId">申请单位Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="reseauManagerId">网格经理Id</param>
        /// <param name="applyReason">申请事由</param>
        /// <param name="sceneContactMan">现场联系人</param>
        /// <param name="sceneContactTel">现场联系电话</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string title, Guid customerId, Guid reseauId, Guid reseauManagerId, string applyReason, string sceneContactMan, string sceneContactTel, Guid modifyUserId)
        {
            title.IsNullOrTooLong("标题", true, 50);
            applyReason.IsNullOrTooLong("申请事由", true, 500);

            this.Title = title;
            this.CustomerId = customerId;
            this.ReseauId = reseauId;
            this.ReseauManagerId = reseauManagerId;
            this.ApplyReason = applyReason;
            this.SceneContactMan = sceneContactMan;
            this.SceneContactTel = sceneContactTel;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改隐患上报检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByUpdate(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能修改别人创建的隐患上报单");
            }
            DomainEvent.Publish<WFProcessInstanceOrderModifyingEvent>(new WFProcessInstanceOrderModifyingEvent(this));
        }
    }
}
