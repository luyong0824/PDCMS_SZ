using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;
using PDBM.Domain.Events;
using PDBM.Domain.Events.WorkFlow;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 改造实体
    /// </summary>
    public class Remodeling : AggregateRoot
    {
        protected Remodeling()
        {
        }

        /// <summary>
        /// 构造改造实体
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="proposedNetwork">拟建网络</param>
        /// <param name="remarks">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Remodeling(Profession profession, string placeCode, Guid placeId, string proposedNetwork, string remarks, Guid createUserId)
        {
            profession.IsInvalid("专业");
            placeCode.IsNullOrEmptyOrTooLong("站点编码", true, 50);
            placeId.IsEmpty("站点Id");
            proposedNetwork.IsNullOrTooLong("拟建网络", true, 250);
            remarks.IsNullOrTooLong("备注", true, 250);

            this.Id = Guid.NewGuid();
            this.OrderCode = "";
            this.Profession = profession;
            this.PlaceCode = placeCode;
            this.PlaceId = placeId; this.ProposedNetwork = proposedNetwork;
            this.OrderState = WFProcessInstanceState.未发送;
            this.Remarks = remarks;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 改造申请单编码
        /// </summary>
        public string OrderCode
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
        /// 站点编码
        /// </summary>
        public string PlaceCode
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
        /// 拟建网络
        /// </summary>
        public string ProposedNetwork
        {
            get;
            set;
        }

        /// <summary>
        /// 改造申请单状态
        /// </summary>
        public WFProcessInstanceState OrderState
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
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
        /// 站点实体
        /// </summary>
        protected virtual Place Place
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改改造实体
        /// </summary>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="projectId">项目Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string placeCode, Guid placeId, string proposedNetwork, string remarks, Guid modifyUserId)
        {
            placeCode.IsNullOrEmptyOrTooLong("站点编码", true, 50);
            placeId.IsEmpty("站点Id");
            proposedNetwork.IsNullOrTooLong("拟建网络", true, 250);
            remarks.IsNullOrTooLong("备注", true, 250);

            this.PlaceCode = placeCode;
            this.PlaceId = placeId; this.ProposedNetwork = proposedNetwork;
            this.Remarks = remarks;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }


        /// <summary>
        /// 修改改造检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByUpdate(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能修改别人创建的改造安排");
            }

            //DomainEvent.Publish<WFProcessInstanceOrderModifyingEvent>(new WFProcessInstanceOrderModifyingEvent(this));
        }

        /// <summary>
        /// 删除改造检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByRemove(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("{0}<br>不能删除别人创建的改造安排", this.PlaceCode);
            }
        }
    }
}
