using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.BaseData
{
    /// <summary>
    /// 项目服务接口
    /// </summary>
    [ServiceContract]
    public interface IProjectService : IDistributedService
    {
        /// <summary>
        /// 根据项目Id获取项目
        /// </summary>
        /// <param name="id">项目Id</param>
        /// <returns>项目维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ProjectMaintObject GetProjectById(Guid id);

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
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectsPage(int pageIndex, int pageSize, string projectCode, string projectName, string projectFullName, int projectCategory, Guid accountingEntityId, int projectProgress, int state);

        /// <summary>
        /// 新增或者修改项目
        /// </summary>
        /// <param name="projectMaintObject">要新增或者修改的项目维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateProject(ProjectMaintObject projectMaintObject);

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="projectMaintObjects">要删除的项目维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveProjects(IList<ProjectMaintObject> projectMaintObjects);

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
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectsPageBySelect(int pageIndex, int pageSize, string projectCode, string projectName, string projectFullName, Guid accountingEntityId,
            int isCheckedProjectProgress1, int isCheckedProjectProgress2, int isCheckedState1, int isCheckedState2);
    }
}
