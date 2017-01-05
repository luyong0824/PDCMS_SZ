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
    /// 任务属性维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class TaskPropertyMaintObject
    {
        /// <summary>
        /// 任务属性Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 任务Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid ConstructionTaskId
        {
            get;
            set;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int TaskModel
        {
            get;
            set;
        }

        /// <summary>
        /// 资源Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 建设进度
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int ConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 进度简述
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string ProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 进度登记人
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid? ProgressUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 进度登记日期
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string ProgressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 资料提交状态
        /// </summary>
        [DataMember, ProtoMember(9)]
        public int SubmitState
        {
            get;
            set;
        }

        /// <summary>
        /// 资料提交人
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid? SubmitUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 资料提交日期
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string SubmitModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 进度登记人名称
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string ProgressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 资料提交人名称
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string SubmitFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 施工单位Id
        /// </summary>
        [DataMember, ProtoMember(15)]
        public Guid ConstructionCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位Id
        /// </summary>
        [DataMember, ProtoMember(16)]
        public Guid SupervisorCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔类型
        /// </summary>
        [DataMember, ProtoMember(17)]
        public int TowerType
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔高度
        /// </summary>
        [DataMember, ProtoMember(18)]
        public decimal TowerHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 平台数量
        /// </summary>
        [DataMember, ProtoMember(19)]
        public int PlatFormNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 抱杆数量
        /// </summary>
        [DataMember, ProtoMember(20)]
        public int PoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础类型
        /// </summary>
        [DataMember, ProtoMember(21)]
        public int TowerBaseType
        {
            get;
            set;
        }

        /// <summary>
        /// 机房类型
        /// </summary>
        [DataMember, ProtoMember(22)]
        public int MachineRoomType
        {
            get;
            set;
        }

        /// <summary>
        /// 机房面积
        /// </summary>
        [DataMember, ProtoMember(23)]
        public decimal MachineRoomArea
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入类型
        /// </summary>
        [DataMember, ProtoMember(24)]
        public int ExternalElectric
        {
            get;
            set;
        }

        /// <summary>
        /// 开关电源
        /// </summary>
        [DataMember, ProtoMember(25)]
        public decimal SwitchPower
        {
            get;
            set;
        }

        /// <summary>
        /// 蓄电池
        /// </summary>
        [DataMember, ProtoMember(26)]
        public decimal Battery
        {
            get;
            set;
        }

        /// <summary>
        /// 机柜数量
        /// </summary>
        [DataMember, ProtoMember(27)]
        public int CabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔图纸
        /// </summary>
        [DataMember, ProtoMember(28)]
        public string FileIdListTower
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔图纸数量
        /// </summary>
        [DataMember, ProtoMember(29)]
        public int TowerCount
        {
            get;
            set;
        }

        /// <summary>
        /// 塔基图纸
        /// </summary>
        [DataMember, ProtoMember(30)]
        public string FileIdListTowerBase
        {
            get;
            set;
        }

        /// <summary>
        /// 塔基图纸数量
        /// </summary>
        [DataMember, ProtoMember(31)]
        public int TowerBaseCount
        {
            get;
            set;
        }

        /// <summary>
        /// 机房图纸
        /// </summary>
        [DataMember, ProtoMember(32)]
        public string FileIdListMachineRoom
        {
            get;
            set;
        }

        /// <summary>
        /// 机房图纸数量
        /// </summary>
        [DataMember, ProtoMember(33)]
        public int MachineRoomCount
        {
            get;
            set;
        }

        /// <summary>
        /// 路由图
        /// </summary>
        [DataMember, ProtoMember(34)]
        public string FileIdListExternal
        {
            get;
            set;
        }

        /// <summary>
        /// 路由图数量
        /// </summary>
        [DataMember, ProtoMember(35)]
        public int ExternalCount
        {
            get;
            set;
        }

        /// <summary>
        /// 安装图纸
        /// </summary>
        [DataMember, ProtoMember(36)]
        public string FileIdListEquipmentInstall
        {
            get;
            set;
        }

        /// <summary>
        /// 安装图纸数量
        /// </summary>
        [DataMember, ProtoMember(37)]
        public int EquipmentCount
        {
            get;
            set;
        }

        /// <summary>
        /// 地勘报告
        /// </summary>
        [DataMember, ProtoMember(38)]
        public string FileIdListAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 地勘报告数量
        /// </summary>
        [DataMember, ProtoMember(39)]
        public int AddressCount
        {
            get;
            set;
        }

        /// <summary>
        /// 动测报告
        /// </summary>
        [DataMember, ProtoMember(40)]
        public string FileIdListFoundation
        {
            get;
            set;
        }

        /// <summary>
        /// 动测报告数量
        /// </summary>
        [DataMember, ProtoMember(41)]
        public int FoundationCount
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔Id
        /// </summary>
        [DataMember, ProtoMember(42)]
        public Guid TowerId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础Id
        /// </summary>
        [DataMember, ProtoMember(43)]
        public Guid TowerBaseId
        {
            get;
            set;
        }

        /// <summary>
        /// 机房Id
        /// </summary>
        [DataMember, ProtoMember(44)]
        public Guid MachineRoomId
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入Id
        /// </summary>
        [DataMember, ProtoMember(45)]
        public Guid ExternalElectricPowerId
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装Id
        /// </summary>
        [DataMember, ProtoMember(46)]
        public Guid EquipmentInstallId
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探Id
        /// </summary>
        [DataMember, ProtoMember(47)]
        public Guid AddressExplorId
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测Id
        /// </summary>
        [DataMember, ProtoMember(48)]
        public Guid FoundationTestId
        {
            get;
            set;
        }

        /// <summary>
        /// 现场影像数量
        /// </summary>
        [DataMember, ProtoMember(49)]
        public int Count1
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔修改人名称
        /// </summary>
        [DataMember, ProtoMember(50)]
        public string TowerFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔修改日期
        /// </summary>
        [DataMember, ProtoMember(51)]
        public string TowerModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础修改人名称
        /// </summary>
        [DataMember, ProtoMember(52)]
        public string TowerBaseFullName
        {
            get;
            set;
        }

        /// <summary>
        ///铁塔基础修改日期 
        /// </summary>
        [DataMember, ProtoMember(53)]
        public string TowerBaseModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 机房修改人名称
        /// </summary>
        [DataMember, ProtoMember(54)]
        public string MachineRoomFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 机房修改日期
        /// </summary>
        [DataMember, ProtoMember(55)]
        public string MachineRoomModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入修改人名称
        /// </summary>
        [DataMember, ProtoMember(56)]
        public string ExternalFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入修改日期
        /// </summary>
        [DataMember, ProtoMember(57)]
        public string ExternalModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装修改人名称
        /// </summary>
        [DataMember, ProtoMember(58)]
        public string EquipmentFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装修改日期
        /// </summary>
        [DataMember, ProtoMember(59)]
        public string EquipmentModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探修改人名称
        /// </summary>
        [DataMember, ProtoMember(60)]
        public string AddressFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 地质勘探修改日期
        /// </summary>
        [DataMember, ProtoMember(61)]
        public string AddressModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测修改人名称
        /// </summary>
        [DataMember, ProtoMember(62)]
        public string FoundationFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测修改日期
        /// </summary>
        [DataMember, ProtoMember(63)]
        public string FoundationModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(64)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔预算
        /// </summary>
        [DataMember, ProtoMember(65)]
        public decimal TowerBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔基础预算
        /// </summary>
        [DataMember, ProtoMember(66)]
        public decimal TowerBaseBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 机房预算
        /// </summary>
        [DataMember, ProtoMember(67)]
        public decimal MachineRoomBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入预算
        /// </summary>
        [DataMember, ProtoMember(68)]
        public decimal ExternalBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装预算
        /// </summary>
        [DataMember, ProtoMember(69)]
        public decimal EquipmentBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 地址勘探预算
        /// </summary>
        [DataMember, ProtoMember(70)]
        public decimal AddressBudget
        {
            get;
            set;
        }

        /// <summary>
        /// 桩基动测预算
        /// </summary>
        [DataMember, ProtoMember(71)]
        public decimal FoundationBudget
        {
            get;
            set;
        }

        [DataMember, ProtoMember(72)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 现场影像数量
        /// </summary>
        [DataMember, ProtoMember(73)]
        public int Count2
        {
            get;
            set;
        }

        /// <summary>
        /// 现场影像数量
        /// </summary>
        [DataMember, ProtoMember(74)]
        public int Count3
        {
            get;
            set;
        }

        /// <summary>
        /// 现场影像数量
        /// </summary>
        [DataMember, ProtoMember(75)]
        public int Count4
        {
            get;
            set;
        }

        /// <summary>
        /// 现场影像数量
        /// </summary>
        [DataMember, ProtoMember(76)]
        public int Count5
        {
            get;
            set;
        }

        /// <summary>
        /// 现场影像数量
        /// </summary>
        [DataMember, ProtoMember(77)]
        public int Count6
        {
            get;
            set;
        }

        /// <summary>
        /// 现场影像数量
        /// </summary>
        [DataMember, ProtoMember(78)]
        public int Count7
        {
            get;
            set;
        }
    }
}
