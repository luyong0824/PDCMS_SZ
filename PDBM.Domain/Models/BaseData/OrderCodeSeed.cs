using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 单据编码种子
    /// </summary>
    public class OrderCodeSeed : AggregateRoot
    {
        protected OrderCodeSeed()
        {
        }

        /// <summary>
        /// 种子
        /// </summary>
        public string Seed
        {
            get;
            set;
        }
    }
}
