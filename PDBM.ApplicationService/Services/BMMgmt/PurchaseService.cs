using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Services;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 购置站点应用层服务
    /// </summary>
    public class PurchaseService : DataService, IPurchaseService
    {
        private readonly IRepository<Purchase> purchaseRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly ICodeSeedRepository codeSeedRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IBMMgmtService bmMgmtService;
        private readonly IRepository<OperatorsSharing> operatorsSharingRepository;
        private readonly IRepository<Remodeling> remodelingRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<PlaceProperty> placePropertyRepository;
        private readonly IRepository<PlacePropertyLog> placePropertyLogRepository;
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FoundationTest> foundationTestRepository;

        public PurchaseService(IRepositoryContext context,
            IRepository<Purchase> purchaseRepository,
            IRepository<Reseau> reseauRepository,
            ICodeSeedRepository codeSeedRepository,
            IRepository<Place> placeRepository,
            IBMMgmtService bmMgmtService,
            IRepository<OperatorsSharing> operatorsSharingRepository,
            IRepository<Remodeling> remodelingRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<PlaceProperty> placePropertyRepository,
            IRepository<PlacePropertyLog> placePropertyLogRepository,
            IRepository<Tower> towerRepository,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FoundationTest> foundationTestRepository)
            : base(context)
        {
            this.purchaseRepository = purchaseRepository;
            this.reseauRepository = reseauRepository;
            this.codeSeedRepository = codeSeedRepository;
            this.placeRepository = placeRepository;
            this.bmMgmtService = bmMgmtService;
            this.operatorsSharingRepository = operatorsSharingRepository;
            this.remodelingRepository = remodelingRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.placePropertyRepository = placePropertyRepository;
            this.placePropertyLogRepository = placePropertyLogRepository;
            this.towerRepository = towerRepository;
            this.towerBaseRepository = towerBaseRepository;
            this.machineRoomRepository = machineRoomRepository;
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.addressExplorRepository = addressExplorRepository;
            this.foundationTestRepository = foundationTestRepository;
        }

        /// <summary>
        /// 根据购置站点Id获取购置站点
        /// </summary>
        /// <param name="id">购置站点Id</param>
        /// <returns>购置站点维护对象</returns>
        public PurchaseMaintObject GetPurchaseById(Guid id)
        {
            Purchase purchase = purchaseRepository.FindByKey(id);
            if (purchase != null)
            {
                PurchaseMaintObject purchaseMaintObject = MapperHelper.Map<Purchase, PurchaseMaintObject>(purchase);
                purchaseMaintObject.PurchaseDateText = purchase.PurchaseDate.ToShortDateString();
                Reseau reseau = reseauRepository.GetByKey(purchaseMaintObject.ReseauId);
                purchaseMaintObject.AreaId = reseau.AreaId;
                purchaseMaintObject.FileIdList = "";
                FileAssociation purchaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == purchase.Id && entity.EntityName == "Purchase"));
                if (purchaseFileAssociation != null)
                {
                    int count = 0;
                    if (purchaseFileAssociation.FileIdList != "")
                    {
                        if (purchaseFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = purchaseFileAssociation.FileIdList.Split(',');
                            foreach (string i in strFileList)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    purchaseMaintObject.Count = count;
                    purchaseMaintObject.FileIdList = purchaseFileAssociation.FileIdList;
                }
                else
                {
                    purchaseMaintObject.Count = 0;
                    purchaseMaintObject.FileIdList = "";
                }
                return purchaseMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的购置站点在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页购置站点列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="groupPlaceCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="propertyRightSql">产权列表Sql语句</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="telecomShare">是否电信共享</param>
        /// <param name="mobileShare">是否移动共享</param>
        /// <param name="unicomShare">是否联通共享</param>
        /// <returns>分页购置站点列表的Json字符串</returns>
        public string GetPurchasesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string groupPlaceCode, string placeName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, string propertyRightSql, int importance, int telecomShare, int mobileShare, int unicomShare)
        {
            List<Parameter> parameters = new List<Parameter>(15);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "GroupPlaceCode", Type = SqlDbType.NVarChar, Value = groupPlaceCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "PropertyRightSql", Type = SqlDbType.VarChar, Value = propertyRightSql });
            parameters.Add(new Parameter() { Name = "Importance", Type = SqlDbType.Int, Value = importance });
            parameters.Add(new Parameter() { Name = "TelecomShare", Type = SqlDbType.Int, Value = telecomShare });
            parameters.Add(new Parameter() { Name = "MobileShare", Type = SqlDbType.Int, Value = mobileShare });
            parameters.Add(new Parameter() { Name = "UnicomShare", Type = SqlDbType.Int, Value = unicomShare });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryPurchasesPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改购置站点
        /// </summary>
        /// <param name="purchaseMaintObject">要新增或者修改的购置站点维护对象</param>
        public void AddOrUpdatePurchase(PurchaseMaintObject purchaseMaintObject)
        {
            if (purchaseMaintObject.Id == Guid.Empty)
            {
                Purchase purchase = AggregateFactory.CreatePurchase(DateTime.Parse(purchaseMaintObject.PurchaseDateText), purchaseMaintObject.GroupPlaceCode, purchaseMaintObject.PlaceName, (Profession)purchaseMaintObject.Profession, purchaseMaintObject.PlaceCategoryId, purchaseMaintObject.ReseauId,
                    purchaseMaintObject.Lng, purchaseMaintObject.Lat, (PropertyRight)purchaseMaintObject.PropertyRight, (Importance)purchaseMaintObject.Importance, purchaseMaintObject.SceneId, purchaseMaintObject.DetailedAddress,
                    purchaseMaintObject.OwnerName, purchaseMaintObject.OwnerContact, purchaseMaintObject.OwnerPhoneNumber, (Bool)purchaseMaintObject.TelecomShare, (Bool)purchaseMaintObject.MobileShare, (Bool)purchaseMaintObject.UnicomShare,
                    purchaseMaintObject.Remarks, purchaseMaintObject.CreateUserId);
                purchaseRepository.Add(purchase);
                Place place = bmMgmtService.CreatePlace(purchase, codeSeedRepository.GenerateCode("Place"));
                placeRepository.Add(place);
                PlaceProperty placeProperty = AggregateFactory.CreatePlaceProperty(place.Id, PropertyType.站点参数, (Bool)purchaseMaintObject.MobileShare, 0, 0, 0, Guid.Empty, (Bool)purchaseMaintObject.TelecomShare, 0, 0, 0, Guid.Empty, (Bool)purchaseMaintObject.UnicomShare, 0, 0, 0, Guid.Empty);
                placePropertyRepository.Add(placeProperty);
                if ((Bool)purchaseMaintObject.MobileShare == Bool.是)
                {
                    PlacePropertyLog mobileLog = AggregateFactory.CreatePlacePropertyLog(OperationType.新增, place.Id, PropertyType.站点参数, CompanyNameId.移动, (Bool)purchaseMaintObject.MobileShare, 0, 0, 0, Guid.Empty, (Bool)purchaseMaintObject.TelecomShare, 0, 0, 0, Guid.Empty, (Bool)purchaseMaintObject.UnicomShare, 0, 0, 0, Guid.Empty);
                    placePropertyLogRepository.Add(mobileLog);
                }
                if ((Bool)purchaseMaintObject.TelecomShare == Bool.是)
                {
                    PlacePropertyLog telecomLog = AggregateFactory.CreatePlacePropertyLog(OperationType.新增, place.Id, PropertyType.站点参数, CompanyNameId.电信, (Bool)purchaseMaintObject.MobileShare, 0, 0, 0, Guid.Empty, (Bool)purchaseMaintObject.TelecomShare, 0, 0, 0, Guid.Empty, (Bool)purchaseMaintObject.UnicomShare, 0, 0, 0, Guid.Empty);
                    placePropertyLogRepository.Add(telecomLog);
                }
                if ((Bool)purchaseMaintObject.UnicomShare == Bool.是)
                {
                    PlacePropertyLog unicomLog = AggregateFactory.CreatePlacePropertyLog(OperationType.新增, place.Id, PropertyType.站点参数, CompanyNameId.联通, (Bool)purchaseMaintObject.MobileShare, 0, 0, 0, Guid.Empty, (Bool)purchaseMaintObject.TelecomShare, 0, 0, 0, Guid.Empty, (Bool)purchaseMaintObject.UnicomShare, 0, 0, 0, Guid.Empty);
                    placePropertyLogRepository.Add(unicomLog);
                }

                if (purchaseMaintObject.FileIdList != "")
                {
                    FileAssociation purchaseFileAssociation = AggregateFactory.CreateFileAssociation("Purchase", purchase.Id, purchaseMaintObject.FileIdList, purchaseMaintObject.CreateUserId);
                    fileAssociationRepository.Add(purchaseFileAssociation);
                    FileAssociation placeFileAssociation = AggregateFactory.CreateFileAssociation("Place", place.Id, purchaseMaintObject.FileIdList, purchaseMaintObject.CreateUserId);
                    fileAssociationRepository.Add(placeFileAssociation);
                }
            }
            else
            {
                Purchase purchase = purchaseRepository.FindByKey(purchaseMaintObject.Id);
                if (purchase != null)
                {
                    purchase.CheckByUpdate(purchaseMaintObject.ModifyUserId);
                    purchase.Modify(DateTime.Parse(purchaseMaintObject.PurchaseDateText), purchaseMaintObject.GroupPlaceCode, purchaseMaintObject.PlaceName, purchaseMaintObject.PlaceCategoryId, purchaseMaintObject.ReseauId, purchaseMaintObject.Lng, purchaseMaintObject.Lat,
                        (PropertyRight)purchaseMaintObject.PropertyRight, (Importance)purchaseMaintObject.Importance, purchaseMaintObject.SceneId, purchaseMaintObject.DetailedAddress,
                        purchaseMaintObject.OwnerName, purchaseMaintObject.OwnerContact, purchaseMaintObject.OwnerPhoneNumber, (Bool)purchaseMaintObject.TelecomShare, (Bool)purchaseMaintObject.MobileShare,
                        (Bool)purchaseMaintObject.UnicomShare, purchaseMaintObject.Remarks, purchaseMaintObject.ModifyUserId);
                    purchaseRepository.Update(purchase);

                    PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == purchase.PlaceId && entity.PropertyType == PropertyType.站点参数));
                    if ((Bool)purchaseMaintObject.MobileShare != placeProperty.MobileShare)
                    {
                        placeProperty.ModifyMobile((Bool)purchaseMaintObject.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, purchaseMaintObject.ModifyUserId);
                        placePropertyRepository.Update(placeProperty);
                        PlacePropertyLog mobilePlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.移动, (Bool)purchaseMaintObject.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, purchaseMaintObject.ModifyUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                        placePropertyLogRepository.Add(mobilePlacePropertyLog);
                    }
                    if ((Bool)purchaseMaintObject.TelecomShare != placeProperty.TelecomShare)
                    {
                        placeProperty.ModifyTelecom((Bool)purchaseMaintObject.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, purchaseMaintObject.ModifyUserId);
                        placePropertyRepository.Update(placeProperty);
                        PlacePropertyLog telecomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.电信, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, (Bool)purchaseMaintObject.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, purchaseMaintObject.ModifyUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                        placePropertyLogRepository.Add(telecomPlacePropertyLog);
                    }
                    if ((Bool)purchaseMaintObject.UnicomShare != placeProperty.UnicomShare)
                    {
                        placeProperty.ModifyUnicom((Bool)purchaseMaintObject.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, purchaseMaintObject.ModifyUserId);
                        placePropertyRepository.Update(placeProperty);
                        PlacePropertyLog unicomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.联通, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, (Bool)purchaseMaintObject.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, purchaseMaintObject.ModifyUserId);
                        placePropertyLogRepository.Add(unicomPlacePropertyLog);
                    }

                    FileAssociation purchaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == purchase.Id && entity.EntityName == "Purchase"));
                    if (purchaseFileAssociation == null && purchaseMaintObject.FileIdList != "")
                    {
                        FileAssociation newPurchaseFileAssociation = AggregateFactory.CreateFileAssociation("Purchase", purchase.Id, purchaseMaintObject.FileIdList, purchaseMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newPurchaseFileAssociation);
                    }
                    else if (purchaseFileAssociation != null && purchaseMaintObject.FileIdList != purchaseFileAssociation.FileIdList)
                    {
                        purchaseFileAssociation.Modify(purchaseMaintObject.FileIdList, purchaseMaintObject.ModifyUserId);
                        fileAssociationRepository.Update(purchaseFileAssociation);
                    }

                    if (purchase.PlaceId != Guid.Empty)
                    {
                        Place place = placeRepository.GetByKey(purchase.PlaceId);
                        Place modifiedPlace = bmMgmtService.ModifyPlace(place, purchase);
                        placeRepository.Update(modifiedPlace);

                        FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == place.Id && entity.EntityName == "Place"));
                        if (placeFileAssociation == null && purchaseMaintObject.FileIdList != "")
                        {
                            FileAssociation newPlaceFileAssociation = AggregateFactory.CreateFileAssociation("Place", place.Id, purchaseMaintObject.FileIdList, purchaseMaintObject.ModifyUserId);
                            fileAssociationRepository.Add(newPlaceFileAssociation);
                        }
                        else if (placeFileAssociation != null && purchaseMaintObject.FileIdList != placeFileAssociation.FileIdList)
                        {
                            placeFileAssociation.Modify(purchaseMaintObject.FileIdList, purchaseMaintObject.ModifyUserId);
                            fileAssociationRepository.Update(placeFileAssociation);
                        }
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_GroupPlaceCode"))
                {
                    throw new ApplicationFault("站点编码重复");
                }
                if (ex.Message.Contains("IX_UQ_PlaceCode"))
                {
                    throw new ApplicationFault("站点编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_PlaceName"))
                {
                    throw new ApplicationFault("站点名称重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Purchase_dbo.tbl_PlaceCategory_PlaceCategoryId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                {
                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Purchase_dbo.tbl_Reseau_ReseauId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("选择的网格在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Purchase_dbo.tbl_Scene_SceneId") || ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Scene_SceneId"))
                {
                    throw new ApplicationFault("选择的周边场景在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除购置站点
        /// </summary>
        /// <param name="purchaseMaintObjects">要删除的购置站点维护对象列表</param>
        public void RemovePurchases(IList<PurchaseMaintObject> purchaseMaintObjects)
        {
            foreach (PurchaseMaintObject purchaseMaintObject in purchaseMaintObjects)
            {
                Purchase purchase = purchaseRepository.FindByKey(purchaseMaintObject.Id);
                if (purchase != null)
                {
                    purchase.CheckByRemove(purchaseMaintObject.ModifyUserId);
                    purchaseRepository.Remove(purchase);

                    FileAssociation purchaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == purchase.Id && entity.EntityName == "Purchase"));
                    if (purchaseFileAssociation != null)
                    {
                        fileAssociationRepository.Remove(purchaseFileAssociation);
                    }

                    if (purchase.PlaceId != Guid.Empty)
                    {
                        if (towerRepository.Exists(Specification<Tower>.Eval(entity => entity.ParentId == purchase.PlaceId && entity.PropertyType == PropertyType.站点参数)))
                        {
                            throw new ApplicationFault("{0}<br>已在登记过铁塔", purchase.PlaceCode);
                        }
                        if (towerBaseRepository.Exists(Specification<TowerBase>.Eval(entity => entity.ParentId == purchase.PlaceId && entity.PropertyType == PropertyType.站点参数)))
                        {
                            throw new ApplicationFault("{0}<br>已在登记过铁塔基础", purchase.PlaceCode);
                        }
                        if (machineRoomRepository.Exists(Specification<MachineRoom>.Eval(entity => entity.ParentId == purchase.PlaceId && entity.PropertyType == PropertyType.站点参数)))
                        {
                            throw new ApplicationFault("{0}<br>已在登记过机房", purchase.PlaceCode);
                        }
                        if (externalElectricPowerRepository.Exists(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == purchase.PlaceId && entity.PropertyType == PropertyType.站点参数)))
                        {
                            throw new ApplicationFault("{0}<br>已在登记过外电引入", purchase.PlaceCode);
                        }
                        if (equipmentInstallRepository.Exists(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == purchase.PlaceId && entity.PropertyType == PropertyType.站点参数)))
                        {
                            throw new ApplicationFault("{0}<br>已在登记过设备安装", purchase.PlaceCode);
                        }
                        if (addressExplorRepository.Exists(Specification<AddressExplor>.Eval(entity => entity.ParentId == purchase.PlaceId && entity.PropertyType == PropertyType.站点参数)))
                        {
                            throw new ApplicationFault("{0}<br>已在登记过地质勘探", purchase.PlaceCode);
                        }
                        if (foundationTestRepository.Exists(Specification<FoundationTest>.Eval(entity => entity.ParentId == purchase.PlaceId && entity.PropertyType == PropertyType.站点参数)))
                        {
                            throw new ApplicationFault("{0}<br>已在登记过桩基动测", purchase.PlaceCode);
                        }
                        if (placePropertyRepository.Exists(Specification<PlaceProperty>.Eval(entity => entity.ParentId == purchase.PlaceId && entity.PropertyType == PropertyType.站点参数 && (entity.MobileCreateUserId != Guid.Empty || entity.TelecomCreateUserId != Guid.Empty || entity.UnicomCreateUserId != Guid.Empty))))
                        {
                            throw new ApplicationFault("{0}<br>已登记过运营商使用情况", purchase.PlaceCode);
                        }
                        if (operatorsSharingRepository.Exists(Specification<OperatorsSharing>.Eval(entity => entity.PlaceId == purchase.PlaceId)))
                        {
                            throw new ApplicationFault("{0}<br>已被运营商申请共享", purchase.PlaceCode);
                        }
                        if (remodelingRepository.Exists(Specification<Remodeling>.Eval(entity => entity.PlaceId == purchase.PlaceId)))
                        {
                            throw new ApplicationFault("{0}<br>已在基站改造中使用", purchase.PlaceCode);
                        }

                        Place place = placeRepository.GetByKey(purchase.PlaceId);
                        placeRepository.Remove(place);
                        PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == purchase.PlaceId && entity.PropertyType == PropertyType.站点参数));
                        placePropertyRepository.Remove(placeProperty);

                        if (purchase.MobileShare == Bool.是)
                        {
                            PlacePropertyLog mobilePlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.删除, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.移动, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                            placePropertyLogRepository.Add(mobilePlacePropertyLog);
                        }
                        if (purchase.TelecomShare == Bool.是)
                        {
                            PlacePropertyLog telecomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.删除, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.电信, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                            placePropertyLogRepository.Add(telecomPlacePropertyLog);
                        }
                        if (purchase.UnicomShare == Bool.是)
                        {
                            PlacePropertyLog unicomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.删除, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.联通, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                            placePropertyLogRepository.Add(unicomPlacePropertyLog);
                        }
                        FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == place.Id && entity.EntityName == "Place"));
                        if (placeFileAssociation != null)
                        {
                            fileAssociationRepository.Remove(placeFileAssociation);
                        }
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_OperatorsSharing_dbo.tbl_Place_PlaceId"))
                {
                    throw new ApplicationFault("已被运营商申请共享");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Remodeling_dbo.tbl_Place_PlaceId"))
                {
                    throw new ApplicationFault("已在基站改造中使用");
                }
                throw ex;
            }
        }
    }
}
