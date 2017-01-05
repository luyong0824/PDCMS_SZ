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
    /// 站点类型维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class PlaceCategoryMaintObject
    {
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
        /// 专业
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型编码
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string PlaceCategoryCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型名称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string PlaceCategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid ModifyUserId
        {
            get;
            set;
        }
    }
}
