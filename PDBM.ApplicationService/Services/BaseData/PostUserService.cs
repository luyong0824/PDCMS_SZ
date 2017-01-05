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
    /// 岗位用户应用层服务
    /// </summary>
    public class PostUserService : DataService, IPostUserService
    {
        private readonly IRepository<PostUser> postUserRepository;

        public PostUserService(IRepositoryContext context,
            IRepository<PostUser> postUserRepository)
            : base(context)
        {
            this.postUserRepository = postUserRepository;
        }

        /// <summary>
        /// 根据公司Id，部门Id，岗位Id获取岗位用户列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="postId">岗位Id</param>
        /// <returns>岗位用户列表的Json字符串</returns>
        public string GetPostUsers(Guid companyId, Guid departmentId, Guid postId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "PostId", Type = SqlDbType.UniqueIdentifier, Value = postId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryPostUsers", parameters))
            {
                dt.Columns.Add("isLeaf", typeof(bool), "Convert(IsLeafStr, 'System.Boolean')");
                dt.Columns.Add("asyncLoad", typeof(bool), "Convert(AsyncLoadStr, 'System.Boolean')");
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 新增或者删除岗位用户
        /// </summary>
        /// <param name="postUserMaintObjects">要新增或者删除的岗位用户维护对象列表</param>
        public void AddOrRemovePostUsers(IList<PostUserMaintObject> postUserMaintObjects)
        {
            foreach (var postUserMaintObject in postUserMaintObjects)
            {
                if (postUserMaintObject.Id == Guid.Empty)
                {
                    PostUser postUser = AggregateFactory.CreatePostUser(postUserMaintObject.PostId,
                        postUserMaintObject.UserId, postUserMaintObject.CreateUserId);
                    postUserRepository.Add(postUser);
                }
                else
                {
                    PostUser postUser = postUserRepository.FindByKey(postUserMaintObject.Id);
                    if (postUser != null)
                    {
                        postUserRepository.Remove(postUser);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PostIdUserId"))
                {
                    throw new ApplicationFault("岗位用户重复添加");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_PostUser_dbo.tbl_Post_PostId"))
                {
                    throw new ApplicationFault("选择的岗位在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_PostUser_dbo.tbl_User_UserId"))
                {
                    throw new ApplicationFault("选择的用户在系统中不存在");
                }
                throw ex;
            }
        }
    }
}
