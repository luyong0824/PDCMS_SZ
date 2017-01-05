using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 产权分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PlaceOwnerService : IPlaceOwnerService
    {
        private readonly IPlaceOwnerService placeOwnerServiceImpl = ServiceLocator.Instance.GetService<IPlaceOwnerService>();

        public PlaceOwnerMaintObject GetPlaceOwnerById(Guid id)
        {
            try
            {
                return placeOwnerServiceImpl.GetPlaceOwnerById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<PlaceOwnerMaintObject> GetPlaceOwners()
        {
            try
            {
                return placeOwnerServiceImpl.GetPlaceOwners();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<PlaceOwnerSelectObject> GetUsedPlaceOwners()
        {
            try
            {
                return placeOwnerServiceImpl.GetUsedPlaceOwners();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdatePlaceOwner(PlaceOwnerMaintObject placeOwnerMaintObject)
        {
            try
            {
                placeOwnerServiceImpl.AddOrUpdatePlaceOwner(placeOwnerMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemovePlaceOwners(IList<PlaceOwnerMaintObject> placeOwnerMaintObjects)
        {
            try
            {
                placeOwnerServiceImpl.RemovePlaceOwners(placeOwnerMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            placeOwnerServiceImpl.Dispose();
        }
    }
}
