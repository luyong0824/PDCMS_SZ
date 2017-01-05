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
    /// 运营商使用情况历史记录维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class PlacePropertyLogMaintObject
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int PropertyType
        {
            get;
            set;
        }

        /// <summary>
        /// 移动是否完成安装
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int MobileIsFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 移动抱杆数量
        /// </summary>
        [DataMember, ProtoMember(15)]
        public int MobilePoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动机柜数量
        /// </summary>
        [DataMember, ProtoMember(16)]
        public int MobileCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动用电量
        /// </summary>
        [DataMember, ProtoMember(17)]
        public decimal MobilePowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记人
        /// </summary>
        [DataMember, ProtoMember(18)]
        public Guid MobileCreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记日期
        /// </summary>
        [DataMember, ProtoMember(19)]
        public DateTime MobileCreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 电信是否完成安装
        /// </summary>
        [DataMember, ProtoMember(20)]
        public int TelecomIsFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 电信抱杆数量
        /// </summary>
        [DataMember, ProtoMember(21)]
        public int TelecomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信机柜数量
        /// </summary>
        [DataMember, ProtoMember(22)]
        public int TelecomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信用电量
        /// </summary>
        [DataMember, ProtoMember(23)]
        public decimal TelecomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记人
        /// </summary>
        [DataMember, ProtoMember(24)]
        public Guid TelecomCreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记日期
        /// </summary>
        [DataMember, ProtoMember(25)]
        public DateTime TelecomCreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 联通是否完成安装
        /// </summary>
        [DataMember, ProtoMember(26)]
        public int UnicomIsFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 联通抱杆数量
        /// </summary>
        [DataMember, ProtoMember(27)]
        public int UnicomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通机柜数量
        /// </summary>
        [DataMember, ProtoMember(28)]
        public int UnicomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通用电量
        /// </summary>
        [DataMember, ProtoMember(29)]
        public decimal UnicomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记人
        /// </summary>
        [DataMember, ProtoMember(30)]
        public Guid UnicomCreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记日期
        /// </summary>
        [DataMember, ProtoMember(31)]
        public DateTime UnicomCreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(32)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记人姓名
        /// </summary>
        [DataMember, ProtoMember(33)]
        public string MobileFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记人姓名
        /// </summary>
        [DataMember, ProtoMember(34)]
        public string TelecomFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记人姓名
        /// </summary>
        [DataMember, ProtoMember(35)]
        public string UnicomFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(36)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        [DataMember, ProtoMember(37)]
        public int operationType
        {
            get;
            set;
        }
    }
}
