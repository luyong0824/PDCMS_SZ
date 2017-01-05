using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace PDBM.Infrastructure.Caching
{
    /// <summary>
    /// 基于System.Runtime.Caching.MemoryCache缓存机制的实现
    /// </summary>
    public class MemoryCacheProvider : ICacheProvider
    {
        private readonly ObjectCache cacheManager = MemoryCache.Default;

        /// <summary>
        /// 向缓存中添加一个对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        /// <param name="value">缓存的对象</param>
        public void Set(string key, string valueKey, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (string.IsNullOrWhiteSpace(valueKey))
            {
                throw new ArgumentNullException("valueKey");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Dictionary<string, object> dic = null;
            if (cacheManager.Contains(key))
            {
                dic = (Dictionary<string, object>)cacheManager[key];
            }
            else
            {
                dic = new Dictionary<string, object>();
            }
            dic[valueKey] = value;
            CacheItemPolicy policy = new CacheItemPolicy();
            cacheManager.Set(key, dic, policy);
        }

        /// <summary>
        /// 从缓存中读取一个对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        /// <returns>被缓存的对象</returns>
        public object Get(string key, string valueKey)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (string.IsNullOrWhiteSpace(valueKey))
            {
                throw new ArgumentNullException("valueKey");
            }

            if (cacheManager.Contains(key))
            {
                Dictionary<string, object> dic = (Dictionary<string, object>)cacheManager[key];
                if (dic.ContainsKey(valueKey))
                {
                    return dic[valueKey];
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// 根据缓存键值从缓存中移除对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        public void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            if (cacheManager.Contains(key))
            {
                cacheManager.Remove(key);
            }
        }

        /// <summary>
        /// 从缓存中移除一个对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        public void Remove(string key, string valueKey)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (string.IsNullOrWhiteSpace(valueKey))
            {
                throw new ArgumentNullException("valueKey");
            }

            if (cacheManager.Contains(key))
            {
                Dictionary<string, object> dic = (Dictionary<string, object>)cacheManager[key];
                if (dic.ContainsKey(valueKey))
                {
                    dic.Remove(valueKey);
                }
            }
        }

        /// <summary>
        /// 根据缓存键值判断是否存在缓存对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <returns>如果存在，则返回true，否则返回false</returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            return cacheManager.Contains(key);
        }

        /// <summary>
        /// 根据缓存键值和缓存对象的键值判断是否存在缓存对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="valueKey">缓存对象的键值</param>
        /// <returns>如果存在，则返回true，否则返回false</returns>
        public bool Exists(string key, string valueKey)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (string.IsNullOrWhiteSpace(valueKey))
            {
                throw new ArgumentNullException("valueKey");
            }

            return cacheManager.Contains(key) &&
                ((Dictionary<string, object>)cacheManager[key]).ContainsKey(valueKey);
        }
    }
}
