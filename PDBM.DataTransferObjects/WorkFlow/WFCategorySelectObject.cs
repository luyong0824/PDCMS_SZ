using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.WorkFlow
{
    /// <summary>
    /// 工作流类型选择对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WFCategorySelectObject
    {
        /// <summary>
        /// 工作流类型Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流类型名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string WFCategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 打印页地址
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string PrintUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 公文标题
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string WFProcessInstanceName
        {
            get;
            set;
        }
    }
}
