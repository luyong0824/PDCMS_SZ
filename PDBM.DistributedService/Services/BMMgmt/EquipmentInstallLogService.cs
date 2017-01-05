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
    /// 设备安装历史记录分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class EquipmentInstallLogService : IEquipmentInstallLogService
    {
        private readonly IEquipmentInstallLogService equipmentInstallLogServiceImpl = ServiceLocator.Instance.GetService<IEquipmentInstallLogService>();

        public void AddOrUpdateEquipmentInstallLog(EquipmentInstallMaintObject equipmentInstallMaintObject)
        {
            try
            {
                equipmentInstallLogServiceImpl.AddOrUpdateEquipmentInstallLog(equipmentInstallMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetEquipmentInstallLog(int propertyType, Guid parentId)
        {
            try
            {
                return equipmentInstallLogServiceImpl.GetEquipmentInstallLog(propertyType, parentId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            equipmentInstallLogServiceImpl.Dispose();
        }
    }
}
