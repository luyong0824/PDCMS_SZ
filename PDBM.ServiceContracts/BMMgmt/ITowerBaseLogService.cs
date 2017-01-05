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
    /// 铁塔基础历史记录服务接口
    /// </summary>
    [ServiceContract]
    public interface ITowerBaseLogService : IDistributedService
    {
        /// <summary>
        /// 新增或者修改铁塔基础
        /// </summary>
        /// <param name="towerBaseMaintObject">要新增或者修改的铁塔基础维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateTowerBaseLog(TowerBaseMaintObject towerBaseMaintObject);

        /// <summary>
        /// 根据资源类型获取铁塔基础历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetTowerBaseLog(int propertyType, Guid parentId);
    }
}
