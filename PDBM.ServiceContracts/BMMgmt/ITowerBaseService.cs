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
    /// 铁塔基础服务接口
    /// </summary>
    [ServiceContract]
    public interface ITowerBaseService : IDistributedService
    {
        /// <summary>
        /// 根据铁塔基础Id获取任务
        /// </summary>
        /// <param name="id">铁塔基础Id</param>
        /// <returns>铁塔基础维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        TowerBaseMaintObject GetTowerBaseById(Guid id);

        /// <summary>
        /// 新增或者修改铁塔基础
        /// </summary>
        /// <param name="towerBaseMaintObject">要新增或者修改的铁塔基础维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateTowerBase(TowerBaseMaintObject towerBaseMaintObject);

        /// <summary>
        /// 删除铁塔基础
        /// </summary>
        /// <param name="towerMaintObjects">要删除的铁塔基础维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveTowerBase(IList<TowerBaseMaintObject> towerBaseMaintObjects);
    }
}
