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
    /// 室分建维控制器
    /// </summary>
    [AuthorizeFilter]
    public class IndoorDistributionBMController : BaseController
    {
        private const int PROFESSION = 2;

        #region 室分建设申请

        /// <summary>
        /// 室分建设申请
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
        /// 根据室分建设申请单获取室分建设申请单信息
        /// </summary>
        /// <param name="id">室分建设申请单Id</param>
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
        /// 根据室分建设申请单获取相关联的室分建设申请
        /// </summary>
        /// <param name="id">室分建设申请单Id</param>
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
        /// 取消关联室分建设申请
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

        #region 待处理室分建设申请

        /// <summary>
        /// 待处理室分建设申请
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

        #region 室分规划

        /// <summary>
        /// 室分规划
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
                    OptionalAddress = "",
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
                    OptionalAddress = "",
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
                OptionalAddress = "",
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

        #region 室分改造

        /// <summary>
        /// 室分改造
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
        /// 保存室分改造
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
        /// 删除室分改造
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
                ViewData["Mark5"] = projectTaskEditObject.Mark5;
                ViewData["Mark6"] = projectTaskEditObject.Mark6;
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
        /// 获取分页规划列表
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
        /// 根据规划Id获取规划
        /// </summary>
        /// <param name="id">规划Id</param>
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
        /// 保存施工设计
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
        /// 获取分页规划列表
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
        /// 保存施工设计
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
        #endregion

        #region 室分导入

        /// <summary>
        /// 室分导入
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
    }
}