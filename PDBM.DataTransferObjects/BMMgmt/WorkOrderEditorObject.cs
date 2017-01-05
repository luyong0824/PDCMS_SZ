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
    /// 零星派工单审批对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WorkOrderEditorObject
    {
        /// <summary>
        /// 零星派工单Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间
        /// </summary>
        [DataMember, ProtoMember(2)]
        public DateTime WorkBeginDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间(小时)
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int BeginHour
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间(分钟)
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int BeginMinute
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间
        /// </summary>
        [DataMember, ProtoMember(5)]
        public DateTime WorkEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间(小时)
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int EndHour
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间(分钟)
        /// </summary>
        [DataMember, ProtoMember(7)]
        public int EndMinute
        {
            get;
            set;
        }

        /// <summary>
        /// 执行情况
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string ExecuteSituation
        {
            get;
            set;
        }

        /// <summary>
        /// 材料消耗
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string MaterialConsumption
        {
            get;
            set;
        }

        /// <summary>
        /// 人员数量
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string PersonnelNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 车辆类型
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string CarType
        {
            get;
            set;
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int IsFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 各天执行情况附件Id
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string WFFileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 各天执行情况附件数量
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int WFCount
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string WorkBeginDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string WorkEndDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(17)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 公文Id
        /// </summary>
        [DataMember, ProtoMember(18)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }
    }
}
