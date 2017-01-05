using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.Common;

namespace PDBM.Infrastructure.Transaction
{
    /// <summary>
    /// 事务协调器工厂
    /// </summary>
    public static class TransactionCoordinatorFactory
    {
        public static ITransactionCoordinator Create(params IUnitOfWork[] unitOfWorks)
        {
            if (unitOfWorks == null)
            {
                throw new ArgumentNullException("unitOfWorks");
            }

            bool isDistributedTransactionSupported = true;
            foreach (var unitOfWork in unitOfWorks)
            {
                if (unitOfWork == null)
                {
                    throw new ArgumentNullException("unitOfWork");
                }

                isDistributedTransactionSupported = isDistributedTransactionSupported && unitOfWork.DistributedTransactionSupported;
            }
            if (isDistributedTransactionSupported)
            {
                return new DistributedTransactionCoordinator(unitOfWorks);
            }
            else
            {
                return new SuppressedTransactionCoordinator(unitOfWorks);
            }
        }
    }
}
