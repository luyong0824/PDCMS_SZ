using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Communication;
using PDBM.Infrastructure.IoC;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using PDBM.ServiceContracts.Enum;
using PDBM.Web.Filters;
using PDBM.ServiceContracts.BMMgmt;
using PDBM.DataTransferObjects.BMMgmt;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 基础数据控制器
    /// </summary>
    [AuthorizeFilter]
    public class BaseDataController : BaseController
    {
        private const int PROFESSION = 1;

        #region 公司

        /// <summary>
        /// 公司
        /// </summary>
        /// <returns></returns>
        public ActionResult Company()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyNature"] = JsonHelper.Encode(enumService.GetCompanyNatureEnum());
            return View();
        }

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetCompanys()
        {
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetCompanys()), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 部门

        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Department()
        {
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
                companySelectObjects.Add(new CompanySelectObject() { Id = Guid.Empty, CompanyName = "公司", PId = Guid.NewGuid(), isLeaf = false });
                ViewData["CompanysTree"] = JsonHelper.Encode(companySelectObjects);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据部门Id获取部门
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetDepartmentById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetDepartmentById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDepartments()
        {
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }

            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetDepartments(Guid.Parse(Request["CompanyId"])));
            }
        }

        /// <summary>
        /// 根据公司Id获取状态为使用的部门列表
        /// </summary>
        /// <param name="id">公司Id</param>
        /// <param name="getType">获取类型</param>
        /// <returns></returns>
        public async Task<ActionResult> GetUsedDepartments(Guid id, int getType)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (getType != 1 && getType != 2 && getType != 3)
            {
                throw new ArgumentException("无效的getType");
            }

            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                IList<DepartmentSelectObject> departmentSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedDepartments(id));
                if (getType == 1)
                {
                    departmentSelectObjects.Insert(0, new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "请选择" });
                }
                else if (getType == 2)
                {
                    departmentSelectObjects.Insert(0, new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "全部" });
                }
                else if (getType == 3)
                {
                    departmentSelectObjects.Insert(0, new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "全部部门" });
                }
                return Json(departmentSelectObjects, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据公司Id获取状态为使用的部门列表树形结构
        /// </summary>
        /// <param name="id">公司Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetUsedDepartmentsByTree(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                IList<DepartmentSelectObject> departmentSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedDepartments(id));
                departmentSelectObjects.Add(new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "部门", PId = Guid.NewGuid(), isLeaf = false });
                return Json(departmentSelectObjects, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据公司Id,岗位Id获取状态为使用的部门列表，用于发送公文
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetUsedDepartmentsBySend()
        {
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["PostId"] == null)
            {
                throw new ArgumentNullException("PostId");
            }

            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetUsedDepartmentsBySend(Guid.Parse(Request["CompanyId"]), Guid.Parse(Request["PostId"])));
            }
        }

        /// <summary>
        /// 保存部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveDepartment()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            DepartmentMaintObject departmentMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                departmentMaintObject = new DepartmentMaintObject()
                {
                    Id = Guid.Empty,
                    CompanyId = Guid.Parse(row["CompanyId"].ToString()),
                    DepartmentCode = row["DepartmentCode"].ToString().Trim(),
                    DepartmentName = row["DepartmentName"].ToString().Trim(),
                    ManagerUserId = Guid.Parse(row["ManagerUserId"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                departmentMaintObject = new DepartmentMaintObject()
                {
                    Id = id,
                    DepartmentCode = row["DepartmentCode"].ToString().Trim(),
                    DepartmentName = row["DepartmentName"].ToString().Trim(),
                    ManagerUserId = Guid.Parse(row["ManagerUserId"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateDepartment(departmentMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveDepartments()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<DepartmentMaintObject> departmentMaintObjects = new List<DepartmentMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    departmentMaintObjects.Add(new DepartmentMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveDepartments(departmentMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 用户账号

        /// <summary>
        /// 根据用户Id获取用户账号
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetUserAccountById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetUserById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据部门Id获取状态为使用的用户列表
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <param name="getType">获取类型</param>
        /// <returns></returns>
        public async Task<ActionResult> GetUsedUsers(Guid id, int getType)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (getType != 1 && getType != 2 && getType != 3)
            {
                throw new ArgumentException("无效的getType");
            }

            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                IList<UserSelectObject> userSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedUsers(id));
                if (getType == 1)
                {
                    userSelectObjects.Insert(0, new UserSelectObject() { Id = Guid.Empty, FullName = "请选择" });
                }
                else if (getType == 2)
                {
                    userSelectObjects.Insert(0, new UserSelectObject() { Id = Guid.Empty, FullName = "全部" });
                }
                else if (getType == 3)
                {
                    userSelectObjects.Insert(0, new UserSelectObject() { Id = Guid.Empty, FullName = "全部用户" });
                }
                return Json(userSelectObjects, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 用户账号
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> UserAccount()
        {
            ViewData["CompanyId"] = this.CompanyId;
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumStateList = enumService.GetStateEnum();
            ViewData["State"] = JsonHelper.Encode(enumStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumStateList.Insert(0, allDict);
            ViewData["StateByAll"] = JsonHelper.Encode(enumStateList);
            return View();
        }

        /// <summary>
        /// 获取分页用户账号
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetUserAccountsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["DepartmentId"] == null)
            {
                throw new ArgumentNullException("DepartmentId");
            }
            if (Request["UserName"] == null)
            {
                throw new ArgumentNullException("UserName");
            }
            if (Request["FullName"] == null)
            {
                throw new ArgumentNullException("FullName");
            }
            if (Request["Email"] == null)
            {
                throw new ArgumentNullException("Email");
            }
            if (Request["PhoneNumber"] == null)
            {
                throw new ArgumentNullException("PhoneNumber");
            }
            if (Request["State"] == null)
            {
                throw new ArgumentNullException("State");
            }

            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetUsersPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Guid.Parse(Request["CompanyId"]), Guid.Parse(Request["DepartmentId"]), Request["UserName"].Trim(), Request["FullName"].Trim(),
                    Request["Email"].Trim(), Request["PhoneNumber"].Trim(), int.Parse(Request["State"])));
            }
        }

        /// <summary>
        /// 获取状态为使用的分页用户账号，用于选择用户
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetUsedUserAccountsPageBySelect()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["DepartmentId"] == null)
            {
                throw new ArgumentNullException("DepartmentId");
            }
            if (Request["FullName"] == null)
            {
                throw new ArgumentNullException("FullName");
            }

            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetUsedUsersPageBySelect(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Guid.Parse(Request["CompanyId"]), Guid.Parse(Request["DepartmentId"]), Request["FullName"].Trim()));
            }
        }

        /// <summary>
        /// 根据部门Id，岗位Id获取状态为使用的用户账号列表，用于发送公文
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetUsedUsersBySend()
        {
            if (Request["DepartmentId"] == null)
            {
                throw new ArgumentNullException("DepartmentId");
            }
            if (Request["PostId"] == null)
            {
                throw new ArgumentNullException("PostId");
            }

            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetUsedUsersBySend(Guid.Parse(Request["DepartmentId"]), Guid.Parse(Request["PostId"])));
            }
        }

        /// <summary>
        /// 保存用户账号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveUserAccount()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            UserMaintObject userMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                userMaintObject = new UserMaintObject()
                {
                    Id = Guid.Empty,
                    DepartmentId = Guid.Parse(row["DepartmentId"].ToString()),
                    UserName = row["UserName"].ToString().Trim(),
                    FullName = row["FullName"].ToString().Trim(),
                    Email = row["Email"].ToString().Trim(),
                    PhoneNumber = row["PhoneNumber"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    UniqueCode = Guid.NewGuid(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                userMaintObject = new UserMaintObject()
                {
                    Id = id,
                    UserName = row["UserName"].ToString().Trim(),
                    FullName = row["FullName"].ToString().Trim(),
                    Email = row["Email"].ToString().Trim(),
                    PhoneNumber = row["PhoneNumber"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateUser(userMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除用户账号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveUserAccounts()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<UserMaintObject> userMaintObjects = new List<UserMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    userMaintObjects.Add(new UserMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveUsers(userMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 用户信息

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfo()
        {
            return View();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetUserInfo()
        {
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetUserInfo(this.UserId)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveUserInfo()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            UserInfoMaintObject userInfoMaintObject = new UserInfoMaintObject()
            {
                Id = this.UserId,
                FullName = row["FullName"].ToString().Trim(),
                Email = row["Email"].ToString().Trim(),
                PhoneNumber = row["PhoneNumber"].ToString().Trim(),
                OldUserPassword = row["OldUserPassword"].ToString().Trim(),
                NewUserPassword = row["NewUserPassword"].ToString().Trim(),
                ConfirmNewUserPassword = row["ConfirmNewUserPassword"].ToString().Trim()
            };
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.UpdateUserInfo(userInfoMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 角色

        /// <summary>
        /// 角色
        /// </summary>
        /// <returns></returns>
        public ActionResult Role()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据角色Id获取角色
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetRoleById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IRoleService> proxy = new ServiceProxy<IRoleService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetRoleById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetRoles()
        {
            using (ServiceProxy<IRoleService> proxy = new ServiceProxy<IRoleService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetRoles()), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveRole()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            RoleMaintObject roleMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                roleMaintObject = new RoleMaintObject()
                {
                    Id = Guid.Empty,
                    RoleCode = row["RoleCode"].ToString().Trim(),
                    RoleName = row["RoleName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                roleMaintObject = new RoleMaintObject()
                {
                    Id = id,
                    RoleCode = row["RoleCode"].ToString().Trim(),
                    RoleName = row["RoleName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IRoleService> proxy = new ServiceProxy<IRoleService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateRole(roleMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveRoles()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<RoleMaintObject> roleMaintObjects = new List<RoleMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    roleMaintObjects.Add(new RoleMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IRoleService> proxy = new ServiceProxy<IRoleService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveRoles(roleMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 角色菜单

        /// <summary>
        /// 角色菜单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> RoleMenuItem()
        {
            using (ServiceProxy<IMenuService> proxy = new ServiceProxy<IMenuService>())
            {
                IList<MenuSelectObject> menuSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetMenus());
                menuSelectObjects.Insert(0, new MenuSelectObject() { Id = Guid.Empty, MenuName = "全部菜单" });
                ViewData["Menus"] = JsonHelper.Encode(menuSelectObjects);
            }
            using (ServiceProxy<IRoleService> proxy = new ServiceProxy<IRoleService>())
            {
                IList<RoleSelectObject> roleSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedRoles());
                roleSelectObjects.Add(new RoleSelectObject() { Id = Guid.Empty, RoleName = "角色", PId = Guid.NewGuid(), isLeaf = false });
                ViewData["Roles"] = JsonHelper.Encode(roleSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取角色菜单列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRoleMenuItems()
        {
            if (Request["RoleId"] == null)
            {
                throw new ArgumentNullException("RoleId");
            }
            if (Request["MenuId"] == null)
            {
                throw new ArgumentNullException("MenuId");
            }

            using (ServiceProxy<IRoleMenuItemService> proxy = new ServiceProxy<IRoleMenuItemService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetRoleMenuItems(Guid.Parse(Request["RoleId"]), Guid.Parse(Request["MenuId"])));
            }
        }

        /// <summary>
        /// 保存角色菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveRoleMenuItems()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }
            if (Request["RoleId"] == null)
            {
                throw new ArgumentNullException("RoleId");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            Guid roleId = Guid.Parse(Request["RoleId"]);
            IList<RoleMenuItemMaintObject> roleMenuItemMaintObjects = new List<RoleMenuItemMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                roleMenuItemMaintObjects.Add(new RoleMenuItemMaintObject()
                {
                    Id = Guid.Parse(row["RoleMenuItemId"].ToString()),
                    RoleId = roleId,
                    MenuItemId = Guid.Parse(row["Id"].ToString()),
                    CreateUserId = this.UserId
                });
            }
            using (ServiceProxy<IRoleMenuItemService> proxy = new ServiceProxy<IRoleMenuItemService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrRemoveRoleMenuItems(roleMenuItemMaintObjects));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 角色用户

        /// <summary>
        /// 角色用户
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> RoleUser()
        {
            ViewData["CompanyId"] = this.CompanyId;
            using (ServiceProxy<IRoleService> proxy = new ServiceProxy<IRoleService>())
            {
                IList<RoleSelectObject> roleSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedRoles());
                roleSelectObjects.Add(new RoleSelectObject() { Id = Guid.Empty, RoleName = "角色", PId = Guid.NewGuid(), isLeaf = false });
                ViewData["Roles"] = JsonHelper.Encode(roleSelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取角色用户列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRoleUsers()
        {
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["DepartmentId"] == null)
            {
                throw new ArgumentNullException("DepartmentId");
            }
            if (Request["RoleId"] == null)
            {
                throw new ArgumentNullException("RoleId");
            }

            using (ServiceProxy<IRoleUserService> proxy = new ServiceProxy<IRoleUserService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetRoleUsers(Guid.Parse(Request["CompanyId"]), Guid.Parse(Request["DepartmentId"]), Guid.Parse(Request["RoleId"])));
            }
        }

        /// <summary>
        /// 保存角色用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveRoleUsers()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }
            if (Request["RoleId"] == null)
            {
                throw new ArgumentNullException("RoleId");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            Guid roleId = Guid.Parse(Request["RoleId"]);
            IList<RoleUserMaintObject> roleUserMaintObjects = new List<RoleUserMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                roleUserMaintObjects.Add(new RoleUserMaintObject()
                {
                    Id = Guid.Parse(row["RoleUserId"].ToString()),
                    RoleId = roleId,
                    UserId = Guid.Parse(row["Id"].ToString()),
                    CreateUserId = this.UserId
                });
            }
            using (ServiceProxy<IRoleUserService> proxy = new ServiceProxy<IRoleUserService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrRemoveRoleUsers(roleUserMaintObjects));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 岗位

        /// <summary>
        /// 岗位
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据岗位Id获取岗位
        /// </summary>
        /// <param name="id">岗位Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPostById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPostService> proxy = new ServiceProxy<IPostService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPostById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取岗位列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPosts()
        {
            using (ServiceProxy<IPostService> proxy = new ServiceProxy<IPostService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPosts()), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存岗位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePost()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PostMaintObject postMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                postMaintObject = new PostMaintObject()
                {
                    Id = Guid.Empty,
                    PostCode = row["PostCode"].ToString().Trim(),
                    PostName = row["PostName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                postMaintObject = new PostMaintObject()
                {
                    Id = id,
                    PostCode = row["PostCode"].ToString().Trim(),
                    PostName = row["PostName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPostService> proxy = new ServiceProxy<IPostService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdatePost(postMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePosts()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PostMaintObject> postMaintObjects = new List<PostMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    postMaintObjects.Add(new PostMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IPostService> proxy = new ServiceProxy<IPostService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemovePosts(postMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 岗位用户

        /// <summary>
        /// 岗位用户
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PostUser()
        {
            ViewData["CompanyId"] = this.CompanyId;
            using (ServiceProxy<IPostService> proxy = new ServiceProxy<IPostService>())
            {
                IList<PostSelectObject> postSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPosts());
                postSelectObjects.Add(new PostSelectObject() { Id = Guid.Empty, PostName = "岗位", PId = Guid.NewGuid(), isLeaf = false });
                ViewData["Posts"] = JsonHelper.Encode(postSelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取岗位用户列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPostUsers()
        {
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["DepartmentId"] == null)
            {
                throw new ArgumentNullException("DepartmentId");
            }
            if (Request["PostId"] == null)
            {
                throw new ArgumentNullException("PostId");
            }

            using (ServiceProxy<IPostUserService> proxy = new ServiceProxy<IPostUserService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPostUsers(Guid.Parse(Request["CompanyId"]), Guid.Parse(Request["DepartmentId"]), Guid.Parse(Request["PostId"])));
            }
        }

        /// <summary>
        /// 保存岗位用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePostUsers()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }
            if (Request["PostId"] == null)
            {
                throw new ArgumentNullException("PostId");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            Guid postId = Guid.Parse(Request["PostId"]);
            IList<PostUserMaintObject> postUserMaintObjects = new List<PostUserMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                postUserMaintObjects.Add(new PostUserMaintObject()
                {
                    Id = Guid.Parse(row["PostUserId"].ToString()),
                    PostId = postId,
                    UserId = Guid.Parse(row["Id"].ToString()),
                    CreateUserId = this.UserId
                });
            }
            using (ServiceProxy<IPostUserService> proxy = new ServiceProxy<IPostUserService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrRemovePostUsers(postUserMaintObjects));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 会计主体

        /// <summary>
        /// 会计主体
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountingEntity()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据会计主体Id获取会计主体
        /// </summary>
        /// <param name="id">会计主体Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetAccountingEntityById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IAccountingEntityService> proxy = new ServiceProxy<IAccountingEntityService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetAccountingEntityById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取会计主体列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetAccountingEntitys()
        {
            using (ServiceProxy<IAccountingEntityService> proxy = new ServiceProxy<IAccountingEntityService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetAccountingEntitys()), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存会计主体
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAccountingEntity()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            AccountingEntityMaintObject accountingEntityMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                accountingEntityMaintObject = new AccountingEntityMaintObject()
                {
                    Id = Guid.Empty,
                    AccountingEntityCode = row["AccountingEntityCode"].ToString().Trim(),
                    AccountingEntityName = row["AccountingEntityName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                accountingEntityMaintObject = new AccountingEntityMaintObject()
                {
                    Id = id,
                    AccountingEntityCode = row["AccountingEntityCode"].ToString().Trim(),
                    AccountingEntityName = row["AccountingEntityName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IAccountingEntityService> proxy = new ServiceProxy<IAccountingEntityService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateAccountingEntity(accountingEntityMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除会计主体
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveAccountingEntitys()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<AccountingEntityMaintObject> accountingEntityMaintObjects = new List<AccountingEntityMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    accountingEntityMaintObjects.Add(new AccountingEntityMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IAccountingEntityService> proxy = new ServiceProxy<IAccountingEntityService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveAccountingEntitys(accountingEntityMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 项目

        /// <summary>
        /// 项目
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Project()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectCategoryList = enumService.GetProjectCategoryEnum();
            IList<Dictionary<string, string>> enumProjectProgressList = enumService.GetProjectProgressEnum();
            IList<Dictionary<string, string>> enumStateList = enumService.GetStateEnum();

            ViewData["Profession"] = JsonHelper.Encode(enumService.GetProfessionEnum());
            ViewData["ProjectCategory"] = JsonHelper.Encode(enumProjectCategoryList);
            ViewData["ProjectProgress"] = JsonHelper.Encode(enumProjectProgressList);
            ViewData["State"] = JsonHelper.Encode(enumStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");

            enumProjectCategoryList.Insert(0, allDict);
            enumProjectProgressList.Insert(0, allDict);
            enumStateList.Insert(0, allDict);

            ViewData["ProjectCategoryByAll"] = JsonHelper.Encode(enumProjectCategoryList);
            ViewData["ProjectProgressByAll"] = JsonHelper.Encode(enumProjectProgressList);
            ViewData["StateByAll"] = JsonHelper.Encode(enumStateList);

            using (ServiceProxy<IAccountingEntityService> proxy = new ServiceProxy<IAccountingEntityService>())
            {
                IList<AccountingEntitySelectObject> accountingEntitySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAccountingEntitys());
                accountingEntitySelectObjects.Insert(0, new AccountingEntitySelectObject() { Id = Guid.Empty, AccountingEntityName = "请选择" });
                ViewData["AccountingEntitys"] = JsonHelper.Encode(accountingEntitySelectObjects);
                accountingEntitySelectObjects.RemoveAt(0);
                accountingEntitySelectObjects.Insert(0, new AccountingEntitySelectObject() { Id = Guid.Empty, AccountingEntityName = "全部" });
                ViewData["AccountingEntitysByAll"] = JsonHelper.Encode(accountingEntitySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据项目Id获取项目
        /// </summary>
        /// <param name="id">项目Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetProjectById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IProjectService> proxy = new ServiceProxy<IProjectService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetProjectById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页项目列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["ProjectCode"] == null)
            {
                throw new ArgumentNullException("ProjectCode");
            }
            if (Request["ProjectName"] == null)
            {
                throw new ArgumentNullException("ProjectName");
            }
            if (Request["ProjectFullName"] == null)
            {
                throw new ArgumentNullException("ProjectFullName");
            }
            //if (Request["ProjectCategory"] == null)
            //{
            //    throw new ArgumentNullException("ProjectCategory");
            //}
            //if (Request["AccountingEntityId"] == null)
            //{
            //    throw new ArgumentNullException("AccountingEntityId");
            //}
            if (Request["ProjectProgress"] == null)
            {
                throw new ArgumentNullException("ProjectProgress");
            }
            if (Request["State"] == null)
            {
                throw new ArgumentNullException("State");
            }

            using (ServiceProxy<IProjectService> proxy = new ServiceProxy<IProjectService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["ProjectCode"].Trim(), Request["ProjectName"].Trim(), Request["ProjectFullName"].Trim(), 0,
                    Guid.Empty, int.Parse(Request["ProjectProgress"]), int.Parse(Request["State"])));
            }
        }

        /// <summary>
        /// 获取分页项目列表，用于选择项目
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectsPageBySelect()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["ProjectCode"] == null)
            {
                throw new ArgumentNullException("ProjectCode");
            }
            if (Request["ProjectName"] == null)
            {
                throw new ArgumentNullException("ProjectName");
            }
            if (Request["ProjectFullName"] == null)
            {
                throw new ArgumentNullException("ProjectFullName");
            }
            if (Request["AccountingEntityId"] == null)
            {
                throw new ArgumentNullException("AccountingEntityId");
            }
            if (Request["IsCheckedProjectProgress1"] == null)
            {
                throw new ArgumentNullException("IsCheckedProjectProgress1");
            }
            if (Request["IsCheckedProjectProgress2"] == null)
            {
                throw new ArgumentNullException("IsCheckedProjectProgress2");
            }
            if (Request["IsCheckedState1"] == null)
            {
                throw new ArgumentNullException("IsCheckedState1");
            }
            if (Request["IsCheckedState2"] == null)
            {
                throw new ArgumentNullException("IsCheckedState2");
            }

            using (ServiceProxy<IProjectService> proxy = new ServiceProxy<IProjectService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectsPageBySelect(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]), Request["ProjectCode"].Trim(),
                    Request["ProjectName"].Trim(), Request["ProjectFullName"].Trim(), Guid.Parse(Request["AccountingEntityId"]), Request["IsCheckedProjectProgress1"] == "true" ? 1 : 0, Request["IsCheckedProjectProgress2"] == "true" ? 1 : 0,
                    Request["IsCheckedState1"] == "true" ? 1 : 0, Request["IsCheckedState2"] == "true" ? 1 : 0));
            }
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProject()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectMaintObject projectMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                projectMaintObject = new ProjectMaintObject()
                {
                    Id = Guid.Empty,
                    ProjectCode = row["ProjectCode"].ToString().Trim(),
                    ProjectName = row["ProjectName"].ToString().Trim(),
                    ProjectFullName = row["ProjectFullName"].ToString().Trim(),
                    ProjectCategory = int.Parse(row["ProjectCategory"].ToString()),
                    AccountingEntityId = Guid.Parse(row["AccountingEntityId"].ToString()),
                    ManagerUserId = Guid.Parse(row["ManagerUserId"].ToString()),
                    ResponsibleUserId = Guid.Parse(row["ResponsibleUserId"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    ProjectProgress = int.Parse(row["ProjectProgress"].ToString()),
                    State = int.Parse(row["State"].ToString()),
                    ProfessionList = row["ProfessionList"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                projectMaintObject = new ProjectMaintObject()
                {
                    Id = id,
                    ProjectCode = row["ProjectCode"].ToString().Trim(),
                    ProjectName = row["ProjectName"].ToString().Trim(),
                    ProjectFullName = row["ProjectFullName"].ToString().Trim(),
                    ProjectCategory = int.Parse(row["ProjectCategory"].ToString()),
                    AccountingEntityId = Guid.Parse(row["AccountingEntityId"].ToString()),
                    ManagerUserId = Guid.Parse(row["ManagerUserId"].ToString()),
                    ResponsibleUserId = Guid.Parse(row["ResponsibleUserId"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    ProjectProgress = int.Parse(row["ProjectProgress"].ToString()),
                    State = int.Parse(row["State"].ToString()),
                    ProfessionList = row["ProfessionList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IProjectService> proxy = new ServiceProxy<IProjectService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateProject(projectMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveProjects()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<ProjectMaintObject> projectMaintObjects = new List<ProjectMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    projectMaintObjects.Add(new ProjectMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IProjectService> proxy = new ServiceProxy<IProjectService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveProjects(projectMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 区域

        /// <summary>
        /// 区域
        /// </summary>
        /// <returns></returns>
        public ActionResult Area()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据区域Id获取区域
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetAreaById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetAreaById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetAreas()
        {
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetAreas()), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存区域
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveArea()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            AreaMaintObject areaMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                areaMaintObject = new AreaMaintObject()
                {
                    Id = Guid.Empty,
                    AreaCode = row["AreaCode"].ToString().Trim(),
                    AreaName = row["AreaName"].ToString().Trim(),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    AreaManagerId = Guid.Parse(row["AreaManagerId"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                areaMaintObject = new AreaMaintObject()
                {
                    Id = id,
                    AreaCode = row["AreaCode"].ToString().Trim(),
                    AreaName = row["AreaName"].ToString().Trim(),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    AreaManagerId = Guid.Parse(row["AreaManagerId"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateArea(areaMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveAreas()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<AreaMaintObject> areaMaintObjects = new List<AreaMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    areaMaintObjects.Add(new AreaMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveAreas(areaMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAllAreas()
        {
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetAllAreas(0));
            }
        }

        #endregion

        #region 网格

        /// <summary>
        /// 网格
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Reseau()
        {
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["Areas"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "区域", PId = Guid.NewGuid(), isLeaf = false });
                ViewData["AreasTree"] = JsonHelper.Encode(areaSelectObjects);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据网格Id获取网格
        /// </summary>
        /// <param name="id">网格Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetReseauById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetReseauById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取网格列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAllReseaus()
        {
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetAllReseaus(Guid.Parse(Request["AreaId"])));
            }
        }

        /// <summary>
        /// 获取网格列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetReseaus()
        {
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetReseaus(Guid.Parse(Request["AreaId"]))), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据区域Id获取状态为使用的网格列表
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <param name="getType">获取类型</param>
        /// <returns></returns>
        public async Task<ActionResult> GetUsedReseaus(Guid id, int getType)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (getType != 1 && getType != 2)
            {
                throw new ArgumentException("无效的getType");
            }

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                IList<ReseauSelectObject> reseauSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedReseaus(id));
                if (getType == 1)
                {
                    reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "请选择" });
                }
                else if (getType == 2)
                {
                    reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "全部" });
                }
                return Json(reseauSelectObjects, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存网格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveReseau()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ReseauMaintObject reseauMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                reseauMaintObject = new ReseauMaintObject()
                {
                    Id = Guid.Empty,
                    AreaId = Guid.Parse(row["AreaId"].ToString()),
                    ReseauCode = row["ReseauCode"].ToString().Trim(),
                    ReseauName = row["ReseauName"].ToString().Trim(),
                    ReseauManagerId = Guid.Parse(row["ReseauManagerId"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                reseauMaintObject = new ReseauMaintObject()
                {
                    Id = id,
                    ReseauCode = row["ReseauCode"].ToString().Trim(),
                    ReseauName = row["ReseauName"].ToString().Trim(),
                    ReseauManagerId = Guid.Parse(row["ReseauManagerId"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateReseau(reseauMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除网格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveReseaus()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<ReseauMaintObject> reseauMaintObjects = new List<ReseauMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    reseauMaintObjects.Add(new ReseauMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveReseaus(reseauMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 周边场景

        /// <summary>
        /// 周边场景
        /// </summary>
        /// <returns></returns>
        public ActionResult Scene()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据周边场景Id获取周边场景
        /// </summary>
        /// <param name="id">周边场景Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetSceneById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetSceneById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取周边场景列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetScenes()
        {
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetScenes()), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存周边场景
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveScene()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            SceneMaintObject sceneMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                sceneMaintObject = new SceneMaintObject()
                {
                    Id = Guid.Empty,
                    SceneCode = row["SceneCode"].ToString().Trim(),
                    SceneName = row["SceneName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                sceneMaintObject = new SceneMaintObject()
                {
                    Id = id,
                    SceneCode = row["SceneCode"].ToString().Trim(),
                    SceneName = row["SceneName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateScene(sceneMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除周边场景
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveScenes()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<SceneMaintObject> sceneMaintObjects = new List<SceneMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    sceneMaintObjects.Add(new SceneMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveScenes(sceneMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 站点类型

        /// <summary>
        /// 站点类型
        /// </summary>
        /// <returns></returns>
        public ActionResult PlaceCategory()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            ViewData["Profession"] = JsonHelper.Encode(enumService.GetProfessionEnum());
            IList<Dictionary<string, object>> professionsTree = enumService.GetProfessionEnumByTree();
            Dictionary<string, object> root = new Dictionary<string, object>(5);
            root.Add("id", "0");
            root.Add("text", "专业");
            root.Add("pid", "null");
            root.Add("isLeaf", false);
            root.Add("asyncLoad", false);
            professionsTree.Insert(0, root);
            ViewData["ProfessionTree"] = JsonHelper.Encode(professionsTree);
            return View();
        }

        /// <summary>
        /// 获取专业
        /// </summary>
        /// <returns></returns>
        public string GetProfessions()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            return JsonHelper.Encode(enumService.GetProfessionEnum());
        }

        /// <summary>
        /// 根据站点类型Id获取站点类型
        /// </summary>
        /// <param name="id">站点类型Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlaceCategoryById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlaceCategoryById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取站点类型列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlaceCategorys()
        {
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlaceCategorys(int.Parse(Request["Profession"]))), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据专业获取状态为使用的站点类型列表
        /// </summary>
        /// <param name="id">专业</param>
        /// <param name="getType"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetUsedPlaceCategorys(int id, int getType)
        {
            if (id != 0 && id != 1 && id != 2)
            {
                throw new ArgumentException("无效的id");
            }
            if (getType != 1 && getType != 2)
            {
                throw new ArgumentException("无效的getType");
            }

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(id));
                if (getType == 1)
                {
                    placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                }
                else if (getType == 2)
                {
                    placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                }
                return Json(placeCategorySelectObjects, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存站点类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlaceCategory()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceCategoryMaintObject placeCategoryMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                placeCategoryMaintObject = new PlaceCategoryMaintObject()
                {
                    Id = Guid.Empty,
                    Profession = int.Parse(row["Profession"].ToString()),
                    PlaceCategoryCode = row["PlaceCategoryCode"].ToString().Trim(),
                    PlaceCategoryName = row["PlaceCategoryName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                placeCategoryMaintObject = new PlaceCategoryMaintObject()
                {
                    Id = id,
                    PlaceCategoryCode = row["PlaceCategoryCode"].ToString().Trim(),
                    PlaceCategoryName = row["PlaceCategoryName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdatePlaceCategory(placeCategoryMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除站点类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePlaceCategorys()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlaceCategoryMaintObject> placeCategoryMaintObjects = new List<PlaceCategoryMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    placeCategoryMaintObjects.Add(new PlaceCategoryMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemovePlaceCategorys(placeCategoryMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 站点

        /// <summary>
        /// 站点
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Place()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProfessionList = enumService.GetProfessionEnum();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumStateList = enumService.GetStateEnum();

            ViewData["Profession"] = JsonHelper.Encode(enumProfessionList);
            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["State"] = JsonHelper.Encode(enumStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumProfessionList.Insert(0, allDict);
            enumImportanceList.Insert(0, allDict);
            enumStateList.Insert(0, allDict);

            ViewData["ProfessionByAll"] = JsonHelper.Encode(enumProfessionList);
            ViewData["ImportanceByAll"] = JsonHelper.Encode(enumImportanceList);
            ViewData["StateByAll"] = JsonHelper.Encode(enumStateList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                IList<PlaceOwnerSelectObject> placeOwnerSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceOwners());
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "全部" });
                ViewData["PlaceOwnersByAll"] = JsonHelper.Encode(placeOwnerSelectObjects);
                placeOwnerSelectObjects.RemoveAt(0);
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "请选择" });
                ViewData["PlaceOwnersBySelect"] = JsonHelper.Encode(placeOwnerSelectObjects);
            }
            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                IList<DepartmentSelectObject> departmentSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedDepartments(this.CompanyId));
                departmentSelectObjects.Insert(0, new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "请选择" });
                ViewData["AddressingDepartmentsBySelect"] = JsonHelper.Encode(departmentSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据站点Id获取站点
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlaceById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlaceById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页站点列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlacesPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["PlaceOwner"] == null)
            {
                throw new ArgumentNullException("PlaceOwner");
            }
            if (Request["State"] == null)
            {
                throw new ArgumentNullException("State");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlacesPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    int.Parse(Request["Profession"]), Request["PlaceName"].Trim(), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                     Guid.Parse(Request["PlaceOwner"]), int.Parse(Request["State"])));
            }
        }

        /// <summary>
        /// 获取分页站点列表，用于选择站点
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlacesPageBySelect()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceCode"] == null)
            {
                throw new ArgumentNullException("PlaceCode");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlacesPageBySelect(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceCode"].Trim(), Request["PlaceName"].Trim(), int.Parse(Request["Profession"]), Guid.Parse(Request["PlaceCategoryId"]),
                    Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"])));
            }
        }

        /// <summary>
        /// 保存站点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlace()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceMaintObject placeMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id != Guid.Empty)
            {
                placeMaintObject = new PlaceMaintObject()
                {
                    Id = id,
                    //GroupPlaceCode = row["GroupPlaceCode"].ToString().Trim(),
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    //PropertyRight = int.Parse(row["PropertyRight"].ToString()),
                    Importance = int.Parse(row["Importance"].ToString()),
                    //SceneId = Guid.Parse(row["SceneId"].ToString()),
                    DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                    OwnerName = row["OwnerName"].ToString().Trim(),
                    OwnerContact = row["OwnerContact"].ToString().Trim(),
                    OwnerPhoneNumber = row["OwnerPhoneNumber"].ToString().Trim(),
                    //TelecomShare = bool.Parse(row["TelecomShare"].ToString()) ? 1 : 2,
                    //MobileShare = bool.Parse(row["MobileShare"].ToString()) ? 1 : 2,
                    //UnicomShare = bool.Parse(row["UnicomShare"].ToString()) ? 1 : 2,
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.UpdatePlace(placeMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 站点2

        /// <summary>
        /// 站点2
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Place2()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProfessionList = enumService.GetProfessionEnum();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumStateList = enumService.GetStateEnum();

            ViewData["Profession"] = JsonHelper.Encode(enumProfessionList);
            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["State"] = JsonHelper.Encode(enumStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumProfessionList.Insert(0, allDict);
            enumImportanceList.Insert(0, allDict);
            enumStateList.Insert(0, allDict);

            ViewData["ProfessionByAll"] = JsonHelper.Encode(enumProfessionList);
            ViewData["ImportanceByAll"] = JsonHelper.Encode(enumImportanceList);
            ViewData["StateByAll"] = JsonHelper.Encode(enumStateList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                IList<PlaceOwnerSelectObject> placeOwnerSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceOwners());
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "全部" });
                ViewData["PlaceOwnersByAll"] = JsonHelper.Encode(placeOwnerSelectObjects);
                placeOwnerSelectObjects.RemoveAt(0);
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "请选择" });
                ViewData["PlaceOwnersBySelect"] = JsonHelper.Encode(placeOwnerSelectObjects);
            }
            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                IList<DepartmentSelectObject> departmentSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedDepartments(this.CompanyId));
                departmentSelectObjects.Insert(0, new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "请选择" });
                ViewData["AddressingDepartmentsBySelect"] = JsonHelper.Encode(departmentSelectObjects);
            }
            return View();
        }
        #endregion

        #region 计量单位

        /// <summary>
        /// 计量单位
        /// </summary>
        /// <returns></returns>
        public ActionResult Unit()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据计量单位Id获取计量单位
        /// </summary>
        /// <param name="id">计量单位Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetUnitById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IUnitService> proxy = new ServiceProxy<IUnitService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetUnitById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取计量单位列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetUnits()
        {
            using (ServiceProxy<IUnitService> proxy = new ServiceProxy<IUnitService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetUnits()), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存计量单位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveUnit()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            UnitMaintObject unitMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                unitMaintObject = new UnitMaintObject()
                {
                    Id = Guid.Empty,
                    UnitName = row["UnitName"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                unitMaintObject = new UnitMaintObject()
                {
                    Id = id,
                    UnitName = row["UnitName"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IUnitService> proxy = new ServiceProxy<IUnitService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateUnit(unitMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除计量单位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveUnits()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<UnitMaintObject> unitMaintObjects = new List<UnitMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    unitMaintObjects.Add(new UnitMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IUnitService> proxy = new ServiceProxy<IUnitService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveUnits(unitMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 物资类别

        /// <summary>
        /// 物资类别
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterialCategory()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据物资类别Id获取物资类别
        /// </summary>
        /// <param name="id">物资类别Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetMaterialCategoryById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IMaterialCategoryService> proxy = new ServiceProxy<IMaterialCategoryService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMaterialCategoryById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取物资类别列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetMaterialCategorys()
        {
            using (ServiceProxy<IMaterialCategoryService> proxy = new ServiceProxy<IMaterialCategoryService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMaterialCategorys()), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存物资类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveMaterialCategory()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            MaterialCategoryMaintObject materialCategoryMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                materialCategoryMaintObject = new MaterialCategoryMaintObject()
                {
                    Id = Guid.Empty,
                    MaterialCategoryCode = row["MaterialCategoryCode"].ToString().Trim(),
                    MaterialCategoryName = row["MaterialCategoryName"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                materialCategoryMaintObject = new MaterialCategoryMaintObject()
                {
                    Id = id,
                    MaterialCategoryCode = row["MaterialCategoryCode"].ToString().Trim(),
                    MaterialCategoryName = row["MaterialCategoryName"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IMaterialCategoryService> proxy = new ServiceProxy<IMaterialCategoryService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateMaterialCategory(materialCategoryMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除物资类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveMaterialCategorys()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<MaterialCategoryMaintObject> materialCategoryMaintObjects = new List<MaterialCategoryMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    materialCategoryMaintObjects.Add(new MaterialCategoryMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IMaterialCategoryService> proxy = new ServiceProxy<IMaterialCategoryService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveMaterialCategorys(materialCategoryMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 物资名称

        /// <summary>
        /// 物资名称
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Material()
        {
            using (ServiceProxy<IMaterialCategoryService> proxy = new ServiceProxy<IMaterialCategoryService>())
            {
                IList<MaterialCategorySelectObject> materialCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedMaterialCategorys());
                materialCategorySelectObjects.Insert(0, new MaterialCategorySelectObject() { Id = Guid.Empty, MaterialCategoryName = "请选择" });
                ViewData["MaterialCategorys"] = JsonHelper.Encode(materialCategorySelectObjects);
                materialCategorySelectObjects.RemoveAt(0);
                materialCategorySelectObjects.Insert(0, new MaterialCategorySelectObject() { Id = Guid.Empty, MaterialCategoryName = "物资类别", PId = Guid.NewGuid(), isLeaf = false });
                ViewData["MaterialCategorysTree"] = JsonHelper.Encode(materialCategorySelectObjects);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据物资名称网格Id获取物资名称
        /// </summary>
        /// <param name="id">物资名称Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetMaterialById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IMaterialService> proxy = new ServiceProxy<IMaterialService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMaterialById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取物资名称列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetMaterials()
        {
            if (Request["MaterialCategoryId"] == null)
            {
                throw new ArgumentNullException("MaterialCategoryId");
            }

            using (ServiceProxy<IMaterialService> proxy = new ServiceProxy<IMaterialService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMaterials(Guid.Parse(Request["MaterialCategoryId"]))), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据物资类别Id获取状态为使用的物资名称列表
        /// </summary>
        /// <param name="id">物资类别Id</param>
        /// <param name="getType">获取类型</param>
        /// <returns></returns>
        public async Task<ActionResult> GetUsedMaterials(Guid id, int getType)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (getType != 1 && getType != 2)
            {
                throw new ArgumentException("无效的getType");
            }

            using (ServiceProxy<IMaterialService> proxy = new ServiceProxy<IMaterialService>())
            {
                IList<MaterialSelectObject> materialSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedMaterials(id));
                if (getType == 1)
                {
                    materialSelectObjects.Insert(0, new MaterialSelectObject() { Id = Guid.Empty, MaterialName = "请选择" });
                }
                else if (getType == 2)
                {
                    materialSelectObjects.Insert(0, new MaterialSelectObject() { Id = Guid.Empty, MaterialName = "全部" });
                }
                return Json(materialSelectObjects, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取物资名称列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetUsedMaterialsBySelf(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IMaterialService> proxy = new ServiceProxy<IMaterialService>())
            {
                IList<MaterialMaintObject> materialMaintObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedMaterialsBySelf(id));
                materialMaintObjects.Insert(0, new MaterialMaintObject() { Id = Guid.Empty, MaterialName = "请选择" });
                return Json(materialMaintObjects, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存物资名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveMaterial()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            MaterialMaintObject materialMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                materialMaintObject = new MaterialMaintObject()
                {
                    Id = Guid.Empty,
                    MaterialCategoryId = Guid.Parse(row["MaterialCategoryId"].ToString()),
                    MaterialCode = row["MaterialCode"].ToString().Trim(),
                    MaterialName = row["MaterialName"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                materialMaintObject = new MaterialMaintObject()
                {
                    Id = id,
                    MaterialCode = row["MaterialCode"].ToString().Trim(),
                    MaterialName = row["MaterialName"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IMaterialService> proxy = new ServiceProxy<IMaterialService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateMaterial(materialMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除物资名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveMaterials()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<MaterialMaintObject> materialMaintObjects = new List<MaterialMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    materialMaintObjects.Add(new MaterialMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IMaterialService> proxy = new ServiceProxy<IMaterialService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveMaterials(materialMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 设计规格

        /// <summary>
        /// 设计规格
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> MaterialSpec()
        {
            using (ServiceProxy<IMaterialService> proxy = new ServiceProxy<IMaterialService>())
            {
                ViewData["MaterialsTree"] = await Task.Factory.StartNew(() => proxy.Channel.GetAllUsedMaterials());
            }
            using (ServiceProxy<IUnitService> proxy = new ServiceProxy<IUnitService>())
            {
                IList<UnitMaintObject> unitMaintObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedUnits());
                unitMaintObjects.Insert(0, new UnitMaintObject() { Id = Guid.Empty, UnitName = "请选择" });
                ViewData["Units"] = JsonHelper.Encode(unitMaintObjects);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据设计规格Id获取设计规格
        /// </summary>
        /// <param name="id">设计规格Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetMaterialSpecById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IMaterialSpecService> proxy = new ServiceProxy<IMaterialSpecService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMaterialSpecById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取设计规格列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMaterialSpecs()
        {
            if (Request["MaterialId"] == null)
            {
                throw new ArgumentNullException("MaterialId");
            }

            using (ServiceProxy<IMaterialSpecService> proxy = new ServiceProxy<IMaterialSpecService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetMaterialSpecs(Guid.Parse(Request["MaterialId"])));
            }
        }

        /// <summary>
        /// 根据区域Id获取状态为使用的设计规格列表
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <param name="getType">获取类型</param>
        /// <returns></returns>
        public async Task<ActionResult> GetUsedMaterialSpecs(Guid id, int getType)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (getType != 1 && getType != 2)
            {
                throw new ArgumentException("无效的getType");
            }

            using (ServiceProxy<IMaterialSpecService> proxy = new ServiceProxy<IMaterialSpecService>())
            {
                IList<MaterialSpecMaintObject> materialSpecMaintObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedMaterialSpecs(id));
                if (getType == 1)
                {
                    materialSpecMaintObjects.Insert(0, new MaterialSpecMaintObject() { Id = Guid.Empty, MaterialSpecName = "请选择" });
                }
                else if (getType == 2)
                {
                    materialSpecMaintObjects.Insert(0, new MaterialSpecMaintObject() { Id = Guid.Empty, MaterialSpecName = "全部" });
                }
                return Json(materialSpecMaintObjects, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存设计规格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveMaterialSpec()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            MaterialSpecMaintObject materialSpecMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                materialSpecMaintObject = new MaterialSpecMaintObject()
                {
                    Id = Guid.Empty,
                    MaterialId = Guid.Parse(row["MaterialId"].ToString()),
                    MaterialSpecCode = row["MaterialSpecCode"].ToString().Trim(),
                    MaterialSpecName = row["MaterialSpecName"].ToString().Trim(),
                    UnitId = Guid.Parse(row["UnitId"].ToString()),
                    Price = decimal.Parse(row["Price"].ToString()),
                    //CustomerId = Guid.Parse(row["CustomerId"].ToString()),
                    CustomerId = Guid.Empty,
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                materialSpecMaintObject = new MaterialSpecMaintObject()
                {
                    Id = id,
                    MaterialSpecCode = row["MaterialSpecCode"].ToString().Trim(),
                    MaterialSpecName = row["MaterialSpecName"].ToString().Trim(),
                    UnitId = Guid.Parse(row["UnitId"].ToString()),
                    Price = decimal.Parse(row["Price"].ToString()),
                    //CustomerId = Guid.Parse(row["CustomerId"].ToString()),
                    CustomerId = Guid.Empty,
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IMaterialSpecService> proxy = new ServiceProxy<IMaterialSpecService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateMaterialSpec(materialSpecMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除设计规格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveMaterialSpecs()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<MaterialSpecMaintObject> materialSpecMaintObjects = new List<MaterialSpecMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    materialSpecMaintObjects.Add(new MaterialSpecMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IMaterialSpecService> proxy = new ServiceProxy<IMaterialSpecService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveMaterialSpecs(materialSpecMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        /// <summary>
        /// 根据设计规格获取供应商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetSupplierCustomerNameByMaterialSpecId(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IMaterialSpecService> proxy = new ServiceProxy<IMaterialSpecService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetSupplierCustomerNameByMaterialSpecId(id)), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 往来单位

        /// <summary>
        /// 往来单位
        /// </summary>
        /// <returns></returns>
        public ActionResult Customer()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumStateList = enumService.GetStateEnum();
            IList<Dictionary<string, string>> enumCustomerTypeList = enumService.GetCustomerTypeEnum();
            ViewData["State"] = JsonHelper.Encode(enumStateList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumStateList.Insert(0, allDict);
            enumCustomerTypeList.Insert(0, allDict);
            ViewData["StateByAll"] = JsonHelper.Encode(enumStateList);
            ViewData["CustomerTypeByAll"] = JsonHelper.Encode(enumCustomerTypeList);
            enumCustomerTypeList.RemoveAt(0);
            Dictionary<string, string> selectDict = new Dictionary<string, string>(2);
            selectDict.Add("id", "0");
            selectDict.Add("text", "请选择");
            enumCustomerTypeList.Insert(0, selectDict);
            ViewData["CustomerTypeBySelect"] = JsonHelper.Encode(enumCustomerTypeList);
            return View();
        }

        /// <summary>
        /// 根据往来单位Id获取往来单位
        /// </summary>
        /// <param name="id">往来单位Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetCustomerById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<ICustomerService> proxy = new ServiceProxy<ICustomerService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetCustomerById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页往来单位列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCustomersPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["CustomerType"] == null)
            {
                throw new ArgumentNullException("CustomerType");
            }
            if (Request["CustomerCode"] == null)
            {
                throw new ArgumentNullException("CustomerCode");
            }
            if (Request["CustomerName"] == null)
            {
                throw new ArgumentNullException("CustomerName");
            }
            if (Request["CustomerFullName"] == null)
            {
                throw new ArgumentNullException("CustomerFullName");
            }
            if (Request["State"] == null)
            {
                throw new ArgumentNullException("State");
            }

            using (ServiceProxy<ICustomerService> proxy = new ServiceProxy<ICustomerService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetCustomersPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    int.Parse(Request["CustomerType"].Trim()), Request["CustomerCode"].Trim(), Request["CustomerName"].Trim(), Request["CustomerFullName"].Trim(), int.Parse(Request["State"])));
            }
        }

        /// <summary>
        /// 获取分页往来单位列表，用于选择往来单位
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCustomersPageBySelect()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["CustomerCode"] == null)
            {
                throw new ArgumentNullException("CustomerCode");
            }
            if (Request["CustomerName"] == null)
            {
                throw new ArgumentNullException("CustomerName");
            }
            if (Request["CustomerFullName"] == null)
            {
                throw new ArgumentNullException("CustomerFullName");
            }
            if (Request["CustomerType"] == null)
            {
                throw new ArgumentNullException("CustomerType");
            }

            using (ServiceProxy<ICustomerService> proxy = new ServiceProxy<ICustomerService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetCustomersPageBySelect(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]), Request["CustomerCode"].Trim(),
                    Request["CustomerName"].Trim(), Request["CustomerFullName"].Trim(), int.Parse(Request["CustomerType"])));
            }
        }

        /// <summary>
        /// 保存往来单位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveCustomer()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            CustomerMaintObject customerMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                customerMaintObject = new CustomerMaintObject()
                {
                    Id = Guid.Empty,
                    CustomerCode = row["CustomerCode"].ToString().Trim(),
                    CustomerType = int.Parse(row["CustomerType"].ToString().Trim()),
                    CustomerName = row["CustomerName"].ToString().Trim(),
                    CustomerFullName = row["CustomerFullName"].ToString().Trim(),
                    CustomerUserId = Guid.Parse(row["CustomerUserId"].ToString()),
                    ContactMan = row["ContactMan"].ToString().Trim(),
                    ContactTel = row["ContactTel"].ToString().Trim(),
                    ContactAddr = row["ContactAddr"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                customerMaintObject = new CustomerMaintObject()
                {
                    Id = id,
                    CustomerType = int.Parse(row["CustomerType"].ToString().Trim()),
                    CustomerName = row["CustomerName"].ToString().Trim(),
                    CustomerFullName = row["CustomerFullName"].ToString().Trim(),
                    CustomerUserId = Guid.Parse(row["CustomerUserId"].ToString()),
                    ContactMan = row["ContactMan"].ToString().Trim(),
                    ContactTel = row["ContactTel"].ToString().Trim(),
                    ContactAddr = row["ContactAddr"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<ICustomerService> proxy = new ServiceProxy<ICustomerService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateCustomer(customerMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除往来单位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveCustomers()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<CustomerMaintObject> customerMaintObjects = new List<CustomerMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    customerMaintObjects.Add(new CustomerMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<ICustomerService> proxy = new ServiceProxy<ICustomerService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveCustomers(customerMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        /// <summary>
        /// 根据单位类型获取往来单位列表
        /// </summary>
        /// <param name="customerType">单位类型</param>
        /// <param name="getType">获取类型</param>
        /// <returns></returns>
        public async Task<ActionResult> GetCustomersByType(int getType)
        {
            //if (customerType == null)
            //{
            //    throw new ArgumentNullException("customerType");
            //}
            if (Request["CustomerType"] == null)
            {
                throw new ArgumentNullException("CustomerType");
            }

            if (getType != 1 && getType != 2)
            {
                throw new ArgumentException("无效的getType");
            }

            using (ServiceProxy<ICustomerService> proxy = new ServiceProxy<ICustomerService>())
            {
                IList<CustomerMaintObject> customerMaintObjects = await Task.Factory.StartNew(() => proxy.Channel.GetCustomersByType(int.Parse(Request["CustomerType"])));
                if (getType == 1)
                {
                    customerMaintObjects.Insert(0, new CustomerMaintObject() { Id = Guid.Empty, CustomerName = "请选择" });
                }
                else if (getType == 2)
                {
                    customerMaintObjects.Insert(0, new CustomerMaintObject() { Id = Guid.Empty, CustomerName = "全部" });
                }
                return Json(customerMaintObjects, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 物资申购

        /// <summary>
        /// 物资申购
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> MaterialPurchase()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumDoStateList = enumService.GetDoStateEnum();
            ViewData["DoState"] = JsonHelper.Encode(enumDoStateList);
            ViewData["DoStateByConfirm"] = JsonHelper.Encode(enumDoStateList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumDoStateList.Insert(0, allDict);
            ViewData["DoStateByAll"] = JsonHelper.Encode(enumDoStateList);
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页申购清单列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMaterialPurchasePage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["MaterialName"] == null)
            {
                throw new ArgumentNullException("MaterialName");
            }
            if (Request["DoState"] == null)
            {
                throw new ArgumentNullException("DoState");
            }

            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetMaterialPurchasePage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]),
                    Guid.Parse(Request["ReseauId"]), Request["MaterialName"].Trim(), int.Parse(Request["DoState"])));
            }
        }

        /// <summary>
        /// 申购确认
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoStateConfirm()
        {
            if (Request["DoState"] == null)
            {
                throw new ArgumentNullException("DoState");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<MaterialListMaintObject> materialListMaintObjects = new List<MaterialListMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    materialListMaintObjects.Add(new MaterialListMaintObject() { Id = id, DoState = int.Parse(Request["DoState"].ToString()) });
                }
            }
            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.DoStateConfirm(materialListMaintObjects));
            }
            return this.Sucess("申购确认成功");
        }

        #endregion

        #region 站点信息
        /// <summary>
        /// 站点信息
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns></returns>
        public async Task<ActionResult> PlaceInfo(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();
            IList<Dictionary<string, string>> enumProjectProgressList = enumService.GetProjectProgressEnum();
            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["ProjectProgress"] = JsonHelper.Encode(enumProjectProgressList);

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                PlaceInfoObject placeInfoObject = await Task.Factory.StartNew(() => proxy.Channel.GetPlaceInfoById(id));
                ViewData["Id"] = id;
                ViewData["PlaceCode"] = placeInfoObject.PlaceCode;
                ViewData["PlaceName"] = placeInfoObject.PlaceName;
                ViewData["AreaName"] = placeInfoObject.AreaName;
                ViewData["ReseauName"] = placeInfoObject.ReseauName;
                ViewData["PlaceCategoryName"] = placeInfoObject.PlaceCategoryName;
                ViewData["ImportanceName"] = placeInfoObject.ImportanceName;
                ViewData["Lng"] = placeInfoObject.Lng;
                ViewData["Lat"] = placeInfoObject.Lat;
                ViewData["AddressingDepartmentName"] = placeInfoObject.AddressingDepartmentName;
                ViewData["AddressingRealName"] = placeInfoObject.AddressingRealName;
                ViewData["PlaceOwnerName"] = placeInfoObject.PlaceOwnerName;
                ViewData["OwnerName"] = placeInfoObject.OwnerName;
                ViewData["OwnerContact"] = placeInfoObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = placeInfoObject.OwnerPhoneNumber;
                ViewData["DetailedAddress"] = placeInfoObject.DetailedAddress;
                ViewData["Remarks"] = placeInfoObject.Remarks;
                ViewData["G2Number"] = placeInfoObject.G2Number;
                ViewData["D2Number"] = placeInfoObject.D2Number;
                ViewData["G3Number"] = placeInfoObject.G3Number;
                ViewData["G4Number"] = placeInfoObject.G4Number;
                ViewData["G5Number"] = placeInfoObject.G5Number;
                ViewData["StateName"] = placeInfoObject.StateName;
                ViewData["CreateUserName"] = placeInfoObject.CreateUserName;
                ViewData["CreateDate"] = placeInfoObject.CreateDate;
                ViewData["Count"] = placeInfoObject.Count;
            }
            return View();
        }

        /// <summary>
        /// 获取站点信息(移动端)
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlaceInfoMobile(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlaceInfoById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 更新站点方位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SavePlacePositionMobile()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceMaintObject placeMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());

            placeMaintObject = new PlaceMaintObject()
            {
                Id = id,
                Lng = decimal.Parse(row["Lng"].ToString()),
                Lat = decimal.Parse(row["Lat"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SavePlacePositionMobile(placeMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 获取建设任务历史记录列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectTaskHistory()
        {
            if (Request["PlaceId"] == null)
            {
                throw new ArgumentNullException("PlaceId");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskHistory(Guid.Parse(Request["PlaceId"])));
            }
        }

        /// <summary>
        /// 站点修改(移动端)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SavePlaceMobile()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceMaintObject placeMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());

            placeMaintObject = new PlaceMaintObject()
            {
                Id = id,
                PlaceName = row["PlaceName"].ToString().Trim(),
                PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                AreaId = Guid.Parse(row["AreaId"].ToString()),
                ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                Lng = decimal.Parse(row["Lng"].ToString()),
                Lat = decimal.Parse(row["Lat"].ToString()),
                PlaceOwner = Guid.Parse(row["PlaceOwner"].ToString()),
                Importance = int.Parse(row["Importance"].ToString()),
                AddressingRealName = row["AddressingRealName"].ToString().Trim(),
                OwnerName = row["OwnerName"].ToString().Trim(),
                OwnerContact = row["OwnerContact"].ToString().Trim(),
                OwnerPhoneNumber = row["OwnerPhoneNumber"].ToString().Trim(),
                DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SavePlaceMobile(placeMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 派工单大类

        /// <summary>
        /// 派工单大类
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkBigClass()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据派工单大类Id获取派工单大类
        /// </summary>
        /// <param name="id">派工单大类Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetWorkBigClassById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IWorkBigClassService> proxy = new ServiceProxy<IWorkBigClassService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWorkBigClassById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取派工单大类列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetWorkBigClasss()
        {
            using (ServiceProxy<IWorkBigClassService> proxy = new ServiceProxy<IWorkBigClassService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWorkBigClasss()), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存派工单大类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWorkBigClass()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            WorkBigClassMaintObject workBigClassMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                workBigClassMaintObject = new WorkBigClassMaintObject()
                {
                    Id = Guid.Empty,
                    BigClassCode = row["BigClassCode"].ToString().Trim(),
                    BigClassName = row["BigClassName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                workBigClassMaintObject = new WorkBigClassMaintObject()
                {
                    Id = id,
                    BigClassCode = row["BigClassCode"].ToString().Trim(),
                    BigClassName = row["BigClassName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IWorkBigClassService> proxy = new ServiceProxy<IWorkBigClassService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateWorkBigClass(workBigClassMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除派工单大类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveWorkBigClasss()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WorkBigClassMaintObject> workBigClassMaintObjects = new List<WorkBigClassMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    workBigClassMaintObjects.Add(new WorkBigClassMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IWorkBigClassService> proxy = new ServiceProxy<IWorkBigClassService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveWorkBigClasss(workBigClassMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 派工单小类

        /// <summary>
        /// 派工单小类
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WorkSmallClass()
        {
            using (ServiceProxy<IWorkBigClassService> proxy = new ServiceProxy<IWorkBigClassService>())
            {
                IList<WorkBigClassSelectObject> workBigClassSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWorkBigClasss());
                workBigClassSelectObjects.Insert(0, new WorkBigClassSelectObject() { Id = Guid.Empty, BigClassName = "请选择" });
                ViewData["WorkBigClasss"] = JsonHelper.Encode(workBigClassSelectObjects);
                workBigClassSelectObjects.RemoveAt(0);
                workBigClassSelectObjects.Insert(0, new WorkBigClassSelectObject() { Id = Guid.Empty, BigClassName = "工单大类", PId = Guid.NewGuid(), isLeaf = false });
                ViewData["WorkBigClasssTree"] = JsonHelper.Encode(workBigClassSelectObjects);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据派工单小类Id获取派工单小类
        /// </summary>
        /// <param name="id">派工单小类Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetWorkSmallClassById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IWorkSmallClassService> proxy = new ServiceProxy<IWorkSmallClassService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWorkSmallClassById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取派工单小类列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetWorkSmallClasss()
        {
            if (Request["WorkBigClassId"] == null)
            {
                throw new ArgumentNullException("WorkBigClassId");
            }

            using (ServiceProxy<IWorkSmallClassService> proxy = new ServiceProxy<IWorkSmallClassService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWorkSmallClasss(Guid.Parse(Request["WorkBigClassId"]))), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据区域Id获取状态为使用的派工单小类列表
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <param name="getType">获取类型</param>
        /// <returns></returns>
        public async Task<ActionResult> GetUsedWorkSmallClass(Guid id, int getType)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (getType != 1 && getType != 2)
            {
                throw new ArgumentException("无效的getType");
            }

            using (ServiceProxy<IWorkSmallClassService> proxy = new ServiceProxy<IWorkSmallClassService>())
            {
                IList<WorkSmallClassSelectObject> workSmallClassSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWorkSmallClass(id));
                if (getType == 1)
                {
                    workSmallClassSelectObjects.Insert(0, new WorkSmallClassSelectObject() { Id = Guid.Empty, SmallClassName = "请选择" });
                }
                else if (getType == 2)
                {
                    workSmallClassSelectObjects.Insert(0, new WorkSmallClassSelectObject() { Id = Guid.Empty, SmallClassName = "全部" });
                }
                return Json(workSmallClassSelectObjects, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存派工单小类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWorkSmallClass()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            WorkSmallClassMaintObject workSmallClassMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                workSmallClassMaintObject = new WorkSmallClassMaintObject()
                {
                    Id = Guid.Empty,
                    WorkBigClassId = Guid.Parse(row["WorkBigClassId"].ToString()),
                    SmallClassCode = row["SmallClassCode"].ToString().Trim(),
                    SmallClassName = row["SmallClassName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                workSmallClassMaintObject = new WorkSmallClassMaintObject()
                {
                    Id = id,
                    SmallClassCode = row["SmallClassCode"].ToString().Trim(),
                    SmallClassName = row["SmallClassName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IWorkSmallClassService> proxy = new ServiceProxy<IWorkSmallClassService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateWorkSmallClass(workSmallClassMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除派工单小类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveWorkSmallClasss()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WorkSmallClassMaintObject> workSmallClassMaintObjects = new List<WorkSmallClassMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    workSmallClassMaintObjects.Add(new WorkSmallClassMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IWorkSmallClassService> proxy = new ServiceProxy<IWorkSmallClassService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveWorkSmallClasss(workSmallClassMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 部门调动
        public async Task<ActionResult> UserDepartmentChange()
        {
            ViewData["CompanyId"] = this.CompanyId;
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumStateList = enumService.GetStateEnum();
            ViewData["State"] = JsonHelper.Encode(enumStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumStateList.Insert(0, allDict);
            ViewData["StateByAll"] = JsonHelper.Encode(enumStateList);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveUserDepartment()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            UserMaintObject userMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            userMaintObject = new UserMaintObject()
            {
                Id = id,
                DepartmentId = Guid.Parse(row["DepartmentId"].ToString()),
                UserName = row["UserName"].ToString().Trim(),
                FullName = row["FullName"].ToString().Trim(),
                Email = row["Email"].ToString().Trim(),
                PhoneNumber = row["PhoneNumber"].ToString().Trim(),
                State = int.Parse(row["State"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveUserDepartment(userMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 往来单位用户

        /// <summary>
        /// 往来单位用户
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CustomerUser()
        {
            ViewData["CompanyId"] = this.CompanyId;
            using (ServiceProxy<ICustomerService> proxy = new ServiceProxy<ICustomerService>())
            {
                ViewData["CustomerTree"] = await Task.Factory.StartNew(() => proxy.Channel.GetAllUsedCustomers());
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取往来单位用户列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCustomerUsers()
        {
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["DepartmentId"] == null)
            {
                throw new ArgumentNullException("DepartmentId");
            }
            if (Request["CustomerId"] == null)
            {
                throw new ArgumentNullException("CustomerId");
            }

            using (ServiceProxy<ICustomerUserService> proxy = new ServiceProxy<ICustomerUserService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetCustomerUsers(Guid.Parse(Request["CompanyId"]), Guid.Parse(Request["DepartmentId"]), Guid.Parse(Request["CustomerId"])));
            }
        }

        /// <summary>
        /// 保存往来单位用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveCustomerUsers()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }
            if (Request["CustomerId"] == null)
            {
                throw new ArgumentNullException("CustomerId");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            Guid customerId = Guid.Parse(Request["CustomerId"]);
            IList<CustomerUserMaintObject> customerUserMaintObjects = new List<CustomerUserMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                customerUserMaintObjects.Add(new CustomerUserMaintObject()
                {
                    Id = Guid.Parse(row["CustomerUserId"].ToString()),
                    CustomerId = customerId,
                    UserId = Guid.Parse(row["Id"].ToString()),
                    CreateUserId = this.UserId
                });
            }
            using (ServiceProxy<ICustomerUserService> proxy = new ServiceProxy<ICustomerUserService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrRemoveCustomerUsers(customerUserMaintObjects));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 根据部门Id，岗位Id获取状态为使用的用户账号列表，用于发送公文
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetUsersByCustomer()
        {
            if (Request["CustomerId"] == null)
            {
                throw new ArgumentNullException("CustomerId");
            }

            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetUsersByCustomer(Guid.Parse(Request["CustomerId"])));
            }
        }
        #endregion

        #region 产权

        /// <summary>
        /// 产权
        /// </summary>
        /// <returns></returns>
        public ActionResult PlaceOwner()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            return View();
        }

        /// <summary>
        /// 根据产权Id获取产权
        /// </summary>
        /// <param name="id">产权Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlaceOwnerById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlaceOwnerById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取产权列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlaceOwners()
        {
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlaceOwners()), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存产权
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlaceOwner()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceOwnerMaintObject placeOwnerMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                placeOwnerMaintObject = new PlaceOwnerMaintObject()
                {
                    Id = Guid.Empty,
                    PlaceOwnerCode = row["PlaceOwnerCode"].ToString().Trim(),
                    PlaceOwnerName = row["PlaceOwnerName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                placeOwnerMaintObject = new PlaceOwnerMaintObject()
                {
                    Id = id,
                    PlaceOwnerCode = row["PlaceOwnerCode"].ToString().Trim(),
                    PlaceOwnerName = row["PlaceOwnerName"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdatePlaceOwner(placeOwnerMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除产权
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePlaceOwners()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlaceOwnerMaintObject> placeOwnerMaintObjects = new List<PlaceOwnerMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    placeOwnerMaintObjects.Add(new PlaceOwnerMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemovePlaceOwners(placeOwnerMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 用户职务

        /// <summary>
        /// 用户职务
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> DutyUser()
        {
            ViewData["CompanyId"] = this.CompanyId;
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, object>> dutysTree = enumService.GetDutyEnumByTree();
            Dictionary<string, object> root = new Dictionary<string, object>(5);
            root.Add("id", "0");
            root.Add("text", "职务");
            root.Add("pid", "null");
            root.Add("isLeaf", false);
            root.Add("asyncLoad", false);
            dutysTree.Insert(0, root);
            ViewData["DutyTree"] = JsonHelper.Encode(dutysTree);
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取用户职务列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDutyUsers()
        {
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["DepartmentId"] == null)
            {
                throw new ArgumentNullException("DepartmentId");
            }
            if (Request["Duty"] == null)
            {
                throw new ArgumentNullException("Duty");
            }

            using (ServiceProxy<IDutyUserService> proxy = new ServiceProxy<IDutyUserService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetDutyUsers(Guid.Parse(Request["CompanyId"]), Guid.Parse(Request["DepartmentId"]), int.Parse(Request["Duty"])));
            }
        }

        /// <summary>
        /// 保存岗位用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveDutyUsers()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }
            if (Request["Duty"] == null)
            {
                throw new ArgumentNullException("Duty");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            int duty = int.Parse(Request["Duty"]);
            IList<DutyUserMaintObject> dutyUserMaintObjects = new List<DutyUserMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                dutyUserMaintObjects.Add(new DutyUserMaintObject()
                {
                    Id = Guid.Parse(row["DutyUserId"].ToString()),
                    Duty = duty,
                    UserId = Guid.Parse(row["Id"].ToString()),
                    CreateUserId = this.UserId
                });
            }
            using (ServiceProxy<IDutyUserService> proxy = new ServiceProxy<IDutyUserService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrRemoveDutyUsers(dutyUserMaintObjects));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion
    }
}