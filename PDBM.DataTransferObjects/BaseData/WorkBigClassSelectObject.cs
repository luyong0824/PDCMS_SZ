using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DataTransferObjects.BaseData
{
    /// <summary>
    /// 派工大类选择对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WorkBigClassSelectObject
    {
        public WorkBigClassSelectObject()
        {
            PId = Guid.Empty;
            isLeaf = true;
        }

        /// <summary>
        /// 派工大类Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 派工大类名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string BigClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 父级Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid PId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否叶子
        /// </summary>
        [DataMember, ProtoMember(4)]
        public bool isLeaf
        {
            get;
            set;
        }
    }
}
