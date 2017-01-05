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
    /// 盲点反馈维护实体
    /// </summary>
    [DataContract, ProtoContract]
    public class BlindSpotFeedBackMaintObject
    {
        /// <summary>
        /// 盲点反馈Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 盲点地名
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(4)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(5)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈内容
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string FeedBackContent
        {
            get;
            set;
        }

        /// <summary>
        /// 处理人
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid DoUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 处理状态
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int DoState
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈结果
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string FeedBackResult
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(12)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(13)]
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 附件Id列表
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 附件Id列表
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string[] Base64String
        {
            get;
            set;
        }
    }
}
