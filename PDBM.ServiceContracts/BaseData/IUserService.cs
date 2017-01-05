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
    /// 用户服务接口
    /// </summary>
    [ServiceContract]
    public interface IUserService : IDistributedService
    {
        /// <summary>
        /// 根据用户Id获取用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>用户维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        UserMaintObject GetUserById(Guid id);

        /// <summary>
        /// 根据用户名和密码进行登录，登录成功则返回用户登录信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPassword">密码</param>
        /// <returns>用户登录对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        UserLoginObject UserLogin(string userName, string userPassword);

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
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetUsersPage(int pageIndex, int pageSize, Guid companyId, Guid departmentId, string userName, string fullName, string email, string phoneNumber, int state);

        /// <summary>
        /// 新增或者修改用户
        /// </summary>
        /// <param name="userMaintObject">要新增或者修改的用户维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdateUser(UserMaintObject userMaintObject);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userMaintObjects">要删除的用户维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemoveUsers(IList<UserMaintObject> userMaintObjects);

        /// <summary>
        /// 根据用户Id获取用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户信息维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        UserInfoMaintObject GetUserInfo(Guid userId);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userInfoMaintObject">要修改的用户信息维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void UpdateUserInfo(UserInfoMaintObject userInfoMaintObject);

        /// <summary>
        /// 根据条件获取状态为使用的用户分页列表，用于选择用户
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="fullName">姓名</param>
        /// <returns>分页用户列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetUsedUsersPageBySelect(int pageIndex, int pageSize, Guid companyId, Guid departmentId, string fullName);

        /// <summary>
        /// 根据部门Id获取状态为使用的用户列表
        /// </summary>
        /// <param name="departmentId">往来单位Id</param>
        /// <returns>用户选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<UserSelectObject> GetUsedUsers(Guid departmentId);

        /// <summary>
        /// 根据部门Id和岗位Id获取状态为使用的用户列表，用于发送工作流实例
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <param name="postId">岗位Id</param>
        /// <returns>用户列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetUsedUsersBySend(Guid departmentId, Guid postId);

        /// <summary>
        /// 保存用户部门调动
        /// </summary>
        /// <param name="userMaintObject">要维护的用户对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void SaveUserDepartment(UserMaintObject userMaintObject);

        /// <summary>
        /// 根据往来单位Id获取状态为使用的用户列表
        /// </summary>
        /// <param name="customerId">往来单位Id</param>
        /// <returns>用户选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetUsersByCustomer(Guid customerId);

        /// <summary>
        /// 获取所有状态为使用的用户列表
        /// </summary>
        /// <returns>用户选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<UserSelectObject> GetAllUsedUsers();
    }
}
