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
    /// 往来单位用户维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class CustomerUserMaintObject
    {
        /// <summary>
        /// 岗位用户Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 往来单位Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid CreateUserId
        {
            get;
            set;
        }
    }
}
