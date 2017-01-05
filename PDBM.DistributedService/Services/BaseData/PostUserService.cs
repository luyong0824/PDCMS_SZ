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
    /// 岗位用户分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PostUserService : IPostUserService
    {
        private readonly IPostUserService postUserServiceImpl = ServiceLocator.Instance.GetService<IPostUserService>();

        public string GetPostUsers(Guid companyId, Guid departmentId, Guid postId)
        {
            try
            {
                return postUserServiceImpl.GetPostUsers(companyId, departmentId, postId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrRemovePostUsers(IList<PostUserMaintObject> postUserMaintObjects)
        {
            try
            {
                postUserServiceImpl.AddOrRemovePostUsers(postUserMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            postUserServiceImpl.Dispose();
        }
    }
}
