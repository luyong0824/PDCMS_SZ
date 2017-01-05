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
    /// 岗位服务接口
    /// </summary>
    [ServiceContract]
    public interface IPostService : IDistributedService
    {
        /// <summary>
        /// 根据岗位Id获取岗位
        /// </summary>
        /// <param name="id">岗位Id</param>
        /// <returns>岗位维护对象</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        PostMaintObject GetPostById(Guid id);

        /// <summary>
        /// 获取岗位列表
        /// </summary>
        /// <returns>岗位维护对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<PostMaintObject> GetPosts();

        /// <summary>
        /// 获取状态为使用的岗位列表
        /// </summary>
        /// <returns>岗位选择对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<PostSelectObject> GetUsedPosts();

        /// <summary>
        /// 新增或者修改岗位
        /// </summary>
        /// <param name="postMaintObject">要新增或者修改的岗位维护对象</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrUpdatePost(PostMaintObject postMaintObject);

        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="postMaintObjects">要删除的岗位维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void RemovePosts(IList<PostMaintObject> postMaintObjects);
    }
}
