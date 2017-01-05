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
    /// 工作流活动实例处理对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WFActivityInstanceDoObject
    {
        /// <summary>
        /// 工作流活动实例Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例流转选项
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int WFActivityInstanceFlow
        {
            get;
            set;
        }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 文件列表
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string FileIdList
        {
            get;
            set;
        }
    }
}
