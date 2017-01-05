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
    /// 建设申请单服务接口
    /// </summary>
    [ServiceContract]
    public interface IPlanningApplyHeaderService : IDistributedService
    {
        /// <summary>
        /// 根据建设申请单Id获取建设申请单
        /// </summary>
        /// <param name="id">建设申请单Id</param>
        /// <returns>建设申请单维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlanningApplyHeaderMaintObject GetPlanningApplyHeaderById(Guid id);

        /// <summary>
        /// 新增或者修改建设申请单
        /// </summary>
        /// <param name="planningApplyHeaderMaintObject">要新增或者修改的建设申请单维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdatePlanningApplyHeader(PlanningApplyHeaderMaintObject planningApplyHeaderMaintObject);
    }
}
