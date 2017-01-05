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
    /// 往来单位用户服务接口
    /// </summary>
    [ServiceContract]
    public interface ICustomerUserService : IDistributedService
    {
        /// <summary>
        /// 根据公司Id，部门Id，往来单位Id获取岗位用户列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="customerId">往来单位Id</param>
        /// <returns>往来单位用户列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetCustomerUsers(Guid companyId, Guid departmentId, Guid customerId);

        /// <summary>
        /// 新增或者删除往来单位用户
        /// </summary>
        /// <param name="customerUserMaintObjects">要新增或者删除的往来单位用户维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrRemoveCustomerUsers(IList<CustomerUserMaintObject> customerUserMaintObjects);
    }
}
