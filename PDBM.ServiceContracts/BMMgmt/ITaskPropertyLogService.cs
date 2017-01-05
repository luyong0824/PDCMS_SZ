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
    /// 子任务历史记录服务接口
    /// </summary>
    [ServiceContract]
    public interface ITaskPropertyLogService : IDistributedService
    {
        /// <summary>
        /// 新增或者修改任务属性
        /// </summary>
        /// <param name="taskPropertyMaintObject">要新增或者修改的任务属性维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]

        void AddOrUpdateTaskPropertyLog(TaskPropertyMaintObject taskPropertyMaintObject);

        /// <summary>
        /// 获取子任务历史记录
        /// </summary>
        /// <param name="taskModel">工程名称</param>
        /// <param name="parentId">任务Id</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetTaskPropertyLog(int taskModel,Guid parentId);
    }
}
