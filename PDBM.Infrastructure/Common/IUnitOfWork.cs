using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Common
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 是否支持分布式事务
        /// </summary>
        bool DistributedTransactionSupported { get; }

        /// <summary>
        /// 是否已提交
        /// </summary>
        bool Committed { get; }

        /// <summary>
        /// 提交
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚
        /// </summary>
        void RollBack();
    }
}
