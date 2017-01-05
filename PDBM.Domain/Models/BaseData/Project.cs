using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 项目实体
    /// </summary>
    public class Project : AggregateRoot
    {
        protected Project()
        {
        }

        /// <summary>
        /// 构造项目实体
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="projectName">项目简称</param>
        /// <param name="projectFullName">项目全称</param>
        /// <param name="projectCategory">项目类型</param>
        /// <param name="accountingEntityId">会计主体Id</param>
        /// <param name="managerUserId">分管总经理用户Id</param>
        /// <param name="responsibleUserId">项目负责人用户Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="state">状态</param>
        /// <param name="professionList">所涉专业列表</param>
        /// <param name="projectApplyDate">申请立项时间</param>
        /// <param name="projectDoApplyDate">完成立项时间</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Project(string projectCode, string projectName, string projectFullName, ProjectCategory projectCategory,
            Guid? accountingEntityId, Guid? managerUserId, Guid? responsibleUserId, string remarks, ProjectProgress projectProgress,
            State state, string professionList, decimal budgetPrice, DateTime projectApplyDate, DateTime projectDoApplyDate, Guid createUserId)
        {
            projectCode.IsNullOrEmptyOrTooLong("项目编码", true, 50);
            projectName.IsNullOrEmptyOrTooLong("项目简称", true, 100);
            projectFullName.IsNullOrEmptyOrTooLong("项目全称", true, 100);
            projectCategory.IsInvalid("项目类型");
            remarks.IsNullOrTooLong("备注", true, 150);
            projectProgress.IsInvalid("项目进度");
            state.IsInvalid("项目状态");
            professionList.IsNullOrEmptyOrTooLong("所涉专业列表", false, 50);

            this.Id = Guid.NewGuid();
            this.ProjectCode = projectCode;
            this.ProjectName = projectName;
            this.ProjectFullName = projectFullName;
            this.ProjectCategory = projectCategory;
            this.AccountingEntityId = accountingEntityId;
            this.ManagerUserId = managerUserId;
            this.ResponsibleUserId = responsibleUserId;
            this.Remarks = remarks;
            this.ProjectProgress = projectProgress;
            this.State = state;
            this.ProfessionList = professionList;
            this.BudgetPrice = budgetPrice;
            this.ProjectApplyDate = projectApplyDate;
            this.ProjectDoApplyDate = projectDoApplyDate;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
            this.FinishDate = this.CreateDate;
        }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 项目简称
        /// </summary>
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目全称
        /// </summary>
        public string ProjectFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        public ProjectCategory ProjectCategory
        {
            get;
            set;
        }

        /// <summary>
        /// 会计主体Id
        /// </summary>
        public Guid? AccountingEntityId
        {
            get;
            set;
        }

        /// <summary>
        /// 分管总经理用户Id
        /// </summary>
        public Guid? ManagerUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目负责人用户Id
        /// </summary>
        public Guid? ResponsibleUserId
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
        /// 项目进度
        /// </summary>
        public ProjectProgress ProjectProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal BudgetPrice
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
        /// 所涉专业列表
        /// </summary>
        public string ProfessionList
        {
            get;
            set;
        }

        /// <summary>
        /// 申请立项时间
        /// </summary>
        public DateTime ProjectApplyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 完成立项时间
        /// </summary>
        public DateTime ProjectDoApplyDate
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
        /// 完工日期
        /// </summary>
        public DateTime FinishDate
        {
            get;
            set;
        }

        #region Relations
        /// <summary>
        /// 项目专业实体列表
        /// </summary>
        protected virtual ICollection<ProjectProfession> ProjectProfessions
        {
            get;
            set;
        }

        /// <summary>
        /// 会计主体实体
        /// </summary>
        protected virtual AccountingEntity AccountingEntity
        {
            get;
            set;
        }

        /// <summary>
        /// 分管总经理用户实体
        /// </summary>
        public virtual User ManagerUser
        {
            get;
            set;
        }

        /// <summary>
        /// 项目负责人用户实体
        /// </summary>
        public virtual User ResponsibleUser
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址确认实体列表
        /// </summary>
        protected virtual ICollection<Addressing> Addressings
        {
            get;
            set;
        }

        /// <summary>
        /// 改造实体列表
        /// </summary>
        protected virtual ICollection<Remodeling> Remodelings
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改项目实体
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="projectName">项目简称</param>
        /// <param name="projectFullName">项目全称</param>
        /// <param name="projectCategory">项目类型</param>
        /// <param name="accountingEntityId">会计主体Id</param>
        /// <param name="managerUserId">分管总经理用户Id</param>
        /// <param name="responsibleUserId">项目负责人用户Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="state">状态</param>
        /// <param name="professionList">所涉专业列表</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string projectCode, string projectName, string projectFullName, ProjectCategory projectCategory,
            Guid? accountingEntityId, Guid? managerUserId, Guid? responsibleUserId, string remarks, ProjectProgress projectProgress,
            State state, string professionList, decimal budgetPrice, Guid modifyUserId)
        {
            projectCode.IsNullOrEmptyOrTooLong("项目编码", true, 50);
            projectName.IsNullOrEmptyOrTooLong("项目简称", true, 100);
            projectFullName.IsNullOrEmptyOrTooLong("项目全称", true, 100);
            projectCategory.IsInvalid("项目类型");
            remarks.IsNullOrTooLong("备注", true, 150);
            projectProgress.IsInvalid("项目进度");
            state.IsInvalid("项目状态");
            professionList.IsNullOrEmptyOrTooLong("所涉专业列表", false, 50);

            this.ProjectCode = projectCode;
            this.ProjectName = projectName;
            this.ProjectFullName = projectFullName;
            this.ProjectCategory = projectCategory;
            this.AccountingEntityId = accountingEntityId;
            this.ManagerUserId = managerUserId;
            this.ResponsibleUserId = responsibleUserId;
            this.Remarks = remarks;
            this.ProjectProgress = projectProgress;
            this.BudgetPrice = budgetPrice;
            this.State = state;
            this.ProfessionList = professionList;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改项目名称及预算
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="ProjectName">项目名称</param>
        /// <param name="budgetPrice">项目预算</param>
        public void ModifyProject(string projectCode, string ProjectName, decimal budgetPrice)
        {
            this.ProjectCode = projectCode;
            this.ProjectName = ProjectName;
            this.ProjectFullName = ProjectName;
            this.BudgetPrice = budgetPrice;
        }

        /// <summary>
        /// 工程经理修改进度为完工时将项目改成已完工
        /// </summary>
        /// <param name="mark"></param>
        public void ModifyProjectProgress(int mark)
        {
            //if (mark == 1)
            //{
            //    this.ProjectProgress = ProjectProgress.已完工;
            //    this.FinishDate = DateTime.Now;
            //}
            //else
            //{
            //    this.ProjectProgress = ProjectProgress.在建中;
            //    this.ModifyDate = DateTime.Now;
            //}
        }
    }
}
