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
    /// 站点类型服务接口
    /// </summary>
    [ServiceContract]
    public interface IPlaceCategoryService : IDistributedService
    {
        /// <summary>
        /// 根据站点类型Id获取站点类型
        /// </summary>
        /// <param name="id">站点类型Id</param>
        /// <returns>站点类型维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PlaceCategoryMaintObject GetPlaceCategoryById(Guid id);

        /// <summary>
        /// 根据专业获取站点类型列表
        /// </summary>
        /// <param name="profession">专业</param>
        /// <returns>站点类型维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<PlaceCategoryMaintObject> GetPlaceCategorys(int profession);

        /// <summary>
        /// 根据专业获取状态为使用的站点类型列表
        /// </summary>
        /// <param name="profession">专业</param>
        /// <returns>站点类型选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<PlaceCategorySelectObject> GetUsedPlaceCategorys(int profession);

        /// <summary>
        /// 新增或者修改站点类型
        /// </summary>
        /// <param name="placeCategoryMaintObject">要新增或者修改的站点类型维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdatePlaceCategory(PlaceCategoryMaintObject placeCategoryMaintObject);

        /// <summary>
        /// 删除站点类型
        /// </summary>
        /// <param name="placeCategoryMaintObjects">要删除的站点类型维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemovePlaceCategorys(IList<PlaceCategoryMaintObject> placeCategoryMaintObjects);
    }
}
