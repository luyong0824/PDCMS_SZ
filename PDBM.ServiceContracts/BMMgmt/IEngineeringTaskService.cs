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
    /// 工程任务服务接口
    /// </summary>
    [ServiceContract]
    public interface IEngineeringTaskService : IDistributedService
    {
        /// <summary>
        /// 根据工程任务Id获取工程任务信息
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns>工程任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        EngineeringTaskEditObject GetEngineeringTaskEditById(Guid id);

        /// <summary>
        /// 新增工程任务
        /// </summary>
        /// <param name="projectTaskMaintObject">要新增的工程任务维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddEngineeringTask(EngineeringTaskMaintObject engineeringTaskMaintObject);

        /// <summary>
        /// 删除工程任务
        /// </summary>
        /// <param name="engineeringTaskMaintObjects">要删除的工程任务维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveEngineeringTasks(IList<EngineeringTaskMaintObject> engineeringTaskMaintObjects);

        /// <summary>
        /// 根据条件获取分页施工设计任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="designState">设计完成</param>
        /// <param name="profession">专业</param>
        /// <param name="customerUserId">设计单位登陆人Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetEngineeringDesignsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId,
            Guid reseauId, int taskModel, string designRealName, int designState, int profession, Guid customerUserId);

        /// <summary>
        /// 根据工程任务Id获取施工设计
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns>工程任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        EngineeringTaskEditObject GetEngineeringDesignById(Guid id);

        /// <summary>
        /// 保存施工设计
        /// </summary>
        /// <param name="engineeringTaskEditObject">要修改的工程任务维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveEngineeringDesign(EngineeringTaskEditObject engineeringTaskEditObject);

        /// <summary>
        /// 根据条件获取分页工程进度任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="engineeringProgress">工程进度</param>
        /// <param name="profession">专业</param>
        /// <param name="currentUserId">登陆人Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetEngineeringProgresssPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId,
            Guid reseauId, int taskModel, int engineeringProgress, int profession, Guid currentUserId);

        /// <summary>
        /// 根据条件获取分页工程进度任务列表(移动端)
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="engineeringProgress">工程进度</param>
        /// <param name="currentUserId">登陆人Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetEngineeringProgresssPageMobile(int profession, string projectCode, string placeName, int taskModel, int engineeringProgress, Guid currentUserId);

        /// <summary>
        /// 根据工程任务Id获取工程进度登记
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns>工程任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        EngineeringTaskEditObject GetEngineeringProgressById(Guid id);

        /// <summary>
        /// 根据工程任务Id获取工程进度登记(移动端)
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns>工程任务修改对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        EngineeringTaskEditObject GetEngineeringProgressByIdMobile(Guid id, string header);

        /// <summary>
        /// 保存工程进度
        /// </summary>
        /// <param name="engineeringTaskEditObject">要修改的工程任务维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveEngineeringProgress(EngineeringTaskEditObject engineeringTaskEditObject);

        /// <summary>
        /// 保存工程进度(移动端)
        /// </summary>
        /// <param name="engineeringTaskEditObject">要修改的工程任务维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveEngineeringProgressMobile(EngineeringTaskEditObject engineeringTaskEditObject);

        /// <summary>
        /// 根据条件获取分页工程进度表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="engineeringProgress">工程进度</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="constructionCustomerId">施工单位Id</param>
        /// <param name="supervisionCustomerId">监理单位Id</param>
        /// <param name="profession">专业</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetEngineeringProgressReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel,
            int engineeringProgress, int projectType, Guid projectManagerId, Guid constructionCustomerId, Guid supervisionCustomerId, int profession, Guid companyId);

        /// <summary>
        /// 根据条件获取分页工程设计清单
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="designCustomerId">设计单位Id</param>
        /// <param name="profession">专业</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetEngineeringDesignReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel,
            string designRealName, Guid designCustomerId, int profession, Guid companyId);
    }
}
