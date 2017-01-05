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
    /// 零星派工单服务接口
    /// </summary>
    [ServiceContract]
    public interface IWorkOrderService : IDistributedService
    {
        /// <summary>
        /// 根据零星派工单Id获取任务
        /// </summary>
        /// <param name="id">零星派工单Id</param>
        /// <returns>零星派工单维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WorkOrderMaintObject GetWorkOrderById(Guid id);

        /// <summary>
        /// 根据零星派工单Id获取零星派工单审批维护对象
        /// </summary>
        /// <param name="id">零星派工单Id</param>
        /// <returns>零星派工单审批维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WorkOrderEditorObject GetWorkOrderEditorById(Guid id);

        /// <summary>
        /// 根据零星派工单Id获取零星派工单打印信息
        /// </summary>
        /// <param name="id">零星派工单Id</param>
        /// <returns>零星派工单打印对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WorkOrderPrintObject GetWorkOrderPrintById(Guid id);

        /// <summary>
        /// 根据条件获取分页零星零星派工单列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="workBigClassId">工单大类Id</param>
        /// <param name="workSmallClassId">工单小类Id</param>
        /// <param name="customerId">代维单位Id</param>
        /// <param name="maintainContactMan">代维人员</param>
        /// <param name="sendUserId">派单人Id</param>
        /// <param name="isFinish">是否完成</param>
        /// <param name="orderState">申请状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWorkOrdersPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid workBigClassId, Guid workSmallClassId, Guid customerId, string maintainContactMan, Guid sendUserId, int isFinish, int orderState, Guid createUserId);


        /// <summary>
        /// 新增或者修改零星派工单
        /// </summary>
        /// <param name="workOrderMaintObject">要新增或者修改的零星派工单维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateWorkOrder(WorkOrderMaintObject workOrderMaintObject);

        /// <summary>
        /// 删除零星派工单
        /// </summary>
        /// <param name="towerMaintObjects">要删除的零星派工单维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveWorkOrder(IList<WorkOrderMaintObject> workOrderMaintObjects);

        /// <summary>
        /// 保存结算登记
        /// </summary>
        /// <param name="workOrderEditorObject">零星派工单审批对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveWorkOrderWF(WorkOrderEditorObject workOrderEditorObject);

        /// <summary>
        /// 保存通过派单申请新增的零星派工单
        /// </summary>
        /// <param name="workOrderMaintObject">零星派工单维护对象</param>
        /// <param name="workApplyMaintObjects">隐患上报维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveWorkOrderByWorkApply(WorkOrderMaintObject workOrderMaintObject, IList<WorkApplyMaintObject> workApplyMaintObjects);
    }
}
