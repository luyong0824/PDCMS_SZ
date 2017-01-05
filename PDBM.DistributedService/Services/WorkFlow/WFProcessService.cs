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
    /// 工作流过程分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WFProcessService : IWFProcessService
    {
        private readonly IWFProcessService wfProcessServiceImpl = ServiceLocator.Instance.GetService<IWFProcessService>();

        public WFProcessMaintObject GetWFProcessById(Guid id)
        {
            try
            {
                return wfProcessServiceImpl.GetWFProcessById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetWFProcesses()
        {
            try
            {
                return wfProcessServiceImpl.GetWFProcesses();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<WFProcessSelectObject> GetUsedWFProcesses()
        {
            try
            {
                return wfProcessServiceImpl.GetUsedWFProcesses();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<WFProcessSelectObject> GetUsedWFProcessesByWFCategoryId(Guid wfCategoryId)
        {
            try
            {
                return wfProcessServiceImpl.GetUsedWFProcessesByWFCategoryId(wfCategoryId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateWFProcess(WFProcessMaintObject wfProcessMaintObject)
        {
            try
            {
                wfProcessServiceImpl.AddOrUpdateWFProcess(wfProcessMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveWFProcesses(IList<WFProcessMaintObject> wfProcessMaintObjects)
        {
            try
            {
                wfProcessServiceImpl.RemoveWFProcesses(wfProcessMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            wfProcessServiceImpl.Dispose();
        }
    }
}
