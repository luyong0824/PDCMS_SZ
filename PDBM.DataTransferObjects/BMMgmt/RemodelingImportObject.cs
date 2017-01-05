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
    /// 新模式改造基站批量导入对象
    /// </summary>
    [DataContract, ProtoContract]
    public class RemodelingImportObject
    {
        /// <summary>
        /// 建设方式
        /// </summary>
        [DataMember, ProtoMember(1)]
        public int ProjectType
        {
            get;
            set;
        }
    }
}
