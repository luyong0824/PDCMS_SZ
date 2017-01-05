using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BMMgmt;

namespace PDBM.DistributedService.Services.BMMgmt
{
    /// <summary>
    /// 建设申请分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PlanningApplyHeaderService : IPlanningApplyHeaderService
    {
        private readonly IPlanningApplyHeaderService planningApplyHeaderServiceImpl = ServiceLocator.Instance.GetService<IPlanningApplyHeaderService>();

        public PlanningApplyHeaderMaintObject GetPlanningApplyHeaderById(Guid id)
        {
            try
            {
                return planningApplyHeaderServiceImpl.GetPlanningApplyHeaderById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdatePlanningApplyHeader(PlanningApplyHeaderMaintObject planningApplyHeaderMaintObject)
        {
            try
            {
                planningApplyHeaderServiceImpl.AddOrUpdatePlanningApplyHeader(planningApplyHeaderMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            planningApplyHeaderServiceImpl.Dispose();
        }
    }
}
