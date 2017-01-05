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
    /// 派工大类服务接口
    /// </summary>
    [ServiceContract]
    public interface IWorkBigClassService : IDistributedService
    {
        /// <summary>
        /// 根据派工大类Id获取派工大类
        /// </summary>
        /// <param name="id">派工大类Id</param>
        /// <returns>派工大类维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WorkBigClassMaintObject GetWorkBigClassById(Guid id);

        /// <summary>
        /// 获取派工大类列表
        /// </summary>
        /// <returns>派工大类维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<WorkBigClassMaintObject> GetWorkBigClasss();

        /// <summary>
        /// 获取状态为使用的派工大类列表
        /// </summary>
        /// <returns>派工大类选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<WorkBigClassSelectObject> GetUsedWorkBigClasss();

        /// <summary>
        /// 新增或者修改派工大类
        /// </summary>
        /// <param name="workBigClassMaintObject">要新增或者修改的派工大类维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateWorkBigClass(WorkBigClassMaintObject workBigClassMaintObject);

        /// <summary>
        /// 删除派工大类
        /// </summary>
        /// <param name="workBigClassMaintObjects">要删除的派工大类维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveWorkBigClasss(IList<WorkBigClassMaintObject> workBigClassMaintObjects);
    }
}
