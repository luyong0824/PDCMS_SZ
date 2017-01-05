using PDBM.Domain.Events;
using PDBM.Domain.Events.WorkFlow;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 派工单实体
    /// </summary>
    public class WorkOrder : AggregateRoot
    {
        protected WorkOrder()
        {
        }

        /// <summary>
        /// 构造派工单实体
        /// </summary>
        /// <param name="placeName">站点名称</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="workSmallClassId">工单小类Id</param>
        /// <param name="sceneContactMan">现场联系人</param>
        /// <param name="sceneContactTel">现场联系电话</param>
        /// <param name="requireSendDate">要求派工日期</param>
        /// <param name="days">派工时长</param>
        /// <param name="customerId">代维单位</param>
        /// <param name="customerUserId">代维联系人Id</param>
        /// <param name="workContent">工作内容</param>
        /// <param name="humanRequire">用人要求</param>
        /// <param name="carRequire">车辆要求</param>
        /// <param name="materialRequire">材料要求</param>
        /// <param name="remarks">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal WorkOrder(string placeName, string title, Guid reseauId, Guid workSmallClassId, string sceneContactMan, string sceneContactTel, DateTime requireSendDate, int days, Guid customerId,
            Guid customerUserId, string workContent, string humanRequire, string carRequire, string materialRequire, Guid createUserId)
        {
            placeName.IsNullOrEmptyOrTooLong("站点名称", true, 50);
            title.IsNullOrEmptyOrTooLong("标题", true, 150);
            reseauId.IsEmpty("网格经理Id");
            workSmallClassId.IsEmpty("派工小类Id");
            customerId.IsEmpty("代维单位Id");
            sceneContactMan.IsNullOrTooLong("现场联系人", true, 50);
            sceneContactTel.IsNullOrTooLong("现场联系电话", true, 50);
            customerUserId.IsEmpty("代维联系人Id");
            workContent.IsNullOrTooLong("工作内容", true, 500);
            humanRequire.IsNullOrTooLong("用人要求", true, 500);
            carRequire.IsNullOrTooLong("用车要求", true, 500);
            materialRequire.IsNullOrTooLong("材料要求", true, 500);

            this.Id = Guid.NewGuid();
            this.OrderCode = "";
            this.PlaceName = placeName;
            this.Title = title;
            this.ReseauId = reseauId;
            this.WorkSmallClassId = workSmallClassId;
            this.SceneContactMan = sceneContactMan;
            this.SceneContactTel = sceneContactTel;
            this.RequireSendDate = requireSendDate;
            this.Days = days;
            this.CustomerId = customerId;
            this.CustomerUserId = customerUserId;
            this.WorkContent = workContent;
            this.HumanRequire = humanRequire;
            this.CarRequire = carRequire;
            this.MaterialRequire = materialRequire;
            this.Remarks = "";
            this.WorkBeginDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            this.BeginHour = 0;
            this.BeginMinute = 0;
            this.WorkEndDate = WorkBeginDate;
            this.EndHour = 0;
            this.EndMinute = 0;
            this.IsFinish = Bool.否;
            this.RegisterUserId = Guid.Empty;
            this.RegisterDate = WorkBeginDate;
            this.ExecuteSituation = "";
            this.MaterialConsumption = "";
            this.PersonnelNumber = "";
            this.CarType = "";
            this.OrderState = WFProcessInstanceState.未发送;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        public string OrderCode
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
        /// 标题
        /// </summary>
        public string Title
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
        /// 工单小类Id
        /// </summary>
        public Guid WorkSmallClassId
        {
            get;
            set;
        }

        /// <summary>
        /// 现场联系人
        /// </summary>
        public string SceneContactMan
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string SceneContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 要求派工日期
        /// </summary>
        public DateTime RequireSendDate
        {
            get;
            set;
        }

        /// <summary>
        /// 派工时长
        /// </summary>
        public int Days
        {
            get;
            set;
        }

        /// <summary>
        /// 代维单位
        /// </summary>
        public Guid CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 代维联系人Id
        /// </summary>
        public Guid CustomerUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作内容
        /// </summary>
        public string WorkContent
        {
            get;
            set;
        }

        /// <summary>
        /// 用人要求
        /// </summary>
        public string HumanRequire
        {
            get;
            set;
        }

        /// <summary>
        /// 用车要求
        /// </summary>
        public string CarRequire
        {
            get;
            set;
        }

        /// <summary>
        /// 材料要求
        /// </summary>
        public string MaterialRequire
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
        public Bool IsFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 结算登记人
        /// </summary>
        public Guid? RegisterUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 结算登记日期
        /// </summary>
        public DateTime RegisterDate
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
        /// 派工单审批状态
        /// </summary>
        public WFProcessInstanceState OrderState
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
        /// 修改派工单
        /// </summary>
        /// <param name="placeName">站点名称</param>
        /// <param name="title">标题</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="workSmallClassId">工单小类Id</param>
        /// <param name="sceneContactMan">现场联系人</param>
        /// <param name="sceneContactTel">现场联系电话</param>
        /// <param name="requireSendDate">要求派工时间</param>
        /// <param name="days">派工时长</param>
        /// <param name="customerId">代维单位Id</param>
        /// <param name="customerUserId">代维联系人Id</param>
        /// <param name="workContent">工作内容</param>
        /// <param name="humanRequire">用人要求</param>
        /// <param name="carRequire">用车要求</param>
        /// <param name="materialRequire">材料要求</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string placeName, string title, Guid reseauId, Guid workSmallClassId, string sceneContactMan, string sceneContactTel, DateTime requireSendDate, int days, Guid customerId,
            Guid customerUserId, string workContent, string humanRequire, string carRequire, string materialRequire, Guid modifyUserId)
        {
            placeName.IsNullOrEmptyOrTooLong("站点名称", true, 50);
            title.IsNullOrEmptyOrTooLong("标题", true, 150);
            reseauId.IsEmpty("网格Id");
            workSmallClassId.IsEmpty("派工小类Id");
            customerId.IsEmpty("代维单位Id");
            sceneContactMan.IsNullOrTooLong("现场联系人", true, 50);
            sceneContactTel.IsNullOrTooLong("现场联系电话", true, 50);
            customerUserId.IsEmpty("代维联系人Id");
            workContent.IsNullOrTooLong("工作内容", true, 500);
            humanRequire.IsNullOrTooLong("用人要求", true, 500);
            carRequire.IsNullOrTooLong("用车要求", true, 500);
            materialRequire.IsNullOrTooLong("材料要求", true, 500);

            this.PlaceName = placeName;
            this.Title = title;
            this.ReseauId = reseauId;
            this.WorkSmallClassId = workSmallClassId;
            this.SceneContactMan = sceneContactMan;
            this.SceneContactTel = sceneContactTel;
            this.RequireSendDate = requireSendDate;
            this.Days = days;
            this.CustomerId = customerId;
            this.CustomerUserId = customerUserId;
            this.WorkContent = workContent;
            this.HumanRequire = humanRequire;
            this.CarRequire = carRequire;
            this.MaterialRequire = materialRequire;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 登记结算
        /// </summary>
        /// <param name="workBeginDate">工作开始时间</param>
        /// <param name="beginHour">工作开始时间(小时)</param>
        /// <param name="beginMinute">工作开始时间(分钟)</param>
        /// <param name="workEndDate">工作结束时间</param>
        /// <param name="endHour">工作结束时间(小时)</param>
        /// <param name="endMinute">工作结束时间(分钟)</param>
        /// <param name="executeSituation">执行情况</param>
        /// <param name="materialConsumption">材料消耗</param>
        /// <param name="personnelNumber">人员数量</param>
        /// <param name="carType">车辆类型</param>
        /// <param name="isFinish">是否完成</param>
        /// <param name="ModifyUserId">登记人用户Id</param>
        public void SaveWorkOrderWF(DateTime workBeginDate, int beginHour, int beginMinute, DateTime workEndDate, int endHour, int endMinute, string executeSituation, string materialConsumption, string personnelNumber, string carType, Bool isFinish, Guid ModifyUserId)
        {
            executeSituation.IsNullOrTooLong("执行情况", true, 500);
            materialConsumption.IsNullOrTooLong("材料消耗", true, 500);
            personnelNumber.IsNullOrTooLong("人员数量", true, 500);
            carType.IsNullOrTooLong("车辆类型", true, 500);

            this.WorkBeginDate = DateTime.Parse(workBeginDate.ToShortDateString());
            this.BeginHour = beginHour;
            this.BeginMinute = beginMinute;
            this.WorkEndDate = DateTime.Parse(workEndDate.ToShortDateString());
            this.EndHour = endHour;
            this.EndMinute = endMinute;
            this.ExecuteSituation = executeSituation;
            this.MaterialConsumption = materialConsumption;
            this.PersonnelNumber = personnelNumber;
            this.CarType = carType;
            this.IsFinish = isFinish;
            this.RegisterUserId = ModifyUserId;
            this.RegisterDate = DateTime.Now;
        }

        /// <summary>
        /// 修改零星派工单检查
        /// </summary>
        /// <param name="currentUserId">当前操作用户Id</param>
        public void CheckByUpdate(Guid currentUserId)
        {
            if (this.CreateUserId != currentUserId)
            {
                throw new DomainFault("不能修改别人创建的零星派工单");
            }
            DomainEvent.Publish<WFProcessInstanceOrderModifyingEvent>(new WFProcessInstanceOrderModifyingEvent(this));
        }
    }
}
