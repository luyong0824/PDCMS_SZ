using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.FileMgmt
{
    /// <summary>
    /// 文件关联实体
    /// </summary>
    public class FileAssociation : AggregateRoot
    {
        protected FileAssociation()
        {
        }

        /// <summary>
        /// 构造文件关联实体
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <param name="entityId">实体Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal FileAssociation(string entityName, Guid entityId, string fileIdList, Guid createUserId)
        {
            entityName.IsNullOrEmptyOrTooLong("实体名称", true, 50);
            entityId.IsEmpty("实体Id");
            fileIdList.IsNull("文件Id列表");

            this.Id = Guid.NewGuid();
            this.EntityName = entityName;
            this.EntityId = entityId;
            this.FileIdList = fileIdList;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName
        {
            get;
            set;
        }

        /// <summary>
        /// 实体Id
        /// </summary>
        public Guid EntityId
        {
            get;
            set;
        }

        /// <summary>
        /// 文件Id
        /// </summary>
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改文件关联实体
        /// </summary>
        /// <param name="fileIdList">文件实体列表</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string fileIdList, Guid modifyUserId)
        {
            fileIdList.IsNull("文件Id列表");

            this.FileIdList = fileIdList;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
