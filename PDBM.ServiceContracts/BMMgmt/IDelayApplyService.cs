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
    /// 工期延误申请服务接口
    /// </summary>
    [ServiceContract]
    public interface IDelayApplyService : IDistributedService
    {
        /// <summary>
        /// 根据工期延误申请Id获取任务
        /// </summary>
        /// <param name="id">工期延误申请Id</param>
        /// <returns>工期延误申请维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        DelayApplyMaintObject GetDelayApplyById(Guid id);

        /// <summary>
        /// 新增或者修改工期延误申请
        /// </summary>
        /// <param name="delayApplyMaintObject">要新增或者修改的工期延误申请维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateDelayApply(DelayApplyMaintObject delayApplyMaintObject);

        /// <summary>
        /// 删除工期延误申请
        /// </summary>
        /// <param name="towerMaintObjects">要删除的工期延误申请维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveDelayApplys(IList<DelayApplyMaintObject> delayApplyMaintObjects);
    }
}
