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
    /// 任务卡片打印对象
    /// </summary>
    [DataContract, ProtoContract]
    public class ConstructionTaskPrintObject
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔类型名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string TowerTypeName
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
        /// 铁塔基础类型名称
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string TowerBaseTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// 机房类型名称
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string MachineRoomTypeName
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
        /// 外电引入类型名称
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string ExternalElectricName
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
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 移动安装是否完成
        /// </summary>
        [DataMember, ProtoMember(54)]
        public string IsFinishMobile
        {
            get;
            set;
        }

        /// <summary>
        /// 电信安装是否完成
        /// </summary>
        [DataMember, ProtoMember(55)]
        public string IsFinishTelecom
        {
            get;
            set;
        }

        /// <summary>
        /// 联通安装是否完成
        /// </summary>
        [DataMember, ProtoMember(56)]
        public string IsFinishUnicom
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔登记人名称
        /// </summary>
        [DataMember, ProtoMember(57)]
        public string TowerFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔登记日期
        /// </summary>
        [DataMember, ProtoMember(58)]
        public string TowerModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础登记人名称
        /// </summary>
        [DataMember, ProtoMember(59)]
        public string TowerBaseFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础登记日期
        /// </summary>
        [DataMember, ProtoMember(60)]
        public string TowerBaseModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 机房登记人名称
        /// </summary>
        [DataMember, ProtoMember(61)]
        public string MachineRoomFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 机房登记日期
        /// </summary>
        [DataMember, ProtoMember(62)]
        public string MachineRoomModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入登记人名称
        /// </summary>
        [DataMember, ProtoMember(63)]
        public string ExternalFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入登记日期
        /// </summary>
        [DataMember, ProtoMember(64)]
        public string ExternalModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装登记人名称
        /// </summary>
        [DataMember, ProtoMember(65)]
        public string EquipmentFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装登记日期
        /// </summary>
        [DataMember, ProtoMember(66)]
        public string EquipmentModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探登记人名称
        /// </summary>
        [DataMember, ProtoMember(67)]
        public string AddressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探登记日期
        /// </summary>
        [DataMember, ProtoMember(68)]
        public string AddressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测登记人名称
        /// </summary>
        [DataMember, ProtoMember(69)]
        public string FoundationFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测登记日期
        /// </summary>
        [DataMember, ProtoMember(70)]
        public string FoundationModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔建设进度
        /// </summary>
        [DataMember, ProtoMember(71)]
        public string TowerConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔进度简述
        /// </summary>
        [DataMember, ProtoMember(72)]
        public string TowerProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔现场影像附件数量
        /// </summary>
        [DataMember, ProtoMember(73)]
        public int TowerProgressCount
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔子任务Id
        /// </summary>
        [DataMember, ProtoMember(74)]
        public Guid TowerTaskPropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔建设进度登记人名称
        /// </summary>
        [DataMember, ProtoMember(75)]
        public string TowerProgressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔建设进度登记日期
        /// </summary>
        [DataMember, ProtoMember(76)]
        public string TowerProgressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔资料提交状态
        /// </summary>
        [DataMember, ProtoMember(77)]
        public string TowerSubmitState
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔资料提交人名称
        /// </summary>
        [DataMember, ProtoMember(78)]
        public string TowerSubmitFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔资料提交日期
        /// </summary>
        [DataMember, ProtoMember(79)]
        public string TowerSubmitModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础建设进度
        /// </summary>
        [DataMember, ProtoMember(80)]
        public string TowerBaseConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础进度简述
        /// </summary>
        [DataMember, ProtoMember(81)]
        public string TowerBaseProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础现场影像附件数量
        /// </summary>
        [DataMember, ProtoMember(82)]
        public int TowerBaseProgressCount
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础子任务Id
        /// </summary>
        [DataMember, ProtoMember(83)]
        public Guid TowerBaseTaskPropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础建设进度登记人名称
        /// </summary>
        [DataMember, ProtoMember(84)]
        public string TowerBaseProgressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础建设进度登记日期
        /// </summary>
        [DataMember, ProtoMember(85)]
        public string TowerBaseProgressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础资料提交状态
        /// </summary>
        [DataMember, ProtoMember(86)]
        public string TowerBaseSubmitState
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础资料提交人名称
        /// </summary>
        [DataMember, ProtoMember(87)]
        public string TowerBaseSubmitFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础资料提交日期
        /// </summary>
        [DataMember, ProtoMember(88)]
        public string TowerBaseSubmitModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 机房建设进度
        /// </summary>
        [DataMember, ProtoMember(89)]
        public string MachineRoomConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 机房进度简述
        /// </summary>
        [DataMember, ProtoMember(90)]
        public string MachineRoomProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 机房现场影像附件数量
        /// </summary>
        [DataMember, ProtoMember(91)]
        public int MachineRoomProgressCount
        {
            get;
            set;
        }

        /// <summary>
        /// 机房子任务Id
        /// </summary>
        [DataMember, ProtoMember(92)]
        public Guid MachineRoomTaskPropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// 机房建设进度登记人名称
        /// </summary>
        [DataMember, ProtoMember(93)]
        public string MachineRoomProgressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 机房建设进度登记日期
        /// </summary>
        [DataMember, ProtoMember(94)]
        public string MachineRoomProgressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 机房资料提交状态
        /// </summary>
        [DataMember, ProtoMember(95)]
        public string MachineRoomSubmitState
        {
            get;
            set;
        }

        /// <summary>
        /// 机房资料提交人名称
        /// </summary>
        [DataMember, ProtoMember(96)]
        public string MachineRoomSubmitFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 机房资料提交日期
        /// </summary>
        [DataMember, ProtoMember(97)]
        public string MachineRoomSubmitModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入建设进度
        /// </summary>
        [DataMember, ProtoMember(98)]
        public string ExternalConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入进度简述
        /// </summary>
        [DataMember, ProtoMember(99)]
        public string ExternalProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入现场影像附件数量
        /// </summary>
        [DataMember, ProtoMember(100)]
        public int ExternalProgressCount
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入子任务Id
        /// </summary>
        [DataMember, ProtoMember(101)]
        public Guid ExternalTaskPropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入建设进度登记人名称
        /// </summary>
        [DataMember, ProtoMember(102)]
        public string ExternalProgressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入建设进度登记日期
        /// </summary>
        [DataMember, ProtoMember(103)]
        public string ExternalProgressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入资料提交状态
        /// </summary>
        [DataMember, ProtoMember(104)]
        public string ExternalSubmitState
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入资料提交人名称
        /// </summary>
        [DataMember, ProtoMember(105)]
        public string ExternalSubmitFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入资料提交日期
        /// </summary>
        [DataMember, ProtoMember(106)]
        public string ExternalSubmitModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装建设进度
        /// </summary>
        [DataMember, ProtoMember(107)]
        public string EquipmentConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装进度简述
        /// </summary>
        [DataMember, ProtoMember(108)]
        public string EquipmentProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装现场影像附件数量
        /// </summary>
        [DataMember, ProtoMember(109)]
        public int EquipmentProgressCount
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装子任务Id
        /// </summary>
        [DataMember, ProtoMember(110)]
        public Guid EquipmentTaskPropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装建设进度登记人名称
        /// </summary>
        [DataMember, ProtoMember(111)]
        public string EquipmentProgressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装建设进度登记日期
        /// </summary>
        [DataMember, ProtoMember(112)]
        public string EquipmentProgressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装资料提交状态
        /// </summary>
        [DataMember, ProtoMember(113)]
        public string EquipmentSubmitState
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装资料提交人名称
        /// </summary>
        [DataMember, ProtoMember(114)]
        public string EquipmentSubmitFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装资料提交日期
        /// </summary>
        [DataMember, ProtoMember(115)]
        public string EquipmentSubmitModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探建设进度
        /// </summary>
        [DataMember, ProtoMember(116)]
        public string AddressConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探进度简述
        /// </summary>
        [DataMember, ProtoMember(117)]
        public string AddressProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探现场影像附件数量
        /// </summary>
        [DataMember, ProtoMember(118)]
        public int AddressProgressCount
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探子任务Id
        /// </summary>
        [DataMember, ProtoMember(119)]
        public Guid AddressTaskPropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探建设进度登记人名称
        /// </summary>
        [DataMember, ProtoMember(120)]
        public string AddressProgressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探建设进度登记日期
        /// </summary>
        [DataMember, ProtoMember(121)]
        public string AddressProgressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探资料提交状态
        /// </summary>
        [DataMember, ProtoMember(122)]
        public string AddressSubmitState
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探资料提交人名称
        /// </summary>
        [DataMember, ProtoMember(123)]
        public string AddressSubmitFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探资料提交日期
        /// </summary>
        [DataMember, ProtoMember(124)]
        public string AddressSubmitModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测建设进度
        /// </summary>
        [DataMember, ProtoMember(125)]
        public string FoundationConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测进度简述
        /// </summary>
        [DataMember, ProtoMember(126)]
        public string FoundationProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测现场影像附件数量
        /// </summary>
        [DataMember, ProtoMember(127)]
        public int FoundationProgressCount
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测子任务Id
        /// </summary>
        [DataMember, ProtoMember(128)]
        public Guid FoundationTaskPropertyId
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测建设进度登记人名称
        /// </summary>
        [DataMember, ProtoMember(129)]
        public string FoundationProgressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测建设进度登记日期
        /// </summary>
        [DataMember, ProtoMember(130)]
        public string FoundationProgressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测资料提交状态
        /// </summary>
        [DataMember, ProtoMember(131)]
        public string FoundationSubmitState
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测资料提交人名称
        /// </summary>
        [DataMember, ProtoMember(132)]
        public string FoundationSubmitFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测资料提交日期
        /// </summary>
        [DataMember, ProtoMember(133)]
        public string FoundationSubmitModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记人名称
        /// </summary>
        [DataMember, ProtoMember(134)]
        public string MobileFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 移动登记日期
        /// </summary>
        [DataMember, ProtoMember(135)]
        public string MobileModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记人名称
        /// </summary>
        [DataMember, ProtoMember(136)]
        public string TelecomFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 电信登记日期
        /// </summary>
        [DataMember, ProtoMember(137)]
        public string TelecomModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记人名称
        /// </summary>
        [DataMember, ProtoMember(138)]
        public string UnicomFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 联通登记日期
        /// </summary>
        [DataMember, ProtoMember(139)]
        public string UnicomModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        [DataMember, ProtoMember(140)]
        public string GroupPlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(141)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 区域名称
        /// </summary>
        [DataMember, ProtoMember(142)]
        public string AreaName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格名称
        /// </summary>
        [DataMember, ProtoMember(143)]
        public string ReseauName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型名称
        /// </summary>
        [DataMember, ProtoMember(144)]
        public string PlaceCategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 重要性程度
        /// </summary>
        [DataMember, ProtoMember(145)]
        public string ImportanceName
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(146)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(147)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 周边场景名称
        /// </summary>
        [DataMember, ProtoMember(148)]
        public string SceneName
        {
            get;
            set;
        }

        /// <summary>
        /// 产权
        /// </summary>
        [DataMember, ProtoMember(149)]
        public string PropertyRightName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主名称
        /// </summary>
        [DataMember, ProtoMember(150)]
        public string OwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember, ProtoMember(151)]
        public string OwnerContact
        {
            get;
            set;
        }

        /// <summary>
        /// 联系方式
        /// </summary>
        [DataMember, ProtoMember(152)]
        public string OwnerPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信需求
        /// </summary>
        [DataMember, ProtoMember(153)]
        public string TelecomDemandName
        {
            get;
            set;
        }

        /// <summary>
        /// 移动需求
        /// </summary>
        [DataMember, ProtoMember(154)]
        public string MobileDemandName
        {
            get;
            set;
        }

        /// <summary>
        /// 联通需求
        /// </summary>
        [DataMember, ProtoMember(155)]
        public string UnicomDemandName
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember, ProtoMember(156)]
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(157)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 附件列表Id
        /// </summary>
        [DataMember, ProtoMember(158)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 登记人
        /// </summary>
        [DataMember, ProtoMember(159)]
        public string CreateUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 登记日期
        /// </summary>
        [DataMember, ProtoMember(160)]
        public string CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(161)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(162)]
        public Guid PlaceId
        {
            get;
            set;
        }
    }
}
