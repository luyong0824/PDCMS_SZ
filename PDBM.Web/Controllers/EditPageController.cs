using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PDBM.DataTransferObjects.BaseData;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Communication;
using PDBM.Infrastructure.IoC;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using PDBM.ServiceContracts.BMMgmt;
using PDBM.ServiceContracts.Enum;
using PDBM.Web.Filters;
using System.Data;
using System.IO;
using System.Text;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 单据编辑控制器
    /// </summary>
    [AuthorizeFilter]
    public class EditPageController : BaseController
    {
        #region 业务审核
        /// <summary>
        /// 业务审核
        /// </summary>
        /// <param name="id">建设申请Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> BusinessAudit(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["ImportanceBySelect"] = JsonHelper.Encode(enumService.GetImportanceEnum());
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                PlanningApplyMaintObject planningApplyMaintObject = await Task.Factory.StartNew(() => proxy.Channel.GetPlanningApplyById(id));
                ViewData["Id"] = id;
                ViewData["CreateDateText"] = planningApplyMaintObject.CreateDateText;
                ViewData["PlanningCode"] = planningApplyMaintObject.PlanningCode;
                ViewData["PlanningName"] = planningApplyMaintObject.PlanningName;
                ViewData["AreaName"] = planningApplyMaintObject.AreaName;
                ViewData["ReseauName"] = planningApplyMaintObject.ReseauName;
                ViewData["AreaName"] = planningApplyMaintObject.AreaName;
                ViewData["ReseauName"] = planningApplyMaintObject.ReseauName;
                ViewData["Lng"] = planningApplyMaintObject.Lng;
                ViewData["Lat"] = planningApplyMaintObject.Lat;
                ViewData["DetailedAddress"] = planningApplyMaintObject.DetailedAddress;
                ViewData["Remarks"] = planningApplyMaintObject.Remarks;
                ViewData["Importance"] = planningApplyMaintObject.Importance;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
            }
            return View();
        }

        /// <summary>
        /// 保存业务审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveBusinessAudit()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlanningApplyMaintObject planningApplyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            planningApplyMaintObject = new PlanningApplyMaintObject()
            {
                Id = id,
                Importance = int.Parse(row["Importance"].ToString()),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString())
            };
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveBusinessAudit(planningApplyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 技术审核
        /// <summary>
        /// 技术审核
        /// </summary>
        /// <param name="id">建设申请Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> TechnicalAudit(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["Importance"] = JsonHelper.Encode(enumService.GetImportanceEnum());
            ViewData["PlanningAdvice"] = JsonHelper.Encode(enumService.GetPlanningAdviceEnum());

            ViewData["Id"] = id;
            ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                PlanningApplyHeaderPrintObject planningApplyHeaderPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetPlanningApplyPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = planningApplyHeaderPrintObject.OrderCode;
                ViewData["DepartmentName"] = planningApplyHeaderPrintObject.DepartmentName;
                ViewData["CreateFullName"] = planningApplyHeaderPrintObject.CreateFullName;
                ViewData["CreateDateText"] = planningApplyHeaderPrintObject.CreateDateText;
                ViewData["Title"] = planningApplyHeaderPrintObject.Title;
            }
            return View();
        }

        /// <summary>
        /// 保存技术审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTechnicalAudit()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlanningApplyMaintObject planningApplyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            planningApplyMaintObject = new PlanningApplyMaintObject()
            {
                Id = id,
                PlaceOwner = Guid.Parse(row["PlaceOwner"].ToString()),
                OptionalAddress = row["OptionalAddress"].ToString().Trim(),
                ProposedNetwork = row["ProposedNetwork"].ToString().Trim(),
                Lng = decimal.Parse(row["Lng"].ToString().Trim()),
                Lat = decimal.Parse(row["Lat"].ToString().Trim()),
                FileIdList = row["FileIdList"].ToString().Trim(),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                PlanningUserId = this.UserId
            };
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveTechnicalAudit(planningApplyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 指定项目经理及总设单位
        /// <summary>
        /// 指定项目经理及总设单位
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointAreaAndDesignUser(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

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
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["AreaManagerId"] = projectTaskEditObject.AreaManagerId;
                ViewData["GeneralDesignId"] = projectTaskEditObject.GeneralDesignId;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAreaAndDesignUser()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                AreaManagerId = Guid.Parse(row["AreaManagerId"].ToString()),
                GeneralDesignId = Guid.Parse(row["GeneralDesignId"].ToString()),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AppointAreaAndDesignUser(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 任务分配
        /// <summary>
        /// 任务分配
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ProjectDesign(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingPrintObject addressingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingPrintObject.OrderCode;
                ViewData["CreateDateText"] = addressingPrintObject.CreateDateText;
                ViewData["PlaceName"] = addressingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = addressingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = addressingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = addressingPrintObject.PlaceId;
                ViewData["PlanningId"] = addressingPrintObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
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
                ViewData["ProjectBeginDate"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["ProjectManagerId1"] = projectTaskEditObject.ProjectManagerId1;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["DesignCustomerId1"] = projectTaskEditObject.DesignCustomerId1;
                ViewData["DesignCustomerName1"] = projectTaskEditObject.DesignCustomerName1;
                ViewData["ConstructionCustomerId1"] = projectTaskEditObject.ConstructionCustomerId1;
                ViewData["ConstructionCustomerName1"] = projectTaskEditObject.ConstructionCustomerName1;
                ViewData["SupervisionCustomerId1"] = projectTaskEditObject.SupervisionCustomerId1;
                ViewData["SupervisionCustomerName1"] = projectTaskEditObject.SupervisionCustomerName1;
                ViewData["ProjectManagerId2"] = projectTaskEditObject.ProjectManagerId2;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["DesignCustomerId2"] = projectTaskEditObject.DesignCustomerId2;
                ViewData["DesignCustomerName2"] = projectTaskEditObject.DesignCustomerName2;
                ViewData["ConstructionCustomerId2"] = projectTaskEditObject.ConstructionCustomerId2;
                ViewData["ConstructionCustomerName2"] = projectTaskEditObject.ConstructionCustomerName2;
                ViewData["SupervisionCustomerId2"] = projectTaskEditObject.SupervisionCustomerId2;
                ViewData["SupervisionCustomerName2"] = projectTaskEditObject.SupervisionCustomerName2;
                ViewData["ProjectManagerId3"] = projectTaskEditObject.ProjectManagerId3;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["DesignCustomerId3"] = projectTaskEditObject.DesignCustomerId3;
                ViewData["DesignCustomerName3"] = projectTaskEditObject.DesignCustomerName3;
                ViewData["ConstructionCustomerId3"] = projectTaskEditObject.ConstructionCustomerId3;
                ViewData["ConstructionCustomerName3"] = projectTaskEditObject.ConstructionCustomerName3;
                ViewData["SupervisionCustomerId3"] = projectTaskEditObject.SupervisionCustomerId3;
                ViewData["SupervisionCustomerName3"] = projectTaskEditObject.SupervisionCustomerName3;
                ViewData["ProjectManagerId4"] = projectTaskEditObject.ProjectManagerId4;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["DesignCustomerId4"] = projectTaskEditObject.DesignCustomerId4;
                ViewData["DesignCustomerName4"] = projectTaskEditObject.DesignCustomerName4;
                ViewData["ConstructionCustomerId4"] = projectTaskEditObject.ConstructionCustomerId4;
                ViewData["ConstructionCustomerName4"] = projectTaskEditObject.ConstructionCustomerName4;
                ViewData["SupervisionCustomerId4"] = projectTaskEditObject.SupervisionCustomerId4;
                ViewData["SupervisionCustomerName4"] = projectTaskEditObject.SupervisionCustomerName4;
                ViewData["ProjectManagerId5"] = projectTaskEditObject.ProjectManagerId5;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerId5"] = projectTaskEditObject.DesignCustomerId5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerId5"] = projectTaskEditObject.ConstructionCustomerId5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerId5"] = projectTaskEditObject.SupervisionCustomerId5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["ProjectManagerId6"] = projectTaskEditObject.ProjectManagerId6;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerId6"] = projectTaskEditObject.DesignCustomerId6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerId6"] = projectTaskEditObject.ConstructionCustomerId6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerId6"] = projectTaskEditObject.SupervisionCustomerId6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
            }
            return View();
        }

        /// <summary>
        /// 保存项目设计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProjectDesign()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                Id1 = Guid.Parse(row["Id1"].ToString()),
                Id2 = Guid.Parse(row["Id2"].ToString()),
                Id3 = Guid.Parse(row["Id3"].ToString()),
                Id4 = Guid.Parse(row["Id4"].ToString()),
                Id5 = Guid.Parse(row["Id5"].ToString()),
                Id6 = Guid.Parse(row["Id6"].ToString()),
                Mark1 = int.Parse(row["Mark1"].ToString()),
                Mark2 = int.Parse(row["Mark2"].ToString()),
                Mark3 = int.Parse(row["Mark3"].ToString()),
                Mark4 = int.Parse(row["Mark4"].ToString()),
                Mark5 = int.Parse(row["Mark5"].ToString()),
                Mark6 = int.Parse(row["Mark6"].ToString()),
                ProjectManagerId1 = Guid.Parse(row["ProjectManagerId1"].ToString()),
                DesignCustomerId1 = Guid.Parse(row["DesignCustomerId1"].ToString()),
                ConstructionCustomerId1 = Guid.Parse(row["ConstructionCustomerId1"].ToString()),
                SupervisionCustomerId1 = Guid.Parse(row["SupervisionCustomerId1"].ToString()),
                ProjectManagerId2 = Guid.Parse(row["ProjectManagerId2"].ToString()),
                DesignCustomerId2 = Guid.Parse(row["DesignCustomerId2"].ToString()),
                ConstructionCustomerId2 = Guid.Parse(row["ConstructionCustomerId2"].ToString()),
                SupervisionCustomerId2 = Guid.Parse(row["SupervisionCustomerId2"].ToString()),
                ProjectManagerId3 = Guid.Parse(row["ProjectManagerId3"].ToString()),
                DesignCustomerId3 = Guid.Parse(row["DesignCustomerId3"].ToString()),
                ConstructionCustomerId3 = Guid.Parse(row["ConstructionCustomerId3"].ToString()),
                SupervisionCustomerId3 = Guid.Parse(row["SupervisionCustomerId3"].ToString()),
                ProjectManagerId4 = Guid.Parse(row["ProjectManagerId4"].ToString()),
                DesignCustomerId4 = Guid.Parse(row["DesignCustomerId4"].ToString()),
                ConstructionCustomerId4 = Guid.Parse(row["ConstructionCustomerId4"].ToString()),
                SupervisionCustomerId4 = Guid.Parse(row["SupervisionCustomerId4"].ToString()),
                ProjectManagerId5 = Guid.Parse(row["ProjectManagerId5"].ToString()),
                DesignCustomerId5 = Guid.Parse(row["DesignCustomerId5"].ToString()),
                ConstructionCustomerId5 = Guid.Parse(row["ConstructionCustomerId5"].ToString()),
                SupervisionCustomerId5 = Guid.Parse(row["SupervisionCustomerId5"].ToString()),
                ProjectManagerId6 = Guid.Parse(row["ProjectManagerId6"].ToString()),
                DesignCustomerId6 = Guid.Parse(row["DesignCustomerId6"].ToString()),
                ConstructionCustomerId6 = Guid.Parse(row["ConstructionCustomerId6"].ToString()),
                SupervisionCustomerId6 = Guid.Parse(row["SupervisionCustomerId6"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveProjectDesign(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 项目设计
        /// <summary>
        /// 项目设计
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> DesignDrawing(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingPrintObject addressingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingPrintObject.OrderCode;
                ViewData["CreateDateText"] = addressingPrintObject.CreateDateText;
                ViewData["PlaceName"] = addressingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = addressingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = addressingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = addressingPrintObject.PlaceId;
                ViewData["PlanningId"] = addressingPrintObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["DesignRealName"] = projectTaskEditObject.DesignRealName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveDesignDrawing()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                DesignDate = DateTime.Parse(row["DesignDate"].ToString()),
                DesignFileIdList = row["DesignFileIdList"].ToString().Trim(),
                DesignRealName = row["DesignRealName"].ToString().Trim(),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveDesignDrawing(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 登记逻辑号
        /// <summary>
        /// 登记逻辑号
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> LogicalNumber(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingPrintObject addressingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingPrintObject.OrderCode;
                ViewData["CreateDateText"] = addressingPrintObject.CreateDateText;
                ViewData["PlaceName"] = addressingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = addressingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = addressingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = addressingPrintObject.PlaceId;
                ViewData["PlanningId"] = addressingPrintObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["Mark1"] = projectTaskEditObject.Mark1;
                ViewData["Mark2"] = projectTaskEditObject.Mark2;
                ViewData["Mark3"] = projectTaskEditObject.Mark3;
                ViewData["Mark4"] = projectTaskEditObject.Mark4;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["EngineeringProgressName1"] = projectTaskEditObject.EngineeringProgressName1;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["EngineeringProgressName2"] = projectTaskEditObject.EngineeringProgressName2;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["EngineeringProgressName3"] = projectTaskEditObject.EngineeringProgressName3;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["EngineeringProgressName4"] = projectTaskEditObject.EngineeringProgressName4;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
            }
            return View();
        }

        /// <summary>
        /// 保存逻辑号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveLogicalNumber()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                G2Number = row["G2Number"].ToString().Trim(),
                D2Number = row["D2Number"].ToString().Trim(),
                G3Number = row["G3Number"].ToString().Trim(),
                G4Number = row["G4Number"].ToString().Trim(),
                G5Number = row["G5Number"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveLogicalNumber(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 项目开通
        /// <summary>
        /// 项目开通
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ProjectOpening(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingPrintObject addressingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingPrintObject.OrderCode;
                ViewData["CreateDateText"] = addressingPrintObject.CreateDateText;
                ViewData["PlaceName"] = addressingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = addressingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = addressingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = addressingPrintObject.PlaceId;
                ViewData["PlanningId"] = addressingPrintObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
            }
            return View();
        }

        /// <summary>
        /// 保存项目设计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProjectOpening()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                ProjectDate = DateTime.Parse(row["ProjectDate"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveProjectOpening(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造站指定项目经理及总设单位
        /// <summary>
        /// 改造站指定项目经理及总设单位
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointAreaAndDesignUserR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

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
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["AreaManagerId"] = projectTaskEditObject.AreaManagerId;
                ViewData["GeneralDesignId"] = projectTaskEditObject.GeneralDesignId;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAreaAndDesignUserR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                AreaManagerId = Guid.Parse(row["AreaManagerId"].ToString()),
                GeneralDesignId = Guid.Parse(row["GeneralDesignId"].ToString()),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AppointAreaAndDesignUserR(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造站任务分配
        /// <summary>
        /// 改造站任务分配
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ProjectDesignR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
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
                ViewData["ProjectBeginDate"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["ProjectManagerId1"] = projectTaskEditObject.ProjectManagerId1;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["DesignCustomerId1"] = projectTaskEditObject.DesignCustomerId1;
                ViewData["DesignCustomerName1"] = projectTaskEditObject.DesignCustomerName1;
                ViewData["ConstructionCustomerId1"] = projectTaskEditObject.ConstructionCustomerId1;
                ViewData["ConstructionCustomerName1"] = projectTaskEditObject.ConstructionCustomerName1;
                ViewData["SupervisionCustomerId1"] = projectTaskEditObject.SupervisionCustomerId1;
                ViewData["SupervisionCustomerName1"] = projectTaskEditObject.SupervisionCustomerName1;
                ViewData["ProjectManagerId2"] = projectTaskEditObject.ProjectManagerId2;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["DesignCustomerId2"] = projectTaskEditObject.DesignCustomerId2;
                ViewData["DesignCustomerName2"] = projectTaskEditObject.DesignCustomerName2;
                ViewData["ConstructionCustomerId2"] = projectTaskEditObject.ConstructionCustomerId2;
                ViewData["ConstructionCustomerName2"] = projectTaskEditObject.ConstructionCustomerName2;
                ViewData["SupervisionCustomerId2"] = projectTaskEditObject.SupervisionCustomerId2;
                ViewData["SupervisionCustomerName2"] = projectTaskEditObject.SupervisionCustomerName2;
                ViewData["ProjectManagerId3"] = projectTaskEditObject.ProjectManagerId3;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["DesignCustomerId3"] = projectTaskEditObject.DesignCustomerId3;
                ViewData["DesignCustomerName3"] = projectTaskEditObject.DesignCustomerName3;
                ViewData["ConstructionCustomerId3"] = projectTaskEditObject.ConstructionCustomerId3;
                ViewData["ConstructionCustomerName3"] = projectTaskEditObject.ConstructionCustomerName3;
                ViewData["SupervisionCustomerId3"] = projectTaskEditObject.SupervisionCustomerId3;
                ViewData["SupervisionCustomerName3"] = projectTaskEditObject.SupervisionCustomerName3;
                ViewData["ProjectManagerId4"] = projectTaskEditObject.ProjectManagerId4;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["DesignCustomerId4"] = projectTaskEditObject.DesignCustomerId4;
                ViewData["DesignCustomerName4"] = projectTaskEditObject.DesignCustomerName4;
                ViewData["ConstructionCustomerId4"] = projectTaskEditObject.ConstructionCustomerId4;
                ViewData["ConstructionCustomerName4"] = projectTaskEditObject.ConstructionCustomerName4;
                ViewData["SupervisionCustomerId4"] = projectTaskEditObject.SupervisionCustomerId4;
                ViewData["SupervisionCustomerName4"] = projectTaskEditObject.SupervisionCustomerName4;
                ViewData["ProjectManagerId5"] = projectTaskEditObject.ProjectManagerId5;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerId5"] = projectTaskEditObject.DesignCustomerId5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerId5"] = projectTaskEditObject.ConstructionCustomerId5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerId5"] = projectTaskEditObject.SupervisionCustomerId5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["ProjectManagerId6"] = projectTaskEditObject.ProjectManagerId6;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerId6"] = projectTaskEditObject.DesignCustomerId6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerId6"] = projectTaskEditObject.ConstructionCustomerId6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerId6"] = projectTaskEditObject.SupervisionCustomerId6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
            }
            return View();
        }

        /// <summary>
        /// 保存项目设计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProjectDesignR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                Id1 = Guid.Parse(row["Id1"].ToString()),
                Id2 = Guid.Parse(row["Id2"].ToString()),
                Id3 = Guid.Parse(row["Id3"].ToString()),
                Id4 = Guid.Parse(row["Id4"].ToString()),
                Id5 = Guid.Parse(row["Id5"].ToString()),
                Id6 = Guid.Parse(row["Id6"].ToString()),
                Mark1 = int.Parse(row["Mark1"].ToString()),
                Mark2 = int.Parse(row["Mark2"].ToString()),
                Mark3 = int.Parse(row["Mark3"].ToString()),
                Mark4 = int.Parse(row["Mark4"].ToString()),
                Mark5 = int.Parse(row["Mark5"].ToString()),
                Mark6 = int.Parse(row["Mark6"].ToString()),
                ProjectManagerId1 = Guid.Parse(row["ProjectManagerId1"].ToString()),
                DesignCustomerId1 = Guid.Parse(row["DesignCustomerId1"].ToString()),
                ConstructionCustomerId1 = Guid.Parse(row["ConstructionCustomerId1"].ToString()),
                SupervisionCustomerId1 = Guid.Parse(row["SupervisionCustomerId1"].ToString()),
                ProjectManagerId2 = Guid.Parse(row["ProjectManagerId2"].ToString()),
                DesignCustomerId2 = Guid.Parse(row["DesignCustomerId2"].ToString()),
                ConstructionCustomerId2 = Guid.Parse(row["ConstructionCustomerId2"].ToString()),
                SupervisionCustomerId2 = Guid.Parse(row["SupervisionCustomerId2"].ToString()),
                ProjectManagerId3 = Guid.Parse(row["ProjectManagerId3"].ToString()),
                DesignCustomerId3 = Guid.Parse(row["DesignCustomerId3"].ToString()),
                ConstructionCustomerId3 = Guid.Parse(row["ConstructionCustomerId3"].ToString()),
                SupervisionCustomerId3 = Guid.Parse(row["SupervisionCustomerId3"].ToString()),
                ProjectManagerId4 = Guid.Parse(row["ProjectManagerId4"].ToString()),
                DesignCustomerId4 = Guid.Parse(row["DesignCustomerId4"].ToString()),
                ConstructionCustomerId4 = Guid.Parse(row["ConstructionCustomerId4"].ToString()),
                SupervisionCustomerId4 = Guid.Parse(row["SupervisionCustomerId4"].ToString()),
                ProjectManagerId5 = Guid.Parse(row["ProjectManagerId5"].ToString()),
                DesignCustomerId5 = Guid.Parse(row["DesignCustomerId5"].ToString()),
                ConstructionCustomerId5 = Guid.Parse(row["ConstructionCustomerId5"].ToString()),
                SupervisionCustomerId5 = Guid.Parse(row["SupervisionCustomerId5"].ToString()),
                ProjectManagerId6 = Guid.Parse(row["ProjectManagerId6"].ToString()),
                DesignCustomerId6 = Guid.Parse(row["DesignCustomerId6"].ToString()),
                ConstructionCustomerId6 = Guid.Parse(row["ConstructionCustomerId6"].ToString()),
                SupervisionCustomerId6 = Guid.Parse(row["SupervisionCustomerId6"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveProjectDesignR(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造站项目设计
        /// <summary>
        /// 改造站项目设计
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> DesignDrawingR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["DesignRealName"] = projectTaskEditObject.DesignRealName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
            }
            return View();
        }

        /// <summary>
        /// 保存改造确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveDesignDrawingR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                DesignDate = DateTime.Parse(row["DesignDate"].ToString()),
                DesignFileIdList = row["DesignFileIdList"].ToString().Trim(),
                DesignRealName = row["DesignRealName"].ToString().Trim(),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveDesignDrawingR(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造站登记逻辑号
        /// <summary>
        /// 改造站登记逻辑号
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> LogicalNumberR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["Mark1"] = projectTaskEditObject.Mark1;
                ViewData["Mark2"] = projectTaskEditObject.Mark2;
                ViewData["Mark3"] = projectTaskEditObject.Mark3;
                ViewData["Mark4"] = projectTaskEditObject.Mark4;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["EngineeringProgressName1"] = projectTaskEditObject.EngineeringProgressName1;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["EngineeringProgressName2"] = projectTaskEditObject.EngineeringProgressName2;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["EngineeringProgressName3"] = projectTaskEditObject.EngineeringProgressName3;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["EngineeringProgressName4"] = projectTaskEditObject.EngineeringProgressName4;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
            }
            return View();
        }

        /// <summary>
        /// 保存逻辑号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveLogicalNumberR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                G2Number = row["G2Number"].ToString().Trim(),
                D2Number = row["D2Number"].ToString().Trim(),
                G3Number = row["G3Number"].ToString().Trim(),
                G4Number = row["G4Number"].ToString().Trim(),
                G5Number = row["G5Number"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveLogicalNumberR(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造站项目开通
        /// <summary>
        /// 改造站项目开通
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ProjectOpeningR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
            }
            return View();
        }

        /// <summary>
        /// 保存项目设计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProjectOpeningR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                ProjectDate = DateTime.Parse(row["ProjectDate"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveProjectOpeningR(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造站站点状态变更
        /// <summary>
        /// 改造站站点状态变更
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> PlaceState(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["StateBySelect"] = JsonHelper.Encode(enumService.GetStateEnum());
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
                ViewData["PlaceState"] = remodelingPrintObject.PlaceState;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
            }
            return View();
        }

        /// <summary>
        /// 保存站点状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlaceState()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                PlaceState = int.Parse(row["PlaceState"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SavePlaceState(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造站指定项目经理及总设单位和任务分配
        /// <summary>
        /// 改造站指定项目经理及总设单位和任务分配
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointAreaAndDesignUserAndProjectDesignR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
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
                ViewData["AreaManagerId"] = projectTaskEditObject.AreaManagerId;
                ViewData["GeneralDesignId"] = projectTaskEditObject.GeneralDesignId;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectBeginDate"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["ProjectManagerId1"] = projectTaskEditObject.ProjectManagerId1;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["DesignCustomerId1"] = projectTaskEditObject.DesignCustomerId1;
                ViewData["DesignCustomerName1"] = projectTaskEditObject.DesignCustomerName1;
                ViewData["ConstructionCustomerId1"] = projectTaskEditObject.ConstructionCustomerId1;
                ViewData["ConstructionCustomerName1"] = projectTaskEditObject.ConstructionCustomerName1;
                ViewData["SupervisionCustomerId1"] = projectTaskEditObject.SupervisionCustomerId1;
                ViewData["SupervisionCustomerName1"] = projectTaskEditObject.SupervisionCustomerName1;
                ViewData["ProjectManagerId2"] = projectTaskEditObject.ProjectManagerId2;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["DesignCustomerId2"] = projectTaskEditObject.DesignCustomerId2;
                ViewData["DesignCustomerName2"] = projectTaskEditObject.DesignCustomerName2;
                ViewData["ConstructionCustomerId2"] = projectTaskEditObject.ConstructionCustomerId2;
                ViewData["ConstructionCustomerName2"] = projectTaskEditObject.ConstructionCustomerName2;
                ViewData["SupervisionCustomerId2"] = projectTaskEditObject.SupervisionCustomerId2;
                ViewData["SupervisionCustomerName2"] = projectTaskEditObject.SupervisionCustomerName2;
                ViewData["ProjectManagerId3"] = projectTaskEditObject.ProjectManagerId3;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["DesignCustomerId3"] = projectTaskEditObject.DesignCustomerId3;
                ViewData["DesignCustomerName3"] = projectTaskEditObject.DesignCustomerName3;
                ViewData["ConstructionCustomerId3"] = projectTaskEditObject.ConstructionCustomerId3;
                ViewData["ConstructionCustomerName3"] = projectTaskEditObject.ConstructionCustomerName3;
                ViewData["SupervisionCustomerId3"] = projectTaskEditObject.SupervisionCustomerId3;
                ViewData["SupervisionCustomerName3"] = projectTaskEditObject.SupervisionCustomerName3;
                ViewData["ProjectManagerId4"] = projectTaskEditObject.ProjectManagerId4;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["DesignCustomerId4"] = projectTaskEditObject.DesignCustomerId4;
                ViewData["DesignCustomerName4"] = projectTaskEditObject.DesignCustomerName4;
                ViewData["ConstructionCustomerId4"] = projectTaskEditObject.ConstructionCustomerId4;
                ViewData["ConstructionCustomerName4"] = projectTaskEditObject.ConstructionCustomerName4;
                ViewData["SupervisionCustomerId4"] = projectTaskEditObject.SupervisionCustomerId4;
                ViewData["SupervisionCustomerName4"] = projectTaskEditObject.SupervisionCustomerName4;
                ViewData["ProjectManagerId5"] = projectTaskEditObject.ProjectManagerId5;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerId5"] = projectTaskEditObject.DesignCustomerId5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerId5"] = projectTaskEditObject.ConstructionCustomerId5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerId5"] = projectTaskEditObject.SupervisionCustomerId5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["ProjectManagerId6"] = projectTaskEditObject.ProjectManagerId6;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerId6"] = projectTaskEditObject.DesignCustomerId6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerId6"] = projectTaskEditObject.ConstructionCustomerId6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerId6"] = projectTaskEditObject.SupervisionCustomerId6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
            }
            return View();
        }

        /// <summary>
        /// 保存项目设计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAreaAndDesignUserAndProjectDesignR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                Id1 = Guid.Parse(row["Id1"].ToString()),
                Id2 = Guid.Parse(row["Id2"].ToString()),
                Id3 = Guid.Parse(row["Id3"].ToString()),
                Id4 = Guid.Parse(row["Id4"].ToString()),
                Id5 = Guid.Parse(row["Id5"].ToString()),
                Id6 = Guid.Parse(row["Id6"].ToString()),
                Mark1 = int.Parse(row["Mark1"].ToString()),
                Mark2 = int.Parse(row["Mark2"].ToString()),
                Mark3 = int.Parse(row["Mark3"].ToString()),
                Mark4 = int.Parse(row["Mark4"].ToString()),
                Mark5 = int.Parse(row["Mark5"].ToString()),
                Mark6 = int.Parse(row["Mark6"].ToString()),
                AreaManagerId = Guid.Parse(row["AreaManagerId"].ToString()),
                GeneralDesignId = Guid.Parse(row["GeneralDesignId"].ToString()),
                ProjectManagerId1 = Guid.Parse(row["ProjectManagerId1"].ToString()),
                DesignCustomerId1 = Guid.Parse(row["DesignCustomerId1"].ToString()),
                ConstructionCustomerId1 = Guid.Parse(row["ConstructionCustomerId1"].ToString()),
                SupervisionCustomerId1 = Guid.Parse(row["SupervisionCustomerId1"].ToString()),
                ProjectManagerId2 = Guid.Parse(row["ProjectManagerId2"].ToString()),
                DesignCustomerId2 = Guid.Parse(row["DesignCustomerId2"].ToString()),
                ConstructionCustomerId2 = Guid.Parse(row["ConstructionCustomerId2"].ToString()),
                SupervisionCustomerId2 = Guid.Parse(row["SupervisionCustomerId2"].ToString()),
                ProjectManagerId3 = Guid.Parse(row["ProjectManagerId3"].ToString()),
                DesignCustomerId3 = Guid.Parse(row["DesignCustomerId3"].ToString()),
                ConstructionCustomerId3 = Guid.Parse(row["ConstructionCustomerId3"].ToString()),
                SupervisionCustomerId3 = Guid.Parse(row["SupervisionCustomerId3"].ToString()),
                ProjectManagerId4 = Guid.Parse(row["ProjectManagerId4"].ToString()),
                DesignCustomerId4 = Guid.Parse(row["DesignCustomerId4"].ToString()),
                ConstructionCustomerId4 = Guid.Parse(row["ConstructionCustomerId4"].ToString()),
                SupervisionCustomerId4 = Guid.Parse(row["SupervisionCustomerId4"].ToString()),
                ProjectManagerId5 = Guid.Parse(row["ProjectManagerId5"].ToString()),
                DesignCustomerId5 = Guid.Parse(row["DesignCustomerId5"].ToString()),
                ConstructionCustomerId5 = Guid.Parse(row["ConstructionCustomerId5"].ToString()),
                SupervisionCustomerId5 = Guid.Parse(row["SupervisionCustomerId5"].ToString()),
                ProjectManagerId6 = Guid.Parse(row["ProjectManagerId6"].ToString()),
                DesignCustomerId6 = Guid.Parse(row["DesignCustomerId6"].ToString()),
                ConstructionCustomerId6 = Guid.Parse(row["ConstructionCustomerId6"].ToString()),
                SupervisionCustomerId6 = Guid.Parse(row["SupervisionCustomerId6"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveAreaAndDesignUserAndProjectDesignR(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造站登记逻辑号及项目开通
        /// <summary>
        /// 改造站登记逻辑号及项目开通
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> LogicalNumberAndProjectOpeningR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["Mark1"] = projectTaskEditObject.Mark1;
                ViewData["Mark2"] = projectTaskEditObject.Mark2;
                ViewData["Mark3"] = projectTaskEditObject.Mark3;
                ViewData["Mark4"] = projectTaskEditObject.Mark4;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["EngineeringProgressName1"] = projectTaskEditObject.EngineeringProgressName1;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["EngineeringProgressName2"] = projectTaskEditObject.EngineeringProgressName2;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["EngineeringProgressName3"] = projectTaskEditObject.EngineeringProgressName3;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["EngineeringProgressName4"] = projectTaskEditObject.EngineeringProgressName4;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
            }
            return View();
        }

        /// <summary>
        /// 保存逻辑号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveLogicalNumberAndProjectOpeningR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                G2Number = row["G2Number"].ToString().Trim(),
                D2Number = row["D2Number"].ToString().Trim(),
                G3Number = row["G3Number"].ToString().Trim(),
                G4Number = row["G4Number"].ToString().Trim(),
                G5Number = row["G5Number"].ToString().Trim(),
                ProjectDate = DateTime.Parse(row["ProjectDate"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveLogicalNumberAndProjectOpeningR(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 登记逻辑号及项目开通和站点状态变更
        /// <summary>
        /// 登记逻辑号及项目开通和站点状态变更
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> LogicalNumberAndProjectOpeningAndPlaceStateR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["StateBySelect"] = JsonHelper.Encode(enumService.GetStateEnum());
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
                ViewData["PlaceState"] = remodelingPrintObject.PlaceState;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["Mark1"] = projectTaskEditObject.Mark1;
                ViewData["Mark2"] = projectTaskEditObject.Mark2;
                ViewData["Mark3"] = projectTaskEditObject.Mark3;
                ViewData["Mark4"] = projectTaskEditObject.Mark4;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["EngineeringProgressName1"] = projectTaskEditObject.EngineeringProgressName1;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["EngineeringProgressName2"] = projectTaskEditObject.EngineeringProgressName2;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["EngineeringProgressName3"] = projectTaskEditObject.EngineeringProgressName3;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["EngineeringProgressName4"] = projectTaskEditObject.EngineeringProgressName4;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
            }
            return View();
        }

        /// <summary>
        /// 保存逻辑号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveLogicalNumberAndProjectOpeningAndPlaceStateR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                G2Number = row["G2Number"].ToString().Trim(),
                D2Number = row["D2Number"].ToString().Trim(),
                G3Number = row["G3Number"].ToString().Trim(),
                G4Number = row["G4Number"].ToString().Trim(),
                G5Number = row["G5Number"].ToString().Trim(),
                ProjectDate = DateTime.Parse(row["ProjectDate"].ToString()),
                PlaceState = int.Parse(row["PlaceState"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveLogicalNumberAndProjectOpeningAndPlaceStateR(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 指定项目经理及总设单位(室分)
        /// <summary>
        /// 指定项目经理及总设单位(室分)
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointAreaAndDesignUserID(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

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
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["AreaManagerId"] = projectTaskEditObject.AreaManagerId;
                ViewData["GeneralDesignId"] = projectTaskEditObject.GeneralDesignId;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
            }
            return View();
        }

        #endregion

        #region 任务分配(室分)
        /// <summary>
        /// 任务分配(室分)
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ProjectDesignID(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingPrintObject addressingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingPrintObject.OrderCode;
                ViewData["CreateDateText"] = addressingPrintObject.CreateDateText;
                ViewData["PlaceName"] = addressingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = addressingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = addressingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = addressingPrintObject.PlaceId;
                ViewData["PlanningId"] = addressingPrintObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["Id5"] = projectTaskEditObject.Id5;
                ViewData["Id6"] = projectTaskEditObject.Id6;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectBeginDate"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["ProjectManagerId5"] = projectTaskEditObject.ProjectManagerId5;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerId5"] = projectTaskEditObject.DesignCustomerId5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerId5"] = projectTaskEditObject.ConstructionCustomerId5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerId5"] = projectTaskEditObject.SupervisionCustomerId5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["ProjectManagerId6"] = projectTaskEditObject.ProjectManagerId6;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerId6"] = projectTaskEditObject.DesignCustomerId6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerId6"] = projectTaskEditObject.ConstructionCustomerId6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerId6"] = projectTaskEditObject.SupervisionCustomerId6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
            }
            return View();
        }

        /// <summary>
        /// 保存项目设计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProjectDesignID()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                Id5 = Guid.Parse(row["Id5"].ToString()),
                Id6 = Guid.Parse(row["Id6"].ToString()),
                Mark5 = int.Parse(row["Mark5"].ToString()),
                Mark6 = int.Parse(row["Mark6"].ToString()),
                ProjectManagerId5 = Guid.Parse(row["ProjectManagerId5"].ToString()),
                DesignCustomerId5 = Guid.Parse(row["DesignCustomerId5"].ToString()),
                ConstructionCustomerId5 = Guid.Parse(row["ConstructionCustomerId5"].ToString()),
                SupervisionCustomerId5 = Guid.Parse(row["SupervisionCustomerId5"].ToString()),
                ProjectManagerId6 = Guid.Parse(row["ProjectManagerId6"].ToString()),
                DesignCustomerId6 = Guid.Parse(row["DesignCustomerId6"].ToString()),
                ConstructionCustomerId6 = Guid.Parse(row["ConstructionCustomerId6"].ToString()),
                SupervisionCustomerId6 = Guid.Parse(row["SupervisionCustomerId6"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveProjectDesign(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 项目设计(室分)
        /// <summary>
        /// 项目设计(室分)
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> DesignDrawingID(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingPrintObject addressingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingPrintObject.OrderCode;
                ViewData["CreateDateText"] = addressingPrintObject.CreateDateText;
                ViewData["PlaceName"] = addressingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = addressingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = addressingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = addressingPrintObject.PlaceId;
                ViewData["PlanningId"] = addressingPrintObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["DesignRealName"] = projectTaskEditObject.DesignRealName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
            }
            return View();
        }
        #endregion

        #region 登记逻辑号(室分)
        /// <summary>
        /// 登记逻辑号(室分)
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> LogicalNumberID(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingPrintObject addressingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingPrintObject.OrderCode;
                ViewData["CreateDateText"] = addressingPrintObject.CreateDateText;
                ViewData["PlaceName"] = addressingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = addressingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = addressingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = addressingPrintObject.PlaceId;
                ViewData["PlanningId"] = addressingPrintObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
            }
            return View();
        }
        #endregion

        #region 项目开通(室分)
        /// <summary>
        /// 项目开通(室分)
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ProjectOpeningID(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingPrintObject addressingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingPrintObject.OrderCode;
                ViewData["CreateDateText"] = addressingPrintObject.CreateDateText;
                ViewData["PlaceName"] = addressingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = addressingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = addressingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = addressingPrintObject.PlaceId;
                ViewData["PlanningId"] = addressingPrintObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = addressingPrintObject.FileIdList;
                ViewData["Count"] = addressingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditById(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
            }
            return View();
        }
        #endregion

        #region 改造站指定项目经理及总设单位(室分)
        /// <summary>
        /// 改造站指定项目经理及总设单位(室分)
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointAreaAndDesignUserIDR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

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
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["AreaManagerId"] = projectTaskEditObject.AreaManagerId;
                ViewData["GeneralDesignId"] = projectTaskEditObject.GeneralDesignId;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
            }
            return View();
        }
        #endregion

        #region 改造站任务分配(室分)
        /// <summary>
        /// 改造站任务分配(室分)
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ProjectDesignIDR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["Id5"] = projectTaskEditObject.Id5;
                ViewData["Id6"] = projectTaskEditObject.Id6;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectBeginDate"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["ProjectManagerId5"] = projectTaskEditObject.ProjectManagerId5;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerId5"] = projectTaskEditObject.DesignCustomerId5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerId5"] = projectTaskEditObject.ConstructionCustomerId5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerId5"] = projectTaskEditObject.SupervisionCustomerId5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["ProjectManagerId6"] = projectTaskEditObject.ProjectManagerId6;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerId6"] = projectTaskEditObject.DesignCustomerId6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerId6"] = projectTaskEditObject.ConstructionCustomerId6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerId6"] = projectTaskEditObject.SupervisionCustomerId6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
            }
            return View();
        }

        /// <summary>
        /// 保存项目设计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProjectDesignIDR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                Id5 = Guid.Parse(row["Id5"].ToString()),
                Id6 = Guid.Parse(row["Id6"].ToString()),
                Mark5 = int.Parse(row["Mark5"].ToString()),
                Mark6 = int.Parse(row["Mark6"].ToString()),
                ProjectManagerId5 = Guid.Parse(row["ProjectManagerId5"].ToString()),
                DesignCustomerId5 = Guid.Parse(row["DesignCustomerId5"].ToString()),
                ConstructionCustomerId5 = Guid.Parse(row["ConstructionCustomerId5"].ToString()),
                SupervisionCustomerId5 = Guid.Parse(row["SupervisionCustomerId5"].ToString()),
                ProjectManagerId6 = Guid.Parse(row["ProjectManagerId6"].ToString()),
                DesignCustomerId6 = Guid.Parse(row["DesignCustomerId6"].ToString()),
                ConstructionCustomerId6 = Guid.Parse(row["ConstructionCustomerId6"].ToString()),
                SupervisionCustomerId6 = Guid.Parse(row["SupervisionCustomerId6"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveProjectDesignR(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造站项目设计(室分)
        /// <summary>
        /// 改造站项目设计(室分)
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> DesignDrawingIDR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["DesignRealName"] = projectTaskEditObject.DesignRealName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
            }
            return View();
        }

        #endregion

        #region 改造站登记逻辑号(室分)
        /// <summary>
        /// 改造站登记逻辑号(室分)
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> LogicalNumberIDR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
            }
            return View();
        }

        #endregion

        #region 改造站项目开通(室分)
        /// <summary>
        /// 改造站项目开通(室分)
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ProjectOpeningIDR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
            }
            return View();
        }

        #endregion

        #region 改造站站点状态变更(室分)
        /// <summary>
        /// 改造站站点状态变更(室分)
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> PlaceStateID(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["StateBySelect"] = JsonHelper.Encode(enumService.GetStateEnum());
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
                ViewData["PlaceState"] = remodelingPrintObject.PlaceState;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
            }
            return View();
        }

        #endregion

        #region 改造站指定项目经理及总设单位和任务分配(室分)
        /// <summary>
        /// 改造站指定项目经理及总设单位和任务分配(室分)
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointAreaAndDesignUserAndProjectDesignIDR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["Id5"] = projectTaskEditObject.Id5;
                ViewData["Id6"] = projectTaskEditObject.Id6;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["AreaManagerId"] = projectTaskEditObject.AreaManagerId;
                ViewData["GeneralDesignId"] = projectTaskEditObject.GeneralDesignId;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectBeginDate"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["ProjectManagerId5"] = projectTaskEditObject.ProjectManagerId5;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerId5"] = projectTaskEditObject.DesignCustomerId5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerId5"] = projectTaskEditObject.ConstructionCustomerId5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerId5"] = projectTaskEditObject.SupervisionCustomerId5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["ProjectManagerId6"] = projectTaskEditObject.ProjectManagerId6;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerId6"] = projectTaskEditObject.DesignCustomerId6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerId6"] = projectTaskEditObject.ConstructionCustomerId6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerId6"] = projectTaskEditObject.SupervisionCustomerId6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
            }
            return View();
        }

        /// <summary>
        /// 保存项目设计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAreaAndDesignUserAndProjectDesignIDR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectTaskEditObject projectTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            projectTaskEditObject = new ProjectTaskEditObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                Id5 = Guid.Parse(row["Id5"].ToString()),
                Id6 = Guid.Parse(row["Id6"].ToString()),
                Mark5 = int.Parse(row["Mark5"].ToString()),
                Mark6 = int.Parse(row["Mark6"].ToString()),
                AreaManagerId = Guid.Parse(row["AreaManagerId"].ToString()),
                GeneralDesignId = Guid.Parse(row["GeneralDesignId"].ToString()),
                ProjectManagerId5 = Guid.Parse(row["ProjectManagerId5"].ToString()),
                DesignCustomerId5 = Guid.Parse(row["DesignCustomerId5"].ToString()),
                ConstructionCustomerId5 = Guid.Parse(row["ConstructionCustomerId5"].ToString()),
                SupervisionCustomerId5 = Guid.Parse(row["SupervisionCustomerId5"].ToString()),
                ProjectManagerId6 = Guid.Parse(row["ProjectManagerId6"].ToString()),
                DesignCustomerId6 = Guid.Parse(row["DesignCustomerId6"].ToString()),
                ConstructionCustomerId6 = Guid.Parse(row["ConstructionCustomerId6"].ToString()),
                SupervisionCustomerId6 = Guid.Parse(row["SupervisionCustomerId6"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveAreaAndDesignUserAndProjectDesignR(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造站登记逻辑号及项目开通(室分)
        /// <summary>
        /// 改造站登记逻辑号及项目开通(室分)
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> LogicalNumberAndProjectOpeningIDR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
            }
            return View();
        }

        #endregion

        #region 登记逻辑号及项目开通和站点状态变更(室分)
        /// <summary>
        /// 登记逻辑号及项目开通和站点状态变更(室分)
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> LogicalNumberAndProjectOpeningAndPlaceStateIDR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["StateBySelect"] = JsonHelper.Encode(enumService.GetStateEnum());
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                RemodelingPrintObject remodelingPrintObject = await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingPrintById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = remodelingPrintObject.OrderCode;
                ViewData["CreateDateText"] = remodelingPrintObject.CreateDateText;
                ViewData["PlaceName"] = remodelingPrintObject.PlaceName;
                ViewData["PlaceCategoryName"] = remodelingPrintObject.PlaceCategoryName;
                ViewData["ProposedNetwork"] = remodelingPrintObject.ProposedNetwork;
                ViewData["PlaceId"] = remodelingPrintObject.PlaceId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["FileIdList"] = remodelingPrintObject.FileIdList;
                ViewData["Count"] = remodelingPrintObject.Count;
                ViewData["PlaceState"] = remodelingPrintObject.PlaceState;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskEditByRemodelingId(id));
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["ProjectTaskId"] = projectTaskEditObject.Id;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["DesignFileIdList"] = projectTaskEditObject.DesignFileIdList;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["Mark1"] = projectTaskEditObject.Mark1;
                ViewData["Mark2"] = projectTaskEditObject.Mark2;
                ViewData["Mark3"] = projectTaskEditObject.Mark3;
                ViewData["Mark4"] = projectTaskEditObject.Mark4;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["EngineeringProgressName1"] = projectTaskEditObject.EngineeringProgressName1;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["EngineeringProgressName2"] = projectTaskEditObject.EngineeringProgressName2;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["EngineeringProgressName3"] = projectTaskEditObject.EngineeringProgressName3;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["EngineeringProgressName4"] = projectTaskEditObject.EngineeringProgressName4;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["G2Number"] = projectTaskEditObject.G2Number;
                ViewData["D2Number"] = projectTaskEditObject.D2Number;
                ViewData["G3Number"] = projectTaskEditObject.G3Number;
                ViewData["G4Number"] = projectTaskEditObject.G4Number;
                ViewData["G5Number"] = projectTaskEditObject.G5Number;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
            }
            return View();
        }

        #endregion
    }
}