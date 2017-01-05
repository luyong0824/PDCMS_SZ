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
    /// 工作流过程选择对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WFProcessSelectObject
    {
        public WFProcessSelectObject()
        {
            PId = Guid.Empty;
            isLeaf = true;
        }

        /// <summary>
        /// 工作流过程Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流类型Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid WFCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string WFProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// 父级Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid PId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否叶子
        /// </summary>
        [DataMember, ProtoMember(5)]
        public bool isLeaf
        {
            get;
            set;
        }
    }
}
