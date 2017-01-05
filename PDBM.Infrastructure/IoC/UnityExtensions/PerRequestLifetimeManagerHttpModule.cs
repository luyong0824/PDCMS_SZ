using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PDBM.Infrastructure.IoC.UnityExtensions
{
    public class PerRequestLifetimeManagerHttpModule : IHttpModule
    {
        internal const string HTTPCONTEXTKEY = "PDBM.IoC.HttpContextObject";

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += context_EndRequest;
        }

        private void context_EndRequest(object sender, EventArgs e)
        {
            IDictionary<Guid, object> objs = HttpContext.Current.Items[HTTPCONTEXTKEY] as IDictionary<Guid, object>;
            if (objs != null)
            {
                foreach (Guid key in objs.Keys)
                {
                    IDisposable obj = objs[key] as IDisposable;
                    if (obj != null)
                    {
                        obj.Dispose();
                    }
                }
                HttpContext.Current.Items.Remove(HTTPCONTEXTKEY);
            }
        }
    }
}
