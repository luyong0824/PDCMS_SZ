using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Events;
using PDBM.Domain.Events.WorkFlow;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 规划实体
    /// </summary>
    public class Planning : AggregateRoot
    {
        protected Planning()
        {
        }

        /// <summary>
        /// 构造规划实体
        /// </summary>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">建设理由</param>
        /// <param name="proposedNetwork">拟建网络</param>
        /// <param name="optionalAddress">可选位置</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Planning(string planningCode, string planningName, Profession profession, Guid placeCategoryId, Guid reseauId, decimal lng, decimal lat,
              string detailedAddress, string remarks, string proposedNetwork, string optionalAddress, Importance importance, Guid placeOwner, Guid createUserId)
        {
            planningCode.IsNullOrEmptyOrTooLong("规划编码", true, 50);
            planningName.IsNullOrEmptyOrTooLong("规划名称", true, 100);
            profession.IsInvalid("专业");
            //placeCategoryId.IsEmpty("站点类型Id");
            reseauId.IsEmpty("网格Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            importance.IsInvalid("重要性程度");
            detailedAddress.IsNullOrTooLong("详细地址", true, 250);
            remarks.IsNullOrTooLong("建设理由", true, 250);
            proposedNetwork.IsNullOrTooLong("拟建网络", true, 250);
            optionalAddress.IsNullOrTooLong("可选地址", true, 250);

            this.Id = Guid.NewGuid();
            this.PlanningCode = planningCode;
            this.PlanningName = planningName;
            this.Profession = profession;
            this.PlaceCategoryId = placeCategoryId;
            this.ReseauId = reseauId;
            this.Lng = lng;
            this.Lat = lat;
            this.DetailedAddress = detailedAddress;
            this.Remarks = remarks;
            this.ProposedNetwork = proposedNetwork;
            this.OptionalAddress = optionalAddress;
            this.Importance = importance;
            this.PlaceOwner = placeOwner;
            this.Issued = Bool.否;
            this.AddressingState = AddressingState.未寻址确认;
            this.PlaceId = Guid.Empty;
            this.AddressingUserId = Guid.Empty;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
            this.AddressingUserDate = this.CreateDate;
            this.AddressingDate = this.CreateDate;
        }

        /// <summary>
        /// 规划编码
        /// </summary>
        public string PlanningCode
        {
            get;
            set;
        }

        /// <summary>
        /// 规划名称
        /// </summary>
        public string PlanningName
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
        /// 站点类型Id
        /// </summary>
        public Guid PlaceCategoryId
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
        /// 经度
        /// </summary>
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 建设理由
        /// </summary>
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 拟建网络
        /// </summary>
        public string ProposedNetwork
        {
            get;
            set;
        }

        /// <summary>
        /// 可选位置
        /// </summary>
        public string OptionalAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 重要性程度
        /// </summary>
        public Importance Importance
        {
            get;
            set;
        }

        /// <summary>
        /// 产权
        /// </summary>
        public Guid PlaceOwner
        {
            get;
            set;
        }

        /// <summary>
        /// 是否下达
        /// </summary>
        public Bool Issued
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址状态
        /// </summary>
        public AddressingState AddressingState
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
        /// 租赁人
        /// </summary>
        public Guid AddressingUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 指定租赁人时间
        /// </summary>
        public DateTime AddressingUserDate
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址确认完成时间
        /// </summary>
        public DateTime AddressingDate
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

        #region Relations
        /// <summary>
        /// 站点类型实体
        /// </summary>
        protected virtual PlaceCategory PlaceCategory
        {
            get;
            set;
        }

        /// <summary>
        /// 网格实体
        /// </summary>
        protected virtual Reseau Reseau
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址确认实体列表
        /// </summary>
        protected virtual ICollection<Addressing> Addressings
        {
            get;
            set;
        }

        /// <summary>
        /// 租赁人用户实体
        /// </summary>
        public virtual User AddressingUser
        {
            get;
            set;
        }

        /// <summary>
        /// 运营商确认明细实体列表
        /// </summary>
        protected virtual ICollection<OperatorsConfirmDetail> OperatorsConfirmDetails
        {
            get;
            set;
        }

        /// <summary>
        /// 运营商规划实体列表
        /// </summary>
        protected virtual ICollection<OperatorsPlanning> OperatorsPlannings
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改规划实体
        /// </summary>
        /// <param name="planningName">规划名称</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">建设理由</param>
        /// <param name="proposedNetwork">拟建网络</param>
        /// <param name="optionalAddress">可选位置</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string planningName, Guid placeCategoryId, Guid reseauId, decimal lng, decimal lat, string detailedAddress,
              string remarks, string proposedNetwork, string optionalAddress, Importance importance, Guid placeOwner, Guid modifyUserId)
        {
            planningName.IsNullOrEmptyOrTooLong("规划名称", true, 100);
            placeCategoryId.IsEmpty("站点类型Id");
            reseauId.IsEmpty("网格Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            importance.IsInvalid("重要性程度");
            detailedAddress.IsNullOrTooLong("详细地址", true, 250);
            remarks.IsNullOrTooLong("建设理由", true, 250);
            proposedNetwork.IsNullOrTooLong("拟建网络", true, 250);
            optionalAddress.IsNullOrTooLong("可选地址", true, 250);

            this.PlanningName = planningName;
            this.PlaceCategoryId = placeCategoryId;
            this.ReseauId = reseauId;
            this.Lng = lng;
            this.Lat = lat;
            this.DetailedAddress = detailedAddress;
            this.Remarks = remarks;
            this.ProposedNetwork = proposedNetwork;
            this.OptionalAddress = optionalAddress;
            this.Importance = importance;
            this.PlaceOwner = placeOwner;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改规划检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByUpdate(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能修改别人创建的规划");
            }
            if (this.AddressingState != AddressingState.未寻址确认)
            {
                throw new DomainFault("只能修改未寻址确认的规划");
            }
            //if (this.Issued == Bool.是)
            //{
            //    throw new DomainFault("不能修改已下达的规划");
            //}
        }

        /// <summary>
        /// 租赁主管修改规划检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByUpdateAddressing()
        {
            if (this.AddressingState != AddressingState.未寻址确认)
            {
                throw new DomainFault("只能修改未寻址确认的规划");
            }
        }

        /// <summary>
        /// 删除规划检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByRemove(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("{0}<br>不能删除别人创建的规划", this.PlanningCode);
            }
            if (this.Issued == Bool.是)
            {
                throw new DomainFault("{0}<br>不能删除已下达的规划", this.PlanningCode);
            }
        }

        /// <summary>
        /// 指定租赁人
        /// </summary>
        /// <param name="userId">租赁人用户Id</param>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void AppointAddressingUser(Guid userId, Guid currentUserId)
        {
            userId.IsEmpty("租赁人用户Id");

            this.AddressingUserId = userId;
            this.AddressingUserDate = DateTime.Now;
            this.ModifyUserId = currentUserId;
            this.ModifyDate = this.AddressingUserDate;
        }

        /// <summary>
        /// 下达规划
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void Issue(Guid currentUserId)
        {
            //if (this.CreateUserId != currentUserId)
            //{
            //    throw new DomainFault("{0}<br>不能操作别人创建的规划", this.PlanningCode);
            //}
            if (this.Issued == Bool.是)
            {
                throw new DomainFault("{0}<br>不能操作已下达的规划", this.PlanningCode);
            }

            this.Issued = Bool.是;
            this.ModifyUserId = currentUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 取消下达规划
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CancelIssue(Guid currentUserId)
        {
            //if (this.CreateUserId != currentUserId)
            //{
            //    throw new DomainFault("{0}<br>不能操作别人创建的规划", this.PlanningCode);
            //}
            if (this.Issued != Bool.是)
            {
                throw new DomainFault("{0}<br>不能操作未下达的规划", this.PlanningCode);
            }
            if (this.AddressingUserId != Guid.Empty)
            {
                throw new DomainFault("{0}<br>只能操作未指定租赁人的规划", this.PlanningCode);
            }

            this.Issued = Bool.否;
            this.ModifyUserId = currentUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 新增寻址确认检查
        /// </summary>
        /// <param name="currentUserId">当前操作人用户Id</param>
        public void CheckByAddAddressing(Guid currentUserId)
        {
            if (this.AddressingUserId != currentUserId)
            {
                throw new DomainFault("{0}<br>不能操作别的租赁人的规划", this.PlanningCode);
            }
            if (this.Issued != Bool.是)
            {
                throw new DomainFault("{0}<br>不能操作未下达的规划", this.PlanningCode);
            }
            if (this.AddressingState != AddressingState.未寻址确认)
            {
                throw new DomainFault("{0}<br>只能操作状态为未寻址确认的规划", this.PlanningCode);
            }
        }

        /// <summary>
        /// 修改寻址确认检查
        /// </summary>
        /// <param name="currentUserId">当前操作人用户Id</param>
        public void CheckByUpdateAddressing(Guid currentUserId)
        {
            if (this.AddressingUserId != currentUserId)
            {
                throw new DomainFault("{0}<br>不能操作别的租赁人的规划", this.PlanningCode);
            }
            if (this.AddressingState == AddressingState.已寻址确认)
            {
                throw new DomainFault("{0}<br>不能操作已寻址确认的规划", this.PlanningCode);
            }
            if (this.AddressingState == AddressingState.流程终止)
            {
                throw new DomainFault("{0}<br>不能操作流程终止的规划", this.PlanningCode);
            }
            //DomainEvent.Publish<WFProcessInstanceOrderModifyingEvent>(new WFProcessInstanceOrderModifyingEvent(this));
        }

        /// <summary>
        /// 退回寻址确认任务
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void ReturnAddressing(Guid currentUserId)
        {
            if (this.AddressingUserId != currentUserId)
            {
                throw new DomainFault("{0}<br>不能操作别的租赁人的规划", this.PlanningCode);
            }
            if (this.Issued != Bool.是)
            {
                throw new DomainFault("{0}<br>不能操作未下达的规划", this.PlanningCode);
            }
            if (this.AddressingState != AddressingState.未寻址确认)
            {
                throw new DomainFault("{0}<br>只能操作状态为未寻址确认的规划", this.PlanningCode);
            }

            this.AddressingUserId = Guid.Empty;
        }

        /// <summary>
        /// 删除寻址确认任务
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckRemoveAddressing(Guid currentUserId)
        {
            if (this.AddressingState != AddressingState.未寻址确认)
            {
                throw new DomainFault("{0}<br>只能操作状态为未寻址确认的规划", this.PlanningCode);
            }
        }

        /// <summary>
        /// 获取寻址确认任务
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void GetAddressing(Guid currentUserId)
        {
            currentUserId.IsEmpty("操作人用户Id");
            if (this.Issued != Bool.是)
            {
                throw new DomainFault("{0}<br>不能操作未下达的规划", this.PlanningCode);
            }
            if (this.AddressingUserId != Guid.Empty)
            {
                throw new DomainFault("{0}<br>不能操作已指定租赁人的规划", this.PlanningCode);
            }
            if (this.AddressingState != AddressingState.未寻址确认)
            {
                throw new DomainFault("{0}<br>只能操作状态为未寻址确认的规划", this.PlanningCode);
            }

            this.AddressingUserId = currentUserId;
            this.AddressingUserDate = DateTime.Now;
        }
    }
}
