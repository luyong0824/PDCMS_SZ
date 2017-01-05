using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 编码种子
    /// </summary>
    public class CodeSeed : AggregateRoot
    {
        protected CodeSeed()
        {
        }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName
        {
            get;
            set;
        }

        /// <summary>
        /// 位数
        /// </summary>
        public int Digit
        {
            get;
            set;
        }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix
        {
            get;
            set;
        }

        /// <summary>
        /// 种子
        /// </summary>
        public int Seed
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 行版本号
        /// </summary>
        public byte[] RowVersion
        {
            get;
            set;
        }
    }
}
