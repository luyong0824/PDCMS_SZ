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
    /// 工作流活动编辑器选择对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WFActivityEditorSelectObject
    {
        public WFActivityEditorSelectObject()
        {
            PId = Guid.Empty;
            isLeaf = true;
        }

        /// <summary>
        /// 工作流活动编辑器Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动编辑器名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string WFActivityEditorName
        {
            get;
            set;
        }

        /// <summary>
        /// 父级Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid PId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否叶子
        /// </summary>
        [DataMember, ProtoMember(4)]
        public bool isLeaf
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编辑Url
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string EditorUrl
        {
            get;
            set;
        }
    }
}
