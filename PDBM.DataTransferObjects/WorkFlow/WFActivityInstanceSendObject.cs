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
    /// 工作流活动实例发送对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WFActivityInstanceSendObject
    {
        /// <summary>
        /// 工作流活动实例名称
        /// </summary>
        [DataMember, ProtoMember(1)]
        public string WFActivityInstanceName
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动操作类型
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int WFActivityOperate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动编辑器Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid WFActivityEditorId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动顺序类型
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int WFActivityOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 行号
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int RowId
        {
            get;
            set;
        }

        /// <summary>
        /// 时限
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int Timelimit
        {
            get;
            set;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否必须编辑
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int IsMustEdit
        {
            get;
            set;
        }
    }
}
