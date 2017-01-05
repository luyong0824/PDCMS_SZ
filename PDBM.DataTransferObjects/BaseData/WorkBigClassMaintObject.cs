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
    /// 派工大类维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WorkBigClassMaintObject
    {
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
        /// 大类编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string BigClassCode
        {
            get;
            set;
        }

        /// <summary>
        /// 大类名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string BigClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 大类状态
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(8)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(9)]
        public DateTime ModifyDate
        {
            get;
            set;
        }
    }
}
