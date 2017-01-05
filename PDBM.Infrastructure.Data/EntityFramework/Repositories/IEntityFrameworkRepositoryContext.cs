using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Repositories;

namespace PDBM.Infrastructure.Data.EntityFramework.Repositories
{
    /// <summary>
    /// 实体框架仓储上下文接口
    /// </summary>
    public interface IEntityFrameworkRepositoryContext : IRepositoryContext
    {
        /// <summary>
        /// 获取当前实体框架仓储使用的DbContext
        /// </summary>
        DbContext Context { get; }
    }
}
