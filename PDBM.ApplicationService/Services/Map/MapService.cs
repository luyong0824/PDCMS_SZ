using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.Map;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.Map;
using PDBM.Domain.Models.Enum;

namespace PDBM.ApplicationService.Services.Map
{
    /// <summary>
    /// 地图应用层服务
    /// </summary>
    public class MapService : DataService, IMapService
    {
        private readonly IRepository<PlanningApply> planningApplyRepository;
        private readonly IRepository<OperatorsPlanning> operatorsPlanningRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<Purchase> purchaseRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<Area> areaRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<PlaceCategory> placeCategoryRepository;
        private readonly IRepository<Company> companyRepository;
        private readonly IRepository<OperatorsPlanningDemand> operatorsPlanningDemandRepository;
        private readonly IRepository<PlaceOwner> placeOwnerRepository;
        private readonly IRepository<BlindSpotFeedBack> blindSpotFeedBackRepository;

        public MapService(IRepositoryContext context,
            IRepository<PlanningApply> planningApplyRepository,
            IRepository<OperatorsPlanning> operatorsPlanningRepository,
            IRepository<Planning> planningRepository,
            IRepository<Purchase> purchaseRepository,
            IRepository<Place> placeRepository,
            IRepository<Area> areaRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<PlaceCategory> placeCategoryRepository,
            IRepository<Company> companyRepository,
            IRepository<OperatorsPlanningDemand> operatorsPlanningDemandRepository,
            IRepository<PlaceOwner> placeOwnerRepository,
            IRepository<BlindSpotFeedBack> blindSpotFeedBackRepository)
            : base(context)
        {
            this.planningApplyRepository = planningApplyRepository;
            this.operatorsPlanningRepository = operatorsPlanningRepository;
            this.planningRepository = planningRepository;
            this.purchaseRepository = purchaseRepository;
            this.placeRepository = placeRepository;
            this.areaRepository = areaRepository;
            this.reseauRepository = reseauRepository;
            this.placeCategoryRepository = placeCategoryRepository;
            this.companyRepository = companyRepository;
            this.operatorsPlanningDemandRepository = operatorsPlanningDemandRepository;
            this.placeOwnerRepository = placeOwnerRepository;
            this.blindSpotFeedBackRepository = blindSpotFeedBackRepository;
        }

        /// <summary>
        /// 根据盲点反馈获取点对象
        /// </summary>
        /// <param name="blindSpotFeedBackId">盲点反馈Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="placeName">盲点地名</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>点对象</returns>
        public PointObject GetPointByBlindSpotFeedBack(Guid blindSpotFeedBackId, Guid areaId, string placeName, decimal lng, decimal lat, int profession, Guid companyId)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 1;
            if (blindSpotFeedBackId == Guid.Empty)
            {
                pointObject.Profession = profession;
                pointObject.PlanningName = placeName == "" ? "盲点地名" : placeName;
                pointObject.AddressingStateName = "";
                pointObject.PlaceCategoryName = "";
                pointObject.AreaName = "";
                pointObject.ReseauName = "";
                pointObject.Lng = lng;
                pointObject.Lat = lat;
                pointObject.CompanyId = companyId;
                pointObject.CompanyName = "";
                if (areaId != Guid.Empty)
                {
                    Area area = areaRepository.FindByKey(areaId);
                    if (area != null)
                    {
                        pointObject.AreaName = area.AreaName;
                        if (lng == 0)
                        {
                            pointObject.Lng = area.Lng;
                        }
                        if (lat == 0)
                        {
                            pointObject.Lat = area.Lat;
                        }
                    }
                }
            }
            else
            {
                BlindSpotFeedBack blindSpotFeedBack = blindSpotFeedBackRepository.FindByKey(blindSpotFeedBackId);
                Company company = companyRepository.FindByKey(companyId);
                if (blindSpotFeedBack != null)
                {
                    Area area = areaRepository.GetByKey(blindSpotFeedBack.AreaId);

                    pointObject.Id = blindSpotFeedBack.Id;
                    pointObject.Profession = (int)Profession.基站;
                    pointObject.PlanningName = blindSpotFeedBack.PlaceName;
                    pointObject.AddressingStateName = "";
                    pointObject.PlaceCategoryName = "";
                    pointObject.AreaName = area.AreaName;
                    pointObject.ReseauName = "";
                    pointObject.Lng = blindSpotFeedBack.Lng;
                    pointObject.Lat = blindSpotFeedBack.Lat;
                    pointObject.CompanyId = company.Id;
                    pointObject.CompanyName = company.CompanyName;
                }
                else
                {
                    throw new ApplicationFault("选择的盲点反馈在系统中不存在");
                }
            }
            return pointObject;
        }

