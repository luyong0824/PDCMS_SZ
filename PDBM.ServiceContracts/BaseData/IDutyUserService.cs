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
    /// 职务用户服务接口
    /// </summary>
    [ServiceContract]
    public interface IDutyUserService : IDistributedService
    {
        /// <summary>
        /// 根据公司Id，部门Id，职务Id获取职务用户列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="duty">职务Id</param>
        /// <returns>职务用户列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetDutyUsers(Guid companyId, Guid departmentId, int duty);

        /// <summary>
        /// 新增或者删除职务用户
        /// </summary>
        /// <param name="dutyUserMaintObjects">要新增或者删除的职务用户维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrRemoveDutyUsers(IList<DutyUserMaintObject> dutyUserMaintObjects);
    }
}
