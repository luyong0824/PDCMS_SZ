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
    /// 网格服务接口
    /// </summary>
    [ServiceContract]
    public interface IReseauService : IDistributedService
    {
        /// <summary>
        /// 根据网格Id获取网格
        /// </summary>
        /// <param name="id">网格Id</param>
        /// <returns>网格维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        ReseauMaintObject GetReseauById(Guid id);

        /// <summary>
        /// 根据区域Id获取网格列表
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <returns>网格维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ReseauMaintObject> GetReseaus(Guid areaId);

        /// <summary>
        /// 根据区域Id获取网格列表
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <returns>网格维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetAllReseaus(Guid areaId);

        /// <summary>
        /// 根据区域Id获取状态为使用的网格列表
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <returns>网格选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ReseauSelectObject> GetUsedReseaus(Guid areaId);

        /// <summary>
        /// 获取所有状态为使用的网格列表
        /// </summary>
        /// <returns>网格选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<ReseauSelectObject> GetAllUsedReseaus();

        /// <summary>
        /// 新增或者修改网格
        /// </summary>
        /// <param name="reseauMaintObject">要新增或者修改的网格维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateReseau(ReseauMaintObject reseauMaintObject);

        /// <summary>
        /// 删除网格
        /// </summary>
        /// <param name="reseauMaintObjects">要删除的网格维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveReseaus(IList<ReseauMaintObject> reseauMaintObjects);
    }
}
