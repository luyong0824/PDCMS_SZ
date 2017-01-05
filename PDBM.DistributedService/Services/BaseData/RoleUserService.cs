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
    /// 角色用户分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class RoleUserService : IRoleUserService
    {
        private readonly IRoleUserService roleUserServiceImpl = ServiceLocator.Instance.GetService<IRoleUserService>();

        public string GetRoleUsers(Guid companyId, Guid departmentId, Guid roleId)
        {
            try
            {
                return roleUserServiceImpl.GetRoleUsers(companyId, departmentId, roleId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrRemoveRoleUsers(IList<RoleUserMaintObject> roleUserMaintObjects)
        {
            try
            {
                roleUserServiceImpl.AddOrRemoveRoleUsers(roleUserMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            roleUserServiceImpl.Dispose();
        }
    }
}
