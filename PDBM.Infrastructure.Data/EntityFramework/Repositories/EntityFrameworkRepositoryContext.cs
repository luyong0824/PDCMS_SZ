using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;

namespace PDBM.Infrastructure.Data.EntityFramework.Repositories
{
    /// <summary>
    /// 实体框架仓储上下文
    /// </summary>
    public class EntityFrameworkRepositoryContext : RepositoryContext, IEntityFrameworkRepositoryContext
    {
        private readonly object syncObj = new object();
        private readonly PDBMDbContext dbContext = new PDBMDbContext();

        public EntityFrameworkRepositoryContext()
        {
        }

        /// <summary>
        /// 向工作单元注册新增的聚合根
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根接口</typeparam>
        /// <param name="aggregateRoot">新增的聚合根</param>
        public override void RegisterNew<TAggregateRoot>(TAggregateRoot aggregateRoot)
        {
            lock (syncObj)
            {
                dbContext.Set<TAggregateRoot>().Add(aggregateRoot);
                this.Committed = false;
            }
        }

        /// <summary>
        /// 向工作单元注册修改的聚合根
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根接口</typeparam>
        /// <param name="aggregateRoot">修改的聚合根</param>
        public override void RegisterModified<TAggregateRoot>(TAggregateRoot aggregateRoot)
        {
            lock (syncObj)
            {
                dbContext.Entry<TAggregateRoot>(aggregateRoot).State = EntityState.Modified;
                this.Committed = false;
            }
        }

        /// <summary>
        /// 向工作单元注册删除的聚合根
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根接口</typeparam>
        /// <param name="aggregateRoot">删除的聚合根</param>
        public override void RegisterDeleted<TAggregateRoot>(TAggregateRoot aggregateRoot)
        {
            lock (syncObj)
            {
                dbContext.Set<TAggregateRoot>().Remove(aggregateRoot);
                this.Committed = false;
            }
        }

        /// <summary>
        /// 释放仓储资源
        /// </summary>
        /// <param name="disposing">是否释放仓储资源</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
        }

        /// <summary>
        /// 获取当前实体框架仓储使用的DbContext
        /// </summary>
        public DbContext Context
        {
            get
            {
                return dbContext;
            }
        }

        /// <summary>
        /// 获取是否支持分布式事务标识
        /// </summary>
        public override bool DistributedTransactionSupported
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 提交工作单元
        /// </summary>
        public override void Commit()
        {
            lock (syncObj)
            {
                if (!this.Committed)
                {
                    try
                    {
                        var entityValidationResults = dbContext.GetValidationErrors();
                        if (entityValidationResults != null && entityValidationResults.Count() > 0)
                        {
                            throw new DomainFault(entityValidationResults.First().ValidationErrors.First().ErrorMessage);
                        }
                        var count = dbContext.SaveChanges();
                        this.Committed = true;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw new DomainFault("发生并发冲突,请重新操作");
                    }
                    catch (DbUpdateException ex)
                    {
                        throw new DomainFault(ex.GetBaseException().Message);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 回滚工作单元
        /// </summary>
        public override void RollBack()
        {
            this.Committed = false;
        }
    }
}
