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
    /// 铁塔资源历史记录服务接口
    /// </summary>
    [ServiceContract]
    public interface ITowerLogService : IDistributedService
    {
        /// <summary>
        /// 新增或者修改铁塔资源
        /// </summary>
        /// <param name="towerMaintObject">要新增或者修改的铁塔资源维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateTowerLog(TowerMaintObject towerMaintObject);

        /// <summary>
        /// 根据资源类型获取铁塔历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetTowerLog(int propertyType, Guid parentId);
    }
}
