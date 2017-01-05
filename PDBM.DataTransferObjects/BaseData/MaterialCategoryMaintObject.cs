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
    /// 物资类别维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class MaterialCategoryMaintObject
    {
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
        /// 物资类别编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string MaterialCategoryCode
        {
            get;
            set;
        }

        /// <summary>
        /// 物资类别名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string MaterialCategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid ModifyUserId
        {
            get;
            set;
        }
    }
}
