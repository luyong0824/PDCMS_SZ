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
    /// 通知维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class NoticeMaintObject
    {
        /// <summary>
        /// 通知Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 通知类型
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int NoticeType
        {
            get;
            set;
        }

        /// <summary>
        /// 父表Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 原规划经度
        /// </summary>
        [DataMember, ProtoMember(4)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 原规划纬度
        /// </summary>
        [DataMember, ProtoMember(5)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 通知内容
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string NoticeContent
        {
            get;
            set;
        }

        /// <summary>
        /// 接收人用户Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid ReceiveUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 通知状态
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int NoticeState
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(11)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(12)]
        public DateTime ModifyDate
        {
            get;
            set;
        }
    }
}
