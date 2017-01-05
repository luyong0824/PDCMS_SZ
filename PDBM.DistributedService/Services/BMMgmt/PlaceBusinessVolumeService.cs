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
    /// 站点业务量分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PlaceBusinessVolumeService : IPlaceBusinessVolumeService
    {
        private readonly IPlaceBusinessVolumeService placeBusinessVolumeServiceImpl = ServiceLocator.Instance.GetService<IPlaceBusinessVolumeService>();

        public void AddOrUpdatePlaceBusinessVolume(PlaceBusinessVolumeMaintObject placeBusinessVolumeMaintObject)
        {
            try
            {
                placeBusinessVolumeServiceImpl.AddOrUpdatePlaceBusinessVolume(placeBusinessVolumeMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            placeBusinessVolumeServiceImpl.Dispose();
        }
    }
}
