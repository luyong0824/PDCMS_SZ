using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Common;
using System.ServiceModel;

namespace PDBM.ServiceContracts.BMMgmt
{
    /// <summary>
    /// 建设任务服务接口
    /// </summary>
    [ServiceContract]
    public interface IConstructionTaskService : IDistributedService
    {
        /// <summary>
        /// 根据任务Id获取任务
        /// </summary>
        /// <param name="id">任务Id</param>
        /// <returns>任务维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ConstructionTaskMaintObject GetConstructionTaskById(Guid id);

        /// <summary>
        /// 新增或者修改任务
        /// </summary>
        /// <param name="constructionTaskMaintObject">要新增或者修改的任务维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateConstructionTask(ConstructionTaskMaintObject constructionTaskMaintObject);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="constructionTaskMaintObjects">要删除的任务维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveConstructionTask(IList<ConstructionTaskMaintObject> constructionTaskMaintObjects);

        /// <summary>
        /// 根据条件获取分页建设任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型</param>
        /// <param name="areaId">区域</param>
        /// <param name="reseauId">网格</param>
        /// <param name="projectId">项目Id</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="projectManagerId">工程经理</param>
        /// <param name="constructionMethod">建设方式</param>
        /// <returns>分页建设任务列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetConstructionPlanningsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid projectId, int constructionProgress, Guid projectManagerId, int constructionMethod);

        /// <summary>
        /// 根据建设任务Id和站点Id获取建设进度维护对象
        /// </summary>
        /// <param name="id">建设任务Id</param>
        /// <param name="placeId">站点Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ConstructionTaskEditorObject GetConstructionPlanningById(Guid id, Guid placeId);

        /// <summary>
        /// 修改新增基站建设
        /// </summary>
        /// <param name="constructionTaskEditorObject"></param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveConstructionPlanning(ConstructionTaskEditorObject constructionTaskEditorObject);

        /// <summary>
        /// 运营商根据条件获取分页建设任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型</param>
        /// <param name="areaId">区域</param>
        /// <param name="reseauId">网格</param>
        /// <param name="projectId">项目Id</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="isFinish">安装是否完成</param>
        /// <param name="companyId">运营商公司Id</param>
        /// <param name="constructionMethod">建设方式</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetRegisterPlanningsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, int constructionProgress, int isFinish, Guid companyId, int constructionMethod);

        /// <summary>
        /// 运营商根据条件获取分页建设任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型</param>
        /// <param name="areaId">区域</param>
        /// <param name="reseauId">网格</param>
        /// <param name="projectId">项目Id</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="isFinish">安装是否完成</param>
        /// <param name="companyId">运营商公司Id</param>
        /// <param name="constructionMethod">建设方式</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetRegisterRemodeingsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, int constructionProgress, int isFinish, Guid companyId, int constructionMethod);

        /// <summary>
        /// 根据站点属性Id和分公司Id获取站点属性维护对象
        /// </summary>
        /// <param name="id">站点属性Id</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlacePropertyEditorObject GetRegisterPlanningById(Guid id, Guid constructionTaskId, Guid companyId);

        /// <summary>
        /// 修改站点属性
        /// </summary>
        /// <param name="placePropertyEditorObject">站点属性维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveRegisterPlanning(PlacePropertyEditorObject placePropertyEditorObject);

        /// <summary>
        /// 根据条件获取分页建设任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型</param>
        /// <param name="areaId">区域</param>
        /// <param name="reseauId">网格</param>
        /// <param name="customerId">监理单位Id</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="userId">登陆用户Id</param>
        /// <returns>分页建设任务列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetConstructionTasksPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid customerId, int constructionProgress, Guid userId);

        /// <summary>
        /// 修改建设进度
        /// </summary>
        /// <param name="constructionTaskEditorObject"></param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveConstructionTaskProgress(ConstructionTaskEditorObject constructionTaskEditorObject);

        /// <summary>
        /// 根据条件获取分页子建设任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型</param>
        /// <param name="areaId">区域</param>
        /// <param name="reseauId">网格</param>
        /// <param name="customerId">施工单位Id</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="supervisorCustomerId">监理单位Id</param>
        /// <param name="userId">登陆用户Id</param>
        /// <returns>分页建设任务列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetTaskPropertysPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid customerId, int constructionProgress, int taskModel, Guid supervisorCustomerId, Guid userId);

        /// <summary>
        /// 根据任务Id获取任务卡片信息
        /// </summary>
        /// <param name="id">任务Id</param>
        /// <returns>任务卡片信息</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ConstructionTaskPrintObject GetConstructionTaskCardById(Guid id);

        /// <summary>
        /// 根据任务Id获取任务资源信息
        /// </summary>
        /// <param name="id">任务Id</param>
        /// <returns>任务资源信息</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ResourceUpdateObject GetResourceUpdatePrint(Guid id);

        /// <summary>
        /// 根据条件获取分页项目信息列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="propertyRightSql">产权</param>
        /// <param name="groupPlaceCode">站点编码</param>
        /// <param name="PlaceName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="constructionMethod">建设方式</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetProjectInformationPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string propertyRightSql, string groupPlaceCode, string placeName, Guid areaId, Guid reseauId, int constructionMethod, int constructionProgress);

        /// <summary>
        /// 设置工程经理
        /// </summary>
        /// <param name="constructionTaskMaintObjects">要设置工程经理的任务维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SettingProjectManager(IList<ConstructionTaskMaintObject> constructionTaskMaintObjects);

        /// <summary>
        /// 设置监理单位
        /// </summary>
        /// <param name="constructionTaskMaintObjects">要设置监理单位的任务维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SettingSupervisorCustomer(IList<ConstructionTaskMaintObject> constructionTaskMaintObjects);
    }
}
