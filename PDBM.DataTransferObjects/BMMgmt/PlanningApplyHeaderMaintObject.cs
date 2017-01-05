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
    /// 建设申请单维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class PlanningApplyHeaderMaintObject
    {
        /// <summary>
        /// 建设申请单Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 公文审批Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int OrderState
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期文本
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string CreateDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 建设申请单Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid HeaderEditId
        {
            get;
            set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string TitleEdit
        {
            get;
            set;
        }
    }
}
