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
    /// 仓储上下文抽象类
    /// </summary>
    public abstract class RepositoryContext : DisposableObject, IRepositoryContext
    {
        private volatile bool committed = true;

        /// <summary>
        /// 向工作单元注册新增的聚合根
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根接口</typeparam>
        /// <param name="aggregateRoot">新增的聚合根</param>
        public abstract void RegisterNew<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRoot;

        /// <summary>
        /// 向工作单元注册修改的聚合根
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根接口</typeparam>
        /// <param name="aggregateRoot">修改的聚合根</param>
        public abstract void RegisterModified<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRoot;

        /// <summary>
        /// 向工作单元注册删除的聚合根
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根接口</typeparam>
        /// <param name="aggregateRoot">删除的聚合根</param>
        public abstract void RegisterDeleted<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRoot;

        /// <summary>
        /// 获取是否支持分布式事务标识
        /// </summary>
        public abstract bool DistributedTransactionSupported { get; }

        /// <summary>
        /// 获取工作单元是否已提交标识
        /// </summary>
        public bool Committed
        {
            get
            {
                return committed;
            }
            protected set
            {
                committed = value;
            }
        }

        /// <summary>
        /// 提交工作单元
        /// </summary>
        public abstract void Commit();

        /// <summary>
        /// 回滚工作单元
        /// </summary>
        public abstract void RollBack();
    }
}
