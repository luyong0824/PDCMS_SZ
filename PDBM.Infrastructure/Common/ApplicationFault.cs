using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Common
{
    /// <summary>
    /// 应用层异常错误
    /// </summary>
    public class ApplicationFault : Exception
    {
        public ApplicationFault()
            : base()
        {
        }

        public ApplicationFault(string message)
            : base(message)
        {
        }

        public ApplicationFault(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        protected ApplicationFault(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ApplicationFault(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
