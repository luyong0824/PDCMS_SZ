using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.Common;

namespace PDBM.Infrastructure.Communication
{
    /// <summary>
    /// 通道工厂管理器
    /// </summary>
    internal sealed class ChannelFactoryManager : DisposableObject
    {
        private static readonly Dictionary<Type, ChannelFactory> factories = new Dictionary<Type, ChannelFactory>();
        private static readonly ChannelFactoryManager instance = new ChannelFactoryManager();
        private static readonly object syncObj = new object();

        private ChannelFactoryManager()
        {
        }

        /// <summary>
        /// 获取通道工厂管理器实例
        /// </summary>
        public static ChannelFactoryManager Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 获取与指定服务契约类型相关的通道工厂的实例
        /// </summary>
        /// <typeparam name="T">服务契约类型</typeparam>
        /// <returns>与指定服务契约类型相关的通道工厂的实例</returns>
        public ChannelFactory<T> GetFactory<T>()
            where T : class, IDistributedService
        {
            lock (syncObj)
            {
                ChannelFactory factory = null;
                if (!factories.TryGetValue(typeof(T), out factory))
                {
                    factory = new ChannelFactory<T>(typeof(T).Name);
                    factory.Open();
                    factories.Add(typeof(T), factory);
                }
                return factory as ChannelFactory<T>;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (syncObj)
                {
                    foreach (Type type in factories.Keys)
                    {
                        ChannelFactory factory = factories[type];
                        try
                        {
                            factory.Close();
                        }
                        catch
                        {
                            factory.Abort();
                        }
                    }
                    factories.Clear();
                }
            }
        }
    }
}
