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
    /// 工作流活动编辑器服务接口
    /// </summary>
    [ServiceContract]
    public interface IWFActivityEditorService : IDistributedService
    {
        /// <summary>
        /// 根据工作流类型Id获取状态为使用的工作流活动编辑器列表
        /// </summary>
        /// <param name="wfCategoryId">工作流类型Id</param>
        /// <returns>工作流活动编辑器选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<WFActivityEditorSelectObject> GetUsedWFActivityEditors(Guid wfCategoryId);
    }
}
