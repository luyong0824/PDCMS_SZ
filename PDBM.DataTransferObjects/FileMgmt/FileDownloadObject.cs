using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.FileMgmt
{
    /// <summary>
    /// 文件下载对象
    /// </summary>
    [DataContract, ProtoContract]
    public class FileDownloadObject
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        [DataMember, ProtoMember(3)]
        public long FileSize
        {
            get;
            set;
        }

        /// <summary>
        /// 文件存储路径
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string FilePath
        {
            get;
            set;
        }
    }
}
