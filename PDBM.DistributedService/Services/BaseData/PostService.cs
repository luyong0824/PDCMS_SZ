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
    /// 岗位分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PostService : IPostService
    {
        private readonly IPostService postServiceImpl = ServiceLocator.Instance.GetService<IPostService>();

        public PostMaintObject GetPostById(Guid id)
        {
            try
            {
                return postServiceImpl.GetPostById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<PostMaintObject> GetPosts()
        {
            try
            {
                return postServiceImpl.GetPosts();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<PostSelectObject> GetUsedPosts()
        {
            try
            {
                return postServiceImpl.GetUsedPosts();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdatePost(PostMaintObject postMaintObject)
        {
            try
            {
                postServiceImpl.AddOrUpdatePost(postMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemovePosts(IList<PostMaintObject> postMaintObjects)
        {
            try
            {
                postServiceImpl.RemovePosts(postMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            postServiceImpl.Dispose();
        }
    }
}
