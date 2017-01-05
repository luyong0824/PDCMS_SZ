using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 用户应用层服务
    /// </summary>
    public class UserService : DataService, IUserService
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Department> departmentRepository;
        private readonly IRepository<Company> companyRepository;
        private readonly IRepository<RoleUser> roleUserRepository;
        private readonly IRepository<PostUser> postUserRepository;
        private readonly IRepository<Project> projectRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<WFActivity> wfActivityRepository;
        private readonly IRepository<WFActivityInstance> wfActivityInstanceRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<PlaceDesign> placeDesignRepository;

        public UserService(IRepositoryContext context,
            IRepository<User> userRepository,
            IRepository<Department> departmentRepository,
            IRepository<Company> companyRepository,
            IRepository<RoleUser> roleUserRepository,
            IRepository<PostUser> postUserRepository,
            IRepository<Project> projectRepository,
            IRepository<Planning> planningRepository,
            IRepository<WFActivity> wfActivityRepository,
            IRepository<WFActivityInstance> wfActivityInstanceRepository,
            IRepository<Customer> customerRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<PlaceDesign> placeDesignRepository)
            : base(context)
        {
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
            this.companyRepository = companyRepository;
            this.roleUserRepository = roleUserRepository;
            this.postUserRepository = postUserRepository;
            this.projectRepository = projectRepository;
            this.planningRepository = planningRepository;
            this.wfActivityRepository = wfActivityRepository;
            this.wfActivityInstanceRepository = wfActivityInstanceRepository;
            this.customerRepository = customerRepository;
            this.reseauRepository = reseauRepository;
            this.placeDesignRepository = placeDesignRepository;
        }

        /// <summary>
        /// 根据用户Id获取用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>用户维护对象</returns>
        public UserMaintObject GetUserById(Guid id)
        {
            User user = userRepository.FindByKey(id);
            if (user != null)
            {
                UserMaintObject userMaintObject = MapperHelper.Map<User, UserMaintObject>(user);
                Department department = departmentRepository.GetByKey(user.DepartmentId);
                userMaintObject.CompanyId = department.CompanyId;
                return userMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的用户账号在系统中不存在");
            }
        }

        /// <summary>
        /// 根据用户名和密码进行登录，登录成功则返回用户登录信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPassword">密码</param>
        /// <returns>用户登录对象</returns>
        public UserLoginObject UserLogin(string userName, string userPassword)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("用户名无效");
            }
            if (string.IsNullOrWhiteSpace(userPassword))
            {
                throw new ArgumentNullException("密码无效");
            }

            userPassword = StringHelper.Encrypto(userPassword);
            User user = userRepository.Find(Specification<User>.Eval(entity => entity.UserName == userName && entity.UserPassword == userPassword && entity.IsCurrentUsed == Bool.是));
            if (user == null)
            {
                throw new ApplicationFault("用户名或者密码错误");
            }
            user.CheckByLogin();
            Department department = departmentRepository.GetByKey(user.DepartmentId);
            department.CheckByLogin();
            Company company = companyRepository.GetByKey(department.CompanyId);
            company.CheckByLogin();

            return new UserLoginObject()
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                DepartmentId = department.Id,
                DepartmentName = department.DepartmentName,
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                CompanyNature = (int)company.CompanyNature
            };
        }

        /// <summary>
        /// 根据条件获取分页用户列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="fullName">姓名</param>
        /// <param name="email">邮箱</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="state">状态</param>
        /// <returns>分页用户列表的Json字符串</returns>
        public string GetUsersPage(int pageIndex, int pageSize, Guid companyId, Guid departmentId, string userName, string fullName, string email, string phoneNumber, int state)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "UserName", Type = SqlDbType.NVarChar, Value = userName });
            parameters.Add(new Parameter() { Name = "FullName", Type = SqlDbType.NVarChar, Value = fullName });
            parameters.Add(new Parameter() { Name = "Email", Type = SqlDbType.NVarChar, Value = email });
            parameters.Add(new Parameter() { Name = "PhoneNumber", Type = SqlDbType.NVarChar, Value = phoneNumber });
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = state });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryUsersPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改用户
        /// </summary>
        /// <param name="userMaintObject">要新增或者修改的用户维护对象</param>
        public void AddOrUpdateUser(UserMaintObject userMaintObject)
        {
            if (userMaintObject.Id == Guid.Empty)
            {
                if (userRepository.Exists(Specification<User>.Eval(entity => entity.UserName == userMaintObject.UserName)))
                {
                    throw new ApplicationFault("用户名重复");
                }
                User user = AggregateFactory.CreateUser(userMaintObject.DepartmentId, userMaintObject.UserName, userMaintObject.FullName,
                    userMaintObject.Email, userMaintObject.PhoneNumber, (State)userMaintObject.State, userMaintObject.UniqueCode, userMaintObject.CreateUserId);
                userRepository.Add(user);
            }
            else
            {
                User user = userRepository.FindByKey(userMaintObject.Id);
                if (userRepository.Exists(Specification<User>.Eval(entity => entity.Id != userMaintObject.Id && entity.UserName == userMaintObject.UserName && entity.UniqueCode != user.UniqueCode)))
                {
                    throw new ApplicationFault("用户名重复");
                }
                if (user != null)
                {
                    user.CheckByUpdate();
                    user.Modify(userMaintObject.UserName, userMaintObject.FullName, userMaintObject.Email, userMaintObject.PhoneNumber,
                        (State)userMaintObject.State, userMaintObject.ModifyUserId);
                    userRepository.Update(user);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_UserName"))
                {
                    throw new ApplicationFault("用户名重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_User_dbo.tbl_Department_DepartmentId"))
                {
                    throw new ApplicationFault("选择的部门在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userMaintObjects">要删除的用户维护对象列表</param>
        public void RemoveUsers(IList<UserMaintObject> userMaintObjects)
        {
            foreach (UserMaintObject userMaintObject in userMaintObjects)
            {
                User user = userRepository.FindByKey(userMaintObject.Id);
                if (user != null)
                {
                    user.CheckByRemove();
                    if (reseauRepository.Exists(Specification<Reseau>.Eval(entity => entity.ReseauManagerId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被设置为网格经理", user.UserName);
                    }
                    if (roleUserRepository.Exists(Specification<RoleUser>.Eval(entity => entity.UserId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被分配过角色", user.UserName);
                    }
                    if (postUserRepository.Exists(Specification<PostUser>.Eval(entity => entity.UserId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被分配过岗位", user.UserName);
                    }
                    if (departmentRepository.Exists(Specification<Department>.Eval(entity => entity.ManagerUserId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被设置为部门经理", user.UserName);
                    }
                    if (projectRepository.Exists(Specification<Project>.Eval(entity => entity.ManagerUserId == user.Id || entity.ResponsibleUserId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在项目中使用", user.UserName);
                    }
                    if (planningRepository.Exists(Specification<Planning>.Eval(entity => entity.AddressingUserId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被设置为租赁人", user.UserName);
                    }
                    if (wfActivityRepository.Exists(Specification<WFActivity>.Eval(entity => entity.UserId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在流程步骤中使用", user.UserName);
                    }
                    if (wfActivityInstanceRepository.Exists(Specification<WFActivityInstance>.Eval(entity => entity.UserId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在公文流程中使用", user.UserName);
                    }
                    if (customerRepository.Exists(Specification<Customer>.Eval(entity => entity.CustomerUserId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在往来单位中使用", user.UserName);
                    }
                    if (placeDesignRepository.Exists(Specification<PlaceDesign>.Eval(entity => entity.DesignUserId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被设置为设计人员", user.UserName);
                    }
                    if (placeDesignRepository.Exists(Specification<PlaceDesign>.Eval(entity => entity.SupervisorUserId == user.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已被设置为监理人", user.UserName);
                    }
                    userRepository.Remove(user);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_Reseau_dbo.tbl_User_ReseauManagerId"))
                {
                    throw new ApplicationFault("已被设置为网格经理");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_RoleUser_dbo.tbl_User_UserId"))
                {
                    throw new ApplicationFault("已被分配过角色");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_PostUser_dbo.tbl_User_UserId"))
                {
                    throw new ApplicationFault("已被分配过岗位");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Department_dbo.tbl_User_ManagerUserId"))
                {
                    throw new ApplicationFault("已被设置为部门经理");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Project_dbo.tbl_User_ManagerUserId"))
                {
                    throw new ApplicationFault("已在项目中使用");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Project_dbo.tbl_User_ResponsibleUserId"))
                {
                    throw new ApplicationFault("已在项目中使用");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_User_AddressingUserId"))
                {
                    throw new ApplicationFault("已被设置为租赁人");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivity_dbo.tbl_User_UserId"))
                {
                    throw new ApplicationFault("已在流程步骤中使用");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivityInstance_dbo.tbl_User_UserId"))
                {
                    throw new ApplicationFault("已在公文流程中使用");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 根据用户Id获取用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户信息维护对象</returns>
        public UserInfoMaintObject GetUserInfo(Guid userId)
        {
            User user = userRepository.GetByKey(userId);
            UserInfoMaintObject userInfoMaintObject = MapperHelper.Map<User, UserInfoMaintObject>(user);
            Department department = departmentRepository.GetByKey(user.DepartmentId);
            userInfoMaintObject.DepartmentName = department.DepartmentName;
            Company company = companyRepository.GetByKey(department.CompanyId);
            userInfoMaintObject.CompanyName = company.CompanyName;
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetRoleNamesByUserId", parameters))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    userInfoMaintObject.RoleNameList += dr["RoleName"].ToString().Trim() + ",";
                }
            }
            userInfoMaintObject.RoleNameList = string.IsNullOrWhiteSpace(userInfoMaintObject.RoleNameList) ? "" : userInfoMaintObject.RoleNameList.Remove(userInfoMaintObject.RoleNameList.Length - 1);
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetPostNamesByUserId", parameters))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    userInfoMaintObject.PostNameList += dr["PostName"].ToString().Trim() + ",";
                }
            }
            userInfoMaintObject.PostNameList = string.IsNullOrWhiteSpace(userInfoMaintObject.PostNameList) ? "" : userInfoMaintObject.PostNameList.Remove(userInfoMaintObject.PostNameList.Length - 1);
            return userInfoMaintObject;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userInfoMaintObject">要修改的用户信息维护对象</param>
        public void UpdateUserInfo(UserInfoMaintObject userInfoMaintObject)
        {
            User user = userRepository.GetByKey(userInfoMaintObject.Id);
            user.ModifyUserInfo(userInfoMaintObject.FullName, userInfoMaintObject.Email, userInfoMaintObject.PhoneNumber,
                userInfoMaintObject.OldUserPassword, userInfoMaintObject.NewUserPassword, userInfoMaintObject.ConfirmNewUserPassword);
            userRepository.Update(user);
            try
            {
                this.Context.Commit();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据条件获取状态为使用的用户分页列表，用于选择用户
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="fullName">姓名</param>
        /// <returns>分页用户列表的Json字符串</returns>
        public string GetUsedUsersPageBySelect(int pageIndex, int pageSize, Guid companyId, Guid departmentId, string fullName)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "FullName", Type = SqlDbType.NVarChar, Value = fullName.Trim() });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryUsedUsersPageBySelect", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据部门Id获取状态为使用的用户列表
        /// </summary>
        /// <param name="departmentId">用户列表</param>
        /// <returns>用户选择对象列表</returns>
        public IList<UserSelectObject> GetUsedUsers(Guid departmentId)
        {
            IList<UserSelectObject> userSelectObjects = new List<UserSelectObject>();
            IEnumerable<User> users = userRepository.FindAll(Specification<User>.Eval(entity => entity.DepartmentId == departmentId && entity.State == State.使用), "FullName");
            if (users != null)
            {
                foreach (var user in users)
                {
                    userSelectObjects.Add(MapperHelper.Map<User, UserSelectObject>(user));
                }
            }
            return userSelectObjects;
        }

        /// <summary>
        /// 根据部门Id和岗位Id获取状态为使用的用户列表，用于发送工作流实例
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <param name="postId">岗位Id</param>
        /// <returns>用户列表的Json字符串</returns>
        public string GetUsedUsersBySend(Guid departmentId, Guid postId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "PostId", Type = SqlDbType.UniqueIdentifier, Value = postId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetUsedUsersBySend", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 保存用户部门调动
        /// </summary>
        /// <param name="userMaintObject">要维护的用户对象</param>
        public void SaveUserDepartment(UserMaintObject userMaintObject)
        {
            User user = userRepository.FindByKey(userMaintObject.Id);
            if (user != null)
            {
                user.CheckByUpdate();
                if (user.DepartmentId == userMaintObject.DepartmentId)
                {
                    throw new ApplicationFault("调入部门与原部门一致，无需调动");
                }
                if (userRepository.Exists(Specification<User>.Eval(entity => entity.Id != userMaintObject.Id && entity.UserName == userMaintObject.UserName && entity.IsCurrentUsed == Bool.是)))
                {
                    throw new ApplicationFault("用户名重复");
                }
                if (userRepository.Exists(Specification<User>.Eval(entity => entity.Id != userMaintObject.Id && entity.DepartmentId == userMaintObject.DepartmentId && entity.IsCurrentUsed == Bool.否 && entity.UniqueCode == user.UniqueCode)))
                {
                    user.State = State.停用;
                    user.IsCurrentUsed = Bool.否;
                    userRepository.Update(user);
                    User oldUser = userRepository.Find(Specification<User>.Eval(entity => entity.Id != userMaintObject.Id && entity.DepartmentId == userMaintObject.DepartmentId && entity.IsCurrentUsed == Bool.否 && entity.UniqueCode == user.UniqueCode));
                    oldUser.UserName = userMaintObject.UserName;
                    oldUser.UserPassword = user.UserPassword;
                    oldUser.FullName = userMaintObject.FullName;
                    oldUser.Email = userMaintObject.Email;
                    oldUser.PhoneNumber = userMaintObject.PhoneNumber;
                    oldUser.State = State.使用;
                    oldUser.IsCurrentUsed = Bool.是;
                    userRepository.Update(oldUser);
                }
                else
                {
                    user.State = State.停用;
                    user.IsCurrentUsed = Bool.否;
                    userRepository.Update(user);
                    User newUser = AggregateFactory.CreateUser(userMaintObject.DepartmentId, userMaintObject.UserName, userMaintObject.FullName,
                    userMaintObject.Email, userMaintObject.PhoneNumber, (State)userMaintObject.State, user.UniqueCode, userMaintObject.ModifyUserId);
                    userRepository.Add(newUser);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_UserName"))
                {
                    throw new ApplicationFault("用户名重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_User_dbo.tbl_Department_DepartmentId"))
                {
                    throw new ApplicationFault("选择的部门在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 根据往来单位Id获取状态为使用的用户列表
        /// </summary>
        /// <param name="customerId">往来单位Id</param>
        /// <returns>用户选择对象列表</returns>
        public string GetUsersByCustomer(Guid customerId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "CustomerId", Type = SqlDbType.UniqueIdentifier, Value = customerId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetUsersByCustomer", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 获取所有状态为使用的用户列表
        /// </summary>
        /// <returns>用户选择对象列表</returns>
        public IList<UserSelectObject> GetAllUsedUsers()
        {
            IList<UserSelectObject> userSelectObjects = new List<UserSelectObject>();
            IEnumerable<User> users = userRepository.FindAll(Specification<User>.Eval(entity => entity.State == State.使用), "FullName");
            if (users != null)
            {
                foreach (var user in users)
                {
                    userSelectObjects.Add(MapperHelper.Map<User, UserSelectObject>(user));
                }
            }
            return userSelectObjects;
        }
    }
}
