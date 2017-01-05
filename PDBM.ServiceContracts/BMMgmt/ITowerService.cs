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
    /// 铁塔资源服务接口
    /// </summary>
    [ServiceContract]
    public interface ITowerService : IDistributedService
    {
        /// <summary>
        /// 根据铁塔资源Id获取任务
        /// </summary>
        /// <param name="id">铁塔资源Id</param>
        /// <returns>铁塔资源维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        TowerMaintObject GetTowerById(Guid id);

        /// <summary>
        /// 新增或者修改铁塔资源
        /// </summary>
        /// <param name="towerMaintObject">要新增或者修改的铁塔资源维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateTower(TowerMaintObject towerMaintObject);

        /// <summary>
        /// 删除铁塔资源
        /// </summary>
        /// <param name="towerMaintObjects">要删除的铁塔资源维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveTower(IList<TowerMaintObject> towerMaintObjects);
    }
}
