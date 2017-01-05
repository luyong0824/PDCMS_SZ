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
    /// 基站报表控制器
    /// </summary>
    [AuthorizeFilter]
    public class BaseStationReportController : BaseController
    {
        private const int PROFESSION = 1;

        #region 基站清单
        /// <summary>
        /// 基站清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PlaceReport()
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
            }
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(PROFESSION));
                placeCategorySelectObjects.Insert(0, new PlaceCategorySelectObject() { Id = Guid.Empty, PlaceCategoryName = "全部" });
                ViewData["PlaceCategorysByAll"] = JsonHelper.Encode(placeCategorySelectObjects);
            }
            using (ServiceProxy<IPlaceOwnerService> proxy = new ServiceProxy<IPlaceOwnerService>())
            {
                IList<PlaceOwnerSelectObject> placeOwnerSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceOwners());
                placeOwnerSelectObjects.Insert(0, new PlaceOwnerSelectObject() { Id = Guid.Empty, PlaceOwnerName = "全部" });
                ViewData["PlaceOwnersByAll"] = JsonHelper.Encode(placeOwnerSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页站点列表(移动端)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlacesMobile()
        {
            string professionList = "0";
            if (Request["ProfessionList"] != null && Request["ProfessionList"] != "")
            {
                professionList = Request["ProfessionList"].Trim();
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
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

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlacesMobile(1, int.Parse(Request["PageSize"]), professionListSql, Request["PlaceName"].Trim(), this.CompanyId));
            }
        }

        /// <summary>
        /// 获取分页站点列表(移动端)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlacesPageMobile()
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

            using (ServiceProxy<IPlaceService> proxy = new ServiceProxy<IPlaceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetPlacesPageMobile(1, int.Parse(Request["PageSize"]), professionListSql, decimal.Parse(Request["Lng"]), decimal.Parse(Request["Lat"]), decimal.Parse(Request["Distance"]), this.CompanyId));
            }
        }
        #endregion

        #region 租赁进度表

        /// <summary>
        /// 租赁进度表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Addressing()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumImportanceList = enumService.GetImportanceEnum();
            IList<Dictionary<string, string>> enumAddressingStateList = enumService.GetAddressingStateEnum();

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
            }
            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                IList<DepartmentSelectObject> departmentSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedDepartments(this.CompanyId));
                departmentSelectObjects.Insert(0, new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "全部" });
                ViewData["AddressingDepartmentsByAll"] = JsonHelper.Encode(departmentSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页寻址确认列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAddressingReportPage()
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
            if (Request["AddressingDepartmentId"] == null)
            {
                throw new ArgumentNullException("AddressingDepartmentId");
            }
            if (Request["AddressingUserId"] == null)
            {
                throw new ArgumentNullException("AddressingUserId");
            }
            if (Request["IsAppoint"] == null)
            {
                throw new ArgumentNullException("IsAppoint");
            }

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetAddressingReportPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlanningCode"].Trim(), Request["PlanningName"].Trim(),
                    PROFESSION, Guid.Parse(Request["PlaceCategoryId"]), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["Importance"]),
                    int.Parse(Request["AddressingState"]), Guid.Parse(Request["AddressingDepartmentId"]), Guid.Parse(Request["AddressingUserId"]), int.Parse(Request["IsAppoint"]),
                    this.CompanyId));
            }
        }
        #endregion

        #region 项目进度表

        /// <summary>
        /// 项目进度表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ProjectProgress()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();
            IList<Dictionary<string, string>> enumProjectProgressList = enumService.GetProjectProgressEnum();

            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["ProjectProgress"] = JsonHelper.Encode(enumProjectProgressList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumProjectTypeList.Insert(0, allDict);
            enumProjectProgressList.Insert(0, allDict);
            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "7");
            Dict1.Add("text", "未完成");
            enumProjectProgressList.Insert(7, Dict1);

            ViewData["BoolByAll"] = JsonHelper.Encode(enumBoolList);
            ViewData["ProjectTypesByAll"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["ProjectProgressByAll"] = JsonHelper.Encode(enumProjectProgressList);

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
        public async Task<string> GetProjectProgresssReportPage()
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
            if (Request["ProjectType"] == null)
            {
                throw new ArgumentNullException("ProjectType");
            }
            if (Request["ProjectProgress"] == null)
            {
                throw new ArgumentNullException("ProjectProgress");
            }
            if (Request["ProjectManagerId"] == null)
            {
                throw new ArgumentNullException("ProjectManagerId");
            }
            if (Request["IsOverTime"] == null)
            {
                throw new ArgumentNullException("IsOverTime");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectProgresssReportPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["ProjectCode"].Trim(), Request["PlaceName"].Trim(), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["ProjectType"]),
                    int.Parse(Request["ProjectProgress"]), Guid.Parse(Request["ProjectManagerId"]), int.Parse(Request["IsOverTime"]), PROFESSION, this.CompanyId));
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
            enumProjectTypeList.Insert(0, allDict);
            enumTaskModelList.Insert(0, allDict);
            enumEngineeringProgressList.Insert(0, allDict);
            Dictionary<string, string> Dict1 = new Dictionary<string, string>(2);
            Dict1.Add("id", "6");
            Dict1.Add("text", "未完成");
            enumEngineeringProgressList.Insert(6, Dict1);

            ViewData["ProjectTypeByAll"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["TaskModelByAll"] = JsonHelper.Encode(enumTaskModelList);
            ViewData["EngineeringProgressByAll"] = JsonHelper.Encode(enumEngineeringProgressList);

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
        public async Task<string> GetEngineeringProgressReportPage()
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
            if (Request["TaskModel"] == null)
            {
                throw new ArgumentNullException("TaskModel");
            }
            if (Request["EngineeringProgress"] == null)
            {
                throw new ArgumentNullException("EngineeringProgress");
            }
            if (Request["ProjectType"] == null)
            {
                throw new ArgumentNullException("ProjectType");
            }
            if (Request["ProjectManagerId"] == null)
            {
                throw new ArgumentNullException("ProjectManagerId");
            }
            if (Request["ConstructionCustomerId"] == null)
            {
                throw new ArgumentNullException("ConstructionCustomerId");
            }
            if (Request["SupervisionCustomerId"] == null)
            {
                throw new ArgumentNullException("SupervisionCustomerId");
            }

            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetEngineeringProgressReportPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["ProjectCode"].Trim(), Request["PlaceName"].Trim(), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), int.Parse(Request["TaskModel"]),
                    int.Parse(Request["EngineeringProgress"]), int.Parse(Request["ProjectType"]), Guid.Parse(Request["ProjectManagerId"]), Guid.Parse(Request["ConstructionCustomerId"]),
                    Guid.Parse(Request["SupervisionCustomerId"]), PROFESSION, this.CompanyId));
            }
        }
        #endregion

        #region 项目设计清单

        /// <summary>
        /// 项目设计清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ProjectDesign()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();

            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页项目设计清单列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectDesignReportPage()
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
            if (Request["GeneralDesignId"] == null)
            {
                throw new ArgumentNullException("GeneralDesignId");
            }
            if (Request["DesignRealName"] == null)
            {
                throw new ArgumentNullException("DesignRealName");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectDesignReportPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]), Request["ProjectCode"].Trim(),
                    Request["PlaceName"].Trim(), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), Guid.Parse(Request["GeneralDesignId"]), Request["DesignRealName"].Trim(), PROFESSION,
                    this.CompanyId));
            }
        }

        #endregion

        #region 工程设计清单

        /// <summary>
        /// 工程设计清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> EngineeringDesign()
        {
            ViewData["UserId"] = this.UserId;
            ViewData["FullName"] = this.FullName;

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumProjectTypeList = enumService.GetProjectTypeEnum();
            IList<Dictionary<string, string>> enumTaskModelList = enumService.GetTaskModelEnum();

            ViewData["ProjectType"] = JsonHelper.Encode(enumProjectTypeList);
            ViewData["TaskModel"] = JsonHelper.Encode(enumTaskModelList);

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumTaskModelList.Insert(0, allDict);

            ViewData["TaskModelByAll"] = JsonHelper.Encode(enumTaskModelList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["AreasByAll"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取分页工程设计清单列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetEngineeringDesignReportPage()
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
            if (Request["TaskModel"] == null)
            {
                throw new ArgumentNullException("TaskModel");
            }
            if (Request["DesignRealName"] == null)
            {
                throw new ArgumentNullException("DesignRealName");
            }
            if (Request["DesignCustomerId"] == null)
            {
                throw new ArgumentNullException("DesignCustomerId");
            }

            using (ServiceProxy<IEngineeringTaskService> proxy = new ServiceProxy<IEngineeringTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetEngineeringDesignReportPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    Request["ProjectCode"].Trim(), Request["PlaceName"].Trim(), Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]),
                    int.Parse(Request["TaskModel"]), Request["DesignRealName"].Trim(), Guid.Parse(Request["DesignCustomerId"]), PROFESSION, this.CompanyId));
            }
        }
        #endregion

        #region 逻辑号业务清单

        /// <summary>
        /// 逻辑号业务清单
        /// </summary>
        /// <returns></returns>
        public ActionResult LogicalBusinessVolume()
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
        #endregion

        #region 基站业务清单

        /// <summary>
        /// 基站业务清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> BusinessVolume()
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
        /// 获取分页基站业务量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeReportPage()
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
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeReportPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["PlaceName"].Trim(), Guid.Parse(Request["AreaId"]),
                    Guid.Parse(Request["ReseauId"]), PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 网格业务清单

        /// <summary>
        /// 网格业务清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> BusinessVolumeReseau()
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
        /// 获取网格业务量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeReseau()
        {
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
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeReseau(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1),
                    Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 区域业务清单

        /// <summary>
        /// 区域业务清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> BusinessVolumeArea()
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
        /// 获取区域业务量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeArea()
        {
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

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeArea(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1),
                    Guid.Parse(Request["AreaId"]), PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 公司业务清单

        /// <summary>
        /// 公司业务清单
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessVolumeCompany()
        {
            return View();
        }

        /// <summary>
        /// 获取公司业务量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeCompany()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeCompany(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1),
                    PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 基站业务月清单

        /// <summary>
        /// 基站业务月清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> BusinessVolumeMonthPlace()
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
        /// 获取基站业务量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeMonthPlace()
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
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["SortField"] == null)
            {
                throw new ArgumentNullException("SortField");
            }
            if (Request["SortOrder"] == null)
            {
                throw new ArgumentNullException("SortOrder");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeMonthPlace(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddMonths(1), Guid.Parse(Request["AreaId"]),
                    Guid.Parse(Request["ReseauId"]), Request["PlaceName"].Trim(), PROFESSION, this.CompanyId, Request["SortField"].Trim(), Request["SortOrder"].Trim()));
            }
        }

        /// <summary>
        /// 获取基站业务月增量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeMonthRisePlace()
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

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeMonthRisePlace(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddMonths(1), PROFESSION, this.CompanyId));
            }
        }

        /// <summary>
        /// 获取基站业务年增量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeYearRisePlace()
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

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeYearRisePlace(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddYears(1), PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 网格业务月清单

        /// <summary>
        /// 网格业务月清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> BusinessVolumeMonthReseau()
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
        /// 获取网格业务量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeMonthReseau()
        {
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
            if (Request["ReseauId"] == null)
            {
                throw new ArgumentNullException("ReseauId");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeMonthReseau(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddMonths(1),
                    Guid.Parse(Request["AreaId"]), Guid.Parse(Request["ReseauId"]), PROFESSION, this.CompanyId));
            }
        }

        /// <summary>
        /// 获取网格业务月增量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeMonthRiseReseau()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeMonthRiseReseau(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddMonths(1),
                    PROFESSION, this.CompanyId));
            }
        }

        /// <summary>
        /// 获取网格业务年增量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeYearRiseReseau()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeYearRiseReseau(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddYears(1),
                    PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 区域业务月清单

        /// <summary>
        /// 区域业务月清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> BusinessVolumeMonthArea()
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
        /// 获取区域业务量月列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeMonthArea()
        {
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

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeMonthArea(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddMonths(1),
                    Guid.Parse(Request["AreaId"]), PROFESSION, this.CompanyId));
            }
        }

        /// <summary>
        /// 获取区域业务月增量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeMonthRiseArea()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeMonthRiseArea(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddMonths(1),
                    PROFESSION, this.CompanyId));
            }
        }

        /// <summary>
        /// 获取区域业务年增量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeYearRiseArea()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeYearRiseArea(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddYears(1),
                    PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 公司业务月清单

        /// <summary>
        /// 公司业务月清单
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessVolumeMonthCompany()
        {
            return View();
        }

        /// <summary>
        /// 获取公司业务量月列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeMonthCompany()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeMonthCompany(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddMonths(1),
                    PROFESSION, this.CompanyId));
            }
        }

        /// <summary>
        /// 获取公司业务月增量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeMonthRiseCompany()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeMonthRiseCompany(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddMonths(1),
                    PROFESSION, this.CompanyId));
            }
        }

        /// <summary>
        /// 获取公司业务年增量列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeYearRiseCompany()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeYearRiseCompany(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddYears(1),
                    PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 网格年度成长报表

        /// <summary>
        /// 网格年度成长报表
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessVolumeYearGrowthReseau()
        {
            return View();
        }

        /// <summary>
        /// 获取网格年底成长列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeYearGrowthReseau()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeYearGrowthReseau(DateTime.Parse(Request["BeginDate"]), PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 区域年度成长报表

        /// <summary>
        /// 区域年度成长报表
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessVolumeYearGrowthArea()
        {
            return View();
        }

        /// <summary>
        /// 获取区域年底成长列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeYearGrowthArea()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeYearGrowthArea(DateTime.Parse(Request["BeginDate"]), PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 公司年度成长报表

        /// <summary>
        /// 公司年度成长报表
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessVolumeYearGrowthCompany()
        {
            return View();
        }

        /// <summary>
        /// 获取公司年底成长列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBusinessVolumeYearGrowthCompany()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }

            using (ServiceProxy<IBusinessVolumeService> proxy = new ServiceProxy<IBusinessVolumeService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetBusinessVolumeYearGrowthCompany(DateTime.Parse(Request["BeginDate"]), PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 项目经理月报

        /// <summary>
        /// 项目经理月报
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ProjectTaskProjectManager()
        {
            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                IList<DepartmentSelectObject> departmentSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedDepartments(this.CompanyId));
                departmentSelectObjects.Insert(0, new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "全部" });
                ViewData["DepartmentsByAll"] = JsonHelper.Encode(departmentSelectObjects);
            }

            return View();
        }

        /// <summary>
        /// 获取项目经理月报
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectTaskProjectManager()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["BeginDateYear"] == null)
            {
                throw new ArgumentNullException("BeginDateYear");
            }
            if (Request["DepartmentId"] == null)
            {
                throw new ArgumentNullException("DepartmentId");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskProjectManager(DateTime.Parse(Request["BeginDate"]),
                    DateTime.Parse(Request["BeginDateYear"]), Guid.Parse(Request["DepartmentId"]), PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 部门建设月报

        /// <summary>
        /// 部门建设月报
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectTaskDepartment()
        {
            return View();
        }

        /// <summary>
        /// 获取项目经理月报
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetProjectTaskDepartment()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["BeginDateYear"] == null)
            {
                throw new ArgumentNullException("BeginDateYear");
            }

            using (ServiceProxy<IProjectTaskService> proxy = new ServiceProxy<IProjectTaskService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetProjectTaskDepartment(DateTime.Parse(Request["BeginDate"]),
                    DateTime.Parse(Request["BeginDateYear"]), PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 租赁月报

        /// <summary>
        /// 租赁月报
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AddressingMonthReseau()
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
        /// 获取租赁月报列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAddressingMonthReseau()
        {
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

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetAddressingMonthReseau(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1),
                    Guid.Parse(Request["AreaId"]), PROFESSION, this.CompanyId));
            }
        }

        #endregion

        #region 租赁人月报

        /// <summary>
        /// 租赁人月报
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AddressingMonthUser()
        {
            using (ServiceProxy<IDepartmentService> proxy = new ServiceProxy<IDepartmentService>())
            {
                IList<DepartmentSelectObject> departmentSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedDepartments(this.CompanyId));
                departmentSelectObjects.Insert(0, new DepartmentSelectObject() { Id = Guid.Empty, DepartmentName = "全部" });
                ViewData["DepartmentsByAll"] = JsonHelper.Encode(departmentSelectObjects);
            }

            return View();
        }

        /// <summary>
        /// 获取租赁人月报列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAddressingMonthUser()
        {
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["DepartmentId"] == null)
            {
                throw new ArgumentNullException("DepartmentId");
            }

            using (ServiceProxy<IAddressingService> proxy = new ServiceProxy<IAddressingService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetAddressingMonthUser(DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1),
                    Guid.Parse(Request["DepartmentId"]), PROFESSION, this.CompanyId));
            }
        }

        #endregion
    }
}