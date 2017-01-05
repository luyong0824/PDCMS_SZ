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
    /// 新模式改造基站批量导入对象
    /// </summary>
    [DataContract, ProtoContract]
    public class NewRemodelingImportObject
    {
        /// <summary>
        /// 移动需求
        /// </summary>
        [DataMember, ProtoMember(1)]
        public int MobileDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 电信需求
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int TelecomDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 联通需求
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int UnicomDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 基站名称
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
        /// 移动用电量(KW)
        /// </summary>
        [DataMember, ProtoMember(6)]
        public decimal MobilePowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 移动抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(7)]
        public int MobilePoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int MobileCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动确认人用户Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid MobileUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 电信用电量(KW)
        /// </summary>
        [DataMember, ProtoMember(10)]
        public decimal TelecomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 电信抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(11)]
        public int TelecomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int TelecomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信确认人用户Id
        /// </summary>
        [DataMember, ProtoMember(13)]
        public Guid TelecomUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 联通用电量(KW)
        /// </summary>
        [DataMember, ProtoMember(14)]
        public decimal UnicomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 联通抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(15)]
        public int UnicomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(16)]
        public int UnicomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通确认人用户Id
        /// </summary>
        [DataMember, ProtoMember(17)]
        public Guid UnicomUserId
        {
            get;
            set;
        }
    }
}
