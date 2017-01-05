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
    /// 派工小类分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WorkSmallClassService : IWorkSmallClassService
    {
        private readonly IWorkSmallClassService workSmallClassServiceImpl = ServiceLocator.Instance.GetService<IWorkSmallClassService>();

        public WorkSmallClassMaintObject GetWorkSmallClassById(Guid id)
        {
            try
            {
                return workSmallClassServiceImpl.GetWorkSmallClassById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<WorkSmallClassMaintObject> GetWorkSmallClasss(Guid areaId)
        {
            try
            {
                return workSmallClassServiceImpl.GetWorkSmallClasss(areaId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<WorkSmallClassSelectObject> GetUsedWorkSmallClass(Guid workBigClassId)
        {
            try
            {
                return workSmallClassServiceImpl.GetUsedWorkSmallClass(workBigClassId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateWorkSmallClass(WorkSmallClassMaintObject workSmallClassMaintObject)
        {
            try
            {
                workSmallClassServiceImpl.AddOrUpdateWorkSmallClass(workSmallClassMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveWorkSmallClasss(IList<WorkSmallClassMaintObject> workSmallClassMaintObjects)
        {
            try
            {
                workSmallClassServiceImpl.RemoveWorkSmallClasss(workSmallClassMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            workSmallClassServiceImpl.Dispose();
        }
    }
}
