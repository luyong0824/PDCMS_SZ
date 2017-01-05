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
    /// 派工单明细维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WorkOrderDetailMaintObject
    {
        /// <summary>
        /// 派工单明细Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid WorkOrderDetailId
        {
            get;
            set;
        }

        /// <summary>
        /// 派工单Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid WorkOrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间
        /// </summary>
        [DataMember, ProtoMember(3)]
        public DateTime dp_WorkBeginDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间(小时)
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int sp_BeginHour
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间(分钟)
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int sp_BeginMinute
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间
        /// </summary>
        [DataMember, ProtoMember(6)]
        public DateTime dp_WorkEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间(小时)
        /// </summary>
        [DataMember, ProtoMember(7)]
        public int sp_EndHour
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间(分钟)
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int sp_EndMinute
        {
            get;
            set;
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        [DataMember, ProtoMember(9)]
        public int cb_IsFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 执行情况
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string txt_ExecuteSituation
        {
            get;
            set;
        }

        /// <summary>
        /// 材料消耗
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string txt_MaterialConsumption
        {
            get;
            set;
        }

        /// <summary>
        /// 人员数量
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string txt_PersonnelNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 车辆类型
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string txt_CarType
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(14)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(15)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(16)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(17)]
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间
        /// </summary>
        [DataMember, ProtoMember(18)]
        public string dp_WorkBeginDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string dp_WorkEndDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 公文Id
        /// </summary>
        [DataMember, ProtoMember(20)]
        public Guid txt_WFActivityInstanceId
        {
            get;
            set;
        }
    }
}
