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
    /// 工作流活动实例维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WFActivityInstanceSelectObject
    {
        /// <summary>
        /// 工作流类型Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid WFCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程实例Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid WFProcessInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string WFProcessName
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
        /// 工作流过程实例内容
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string WFProcessInstanceContent
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程实例发送人姓名
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程实例发送时间
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动操作类型
        /// </summary>
        [DataMember, ProtoMember(9)]
        public int WFActivityOperate
        {
            get;
            set;
        }

        /// <summary>
        /// 打印地址
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string PrintUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 编辑地址
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string EditorUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 编辑单据Id
        /// </summary>
        [DataMember, ProtoMember(12)]
        public Guid EntityId
        {
            get;
            set;
        }

        /// <summary>
        /// 公文单头Id
        /// </summary>
        [DataMember, ProtoMember(13)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 当前审批状态
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int WFActivityInstanceState
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编辑类型Id
        /// </summary>
        [DataMember, ProtoMember(15)]
        public Guid? WFActivityEditorId
        {
            get;
            set;
        }
    }
}