        /// <summary>
        /// 根据建设申请获取点对象
        /// </summary>
        /// <param name="planningApplyId">建设申请Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>点对象</returns>
        public PointObject GetPointByPlanningApply(Guid planningApplyId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, Guid companyId)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 1;
            if (planningApplyId == Guid.Empty)
            {
                pointObject.Profession = profession;
                pointObject.PlanningName = planningName == "" ? "规划点" : planningName;
                pointObject.AddressingStateName = "";
                pointObject.PlaceCategoryName = "";
                pointObject.AreaName = "";
                pointObject.ReseauName = "";
                pointObject.Lng = lng;
                pointObject.Lat = lat;
                pointObject.CompanyId = companyId;
                pointObject.CompanyName = "";
                if (areaId != Guid.Empty)
                {
                    Area area = areaRepository.FindByKey(areaId);
                    if (area != null)
                    {
                        pointObject.AreaName = area.AreaName;
                        if (lng == 0)
                        {
                            pointObject.Lng = area.Lng;
                        }
                        if (lat == 0)
                        {
                            pointObject.Lat = area.Lat;
                        }
                    }
                }
            }
            else
            {
                PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyId);
                Company company = companyRepository.FindByKey(companyId);
                if (planningApply != null)
                {
                    Reseau reseau = reseauRepository.GetByKey(planningApply.ReseauId);
                    Area area = areaRepository.GetByKey(reseau.AreaId);

                    pointObject.Id = planningApply.Id;
                    pointObject.Profession = (int)planningApply.Profession;
                    pointObject.PlanningName = planningApply.PlanningName;
                    pointObject.AddressingStateName = "";
                    pointObject.PlaceCategoryName = "";
                    pointObject.AreaName = area.AreaName;
                    pointObject.ReseauName = reseau.ReseauName;
                    pointObject.Lng = planningApply.Lng;
                    pointObject.Lat = planningApply.Lat;
                    pointObject.CompanyId = company.Id;
                    pointObject.CompanyName = company.CompanyName;
                }
                else
                {
                    throw new ApplicationFault("选择的建设申请在系统中不存在");
                }
            }
            return pointObject;
        }

        /// <summary>
        /// 根据运营商规划获取点对象
        /// </summary>
        /// <param name="operatorsPlanningId">运营商规划Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>点对象</returns>
        public PointObject GetPointByOperatorsPlanning(Guid operatorsPlanningId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, Guid companyId)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 1;
            if (operatorsPlanningId == Guid.Empty)
            {
                pointObject.Profession = profession;
                pointObject.PlanningName = planningName == "" ? "规划点" : planningName;
                pointObject.PlaceCategoryName = "";
                pointObject.AreaName = "";
                pointObject.ReseauName = "";
                pointObject.Lng = lng;
                pointObject.Lat = lat;
                pointObject.CompanyId = companyId;
                pointObject.CompanyName = "";
                if (areaId != Guid.Empty)
                {
                    Area area = areaRepository.FindByKey(areaId);
                    if (area != null)
                    {
                        pointObject.AreaName = area.AreaName;
                        if (lng == 0)
                        {
                            pointObject.Lng = area.Lng;
                        }
                        if (lat == 0)
                        {
                            pointObject.Lat = area.Lat;
                        }
                    }
                }
            }
            else
            {
                OperatorsPlanning operatorsPlanning = operatorsPlanningRepository.FindByKey(operatorsPlanningId);
                if (operatorsPlanning != null)
                {
                    PlaceCategory placeCategory = placeCategoryRepository.GetByKey(operatorsPlanning.PlaceCategoryId);
                    Area area = areaRepository.GetByKey(operatorsPlanning.AreaId);
                    Company company = companyRepository.GetByKey(operatorsPlanning.CompanyId);

                    pointObject.Id = operatorsPlanningId;
                    pointObject.Profession = (int)operatorsPlanning.Profession;
                    pointObject.PlanningName = operatorsPlanning.PlanningName;
                    pointObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                    pointObject.AreaName = area.AreaName;
                    pointObject.ReseauName = "";
                    pointObject.Lng = operatorsPlanning.Lng;
                    pointObject.Lat = operatorsPlanning.Lat;
                    pointObject.CompanyId = operatorsPlanning.CompanyId;
                    pointObject.CompanyName = company.CompanyName;
                }
                else
                {
                    throw new ApplicationFault("选择的运营商规划在系统中不存在");
                }
            }
            return pointObject;
        }

        /// <summary>
        /// 根据规划获取点对象
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="isFromPlanning">是否是从规划页面查看</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>点对象</returns>
        public PointObject GetPointByPlanning(Guid planningId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, int isFromPlanning, Guid companyId)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 1;
            Company company = companyRepository.GetByKey(companyId);
            pointObject.CompanyId = companyId;
            pointObject.CompanyName = company.CompanyName;
            if (planningId == Guid.Empty)
            {
                pointObject.Profession = profession;
                pointObject.PlanningName = planningName == "" ? "规划点" : planningName;
                pointObject.PlaceCategoryName = "";
                pointObject.AreaName = "";
                pointObject.ReseauName = "";
                pointObject.Lng = lng;
                pointObject.Lat = lat;
                pointObject.Issued = 2;
                pointObject.IsFromPlanning = isFromPlanning;
                pointObject.AddressingStateName = "";
                if (areaId != Guid.Empty)
                {
                    Area area = areaRepository.FindByKey(areaId);
                    if (area != null)
                    {
                        pointObject.AreaName = area.AreaName;
                        if (lng == 0)
                        {
                            pointObject.Lng = area.Lng;
                        }
                        if (lat == 0)
                        {
                            pointObject.Lat = area.Lat;
                        }
                    }
                }
            }
            else
            {
                Planning planning = planningRepository.FindByKey(planningId);
                if (planning != null)
                {
                    if (planning.PlaceCategoryId != Guid.Empty)
                    {
                        PlaceCategory placeCategory = placeCategoryRepository.GetByKey(planning.PlaceCategoryId);
                        pointObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                    }
                    else
                    {
                        pointObject.PlaceCategoryName = "";
                    }
                    Reseau reseau = reseauRepository.GetByKey(planning.ReseauId);
                    Area area = areaRepository.GetByKey(reseau.AreaId);

                    pointObject.Id = planningId;
                    pointObject.Profession = (int)planning.Profession;
                    pointObject.PlanningName = planning.PlanningName;
                    pointObject.AreaName = area.AreaName;
                    pointObject.ReseauName = reseau.ReseauName;
                    pointObject.Lng = planning.Lng;
                    pointObject.Lat = planning.Lat;
                    pointObject.Issued = (int)planning.Issued;
                    pointObject.IsFromPlanning = isFromPlanning;
                    pointObject.AddressingStateName = EnumHelper.GetEnumText(typeof(AddressingState), planning.AddressingState);
                }
                else
                {
                    throw new ApplicationFault("选择的规划在系统中不存在");
                }
            }
            return pointObject;
        }

        /// <summary>
        /// 根据规划获取点对象，包括选中的运营商规划列表
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="operatorsPlanningIdsSql">运营商规划Id列表Sql语句</param>
        /// <returns>点对象</returns>
        public PointObject GetPointByPlanningAndOperatorsPlannings(Guid planningId, Guid areaId, string planningName, decimal lng, decimal lat, int profession, string operatorsPlanningIdsSql)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 1;
            Company company = companyRepository.GetByKey(Guid.Parse("9D4A4487-2AD6-4C19-8633-00742E8F1D28"));
            pointObject.CompanyId = Guid.Parse("9D4A4487-2AD6-4C19-8633-00742E8F1D28");
            pointObject.CompanyName = company.CompanyName;
            if (planningId == Guid.Empty)
            {
                pointObject.Profession = profession;
                pointObject.PlanningName = planningName == "" ? "规划点" : planningName;
                pointObject.PlaceCategoryName = "";
                pointObject.AreaName = "";
                pointObject.ReseauName = "";
                pointObject.Lng = lng;
                pointObject.Lat = lat;
                if (areaId != Guid.Empty)
                {
                    Area area = areaRepository.FindByKey(areaId);
                    if (area != null)
                    {
                        pointObject.AreaName = area.AreaName;
                        if (lng == 0)
                        {
                            pointObject.Lng = area.Lng;
                        }
                        if (lat == 0)
                        {
                            pointObject.Lat = area.Lat;
                        }
                    }
                }
            }
            else
            {
                Planning planning = planningRepository.FindByKey(planningId);
                if (planning != null)
                {
                    PlaceCategory placeCategory = placeCategoryRepository.GetByKey(planning.PlaceCategoryId);
                    Reseau reseau = reseauRepository.GetByKey(planning.ReseauId);
                    Area area = areaRepository.GetByKey(reseau.AreaId);

                    pointObject.Id = planningId;
                    pointObject.Profession = (int)planning.Profession;
                    pointObject.PlanningName = planning.PlanningName;
                    pointObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                    pointObject.AreaName = area.AreaName;
                    pointObject.ReseauName = reseau.ReseauName;
                    pointObject.Lng = planning.Lng;
                    pointObject.Lat = planning.Lat;
                }
                else
                {
                    throw new ApplicationFault("选择的规划在系统中不存在");
                }
            }

            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "OperatorsPlanningIdsSql", Type = SqlDbType.VarChar, Value = operatorsPlanningIdsSql });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetOperatorsPlanningsByIds", parameters))
            {
                pointObject.OperatorsPlanningsAndPlaces = JsonHelper.Encode(dt);
            }
            return pointObject;
        }

        /// <summary>
        /// 基站清单中获取数据
        /// </summary>
        /// <param name="placeIdsSql"></param>
        /// <returns></returns>
        public PointObject GetPointsBySearch(string placeIdsSql)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 2;

            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "placeIdsSql", Type = SqlDbType.VarChar, Value = placeIdsSql });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetPlaceByIds", parameters))
            {
                string netWorks = "";
                foreach (DataRow dr in dt.Rows)
                {
                    netWorks = "";
                    if (dr["G2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2G/";
                    }
                    if (dr["D2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2D/";
                    }
                    if (dr["G3Number"].ToString() != "")
                    {
                        netWorks = netWorks + "3G/";
                    }
                    if (dr["G4Number"].ToString() != "")
                    {
                        netWorks = netWorks + "4G/";
                    }
                    if (netWorks.Length > 0)
                    {
                        netWorks = netWorks.Substring(0, netWorks.Length - 1);
                    }
                    dr["NetWorks"] = netWorks;
                }
                pointObject.Places = JsonHelper.Encode(dt);
            }

            return pointObject;
        }

        /// <summary>
        /// 获取规划站点及已有站点数据(移动端)
        /// </summary>
        /// <param name="planningIdsSql">规划站点Id列表Sql语句</param>
        /// <param name="placeIdsSql">已有站点Id列表Sql语句</param>
        /// <returns></returns>
        public PointObject GetPlanningAndPlacePoints(string planningIdsSql, string placeIdsSql)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 2;

            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "planningIdsSql", Type = SqlDbType.VarChar, Value = planningIdsSql });
            parameters.Add(new Parameter() { Name = "placeIdsSql", Type = SqlDbType.VarChar, Value = placeIdsSql });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetPlanningAndPlaceByIds", parameters))
            {
                string netWorks = "";
                foreach (DataRow dr in dt.Rows)
                {
                    netWorks = "";
                    if (dr["G2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2G/";
                    }
                    if (dr["D2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2D/";
                    }
                    if (dr["G3Number"].ToString() != "")
                    {
                        netWorks = netWorks + "3G/";
                    }
                    if (dr["G4Number"].ToString() != "")
                    {
                        netWorks = netWorks + "4G/";
                    }
                    if (netWorks.Length > 0)
                    {
                        netWorks = netWorks.Substring(0, netWorks.Length - 1);
                    }
                    dr["NetWorks"] = netWorks;
                }
                pointObject.Places = JsonHelper.Encode(dt);
            }

            return pointObject;
        }

        /// <summary>
        /// 基站规划清单中获取数据
        /// </summary>
        /// <param name="planningIdsSql"></param>
        /// <returns></returns>
        public PointObject GetPlanningPointsBySearch(string planningIdsSql)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 1;

            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "planningIdsSql", Type = SqlDbType.VarChar, Value = planningIdsSql });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetPlanningByIds", parameters))
            {
                pointObject.Plannings = JsonHelper.Encode(dt);
            }

            return pointObject;
        }

        /// <summary>
        /// 基站建设申请中获取数据
        /// </summary>
        /// <param name="planningApplyIdsSql"></param>
        /// <returns></returns>
        public PointObject GetPlanningApplyPointsBySearch(string planningApplyIdsSql)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 1;

            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "planningApplyIdsSql", Type = SqlDbType.VarChar, Value = planningApplyIdsSql });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetPlanningApplyByIds", parameters))
            {
                pointObject.Plannings = JsonHelper.Encode(dt);
            }

            return pointObject;
        }

        /// <summary>
        /// 运营商清单中获取数据
        /// </summary>
        /// <param name="operatorsIdsSql"></param>
        /// <returns></returns>
        public PointObject GetOperatorsPlanningPointsBySearch(string operatorsIdsSql)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 1;

            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "operatorsIdsSql", Type = SqlDbType.VarChar, Value = operatorsIdsSql });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetOperatorsPlanningByIds", parameters))
            {
                pointObject.Operators = JsonHelper.Encode(dt);
            }

            return pointObject;
        }

        /// <summary>
        /// 根据规划和关联的运营商规划获取点对象
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <returns>点对象</returns>
        public PointObject GetPointByPlanningAndAssociatedOperatorsPlannings(Guid planningId)
        {
            Planning planning = planningRepository.FindByKey(planningId);
            if (planning != null)
            {
                PlaceCategory placeCategory = placeCategoryRepository.GetByKey(planning.PlaceCategoryId);
                Reseau reseau = reseauRepository.GetByKey(planning.ReseauId);
                Area area = areaRepository.GetByKey(reseau.AreaId);

                PointObject pointObject = new PointObject();
                pointObject.DataType = 1;
                Company company = companyRepository.GetByKey(Guid.Parse("9D4A4487-2AD6-4C19-8633-00742E8F1D28"));
                pointObject.CompanyId = Guid.Parse("9D4A4487-2AD6-4C19-8633-00742E8F1D28");
                pointObject.CompanyName = company.CompanyName;
                pointObject.Id = planningId;
                pointObject.Profession = (int)planning.Profession;
                pointObject.PlanningName = planning.PlanningName;
                pointObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                pointObject.AreaName = area.AreaName;
                pointObject.ReseauName = reseau.ReseauName;
                pointObject.Lng = planning.Lng;
                pointObject.Lat = planning.Lat;

                List<Parameter> parameters = new List<Parameter>(1);
                parameters.Add(new Parameter() { Name = "PlanningId", Type = SqlDbType.UniqueIdentifier, Value = planningId });
                using (var dt = SqlHelper.ExecuteDataTable("prc_GetOperatorsPlanningsByPlanning", parameters))
                {
                    pointObject.OperatorsPlannings = JsonHelper.Encode(dt);
                }

                return pointObject;
            }
            else
            {
                throw new ApplicationFault("选择的规划在系统中不存在");
            }
        }

        /// <summary>
        /// 根据购置获取点对象
        /// </summary>
        /// <param name="purchaseId">购置Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <returns>点对象</returns>
        public PointObject GetPointByPurchase(Guid purchaseId, Guid areaId, string placeName, decimal lng, decimal lat, int profession)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 2;
            if (purchaseId == Guid.Empty)
            {
                pointObject.Profession = profession;
                pointObject.PlaceName = placeName == "" ? "购置点" : placeName;
                pointObject.PlaceCategoryName = "";
                pointObject.AreaName = "";
                pointObject.ReseauName = "";
                pointObject.Lng = lng;
                pointObject.Lat = lat;
                pointObject.PropertyRight = 0;
                pointObject.TelecomShare = 0;
                pointObject.MobileShare = 0;
                pointObject.UnicomShare = 0;
                pointObject.OwnerName = "";
                pointObject.OwnerContact = "";
                pointObject.OwnerPhoneNumber = "";
                if (areaId != Guid.Empty)
                {
                    Area area = areaRepository.FindByKey(areaId);
                    if (area != null)
                    {
                        pointObject.AreaName = area.AreaName;
                        if (lng == 0)
                        {
                            pointObject.Lng = area.Lng;
                        }
                        if (lat == 0)
                        {
                            pointObject.Lat = area.Lat;
                        }
                    }
                }
            }
            else
            {
                Purchase purchase = purchaseRepository.FindByKey(purchaseId);
                if (purchase != null)
                {
                    PlaceCategory placeCategory = placeCategoryRepository.GetByKey(purchase.PlaceCategoryId);
                    Reseau reseau = reseauRepository.GetByKey(purchase.ReseauId);
                    Area area = areaRepository.GetByKey(reseau.AreaId);

                    pointObject.Id = purchaseId;
                    pointObject.Profession = (int)purchase.Profession;
                    pointObject.PlaceName = purchase.PlaceName;
                    pointObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                    pointObject.AreaName = area.AreaName;
                    pointObject.ReseauName = reseau.ReseauName;
                    pointObject.Lng = purchase.Lng;
                    pointObject.Lat = purchase.Lat;
                    pointObject.PropertyRight = (int)purchase.PropertyRight;
                    pointObject.TelecomShare = (int)purchase.TelecomShare;
                    pointObject.MobileShare = (int)purchase.MobileShare;
                    pointObject.UnicomShare = (int)purchase.UnicomShare;
                    pointObject.OwnerName = purchase.OwnerName;
                    pointObject.OwnerContact = purchase.OwnerContact;
                    pointObject.OwnerPhoneNumber = purchase.OwnerPhoneNumber;
                }
                else
                {
                    throw new ApplicationFault("选择的购置站点在系统中不存在");
                }
            }
            return pointObject;
        }

        /// <summary>
        /// 根据站点获取点对象
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <returns>点对象</returns>
        public PointObject GetPointByPlace(Guid placeId, Guid areaId, string placeName, decimal lng, decimal lat, int profession)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 2;
            if (placeId != Guid.Empty)
            {
                Place place = placeRepository.FindByKey(placeId);
                if (place != null)
                {
                    PlaceCategory placeCategory = placeCategoryRepository.GetByKey(place.PlaceCategoryId);
                    Reseau reseau = reseauRepository.GetByKey(place.ReseauId);
                    Area area = areaRepository.GetByKey(reseau.AreaId);

                    pointObject.Id = placeId;
                    pointObject.Profession = (int)place.Profession;
                    pointObject.PlaceName = place.PlaceName;
                    pointObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                    pointObject.AreaName = area.AreaName;
                    pointObject.ReseauName = reseau.ReseauName;
                    pointObject.Lng = place.Lng;
                    pointObject.Lat = place.Lat;
                    pointObject.OwnerName = place.OwnerName;
                    pointObject.OwnerContact = place.OwnerContact;
                    pointObject.OwnerPhoneNumber = place.OwnerPhoneNumber;
                    pointObject.PlaceMapState = (int)place.PlaceMapState;
                    PlaceOwner placeOwner = placeOwnerRepository.GetByKey(place.PlaceOwner);
                    if (placeOwner != null)
                    {
                        pointObject.PlaceOwnerName = placeOwner.PlaceOwnerName;
                    }
                    else
                    {
                        pointObject.PlaceOwnerName = "";
                    }

                    string netWorks = "";
                    if (place.G2Number != "")
                    {
                        netWorks = netWorks + "2G/";
                    }
                    if (place.D2Number != "")
                    {
                        netWorks = netWorks + "2D/";
                    }
                    if (place.G3Number != "")
                    {
                        netWorks = netWorks + "3G/";
                    }
                    if (place.G4Number != "")
                    {
                        netWorks = netWorks + "4G/";
                    }
                    if (netWorks.Length > 0)
                    {
                        netWorks = netWorks.Substring(0, netWorks.Length - 1);
                    }
                    pointObject.NetWorks = netWorks;

                    return pointObject;
                }
                else
                {
                    throw new ApplicationFault("选择的站点在系统中不存在");
                }
            }
            else
            {
                throw new ApplicationFault("选择的站点在系统中不存在");
            }
        }

        /// <summary>
        /// 根据站点获取点对象，包括选中的运营商规划列表
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="profession">专业</param>
        /// <param name="operatorsPlanningIdsSql">运营商规划Id列表Sql语句</param>
        /// <returns>点对象</returns>
        public PointObject GetPointByPlaceAndOperatorsPlannings(Guid placeId, Guid areaId, string placeName, decimal lng, decimal lat, int profession, string operatorsPlanningIdsSql)
        {
            PointObject pointObject = new PointObject();
            pointObject.DataType = 2;
            if (placeId != Guid.Empty)
            {
                Place place = placeRepository.FindByKey(placeId);
                if (place != null)
                {
                    PlaceCategory placeCategory = placeCategoryRepository.GetByKey(place.PlaceCategoryId);
                    Reseau reseau = reseauRepository.GetByKey(place.ReseauId);
                    Area area = areaRepository.GetByKey(reseau.AreaId);

                    pointObject.Id = placeId;
                    pointObject.Profession = (int)place.Profession;
                    pointObject.PlaceName = place.PlaceName;
                    pointObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                    pointObject.AreaName = area.AreaName;
                    pointObject.ReseauName = reseau.ReseauName;
                    pointObject.Lng = place.Lng;
                    pointObject.Lat = place.Lat;
                    //pointObject.PropertyRight = (int)place.PropertyRight;
                    //pointObject.TelecomShare = (int)place.TelecomShare;
                    //pointObject.MobileShare = (int)place.MobileShare;
                    //pointObject.UnicomShare = (int)place.UnicomShare;
                    pointObject.OwnerName = place.OwnerName;
                    pointObject.OwnerContact = place.OwnerContact;
                    pointObject.OwnerPhoneNumber = place.OwnerPhoneNumber;
                    pointObject.PlaceMapState = (int)place.PlaceMapState;

                    List<Parameter> parameters = new List<Parameter>(1);
                    parameters.Add(new Parameter() { Name = "OperatorsPlanningIdsSql", Type = SqlDbType.VarChar, Value = operatorsPlanningIdsSql });
                    using (var dt = SqlHelper.ExecuteDataTable("prc_GetOperatorsPlanningsByIds", parameters))
                    {
                        pointObject.OperatorsPlanningsAndPlaces = JsonHelper.Encode(dt);
                    }

                    return pointObject;
                }
                else
                {
                    throw new ApplicationFault("选择的站点在系统中不存在");
                }
            }
            else
            {
                throw new ApplicationFault("选择的站点在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取附近指定距离内的规划和站点列表
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="planningId">规划Id</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="bsPlanningPlaceCategorySql">规划基站类型列表Sql语句</param>
        /// <param name="bsPlaceCategorySql">基站类型列表Sql语句</param>
        /// <param name="distance">距离(公里)</param>
        /// <returns>附近指定距离内的规划和站点列表的Json字符串</returns>
        public string GetNearbyPlanningsAndPlaces(decimal lng, decimal lat, Guid planningId, Guid placeId, string bsPlanningPlaceCategorySql, string bsPlaceCategorySql, decimal distance)
        {
            List<Parameter> parameters = new List<Parameter>(7);
            parameters.Add(new Parameter() { Name = "Lng", Type = SqlDbType.Decimal, Value = lng });
            parameters.Add(new Parameter() { Name = "Lat", Type = SqlDbType.Decimal, Value = lat });
            parameters.Add(new Parameter() { Name = "PlanningId", Type = SqlDbType.UniqueIdentifier, Value = planningId });
            parameters.Add(new Parameter() { Name = "PlaceId", Type = SqlDbType.UniqueIdentifier, Value = placeId });
            parameters.Add(new Parameter() { Name = "BSPlanningPlaceCategorySql", Type = SqlDbType.VarChar, Value = bsPlanningPlaceCategorySql });
            parameters.Add(new Parameter() { Name = "BSPlaceCategorySql", Type = SqlDbType.VarChar, Value = bsPlaceCategorySql });
            parameters.Add(new Parameter() { Name = "Distance", Type = SqlDbType.Decimal, Value = distance });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetNearbyPlanningsAndPlaces", parameters))
            {
                DataTable dt = ds.Tables[1];
                string netWorks = "";
                foreach (DataRow dr in dt.Rows)
                {
                    netWorks = "";
                    if (dr["G2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2G/";
                    }
                    if (dr["D2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2D/";
                    }
                    if (dr["G3Number"].ToString() != "")
                    {
                        netWorks = netWorks + "3G/";
                    }
                    if (dr["G4Number"].ToString() != "")
                    {
                        netWorks = netWorks + "4G/";
                    }
                    if (netWorks.Length > 0)
                    {
                        netWorks = netWorks.Substring(0, netWorks.Length - 1);
                    }
                    dr["NetWorks"] = netWorks;
                }
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["bsPlannings"] = ds.Tables[0];
                result["bsPlaces"] = dt;
                //result["bsPlaces"] = ds.Tables[1];
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取附近指定距离内的规划和站点列表(室分)
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="planningId">规划Id</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="idPlanningPlaceCategorySql">规划室分类型列表Sql语句</param>
        /// <param name="idPlaceCategorySql">室分类型列表Sql语句</param>
        /// <param name="distance">距离(公里)</param>
        /// <returns>附近指定距离内的规划和站点列表的Json字符串</returns>
        public string GetNearbyPlanningsAndPlacesID(decimal lng, decimal lat, Guid planningId, Guid placeId, string idPlanningPlaceCategorySql, string idPlaceCategorySql, decimal distance)
        {
            List<Parameter> parameters = new List<Parameter>(7);
            parameters.Add(new Parameter() { Name = "Lng", Type = SqlDbType.Decimal, Value = lng });
            parameters.Add(new Parameter() { Name = "Lat", Type = SqlDbType.Decimal, Value = lat });
            parameters.Add(new Parameter() { Name = "PlanningId", Type = SqlDbType.UniqueIdentifier, Value = planningId });
            parameters.Add(new Parameter() { Name = "PlaceId", Type = SqlDbType.UniqueIdentifier, Value = placeId });
            parameters.Add(new Parameter() { Name = "IDPlanningPlaceCategorySql", Type = SqlDbType.VarChar, Value = idPlanningPlaceCategorySql });
            parameters.Add(new Parameter() { Name = "IDPlaceCategorySql", Type = SqlDbType.VarChar, Value = idPlaceCategorySql });
            parameters.Add(new Parameter() { Name = "Distance", Type = SqlDbType.Decimal, Value = distance });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetNearbyPlanningsAndPlacesID", parameters))
            {
                DataTable dt = ds.Tables[1];
                string netWorks = "";
                foreach (DataRow dr in dt.Rows)
                {
                    netWorks = "";
                    if (dr["G2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2G/";
                    }
                    if (dr["D2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2D/";
                    }
                    if (dr["G3Number"].ToString() != "")
                    {
                        netWorks = netWorks + "3G/";
                    }
                    if (dr["G4Number"].ToString() != "")
                    {
                        netWorks = netWorks + "4G/";
                    }
                    if (netWorks.Length > 0)
                    {
                        netWorks = netWorks.Substring(0, netWorks.Length - 1);
                    }
                    dr["NetWorks"] = netWorks;
                }
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["idPlannings"] = ds.Tables[0];
                result["idPlaces"] = dt;
                //result["bsPlaces"] = ds.Tables[1];
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取附近指定距离内的规划和站点列表
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="distance">距离(公里)</param>
        /// <returns>附近指定距离内的规划和站点列表的Json字符串</returns>
        public string GetNearbyPlanningsAndPlacesMobile(decimal lng, decimal lat, decimal distance)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "Lng", Type = SqlDbType.Decimal, Value = lng });
            parameters.Add(new Parameter() { Name = "Lat", Type = SqlDbType.Decimal, Value = lat });
            parameters.Add(new Parameter() { Name = "Distance", Type = SqlDbType.Decimal, Value = distance });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetNearbyPlanningsAndPlacesMobile", parameters))
            {
                DataTable dt = ds.Tables[1];
                string netWorks = "";
                foreach (DataRow dr in dt.Rows)
                {
                    netWorks = "";
                    if (dr["G2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2G/";
                    }
                    if (dr["D2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2D/";
                    }
                    if (dr["G3Number"].ToString() != "")
                    {
                        netWorks = netWorks + "3G/";
                    }
                    if (dr["G4Number"].ToString() != "")
                    {
                        netWorks = netWorks + "4G/";
                    }
                    if (netWorks.Length > 0)
                    {
                        netWorks = netWorks.Substring(0, netWorks.Length - 1);
                    }
                    dr["NetWorks"] = netWorks;
                }
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["bsPlannings"] = ds.Tables[0];
                result["bsPlaces"] = dt;
                //result["bsPlaces"] = ds.Tables[1];
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取附近指定距离内的规划和站点列表
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="planningId">规划Id</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="planningProfessionsSql">规划站专业</param>
        /// <param name="placeProfessionsSql">已有站专业</param>
        /// <param name="distance">距离(公里)</param>
        /// <returns>附近指定距离内的规划和站点列表的Json字符串</returns>
        public string GetNearbyPlanningsAndPlacesAll(decimal lng, decimal lat, Guid planningId, Guid placeId, string planningProfessionsSql, string placeProfessionsSql, decimal distance)
        {
            List<Parameter> parameters = new List<Parameter>(7);
            parameters.Add(new Parameter() { Name = "Lng", Type = SqlDbType.Decimal, Value = lng });
            parameters.Add(new Parameter() { Name = "Lat", Type = SqlDbType.Decimal, Value = lat });
            parameters.Add(new Parameter() { Name = "PlanningId", Type = SqlDbType.UniqueIdentifier, Value = planningId });
            parameters.Add(new Parameter() { Name = "PlaceId", Type = SqlDbType.UniqueIdentifier, Value = placeId });
            parameters.Add(new Parameter() { Name = "PlanningProfessionsSql", Type = SqlDbType.VarChar, Value = planningProfessionsSql });
            parameters.Add(new Parameter() { Name = "PlaceProfessionsSql", Type = SqlDbType.VarChar, Value = placeProfessionsSql });
            parameters.Add(new Parameter() { Name = "Distance", Type = SqlDbType.Decimal, Value = distance });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetNearbyPlanningsAndPlacesAll", parameters))
            {
                DataTable dt = ds.Tables[1];
                string netWorks = "";
                foreach (DataRow dr in dt.Rows)
                {
                    netWorks = "";
                    if (dr["G2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2G/";
                    }
                    if (dr["D2Number"].ToString() != "")
                    {
                        netWorks = netWorks + "2D/";
                    }
                    if (dr["G3Number"].ToString() != "")
                    {
                        netWorks = netWorks + "3G/";
                    }
                    if (dr["G4Number"].ToString() != "")
                    {
                        netWorks = netWorks + "4G/";
                    }
                    if (netWorks.Length > 0)
                    {
                        netWorks = netWorks.Substring(0, netWorks.Length - 1);
                    }
                    dr["NetWorks"] = netWorks;
                }
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["plannings"] = ds.Tables[0];
                result["places"] = dt;
                //result["bsPlaces"] = ds.Tables[1];
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据规划和关联的运营商规划获取点对象
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <returns>点对象</returns>
        public PointObject GetPointByOperatorsPlanningDemandAndAssociatedOperatorsPlannings(Guid operatorsPlanningDemandId)
        {
            OperatorsPlanningDemand operatorsPlanningDemand = operatorsPlanningDemandRepository.FindByKey(operatorsPlanningDemandId);
            if (operatorsPlanningDemand != null)
            {
                Place place = placeRepository.GetByKey(operatorsPlanningDemand.PlaceId);
                PlaceCategory placeCategory = placeCategoryRepository.GetByKey(place.PlaceCategoryId);
                Reseau reseau = reseauRepository.GetByKey(place.ReseauId);
                Area area = areaRepository.GetByKey(reseau.AreaId);

                PointObject pointObject = new PointObject();
                pointObject.DataType = 2;
                Company company = companyRepository.GetByKey(Guid.Parse("9D4A4487-2AD6-4C19-8633-00742E8F1D28"));
                pointObject.CompanyId = Guid.Parse("9D4A4487-2AD6-4C19-8633-00742E8F1D28");
                pointObject.CompanyName = company.CompanyName;
                pointObject.Id = place.Id;
                pointObject.Profession = (int)place.Profession;
                pointObject.PlaceName = place.PlaceName;
                pointObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                pointObject.AreaName = area.AreaName;
                pointObject.ReseauName = reseau.ReseauName;
                pointObject.Lng = place.Lng;
                pointObject.Lat = place.Lat;
                //pointObject.PropertyRight = (int)place.PropertyRight;
                //pointObject.TelecomShare = (int)place.TelecomShare;
                //pointObject.MobileShare = (int)place.MobileShare;
                //pointObject.UnicomShare = (int)place.UnicomShare;
                pointObject.OwnerName = place.OwnerName;
                pointObject.OwnerContact = place.OwnerContact;
                pointObject.OwnerPhoneNumber = place.OwnerPhoneNumber;
                pointObject.PlaceMapState = (int)place.PlaceMapState;

                List<Parameter> parameters = new List<Parameter>(1);
                parameters.Add(new Parameter() { Name = "OperatorsPlanningDemandId", Type = SqlDbType.UniqueIdentifier, Value = operatorsPlanningDemandId });
                using (var dt = SqlHelper.ExecuteDataTable("prc_GetOperatorsPlanningsByOperatorsPlanningDemand", parameters))
                {
                    pointObject.OperatorsPlannings = JsonHelper.Encode(dt);
                }

                return pointObject;
            }
            else
            {
                throw new ApplicationFault("选择的需求确认在系统中不存在");
            }
        }

        /// <summary>
        /// 获取周边资源列表(移动端)
        /// </summary>
        /// <param name="professionListSql">专业sql</param>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <param name="distance">距离(公里)</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>附近指定距离内的规划和站点列表的Json字符串</returns>
        public string GetNearbyPlanningsAndPlacesListMobile(string professionListSql, decimal lng, decimal lat, decimal distance, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "ProfessionListSql", Type = SqlDbType.VarChar, Value = professionListSql });
            parameters.Add(new Parameter() { Name = "Lng", Type = SqlDbType.Decimal, Value = lng });
            parameters.Add(new Parameter() { Name = "Lat", Type = SqlDbType.Decimal, Value = lat });
            parameters.Add(new Parameter() { Name = "Distance", Type = SqlDbType.Decimal, Value = distance });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetNearbyPlanningsAndPlacesListMobile", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["PlanningAndPlaceList"] = ds.Tables[0];
                result["ProfessionList"] = ds.Tables[1];
                return JsonHelper.Encode(result);
            }
        }
    }
}
