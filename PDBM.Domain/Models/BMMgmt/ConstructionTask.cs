using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 建设任务实体
    /// </summary>
    public class ConstructionTask : AggregateRoot
    {
        protected ConstructionTask()
        {
        }

        /// <summary>
        /// 构造任务实体
        /// </summary>
        /// <param name="constructionMethod">建设方式</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="projectId">项目Id</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="progressMemos">进度简述</param>
        internal ConstructionTask(ConstructionMethod constructionMethod, Guid placeId, Guid projectId, Guid projectManagerId, Guid supervisorCustomerId, Guid supervisorUserId, EngineeringProgress constructionProgress, string progressMemos)
        {
            constructionMethod.IsInvalid("建设方式");
            placeId.IsEmpty("站点Id");
            projectId.IsEmpty("项目Id");
            projectManagerId.IsEmpty("工程经理Id");
            supervisorCustomerId.IsEmpty("监理单位Id");
            supervisorUserId.IsEmpty("监理人员Id");
            constructionProgress.IsInvalid("建设进度");
            //progressMemos.IsNullOrEmptyOrTooLong("进度简述", true, 500);

            this.Id = Guid.NewGuid();
            this.ConstructionMethod = constructionMethod;
            this.PlaceId = placeId;
            this.ProjectId = projectId;
            this.ProjectManagerId = projectManagerId;
            this.SupervisorCustomerId = supervisorCustomerId;
            this.SupervisorUserId = supervisorUserId;
            this.ConstructionProgress = constructionProgress;
            this.ProgressMemos = progressMemos;
            this.RequestState = RequestState.未请求;
            this.IsApply = Bool.否;
            this.IsFinishMobile = Bool.否;
            this.IsFinishTelecom = Bool.否;
            this.IsFinishUnicom = Bool.否;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 建设方式
        /// </summary>
        public ConstructionMethod ConstructionMethod
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理Id
        /// </summary>
        public Guid ProjectManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位Id
        /// </summary>
        public Guid SupervisorCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理人员Id
        /// </summary>
        public Guid SupervisorUserId
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
        /// 完工请求状态
        /// </summary>
        public RequestState RequestState
        {
            get;
            set;
        }

        /// <summary>
        /// 是否在工期延误审批中
        /// </summary>
        public Bool IsApply
        {
            get;
            set;
        }

        /// <summary>
        /// 移动安装是否完成
        /// </summary>
        public Bool IsFinishMobile
        {
            get;
            set;
        }

        /// <summary>
        /// 电信安装是否完成
        /// </summary>
        public Bool IsFinishTelecom
        {
            get;
            set;
        }

        /// <summary>
        /// 联通安装是否完成
        /// </summary>
        public Bool IsFinishUnicom
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
        /// 修改任务实体
        /// </summary>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="progressMemos">进度简述</param>
        public void Modify(EngineeringProgress constructionProgress, string progressMemos)
        {
            constructionProgress.IsInvalid("建设进度");
            progressMemos.IsNullOrTooLong("进度简述", true, 500);

            this.ConstructionProgress = constructionProgress;
            this.ProgressMemos = progressMemos;
            this.ModifyDate = DateTime.Now;
        }

        public void ModifyMobile(Bool isFinish)
        {
            isFinish.IsInvalid("安装完成否");
            this.IsFinishMobile = isFinish;
        }

        public void ModifyTelecom(Bool isFinish)
        {
            isFinish.IsInvalid("安装完成否");
            this.IsFinishTelecom = isFinish;
        }

        public void ModifyUnicom(Bool isFinish)
        {
            isFinish.IsInvalid("安装完成否");
            this.IsFinishUnicom = isFinish;
        }

        public void CheckByUpdate(Guid currentUserId)
        {
            if (this.ProjectManagerId != currentUserId || this.SupervisorUserId != currentUserId)
            {
                throw new DomainFault("不能修改别人的任务");
            }
        }

        /// <summary>
        /// 设置工程经理
        /// </summary>
        /// <param name="projectManagerId">工程经理用户Id</param>
        public void SettingProjectManager(Guid projectManagerId)
        {
            this.ProjectManagerId = projectManagerId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 设置监理单位
        /// </summary>
        /// <param name="supervisorCustomerId">监理单位Id</param>
        /// <param name="supervisorUserId">监理单位登陆人Id</param>
        public void SettingSupervisorCustomer(Guid supervisorCustomerId, Guid supervisorUserId)
        {
            this.SupervisorCustomerId = supervisorCustomerId;
            this.SupervisorUserId = supervisorUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
