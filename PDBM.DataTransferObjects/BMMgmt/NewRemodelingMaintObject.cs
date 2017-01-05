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
    /// 改造维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class NewRemodelingMaintObject
    {
        /// <summary>
        /// 改造Id
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
        /// 电信需求
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int TelecomDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 移动需求
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int MobileDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 联通需求
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int UnicomDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期文本
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string CreateDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string PlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型Id
        /// </summary>
        [DataMember, ProtoMember(12)]
        public Guid PlaceCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(13)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(14)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 产权
        /// </summary>
        [DataMember, ProtoMember(15)]
        public int PropertyRight
        {
            get;
            set;
        }

        /// <summary>
        /// 电信共享
        /// </summary>
        [DataMember, ProtoMember(16)]
        public int TelecomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 移动共享
        /// </summary>
        [DataMember, ProtoMember(17)]
        public int MobileShare
        {
            get;
            set;
        }

        /// <summary>
        /// 联通共享
        /// </summary>
        [DataMember, ProtoMember(18)]
        public int UnicomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 改造流程状态
        /// </summary>
        [DataMember, ProtoMember(19)]
        public int OrderState
        {
            get;
            set;
        }

        /// <summary>
        /// 改造状态
        /// </summary>
        [DataMember, ProtoMember(20)]
        public string OrderStateName
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
        /// 移动抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(22)]
        public int MobilePoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(23)]
        public int MobileCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动用电量(KW)
        /// </summary>
        [DataMember, ProtoMember(24)]
        public decimal MobilePowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 移动确认人用户Id
        /// </summary>
        [DataMember, ProtoMember(25)]
        public Guid MobileUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 移动确认人用户名称
        /// </summary>
        [DataMember, ProtoMember(26)]
        public string MobileUserFullName
        {
            get;
            set;
        }


        /// <summary>
        /// 电信抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(27)]
        public int TelecomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(28)]
        public int TelecomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信用电量(KW)
        /// </summary>
        [DataMember, ProtoMember(29)]
        public decimal TelecomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 电信确认人用户Id
        /// </summary>
        [DataMember, ProtoMember(30)]
        public Guid TelecomUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 电信确认人用户名称
        /// </summary>
        [DataMember, ProtoMember(31)]
        public string TelecomUserFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 联通抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(32)]
        public int UnicomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(33)]
        public int UnicomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通用电量(KW)
        /// </summary>
        [DataMember, ProtoMember(34)]
        public decimal UnicomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 联通确认人用户Id
        /// </summary>
        [DataMember, ProtoMember(35)]
        public Guid UnicomUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 联通确认人用户名称
        /// </summary>
        [DataMember, ProtoMember(36)]
        public string UnicomUserFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(37)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(38)]
        public Guid ModifyUserId
        {
            get;
            set;
        }
    }
}
