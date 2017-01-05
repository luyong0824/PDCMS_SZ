using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.BaseData
{
    /// <summary>
    /// 产权服务接口
    /// </summary>
    [ServiceContract]
    public interface IPlaceOwnerService:IDistributedService
    {
        /// <summary>
        /// 根据产权Id获取产权
        /// </summary>
        /// <param name="id">产权Id</param>
        /// <returns>产权维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlaceOwnerMaintObject GetPlaceOwnerById(Guid id);

        /// <summary>
        /// 获取产权列表
        /// </summary>
        /// <returns>产权维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<PlaceOwnerMaintObject> GetPlaceOwners();

        /// <summary>
        /// 获取状态为使用的产权列表
        /// </summary>
        /// <returns>产权选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<PlaceOwnerSelectObject> GetUsedPlaceOwners();

        /// <summary>
        /// 新增或者修改产权
        /// </summary>
        /// <param name="placeOwnerMaintObject">要新增或者修改的产权维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdatePlaceOwner(PlaceOwnerMaintObject placeOwnerMaintObject);

        /// <summary>
        /// 删除产权
        /// </summary>
        /// <param name="placeOwnerMaintObjects">要删除的产权维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemovePlaceOwners(IList<PlaceOwnerMaintObject> placeOwnerMaintObjects);
    }
}
