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
    /// 机房分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MachineRoomService : IMachineRoomService
    {
        private readonly IMachineRoomService machineRoomServiceImpl = ServiceLocator.Instance.GetService<IMachineRoomService>();

        public MachineRoomMaintObject GetMachineRoomById(Guid id)
        {
            try
            {
                return machineRoomServiceImpl.GetMachineRoomById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateMachineRoom(MachineRoomMaintObject machineRoomMaintObject)
        {
            try
            {
                machineRoomServiceImpl.AddOrUpdateMachineRoom(machineRoomMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveMachineRoom(IList<MachineRoomMaintObject> machineRoomMaintObjects)
        {
            try
            {
                machineRoomServiceImpl.RemoveMachineRoom(machineRoomMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            machineRoomServiceImpl.Dispose();
        }
    }
}
