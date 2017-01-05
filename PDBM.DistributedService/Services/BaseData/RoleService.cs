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
    /// 角色分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class RoleService : IRoleService
    {
        private readonly IRoleService roleServiceImpl = ServiceLocator.Instance.GetService<IRoleService>();

        public RoleMaintObject GetRoleById(Guid id)
        {
            try
            {
                return roleServiceImpl.GetRoleById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<RoleMaintObject> GetRoles()
        {
            try
            {
                return roleServiceImpl.GetRoles();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<RoleSelectObject> GetUsedRoles()
        {
            try
            {
                return roleServiceImpl.GetUsedRoles();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateRole(RoleMaintObject roleMaintObject)
        {
            try
            {
                roleServiceImpl.AddOrUpdateRole(roleMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveRoles(IList<RoleMaintObject> roleMaintObjects)
        {
            try
            {
                roleServiceImpl.RemoveRoles(roleMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            roleServiceImpl.Dispose();
        }
    }
}
