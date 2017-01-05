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
    /// 站点类型选择对象
    /// </summary>
    [DataContract, ProtoContract]
    public class PlaceCategorySelectObject
    {
        public PlaceCategorySelectObject()
        {
            PId = Guid.NewGuid();
        }

        /// <summary>
        /// 站点类型Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string PlaceCategoryName
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
