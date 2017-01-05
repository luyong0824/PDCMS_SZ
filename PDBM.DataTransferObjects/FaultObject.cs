using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.Common;

namespace PDBM.DataTransferObjects
{
    /// <summary>
    /// 异常信息
    /// </summary>
    [DataContract]
    public class FaultObject
    {
        [DataMember]
        public string FaultType
        {
            get;
            set;
        }

        [DataMember]
        public string Message
        {
            get;
            set;
        }

        [DataMember]
        public string FullMessage
        {
            get;
            set;
        }

        [DataMember]
        public string StackTrace
        {
            get;
            set;
        }

        public static FaultObject CreateFromException(Exception ex)
        {
            return new FaultObject
            {
                FaultType = (ex is ApplicationFault || ex is DomainFault) ? "Bisuness" : "System",
                Message = ex.Message,
                FullMessage = ex.ToString(),
                StackTrace = ex.StackTrace
            };
        }

        public static FaultReason CreateFaultReason(Exception ex)
        {
            return new FaultReason(ex.Message);
        }
    }
}
