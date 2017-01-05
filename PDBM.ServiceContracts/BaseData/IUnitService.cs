using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ServiceContracts.BaseData
{
    /// <summary>
    /// 计量单位分布式服务接口
    /// </summary>
    [ServiceContract]
    public interface IUnitService : IDistributedService
    {
        /// <summary>
        /// 根据计量单位Id获取计量单位
        /// </summary>
        /// <param name="id">计量单位Id</param>
        /// <returns>计量单位维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        UnitMaintObject GetUnitById(Guid id);

        /// <summary>
        /// 获取计量单位列表
        /// </summary>
        /// <returns>计量单位维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<UnitMaintObject> GetUnits();

        /// <summary>
        /// 获取状态为使用的计量单位列表
        /// </summary>
        /// <returns>计量单位选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<UnitMaintObject> GetUsedUnits();

        /// <summary>
        /// 新增或者修改计量单位
        /// </summary>
        /// <param name="unitMaintObject">要新增或者修改的计量单位维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateUnit(UnitMaintObject unitMaintObject);

        /// <summary>
        /// 删除计量单位
        /// </summary>
        /// <param name="unitMaintObjects">要删除的计量单位维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveUnits(IList<UnitMaintObject> unitMaintObjects);
    }
}
