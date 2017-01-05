using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DistributedService.Services.BMMgmt
{
    /// <summary>
    /// 工期延误申请分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class DelayApplyService : IDelayApplyService
    {
        private readonly IDelayApplyService delayApplyServiceImpl = ServiceLocator.Instance.GetService<IDelayApplyService>();

        public DelayApplyMaintObject GetDelayApplyById(Guid id)
        {
            try
            {
                return delayApplyServiceImpl.GetDelayApplyById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateDelayApply(DelayApplyMaintObject delayApplyMaintObject)
        {
            try
            {
                delayApplyServiceImpl.AddOrUpdateDelayApply(delayApplyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveDelayApplys(IList<DelayApplyMaintObject> delayApplyMaintObjects)
        {
            try
            {
                delayApplyServiceImpl.RemoveDelayApplys(delayApplyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            delayApplyServiceImpl.Dispose();
        }
    }
}
