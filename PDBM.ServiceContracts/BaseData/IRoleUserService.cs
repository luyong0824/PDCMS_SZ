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
    /// 角色用户服务接口
    /// </summary>
    [ServiceContract]
    public interface IRoleUserService : IDistributedService
    {
        /// <summary>
        /// 根据公司Id，部门Id，角色Id获取角色用户列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns>角色用户列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetRoleUsers(Guid companyId, Guid departmentId, Guid roleId);

        /// <summary>
        /// 新增或者删除角色用户
        /// </summary>
        /// <param name="roleUserMaintObjects">要新增或者删除的角色用户维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrRemoveRoleUsers(IList<RoleUserMaintObject> roleUserMaintObjects);
    }
}
