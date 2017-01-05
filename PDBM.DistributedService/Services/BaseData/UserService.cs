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
    /// 用户分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class UserService : IUserService
    {
        private readonly IUserService userServiceImpl = ServiceLocator.Instance.GetService<IUserService>();

        public UserMaintObject GetUserById(Guid id)
        {
            try
            {
                return userServiceImpl.GetUserById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public UserLoginObject UserLogin(string userName, string userPassword)
        {
            try
            {
                return userServiceImpl.UserLogin(userName, userPassword);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetUsersPage(int pageIndex, int pageSize, Guid companyId, Guid departmentId, string userName, string fullName, string email, string phoneNumber, int state)
        {
            try
            {
                return userServiceImpl.GetUsersPage(pageIndex, pageSize, companyId, departmentId, userName, fullName, email, phoneNumber, state);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateUser(UserMaintObject userMaintObject)
        {
            try
            {
                userServiceImpl.AddOrUpdateUser(userMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveUsers(IList<UserMaintObject> userMaintObjects)
        {
            try
            {
                userServiceImpl.RemoveUsers(userMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public UserInfoMaintObject GetUserInfo(Guid userId)
        {
            try
            {
                return userServiceImpl.GetUserInfo(userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void UpdateUserInfo(UserInfoMaintObject userInfoMaintObject)
        {
            try
            {
                userServiceImpl.UpdateUserInfo(userInfoMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetUsedUsersPageBySelect(int pageIndex, int pageSize, Guid companyId, Guid departmentId, string fullName)
        {
            try
            {
                return userServiceImpl.GetUsedUsersPageBySelect(pageIndex, pageSize, companyId, departmentId, fullName);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<UserSelectObject> GetUsedUsers(Guid departmentId)
        {
            try
            {
                return userServiceImpl.GetUsedUsers(departmentId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetUsedUsersBySend(Guid departmentId, Guid postId)
        {
            try
            {
                return userServiceImpl.GetUsedUsersBySend(departmentId, postId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void SaveUserDepartment(UserMaintObject userMaintObject)
        {
            try
            {
                userServiceImpl.SaveUserDepartment(userMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetUsersByCustomer(Guid customerId)
        {
            try
            {
                return userServiceImpl.GetUsersByCustomer(customerId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<UserSelectObject> GetAllUsedUsers()
        {
            try
            {
                return userServiceImpl.GetAllUsedUsers();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            userServiceImpl.Dispose();
        }
    }
}
