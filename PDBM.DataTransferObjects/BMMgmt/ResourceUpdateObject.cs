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
    /// 资源更新显示对象
    /// </summary>
    [DataContract, ProtoContract]
    public class ResourceUpdateObject
    {
        /// <summary>
        /// 铁塔类型
        /// </summary>
        [DataMember, ProtoMember(1)]
        public string TowerTypePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔高度
        /// </summary>
        [DataMember, ProtoMember(2)]
        public decimal TowerHeightPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 平台数量
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int PlatFormNumberPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 抱杆数量
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int PoleNumberPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础类型
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string TowerBaseTypePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 机房类型
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string MachineRoomTypePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 机房面积
        /// </summary>
        [DataMember, ProtoMember(7)]
        public decimal MachineRoomAreaPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入类型
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string ExternalElectricPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 开关电源
        /// </summary>
        [DataMember, ProtoMember(9)]
        public decimal SwitchPowerPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 蓄电池
        /// </summary>
        [DataMember, ProtoMember(10)]
        public decimal BatteryPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 机柜数量
        /// </summary>
        [DataMember, ProtoMember(11)]
        public int CabinetNumberPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔图纸数量
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int TowerCountPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 塔基图纸数量
        /// </summary>
        [DataMember, ProtoMember(13)]
        public int TowerBaseCountPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 机房图纸数量
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int MachineRoomCountPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 路由图数量
        /// </summary>
        [DataMember, ProtoMember(15)]
        public int ExternalCountPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 安装图纸数量
        /// </summary>
        [DataMember, ProtoMember(16)]
        public int EquipmentInstallCountPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 地勘报告数量
        /// </summary>
        [DataMember, ProtoMember(17)]
        public int AddressCountPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 动测报告数量
        /// </summary>
        [DataMember, ProtoMember(18)]
        public int FoundationCountPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔Id
        /// </summary>
        [DataMember, ProtoMember(19)]
        public Guid? TowerIdPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础Id
        /// </summary>
        [DataMember, ProtoMember(20)]
        public Guid? TowerBaseIdPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 机房Id
        /// </summary>
        [DataMember, ProtoMember(21)]
        public Guid? MachineRoomIdPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入Id
        /// </summary>
        [DataMember, ProtoMember(22)]
        public Guid? ExternalElectricPowerIdPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装Id
        /// </summary>
        [DataMember, ProtoMember(23)]
        public Guid? EquipmentInstallIdPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探Id
        /// </summary>
        [DataMember, ProtoMember(24)]
        public Guid? AddressExplorIdPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测Id
        /// </summary>
        [DataMember, ProtoMember(25)]
        public Guid? FoundationTestIdPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 电信抱杆数量
        /// </summary>
        [DataMember, ProtoMember(26)]
        public int TelecomPoleNumberPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 电信机柜数量
        /// </summary>
        [DataMember, ProtoMember(27)]
        public int TelecomCabinetNumberPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 电信用电量
        /// </summary>
        [DataMember, ProtoMember(28)]
        public decimal TelecomPowerUsedPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 移动抱杆数量
        /// </summary>
        [DataMember, ProtoMember(29)]
        public int MobilePoleNumberPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 移动机柜数量
        /// </summary>
        [DataMember, ProtoMember(30)]
        public int MobileCabinetNumberPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 移动用电量
        /// </summary>
        [DataMember, ProtoMember(31)]
        public decimal MobilePowerUsedPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 联通抱杆数量
        /// </summary>
        [DataMember, ProtoMember(32)]
        public int UnicomPoleNumberPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 联通机柜数量
        /// </summary>
        [DataMember, ProtoMember(33)]
        public int UnicomCabinetNumberPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 联通用电量
        /// </summary>
        [DataMember, ProtoMember(34)]
        public decimal UnicomPowerUsedPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔修改人名称
        /// </summary>
        [DataMember, ProtoMember(35)]
        public string TowerFullNamePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔修改日期
        /// </summary>
        [DataMember, ProtoMember(36)]
        public string TowerModifyDatePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础修改人名称
        /// </summary>
        [DataMember, ProtoMember(37)]
        public string TowerBaseFullNamePrint
        {
            get;
            set;
        }

        /// <summary>
        ///铁塔基础修改日期 
        /// </summary>
        [DataMember, ProtoMember(38)]
        public string TowerBaseModifyDatePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 机房修改人名称
        /// </summary>
        [DataMember, ProtoMember(39)]
        public string MachineRoomFullNamePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 机房修改日期
        /// </summary>
        [DataMember, ProtoMember(40)]
        public string MachineRoomModifyDatePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入修改人名称
        /// </summary>
        [DataMember, ProtoMember(41)]
        public string ExternalFullNamePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入修改日期
        /// </summary>
        [DataMember, ProtoMember(42)]
        public string ExternalModifyDatePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装修改人名称
        /// </summary>
        [DataMember, ProtoMember(43)]
        public string EquipmentFullNamePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装修改日期
        /// </summary>
        [DataMember, ProtoMember(44)]
        public string EquipmentModifyDatePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探修改人名称
        /// </summary>
        [DataMember, ProtoMember(45)]
        public string AddressFullNamePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探修改日期
        /// </summary>
        [DataMember, ProtoMember(46)]
        public string AddressModifyDatePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测修改人名称
        /// </summary>
        [DataMember, ProtoMember(47)]
        public string FoundationFullNamePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测修改日期
        /// </summary>
        [DataMember, ProtoMember(48)]
        public string FoundationModifyDatePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记人名称
        /// </summary>
        [DataMember, ProtoMember(49)]
        public string MobileFullNamePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记日期
        /// </summary>
        [DataMember, ProtoMember(50)]
        public string MobileModifyDatePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记人名称
        /// </summary>
        [DataMember, ProtoMember(51)]
        public string TelecomFullNamePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记日期
        /// </summary>
        [DataMember, ProtoMember(52)]
        public string TelecomModifyDatePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记人名称
        /// </summary>
        [DataMember, ProtoMember(53)]
        public string UnicomFullNamePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记日期
        /// </summary>
        [DataMember, ProtoMember(54)]
        public string UnicomModifyDatePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 移动安装是否完成
        /// </summary>
        [DataMember, ProtoMember(55)]
        public string IsFinishMobilePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 电信安装是否完成
        /// </summary>
        [DataMember, ProtoMember(56)]
        public string IsFinishTelecomPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 联通安装是否完成
        /// </summary>
        [DataMember, ProtoMember(57)]
        public string IsFinishUnicomPrint
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(58)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 移动共享
        /// </summary>
        [DataMember, ProtoMember(59)]
        public int MobileSharePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 电信共享
        /// </summary>
        [DataMember, ProtoMember(60)]
        public int TelecomSharePrint
        {
            get;
            set;
        }

        /// <summary>
        /// 联通共享
        /// </summary>
        [DataMember, ProtoMember(61)]
        public int UnicomSharePrint
        {
            get;
            set;
        }
    }
}
