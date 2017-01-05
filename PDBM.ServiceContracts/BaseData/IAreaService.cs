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
    /// 区域服务接口
    /// </summary>
    [ServiceContract]
    public interface IAreaService : IDistributedService
    {
        /// <summary>
        /// 根据区域Id获取区域
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <returns>区域维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        AreaMaintObject GetAreaById(Guid id);

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns>区域维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<AreaMaintObject> GetAreas();

        /// <summary>
        /// 获取状态为使用的区域列表
        /// </summary>
        /// <returns>区域选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<AreaSelectObject> GetUsedAreas();

        /// <summary>
        /// 新增或者修改区域
        /// </summary>
        /// <param name="areaMaintObject">要新增或者修改的区域维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateArea(AreaMaintObject areaMaintObject);

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="areaMaintObjects">要删除的区域维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveAreas(IList<AreaMaintObject> areaMaintObjects);

        /// <summary>
        /// 获取所有区域
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetAllAreas(int state);
    }
}
