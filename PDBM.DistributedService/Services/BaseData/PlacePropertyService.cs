using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;
using System.ServiceModel;
using PDBM.DataTransferObjects.BMMgmt;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 站点属性分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PlacePropertyService : IPlacePropertyService
    {
        private readonly IPlacePropertyService placePropertyServiceImpl = ServiceLocator.Instance.GetService<IPlacePropertyService>();

        public PlacePropertyMaintObject GetPlacePropertyById(Guid id)
        {
            try
            {
                return placePropertyServiceImpl.GetPlacePropertyById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdatePlaceProperty(PlacePropertyMaintObject placePropertyMaintObject)
        {
            try
            {
                placePropertyServiceImpl.AddOrUpdatePlaceProperty(placePropertyMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemovePlaceProperty(IList<PlacePropertyMaintObject> placePropertyMaintObjects)
        {
            try
            {
                placePropertyServiceImpl.RemovePlaceProperty(placePropertyMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public ResourceMaintObject GetResourceMaintenanceById(Guid id)
        {
            try
            {
                return placePropertyServiceImpl.GetResourceMaintenanceById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveResourceMaintenance(ResourceMaintObject resourceMaintObject)
        {
            try
            {
                placePropertyServiceImpl.SaveResourceMaintenance(resourceMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            placePropertyServiceImpl.Dispose();
        }
    }
}
