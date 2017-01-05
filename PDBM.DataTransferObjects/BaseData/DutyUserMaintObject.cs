﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.BaseData
{
    /// <summary>
    /// 职务用户维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class DutyUserMaintObject
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
        /// 职务
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int Duty
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
