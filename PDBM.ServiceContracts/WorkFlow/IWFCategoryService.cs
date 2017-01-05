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
    /// 工作流类型服务接口
    /// </summary>
    [ServiceContract]
    public interface IWFCategoryService : IDistributedService
    {
        /// <summary>
        /// 根据工作流类型Id获取工作流类型
        /// </summary>
        /// <param name="id">工作流类型Id</param>
        /// <returns>工作流类型选择对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        WFCategorySelectObject GetWFCategoryById(Guid id);

        /// <summary>
        /// 获取状态为使用的工作流类型列表
        /// </summary>
        /// <returns>工作流类型选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<WFCategorySelectObject> GetUsedWFCategorys();
    }
}
