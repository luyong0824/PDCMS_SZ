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
    /// 运营商共享维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class OperatorsSharingMaintObject
    {
        /// <summary>
        /// 运营商共享Id
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
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid PlaceId
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
        /// 站点类型Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid PlaceCategoryId
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
        /// 电信共享
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int TelecomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 移动共享
        /// </summary>
        [DataMember, ProtoMember(13)]
        public int MobileShare
        {
            get;
            set;
        }

        /// <summary>
        /// 联通共享
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int UnicomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 用电量
        /// </summary>
        [DataMember, ProtoMember(15)]
        public decimal PowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(16)]
        public int PoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(17)]
        public int CabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 紧要程度
        /// </summary>
        [DataMember, ProtoMember(18)]
        public int Urgency
        {
            get;
            set;
        }

        /// <summary>
        /// 是否采纳
        /// </summary>
        [DataMember, ProtoMember(19)]
        public int Solved
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(20)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        [DataMember, ProtoMember(21)]
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 改造Id
        /// </summary>
        [DataMember, ProtoMember(22)]
        public Guid RemodelingId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(23)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(24)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期文本
        /// </summary>
        [DataMember, ProtoMember(25)]
        public string CreateDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人所在公司性质
        /// </summary>
        [DataMember, ProtoMember(26)]
        public int CurrentCompanyNature
        {
            get;
            set;
        }
    }
}
