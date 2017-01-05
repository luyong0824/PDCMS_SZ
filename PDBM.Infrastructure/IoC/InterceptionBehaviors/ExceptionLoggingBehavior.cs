using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;

namespace PDBM.Infrastructure.IoC.InterceptionBehaviors
{
    /// <summary>
    /// 异常拦截器
    /// </summary>
    public class ExceptionLoggingBehavior : IInterceptionBehavior
    {
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var methodReturn = getNext().Invoke(input, getNext);
            if (methodReturn.Exception != null)
            {
                if (!(methodReturn.Exception is ApplicationFault || methodReturn.Exception is DomainFault))
                {
                    LogHelper.Log(methodReturn.Exception);
                }
            }
            return methodReturn;
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}
