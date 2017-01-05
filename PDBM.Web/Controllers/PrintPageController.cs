using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Communication;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 打印页控制器
    /// </summary>
    public class PrintPageController : BaseController
    {
        /// <summary>
        /// 建设申请打印页
        /// </summary>
        /// <param name="id">建设申请Id</param>
        /// <returns></returns>
        public async Task<ActionResult> PlanningApply(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                PlanningApplyHeaderPrintObject planningApplyHeaderPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetPlanningApplyPrintById(id));
                ViewData["OrderCode"] = planningApplyHeaderPrintObject.OrderCode;
                ViewData["DepartmentName"] = planningApplyHeaderPrintObject.DepartmentName;
                ViewData["CreateFullName"] = planningApplyHeaderPrintObject.CreateFullName;
                ViewData["CreateDateText"] = planningApplyHeaderPrintObject.CreateDateText;
                ViewData["Title"] = planningApplyHeaderPrintObject.Title;
                ViewData["PlanningApplyDetailHtml"] = planningApplyHeaderPrintObject.PlanningApplyDetailHtml;
                ViewData["WFActivityInstancesInfoHtml"] = planningApplyHeaderPrintObject.WFActivityInstancesInfoHtml;
            }
            return View();
        }

        /// <summary>
        /// 寻址确认打印页
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <returns></returns>
        public async Task<ActionResult> Addressing(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingPrintObject addressingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingPrintObject.OrderCode;
                ViewData["CreateDateText"] = addressingPrintObject.CreateDateText;
                ViewData["PlanningName"] = addressingPrintObject.PlanningName;
                ViewData["PlaceName"] = addressingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = addressingPrintObject.PlaceCategoryName;
                ViewData["ImportanceName"] = addressingPrintObject.ImportanceName;
                ViewData["AreaName"] = addressingPrintObject.AreaName;
                ViewData["ReseauName"] = addressingPrintObject.ReseauName;
                ViewData["Lng"] = addressingPrintObject.Lng;
                ViewData["Lat"] = addressingPrintObject.Lat;
                ViewData["PlaceOwnerName"] = addressingPrintObject.PlaceOwnerName;
                ViewData["ProposedNetwork"] = addressingPrintObject.ProposedNetwork;
                ViewData["AddressingDepartmentName"] = addressingPrintObject.AddressingDepartmentName;
                ViewData["AddressingRealName"] = addressingPrintObject.AddressingRealName;
                ViewData["OwnerName"] = addressingPrintObject.OwnerName;
                ViewData["OwnerContact"] = addressingPrintObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = addressingPrintObject.OwnerPhoneNumber;
                ViewData["DetailedAddress"] = addressingPrintObject.DetailedAddress;
                ViewData["Remarks"] = addressingPrintObject.Remarks;
                ViewData["PlaceId"] = addressingPrintObject.PlaceId;
                ViewData["PlanningId"] = addressingPrintObject.PlanningId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["Id1"] = projectTaskEditObject.Id1;
                ViewData["Id2"] = projectTaskEditObject.Id2;
                ViewData["Id3"] = projectTaskEditObject.Id3;
                ViewData["Id4"] = projectTaskEditObject.Id4;
                ViewData["Id5"] = projectTaskEditObject.Id5;
                ViewData["Id6"] = projectTaskEditObject.Id6;
                ViewData["Mark1"] = projectTaskEditObject.Mark1;
                ViewData["Mark2"] = projectTaskEditObject.Mark2;
                ViewData["Mark3"] = projectTaskEditObject.Mark3;
                ViewData["Mark4"] = projectTaskEditObject.Mark4;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["DesignCustomerName1"] = projectTaskEditObject.DesignCustomerName1;
                ViewData["ConstructionCustomerName1"] = projectTaskEditObject.ConstructionCustomerName1;
                ViewData["SupervisionCustomerName1"] = projectTaskEditObject.SupervisionCustomerName1;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["DesignCustomerName2"] = projectTaskEditObject.DesignCustomerName2;
                ViewData["ConstructionCustomerName2"] = projectTaskEditObject.ConstructionCustomerName2;
                ViewData["SupervisionCustomerName2"] = projectTaskEditObject.SupervisionCustomerName2;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["DesignCustomerName3"] = projectTaskEditObject.DesignCustomerName3;
                ViewData["ConstructionCustomerName3"] = projectTaskEditObject.ConstructionCustomerName3;
                ViewData["SupervisionCustomerName3"] = projectTaskEditObject.SupervisionCustomerName3;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["DesignCustomerName4"] = projectTaskEditObject.DesignCustomerName4;
                ViewData["ConstructionCustomerName4"] = projectTaskEditObject.ConstructionCustomerName4;
                ViewData["SupervisionCustomerName4"] = projectTaskEditObject.SupervisionCustomerName4;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["EngineeringProgressName1"] = projectTaskEditObject.EngineeringProgressName1;
                ViewData["EngineeringProgressName2"] = projectTaskEditObject.EngineeringProgressName2;
                ViewData["EngineeringProgressName3"] = projectTaskEditObject.EngineeringProgressName3;
                ViewData["EngineeringProgressName4"] = projectTaskEditObject.EngineeringProgressName4;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
                ViewData["ProjectBeginDateText"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["WFActivityInstancesInfoHtml"] = projectTaskEditObject.WFActivityInstancesInfoHtml;
            }
            return View();
        }

        /// <summary>
        /// 改造确认打印页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Remodeling(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ImportanceName"] = remodelingPrintObject.ImportanceName;
                ViewData["AreaName"] = remodelingPrintObject.AreaName;
                ViewData["ReseauName"] = remodelingPrintObject.ReseauName;
                ViewData["Lng"] = remodelingPrintObject.Lng;
                ViewData["Lat"] = remodelingPrintObject.Lat;
                ViewData["PlaceOwnerName"] = remodelingPrintObject.PlaceOwnerName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["OwnerName"] = remodelingPrintObject.OwnerName;
                ViewData["OwnerContact"] = remodelingPrintObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = remodelingPrintObject.OwnerPhoneNumber;
                ViewData["DetailedAddress"] = remodelingPrintObject.DetailedAddress;
                ViewData["Remarks"] = remodelingPrintObject.Remarks;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
                ViewData["WFActivityInstancesInfoHtml"] = remodelingPrintObject.WFActivityInstancesInfoHtml;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["Id1"] = projectTaskEditObject.Id1;
                ViewData["Id2"] = projectTaskEditObject.Id2;
                ViewData["Id3"] = projectTaskEditObject.Id3;
                ViewData["Id4"] = projectTaskEditObject.Id4;
                ViewData["Id5"] = projectTaskEditObject.Id5;
                ViewData["Id6"] = projectTaskEditObject.Id6;
                ViewData["Mark1"] = projectTaskEditObject.Mark1;
                ViewData["Mark2"] = projectTaskEditObject.Mark2;
                ViewData["Mark3"] = projectTaskEditObject.Mark3;
                ViewData["Mark4"] = projectTaskEditObject.Mark4;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["DesignCustomerName1"] = projectTaskEditObject.DesignCustomerName1;
                ViewData["ConstructionCustomerName1"] = projectTaskEditObject.ConstructionCustomerName1;
                ViewData["SupervisionCustomerName1"] = projectTaskEditObject.SupervisionCustomerName1;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["DesignCustomerName2"] = projectTaskEditObject.DesignCustomerName2;
                ViewData["ConstructionCustomerName2"] = projectTaskEditObject.ConstructionCustomerName2;
                ViewData["SupervisionCustomerName2"] = projectTaskEditObject.SupervisionCustomerName2;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["DesignCustomerName3"] = projectTaskEditObject.DesignCustomerName3;
                ViewData["ConstructionCustomerName3"] = projectTaskEditObject.ConstructionCustomerName3;
                ViewData["SupervisionCustomerName3"] = projectTaskEditObject.SupervisionCustomerName3;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["DesignCustomerName4"] = projectTaskEditObject.DesignCustomerName4;
                ViewData["ConstructionCustomerName4"] = projectTaskEditObject.ConstructionCustomerName4;
                ViewData["SupervisionCustomerName4"] = projectTaskEditObject.SupervisionCustomerName4;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["EngineeringProgressName1"] = projectTaskEditObject.EngineeringProgressName1;
                ViewData["EngineeringProgressName2"] = projectTaskEditObject.EngineeringProgressName2;
                ViewData["EngineeringProgressName3"] = projectTaskEditObject.EngineeringProgressName3;
                ViewData["EngineeringProgressName4"] = projectTaskEditObject.EngineeringProgressName4;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
                ViewData["ProjectBeginDateText"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectTypeName"] = projectTaskEditObject.ProjectTypeName;
            }
            return View();
        }

        /// <summary>
        /// 隐患上报打印页
        /// </summary>
        /// <param name="id">隐患上报Id</param>
        /// <returns></returns>
        public async Task<ActionResult> WorkApply(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                WorkApplyPrintObject workApplyPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetWorkApplyPrintById(id));
                ViewData["OrderCode"] = workApplyPrintObject.OrderCode;
                ViewData["DepartmentName"] = workApplyPrintObject.DepartmentName;
                ViewData["CreateFullName"] = workApplyPrintObject.CreateFullName;
                ViewData["SendFullName"] = workApplyPrintObject.SendFullName;
                ViewData["CreateDate"] = workApplyPrintObject.CreateDate;
                ViewData["Title"] = workApplyPrintObject.Title;
                ViewData["ApplyReason"] = workApplyPrintObject.ApplyReason;
                ViewData["ReseauName"] = workApplyPrintObject.ReseauName;
                ViewData["CustomerName"] = workApplyPrintObject.CustomerName;
                ViewData["Id"] = workApplyPrintObject.Id;
                ViewData["Count"] = workApplyPrintObject.Count;
                ViewData["WFActivityInstancesInfoHtml"] = workApplyPrintObject.WFActivityInstancesInfoHtml;
            }
            return View();
        }

        /// <summary>
        /// 隐患上报打印页(可查看附件)
        /// </summary>
        /// <param name="id">隐患上报Id</param>
        /// <returns></returns>
        public async Task<ActionResult> WorkApplyPrint(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                WorkApplyPrintObject workApplyPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetWorkApplyPrintById(id));
                ViewData["OrderCode"] = workApplyPrintObject.OrderCode;
                ViewData["DepartmentName"] = workApplyPrintObject.DepartmentName;
                ViewData["CreateFullName"] = workApplyPrintObject.CreateFullName;
                ViewData["SendFullName"] = workApplyPrintObject.SendFullName;
                ViewData["CreateDate"] = workApplyPrintObject.CreateDate;
                ViewData["Title"] = workApplyPrintObject.Title;
                ViewData["ApplyReason"] = workApplyPrintObject.ApplyReason;
                ViewData["ReseauName"] = workApplyPrintObject.ReseauName;
                ViewData["CustomerName"] = workApplyPrintObject.CustomerName;
                ViewData["Id"] = id;
                ViewData["Count"] = workApplyPrintObject.Count;
                ViewData["SceneContactMan"] = workApplyPrintObject.SceneContactMan;
                ViewData["SceneContactTel"] = workApplyPrintObject.SceneContactTel;
                ViewData["ProjectCode"] = workApplyPrintObject.ProjectCode;
                ViewData["WFActivityInstancesInfoHtml"] = workApplyPrintObject.WFActivityInstancesInfoHtml;
            }
            return View();
        }

        /// <summary>
        /// 零星派工单打印页
        /// </summary>
        /// <param name="id">零星派工单Id</param>
        /// <returns></returns>
        public async Task<ActionResult> WorkOrder(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IWorkOrderService> proxy = new ServiceProxy<IWorkOrderService>())
            {
                WorkOrderPrintObject workOrderPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetWorkOrderPrintById(id));
                ViewData["Id"] = id;
                ViewData["Count"] = workOrderPrintObject.Count;
                ViewData["SettlementCount"] = workOrderPrintObject.SettlementCount;
                ViewData["OrderCode"] = workOrderPrintObject.OrderCode;
                ViewData["FullName"] = workOrderPrintObject.FullName;
                ViewData["CreateDate"] = workOrderPrintObject.CreateDate;
                ViewData["Title"] = workOrderPrintObject.Title;
                ViewData["ReseauName"] = workOrderPrintObject.ReseauName;
                ViewData["ClassName"] = workOrderPrintObject.ClassName;
                ViewData["RequireSendDate"] = workOrderPrintObject.RequireSendDate;
                ViewData["SceneContactMan"] = workOrderPrintObject.SceneContactMan;
                ViewData["SceneContactTel"] = workOrderPrintObject.SceneContactTel;
                ViewData["CustomerName"] = workOrderPrintObject.CustomerName;
                ViewData["Days"] = workOrderPrintObject.Days;
                ViewData["MaintainContactMan"] = workOrderPrintObject.MaintainContactMan;
                ViewData["MainTainContactTel"] = workOrderPrintObject.MainTainContactTel;
                ViewData["WorkContent"] = workOrderPrintObject.WorkContent;
                ViewData["HumanRequire"] = workOrderPrintObject.HumanRequire;
                ViewData["CarRequire"] = workOrderPrintObject.CarRequire;
                ViewData["MaterialRequire"] = workOrderPrintObject.MaterialRequire;
                ViewData["WorkBeginDate"] = workOrderPrintObject.WorkBeginDate;
                ViewData["WorkEndDate"] = workOrderPrintObject.WorkEndDate;
                ViewData["IsFinish"] = workOrderPrintObject.IsFinish;
                ViewData["RegisterFullName"] = workOrderPrintObject.RegisterFullName;
                ViewData["RegisterDate"] = workOrderPrintObject.RegisterDate;
                ViewData["ExecuteSituation"] = workOrderPrintObject.ExecuteSituation;
                ViewData["MaterialConsumption"] = workOrderPrintObject.MaterialConsumption;
                ViewData["PersonnelNumber"] = workOrderPrintObject.PersonnelNumber;
                ViewData["CarType"] = workOrderPrintObject.CarType;
                ViewData["WorkOrderDetailInfoHtml"] = workOrderPrintObject.WorkOrderDetailInfoHtml;
                ViewData["WFActivityInstancesInfoHtml"] = workOrderPrintObject.WFActivityInstancesInfoHtml;
            }
            return View();
        }

        /// <summary>
        /// 零星派工单打印页
        /// </summary>
        /// <param name="id">零星派工单Id</param>
        /// <returns></returns>
        public async Task<ActionResult> WorkOrderPrint(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IWorkOrderService> proxy = new ServiceProxy<IWorkOrderService>())
            {
                WorkOrderPrintObject workOrderPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetWorkOrderPrintById(id));
                ViewData["OrderCode"] = workOrderPrintObject.OrderCode;
                ViewData["FullName"] = workOrderPrintObject.FullName;
                ViewData["CreateDate"] = workOrderPrintObject.CreateDate;
                ViewData["Title"] = workOrderPrintObject.Title;
                ViewData["ReseauName"] = workOrderPrintObject.ReseauName;
                ViewData["ClassName"] = workOrderPrintObject.ClassName;
                ViewData["RequireSendDate"] = workOrderPrintObject.RequireSendDate;
                ViewData["SceneContactMan"] = workOrderPrintObject.SceneContactMan;
                ViewData["SceneContactTel"] = workOrderPrintObject.SceneContactTel;
                ViewData["CustomerName"] = workOrderPrintObject.CustomerName;
                ViewData["Days"] = workOrderPrintObject.Days;
                ViewData["MaintainContactMan"] = workOrderPrintObject.MaintainContactMan;
                ViewData["MainTainContactTel"] = workOrderPrintObject.MainTainContactTel;
                ViewData["WorkContent"] = workOrderPrintObject.WorkContent;
                ViewData["HumanRequire"] = workOrderPrintObject.HumanRequire;
                ViewData["CarRequire"] = workOrderPrintObject.CarRequire;
                ViewData["MaterialRequire"] = workOrderPrintObject.MaterialRequire;
                ViewData["WorkBeginDate"] = workOrderPrintObject.WorkBeginDate;
                ViewData["WorkEndDate"] = workOrderPrintObject.WorkEndDate;
                ViewData["IsFinish"] = workOrderPrintObject.IsFinish;
                ViewData["RegisterFullName"] = workOrderPrintObject.RegisterFullName;
                ViewData["RegisterDate"] = workOrderPrintObject.RegisterDate;
                ViewData["ExecuteSituation"] = workOrderPrintObject.ExecuteSituation;
                ViewData["MaterialConsumption"] = workOrderPrintObject.MaterialConsumption;
                ViewData["PersonnelNumber"] = workOrderPrintObject.PersonnelNumber;
                ViewData["CarType"] = workOrderPrintObject.CarType;
                ViewData["WorkOrderDetailInfoHtml"] = workOrderPrintObject.WorkOrderDetailInfoHtml;
                ViewData["WFActivityInstancesInfoHtml"] = workOrderPrintObject.WFActivityInstancesInfoHtml;
            }
            return View();
        }

        /// <summary>
        /// 寻址确认打印页(室分)
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AddressingID(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingPrintObject addressingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingPrintObject.OrderCode;
                ViewData["CreateDateText"] = addressingPrintObject.CreateDateText;
                ViewData["PlanningName"] = addressingPrintObject.PlanningName;
                ViewData["PlaceName"] = addressingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = addressingPrintObject.PlaceCategoryName;
                ViewData["ImportanceName"] = addressingPrintObject.ImportanceName;
                ViewData["AreaName"] = addressingPrintObject.AreaName;
                ViewData["ReseauName"] = addressingPrintObject.ReseauName;
                ViewData["Lng"] = addressingPrintObject.Lng;
                ViewData["Lat"] = addressingPrintObject.Lat;
                ViewData["PlaceOwnerName"] = addressingPrintObject.PlaceOwnerName;
                ViewData["ProposedNetwork"] = addressingPrintObject.ProposedNetwork;
                ViewData["AddressingDepartmentName"] = addressingPrintObject.AddressingDepartmentName;
                ViewData["AddressingRealName"] = addressingPrintObject.AddressingRealName;
                ViewData["OwnerName"] = addressingPrintObject.OwnerName;
                ViewData["OwnerContact"] = addressingPrintObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = addressingPrintObject.OwnerPhoneNumber;
                ViewData["DetailedAddress"] = addressingPrintObject.DetailedAddress;
                ViewData["Remarks"] = addressingPrintObject.Remarks;
                ViewData["PlaceId"] = addressingPrintObject.PlaceId;
                ViewData["PlanningId"] = addressingPrintObject.PlanningId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["Id5"] = projectTaskEditObject.Id5;
                ViewData["Id6"] = projectTaskEditObject.Id6;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
                ViewData["ProjectBeginDateText"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["WFActivityInstancesInfoHtml"] = projectTaskEditObject.WFActivityInstancesInfoHtml;
            }
            return View();
        }

        /// <summary>
        /// 改造确认打印页(室分)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> RemodelingID(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ImportanceName"] = remodelingPrintObject.ImportanceName;
                ViewData["AreaName"] = remodelingPrintObject.AreaName;
                ViewData["ReseauName"] = remodelingPrintObject.ReseauName;
                ViewData["Lng"] = remodelingPrintObject.Lng;
                ViewData["Lat"] = remodelingPrintObject.Lat;
                ViewData["PlaceOwnerName"] = remodelingPrintObject.PlaceOwnerName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["OwnerName"] = remodelingPrintObject.OwnerName;
                ViewData["OwnerContact"] = remodelingPrintObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = remodelingPrintObject.OwnerPhoneNumber;
                ViewData["DetailedAddress"] = remodelingPrintObject.DetailedAddress;
                ViewData["Remarks"] = remodelingPrintObject.Remarks;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
                ViewData["WFActivityInstancesInfoHtml"] = remodelingPrintObject.WFActivityInstancesInfoHtml;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["Id5"] = projectTaskEditObject.Id5;
                ViewData["Id6"] = projectTaskEditObject.Id6;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
                ViewData["ProjectBeginDateText"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectTypeName"] = projectTaskEditObject.ProjectTypeName;
            }
            return View();
        }
    }
}