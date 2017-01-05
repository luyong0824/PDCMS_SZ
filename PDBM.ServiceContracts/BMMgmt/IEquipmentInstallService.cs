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
    /// 设备安装服务接口
    /// </summary>
    [ServiceContract]
    public interface IEquipmentInstallService : IDistributedService
    {
        /// <summary>
        /// 根据设备安装Id获取任务
        /// </summary>
        /// <param name="id">设备安装Id</param>
        /// <returns>设备安装维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        EquipmentInstallMaintObject GetEquipmentInstallById(Guid id);

        /// <summary>
        /// 新增或者修改设备安装
        /// </summary>
        /// <param name="equipmentInstallMaintObject">要新增或者修改的设备安装维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateEquipmentInstall(EquipmentInstallMaintObject equipmentInstallMaintObject);

        /// <summary>
        /// 删除设备安装
        /// </summary>
        /// <param name="towerMaintObjects">要删除的设备安装维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveEquipmentInstall(IList<EquipmentInstallMaintObject> equipmentInstallMaintObjects);
    }
}
