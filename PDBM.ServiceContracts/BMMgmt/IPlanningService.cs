using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.BMMgmt
{
    /// <summary>
    /// 规划服务接口
    /// </summary>
    [ServiceContract]
    public interface IPlanningService : IDistributedService
    {
        /// <summary>
        /// 根据规划Id获取规划
        /// </summary>
        /// <param name="id">规划Id</param>
        /// <returns>规划维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlanningMaintObject GetPlanningById(Guid id);

        /// <summary>
        /// 根据规划Id获取规划(移动端)
        /// </summary>
        /// <param name="id">规划Id</param>
        /// <returns>规划维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlanningMaintObject GetPlanningByIdMobile(Guid id, string header);

        /// <summary>
        /// 根据条件获取分页规划列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="issued">是否下达</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="createUserId">规划人</param>
        /// <returns>分页规划列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlanningsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName,
             int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int issued, int addressingState, Guid createUserId);

        /// <summary>
        /// 根据条件获取分页租赁任务分配列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="isAppoint">是否指定租赁人</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="createUserId">规划人</param>
        /// <returns>分页租赁任务分配列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetAddressingUsersPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName,
             int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int isAppoint, int addressingState, Guid createUserId);


        //[OperationContract]
        //[FaultContract(typeof(FaultObject))]
        //string GetPlanningsExcel(DateTime beginDate, DateTime endDate,
        //    string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId,
        //    int urgency, int telecomDemand, int mobileDemand, int unicomDemand, int demandState, int issued, int addressingState, Guid createUserId);

        /// <summary>
        /// 新增或者修改规划
        /// </summary>
        /// <param name="planningMaintObject">要新增或者修改的规划维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdatePlanning(PlanningMaintObject planningMaintObject);

        /// <summary>
        /// 租赁主管修改规划
        /// </summary>
        /// <param name="planningMaintObject">要修改的规划维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void UpdatePlanningAddressing(PlanningMaintObject planningMaintObject);

        /// <summary>
        /// 删除规划
        /// </summary>
        /// <param name="planningMaintObjects">要删除的规划维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemovePlannings(IList<PlanningMaintObject> planningMaintObjects);

        /// <summary>
        /// 指定租赁人
        /// </summary>
        /// <param name="planningMaintObjects">要指定租赁人的规划维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AppointAddressingUser(IList<PlanningMaintObject> planningMaintObjects);

        /// <summary>
        /// 下达规划
        /// </summary>
        /// <param name="planningMaintObjects">要下达的规划维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void Issue(IList<PlanningMaintObject> planningMaintObjects);

        /// <summary>
        /// 取消下达规划
        /// </summary>
        /// <param name="planningMaintObjects">要取消下达的规划维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void CancelIssue(IList<PlanningMaintObject> planningMaintObjects);

        /// <summary>
        /// 根据条件获取分页规划列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="telecomDemand">电信需求</param>
        /// <param name="mobileDemand">移动需求</param>
        /// <param name="unicomDemand">联通需求</param>
        /// <param name="demandState">确认状态</param>
        /// <param name="issued">是否下达</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="createUserId">规划人</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="addressingUserId">租赁人</param>
        /// <param name="projectManagerId">工程经理</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <returns>分页规划列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetConstructionPlanningsReportPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate,
            string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId,
            int urgency, int telecomDemand, int mobileDemand, int unicomDemand, int demandState, int issued, int addressingState,
            Guid createUserId, string placeName, Guid addressingUserId, Guid projectManagerId, int constructionProgress);

        /// <summary>
        /// 根据规划Id获取规划
        /// </summary>
        /// <param name="id">规划Id</param>
        /// <returns>新模式规划维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        NewPlanningMaintObject GetNewPlanningById(Guid id);

        /// <summary>
        /// 新增或者修改规划
        /// </summary>
        /// <param name="newPlanningMaintObject">要新增或者修改的新模式规划维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateNewPlanning(NewPlanningMaintObject newPlanningMaintObject);

        /// <summary>
        /// 删除规划
        /// </summary>
        /// <param name="newPlanningMaintObjects">要删除的新模式规划维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveNewPlannings(IList<NewPlanningMaintObject> newPlanningMaintObjects);

        /// <summary>
        /// 根据条件获取分页规划列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="sceneId">周边环境</param>
        /// <param name="telecomDemand">电信需求</param>
        /// <param name="mobileDemand">移动需求</param>
        /// <param name="unicomDemand">联通需求</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="createUserId">规划人</param>
        /// <returns>分页规划列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetNewPlanningsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate,
            string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId,
            Guid sceneId, int telecomDemand, int mobileDemand, int unicomDemand, int addressingState, Guid createUserId);

        /// <summary>
        /// 根据条件获取规划列表(移动端)
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="professionListSql">专业sql</param>
        /// <param name="planningName">规划点名称</param>
        /// <returns>分页规划点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlanningsMobile(int pageIndex, int pageSize, string professionListSql, string planningName, Guid companyId);

        /// <summary>
        /// 根据条件获取规划列表(移动端)
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="professionListSql">专业sql</param>
        /// <returns>分页规划点列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlanningsPageMobile(int pageIndex, int pageSize, string professionListSql, decimal lng, decimal lat, decimal distance, Guid companyId);

        /// <summary>
        /// 更新规划点方位(移动端)
        /// </summary>
        /// <param name="planningMaintObject">要修改的规划维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SavePlanningPositionMobile(PlanningMaintObject planningMaintObject);

        /// <summary>
        /// 规划点修改(移动端)
        /// </summary>
        /// <param name="planningMaintObject">要修改的规划点维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SavePlanningMobile(PlanningMaintObject planningMaintObject);
    }
}
