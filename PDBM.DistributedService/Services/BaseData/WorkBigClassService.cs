using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 派工大类分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WorkBigClassService : IWorkBigClassService
    {
        private readonly IWorkBigClassService workBigClassServiceImpl = ServiceLocator.Instance.GetService<IWorkBigClassService>();

        public WorkBigClassMaintObject GetWorkBigClassById(Guid id)
        {
            try
            {
                return workBigClassServiceImpl.GetWorkBigClassById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<WorkBigClassMaintObject> GetWorkBigClasss()
        {
            try
            {
                return workBigClassServiceImpl.GetWorkBigClasss();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<WorkBigClassSelectObject> GetUsedWorkBigClasss()
        {
            try
            {
                return workBigClassServiceImpl.GetUsedWorkBigClasss();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateWorkBigClass(WorkBigClassMaintObject workBigClassMaintObject)
        {
            try
            {
                workBigClassServiceImpl.AddOrUpdateWorkBigClass(workBigClassMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveWorkBigClasss(IList<WorkBigClassMaintObject> workBigClassMaintObjects)
        {
            try
            {
                workBigClassServiceImpl.RemoveWorkBigClasss(workBigClassMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            workBigClassServiceImpl.Dispose();
        }
    }
}
