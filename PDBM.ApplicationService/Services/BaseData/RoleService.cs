using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 角色应用层服务
    /// </summary>
    public class RoleService : DataService, IRoleService
    {
        private readonly IRepository<Role> roleRepository;
        private readonly IRepository<RoleMenuItem> roleMenuItemRepository;
        private readonly IRepository<RoleUser> roleUserRepository;

        public RoleService(IRepositoryContext context,
            IRepository<Role> roleRepository,
            IRepository<RoleMenuItem> roleMenuItemRepository,
            IRepository<RoleUser> roleUserRepository)
            : base(context)
        {
            this.roleRepository = roleRepository;
            this.roleMenuItemRepository = roleMenuItemRepository;
            this.roleUserRepository = roleUserRepository;
        }

        /// <summary>
        /// 根据角色Id获取角色
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns>角色维护对象</returns>
        public RoleMaintObject GetRoleById(Guid id)
        {
            Role role = roleRepository.FindByKey(id);
            if (role != null)
            {
                RoleMaintObject roleMaintObject = MapperHelper.Map<Role, RoleMaintObject>(role);
                return roleMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的角色在系统中不存在");
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns>角色维护对象列表</returns>
        public IList<RoleMaintObject> GetRoles()
        {
            IList<RoleMaintObject> roleMaintObjects = new List<RoleMaintObject>();
            IEnumerable<Role> roles = roleRepository.FindAll(null, "RoleCode");
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    roleMaintObjects.Add(MapperHelper.Map<Role, RoleMaintObject>(role));
                }
            }
            return roleMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的角色列表
        /// </summary>
        /// <returns>角色选择对象列表</returns>
        public IList<RoleSelectObject> GetUsedRoles()
        {
            IList<RoleSelectObject> roleSelectObjects = new List<RoleSelectObject>();
            IEnumerable<Role> roles = roleRepository.FindAll(Specification<Role>.Eval(entity => entity.State == State.使用), "RoleCode");
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    roleSelectObjects.Add(MapperHelper.Map<Role, RoleSelectObject>(role));
                }
            }
            return roleSelectObjects;
        }

        /// <summary>
        /// 新增或者修改角色
        /// </summary>
        /// <param name="roleMaintObject">要新增或者修改的角色维护对象</param>
        public void AddOrUpdateRole(RoleMaintObject roleMaintObject)
        {
            if (roleMaintObject.Id == Guid.Empty)
            {
                Role role = AggregateFactory.CreateRole(roleMaintObject.RoleCode, roleMaintObject.RoleName,
                    roleMaintObject.Remarks, (State)roleMaintObject.State, roleMaintObject.CreateUserId);
                roleRepository.Add(role);
            }
            else
            {
                Role role = roleRepository.FindByKey(roleMaintObject.Id);
                if (role != null)
                {
                    role.Modify(roleMaintObject.RoleCode, roleMaintObject.RoleName, roleMaintObject.Remarks,
                        (State)roleMaintObject.State, roleMaintObject.ModifyUserId);
                    roleRepository.Update(role);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_RoleCode"))
                {
                    throw new ApplicationFault("角色编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_RoleName"))
                {
                    throw new ApplicationFault("角色名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleMaintObjects">要删除的角色维护对象列表</param>
        public void RemoveRoles(IList<RoleMaintObject> roleMaintObjects)
        {
            foreach (RoleMaintObject roleMaintObject in roleMaintObjects)
            {
                Role role = roleRepository.FindByKey(roleMaintObject.Id);
                if (role != null)
                {
                    if (roleMenuItemRepository.Exists(Specification<RoleMenuItem>.Eval(entity => entity.RoleId == role.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已存在角色菜单", role.RoleCode);
                    }
                    else if (roleUserRepository.Exists(Specification<RoleUser>.Eval(entity => entity.RoleId == role.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已存在角色用户", role.RoleCode);
                    }
                    roleRepository.Remove(role);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_RoleMenuItem_dbo.tbl_Role_RoleId"))
                {
                    throw new ApplicationFault("已存在角色菜单");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_RoleUser_dbo.tbl_Role_RoleId"))
                {
                    throw new ApplicationFault("已存在角色用户");
                }
                throw ex;
            }
        }
    }
}
