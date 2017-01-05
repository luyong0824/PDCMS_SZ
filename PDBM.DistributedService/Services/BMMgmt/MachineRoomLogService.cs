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
    /// 机房历史记录分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MachineRoomLogService : IMachineRoomLogService
    {
        private readonly IMachineRoomLogService machineRoomLogServiceImpl = ServiceLocator.Instance.GetService<IMachineRoomLogService>();

        public void AddOrUpdateMachineRoomLog(MachineRoomMaintObject machineRoomMaintObject)
        {
            try
            {
                machineRoomLogServiceImpl.AddOrUpdateMachineRoomLog(machineRoomMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetMachineRoomLog(int propertyType, Guid parentId)
        {
            try
            {
                return machineRoomLogServiceImpl.GetMachineRoomLog(propertyType, parentId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            machineRoomLogServiceImpl.Dispose();
        }
    }
}
