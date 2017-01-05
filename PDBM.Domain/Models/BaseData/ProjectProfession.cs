using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 项目专业实体
    /// </summary>
    public class ProjectProfession : AggregateRoot
    {
        protected ProjectProfession()
        {
        }

        /// <summary>
        /// 构造项目专业实体
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="profession">专业</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal ProjectProfession(Guid projectId, Profession profession, Guid createUserId)
        {
            projectId.IsEmpty("项目Id");
            profession.IsInvalid("专业");

            this.Id = Guid.NewGuid();
            this.ProjectId = projectId;
            this.Profession = profession;
            this.CreateUserId = createUserId;
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        public Profession Profession
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
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        #region Relations
        /// <summary>
        /// 项目实体
        /// </summary>
        protected virtual Project Project
        {
            get;
            set;
        }
        #endregion
    }
}
