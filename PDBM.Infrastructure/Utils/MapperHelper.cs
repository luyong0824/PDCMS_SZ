using EmitMapper;
using EmitMapper.MappingConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Utils
{
    /// <summary>
    /// 映射工具
    /// </summary>
    public static class MapperHelper
    {
        /// <summary>
        /// 将源对象映射为目标对象
        /// </summary>
        /// <typeparam name="TFrom">源类型</typeparam>
        /// <typeparam name="TTo">目标类型</typeparam>
        /// <param name="from">源对象</param>
        /// <returns>目标对象</returns>
        public static TTo Map<TFrom, TTo>(TFrom from)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            return ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>(new DefaultMapConfig().NullSubstitution<Guid?, Guid>((value) => Guid.Empty)).Map(from);
        }
    }
}
