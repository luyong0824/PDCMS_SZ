using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.FileMgmt
{
    /// <summary>
    /// 文件实体
    /// </summary>
    public class File : AggregateRoot
    {
        protected File()
        {
        }

        /// <summary>
        /// 构造文件实体
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileType">文件MIME类型</param>
        /// <param name="fileExtension">文件扩展名</param>
        /// <param name="fileSize">文件字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="uploadUserId">上传用户Id</param>
        internal File(Guid id, string fileName, string fileType, string fileExtension, long fileSize, string filePath, Guid uploadUserId)
        {
            id.IsEmpty("文件Id");
            fileName.IsNullOrEmptyOrTooLong("文件名称", true, 255);
            fileType.IsNullOrTooLong("文件类型", false, 255);
            fileExtension.IsNullOrTooLong("文件扩展名", true, 255);
            filePath.IsNullOrEmptyOrTooLong("文件路径", true, 400);

            this.Id = id;
            this.FileName = fileName;
            this.FileType = fileType;
            this.FileExtension = fileExtension;
            this.FileSize = fileSize;
            this.FilePath = filePath;
            this.UploadUserId = uploadUserId;
            this.UploadDate = DateTime.Now;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件MIME类型
        /// </summary>
        public string FileType
        {
            get;
            set;
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtension
        {
            get;
            set;
        }

        /// <summary>
        /// 文件字节大小
        /// </summary>
        public long FileSize
        {
            get;
            set;
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }

        /// <summary>
        /// 上传用户Id
        /// </summary>
        public Guid UploadUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime UploadDate
        {
            get;
            set;
        }
    }
}
