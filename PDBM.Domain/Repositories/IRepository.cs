using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Common;
using PDBM.Domain.Specifications;

namespace PDBM.Domain.Repositories
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根</typeparam>
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 获取当前仓储使用的仓储上下文实例
        /// </summary>
        IRepositoryContext Context { get; }

        /// <summary>
        /// 添加聚合根
        /// </summary>
        /// <param name="aggregateRoot">聚合根</param>
        void Add(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 移除聚合根
        /// </summary>
        /// <param name="aggregateRoot">聚合根</param>
        void Remove(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 更新聚合根
        /// </summary>
        /// <param name="aggregateRoot">聚合根</param>
        void Update(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 根据规约判断聚合根是否存在
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>存在则返回true,否则返回false</returns>
        bool Exists(ISpecification<TAggregateRoot> specification);

        /// <summary>
        /// 根据聚合根Id获取聚合根，不存在则抛出异常
        /// </summary>
        /// <param name="key">聚合根Id</param>
        /// <returns>聚合根</returns>
        TAggregateRoot GetByKey(Guid key);

        /// <summary>
        /// 根据规约获取聚合根，不存在则抛出异常
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>聚合根</returns>
        TAggregateRoot Get(ISpecification<TAggregateRoot> specification);

        /// <summary>
        /// 根据规约，饥饿加载表达式获取聚合根，不存在则抛出异常
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="eagerLodingProperty">饥饿加载表达式</param>
        /// <returns>聚合根</returns>
        TAggregateRoot Get(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, object>> eagerLodingProperty);

        /// <summary>
        /// 根据规约，排序字符串，饥饿加载表达式获取聚合根列表，不存在则抛出异常
        /// </summary>
        /// <param name="specification">规约，默认null</param>
        /// <param name="sortString">排序字符串，默认null</param>
        /// <param name="eagerLodingProperty">饥饿加载表达式，默认null</param>
        /// <returns>聚合根列表</returns>
        IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification = null, string sortString = null, Expression<Func<TAggregateRoot, object>> eagerLodingProperty = null);

        /// <summary>
        /// 根据聚合根Id查找聚合根
        /// </summary>
        /// <param name="key">聚合根Id</param>
        /// <returns>聚合根</returns>
        TAggregateRoot FindByKey(Guid key);

        /// <summary>
        /// 根据规约查找聚合根
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>聚合根</returns>
        TAggregateRoot Find(ISpecification<TAggregateRoot> specification);

        /// <summary>
        /// 根据规约，饥饿加载表达式查找聚合根
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="eagerLodingProperty">饥饿加载表达式</param>
        /// <returns>聚合根</returns>
        TAggregateRoot Find(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, object>> eagerLodingProperty);

        /// <summary>
        /// 根据规约，排序字符串，饥饿加载表达式查找聚合根列表
        /// </summary>
        /// <param name="specification">规约，默认null</param>
        /// <param name="sortString">排序字符串，默认null</param>
        /// <param name="eagerLodingProperty">饥饿加载表达式，默认null</param>
        /// <returns>聚合根列表</returns>
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification = null, string sortString = null, Expression<Func<TAggregateRoot, object>> eagerLodingProperty = null);
    }
}
