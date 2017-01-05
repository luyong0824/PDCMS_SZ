using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 角色用户应用层服务
    /// </summary>
    public class RoleUserService : DataService, IRoleUserService
    {
        private readonly IRepository<RoleUser> roleUserRepository;

        public RoleUserService(IRepositoryContext context,
            IRepository<RoleUser> roleUserRepository)
            : base(context)
        {
            this.roleUserRepository = roleUserRepository;
        }

        /// <summary>
        /// 根据公司Id，部门Id，角色Id获取角色用户列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns>角色用户列表的Json字符串</returns>
        public string GetRoleUsers(Guid companyId, Guid departmentId, Guid roleId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "RoleId", Type = SqlDbType.UniqueIdentifier, Value = roleId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryRoleUsers", parameters))
            {
                dt.Columns.Add("isLeaf", typeof(bool), "Convert(IsLeafStr, 'System.Boolean')");
                dt.Columns.Add("asyncLoad", typeof(bool), "Convert(AsyncLoadStr, 'System.Boolean')");
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 新增或者删除角色用户
        /// </summary>
        /// <param name="roleUserMaintObjects">要新增或者删除的角色用户维护对象列表</param>
        public void AddOrRemoveRoleUsers(IList<RoleUserMaintObject> roleUserMaintObjects)
        {
            foreach (var roleUserMaintObject in roleUserMaintObjects)
            {
                if (roleUserMaintObject.Id == Guid.Empty)
                {
                    RoleUser roleUser = AggregateFactory.CreateRoleUser(roleUserMaintObject.RoleId,
                        roleUserMaintObject.UserId, roleUserMaintObject.CreateUserId);
                    roleUserRepository.Add(roleUser);
                }
                else
                {
                    RoleUser roleUser = roleUserRepository.FindByKey(roleUserMaintObject.Id);
                    if (roleUser != null)
                    {
                        roleUserRepository.Remove(roleUser);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_RoleIdUserId"))
                {
                    throw new ApplicationFault("角色用户重复添加");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_RoleUser_dbo.tbl_Role_RoleId"))
                {
                    throw new ApplicationFault("选择的角色在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_RoleUser_dbo.tbl_User_UserId"))
                {
                    throw new ApplicationFault("选择的用户在系统中不存在");
                }
                throw ex;
            }
        }
    }
}
