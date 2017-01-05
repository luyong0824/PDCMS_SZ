using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Communication;
using PDBM.Infrastructure.IoC;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using PDBM.ServiceContracts.Enum;
using PDBM.Web.Filters;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 自定义控件控制器
    /// </summary>
    [AuthorizeFilter]
    public class CustomControlController : BaseController
    {
        /// <summary>
        /// 用户选择控件
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> UserSelection()
        {
            ViewData["CompanyId"] = this.CompanyId;
            ViewData["DepartmentId"] = this.DepartmentId;

            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 项目选择控件
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ProjectSelection()
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["ProjectProgress"] = JsonHelper.Encode(enumService.GetProjectProgressEnum());
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            using (ServiceProxy<IAccountingEntityService> proxy = new ServiceProxy<IAccountingEntityService>())
            {
                IList<AccountingEntitySelectObject> accountingEntitySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAccountingEntitys());
                accountingEntitySelectObjects.Insert(0, new AccountingEntitySelectObject() { Id = Guid.Empty, AccountingEntityName = "全部" });
                ViewData["AccountingEntitys"] = JsonHelper.Encode(accountingEntitySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 站点选择控件
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PlaceSelection()
        {
            ViewData["ProfessionSelected"] = Request["Profession"] == null ? "0" : Request["Profession"];

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumBoolList = enumService.GetBoolEnum();
            IList<Dictionary<string, string>> enumProfessionList = enumService.GetProfessionEnum();
            IList<Dictionary<string, string>> enumPropertyRightList = enumService.GetPropertyRightEnum();

            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumBoolList.Insert(0, allDict);
            enumProfessionList.Insert(0, allDict);
            enumPropertyRightList.Insert(0, allDict);

            ViewData["Bool"] = JsonHelper.Encode(enumBoolList);
            ViewData["Profession"] = JsonHelper.Encode(enumProfessionList);
            ViewData["PropertyRight"] = JsonHelper.Encode(enumPropertyRightList);

            using (ServiceProxy<IAreaService> proxy = new ServiceProxy<IAreaService>())
            {
                IList<AreaSelectObject> areaSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedAreas());
                areaSelectObjects.Insert(0, new AreaSelectObject() { Id = Guid.Empty, AreaName = "全部" });
                ViewData["Areas"] = JsonHelper.Encode(areaSelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 往来单位选择控件
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerSelection(int typeId)
        {
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumStateList = enumService.GetStateEnum();
            IList<Dictionary<string, string>> enumCustomerTypeList = enumService.GetCustomerTypeEnum();
            ViewData["State"] = JsonHelper.Encode(enumStateList);
            ViewData["CustomerType"] = JsonHelper.Encode(enumCustomerTypeList);
            ViewData["TypeId"] = typeId;
            return View();
        }
    }
}