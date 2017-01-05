using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 寻址确认实体
    /// </summary>
    public class Addressing : AggregateRoot
    {
        protected Addressing()
        {
        }

        /// <summary>
        /// 构造寻址确认实体
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="addressingDepartmentId">租赁部门Id</param>
        /// <param name="addressingRealName">租赁人</param>
        /// <param name="ownerName">业主</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系电话</param>
        /// <param name="remarks">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Addressing(Guid planningId, string placeName, Guid addressingDepartmentId, string addressingRealName, string ownerName,
            string ownerContact, string ownerPhoneNumber, string remarks, Guid createUserId)
        {
            planningId.IsEmpty("规划Id");
            placeName.IsNullOrEmptyOrTooLong("站点名称", true, 100);
            addressingDepartmentId.IsEmpty("租赁部门Id");
            addressingRealName.IsNullOrTooLong("租赁人", true, 50);
            ownerName.IsNullOrTooLong("业主名称", true, 100);
            ownerContact.IsNullOrTooLong("业主联系人", true, 100);
            ownerPhoneNumber.IsNullOrTooLong("业主联系电话", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);

            this.Id = Guid.NewGuid();
            this.OrderCode = "";
            this.AddressingDate = DateTime.Now;
            this.PlanningId = planningId;
            this.PlaceName = placeName;
            this.AddressingDepartmentId = addressingDepartmentId;
            this.AddressingRealName = addressingRealName;
            this.OwnerName = ownerName;
            this.OwnerContact = ownerContact;
            this.OwnerPhoneNumber = ownerPhoneNumber;
            this.OrderState = WFProcessInstanceState.未发送;
            this.Remarks = remarks;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 寻址确认单编码
        /// </summary>
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 租赁日期
        /// </summary>
        public DateTime AddressingDate
        {
            get;
            set;
        }

        /// <summary>
        /// 规划Id
        /// </summary>
        public Guid PlanningId
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
        /// 租赁部门
        /// </summary>
        public Guid AddressingDepartmentId
        {
            get;
            set;
        }

        /// <summary>
        /// 实际租赁人
        /// </summary>
        public string AddressingRealName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主名称
        /// </summary>
        public string OwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系人
        /// </summary>
        public string OwnerContact
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系电话
        /// </summary>
        public string OwnerPhoneNumber
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
        public Guid DesignCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址确认单状态
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
        public Guid? ModifyUserId
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
        /// 规划实体
        /// </summary>
        protected virtual Planning Planning
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改寻址确认实体
        /// </summary>
        /// <param name="placeName">基站名称</param>
        /// <param name="addressingDepartmentId">租赁部门Id</param>
        /// <param name="addressingRealName">租赁人</param>
        /// <param name="ownerName">业主</param>
        /// <param name="ownerContact">业主联系人</param>
        /// <param name="ownerPhoneNumber">业主联系电话</param>
        /// <param name="remarks">备注</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string placeName, Guid addressingDepartmentId, string addressingRealName, string ownerName,
             string ownerContact, string ownerPhoneNumber, string remarks, Guid modifyUserId)
        {
            placeName.IsNullOrEmptyOrTooLong("站点名称", true, 100);
            addressingDepartmentId.IsEmpty("租赁部门Id");
            addressingRealName.IsNullOrEmptyOrTooLong("租赁人", true, 50);
            ownerName.IsNullOrTooLong("业主名称", true, 100);
            ownerContact.IsNullOrTooLong("业主联系人", true, 100);
            ownerPhoneNumber.IsNullOrTooLong("业主联系电话", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);

            this.PlaceName = placeName;
            this.AddressingDepartmentId = addressingDepartmentId;
            this.AddressingRealName = addressingRealName;
            this.OwnerName = ownerName;
            this.OwnerContact = ownerContact;
            this.OwnerPhoneNumber = ownerPhoneNumber;
            this.Remarks = remarks;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改寻址确认实体
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="modifyUserId">修改人</param>
        public void UpdateAddressingEdit(Guid projectId, Guid projectManagerId, Guid modifyUserId)
        {
            projectId.IsEmpty("项目Id");
            projectManagerId.IsEmpty("工程经理Id");

            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
