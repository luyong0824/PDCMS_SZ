using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 子任务历史记录实体
    /// </summary>
    public class TaskPropertyLog : AggregateRoot
    {
        protected TaskPropertyLog()
        {
        }

        /// <summary>
        /// 构造子任务历史记录实体
        /// </summary>
        /// <param name="registerType">登记类型</param>
        /// <param name="constructionTaskId">任务Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="parentId">父表Id</param>
        /// <param name="constructionCustomerId">施工单位Id</param>
        /// <param name="supervisorCustomerId">监理单位Id</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="progressMemos">进度简述</param>
        /// <param name="progressUserId">进度登记人Id</param>
        /// <param name="progressModifyDate">进度登记日期</param>
        /// <param name="submitState">资料提交状态</param>
        /// <param name="submitUserId">资料提交人Id</param>
        /// <param name="submitModifyDate">资料提交日期</param>
        internal TaskPropertyLog(RegisterType registerType, Guid constructionTaskId, TaskModel taskModel, Guid parentId, Guid constructionCustomerId, Guid supervisorCustomerId,
            EngineeringProgress constructionProgress, string progressMemos, Guid? progressUserId, SubmitState submitState,
            Guid? submitUserId, int timeLimit)
        {
            this.Id = Guid.NewGuid();
            this.RegisterType = registerType;
            this.ConstructionTaskId = constructionTaskId;
            this.TaskModel = taskModel;
            this.ParentId = parentId;
            this.ConstructionCustomerId = constructionCustomerId;
            this.SupervisorCustomerId = supervisorCustomerId;
            this.ConstructionProgress = constructionProgress;
            this.ProgressMemos = progressMemos;
            this.ProgressUserId = progressUserId;
            this.ProgressModifyDate = DateTime.Now;
            this.SubmitState = submitState;
            this.SubmitUserId = submitUserId;
            this.SubmitModifyDate = this.ProgressModifyDate; ;
            this.TimeLimit = timeLimit;
            this.CreateDate = this.ProgressModifyDate;
        }

        /// <summary>
        /// 登记类型
        /// </summary>
        public RegisterType RegisterType
        {
            get;
            set;
        }

        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid ConstructionTaskId
        {
            get;
            set;
        }

        /// <summary>
        /// 资源名称Id
        /// </summary>
        public TaskModel TaskModel
        {
            get;
            set;
        }

        /// <summary>
        /// 资源Id
        /// </summary>
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 施工单位Id
        /// </summary>
        public Guid ConstructionCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位
        /// </summary>
        public Guid SupervisorCustomerId
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
        /// 进度简述
        /// </summary>
        public string ProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 进度登记人
        /// </summary>
        public Guid? ProgressUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 进度登记日期
        /// </summary>
        public DateTime ProgressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 资料提交状态
        /// </summary>
        public SubmitState SubmitState
        {
            get;
            set;
        }

        /// <summary>
        /// 资料提交人
        /// </summary>
        public Guid? SubmitUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 资料提交日期
        /// </summary>
        public DateTime SubmitModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 完成时限(天)
        /// </summary>
        public int TimeLimit
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
    }
}
