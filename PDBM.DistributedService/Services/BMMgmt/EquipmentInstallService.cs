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
    /// 设备安装分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class EquipmentInstallService : IEquipmentInstallService
    {
        private readonly IEquipmentInstallService equipmentInstallServiceImpl = ServiceLocator.Instance.GetService<IEquipmentInstallService>();

        public EquipmentInstallMaintObject GetEquipmentInstallById(Guid id)
        {
            try
            {
                return equipmentInstallServiceImpl.GetEquipmentInstallById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateEquipmentInstall(EquipmentInstallMaintObject equipmentInstallMaintObject)
        {
            try
            {
                equipmentInstallServiceImpl.AddOrUpdateEquipmentInstall(equipmentInstallMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveEquipmentInstall(IList<EquipmentInstallMaintObject> equipmentInstallMaintObjects)
        {
            try
            {
                equipmentInstallServiceImpl.RemoveEquipmentInstall(equipmentInstallMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            equipmentInstallServiceImpl.Dispose();
        }
    }
}
