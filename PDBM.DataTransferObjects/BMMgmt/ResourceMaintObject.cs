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
    /// 资源维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class ResourceMaintObject
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔类型
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int TowerType
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔高度
        /// </summary>
        [DataMember, ProtoMember(3)]
        public decimal TowerHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 平台数量
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int PlatFormNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 抱杆数量
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int PoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础类型
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int TowerBaseType
        {
            get;
            set;
        }

        /// <summary>
        /// 机房类型
        /// </summary>
        [DataMember, ProtoMember(7)]
        public int MachineRoomType
        {
            get;
            set;
        }

        /// <summary>
        /// 机房面积
        /// </summary>
        [DataMember, ProtoMember(8)]
        public decimal MachineRoomArea
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入类型
        /// </summary>
        [DataMember, ProtoMember(9)]
        public int ExternalElectric
        {
            get;
            set;
        }

        /// <summary>
        /// 开关电源
        /// </summary>
        [DataMember, ProtoMember(10)]
        public decimal SwitchPower
        {
            get;
            set;
        }

        /// <summary>
        /// 蓄电池
        /// </summary>
        [DataMember, ProtoMember(11)]
        public decimal Battery
        {
            get;
            set;
        }

        /// <summary>
        /// 机柜数量
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int CabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔图纸
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string TowerFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔图纸数量
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int TowerCount
        {
            get;
            set;
        }

        /// <summary>
        /// 塔基图纸
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string TowerBaseFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 塔基图纸数量
        /// </summary>
        [DataMember, ProtoMember(16)]
        public int TowerBaseCount
        {
            get;
            set;
        }

        /// <summary>
        /// 机房图纸
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string MachineRoomFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 机房图纸数量
        /// </summary>
        [DataMember, ProtoMember(18)]
        public int MachineRoomCount
        {
            get;
            set;
        }

        /// <summary>
        /// 路由图
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string ExternalFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 路由图数量
        /// </summary>
        [DataMember, ProtoMember(20)]
        public int ExternalCount
        {
            get;
            set;
        }

        /// <summary>
        /// 安装图纸数量
        /// </summary>
        [DataMember, ProtoMember(21)]
        public int EquipmentInstallCount
        {
            get;
            set;
        }

        /// <summary>
        /// 安装图纸
        /// </summary>
        [DataMember, ProtoMember(22)]
        public string EquipmentInstallFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 地勘报告
        /// </summary>
        [DataMember, ProtoMember(23)]
        public string AddressFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 地勘报告数量
        /// </summary>
        [DataMember, ProtoMember(24)]
        public int AddressCount
        {
            get;
            set;
        }

        /// <summary>
        /// 动测报告
        /// </summary>
        [DataMember, ProtoMember(25)]
        public string FoundationFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 动测报告数量
        /// </summary>
        [DataMember, ProtoMember(26)]
        public int FoundationCount
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有铁塔资源
        /// </summary>
        [DataMember, ProtoMember(27)]
        public int TowerMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有铁塔基础资源
        /// </summary>
        [DataMember, ProtoMember(28)]
        public int TowerBaseMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有机房资源
        /// </summary>
        [DataMember, ProtoMember(29)]
        public int MachineRoomMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有外电引入资源
        /// </summary>
        [DataMember, ProtoMember(30)]
        public int ExternalElectricPowerMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有设备安装资源
        /// </summary>
        [DataMember, ProtoMember(31)]
        public int EquipmentInstallMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有地质勘探资源
        /// </summary>
        [DataMember, ProtoMember(32)]
        public int AddressExplorMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有桩基动测资源
        /// </summary>
        [DataMember, ProtoMember(33)]
        public int FoundationTestMark
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔Id
        /// </summary>
        [DataMember, ProtoMember(34)]
        public Guid? TowerId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础Id
        /// </summary>
        [DataMember, ProtoMember(35)]
        public Guid? TowerBaseId
        {
            get;
            set;
        }

        /// <summary>
        /// 机房Id
        /// </summary>
        [DataMember, ProtoMember(36)]
        public Guid? MachineRoomId
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入Id
        /// </summary>
        [DataMember, ProtoMember(37)]
        public Guid? ExternalElectricPowerId
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装Id
        /// </summary>
        [DataMember, ProtoMember(38)]
        public Guid? EquipmentInstallId
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探Id
        /// </summary>
        [DataMember, ProtoMember(39)]
        public Guid? AddressExplorId
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测Id
        /// </summary>
        [DataMember, ProtoMember(40)]
        public Guid? FoundationTestId
        {
            get;
            set;
        }

        /// <summary>
        /// 电信抱杆数量
        /// </summary>
        [DataMember, ProtoMember(41)]
        public int TelecomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信机柜数量
        /// </summary>
        [DataMember, ProtoMember(42)]
        public int TelecomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信用电量
        /// </summary>
        [DataMember, ProtoMember(43)]
        public decimal TelecomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 移动抱杆数量
        /// </summary>
        [DataMember, ProtoMember(44)]
        public int MobilePoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动机柜数量
        /// </summary>
        [DataMember, ProtoMember(45)]
        public int MobileCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动用电量
        /// </summary>
        [DataMember, ProtoMember(46)]
        public decimal MobilePowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 联通抱杆数量
        /// </summary>
        [DataMember, ProtoMember(47)]
        public int UnicomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通机柜数量
        /// </summary>
        [DataMember, ProtoMember(48)]
        public int UnicomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通用电量
        /// </summary>
        [DataMember, ProtoMember(49)]
        public decimal UnicomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 移动共享
        /// </summary>
        [DataMember, ProtoMember(50)]
        public int MobileShare
        {
            get;
            set;
        }

        /// <summary>
        /// 电信共享
        /// </summary>
        [DataMember, ProtoMember(51)]
        public int TelecomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 联通共享
        /// </summary>
        [DataMember, ProtoMember(52)]
        public int UnicomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(53)]
        public Guid? ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔修改人名称
        /// </summary>
        [DataMember, ProtoMember(54)]
        public string TowerFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔修改日期
        /// </summary>
        [DataMember, ProtoMember(55)]
        public string TowerModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础修改人名称
        /// </summary>
        [DataMember, ProtoMember(56)]
        public string TowerBaseFullName
        {
            get;
            set;
        }

        /// <summary>
        ///铁塔基础修改日期 
        /// </summary>
        [DataMember, ProtoMember(57)]
        public string TowerBaseModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 机房修改人名称
        /// </summary>
        [DataMember, ProtoMember(58)]
        public string MachineRoomFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 机房修改日期
        /// </summary>
        [DataMember, ProtoMember(59)]
        public string MachineRoomModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入修改人名称
        /// </summary>
        [DataMember, ProtoMember(60)]
        public string ExternalFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入修改日期
        /// </summary>
        [DataMember, ProtoMember(61)]
        public string ExternalModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装修改人名称
        /// </summary>
        [DataMember, ProtoMember(62)]
        public string EquipmentFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装修改日期
        /// </summary>
        [DataMember, ProtoMember(63)]
        public string EquipmentModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探修改人名称
        /// </summary>
        [DataMember, ProtoMember(64)]
        public string AddressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探修改日期
        /// </summary>
        [DataMember, ProtoMember(65)]
        public string AddressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测修改人名称
        /// </summary>
        [DataMember, ProtoMember(66)]
        public string FoundationFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测修改日期
        /// </summary>
        [DataMember, ProtoMember(67)]
        public string FoundationModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记人名称
        /// </summary>
        [DataMember, ProtoMember(68)]
        public string MobileFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记日期
        /// </summary>
        [DataMember, ProtoMember(69)]
        public string MobileModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记人名称
        /// </summary>
        [DataMember, ProtoMember(70)]
        public string TelecomFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记日期
        /// </summary>
        [DataMember, ProtoMember(71)]
        public string TelecomModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记人名称
        /// </summary>
        [DataMember, ProtoMember(72)]
        public string UnicomFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记日期
        /// </summary>
        [DataMember, ProtoMember(73)]
        public string UnicomModifyDate
        {
            get;
            set;
        }
    }
}
