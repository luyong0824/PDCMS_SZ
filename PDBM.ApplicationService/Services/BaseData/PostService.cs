using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 岗位应用层服务
    /// </summary>
    public class PostService : DataService, IPostService
    {
        private readonly IRepository<Post> postRepository;
        private readonly IRepository<PostUser> postUserRepository;
        private readonly IRepository<WFActivity> wfActivityRepository;

        public PostService(IRepositoryContext context,
            IRepository<Post> postRepository,
            IRepository<PostUser> postUserRepository,
            IRepository<WFActivity> wfActivityRepository)
            : base(context)
        {
            this.postRepository = postRepository;
            this.postUserRepository = postUserRepository;
            this.wfActivityRepository = wfActivityRepository;
        }

        /// <summary>
        /// 根据岗位Id获取岗位
        /// </summary>
        /// <param name="id">岗位Id</param>
        /// <returns>岗位维护对象</returns>
        public PostMaintObject GetPostById(Guid id)
        {
            Post post = postRepository.FindByKey(id);
            if (post != null)
            {
                PostMaintObject postMaintObject = MapperHelper.Map<Post, PostMaintObject>(post);
                return postMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的岗位在系统中不存在");
            }
        }

        /// <summary>
        /// 获取岗位列表
        /// </summary>
        /// <returns>岗位维护对象列表</returns>
        public IList<PostMaintObject> GetPosts()
        {
            IList<PostMaintObject> postMaintObjects = new List<PostMaintObject>();
            IEnumerable<Post> posts = postRepository.FindAll(null, "PostCode");
            if (posts != null)
            {
                foreach (var post in posts)
                {
                    postMaintObjects.Add(MapperHelper.Map<Post, PostMaintObject>(post));
                }
            }
            return postMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的岗位列表
        /// </summary>
        /// <returns>岗位选择对象列表</returns>
        public IList<PostSelectObject> GetUsedPosts()
        {
            IList<PostSelectObject> postSelectObjects = new List<PostSelectObject>();
            IEnumerable<Post> posts = postRepository.FindAll(Specification<Post>.Eval(entity => entity.State == State.使用), "PostCode");
            if (posts != null)
            {
                foreach (var post in posts)
                {
                    postSelectObjects.Add(MapperHelper.Map<Post, PostSelectObject>(post));
                }
            }
            return postSelectObjects;
        }

        /// <summary>
        /// 新增或者修改岗位
        /// </summary>
        /// <param name="postMaintObject">要新增或者修改的岗位维护对象</param>
        public void AddOrUpdatePost(PostMaintObject postMaintObject)
        {
            if (postMaintObject.Id == Guid.Empty)
            {
                Post post = AggregateFactory.CreatePost(postMaintObject.PostCode, postMaintObject.PostName,
                    postMaintObject.Remarks, (State)postMaintObject.State, postMaintObject.CreateUserId);
                postRepository.Add(post);
            }
            else
            {
                Post post = postRepository.FindByKey(postMaintObject.Id);
                if (post != null)
                {
                    post.Modify(postMaintObject.PostCode, postMaintObject.PostName, postMaintObject.Remarks,
                        (State)postMaintObject.State, postMaintObject.ModifyUserId);
                    postRepository.Update(post);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PostCode"))
                {
                    throw new ApplicationFault("岗位编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_PostName"))
                {
                    throw new ApplicationFault("岗位名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="postMaintObjects">要删除的岗位维护对象列表</param>
        public void RemovePosts(IList<PostMaintObject> postMaintObjects)
        {
            foreach (PostMaintObject postMaintObject in postMaintObjects)
            {
                Post post = postRepository.FindByKey(postMaintObject.Id);
                if (post != null)
                {
                    if (postUserRepository.Exists(Specification<PostUser>.Eval(entity => entity.PostId == post.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已存在岗位用户", post.PostCode);
                    }
                    if (wfActivityRepository.Exists(Specification<WFActivity>.Eval(entity => entity.PostId == post.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在流程步骤中使用", post.PostCode);
                    }
                    postRepository.Remove(post);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_PostUser_dbo.tbl_Post_PostId"))
                {
                    throw new ApplicationFault("已存在岗位用户");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivity_dbo.tbl_Post_PostId"))
                {
                    throw new ApplicationFault("已在流程步骤中使用");
                }
                throw ex;
            }
        }
    }
}
