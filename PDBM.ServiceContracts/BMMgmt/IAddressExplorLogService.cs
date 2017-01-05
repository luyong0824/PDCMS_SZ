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
    /// 地质勘探历史记录服务接口
    /// </summary>
    [ServiceContract]
    public interface IAddressExplorLogService : IDistributedService
    {
        /// <summary>
        /// 新增或者修改地质勘探
        /// </summary>
        /// <param name="addressExplorMaintObject">要新增或者修改的地质勘探维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateAddressExplorLog(AddressExplorMaintObject addressExplorMaintObject);

        /// <summary>
        /// 根据资源类型获取地质勘探历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetAddressExplorLog(int propertyType, Guid parentId);
    }
}
