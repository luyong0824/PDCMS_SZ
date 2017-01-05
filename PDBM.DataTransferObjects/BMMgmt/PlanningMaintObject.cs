using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.BMMgmt
{
    /// <summary>
    /// 规划维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class PlanningMaintObject
    {
        /// <summary>
        /// 规划Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 规划编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string PlanningCode
        {
            get;
            set;
        }

        /// <summary>
        /// 规划名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string PlanningName
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid PlaceCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(8)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(9)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 紧要程度
        /// </summary>
        [DataMember, ProtoMember(10)]
        public int Urgency
        {
            get;
            set;
        }

        /// <summary>
        /// 电信需求
        /// </summary>
        [DataMember, ProtoMember(11)]
        public int TelecomDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 移动需求
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int MobileDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 联通需求
        /// </summary>
        [DataMember, ProtoMember(13)]
        public int UnicomDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 需求状态
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int DemandState
        {
            get;
            set;
        }

        /// <summary>
        /// 是否下达
        /// </summary>
        [DataMember, ProtoMember(15)]
        public int Issued
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址人用户Id
        /// </summary>
        [DataMember, ProtoMember(16)]
        public Guid AddressingUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址人姓名
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string AddressingUserFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址状态
        /// </summary>
        [DataMember, ProtoMember(18)]
        public int AddressingState
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(20)]
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(21)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(22)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期文本
        /// </summary>
        [DataMember, ProtoMember(23)]
        public string CreateDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember, ProtoMember(24)]
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 拟建网络
        /// </summary>
        [DataMember, ProtoMember(25)]
        public string ProposedNetwork
        {
            get;
            set;
        }

        /// <summary>
        /// 可选位置
        /// </summary>
        [DataMember, ProtoMember(26)]
        public string OptionalAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 重要性程度
        /// </summary>
        [DataMember, ProtoMember(27)]
        public int Importance
        {
            get;
            set;
        }

        /// <summary>
        /// 产权
        /// </summary>
        [DataMember, ProtoMember(28)]
        public Guid PlaceOwner
        {
            get;
            set;
        }

        /// <summary>
        /// 文件Id列表
        /// </summary>
        [DataMember, ProtoMember(29)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(30)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 图片url
        /// </summary>
        [DataMember, ProtoMember(31)]
        public string ImageUrl
        {
            get;
            set;
        }

        /// <summary>
        /// base64字符串
        /// </summary>
        [DataMember, ProtoMember(32)]
        public string[] Base64String
        {
            get;
            set;
        }
    }
}
