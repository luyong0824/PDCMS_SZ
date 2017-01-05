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
    /// 建设申请实体
    /// </summary>
    public class PlanningApply : AggregateRoot
    {
        protected PlanningApply()
        {
        }

        /// <summary>
        /// 构造建设申请实体
        /// </summary>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">建设理由</param>
        /// <param name="wfProcessInstanceState">审批状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal PlanningApply(string planningCode, string planningName, Profession profession, Guid reseauId, decimal lng, decimal lat, Importance importance, string detailedAddress, string remarks, Guid createUserId)
        {
            planningCode.IsNullOrEmptyOrTooLong("规划编码", true, 50);
            planningName.IsNullOrEmptyOrTooLong("规划名称", true, 100);
            profession.IsInvalid("专业");
            reseauId.IsEmpty("网格Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            importance.IsInvalid("重要性程度");
            detailedAddress.IsNullOrTooLong("详细地址", true, 250);
            remarks.IsNullOrTooLong("建设理由", true, 250);

            this.Id = Guid.NewGuid();
            this.PlanningCode = planningCode;
            this.PlanningName = planningName;
            this.Profession = profession;
            this.ReseauId = reseauId;
            this.Lng = lng;
            this.Lat = lat;
            this.Importance = importance;
            this.DetailedAddress = detailedAddress;
            this.Remarks = remarks;
            this.Issued = Bool.否;
            this.PlanningUserId = Guid.Empty;
            this.PlanningAdvice = PlanningAdvice.暂不考虑;
            this.DoState = DoState.未处理;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
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
        /// 重要性程度
        /// </summary>
        public Importance Importance
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
        /// 是否下达
        /// </summary>
        public Bool Issued
        {
            get;
            set;
        }

        /// <summary>
        /// 网优人员
        /// </summary>
        public Guid PlanningUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 规划意见
        /// </summary>
        public PlanningAdvice PlanningAdvice
        {
            get;
            set;
        }

        /// <summary>
        /// 处理状态
        /// </summary>
        public DoState DoState
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
        /// 修改建设申请实体
        /// </summary>
        /// <param name="planningName">规划名称</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="detailedAddress">详细地址</param>
        /// <param name="remarks">建设理由</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string planningName, Guid reseauId, decimal lng, decimal lat, Importance importance, string detailedAddress, string remarks, Guid modifyUserId)
        {
            planningName.IsNullOrEmptyOrTooLong("规划名称", true, 100);
            reseauId.IsEmpty("网格Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            importance.IsInvalid("重要性程度");
            detailedAddress.IsNullOrTooLong("详细地址", true, 250);
            remarks.IsNullOrTooLong("建设理由", true, 250);

            this.PlanningName = planningName;
            this.ReseauId = reseauId;
            this.Lng = lng;
            this.Lat = lat;
            this.Importance = importance;
            this.DetailedAddress = detailedAddress;
            this.Remarks = remarks;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 保存业务审核
        /// </summary>
        /// <param name="importance"></param>
        public void SaveBusinessAudit(Importance importance)
        {
            this.Importance = importance;
        }

        /// <summary>
        /// 保存技术审核
        /// </summary>
        /// <param name="planningAdvice">规划意见</param>
        /// <param name="planningUserId">规划用户Id</param>
        public void SaveTechnicalAudit(PlanningAdvice planningAdvice, Guid planningUserId)
        {
            this.PlanningAdvice = planningAdvice;
            this.PlanningUserId = planningUserId;
        }

        /// <summary>
        /// 修改建设申请检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByUpdate(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能修改别人创建的建设申请");
            }
            if (this.Issued == Bool.是)
            {
                throw new DomainFault("不能修改已下达的建设申请");
            }
        }

        /// <summary>
        /// 删除建设申请检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByRemove(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("{0}<br>不能删除别人创建的建设申请", this.PlanningCode);
            }
            if (this.Issued == Bool.是)
            {
                throw new DomainFault("{0}<br>不能删除已下达的建设申请", this.PlanningCode);
            }
        }

        /// <summary>
        /// 取消关联建设申请检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByRemoveDetail(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("{0}<br>不能取消关联别人创建的建设申请", this.PlanningCode);
            }
        }

        /// <summary>
        /// 提交检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByIssued(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能提交别人创建的建设申请");
            }
            if (this.Issued == Bool.是)
            {
                throw new DomainFault("不能提交已提交的建设申请");
            }
            if (this.PlanningUserId == Guid.Empty)
            {
                throw new DomainFault("还未指定网优人员，无法提交");
            }
        }

        /// <summary>
        /// 取消提交检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByCancelIssued(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能取消提交别人创建的建设申请");
            }
            if (this.DoState != DoState.未处理)
            {
                throw new DomainFault("只能取消提交未处理的建设申请");
            }
        }

        /// <summary>
        /// 指定规划意见
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByPlanningAdvice(Guid currentUserId)
        {
            if (this.PlanningUserId != currentUserId)
            {
                throw new DomainFault("不能修改他人的待处理建设申请");
            }
            if (this.PlanningAdvice == PlanningAdvice.列入规划)
            {
                throw new DomainFault("不能修改已列入规划的待处理建设申请");
            }
        }
    }
}
