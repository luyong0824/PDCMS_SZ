using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 立项信息应用层服务
    /// </summary>
    public class ProjectCodeListService : DataService, IProjectCodeListService
    {
        private readonly IRepository<ProjectCodeList> projectCodeListRepository;
        private readonly IRepository<User> userRepository;

        public ProjectCodeListService(IRepositoryContext context,
            IRepository<ProjectCodeList> projectCodeListRepository,
            IRepository<User> userRepository)
            : base(context)
        {
            this.projectCodeListRepository = projectCodeListRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// 根据立项信息表Id获取立项信息
        /// </summary>
        /// <param name="id">立项信息表Id</param>
        /// <returns>立项信息维护对象</returns>
        public ProjectCodeListMaintObject GetProjectCodeListById(Guid id)
        {
            ProjectCodeList projectCodeList = projectCodeListRepository.FindByKey(id);
            if (projectCodeList != null)
            {
                ProjectCodeListMaintObject projectCodeListMaintObject = MapperHelper.Map<ProjectCodeList, ProjectCodeListMaintObject>(projectCodeList);
                projectCodeListMaintObject.ProjectDateText = projectCodeList.ProjectDate.ToShortDateString();
                User user = userRepository.FindByKey(projectCodeList.ProjectManagerId);
                projectCodeListMaintObject.ProjectManagerFullName = user.FullName;
                return projectCodeListMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的立项信息在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改立项信息
        /// </summary>
        /// <param name="projectCodeListMaintObject">要新增或者修改的立项信息维护对象</param>
        public void AddOrUpdateProjectCodeList(ProjectCodeListMaintObject projectCodeListMaintObject)
        {
            if (projectCodeListMaintObject.Id == Guid.Empty)
            {
                ProjectCodeList projectCodeList = AggregateFactory.CreateProjectCodeList(projectCodeListMaintObject.ProjectCode, (ProjectType)projectCodeListMaintObject.ProjectType, projectCodeListMaintObject.ProjectDate,
                    projectCodeListMaintObject.PlaceName, projectCodeListMaintObject.ReseauId, projectCodeListMaintObject.ProjectManagerId,State.使用, projectCodeListMaintObject.CreateUserId);
                projectCodeListRepository.Add(projectCodeList);
            }
            else
            {
                ProjectCodeList projectCodeList = projectCodeListRepository.FindByKey(projectCodeListMaintObject.Id);
                if (projectCodeList != null)
                {
                    projectCodeList.Modify(projectCodeListMaintObject.ProjectCode, (ProjectType)projectCodeListMaintObject.ProjectType, projectCodeListMaintObject.ProjectDate,
                    projectCodeListMaintObject.PlaceName, projectCodeListMaintObject.ReseauId, projectCodeListMaintObject.ProjectManagerId, State.使用, projectCodeListMaintObject.ModifyUserId);
                    projectCodeListRepository.Update(projectCodeList);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_ProjectCodeListProjectCode"))
                {
                    throw new ApplicationFault("立项编号重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 获取分页立项信息列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectCode">立项编号</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public string GetProjectCodeListPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string projectCode, int projectType, string placeName, Guid reseauId, Guid projectManagerId, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "ProjectType", Type = SqlDbType.Int, Value = projectType });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ProjectManagerId", Type = SqlDbType.UniqueIdentifier, Value = projectManagerId });            
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryProjectCodeListPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }
    }
}
