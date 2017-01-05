using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Common;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;

namespace PDBM.Infrastructure.Data.EntityFramework.Repositories
{
    /// <summary>
    /// 实体框架仓储
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根</typeparam>
    public class EntityFrameworkRepository<TAggregateRoot> : Repository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public EntityFrameworkRepository(IRepositoryContext context)
            : base(context)
        {
            efContext = context as IEntityFrameworkRepositoryContext;
        }

        /// <summary>
        /// 获取当前实体框架仓储使用的实体框架仓储上下文实例
        /// </summary>
        protected IEntityFrameworkRepositoryContext EFContext
        {
            get
            {
                return efContext;
            }
        }

        /// <summary>
        /// 添加聚合根
        /// </summary>
        /// <param name="aggregateRoot">聚合根</param>
        public override void Add(TAggregateRoot aggregateRoot)
        {
            efContext.RegisterNew<TAggregateRoot>(aggregateRoot);
        }

        /// <summary>
        /// 移除聚合根
        /// </summary>
        /// <param name="aggregateRoot">聚合根</param>
        public override void Remove(TAggregateRoot aggregateRoot)
        {
            efContext.RegisterDeleted<TAggregateRoot>(aggregateRoot);
        }

        /// <summary>
        /// 更新聚合根
        /// </summary>
        /// <param name="aggregateRoot">聚合根</param>
        public override void Update(TAggregateRoot aggregateRoot)
        {
            efContext.RegisterModified<TAggregateRoot>(aggregateRoot);
        }

        /// <summary>
        /// 根据规约判断聚合根是否存在
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>存在则返回true,否则返回false</returns>
        public override bool Exists(ISpecification<TAggregateRoot> specification)
        {
            var count = efContext.Context.Set<TAggregateRoot>().Count(specification.GetExpression());
            return count != 0;
        }

        /// <summary>
        /// 根据聚合根Id获取聚合根，不存在则抛出异常
        /// </summary>
        /// <param name="key">聚合根Id</param>
        /// <returns>聚合根</returns>
        public override TAggregateRoot GetByKey(Guid key)
        {
            TAggregateRoot result = FindByKey(key);
            if (result == null)
            {
                throw new ArgumentException("无法根据指定的Key找到所需的聚合根。");
            }
            return result;
        }

        /// <summary>
        /// 根据规约获取聚合根，不存在则抛出异常
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>聚合根</returns>
        public override TAggregateRoot Get(ISpecification<TAggregateRoot> specification)
        {
            TAggregateRoot result = Find(specification);
            if (result == null)
            {
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根。");
            }
            return result;
        }

        /// <summary>
        /// 根据规约，饥饿加载表达式获取聚合根，不存在则抛出异常
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="eagerLodingProperty">饥饿加载表达式</param>
        /// <returns>聚合根</returns>
        public override TAggregateRoot Get(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, object>> eagerLodingProperty)
        {
            TAggregateRoot result = Find(specification, eagerLodingProperty);
            if (result == null)
            {
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根。");
            }
            return result;
        }

        /// <summary>
        /// 根据规约，排序字符串，饥饿加载表达式获取聚合根列表，不存在则抛出异常
        /// </summary>
        /// <param name="specification">规约，默认null</param>
        /// <param name="sortString">排序字符串，默认null</param>
        /// <param name="eagerLodingProperty">饥饿加载表达式，默认null</param>
        /// <returns>聚合根列表</returns>
        public override IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification = null, string sortString = null, Expression<Func<TAggregateRoot, object>> eagerLodingProperty = null)
        {
            IEnumerable<TAggregateRoot> result = FindAll(specification, sortString, eagerLodingProperty);
            if (result == null)
            {
                throw new ArgumentException("无法根据指定的查询条件找到所需的聚合根列表。");
            }
            return result;
        }

        /// <summary>
        /// 根据聚合根Id查找聚合根
        /// </summary>
        /// <param name="key">聚合根Id</param>
        /// <returns>聚合根</returns>
        public override TAggregateRoot FindByKey(Guid key)
        {
            return efContext.Context.Set<TAggregateRoot>().Where(o => o.Id == key).FirstOrDefault();
        }

        /// <summary>
        /// 根据规约查找聚合根
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>聚合根</returns>
        public override TAggregateRoot Find(ISpecification<TAggregateRoot> specification)
        {
            return efContext.Context.Set<TAggregateRoot>().Where(specification.GetExpression()).FirstOrDefault();
        }

        /// <summary>
        /// 根据规约，饥饿加载表达式查找聚合根
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="eagerLodingProperty">饥饿加载表达式</param>
        /// <returns>聚合根</returns>
        public override TAggregateRoot Find(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, object>> eagerLodingProperty)
        {
            return efContext.Context.Set<TAggregateRoot>().Include(eagerLodingProperty).Where(specification.GetExpression()).FirstOrDefault();
        }

        /// <summary>
        /// 根据规约，排序字符串，饥饿加载表达式查找聚合根列表
        /// </summary>
        /// <param name="specification">规约，默认null</param>
        /// <param name="sortString">排序字符串，默认null</param>
        /// <param name="eagerLodingProperty">饥饿加载表达式，默认null</param>
        /// <returns>聚合根列表</returns>
        public override IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification = null, string sortString = null, Expression<Func<TAggregateRoot, object>> eagerLodingProperty = null)
        {
            var set = efContext.Context.Set<TAggregateRoot>();
            if (specification == null && string.IsNullOrWhiteSpace(sortString) && eagerLodingProperty == null)
                return set.ToList();
            IQueryable<TAggregateRoot> query = set;
            if (specification != null)
                query = query.Where(specification.GetExpression());
            if (!string.IsNullOrWhiteSpace(sortString))
                query = query.OrderBy(sortString);
            if (eagerLodingProperty != null)
                query = query.Include(eagerLodingProperty);
            return query.ToList();
        }
    }
}
