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
    /// 购置站点维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class PurchaseMaintObject
    {
        /// <summary>
        /// 购置站点Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 购置日期文本
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string PurchaseDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string PlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid PlaceCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(9)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(10)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 产权
        /// </summary>
        [DataMember, ProtoMember(11)]
        public int PropertyRight
        {
            get;
            set;
        }

        /// <summary>
        /// 重要性程度
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int Importance
        {
            get;
            set;
        }

        /// <summary>
        /// 周边场景Id
        /// </summary>
        [DataMember, ProtoMember(13)]
        public Guid SceneId
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 业主名称
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string OwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系人
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string OwnerContact
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系电话
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string OwnerPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 是否电信共享
        /// </summary>
        [DataMember, ProtoMember(18)]
        public int TelecomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 是否移动共享
        /// </summary>
        [DataMember, ProtoMember(19)]
        public int MobileShare
        {
            get;
            set;
        }

        /// <summary>
        /// 是否联通共享
        /// </summary>
        [DataMember, ProtoMember(20)]
        public int UnicomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(21)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 文件Id列表
        /// </summary>
        [DataMember, ProtoMember(22)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(23)]
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(24)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(25)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(26)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        [DataMember, ProtoMember(27)]
        public string GroupPlaceCode
        {
            get;
            set;
        }
    }
}
