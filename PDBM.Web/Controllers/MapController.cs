using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Communication;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using PDBM.ServiceContracts.Map;
using PDBM.Web.Filters;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 地图控制器
    /// </summary>
    [AuthorizeFilter]
    public class MapController : BaseController
    {
        /// <summary>
        /// 地图导航
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> NavigationMap()
        {
            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> placeCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(1));
                ViewData["BSPlaceCategorys"] = JsonHelper.Encode(placeCategorySelectObjects);
            }

            using (ServiceProxy<IPlaceCategoryService> proxy = new ServiceProxy<IPlaceCategoryService>())
            {
                IList<PlaceCategorySelectObject> idPlaceCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPlaceCategorys(2));
                ViewData["IDPlaceCategorys"] = JsonHelper.Encode(idPlaceCategorySelectObjects);
            }
            return View();
        }

        /// <summary>
        /// 获取盲点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetBlindSpotFeedBackPoint()
        {
            if (Request["BlindSpotFeedBackId"] == null)
            {
                throw new ArgumentNullException("BlindSpotFeedBackId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }

            decimal lng = Request["Lng"].Trim() == "" ? 0 : decimal.Parse(Request["Lng"]);
            decimal lat = Request["Lat"].Trim() == "" ? 0 : decimal.Parse(Request["Lat"]);

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByBlindSpotFeedBack(Guid.Parse(Request["BlindSpotFeedBackId"]),
                    Guid.Parse(Request["AreaId"]), Request["PlaceName"].Trim(), lng, lat, int.Parse(Request["Profession"]), this.CompanyId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取建设申请点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningApplyPoint()
        {
            if (Request["PlanningApplyId"] == null)
            {
                throw new ArgumentNullException("PlanningApplyId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }

            decimal lng = Request["Lng"].Trim() == "" ? 0 : decimal.Parse(Request["Lng"]);
            decimal lat = Request["Lat"].Trim() == "" ? 0 : decimal.Parse(Request["Lat"]);

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByPlanningApply(Guid.Parse(Request["PlanningApplyId"]),
                    Guid.Parse(Request["AreaId"]), Request["PlanningName"].Trim(), lng, lat, int.Parse(Request["Profession"]), this.CompanyId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取运营商规划点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetOperatorsPlanningPoint()
        {
            if (Request["OperatorsPlanningId"] == null)
            {
                throw new ArgumentNullException("OperatorsPlanningId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }

            decimal lng = Request["Lng"].Trim() == "" ? 0 : decimal.Parse(Request["Lng"]);
            decimal lat = Request["Lat"].Trim() == "" ? 0 : decimal.Parse(Request["Lat"]);

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByOperatorsPlanning(Guid.Parse(Request["OperatorsPlanningId"]),
                    Guid.Parse(Request["AreaId"]), Request["PlanningName"].Trim(), lng, lat, int.Parse(Request["Profession"]), this.CompanyId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取规划点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningPoint()
        {
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }

            decimal lng = Request["Lng"].Trim() == "" ? 0 : decimal.Parse(Request["Lng"]);
            decimal lat = Request["Lat"].Trim() == "" ? 0 : decimal.Parse(Request["Lat"]);

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByPlanning(Guid.Parse(Request["PlanningId"]),
                    Guid.Parse(Request["AreaId"]), Request["PlanningName"].Trim(), lng, lat, int.Parse(Request["Profession"]), 2, this.CompanyId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取规划点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningPointFromPlanning()
        {
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }

            decimal lng = Request["Lng"].Trim() == "" ? 0 : decimal.Parse(Request["Lng"]);
            decimal lat = Request["Lat"].Trim() == "" ? 0 : decimal.Parse(Request["Lat"]);

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByPlanning(Guid.Parse(Request["PlanningId"]),
                    Guid.Parse(Request["AreaId"]), Request["PlanningName"].Trim(), lng, lat, int.Parse(Request["Profession"]), 1, this.CompanyId)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取规划点数据，包括选中的运营商规划
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningAndOperatorsPlanningsPoint()
        {
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["PlanningName"] == null)
            {
                throw new ArgumentNullException("PlanningName");
            }
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }
            if (Request["OperatorsPlanningIdList"] == null)
            {
                throw new ArgumentNullException("OperatorsPlanningIdList");
            }

            decimal lng = Request["Lng"].Trim() == "" ? 0 : decimal.Parse(Request["Lng"]);
            decimal lat = Request["Lat"].Trim() == "" ? 0 : decimal.Parse(Request["Lat"]);
            string operatorsPlanningIdsSql = "";
            if (Request["OperatorsPlanningIdList"].Trim() != "")
            {
                string[] operatorsPlanningIdList = Request["OperatorsPlanningIdList"].Trim().Split(',');
                for (int i = 0; i < operatorsPlanningIdList.Length; i++)
                {
                    operatorsPlanningIdsSql += "select '" + operatorsPlanningIdList[i] + "'";
                    if (i != operatorsPlanningIdList.Length - 1)
                    {
                        operatorsPlanningIdsSql += " union ";
                    }
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByPlanningAndOperatorsPlannings(Guid.Parse(Request["PlanningId"]),
                    Guid.Parse(Request["AreaId"]), Request["PlanningName"].Trim(), lng, lat, int.Parse(Request["Profession"]), operatorsPlanningIdsSql)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取规划以及关联的运营商规划列表点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningAndAssociatedOperatorsPlanningsPoint()
        {
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByPlanningAndAssociatedOperatorsPlannings(Guid.Parse(Request["PlanningId"]))), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取购置点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPurchasePoint()
        {
            if (Request["PurchaseId"] == null)
            {
                throw new ArgumentNullException("PurchaseId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }

            decimal lng = Request["Lng"].Trim() == "" ? 0 : decimal.Parse(Request["Lng"]);
            decimal lat = Request["Lat"].Trim() == "" ? 0 : decimal.Parse(Request["Lat"]);

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByPurchase(Guid.Parse(Request["PurchaseId"]),
                    Guid.Parse(Request["AreaId"]), Request["PlaceName"].Trim(), lng, lat, int.Parse(Request["Profession"]))),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取站点点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlacePoint()
        {
            if (Request["PlaceId"] == null)
            {
                throw new ArgumentNullException("PlaceId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }

            decimal lng = Request["Lng"].Trim() == "" ? 0 : decimal.Parse(Request["Lng"]);
            decimal lat = Request["Lat"].Trim() == "" ? 0 : decimal.Parse(Request["Lat"]);

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByPlace(Guid.Parse(Request["PlaceId"]),
                    Guid.Parse(Request["AreaId"]), Request["PlaceName"].Trim(), lng, lat, int.Parse(Request["Profession"]))),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取站点点数据，包括选中的运营商规划
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlacePointAndOperatorsPlannings()
        {
            if (Request["PlaceId"] == null)
            {
                throw new ArgumentNullException("PlaceId");
            }
            if (Request["AreaId"] == null)
            {
                throw new ArgumentNullException("AreaId");
            }
            if (Request["PlaceName"] == null)
            {
                throw new ArgumentNullException("PlaceName");
            }
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["Profession"] == null)
            {
                throw new ArgumentNullException("Profession");
            }
            if (Request["OperatorsPlanningIdList"] == null)
            {
                throw new ArgumentNullException("OperatorsPlanningIdList");
            }

            decimal lng = Request["Lng"].Trim() == "" ? 0 : decimal.Parse(Request["Lng"]);
            decimal lat = Request["Lat"].Trim() == "" ? 0 : decimal.Parse(Request["Lat"]);
            string operatorsPlanningIdsSql = "";
            if (Request["OperatorsPlanningIdList"].Trim() != "")
            {
                string[] operatorsPlanningIdList = Request["OperatorsPlanningIdList"].Trim().Split(',');
                for (int i = 0; i < operatorsPlanningIdList.Length; i++)
                {
                    operatorsPlanningIdsSql += "select '" + operatorsPlanningIdList[i] + "'";
                    if (i != operatorsPlanningIdList.Length - 1)
                    {
                        operatorsPlanningIdsSql += " union ";
                    }
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByPlaceAndOperatorsPlannings(Guid.Parse(Request["PlaceId"]),
                    Guid.Parse(Request["AreaId"]), Request["PlaceName"].Trim(), lng, lat, int.Parse(Request["Profession"]), operatorsPlanningIdsSql)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 基站清单中获取站点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlacePoints()
        {
            if (Request["PlaceIdList"] == null)
            {
                throw new ArgumentNullException("PlaceIdList");
            }

            string PlaceIdsSql = "";
            if (Request["PlaceIdList"].Trim() != "")
            {
                string[] placeIdList = Request["PlaceIdList"].Trim().Split(',');
                for (int i = 0; i < placeIdList.Length; i++)
                {
                    PlaceIdsSql += "select '" + placeIdList[i] + "'";
                    if (i != placeIdList.Length - 1)
                    {
                        PlaceIdsSql += " union ";
                    }
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointsBySearch(PlaceIdsSql)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取规划站点及已有站点数据(移动端)
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningAndPlacePoints()
        {
            if (Request["PlanningIdList"] == null)
            {
                throw new ArgumentNullException("PlanningIdList");
            }
            if (Request["PlaceIdList"] == null)
            {
                throw new ArgumentNullException("PlaceIdList");
            }

            string PlanningIdsSql = "";
            if (Request["PlanningIdList"].Trim() != "")
            {
                string[] planningIdList = Request["PlanningIdList"].Trim().Split(',');
                for (int i = 0; i < planningIdList.Length; i++)
                {
                    PlanningIdsSql += "select '" + planningIdList[i] + "'";
                    if (i != planningIdList.Length - 1)
                    {
                        PlanningIdsSql += " union ";
                    }
                }
            }

            string PlaceIdsSql = "";
            if (Request["PlaceIdList"].Trim() != "")
            {
                string[] placeIdList = Request["PlaceIdList"].Trim().Split(',');
                for (int i = 0; i < placeIdList.Length; i++)
                {
                    PlaceIdsSql += "select '" + placeIdList[i] + "'";
                    if (i != placeIdList.Length - 1)
                    {
                        PlaceIdsSql += " union ";
                    }
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlanningAndPlacePoints(PlanningIdsSql, PlaceIdsSql)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 运营商规划清单获取站点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetOperatorsPlanningPoints()
        {
            if (Request["OperatorsIdList"] == null)
            {
                throw new ArgumentNullException("OperatorsIdList");
            }

            string OperatorsIdsSql = "";
            if (Request["OperatorsIdList"].Trim() != "")
            {
                string[] operatorsIdList = Request["OperatorsIdList"].Trim().Split(',');
                for (int i = 0; i < operatorsIdList.Length; i++)
                {
                    OperatorsIdsSql += "select '" + operatorsIdList[i] + "'";
                    if (i != operatorsIdList.Length - 1)
                    {
                        OperatorsIdsSql += " union ";
                    }
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetOperatorsPlanningPointsBySearch(OperatorsIdsSql)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 基站清单中获取站点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningPoints()
        {
            if (Request["PlanningIdList"] == null)
            {
                throw new ArgumentNullException("PlanningIdList");
            }

            string PlanningIdsSql = "";
            if (Request["PlanningIdList"].Trim() != "")
            {
                string[] planningIdList = Request["PlanningIdList"].Trim().Split(',');
                for (int i = 0; i < planningIdList.Length; i++)
                {
                    PlanningIdsSql += "select '" + planningIdList[i] + "'";
                    if (i != planningIdList.Length - 1)
                    {
                        PlanningIdsSql += " union ";
                    }
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlanningPointsBySearch(PlanningIdsSql)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 基站建设申请中获取站点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlanningApplyPoints()
        {
            if (Request["PlanningApplyIdList"] == null)
            {
                throw new ArgumentNullException("PlanningApplyIdList");
            }

            string PlanningApplyIdsSql = "";
            if (Request["PlanningApplyIdList"].Trim() != "")
            {
                string[] planningApplyIdList = Request["PlanningApplyIdList"].Trim().Split(',');
                for (int i = 0; i < planningApplyIdList.Length; i++)
                {
                    PlanningApplyIdsSql += "select '" + planningApplyIdList[i] + "'";
                    if (i != planningApplyIdList.Length - 1)
                    {
                        PlanningApplyIdsSql += " union ";
                    }
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlanningApplyPointsBySearch(PlanningApplyIdsSql)),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取附近指定距离内的规划和站点列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNearbyPlanningsAndPlaces()
        {
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }
            if (Request["PlaceId"] == null)
            {
                throw new ArgumentNullException("PlaceId");
            }
            if (Request["BSPlanningPlaceCategoryList"] == null)
            {
                throw new ArgumentNullException("BSPlanningPlaceCategoryList");
            }
            if (Request["BSPlaceCategoryList"] == null)
            {
                throw new ArgumentNullException("BSPlaceCategoryList");
            }

            string bsPlanningPlaceCategorySql = "";
            if (Request["BSPlanningPlaceCategoryList"].Trim() != "")
            {
                string[] bsPlanningPlaceCategoryList = Request["BSPlanningPlaceCategoryList"].Trim().Split(',');
                for (int i = 0; i < bsPlanningPlaceCategoryList.Length; i++)
                {
                    bsPlanningPlaceCategorySql += "select '" + bsPlanningPlaceCategoryList[i] + "'";
                    if (i != bsPlanningPlaceCategoryList.Length - 1)
                    {
                        bsPlanningPlaceCategorySql += " union ";
                    }
                }
            }

            string bsPlaceCategorySql = "";
            if (Request["BSPlaceCategoryList"].Trim() != "")
            {
                string[] bsPlaceCategoryList = Request["BSPlaceCategoryList"].Trim().Split(',');
                for (int i = 0; i < bsPlaceCategoryList.Length; i++)
                {
                    bsPlaceCategorySql += "select '" + bsPlaceCategoryList[i] + "'";
                    if (i != bsPlaceCategoryList.Length - 1)
                    {
                        bsPlaceCategorySql += " union ";
                    }
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetNearbyPlanningsAndPlaces(decimal.Parse(Request["Lng"]), decimal.Parse(Request["Lat"]),
                    Guid.Parse(Request["PlanningId"]), Guid.Parse(Request["PlaceId"]), bsPlanningPlaceCategorySql, bsPlaceCategorySql, 2));
            }
        }

        /// <summary>
        /// 获取附近指定距离内的规划和站点列表(室分)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNearbyPlanningsAndPlacesID()
        {
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }
            if (Request["PlaceId"] == null)
            {
                throw new ArgumentNullException("PlaceId");
            }
            if (Request["IDPlanningPlaceCategoryList"] == null)
            {
                throw new ArgumentNullException("IDPlanningPlaceCategoryList");
            }
            if (Request["IDPlaceCategoryList"] == null)
            {
                throw new ArgumentNullException("IDPlaceCategoryList");
            }

            string idPlanningPlaceCategorySql = "";
            if (Request["IDPlanningPlaceCategoryList"].Trim() != "")
            {
                string[] idPlanningPlaceCategoryList = Request["IDPlanningPlaceCategoryList"].Trim().Split(',');
                for (int i = 0; i < idPlanningPlaceCategoryList.Length; i++)
                {
                    idPlanningPlaceCategorySql += "select '" + idPlanningPlaceCategoryList[i] + "'";
                    if (i != idPlanningPlaceCategoryList.Length - 1)
                    {
                        idPlanningPlaceCategorySql += " union ";
                    }
                }
            }

            string idPlaceCategorySql = "";
            if (Request["IDPlaceCategoryList"].Trim() != "")
            {
                string[] idPlaceCategoryList = Request["IDPlaceCategoryList"].Trim().Split(',');
                for (int i = 0; i < idPlaceCategoryList.Length; i++)
                {
                    idPlaceCategorySql += "select '" + idPlaceCategoryList[i] + "'";
                    if (i != idPlaceCategoryList.Length - 1)
                    {
                        idPlaceCategorySql += " union ";
                    }
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetNearbyPlanningsAndPlacesID(decimal.Parse(Request["Lng"]), decimal.Parse(Request["Lat"]),
                    Guid.Parse(Request["PlanningId"]), Guid.Parse(Request["PlaceId"]), idPlanningPlaceCategorySql, idPlaceCategorySql, 2));
            }
        }

        /// <summary>
        /// 获取附近指定距离内的规划和站点列表(移动端)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNearbyPlanningsAndPlacesMobile()
        {
            string professionList = "0";
            if (Request["ProfessionList"] != null && Request["ProfessionList"] != "")
            {
                professionList = Request["ProfessionList"].Trim();
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
            string[] profession = professionList.Trim().Split(',');
            for (int i = 0; i < profession.Length; i++)
            {
                professionListSql += "select '" + profession[i] + "'";
                if (i != profession.Length - 1)
                {
                    professionListSql += " union ";
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetNearbyPlanningsAndPlacesMobile(decimal.Parse(Request["Lng"]), decimal.Parse(Request["Lat"]), decimal.Parse(Request["Distance"])));
            }
        }

        /// <summary>
        /// 获取周边资源列表(移动端)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNearbyPlanningsAndPlacesListMobile()
        {
            string professionList = "0";
            if (Request["ProfessionList"] != null && Request["ProfessionList"] != "")
            {
                professionList = Request["ProfessionList"].Trim();
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
            string[] profession = professionList.Trim().Split(',');
            for (int i = 0; i < profession.Length; i++)
            {
                professionListSql += "select '" + profession[i] + "'";
                if (i != profession.Length - 1)
                {
                    professionListSql += " union ";
                }
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetNearbyPlanningsAndPlacesListMobile(professionListSql, decimal.Parse(Request["Lng"]), decimal.Parse(Request["Lat"]), decimal.Parse(Request["Distance"]), this.CompanyId));
            }
        }

        /// <summary>
        /// 获取附近指定距离内的规划和站点列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNearbyPlanningsAndPlacesAll()
        {
            if (Request["Lng"] == null)
            {
                throw new ArgumentNullException("Lng");
            }
            if (Request["Lat"] == null)
            {
                throw new ArgumentNullException("Lat");
            }
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }
            if (Request["PlaceId"] == null)
            {
                throw new ArgumentNullException("PlaceId");
            }
            if (Request["BSPlanningProfession"] == null)
            {
                throw new ArgumentNullException("BSPlanningProfession");
            }
            if (Request["IDPlanningProfession"] == null)
            {
                throw new ArgumentNullException("IDPlanningProfession");
            }
            if (Request["BSPlaceProfession"] == null)
            {
                throw new ArgumentNullException("BSPlaceProfession");
            }
            if (Request["IDPlaceProfession"] == null)
            {
                throw new ArgumentNullException("IDPlaceProfession");
            }

            string planningProfessionsSql = "select " + Request["BSPlanningProfession"] + " union select " + Request["IDPlanningProfession"] + "";
            string placeProfessionsSql = "select " + Request["BSPlaceProfession"] + " union select " + Request["IDPlaceProfession"] + "";

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetNearbyPlanningsAndPlacesAll(decimal.Parse(Request["Lng"]), decimal.Parse(Request["Lat"]),
                    Guid.Parse(Request["PlanningId"]), Guid.Parse(Request["PlaceId"]), planningProfessionsSql, placeProfessionsSql, 2));
            }
        }

        /// <summary>
        /// 获取规划以及关联的运营商规划列表点数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPlaceAndAssociatedOperatorsPlanningsPoint()
        {
            if (Request["OperatorsPlanningDemandId"] == null)
            {
                throw new ArgumentNullException("OperatorsPlanningDemandId");
            }

            using (ServiceProxy<IMapService> proxy = new ServiceProxy<IMapService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPointByOperatorsPlanningDemandAndAssociatedOperatorsPlannings(Guid.Parse(Request["OperatorsPlanningDemandId"]))), JsonRequestBehavior.AllowGet);
            }
        }
    }
}