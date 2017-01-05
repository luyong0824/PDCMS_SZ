using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Common;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Repositories
{
    /// <summary>
    /// 仓储上下文接口
    /// </summary>
    public interface IRepositoryContext : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// 向工作单元注册新增的聚合根
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根接口</typeparam>
        /// <param name="aggregateRoot">新增的聚合根</param>
        void RegisterNew<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRoot;

        /// <summary>
        /// 向工作单元注册修改的聚合根
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根接口</typeparam>
        /// <param name="aggregateRoot">修改的聚合根</param>
        void RegisterModified<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRoot;

        /// <summary>
        /// 向工作单元注册删除的聚合根
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根接口</typeparam>
        /// <param name="aggregateRoot">删除的聚合根</param>
        void RegisterDeleted<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRoot;
    }
}
