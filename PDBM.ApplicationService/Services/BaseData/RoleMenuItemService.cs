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
    /// 角色菜单应用层服务
    /// </summary>
    public class RoleMenuItemService : DataService, IRoleMenuItemService
    {
        private readonly IRepository<RoleMenuItem> roleMenuItemRepository;

        public RoleMenuItemService(IRepositoryContext context,
            IRepository<RoleMenuItem> roleMenuItemRepository)
            : base(context)
        {
            this.roleMenuItemRepository = roleMenuItemRepository;
        }

        /// <summary>
        /// 根据角色Id，菜单Id获取角色菜单列表
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns>角色菜单列表的Json字符串</returns>
        public string GetRoleMenuItems(Guid roleId, Guid menuId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "RoleId", Type = SqlDbType.UniqueIdentifier, Value = roleId });
            parameters.Add(new Parameter() { Name = "MenuId", Type = SqlDbType.UniqueIdentifier, Value = menuId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryRoleMenuItems", parameters))
            {
                dt.Columns.Add("isLeaf", typeof(bool), "Convert(IsLeafStr, 'System.Boolean')");
                dt.Columns.Add("asyncLoad", typeof(bool), "Convert(AsyncLoadStr, 'System.Boolean')");
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 新增或者删除角色菜单
        /// </summary>
        /// <param name="roleMenuItemMaintObjects">要新增或者删除的角色菜单维护对象列表</param>
        public void AddOrRemoveRoleMenuItems(IList<RoleMenuItemMaintObject> roleMenuItemMaintObjects)
        {
            foreach (var roleMenuItemMaintObject in roleMenuItemMaintObjects)
            {
                if (roleMenuItemMaintObject.Id == Guid.Empty)
                {
                    RoleMenuItem roleMenuItem = AggregateFactory.CreateRoleMenuItem(roleMenuItemMaintObject.RoleId,
                        roleMenuItemMaintObject.MenuItemId, roleMenuItemMaintObject.CreateUserId);
                    roleMenuItemRepository.Add(roleMenuItem);
                }
                else
                {
                    RoleMenuItem roleMenuItem = roleMenuItemRepository.FindByKey(roleMenuItemMaintObject.Id);
                    if (roleMenuItem != null)
                    {
                        roleMenuItemRepository.Remove(roleMenuItem);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_RoleIdMenuItemId"))
                {
                    throw new ApplicationFault("角色菜单重复添加");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_RoleMenuItem_dbo.tbl_Role_RoleId"))
                {
                    throw new ApplicationFault("选择的角色在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_RoleMenuItem_dbo.tbl_MenuItem_MenuItemId"))
                {
                    throw new ApplicationFault("选择的菜单在系统中不存在");
                }
                throw ex;
            }
        }
    }
}
