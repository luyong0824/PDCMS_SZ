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
    /// 物资类别树形菜单对象
    /// </summary>
    [DataContract, ProtoContract]
    public class MaterialCategoryTreeObject
    {
        public MaterialCategoryTreeObject()
        {
            PId = Guid.Empty;
            isLeaf = false;
        }

        /// <summary>
        /// 物资类别Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 物资类别名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string MaterialCategoryName
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
