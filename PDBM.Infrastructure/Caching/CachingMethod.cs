using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Caching
{
    /// <summary>
    /// 缓存方式枚举值
    /// </summary>
    public enum CachingMethod
    {
        Set,
        Get,
        Remove,
        RemoveAll
    }
}
