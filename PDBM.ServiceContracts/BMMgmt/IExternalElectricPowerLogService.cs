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
    /// 外电引入历史记录服务接口
    /// </summary>
    [ServiceContract]
    public interface IExternalElectricPowerLogService : IDistributedService
    {
        /// <summary>
        /// 新增或者修改外电引入
        /// </summary>
        /// <param name="externalElectricPowerMaintObject">要新增或者修改的外电引入维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateExternalElectricPowerLog(ExternalElectricPowerMaintObject externalElectricPowerMaintObject);

        /// <summary>
        /// 根据资源类型获取外电引入历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetExternalElectricPowerLog(int propertyType, Guid parentId);
    }
}
