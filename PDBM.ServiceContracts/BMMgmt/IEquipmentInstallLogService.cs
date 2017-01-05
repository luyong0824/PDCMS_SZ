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
    /// 设备安装历史记录服务接口
    /// </summary>
    [ServiceContract]
    public interface IEquipmentInstallLogService : IDistributedService
    {
        /// <summary>
        /// 新增或者修改设备安装
        /// </summary>
        /// <param name="equipmentInstallMaintObject">要新增或者修改的设备安装维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateEquipmentInstallLog(EquipmentInstallMaintObject equipmentInstallMaintObject);

        /// <summary>
        /// 根据资源类型获取设备安装历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetEquipmentInstallLog(int propertyType, Guid parentId);
    }
}
