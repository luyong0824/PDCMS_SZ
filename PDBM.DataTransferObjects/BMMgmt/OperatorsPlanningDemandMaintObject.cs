using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DataTransferObjects.BMMgmt
{
    /// <summary>
    /// 改造站点需求确认维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class OperatorsPlanningDemandMaintObject
    {
        /// <summary>
        /// 改造站点需求确认维护对象Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 运营商规划Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid OperatorsPlanningId
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid PlaceId
        {
            get;
            set;
        }


        /// <summary>
        /// 需求确认
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int Demand
        {
            get;
            set;
        }

        /// <summary>
        /// 确认用户Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid ConfirmUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 确认时间
        /// </summary>
        [DataMember, ProtoMember(6)]
        public DateTime ConfirmDate
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
        /// 创建时间
        /// </summary>
        [DataMember, ProtoMember(8)]
        public DateTime CreateDate
        {
            get;
            set;
        }
    }
}
