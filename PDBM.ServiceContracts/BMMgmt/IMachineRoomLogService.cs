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
    /// 机房历史记录服务接口
    /// </summary>
    [ServiceContract]
    public interface IMachineRoomLogService : IDistributedService
    {
        /// <summary>
        /// 新增或者修改机房
        /// </summary>
        /// <param name="machineRoomMaintObject">要新增或者修改的机房维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateMachineRoomLog(MachineRoomMaintObject machineRoomMaintObject);

        /// <summary>
        /// 根据资源类型获取机房历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetMachineRoomLog(int propertyType, Guid parentId);
    }
}
