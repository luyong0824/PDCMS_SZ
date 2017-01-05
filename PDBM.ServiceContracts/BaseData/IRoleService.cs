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
    /// 角色服务接口
    /// </summary>
    [ServiceContract]
    public interface IRoleService : IDistributedService
    {
        /// <summary>
        /// 根据角色Id获取角色
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns>角色维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        RoleMaintObject GetRoleById(Guid id);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns>角色维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<RoleMaintObject> GetRoles();

        /// <summary>
        /// 获取状态为使用的角色列表
        /// </summary>
        /// <returns>角色选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<RoleSelectObject> GetUsedRoles();

        /// <summary>
        /// 新增或者修改角色
        /// </summary>
        /// <param name="roleMaintObject">要新增或者修改的角色维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateRole(RoleMaintObject roleMaintObject);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleMaintObjects">要删除的角色维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveRoles(IList<RoleMaintObject> roleMaintObjects);
    }
}
