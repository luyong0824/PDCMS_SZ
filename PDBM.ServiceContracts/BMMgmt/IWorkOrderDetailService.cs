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
    /// 派工单明细明细接口
    /// </summary>
    [ServiceContract]
    public interface IWorkOrderDetailService : IDistributedService
    {
        /// <summary>
        /// 根据派工单明细Id获取任务
        /// </summary>
        /// <param name="id">派工单明细Id</param>
        /// <returns>派工单明细维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WorkOrderDetailMaintObject GetWorkOrderDetailById(Guid id);

        /// <summary>
        /// 根据零星派工单Id获取各天之情情况
        /// </summary>
        /// <param name="workOrderId">零星派工单Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWorkOrderDetail(Guid workOrderId);

        /// <summary>
        /// 新增或者修改派工单明细
        /// </summary>
        /// <param name="workOrderDetailMaintObject">要新增或者修改的派工单明细维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateWorkOrderDetail(WorkOrderDetailMaintObject workOrderDetailMaintObject);

        /// <summary>
        /// 删除派工单明细
        /// </summary>
        /// <param name="towerMaintObjects">要删除的派工单明细维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveWorkOrderDetail(IList<WorkOrderDetailMaintObject> workOrderDetailMaintObjects);
    }
}
