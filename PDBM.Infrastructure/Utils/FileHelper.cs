using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Utils
{
    /// <summary>
    /// 文件工具
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                throw new ArgumentNullException("directoryPath");
            }

            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 检测指定文件是否存在
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <returns></returns>
        public static bool IsExistFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            return File.Exists(filePath);
        }

        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        /// 将文件以只读共享方式读取到文件流中
        /// </summary>
        /// <returns></returns>
        public static FileStream FileToStream(string filePath)
        {
            if (!IsExistFile(filePath))
            {
                throw new ArgumentException("无效的filePath");
            }

            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// 将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <returns></returns>
        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.Default);
        }

        /// <summary>
        /// 将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string FileToString(string filePath, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("filePath");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            using (StreamReader reader = new StreamReader(filePath, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="source">文件源</param>
        /// <param name="filePath">文件存储路径</param>
        /// <param name="bufferSize">缓存大小</param>
        public static void UploadFile(Stream source, string filePath, int bufferSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (!source.CanRead)
            {
                throw new ArgumentException("无效的source");
            }
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (source)
            {
                byte[] buffer = new byte[bufferSize];
                int perCopied = 0;
                while ((perCopied = source.Read(buffer, 0, bufferSize)) > 0)
                {
                    fs.Write(buffer, 0, perCopied);
                    fs.Flush();
                }
            }
        }
    }
}
