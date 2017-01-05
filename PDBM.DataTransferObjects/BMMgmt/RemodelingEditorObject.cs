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
    /// 基站改造审批维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class RemodelingEditorObject
    {
        /// <summary>
        /// 基站改造Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string PlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 规划名称
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string PlanningName
        {
            get;
            set;
        }

        /// <summary>
        /// 区域名称
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string AreaName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格名称
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string ReseauName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型名称
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string PlaceCategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 紧要程度
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string UrgencyName
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(11)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(12)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 周边场景名称
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string SceneName
        {
            get;
            set;
        }

        /// <summary>
        /// 改造状态
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string OrderStateName
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
        /// 联系人
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string OwnerContact
        {
            get;
            set;
        }

        /// <summary>
        /// 联系方式
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string OwnerPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信共享
        /// </summary>
        [DataMember, ProtoMember(18)]
        public string TelecomShareName
        {
            get;
            set;
        }

        /// <summary>
        /// 移动共享
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string MobileShareName
        {
            get;
            set;
        }

        /// <summary>
        /// 联通共享
        /// </summary>
        [DataMember, ProtoMember(20)]
        public string UnicomShareName
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember, ProtoMember(21)]
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(22)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 项目Id
        /// </summary>
        [DataMember, ProtoMember(23)]
        public Guid? ProjectId
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理Id
        /// </summary>
        [DataMember, ProtoMember(24)]
        public Guid? ProjectManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理名字
        /// </summary>
        [DataMember, ProtoMember(25)]
        public string ProjectManagerName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(26)]
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 基站规划Id
        /// </summary>
        [DataMember, ProtoMember(27)]
        public Guid PlanningId
        {
            get;
            set;
        }

        /// <summary>
        /// 附件列表Id
        /// </summary>
        [DataMember, ProtoMember(28)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(29)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 设计单位
        /// </summary>
        [DataMember, ProtoMember(30)]
        public Guid DesignCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计单位名称
        /// </summary>
        [DataMember, ProtoMember(31)]
        public string DesignCustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 设计人员
        /// </summary>
        [DataMember, ProtoMember(32)]
        public Guid DesignUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计人员名称
        /// </summary>
        [DataMember, ProtoMember(33)]
        public string DesignUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点设计信息Id
        /// </summary>
        [DataMember, ProtoMember(34)]
        public Guid PlaceDesignId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔类型
        /// </summary>
        [DataMember, ProtoMember(35)]
        public int TowerType
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔高度
        /// </summary>
        [DataMember, ProtoMember(36)]
        public decimal TowerHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 平台数量
        /// </summary>
        [DataMember, ProtoMember(37)]
        public int PlatFormNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 抱杆数量
        /// </summary>
        [DataMember, ProtoMember(38)]
        public int PoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔预算价格
        /// </summary>
        [DataMember, ProtoMember(39)]
        public decimal TowerBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础类型
        /// </summary>
        [DataMember, ProtoMember(40)]
        public int TowerBaseType
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础预算价格
        /// </summary>
        [DataMember, ProtoMember(41)]
        public decimal TowerBaseBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 机房类型
        /// </summary>
        [DataMember, ProtoMember(42)]
        public int MachineRoomType
        {
            get;
            set;
        }

        /// <summary>
        /// 机房面积
        /// </summary>
        [DataMember, ProtoMember(43)]
        public decimal MachineRoomArea
        {
            get;
            set;
        }

        /// <summary>
        /// 机房预算价格
        /// </summary>
        [DataMember, ProtoMember(44)]
        public decimal MachineRoomBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入类型
        /// </summary>
        [DataMember, ProtoMember(45)]
        public int ExternalElectric
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入预算价格
        /// </summary>
        [DataMember, ProtoMember(46)]
        public decimal ExternalBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 开关电源
        /// </summary>
        [DataMember, ProtoMember(47)]
        public decimal SwitchPower
        {
            get;
            set;
        }

        /// <summary>
        /// 蓄电池
        /// </summary>
        [DataMember, ProtoMember(48)]
        public decimal Battery
        {
            get;
            set;
        }

        /// <summary>
        /// 机柜数量
        /// </summary>
        [DataMember, ProtoMember(49)]
        public int CabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装预算价格
        /// </summary>
        [DataMember, ProtoMember(50)]
        public decimal EquipmentBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探预算价格
        /// </summary>
        [DataMember, ProtoMember(51)]
        public decimal AddressBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测预算价格
        /// </summary>
        [DataMember, ProtoMember(52)]
        public decimal FoundationBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔图纸
        /// </summary>
        [DataMember, ProtoMember(53)]
        public string TowerFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔图纸数量
        /// </summary>
        [DataMember, ProtoMember(54)]
        public int TowerCount
        {
            get;
            set;
        }

        /// <summary>
        /// 塔基图纸
        /// </summary>
        [DataMember, ProtoMember(55)]
        public string TowerBaseFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 塔基图纸数量
        /// </summary>
        [DataMember, ProtoMember(56)]
        public int TowerBaseCount
        {
            get;
            set;
        }

        /// <summary>
        /// 机房图纸
        /// </summary>
        [DataMember, ProtoMember(57)]
        public string MachineRoomFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 机房图纸数量
        /// </summary>
        [DataMember, ProtoMember(58)]
        public int MachineRoomCount
        {
            get;
            set;
        }

        /// <summary>
        /// 路由图
        /// </summary>
        [DataMember, ProtoMember(59)]
        public string ExternalFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 路由图数量
        /// </summary>
        [DataMember, ProtoMember(60)]
        public int ExternalCount
        {
            get;
            set;
        }

        /// <summary>
        /// 地勘报告
        /// </summary>
        [DataMember, ProtoMember(61)]
        public string AddressFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 地勘报告数量
        /// </summary>
        [DataMember, ProtoMember(62)]
        public int AddressCount
        {
            get;
            set;
        }

        /// <summary>
        /// 动测报告
        /// </summary>
        [DataMember, ProtoMember(63)]
        public string FoundationFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 动测报告数量
        /// </summary>
        [DataMember, ProtoMember(64)]
        public int FoundationCount
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有铁塔资源
        /// </summary>
        [DataMember, ProtoMember(65)]
        public int TowerMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有铁塔基础资源
        /// </summary>
        [DataMember, ProtoMember(66)]
        public int TowerBaseMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有机房资源
        /// </summary>
        [DataMember, ProtoMember(67)]
        public int MachineRoomMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有外电引入资源
        /// </summary>
        [DataMember, ProtoMember(68)]
        public int ExternalElectricPowerMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有设备安装资源
        /// </summary>
        [DataMember, ProtoMember(69)]
        public int EquipmentInstallMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有地质勘探资源
        /// </summary>
        [DataMember, ProtoMember(70)]
        public int AddressExplorMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有桩基动测资源
        /// </summary>
        [DataMember, ProtoMember(71)]
        public int FoundationTestMark
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔Id
        /// </summary>
        [DataMember, ProtoMember(72)]
        public Guid? TowerId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础Id
        /// </summary>
        [DataMember, ProtoMember(73)]
        public Guid? TowerBaseId
        {
            get;
            set;
        }

        /// <summary>
        /// 机房Id
        /// </summary>
        [DataMember, ProtoMember(74)]
        public Guid? MachineRoomId
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入Id
        /// </summary>
        [DataMember, ProtoMember(75)]
        public Guid? ExternalElectricPowerId
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装Id
        /// </summary>
        [DataMember, ProtoMember(76)]
        public Guid? EquipmentInstallId
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探Id
        /// </summary>
        [DataMember, ProtoMember(77)]
        public Guid? AddressExplorId
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测Id
        /// </summary>
        [DataMember, ProtoMember(78)]
        public Guid? FoundationTestId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(79)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 流程步骤Id
        /// </summary>
        [DataMember, ProtoMember(80)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 电信抱杆数量
        /// </summary>
        [DataMember, ProtoMember(81)]
        public int TelecomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信机柜数量
        /// </summary>
        [DataMember, ProtoMember(82)]
        public int TelecomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信用电量
        /// </summary>
        [DataMember, ProtoMember(83)]
        public decimal TelecomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 移动抱杆数量
        /// </summary>
        [DataMember, ProtoMember(84)]
        public int MobilePoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动机柜数量
        /// </summary>
        [DataMember, ProtoMember(85)]
        public int MobileCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动用电量
        /// </summary>
        [DataMember, ProtoMember(86)]
        public decimal MobilePowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 联通抱杆数量
        /// </summary>
        [DataMember, ProtoMember(87)]
        public int UnicomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通机柜数量
        /// </summary>
        [DataMember, ProtoMember(88)]
        public int UnicomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通用电量
        /// </summary>
        [DataMember, ProtoMember(89)]
        public decimal UnicomPowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember, ProtoMember(90)]
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 项目预算
        /// </summary>
        [DataMember, ProtoMember(91)]
        public decimal BudgetPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 集团站点编码
        /// </summary>
        [DataMember, ProtoMember(92)]
        public string GroupPlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔施工单位Id
        /// </summary>
        [DataMember, ProtoMember(93)]
        public Guid? TowerCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔施工单位名称
        /// </summary>
        [DataMember, ProtoMember(94)]
        public string TowerCustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础施工单位Id
        /// </summary>
        [DataMember, ProtoMember(95)]
        public Guid? TowerBaseCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础施工单位名称
        /// </summary>
        [DataMember, ProtoMember(96)]
        public string TowerBaseCustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 机房施工单位Id
        /// </summary>
        [DataMember, ProtoMember(97)]
        public Guid? MachineRoomCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 机房施工单位名称
        /// </summary>
        [DataMember, ProtoMember(98)]
        public string MachineRoomCustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入施工单位Id
        /// </summary>
        [DataMember, ProtoMember(99)]
        public Guid? ExternalCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入施工单位名称
        /// </summary>
        [DataMember, ProtoMember(100)]
        public string ExternalCustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装施工单位Id
        /// </summary>
        [DataMember, ProtoMember(101)]
        public Guid? EquipmentCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装施工单位名称
        /// </summary>
        [DataMember, ProtoMember(102)]
        public string EquipmentCustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探施工单位Id
        /// </summary>
        [DataMember, ProtoMember(103)]
        public Guid? AddressCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探施工单位名称
        /// </summary>
        [DataMember, ProtoMember(104)]
        public string AddressCustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测施工单位Id
        /// </summary>
        [DataMember, ProtoMember(105)]
        public Guid? FoundationCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测施工单位名称
        /// </summary>
        [DataMember, ProtoMember(106)]
        public string FoundationCustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位Id
        /// </summary>
        [DataMember, ProtoMember(107)]
        public Guid? SupervisorCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位名称
        /// </summary>
        [DataMember, ProtoMember(108)]
        public string SupervisorCustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 安装图纸数量
        /// </summary>
        [DataMember, ProtoMember(109)]
        public int EquipmentInstallCount
        {
            get;
            set;
        }

        /// <summary>
        /// 安装图纸
        /// </summary>
        [DataMember, ProtoMember(110)]
        public string EquipmentInstallFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔完成时限(天)
        /// </summary>
        [DataMember, ProtoMember(111)]
        public int TowerTimeLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础完成时限(天)
        /// </summary>
        [DataMember, ProtoMember(112)]
        public int TowerBaseTimeLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 机房完成时限(天)
        /// </summary>
        [DataMember, ProtoMember(113)]
        public int MachineRoomTimeLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入完成时限(天)
        /// </summary>
        [DataMember, ProtoMember(114)]
        public int ExternalTimeLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装完成时限(天)
        /// </summary>
        [DataMember, ProtoMember(115)]
        public int EquipmentTimeLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探完成时限(天)
        /// </summary>
        [DataMember, ProtoMember(116)]
        public int AddressTimeLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测完成时限(天)
        /// </summary>
        [DataMember, ProtoMember(117)]
        public int FoundationTimeLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 是否申请立项
        /// </summary>
        [DataMember, ProtoMember(118)]
        public int ProjectIsApply
        {
            get;
            set;
        }

        /// <summary>
        /// 是否完成立项
        /// </summary>
        [DataMember, ProtoMember(119)]
        public int ProjectIsDoApply
        {
            get;
            set;
        }

        /// <summary>
        /// 立项申请时间
        /// </summary>
        [DataMember, ProtoMember(120)]
        public string ProjectApplyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 立项完成时间
        /// </summary>
        [DataMember, ProtoMember(121)]
        public string ProjectDoApplyDate
        {
            get;
            set;
        }
    }
}
