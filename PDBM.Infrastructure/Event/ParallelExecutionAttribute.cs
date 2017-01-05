using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Event
{
    /// <summary>
    /// 事件处理器并发执行特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ParallelExecutionAttribute : Attribute
    {
    }
}
