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
using Newtonsoft.Json.Linq;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 基站建维控制器
    /// </summary>
    [AuthorizeFilter]
    public class BaseStationBMController : BaseController
    {
        private const int PROFESSION = 1;

        #region 基站建设申请

        /// <summary>
        /// 基站建设申请
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PlanningApply()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumPlanningAdviceList = enumService.GetPlanningAdviceEnum();
            IList<Dictionary<string, string>> enumDoStateList = enumService.GetDoStateEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["PlanningAdvice"] = JsonHelper.Encode(enumPlanningAdviceList);
            ViewData["DoState"] = JsonHelper.Encode(enumDoStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumImportanceList.Insert(0, nullDict);
            //ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据建设申请Id获取建设申请
        /// </summary>
        /// <param name="id">建设申请Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningApplyById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlanningApplyById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页建设申请列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlanningApplysPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["Issued"] == null)
            {
                throw new ArgumentNullException("Issued");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }

            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlanningApplysPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlanningName"].Trim(), Guid.Parse(Request["AreaId"]),
                    Guid.Parse(Request["ReseauId"]), int.Parse(Request["Issued"]), Guid.Parse(Request["CreateUserId"]), PROFESSION));
            }
        }

        /// <summary>
        /// 保存建设申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlanningApply()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlanningApplyMaintObject planningApplyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                planningApplyMaintObject = new PlanningApplyMaintObject()
                {
                    Id = Guid.Empty,
                    PlanningName = row["PlanningName"].ToString().Trim(),
                    Profession = PROFESSION,
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    Importance = int.Parse(row["Importance"].ToString()),
                    DetailedAddress = row["DetailedAddress"].ToString(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                planningApplyMaintObject = new PlanningApplyMaintObject()
                {
                    Id = id,
                    PlanningName = row["PlanningName"].ToString().Trim(),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    Importance = int.Parse(row["Importance"].ToString()),
                    DetailedAddress = row["DetailedAddress"].ToString(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdatePlanningApply(planningApplyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }


        /// <summary>
        /// 删除建设申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePlanningApplys()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlanningApplyMaintObject> planningApplyMaintObjects = new List<PlanningApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningApplyMaintObjects.Add(new PlanningApplyMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemovePlanningApplys(planningApplyMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        /// <summary>
        /// 指定网优人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AppointPlanningUser()
        {
            if (Request["PlanningUserId"] == null)
            {
                throw new ArgumentNullException("PlanningUserId");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            Guid planningUserId = Guid.Parse(Request["PlanningUserId"]);
            IList<PlanningApplyMaintObject> planningApplyMaintObjects = new List<PlanningApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningApplyMaintObjects.Add(new PlanningApplyMaintObject() { Id = id, PlanningUserId = planningUserId, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AppointPlanningUser(planningApplyMaintObjects));
            }
            return this.Sucess("指定网优人员成功");
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IssuePlanningApply()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlanningApplyMaintObject> planningApplyMaintObjects = new List<PlanningApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningApplyMaintObjects.Add(new PlanningApplyMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.IssuePlanningApply(planningApplyMaintObjects));
            }
            return this.Sucess("提交成功");
        }

        /// <summary>
        /// 取消提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelIssuePlanningApply()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlanningApplyMaintObject> planningApplyMaintObjects = new List<PlanningApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningApplyMaintObjects.Add(new PlanningApplyMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.CancelIssuePlanningApply(planningApplyMaintObjects));
            }
            return this.Sucess("取消提交成功");
        }

        /// <summary>
        /// 保存建设申请单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlanningApplyHeader()
        {
            if (Request["dataHeader"] == null)
            {
                throw new ArgumentNullException("dataHeader");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> rowHeader = (Dictionary<string, object>)JsonHelper.Decode(Request["dataHeader"]);
            PlanningApplyHeaderMaintObject planningApplyHeaderMaintObject = null;
            Guid headerId = Guid.Parse((rowHeader["HeaderId"] == null || rowHeader["HeaderId"].ToString() == "" ? Guid.Empty : rowHeader["HeaderId"]).ToString());
            if (headerId == Guid.Empty)
            {
                planningApplyHeaderMaintObject = new PlanningApplyHeaderMaintObject()
                {
                    Id = Guid.Empty,
                    Title = rowHeader["Title"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlanningApplyMaintObject> planningApplyMaintObjects = new List<PlanningApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningApplyMaintObjects.Add(new PlanningApplyMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SavePlanningApplyHeader(planningApplyHeaderMaintObject, planningApplyMaintObjects));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 根据基站建设申请单获取基站建设申请单信息
        /// </summary>
        /// <param name="id">基站建设申请单Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningApplyHeaderById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlanningApplyHeaderService> proxy = new ServiceProxy<IPlanningApplyHeaderService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlanningApplyHeaderById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据基站建设申请单获取相关联的基站建设申请
        /// </summary>
        /// <param name="id">基站建设申请单Id</param>
        /// <returns></returns>
        public async Task<string> GetPlanningApplysByHeaderId()
        {
            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlanningApplysByHeaderId(Guid.Parse(Request["ParentId"])));
            }
        }

        /// <summary>
        /// 保存建设申请单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlanningApplyHeaderEdit()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlanningApplyHeaderMaintObject planningApplyHeaderMaintObject = null;
            Guid id = Guid.Parse((row["HeaderEditId"] == null || row["HeaderEditId"].ToString() == "" ? Guid.Empty : row["HeaderEditId"]).ToString());
            if (id != Guid.Empty)
            {
                planningApplyHeaderMaintObject = new PlanningApplyHeaderMaintObject()
                {
                    Id = id,
                    Title = row["TitleEdit"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPlanningApplyHeaderService> proxy = new ServiceProxy<IPlanningApplyHeaderService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdatePlanningApplyHeader(planningApplyHeaderMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 取消关联基站建设申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePlanningApplyDetail()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlanningApplyMaintObject> planningApplyMaintObjects = new List<PlanningApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningApplyMaintObjects.Add(new PlanningApplyMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemovePlanningApplyDetail(planningApplyMaintObjects));
            }
            return this.Sucess("取消关联成功");
        }

        /// <summary>
        /// 删除建设申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePlanningApplyHeaders()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlanningApplyMaintObject> planningApplyMaintObjects = new List<PlanningApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningApplyMaintObjects.Add(new PlanningApplyMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemovePlanningApplyHeaders(planningApplyMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }
        #endregion

        #region 待处理基站建设申请

        /// <summary>
        /// 待处理基站建设申请
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PlanningApplyWait()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumPlanningAdviceList = enumService.GetPlanningAdviceEnum();
            IList<Dictionary<string, string>> enumDoStateList = enumService.GetDoStateEnum();

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["PlanningAdvice"] = JsonHelper.Encode(enumPlanningAdviceList);
            ViewData["DoState"] = JsonHelper.Encode(enumDoStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumDoStateList.Insert(0, allDict);
            ViewData["DoStateByAll"] = JsonHelper.Encode(enumDoStateList);

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumImportanceList.Insert(0, nullDict);
            //ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页建设申请列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlanningApplysWaitPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["DoState"] == null)
            {
                throw new ArgumentNullException("DoState");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }

            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlanningApplysWaitPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlanningName"].Trim(), Guid.Parse(Request["AreaId"]),
                    Guid.Parse(Request["ReseauId"]), int.Parse(Request["DoState"]), Guid.Parse(Request["CreateUserId"]), this.UserId, PROFESSION));
            }
        }

        /// <summary>
        /// 指定规划意见
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlanningAdvice()
        {
            if (Request["PlanningAdvice"] == null)
            {
                throw new ArgumentNullException("PlanningAdvice");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            int planningAdvice = int.Parse(Request["PlanningAdvice"]);
            IList<PlanningApplyMaintObject> planningApplyMaintObjects = new List<PlanningApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningApplyMaintObjects.Add(new PlanningApplyMaintObject() { Id = id, PlanningAdvice = planningAdvice, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningApplyService> proxy = new ServiceProxy<IPlanningApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SavePlanningAdvice(planningApplyMaintObjects));
            }
            return this.Sucess("指定规划意见成功");
        }
        #endregion

        #region 运营商规划

        /// <summary>
        /// 运营商基站规划
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OperatorsPlanning()
        {
            ViewData["CompanyNature"] = this.CompanyNature;
            ViewData["CompanyId"] = this.CompanyId;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumUrgencyList = enumService.GetUrgencyEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Urgency"] = JsonHelper.Encode(enumUrgencyList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumUrgencyList.Insert(0, allDict);

            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["UrgencyByAll"] = JsonHelper.Encode(enumUrgencyList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanysByNature(2));
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据运营商规划Id获取运营商规划
        /// </summary>
        /// <param name="id">运营商规划Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetOperatorsPlanningById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IOperatorsPlanningService> proxy = new ServiceProxy<IOperatorsPlanningService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsPlanningById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页运营商规划列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOperatorsPlanningsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PlanningCode"] == null)
            {
                throw new ArgumentNullException("PlanningCode");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["Urgency"] == null)
            {
                throw new ArgumentNullException("Urgency");
            }
            if (Request["Solved"] == null)
            {
                throw new ArgumentNullException("Solved");
            }
            if (Request["ToShared"] == null)
            {
                throw new ArgumentNullException("ToShared");
            }

            using (ServiceProxy<IOperatorsPlanningService> proxy = new ServiceProxy<IOperatorsPlanningService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsPlanningsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlanningCode"].Trim(), Request["PlanningName"].Trim(),
                    Guid.Parse(Request["CompanyId"]), PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), int.Parse(Request["Urgency"]),
                    int.Parse(Request["Solved"]), int.Parse(Request["ToShared"])));
            }
        }

        /// <summary>
        /// 获取分页运营商规划列表，用于选择运营商规划
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOperatorsPlanningsPageBySelect()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlanningCode"] == null)
            {
                throw new ArgumentNullException("PlanningCode");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["Urgency"] == null)
            {
                throw new ArgumentNullException("Urgency");
            }

            using (ServiceProxy<IOperatorsPlanningService> proxy = new ServiceProxy<IOperatorsPlanningService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsPlanningsPageBySelect(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlanningCode"].Trim(), Request["PlanningName"].Trim(), Guid.Parse(Request["CompanyId"]), PROFESSION, Guid.Parse(Request["PlaceCategoryId"]),
                    Guid.Parse(Request["AreaId"]), int.Parse(Request["Urgency"])));
            }
        }

        /// <summary>
        /// 根据条件获取指定距离内的运营商规划列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOperatorsPlanningsByDistance()
        {
            if (Request["OperatorsPlanningId"] == null)
            {
                throw new ArgumentNullException("OperatorsPlanningId");
            }
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }
            if (Request["Distance"] == null)
            {
                throw new ArgumentNullException("Distance");
            }

            using (ServiceProxy<IOperatorsPlanningService> proxy = new ServiceProxy<IOperatorsPlanningService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsPlanningsByDistance(Guid.Parse(Request["OperatorsPlanningId"]), Guid.Parse(Request["PlanningId"]), PROFESSION, decimal.Parse(Request["Distance"])));
            }
        }

        /// <summary>
        /// 根据规划获取关联的运营商规划列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOperatorsPlanningsByPlanning()
        {
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }

            using (ServiceProxy<IOperatorsPlanningService> proxy = new ServiceProxy<IOperatorsPlanningService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsPlanningsByPlanning(Guid.Parse(Request["PlanningId"])));
            }
        }

        /// <summary>
        /// 保存运营商规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveOperatorsPlanning()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            OperatorsPlanningMaintObject operatorsPlanningMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                operatorsPlanningMaintObject = new OperatorsPlanningMaintObject()
                {
                    Id = Guid.Empty,
                    PlanningName = row["PlanningName"].ToString().Trim(),
                    Profession = PROFESSION,
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    AreaId = Guid.Parse(row["AreaId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    AntennaHeight = row["AntennaHeight"].ToString().Trim() == "" ? 0 : decimal.Parse(row["AntennaHeight"].ToString()),
                    PoleNumber = row["PoleNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["PoleNumber"].ToString()),
                    CabinetNumber = row["CabinetNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["CabinetNumber"].ToString()),
                    Urgency = int.Parse(row["Urgency"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    CompanyId = this.CompanyId,
                    CreateUserId = this.UserId,
                    CurrentCompanyNature = this.CompanyNature
                };
            }
            else
            {
                operatorsPlanningMaintObject = new OperatorsPlanningMaintObject()
                {
                    Id = id,
                    PlanningName = row["PlanningName"].ToString().Trim(),
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    AreaId = Guid.Parse(row["AreaId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    AntennaHeight = row["AntennaHeight"].ToString().Trim() == "" ? 0 : decimal.Parse(row["AntennaHeight"].ToString()),
                    PoleNumber = row["PoleNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["PoleNumber"].ToString()),
                    CabinetNumber = row["CabinetNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["CabinetNumber"].ToString()),
                    Urgency = int.Parse(row["Urgency"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IOperatorsPlanningService> proxy = new ServiceProxy<IOperatorsPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateOperatorsPlanning(operatorsPlanningMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 关联运营商规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Associate()
        {
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }
            if (Request["PlanningCreateUserId"] == null)
            {
                throw new ArgumentNullException("PlanningCreateUserId");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects = new List<OperatorsPlanningMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    operatorsPlanningMaintObjects.Add(new OperatorsPlanningMaintObject() { Id = id, PlanningId = Guid.Parse(row["PlanningId"].ToString()) });
                }
            }
            using (ServiceProxy<IOperatorsPlanningService> proxy = new ServiceProxy<IOperatorsPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.Associate(Guid.Parse(Request["PlanningId"]), Guid.Parse(Request["PlanningCreateUserId"]), this.UserId, operatorsPlanningMaintObjects));
            }
            return this.Sucess("数据关联成功");
        }

        /// <summary>
        /// 删除运营商规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveOperatorsPlannings()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects = new List<OperatorsPlanningMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    operatorsPlanningMaintObjects.Add(new OperatorsPlanningMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IOperatorsPlanningService> proxy = new ServiceProxy<IOperatorsPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveOperatorsPlannings(operatorsPlanningMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 基站规划

        /// <summary>
        /// 基站规划
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Planning()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumAddressingStateList = enumService.GetAddressingStateEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["AddressingState"] = JsonHelper.Encode(enumAddressingStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumImportanceList.Insert(0, allDict);
            enumAddressingStateList.Insert(0, allDict);

            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["ImportanceByAll"] = JsonHelper.Encode(enumImportanceList);
            ViewData["AddressingStateByAll"] = JsonHelper.Encode(enumAddressingStateList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                IList<PlaceOwnerSelectObject> placeOwnerSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceOwners());
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "全部" });
                ViewData["PlaceOwnersByAll"] = JsonHelper.Encode(placeOwnerSelectObjects);
                placeOwnerSelectObjects.RemoveAt(0);
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "请选择" });
                ViewData["PlaceOwnersBySelect"] = JsonHelper.Encode(placeOwnerSelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanysByNature(2));
                companySelectObjects.Insert(0, new CompanySelectObject() { Id = Guid.Empty, CompanyName = "全部" });
                ViewData["CompanysByAll"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据规划Id获取规划
        /// </summary>
        /// <param name="id">规划Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlanningById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据规划Id获取规划(移动端)
        /// </summary>
        /// <param name="id">规划Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningByIdMobile(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                string header = "http://211.149.154.229/PDCMSFiles";
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlanningByIdMobile(id, header)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页规划列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlanningsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PlanningCode"] == null)
            {
                throw new ArgumentNullException("PlanningCode");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["Importance"] == null)
            {
                throw new ArgumentNullException("Importance");
            }
            if (Request["Issued"] == null)
            {
                throw new ArgumentNullException("Issued");
            }
            if (Request["AddressingState"] == null)
            {
                throw new ArgumentNullException("AddressingState");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }

            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlanningsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlanningCode"].Trim(), Request["PlanningName"].Trim(),
                    PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["Importance"]),
                    int.Parse(Request["Issued"]), int.Parse(Request["AddressingState"]), Guid.Parse(Request["CreateUserId"])));
            }
        }

        /// <summary>
        /// 保存规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlanning()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlanningMaintObject planningMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                planningMaintObject = new PlanningMaintObject()
                {
                    Id = Guid.Empty,
                    PlanningName = row["PlanningName"].ToString().Trim(),
                    Profession = PROFESSION,
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    ProposedNetwork = row["ProposedNetwork"].ToString().Trim(),
                    OptionalAddress = row["OptionalAddress"].ToString().Trim(),
                    Importance = int.Parse(row["Importance"].ToString()),
                    PlaceOwner = Guid.Parse(row["PlaceOwner"].ToString()),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                planningMaintObject = new PlanningMaintObject()
                {
                    Id = id,
                    PlanningName = row["PlanningName"].ToString().Trim(),
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    ProposedNetwork = row["ProposedNetwork"].ToString().Trim(),
                    OptionalAddress = row["OptionalAddress"].ToString().Trim(),
                    Importance = int.Parse(row["Importance"].ToString()),
                    PlaceOwner = Guid.Parse(row["PlaceOwner"].ToString()),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdatePlanning(planningMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePlannings()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlanningMaintObject> planningMaintObjects = new List<PlanningMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningMaintObjects.Add(new PlanningMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemovePlannings(planningMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        /// <summary>
        /// 下达规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Issue()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlanningMaintObject> planningMaintObjects = new List<PlanningMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningMaintObjects.Add(new PlanningMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.Issue(planningMaintObjects));
            }
            return this.Sucess("下达规划成功");
        }

        /// <summary>
        /// 取消下达规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelIssue()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PlanningMaintObject> planningMaintObjects = new List<PlanningMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningMaintObjects.Add(new PlanningMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.CancelIssue(planningMaintObjects));
            }
            return this.Sucess("取消下达规划成功");
        }

        /// <summary>
        /// 获取分页规划列表(移动端)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlanningsMobile()
        {
            string professionList = "0";
            if (Request["ProfessionList"] != null && Request["ProfessionList"] != "")
            {
                professionList = Request["ProfessionList"].Trim();
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }

            string professionListSql = "";
            string[] profession = professionList.Split(',');
            for (int i = 0; i < profession.Length; i++)
            {
                professionListSql += "select '" + profession[i] + "'";
                if (i != profession.Length - 1)
                {
                    professionListSql += " union ";
                }
            }

            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlanningsMobile(1, int.Parse(Request["PageSize"]), professionListSql, Request["PlanningName"].Trim(), this.CompanyId));
            }
        }

        /// <summary>
        /// 获取分页规划列表(移动端)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlanningsPageMobile()
        {
            string professionList = "0";
            if (Request["ProfessionList"] != null && Request["ProfessionList"] != "")
            {
                professionList = Request["ProfessionList"].Trim();
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["Distance"] == null)
            {
                throw new ArgumentNullException("Distance");
            }

            string professionListSql = "";
            string[] profession = professionList.Split(',');
            for (int i = 0; i < profession.Length; i++)
            {
                professionListSql += "select '" + profession[i] + "'";
                if (i != profession.Length - 1)
                {
                    professionListSql += " union ";
                }
            }

            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlanningsPageMobile(1, int.Parse(Request["PageSize"]), professionListSql, decimal.Parse(Request["Lng"]), decimal.Parse(Request["Lat"]), decimal.Parse(Request["Distance"]), this.CompanyId));
            }
        }

        /// <summary>
        /// 保存规划(移动端)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SavePlanningMobile()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlanningMaintObject planningMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());

            string[] arr = new string[1];
            arr[0] = "";
            string[] arrString = new string[] { };
            if (row["Base64String"].ToString().Trim() != "")
            {
                ArrayList al = (ArrayList)JsonHelper.Decode(row["Base64String"].ToString().Trim());
                arrString = (string[])al.ToArray(typeof(string));
            }
            planningMaintObject = new PlanningMaintObject()
            {
                Id = id,
                PlanningName = row["PlanningName"].ToString().Trim(),
                PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                Lng = decimal.Parse(row["Lng"].ToString()),
                Lat = decimal.Parse(row["Lat"].ToString()),
                Importance = int.Parse(row["Importance"].ToString()),
                ProposedNetwork = row["ProposedNetwork"].ToString().Trim(),
                OptionalAddress = row["OptionalAddress"].ToString().Trim(),
                DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                Base64String = row["Base64String"].ToString().Trim() != "" ? arrString : arr,
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SavePlanningMobile(planningMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 更新规划点方位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SavePlanningPositionMobile()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlanningMaintObject planningMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());

            planningMaintObject = new PlanningMaintObject()
            {
                Id = id,
                Lng = decimal.Parse(row["Lng"].ToString()),
                Lat = decimal.Parse(row["Lat"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SavePlanningPositionMobile(planningMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 租赁任务分配

        /// <summary>
        /// 租赁任务分配
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AddressingUser()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumAddressingStateList = enumService.GetAddressingStateEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["AddressingState"] = JsonHelper.Encode(enumAddressingStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumImportanceList.Insert(0, allDict);
            enumAddressingStateList.Insert(0, allDict);

            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["ImportanceByAll"] = JsonHelper.Encode(enumImportanceList);
            ViewData["AddressingStateByAll"] = JsonHelper.Encode(enumAddressingStateList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                IList<PlaceOwnerSelectObject> placeOwnerSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceOwners());
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "全部" });
                ViewData["PlaceOwnersByAll"] = JsonHelper.Encode(placeOwnerSelectObjects);
                placeOwnerSelectObjects.RemoveAt(0);
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "请选择" });
                ViewData["PlaceOwnersBySelect"] = JsonHelper.Encode(placeOwnerSelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanysByNature(2));
                companySelectObjects.Insert(0, new CompanySelectObject() { Id = Guid.Empty, CompanyName = "全部" });
                ViewData["CompanysByAll"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页租赁任务分配列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAddressingUsersPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PlanningCode"] == null)
            {
                throw new ArgumentNullException("PlanningCode");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["Importance"] == null)
            {
                throw new ArgumentNullException("Importance");
            }
            if (Request["IsAppoint"] == null)
            {
                throw new ArgumentNullException("IsAppoint");
            }
            if (Request["AddressingState"] == null)
            {
                throw new ArgumentNullException("AddressingState");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }

            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetAddressingUsersPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlanningCode"].Trim(), Request["PlanningName"].Trim(),
                    PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["Importance"]),
                    int.Parse(Request["IsAppoint"]), int.Parse(Request["AddressingState"]), Guid.Parse(Request["CreateUserId"])));
            }
        }

        /// <summary>
        /// 保存规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlanningAddressing()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlanningMaintObject planningMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            planningMaintObject = new PlanningMaintObject()
            {
                Id = id,
                PlanningName = row["PlanningName"].ToString().Trim(),
                PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                Lng = decimal.Parse(row["Lng"].ToString()),
                Lat = decimal.Parse(row["Lat"].ToString()),
                DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                Remarks = row["Remarks"].ToString().Trim(),
                ProposedNetwork = row["ProposedNetwork"].ToString().Trim(),
                OptionalAddress = row["OptionalAddress"].ToString().Trim(),
                Importance = int.Parse(row["Importance"].ToString()),
                PlaceOwner = Guid.Parse(row["PlaceOwner"].ToString()),
                FileIdList = row["FileIdList"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.UpdatePlanningAddressing(planningMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 指定租赁人
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AppointAddressingUser()
        {
            if (Request["AddressingUserId"] == null)
            {
                throw new ArgumentNullException("AddressingUserId");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            Guid addressingUserId = Guid.Parse(Request["AddressingUserId"]);
            IList<PlanningMaintObject> planningMaintObjects = new List<PlanningMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    planningMaintObjects.Add(new PlanningMaintObject() { Id = id, AddressingUserId = addressingUserId, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AppointAddressingUser(planningMaintObjects));
            }
            return this.Sucess("指定租赁人成功");
        }
        #endregion

        #region 运营商需求确认

        /// <summary>
        /// 运营商需求确认
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OperatorsConfirm()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumDemandList = enumService.GetDemandEnum();
            ViewData["Demand"] = JsonHelper.Encode(enumDemandList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumDemandList.Insert(0, allDict);
            ViewData["DemandByAll"] = JsonHelper.Encode(enumDemandList);
            ViewData["DemandByConfirm"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页运营商需求确认列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOperatorsConfirmDetailsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlanningCode"] == null)
            {
                throw new ArgumentNullException("PlanningCode");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["Demand"] == null)
            {
                throw new ArgumentNullException("Demand");
            }

            using (ServiceProxy<IOperatorsConfirmService> proxy = new ServiceProxy<IOperatorsConfirmService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsConfirmDetailsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlanningCode"].Trim(), Request["PlanningName"].Trim(), PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]),
                    Guid.Parse(Request["ReseauId"]), int.Parse(Request["Demand"]), this.CompanyId));
            }
        }

        /// <summary>
        /// 需求确认
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DemandConfirm()
        {
            if (Request["Demand"] == null)
            {
                throw new ArgumentNullException("Demand");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<OperatorsConfirmDetailMaintObject> operatorsConfirmDetailMaintObjects = new List<OperatorsConfirmDetailMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    operatorsConfirmDetailMaintObjects.Add(new OperatorsConfirmDetailMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IOperatorsConfirmService> proxy = new ServiceProxy<IOperatorsConfirmService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.Confirm(this.UserId, this.CompanyId, this.CompanyNature, int.Parse(Request["Demand"]), operatorsConfirmDetailMaintObjects));
            }
            return this.Sucess("需求确认成功");
        }

        #endregion

        #region 寻址确认

        /// <summary>
        /// 寻址确认
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Addressing()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumAddressingStateList = enumService.GetAddressingStateEnum();

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["AddressingState"] = JsonHelper.Encode(enumAddressingStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumImportanceList.Insert(0, allDict);
            enumAddressingStateList.Insert(0, allDict);

            ViewData["ImportanceByAll"] = JsonHelper.Encode(enumImportanceList);
            ViewData["AddressingStateByAll"] = JsonHelper.Encode(enumAddressingStateList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                IList<PlaceOwnerSelectObject> placeOwnerSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceOwners());
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "请选择" });
                ViewData["PlaceOwnersBySelect"] = JsonHelper.Encode(placeOwnerSelectObjects);
            }
            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                IList<DepartmentSelectObject> departmentSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedDepartments(this.CompanyId));
                departmentSelectObjects.Insert(0, new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "请选择" });
                ViewData["AddressingDepartmentsBySelect"] = JsonHelper.Encode(departmentSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据寻址确认Id和规划Id获取寻址确认
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetAddressingById()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetAddressingById(Guid.Parse(Request["Id"]), Guid.Parse(Request["PlanningId"]))), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页寻址确认列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAddressingsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PlanningCode"] == null)
            {
                throw new ArgumentNullException("PlanningCode");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["Importance"] == null)
            {
                throw new ArgumentNullException("Importance");
            }
            if (Request["AddressingState"] == null)
            {
                throw new ArgumentNullException("AddressingState");
            }
            if (Request["AddressingUserId"] == null)
            {
                throw new ArgumentNullException("AddressingUserId");
            }

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetAddressingsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlanningCode"].Trim(), Request["PlanningName"].Trim(),
                    PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["Importance"]),
                    int.Parse(Request["AddressingState"]), Guid.Parse(Request["AddressingUserId"])));
            }
        }

        /// <summary>
        /// 保存寻址确认
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAddressing()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            AddressingMaintObject addressingMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                addressingMaintObject = new AddressingMaintObject()
                {
                    Id = Guid.Empty,
                    PlanningId = Guid.Parse(row["PlanningId"].ToString()),
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    Importance = int.Parse(row["Importance"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    PlaceOwner = Guid.Parse(row["PlaceOwner"].ToString()),
                    ProposedNetwork = row["ProposedNetwork"].ToString().Trim(),
                    AddressingDepartmentId = Guid.Parse(row["AddressingDepartmentId"].ToString()),
                    AddressingRealName = row["AddressingRealName"].ToString().Trim(),
                    OwnerName = row["OwnerName"].ToString().Trim(),
                    OwnerContact = row["OwnerContact"].ToString().Trim(),
                    OwnerPhoneNumber = row["OwnerPhoneNumber"].ToString().Trim(),
                    DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                addressingMaintObject = new AddressingMaintObject()
                {
                    Id = id,
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    Importance = int.Parse(row["Importance"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    PlaceOwner = Guid.Parse(row["PlaceOwner"].ToString()),
                    ProposedNetwork = row["ProposedNetwork"].ToString().Trim(),
                    AddressingDepartmentId = Guid.Parse(row["AddressingDepartmentId"].ToString()),
                    AddressingRealName = row["AddressingRealName"].ToString().Trim(),
                    OwnerName = row["OwnerName"].ToString().Trim(),
                    OwnerContact = row["OwnerContact"].ToString().Trim(),
                    OwnerPhoneNumber = row["OwnerPhoneNumber"].ToString().Trim(),
                    DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateAddressing(addressingMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 退回寻址确认任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReturnAddressings()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<AddressingMaintObject> addressingMaintObjects = new List<AddressingMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                addressingMaintObjects.Add(new AddressingMaintObject() { Id = Guid.Parse(row["Id"].ToString()), PlanningId = Guid.Parse(row["PlanningId"].ToString()), ModifyUserId = this.UserId });
            }
            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.ReturnAddressings(addressingMaintObjects));
            }
            return this.Sucess("退回寻址确认成功");
        }

        /// <summary>
        /// 获取寻址确认任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetAddressings()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<AddressingMaintObject> addressingMaintObjects = new List<AddressingMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                addressingMaintObjects.Add(new AddressingMaintObject() { Id = Guid.Parse(row["Id"].ToString()), PlanningId = Guid.Parse(row["PlanningId"].ToString()), ModifyUserId = this.UserId });
            }
            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.GetAddressings(addressingMaintObjects));
            }
            return this.Sucess("获取寻址确认成功");
        }

        #endregion

        #region 任务分配

        /// <summary>
        /// 任务分配
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ProjectDesign()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();

            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页规划列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectDesignsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["ProjectCode"] == null)
            {
                throw new ArgumentNullException("ProjectCode");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectDesignsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), Request["PlaceName"].Trim(), Request["ProjectCode"].Trim(), PROFESSION, this.UserId));
            }
        }

        #endregion

        #region 项目设计

        /// <summary>
        /// 项目设计
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> DesignDrawing()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();

            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页规划列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDesignDrawingsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["DesignRealName"] == null)
            {
                throw new ArgumentNullException("DesignRealName");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetDesignDrawingsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    Request["DesignRealName"].Trim(), PROFESSION, this.UserId));
            }
        }

        /// <summary>
        /// 根据项目任务Id获取项目设计
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetDesignDrawingById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetDesignDrawingById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存项目设计
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
                DesignRealName = row["DesignRealName"].ToString().Trim(),
                DesignDate = DateTime.Parse(row["DesignDate"].ToString()),
                FileIdList = row["FileIdList"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveDesignDrawingEdit(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 项目设计修改
        /// <summary>
        /// 项目设计修改
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ProjectDesignEdit(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectDesignEditById(id));
                ViewData["Id"] = id;
                ViewData["Mark1"] = projectTaskEditObject.Mark1;
                ViewData["Mark2"] = projectTaskEditObject.Mark2;
                ViewData["Mark3"] = projectTaskEditObject.Mark3;
                ViewData["Mark4"] = projectTaskEditObject.Mark4;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
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
        public async Task<ActionResult> SaveProjectDesignEdit()
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
                await Task.Factory.StartNew(() => proxy.Channel.SaveProjectDesignEdit(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 施工设计

        /// <summary>
        /// 施工设计
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> EngineeringDesign()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumDesignStateList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();
            IList<Dictionary<string, string>> enumTaskModelList = enumService.GetTaskModelEnum();

            ViewData["DesignState"] = JsonHelper.Encode(enumDesignStateList);
            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["TaskModel"] = JsonHelper.Encode(enumTaskModelList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumTaskModelList.Insert(0, allDict);
            enumDesignStateList.Insert(0, allDict);

            ViewData["DesignStateByAll"] = JsonHelper.Encode(enumDesignStateList);
            ViewData["TaskModelByAll"] = JsonHelper.Encode(enumTaskModelList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页规划列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetEngineeringDesignsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["TaskModel"] == null)
            {
                throw new ArgumentNullException("TaskModel");
            }
            if (Request["DesignRealName"] == null)
            {
                throw new ArgumentNullException("DesignRealName");
            }
            if (Request["DesignState"] == null)
            {
                throw new ArgumentNullException("DesignState");
            }

            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetEngineeringDesignsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    int.Parse(Request["TaskModel"]), Request["DesignRealName"].Trim(), int.Parse(Request["DesignState"]), PROFESSION, this.UserId));
            }
        }

        /// <summary>
        /// 根据工程任务Id获取施工设计
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetEngineeringDesignById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetEngineeringDesignById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存施工设计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveEngineeringDesign()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            EngineeringTaskEditObject engineeringTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());

            engineeringTaskEditObject = new EngineeringTaskEditObject()
            {
                Id = id,
                DesignRealName = row["DesignRealName"].ToString().Trim(),
                DesignMemos = row["DesignMemos"].ToString().Trim(),
                FileIdList = row["FileIdList"].ToString().Trim(),
                DesignState = int.Parse(row["DesignState"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveEngineeringDesign(engineeringTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 项目进度登记

        /// <summary>
        /// 项目进度登记
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ProjectProgress()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();
            IList<Dictionary<string, string>> enumProjectProgressList = enumService.GetProjectProgressEnum();

            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["ProjectProgress"] = JsonHelper.Encode(enumProjectProgressList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumProjectProgressList.Insert(0, allDict);
            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "7");
            Dict1.Add("text", "未完成");
            enumProjectProgressList.Insert(7, Dict1);

            ViewData["ProjectProgressByAll"] = JsonHelper.Encode(enumProjectProgressList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页项目进度列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectProgresssPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["ProjectProgress"] == null)
            {
                throw new ArgumentNullException("ProjectProgress");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectProgresssPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    int.Parse(Request["ProjectProgress"]), PROFESSION, this.UserId));
            }
        }

        /// <summary>
        /// 获取分页项目进度列表(移动端)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectProgresssPageMobile()
        {
            Guid areaId = Guid.Empty;
            Guid reseauId = Guid.Empty;
            string projectCode = "";
            string placeName = "";
            int projectProgress = 7;
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }
            if (Request["AreaId"] != null && Request["AreaId"] != "")
            {
                areaId = Guid.Parse(Request["AreaId"]);
            }
            if (Request["ReseauId"] != null && Request["ReseauId"] != "")
            {
                reseauId = Guid.Parse(Request["ReseauId"]);
            }
            if (Request["ProjectCode"] != null && Request["ProjectCode"] != "")
            {
                projectCode = Request["ProjectCode"].Trim();
            }
            if (Request["PlaceName"] != null && Request["PlaceName"] != "")
            {
                placeName = Request["PlaceName"].Trim();
            }
            if (Request["ProjectProgress"] != null && Request["ProjectProgress"] != "")
            {
                projectProgress = int.Parse(Request["ProjectProgress"]);
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectProgresssPageMobile(int.Parse(Request["Profession"]), areaId, reseauId, projectCode, placeName, projectProgress, this.UserId));
            }
        }

        /// <summary>
        /// 根据项目进度Id获取项目进度
        /// </summary>
        /// <param name="id">项目进度Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetProjectProgressById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetProjectProgressById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存项目进度
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProjectProgress()
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
                ProjectProgress = int.Parse(row["ProjectProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                FileIdList = row["FileIdList"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveProjectProgress(projectTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }

        ///// <summary>
        ///// 保存项目进度(移动端)
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<ActionResult> SaveProjectProgressMobile()
        //{
        //    if (Request["data"] == null)
        //    {
        //        throw new ArgumentNullException("data");
        //    }

        //    Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
        //    ProjectTaskEditObject projectTaskEditObject = null;
        //    Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());

        //    projectTaskEditObject = new ProjectTaskEditObject()
        //    {
        //        Id = id,
        //        ProjectProgress = int.Parse(row["ProjectProgress"].ToString()),
        //        ProgressMemos = row["ProgressMemos"].ToString().Trim(),
        //        Base64String = row["Base64String"].ToString().Trim(),
        //        ModifyUserId = this.UserId
        //    };
        //    using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
        //    {
        //        await Task.Factory.StartNew(() => proxy.Channel.SaveProjectProgressMobile(projectTaskEditObject));
        //    }
        //    return this.Sucess("数据保存成功");
        //}

        /// <summary>
        /// 保存项目或工程进度(移动端)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SaveProgressMobile()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }
            if (Request["TypeId"] == null)
            {
                throw new ArgumentNullException("TypeId");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());

            /*
            string base64String = "";
            if (row["Base64String"].ToString().Trim() != "")
            {
                ArrayList al = (ArrayList)JsonHelper.Decode(row["Base64String"].ToString().Trim());
                if (al.Count > 0)
                {
                    int index = 0;
                    for (int i = 0; i < al.Count; i++)
                    {
                        index += 1;
                        if (index != al.Count)
                        {
                            base64String += al[i].ToString() + "|";
                        }
                        else
                        {
                            base64String += al[i].ToString();
                        }
                    }
                }
            }
             */
            string[] arr = new string[1];
            arr[0] = "";
            string[] arrString = new string[] { };
            if (row["Base64String"].ToString().Trim() != "")
            {
                ArrayList al = (ArrayList)JsonHelper.Decode(row["Base64String"].ToString().Trim());
                arrString = (string[])al.ToArray(typeof(string));
            }

            if (int.Parse(Request["TypeId"]) == 1)
            {
                ProjectTaskEditObject projectTaskEditObject = null;
                projectTaskEditObject = new ProjectTaskEditObject()
                {
                    Id = id,
                    ProjectProgress = int.Parse(row["ProjectProgress"].ToString()),
                    ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                    Base64String = row["Base64String"].ToString().Trim() != "" ? arrString : arr,
                    ModifyUserId = this.UserId
                };
                using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
                {
                    await Task.Factory.StartNew(() => proxy.Channel.SaveProjectProgressMobile(projectTaskEditObject));
                }
            }
            else
            {
                EngineeringTaskEditObject engineeringTaskEditObject = null;
                engineeringTaskEditObject = new EngineeringTaskEditObject()
                {
                    Id = id,
                    EngineeringProgress = int.Parse(row["EngineeringProgress"].ToString()),
                    ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                    Base64String = row["Base64String"].ToString().Trim() != "" ? arrString : arr,
                    ModifyUserId = this.UserId
                };
                using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
                {
                    await Task.Factory.StartNew(() => proxy.Channel.SaveEngineeringProgressMobile(engineeringTaskEditObject));
                }
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 根据项目进度Id获取项目进度(移动端)
        /// </summary>
        /// <param name="id">项目进度Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetProjectProgressByIdMobile(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                string header = "http://211.149.154.229/PDCMSFiles";
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetProjectProgressByIdMobile(id, header)), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 工程进度登记

        /// <summary>
        /// 工程进度登记
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> EngineeringProgress()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();
            IList<Dictionary<string, string>> enumTaskModelList = enumService.GetTaskModelEnum();
            IList<Dictionary<string, string>> enumEngineeringProgressList = enumService.GetEngineeringProgressEnum();

            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["TaskModel"] = JsonHelper.Encode(enumTaskModelList);
            ViewData["EngineeringProgress"] = JsonHelper.Encode(enumEngineeringProgressList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumTaskModelList.Insert(0, allDict);
            enumEngineeringProgressList.Insert(0, allDict);
            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "6");
            Dict1.Add("text", "未完成");
            enumEngineeringProgressList.Insert(6, Dict1);

            ViewData["TaskModelByAll"] = JsonHelper.Encode(enumTaskModelList);
            ViewData["EngineeringProgressByAll"] = JsonHelper.Encode(enumEngineeringProgressList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页工程进度列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetEngineeringProgresssPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["TaskModel"] == null)
            {
                throw new ArgumentNullException("TaskModel");
            }
            if (Request["EngineeringProgress"] == null)
            {
                throw new ArgumentNullException("EngineeringProgress");
            }

            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetEngineeringProgresssPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    int.Parse(Request["TaskModel"]), int.Parse(Request["EngineeringProgress"]), PROFESSION, this.UserId));
            }
        }

        /// <summary>
        /// 获取分页工程进度列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetEngineeringProgresssPageMobile()
        {
            string projectCode = "";
            string placeName = "";
            int taskModel = 0;
            int engineeringProgress = 6;
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }
            if (Request["ProjectCode"] != null && Request["ProjectCode"] != "")
            {
                projectCode = Request["ProjectCode"].Trim();
            }
            if (Request["PlaceName"] != null && Request["PlaceName"] != "")
            {
                placeName = Request["PlaceName"].Trim();
            }
            if (Request["TaskModel"] != null && Request["TaskModel"] != "")
            {
                taskModel = int.Parse(Request["TaskModel"]);
            }
            if (Request["EngineeringProgress"] != null && Request["EngineeringProgress"] != "")
            {
                engineeringProgress = int.Parse(Request["EngineeringProgress"]);
            }

            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetEngineeringProgresssPageMobile(int.Parse(Request["Profession"]), projectCode, placeName, taskModel, engineeringProgress, this.UserId));
            }
        }

        /// <summary>
        /// 根据工程任务Id获取施工设计
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetEngineeringProgressById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetEngineeringProgressById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存工程进度
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveEngineeringProgress()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            EngineeringTaskEditObject engineeringTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());

            engineeringTaskEditObject = new EngineeringTaskEditObject()
            {
                Id = id,
                EngineeringProgress = int.Parse(row["EngineeringProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                FileIdList = row["FileIdList"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveEngineeringProgress(engineeringTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 保存工程进度(移动端)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SaveEngineeringProgressMobile()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            EngineeringTaskEditObject engineeringTaskEditObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());

            engineeringTaskEditObject = new EngineeringTaskEditObject()
            {
                Id = id,
                EngineeringProgress = int.Parse(row["EngineeringProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                FileIdList = row["FileIdList"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveEngineeringProgress(engineeringTaskEditObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 根据工程任务Id获取施工设计(移动端)
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetEngineeringProgressByIdMobile(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                string header = "http://211.149.154.229/PDCMSFiles";
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetEngineeringProgressByIdMobile(id, header)), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 基站导入

        /// <summary>
        /// 基站导入
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PlaceImport()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumStateList = enumService.GetStateEnum();

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["State"] = JsonHelper.Encode(enumStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumImportanceList.Insert(0, allDict);
            enumStateList.Insert(0, allDict);

            ViewData["ImportanceByAll"] = JsonHelper.Encode(enumImportanceList);
            ViewData["StateByAll"] = JsonHelper.Encode(enumStateList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                IList<PlaceOwnerSelectObject> placeOwnerSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceOwners());
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "全部" });
                ViewData["PlaceOwnersByAll"] = JsonHelper.Encode(placeOwnerSelectObjects);
                placeOwnerSelectObjects.RemoveAt(0);
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "请选择" });
                ViewData["PlaceOwnersBySelect"] = JsonHelper.Encode(placeOwnerSelectObjects);
            }
            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                IList<DepartmentSelectObject> departmentSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedDepartments(this.CompanyId));
                departmentSelectObjects.Insert(0, new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "请选择" });
                ViewData["AddressingDepartmentsBySelect"] = JsonHelper.Encode(departmentSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据站点Id获取站点
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlaceImportById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlaceImportById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页站点列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlaceImportsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceCode"] == null)
            {
                throw new ArgumentNullException("PlaceCode");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["PlaceOwner"] == null)
            {
                throw new ArgumentNullException("PlaceOwner");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["Importance"] == null)
            {
                throw new ArgumentNullException("Importance");
            }
            if (Request["State"] == null)
            {
                throw new ArgumentNullException("State");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlaceImportsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceCode"].Trim(), Request["PlaceName"].Trim(), PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["PlaceOwner"]),
                    Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["Importance"]), int.Parse(Request["State"]), this.CompanyId));
            }
        }

        /// <summary>
        /// 保存站点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlaceImport()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceMaintObject placeMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id != Guid.Empty)
            {
                placeMaintObject = new PlaceMaintObject()
                {
                    Id = id,
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    PlaceOwner = Guid.Parse(row["PlaceOwner"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    Importance = int.Parse(row["Importance"].ToString()),
                    DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                    AddressingDepartmentId = Guid.Parse(row["AddressingDepartmentId"].ToString()),
                    AddressingRealName = row["AddressingRealName"].ToString().Trim(),
                    OwnerName = row["OwnerName"].ToString().Trim(),
                    OwnerContact = row["OwnerContact"].ToString().Trim(),
                    OwnerPhoneNumber = row["OwnerPhoneNumber"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SavePlaceImport(placeMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 登记逻辑号

        /// <summary>
        /// 登记逻辑号
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> LogicalNumber()
        {
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据站点Id获取逻辑号
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetLogicalNumberById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetLogicalNumberById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页站点列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetLogicalNumbersPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceCode"] == null)
            {
                throw new ArgumentNullException("PlaceCode");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["G2Mark"] == null)
            {
                throw new ArgumentNullException("G2Mark");
            }
            if (Request["D2Mark"] == null)
            {
                throw new ArgumentNullException("D2Mark");
            }
            if (Request["G3Mark"] == null)
            {
                throw new ArgumentNullException("G3Mark");
            }
            if (Request["G4Mark"] == null)
            {
                throw new ArgumentNullException("G4Mark");
            }
            if (Request["G2Number"] == null)
            {
                throw new ArgumentNullException("G2Number");
            }
            if (Request["D2Number"] == null)
            {
                throw new ArgumentNullException("D2Number");
            }
            if (Request["G3Number"] == null)
            {
                throw new ArgumentNullException("G3Number");
            }
            if (Request["G4Number"] == null)
            {
                throw new ArgumentNullException("G4Number");
            }
            if (Request["AllMark"] == null)
            {
                throw new ArgumentNullException("AllMark");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetLogicalNumbersPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceCode"].Trim(), Request["PlaceName"].Trim(), PROFESSION, Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    int.Parse(Request["G2Mark"]), int.Parse(Request["D2Mark"]), int.Parse(Request["G3Mark"]), int.Parse(Request["G4Mark"]),
                    Request["G2Number"].Trim(), Request["D2Number"].Trim(), Request["G3Number"].Trim(), Request["G4Number"].Trim(), int.Parse(Request["AllMark"])));
            }
        }

        /// <summary>
        /// 保存站点
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
            PlaceMaintObject placeMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id != Guid.Empty)
            {
                placeMaintObject = new PlaceMaintObject()
                {
                    Id = id,
                    G2Number = row["G2Number"].ToString().Trim(),
                    D2Number = row["D2Number"].ToString().Trim(),
                    G3Number = row["G3Number"].ToString().Trim(),
                    G4Number = row["G4Number"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveLogicalNumber(placeMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 业务量导入

        /// <summary>
        /// 业务量导入
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessVolume()
        {

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumLogicalTypeList = enumService.GetLogicalTypeEnumPart("1,2,3,4");

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumLogicalTypeList.Insert(0, allDict);

            ViewData["LogicalTypeByAll"] = JsonHelper.Encode(enumLogicalTypeList);

            return View();
        }

        /// <summary>
        /// 根据站点Id获取逻辑号
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetBusinessVolumeById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页站点列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumesPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["LogicalType"] == null)
            {
                throw new ArgumentNullException("LogicalType");
            }
            if (Request["LogicalNumber"] == null)
            {
                throw new ArgumentNullException("LogicalNumber");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumesPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), int.Parse(Request["LogicalType"]), Request["LogicalNumber"].Trim(),
                    PROFESSION, this.CompanyId));
            }
        }

        /// <summary>
        /// 保存站点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveBusinessVolume()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            BusinessVolumeMaintObject businessVolumeMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id != Guid.Empty)
            {
                businessVolumeMaintObject = new BusinessVolumeMaintObject()
                {
                    Id = id,
                    LogicalNumber = row["LogicalNumber"].ToString().Trim(),
                    TrafficVolumes = decimal.Parse(row["TrafficVolumes"].ToString()),
                    BusinessVolumes = decimal.Parse(row["BusinessVolumes"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateBusinessVolume(businessVolumeMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 待阅通知

        /// <summary>
        /// 待阅通知
        /// </summary>
        /// <returns></returns>
        public ActionResult Notice()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumNoticeTypeList = enumService.GetNoticeTypeEnum();
            IList<Dictionary<string, string>> enumNoticeStateList = enumService.GetNoticeStateEnum();

            ViewData["NoticeType"] = JsonHelper.Encode(enumNoticeTypeList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumNoticeStateList.Insert(0, allDict);
            ViewData["NoticeStateByAll"] = JsonHelper.Encode(enumNoticeStateList);

            return View();
        }

        /// <summary>
        /// 获取分页待阅通知列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNoticePage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["NoticeContent"] == null)
            {
                throw new ArgumentNullException("NoticeContent");
            }
            if (Request["NoticeState"] == null)
            {
                throw new ArgumentNullException("NoticeState");
            }

            using (ServiceProxy<INoticeService> proxy = new ServiceProxy<INoticeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetNoticesPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["NoticeContent"].Trim(), int.Parse(Request["NoticeState"]),
                    this.UserId));
            }
        }

        /// <summary>
        /// 标记为已阅
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> SaveNoticeState(Guid id)
        {
            NoticeMaintObject noticeMaintObject = null;
            noticeMaintObject = new NoticeMaintObject() { Id = id, ModifyUserId = this.UserId };

            using (ServiceProxy<INoticeService> proxy = new ServiceProxy<INoticeService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveNoticeState(noticeMaintObject));
            }
            return this.Sucess("标记已阅成功");
        }

        /// <summary>
        /// 批量标记为已阅
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveNoticeStates()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<NoticeMaintObject> noticeMaintObjects = new List<NoticeMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    noticeMaintObjects.Add(new NoticeMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<INoticeService> proxy = new ServiceProxy<INoticeService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveNoticeStates(noticeMaintObjects));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 盲点反馈

        /// <summary>
        /// 盲点反馈
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> BlindSpotFeedBack()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumDoStateList = enumService.GetDoStateEnum();

            ViewData["DoState"] = JsonHelper.Encode(enumDoStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumDoStateList.Insert(0, allDict);
            ViewData["DoStateByAll"] = JsonHelper.Encode(enumDoStateList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据盲点反馈Id获取盲点反馈
        /// </summary>
        /// <param name="id">盲点反馈Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetBlindSpotFeedBackById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IBlindSpotFeedBackService> proxy = new ServiceProxy<IBlindSpotFeedBackService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetBlindSpotFeedBackById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页盲点反馈列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBlindSpotFeedBacksPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["DoState"] == null)
            {
                throw new ArgumentNullException("DoState");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }

            using (ServiceProxy<IBlindSpotFeedBackService> proxy = new ServiceProxy<IBlindSpotFeedBackService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBlindSpotFeedBacksPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Guid.Parse(Request["AreaId"]), Request["PlaceName"].Trim(),
                    int.Parse(Request["DoState"]), Guid.Parse(Request["CreateUserId"]), this.CompanyId));
            }
        }

        /// <summary>
        /// 保存盲点反馈
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveBlindSpotFeedBack()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                blindSpotFeedBackMaintObject = new BlindSpotFeedBackMaintObject()
                {
                    Id = Guid.Empty,
                    AreaId = Guid.Parse(row["AreaId"].ToString()),
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    FeedBackContent = row["FeedBackContent"].ToString(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                blindSpotFeedBackMaintObject = new BlindSpotFeedBackMaintObject()
                {
                    Id = id,
                    AreaId = Guid.Parse(row["AreaId"].ToString()),
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    FeedBackContent = row["FeedBackContent"].ToString(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IBlindSpotFeedBackService> proxy = new ServiceProxy<IBlindSpotFeedBackService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateBlindSpotFeedBack(blindSpotFeedBackMaintObject));
            }
            return this.Sucess("数据保存成功");
        }


        /// <summary>
        /// 删除盲点反馈
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveBlindSpotFeedBacks()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<BlindSpotFeedBackMaintObject> blindSpotFeedBackMaintObjects = new List<BlindSpotFeedBackMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    blindSpotFeedBackMaintObjects.Add(new BlindSpotFeedBackMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IBlindSpotFeedBackService> proxy = new ServiceProxy<IBlindSpotFeedBackService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveBlindSpotFeedBacks(blindSpotFeedBackMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        /// <summary>
        /// 保存盲点反馈(移动端)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SaveBlindSpotFeedBackMobile()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());

            /*
            string base64String = "";
            if (row["Base64String"].ToString().Trim() != "")
            {
                ArrayList al = (ArrayList)JsonHelper.Decode(row["Base64String"].ToString().Trim());
                if (al.Count > 0)
                {
                    int index = 0;
                    for (int i = 0; i < al.Count; i++)
                    {
                        index += 1;
                        if (index != al.Count)
                        {
                            base64String += al[i].ToString() + "|";
                        }
                        else
                        {
                            base64String += al[i].ToString();
                        }
                    }
                }
            }
             */
            string[] arr = new string[1];
            arr[0] = "";
            string[] arrString = new string[] { };
            if (row["Base64String"].ToString().Trim() != "")
            {
                ArrayList al = (ArrayList)JsonHelper.Decode(row["Base64String"].ToString().Trim());
                arrString = (string[])al.ToArray(typeof(string));
            }

            if (id == Guid.Empty)
            {
                blindSpotFeedBackMaintObject = new BlindSpotFeedBackMaintObject()
                {
                    Id = Guid.Empty,
                    AreaId = Guid.Parse(row["AreaId"].ToString()),
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    FeedBackContent = row["FeedBackContent"].ToString(),
                    Base64String = row["Base64String"].ToString().Trim() != "" ? arrString : arr,
                    CreateUserId = this.UserId
                };
            }
            using (ServiceProxy<IBlindSpotFeedBackService> proxy = new ServiceProxy<IBlindSpotFeedBackService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveBlindSpotFeedBackMobile(blindSpotFeedBackMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 盲点处理

        /// <summary>
        /// 盲点处理
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> BlindSpotHanding()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumDoStateList = enumService.GetDoStateEnum();

            ViewData["DoState"] = JsonHelper.Encode(enumDoStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumDoStateList.Insert(0, allDict);
            ViewData["DoStateByAll"] = JsonHelper.Encode(enumDoStateList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 保存反馈处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveBlindSpotHanding()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id != Guid.Empty)
            {
                blindSpotFeedBackMaintObject = new BlindSpotFeedBackMaintObject()
                {
                    Id = id,
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    FeedBackResult = row["FeedBackResult"].ToString(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IBlindSpotFeedBackService> proxy = new ServiceProxy<IBlindSpotFeedBackService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveBlindSpotHanding(blindSpotFeedBackMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 站点导入

        /// <summary>
        /// 站点导入
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Purchase()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumPropertyRightList = enumService.GetPropertyRightEnum("1,2,3,4");
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["PropertyRight"] = JsonHelper.Encode(enumPropertyRightList);
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumImportanceList.Insert(0, allDict);
            enumBoolList.Insert(0, allDict);

            ViewData["ImportanceByAll"] = JsonHelper.Encode(enumImportanceList);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据购置基站Id获取购置基站
        /// </summary>
        /// <param name="id">购置基站Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPurchaseById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPurchaseService> proxy = new ServiceProxy<IPurchaseService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPurchaseById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页购置基站列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPurchasesPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["GroupPlaceCode"] == null)
            {
                throw new ArgumentNullException("GroupPlaceCode");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["PropertyRightList"] == null)
            {
                throw new ArgumentNullException("PropertyRightList");
            }
            if (Request["Importance"] == null)
            {
                throw new ArgumentNullException("Importance");
            }
            if (Request["TelecomShare"] == null)
            {
                throw new ArgumentNullException("TelecomShare");
            }
            if (Request["MobileShare"] == null)
            {
                throw new ArgumentNullException("MobileShare");
            }
            if (Request["UnicomShare"] == null)
            {
                throw new ArgumentNullException("UnicomShare");
            }

            string propertyRightSql = "";
            if (Request["PropertyRightList"].Trim() != "")
            {
                string[] propertyRightList = Request["PropertyRightList"].Trim().Split(',');
                for (int i = 0; i < propertyRightList.Length; i++)
                {
                    propertyRightSql += "select " + propertyRightList[i];
                    if (i != propertyRightList.Length - 1)
                    {
                        propertyRightSql += " union ";
                    }
                }
            }

            using (ServiceProxy<IPurchaseService> proxy = new ServiceProxy<IPurchaseService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPurchasesPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["GroupPlaceCode"].Trim(), Request["PlaceName"].Trim(), PROFESSION,
                    Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), propertyRightSql,
                    int.Parse(Request["Importance"]), int.Parse(Request["TelecomShare"]), int.Parse(Request["MobileShare"]), int.Parse(Request["UnicomShare"])));
            }
        }

        /// <summary>
        /// 保存购置基站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePurchase()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PurchaseMaintObject purchaseMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                purchaseMaintObject = new PurchaseMaintObject()
                {
                    Id = Guid.Empty,
                    PurchaseDateText = row["PurchaseDate"].ToString(),
                    GroupPlaceCode = row["GroupPlaceCode"].ToString(),
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    Profession = PROFESSION,
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    PropertyRight = int.Parse(row["PropertyRight"].ToString()),
                    Importance = int.Parse(row["Importance"].ToString()),
                    SceneId = Guid.Parse(row["SceneId"].ToString()),
                    DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                    OwnerName = row["OwnerName"].ToString().Trim(),
                    OwnerContact = row["OwnerContact"].ToString().Trim(),
                    OwnerPhoneNumber = row["OwnerPhoneNumber"].ToString().Trim(),
                    TelecomShare = bool.Parse(row["TelecomShare"].ToString()) ? 1 : 2,
                    MobileShare = bool.Parse(row["MobileShare"].ToString()) ? 1 : 2,
                    UnicomShare = bool.Parse(row["UnicomShare"].ToString()) ? 1 : 2,
                    Remarks = row["Remarks"].ToString().Trim(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                purchaseMaintObject = new PurchaseMaintObject()
                {
                    Id = id,
                    PurchaseDateText = row["PurchaseDate"].ToString(),
                    GroupPlaceCode = row["GroupPlaceCode"].ToString(),
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    PropertyRight = int.Parse(row["PropertyRight"].ToString()),
                    Importance = int.Parse(row["Importance"].ToString()),
                    SceneId = Guid.Parse(row["SceneId"].ToString()),
                    DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                    OwnerName = row["OwnerName"].ToString().Trim(),
                    OwnerContact = row["OwnerContact"].ToString().Trim(),
                    OwnerPhoneNumber = row["OwnerPhoneNumber"].ToString().Trim(),
                    TelecomShare = bool.Parse(row["TelecomShare"].ToString()) ? 1 : 2,
                    MobileShare = bool.Parse(row["MobileShare"].ToString()) ? 1 : 2,
                    UnicomShare = bool.Parse(row["UnicomShare"].ToString()) ? 1 : 2,
                    Remarks = row["Remarks"].ToString().Trim(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPurchaseService> proxy = new ServiceProxy<IPurchaseService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdatePurchase(purchaseMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除购置基站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePurchases()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<PurchaseMaintObject> purchaseMaintObjects = new List<PurchaseMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    purchaseMaintObjects.Add(new PurchaseMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPurchaseService> proxy = new ServiceProxy<IPurchaseService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemovePurchases(purchaseMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 运营商共享

        /// <summary>
        /// 运营商共享基站
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OperatorsSharing()
        {
            ViewData["CompanyNature"] = this.CompanyNature;
            ViewData["CompanyId"] = this.CompanyId;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumUrgencyList = enumService.GetUrgencyEnum();
            IList<Dictionary<string, string>> enumPropertyRightList = enumService.GetPropertyRightEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Urgency"] = JsonHelper.Encode(enumUrgencyList);
            ViewData["PropertyRight"] = JsonHelper.Encode(enumPropertyRightList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumUrgencyList.Insert(0, allDict);

            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["UrgencyByAll"] = JsonHelper.Encode(enumUrgencyList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanysByNature(2));
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据运营商共享Id获取运营商共享
        /// </summary>
        /// <param name="id">运营商共享Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetOperatorsSharingById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IOperatorsSharingService> proxy = new ServiceProxy<IOperatorsSharingService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsSharingById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页运营商共享列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOperatorsSharingsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PlaceCode"] == null)
            {
                throw new ArgumentNullException("PlaceCode");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["Urgency"] == null)
            {
                throw new ArgumentNullException("Urgency");
            }
            if (Request["Solved"] == null)
            {
                throw new ArgumentNullException("Solved");
            }

            using (ServiceProxy<IOperatorsSharingService> proxy = new ServiceProxy<IOperatorsSharingService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsSharingsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlaceCode"].Trim(), Request["PlaceName"].Trim(),
                    Guid.Parse(Request["CompanyId"]), PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    int.Parse(Request["Urgency"]), int.Parse(Request["Solved"])));
            }
        }

        /// <summary>
        /// 获取分页运营商共享列表，用于选择运营商共享申请
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOperatorsSharingsPageBySelect()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceCode"] == null)
            {
                throw new ArgumentNullException("PlaceCode");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["CompanyId"] == null)
            {
                throw new ArgumentNullException("CompanyId");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["Urgency"] == null)
            {
                throw new ArgumentNullException("Urgency");
            }

            using (ServiceProxy<IOperatorsSharingService> proxy = new ServiceProxy<IOperatorsSharingService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsSharingsPageBySelect(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceCode"].Trim(), Request["PlaceName"].Trim(), Guid.Parse(Request["CompanyId"]), PROFESSION, Guid.Parse(Request["PlaceCategoryId"]),
                    Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["Urgency"])));
            }
        }

        /// <summary>
        /// 根据条件获取指定站点的运营商规划列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOperatorsSharingsByPlace()
        {
            if (Request["OperatorsSharingId"] == null)
            {
                throw new ArgumentNullException("OperatorsSharingId");
            }
            if (Request["RemodelingId"] == null)
            {
                throw new ArgumentNullException("RemodelingId");
            }

            using (ServiceProxy<IOperatorsSharingService> proxy = new ServiceProxy<IOperatorsSharingService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsSharingsByPlace(Guid.Parse(Request["OperatorsSharingId"]), Guid.Parse(Request["RemodelingId"])));
            }
        }

        /// <summary>
        /// 保存运营商共享
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveOperatorsSharing()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            OperatorsSharingMaintObject operatorsSharingMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                operatorsSharingMaintObject = new OperatorsSharingMaintObject()
                {
                    Id = Guid.Empty,
                    PlaceCode = row["PlaceCode"].ToString().Trim(),
                    Profession = PROFESSION,
                    PlaceId = Guid.Parse(row["PlaceId"].ToString()),
                    PowerUsed = row["PowerUsed"].ToString().Trim() == "" ? 0 : decimal.Parse(row["PowerUsed"].ToString()),
                    PoleNumber = row["PoleNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["PoleNumber"].ToString()),
                    CabinetNumber = row["CabinetNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["CabinetNumber"].ToString()),
                    Urgency = int.Parse(row["Urgency"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    CompanyId = this.CompanyId,
                    CreateUserId = this.UserId,
                    CurrentCompanyNature = this.CompanyNature
                };
            }
            else
            {
                operatorsSharingMaintObject = new OperatorsSharingMaintObject()
                {
                    Id = id,
                    PlaceCode = row["PlaceCode"].ToString().Trim(),
                    PlaceId = Guid.Parse(row["PlaceId"].ToString()),
                    PowerUsed = row["PowerUsed"].ToString().Trim() == "" ? 0 : decimal.Parse(row["PowerUsed"].ToString()),
                    PoleNumber = row["PoleNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["PoleNumber"].ToString()),
                    CabinetNumber = row["CabinetNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["CabinetNumber"].ToString()),
                    Urgency = int.Parse(row["Urgency"].ToString()),
                    Remarks = row["Remarks"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IOperatorsSharingService> proxy = new ServiceProxy<IOperatorsSharingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateOperatorsSharing(operatorsSharingMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 关联运营商共享
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemodelingAssociate()
        {
            if (Request["RemodelingId"] == null)
            {
                throw new ArgumentNullException("RemodelingId");
            }
            if (Request["RemodelingCreateUserId"] == null)
            {
                throw new ArgumentNullException("RemodelingCreateUserId");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<OperatorsSharingMaintObject> operatorsSharingMaintObjects = new List<OperatorsSharingMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    operatorsSharingMaintObjects.Add(new OperatorsSharingMaintObject() { Id = id, RemodelingId = Guid.Parse(row["RemodelingId"].ToString()) });
                }
            }
            using (ServiceProxy<IOperatorsSharingService> proxy = new ServiceProxy<IOperatorsSharingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.Associate(Guid.Parse(Request["RemodelingId"]), Guid.Parse(Request["RemodelingCreateUserId"]), this.UserId, operatorsSharingMaintObjects));
            }
            return this.Sucess("数据关联成功");
        }

        /// <summary>
        /// 删除运营商共享
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveOperatorsSharings()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<OperatorsSharingMaintObject> operatorsSharingMaintObjects = new List<OperatorsSharingMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    operatorsSharingMaintObjects.Add(new OperatorsSharingMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IOperatorsSharingService> proxy = new ServiceProxy<IOperatorsSharingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveOperatorsSharings(operatorsSharingMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 基站改造

        /// <summary>
        /// 基站改造
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Remodeling()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();
            IList<Dictionary<string, string>> enumWFProcessInstanceStateList = enumService.GetWFProcessInstanceStateEnum();

            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["OrderState"] = JsonHelper.Encode(enumWFProcessInstanceStateList);

            IList<Dictionary<string, string>> enumProjectTypeAllList = enumService.GetProjectTypeEnum("2,3,4");
            IList<Dictionary<string, string>> enumProjectTypeSelectList = enumService.GetProjectTypeEnum("2,3,4");

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumProjectTypeAllList.Insert(0, allDict);
            enumWFProcessInstanceStateList.Insert(0, allDict);
            ViewData["ProjectTypeByAll"] = JsonHelper.Encode(enumProjectTypeAllList);
            ViewData["OrderStateByAll"] = JsonHelper.Encode(enumWFProcessInstanceStateList);

            ViewData["ProjectTypeBySelect"] = JsonHelper.Encode(enumProjectTypeSelectList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                IList<PlaceOwnerSelectObject> placeOwnerSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceOwners());
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "全部" });
                ViewData["PlaceOwnersByAll"] = JsonHelper.Encode(placeOwnerSelectObjects);
                placeOwnerSelectObjects.RemoveAt(0);
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "请选择" });
                ViewData["PlaceOwnersBySelect"] = JsonHelper.Encode(placeOwnerSelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanysByNature(2));
                companySelectObjects.Insert(0, new CompanySelectObject() { Id = Guid.Empty, CompanyName = "全部" });
                ViewData["CompanysByAll"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据改造Id获取改造
        /// </summary>
        /// <param name="id">改造Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetRemodelingById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页改造列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRemodelingsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["ProjectType"] == null)
            {
                throw new ArgumentNullException("ProjectType");
            }
            if (Request["OrderState"] == null)
            {
                throw new ArgumentNullException("OrderState");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }

            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlaceName"].Trim(),
                    PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    int.Parse(Request["ProjectType"]), int.Parse(Request["OrderState"]), Guid.Parse(Request["CreateUserId"])));
            }
        }

        /// <summary>
        /// 保存基站改造
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveRemodeling()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            RemodelingMaintObject remodelingMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                remodelingMaintObject = new RemodelingMaintObject()
                {
                    Id = Guid.Empty,
                    Profession = PROFESSION,
                    PlaceId = Guid.Parse(row["PlaceId"].ToString()),
                    ProjectType = int.Parse(row["ProjectType"].ToString()),
                    ProposedNetwork = row["ProposedNetwork"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                remodelingMaintObject = new RemodelingMaintObject()
                {
                    Id = id,
                    PlaceId = Guid.Parse(row["PlaceId"].ToString()),
                    ProjectType = int.Parse(row["ProjectType"].ToString()),
                    ProposedNetwork = row["ProposedNetwork"].ToString().Trim(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateRemodeling(remodelingMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除基站改造
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveRemodelings()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<RemodelingMaintObject> remodelingMaintObjects = new List<RemodelingMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    remodelingMaintObjects.Add(new RemodelingMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IRemodelingService> proxy = new ServiceProxy<IRemodelingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveRemodelings(remodelingMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 共享基站汇总

        /// <summary>
        /// 共享基站汇总
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ShareSummary()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumUrgencyList = enumService.GetUrgencyEnum();
            IList<Dictionary<string, string>> enumPropertyRightList = enumService.GetPropertyRightEnum();
            IList<Dictionary<string, string>> enumWFProcessInstanceStateList = enumService.GetWFProcessInstanceStateEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Urgency"] = JsonHelper.Encode(enumUrgencyList);
            ViewData["PropertyRight"] = JsonHelper.Encode(enumPropertyRightList);
            ViewData["OrderState"] = JsonHelper.Encode(enumWFProcessInstanceStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumUrgencyList.Insert(0, allDict);
            enumWFProcessInstanceStateList.Insert(0, allDict);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["OrderStateAll"] = JsonHelper.Encode(enumWFProcessInstanceStateList);
            enumBoolList.RemoveAt(0);
            ViewData["UrgencyByAll"] = JsonHelper.Encode(enumUrgencyList);

            Dictionary<string, string> selectDict = new Dictionary<string, string>(2);
            selectDict.Add("id", "0");
            selectDict.Add("text", "请选择");
            enumBoolList.Insert(0, selectDict);
            enumPropertyRightList.Insert(0, selectDict);
            ViewData["BoolBySelect"] = JsonHelper.Encode(enumBoolList);
            ViewData["PropertyRightBySelect"] = JsonHelper.Encode(enumPropertyRightList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanysByNature(2));
                companySelectObjects.Insert(0, new CompanySelectObject() { Id = Guid.Empty, CompanyName = "全部" });
                ViewData["CompanysByAll"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        #endregion

        #region 运营商规划清单
        public async Task<ActionResult> OperatorsPlanningReport()
        {
            ViewData["CompanyNature"] = this.CompanyNature;
            ViewData["CompanyId"] = this.CompanyId;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumUrgencyList = enumService.GetUrgencyEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Urgency"] = JsonHelper.Encode(enumUrgencyList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumUrgencyList.Insert(0, allDict);

            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["UrgencyByAll"] = JsonHelper.Encode(enumUrgencyList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanysByNature(2));
                companySelectObjects.Insert(0, new CompanySelectObject() { Id = Guid.Empty, CompanyName = "全部" });
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        #endregion

        #region 寻址确认编辑
        /// <summary>
        /// 寻址确认编辑
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AddressingEdit(Guid id, Guid wfActivityInstanceId)
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
            ViewData["Demand"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingEditorObject addressingEditorObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingEditorById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingEditorObject.OrderCode;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["PlaceCode"] = addressingEditorObject.PlaceCode;
                ViewData["PlaceName"] = addressingEditorObject.PlaceName;
                ViewData["PlanningName"] = addressingEditorObject.PlanningName;
                ViewData["AreaName"] = addressingEditorObject.AreaName;
                ViewData["ReseauName"] = addressingEditorObject.ReseauName;
                ViewData["PlaceCategoryName"] = addressingEditorObject.PlaceCategoryName;
                ViewData["ImportanceName"] = addressingEditorObject.ImportanceName;
                ViewData["Lng"] = addressingEditorObject.Lng;
                ViewData["Lat"] = addressingEditorObject.Lat;
                ViewData["SceneName"] = addressingEditorObject.SceneName;
                ViewData["AddressingStateName"] = addressingEditorObject.AddressingStateName;
                ViewData["OwnerName"] = addressingEditorObject.OwnerName;
                ViewData["OwnerContact"] = addressingEditorObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = addressingEditorObject.OwnerPhoneNumber;
                ViewData["TelecomDemand"] = addressingEditorObject.TelecomDemand;
                ViewData["MobileDemand"] = addressingEditorObject.MobileDemand;
                ViewData["UnicomDemand"] = addressingEditorObject.UnicomDemand;
                ViewData["DetailedAddress"] = addressingEditorObject.DetailedAddress;
                ViewData["Remarks"] = addressingEditorObject.Remarks;
                ViewData["ProjectId"] = addressingEditorObject.ProjectId;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["ProjectManagerId"] = addressingEditorObject.ProjectManagerId;
                ViewData["ProjectManagerName"] = addressingEditorObject.ProjectManagerName;
                ViewData["PlaceId"] = addressingEditorObject.PlaceId;
                ViewData["PlanningId"] = addressingEditorObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["Count"] = addressingEditorObject.Count;
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAddressingEdit()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            AddressingMaintObject addressingMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            addressingMaintObject = new AddressingMaintObject()
            {
                Id = id,
                //ProjectId = Guid.Parse(row["ProjectId"].ToString()),
                //ProjectManagerId = Guid.Parse(row["ProjectManagerId"].ToString()),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                //MobileDemand = int.Parse(row["MobileDemand"].ToString()),
                //TelecomDemand = int.Parse(row["TelecomDemand"].ToString()),
                //UnicomDemand = int.Parse(row["UnicomDemand"].ToString()),
                CompanyId = this.CompanyId,
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.UpdateAddressingEdit(addressingMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 新增基站建设
        /// <summary>
        /// 新增基站建设
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ConstructionPlanning()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumConstructionMethodList = enumService.GetConstructionMethodEnum();
            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            IList<Dictionary<string, string>> enumFireControlList = enumService.GetFireControlEnum();

            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumImportanceList.Insert(0, nullDict);
            //enumConstructionProgressList.Insert(0, nullDict);
            enumConstructionMethodList.Insert(0, nullDict);
            enumTowerTypeList.Insert(0, nullDict);
            enumMachineRoomTypeList.Insert(0, nullDict);
            enumExternalElectricList.Insert(0, nullDict);
            enumFireControlList.Insert(0, nullDict);

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);
            ViewData["ConstructionMethod"] = JsonHelper.Encode(enumConstructionMethodList);
            ViewData["TowerType"] = JsonHelper.Encode(enumTowerTypeList);
            ViewData["MachineRoomType"] = JsonHelper.Encode(enumMachineRoomTypeList);
            ViewData["ExternalElectric"] = JsonHelper.Encode(enumExternalElectricList);
            ViewData["FireControl"] = JsonHelper.Encode(enumFireControlList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumConstructionProgressAllList.Insert(0, allDict);

            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "5");
            Dict1.Add("text", "未完成");
            enumConstructionProgressAllList.Insert(5, Dict1);

            ViewData["ConstructionProgressByAll"] = JsonHelper.Encode(enumConstructionProgressAllList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取建设任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetConstructionPlanningsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["ProjectId"] == null)
            {
                throw new ArgumentNullException("ProjectId");
            }
            if (Request["ConstructionProgress"] == null)
            {
                throw new ArgumentNullException("ConstructionProgress");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetConstructionPlanningsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    Guid.Parse(Request["ProjectId"]), int.Parse(Request["ConstructionProgress"]), this.UserId, 1));
            }
        }

        /// <summary>
        /// 获取建设任务维护实体
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetConstructionPlanningById()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["PlaceId"] == null)
            {
                throw new ArgumentNullException("PlaceId");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetConstructionPlanningById(Guid.Parse(Request["Id"]), Guid.Parse(Request["PlaceId"]))), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改建设任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveConstructionPlanning()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ConstructionTaskEditorObject constructionTaskEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            constructionTaskEditorObject = new ConstructionTaskEditorObject()
            {
                Id = id,
                PlaceId = Guid.Parse(row["PlaceId"].ToString()),
                ConstructionProgress = int.Parse(row["ConstructionProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                TowerType = int.Parse(row["TowerType"].ToString()),
                TowerHeight = decimal.Parse(row["TowerHeight"].ToString()),
                PlatFormNumber = int.Parse(row["PlatFormNumber"].ToString()),
                PoleNumber = int.Parse(row["PoleNumber"].ToString()),
                MachineRoomType = int.Parse(row["MachineRoomType"].ToString()),
                MachineRoomArea = decimal.Parse(row["MachineRoomArea"].ToString()),
                ExternalElectric = int.Parse(row["ExternalElectric"].ToString()),
                SwitchPower = int.Parse(row["SwitchPower"].ToString()),
                Battery = int.Parse(row["Battery"].ToString()),
                AirConditioner = int.Parse(row["AirConditioner"].ToString()),
                FireControl = int.Parse(row["FireControl"].ToString()),
                FileIdList = row["FileIdList"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveConstructionPlanning(constructionTaskEditorObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 改造基站建设
        /// <summary>
        /// 改造基站建设
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ConstructionRemodeing()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumConstructionMethodList = enumService.GetConstructionMethodEnum();
            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            IList<Dictionary<string, string>> enumFireControlList = enumService.GetFireControlEnum();

            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumImportanceList.Insert(0, nullDict);
            //enumConstructionProgressList.Insert(0, nullDict);
            enumConstructionMethodList.Insert(0, nullDict);
            enumTowerTypeList.Insert(0, nullDict);
            enumMachineRoomTypeList.Insert(0, nullDict);
            enumExternalElectricList.Insert(0, nullDict);
            enumFireControlList.Insert(0, nullDict);

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);
            ViewData["ConstructionMethod"] = JsonHelper.Encode(enumConstructionMethodList);
            ViewData["TowerType"] = JsonHelper.Encode(enumTowerTypeList);
            ViewData["MachineRoomType"] = JsonHelper.Encode(enumMachineRoomTypeList);
            ViewData["ExternalElectric"] = JsonHelper.Encode(enumExternalElectricList);
            ViewData["FireControl"] = JsonHelper.Encode(enumFireControlList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumConstructionProgressAllList.Insert(0, allDict);

            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "5");
            Dict1.Add("text", "未完成");
            enumConstructionProgressAllList.Insert(5, Dict1);

            ViewData["ConstructionProgressByAll"] = JsonHelper.Encode(enumConstructionProgressAllList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取建设任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetConstructionRemodeingsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["ProjectId"] == null)
            {
                throw new ArgumentNullException("ProjectId");
            }
            if (Request["ConstructionProgress"] == null)
            {
                throw new ArgumentNullException("ConstructionProgress");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetConstructionPlanningsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    Guid.Parse(Request["ProjectId"]), int.Parse(Request["ConstructionProgress"]), this.UserId, 2));
            }
        }

        /// <summary>
        /// 获取建设任务维护实体
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetConstructionRemodeingById()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["PlaceId"] == null)
            {
                throw new ArgumentNullException("PlaceId");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetConstructionPlanningById(Guid.Parse(Request["Id"]), Guid.Parse(Request["PlaceId"]))), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改建设任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveConstructionRemodeing()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ConstructionTaskEditorObject constructionTaskEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            constructionTaskEditorObject = new ConstructionTaskEditorObject()
            {
                Id = id,
                PlaceId = Guid.Parse(row["PlaceId"].ToString()),
                ConstructionProgress = int.Parse(row["ConstructionProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                TowerType = int.Parse(row["TowerType"].ToString()),
                TowerHeight = decimal.Parse(row["TowerHeight"].ToString()),
                PlatFormNumber = int.Parse(row["PlatFormNumber"].ToString()),
                PoleNumber = int.Parse(row["PoleNumber"].ToString()),
                MachineRoomType = int.Parse(row["MachineRoomType"].ToString()),
                MachineRoomArea = decimal.Parse(row["MachineRoomArea"].ToString()),
                ExternalElectric = int.Parse(row["ExternalElectric"].ToString()),
                SwitchPower = int.Parse(row["SwitchPower"].ToString()),
                Battery = int.Parse(row["Battery"].ToString()),
                AirConditioner = int.Parse(row["AirConditioner"].ToString()),
                FireControl = int.Parse(row["FireControl"].ToString()),
                FileIdList = row["FileIdList"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveConstructionPlanning(constructionTaskEditorObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 新增站安装登记
        /// <summary>
        /// 新增站安装登记
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> RegisterPlanning()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumIsFinishList = enumService.GetBoolEnum();

            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumIsFinishAllList = enumService.GetBoolEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumImportanceList.Insert(0, nullDict);
            enumConstructionProgressList.Insert(0, nullDict);
            //enumIsFinishList.Insert(0, nullDict);

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);
            ViewData["IsFinish"] = JsonHelper.Encode(enumIsFinishList);
            ViewData["CompanyId"] = this.CompanyId;

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumConstructionProgressAllList.Insert(0, allDict);
            enumIsFinishAllList.Insert(0, allDict);

            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "6");
            Dict1.Add("text", "未完成");
            enumConstructionProgressAllList.Insert(6, Dict1);

            ViewData["ConstructionProgressByAll"] = JsonHelper.Encode(enumConstructionProgressAllList);
            ViewData["IsFinishByAll"] = JsonHelper.Encode(enumIsFinishAllList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            return View();
        }

        public async Task<string> GetRegisterPlanningsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["ConstructionProgress"] == null)
            {
                throw new ArgumentNullException("ConstructionProgress");
            }
            if (Request["IsFinish"] == null)
            {
                throw new ArgumentNullException("IsFinish");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetRegisterPlanningsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    int.Parse(Request["ConstructionProgress"]), int.Parse(Request["IsFinish"]), this.CompanyId, 1));
            }
        }

        public async Task<ActionResult> GetRegisterPlanningById()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["ConstructionTaskId"] == null)
            {
                throw new ArgumentNullException("ConstructionTaskId");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetRegisterPlanningById(Guid.Parse(Request["Id"]), Guid.Parse(Request["ConstructionTaskId"]), this.CompanyId)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改站点属性
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveRegisterPlanning()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlacePropertyEditorObject placePropertyEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            placePropertyEditorObject = new PlacePropertyEditorObject()
            {
                Id = id,
                PoleNumber = int.Parse(row["PoleNumber"].ToString()),
                CabinetNumber = int.Parse(row["CabinetNumber"].ToString()),
                PowerUsed = decimal.Parse(row["PowerUsed"].ToString()),
                IsFinish = int.Parse(row["IsFinish"].ToString()),
                ConstructionTaskId = Guid.Parse(row["ConstructionTaskId"].ToString()),
                CreateUserId = this.UserId,
                CompanyId = this.CompanyId
            };
            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveRegisterPlanning(placePropertyEditorObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 共享站安装登记
        /// <summary>
        /// 共享站安装登记
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> RegisterRemodeing()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumIsFinishList = enumService.GetBoolEnum();

            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumIsFinishAllList = enumService.GetBoolEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumImportanceList.Insert(0, nullDict);
            enumConstructionProgressList.Insert(0, nullDict);
            //enumIsFinishList.Insert(0, nullDict);

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);
            ViewData["IsFinish"] = JsonHelper.Encode(enumIsFinishList);
            ViewData["CompanyId"] = this.CompanyId;

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumConstructionProgressAllList.Insert(0, allDict);
            enumIsFinishAllList.Insert(0, allDict);

            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "6");
            Dict1.Add("text", "未完成");
            enumConstructionProgressAllList.Insert(6, Dict1);

            ViewData["ConstructionProgressByAll"] = JsonHelper.Encode(enumConstructionProgressAllList);
            ViewData["IsFinishByAll"] = JsonHelper.Encode(enumIsFinishAllList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            return View();
        }

        public async Task<string> GetRegisterRemodeingsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["ConstructionProgress"] == null)
            {
                throw new ArgumentNullException("ConstructionProgress");
            }
            if (Request["IsFinish"] == null)
            {
                throw new ArgumentNullException("IsFinish");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetRegisterRemodeingsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    int.Parse(Request["ConstructionProgress"]), int.Parse(Request["IsFinish"]), this.CompanyId, 2));
            }
        }

        public async Task<ActionResult> GetRegisterRemodeingById()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["ConstructionTaskId"] == null)
            {
                throw new ArgumentNullException("ConstructionTaskId");
            }
            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetRegisterPlanningById(Guid.Parse(Request["Id"]), Guid.Parse(Request["ConstructionTaskId"]), this.CompanyId)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改站点属性
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveRegisterRemodeing()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlacePropertyEditorObject placePropertyEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            placePropertyEditorObject = new PlacePropertyEditorObject()
            {
                Id = id,
                PoleNumber = int.Parse(row["PoleNumber"].ToString()),
                CabinetNumber = int.Parse(row["CabinetNumber"].ToString()),
                PowerUsed = decimal.Parse(row["PowerUsed"].ToString()),
                IsFinish = int.Parse(row["IsFinish"].ToString()),
                ConstructionTaskId = Guid.Parse(row["ConstructionTaskId"].ToString()),
                CreateUserId = this.UserId,
                CompanyId = this.CompanyId
            };
            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveRegisterPlanning(placePropertyEditorObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 新增基站建设进度表
        public async Task<ActionResult> ConstructionPlanningReport()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumUrgencyList = enumService.GetUrgencyEnum();
            IList<Dictionary<string, string>> enumDemandList = enumService.GetDemandEnum();
            IList<Dictionary<string, string>> enumDemandStateList = enumService.GetDemandStateEnum();
            IList<Dictionary<string, string>> enumAddressingStateList = enumService.GetAddressingStateEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Urgency"] = JsonHelper.Encode(enumUrgencyList);
            ViewData["Demand"] = JsonHelper.Encode(enumDemandList);
            ViewData["DemandState"] = JsonHelper.Encode(enumDemandStateList);
            ViewData["AddressingState"] = JsonHelper.Encode(enumAddressingStateList);
            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumConstructionProgressList.Insert(0, nullDict);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumUrgencyList.Insert(0, allDict);
            enumDemandList.Insert(0, allDict);
            enumDemandStateList.Insert(0, allDict);
            enumAddressingStateList.Insert(0, allDict);
            enumConstructionProgressAllList.Insert(0, allDict);

            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["UrgencyByAll"] = JsonHelper.Encode(enumUrgencyList);
            ViewData["DemandByAll"] = JsonHelper.Encode(enumDemandList);
            ViewData["DemandStateByAll"] = JsonHelper.Encode(enumDemandStateList);
            ViewData["AddressingStateByAll"] = JsonHelper.Encode(enumAddressingStateList);
            ViewData["ConstructionProgressAll"] = JsonHelper.Encode(enumConstructionProgressAllList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanysByNature(2));
                companySelectObjects.Insert(0, new CompanySelectObject() { Id = Guid.Empty, CompanyName = "全部" });
                ViewData["CompanysByAll"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        public async Task<string> GetConstructionPlanningsReportPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PlanningCode"] == null)
            {
                throw new ArgumentNullException("PlanningCode");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["Urgency"] == null)
            {
                throw new ArgumentNullException("Urgency");
            }
            if (Request["TelecomDemand"] == null)
            {
                throw new ArgumentNullException("TelecomDemand");
            }
            if (Request["MobileDemand"] == null)
            {
                throw new ArgumentNullException("MobileDemand");
            }
            if (Request["UnicomDemand"] == null)
            {
                throw new ArgumentNullException("UnicomDemand");
            }
            if (Request["DemandState"] == null)
            {
                throw new ArgumentNullException("DemandState");
            }
            if (Request["Issued"] == null)
            {
                throw new ArgumentNullException("Issued");
            }
            if (Request["AddressingState"] == null)
            {
                throw new ArgumentNullException("AddressingState");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["AddressingUserId"] == null)
            {
                throw new ArgumentNullException("AddressingUserId");
            }
            if (Request["ProjectManagerId"] == null)
            {
                throw new ArgumentNullException("ProjectManagerId");
            }
            if (Request["ConstructionProgress"] == null)
            {
                throw new ArgumentNullException("ConstructionProgress");
            }

            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetConstructionPlanningsReportPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlanningCode"].Trim(), Request["PlanningName"].Trim(),
                    PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["Urgency"]),
                    int.Parse(Request["TelecomDemand"]), int.Parse(Request["MobileDemand"]), int.Parse(Request["UnicomDemand"]), int.Parse(Request["DemandState"]),
                    int.Parse(Request["Issued"]), int.Parse(Request["AddressingState"]), Guid.Parse(Request["CreateUserId"]), Request["PlaceName"].Trim(),
                    Guid.Parse(Request["AddressingUserId"]), Guid.Parse(Request["ProjectManagerId"]), int.Parse(Request["ConstructionProgress"])));
            }
        }
        #endregion

        #region 改造基站建设进度表
        /// <summary>
        /// 改造基站建设进度表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ConstructionRemodeingReport()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumUrgencyList = enumService.GetUrgencyEnum();
            IList<Dictionary<string, string>> enumPropertyRightList = enumService.GetPropertyRightEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Urgency"] = JsonHelper.Encode(enumUrgencyList);
            ViewData["PropertyRight"] = JsonHelper.Encode(enumPropertyRightList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumUrgencyList.Insert(0, allDict);
            enumConstructionProgressAllList.Insert(0, allDict);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["UrgencyByAll"] = JsonHelper.Encode(enumUrgencyList);
            ViewData["ConstructionProgressAll"] = JsonHelper.Encode(enumConstructionProgressAllList);

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumConstructionProgressList.Insert(0, nullDict);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            return View();
        }

        #endregion

        #region 改造站需求确认

        /// <summary>
        /// 改造站需求确认
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OperatorsPlanningDemand()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumDemandList = enumService.GetDemandEnum();
            ViewData["Demand"] = JsonHelper.Encode(enumDemandList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumDemandList.Insert(0, allDict);
            ViewData["DemandByAll"] = JsonHelper.Encode(enumDemandList);
            ViewData["DemandByConfirm"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页改造站需求确认列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOperatorsPlanningDemandsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlanningCode"] == null)
            {
                throw new ArgumentNullException("PlanningCode");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["Demand"] == null)
            {
                throw new ArgumentNullException("Demand");
            }

            using (ServiceProxy<IOperatorsPlanningDemandService> proxy = new ServiceProxy<IOperatorsPlanningDemandService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsPlanningDemandsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlanningCode"].Trim(), Request["PlanningName"].Trim(), PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]),
                    Guid.Parse(Request["ReseauId"]), int.Parse(Request["Demand"]), this.CompanyId));
            }
        }

        /// <summary>
        /// 需求确认
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OperatorsPlanningDemandConfirm()
        {
            if (Request["Demand"] == null)
            {
                throw new ArgumentNullException("Demand");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<OperatorsPlanningDemandMaintObject> operatorsPlanningDemandMaintObjects = new List<OperatorsPlanningDemandMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    operatorsPlanningDemandMaintObjects.Add(new OperatorsPlanningDemandMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IOperatorsPlanningDemandService> proxy = new ServiceProxy<IOperatorsPlanningDemandService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.OperatorsPlanningDemandConfirm(this.UserId, int.Parse(Request["Demand"]), operatorsPlanningDemandMaintObjects));
            }
            return this.Sucess("需求确认成功");
        }

        /// <summary>
        /// 根据规划获取关联的运营商规划列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOperatorsPlanningsByOperatorsPlanningDemandId()
        {
            if (Request["OperatorsPlanningDemandId"] == null)
            {
                throw new ArgumentNullException("OperatorsPlanningDemandId");
            }

            using (ServiceProxy<IOperatorsPlanningDemandService> proxy = new ServiceProxy<IOperatorsPlanningDemandService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsPlanningsByOperatorsPlanningDemandId(Guid.Parse(Request["OperatorsPlanningDemandId"])));
            }
        }

        #endregion

        #region 指定设计单位
        /// <summary>
        /// 指定设计单位
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointDesign(Guid id, Guid wfActivityInstanceId)
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
            ViewData["Demand"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingEditorObject addressingEditorObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingEditorById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingEditorObject.OrderCode;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["PlaceCode"] = addressingEditorObject.PlaceCode;
                ViewData["PlaceName"] = addressingEditorObject.PlaceName;
                ViewData["PlanningName"] = addressingEditorObject.PlanningName;
                ViewData["AreaName"] = addressingEditorObject.AreaName;
                ViewData["ReseauName"] = addressingEditorObject.ReseauName;
                ViewData["PlaceCategoryName"] = addressingEditorObject.PlaceCategoryName;
                ViewData["ImportanceName"] = addressingEditorObject.ImportanceName;
                ViewData["Lng"] = addressingEditorObject.Lng;
                ViewData["Lat"] = addressingEditorObject.Lat;
                ViewData["SceneName"] = addressingEditorObject.SceneName;
                ViewData["AddressingStateName"] = addressingEditorObject.AddressingStateName;
                ViewData["OwnerName"] = addressingEditorObject.OwnerName;
                ViewData["OwnerContact"] = addressingEditorObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = addressingEditorObject.OwnerPhoneNumber;
                ViewData["TelecomDemand"] = addressingEditorObject.TelecomDemand;
                ViewData["MobileDemand"] = addressingEditorObject.MobileDemand;
                ViewData["UnicomDemand"] = addressingEditorObject.UnicomDemand;
                ViewData["DetailedAddress"] = addressingEditorObject.DetailedAddress;
                ViewData["Remarks"] = addressingEditorObject.Remarks;
                ViewData["ProjectId"] = addressingEditorObject.ProjectId;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["ProjectManagerId"] = addressingEditorObject.ProjectManagerId;
                ViewData["ProjectManagerName"] = addressingEditorObject.ProjectManagerName;
                ViewData["PlaceId"] = addressingEditorObject.PlaceId;
                ViewData["PlanningId"] = addressingEditorObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["Count"] = addressingEditorObject.Count;
                ViewData["PlaceDesignId"] = addressingEditorObject.PlaceDesignId;
                ViewData["DesignCustomerId"] = addressingEditorObject.DesignCustomerId;
                ViewData["DesignCustomerName"] = addressingEditorObject.DesignCustomerName;
                ViewData["DesignUserId"] = addressingEditorObject.DesignUserId;
                ViewData["DesignUserName"] = addressingEditorObject.DesignUserName;
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAppointDesign()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceDesignMaintObject placeDesignMaintObject = null;
            //Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            placeDesignMaintObject = new PlaceDesignMaintObject()
            {
                Id = Guid.Parse(row["PlaceDesignId"].ToString()),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                DesignCustomerId = Guid.Parse(row["DesignCustomerId"].ToString()),
                //DesignUserId = Guid.Parse(row["DesignUserId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceDesignService> proxy = new ServiceProxy<IPlaceDesignService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveAppointDesign(placeDesignMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 指定设计人员
        /// <summary>
        /// 指定设计人员
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointDesignUser(Guid id, Guid wfActivityInstanceId)
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
            ViewData["Demand"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingEditorObject addressingEditorObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingEditorById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingEditorObject.OrderCode;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["PlaceCode"] = addressingEditorObject.PlaceCode;
                ViewData["PlaceName"] = addressingEditorObject.PlaceName;
                ViewData["PlanningName"] = addressingEditorObject.PlanningName;
                ViewData["AreaName"] = addressingEditorObject.AreaName;
                ViewData["ReseauName"] = addressingEditorObject.ReseauName;
                ViewData["PlaceCategoryName"] = addressingEditorObject.PlaceCategoryName;
                ViewData["ImportanceName"] = addressingEditorObject.ImportanceName;
                ViewData["Lng"] = addressingEditorObject.Lng;
                ViewData["Lat"] = addressingEditorObject.Lat;
                ViewData["SceneName"] = addressingEditorObject.SceneName;
                ViewData["AddressingStateName"] = addressingEditorObject.AddressingStateName;
                ViewData["OwnerName"] = addressingEditorObject.OwnerName;
                ViewData["OwnerContact"] = addressingEditorObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = addressingEditorObject.OwnerPhoneNumber;
                ViewData["TelecomDemand"] = addressingEditorObject.TelecomDemand;
                ViewData["MobileDemand"] = addressingEditorObject.MobileDemand;
                ViewData["UnicomDemand"] = addressingEditorObject.UnicomDemand;
                ViewData["DetailedAddress"] = addressingEditorObject.DetailedAddress;
                ViewData["Remarks"] = addressingEditorObject.Remarks;
                ViewData["ProjectId"] = addressingEditorObject.ProjectId;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["ProjectManagerId"] = addressingEditorObject.ProjectManagerId;
                ViewData["ProjectManagerName"] = addressingEditorObject.ProjectManagerName;
                ViewData["PlaceId"] = addressingEditorObject.PlaceId;
                ViewData["PlanningId"] = addressingEditorObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["Count"] = addressingEditorObject.Count;
                ViewData["PlaceDesignId"] = addressingEditorObject.PlaceDesignId;
                ViewData["DesignCustomerId"] = addressingEditorObject.DesignCustomerId;
                ViewData["DesignCustomerName"] = addressingEditorObject.DesignCustomerName;
                ViewData["DesignUserId"] = addressingEditorObject.DesignUserId;
                ViewData["DesignUserName"] = addressingEditorObject.DesignUserName;
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAppointDesignUser()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceDesignMaintObject placeDesignMaintObject = null;
            //Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            placeDesignMaintObject = new PlaceDesignMaintObject()
            {
                Id = Guid.Parse(row["PlaceDesignId"].ToString()),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                //DesignCustomerId = Guid.Parse(row["DesignCustomerId"].ToString()),
                DesignUserId = Guid.Parse(row["DesignUserId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceDesignService> proxy = new ServiceProxy<IPlaceDesignService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveAppointDesignUser(placeDesignMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 施工设计
        /// <summary>
        /// 施工设计
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ConstructionDesign(Guid id, Guid wfActivityInstanceId)
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
            ViewData["Demand"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));
            ViewData["CompanyId"] = this.CompanyId;

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "请选择");

            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            enumTowerTypeList.Insert(0, nullDict);
            ViewData["TowerTypeList"] = JsonHelper.Encode(enumTowerTypeList);

            IList<Dictionary<string, string>> enumTowerBaseTypeList = enumService.GetTowerBaseTypeEnum();
            enumTowerBaseTypeList.Insert(0, nullDict);
            ViewData["TowerBaseTypeList"] = JsonHelper.Encode(enumTowerBaseTypeList);


            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            enumMachineRoomTypeList.Insert(0, nullDict);
            ViewData["MachineRoomTypeList"] = JsonHelper.Encode(enumMachineRoomTypeList);


            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            enumExternalElectricList.Insert(0, nullDict);
            ViewData["ExternalElectricList"] = JsonHelper.Encode(enumExternalElectricList);

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingEditorObject addressingEditorObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingEditorById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingEditorObject.OrderCode;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["PlaceCode"] = addressingEditorObject.PlaceCode;
                ViewData["PlaceName"] = addressingEditorObject.PlaceName;
                ViewData["PlanningName"] = addressingEditorObject.PlanningName;
                ViewData["AreaName"] = addressingEditorObject.AreaName;
                ViewData["ReseauName"] = addressingEditorObject.ReseauName;
                ViewData["PlaceCategoryName"] = addressingEditorObject.PlaceCategoryName;
                ViewData["ImportanceName"] = addressingEditorObject.ImportanceName;
                ViewData["Lng"] = addressingEditorObject.Lng;
                ViewData["Lat"] = addressingEditorObject.Lat;
                ViewData["SceneName"] = addressingEditorObject.SceneName;
                ViewData["AddressingStateName"] = addressingEditorObject.AddressingStateName;
                ViewData["OwnerName"] = addressingEditorObject.OwnerName;
                ViewData["OwnerContact"] = addressingEditorObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = addressingEditorObject.OwnerPhoneNumber;
                ViewData["TelecomDemand"] = addressingEditorObject.TelecomDemand;
                ViewData["MobileDemand"] = addressingEditorObject.MobileDemand;
                ViewData["UnicomDemand"] = addressingEditorObject.UnicomDemand;
                ViewData["DetailedAddress"] = addressingEditorObject.DetailedAddress;
                ViewData["Remarks"] = addressingEditorObject.Remarks;
                ViewData["ProjectId"] = addressingEditorObject.ProjectId;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["ProjectManagerId"] = addressingEditorObject.ProjectManagerId;
                ViewData["ProjectManagerName"] = addressingEditorObject.ProjectManagerName;
                ViewData["PlaceId"] = addressingEditorObject.PlaceId;
                ViewData["PlanningId"] = addressingEditorObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["Count"] = addressingEditorObject.Count;
                ViewData["PlaceDesignId"] = addressingEditorObject.PlaceDesignId;
                ViewData["DesignCustomerId"] = addressingEditorObject.DesignCustomerId;
                ViewData["DesignCustomerName"] = addressingEditorObject.DesignCustomerName;
                ViewData["DesignUserId"] = addressingEditorObject.DesignUserId;
                ViewData["DesignUserName"] = addressingEditorObject.DesignUserName;
                ViewData["TowerCount"] = addressingEditorObject.TowerCount;
                ViewData["TowerBaseCount"] = addressingEditorObject.TowerBaseCount;
                ViewData["MachineRoomCount"] = addressingEditorObject.MachineRoomCount;
                ViewData["ExternalCount"] = addressingEditorObject.ExternalCount;
                ViewData["EquipmentInstallCount"] = addressingEditorObject.EquipmentInstallCount;
                ViewData["AddressCount"] = addressingEditorObject.AddressCount;
                ViewData["FoundationCount"] = addressingEditorObject.FoundationCount;
                ViewData["TowerMark"] = addressingEditorObject.TowerMark;
                ViewData["TowerBaseMark"] = addressingEditorObject.TowerBaseMark;
                ViewData["MachineRoomMark"] = addressingEditorObject.MachineRoomMark;
                ViewData["ExternalElectricPowerMark"] = addressingEditorObject.ExternalElectricPowerMark;
                ViewData["EquipmentInstallMark"] = addressingEditorObject.EquipmentInstallMark;
                ViewData["AddressExplorMark"] = addressingEditorObject.AddressExplorMark;
                ViewData["FoundationTestMark"] = addressingEditorObject.FoundationTestMark;
                ViewData["TowerType"] = addressingEditorObject.TowerType;
                ViewData["TowerHeight"] = addressingEditorObject.TowerHeight;
                ViewData["PlatFormNumber"] = addressingEditorObject.PlatFormNumber;
                ViewData["PoleNumber"] = addressingEditorObject.PoleNumber;
                ViewData["TowerBudget"] = addressingEditorObject.TowerBudget;
                ViewData["TowerBaseType"] = addressingEditorObject.TowerBaseType;
                ViewData["TowerBaseBudget"] = addressingEditorObject.TowerBaseBudget;
                ViewData["MachineRoomType"] = addressingEditorObject.MachineRoomType;
                ViewData["MachineRoomArea"] = addressingEditorObject.MachineRoomArea;
                ViewData["MachineRoomBudget"] = addressingEditorObject.MachineRoomBudget;
                ViewData["ExternalElectric"] = addressingEditorObject.ExternalElectric;
                ViewData["ExternalBudget"] = addressingEditorObject.ExternalBudget;
                ViewData["SwitchPower"] = addressingEditorObject.SwitchPower;
                ViewData["Battery"] = addressingEditorObject.Battery;
                ViewData["CabinetNumber"] = addressingEditorObject.CabinetNumber;
                ViewData["EquipmentBudget"] = addressingEditorObject.EquipmentBudget;
                ViewData["AddressBudget"] = addressingEditorObject.AddressBudget;
                ViewData["FoundationBudget"] = addressingEditorObject.FoundationBudget;
                ViewData["TowerId"] = addressingEditorObject.TowerId;
                ViewData["TowerBaseId"] = addressingEditorObject.TowerBaseId;
                ViewData["MachineRoomId"] = addressingEditorObject.MachineRoomId;
                ViewData["ExternalElectricPowerId"] = addressingEditorObject.ExternalElectricPowerId;
                ViewData["EquipmentInstallId"] = addressingEditorObject.EquipmentInstallId;
                ViewData["AddressExplorId"] = addressingEditorObject.AddressExplorId;
                ViewData["FoundationTestId"] = addressingEditorObject.FoundationTestId;
                ViewData["FileIdListTower"] = addressingEditorObject.TowerFileIdList;
                ViewData["FileIdListTowerBase"] = addressingEditorObject.TowerBaseFileIdList;
                ViewData["FileIdListMachineRoom"] = addressingEditorObject.MachineRoomFileIdList;
                ViewData["FileIdListExternal"] = addressingEditorObject.ExternalFileIdList;
                ViewData["FileIdListEquipmentInstall"] = addressingEditorObject.EquipmentInstallFileIdList;
                ViewData["FileIdListAddress"] = addressingEditorObject.AddressFileIdList;
                ViewData["FileIdListFoundation"] = addressingEditorObject.FoundationFileIdList;
                ViewData["TowerTimeLimit"] = addressingEditorObject.TowerTimeLimit;
                ViewData["TowerBaseTimeLimit"] = addressingEditorObject.TowerBaseTimeLimit;
                ViewData["MachineRoomTimeLimit"] = addressingEditorObject.MachineRoomTimeLimit;
                ViewData["ExternalTimeLimit"] = addressingEditorObject.ExternalTimeLimit;
                ViewData["EquipmentTimeLimit"] = addressingEditorObject.EquipmentTimeLimit;
                ViewData["AddressTimeLimit"] = addressingEditorObject.AddressTimeLimit;
                ViewData["FoundationTimeLimit"] = addressingEditorObject.FoundationTimeLimit;
            }
            using (ServiceProxy<IMaterialCategoryService> proxy = new ServiceProxy<IMaterialCategoryService>())
            {
                IList<MaterialCategoryMaintObject> materialCategoryMaintObjects = await Task.Factory.StartNew(() => proxy.Channel.GetMaterialCategorys());
                materialCategoryMaintObjects.Insert(0, new MaterialCategoryMaintObject() { Id = Guid.Empty, MaterialCategoryName = "请选择" });
                ViewData["MaterialCategoryIdsBySelect"] = JsonHelper.Encode(materialCategoryMaintObjects);
            }
            using (ServiceProxy<IMaterialService> proxy = new ServiceProxy<IMaterialService>())
            {
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveConstructionDesign()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            AddressingEditorObject addressingEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            addressingEditorObject = new AddressingEditorObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                TowerMark = int.Parse(row["TowerMark"].ToString()),
                TowerBaseMark = int.Parse(row["TowerBaseMark"].ToString()),
                MachineRoomMark = int.Parse(row["MachineRoomMark"].ToString()),
                ExternalElectricPowerMark = int.Parse(row["ExternalElectricPowerMark"].ToString()),
                EquipmentInstallMark = int.Parse(row["EquipmentInstallMark"].ToString()),
                AddressExplorMark = int.Parse(row["AddressExplorMark"].ToString()),
                FoundationTestMark = int.Parse(row["FoundationTestMark"].ToString()),
                TowerType = int.Parse(row["TowerType"].ToString()),
                TowerHeight = decimal.Parse(row["TowerHeight"].ToString()),
                PlatFormNumber = int.Parse(row["PlatFormNumber"].ToString()),
                PoleNumber = int.Parse(row["PoleNumber"].ToString()),
                TowerBudget = decimal.Parse(row["TowerBudget"].ToString()),
                TowerBaseType = int.Parse(row["TowerBaseType"].ToString()),
                TowerBaseBudget = decimal.Parse(row["TowerBaseBudget"].ToString()),
                MachineRoomType = int.Parse(row["MachineRoomType"].ToString()),
                MachineRoomArea = decimal.Parse(row["MachineRoomArea"].ToString()),
                MachineRoomBudget = decimal.Parse(row["MachineRoomBudget"].ToString()),
                ExternalElectric = int.Parse(row["ExternalElectric"].ToString()),
                ExternalBudget = decimal.Parse(row["ExternalBudget"].ToString()),
                SwitchPower = decimal.Parse(row["SwitchPower"].ToString()),
                Battery = decimal.Parse(row["Battery"].ToString()),
                CabinetNumber = int.Parse(row["CabinetNumber"].ToString()),
                EquipmentBudget = decimal.Parse(row["EquipmentBudget"].ToString()),
                AddressBudget = decimal.Parse(row["AddressBudget"].ToString()),
                FoundationBudget = decimal.Parse(row["FoundationBudget"].ToString()),
                TowerId = Guid.Parse(row["TowerId"].ToString()),
                TowerBaseId = Guid.Parse(row["TowerBaseId"].ToString()),
                MachineRoomId = Guid.Parse(row["MachineRoomId"].ToString()),
                ExternalElectricPowerId = Guid.Parse(row["ExternalElectricPowerId"].ToString()),
                EquipmentInstallId = Guid.Parse(row["EquipmentInstallId"].ToString()),
                AddressExplorId = Guid.Parse(row["AddressExplorId"].ToString()),
                FoundationTestId = Guid.Parse(row["FoundationTestId"].ToString()),
                TowerFileIdList = row["FileIdListTower"].ToString().Trim(),
                TowerBaseFileIdList = row["FileIdListTowerBase"].ToString().Trim(),
                MachineRoomFileIdList = row["FileIdListMachineRoom"].ToString().Trim(),
                EquipmentInstallFileIdList = row["FileIdListEquipmentInstall"].ToString().Trim(),
                ExternalFileIdList = row["FileIdListExternal"].ToString().Trim(),
                AddressFileIdList = row["FileIdListAddress"].ToString().Trim(),
                FoundationFileIdList = row["FileIdListFoundation"].ToString().Trim(),
                TowerTimeLimit = int.Parse(row["TowerTimeLimit"].ToString().Trim()),
                TowerBaseTimeLimit = int.Parse(row["TowerBaseTimeLimit"].ToString().Trim()),
                MachineRoomTimeLimit = int.Parse(row["MachineRoomTimeLimit"].ToString().Trim()),
                ExternalTimeLimit = int.Parse(row["ExternalTimeLimit"].ToString().Trim()),
                EquipmentTimeLimit = int.Parse(row["EquipmentTimeLimit"].ToString().Trim()),
                AddressTimeLimit = int.Parse(row["AddressTimeLimit"].ToString().Trim()),
                FoundationTimeLimit = int.Parse(row["FoundationTimeLimit"].ToString().Trim()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceDesignService> proxy = new ServiceProxy<IPlaceDesignService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveConstructionDesign(addressingEditorObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 获取物资清单
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMaterialList()
        {
            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetMaterialList(Guid.Parse(Request["ParentId"]), 1));
            }
        }

        public async Task<ActionResult> GetMaterialListById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMaterialListById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存物资清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveMaterialList()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            MaterialListMaintObject materialListMaintObject = null;
            Guid id = Guid.Parse((row["MaterialListId"] == null || row["MaterialListId"].ToString() == "" ? Guid.Empty : row["MaterialListId"]).ToString());
            if (id == Guid.Empty)
            {
                materialListMaintObject = new MaterialListMaintObject()
                {
                    Id = Guid.Empty,
                    ParentId = Guid.Parse(row["ParentId"].ToString()),
                    PropertyType = 1,
                    MaterialId = Guid.Parse(row["MaterialId"].ToString()),
                    MaterialSpecId = Guid.Parse(row["MaterialSpecId"].ToString()),
                    //BudgetPrice = decimal.Parse(row["BudgetPrice"].ToString()),
                    BudgetPrice = 0,
                    SpecNumber = decimal.Parse(row["SpecNumber"].ToString()),
                    Memos = row["Memos"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                materialListMaintObject = new MaterialListMaintObject()
                {
                    Id = id,
                    MaterialId = Guid.Parse(row["MaterialId"].ToString()),
                    MaterialSpecId = Guid.Parse(row["MaterialSpecId"].ToString()),
                    //BudgetPrice = decimal.Parse(row["BudgetPrice"].ToString()),
                    BudgetPrice = 0,
                    SpecNumber = decimal.Parse(row["SpecNumber"].ToString()),
                    Memos = row["Memos"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateMaterialList(materialListMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除物资清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveMaterialList()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<MaterialListMaintObject> materialListMaintObjects = new List<MaterialListMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["MaterialListId"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    materialListMaintObjects.Add(new MaterialListMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveMaterialList(materialListMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }
        #endregion

        #region 运营商再次确认
        /// <summary>
        /// 运营商再次确认
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> OperatorConfirm(Guid id, Guid wfActivityInstanceId)
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
            ViewData["Demand"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));
            ViewData["CompanyId"] = this.CompanyId;

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingEditorObject addressingEditorObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingEditorById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingEditorObject.OrderCode;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["PlaceCode"] = addressingEditorObject.PlaceCode;
                ViewData["PlaceName"] = addressingEditorObject.PlaceName;
                ViewData["PlanningName"] = addressingEditorObject.PlanningName;
                ViewData["AreaName"] = addressingEditorObject.AreaName;
                ViewData["ReseauName"] = addressingEditorObject.ReseauName;
                ViewData["PlaceCategoryName"] = addressingEditorObject.PlaceCategoryName;
                ViewData["ImportanceName"] = addressingEditorObject.ImportanceName;
                ViewData["Lng"] = addressingEditorObject.Lng;
                ViewData["Lat"] = addressingEditorObject.Lat;
                ViewData["SceneName"] = addressingEditorObject.SceneName;
                ViewData["AddressingStateName"] = addressingEditorObject.AddressingStateName;
                ViewData["OwnerName"] = addressingEditorObject.OwnerName;
                ViewData["OwnerContact"] = addressingEditorObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = addressingEditorObject.OwnerPhoneNumber;
                ViewData["TelecomDemand"] = addressingEditorObject.TelecomDemand;
                ViewData["MobileDemand"] = addressingEditorObject.MobileDemand;
                ViewData["UnicomDemand"] = addressingEditorObject.UnicomDemand;
                ViewData["DetailedAddress"] = addressingEditorObject.DetailedAddress;
                ViewData["Remarks"] = addressingEditorObject.Remarks;
                ViewData["ProjectId"] = addressingEditorObject.ProjectId;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["ProjectManagerId"] = addressingEditorObject.ProjectManagerId;
                ViewData["ProjectManagerName"] = addressingEditorObject.ProjectManagerName;
                ViewData["PlaceId"] = addressingEditorObject.PlaceId;
                ViewData["PlanningId"] = addressingEditorObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["Count"] = addressingEditorObject.Count;
                ViewData["TelecomPoleNumber"] = addressingEditorObject.TelecomPoleNumber;
                ViewData["TelecomCabinetNumber"] = addressingEditorObject.TelecomCabinetNumber;
                ViewData["TelecomPowerUsed"] = addressingEditorObject.TelecomPowerUsed;
                ViewData["MobilePoleNumber"] = addressingEditorObject.MobilePoleNumber;
                ViewData["MobileCabinetNumber"] = addressingEditorObject.MobileCabinetNumber;
                ViewData["MobilePowerUsed"] = addressingEditorObject.MobilePowerUsed;
                ViewData["UnicomPoleNumber"] = addressingEditorObject.UnicomPoleNumber;
                ViewData["UnicomCabinetNumber"] = addressingEditorObject.UnicomCabinetNumber;
                ViewData["UnicomPowerUsed"] = addressingEditorObject.UnicomPowerUsed;
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveOperatorConfirm()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            AddressingMaintObject addressingMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            addressingMaintObject = new AddressingMaintObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                //MobileDemand = int.Parse(row["MobileDemand"].ToString()),
                //TelecomDemand = int.Parse(row["TelecomDemand"].ToString()),
                //UnicomDemand = int.Parse(row["UnicomDemand"].ToString()),
                //TelecomPoleNumber = int.Parse(row["TelecomPoleNumber"].ToString()),
                //TelecomCabinetNumber = int.Parse(row["TelecomCabinetNumber"].ToString()),
                //TelecomPowerUsed = decimal.Parse(row["TelecomPowerUsed"].ToString()),
                //MobilePoleNumber = int.Parse(row["MobilePoleNumber"].ToString()),
                //MobileCabinetNumber = int.Parse(row["MobileCabinetNumber"].ToString()),
                //MobilePowerUsed = decimal.Parse(row["MobilePowerUsed"].ToString()),
                //UnicomPoleNumber = int.Parse(row["UnicomPoleNumber"].ToString()),
                //UnicomCabinetNumber = int.Parse(row["UnicomCabinetNumber"].ToString()),
                //UnicomPowerUsed = decimal.Parse(row["UnicomPowerUsed"].ToString()),
                CompanyId = this.CompanyId,
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveOperatorConfirm(addressingMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 指定项目及站点编码
        /// <summary>
        /// 指定项目及站点编码
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointProjectAndPlaceCode(Guid id, Guid wfActivityInstanceId)
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
            ViewData["Demand"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingEditorObject addressingEditorObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingEditorById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingEditorObject.OrderCode;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["PlaceCode"] = addressingEditorObject.PlaceCode;
                ViewData["PlaceName"] = addressingEditorObject.PlaceName;
                ViewData["PlanningName"] = addressingEditorObject.PlanningName;
                ViewData["AreaName"] = addressingEditorObject.AreaName;
                ViewData["ReseauName"] = addressingEditorObject.ReseauName;
                ViewData["PlaceCategoryName"] = addressingEditorObject.PlaceCategoryName;
                ViewData["ImportanceName"] = addressingEditorObject.ImportanceName;
                ViewData["Lng"] = addressingEditorObject.Lng;
                ViewData["Lat"] = addressingEditorObject.Lat;
                ViewData["SceneName"] = addressingEditorObject.SceneName;
                ViewData["AddressingStateName"] = addressingEditorObject.AddressingStateName;
                ViewData["OwnerName"] = addressingEditorObject.OwnerName;
                ViewData["OwnerContact"] = addressingEditorObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = addressingEditorObject.OwnerPhoneNumber;
                ViewData["TelecomDemand"] = addressingEditorObject.TelecomDemand;
                ViewData["MobileDemand"] = addressingEditorObject.MobileDemand;
                ViewData["UnicomDemand"] = addressingEditorObject.UnicomDemand;
                ViewData["DetailedAddress"] = addressingEditorObject.DetailedAddress;
                ViewData["Remarks"] = addressingEditorObject.Remarks;
                ViewData["ProjectId"] = addressingEditorObject.ProjectId;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["ProjectManagerId"] = addressingEditorObject.ProjectManagerId;
                ViewData["ProjectManagerName"] = addressingEditorObject.ProjectManagerName;
                ViewData["PlaceId"] = addressingEditorObject.PlaceId;
                ViewData["PlanningId"] = addressingEditorObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["Count"] = addressingEditorObject.Count;
                ViewData["ProjectCode"] = addressingEditorObject.ProjectCode;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["BudgetPrice"] = addressingEditorObject.BudgetPrice;
                ViewData["GroupPlaceCode"] = addressingEditorObject.GroupPlaceCode;
                ViewData["ProjectIsApply"] = addressingEditorObject.ProjectIsApply;
                ViewData["ProjectIsDoApply"] = addressingEditorObject.ProjectIsDoApply;
                ViewData["ProjectApplyDate"] = addressingEditorObject.ProjectApplyDate;
                ViewData["ProjectDoApplyDate"] = addressingEditorObject.ProjectDoApplyDate;
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProjectAndPlaceCode()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            AddressingMaintObject addressingMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            addressingMaintObject = new AddressingMaintObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                //ProjectCode = row["ProjectCode"].ToString().Trim(),
                //ProjectName = row["ProjectName"].ToString().Trim(),
                //GroupPlaceCode = row["GroupPlaceCode"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveProjectAndPlaceCode(addressingMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 项目申请立项
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveApplyProject()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            AddressingEditorObject addressingEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            addressingEditorObject = new AddressingEditorObject()
            {
                Id = id,
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveApplyProject(addressingEditorObject));
            }
            return this.Sucess("申请立项成功");
        }

        /// <summary>
        /// 项目完成立项
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveDoApplyProject()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            AddressingEditorObject addressingEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            addressingEditorObject = new AddressingEditorObject()
            {
                Id = id,
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveDoApplyProject(addressingEditorObject));
            }
            return this.Sucess("完成立项成功");
        }
        #endregion

        #region 指定施工单位及供应商
        /// <summary>
        /// 指定施工单位及供应商
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointCustomer(Guid id, Guid wfActivityInstanceId)
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
            ViewData["Demand"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));
            ViewData["CompanyId"] = this.CompanyId;

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "请选择");

            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            enumTowerTypeList.Insert(0, nullDict);
            ViewData["TowerTypeList"] = JsonHelper.Encode(enumTowerTypeList);

            IList<Dictionary<string, string>> enumTowerBaseTypeList = enumService.GetTowerBaseTypeEnum();
            enumTowerBaseTypeList.Insert(0, nullDict);
            ViewData["TowerBaseTypeList"] = JsonHelper.Encode(enumTowerBaseTypeList);


            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            enumMachineRoomTypeList.Insert(0, nullDict);
            ViewData["MachineRoomTypeList"] = JsonHelper.Encode(enumMachineRoomTypeList);


            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            enumExternalElectricList.Insert(0, nullDict);
            ViewData["ExternalElectricList"] = JsonHelper.Encode(enumExternalElectricList);

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingEditorObject addressingEditorObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingEditorById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingEditorObject.OrderCode;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["PlaceCode"] = addressingEditorObject.PlaceCode;
                ViewData["PlaceName"] = addressingEditorObject.PlaceName;
                ViewData["PlanningName"] = addressingEditorObject.PlanningName;
                ViewData["AreaName"] = addressingEditorObject.AreaName;
                ViewData["ReseauName"] = addressingEditorObject.ReseauName;
                ViewData["PlaceCategoryName"] = addressingEditorObject.PlaceCategoryName;
                ViewData["ImportanceName"] = addressingEditorObject.ImportanceName;
                ViewData["Lng"] = addressingEditorObject.Lng;
                ViewData["Lat"] = addressingEditorObject.Lat;
                ViewData["SceneName"] = addressingEditorObject.SceneName;
                ViewData["AddressingStateName"] = addressingEditorObject.AddressingStateName;
                ViewData["OwnerName"] = addressingEditorObject.OwnerName;
                ViewData["OwnerContact"] = addressingEditorObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = addressingEditorObject.OwnerPhoneNumber;
                ViewData["TelecomDemand"] = addressingEditorObject.TelecomDemand;
                ViewData["MobileDemand"] = addressingEditorObject.MobileDemand;
                ViewData["UnicomDemand"] = addressingEditorObject.UnicomDemand;
                ViewData["DetailedAddress"] = addressingEditorObject.DetailedAddress;
                ViewData["Remarks"] = addressingEditorObject.Remarks;
                ViewData["ProjectId"] = addressingEditorObject.ProjectId;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["ProjectManagerId"] = addressingEditorObject.ProjectManagerId;
                ViewData["ProjectManagerName"] = addressingEditorObject.ProjectManagerName;
                ViewData["PlaceId"] = addressingEditorObject.PlaceId;
                ViewData["PlanningId"] = addressingEditorObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["Count"] = addressingEditorObject.Count;
                ViewData["PlaceDesignId"] = addressingEditorObject.PlaceDesignId;
                ViewData["DesignCustomerId"] = addressingEditorObject.DesignCustomerId;
                ViewData["DesignCustomerName"] = addressingEditorObject.DesignCustomerName;
                ViewData["DesignUserId"] = addressingEditorObject.DesignUserId;
                ViewData["DesignUserName"] = addressingEditorObject.DesignUserName;
                ViewData["TowerCount"] = addressingEditorObject.TowerCount;
                ViewData["TowerBaseCount"] = addressingEditorObject.TowerBaseCount;
                ViewData["MachineRoomCount"] = addressingEditorObject.MachineRoomCount;
                ViewData["ExternalCount"] = addressingEditorObject.ExternalCount;
                ViewData["EquipmentInstallCount"] = addressingEditorObject.EquipmentInstallCount;
                ViewData["AddressCount"] = addressingEditorObject.AddressCount;
                ViewData["FoundationCount"] = addressingEditorObject.FoundationCount;
                ViewData["TowerMark"] = addressingEditorObject.TowerMark;
                ViewData["TowerBaseMark"] = addressingEditorObject.TowerBaseMark;
                ViewData["MachineRoomMark"] = addressingEditorObject.MachineRoomMark;
                ViewData["ExternalElectricPowerMark"] = addressingEditorObject.ExternalElectricPowerMark;
                ViewData["EquipmentInstallMark"] = addressingEditorObject.EquipmentInstallMark;
                ViewData["AddressExplorMark"] = addressingEditorObject.AddressExplorMark;
                ViewData["FoundationTestMark"] = addressingEditorObject.FoundationTestMark;
                ViewData["TowerType"] = addressingEditorObject.TowerType;
                ViewData["TowerHeight"] = addressingEditorObject.TowerHeight;
                ViewData["PlatFormNumber"] = addressingEditorObject.PlatFormNumber;
                ViewData["PoleNumber"] = addressingEditorObject.PoleNumber;
                ViewData["TowerBudget"] = addressingEditorObject.TowerBudget;
                ViewData["TowerBaseType"] = addressingEditorObject.TowerBaseType;
                ViewData["TowerBaseBudget"] = addressingEditorObject.TowerBaseBudget;
                ViewData["MachineRoomType"] = addressingEditorObject.MachineRoomType;
                ViewData["MachineRoomArea"] = addressingEditorObject.MachineRoomArea;
                ViewData["MachineRoomBudget"] = addressingEditorObject.MachineRoomBudget;
                ViewData["ExternalElectric"] = addressingEditorObject.ExternalElectric;
                ViewData["ExternalBudget"] = addressingEditorObject.ExternalBudget;
                ViewData["SwitchPower"] = addressingEditorObject.SwitchPower;
                ViewData["Battery"] = addressingEditorObject.Battery;
                ViewData["CabinetNumber"] = addressingEditorObject.CabinetNumber;
                ViewData["EquipmentBudget"] = addressingEditorObject.EquipmentBudget;
                ViewData["AddressBudget"] = addressingEditorObject.AddressBudget;
                ViewData["FoundationBudget"] = addressingEditorObject.FoundationBudget;
                ViewData["TowerId"] = addressingEditorObject.TowerId;
                ViewData["TowerBaseId"] = addressingEditorObject.TowerBaseId;
                ViewData["MachineRoomId"] = addressingEditorObject.MachineRoomId;
                ViewData["ExternalElectricPowerId"] = addressingEditorObject.ExternalElectricPowerId;
                ViewData["EquipmentInstallId"] = addressingEditorObject.EquipmentInstallId;
                ViewData["AddressExplorId"] = addressingEditorObject.AddressExplorId;
                ViewData["FoundationTestId"] = addressingEditorObject.FoundationTestId;
                ViewData["TowerCustomerId"] = addressingEditorObject.TowerCustomerId;
                ViewData["TowerCustomerName"] = addressingEditorObject.TowerCustomerName;
                ViewData["TowerBaseCustomerId"] = addressingEditorObject.TowerBaseCustomerId;
                ViewData["TowerBaseCustomerName"] = addressingEditorObject.TowerBaseCustomerName;
                ViewData["MachineRoomCustomerId"] = addressingEditorObject.MachineRoomCustomerId;
                ViewData["MachineRoomCustomerName"] = addressingEditorObject.MachineRoomCustomerName;
                ViewData["ExternalCustomerId"] = addressingEditorObject.ExternalCustomerId;
                ViewData["ExternalCustomerName"] = addressingEditorObject.ExternalCustomerName;
                ViewData["EquipmentCustomerId"] = addressingEditorObject.EquipmentCustomerId;
                ViewData["EquipmentCustomerName"] = addressingEditorObject.EquipmentCustomerName;
                ViewData["AddressCustomerId"] = addressingEditorObject.AddressCustomerId;
                ViewData["AddressCustomerName"] = addressingEditorObject.AddressCustomerName;
                ViewData["FoundationCustomerId"] = addressingEditorObject.FoundationCustomerId;
                ViewData["FoundationCustomerName"] = addressingEditorObject.FoundationCustomerName;
                ViewData["TowerTimeLimit"] = addressingEditorObject.TowerTimeLimit;
                ViewData["TowerBaseTimeLimit"] = addressingEditorObject.TowerBaseTimeLimit;
                ViewData["MachineRoomTimeLimit"] = addressingEditorObject.MachineRoomTimeLimit;
                ViewData["ExternalTimeLimit"] = addressingEditorObject.ExternalTimeLimit;
                ViewData["EquipmentTimeLimit"] = addressingEditorObject.EquipmentTimeLimit;
                ViewData["AddressTimeLimit"] = addressingEditorObject.AddressTimeLimit;
                ViewData["FoundationTimeLimit"] = addressingEditorObject.FoundationTimeLimit;

            }
            using (ServiceProxy<IMaterialCategoryService> proxy = new ServiceProxy<IMaterialCategoryService>())
            {
                IList<MaterialCategoryMaintObject> materialCategoryMaintObjects = await Task.Factory.StartNew(() => proxy.Channel.GetMaterialCategorys());
                materialCategoryMaintObjects.Insert(0, new MaterialCategoryMaintObject() { Id = Guid.Empty, MaterialCategoryName = "请选择" });
                ViewData["MaterialCategoryIdsBySelect"] = JsonHelper.Encode(materialCategoryMaintObjects);
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveCustomer()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            AddressingEditorObject addressingEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            addressingEditorObject = new AddressingEditorObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                TowerCustomerId = Guid.Parse(row["TowerCustomerId"].ToString()),
                TowerBaseCustomerId = Guid.Parse(row["TowerBaseCustomerId"].ToString()),
                MachineRoomCustomerId = Guid.Parse(row["MachineRoomCustomerId"].ToString()),
                ExternalCustomerId = Guid.Parse(row["ExternalCustomerId"].ToString()),
                EquipmentCustomerId = Guid.Parse(row["EquipmentCustomerId"].ToString()),
                AddressCustomerId = Guid.Parse(row["AddressCustomerId"].ToString()),
                FoundationCustomerId = Guid.Parse(row["FoundationCustomerId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceDesignService> proxy = new ServiceProxy<IPlaceDesignService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveCustomer(addressingEditorObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 获取物资清单
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMaterialListCustomer()
        {
            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetMaterialList(Guid.Parse(Request["ParentId"]), 1));
            }
        }

        public async Task<ActionResult> GetMaterialListByIdCustomer(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMaterialListById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存物资清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveMaterialListCustomer()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            MaterialListMaintObject materialListMaintObject = null;
            Guid id = Guid.Parse((row["MaterialListId"] == null || row["MaterialListId"].ToString() == "" ? Guid.Empty : row["MaterialListId"]).ToString());
            materialListMaintObject = new MaterialListMaintObject()
            {
                Id = id,
                MaterialSpecId = Guid.Parse(row["MaterialSpecId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveMaterialSpec(materialListMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 指定工程经理及监理单位
        /// <summary>
        /// 指定工程经理及监理单位
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointManagerAndSupervisor(Guid id, Guid wfActivityInstanceId)
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
            ViewData["Demand"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                AddressingEditorObject addressingEditorObject = await Task.Factory.StartNew(() => proxy.Channel.GetAddressingEditorById(id));
                ViewData["Id"] = id;
                ViewData["OrderCode"] = addressingEditorObject.OrderCode;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["PlaceCode"] = addressingEditorObject.PlaceCode;
                ViewData["PlaceName"] = addressingEditorObject.PlaceName;
                ViewData["PlanningName"] = addressingEditorObject.PlanningName;
                ViewData["AreaName"] = addressingEditorObject.AreaName;
                ViewData["ReseauName"] = addressingEditorObject.ReseauName;
                ViewData["PlaceCategoryName"] = addressingEditorObject.PlaceCategoryName;
                ViewData["ImportanceName"] = addressingEditorObject.ImportanceName;
                ViewData["Lng"] = addressingEditorObject.Lng;
                ViewData["Lat"] = addressingEditorObject.Lat;
                ViewData["SceneName"] = addressingEditorObject.SceneName;
                ViewData["AddressingStateName"] = addressingEditorObject.AddressingStateName;
                ViewData["OwnerName"] = addressingEditorObject.OwnerName;
                ViewData["OwnerContact"] = addressingEditorObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = addressingEditorObject.OwnerPhoneNumber;
                ViewData["TelecomDemand"] = addressingEditorObject.TelecomDemand;
                ViewData["MobileDemand"] = addressingEditorObject.MobileDemand;
                ViewData["UnicomDemand"] = addressingEditorObject.UnicomDemand;
                ViewData["DetailedAddress"] = addressingEditorObject.DetailedAddress;
                ViewData["Remarks"] = addressingEditorObject.Remarks;
                ViewData["ProjectId"] = addressingEditorObject.ProjectId;
                ViewData["ProjectName"] = addressingEditorObject.ProjectName;
                ViewData["PlaceId"] = addressingEditorObject.PlaceId;
                ViewData["PlanningId"] = addressingEditorObject.PlanningId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["Count"] = addressingEditorObject.Count;
                ViewData["PlaceDesignId"] = addressingEditorObject.PlaceDesignId;
                ViewData["SupervisorCustomerId"] = addressingEditorObject.SupervisorCustomerId;
                ViewData["SupervisorCustomerName"] = addressingEditorObject.SupervisorCustomerName;
                ViewData["ProjectManagerId"] = addressingEditorObject.ProjectManagerId;
                ViewData["ProjectManagerName"] = addressingEditorObject.ProjectManagerName;
            }
            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveManagerAndSupervisor()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceDesignMaintObject placeDesignMaintObject = null;
            //Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            placeDesignMaintObject = new PlaceDesignMaintObject()
            {
                Id = Guid.Parse(row["PlaceDesignId"].ToString()),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                SupervisorCustomerId = Guid.Parse(row["SupervisorCustomerId"].ToString()),
                ProjectManagerId = Guid.Parse(row["ProjectManagerId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceDesignService> proxy = new ServiceProxy<IPlaceDesignService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveManagerAndSupervisor(placeDesignMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 共享站指定设计单位
        /// <summary>
        /// 共享站指定设计单位
        /// </summary>
        /// <param name="id">基站改造Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointDesignR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }

            ViewData["CompanyId"] = this.CompanyId;


            return View();
        }

        /// <summary>
        /// 保存寻址确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAppointDesignR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceDesignMaintObject placeDesignMaintObject = null;
            //Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            placeDesignMaintObject = new PlaceDesignMaintObject()
            {
                Id = Guid.Parse(row["PlaceDesignId"].ToString()),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                DesignCustomerId = Guid.Parse(row["DesignCustomerId"].ToString()),
                //DesignUserId = Guid.Parse(row["DesignUserId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceDesignService> proxy = new ServiceProxy<IPlaceDesignService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveAppointDesignR(placeDesignMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 共享站指定设计人员
        /// <summary>
        /// 共享站指定设计人员
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointDesignUserR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }


            return View();
        }

        /// <summary>
        /// 指定设计人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAppointDesignUserR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceDesignMaintObject placeDesignMaintObject = null;
            //Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            placeDesignMaintObject = new PlaceDesignMaintObject()
            {
                Id = Guid.Parse(row["PlaceDesignId"].ToString()),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                //DesignCustomerId = Guid.Parse(row["DesignCustomerId"].ToString()),
                DesignUserId = Guid.Parse(row["DesignUserId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceDesignService> proxy = new ServiceProxy<IPlaceDesignService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveAppointDesignUserR(placeDesignMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 共享站施工设计
        /// <summary>
        /// 共享站施工设计
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ConstructionDesignR(Guid id, Guid wfActivityInstanceId)
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

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "请选择");

            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            enumTowerTypeList.Insert(0, nullDict);
            ViewData["TowerTypeList"] = JsonHelper.Encode(enumTowerTypeList);

            IList<Dictionary<string, string>> enumTowerBaseTypeList = enumService.GetTowerBaseTypeEnum();
            enumTowerBaseTypeList.Insert(0, nullDict);
            ViewData["TowerBaseTypeList"] = JsonHelper.Encode(enumTowerBaseTypeList);


            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            enumMachineRoomTypeList.Insert(0, nullDict);
            ViewData["MachineRoomTypeList"] = JsonHelper.Encode(enumMachineRoomTypeList);


            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            enumExternalElectricList.Insert(0, nullDict);
            ViewData["ExternalElectricList"] = JsonHelper.Encode(enumExternalElectricList);


            using (ServiceProxy<IMaterialCategoryService> proxy = new ServiceProxy<IMaterialCategoryService>())
            {
                IList<MaterialCategoryMaintObject> materialCategoryMaintObjects = await Task.Factory.StartNew(() => proxy.Channel.GetMaterialCategorys());
                materialCategoryMaintObjects.Insert(0, new MaterialCategoryMaintObject() { Id = Guid.Empty, MaterialCategoryName = "请选择" });
                ViewData["MaterialCategoryIdsBySelect"] = JsonHelper.Encode(materialCategoryMaintObjects);
            }
            using (ServiceProxy<IMaterialService> proxy = new ServiceProxy<IMaterialService>())
            {
            }
            return View();
        }

        /// <summary>
        /// 保存改造确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveConstructionDesignR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            RemodelingEditorObject remodelingEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            remodelingEditorObject = new RemodelingEditorObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                TowerMark = int.Parse(row["TowerMark"].ToString()),
                TowerBaseMark = int.Parse(row["TowerBaseMark"].ToString()),
                MachineRoomMark = int.Parse(row["MachineRoomMark"].ToString()),
                ExternalElectricPowerMark = int.Parse(row["ExternalElectricPowerMark"].ToString()),
                EquipmentInstallMark = int.Parse(row["EquipmentInstallMark"].ToString()),
                AddressExplorMark = int.Parse(row["AddressExplorMark"].ToString()),
                FoundationTestMark = int.Parse(row["FoundationTestMark"].ToString()),
                TowerType = int.Parse(row["TowerType"].ToString()),
                TowerHeight = decimal.Parse(row["TowerHeight"].ToString()),
                PlatFormNumber = int.Parse(row["PlatFormNumber"].ToString()),
                PoleNumber = int.Parse(row["PoleNumber"].ToString()),
                TowerBudget = decimal.Parse(row["TowerBudget"].ToString()),
                TowerBaseType = int.Parse(row["TowerBaseType"].ToString()),
                TowerBaseBudget = decimal.Parse(row["TowerBaseBudget"].ToString()),
                MachineRoomType = int.Parse(row["MachineRoomType"].ToString()),
                MachineRoomArea = decimal.Parse(row["MachineRoomArea"].ToString()),
                MachineRoomBudget = decimal.Parse(row["MachineRoomBudget"].ToString()),
                ExternalElectric = int.Parse(row["ExternalElectric"].ToString()),
                ExternalBudget = decimal.Parse(row["ExternalBudget"].ToString()),
                SwitchPower = decimal.Parse(row["SwitchPower"].ToString()),
                Battery = decimal.Parse(row["Battery"].ToString()),
                CabinetNumber = int.Parse(row["CabinetNumber"].ToString()),
                EquipmentBudget = decimal.Parse(row["EquipmentBudget"].ToString()),
                AddressBudget = decimal.Parse(row["AddressBudget"].ToString()),
                FoundationBudget = decimal.Parse(row["FoundationBudget"].ToString()),
                TowerId = Guid.Parse(row["TowerId"].ToString()),
                TowerBaseId = Guid.Parse(row["TowerBaseId"].ToString()),
                MachineRoomId = Guid.Parse(row["MachineRoomId"].ToString()),
                ExternalElectricPowerId = Guid.Parse(row["ExternalElectricPowerId"].ToString()),
                EquipmentInstallId = Guid.Parse(row["EquipmentInstallId"].ToString()),
                AddressExplorId = Guid.Parse(row["AddressExplorId"].ToString()),
                FoundationTestId = Guid.Parse(row["FoundationTestId"].ToString()),
                TowerFileIdList = row["FileIdListTower"].ToString().Trim(),
                TowerBaseFileIdList = row["FileIdListTowerBase"].ToString().Trim(),
                MachineRoomFileIdList = row["FileIdListMachineRoom"].ToString().Trim(),
                ExternalFileIdList = row["FileIdListExternal"].ToString().Trim(),
                EquipmentInstallFileIdList = row["FileIdListEquipmentInstall"].ToString().Trim(),
                AddressFileIdList = row["FileIdListAddress"].ToString().Trim(),
                FoundationFileIdList = row["FileIdListFoundation"].ToString().Trim(),
                TowerTimeLimit = int.Parse(row["TowerTimeLimit"].ToString().Trim()),
                TowerBaseTimeLimit = int.Parse(row["TowerBaseTimeLimit"].ToString().Trim()),
                MachineRoomTimeLimit = int.Parse(row["MachineRoomTimeLimit"].ToString().Trim()),
                ExternalTimeLimit = int.Parse(row["ExternalTimeLimit"].ToString().Trim()),
                EquipmentTimeLimit = int.Parse(row["EquipmentTimeLimit"].ToString().Trim()),
                AddressTimeLimit = int.Parse(row["AddressTimeLimit"].ToString().Trim()),
                FoundationTimeLimit = int.Parse(row["FoundationTimeLimit"].ToString().Trim()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceDesignService> proxy = new ServiceProxy<IPlaceDesignService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveConstructionDesignR(remodelingEditorObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 获取物资清单
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMaterialListR()
        {
            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetMaterialList(Guid.Parse(Request["ParentId"]), 2));
            }
        }

        /// <summary>
        /// 根据物资清单Id获取物资清单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetMaterialListByIdR(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMaterialListById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存物资清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveMaterialListR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            MaterialListMaintObject materialListMaintObject = null;
            Guid id = Guid.Parse((row["MaterialListId"] == null || row["MaterialListId"].ToString() == "" ? Guid.Empty : row["MaterialListId"]).ToString());
            if (id == Guid.Empty)
            {
                materialListMaintObject = new MaterialListMaintObject()
                {
                    Id = Guid.Empty,
                    ParentId = Guid.Parse(row["ParentId"].ToString()),
                    PropertyType = 2,
                    MaterialId = Guid.Parse(row["MaterialId"].ToString()),
                    MaterialSpecId = Guid.Parse(row["MaterialSpecId"].ToString()),
                    //BudgetPrice = decimal.Parse(row["BudgetPrice"].ToString()),
                    BudgetPrice = 0,
                    SpecNumber = decimal.Parse(row["SpecNumber"].ToString()),
                    Memos = row["Memos"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                materialListMaintObject = new MaterialListMaintObject()
                {
                    Id = id,
                    MaterialId = Guid.Parse(row["MaterialId"].ToString()),
                    MaterialSpecId = Guid.Parse(row["MaterialSpecId"].ToString()),
                    //BudgetPrice = decimal.Parse(row["BudgetPrice"].ToString()),
                    BudgetPrice = 0,
                    SpecNumber = decimal.Parse(row["SpecNumber"].ToString()),
                    Memos = row["Memos"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateMaterialList(materialListMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除物资清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveMaterialListR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<MaterialListMaintObject> materialListMaintObjects = new List<MaterialListMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["MaterialListId"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    materialListMaintObjects.Add(new MaterialListMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveMaterialList(materialListMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }
        #endregion

        #region 共享站指定项目
        /// <summary>
        /// 共享站指定项目
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointProjectR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }


            return View();
        }

        #endregion

        #region 共享站指定施工单位及供应商
        /// <summary>
        /// 共享站指定施工单位及供应商
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointCustomerR(Guid id, Guid wfActivityInstanceId)
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

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "请选择");

            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            enumTowerTypeList.Insert(0, nullDict);
            ViewData["TowerTypeList"] = JsonHelper.Encode(enumTowerTypeList);

            IList<Dictionary<string, string>> enumTowerBaseTypeList = enumService.GetTowerBaseTypeEnum();
            enumTowerBaseTypeList.Insert(0, nullDict);
            ViewData["TowerBaseTypeList"] = JsonHelper.Encode(enumTowerBaseTypeList);


            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            enumMachineRoomTypeList.Insert(0, nullDict);
            ViewData["MachineRoomTypeList"] = JsonHelper.Encode(enumMachineRoomTypeList);


            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            enumExternalElectricList.Insert(0, nullDict);
            ViewData["ExternalElectricList"] = JsonHelper.Encode(enumExternalElectricList);


            using (ServiceProxy<IMaterialCategoryService> proxy = new ServiceProxy<IMaterialCategoryService>())
            {
                IList<MaterialCategoryMaintObject> materialCategoryMaintObjects = await Task.Factory.StartNew(() => proxy.Channel.GetMaterialCategorys());
                materialCategoryMaintObjects.Insert(0, new MaterialCategoryMaintObject() { Id = Guid.Empty, MaterialCategoryName = "请选择" });
                ViewData["MaterialCategoryIdsBySelect"] = JsonHelper.Encode(materialCategoryMaintObjects);
            }
            return View();
        }

        /// <summary>
        /// 保存改造确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveCustomerR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            RemodelingEditorObject remodelingEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            remodelingEditorObject = new RemodelingEditorObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                TowerCustomerId = Guid.Parse(row["TowerCustomerId"].ToString()),
                TowerBaseCustomerId = Guid.Parse(row["TowerBaseCustomerId"].ToString()),
                MachineRoomCustomerId = Guid.Parse(row["MachineRoomCustomerId"].ToString()),
                ExternalCustomerId = Guid.Parse(row["ExternalCustomerId"].ToString()),
                EquipmentCustomerId = Guid.Parse(row["EquipmentCustomerId"].ToString()),
                AddressCustomerId = Guid.Parse(row["AddressCustomerId"].ToString()),
                FoundationCustomerId = Guid.Parse(row["FoundationCustomerId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceDesignService> proxy = new ServiceProxy<IPlaceDesignService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveCustomerR(remodelingEditorObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 获取物资清单
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMaterialListCustomerR()
        {
            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetMaterialList(Guid.Parse(Request["ParentId"]), 2));
            }
        }

        public async Task<ActionResult> GetMaterialListByIdCustomerR(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMaterialListById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存物资清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveMaterialListCustomerR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            MaterialListMaintObject materialListMaintObject = null;
            Guid id = Guid.Parse((row["MaterialListId"] == null || row["MaterialListId"].ToString() == "" ? Guid.Empty : row["MaterialListId"]).ToString());
            materialListMaintObject = new MaterialListMaintObject()
            {
                Id = id,
                MaterialSpecId = Guid.Parse(row["MaterialSpecId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IMaterialListService> proxy = new ServiceProxy<IMaterialListService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveMaterialSpec(materialListMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 共享站指定工程经理及监理单位
        /// <summary>
        /// 共享站指定工程经理及监理单位
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> AppointManagerAndSupervisorR(Guid id, Guid wfActivityInstanceId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (wfActivityInstanceId == null)
            {
                throw new ArgumentNullException("wfActivityInstanceId");
            }


            return View();
        }

        /// <summary>
        /// 保存改造确认编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveManagerAndSupervisorR()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceDesignMaintObject placeDesignMaintObject = null;
            //Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            placeDesignMaintObject = new PlaceDesignMaintObject()
            {
                Id = Guid.Parse(row["PlaceDesignId"].ToString()),
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                SupervisorCustomerId = Guid.Parse(row["SupervisorCustomerId"].ToString()),
                ProjectManagerId = Guid.Parse(row["ProjectManagerId"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlaceDesignService> proxy = new ServiceProxy<IPlaceDesignService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveManagerAndSupervisorR(placeDesignMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 项目管理
        /// <summary>
        /// 项目管理
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ProjectManagement()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumConstructionMethodList = enumService.GetConstructionMethodEnum();

            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumImportanceList.Insert(0, nullDict);
            //enumConstructionProgressList.Insert(0, nullDict);
            enumConstructionMethodList.Insert(0, nullDict);

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);
            ViewData["ConstructionMethod"] = JsonHelper.Encode(enumConstructionMethodList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumConstructionProgressAllList.Insert(0, allDict);

            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "6");
            Dict1.Add("text", "未完成");
            enumConstructionProgressAllList.Insert(6, Dict1);

            ViewData["ConstructionProgressByAll"] = JsonHelper.Encode(enumConstructionProgressAllList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取建设任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetConstructionTasksPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["CustomerId"] == null)
            {
                throw new ArgumentNullException("CustomerId");
            }
            if (Request["ConstructionProgress"] == null)
            {
                throw new ArgumentNullException("ConstructionProgress");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetConstructionTasksPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    Guid.Parse(Request["CustomerId"]), int.Parse(Request["ConstructionProgress"]), this.UserId));
            }
        }

        /// <summary>
        /// 获取建设任务维护实体
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetConstructionTaskById()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetConstructionTaskById(Guid.Parse(Request["Id"]))), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改建设任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveConstructionTaskProgress()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ConstructionTaskEditorObject constructionTaskEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            constructionTaskEditorObject = new ConstructionTaskEditorObject()
            {
                Id = id,
                ConstructionProgress = int.Parse(row["ConstructionProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveConstructionTaskProgress(constructionTaskEditorObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 工程管理
        /// <summary>
        /// 工程管理
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> EngineeringManagement()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumConstructionMethodList = enumService.GetConstructionMethodEnum();
            IList<Dictionary<string, string>> enumTaskModelList = enumService.GetTaskModelEnum();
            IList<Dictionary<string, string>> enumSubmitStateList = enumService.GetSubmitStateEnum();

            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumImportanceList.Insert(0, nullDict);
            //enumConstructionProgressList.Insert(0, nullDict);
            enumConstructionMethodList.Insert(0, nullDict);

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);
            ViewData["ConstructionMethod"] = JsonHelper.Encode(enumConstructionMethodList);
            ViewData["TaskModel"] = JsonHelper.Encode(enumTaskModelList);
            ViewData["SubmitState"] = JsonHelper.Encode(enumSubmitStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumConstructionProgressAllList.Insert(0, allDict);
            enumTaskModelList.Insert(0, allDict);

            ViewData["TaskModelByAll"] = JsonHelper.Encode(enumTaskModelList);

            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "6");
            Dict1.Add("text", "未完成");
            enumConstructionProgressAllList.Insert(6, Dict1);

            ViewData["ConstructionProgressByAll"] = JsonHelper.Encode(enumConstructionProgressAllList);

            Dictionary<string, string> Dict2 = new Dictionary<string, string>(2);
            Dict2.Add("id", "0");
            Dict2.Add("text", "请选择");

            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            enumTowerTypeList.Insert(0, Dict2);
            ViewData["TowerTypeList"] = JsonHelper.Encode(enumTowerTypeList);

            IList<Dictionary<string, string>> enumTowerBaseTypeList = enumService.GetTowerBaseTypeEnum();
            enumTowerBaseTypeList.Insert(0, Dict2);
            ViewData["TowerBaseTypeList"] = JsonHelper.Encode(enumTowerBaseTypeList);


            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            enumMachineRoomTypeList.Insert(0, Dict2);
            ViewData["MachineRoomTypeList"] = JsonHelper.Encode(enumMachineRoomTypeList);


            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            enumExternalElectricList.Insert(0, Dict2);
            ViewData["ExternalElectricList"] = JsonHelper.Encode(enumExternalElectricList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取建设任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTaskPropertysPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["CustomerId"] == null)
            {
                throw new ArgumentNullException("CustomerId");
            }
            if (Request["ConstructionProgress"] == null)
            {
                throw new ArgumentNullException("ConstructionProgress");
            }
            if (Request["TaskModel"] == null)
            {
                throw new ArgumentNullException("TaskModel");
            }
            if (Request["SupervisorCustomerId"] == null)
            {
                throw new ArgumentNullException("SupervisorCustomerId");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetTaskPropertysPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    Guid.Parse(Request["CustomerId"]), int.Parse(Request["ConstructionProgress"]), int.Parse(Request["TaskModel"]), Guid.Parse(Request["SupervisorCustomerId"]), this.UserId));
            }
        }

        /// <summary>
        /// 获取建设任务维护实体
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetTaskPropertyById()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            using (ServiceProxy<ITaskPropertyService> proxy = new ServiceProxy<ITaskPropertyService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetTaskPropertyById(Guid.Parse(Request["Id"]))), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改建设任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTaskProperty1()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            TaskPropertyMaintObject taskPropertyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            taskPropertyMaintObject = new TaskPropertyMaintObject()
            {
                Id = id,
                ConstructionProgress = int.Parse(row["ConstructionProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                FileIdList = row["FileIdList1"].ToString().Trim(),
                SubmitState = int.Parse(row["SubmitState"].ToString()),
                TowerType = int.Parse(row["TowerType"].ToString()),
                TowerHeight = decimal.Parse(row["TowerHeight"].ToString()),
                PlatFormNumber = int.Parse(row["PlatFormNumber"].ToString()),
                PoleNumber = int.Parse(row["PoleNumber"].ToString()),
                TowerBudget = decimal.Parse(row["TowerBudget"].ToString()),
                FileIdListTower = row["FileIdListTower"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<ITaskPropertyService> proxy = new ServiceProxy<ITaskPropertyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveTaskProperty(taskPropertyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 修改建设任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTaskProperty2()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            TaskPropertyMaintObject taskPropertyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            taskPropertyMaintObject = new TaskPropertyMaintObject()
            {
                Id = id,
                ConstructionProgress = int.Parse(row["ConstructionProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                FileIdList = row["FileIdList2"].ToString().Trim(),
                SubmitState = int.Parse(row["SubmitState"].ToString()),
                TowerBaseType = int.Parse(row["TowerBaseType"].ToString()),
                TowerBaseBudget = decimal.Parse(row["TowerBaseBudget"].ToString()),
                FileIdListTowerBase = row["FileIdListTowerBase"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<ITaskPropertyService> proxy = new ServiceProxy<ITaskPropertyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveTaskProperty(taskPropertyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 修改建设任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTaskProperty3()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            TaskPropertyMaintObject taskPropertyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            taskPropertyMaintObject = new TaskPropertyMaintObject()
            {
                Id = id,
                ConstructionProgress = int.Parse(row["ConstructionProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                FileIdList = row["FileIdList3"].ToString().Trim(),
                SubmitState = int.Parse(row["SubmitState"].ToString()),
                MachineRoomType = int.Parse(row["MachineRoomType"].ToString()),
                MachineRoomArea = decimal.Parse(row["MachineRoomArea"].ToString()),
                MachineRoomBudget = decimal.Parse(row["MachineRoomBudget"].ToString()),
                FileIdListMachineRoom = row["FileIdListMachineRoom"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<ITaskPropertyService> proxy = new ServiceProxy<ITaskPropertyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveTaskProperty(taskPropertyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 修改建设任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTaskProperty4()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            TaskPropertyMaintObject taskPropertyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            taskPropertyMaintObject = new TaskPropertyMaintObject()
            {
                Id = id,
                ConstructionProgress = int.Parse(row["ConstructionProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                FileIdList = row["FileIdList4"].ToString().Trim(),
                SubmitState = int.Parse(row["SubmitState"].ToString()),
                ExternalElectric = int.Parse(row["ExternalElectric"].ToString()),
                ExternalBudget = decimal.Parse(row["ExternalBudget"].ToString()),
                FileIdListExternal = row["FileIdListExternal"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<ITaskPropertyService> proxy = new ServiceProxy<ITaskPropertyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveTaskProperty(taskPropertyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 修改建设任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTaskProperty5()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            TaskPropertyMaintObject taskPropertyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            taskPropertyMaintObject = new TaskPropertyMaintObject()
            {
                Id = id,
                ConstructionProgress = int.Parse(row["ConstructionProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                FileIdList = row["FileIdList5"].ToString().Trim(),
                SubmitState = int.Parse(row["SubmitState"].ToString()),
                SwitchPower = decimal.Parse(row["SwitchPower"].ToString()),
                Battery = decimal.Parse(row["Battery"].ToString()),
                CabinetNumber = int.Parse(row["CabinetNumber"].ToString()),
                EquipmentBudget = decimal.Parse(row["EquipmentBudget"].ToString()),
                FileIdListEquipmentInstall = row["FileIdListEquipmentInstall"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<ITaskPropertyService> proxy = new ServiceProxy<ITaskPropertyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveTaskProperty(taskPropertyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 修改建设任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTaskProperty6()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            TaskPropertyMaintObject taskPropertyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            taskPropertyMaintObject = new TaskPropertyMaintObject()
            {
                Id = id,
                ConstructionProgress = int.Parse(row["ConstructionProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                FileIdList = row["FileIdList6"].ToString().Trim(),
                SubmitState = int.Parse(row["SubmitState"].ToString()),
                AddressBudget = decimal.Parse(row["AddressBudget"].ToString()),
                FileIdListAddress = row["FileIdListAddress"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<ITaskPropertyService> proxy = new ServiceProxy<ITaskPropertyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveTaskProperty(taskPropertyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 修改建设任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTaskProperty7()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            TaskPropertyMaintObject taskPropertyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            taskPropertyMaintObject = new TaskPropertyMaintObject()
            {
                Id = id,
                ConstructionProgress = int.Parse(row["ConstructionProgress"].ToString()),
                ProgressMemos = row["ProgressMemos"].ToString().Trim(),
                FileIdList = row["FileIdList7"].ToString().Trim(),
                SubmitState = int.Parse(row["SubmitState"].ToString()),
                FoundationBudget = decimal.Parse(row["FoundationBudget"].ToString()),
                FileIdListFoundation = row["FileIdListFoundation"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<ITaskPropertyService> proxy = new ServiceProxy<ITaskPropertyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveTaskProperty(taskPropertyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 站点维护

        /// <summary>
        /// 站点维护
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PlaceMaintenance()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProfessionList = enumService.GetProfessionEnum();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumPropertyRightList = enumService.GetPropertyRightEnum();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumStateList = enumService.GetStateEnum();

            ViewData["Profession"] = JsonHelper.Encode(enumProfessionList);
            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["PropertyRight"] = JsonHelper.Encode(enumPropertyRightList);
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["State"] = JsonHelper.Encode(enumStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumProfessionList.Insert(0, allDict);
            enumImportanceList.Insert(0, allDict);
            enumPropertyRightList.Insert(0, allDict);
            enumBoolList.Insert(0, allDict);
            enumStateList.Insert(0, allDict);

            ViewData["ProfessionByAll"] = JsonHelper.Encode(enumProfessionList);
            ViewData["ImportanceByAll"] = JsonHelper.Encode(enumImportanceList);
            ViewData["PropertyRightByAll"] = JsonHelper.Encode(enumPropertyRightList);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["StateByAll"] = JsonHelper.Encode(enumStateList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据站点Id获取站点
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetPlaceById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlaceById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页站点列表
        /// </summary>
        /// <returns></returns>
        //public async Task<string> GetPlacesPage()
        //{
        //    if (Request["PageIndex"] == null)
        //    {
        //        throw new ArgumentNullException("PageIndex");
        //    }
        //    if (Request["PageSize"] == null)
        //    {
        //        throw new ArgumentNullException("PageSize");
        //    }
        //    if (Request["PlaceCode"] == null)
        //    {
        //        throw new ArgumentNullException("PlaceCode");
        //    }
        //    if (Request["PlaceName"] == null)
        //    {
        //        throw new ArgumentNullException("PlaceName");
        //    }
        //    if (Request["Profession"] == null)
        //    {
        //        throw new ArgumentNullException("Profession");
        //    }
        //    if (Request["PlaceCategoryId"] == null)
        //    {
        //        throw new ArgumentNullException("PlaceCategoryId");
        //    }
        //    if (Request["AreaId"] == null)
        //    {
        //        throw new ArgumentNullException("AreaId");
        //    }
        //    if (Request["ReseauId"] == null)
        //    {
        //        throw new ArgumentNullException("ReseauId");
        //    }
        //    if (Request["PropertyRight"] == null)
        //    {
        //        throw new ArgumentNullException("PropertyRight");
        //    }
        //    if (Request["Importance"] == null)
        //    {
        //        throw new ArgumentNullException("Importance");
        //    }
        //    if (Request["TelecomShare"] == null)
        //    {
        //        throw new ArgumentNullException("TelecomShare");
        //    }
        //    if (Request["MobileShare"] == null)
        //    {
        //        throw new ArgumentNullException("MobileShare");
        //    }
        //    if (Request["UnicomShare"] == null)
        //    {
        //        throw new ArgumentNullException("UnicomShare");
        //    }
        //    if (Request["State"] == null)
        //    {
        //        throw new ArgumentNullException("State");
        //    }

        //    using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
        //    {
        //        return await Task.Factory.StartNew(() => proxy.Channel.GetPlacesPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
        //            Request["PlaceCode"].Trim(), Request["PlaceName"].Trim(), int.Parse(Request["Profession"]), Guid.Parse(Request["PlaceCategoryId"]),
        //            Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["PropertyRight"]), int.Parse(Request["Importance"]),
        //            int.Parse(Request["TelecomShare"]), int.Parse(Request["MobileShare"]), int.Parse(Request["UnicomShare"]), int.Parse(Request["State"])));
        //    }
        //}

        /// <summary>
        /// 获取分页站点列表，用于选择站点
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlacesPageBySelect()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceCode"] == null)
            {
                throw new ArgumentNullException("PlaceCode");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["PropertyRight"] == null)
            {
                throw new ArgumentNullException("PropertyRight");
            }
            if (Request["TelecomShare"] == null)
            {
                throw new ArgumentNullException("TelecomShare");
            }
            if (Request["MobileShare"] == null)
            {
                throw new ArgumentNullException("MobileShare");
            }
            if (Request["UnicomShare"] == null)
            {
                throw new ArgumentNullException("UnicomShare");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlacesPageBySelect(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceCode"].Trim(), Request["PlaceName"].Trim(), int.Parse(Request["Profession"]), Guid.Parse(Request["PlaceCategoryId"]),
                    Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"])));
            }
        }

        /// <summary>
        /// 保存站点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePlace()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            PlaceMaintObject placeMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id != Guid.Empty)
            {
                placeMaintObject = new PlaceMaintObject()
                {
                    Id = id,
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    //PropertyRight = int.Parse(row["PropertyRight"].ToString()),
                    Importance = int.Parse(row["Importance"].ToString()),
                    //SceneId = Guid.Parse(row["SceneId"].ToString()),
                    DetailedAddress = row["DetailedAddress"].ToString().Trim(),
                    OwnerName = row["OwnerName"].ToString().Trim(),
                    OwnerContact = row["OwnerContact"].ToString().Trim(),
                    OwnerPhoneNumber = row["OwnerPhoneNumber"].ToString().Trim(),
                    //TelecomShare = bool.Parse(row["TelecomShare"].ToString()) ? 1 : 2,
                    //MobileShare = bool.Parse(row["MobileShare"].ToString()) ? 1 : 2,
                    //UnicomShare = bool.Parse(row["UnicomShare"].ToString()) ? 1 : 2,
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.UpdatePlace(placeMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 资源维护
        /// <summary>
        /// 资源维护
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ResourceMaintenance(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["Demand"] = JsonHelper.Encode(enumService.GetDemandEnum("2,3"));
            ViewData["CompanyId"] = this.CompanyId;

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "请选择");

            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            enumTowerTypeList.Insert(0, nullDict);
            ViewData["TowerTypeList"] = JsonHelper.Encode(enumTowerTypeList);

            IList<Dictionary<string, string>> enumTowerBaseTypeList = enumService.GetTowerBaseTypeEnum();
            enumTowerBaseTypeList.Insert(0, nullDict);
            ViewData["TowerBaseTypeList"] = JsonHelper.Encode(enumTowerBaseTypeList);


            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            enumMachineRoomTypeList.Insert(0, nullDict);
            ViewData["MachineRoomTypeList"] = JsonHelper.Encode(enumMachineRoomTypeList);


            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            enumExternalElectricList.Insert(0, nullDict);
            ViewData["ExternalElectricList"] = JsonHelper.Encode(enumExternalElectricList);

            using (ServiceProxy<IPlacePropertyService> proxy = new ServiceProxy<IPlacePropertyService>())
            {
                ResourceMaintObject resourceMaintObject = await Task.Factory.StartNew(() => proxy.Channel.GetResourceMaintenanceById(id));
                ViewData["Id"] = id;
                ViewData["TowerCount"] = resourceMaintObject.TowerCount;
                ViewData["TowerBaseCount"] = resourceMaintObject.TowerBaseCount;
                ViewData["MachineRoomCount"] = resourceMaintObject.MachineRoomCount;
                ViewData["ExternalCount"] = resourceMaintObject.ExternalCount;
                ViewData["EquipmentInstallCount"] = resourceMaintObject.EquipmentInstallCount;
                ViewData["AddressCount"] = resourceMaintObject.AddressCount;
                ViewData["FoundationCount"] = resourceMaintObject.FoundationCount;
                ViewData["TowerMark"] = resourceMaintObject.TowerMark;
                ViewData["TowerBaseMark"] = resourceMaintObject.TowerBaseMark;
                ViewData["MachineRoomMark"] = resourceMaintObject.MachineRoomMark;
                ViewData["ExternalElectricPowerMark"] = resourceMaintObject.ExternalElectricPowerMark;
                ViewData["EquipmentInstallMark"] = resourceMaintObject.EquipmentInstallMark;
                ViewData["AddressExplorMark"] = resourceMaintObject.AddressExplorMark;
                ViewData["FoundationTestMark"] = resourceMaintObject.FoundationTestMark;
                ViewData["TowerType"] = resourceMaintObject.TowerType;
                ViewData["TowerHeight"] = resourceMaintObject.TowerHeight;
                ViewData["PlatFormNumber"] = resourceMaintObject.PlatFormNumber;
                ViewData["PoleNumber"] = resourceMaintObject.PoleNumber;
                ViewData["TowerBaseType"] = resourceMaintObject.TowerBaseType;
                ViewData["MachineRoomType"] = resourceMaintObject.MachineRoomType;
                ViewData["MachineRoomArea"] = resourceMaintObject.MachineRoomArea;
                ViewData["ExternalElectric"] = resourceMaintObject.ExternalElectric;
                ViewData["SwitchPower"] = resourceMaintObject.SwitchPower;
                ViewData["Battery"] = resourceMaintObject.Battery;
                ViewData["CabinetNumber"] = resourceMaintObject.CabinetNumber;
                ViewData["TowerId"] = resourceMaintObject.TowerId;
                ViewData["TowerBaseId"] = resourceMaintObject.TowerBaseId;
                ViewData["MachineRoomId"] = resourceMaintObject.MachineRoomId;
                ViewData["ExternalElectricPowerId"] = resourceMaintObject.ExternalElectricPowerId;
                ViewData["EquipmentInstallId"] = resourceMaintObject.EquipmentInstallId;
                ViewData["AddressExplorId"] = resourceMaintObject.AddressExplorId;
                ViewData["FoundationTestId"] = resourceMaintObject.FoundationTestId;
                ViewData["FileIdListTower"] = resourceMaintObject.TowerFileIdList;
                ViewData["FileIdListTowerBase"] = resourceMaintObject.TowerBaseFileIdList;
                ViewData["FileIdListMachineRoom"] = resourceMaintObject.MachineRoomFileIdList;
                ViewData["FileIdListExternal"] = resourceMaintObject.ExternalFileIdList;
                ViewData["FileIdListEquipmentInstall"] = resourceMaintObject.EquipmentInstallFileIdList;
                ViewData["FileIdListAddress"] = resourceMaintObject.AddressFileIdList;
                ViewData["FileIdListFoundation"] = resourceMaintObject.FoundationFileIdList;
                ViewData["TelecomPoleNumber"] = resourceMaintObject.TelecomPoleNumber;
                ViewData["TelecomCabinetNumber"] = resourceMaintObject.TelecomCabinetNumber;
                ViewData["TelecomPowerUsed"] = resourceMaintObject.TelecomPowerUsed;
                ViewData["MobilePoleNumber"] = resourceMaintObject.MobilePoleNumber;
                ViewData["MobileCabinetNumber"] = resourceMaintObject.MobileCabinetNumber;
                ViewData["MobilePowerUsed"] = resourceMaintObject.MobilePowerUsed;
                ViewData["UnicomPoleNumber"] = resourceMaintObject.UnicomPoleNumber;
                ViewData["UnicomCabinetNumber"] = resourceMaintObject.UnicomCabinetNumber;
                ViewData["UnicomPowerUsed"] = resourceMaintObject.UnicomPowerUsed;
                ViewData["MobileShare"] = resourceMaintObject.MobileShare;
                ViewData["TelecomShare"] = resourceMaintObject.TelecomShare;
                ViewData["UnicomShare"] = resourceMaintObject.UnicomShare;

            }
            return View();
        }

        /// <summary>
        /// 保存资源
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveResourceMaintenance()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ResourceMaintObject resourceMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            resourceMaintObject = new ResourceMaintObject()
            {
                Id = id,
                TowerMark = int.Parse(row["TowerMark"].ToString()),
                TowerBaseMark = int.Parse(row["TowerBaseMark"].ToString()),
                MachineRoomMark = int.Parse(row["MachineRoomMark"].ToString()),
                ExternalElectricPowerMark = int.Parse(row["ExternalElectricPowerMark"].ToString()),
                EquipmentInstallMark = int.Parse(row["EquipmentInstallMark"].ToString()),
                AddressExplorMark = int.Parse(row["AddressExplorMark"].ToString()),
                FoundationTestMark = int.Parse(row["FoundationTestMark"].ToString()),
                TowerType = int.Parse(row["TowerType"].ToString()),
                TowerHeight = decimal.Parse(row["TowerHeight"].ToString()),
                PlatFormNumber = int.Parse(row["PlatFormNumber"].ToString()),
                PoleNumber = int.Parse(row["PoleNumber"].ToString()),
                TowerBaseType = int.Parse(row["TowerBaseType"].ToString()),
                MachineRoomType = int.Parse(row["MachineRoomType"].ToString()),
                MachineRoomArea = decimal.Parse(row["MachineRoomArea"].ToString()),
                ExternalElectric = int.Parse(row["ExternalElectric"].ToString()),
                SwitchPower = decimal.Parse(row["SwitchPower"].ToString()),
                Battery = decimal.Parse(row["Battery"].ToString()),
                CabinetNumber = int.Parse(row["CabinetNumber"].ToString()),
                TowerId = Guid.Parse(row["TowerId"].ToString()),
                TowerBaseId = Guid.Parse(row["TowerBaseId"].ToString()),
                MachineRoomId = Guid.Parse(row["MachineRoomId"].ToString()),
                ExternalElectricPowerId = Guid.Parse(row["ExternalElectricPowerId"].ToString()),
                EquipmentInstallId = Guid.Parse(row["EquipmentInstallId"].ToString()),
                AddressExplorId = Guid.Parse(row["AddressExplorId"].ToString()),
                FoundationTestId = Guid.Parse(row["FoundationTestId"].ToString()),
                TowerFileIdList = row["FileIdListTower"].ToString().Trim(),
                TowerBaseFileIdList = row["FileIdListTowerBase"].ToString().Trim(),
                MachineRoomFileIdList = row["FileIdListMachineRoom"].ToString().Trim(),
                EquipmentInstallFileIdList = row["FileIdListEquipmentInstall"].ToString().Trim(),
                ExternalFileIdList = row["FileIdListExternal"].ToString().Trim(),
                AddressFileIdList = row["FileIdListAddress"].ToString().Trim(),
                FoundationFileIdList = row["FileIdListFoundation"].ToString().Trim(),
                MobilePoleNumber = int.Parse(row["MobilePoleNumber"].ToString()),
                MobileCabinetNumber = int.Parse(row["MobileCabinetNumber"].ToString()),
                MobilePowerUsed = decimal.Parse(row["MobilePowerUsed"].ToString()),
                TelecomPoleNumber = int.Parse(row["TelecomPoleNumber"].ToString()),
                TelecomCabinetNumber = int.Parse(row["TelecomCabinetNumber"].ToString()),
                TelecomPowerUsed = decimal.Parse(row["TelecomPowerUsed"].ToString()),
                UnicomPoleNumber = int.Parse(row["UnicomPoleNumber"].ToString()),
                UnicomCabinetNumber = int.Parse(row["UnicomCabinetNumber"].ToString()),
                UnicomPowerUsed = decimal.Parse(row["UnicomPowerUsed"].ToString()),
                MobileShare = int.Parse(row["MobileShare"].ToString()),
                TelecomShare = int.Parse(row["TelecomShare"].ToString()),
                UnicomShare = int.Parse(row["UnicomShare"].ToString()),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IPlacePropertyService> proxy = new ServiceProxy<IPlacePropertyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveResourceMaintenance(resourceMaintObject));
            }
            return this.Sucess("数据保存成功");
        }
        #endregion

        #region 任务卡片
        /// <summary>
        /// 任务卡片
        /// </summary>
        /// <param name="id">任务Id</param>
        /// <returns></returns>
        public async Task<ActionResult> TaskCard(Guid Id, Guid PlaceId)
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["PlaceId"] == null)
            {
                throw new ArgumentNullException("PlaceId");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                PlaceInfoObject placeInfoObject = await Task.Factory.StartNew(() => proxy.Channel.GetPlaceInfoById(PlaceId));
                ViewData["PlaceId"] = PlaceId;
                ViewData["PlaceCode"] = placeInfoObject.PlaceCode;
                ViewData["PlaceName"] = placeInfoObject.PlaceName;
                ViewData["AreaName"] = placeInfoObject.AreaName;
                ViewData["ReseauName"] = placeInfoObject.ReseauName;
                ViewData["PlaceCategoryName"] = placeInfoObject.PlaceCategoryName;
                ViewData["ImportanceName"] = placeInfoObject.ImportanceName;
                ViewData["Lng"] = placeInfoObject.Lng;
                ViewData["Lat"] = placeInfoObject.Lat;
                ViewData["AddressingDepartmentName"] = placeInfoObject.AddressingDepartmentName;
                ViewData["AddressingRealName"] = placeInfoObject.AddressingRealName;
                ViewData["PlaceOwnerName"] = placeInfoObject.PlaceOwnerName;
                ViewData["OwnerName"] = placeInfoObject.OwnerName;
                ViewData["OwnerContact"] = placeInfoObject.OwnerContact;
                ViewData["OwnerPhoneNumber"] = placeInfoObject.OwnerPhoneNumber;
                ViewData["DetailedAddress"] = placeInfoObject.DetailedAddress;
                ViewData["Remarks"] = placeInfoObject.Remarks;
                ViewData["G2Number"] = placeInfoObject.G2Number;
                ViewData["D2Number"] = placeInfoObject.D2Number;
                ViewData["G3Number"] = placeInfoObject.G3Number;
                ViewData["G4Number"] = placeInfoObject.G4Number;
                ViewData["G5Number"] = placeInfoObject.G5Number;
                ViewData["CreateUserName"] = placeInfoObject.CreateUserName;
                ViewData["CreateDate"] = placeInfoObject.CreateDate;
                ViewData["Count"] = placeInfoObject.Count;
            }
            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                ProjectTaskEditObject projectTaskEditObject = await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskById(Id));
                ViewData["Id"] = projectTaskEditObject.Id;
                ViewData["AreaManagerName"] = projectTaskEditObject.AreaManagerName;
                ViewData["GeneralDesignName"] = projectTaskEditObject.GeneralDesignName;
                ViewData["DesignRealName"] = projectTaskEditObject.DesignRealName;
                ViewData["DesignDateText"] = projectTaskEditObject.DesignDateText;
                ViewData["ProjectCode"] = projectTaskEditObject.ProjectCode;
                ViewData["ProjectProgressName"] = projectTaskEditObject.ProjectProgressName;
                ViewData["ProjectBeginDateText"] = projectTaskEditObject.ProjectBeginDateText;
                ViewData["ProjectDateText"] = projectTaskEditObject.ProjectDateText;
                ViewData["ProgressMemos"] = projectTaskEditObject.ProgressMemos;
                ViewData["DesignCount"] = projectTaskEditObject.DesignCount;
                ViewData["ImageCount"] = projectTaskEditObject.ImageCount;
                ViewData["Mark1"] = projectTaskEditObject.Mark1;
                ViewData["Id1"] = projectTaskEditObject.Id1;
                ViewData["ProjectManagerName1"] = projectTaskEditObject.ProjectManagerName1;
                ViewData["DesignCustomerName1"] = projectTaskEditObject.DesignCustomerName1;
                ViewData["ConstructionCustomerName1"] = projectTaskEditObject.ConstructionCustomerName1;
                ViewData["SupervisionCustomerName1"] = projectTaskEditObject.SupervisionCustomerName1;
                ViewData["DesignRealName1"] = projectTaskEditObject.DesignRealName1;
                ViewData["DesignCount1"] = projectTaskEditObject.DesignCount1;
                ViewData["DesignDateText1"] = projectTaskEditObject.DesignDateText1;
                ViewData["DesignStateName1"] = projectTaskEditObject.DesignStateName1;
                ViewData["EngineeringProgressName1"] = projectTaskEditObject.EngineeringProgressName1;
                ViewData["ImageCount1"] = projectTaskEditObject.ImageCount1;
                ViewData["DesignMemos1"] = projectTaskEditObject.DesignMemos1;
                ViewData["ProgressMemos1"] = projectTaskEditObject.ProgressMemos1;
                ViewData["Mark2"] = projectTaskEditObject.Mark2;
                ViewData["Id2"] = projectTaskEditObject.Id2;
                ViewData["ProjectManagerName2"] = projectTaskEditObject.ProjectManagerName2;
                ViewData["DesignCustomerName2"] = projectTaskEditObject.DesignCustomerName2;
                ViewData["ConstructionCustomerName2"] = projectTaskEditObject.ConstructionCustomerName2;
                ViewData["SupervisionCustomerName2"] = projectTaskEditObject.SupervisionCustomerName2;
                ViewData["DesignRealName2"] = projectTaskEditObject.DesignRealName2;
                ViewData["DesignCount2"] = projectTaskEditObject.DesignCount2;
                ViewData["DesignDateText2"] = projectTaskEditObject.DesignDateText2;
                ViewData["DesignStateName2"] = projectTaskEditObject.DesignStateName2;
                ViewData["EngineeringProgressName2"] = projectTaskEditObject.EngineeringProgressName2;
                ViewData["ImageCount2"] = projectTaskEditObject.ImageCount2;
                ViewData["DesignMemos2"] = projectTaskEditObject.DesignMemos2;
                ViewData["ProgressMemos2"] = projectTaskEditObject.ProgressMemos2;
                ViewData["Mark3"] = projectTaskEditObject.Mark3;
                ViewData["Id3"] = projectTaskEditObject.Id3;
                ViewData["ProjectManagerName3"] = projectTaskEditObject.ProjectManagerName3;
                ViewData["DesignCustomerName3"] = projectTaskEditObject.DesignCustomerName3;
                ViewData["ConstructionCustomerName3"] = projectTaskEditObject.ConstructionCustomerName3;
                ViewData["SupervisionCustomerName3"] = projectTaskEditObject.SupervisionCustomerName3;
                ViewData["DesignRealName3"] = projectTaskEditObject.DesignRealName3;
                ViewData["DesignCount3"] = projectTaskEditObject.DesignCount3;
                ViewData["DesignDateText3"] = projectTaskEditObject.DesignDateText3;
                ViewData["DesignStateName3"] = projectTaskEditObject.DesignStateName3;
                ViewData["EngineeringProgressName3"] = projectTaskEditObject.EngineeringProgressName3;
                ViewData["ImageCount3"] = projectTaskEditObject.ImageCount3;
                ViewData["DesignMemos3"] = projectTaskEditObject.DesignMemos3;
                ViewData["ProgressMemos3"] = projectTaskEditObject.ProgressMemos3;
                ViewData["Mark4"] = projectTaskEditObject.Mark4;
                ViewData["Id4"] = projectTaskEditObject.Id4;
                ViewData["ProjectManagerName4"] = projectTaskEditObject.ProjectManagerName4;
                ViewData["DesignCustomerName4"] = projectTaskEditObject.DesignCustomerName4;
                ViewData["ConstructionCustomerName4"] = projectTaskEditObject.ConstructionCustomerName4;
                ViewData["SupervisionCustomerName4"] = projectTaskEditObject.SupervisionCustomerName4;
                ViewData["DesignRealName4"] = projectTaskEditObject.DesignRealName4;
                ViewData["DesignCount4"] = projectTaskEditObject.DesignCount4;
                ViewData["DesignDateText4"] = projectTaskEditObject.DesignDateText4;
                ViewData["DesignStateName4"] = projectTaskEditObject.DesignStateName4;
                ViewData["EngineeringProgressName4"] = projectTaskEditObject.EngineeringProgressName4;
                ViewData["ImageCount4"] = projectTaskEditObject.ImageCount4;
                ViewData["DesignMemos4"] = projectTaskEditObject.DesignMemos4;
                ViewData["ProgressMemos4"] = projectTaskEditObject.ProgressMemos4;
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Id5"] = projectTaskEditObject.Id5;
                ViewData["ProjectManagerName5"] = projectTaskEditObject.ProjectManagerName5;
                ViewData["DesignCustomerName5"] = projectTaskEditObject.DesignCustomerName5;
                ViewData["ConstructionCustomerName5"] = projectTaskEditObject.ConstructionCustomerName5;
                ViewData["SupervisionCustomerName5"] = projectTaskEditObject.SupervisionCustomerName5;
                ViewData["DesignRealName5"] = projectTaskEditObject.DesignRealName5;
                ViewData["DesignCount5"] = projectTaskEditObject.DesignCount5;
                ViewData["DesignDateText5"] = projectTaskEditObject.DesignDateText5;
                ViewData["DesignStateName5"] = projectTaskEditObject.DesignStateName5;
                ViewData["EngineeringProgressName5"] = projectTaskEditObject.EngineeringProgressName5;
                ViewData["ImageCount5"] = projectTaskEditObject.ImageCount5;
                ViewData["DesignMemos5"] = projectTaskEditObject.DesignMemos5;
                ViewData["ProgressMemos5"] = projectTaskEditObject.ProgressMemos5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
                ViewData["Id6"] = projectTaskEditObject.Id6;
                ViewData["ProjectManagerName6"] = projectTaskEditObject.ProjectManagerName6;
                ViewData["DesignCustomerName6"] = projectTaskEditObject.DesignCustomerName6;
                ViewData["ConstructionCustomerName6"] = projectTaskEditObject.ConstructionCustomerName6;
                ViewData["SupervisionCustomerName6"] = projectTaskEditObject.SupervisionCustomerName6;
                ViewData["DesignRealName6"] = projectTaskEditObject.DesignRealName6;
                ViewData["DesignCount6"] = projectTaskEditObject.DesignCount6;
                ViewData["DesignDateText6"] = projectTaskEditObject.DesignDateText6;
                ViewData["DesignStateName6"] = projectTaskEditObject.DesignStateName6;
                ViewData["EngineeringProgressName6"] = projectTaskEditObject.EngineeringProgressName6;
                ViewData["ImageCount6"] = projectTaskEditObject.ImageCount6;
                ViewData["DesignMemos6"] = projectTaskEditObject.DesignMemos6;
                ViewData["ProgressMemos6"] = projectTaskEditObject.ProgressMemos6;
            }
            return View();
        }
        #endregion

        #region 资源导入

        /// <summary>
        /// 资源导入
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ResourceImport()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProfessionList = enumService.GetProfessionEnum();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumPropertyRightList = enumService.GetPropertyRightEnum();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumStateList = enumService.GetStateEnum();

            ViewData["Profession"] = JsonHelper.Encode(enumProfessionList);
            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["PropertyRight"] = JsonHelper.Encode(enumPropertyRightList);
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["State"] = JsonHelper.Encode(enumStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumProfessionList.Insert(0, allDict);
            enumImportanceList.Insert(0, allDict);
            enumPropertyRightList.Insert(0, allDict);
            enumBoolList.Insert(0, allDict);
            enumStateList.Insert(0, allDict);

            ViewData["ProfessionByAll"] = JsonHelper.Encode(enumProfessionList);
            ViewData["ImportanceByAll"] = JsonHelper.Encode(enumImportanceList);
            ViewData["PropertyRightByAll"] = JsonHelper.Encode(enumPropertyRightList);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["StateByAll"] = JsonHelper.Encode(enumStateList);

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "");

            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            enumTowerTypeList.Insert(0, nullDict);
            ViewData["TowerType"] = JsonHelper.Encode(enumTowerTypeList);

            IList<Dictionary<string, string>> enumTowerBaseTypeList = enumService.GetTowerBaseTypeEnum();
            enumTowerBaseTypeList.Insert(0, nullDict);
            ViewData["TowerBaseType"] = JsonHelper.Encode(enumTowerBaseTypeList);


            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            enumMachineRoomTypeList.Insert(0, nullDict);
            ViewData["MachineRoomType"] = JsonHelper.Encode(enumMachineRoomTypeList);


            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            enumExternalElectricList.Insert(0, nullDict);
            ViewData["ExternalElectric"] = JsonHelper.Encode(enumExternalElectricList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
            }
            return View();
        }

        /// <summary>
        /// 获取分页站点列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetResourcePlacesPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["GroupPlaceCode"] == null)
            {
                throw new ArgumentNullException("GroupPlaceCode");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["PropertyRight"] == null)
            {
                throw new ArgumentNullException("PropertyRight");
            }
            if (Request["Importance"] == null)
            {
                throw new ArgumentNullException("Importance");
            }
            if (Request["TelecomShare"] == null)
            {
                throw new ArgumentNullException("TelecomShare");
            }
            if (Request["MobileShare"] == null)
            {
                throw new ArgumentNullException("MobileShare");
            }
            if (Request["UnicomShare"] == null)
            {
                throw new ArgumentNullException("UnicomShare");
            }
            if (Request["State"] == null)
            {
                throw new ArgumentNullException("State");
            }

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetResourcePlacesPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["GroupPlaceCode"].Trim(), Request["PlaceName"].Trim(), int.Parse(Request["Profession"]), Guid.Parse(Request["PlaceCategoryId"]),
                    Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["PropertyRight"]), int.Parse(Request["Importance"]),
                    int.Parse(Request["TelecomShare"]), int.Parse(Request["MobileShare"]), int.Parse(Request["UnicomShare"]), int.Parse(Request["State"])));
            }
        }

        #endregion

        #region 资源更新
        /// <summary>
        /// 资源更新
        /// </summary>
        /// <param name="id">任务Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ResourceUpdate(Guid id, Guid placeId)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (placeId == null)
            {
                throw new ArgumentNullException("placeId");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "请选择");

            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            enumTowerTypeList.Insert(0, nullDict);
            ViewData["TowerTypeList"] = JsonHelper.Encode(enumTowerTypeList);

            IList<Dictionary<string, string>> enumTowerBaseTypeList = enumService.GetTowerBaseTypeEnum();
            enumTowerBaseTypeList.Insert(0, nullDict);
            ViewData["TowerBaseTypeList"] = JsonHelper.Encode(enumTowerBaseTypeList);


            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            enumMachineRoomTypeList.Insert(0, nullDict);
            ViewData["MachineRoomTypeList"] = JsonHelper.Encode(enumMachineRoomTypeList);


            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            enumExternalElectricList.Insert(0, nullDict);
            ViewData["ExternalElectricList"] = JsonHelper.Encode(enumExternalElectricList);

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                ResourceUpdateObject resourceUpdateObject = await Task.Factory.StartNew(() => proxy.Channel.GetResourceUpdatePrint(id));
                ViewData["TowerCountPrint"] = resourceUpdateObject.TowerCountPrint;
                ViewData["TowerBaseCountPrint"] = resourceUpdateObject.TowerBaseCountPrint;
                ViewData["MachineRoomCountPrint"] = resourceUpdateObject.MachineRoomCountPrint;
                ViewData["ExternalCountPrint"] = resourceUpdateObject.ExternalCountPrint;
                ViewData["EquipmentInstallCountPrint"] = resourceUpdateObject.EquipmentInstallCountPrint;
                ViewData["AddressCountPrint"] = resourceUpdateObject.AddressCountPrint;
                ViewData["FoundationCountPrint"] = resourceUpdateObject.FoundationCountPrint;
                ViewData["TowerTypePrint"] = resourceUpdateObject.TowerTypePrint;
                ViewData["TowerHeightPrint"] = resourceUpdateObject.TowerHeightPrint;
                ViewData["PlatFormNumberPrint"] = resourceUpdateObject.PlatFormNumberPrint;
                ViewData["PoleNumberPrint"] = resourceUpdateObject.PoleNumberPrint;
                ViewData["TowerBaseTypePrint"] = resourceUpdateObject.TowerBaseTypePrint;
                ViewData["MachineRoomTypePrint"] = resourceUpdateObject.MachineRoomTypePrint;
                ViewData["MachineRoomAreaPrint"] = resourceUpdateObject.MachineRoomAreaPrint;
                ViewData["ExternalElectricPrint"] = resourceUpdateObject.ExternalElectricPrint;
                ViewData["SwitchPowerPrint"] = resourceUpdateObject.SwitchPowerPrint;
                ViewData["BatteryPrint"] = resourceUpdateObject.BatteryPrint;
                ViewData["CabinetNumberPrint"] = resourceUpdateObject.CabinetNumberPrint;
                ViewData["TowerIdPrint"] = resourceUpdateObject.TowerIdPrint;
                ViewData["TowerBaseIdPrint"] = resourceUpdateObject.TowerBaseIdPrint;
                ViewData["MachineRoomIdPrint"] = resourceUpdateObject.MachineRoomIdPrint;
                ViewData["ExternalElectricPowerIdPrint"] = resourceUpdateObject.ExternalElectricPowerIdPrint;
                ViewData["EquipmentInstallIdPrint"] = resourceUpdateObject.EquipmentInstallIdPrint;
                ViewData["AddressExplorIdPrint"] = resourceUpdateObject.AddressExplorIdPrint;
                ViewData["FoundationTestIdPrint"] = resourceUpdateObject.FoundationTestIdPrint;
                ViewData["TelecomPoleNumberPrint"] = resourceUpdateObject.TelecomPoleNumberPrint;
                ViewData["TelecomCabinetNumberPrint"] = resourceUpdateObject.TelecomCabinetNumberPrint;
                ViewData["TelecomPowerUsedPrint"] = resourceUpdateObject.TelecomPowerUsedPrint;
                ViewData["MobilePoleNumberPrint"] = resourceUpdateObject.MobilePoleNumberPrint;
                ViewData["MobileCabinetNumberPrint"] = resourceUpdateObject.MobileCabinetNumberPrint;
                ViewData["MobilePowerUsedPrint"] = resourceUpdateObject.MobilePowerUsedPrint;
                ViewData["UnicomPoleNumberPrint"] = resourceUpdateObject.UnicomPoleNumberPrint;
                ViewData["UnicomCabinetNumberPrint"] = resourceUpdateObject.UnicomCabinetNumberPrint;
                ViewData["UnicomPowerUsedPrint"] = resourceUpdateObject.UnicomPowerUsedPrint;
                ViewData["IsFinishMobilePrint"] = resourceUpdateObject.IsFinishMobilePrint;
                ViewData["IsFinishTelecomPrint"] = resourceUpdateObject.IsFinishTelecomPrint;
                ViewData["IsFinishUnicomPrint"] = resourceUpdateObject.IsFinishUnicomPrint;
                ViewData["TowerFullNamePrint"] = resourceUpdateObject.TowerFullNamePrint;
                ViewData["TowerModifyDatePrint"] = resourceUpdateObject.TowerModifyDatePrint;
                ViewData["TowerBaseFullNamePrint"] = resourceUpdateObject.TowerBaseFullNamePrint;
                ViewData["TowerBaseModifyDatePrint"] = resourceUpdateObject.TowerBaseModifyDatePrint;
                ViewData["MachineRoomFullNamePrint"] = resourceUpdateObject.MachineRoomFullNamePrint;
                ViewData["MachineRoomModifyDatePrint"] = resourceUpdateObject.MachineRoomModifyDatePrint;
                ViewData["ExternalFullNamePrint"] = resourceUpdateObject.ExternalFullNamePrint;
                ViewData["ExternalModifyDatePrint"] = resourceUpdateObject.ExternalModifyDatePrint;
                ViewData["EquipmentFullNamePrint"] = resourceUpdateObject.EquipmentFullNamePrint;
                ViewData["EquipmentModifyDatePrint"] = resourceUpdateObject.EquipmentModifyDatePrint;
                ViewData["AddressFullNamePrint"] = resourceUpdateObject.AddressFullNamePrint;
                ViewData["AddressModifyDatePrint"] = resourceUpdateObject.AddressModifyDatePrint;
                ViewData["FoundationFullNamePrint"] = resourceUpdateObject.FoundationFullNamePrint;
                ViewData["FoundationModifyDatePrint"] = resourceUpdateObject.FoundationModifyDatePrint;
                ViewData["MobileFullNamePrint"] = resourceUpdateObject.MobileFullNamePrint;
                ViewData["MobileModifyDatePrint"] = resourceUpdateObject.MobileModifyDatePrint;
                ViewData["TelecomFullNamePrint"] = resourceUpdateObject.TelecomFullNamePrint;
                ViewData["TelecomModifyDatePrint"] = resourceUpdateObject.TelecomModifyDatePrint;
                ViewData["UnicomFullNamePrint"] = resourceUpdateObject.UnicomFullNamePrint;
                ViewData["UnicomModifyDatePrint"] = resourceUpdateObject.UnicomModifyDatePrint;
                ViewData["MobileSharePrint"] = resourceUpdateObject.MobileSharePrint;
                ViewData["TelecomSharePrint"] = resourceUpdateObject.TelecomSharePrint;
                ViewData["UnicomSharePrint"] = resourceUpdateObject.UnicomSharePrint;
            }

            using (ServiceProxy<IPlacePropertyService> proxy = new ServiceProxy<IPlacePropertyService>())
            {
                ResourceMaintObject resourceMaintObject = await Task.Factory.StartNew(() => proxy.Channel.GetResourceMaintenanceById(placeId));
                ViewData["Id"] = placeId;
                ViewData["TowerCount"] = resourceMaintObject.TowerCount;
                ViewData["TowerBaseCount"] = resourceMaintObject.TowerBaseCount;
                ViewData["MachineRoomCount"] = resourceMaintObject.MachineRoomCount;
                ViewData["ExternalCount"] = resourceMaintObject.ExternalCount;
                ViewData["EquipmentInstallCount"] = resourceMaintObject.EquipmentInstallCount;
                ViewData["AddressCount"] = resourceMaintObject.AddressCount;
                ViewData["FoundationCount"] = resourceMaintObject.FoundationCount;
                ViewData["TowerMark"] = resourceMaintObject.TowerMark;
                ViewData["TowerBaseMark"] = resourceMaintObject.TowerBaseMark;
                ViewData["MachineRoomMark"] = resourceMaintObject.MachineRoomMark;
                ViewData["ExternalElectricPowerMark"] = resourceMaintObject.ExternalElectricPowerMark;
                ViewData["EquipmentInstallMark"] = resourceMaintObject.EquipmentInstallMark;
                ViewData["AddressExplorMark"] = resourceMaintObject.AddressExplorMark;
                ViewData["FoundationTestMark"] = resourceMaintObject.FoundationTestMark;
                ViewData["TowerType"] = resourceMaintObject.TowerType;
                ViewData["TowerHeight"] = resourceMaintObject.TowerHeight;
                ViewData["PlatFormNumber"] = resourceMaintObject.PlatFormNumber;
                ViewData["PoleNumber"] = resourceMaintObject.PoleNumber;
                ViewData["TowerBaseType"] = resourceMaintObject.TowerBaseType;
                ViewData["MachineRoomType"] = resourceMaintObject.MachineRoomType;
                ViewData["MachineRoomArea"] = resourceMaintObject.MachineRoomArea;
                ViewData["ExternalElectric"] = resourceMaintObject.ExternalElectric;
                ViewData["SwitchPower"] = resourceMaintObject.SwitchPower;
                ViewData["Battery"] = resourceMaintObject.Battery;
                ViewData["CabinetNumber"] = resourceMaintObject.CabinetNumber;
                ViewData["TowerId"] = resourceMaintObject.TowerId;
                ViewData["TowerBaseId"] = resourceMaintObject.TowerBaseId;
                ViewData["MachineRoomId"] = resourceMaintObject.MachineRoomId;
                ViewData["ExternalElectricPowerId"] = resourceMaintObject.ExternalElectricPowerId;
                ViewData["EquipmentInstallId"] = resourceMaintObject.EquipmentInstallId;
                ViewData["AddressExplorId"] = resourceMaintObject.AddressExplorId;
                ViewData["FoundationTestId"] = resourceMaintObject.FoundationTestId;
                ViewData["FileIdListTower"] = resourceMaintObject.TowerFileIdList;
                ViewData["FileIdListTowerBase"] = resourceMaintObject.TowerBaseFileIdList;
                ViewData["FileIdListMachineRoom"] = resourceMaintObject.MachineRoomFileIdList;
                ViewData["FileIdListExternal"] = resourceMaintObject.ExternalFileIdList;
                ViewData["FileIdListEquipmentInstall"] = resourceMaintObject.EquipmentInstallFileIdList;
                ViewData["FileIdListAddress"] = resourceMaintObject.AddressFileIdList;
                ViewData["FileIdListFoundation"] = resourceMaintObject.FoundationFileIdList;
                ViewData["TelecomPoleNumber"] = resourceMaintObject.TelecomPoleNumber;
                ViewData["TelecomCabinetNumber"] = resourceMaintObject.TelecomCabinetNumber;
                ViewData["TelecomPowerUsed"] = resourceMaintObject.TelecomPowerUsed;
                ViewData["MobilePoleNumber"] = resourceMaintObject.MobilePoleNumber;
                ViewData["MobileCabinetNumber"] = resourceMaintObject.MobileCabinetNumber;
                ViewData["MobilePowerUsed"] = resourceMaintObject.MobilePowerUsed;
                ViewData["UnicomPoleNumber"] = resourceMaintObject.UnicomPoleNumber;
                ViewData["UnicomCabinetNumber"] = resourceMaintObject.UnicomCabinetNumber;
                ViewData["UnicomPowerUsed"] = resourceMaintObject.UnicomPowerUsed;
                ViewData["MobileShare"] = resourceMaintObject.MobileShare;
                ViewData["TelecomShare"] = resourceMaintObject.TelecomShare;
                ViewData["UnicomShare"] = resourceMaintObject.UnicomShare;
                ViewData["TowerFullName"] = resourceMaintObject.TowerFullName;
                ViewData["TowerModifyDate"] = resourceMaintObject.TowerModifyDate;
                ViewData["TowerBaseFullName"] = resourceMaintObject.TowerBaseFullName;
                ViewData["TowerBaseModifyDate"] = resourceMaintObject.TowerBaseModifyDate;
                ViewData["MachineRoomFullName"] = resourceMaintObject.MachineRoomFullName;
                ViewData["MachineRoomModifyDate"] = resourceMaintObject.MachineRoomModifyDate;
                ViewData["ExternalFullName"] = resourceMaintObject.ExternalFullName;
                ViewData["ExternalModifyDate"] = resourceMaintObject.ExternalModifyDate;
                ViewData["EquipmentFullName"] = resourceMaintObject.EquipmentFullName;
                ViewData["EquipmentModifyDate"] = resourceMaintObject.EquipmentModifyDate;
                ViewData["AddressFullName"] = resourceMaintObject.AddressFullName;
                ViewData["AddressModifyDate"] = resourceMaintObject.AddressModifyDate;
                ViewData["FoundationFullName"] = resourceMaintObject.FoundationFullName;
                ViewData["FoundationModifyDate"] = resourceMaintObject.FoundationModifyDate;
                ViewData["MobileFullName"] = resourceMaintObject.MobileFullName;
                ViewData["MobileModifyDate"] = resourceMaintObject.MobileModifyDate;
                ViewData["TelecomFullName"] = resourceMaintObject.TelecomFullName;
                ViewData["TelecomModifyDate"] = resourceMaintObject.TelecomModifyDate;
                ViewData["UnicomFullName"] = resourceMaintObject.UnicomFullName;
                ViewData["UnicomModifyDate"] = resourceMaintObject.UnicomModifyDate;
            }

            return View();
        }
        #endregion

        #region 项目信息表
        /// <summary>
        /// 项目信息表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ProjectInformation()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumPropertyRight = enumService.GetPropertyRightEnum();
            IList<Dictionary<string, string>> enumConstructionProgress = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumConstructionMethod = enumService.GetConstructionMethodEnum();
            IList<Dictionary<string, string>> enumTowerType = enumService.GetTowerTypeEnum();
            IList<Dictionary<string, string>> enumTowerBaseType = enumService.GetTowerBaseTypeEnum();
            IList<Dictionary<string, string>> enumMachineRoomType = enumService.GetMachineRoomTypeEnum();
            IList<Dictionary<string, string>> enumExternalElectric = enumService.GetExternalElectricEnum();
            IList<Dictionary<string, string>> enumBool = enumService.GetBoolEnum();

            IList<Dictionary<string, string>> enumPropertyRightAllList = enumService.GetPropertyRightEnum();
            IList<Dictionary<string, string>> enumConstructionMethodAllList = enumService.GetConstructionMethodEnum();
            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumPropertyRight.Insert(0, nullDict);
            enumConstructionProgress.Insert(0, nullDict);
            enumConstructionMethod.Insert(0, nullDict);
            enumTowerType.Insert(0, nullDict);
            enumTowerBaseType.Insert(0, nullDict);
            enumMachineRoomType.Insert(0, nullDict);
            enumExternalElectric.Insert(0, nullDict);

            ViewData["PropertyRight"] = JsonHelper.Encode(enumPropertyRight);
            ViewData["ConstructionMethod"] = JsonHelper.Encode(enumConstructionMethod);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgress);
            ViewData["TowerType"] = JsonHelper.Encode(enumTowerType);
            ViewData["TowerBaseType"] = JsonHelper.Encode(enumTowerBaseType);
            ViewData["MachineRoomType"] = JsonHelper.Encode(enumMachineRoomType);
            ViewData["ExternalElectric"] = JsonHelper.Encode(enumExternalElectric);
            ViewData["Bool"] = JsonHelper.Encode(enumBool);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumConstructionMethodAllList.Insert(0, allDict);
            enumConstructionProgressAllList.Insert(0, allDict);

            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "6");
            Dict1.Add("text", "未完成");
            enumConstructionProgressAllList.Insert(6, Dict1);

            ViewData["PropertyRightByAll"] = JsonHelper.Encode(enumPropertyRightAllList);
            ViewData["ConstructionMethodByAll"] = JsonHelper.Encode(enumConstructionMethodAllList);
            ViewData["ConstructionProgressByAll"] = JsonHelper.Encode(enumConstructionProgressAllList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
            }
            return View();
        }

        /// <summary>
        /// 获取项目信息列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectInformationPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PropertyRightList"] == null)
            {
                throw new ArgumentNullException("PropertyRightList");
            }
            if (Request["GroupPlaceCode"] == null)
            {
                throw new ArgumentNullException("GroupPlaceCode");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["ConstructionMethod"] == null)
            {
                throw new ArgumentNullException("ConstructionMethod");
            }
            if (Request["ConstructionProgress"] == null)
            {
                throw new ArgumentNullException("ConstructionProgress");
            }

            string propertyRightSql = "";
            if (Request["PropertyRightList"].Trim() != "")
            {
                string[] propertyRightList = Request["PropertyRightList"].Trim().Split(',');
                for (int i = 0; i < propertyRightList.Length; i++)
                {
                    propertyRightSql += "select " + propertyRightList[i];
                    if (i != propertyRightList.Length - 1)
                    {
                        propertyRightSql += " union ";
                    }
                }
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectInformationPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), propertyRightSql, Request["GroupPlaceCode"].Trim(),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["ConstructionMethod"]),
                    int.Parse(Request["ConstructionProgress"])));
            }
        }

        #endregion

        #region 基站信息历史记录
        /// <summary>
        /// 基站信息历史记录
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PlaceInformationLog(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumOperationType = enumService.GetOperationTypeEnum();
            IList<Dictionary<string, string>> enumTowerType = enumService.GetTowerTypeEnum();
            IList<Dictionary<string, string>> enumTowerBaseType = enumService.GetTowerBaseTypeEnum();
            IList<Dictionary<string, string>> enumMachineRoomType = enumService.GetMachineRoomTypeEnum();
            IList<Dictionary<string, string>> enumExternalElectric = enumService.GetExternalElectricEnum();
            IList<Dictionary<string, string>> enumBool = enumService.GetBoolEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumTowerType.Insert(0, nullDict);
            enumTowerBaseType.Insert(0, nullDict);
            enumMachineRoomType.Insert(0, nullDict);
            enumExternalElectric.Insert(0, nullDict);

            ViewData["OperationType"] = JsonHelper.Encode(enumOperationType);
            ViewData["TowerType"] = JsonHelper.Encode(enumTowerType);
            ViewData["TowerBaseType"] = JsonHelper.Encode(enumTowerBaseType);
            ViewData["MachineRoomType"] = JsonHelper.Encode(enumMachineRoomType);
            ViewData["ExternalElectric"] = JsonHelper.Encode(enumExternalElectric);
            ViewData["Bool"] = JsonHelper.Encode(enumBool);
            ViewData["ParentId"] = id;

            return View();
        }

        public async Task<string> GetTowerLog()
        {
            if (Request["PropertyType"] == null)
            {
                throw new ArgumentNullException("PropertyType");
            }

            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<ITowerLogService> proxy = new ServiceProxy<ITowerLogService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetTowerLog(int.Parse(Request["PropertyType"]), Guid.Parse(Request["ParentId"])));
            }
        }

        public async Task<string> GetTowerBaseLog()
        {
            if (Request["PropertyType"] == null)
            {
                throw new ArgumentNullException("PropertyType");
            }

            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<ITowerBaseLogService> proxy = new ServiceProxy<ITowerBaseLogService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetTowerBaseLog(int.Parse(Request["PropertyType"]), Guid.Parse(Request["ParentId"])));
            }
        }

        public async Task<string> GetMachineRoomLog()
        {
            if (Request["PropertyType"] == null)
            {
                throw new ArgumentNullException("PropertyType");
            }

            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<IMachineRoomLogService> proxy = new ServiceProxy<IMachineRoomLogService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetMachineRoomLog(int.Parse(Request["PropertyType"]), Guid.Parse(Request["ParentId"])));
            }
        }

        public async Task<string> GetExternalElectricPowerLog()
        {
            if (Request["PropertyType"] == null)
            {
                throw new ArgumentNullException("PropertyType");
            }

            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<IExternalElectricPowerLogService> proxy = new ServiceProxy<IExternalElectricPowerLogService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetExternalElectricPowerLog(int.Parse(Request["PropertyType"]), Guid.Parse(Request["ParentId"])));
            }
        }

        public async Task<string> GetEquipmentInstallLog()
        {
            if (Request["PropertyType"] == null)
            {
                throw new ArgumentNullException("PropertyType");
            }

            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<IEquipmentInstallLogService> proxy = new ServiceProxy<IEquipmentInstallLogService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetEquipmentInstallLog(int.Parse(Request["PropertyType"]), Guid.Parse(Request["ParentId"])));
            }
        }

        public async Task<string> GetAddressExplorLog()
        {
            if (Request["PropertyType"] == null)
            {
                throw new ArgumentNullException("PropertyType");
            }

            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<IAddressExplorLogService> proxy = new ServiceProxy<IAddressExplorLogService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetAddressExplorLog(int.Parse(Request["PropertyType"]), Guid.Parse(Request["ParentId"])));
            }
        }

        public async Task<string> GetFoundationTestLog()
        {
            if (Request["PropertyType"] == null)
            {
                throw new ArgumentNullException("PropertyType");
            }

            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<IFoundationTestLogService> proxy = new ServiceProxy<IFoundationTestLogService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetFoundationTestLog(int.Parse(Request["PropertyType"]), Guid.Parse(Request["ParentId"])));
            }
        }

        public async Task<string> GetPlacePropertyLog()
        {
            if (Request["PropertyType"] == null)
            {
                throw new ArgumentNullException("PropertyType");
            }

            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            if (Request["CompanyNameId"] == null)
            {
                throw new ArgumentNullException("CompanyNameId");
            }

            using (ServiceProxy<IPlacePropertyLogService> proxy = new ServiceProxy<IPlacePropertyLogService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlacePropertyLog(int.Parse(Request["PropertyType"]), Guid.Parse(Request["ParentId"]), int.Parse(Request["CompanyNameId"])));
            }
        }

        #endregion

        #region 任务信息历史记录
        /// <summary>
        /// 任务信息历史记录
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> TaskInformationLog(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumOperationType = enumService.GetOperationTypeEnum();
            IList<Dictionary<string, string>> enumTowerType = enumService.GetTowerTypeEnum();
            IList<Dictionary<string, string>> enumTowerBaseType = enumService.GetTowerBaseTypeEnum();
            IList<Dictionary<string, string>> enumMachineRoomType = enumService.GetMachineRoomTypeEnum();
            IList<Dictionary<string, string>> enumExternalElectric = enumService.GetExternalElectricEnum();
            IList<Dictionary<string, string>> enumBool = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumTowerType.Insert(0, nullDict);
            enumTowerBaseType.Insert(0, nullDict);
            enumMachineRoomType.Insert(0, nullDict);
            enumExternalElectric.Insert(0, nullDict);

            ViewData["OperationType"] = JsonHelper.Encode(enumOperationType);
            ViewData["TowerType"] = JsonHelper.Encode(enumTowerType);
            ViewData["TowerBaseType"] = JsonHelper.Encode(enumTowerBaseType);
            ViewData["MachineRoomType"] = JsonHelper.Encode(enumMachineRoomType);
            ViewData["ExternalElectric"] = JsonHelper.Encode(enumExternalElectric);
            ViewData["Bool"] = JsonHelper.Encode(enumBool);
            ViewData["ParentId"] = id;
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);

            return View();
        }

        /// <summary>
        /// 获取子任务历史记录
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTaskPropertyLog()
        {
            if (Request["TaskModel"] == null)
            {
                throw new ArgumentNullException("TaskModel");
            }

            if (Request["ParentId"] == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            using (ServiceProxy<ITaskPropertyLogService> proxy = new ServiceProxy<ITaskPropertyLogService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetTaskPropertyLog(int.Parse(Request["TaskModel"]), Guid.Parse(Request["ParentId"])));
            }
        }

        #endregion

        #region 隐患上报

        /// <summary>
        /// 隐患上报
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WorkApply()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumOrderStateList = enumService.GetWFProcessInstanceStateEnum();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            ViewData["OrderState"] = JsonHelper.Encode(enumOrderStateList);
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumOrderStateList.Insert(0, allDict);
            enumBoolList.Insert(0, allDict);
            ViewData["OrderStateByAll"] = JsonHelper.Encode(enumOrderStateList);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["Count"] = 0;
            ViewData["FileIdList"] = "";

            ViewData["FullName"] = this.FullName;
            ViewData["PhoneNumber"] = this.PhoneNumber;

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                IList<ReseauSelectObject> reseauSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetAllUsedReseaus());
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "全部" });
                ViewData["ReseauByAll"] = JsonHelper.Encode(reseauSelectObjects);
                reseauSelectObjects.RemoveAt(0);
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "请选择" });
                ViewData["ReseauBySelect"] = JsonHelper.Encode(reseauSelectObjects);
            }

            using (ServiceProxy<ICustomerService> proxy = new ServiceProxy<ICustomerService>())
            {
                CustomerMaintObject customerMaintObject = await Task.Factory.StartNew(() => proxy.Channel.GetCustomerByUserId(this.UserId));
                ViewData["CustomerId"] = customerMaintObject.Id;
                ViewData["CustomerName"] = customerMaintObject.CustomerName;
            }
            return View();
        }

        /// <summary>
        /// 根据隐患上报Id获取隐患上报
        /// </summary>
        /// <param name="id">隐患上报Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetWorkApplyById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWorkApplyById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页隐患上报列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWorkApplysPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["Title"] == null)
            {
                throw new ArgumentNullException("Title");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["OrderState"] == null)
            {
                throw new ArgumentNullException("OrderState");
            }
            if (Request["IsSoved"] == null)
            {
                throw new ArgumentNullException("IsSoved");
            }

            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWorkApplysPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["Title"].Trim(), Guid.Parse(Request["ReseauId"]),
                    int.Parse(Request["OrderState"]), int.Parse(Request["IsSoved"]), this.UserId));
            }
        }

        /// <summary>
        /// 保存隐患上报
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWorkApply()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            WorkApplyMaintObject workApplyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                workApplyMaintObject = new WorkApplyMaintObject()
                {
                    Id = Guid.Empty,
                    Title = row["Title"].ToString().Trim(),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    CustomerId = Guid.Parse(row["CustomerId"].ToString()),
                    ApplyReason = row["ApplyReason"].ToString().Trim(),
                    SceneContactMan = row["SceneContactMan"].ToString().Trim(),
                    SceneContactTel = row["SceneContactTel"].ToString().Trim(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                workApplyMaintObject = new WorkApplyMaintObject()
                {
                    Id = id,
                    Title = row["Title"].ToString().Trim(),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    CustomerId = Guid.Parse(row["CustomerId"].ToString()),
                    ApplyReason = row["ApplyReason"].ToString().Trim(),
                    SceneContactMan = row["SceneContactMan"].ToString().Trim(),
                    SceneContactTel = row["SceneContactTel"].ToString().Trim(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateWorkApply(workApplyMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 发送隐患上报
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendWorkApplys()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WorkApplyMaintObject> workApplyMaintObjects = new List<WorkApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    workApplyMaintObjects.Add(new WorkApplyMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SendWorkApplys(workApplyMaintObjects));
            }
            return this.Sucess("单据发送成功");
        }

        /// <summary>
        /// 删除隐患上报
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveWorkApplys()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WorkApplyMaintObject> workApplyMaintObjects = new List<WorkApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    workApplyMaintObjects.Add(new WorkApplyMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveWorkApplys(workApplyMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        /// <summary>
        /// 退回隐患上报
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReturnWorkApply()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            WorkApplyMaintObject workApplyMaintObject = null;
            Guid id = Guid.Parse((row["ReturnWorkApplyId"] == null || row["ReturnWorkApplyId"].ToString() == "" ? Guid.Empty : row["ReturnWorkApplyId"]).ToString());
            workApplyMaintObject = new WorkApplyMaintObject()
            {
                Id = id,
                ReturnReason = row["ReturnReason"].ToString().Trim()
            };
            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.ReturnWorkApply(workApplyMaintObject));
            }
            return this.Sucess("单据退回成功");
        }

        #endregion

        #region 零星派工

        /// <summary>
        /// 零星派工
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WorkOrder()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumOrderStateList = enumService.GetWFProcessInstanceStateEnum();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumCustomerTypeList = enumService.GetCustomerTypeEnum();
            ViewData["OrderState"] = JsonHelper.Encode(enumOrderStateList);
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumOrderStateList.Insert(0, allDict);
            enumBoolList.Insert(0, allDict);
            ViewData["OrderStateByAll"] = JsonHelper.Encode(enumOrderStateList);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            Dictionary<string, string> selectDict = new Dictionary<string, string>(2);
            selectDict.Add("id", "0");
            selectDict.Add("text", "请选择");
            enumCustomerTypeList.Insert(0, selectDict);
            ViewData["CustomerTypeBySelect"] = JsonHelper.Encode(enumCustomerTypeList);

            ViewData["Count"] = 0;
            ViewData["FileIdList"] = "";

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                IList<ReseauSelectObject> reseauSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetAllUsedReseaus());
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "全部" });
                ViewData["ReseauByAll"] = JsonHelper.Encode(reseauSelectObjects);
                reseauSelectObjects.RemoveAt(0);
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "请选择" });
                ViewData["ReseauBySelect"] = JsonHelper.Encode(reseauSelectObjects);
            }

            using (ServiceProxy<IWorkBigClassService> proxy = new ServiceProxy<IWorkBigClassService>())
            {
                IList<WorkBigClassSelectObject> workBigClassSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWorkBigClasss());
                workBigClassSelectObjects.Insert(0, new WorkBigClassSelectObject() { Id = Guid.Empty, BigClassName = "全部" });
                ViewData["WorkBigClassByAll"] = JsonHelper.Encode(workBigClassSelectObjects);
                workBigClassSelectObjects.RemoveAt(0);
                workBigClassSelectObjects.Insert(0, new WorkBigClassSelectObject() { Id = Guid.Empty, BigClassName = "请选择" });
                ViewData["WorkBigClassBySelect"] = JsonHelper.Encode(workBigClassSelectObjects);
            }

            using (ServiceProxy<ICustomerService> proxy = new ServiceProxy<ICustomerService>())
            {
                IList<CustomerMaintObject> customerMaintObjects = await Task.Factory.StartNew(() => proxy.Channel.GetCustomersByType(1));
                customerMaintObjects.Insert(0, new CustomerMaintObject() { Id = Guid.Empty, CustomerName = "全部" });
                ViewData["CustomerByAll"] = JsonHelper.Encode(customerMaintObjects);
                customerMaintObjects.RemoveAt(0);
                customerMaintObjects.Insert(0, new CustomerMaintObject() { Id = Guid.Empty, CustomerName = "请选择" });
                ViewData["CustomerBySelect"] = JsonHelper.Encode(customerMaintObjects);
            }

            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                IList<UserSelectObject> userSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedUsers(Guid.Empty));
                userSelectObjects.Insert(0, new UserSelectObject() { Id = Guid.Empty, FullName = "全部" });
                ViewData["UserByAll"] = JsonHelper.Encode(userSelectObjects);
                userSelectObjects.RemoveAt(0);
                userSelectObjects.Insert(0, new UserSelectObject() { Id = Guid.Empty, FullName = "请选择" });
                ViewData["UserBySelect"] = JsonHelper.Encode(userSelectObjects);
            }

            return View();
        }

        /// <summary>
        /// 根据零星派工Id获取零星派工
        /// </summary>
        /// <param name="id">零星派工Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetWorkOrderById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IWorkOrderService> proxy = new ServiceProxy<IWorkOrderService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWorkOrderById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页零星派工列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWorkOrdersPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["Title"] == null)
            {
                throw new ArgumentNullException("Title");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["WorkBigClassId"] == null)
            {
                throw new ArgumentNullException("WorkBigClassId");
            }
            if (Request["WorkSmallClassId"] == null)
            {
                throw new ArgumentNullException("WorkSmallClassId");
            }
            if (Request["CustomerId"] == null)
            {
                throw new ArgumentNullException("CustomerId");
            }
            if (Request["MaintainContactMan"] == null)
            {
                throw new ArgumentNullException("MaintainContactMan");
            }
            if (Request["IsFinish"] == null)
            {
                throw new ArgumentNullException("IsFinish");
            }
            if (Request["OrderState"] == null)
            {
                throw new ArgumentNullException("OrderState");
            }

            using (ServiceProxy<IWorkOrderService> proxy = new ServiceProxy<IWorkOrderService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWorkOrdersPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["Title"].Trim(), Guid.Parse(Request["ReseauId"]), Guid.Parse(Request["WorkBigClassId"]),
                    Guid.Parse(Request["WorkSmallClassId"]), Guid.Parse(Request["CustomerId"]), Request["MaintainContactMan"].Trim(), Guid.Empty, int.Parse(Request["IsFinish"]),
                    int.Parse(Request["OrderState"]), this.UserId));
            }
        }

        /// <summary>
        /// 保存零星派工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWorkOrder()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            WorkOrderMaintObject workOrderMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                workOrderMaintObject = new WorkOrderMaintObject()
                {
                    Id = Guid.Empty,
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    WorkSmallClassId = Guid.Parse(row["WorkSmallClassId"].ToString()),
                    RequireSendDate = DateTime.Parse(row["RequireSendDate"].ToString().Trim()),
                    SceneContactMan = row["SceneContactMan"].ToString(),
                    SceneContactTel = row["SceneContactTel"].ToString(),
                    CustomerId = Guid.Parse(row["CustomerId"].ToString()),
                    Days = int.Parse(row["Days"].ToString()),
                    CustomerUserId = Guid.Parse(row["CustomerUserId"].ToString()),
                    MaintainContactTel = row["MaintainContactTel"].ToString(),
                    WorkContent = row["WorkContent"].ToString(),
                    HumanRequire = row["HumanRequire"].ToString(),
                    CarRequire = row["CarRequire"].ToString(),
                    MaterialRequire = row["MaterialRequire"].ToString(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                workOrderMaintObject = new WorkOrderMaintObject()
                {
                    Id = id,
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    WorkSmallClassId = Guid.Parse(row["WorkSmallClassId"].ToString()),
                    RequireSendDate = DateTime.Parse(row["RequireSendDate"].ToString().Trim()),
                    SceneContactMan = row["SceneContactMan"].ToString(),
                    SceneContactTel = row["SceneContactTel"].ToString(),
                    CustomerId = Guid.Parse(row["CustomerId"].ToString()),
                    Days = int.Parse(row["Days"].ToString()),
                    CustomerUserId = Guid.Parse(row["CustomerUserId"].ToString()),
                    MaintainContactTel = row["MaintainContactTel"].ToString(),
                    WorkContent = row["WorkContent"].ToString(),
                    HumanRequire = row["HumanRequire"].ToString(),
                    CarRequire = row["CarRequire"].ToString(),
                    MaterialRequire = row["MaterialRequire"].ToString(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IWorkOrderService> proxy = new ServiceProxy<IWorkOrderService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateWorkOrder(workOrderMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除零星派工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveWorkOrders()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WorkOrderMaintObject> workOrderMaintObjects = new List<WorkOrderMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    workOrderMaintObjects.Add(new WorkOrderMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IWorkOrderService> proxy = new ServiceProxy<IWorkOrderService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveWorkOrder(workOrderMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 登记结算
        /// <summary>
        /// 登记结算
        /// </summary>
        /// <param name="id">零星派工单Id</param>
        /// <param name="wfActivityInstanceId">公文单据Id</param>
        /// <param name="wfProcessInstanceId">公文步骤Id</param>
        /// <returns></returns>
        public async Task<ActionResult> WorkSettlement(Guid id, Guid wfActivityInstanceId)
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
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);

            using (ServiceProxy<IWorkOrderService> proxy = new ServiceProxy<IWorkOrderService>())
            {
                WorkOrderEditorObject workOrderEditorObject = await Task.Factory.StartNew(() => proxy.Channel.GetWorkOrderEditorById(id));
                ViewData["Id"] = id;
                ViewData["WorkBeginDateText"] = workOrderEditorObject.WorkBeginDateText;
                ViewData["BeginHour"] = workOrderEditorObject.BeginHour;
                ViewData["BeginMinute"] = workOrderEditorObject.BeginMinute;
                ViewData["WorkEndDateText"] = workOrderEditorObject.WorkEndDateText;
                ViewData["EndHour"] = workOrderEditorObject.EndHour;
                ViewData["EndMinute"] = workOrderEditorObject.EndMinute;
                ViewData["ExecuteSituation"] = workOrderEditorObject.ExecuteSituation;
                ViewData["MaterialConsumption"] = workOrderEditorObject.MaterialConsumption;
                ViewData["PersonnelNumber"] = workOrderEditorObject.PersonnelNumber;
                ViewData["CarType"] = workOrderEditorObject.CarType;
                ViewData["IsFinish"] = workOrderEditorObject.IsFinish;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceId;
                ViewData["WFCount"] = workOrderEditorObject.WFCount;
                ViewData["WFFileIdList"] = workOrderEditorObject.WFFileIdList;
            }
            return View();
        }

        /// <summary>
        /// 保存登记结算
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWorkOrderWF()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            WorkOrderEditorObject workOrderEditorObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            workOrderEditorObject = new WorkOrderEditorObject()
            {
                Id = id,
                WFActivityInstanceId = Guid.Parse(row["WFActivityInstanceId"].ToString()),
                WorkBeginDate = DateTime.Parse(row["WorkBeginDate"].ToString()),
                BeginHour = int.Parse(row["BeginHour"].ToString()),
                BeginMinute = int.Parse(row["BeginMinute"].ToString()),
                WorkEndDate = DateTime.Parse(row["WorkEndDate"].ToString()),
                EndHour = int.Parse(row["EndHour"].ToString()),
                EndMinute = int.Parse(row["EndMinute"].ToString()),
                ExecuteSituation = row["ExecuteSituation"].ToString().Trim(),
                MaterialConsumption = row["MaterialConsumption"].ToString().Trim(),
                PersonnelNumber = row["PersonnelNumber"].ToString().Trim(),
                CarType = row["CarType"].ToString().Trim(),
                IsFinish = int.Parse(row["IsFinish"].ToString()),
                WFFileIdList = row["WFFileIdList"].ToString().Trim(),
                ModifyUserId = this.UserId
            };
            using (ServiceProxy<IWorkOrderService> proxy = new ServiceProxy<IWorkOrderService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveWorkOrderWF(workOrderEditorObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 获取各天执行情况
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWorkOrderDetail()
        {
            if (Request["WorkOrderId"] == null)
            {
                throw new ArgumentNullException("WorkOrderId");
            }

            using (ServiceProxy<IWorkOrderDetailService> proxy = new ServiceProxy<IWorkOrderDetailService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWorkOrderDetail(Guid.Parse(Request["WorkOrderId"])));
            }
        }

        /// <summary>
        /// 根据各天执行情况Id获取各天执行情况
        /// </summary>
        /// <param name="id">各天执行情况Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetWorkOrderDetailById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IWorkOrderDetailService> proxy = new ServiceProxy<IWorkOrderDetailService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWorkOrderDetailById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存各天执行情况
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWorkOrderDetail()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            WorkOrderDetailMaintObject workOrderDetailMaintObject = null;
            Guid id = Guid.Parse((row["WorkOrderDetailId"] == null || row["WorkOrderDetailId"].ToString() == "" ? Guid.Empty : row["WorkOrderDetailId"]).ToString());
            if (id == Guid.Empty)
            {
                workOrderDetailMaintObject = new WorkOrderDetailMaintObject()
                {
                    WorkOrderDetailId = Guid.Empty,
                    WorkOrderId = Guid.Parse(row["WorkOrderId"].ToString()),
                    txt_WFActivityInstanceId = Guid.Parse(row["txt_WFActivityInstanceId"].ToString()),
                    dp_WorkBeginDate = DateTime.Parse(row["dp_WorkBeginDate"].ToString()),
                    sp_BeginHour = int.Parse(row["sp_BeginHour"].ToString()),
                    sp_BeginMinute = int.Parse(row["sp_BeginMinute"].ToString()),
                    dp_WorkEndDate = DateTime.Parse(row["dp_WorkEndDate"].ToString()),
                    sp_EndHour = int.Parse(row["sp_EndHour"].ToString()),
                    sp_EndMinute = int.Parse(row["sp_EndMinute"].ToString()),
                    txt_ExecuteSituation = row["txt_ExecuteSituation"].ToString().Trim(),
                    txt_MaterialConsumption = row["txt_MaterialConsumption"].ToString().Trim(),
                    txt_PersonnelNumber = row["txt_PersonnelNumber"].ToString().Trim(),
                    txt_CarType = row["txt_CarType"].ToString().Trim(),
                    cb_IsFinish = int.Parse(row["cb_IsFinish"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                workOrderDetailMaintObject = new WorkOrderDetailMaintObject()
                {
                    WorkOrderDetailId = id,
                    txt_WFActivityInstanceId = Guid.Parse(row["txt_WFActivityInstanceId"].ToString()),
                    dp_WorkBeginDate = DateTime.Parse(row["dp_WorkBeginDate"].ToString()),
                    sp_BeginHour = int.Parse(row["sp_BeginHour"].ToString()),
                    sp_BeginMinute = int.Parse(row["sp_BeginMinute"].ToString()),
                    dp_WorkEndDate = DateTime.Parse(row["dp_WorkEndDate"].ToString()),
                    sp_EndHour = int.Parse(row["sp_EndHour"].ToString()),
                    sp_EndMinute = int.Parse(row["sp_EndMinute"].ToString()),
                    txt_ExecuteSituation = row["txt_ExecuteSituation"].ToString().Trim(),
                    txt_MaterialConsumption = row["txt_MaterialConsumption"].ToString().Trim(),
                    txt_PersonnelNumber = row["txt_PersonnelNumber"].ToString().Trim(),
                    txt_CarType = row["txt_CarType"].ToString().Trim(),
                    cb_IsFinish = int.Parse(row["cb_IsFinish"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IWorkOrderDetailService> proxy = new ServiceProxy<IWorkOrderDetailService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateWorkOrderDetail(workOrderDetailMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除各天执行情况
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveWorkOrderDetail()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WorkOrderDetailMaintObject> workOrderDetailMaintObjects = new List<WorkOrderDetailMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    workOrderDetailMaintObjects.Add(new WorkOrderDetailMaintObject() { WorkOrderDetailId = id });
                }
            }
            using (ServiceProxy<IWorkOrderDetailService> proxy = new ServiceProxy<IWorkOrderDetailService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveWorkOrderDetail(workOrderDetailMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }
        #endregion

        #region 隐患上报待办

        /// <summary>
        /// 隐患上报待办
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WorkApplyWait()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumOrderStateList = enumService.GetWFProcessInstanceStateEnum();
            IList<Dictionary<string, string>> enumCustomerTypeList = enumService.GetCustomerTypeEnum();
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumOrderStateList.Insert(0, allDict);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["OrderStateByAll"] = JsonHelper.Encode(enumOrderStateList);
            Dictionary<string, string> selectDict = new Dictionary<string, string>(2);
            selectDict.Add("id", "0");
            selectDict.Add("text", "请选择");
            enumCustomerTypeList.Insert(0, selectDict);
            ViewData["CustomerTypeBySelect"] = JsonHelper.Encode(enumCustomerTypeList);

            ViewData["Count"] = 0;
            ViewData["FileIdList"] = "";

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                IList<ReseauSelectObject> reseauSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetAllUsedReseaus());
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "全部" });
                ViewData["ReseauByAll"] = JsonHelper.Encode(reseauSelectObjects);
                reseauSelectObjects.RemoveAt(0);
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "请选择" });
                ViewData["ReseauBySelect"] = JsonHelper.Encode(reseauSelectObjects);
            }

            using (ServiceProxy<IWorkBigClassService> proxy = new ServiceProxy<IWorkBigClassService>())
            {
                IList<WorkBigClassSelectObject> workBigClassSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWorkBigClasss());
                workBigClassSelectObjects.Insert(0, new WorkBigClassSelectObject() { Id = Guid.Empty, BigClassName = "全部" });
                ViewData["WorkBigClassByAll"] = JsonHelper.Encode(workBigClassSelectObjects);
                workBigClassSelectObjects.RemoveAt(0);
                workBigClassSelectObjects.Insert(0, new WorkBigClassSelectObject() { Id = Guid.Empty, BigClassName = "请选择" });
                ViewData["WorkBigClassBySelect"] = JsonHelper.Encode(workBigClassSelectObjects);
            }

            using (ServiceProxy<ICustomerService> proxy = new ServiceProxy<ICustomerService>())
            {
                IList<CustomerMaintObject> customerMaintObjects = await Task.Factory.StartNew(() => proxy.Channel.GetCustomersByType(1));
                customerMaintObjects.Insert(0, new CustomerMaintObject() { Id = Guid.Empty, CustomerName = "全部" });
                ViewData["CustomerByAll"] = JsonHelper.Encode(customerMaintObjects);
                customerMaintObjects.RemoveAt(0);
                customerMaintObjects.Insert(0, new CustomerMaintObject() { Id = Guid.Empty, CustomerName = "请选择" });
                ViewData["CustomerBySelect"] = JsonHelper.Encode(customerMaintObjects);
            }

            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                IList<UserSelectObject> userSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedUsers(Guid.Empty));
                userSelectObjects.Insert(0, new UserSelectObject() { Id = Guid.Empty, FullName = "全部" });
                ViewData["UserByAll"] = JsonHelper.Encode(userSelectObjects);
                userSelectObjects.RemoveAt(0);
                userSelectObjects.Insert(0, new UserSelectObject() { Id = Guid.Empty, FullName = "请选择" });
                ViewData["UserBySelect"] = JsonHelper.Encode(userSelectObjects);
            }

            return View();
        }

        /// <summary>
        /// 获取分页隐患上报列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWorkApplyWaitPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["Title"] == null)
            {
                throw new ArgumentNullException("Title");
            }
            if (Request["IsSoved"] == null)
            {
                throw new ArgumentNullException("IsSoved");
            }

            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWorkApplyWaitPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["Title"].Trim(), int.Parse(Request["IsSoved"]), this.UserId));
            }
        }

        /// <summary>
        /// 保存根据隐患上报新增的零星派工单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWorkOrderByWorkApply()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }
            if (Request["dataRows"] == null)
            {
                throw new ArgumentNullException("dataRows");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            WorkOrderMaintObject workOrderMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                workOrderMaintObject = new WorkOrderMaintObject()
                {
                    Id = Guid.Empty,
                    WorkApplyId = Guid.Parse(row["WorkApplyId"].ToString()),
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    WorkSmallClassId = Guid.Parse(row["WorkSmallClassId"].ToString()),
                    RequireSendDate = DateTime.Parse(row["RequireSendDate"].ToString().Trim()),
                    SceneContactMan = row["SceneContactMan"].ToString(),
                    SceneContactTel = row["SceneContactTel"].ToString(),
                    CustomerId = Guid.Parse(row["CustomerId"].ToString()),
                    Days = int.Parse(row["Days"].ToString()),
                    CustomerUserId = Guid.Parse(row["CustomerUserId"].ToString()),
                    //MaintainContactTel = row["MaintainContactTel"].ToString(),
                    WorkContent = row["WorkContent"].ToString(),
                    HumanRequire = row["HumanRequire"].ToString(),
                    CarRequire = row["CarRequire"].ToString(),
                    MaterialRequire = row["MaterialRequire"].ToString(),
                    FileIdList = row["FileIdList"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            ArrayList als = (ArrayList)JsonHelper.Decode(Request["dataRows"]);
            IList<WorkApplyMaintObject> workApplyMaintObjects = new List<WorkApplyMaintObject>();
            foreach (Dictionary<string, object> al in als)
            {
                Guid workApplyId = Guid.Parse((al["Id"] ?? Guid.Empty).ToString());
                if (workApplyId != Guid.Empty)
                {
                    workApplyMaintObjects.Add(new WorkApplyMaintObject() { Id = workApplyId });
                }
            }
            using (ServiceProxy<IWorkOrderService> proxy = new ServiceProxy<IWorkOrderService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveWorkOrderByWorkApply(workOrderMaintObject, workApplyMaintObjects));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 关联隐患上报与零星派工单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWorkApplyAssociate()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }
            if (Request["dataRows"] == null)
            {
                throw new ArgumentNullException("dataRows");
            }

            ArrayList row = (ArrayList)JsonHelper.Decode(Request["data"]);
            WorkOrderMaintObject workOrderMaintObject = null;
            foreach (Dictionary<string, object> r in row)
            {
                workOrderMaintObject = new WorkOrderMaintObject()
                {
                    Id = Guid.Parse((r["Id"] ?? Guid.Empty).ToString()),
                };
                break;
            }

            ArrayList als = (ArrayList)JsonHelper.Decode(Request["dataRows"]);
            IList<WorkApplyMaintObject> workApplyMaintObjects = new List<WorkApplyMaintObject>();
            foreach (Dictionary<string, object> al in als)
            {
                Guid workApplyId = Guid.Parse((al["Id"] ?? Guid.Empty).ToString());
                if (workApplyId != Guid.Empty)
                {
                    workApplyMaintObjects.Add(new WorkApplyMaintObject() { Id = workApplyId });
                }
            }
            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveWorkApplyAssociate(workOrderMaintObject, workApplyMaintObjects));
            }
            return this.Sucess("关联成功");
        }

        #endregion

        #region 隐患上报清单

        /// <summary>
        /// 隐患上报清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WorkApplyReport()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumOrderStateList = enumService.GetWFProcessInstanceStateEnum();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            ViewData["OrderState"] = JsonHelper.Encode(enumOrderStateList);
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumOrderStateList.Insert(0, allDict);
            enumBoolList.Insert(0, allDict);
            ViewData["OrderStateByAll"] = JsonHelper.Encode(enumOrderStateList);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                IList<ReseauSelectObject> reseauSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetAllUsedReseaus());
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "全部" });
                ViewData["ReseauByAll"] = JsonHelper.Encode(reseauSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页隐患上报列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWorkApplysReport()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["Title"] == null)
            {
                throw new ArgumentNullException("Title");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["CustomerId"] == null)
            {
                throw new ArgumentNullException("CustomerId");
            }
            if (Request["OrderState"] == null)
            {
                throw new ArgumentNullException("OrderState");
            }
            if (Request["IsSoved"] == null)
            {
                throw new ArgumentNullException("IsSoved");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }

            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWorkApplysReport(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["Title"].Trim(), Guid.Parse(Request["ReseauId"]),
                    Guid.Parse(Request["CustomerId"]), int.Parse(Request["OrderState"]), int.Parse(Request["IsSoved"]), Guid.Parse(Request["CreateUserId"])));
            }
        }
        #endregion

        #region 零星派工清单

        /// <summary>
        /// 零星派工清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WorkOrderReport()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumOrderStateList = enumService.GetWFProcessInstanceStateEnum();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            ViewData["OrderState"] = JsonHelper.Encode(enumOrderStateList);
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumOrderStateList.Insert(0, allDict);
            enumBoolList.Insert(0, allDict);
            ViewData["OrderStateByAll"] = JsonHelper.Encode(enumOrderStateList);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["Count"] = 0;
            ViewData["FileIdList"] = "";

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                IList<ReseauSelectObject> reseauSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetAllUsedReseaus());
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "全部" });
                ViewData["ReseauByAll"] = JsonHelper.Encode(reseauSelectObjects);
            }

            using (ServiceProxy<IWorkBigClassService> proxy = new ServiceProxy<IWorkBigClassService>())
            {
                IList<WorkBigClassSelectObject> workBigClassSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWorkBigClasss());
                workBigClassSelectObjects.Insert(0, new WorkBigClassSelectObject() { Id = Guid.Empty, BigClassName = "全部" });
                ViewData["WorkBigClassByAll"] = JsonHelper.Encode(workBigClassSelectObjects);
                workBigClassSelectObjects.RemoveAt(0);
                workBigClassSelectObjects.Insert(0, new WorkBigClassSelectObject() { Id = Guid.Empty, BigClassName = "请选择" });
                ViewData["WorkBigClassBySelect"] = JsonHelper.Encode(workBigClassSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页零星派工列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWorkOrdersReport()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["Title"] == null)
            {
                throw new ArgumentNullException("Title");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["WorkBigClassId"] == null)
            {
                throw new ArgumentNullException("WorkBigClassId");
            }
            if (Request["WorkSmallClassId"] == null)
            {
                throw new ArgumentNullException("WorkSmallClassId");
            }
            if (Request["CustomerId"] == null)
            {
                throw new ArgumentNullException("CustomerId");
            }
            if (Request["MaintainContactMan"] == null)
            {
                throw new ArgumentNullException("MaintainContactMan");
            }
            if (Request["SendUserId"] == null)
            {
                throw new ArgumentNullException("SendUserId");
            }
            if (Request["IsFinish"] == null)
            {
                throw new ArgumentNullException("IsFinish");
            }
            if (Request["OrderState"] == null)
            {
                throw new ArgumentNullException("OrderState");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }

            using (ServiceProxy<IWorkOrderService> proxy = new ServiceProxy<IWorkOrderService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWorkOrdersPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["Title"].Trim(), Guid.Parse(Request["ReseauId"]), Guid.Parse(Request["WorkBigClassId"]),
                    Guid.Parse(Request["WorkSmallClassId"]), Guid.Parse(Request["CustomerId"]), Request["MaintainContactMan"].Trim(), Guid.Parse(Request["SendUserId"]), int.Parse(Request["IsFinish"]),
                    int.Parse(Request["OrderState"]), Guid.Parse(Request["CreateUserId"])));
            }
        }
        #endregion

        #region 项目设置
        /// <summary>
        /// 项目设置
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ProjectSetting()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumConstructionMethodList = enumService.GetConstructionMethodEnum();

            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumImportanceList.Insert(0, nullDict);
            //enumConstructionProgressList.Insert(0, nullDict);
            enumConstructionMethodList.Insert(0, nullDict);

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);
            ViewData["ConstructionMethod"] = JsonHelper.Encode(enumConstructionMethodList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumConstructionProgressAllList.Insert(0, allDict);

            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "6");
            Dict1.Add("text", "未完成");
            enumConstructionProgressAllList.Insert(6, Dict1);

            ViewData["ConstructionProgressByAll"] = JsonHelper.Encode(enumConstructionProgressAllList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取建设任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetConstructionTasksSettingPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["CustomerId"] == null)
            {
                throw new ArgumentNullException("CustomerId");
            }
            if (Request["ConstructionProgress"] == null)
            {
                throw new ArgumentNullException("ConstructionProgress");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetConstructionTasksPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    Guid.Parse(Request["CustomerId"]), int.Parse(Request["ConstructionProgress"]), Guid.Empty));
            }
        }

        /// <summary>
        /// 设置工程经理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SettingProjectManager()
        {
            if (Request["ProjectManagerId"] == null)
            {
                throw new ArgumentNullException("ProjectManagerId");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            Guid projectManagerId = Guid.Parse(Request["ProjectManagerId"]);
            IList<ConstructionTaskMaintObject> constructionTaskMaintObjects = new List<ConstructionTaskMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    constructionTaskMaintObjects.Add(new ConstructionTaskMaintObject() { Id = id, ProjectManagerId = projectManagerId });
                }
            }
            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SettingProjectManager(constructionTaskMaintObjects));
            }
            return this.Sucess("设置工程经理成功");
        }

        /// <summary>
        /// 设置监理单位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SettingSupervisorCustomer()
        {
            if (Request["SupervisorCustomerId"] == null)
            {
                throw new ArgumentNullException("SupervisorCustomerId");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            Guid supervisorCustomerId = Guid.Parse(Request["SupervisorCustomerId"]);
            IList<ConstructionTaskMaintObject> constructionTaskMaintObjects = new List<ConstructionTaskMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    constructionTaskMaintObjects.Add(new ConstructionTaskMaintObject() { Id = id, SupervisorCustomerId = supervisorCustomerId });
                }
            }
            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SettingSupervisorCustomer(constructionTaskMaintObjects));
            }
            return this.Sucess("设置监理单位成功");
        }
        #endregion

        #region 工程设置
        /// <summary>
        /// 工程设置
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> EngineeringSetting()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumConstructionProgressList = enumService.GetEngineeringProgressEnum();
            IList<Dictionary<string, string>> enumConstructionMethodList = enumService.GetConstructionMethodEnum();
            IList<Dictionary<string, string>> enumTaskModelList = enumService.GetTaskModelEnum();
            IList<Dictionary<string, string>> enumSubmitStateList = enumService.GetSubmitStateEnum();

            IList<Dictionary<string, string>> enumConstructionProgressAllList = enumService.GetEngineeringProgressEnum();

            Dictionary<string, string> nullDict = new Dictionary<string, string>(2);
            nullDict.Add("id", "0");
            nullDict.Add("text", "无");
            enumImportanceList.Insert(0, nullDict);
            //enumConstructionProgressList.Insert(0, nullDict);
            enumConstructionMethodList.Insert(0, nullDict);

            ViewData["Importance"] = JsonHelper.Encode(enumImportanceList);
            ViewData["ConstructionProgress"] = JsonHelper.Encode(enumConstructionProgressList);
            ViewData["ConstructionMethod"] = JsonHelper.Encode(enumConstructionMethodList);
            ViewData["TaskModel"] = JsonHelper.Encode(enumTaskModelList);
            ViewData["SubmitState"] = JsonHelper.Encode(enumSubmitStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumConstructionProgressAllList.Insert(0, allDict);
            enumTaskModelList.Insert(0, allDict);

            ViewData["TaskModelByAll"] = JsonHelper.Encode(enumTaskModelList);

            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "6");
            Dict1.Add("text", "未完成");
            enumConstructionProgressAllList.Insert(6, Dict1);

            ViewData["ConstructionProgressByAll"] = JsonHelper.Encode(enumConstructionProgressAllList);

            Dictionary<string, string> Dict2 = new Dictionary<string, string>(2);
            Dict2.Add("id", "0");
            Dict2.Add("text", "请选择");

            IList<Dictionary<string, string>> enumTowerTypeList = enumService.GetTowerTypeEnum();
            enumTowerTypeList.Insert(0, Dict2);
            ViewData["TowerTypeList"] = JsonHelper.Encode(enumTowerTypeList);

            IList<Dictionary<string, string>> enumTowerBaseTypeList = enumService.GetTowerBaseTypeEnum();
            enumTowerBaseTypeList.Insert(0, Dict2);
            ViewData["TowerBaseTypeList"] = JsonHelper.Encode(enumTowerBaseTypeList);


            IList<Dictionary<string, string>> enumMachineRoomTypeList = enumService.GetMachineRoomTypeEnum();
            enumMachineRoomTypeList.Insert(0, Dict2);
            ViewData["MachineRoomTypeList"] = JsonHelper.Encode(enumMachineRoomTypeList);


            IList<Dictionary<string, string>> enumExternalElectricList = enumService.GetExternalElectricEnum();
            enumExternalElectricList.Insert(0, Dict2);
            ViewData["ExternalElectricList"] = JsonHelper.Encode(enumExternalElectricList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取建设任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTaskPropertysSettingPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["CustomerId"] == null)
            {
                throw new ArgumentNullException("CustomerId");
            }
            if (Request["ConstructionProgress"] == null)
            {
                throw new ArgumentNullException("ConstructionProgress");
            }
            if (Request["TaskModel"] == null)
            {
                throw new ArgumentNullException("TaskModel");
            }
            if (Request["SupervisorCustomerId"] == null)
            {
                throw new ArgumentNullException("SupervisorCustomerId");
            }

            using (ServiceProxy<IConstructionTaskService> proxy = new ServiceProxy<IConstructionTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetTaskPropertysPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    Guid.Parse(Request["CustomerId"]), int.Parse(Request["ConstructionProgress"]), int.Parse(Request["TaskModel"]), Guid.Parse(Request["SupervisorCustomerId"]), Guid.Empty));
            }
        }

        /// <summary>
        /// 设置施工单位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SettingConstructionCustomer()
        {
            if (Request["ConstructionCustomerId"] == null)
            {
                throw new ArgumentNullException("ConstructionCustomerId");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            Guid constructionCustomerId = Guid.Parse(Request["ConstructionCustomerId"]);
            IList<TaskPropertyMaintObject> taskPropertyMaintObjects = new List<TaskPropertyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    taskPropertyMaintObjects.Add(new TaskPropertyMaintObject() { Id = id, ConstructionCustomerId = constructionCustomerId });
                }
            }
            using (ServiceProxy<ITaskPropertyService> proxy = new ServiceProxy<ITaskPropertyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SettingConstructionCustomer(taskPropertyMaintObjects));
            }
            return this.Sucess("设置施工单位成功");
        }
        #endregion

        #region 新增基站

        /// <summary>
        /// 新增基站
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> NewPlanning()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumUrgencyList = enumService.GetUrgencyEnum();
            IList<Dictionary<string, string>> enumDemandList = enumService.GetDemandEnum("2,3");
            IList<Dictionary<string, string>> enumDemandStateList = enumService.GetDemandStateEnum();
            IList<Dictionary<string, string>> enumAddressingStateList = enumService.GetAddressingStateEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Urgency"] = JsonHelper.Encode(enumUrgencyList);
            ViewData["Demand"] = JsonHelper.Encode(enumDemandList);
            ViewData["DemandState"] = JsonHelper.Encode(enumDemandStateList);
            ViewData["AddressingState"] = JsonHelper.Encode(enumAddressingStateList);
            ViewData["Count"] = 0;

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumUrgencyList.Insert(0, allDict);
            enumDemandList.Insert(0, allDict);
            enumDemandStateList.Insert(0, allDict);
            enumAddressingStateList.Insert(0, allDict);

            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["UrgencyByAll"] = JsonHelper.Encode(enumUrgencyList);
            ViewData["DemandByAll"] = JsonHelper.Encode(enumDemandList);
            ViewData["DemandStateByAll"] = JsonHelper.Encode(enumDemandStateList);
            ViewData["AddressingStateByAll"] = JsonHelper.Encode(enumAddressingStateList);

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<ISceneService> proxy = new ServiceProxy<ISceneService>())
            {
                IList<SceneSelectObject> sceneSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedScenes());
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "全部" });
                ViewData["ScenesByAll"] = JsonHelper.Encode(sceneSelectObjects);
                sceneSelectObjects.RemoveAt(0);
                sceneSelectObjects.Insert(0, new SceneSelectObject() { Id = Guid.Empty, SceneName = "请选择" });
                ViewData["ScenesBySelect"] = JsonHelper.Encode(sceneSelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanysByNature(2));
                companySelectObjects.Insert(0, new CompanySelectObject() { Id = Guid.Empty, CompanyName = "全部" });
                ViewData["CompanysByAll"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 根据规划Id获取规划
        /// </summary>
        /// <param name="id">规划Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetNewPlanningById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetNewPlanningById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页规划列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNewPlanningsPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["PlanningCode"] == null)
            {
                throw new ArgumentNullException("PlanningCode");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["PlaceCategoryId"] == null)
            {
                throw new ArgumentNullException("PlaceCategoryId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["SceneId"] == null)
            {
                throw new ArgumentNullException("SceneId");
            }
            if (Request["TelecomDemand"] == null)
            {
                throw new ArgumentNullException("TelecomDemand");
            }
            if (Request["MobileDemand"] == null)
            {
                throw new ArgumentNullException("MobileDemand");
            }
            if (Request["UnicomDemand"] == null)
            {
                throw new ArgumentNullException("UnicomDemand");
            }
            if (Request["AddressingState"] == null)
            {
                throw new ArgumentNullException("AddressingState");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }

            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetNewPlanningsPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlanningCode"].Trim(), Request["PlanningName"].Trim(),
                    PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), Guid.Parse(Request["SceneId"]),
                    int.Parse(Request["TelecomDemand"]), int.Parse(Request["MobileDemand"]), int.Parse(Request["UnicomDemand"]), int.Parse(Request["AddressingState"]),
                    Guid.Parse(Request["CreateUserId"])));
            }
        }

        /// <summary>
        /// 保存规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveNewPlanning()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            NewPlanningMaintObject newPlanningMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                newPlanningMaintObject = new NewPlanningMaintObject()
                {
                    Id = Guid.Empty,
                    PlanningName = row["PlanningName"].ToString().Trim(),
                    Profession = PROFESSION,
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    AreaId = Guid.Parse(row["AreaId"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    FileIdList = row["FileIdList"].ToString(),
                    SceneId = Guid.Parse(row["SceneId"].ToString()),
                    OwnerName = row["OwnerName"].ToString(),
                    OwnerContact = row["OwnerContact"].ToString(),
                    OwnerPhoneNumber = row["OwnerPhoneNumber"].ToString(),
                    DetailedAddress = row["DetailedAddress"].ToString(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    MobileDemand = int.Parse(row["MobileDemand"].ToString()),
                    MobileAntennaHeight = row["MobileAntennaHeight"].ToString().Trim() == "" ? 0 : decimal.Parse(row["MobileAntennaHeight"].ToString()),
                    MobilePoleNumber = row["MobilePoleNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["MobilePoleNumber"].ToString()),
                    MobileCabinetNumber = row["MobileCabinetNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["MobileCabinetNumber"].ToString()),
                    MobileUserId = Guid.Parse(row["MobileUserId"].ToString()),
                    TelecomDemand = int.Parse(row["TelecomDemand"].ToString()),
                    TelecomAntennaHeight = row["TelecomAntennaHeight"].ToString().Trim() == "" ? 0 : decimal.Parse(row["TelecomAntennaHeight"].ToString()),
                    TelecomPoleNumber = row["TelecomPoleNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["TelecomPoleNumber"].ToString()),
                    TelecomCabinetNumber = row["TelecomCabinetNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["TelecomCabinetNumber"].ToString()),
                    TelecomUserId = Guid.Parse(row["TelecomUserId"].ToString()),
                    UnicomDemand = int.Parse(row["UnicomDemand"].ToString()),
                    UnicomAntennaHeight = row["UnicomAntennaHeight"].ToString().Trim() == "" ? 0 : decimal.Parse(row["UnicomAntennaHeight"].ToString()),
                    UnicomPoleNumber = row["UnicomPoleNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["UnicomPoleNumber"].ToString()),
                    UnicomCabinetNumber = row["UnicomCabinetNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["UnicomCabinetNumber"].ToString()),
                    UnicomUserId = Guid.Parse(row["UnicomUserId"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                newPlanningMaintObject = new NewPlanningMaintObject()
                {
                    Id = id,
                    PlanningName = row["PlanningName"].ToString().Trim(),
                    Profession = PROFESSION,
                    PlaceCategoryId = Guid.Parse(row["PlaceCategoryId"].ToString()),
                    AreaId = Guid.Parse(row["AreaId"].ToString()),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    Lng = decimal.Parse(row["Lng"].ToString()),
                    Lat = decimal.Parse(row["Lat"].ToString()),
                    FileIdList = row["FileIdList"].ToString(),
                    SceneId = Guid.Parse(row["SceneId"].ToString()),
                    OwnerName = row["OwnerName"].ToString(),
                    OwnerContact = row["OwnerContact"].ToString(),
                    OwnerPhoneNumber = row["OwnerPhoneNumber"].ToString(),
                    DetailedAddress = row["DetailedAddress"].ToString(),
                    Remarks = row["Remarks"].ToString().Trim(),
                    MobileDemand = int.Parse(row["MobileDemand"].ToString()),
                    MobileAntennaHeight = row["MobileAntennaHeight"].ToString().Trim() == "" ? 0 : decimal.Parse(row["MobileAntennaHeight"].ToString()),
                    MobilePoleNumber = row["MobilePoleNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["MobilePoleNumber"].ToString()),
                    MobileCabinetNumber = row["MobileCabinetNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["MobileCabinetNumber"].ToString()),
                    MobileUserId = Guid.Parse(row["MobileUserId"].ToString()),
                    TelecomDemand = int.Parse(row["TelecomDemand"].ToString()),
                    TelecomAntennaHeight = row["TelecomAntennaHeight"].ToString().Trim() == "" ? 0 : decimal.Parse(row["TelecomAntennaHeight"].ToString()),
                    TelecomPoleNumber = row["TelecomPoleNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["TelecomPoleNumber"].ToString()),
                    TelecomCabinetNumber = row["TelecomCabinetNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["TelecomCabinetNumber"].ToString()),
                    TelecomUserId = Guid.Parse(row["TelecomUserId"].ToString()),
                    UnicomDemand = int.Parse(row["UnicomDemand"].ToString()),
                    UnicomAntennaHeight = row["UnicomAntennaHeight"].ToString().Trim() == "" ? 0 : decimal.Parse(row["UnicomAntennaHeight"].ToString()),
                    UnicomPoleNumber = row["UnicomPoleNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["UnicomPoleNumber"].ToString()),
                    UnicomCabinetNumber = row["UnicomCabinetNumber"].ToString().Trim() == "" ? 0 : int.Parse(row["UnicomCabinetNumber"].ToString()),
                    UnicomUserId = Guid.Parse(row["UnicomUserId"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateNewPlanning(newPlanningMaintObject));
            }
            return this.Sucess("数据保存成功");
        }


        /// <summary>
        /// 删除规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveNewPlannings()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<NewPlanningMaintObject> newPlanningMaintObjects = new List<NewPlanningMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    newPlanningMaintObjects.Add(new NewPlanningMaintObject() { Id = id, ModifyUserId = this.UserId });
                }
            }
            using (ServiceProxy<IPlanningService> proxy = new ServiceProxy<IPlanningService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveNewPlannings(newPlanningMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }


        #endregion

        #region 改造基站

        /// <summary>
        /// 改造基站
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> NewRemodeling()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumUrgencyList = enumService.GetUrgencyEnum();
            IList<Dictionary<string, string>> enumPropertyRightList = enumService.GetPropertyRightEnum();
            IList<Dictionary<string, string>> enumDemandList = enumService.GetDemandEnum();
            IList<Dictionary<string, string>> enumWFProcessInstanceStateList = enumService.GetWFProcessInstanceStateEnum();

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Urgency"] = JsonHelper.Encode(enumUrgencyList);
            ViewData["PropertyRight"] = JsonHelper.Encode(enumPropertyRightList);
            ViewData["Demand"] = JsonHelper.Encode(enumDemandList);
            ViewData["OrderState"] = JsonHelper.Encode(enumWFProcessInstanceStateList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumUrgencyList.Insert(0, allDict);
            enumWFProcessInstanceStateList.Insert(0, allDict);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["OrderStateAll"] = JsonHelper.Encode(enumWFProcessInstanceStateList);
            enumBoolList.RemoveAt(0);
            ViewData["UrgencyByAll"] = JsonHelper.Encode(enumUrgencyList);

            Dictionary<string, string> selectDict = new Dictionary<string, string>(2);
            selectDict.Add("id", "0");
            selectDict.Add("text", "请选择");
            enumBoolList.Insert(0, selectDict);
            enumPropertyRightList.Insert(0, selectDict);
            ViewData["BoolBySelect"] = JsonHelper.Encode(enumBoolList);
            ViewData["PropertyRightBySelect"] = JsonHelper.Encode(enumPropertyRightList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
                areaSelectObjects.RemoveAt(0);
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "请选择" });
                ViewData["AreasBySelect"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
                placeCategorySelectObjects.RemoveAt(0);
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "请选择" });
                ViewData["PlaceCategorysBySelect"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanysByNature(2));
                companySelectObjects.Insert(0, new CompanySelectObject() { Id = Guid.Empty, CompanyName = "全部" });
                ViewData["CompanysByAll"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }
        #endregion

        #region 待申请立项隐患上报单

        /// <summary>
        /// 待申请立项隐患上报单
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkApplyProject()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumOrderStateList = enumService.GetWFProcessInstanceStateEnum();
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumOrderStateList.Insert(0, allDict);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["OrderStateByAll"] = JsonHelper.Encode(enumOrderStateList);

            return View();
        }

        /// <summary>
        /// 获取分页隐患上报列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWorkApplyProjectPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["Title"] == null)
            {
                throw new ArgumentNullException("Title");
            }
            if (Request["ProjectCode"] == null)
            {
                throw new ArgumentNullException("ProjectCode");
            }
            if (Request["IsProject"] == null)
            {
                throw new ArgumentNullException("IsProject");
            }

            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWorkApplyProjectPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["Title"].Trim(), Request["ProjectCode"].Trim(),
                    int.Parse(Request["IsProject"]), this.UserId));
            }
        }

        /// <summary>
        /// 保存项目编码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWorkApplyProjectCode()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            WorkApplyMaintObject workApplyMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            workApplyMaintObject = new WorkApplyMaintObject()
            {
                Id = id,
                ProjectCode = row["ProjectCode"].ToString().Trim()
            };
            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveWorkApplyProjectCode(workApplyMaintObject));
            }
            return this.Sucess("项目编码保存成功");
        }

        /// <summary>
        /// 保存立项完成隐患上报
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveIsProjectWorkApplys()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WorkApplyMaintObject> workApplyMaintObjects = new List<WorkApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    workApplyMaintObjects.Add(new WorkApplyMaintObject() { Id = id, IsProject = 1 });
                }
            }
            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveIsProjectWorkApplys(workApplyMaintObjects));
            }
            return this.Sucess("立项成功");
        }

        /// <summary>
        /// 保存立项完成隐患上报
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelIsProjectWorkApplys()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WorkApplyMaintObject> workApplyMaintObjects = new List<WorkApplyMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    workApplyMaintObjects.Add(new WorkApplyMaintObject() { Id = id, IsProject = 2 });
                }
            }
            using (ServiceProxy<IWorkApplyService> proxy = new ServiceProxy<IWorkApplyService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SaveIsProjectWorkApplys(workApplyMaintObjects));
            }
            return this.Sucess("取消立项成功");
        }

        #endregion

        #region 隐患立项清单

        /// <summary>
        /// 隐患立项清单
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkApplyProjectReport()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumOrderStateList = enumService.GetWFProcessInstanceStateEnum();
            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumOrderStateList.Insert(0, allDict);
            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["OrderStateByAll"] = JsonHelper.Encode(enumOrderStateList);

            return View();
        }

        #endregion

        #region 导入立项信息

        /// <summary>
        /// 导入立项信息
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ImportProjectCodeList()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();
            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumProjectTypeList.Insert(0, allDict);
            ViewData["ProjectTypeByAll"] = JsonHelper.Encode(enumProjectTypeList);

            enumProjectTypeList.RemoveAt(0);
            Dictionary<string, string> selectDict = new Dictionary<string, string>(2);
            selectDict.Add("id", "0");
            selectDict.Add("text", "请选择");
            enumProjectTypeList.Insert(0, selectDict);
            ViewData["ProjectTypeBySelect"] = JsonHelper.Encode(enumProjectTypeList);

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                IList<ReseauSelectObject> reseauSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetAllUsedReseaus());
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "全部" });
                ViewData["ReseauByAll"] = JsonHelper.Encode(reseauSelectObjects);
                reseauSelectObjects.RemoveAt(0);
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "请选择" });
                ViewData["ReseauBySelect"] = JsonHelper.Encode(reseauSelectObjects);
            }

            return View();
        }

        /// <summary>
        /// 根据立项信息Id获取立项信息
        /// </summary>
        /// <param name="id">立项信息Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetProjectCodeListById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IProjectCodeListService> proxy = new ServiceProxy<IProjectCodeListService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetProjectCodeListById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页立项信息列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectCodeListPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["ProjectCode"] == null)
            {
                throw new ArgumentNullException("ProjectCode");
            }
            if (Request["ProjectType"] == null)
            {
                throw new ArgumentNullException("ProjectType");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceNane");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["ProjectManagerId"] == null)
            {
                throw new ArgumentNullException("ProjectManagerId");
            }

            using (ServiceProxy<IProjectCodeListService> proxy = new ServiceProxy<IProjectCodeListService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectCodeListPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["ProjectCode"].Trim(), int.Parse(Request["ProjectType"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["ReseauId"]), Guid.Parse(Request["ProjectManagerId"]), Guid.Empty));
            }
        }

        /// <summary>
        /// 保存立项信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveProjectCodeList()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            ProjectCodeListMaintObject projectCodeListMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                projectCodeListMaintObject = new ProjectCodeListMaintObject()
                {
                    Id = Guid.Empty,
                    ProjectCode = row["ProjectCode"].ToString().Trim(),
                    ProjectType = int.Parse(row["ProjectType"].ToString()),
                    ProjectDate = DateTime.Parse(row["ProjectDate"].ToString()),
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    ProjectManagerId = Guid.Parse(row["ProjectManagerId"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                projectCodeListMaintObject = new ProjectCodeListMaintObject()
                {
                    Id = id,
                    ProjectCode = row["ProjectCode"].ToString().Trim(),
                    ProjectType = int.Parse(row["ProjectType"].ToString()),
                    ProjectDate = DateTime.Parse(row["ProjectDate"].ToString()),
                    PlaceName = row["PlaceName"].ToString().Trim(),
                    ReseauId = Guid.Parse(row["ReseauId"].ToString()),
                    ProjectManagerId = Guid.Parse(row["ProjectManagerId"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IProjectCodeListService> proxy = new ServiceProxy<IProjectCodeListService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateProjectCodeList(projectCodeListMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 导入采购清单

        /// <summary>
        /// 导入采购清单
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportMaterialSpecList()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumMaterialSpecTypeList = enumService.GetMaterialSpecTypeEnum();
            ViewData["MaterialSpecType"] = JsonHelper.Encode(enumMaterialSpecTypeList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumMaterialSpecTypeList.Insert(0, allDict);
            ViewData["MaterialSpecTypeByAll"] = JsonHelper.Encode(enumMaterialSpecTypeList);

            enumMaterialSpecTypeList.RemoveAt(0);
            Dictionary<string, string> selectDict = new Dictionary<string, string>(2);
            selectDict.Add("id", "0");
            selectDict.Add("text", "请选择");
            enumMaterialSpecTypeList.Insert(0, selectDict);
            ViewData["MaterialSpecTypeBySelect"] = JsonHelper.Encode(enumMaterialSpecTypeList);

            return View();
        }

        /// <summary>
        /// 根据采购清单Id获取采购清单
        /// </summary>
        /// <param name="id">采购清单Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetMaterialSpecListById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IMaterialSpecListService> proxy = new ServiceProxy<IMaterialSpecListService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMaterialSpecListById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取分页导入采购清单列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMaterialSpecListPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["ProjectCode"] == null)
            {
                throw new ArgumentNullException("ProjectCode");
            }
            if (Request["CustomerName"] == null)
            {
                throw new ArgumentNullException("CustomerName");
            }
            if (Request["MaterialSpecType"] == null)
            {
                throw new ArgumentNullException("MaterialSpecType");
            }
            if (Request["MaterialSpecName"] == null)
            {
                throw new ArgumentNullException("MaterialSpecName");
            }
            if (Request["OrderCode"] == null)
            {
                throw new ArgumentNullException("OrderCode");
            }

            using (ServiceProxy<IMaterialSpecListService> proxy = new ServiceProxy<IMaterialSpecListService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetMaterialSpecListPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["ProjectCode"].Trim(), Request["CustomerName"].Trim(), int.Parse(Request["MaterialSpecType"]), Request["MaterialSpecName"].Trim(),
                    Request["OrderCode"].Trim(), Guid.Empty));
            }
        }

        /// <summary>
        /// 保存立项信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveMaterialSpecList()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            MaterialSpecListMaintObject materialSpecListMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                materialSpecListMaintObject = new MaterialSpecListMaintObject()
                {
                    Id = Guid.Empty,
                    ProjectCode = row["ProjectCode"].ToString().Trim(),
                    CustomerName = row["CustomerName"].ToString().Trim(),
                    MaterialSpecType = int.Parse(row["MaterialSpecType"].ToString()),
                    MaterialSpecName = row["MaterialSpecName"].ToString().Trim(),
                    UnitPrice = row["UnitPrice"].ToString().Trim() == "" ? 0 : decimal.Parse(row["UnitPrice"].ToString()),
                    SpecNumber = row["SpecNumber"].ToString().Trim() == "" ? 0 : decimal.Parse(row["SpecNumber"].ToString()),
                    TotalPrice = row["TotalPrice"].ToString().Trim() == "" ? 0 : decimal.Parse(row["TotalPrice"].ToString()),
                    OrderCode = row["OrderCode"].ToString().Trim(),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                materialSpecListMaintObject = new MaterialSpecListMaintObject()
                {
                    Id = id,
                    ProjectCode = row["ProjectCode"].ToString().Trim(),
                    CustomerName = row["CustomerName"].ToString().Trim(),
                    MaterialSpecType = int.Parse(row["MaterialSpecType"].ToString()),
                    MaterialSpecName = row["MaterialSpecName"].ToString().Trim(),
                    UnitPrice = row["UnitPrice"].ToString().Trim() == "" ? 0 : decimal.Parse(row["UnitPrice"].ToString()),
                    SpecNumber = row["SpecNumber"].ToString().Trim() == "" ? 0 : decimal.Parse(row["SpecNumber"].ToString()),
                    TotalPrice = row["TotalPrice"].ToString().Trim() == "" ? 0 : decimal.Parse(row["TotalPrice"].ToString()),
                    OrderCode = row["OrderCode"].ToString().Trim(),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IMaterialSpecListService> proxy = new ServiceProxy<IMaterialSpecListService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateMaterialSpecList(materialSpecListMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        #endregion

        #region 导出清单

        /// <summary>
        /// 导出清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportProjectMaterial()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();
            IList<Dictionary<string, string>> enumMaterialSpecTypeList = enumService.GetMaterialSpecTypeEnum();
            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["MaterialSpecType"] = JsonHelper.Encode(enumMaterialSpecTypeList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumProjectTypeList.Insert(0, allDict);
            enumMaterialSpecTypeList.Insert(0, allDict);
            ViewData["ProjectTypeByAll"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["MaterialSpecTypeByAll"] = JsonHelper.Encode(enumMaterialSpecTypeList);

            using (ServiceProxy<IReseauService> proxy = new ServiceProxy<IReseauService>())
            {
                IList<ReseauSelectObject> reseauSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetAllUsedReseaus());
                reseauSelectObjects.Insert(0, new ReseauSelectObject() { Id = Guid.Empty, ReseauName = "全部" });
                ViewData["ReseauByAll"] = JsonHelper.Encode(reseauSelectObjects);
            }

            return View();
        }

        /// <summary>
        /// 获取分页立项信息列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectCodeListAndMaterialSpecListPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["ProjectCode"] == null)
            {
                throw new ArgumentNullException("ProjectCode");
            }
            if (Request["ProjectType"] == null)
            {
                throw new ArgumentNullException("ProjectType");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceNane");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["ProjectManagerId"] == null)
            {
                throw new ArgumentNullException("ProjectManagerId");
            }
            if (Request["CustomerName"] == null)
            {
                throw new ArgumentNullException("CustomerName");
            }
            if (Request["MaterialSpecType"] == null)
            {
                throw new ArgumentNullException("MaterialSpecType");
            }
            if (Request["MaterialSpecName"] == null)
            {
                throw new ArgumentNullException("MaterialSpecName");
            }
            if (Request["OrderCode"] == null)
            {
                throw new ArgumentNullException("OrderCode");
            }

            using (ServiceProxy<IMaterialSpecListService> proxy = new ServiceProxy<IMaterialSpecListService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectCodeListAndMaterialSpecListPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["ProjectCode"].Trim(), int.Parse(Request["ProjectType"]),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["ReseauId"]), Guid.Parse(Request["ProjectManagerId"]), Request["CustomerName"].Trim(), int.Parse(Request["MaterialSpecType"]),
                    Request["MaterialSpecName"].Trim(), Request["OrderCode"].Trim()));
            }
        }

        #endregion

    }
}