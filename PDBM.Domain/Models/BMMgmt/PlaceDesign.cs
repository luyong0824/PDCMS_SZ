using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 站点设计信息实体
    /// </summary>
    public class PlaceDesign : AggregateRoot
    {
        protected PlaceDesign()
        { 
        }

        /// <summary>
        /// 构造站点设计信息实体
        /// </summary>
        /// <param name="parentId">父表Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal PlaceDesign(Guid parentId, PropertyType propertyType, Guid createUserId)
        {
            parentId.IsEmpty("父表Id");
            propertyType.IsInvalid("资源类型");

            this.Id = Guid.NewGuid();
            this.ParentId = parentId;
            this.PropertyType = propertyType;
            this.DesignCustomerId = Guid.Empty;
            this.DesignUserId = Guid.Empty;
            this.SupervisorCustomerId = Guid.Empty;
            this.SupervisorUserId = Guid.Empty;
            this.ProjectCode = "";
            this.ProjectName = "";
            this.ProjectMoney = 0;
            this.ProjectIsApply = Bool.否;
            this.ProjectApplyDate = DateTime.Now;
            this.ProjectIsDoApply = Bool.否;
            this.ProjectDoApplyDate = ProjectApplyDate;
            this.GroupPlaceCode = "";
            this.TowerMark = Bool.否;
            this.TowerBaseMark = Bool.否;
            this.MachineRoomMark = Bool.否;
            this.ExternalElectricPowerMark = Bool.否;
            this.EquipmentInstallMark = Bool.否;
            this.AddressExplorMark = Bool.否;
            this.FoundationTestMark = Bool.否;
            this.State = State.使用;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
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
        /// 资源类型
        /// </summary>
        public PropertyType PropertyType
        {
            get;
            set;
        }

        /// <summary>
        /// 设计单位Id
        /// </summary>
        public Guid? DesignCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计人员Id
        /// </summary>
        public Guid? DesignUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位Id
        /// </summary>
        public Guid? SupervisorCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理人员Id
        /// </summary>
        public Guid? SupervisorUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 集团项目编码
        /// </summary>
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 集团项目名称
        /// </summary>
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目预算
        /// </summary>
        public decimal ProjectMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 是否申请立项
        /// </summary>
        public Bool ProjectIsApply
        {
            get;
            set;
        }

        /// <summary>
        /// 申请立项时间
        /// </summary>
        public DateTime ProjectApplyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 是否完成立项
        /// </summary>
        public Bool ProjectIsDoApply
        {
            get;
            set;
        }

        /// <summary>
        /// 完成立项时间
        /// </summary>
        public DateTime ProjectDoApplyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 集团地址编码
        /// </summary>
        public string GroupPlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有铁塔资源
        /// </summary>
        public Bool TowerMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有铁塔基础资源
        /// </summary>
        public Bool TowerBaseMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有机房资源
        /// </summary>
        public Bool MachineRoomMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有外电引入资源
        /// </summary>
        public Bool ExternalElectricPowerMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有设备安装资源
        /// </summary>
        public Bool EquipmentInstallMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有地质勘探资源
        /// </summary>
        public Bool AddressExplorMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有桩基动测资源
        /// </summary>
        public Bool FoundationTestMark
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
        /// 创建时间
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
        /// 修改站点设计信息实体
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(State state, Guid modifyUserId)
        {
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改站点资源属性
        /// </summary>
        /// <param name="taskModel">资源名称</param>
        /// <param name="isNew">是否拥有该属性</param>
        public void ModifyPropertyMark(TaskModel taskModel, Bool isNew)
        {
            //if (taskModel == TaskModel.铁塔)
            //{
            //    this.TowerMark = isNew;
            //}
            //else if (taskModel == TaskModel.铁塔基础)
            //{
            //    this.TowerBaseMark = isNew;
            //}
            //else if (taskModel == TaskModel.机房)
            //{
            //    this.MachineRoomMark = isNew;
            //}
            //else if (taskModel == TaskModel.外电引入)
            //{
            //    this.ExternalElectricPowerMark = isNew;
            //}
            //else if (taskModel == TaskModel.设备安装)
            //{
            //    this.EquipmentInstallMark = isNew;
            //}
            //else if (taskModel == TaskModel.地质勘探)
            //{
            //    this.AddressExplorMark = isNew;
            //}
            //else if (taskModel == TaskModel.桩基动测)
            //{
            //    this.FoundationTestMark = isNew;
            //}
        }

        /// <summary>
        /// 指定设计单位
        /// </summary>
        /// <param name="designCustomerId"></param>
        public void AppointDesign(Guid? designCustomerId)
        {
            this.DesignCustomerId = designCustomerId;
        }

        /// <summary>
        /// 指定设计人员
        /// </summary>
        /// <param name="designUserId"></param>
        public void AppointDesignUser(Guid? designUserId)
        {
            this.DesignUserId = designUserId;
        }

        /// <summary>
        /// 指定项目及集团站点编码
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="budgetPrice">项目预算</param>
        /// <param name="groupPlaceCode">集团站点编码</param>
        public void AppointProjectAndPlaceCode(string projectCode, string projectName, decimal budgetPrice, string groupPlaceCode)
        {
            this.ProjectCode = projectCode;
            this.ProjectName = projectName;
            this.ProjectMoney = budgetPrice;
            this.GroupPlaceCode = groupPlaceCode;
        }

        /// <summary>
        /// 指定监理单位
        /// </summary>
        /// <param name="supervisorCustomerId"></param>
        /// <param name="supervisorUserId"></param>
        public void AppointSupervisor(Guid? supervisorCustomerId, Guid? supervisorUserId)
        {
            this.SupervisorCustomerId = supervisorCustomerId;
            this.SupervisorUserId = supervisorUserId;
        }

        /// <summary>
        /// 指定项目
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="budgetPrice">项目预算</param>
        public void AppointProject(string projectCode, string projectName, decimal budgetPrice)
        {
            this.ProjectCode = projectCode;
            this.ProjectName = projectName;
            this.ProjectMoney = budgetPrice;
        }

        /// <summary>
        /// 项目申请立项
        /// </summary>
        public void ApplyProject()
        {
            this.ProjectIsApply = Bool.是;
            this.ProjectApplyDate = DateTime.Now;
        }

        /// <summary>
        /// 项目完成立项
        /// </summary>
        public void DoApplyProject()
        {
            this.ProjectIsDoApply = Bool.是;
            this.ProjectDoApplyDate = DateTime.Now;
        }

        /// <summary>
        /// 流程审批结束后更新项目预算
        /// </summary>
        /// <param name="projectMoney">项目预算总价</param>
        public void AddProjectMoney(decimal projectMoney)
        {
            this.ProjectMoney = projectMoney;
        }
    }
}
