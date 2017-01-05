using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Caching
{
    /// <summary>
    /// 缓存特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CachingAttribute : Attribute
    {
        public CachingAttribute(CachingMethod method)
        {
            Method = method;
        }

        public CachingAttribute(CachingMethod method, params string[] correspondingMethodNames)
            : this(method)
        {
            if (correspondingMethodNames == null)
            {
                throw new ArgumentNullException("correspondingMethodNames");
            }

            CorrespondingMethodNames = correspondingMethodNames;
        }

        /// <summary>
        /// 获取或设置缓存方式
        /// </summary>
        public CachingMethod Method
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置与当前缓存方式相关的方法名称
        /// </summary>
        public string[] CorrespondingMethodNames
        {
            get;
            set;
        }
    }
}
