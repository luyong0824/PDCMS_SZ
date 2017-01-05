using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Common;
using System.ServiceModel;
using PDBM.DataTransferObjects.BMMgmt;

namespace PDBM.ServiceContracts.BaseData
{
    [ServiceContract]
    public interface IPlacePropertyService : IDistributedService
    {
        /// <summary>
        /// 根据站点属性Id获取站点属性
        /// </summary>
        /// <param name="id">站点属性Id</param>
        /// <returns>站点属性维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlacePropertyMaintObject GetPlacePropertyById(Guid id);

        /// <summary>
        /// 新增或者修改站点属性
        /// </summary>
        /// <param name="placePropertyMaintObject">要新增或者修改的站点属性维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdatePlaceProperty(PlacePropertyMaintObject placePropertyMaintObject);

        /// <summary>
        /// 删除站点属性
        /// </summary>
        /// <param name="placePropertyMaintObjects"></param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemovePlaceProperty(IList<PlacePropertyMaintObject> placePropertyMaintObjects);

        /// <summary>
        /// 根据站点Id获取站点资源维护对象
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns>资源维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ResourceMaintObject GetResourceMaintenanceById(Guid id);

        /// <summary>
        /// 保存站点资源维护对象
        /// </summary>
        /// <param name="reseauMaintObject">资源维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void SaveResourceMaintenance(ResourceMaintObject resourceMaintObject);
    }
}
