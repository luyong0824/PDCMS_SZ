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
    /// 桩基动测服务接口
    /// </summary>
    [ServiceContract]
    public interface IFoundationTestService : IDistributedService
    {
        /// <summary>
        /// 根据桩基动测Id获取任务
        /// </summary>
        /// <param name="id">桩基动测Id</param>
        /// <returns>桩基动测维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        FoundationTestMaintObject GetFoundationTestById(Guid id);

        /// <summary>
        /// 新增或者修改桩基动测
        /// </summary>
        /// <param name="foundationTestMaintObject">要新增或者修改的桩基动测维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateFoundationTest(FoundationTestMaintObject foundationTestMaintObject);

        /// <summary>
        /// 删除桩基动测
        /// </summary>
        /// <param name="towerMaintObjects">要删除的桩基动测维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveFoundationTest(IList<FoundationTestMaintObject> foundationTestMaintObjects);
    }
}
