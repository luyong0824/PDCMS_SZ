using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 派工单明细实体
    /// </summary>
    public class WorkOrderDetail : AggregateRoot
    {
        protected WorkOrderDetail()
        {
        }

        /// <summary>
        /// 构造派工单明细实体
        /// </summary>
        /// <param name="workOrderId">派工单Id</param>
        /// <param name="workBeginDate">工作起始时间</param>
        /// <param name="beginHour">工作起始时间(小时)</param>
        /// <param name="beginMinute">工作起始时间(分钟)</param>
        /// <param name="workEndDate">工作结束时间</param>
        /// <param name="endHour">工作结束时间(小时)</param>
        /// <param name="endMinute">工作结束时间(分钟)</param>
        /// <param name="isFinish">是否完成</param>
        /// <param name="executeSituation">执行情况</param>
        /// <param name="materialConsumption">材料消耗</param>
        /// <param name="personnelNumber">人员数量</param>
        /// <param name="carType">车辆类型</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal WorkOrderDetail(Guid workOrderId, DateTime workBeginDate, int beginHour, int beginMinute, DateTime workEndDate, int endHour, int endMinute, int isFinish,
            string executeSituation, string materialConsumption, string personnelNumber, string carType, Guid createUserId)
        {
            executeSituation.IsNullOrTooLong("执行情况", true, 500);
            materialConsumption.IsNullOrTooLong("材料消耗", true, 500);
            personnelNumber.IsNullOrTooLong("人员数量", true, 500);
            carType.IsNullOrTooLong("车辆类型", true, 500);

            this.Id = Guid.NewGuid();
            this.WorkOrderId = workOrderId;
            this.WorkBeginDate = DateTime.Parse(workBeginDate.ToShortDateString());
            this.BeginHour = beginHour;
            this.BeginMinute = beginMinute;
            this.WorkEndDate = DateTime.Parse(workEndDate.ToShortDateString());
            this.EndHour = endHour;
            this.EndMinute = endMinute;
            this.IsFinish = isFinish;
            this.ExecuteSituation = executeSituation;
            this.MaterialConsumption = materialConsumption;
            this.PersonnelNumber = personnelNumber;
            this.CarType = carType;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 派工单Id
        /// </summary>
        public Guid WorkOrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间
        /// </summary>
        public DateTime WorkBeginDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间(小时)
        /// </summary>
        public int BeginHour
        {
            get;
            set;
        }

        /// <summary>
        /// 工作起始时间(分钟)
        /// </summary>
        public int BeginMinute
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间
        /// </summary>
        public DateTime WorkEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间(小时)
        /// </summary>
        public int EndHour
        {
            get;
            set;
        }

        /// <summary>
        /// 工作结束时间(分钟)
        /// </summary>
        public int EndMinute
        {
            get;
            set;
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        public int IsFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 执行情况
        /// </summary>
        public string ExecuteSituation
        {
            get;
            set;
        }

        /// <summary>
        /// 材料消耗
        /// </summary>
        public string MaterialConsumption
        {
            get;
            set;
        }

        /// <summary>
        /// 人员数量
        /// </summary>
        public string PersonnelNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 车辆类型
        /// </summary>
        public string CarType
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
        /// 修改派工单明细实体
        /// </summary>
        /// <param name="workBeginDate">工作起始时间</param>
        /// <param name="beginHour">工作起始时间(小时)</param>
        /// <param name="beginMinute">工作起始时间(分钟)</param>
        /// <param name="workEndDate">工作结束时间</param>
        /// <param name="endHour">工作结束时间(小时)</param>
        /// <param name="endMinute">工作结束时间(分钟)</param>
        /// <param name="isFinish">是否完成</param>
        /// <param name="executeSituation">执行情况</param>
        /// <param name="materialConsumption">材料消耗</param>
        /// <param name="personnelNumber">人员数量</param>
        /// <param name="carType">车辆类型</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(DateTime workBeginDate, int beginHour, int beginMinute, DateTime workEndDate, int endHour, int endMinute, int isFinish,
            string executeSituation, string materialConsumption, string personnelNumber, string carType, Guid modifyUserId)
        {
            executeSituation.IsNullOrTooLong("执行情况", true, 500);
            materialConsumption.IsNullOrTooLong("材料消耗", true, 500);
            personnelNumber.IsNullOrTooLong("人员数量", true, 500);
            carType.IsNullOrTooLong("车辆类型", true, 500);

            this.WorkBeginDate = workBeginDate;
            this.BeginHour = beginHour;
            this.BeginMinute = beginMinute;
            this.WorkEndDate = workEndDate;
            this.EndHour = endHour;
            this.EndMinute = endMinute;
            this.IsFinish = isFinish;
            this.ExecuteSituation = executeSituation;
            this.MaterialConsumption = materialConsumption;
            this.PersonnelNumber = personnelNumber;
            this.CarType = carType;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
