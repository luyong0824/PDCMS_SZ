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
    /// 运营商共享实体
    /// </summary>
    public class OperatorsSharing : AggregateRoot
    {
        protected OperatorsSharing()
        {
        }

        /// <summary>
        /// 构造运营商共享实体
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="powerUsed">用电量</param>
        /// <param name="poleNumber">抱杆数量(根)</param>
        /// <param name="cabinetNumber">机柜数量(个)</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="solved">是否采纳</param>
        /// <param name="remarks">备注</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="remodelingId">基站改造Id</param>
        /// <param name="operatorsPlanningDemandId">建议共享需求Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal OperatorsSharing(Profession profession, string placeCode, Guid placeId, decimal powerUsed, int poleNumber, int cabinetNumber,
            Urgency urgency, Bool solved, string remarks, Guid companyId, Guid? remodelingId, Guid? operatorsPlanningDemandId, Guid createUserId)
        {
            profession.IsInvalid("专业");
            placeCode.IsNullOrEmptyOrTooLong("站点编码", true, 50);
            placeId.IsEmpty("站点Id");
            powerUsed.IsNonnegative("用电量");
            poleNumber.IsNonnegative("抱杆数量");
            cabinetNumber.IsNonnegative("机柜数量");
            urgency.IsInvalid("紧要程度");
            remarks.IsNullOrTooLong("备注", true, 150);

            this.Id = Guid.NewGuid();
            this.Profession = profession;
            this.PlaceCode = placeCode;
            this.PlaceId = placeId;
            this.PowerUsed = powerUsed;
            this.PoleNumber = poleNumber;
            this.CabinetNumber = cabinetNumber;
            this.Urgency = urgency;
            this.Solved = solved;
            this.Remarks = remarks;
            this.CompanyId = companyId;
            this.RemodelingId = remodelingId;
            this.OperatorsPlanningDemandId = operatorsPlanningDemandId;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
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
        /// 用电量
        /// </summary>
        public decimal PowerUsed
        {
            get;
            set;
        }

        /// <summary>
        /// 抱杆数量(根)
        /// </summary>
        public int PoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 机柜数量(个)
        /// </summary>
        public int CabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 紧要程度
        /// </summary>
        public Urgency Urgency
        {
            get;
            set;
        }

        /// <summary>
        /// 是否采纳
        /// </summary>
        public Bool Solved
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
        /// 公司Id
        /// </summary>
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 改造Id
        /// </summary>
        public Guid? RemodelingId
        {
            get;
            set;
        }

        /// <summary>
        /// 改造站需求确认Id
        /// </summary>
        public Guid? OperatorsPlanningDemandId
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

        /// <summary>
        /// 改造实体
        /// </summary>
        protected virtual Remodeling Remodeling
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改运营商共享实体
        /// </summary>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="powerUsed">用电量</param>
        /// <param name="poleNumber">抱杆数量(根)</param>
        /// <param name="cabinetNumber">机柜数量(个)</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="remarks">备注</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string placeCode, Guid placeId, decimal powerUsed, int poleNumber, int cabinetNumber,
            Urgency urgency, string remarks, Guid modifyUserId)
        {
            placeCode.IsNullOrEmptyOrTooLong("站点编码", true, 50);
            placeId.IsEmpty("站点Id");
            powerUsed.IsNonnegative("天线挂高");
            poleNumber.IsNonnegative("抱杆数量");
            cabinetNumber.IsNonnegative("机柜数量");
            urgency.IsInvalid("紧要程度");
            remarks.IsNullOrTooLong("备注", true, 150);

            this.PlaceCode = placeCode;
            this.PlaceId = placeId;
            this.PowerUsed = powerUsed;
            this.PoleNumber = poleNumber;
            this.CabinetNumber = cabinetNumber;
            this.Urgency = urgency;
            this.Remarks = remarks;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改运营商共享检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByUpdate(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能修改别人创建的共享申请");
            }
            if (this.Solved == Bool.是)
            {
                throw new DomainFault("不能修改已解决的共享申请");
            }
        }

        /// <summary>
        /// 删除运营商共享检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByRemove(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("{0}<br>不能删除别人创建的共享申请", this.PlaceCode);
            }
            if (this.Solved == Bool.是)
            {
                throw new DomainFault("{0}<br>不能删除已解决的共享申请", this.PlaceCode);
            }
        }

        /// <summary>
        /// 关联共享
        /// </summary>
        /// <param name="remodelingId">改造Id</param>
        public void Associate(Guid remodelingId)
        {
            remodelingId.IsEmpty("改造Id");

            if (this.RemodelingId == null)
            {
                this.RemodelingId = remodelingId;
                this.Solved = Bool.是;
            }
        }

        /// <summary>
        /// 取消关联共享
        /// </summary>
        public void CancelAssociate()
        {
            if (this.RemodelingId != null)
            {
                this.RemodelingId = null;
                this.Solved = Bool.否;
            }
        }
    }
}
