using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ServiceContracts.BMMgmt
{
    /// <summary>
    /// 机房服务接口
    /// </summary>
    [ServiceContract]
    public interface IMachineRoomService : IDistributedService
    {
        /// <summary>
        /// 根据机房Id获取任务
        /// </summary>
        /// <param name="id">机房Id</param>
        /// <returns>机房维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        MachineRoomMaintObject GetMachineRoomById(Guid id);

        /// <summary>
        /// 新增或者修改机房
        /// </summary>
        /// <param name="machineRoomMaintObject">要新增或者修改的机房维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateMachineRoom(MachineRoomMaintObject machineRoomMaintObject);

        /// <summary>
        /// 删除机房
        /// </summary>
        /// <param name="towerMaintObjects">要删除的机房维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveMachineRoom(IList<MachineRoomMaintObject> machineRoomMaintObjects);
    }
}
