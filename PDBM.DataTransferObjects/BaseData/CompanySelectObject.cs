using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.BaseData
{
    /// <summary>
    /// 公司选择对象
    /// </summary>
    [DataContract, ProtoContract]
    public class CompanySelectObject
    {
        public CompanySelectObject()
        {
            PId = Guid.Empty;
            isLeaf = true;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string CompanyName
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
