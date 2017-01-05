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
    /// 外电引入服务接口
    /// </summary>
    [ServiceContract]
    public interface IExternalElectricPowerService : IDistributedService
    {
        /// <summary>
        /// 根据外电引入Id获取任务
        /// </summary>
        /// <param name="id">外电引入Id</param>
        /// <returns>外电引入维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ExternalElectricPowerMaintObject GetExternalElectricPowerById(Guid id);

        /// <summary>
        /// 新增或者修改外电引入
        /// </summary>
        /// <param name="externalElectricPowerMaintObject">要新增或者修改的外电引入维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateExternalElectricPower(ExternalElectricPowerMaintObject externalElectricPowerMaintObject);

        /// <summary>
        /// 删除外电引入
        /// </summary>
        /// <param name="towerMaintObjects">要删除的外电引入维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveExternalElectricPower(IList<ExternalElectricPowerMaintObject> externalElectricPowerMaintObjects);
    }
}
