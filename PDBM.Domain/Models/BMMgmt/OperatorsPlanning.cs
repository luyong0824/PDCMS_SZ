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
    /// 运营商规划实体
    /// </summary>
    public class OperatorsPlanning : AggregateRoot
    {
        protected OperatorsPlanning()
        {
        }

        /// <summary>
        /// 构造运营商规划实体
        /// </summary>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="antennaHeight">天线挂高(米)</param>
        /// <param name="poleNumber">抱杆数量(根)</param>
        /// <param name="cabinetNumber">机柜数量(个)</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="solved">是否采纳</param>
        /// <param name="remarks">备注</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="planningId">基站规划Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal OperatorsPlanning(string planningCode, string planningName, Profession profession, Guid placeCategoryId, Guid areaId,
            decimal lng, decimal lat, decimal antennaHeight, int poleNumber, int cabinetNumber, Urgency urgency, Bool solved, string remarks,
            Guid companyId, Guid? planningId, Guid createUserId)
        {
            planningCode.IsNullOrEmptyOrTooLong("规划编码", true, 50);
            planningName.IsNullOrEmptyOrTooLong("规划名称", true, 100);
            profession.IsInvalid("专业");
            placeCategoryId.IsEmpty("站点类型Id");
            areaId.IsEmpty("区域Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            antennaHeight.IsNonnegative("天线挂高");
            poleNumber.IsNonnegative("抱杆数量");
            cabinetNumber.IsNonnegative("机柜数量");
            urgency.IsInvalid("紧要程度");
            remarks.IsNullOrTooLong("备注", true, 150);
            companyId.IsEmpty("公司Id");

            this.Id = Guid.NewGuid();
            this.PlanningCode = planningCode;
            this.PlanningName = planningName;
            this.Profession = profession;
            this.PlaceCategoryId = placeCategoryId;
            this.AreaId = areaId;
            this.Lng = lng;
            this.Lat = lat;
            this.AntennaHeight = antennaHeight;
            this.PoleNumber = poleNumber;
            this.CabinetNumber = cabinetNumber;
            this.Urgency = urgency;
            this.Solved = solved;
            this.ToShared = Bool.否;
            this.Remarks = remarks;
            this.CompanyId = companyId;
            this.PlanningId = planningId;
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
        /// 站点类型Id
        /// </summary>
        public Guid PlaceCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        public Guid AreaId
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
        /// 天线挂高(米)
        /// </summary>
        public decimal AntennaHeight
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
        /// 是否转共享
        /// </summary>
        public Bool ToShared
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
        /// 规划Id
        /// </summary>
        public Guid? PlanningId
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
        /// 区域实体
        /// </summary>
        protected virtual Area Area
        {
            get;
            set;
        }

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
        /// 修改运营商规划实体
        /// </summary>
        /// <param name="planningName">规划名称</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="antennaHeight">天线挂高(米)</param>
        /// <param name="poleNumber">抱杆数量(根)</param>
        /// <param name="cabinetNumber">机柜数量(个)</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="remarks">备注</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string planningName, Guid placeCategoryId, Guid areaId, decimal lng, decimal lat, decimal antennaHeight,
            int poleNumber, int cabinetNumber, Urgency urgency, string remarks, Guid modifyUserId)
        {
            planningName.IsNullOrEmptyOrTooLong("规划名称", true, 100);
            placeCategoryId.IsEmpty("站点类型Id");
            areaId.IsEmpty("区域Id");
            lng.IsNonnegative("经度");
            lat.IsNonnegative("纬度");
            antennaHeight.IsNonnegative("天线挂高");
            poleNumber.IsNonnegative("抱杆数量");
            cabinetNumber.IsNonnegative("机柜数量");
            urgency.IsInvalid("紧要程度");
            remarks.IsNullOrTooLong("备注", true, 150);

            this.PlanningName = planningName;
            this.PlaceCategoryId = placeCategoryId;
            this.AreaId = areaId;
            this.Lng = lng;
            this.Lat = lat;
            this.AntennaHeight = antennaHeight;
            this.PoleNumber = poleNumber;
            this.CabinetNumber = cabinetNumber;
            this.Urgency = urgency;
            this.Remarks = remarks;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改规划编码，仅用于批量导入
        /// </summary>
        /// <param name="planningCode">规划编码</param>
        public void ModifyPlanningCode(string planningCode)
        {
            planningCode.IsNullOrEmptyOrTooLong("规划编码", true, 50);

            this.PlanningCode = planningCode;
        }

        /// <summary>
        /// 修改运营商规划检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByUpdate(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能修改别人创建的规划");
            }
            if (this.Solved == Bool.是)
            {
                throw new DomainFault("不能修改已解决的规划");
            }
        }

        /// <summary>
        /// 删除运营商规划检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByRemove(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("{0}<br>不能删除别人创建的规划", this.PlanningCode);
            }
            if (this.Solved == Bool.是)
            {
                throw new DomainFault("{0}<br>不能删除已解决的规划", this.PlanningCode);
            }
        }

        /// <summary>
        /// 关联规划
        /// </summary>
        /// <param name="planningId">规划Id</param>
        public void Associate(Guid planningId)
        {
            planningId.IsEmpty("规划Id");

            if (this.PlanningId == null)
            {
                this.PlanningId = planningId;
                this.Solved = Bool.是;
            }
        }

        /// <summary>
        /// 取消关联规划
        /// </summary>
        public void CancelAssociate()
        {
            if (this.PlanningId != null)
            {
                this.PlanningId = null;
                this.Solved = Bool.否;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DemandSolved(Demand demand)
        {
            if (demand == Demand.需要)
            {
                this.Solved = Bool.是;
            }
            else
            {
                this.Solved = Bool.否;
            }
        }

        /// <summary>
        /// 运营商规划转为基站改造
        /// </summary>
        /// <param name="isToShared"></param>
        public void ModifyIsToShared(Bool toShared)
        {
            this.ToShared = toShared;
        }
    }
}
