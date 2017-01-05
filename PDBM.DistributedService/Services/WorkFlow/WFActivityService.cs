using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.WorkFlow;

namespace PDBM.DistributedService.Services.WorkFlow
{
    /// <summary>
    /// 工作流活动分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WFActivityService : IWFActivityService
    {
        private readonly IWFActivityService wfActivityServiceImpl = ServiceLocator.Instance.GetService<IWFActivityService>();

        public WFActivityMaintObject GetWFActivityById(Guid id)
        {
            try
            {
                return wfActivityServiceImpl.GetWFActivityById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFActivitys(Guid wfProcessId)
        {
            try
            {
                return wfActivityServiceImpl.GetWFActivitys(wfProcessId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFActivitysBySend(Guid wfProcessId, Guid userId, Guid entityId)
        {
            try
            {
                return wfActivityServiceImpl.GetWFActivitysBySend(wfProcessId, userId, entityId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateWFActivitys(IList<WFActivityMaintObject> wfActivityMaintObjects, Guid wfProcessId)
        {
            try
            {
                wfActivityServiceImpl.AddOrUpdateWFActivitys(wfActivityMaintObjects, wfProcessId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveWFActivitys(IList<WFActivityMaintObject> wfActivityMaintObjects, Guid wfProcessId)
        {
            try
            {
                wfActivityServiceImpl.RemoveWFActivitys(wfActivityMaintObjects, wfProcessId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            wfActivityServiceImpl.Dispose();
        }
    }
}
