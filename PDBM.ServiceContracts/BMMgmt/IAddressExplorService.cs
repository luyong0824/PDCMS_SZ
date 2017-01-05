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
    /// 地质勘探服务接口
    /// </summary>
    [ServiceContract]
    public interface IAddressExplorService : IDistributedService
    {
        /// <summary>
        /// 根据地质勘探Id获取任务
        /// </summary>
        /// <param name="id">地质勘探Id</param>
        /// <returns>地质勘探维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        AddressExplorMaintObject GetAddressExplorById(Guid id);

        /// <summary>
        /// 新增或者修改地质勘探
        /// </summary>
        /// <param name="addressExplorMaintObject">要新增或者修改的地质勘探维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateAddressExplor(AddressExplorMaintObject addressExplorMaintObject);

        /// <summary>
        /// 删除地质勘探
        /// </summary>
        /// <param name="towerMaintObjects">要删除的地质勘探维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveAddressExplor(IList<AddressExplorMaintObject> addressExplorMaintObjects);
    }
}
