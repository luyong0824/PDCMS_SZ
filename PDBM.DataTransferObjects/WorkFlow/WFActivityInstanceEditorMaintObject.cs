using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using System.Runtime.Serialization;

namespace PDBM.DataTransferObjects.WorkFlow
{
    /// <summary>
    /// 流程编辑维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WFActivityInstanceEditorMaintObject
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 流程步骤Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(3)]
        public DateTime CreateDate
        {
            get;
            set;
        }
    }
}
