using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 项目任务实体
    /// </summary>
    public class ProjectTask : AggregateRoot
    {
        protected ProjectTask()
        {
        }

        /// <summary>
        /// 构造项目任务实体
        /// </summary>
        /// <param name="projectType">项目类型</param>
        /// <param name="parentId">父表Id</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal ProjectTask(ProjectType projectType, Guid parentId, Guid placeId, string projectCode, Guid createUserId)
        {
            parentId.IsEmpty("父表Id");

            this.Id = Guid.NewGuid();
            this.ProjectType = projectType;
            this.ParentId = parentId;
            this.PlaceId = placeId;
            this.AreaManagerId = Guid.Empty;
            this.GeneralDesignId = Guid.Empty;
            this.DesignRealName = "";
            this.DesignDate = DateTime.Parse("2000-01-01");
            this.ProjectCode = projectCode;
            this.ProjectProgress = ProjectProgress.未开工;
            this.ProgressMemos = "";
            this.ProjectDate = DateTime.Parse("2000-01-01");
            this.ProjectBeginDate = DateTime.Parse("2000-01-01");
            this.State = State.使用;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 工程类型
        /// </summary>
        public ProjectType ProjectType
        {
            get;
            set;
        }

        /// <summary>
        /// 父表Id
        /// </summary>
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目经理Id
        /// </summary>
        public Guid AreaManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 总设单位Id
        /// </summary>
        public Guid GeneralDesignId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计人
        /// </summary>
        public string DesignRealName
        {
            get;
            set;
        }

        /// <summary>
        /// 总设日期
        /// </summary>
        public DateTime DesignDate
        {
            get;
            set;
        }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 项目进度
        /// </summary>
        public ProjectProgress ProjectProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 进度简述
        /// </summary>
        public string ProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 项目开通日期
        /// </summary>
        public DateTime ProjectDate
        {
            get;
            set;
        }

        /// <summary>
        /// 项目启动日期
        /// </summary>
        public DateTime ProjectBeginDate
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public State State
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
        /// 指定项目经理及总设单位
        /// </summary>
        /// <param name="areaManagerId">项目经理Id</param>
        /// <param name="generalDesignId">总设单位Id</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void AppointAreaAndDesignUser(Guid areaManagerId, Guid generalDesignId, Guid modifyUserId)
        {
            this.AreaManagerId = areaManagerId;
            this.GeneralDesignId = generalDesignId;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 更新站点Id
        /// </summary>
        /// <param name="placeId">站点Id</param>
        public void ModifyPlaceId(Guid placeId)
        {
            this.PlaceId = placeId;
        }
    }
}
