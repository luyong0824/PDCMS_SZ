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
    /// 工期延误申请实体
    /// </summary>
    [DataContract, ProtoContract]
    public class DelayApplyMaintObject
    {
        /// <summary>
        /// 工期延误申请Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 建设任务Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid ConstructionTaskId
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string OrderCode
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
        /// 建设进度
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int ConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 延期天数
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int DelayDays
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 申请单审批状态
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int OrderState
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(11)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(12)]
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户名称
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string CreateFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 附件Id列表
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string FileIdList
        {
            get;
            set;
        }
    }
}
