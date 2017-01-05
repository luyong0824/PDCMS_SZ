using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;

namespace PDBM.ApplicationService.Services
{
    /// <summary>
    /// 数据访问应用层服务抽象类
    /// </summary>
    public abstract class DataService : DisposableObject
    {
        private readonly IRepositoryContext context;

        public DataService(IRepositoryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 获取仓储上下文
        /// </summary>
        protected IRepositoryContext Context
        {
            get { return context; }
        }

        /// <summary>
        /// 释放仓储上下文资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
    }
}
