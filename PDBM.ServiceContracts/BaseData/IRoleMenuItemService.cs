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
    /// 角色菜单服务接口
    /// </summary>
    [ServiceContract]
    public interface IRoleMenuItemService : IDistributedService
    {
        /// <summary>
        /// 根据角色Id，菜单Id获取角色菜单列表
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns>角色菜单列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetRoleMenuItems(Guid roleId, Guid menuId);

        /// <summary>
        /// 新增或者删除角色菜单
        /// </summary>
        /// <param name="roleMenuItemMaintObjects">要新增或者删除的角色菜单维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrRemoveRoleMenuItems(IList<RoleMenuItemMaintObject> roleMenuItemMaintObjects);
    }
}
