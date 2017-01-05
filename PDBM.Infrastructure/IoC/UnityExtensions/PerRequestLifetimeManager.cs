using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Practices.Unity;

namespace PDBM.Infrastructure.IoC.UnityExtensions
{
    public class PerRequestLifetimeManager : LifetimeManager
    {
        private Guid key = Guid.NewGuid();
        private readonly object syncObj = new object();

        public PerRequestLifetimeManager()
            : base()
        {
        }

        private IDictionary<Guid, object> GetUnityHttpContextObjects()
        {
            IDictionary<Guid, object> objs = HttpContext.Current.Items[PerRequestLifetimeManagerHttpModule.HTTPCONTEXTKEY] as IDictionary<Guid, object>;
            if (objs == null)
            {
                lock (syncObj)
                {
                    if (HttpContext.Current.Items[PerRequestLifetimeManagerHttpModule.HTTPCONTEXTKEY] == null)
                    {
                        objs = new Dictionary<Guid, object>();
                        HttpContext.Current.Items[PerRequestLifetimeManagerHttpModule.HTTPCONTEXTKEY] = objs;
                    }
                    else
                    {
                        return HttpContext.Current.Items[PerRequestLifetimeManagerHttpModule.HTTPCONTEXTKEY] as IDictionary<Guid, object>;
                    }
                }
            }
            return objs;
        }

        public override object GetValue()
        {
            IDictionary<Guid, object> objs = this.GetUnityHttpContextObjects();
            object obj = null;
            if (objs.TryGetValue(this.key, out obj))
            {
                return obj;
            }
            return null;
        }

        public override void RemoveValue()
        {
            IDictionary<Guid, object> objs = this.GetUnityHttpContextObjects();
            object obj = null;
            if (objs.TryGetValue(this.key, out obj))
            {
                IDisposable disposableObj = obj as IDisposable;
                if (disposableObj != null)
                {
                    disposableObj.Dispose();
                }
                objs.Remove(this.key);
            }
        }

        public override void SetValue(object newValue)
        {
            if(newValue == null)
            {
                throw new ArgumentNullException("newValue");
            }

            IDictionary<Guid, object> objs = this.GetUnityHttpContextObjects();
            objs.Add(this.key, newValue);
        }
    }
}
