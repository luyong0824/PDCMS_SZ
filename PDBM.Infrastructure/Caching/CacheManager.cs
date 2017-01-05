using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.IoC;

namespace PDBM.Infrastructure.Caching
{
    /// <summary>
    /// 缓存管理器
    /// </summary>
    public sealed class CacheManager : ICacheProvider
    {
        private readonly ICacheProvider cacheProvider;
        private static readonly CacheManager instance = new CacheManager();

        private CacheManager()
        {
            cacheProvider = ServiceLocator.Instance.GetService<ICacheProvider>();
        }

        /// <summary>
        /// 缓存管理器实例
        /// </summary>
        public static CacheManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// 向缓存中添加一个对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        /// <param name="value">缓存的对象</param>
        public void Set(string key, string valueKey, object value)
        {
            cacheProvider.Set(key, valueKey, value);
        }

        /// <summary>
        /// 从缓存中读取一个对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        /// <returns>被缓存的对象</returns>
        public object Get(string key, string valueKey)
        {
            return cacheProvider.Get(key, valueKey);
        }

        /// <summary>
        /// 从缓存中移除对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        public void Remove(string key)
        {
            cacheProvider.Remove(key);
        }

        /// <summary>
        /// 从缓存中移除一个对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        public void Remove(string key, string valueKey)
        {
            cacheProvider.Remove(key, valueKey);
        }

        /// <summary>
        /// 根据缓存键值判断是否存在缓存对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <returns>如果存在，则返回true，否则返回false</returns>
        public bool Exists(string key)
        {
            return cacheProvider.Exists(key);
        }

        /// <summary>
        /// 根据缓存键值和缓存对象的键值判断是否存在缓存对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        /// <returns>如果存在，则返回true，否则返回false</returns>
        public bool Exists(string key, string valueKey)
        {
            return cacheProvider.Exists(key, valueKey);
        }
    }
}
