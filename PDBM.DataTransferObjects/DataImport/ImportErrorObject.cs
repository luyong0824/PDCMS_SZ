using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.DataImport
{
    /// <summary>
    /// 导入错误对象
    /// </summary>
    [DataContract, ProtoContract]
    public class ImportErrorObject
    {
        /// <summary>
        /// 对象名称
        /// </summary>
        [DataMember, ProtoMember(1)]
        public string ObjectName
        {
            get;
            set;
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string ErrorMessage
        {
            get;
            set;
        }
    }
}
