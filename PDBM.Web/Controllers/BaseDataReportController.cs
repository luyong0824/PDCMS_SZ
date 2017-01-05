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
    /// 基础数据报表控制器
    /// </summary>
    [AuthorizeFilter]
    public class BaseDataReportController : BaseController
    {
        private const int PROFESSION = 0;

        #region 站点业务清单

        /// <summary>
        /// 站点业务清单
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
        /// 获取分页站点业务量列表
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

        #region 站点业务月清单

        /// <summary>
        /// 站点业务月清单
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
        /// 获取站点业务量列表
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
        /// 获取站点业务月增量列表
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
        /// 获取站点业务年增量列表
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
    }
}