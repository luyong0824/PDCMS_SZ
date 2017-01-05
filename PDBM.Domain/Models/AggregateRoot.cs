using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Common;

namespace PDBM.Domain.Models
{
    /// <summary>
    /// 聚合根抽象基类
    /// </summary>
    public abstract class AggregateRoot : IAggregateRoot
    {
        /// <summary>
        /// 获取或设置当前领域实体的全局唯一标识
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 确定指定的对象是否等于当前对象
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <returns>如果指定的对象与当前对象相等，则返回true，否则返回false。</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            IAggregateRoot ar = obj as IAggregateRoot;
            if (ar == null)
            {
                return false;
            }
            return this.Id == ar.Id;
        }

        /// <summary>
        /// 返回当前领域实体的哈希代码
        /// </summary>
        /// <returns>当前领域实体的哈希代码</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
