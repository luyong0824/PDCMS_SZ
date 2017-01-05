using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace PDBM.Infrastructure.IoC
{
    /// <summary>
    /// 服务定位器
    /// </summary>
    public sealed class ServiceLocator : IServiceProvider
    {
        private readonly UnityContainer container;
        private static readonly ServiceLocator instance = new ServiceLocator();

        private ServiceLocator()
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            container = new UnityContainer();
            section.Configure(container);
        }

        private IEnumerable<ParameterOverride> GetParameterOverrides(object overridedArguments)
        {
            List<ParameterOverride> overrides = new List<ParameterOverride>();
            Type argumentsType = overridedArguments.GetType();
            argumentsType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList()
                .ForEach(property =>
                {
                    var propertyValue = property.GetValue(overridedArguments, null);
                    var propertyName = property.Name;
                    overrides.Add(new ParameterOverride(propertyName, propertyValue));
                });
            return overrides;
        }

        /// <summary>
        /// 服务定位器实例
        /// </summary>
        public static ServiceLocator Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 根据服务类型获取服务实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        public T GetService<T>()
        {
            return container.Resolve<T>();
        }

        /// <summary>
        /// 根据服务类型获取服务实例
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns>服务实例</returns>
        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        /// <summary>
        /// 根据服务类型和参数对象获取服务实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="overridedArguments">参数对象</param>
        /// <returns>服务实例</returns>
        public T GetService<T>(object overridedArguments)
        {
            var overrides = GetParameterOverrides(overridedArguments);
            return container.Resolve<T>(overrides.ToArray());
        }

        /// <summary>
        /// 根据服务类型和参数对象获取服务实例
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="overridedArguments">参数对象</param>
        /// <returns>服务实例</returns>
        public object GetService(Type serviceType, object overridedArguments)
        {
            var overrides = GetParameterOverrides(overridedArguments);
            return container.Resolve(serviceType, overrides.ToArray());
        }

        /// <summary>
        /// 根据服务类型获取服务实例列表
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例列表</returns>
        public IEnumerable<T> GetServices<T>()
        {
            return container.ResolveAll<T>();
        }
    }
}
