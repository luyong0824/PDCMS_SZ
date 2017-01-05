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
    /// 改造分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class RemodelingService : IRemodelingService
    {
        private readonly IRemodelingService remodelingServiceImpl = ServiceLocator.Instance.GetService<IRemodelingService>();

        public RemodelingMaintObject GetRemodelingById(Guid id)
        {
            try
            {
                return remodelingServiceImpl.GetRemodelingById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetRemodelingsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string placeName,
            int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int projectType, int orderState, Guid createUserId)
        {
            try
            {
                return remodelingServiceImpl.GetRemodelingsPage(pageIndex, pageSize, beginDate, endDate, placeName, profession, placeCategoryId, areaId, reseauId, projectType, orderState, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateRemodeling(RemodelingMaintObject remodelingMaintObject)
        {
            try
            {
                remodelingServiceImpl.AddOrUpdateRemodeling(remodelingMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveRemodelings(IList<RemodelingMaintObject> remodelingMaintObjects)
        {
            try
            {
                remodelingServiceImpl.RemoveRemodelings(remodelingMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public RemodelingPrintObject GetRemodelingPrintById(Guid id)
        {
            try
            {
                return remodelingServiceImpl.GetRemodelingPrintById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            remodelingServiceImpl.Dispose();
        }
    }
}
