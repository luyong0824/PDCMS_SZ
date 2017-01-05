using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Caching
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// 向缓存中添加一个对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        /// <param name="value">缓存的对象</param>
        void Set(string key, string valueKey, object value);

        /// <summary>
        /// 从缓存中读取一个对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        /// <returns>被缓存的对象</returns>
        object Get(string key, string valueKey);

        /// <summary>
        /// 从缓存中移除对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        void Remove(string key);

        /// <summary>
        /// 从缓存中移除一个对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        void Remove(string key, string valueKey);

        /// <summary>
        /// 根据缓存键值判断是否存在缓存对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <returns>如果存在，则返回true，否则返回false</returns>
        bool Exists(string key);

        /// <summary>
        /// 根据缓存键值和缓存对象的键值判断是否存在缓存对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        /// <returns>如果存在，则返回true，否则返回false</returns>
        bool Exists(string key, string valueKey);
    }
}
