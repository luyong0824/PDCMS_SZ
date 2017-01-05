using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.BMMgmt
{
    /// <summary>
    /// 建设申请打印对象
    /// </summary>
    [DataContract, ProtoContract]
    public class PlanningApplyHeaderPrintObject
    {
        /// <summary>
        /// 建设申请Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string OrderStateText
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期文本
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string CreateDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 公文审批Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 申请人
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string CreateFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 申请部门
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string DepartmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例信息HTML字符串
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string WFActivityInstancesInfoHtml
        {
            get;
            set;
        }

        /// <summary>
        /// 基站建设申请明细HTML字符串
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string PlanningApplyDetailHtml
        {
            get;
            set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string Title
        {
            get;
            set;
        }
    }
}
