using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PDBM.Infrastructure.Communication;
using PDBM.ServiceContracts.DataImport;
using PDBM.Web.Filters;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 数据导入控制器
    /// </summary>
    [AuthorizeFilter]
    public class DataImportController : BaseController
    {
        /// <summary>
        /// 导入运营商基站规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportOperatorsPlanningBS()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportOperatorsPlanningBS(Guid.Parse(Request["FileId"]), this.UserId, this.CompanyId, this.CompanyNature)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入运营商共享基站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportOperatorsSharingBS()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportOperatorsSharingBS(Guid.Parse(Request["FileId"]), this.UserId, this.CompanyId, this.CompanyNature)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入购置基站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportPurchaseBS()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportPurchaseBS(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入基站规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportPlanning()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportPlanning(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入基站改造
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportRemodeling()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportRemodeling(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入基站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportPlace()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportPlace(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 更新基站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePlace()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.UpdatePlace(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入逻辑号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportLogicalNumber()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportLogicalNumber(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入业务量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportBusinessVolume()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }
            if (Request["LogicalType"] == null)
            {
                throw new ArgumentNullException("LogicalType");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportBusinessVolume(Guid.Parse(Request["FileId"]), int.Parse(Request["LogicalType"]), int.Parse(Request["Profession"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入资源信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportResources()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportResources(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入新增基站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportNewPlanningBS()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportNewPlanningBS(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入改造基站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportNewRemodelingBS()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportNewRemodelingBS(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入立项信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportProjectCodeList()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportProjectCodeList(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入采购清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportMaterialSpecList()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportMaterialSpecList(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入基站建设
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportPlanningApply()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportPlanningApply(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入室分建设
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportPlanningApplyID()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportPlanningApplyID(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入室分规划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportPlanningID()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportPlanningID(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入室分改造
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportRemodelingID()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportRemodelingID(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入室分
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportPlaceID()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.ImportPlaceID(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 更新室分
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePlaceID()
        {
            if (Request["FileId"] == null)
            {
                throw new ArgumentNullException("FileId");
            }

            using (ServiceProxy<IDataImportService> proxy = new ServiceProxy<IDataImportService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.UpdatePlaceID(Guid.Parse(Request["FileId"]), this.UserId)),
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}