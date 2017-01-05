using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.Common;

namespace PDBM.Infrastructure.Transaction
{
    /// <summary>
    /// 事务协调器抽象类
    /// </summary>
    public abstract class TransactionCoordinator : DisposableObject, ITransactionCoordinator
    {
        private readonly List<IUnitOfWork> unitOfWorks = new List<IUnitOfWork>();

        public TransactionCoordinator(params IUnitOfWork[] unitOfWorks)
        {
            if (unitOfWorks == null)
            {
                throw new ArgumentNullException("unitOfWorks");
            }

            if (unitOfWorks.Length > 0)
            {
                foreach (var unitOfWork in unitOfWorks)
                {
                    if (unitOfWork == null)
                    {
                        throw new ArgumentNullException("unitOfWork");
                    }

                    this.unitOfWorks.Add(unitOfWork);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// 无意义
        /// </summary>
        public bool DistributedTransactionSupported
        {
            get { return false; }
        }

        /// <summary>
        /// 无意义
        /// </summary>
        public bool Committed
        {
            get { return false; }
        }

        public virtual void Commit()
        {
            if (unitOfWorks.Count > 0)
            {
                foreach (var unitOfWork in unitOfWorks)
                {
                    if (!unitOfWork.Committed)
                    {
                        unitOfWork.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// 无意义
        /// </summary>
        public virtual void RollBack()
        {
        }
    }
}
