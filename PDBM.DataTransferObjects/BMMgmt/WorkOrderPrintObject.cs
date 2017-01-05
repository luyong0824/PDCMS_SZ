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
    /// 派工单打印对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WorkOrderPrintObject
    {
        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember, ProtoMember(1)]
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 派单人
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// 派单时间
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 工单类型
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 要求派工日期
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string RequireSendDate
        {
            get;
            set;
        }

        /// <summary>
        /// 现场联系人
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string SceneContactMan
        {
            get;
            set;
        }

        /// <summary>
        /// 现场联系电话
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string SceneContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 代维单位
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string CustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 派工时长
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string Days
        {
            get;
            set;
        }

        /// <summary>
        /// 代维联系人
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string MaintainContactMan
        {
            get;
            set;
        }

        /// <summary>
        /// 代维联系电话
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string MainTainContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 工作内容
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string WorkContent
        {
            get;
            set;
        }

        /// <summary>
        /// 用人要求
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string HumanRequire
        {
            get;
            set;
        }

        /// <summary>
        /// 用车要求
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string CarRequire
        {
            get;
            set;
        }

        /// <summary>
        /// 材料要求
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string MaterialRequire
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string WorkBeginDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间(小时)
        /// </summary>
        [DataMember, ProtoMember(18)]
        public string BeginHour
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间(分钟)
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string BeginMinute
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间
        /// </summary>
        [DataMember, ProtoMember(20)]
        public string WorkEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间(小时)
        /// </summary>
        [DataMember, ProtoMember(21)]
        public string EndHour
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间(分钟)
        /// </summary>
        [DataMember, ProtoMember(22)]
        public string EndMinute
        {
            get;
            set;
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        [DataMember, ProtoMember(23)]
        public string IsFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 结算登记人
        /// </summary>
        [DataMember, ProtoMember(24)]
        public string RegisterFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 结算登记日期
        /// </summary>
        [DataMember, ProtoMember(25)]
        public string RegisterDate
        {
            get;
            set;
        }

        /// <summary>
        /// 执行情况
        /// </summary>
        [DataMember, ProtoMember(26)]
        public string ExecuteSituation
        {
            get;
            set;
        }

        /// <summary>
        /// 材料消耗
        /// </summary>
        [DataMember, ProtoMember(27)]
        public string MaterialConsumption
        {
            get;
            set;
        }

        /// <summary>
        /// 人员数量
        /// </summary>
        [DataMember, ProtoMember(28)]
        public string PersonnelNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 车辆类型
        /// </summary>
        [DataMember, ProtoMember(29)]
        public string CarType
        {
            get;
            set;
        }

        /// <summary>
        /// 各天执行情况
        /// </summary>
        [DataMember, ProtoMember(30)]
        public string WorkOrderDetailInfoHtml
        {
            get;
            set;
        }

        /// <summary>
        /// 审批信息
        /// </summary>
        [DataMember, ProtoMember(31)]
        public string WFActivityInstancesInfoHtml
        {
            get;
            set;
        }

        /// <summary>
        /// 网格
        /// </summary>
        [DataMember, ProtoMember(32)]
        public string ReseauName
        {
            get;
            set;
        }

        /// <summary>
        /// 零星派工单Id
        /// </summary>
        [DataMember, ProtoMember(33)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(34)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 结算登记附件数量
        /// </summary>
        [DataMember, ProtoMember(35)]
        public int SettlementCount
        {
            get;
            set;
        }
    }
}
