using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 立项编号信息表实体
    /// </summary>
    public class ProjectCodeList : AggregateRoot
    {
        protected ProjectCodeList()
        { 
        }

        /// <summary>
        /// 构造立项编号信息表实体
        /// </summary>
        /// <param name="projectCode">立项编号</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="projectDate">立项时间</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal ProjectCodeList(string projectCode,ProjectType projectType,DateTime projectDate,string placeName,Guid reseauId,Guid projectManagerId,State state,Guid createUserId)
        {
            projectCode.IsNullOrTooLong("立项编号", true, 50);
            projectType.IsInvalid("建设方式");
            placeName.IsNullOrTooLong("站点名称", true, 50);
            reseauId.IsEmpty("网格Id");
            projectManagerId.IsEmpty("工程经理Id");
            state.IsInvalid("使用状态");

            this.Id = Guid.NewGuid();
            this.ProjectCode = projectCode;
            this.ProjectType = projectType;
            this.ProjectDate = projectDate;
            this.PlaceName = placeName;
            this.ReseauId = reseauId;
            this.ProjectManagerId = projectManagerId;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 立项编号
        /// </summary>
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 建设方式
        /// </summary>
        public ProjectType ProjectType
        {
            get;
            set;
        }

        /// <summary>
        /// 立项时间
        /// </summary>
        public DateTime ProjectDate
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理Id
        /// </summary>
        public Guid  ProjectManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 使用状态
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
        /// 修改立项编号信息表实体
        /// </summary>
        /// <param name="projectCode">立项编号</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="projectDate">立项时间</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string projectCode, ProjectType projectType, DateTime projectDate, string placeName, Guid reseauId, Guid projectManagerId,State state, Guid modifyUserId)
        {
            projectCode.IsNullOrTooLong("立项编号", true, 50);
            projectType.IsInvalid("建设方式");
            placeName.IsNullOrTooLong("站点名称", true, 50);
            reseauId.IsEmpty("网格Id");
            projectManagerId.IsEmpty("工程经理Id");
            state.IsInvalid("使用状态");

            this.ProjectCode = projectCode;
            this.ProjectType = projectType;
            this.ProjectDate = projectDate;
            this.PlaceName = placeName;
            this.ReseauId = reseauId;
            this.ProjectManagerId = projectManagerId;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
