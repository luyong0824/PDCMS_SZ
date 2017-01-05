using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using PDBM.Infrastructure.Caching;

namespace PDBM.Infrastructure.IoC.InterceptionBehaviors
{
    /// <summary>
    /// 缓存拦截器
    /// </summary>
    public class CachingBehavior : IInterceptionBehavior
    {
        private string GetValueKey(IMethodInvocation input, CachingAttribute cachingAttribute)
        {
            switch (cachingAttribute.Method)
            {
                case CachingMethod.RemoveAll:
                    return null;
                case CachingMethod.Set:
                case CachingMethod.Get:
                case CachingMethod.Remove:
                    if (input.Arguments != null && input.Arguments.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < input.Arguments.Count; i++)
                        {
                            sb.Append(input.Arguments[i].GetType().ToString() + "_" + input.Arguments[i].ToString());
                            if (i != input.Arguments.Count - 1)
                            {
                                sb.Append("&");
                            }
                        }
                        return sb.ToString();
                    }
                    return "null";
                default:
                    throw new InvalidOperationException("无效的缓存方式");
            }
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var method = input.MethodBase;
            string key = method.ReflectedType.FullName + "_" + method.Name;
            if (method.IsDefined(typeof(CachingAttribute), false))
            {
                CachingAttribute cachingAttribute = (CachingAttribute)method.GetCustomAttributes(typeof(CachingAttribute), false)[0];
                string valueKey = GetValueKey(input, cachingAttribute);
                switch (cachingAttribute.Method)
                {
                    case CachingMethod.Set:
                        try
                        {
                            IMethodReturn methodReturn = getNext().Invoke(input, getNext);
                            CacheManager.Instance.Set(key, valueKey, methodReturn.ReturnValue);
                            return methodReturn;
                        }
                        catch (Exception ex)
                        {
                            return new VirtualMethodReturn(input, ex);
                        }
                    case CachingMethod.Get:
                        try
                        {
                            if (CacheManager.Instance.Exists(key, valueKey))
                            {
                                var returnValue = CacheManager.Instance.Get(key, valueKey);
                                var arguments = new object[input.Arguments.Count];
                                input.Arguments.CopyTo(arguments, 0);
                                return new VirtualMethodReturn(input, returnValue, arguments);
                            }
                            else
                            {
                                IMethodReturn methodReturn = getNext().Invoke(input, getNext);
                                CacheManager.Instance.Set(key, valueKey, methodReturn.ReturnValue);
                                return methodReturn;
                            }
                        }
                        catch (Exception ex)
                        {
                            return new VirtualMethodReturn(input, ex);
                        }
                    case CachingMethod.Remove: break;
                    case CachingMethod.RemoveAll:
                        try
                        {
                            string[] removeKeys = cachingAttribute.CorrespondingMethodNames;
                            foreach (var removeKey in removeKeys)
                            {
                                string fullKey = method.ReflectedType.FullName + "_" + removeKey;
                                if (CacheManager.Instance.Exists(fullKey))
                                    CacheManager.Instance.Remove(fullKey);
                            }
                            IMethodReturn methodReturn = getNext().Invoke(input, getNext);
                            return methodReturn;
                        }
                        catch (Exception ex)
                        {
                            return new VirtualMethodReturn(input, ex);
                        }
                    default: break;
                }
            }
            return getNext().Invoke(input, getNext);
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}
