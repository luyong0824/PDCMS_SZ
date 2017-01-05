using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.Common;

namespace PDBM.Infrastructure.Transaction
{
    /// <summary>
    /// 不支持分布式事务的事务协调器
    /// </summary>
    internal sealed class SuppressedTransactionCoordinator : TransactionCoordinator
    {
        public SuppressedTransactionCoordinator(params IUnitOfWork[] unitOfWorks)
            : base(unitOfWorks)
        {
        }
    }
}
