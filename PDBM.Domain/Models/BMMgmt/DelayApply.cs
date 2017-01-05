using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 工期延误申请实体
    /// </summary>
    public class DelayApply : AggregateRoot
    {
        protected DelayApply()
        { 
        }

        /// <summary>
        /// 构造工期延误申请实体
        /// </summary>
        /// <param name="constructionTaskId">建设任务Id</param>
        /// <param name="title">标题</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="delayDays">延期天数</param>
        /// <param name="remarks">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal DelayApply(Guid constructionTaskId, string title, EngineeringProgress constructionProgress, int delayDays, string remarks, Guid createUserId)
        {
            title.IsNullOrTooLong("标题", true, 50);
            constructionProgress.IsInvalid("建设进度");
            remarks.IsNullOrTooLong("备注", true, 500);

            this.Id = Guid.NewGuid();
            this.ConstructionTaskId = constructionTaskId;
            this.OrderCode = "";
            this.Title = title;
            this.ConstructionProgress = constructionProgress;
            this.DelayDays = delayDays;
            this.Remarks = remarks;
            this.OrderState = WFProcessInstanceState.未发送;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 建设任务Id
        /// </summary>
        public Guid ConstructionTaskId
        {
            get;
            set;
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
        /// 建设进度
        /// </summary>
        public EngineeringProgress ConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 延期天数
        /// </summary>
        public int DelayDays
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
        /// 申请单审批状态
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
        /// 修改工期延误申请实体
        /// </summary>
        /// <param name="constructionTaskId">建设任务Id</param>
        /// <param name="title">标题</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="delayDays">延期天数</param>
        /// <param name="remarks">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        public void Modify(string title, EngineeringProgress constructionProgress, int delayDays, string remarks, Guid modifyUserId)
        {
            title.IsNullOrTooLong("标题", true, 50);
            constructionProgress.IsInvalid("建设进度");
            remarks.IsNullOrTooLong("备注", true, 500);

            this.Title = title;
            this.ConstructionProgress = constructionProgress;
            this.DelayDays = delayDays;
            this.Remarks = remarks;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
