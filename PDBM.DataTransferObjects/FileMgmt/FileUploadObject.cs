using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DataTransferObjects.FileMgmt
{
    /// <summary>
    /// 文件上传对象
    /// </summary>
    [MessageContract]
    public class FileUploadObject
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        [MessageHeader]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        [MessageHeader]
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件MIME Type
        /// </summary>
        [MessageHeader]
        public string FileType
        {
            get;
            set;
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        [MessageHeader]
        public string FileExtension
        {
            get;
            set;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        [MessageHeader]
        public long FileSize
        {
            get;
            set;
        }

        /// <summary>
        /// 文件数据流
        /// </summary>
        [MessageBodyMember]
        public Stream FileData
        {
            get;
            set;
        }

        /// <summary>
        /// 上传人用户Id
        /// </summary>
        [MessageHeader]
        public Guid UploadUserId
        {
            get;
            set;
        }
    }
}
