using PDBM.DataTransferObjects.BaseData;
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
    /// 项目任务修改实体
    /// </summary>
    [DataContract, ProtoContract]
    public class ProjectTaskEditObject
    {
        /// <summary>
        /// 项目任务Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 建设方式
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int ProjectType
        {
            get;
            set;
        }

        /// <summary>
        /// 父表Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目经理Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid AreaManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 总设单位Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid GeneralDesignId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计人
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string DesignRealName
        {
            get;
            set;
        }

        /// <summary>
        /// 设计日期
        /// </summary>
        [DataMember, ProtoMember(7)]
        public DateTime DesignDate
        {
            get;
            set;
        }

        /// <summary>
        /// 项目编号
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 项目进度
        /// </summary>
        [DataMember, ProtoMember(9)]
        public int ProjectProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 进度简述
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string ProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 项目开通日期
        /// </summary>
        [DataMember, ProtoMember(11)]
        public DateTime ProjectDate
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(13)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(14)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 公文批复Id
        /// </summary>
        [DataMember, ProtoMember(15)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目经理
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string AreaManagerName
        {
            get;
            set;
        }

        /// <summary>
        /// 总设单位
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string GeneralDesignName
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅Id
        /// </summary>
        [DataMember, ProtoMember(18)]
        public Guid Id1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础Id
        /// </summary>
        [DataMember, ProtoMember(19)]
        public Guid Id2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房Id
        /// </summary>
        [DataMember, ProtoMember(20)]
        public Guid Id3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入Id
        /// </summary>
        [DataMember, ProtoMember(21)]
        public Guid Id4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装Id
        /// </summary>
        [DataMember, ProtoMember(22)]
        public Guid Id5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路Id
        /// </summary>
        [DataMember, ProtoMember(23)]
        public Guid Id6
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅标记
        /// </summary>
        [DataMember, ProtoMember(24)]
        public int Mark1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础标记
        /// </summary>
        [DataMember, ProtoMember(25)]
        public int Mark2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房标记
        /// </summary>
        [DataMember, ProtoMember(26)]
        public int Mark3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入标记
        /// </summary>
        [DataMember, ProtoMember(27)]
        public int Mark4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装标记
        /// </summary>
        [DataMember, ProtoMember(28)]
        public int Mark5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路标记
        /// </summary>
        [DataMember, ProtoMember(29)]
        public int Mark6
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅工程经理Id
        /// </summary>
        [DataMember, ProtoMember(30)]
        public Guid ProjectManagerId1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅设计单位Id
        /// </summary>
        [DataMember, ProtoMember(31)]
        public Guid DesignCustomerId1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅施工单位Id
        /// </summary>
        [DataMember, ProtoMember(32)]
        public Guid ConstructionCustomerId1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅监理单位Id
        /// </summary>
        [DataMember, ProtoMember(33)]
        public Guid SupervisionCustomerId1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅工程经理名称
        /// </summary>
        [DataMember, ProtoMember(34)]
        public string ProjectManagerName1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅设计单位名称
        /// </summary>
        [DataMember, ProtoMember(35)]
        public string DesignCustomerName1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅施工单位名称
        /// </summary>
        [DataMember, ProtoMember(36)]
        public string ConstructionCustomerName1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅监理单位名称
        /// </summary>
        [DataMember, ProtoMember(37)]
        public string SupervisionCustomerName1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础工程经理Id
        /// </summary>
        [DataMember, ProtoMember(38)]
        public Guid ProjectManagerId2
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础设计单位Id
        /// </summary>
        [DataMember, ProtoMember(39)]
        public Guid DesignCustomerId2
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础施工单位Id
        /// </summary>
        [DataMember, ProtoMember(40)]
        public Guid ConstructionCustomerId2
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础监理单位Id
        /// </summary>
        [DataMember, ProtoMember(41)]
        public Guid SupervisionCustomerId2
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础工程经理名称
        /// </summary>
        [DataMember, ProtoMember(42)]
        public string ProjectManagerName2
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础设计单位名称
        /// </summary>
        [DataMember, ProtoMember(43)]
        public string DesignCustomerName2
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础施工单位名称
        /// </summary>
        [DataMember, ProtoMember(44)]
        public string ConstructionCustomerName2
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础监理单位名称
        /// </summary>
        [DataMember, ProtoMember(45)]
        public string SupervisionCustomerName2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房工程经理Id
        /// </summary>
        [DataMember, ProtoMember(46)]
        public Guid ProjectManagerId3
        {
            get;
            set;
        }

        /// <summary>
        /// 机房设计单位Id
        /// </summary>
        [DataMember, ProtoMember(47)]
        public Guid DesignCustomerId3
        {
            get;
            set;
        }

        /// <summary>
        /// 机房施工单位Id
        /// </summary>
        [DataMember, ProtoMember(48)]
        public Guid ConstructionCustomerId3
        {
            get;
            set;
        }

        /// <summary>
        /// 机房监理单位Id
        /// </summary>
        [DataMember, ProtoMember(49)]
        public Guid SupervisionCustomerId3
        {
            get;
            set;
        }

        /// <summary>
        /// 机房工程经理名称
        /// </summary>
        [DataMember, ProtoMember(50)]
        public string ProjectManagerName3
        {
            get;
            set;
        }

        /// <summary>
        /// 机房设计单位名称
        /// </summary>
        [DataMember, ProtoMember(51)]
        public string DesignCustomerName3
        {
            get;
            set;
        }

        /// <summary>
        /// 机房施工单位名称
        /// </summary>
        [DataMember, ProtoMember(52)]
        public string ConstructionCustomerName3
        {
            get;
            set;
        }

        /// <summary>
        /// 机房监理单位名称
        /// </summary>
        [DataMember, ProtoMember(53)]
        public string SupervisionCustomerName3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入工程经理Id
        /// </summary>
        [DataMember, ProtoMember(54)]
        public Guid ProjectManagerId4
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入设计单位Id
        /// </summary>
        [DataMember, ProtoMember(55)]
        public Guid DesignCustomerId4
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入施工单位Id
        /// </summary>
        [DataMember, ProtoMember(56)]
        public Guid ConstructionCustomerId4
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入监理单位Id
        /// </summary>
        [DataMember, ProtoMember(57)]
        public Guid SupervisionCustomerId4
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入工程经理名称
        /// </summary>
        [DataMember, ProtoMember(58)]
        public string ProjectManagerName4
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入设计单位名称
        /// </summary>
        [DataMember, ProtoMember(59)]
        public string DesignCustomerName4
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入施工单位名称
        /// </summary>
        [DataMember, ProtoMember(60)]
        public string ConstructionCustomerName4
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入监理单位名称
        /// </summary>
        [DataMember, ProtoMember(61)]
        public string SupervisionCustomerName4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装工程经理Id
        /// </summary>
        [DataMember, ProtoMember(62)]
        public Guid ProjectManagerId5
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装设计单位Id
        /// </summary>
        [DataMember, ProtoMember(63)]
        public Guid DesignCustomerId5
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装施工单位Id
        /// </summary>
        [DataMember, ProtoMember(64)]
        public Guid ConstructionCustomerId5
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装监理单位Id
        /// </summary>
        [DataMember, ProtoMember(65)]
        public Guid SupervisionCustomerId5
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装工程经理名称
        /// </summary>
        [DataMember, ProtoMember(66)]
        public string ProjectManagerName5
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装设计单位名称
        /// </summary>
        [DataMember, ProtoMember(67)]
        public string DesignCustomerName5
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装施工单位名称
        /// </summary>
        [DataMember, ProtoMember(68)]
        public string ConstructionCustomerName5
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装监理单位名称
        /// </summary>
        [DataMember, ProtoMember(69)]
        public string SupervisionCustomerName5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路工程经理Id
        /// </summary>
        [DataMember, ProtoMember(70)]
        public Guid ProjectManagerId6
        {
            get;
            set;
        }

        /// <summary>
        /// 线路设计单位Id
        /// </summary>
        [DataMember, ProtoMember(71)]
        public Guid DesignCustomerId6
        {
            get;
            set;
        }

        /// <summary>
        /// 线路施工单位Id
        /// </summary>
        [DataMember, ProtoMember(72)]
        public Guid ConstructionCustomerId6
        {
            get;
            set;
        }

        /// <summary>
        /// 线路监理单位Id
        /// </summary>
        [DataMember, ProtoMember(73)]
        public Guid SupervisionCustomerId6
        {
            get;
            set;
        }

        /// <summary>
        /// 线路工程经理名称
        /// </summary>
        [DataMember, ProtoMember(74)]
        public string ProjectManagerName6
        {
            get;
            set;
        }

        /// <summary>
        /// 线路设计单位名称
        /// </summary>
        [DataMember, ProtoMember(75)]
        public string DesignCustomerName6
        {
            get;
            set;
        }

        /// <summary>
        /// 线路施工单位名称
        /// </summary>
        [DataMember, ProtoMember(76)]
        public string ConstructionCustomerName6
        {
            get;
            set;
        }

        /// <summary>
        /// 线路监理单位名称
        /// </summary>
        [DataMember, ProtoMember(77)]
        public string SupervisionCustomerName6
        {
            get;
            set;
        }

        /// <summary>
        /// 总设图附件Id列表
        /// </summary>
        [DataMember, ProtoMember(78)]
        public string DesignFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 总设图附件数量
        /// </summary>
        [DataMember, ProtoMember(79)]
        public int DesignCount
        {
            get;
            set;
        }

        /// <summary>
        /// 总设日期文本
        /// </summary>
        [DataMember, ProtoMember(80)]
        public string DesignDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 项目进度
        /// </summary>
        [DataMember, ProtoMember(81)]
        public string ProjectProgressName
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅工程进度进度
        /// </summary>
        [DataMember, ProtoMember(82)]
        public string EngineeringProgressName1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础工程进度进度
        /// </summary>
        [DataMember, ProtoMember(83)]
        public string EngineeringProgressName2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房工程进度进度
        /// </summary>
        [DataMember, ProtoMember(84)]
        public string EngineeringProgressName3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入工程进度进度
        /// </summary>
        [DataMember, ProtoMember(85)]
        public string EngineeringProgressName4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装工程进度进度
        /// </summary>
        [DataMember, ProtoMember(86)]
        public string EngineeringProgressName5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路工程进度进度
        /// </summary>
        [DataMember, ProtoMember(87)]
        public string EngineeringProgressName6
        {
            get;
            set;
        }

        /// <summary>
        /// 2G逻辑号
        /// </summary>
        [DataMember, ProtoMember(88)]
        public string G2Number
        {
            get;
            set;
        }

        /// <summary>
        /// D2逻辑号
        /// </summary>
        [DataMember, ProtoMember(89)]
        public string D2Number
        {
            get;
            set;
        }

        /// <summary>
        /// 3G逻辑号
        /// </summary>
        [DataMember, ProtoMember(90)]
        public string G3Number
        {
            get;
            set;
        }

        /// <summary>
        /// 4G逻辑号
        /// </summary>
        [DataMember, ProtoMember(91)]
        public string G4Number
        {
            get;
            set;
        }

        /// <summary>
        /// 5G逻辑号
        /// </summary>
        [DataMember, ProtoMember(92)]
        public string G5Number
        {
            get;
            set;
        }

        /// <summary>
        /// 项目开通日期
        /// </summary>
        [DataMember, ProtoMember(93)]
        public string ProjectDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 文件Id列表
        /// </summary>
        [DataMember, ProtoMember(94)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(95)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例信息HTML字符串
        /// </summary>
        [DataMember, ProtoMember(96)]
        public string WFActivityInstancesInfoHtml
        {
            get;
            set;
        }

        /// <summary>
        /// 项目启动日期
        /// </summary>
        [DataMember, ProtoMember(97)]
        public DateTime ProjectBeginDate
        {
            get;
            set;
        }

        /// <summary>
        /// 项目启动日期
        /// </summary>
        [DataMember, ProtoMember(98)]
        public string ProjectBeginDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 站点状态
        /// </summary>
        [DataMember, ProtoMember(99)]
        public int PlaceState
        {
            get;
            set;
        }

        /// <summary>
        /// 现场摄像附件数量
        /// </summary>
        [DataMember, ProtoMember(100)]
        public int ImageCount
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(101)]
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅设计人
        /// </summary>
        [DataMember, ProtoMember(102)]
        public string DesignRealName1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础设计人
        /// </summary>
        [DataMember, ProtoMember(103)]
        public string DesignRealName2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房设计人
        /// </summary>
        [DataMember, ProtoMember(104)]
        public string DesignRealName3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入设计人
        /// </summary>
        [DataMember, ProtoMember(105)]
        public string DesignRealName4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装设计人
        /// </summary>
        [DataMember, ProtoMember(106)]
        public string DesignRealName5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路设计人
        /// </summary>
        [DataMember, ProtoMember(107)]
        public string DesignRealName6
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅施工图数量
        /// </summary>
        [DataMember, ProtoMember(108)]
        public int DesignCount1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础施工图数量
        /// </summary>
        [DataMember, ProtoMember(109)]
        public int DesignCount2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房施工图数量
        /// </summary>
        [DataMember, ProtoMember(110)]
        public int DesignCount3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入施工图数量
        /// </summary>
        [DataMember, ProtoMember(111)]
        public int DesignCount4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装施工图数量
        /// </summary>
        [DataMember, ProtoMember(112)]
        public int DesignCount5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路施工图数量
        /// </summary>
        [DataMember, ProtoMember(113)]
        public int DesignCount6
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅设计简述
        /// </summary>
        [DataMember, ProtoMember(114)]
        public string DesignMemos1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础设计简述
        /// </summary>
        [DataMember, ProtoMember(115)]
        public string DesignMemos2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房设计简述
        /// </summary>
        [DataMember, ProtoMember(116)]
        public string DesignMemos3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入设计简述
        /// </summary>
        [DataMember, ProtoMember(117)]
        public string DesignMemos4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装设计简述
        /// </summary>
        [DataMember, ProtoMember(118)]
        public string DesignMemos5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路设计简述
        /// </summary>
        [DataMember, ProtoMember(119)]
        public string DesignMemos6
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅设计完成
        /// </summary>
        [DataMember, ProtoMember(120)]
        public string DesignStateName1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础设计完成
        /// </summary>
        [DataMember, ProtoMember(121)]
        public string DesignStateName2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房设计完成
        /// </summary>
        [DataMember, ProtoMember(122)]
        public string DesignStateName3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入设计完成
        /// </summary>
        [DataMember, ProtoMember(123)]
        public string DesignStateName4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装设计完成
        /// </summary>
        [DataMember, ProtoMember(124)]
        public string DesignStateName5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路设计完成
        /// </summary>
        [DataMember, ProtoMember(125)]
        public string DesignStateName6
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅现场摄像数量
        /// </summary>
        [DataMember, ProtoMember(126)]
        public int ImageCount1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础现场摄像数量
        /// </summary>
        [DataMember, ProtoMember(127)]
        public int ImageCount2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房现场摄像数量
        /// </summary>
        [DataMember, ProtoMember(128)]
        public int ImageCount3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入现场摄像数量
        /// </summary>
        [DataMember, ProtoMember(129)]
        public int ImageCount4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装现场摄像数量
        /// </summary>
        [DataMember, ProtoMember(130)]
        public int ImageCount5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路现场摄像数量
        /// </summary>
        [DataMember, ProtoMember(131)]
        public int ImageCount6
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅设计日期
        /// </summary>
        [DataMember, ProtoMember(132)]
        public string DesignDateText1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础设计日期
        /// </summary>
        [DataMember, ProtoMember(133)]
        public string DesignDateText2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房设计日期
        /// </summary>
        [DataMember, ProtoMember(134)]
        public string DesignDateText3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入设计日期
        /// </summary>
        [DataMember, ProtoMember(135)]
        public string DesignDateText4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装设计日期
        /// </summary>
        [DataMember, ProtoMember(136)]
        public string DesignDateText5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路设计日期
        /// </summary>
        [DataMember, ProtoMember(137)]
        public string DesignDateText6
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅进度简述
        /// </summary>
        [DataMember, ProtoMember(138)]
        public string ProgressMemos1
        {
            get;
            set;
        }

        /// <summary>
        /// 天桅基础进度简述
        /// </summary>
        [DataMember, ProtoMember(139)]
        public string ProgressMemos2
        {
            get;
            set;
        }

        /// <summary>
        /// 机房进度简述
        /// </summary>
        [DataMember, ProtoMember(140)]
        public string ProgressMemos3
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入进度简述
        /// </summary>
        [DataMember, ProtoMember(141)]
        public string ProgressMemos4
        {
            get;
            set;
        }

        /// <summary>
        /// 设备安装进度简述
        /// </summary>
        [DataMember, ProtoMember(142)]
        public string ProgressMemos5
        {
            get;
            set;
        }

        /// <summary>
        /// 线路进度简述
        /// </summary>
        [DataMember, ProtoMember(143)]
        public string ProgressMemos6
        {
            get;
            set;
        }

        /// <summary>
        /// 建设方式
        /// </summary>
        [DataMember, ProtoMember(144)]
        public string ProjectTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// 现场影像base64编码
        /// </summary>
        [DataMember, ProtoMember(145)]
        public string[] Base64String
        {
            get;
            set;
        }

        /// <summary>
        /// 图片url
        /// </summary>
        [DataMember, ProtoMember(146)]
        public string ImageUrl
        {
            get;
            set;
        }
    }
}
