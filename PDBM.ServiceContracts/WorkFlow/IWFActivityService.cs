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
    /// 工作流活动服务接口
    /// </summary>
    [ServiceContract]
    public interface IWFActivityService : IDistributedService
    {
        /// <summary>
        /// 根据工作流活动Id获取工作流活动
        /// </summary>
        /// <param name="id">工作流活动Id</param>
        /// <returns>工作流活动维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WFActivityMaintObject GetWFActivityById(Guid id);

        /// <summary>
        /// 根据工作流过程Id获取工作流活动列表
        /// </summary>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <returns>工作流活动列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFActivitys(Guid wfProcessId);

        /// <summary>
        /// 根据工作流过程Id，用户Id获取工作流活动列表，用于发送工作流实例
        /// </summary>
        /// <param name="wfProcessId">工作流过程Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="entityId">发送单据实体Id</param>
        /// <returns>工作流活动列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetWFActivitysBySend(Guid wfProcessId, Guid userId, Guid entityId);

        /// <summary>
        /// 新增或者修改工作流活动
        /// </summary>
        /// <param name="wfActivityMaintObjects">要新增或者修改的工作流活动维护对象列表</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateWFActivitys(IList<WFActivityMaintObject> wfActivityMaintObjects, Guid wfProcessId);

        /// <summary>
        /// 删除工作流活动
        /// </summary>
        /// <param name="wfActivityMaintObjects">要删除的工作流活动维护对象列表</param>
        /// <param name="wfProcessId">工作流过程Id</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveWFActivitys(IList<WFActivityMaintObject> wfActivityMaintObjects, Guid wfProcessId);
    }
}
