using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 任务属性实体
    /// </summary>
    public class TaskProperty : AggregateRoot
    {
        protected TaskProperty()
        {
        }

        /// <summary>
        /// 构造任务属性实体
        /// </summary>
        /// <param name="constructionTaskId">任务Id</param>
        /// <param name="taskModelId">资源名称Id</param>
        /// <param name="parentId">资源Id</param>
        internal TaskProperty(Guid constructionTaskId, TaskModel taskModel, Guid parentId, Guid constructionCustomerId, Guid supervisorCustomerId, int timeLimit)
        {
            this.Id = Guid.NewGuid();
            this.ConstructionTaskId = constructionTaskId;
            this.TaskModel = taskModel;
            this.ParentId = parentId;
            this.ConstructionCustomerId = constructionCustomerId;
            this.SupervisorCustomerId = supervisorCustomerId;
            this.ConstructionProgress = EngineeringProgress.未开工;
            this.ProgressMemos = "";
            this.ProgressUserId = Guid.Empty;
            this.ProgressModifyDate = DateTime.Now;
            this.SubmitState = SubmitState.未提交;
            this.SubmitUserId = Guid.Empty;
            this.SubmitModifyDate = this.ProgressModifyDate;
            this.TimeLimit = timeLimit;
            this.CreateDate = this.ProgressModifyDate;
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

        /// <summary>
        /// 保存工程信息登记
        /// </summary>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="progressMemos">进度简述</param>
        /// <param name="submitState">提交状态</param>
        /// <param name="modifyUserId">修改用户Id</param>
        public void Modify(EngineeringProgress constructionProgress, string progressMemos, SubmitState submitState, Guid modifyUserId)
        {
            constructionProgress.IsInvalid("建设进度");
            progressMemos.IsNullOrTooLong("进度简述", true, 500);
            submitState.IsInvalid("提交状态");

            this.ConstructionProgress = constructionProgress;
            this.ProgressMemos = progressMemos;
            this.ProgressUserId = modifyUserId;
            this.ProgressModifyDate = DateTime.Now;
            this.SubmitState = submitState;
            this.SubmitUserId = modifyUserId;
            this.SubmitModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改进度登记
        /// </summary>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="progressMemos">进度简述</param>
        /// <param name="progressUserId">进度登记人Id</param>
        public void ModifyProgress(EngineeringProgress constructionProgress, string progressMemos, Guid progressUserId)
        {
            constructionProgress.IsInvalid("建设进度");
            progressMemos.IsNullOrTooLong("进度简述", true, 500);

            this.ConstructionProgress = constructionProgress;
            this.ProgressMemos = progressMemos;
            this.ProgressUserId = progressUserId;
            this.ProgressModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改资料登记状态
        /// </summary>
        /// <param name="submitState">提交状态</param>
        /// <param name="submitUserId">资料提交人Id</param>
        public void ModifySubmitState(SubmitState submitState, Guid submitUserId)
        {
            submitState.IsInvalid("提交状态");

            this.SubmitState = submitState;
            this.SubmitUserId = submitUserId;
            this.SubmitModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 设置监理单位
        /// </summary>
        /// <param name="supervisorCustomerId">监理单位Id</param>
        public void SettingSupervisorCustomer(Guid supervisorCustomerId)
        {
            this.SupervisorCustomerId = supervisorCustomerId;
        }

        /// <summary>
        /// 设置施工单位
        /// </summary>
        /// <param name="constructionCustomerId">施工单位Id</param>
        public void SettingConstructionCustomer(Guid constructionCustomerId)
        {
            this.ConstructionCustomerId = constructionCustomerId;
        }
    }
}
