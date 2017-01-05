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
    /// 岗位用户服务接口
    /// </summary>
    [ServiceContract]
    public interface IPostUserService : IDistributedService
    {
        /// <summary>
        /// 根据公司Id，部门Id，岗位Id获取岗位用户列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="postId">岗位Id</param>
        /// <returns>岗位用户列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetPostUsers(Guid companyId, Guid departmentId, Guid postId);

        /// <summary>
        /// 新增或者删除岗位用户
        /// </summary>
        /// <param name="postUserMaintObjects">要新增或者删除的岗位用户维护对象列表</param>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        void AddOrRemovePostUsers(IList<PostUserMaintObject> postUserMaintObjects);
    }
}
