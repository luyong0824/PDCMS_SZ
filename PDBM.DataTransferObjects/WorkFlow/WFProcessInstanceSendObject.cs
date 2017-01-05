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
    /// 工作流过程实例发送对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WFProcessInstanceSendObject
    {
        /// <summary>
        /// 工作流过程实例Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid WFProcessId
        {
            get;
            set;
        }

        /// <summary>
        /// 实体Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid EntityId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程实例编码
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string WFProcessInstanceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程实例名称
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string WFProcessInstanceName
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程实例状态
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int WFProcessInstanceState
        {
            get;
            set;
        }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 文件Id列表
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string FileIdList
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
        /// 是否必须编辑
        /// </summary>
        [DataMember, ProtoMember(10)]
        public int IsMustEdit
        {
            get;
            set;
        }
    }
}
