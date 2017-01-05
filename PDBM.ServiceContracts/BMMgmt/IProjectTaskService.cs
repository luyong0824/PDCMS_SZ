using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ServiceContracts.BMMgmt
{
    /// <summary>
    /// 项目任务服务接口
    /// </summary>
    [ServiceContract]
    public interface IProjectTaskService : IDistributedService
    {
        /// <summary>
        /// 根据项目任务Id获取项目任务信息
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns>项目任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ProjectTaskEditObject GetProjectTaskById(Guid id);

        /// <summary>
        /// 根据寻址确认Id获取项目任务信息
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <returns>项目任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ProjectTaskEditObject GetProjectTaskEditById(Guid id);

        /// <summary>
        /// 根据改造确认Id获取项目任务信息
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <returns>项目任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ProjectTaskEditObject GetProjectTaskEditByRemodelingId(Guid id);

        /// <summary>
        /// 新增项目任务
        /// </summary>
        /// <param name="projectTaskMaintObject">要新增的项目任务维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddProjectTask(ProjectTaskMaintObject projectTaskMaintObject);

        /// <summary>
        /// 删除项目任务
        /// </summary>
        /// <param name="projectTaskMaintObjects">要删除的项目任务维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveProjectTasks(IList<ProjectTaskMaintObject> projectTaskMaintObjects);

        /// <summary>
        /// 修改项目任务
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AppointAreaAndDesignUser(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 任务分配
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveProjectDesign(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 项目设计
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveDesignDrawing(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 登记逻辑号
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveLogicalNumber(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 项目开通
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveProjectOpening(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 站点状态变更
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SavePlaceState(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 根据条件获取分页项目设计任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="profession">专业</param>
        /// <param name="areaManagerId">项目经理Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectDesignsPage(int pageIndex, int pageSize, Guid areaId, Guid reseauId, string placeName, string projectCode, int profession, Guid areaManagerId);

        /// <summary>
        /// 根据条件获取分页项目设计任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="profession">专业</param>
        /// <param name="userId">当前登陆人Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetDesignDrawingsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, string designRealName, int profession, Guid userId);

        /// <summary>
        /// 根据项目任务Id获取项目任务信息
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns>项目任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ProjectTaskEditObject GetProjectDesignEditById(Guid id);

        /// <summary>
        /// 项目设计
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveProjectDesignEdit(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 根据条件获取分页项目进度登记列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="profession">专业</param>
        /// <param name="areaManagerId">项目经理Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectProgresssPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId,
            Guid reseauId, int projectProgress, int profession, Guid areaManagerId);

        /// <summary>
        /// 根据条件获取分页项目进度登记列表(移动端)
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="areaManagerId">项目经理Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectProgresssPageMobile(int profession, Guid areaId, Guid reseauId, string projectCode, string placeName, int projectProgress, Guid areaManagerId);

        /// <summary>
        /// 根据项目任务Id获取项目进度登记信息
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns>项目任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ProjectTaskEditObject GetProjectProgressById(Guid id);

        /// <summary>
        /// 根据项目任务Id获取项目进度登记信息(移动端)
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns>项目任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ProjectTaskEditObject GetProjectProgressByIdMobile(Guid id, string header);

        /// <summary>
        /// 保存项目进度登记
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveProjectProgress(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 保存项目进度登记(移动端)
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveProjectProgressMobile(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 根据项目任务Id获取项目设计
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns>项目任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ProjectTaskEditObject GetDesignDrawingById(Guid id);

        /// <summary>
        /// 保存项目设计
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveDesignDrawingEdit(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 改造站修改项目任务
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AppointAreaAndDesignUserR(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 改造站任务分配
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveProjectDesignR(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 改造站项目设计
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveDesignDrawingR(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 改造站登记逻辑号
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveLogicalNumberR(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 改造站项目开通
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveProjectOpeningR(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 获取建设任务历史记录列表
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectTaskHistory(Guid placeId);

        /// <summary>
        /// 获取项目进度表分页列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="isOverTime">是否超时</param>
        /// <param name="profession">专业</param>
        /// <returns>项目进度表分页列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectProgresssReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, int projectType,
            int projectProgress, Guid projectManagerId, int isOverTime, int profession, Guid companyId);

        /// <summary>
        /// 获取项目设计清单分页列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="generalDesignId">总设单位Id</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="profession">专业</param>
        /// <returns>项目设计清单分页列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectDesignReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, Guid generalDesignId,
            string designRealName, int profession, Guid companyId);

        /// <summary>
        /// 改造站指定项目经理及总设单位和任务分配
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveAreaAndDesignUserAndProjectDesignR(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 改造站登记逻辑号及项目开通
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveLogicalNumberAndProjectOpeningR(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 改造站登记逻辑号及项目开通和站点状态
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务修改对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveLogicalNumberAndProjectOpeningAndPlaceStateR(ProjectTaskEditObject projectTaskEditObject);

        /// <summary>
        /// 获取项目经理月报
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="beginDateYear">开始年份</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectTaskProjectManager(DateTime beginDate, DateTime beginDateYear, Guid departmentId, int profession, Guid companyId);

        /// <summary>
        /// 获取部门建设月报
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="beginDateYear">开始年份</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectTaskDepartment(DateTime beginDate, DateTime beginDateYear, int profession, Guid companyId);
    }
}
