using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 工程任务实体
    /// </summary>
    public class EngineeringTask : AggregateRoot
    {
        protected EngineeringTask()
        { 
        }

        /// <summary>
        /// 构造工程任务实体
        /// </summary>
        /// <param name="taskModel">工程名称</param>
        /// <param name="projectTaskId">项目任务Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal EngineeringTask(TaskModel taskModel, Guid projectTaskId, Guid createUserId)
        {
            projectTaskId.IsEmpty("项目任务Id");

            this.Id = Guid.NewGuid();
            this.TaskModel = taskModel;
            this.ProjectTaskId = projectTaskId;
            this.DesignCustomerId = Guid.Empty;
            this.ConstructionCustomerId = Guid.Empty;
            this.SupervisionCustomerId = Guid.Empty;
            this.ProjectManagerId = Guid.Empty;
            this.DesignMemos = "";
            this.DesignRealName = "";
            this.DesignDate = DateTime.Parse("2000-01-01");
            this.DesignState = Bool.否;
            this.EngineeringProgress = EngineeringProgress.未开工;
            this.ProgressMemos = "";
            this.ProgressDate = DateTime.Parse("2000-01-01");
            this.State = State.停用;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 工程名称
        /// </summary>
        public TaskModel TaskModel
        {
            get;
            set;
        }

        /// <summary>
        /// 项目任务Id
        /// </summary>
        public Guid ProjectTaskId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计单位
        /// </summary>
        public Guid DesignCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 施工单位
        /// </summary>
        public Guid ConstructionCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位
        /// </summary>
        public Guid SupervisionCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理
        /// </summary>
        public Guid ProjectManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计简述
        /// </summary>
        public string DesignMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 设计人
        /// </summary>
        public string DesignRealName
        {
            get;
            set;
        }

        /// <summary>
        /// 设计日期
        /// </summary>
        public DateTime DesignDate
        {
            get;
            set;
        }

        /// <summary>
        /// 设计状态
        /// </summary>
        public Bool DesignState
        {
            get;
            set;
        }

        /// <summary>
        /// 工程进度
        /// </summary>
        public EngineeringProgress EngineeringProgress
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
        /// 进度登记日期
        /// </summary>
        public DateTime ProgressDate
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
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

        /// <summary>
        /// 保存项目设计
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="designCustomerId">设计单位Id</param>
        /// <param name="constructionCustomerId">施工单位Id</param>
        /// <param name="supervisionCustomerId">监理单位Id</param>
        public void SaveProjectDesign(State state, Guid projectManagerId, Guid designCustomerId, Guid constructionCustomerId, Guid supervisionCustomerId)
        {
            this.State = state;
            this.ProjectManagerId = projectManagerId;
            this.DesignCustomerId = designCustomerId;
            this.ConstructionCustomerId = constructionCustomerId;
            this.SupervisionCustomerId = supervisionCustomerId;
        }
    }
}
