using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Common
{
    /// <summary>
    /// 领域层异常错误
    /// </summary>
    public class DomainFault : Exception
    {
        public DomainFault()
            : base()
        {
        }

        public DomainFault(string message)
            : base(message)
        {
        }

        public DomainFault(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        protected DomainFault(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public DomainFault(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
