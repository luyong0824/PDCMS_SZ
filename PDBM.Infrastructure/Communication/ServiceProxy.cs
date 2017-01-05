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
    /// 客户端使用的服务代理
    /// </summary>
    /// <typeparam name="T">服务契约类型</typeparam>
    public sealed class ServiceProxy<T> : DisposableObject
        where T : class, IDistributedService
    {
        private T client = null;
        private readonly object syncObj = new object();

        /// <summary>
        /// 获取调用WCF服务的通道
        /// </summary>
        public T Channel
        {
            get
            {
                lock (syncObj)
                {
                    if (client != null)
                    {
                        if ((client as IClientChannel).State == CommunicationState.Closed)
                        {
                            client = null;
                        }
                        else
                        {
                            return client;
                        }
                    }
                    var factory = ChannelFactoryManager.Instance.GetFactory<T>();
                    client = factory.CreateChannel();
                    (client as IClientChannel).Open();
                    return client;
                }
            }
        }

        /// <summary>
        /// 关闭通道
        /// </summary>
        public void Close()
        {
            if (client != null)
            {
                var clientChannel = ((IClientChannel)client);
                try
                {
                    if (clientChannel.State != CommunicationState.Faulted)
                    {
                        clientChannel.Close();
                    }
                }
                catch
                {
                    clientChannel.Abort();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (syncObj)
                {
                    this.Close();
                }
            }
        }
    }
}
