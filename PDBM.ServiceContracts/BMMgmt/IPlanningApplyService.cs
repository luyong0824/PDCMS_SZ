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
    /// 建设申请服务接口
    /// </summary>
    [ServiceContract]
    public interface IPlanningApplyService : IDistributedService
    {
        /// <summary>
        /// 根据建设申请Id获取建设申请
        /// </summary>
        /// <param name="id">建设申请Id</param>
        /// <returns>建设申请维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlanningApplyMaintObject GetPlanningApplyById(Guid id);

        /// <summary>
        /// 根据建设申请Id获取健身申请打印信息
        /// </summary>
        /// <param name="id">建设申请Id</param>
        /// <returns>建设申请打印对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlanningApplyHeaderPrintObject GetPlanningApplyPrintById(Guid id);

        /// <summary>
        /// 根据条件获取分页建设申请列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="issued">是否下达</param>
        /// <param name="createUserId">申请人</param>
        /// <param name="profession">专业</param>
        /// <returns>分页建设申请列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlanningApplysPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningName, Guid areaId, Guid reseauId,
            int issued, Guid createUserId, int profession);

        /// <summary>
        /// 新增或者修改建设申请
        /// </summary>
        /// <param name="planningApplyMaintObject">要新增或者修改的建设申请维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdatePlanningApply(PlanningApplyMaintObject planningApplyMaintObject);

        /// <summary>
        /// 删除建设申请
        /// </summary>
        /// <param name="planningMaintObjects">要删除的建设申请维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemovePlanningApplys(IList<PlanningApplyMaintObject> planningApplyMaintObjects);

        /// <summary>
        /// 保存业务审核
        /// </summary>
        /// <param name="planningApplyMaintObject">要修改的建设申请维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveBusinessAudit(PlanningApplyMaintObject planningApplyMaintObject);

        /// <summary>
        /// 保存技术审核
        /// </summary>
        /// <param name="planningApplyMaintObject">要修改的建设申请维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveTechnicalAudit(PlanningApplyMaintObject planningApplyMaintObject);

        /// <summary>
        /// 保存建设申请单
        /// </summary>
        /// <param name="planningApplyHeaderMaintObject">要新增的建设申请单维护对象</param>
        /// <param name="planningApplyMaintObjects">要修改的建设申请维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SavePlanningApplyHeader(PlanningApplyHeaderMaintObject planningApplyHeaderMaintObject, IList<PlanningApplyMaintObject> planningApplyMaintObjects);

        /// <summary>
        /// 根据基站建设申请单获取相关联的基站建设申请
        /// </summary>
        /// <param name="id">基站建设申请单Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlanningApplysByHeaderId(Guid id);

        /// <summary>
        /// 取消关联基站建设申请
        /// </summary>
        /// <param name="planningMaintObjects">要删除的建设申请维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemovePlanningApplyDetail(IList<PlanningApplyMaintObject> planningApplyMaintObjects);

        /// <summary>
        /// 删除建设申请单
        /// </summary>
        /// <param name="planningMaintObjects">要删除的建设申请维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemovePlanningApplyHeaders(IList<PlanningApplyMaintObject> planningApplyMaintObjects);

        /// <summary>
        /// 保存规划建议
        /// </summary>
        /// <param name="planningApplyMaintObjects">要修改的建设申请维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SavePlanningAdvice(IList<PlanningApplyMaintObject> planningApplyMaintObjects);

        /// <summary>
        /// 指定网优人员
        /// </summary>
        /// <param name="planningMaintObjects">要修改的建设申请维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AppointPlanningUser(IList<PlanningApplyMaintObject> planningApplyMaintObjects);

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="planningMaintObjects">要修改的建设申请维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void IssuePlanningApply(IList<PlanningApplyMaintObject> planningApplyMaintObjects);

        /// <summary>
        /// 取消下达
        /// </summary>
        /// <param name="planningMaintObjects">要修改的建设申请维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void CancelIssuePlanningApply(IList<PlanningApplyMaintObject> planningApplyMaintObjects);

        /// <summary>
        /// 根据条件获取分页待处理建设申请列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="doState">处理状态</param>
        /// <param name="createUserId">申请人</param>
        /// <param name="planningUserId">网优人员</param>
        /// <param name="profession">专业</param>
        /// <returns>分页建设申请列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPlanningApplysWaitPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningName, Guid areaId, Guid reseauId,
            int doState, Guid createUserId, Guid planningUserId, int profession);
    }
}
