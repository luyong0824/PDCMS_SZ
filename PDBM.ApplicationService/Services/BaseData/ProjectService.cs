using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 项目应用层服务
    /// </summary>
    public class ProjectService : DataService, IProjectService
    {
        private readonly IRepository<Project> projectRepository;
        private readonly IRepository<ProjectProfession> projectProfessionRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Remodeling> remodelingRepository;

        public ProjectService(IRepositoryContext context,
            IRepository<Project> projectRepository,
            IRepository<ProjectProfession> projectProfessionRepository,
            IRepository<User> userRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<Remodeling> remodelingRepository)
            : base(context)
        {
            this.projectRepository = projectRepository;
            this.projectProfessionRepository = projectProfessionRepository;
            this.userRepository = userRepository;
            this.addressingRepository = addressingRepository;
            this.remodelingRepository = remodelingRepository;
        }

        /// <summary>
        /// 根据项目Id获取项目
        /// </summary>
        /// <param name="id">项目Id</param>
        /// <returns>项目维护对象</returns>
        public ProjectMaintObject GetProjectById(Guid id)
        {
            Project project = projectRepository.FindByKey(id);
            if (project != null)
            {
                ProjectMaintObject projectMaintObject = MapperHelper.Map<Project, ProjectMaintObject>(project);
                if (project.ManagerUserId == Guid.Empty)
                {
                    projectMaintObject.ManagerUserFullName = "";
                }
                else
                {
                    User managerUser = userRepository.GetByKey(project.ManagerUserId.Value);
                    projectMaintObject.ManagerUserFullName = managerUser.FullName;
                }
                if (project.ResponsibleUserId == Guid.Empty)
                {
                    projectMaintObject.ResponsibleUserFullName = "";
                }
                else
                {
                    User responsibleUser = userRepository.GetByKey(project.ResponsibleUserId.Value);
                }
                return projectMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的项目在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页项目列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="projectName">项目简称</param>
        /// <param name="projectFullName">项目全称</param>
        /// <param name="projectCategory">项目类型</param>
        /// <param name="accountingEntityId">会计主体Id</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="state">状态</param>
        /// <returns>分页项目列表的Json字符串</returns>
        public string GetProjectsPage(int pageIndex, int pageSize, string projectCode, string projectName, string projectFullName, int projectCategory, Guid accountingEntityId, int projectProgress, int state)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "ProjectName", Type = SqlDbType.NVarChar, Value = projectName });
            parameters.Add(new Parameter() { Name = "ProjectFullName", Type = SqlDbType.NVarChar, Value = projectFullName });
            parameters.Add(new Parameter() { Name = "ProjectCategory", Type = SqlDbType.Int, Value = projectCategory });
            parameters.Add(new Parameter() { Name = "AccountingEntityId", Type = SqlDbType.UniqueIdentifier, Value = accountingEntityId });
            parameters.Add(new Parameter() { Name = "ProjectProgress", Type = SqlDbType.Int, Value = projectProgress });
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = state });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryProjectsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改项目
        /// </summary>
        /// <param name="projectMaintObject">要新增或者修改的项目维护对象</param>
        public void AddOrUpdateProject(ProjectMaintObject projectMaintObject)
        {
            if (projectMaintObject.Id == Guid.Empty)
            {
                Project project = AggregateFactory.CreateProject(projectMaintObject.ProjectCode, projectMaintObject.ProjectName, projectMaintObject.ProjectFullName, (ProjectCategory)projectMaintObject.ProjectCategory,
                    projectMaintObject.AccountingEntityId, projectMaintObject.ManagerUserId, projectMaintObject.ResponsibleUserId, projectMaintObject.Remarks, (ProjectProgress)projectMaintObject.ProjectProgress,
                    (State)projectMaintObject.State, projectMaintObject.ProfessionList, projectMaintObject.BudgetPrice, DateTime.Now, DateTime.Now, projectMaintObject.CreateUserId);
                projectRepository.Add(project);
                string[] professionArray = project.ProfessionList.Split(',');
                foreach (string professionValue in professionArray)
                {
                    ProjectProfession projectProfession = AggregateFactory.CreateProjectProfession(project.Id, (Profession)int.Parse(professionValue), project.CreateUserId);
                    projectProfessionRepository.Add(projectProfession);
                }
            }
            else
            {
                Project project = projectRepository.FindByKey(projectMaintObject.Id);
                if (project != null)
                {
                    project.Modify(projectMaintObject.ProjectCode, projectMaintObject.ProjectName, projectMaintObject.ProjectFullName, (ProjectCategory)projectMaintObject.ProjectCategory,
                        projectMaintObject.AccountingEntityId, projectMaintObject.ManagerUserId, projectMaintObject.ResponsibleUserId, projectMaintObject.Remarks, (ProjectProgress)projectMaintObject.ProjectProgress,
                        (State)projectMaintObject.State, projectMaintObject.ProfessionList, projectMaintObject.BudgetPrice, projectMaintObject.ModifyUserId);
                    projectRepository.Update(project);
                    List<string> professionArray = project.ProfessionList.Split(',').ToList();
                    IEnumerable<ProjectProfession> projectProfessions = projectProfessionRepository.FindAll(Specification<ProjectProfession>.Eval(entity => entity.ProjectId == project.Id));
                    if (projectProfessions != null)
                    {
                        foreach (ProjectProfession projectProfession in projectProfessions)
                        {
                            if (professionArray.Contains(projectProfession.Profession.ToString()))
                            {
                                professionArray.Remove(projectProfession.Profession.ToString());
                            }
                            else
                            {
                                projectProfessionRepository.Remove(projectProfession);
                            }
                        }
                    }
                    for (int i = 0; i < professionArray.Count(); i++)
                    {
                        ProjectProfession projectProfession = AggregateFactory.CreateProjectProfession(project.Id, (Profession)int.Parse(professionArray[i]), project.CreateUserId);
                        projectProfessionRepository.Add(projectProfession);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_ProjectCode"))
                {
                    throw new ApplicationFault("项目编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_ProjectName"))
                {
                    throw new ApplicationFault("项目简称重复");
                }
                else if (ex.Message.Contains("IX_UQ_ProjectFullName"))
                {
                    throw new ApplicationFault("项目全称重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Project_dbo.tbl_AccountingEntity_AccountingEntityId"))
                {
                    throw new ApplicationFault("选择的会计主体在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Project_dbo.tbl_User_ManagerUserId"))
                {
                    throw new ApplicationFault("选择的分管总经理在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Project_dbo.tbl_User_ResponsibleUserId"))
                {
                    throw new ApplicationFault("选择的项目负责人在系统中不存在");
                }
                else if (ex.Message.Contains("IX_UQ_ProjectIdProfession"))
                {
                    throw new ApplicationFault("所涉专业重复添加");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="projectMaintObjects">要删除的项目维护对象列表</param>
        public void RemoveProjects(IList<ProjectMaintObject> projectMaintObjects)
        {
            foreach (ProjectMaintObject projectMaintObject in projectMaintObjects)
            {
                Project project = projectRepository.FindByKey(projectMaintObject.Id);
                if (project != null)
                {

                    IEnumerable<ProjectProfession> projectProfessions = projectProfessionRepository.FindAll(Specification<ProjectProfession>.Eval(entity => entity.ProjectId == project.Id));
                    if (projectProfessions != null)
                    {
                        foreach (ProjectProfession projectProfession in projectProfessions)
                        {
                            projectProfessionRepository.Remove(projectProfession);
                        }
                    }
                    projectRepository.Remove(project);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据条件获取项目分页列表，用于选择项目
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="projectName">项目简称</param>
        /// <param name="projectFullName">项目全称</param>
        /// <param name="accountingEntityId">会计主体Id</param>
        /// <param name="isCheckedProjectProgress1">在建中</param>
        /// <param name="isCheckedProjectProgress2">已完工</param>
        /// <param name="isCheckedState1">使用</param>
        /// <param name="isCheckedState2">停用</param>
        /// <returns>分页项目列表的Json字符串</returns>
        public string GetProjectsPageBySelect(int pageIndex, int pageSize, string projectCode, string projectName, string projectFullName, Guid accountingEntityId, int isCheckedProjectProgress1, int isCheckedProjectProgress2, int isCheckedState1, int isCheckedState2)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "ProjectName", Type = SqlDbType.NVarChar, Value = projectName });
            parameters.Add(new Parameter() { Name = "ProjectFullName", Type = SqlDbType.NVarChar, Value = projectFullName });
            parameters.Add(new Parameter() { Name = "AccountingEntityId", Type = SqlDbType.UniqueIdentifier, Value = accountingEntityId });
            parameters.Add(new Parameter() { Name = "IsCheckedProjectProgress1", Type = SqlDbType.Int, Value = isCheckedProjectProgress1 });
            parameters.Add(new Parameter() { Name = "IsCheckedProjectProgress2", Type = SqlDbType.Int, Value = isCheckedProjectProgress2 });
            parameters.Add(new Parameter() { Name = "IsCheckedState1", Type = SqlDbType.Int, Value = isCheckedState1 });
            parameters.Add(new Parameter() { Name = "IsCheckedState2", Type = SqlDbType.Int, Value = isCheckedState2 });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryProjectsPageBySelect", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }
    }
}
