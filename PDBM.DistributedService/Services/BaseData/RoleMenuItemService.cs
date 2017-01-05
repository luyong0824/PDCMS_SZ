using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 角色菜单分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class RoleMenuItemService : IRoleMenuItemService
    {
        private readonly IRoleMenuItemService roleMenuItemServiceImpl = ServiceLocator.Instance.GetService<IRoleMenuItemService>();

        public string GetRoleMenuItems(Guid roleId, Guid menuId)
        {
            try
            {
                return roleMenuItemServiceImpl.GetRoleMenuItems(roleId, menuId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrRemoveRoleMenuItems(IList<RoleMenuItemMaintObject> roleMenuItemMaintObjects)
        {
            try
            {
                roleMenuItemServiceImpl.AddOrRemoveRoleMenuItems(roleMenuItemMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            roleMenuItemServiceImpl.Dispose();
        }
    }
}
