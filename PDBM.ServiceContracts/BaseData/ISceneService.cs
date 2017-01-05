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
    /// 周边场景服务接口
    /// </summary>
    [ServiceContract]
    public interface ISceneService : IDistributedService
    {
        /// <summary>
        /// 根据周边场景Id获取周边场景
        /// </summary>
        /// <param name="id">周边场景Id</param>
        /// <returns>周边场景维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        SceneMaintObject GetSceneById(Guid id);

        /// <summary>
        /// 获取周边场景列表
        /// </summary>
        /// <returns>周边场景维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<SceneMaintObject> GetScenes();

        /// <summary>
        /// 获取状态为使用的周边场景列表
        /// </summary>
        /// <returns>周边场景选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<SceneSelectObject> GetUsedScenes();

        /// <summary>
        /// 新增或者修改周边场景
        /// </summary>
        /// <param name="sceneMaintObject">要新增或者修改的周边场景维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateScene(SceneMaintObject sceneMaintObject);

        /// <summary>
        /// 删除周边场景
        /// </summary>
        /// <param name="sceneMaintObjects">要删除的周边场景维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveScenes(IList<SceneMaintObject> sceneMaintObjects);
    }
}
