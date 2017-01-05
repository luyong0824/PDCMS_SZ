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
    /// 工单小类服务接口
    /// </summary>
    [ServiceContract]
    public interface IWorkSmallClassService : IDistributedService
    {
        /// <summary>
        /// 根据工单小类Id获取工单小类
        /// </summary>
        /// <param name="id">工单小类Id</param>
        /// <returns>工单小类维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WorkSmallClassMaintObject GetWorkSmallClassById(Guid id);

        /// <summary>
        /// 根据区域Id获取工单小类列表
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <returns>工单小类维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<WorkSmallClassMaintObject> GetWorkSmallClasss(Guid areaId);

        /// <summary>
        /// 根据工单大类Id获取状态为使用的工单小类列表
        /// </summary>
        /// <param name="workBigClassId">工单大类Id</param>
        /// <returns>工单小类选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<WorkSmallClassSelectObject> GetUsedWorkSmallClass(Guid workBigClassId);

        /// <summary>
        /// 新增或者修改工单小类
        /// </summary>
        /// <param name="workSmallClassMaintObject">要新增或者修改的工单小类维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateWorkSmallClass(WorkSmallClassMaintObject workSmallClassMaintObject);

        /// <summary>
        /// 删除工单小类
        /// </summary>
        /// <param name="workSmallClassMaintObjects">要删除的工单小类维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveWorkSmallClasss(IList<WorkSmallClassMaintObject> workSmallClassMaintObjects);
    }
}
