using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.WorkFlow
{
    /// <summary>
    /// 工作流过程维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WFProcessMaintObject
    {
        /// <summary>
        /// 工作流过程Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流类型Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid WFCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程编码
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string WFProcessCode
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程名称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string WFProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否部门经理审批
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int IsApprovedByManager
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(7)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid ModifyUserId
        {
            get;
            set;
        }
    }
}
