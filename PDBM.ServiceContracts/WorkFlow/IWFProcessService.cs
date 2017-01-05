using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.WorkFlow
{
    /// <summary>
    /// 工作流过程服务接口
    /// </summary>
    [ServiceContract]
    public interface IWFProcessService : IDistributedService
    {
        /// <summary>
        /// 根据工作流过程Id获取工作流过程
        /// </summary>
        /// <param name="id">工作流过程Id</param>
        /// <returns>工作流过程维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WFProcessMaintObject GetWFProcessById(Guid id);

        /// <summary>
        /// 获取工作流过程列表
        /// </summary>
        /// <returns>工作流过程列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFProcesses();

        /// <summary>
        /// 获取状态为使用的工作流过程列表
        /// </summary>
        /// <returns>工作流过程选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<WFProcessSelectObject> GetUsedWFProcesses();

        /// <summary>
        /// 根据工作流类型Id获取状态为使用的工作流过程列表
        /// </summary>
        /// <param name="wfCategoryId">工作流类型Id</param>
        /// <returns>工作流过程选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<WFProcessSelectObject> GetUsedWFProcessesByWFCategoryId(Guid wfCategoryId);

        /// <summary>
        /// 新增或者修改工作流过程
        /// </summary>
        /// <param name="wfProcessMaintObject">要新增或者修改的工作流过程维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateWFProcess(WFProcessMaintObject wfProcessMaintObject);

        /// <summary>
        /// 删除工作流过程
        /// </summary>
        /// <param name="wfProcessMaintObjects">要删除的工作流过程维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveWFProcesses(IList<WFProcessMaintObject> wfProcessMaintObjects);
    }
}
