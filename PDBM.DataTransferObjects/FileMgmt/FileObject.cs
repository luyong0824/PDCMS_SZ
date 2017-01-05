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
    /// 文件对象
    /// </summary>
    [DataContract, ProtoContract]
    public class FileObject
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
        /// WebUploader文件Id ""
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string WebUploaderFileId
        {
            get;
            set;
        }

        /// <summary>
        /// 文件状态 2
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int FileStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string FileSize
        {
            get;
            set;
        }

        /// <summary>
        /// 文件上传进度 100
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int UploadProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 文件上传状态 上传完成
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string UploadStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 文件上传日期
        /// </summary>
        [DataMember, ProtoMember(8)]
        public DateTime UploadDate
        {
            get;
            set;
        }
    }
}
