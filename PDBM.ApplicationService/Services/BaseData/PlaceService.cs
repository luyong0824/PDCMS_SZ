using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using PDBM.Domain.Models.BMMgmt;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 站点应用层服务
    /// </summary>
    public class PlaceService : DataService, IPlaceService
    {
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<DutyUser> dutyUserRepository;
        private readonly IRepository<PlaceProperty> placePropertyRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<Area> areaRepository;
        private readonly IRepository<Scene> sceneRepository;
        private readonly IRepository<PlaceCategory> placeCategoryRepository;
        private readonly IRepository<PlaceOwner> placeOwnerRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Department> departmentRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FoundationTest> foundationTestRepository;

        public PlaceService(IRepositoryContext context,
            IRepository<Place> placeRepository,
            IRepository<DutyUser> dutyUserRepository,
            IRepository<PlaceProperty> placePropertyRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<Area> areaRepository,
            IRepository<Scene> sceneRepository,
            IRepository<PlaceCategory> placeCategoryRepository,
            IRepository<PlaceOwner> placeOwnerRepository,
            IRepository<User> userRepository,
            IRepository<Department> departmentRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<Tower> towerRepository,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FoundationTest> foundationTestRepository)
            : base(context)
        {
            this.placeRepository = placeRepository;
            this.dutyUserRepository = dutyUserRepository;
            this.placePropertyRepository = placePropertyRepository;
            this.reseauRepository = reseauRepository;
            this.areaRepository = areaRepository;
            this.sceneRepository = sceneRepository;
            this.placeCategoryRepository = placeCategoryRepository;
            this.placeOwnerRepository = placeOwnerRepository;
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.towerRepository = towerRepository;
            this.towerBaseRepository = towerBaseRepository;
            this.machineRoomRepository = machineRoomRepository;
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.addressExplorRepository = addressExplorRepository;
            this.foundationTestRepository = foundationTestRepository;
        }

        /// <summary>
        /// 根据站点Id获取站点
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns>站点维护对象</returns>
        public PlaceMaintObject GetPlaceById(Guid id)
        {
            Place place = placeRepository.FindByKey(id);
            if (place != null)
            {
                PlaceMaintObject placeMaintObject = MapperHelper.Map<Place, PlaceMaintObject>(place);
                Reseau reseau = reseauRepository.GetByKey(place.ReseauId);
                placeMaintObject.AreaId = reseau.AreaId;
                User user = userRepository.FindByKey(place.CreateUserId);
                if (user != null)
                {
                    placeMaintObject.FullName = user.FullName;
                }
                placeMaintObject.CreateDateText = place.CreateDate.ToShortDateString();
                placeMaintObject.FileIdList = "";
                FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == place.Id && entity.EntityName == "Place"));
                if (placeFileAssociation != null)
                {
                    int count = 0;
                    if (placeFileAssociation.FileIdList != "")
                    {
                        if (placeFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = placeFileAssociation.FileIdList.Split(',');
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
                    placeMaintObject.Count = count;
                    placeMaintObject.FileIdList = placeFileAssociation.FileIdList;
                }
                else
                {
                    placeMaintObject.Count = 0;
                    placeMaintObject.FileIdList = "";
                }
                return placeMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的站点在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页站点列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="profession">专业</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="state">状态</param>
        /// <returns>分页站点列表的Json字符串</returns>
        public string GetPlacesPage(int pageIndex, int pageSize, int profession, string placeName, Guid areaId, Guid reseauId, Guid placeOwner, int state)
        {
            List<Parameter> parameters = new List<Parameter>(8);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "PlaceOwner", Type = SqlDbType.UniqueIdentifier, Value = placeOwner });
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = state });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryPlacesPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页站点列表，用于选择站点
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <returns>分页站点列表的Json字符串</returns>
        public string GetPlacesPageBySelect(int pageIndex, int pageSize, string placeCode, string placeName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId)
        {
            List<Parameter> parameters = new List<Parameter>(8);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceCode", Type = SqlDbType.NVarChar, Value = placeCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryPlacesPageBySelect", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 修改站点
        /// </summary>
        /// <param name="placeMaintObject">要修改的站点维护对象</param>
        public void UpdatePlace(PlaceMaintObject placeMaintObject)
        {
            if (placeMaintObject.Id != Guid.Empty)
            {
                Place place = placeRepository.FindByKey(placeMaintObject.Id);
                if (place != null)
                {
                    place.Modify(placeMaintObject.PlaceName, placeMaintObject.PlaceCategoryId, placeMaintObject.ReseauId, placeMaintObject.Lng, placeMaintObject.Lat, placeMaintObject.PlaceOwner,
                        (Importance)placeMaintObject.Importance, placeMaintObject.AddressingRealName, placeMaintObject.OwnerName, placeMaintObject.OwnerContact, placeMaintObject.OwnerPhoneNumber,
                        placeMaintObject.DetailedAddress, placeMaintObject.Remarks, (State)placeMaintObject.State, placeMaintObject.G2Number, placeMaintObject.D2Number, placeMaintObject.G3Number,
                         placeMaintObject.G4Number, placeMaintObject.G5Number, placeMaintObject.ModifyUserId);
                    placeRepository.Update(place);

                    FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == place.Id && entity.EntityName == "Place"));
                    if (placeFileAssociation == null && placeMaintObject.FileIdList != "")
                    {
                        FileAssociation newPlaceFileAssociation = AggregateFactory.CreateFileAssociation("Place", place.Id, placeMaintObject.FileIdList, placeMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newPlaceFileAssociation);
                    }
                    else if (placeFileAssociation != null && placeMaintObject.FileIdList != placeFileAssociation.FileIdList)
                    {
                        placeFileAssociation.Modify(placeMaintObject.FileIdList, placeMaintObject.ModifyUserId);
                        fileAssociationRepository.Update(placeFileAssociation);
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
                else if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                {
                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("选择的网格在系统中不存在");
                }
                throw ex;
            }
        }

        public PlaceInfoObject GetPlaceInfoById(Guid id)
        {
            Place place = placeRepository.FindByKey(id);
            if (place != null)
            {
                PlaceInfoObject placeInfoObject = new PlaceInfoObject();
                PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == id));
                Reseau reseau = reseauRepository.GetByKey(place.ReseauId);
                Area area = areaRepository.GetByKey(reseau.AreaId);
                PlaceCategory placeCategory = placeCategoryRepository.GetByKey(place.PlaceCategoryId);
                PlaceOwner placeOwner = placeOwnerRepository.GetByKey(place.PlaceOwner);
                User userPlace = userRepository.FindByKey(place.CreateUserId);
                Department department = departmentRepository.GetByKey(place.AddressingDepartmentId);

                placeInfoObject.PlaceCode = place.PlaceCode;
                placeInfoObject.PlaceName = place.PlaceName;
                placeInfoObject.AreaName = area.AreaName;
                placeInfoObject.ReseauName = reseau.ReseauName;
                placeInfoObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                placeInfoObject.ImportanceName = EnumHelper.GetEnumText(typeof(Importance), place.Importance);
                placeInfoObject.Lat = place.Lat;
                placeInfoObject.Lng = place.Lng;
                placeInfoObject.AddressingDepartmentName = department.DepartmentName;
                placeInfoObject.AddressingRealName = place.AddressingRealName;
                placeInfoObject.PlaceOwnerName = placeOwner.PlaceOwnerName;
                placeInfoObject.OwnerName = place.OwnerName;
                placeInfoObject.OwnerContact = place.OwnerContact;
                placeInfoObject.OwnerPhoneNumber = place.OwnerPhoneNumber;
                placeInfoObject.DetailedAddress = place.DetailedAddress;
                placeInfoObject.Remarks = place.Remarks;
                placeInfoObject.G2Number = place.G2Number;
                placeInfoObject.D2Number = place.D2Number;
                placeInfoObject.G3Number = place.G3Number;
                placeInfoObject.G4Number = place.G4Number;
                placeInfoObject.G5Number = place.G5Number;
                placeInfoObject.StateName = EnumHelper.GetEnumText(typeof(State), place.State);
                placeInfoObject.CreateUserName = userPlace.FullName;
                placeInfoObject.CreateDate = place.CreateDate.ToShortDateString();

                FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == place.Id && entity.EntityName == "Place"));
                if (placeFileAssociation != null)
                {
                    int count = 0;
                    if (placeFileAssociation.FileIdList != "")
                    {
                        if (placeFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = placeFileAssociation.FileIdList.Split(',');
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
                    placeInfoObject.Count = count;
                }
                else
                {
                    placeInfoObject.Count = 0;
                }
                placeInfoObject.Id = place.Id;
                return placeInfoObject;
            }
            else
            {
                throw new ApplicationFault("选择的站点在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页资源导入站点列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="propertyRight">产权</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="telecomShare">是否电信共享</param>
        /// <param name="mobileShare">是否移动共享</param>
        /// <param name="unicomShare">是否联通共享</param>
        /// <param name="state">状态</param>
        /// <returns>分页站点列表的Json字符串</returns>
        public string GetResourcePlacesPage(int pageIndex, int pageSize, string groupPlaceCode, string placeName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int propertyRight, int importance, int telecomShare, int mobileShare, int unicomShare, int state)
        {
            List<Parameter> parameters = new List<Parameter>(14);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "GroupPlaceCode", Type = SqlDbType.NVarChar, Value = groupPlaceCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "PropertyRight", Type = SqlDbType.Int, Value = propertyRight });
            parameters.Add(new Parameter() { Name = "Importance", Type = SqlDbType.Int, Value = importance });
            parameters.Add(new Parameter() { Name = "TelecomShare", Type = SqlDbType.Int, Value = telecomShare });
            parameters.Add(new Parameter() { Name = "MobileShare", Type = SqlDbType.Int, Value = mobileShare });
            parameters.Add(new Parameter() { Name = "UnicomShare", Type = SqlDbType.Int, Value = unicomShare });
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = state });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryResourcePlacesPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据站点Id获取站点
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns>站点维护对象</returns>
        public PlaceMaintObject GetPlaceImportById(Guid id)
        {
            Place place = placeRepository.FindByKey(id);
            if (place != null)
            {
                PlaceMaintObject placeMaintObject = MapperHelper.Map<Place, PlaceMaintObject>(place);
                Reseau reseau = reseauRepository.GetByKey(place.ReseauId);
                placeMaintObject.AreaId = reseau.AreaId;
                User user = userRepository.FindByKey(place.CreateUserId);
                if (user != null)
                {
                    placeMaintObject.FullName = user.FullName;
                }
                placeMaintObject.CreateDateText = place.CreateDate.ToShortDateString();
                placeMaintObject.FileIdList = "";
                FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == place.Id && entity.EntityName == "Place"));
                if (placeFileAssociation != null)
                {
                    int count = 0;
                    if (placeFileAssociation.FileIdList != "")
                    {
                        if (placeFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = placeFileAssociation.FileIdList.Split(',');
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
                    placeMaintObject.Count = count;
                    placeMaintObject.FileIdList = placeFileAssociation.FileIdList;
                }
                else
                {
                    placeMaintObject.Count = 0;
                    placeMaintObject.FileIdList = "";
                }
                return placeMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的站点在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页资源导入站点列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeCode">基站编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">基站类型Id</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="state">状态</param>
        /// <returns>分页站点列表的Json字符串</returns>
        public string GetPlaceImportsPage(int pageIndex, int pageSize, string placeCode, string placeName, int profession, Guid placeCategoryId, Guid placeOwner,
            Guid areaId, Guid reseauId, int importance, int state, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(11);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceCode", Type = SqlDbType.NVarChar, Value = placeCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "PlaceOwner", Type = SqlDbType.UniqueIdentifier, Value = placeOwner });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Importance", Type = SqlDbType.Int, Value = importance });
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = state });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryPlaceImportsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 修改站点
        /// </summary>
        /// <param name="placeMaintObject">要修改的站点维护对象</param>
        public void SavePlaceImport(PlaceMaintObject placeMaintObject)
        {
            if (placeMaintObject.Id != Guid.Empty)
            {
                Place place = placeRepository.FindByKey(placeMaintObject.Id);
                if (place != null)
                {
                    place.ModifyPlaceImport(placeMaintObject.PlaceName, placeMaintObject.PlaceCategoryId, placeMaintObject.ReseauId, placeMaintObject.Lng, placeMaintObject.Lat, placeMaintObject.PlaceOwner,
                        (Importance)placeMaintObject.Importance, placeMaintObject.AddressingDepartmentId, placeMaintObject.AddressingRealName, placeMaintObject.OwnerName, placeMaintObject.OwnerContact,
                        placeMaintObject.OwnerPhoneNumber, placeMaintObject.DetailedAddress, placeMaintObject.Remarks, (State)placeMaintObject.State, placeMaintObject.ModifyUserId);
                    placeRepository.Update(place);

                    FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == place.Id && entity.EntityName == "Place"));
                    if (placeFileAssociation == null && placeMaintObject.FileIdList != "")
                    {
                        FileAssociation newPlaceFileAssociation = AggregateFactory.CreateFileAssociation("Place", place.Id, placeMaintObject.FileIdList, placeMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newPlaceFileAssociation);
                    }
                    else if (placeFileAssociation != null && placeMaintObject.FileIdList != placeFileAssociation.FileIdList)
                    {
                        placeFileAssociation.Modify(placeMaintObject.FileIdList, placeMaintObject.ModifyUserId);
                        fileAssociationRepository.Update(placeFileAssociation);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PlaceName"))
                {
                    throw new ApplicationFault("基站名称重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                {
                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("选择的网格在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 根据站点Id获取逻辑号
        /// </summary>
        /// <param name="id">站点Id</param>
        /// <returns>站点维护对象</returns>
        public PlaceMaintObject GetLogicalNumberById(Guid id)
        {
            Place place = placeRepository.FindByKey(id);
            if (place != null)
            {
                PlaceMaintObject placeMaintObject = MapperHelper.Map<Place, PlaceMaintObject>(place);
                Reseau reseau = reseauRepository.GetByKey(place.ReseauId);
                Area area = areaRepository.GetByKey(reseau.AreaId);

                placeMaintObject.Id = place.Id;
                placeMaintObject.PlaceCode = place.PlaceCode;
                placeMaintObject.PlaceName = place.PlaceName;
                placeMaintObject.AreaName = area.AreaName;
                placeMaintObject.ReseauName = reseau.ReseauName;
                placeMaintObject.G2Number = place.G2Number;
                placeMaintObject.D2Number = place.D2Number;
                placeMaintObject.G3Number = place.G3Number;
                placeMaintObject.G4Number = place.G4Number;

                return placeMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的站点在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页基站站点列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeCode">基站编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="profession">专业</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="g2Mark">2G</param>
        /// <param name="d2Mark">2D</param>
        /// <param name="g3Mark">3G</param>
        /// <param name="g4Mark">4G</param>
        /// <param name="g2Number">2G逻辑号</param>
        /// <param name="d2Number">2D逻辑号</param>
        /// <param name="g3Number">3G逻辑号</param>
        /// <param name="g4Number">4G逻辑号</param>
        /// <param name="allMark">全部</param>
        /// <returns>分页站点列表的Json字符串</returns>
        public string GetLogicalNumbersPage(int pageIndex, int pageSize, string placeCode, string placeName, int profession, Guid areaId, Guid reseauId,
            int g2Mark, int d2Mark, int g3Mark, int g4Mark, string g2Number, string d2Number, string g3Number, string g4Number, int allMark)
        {
            List<Parameter> parameters = new List<Parameter>(16);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceCode", Type = SqlDbType.NVarChar, Value = placeCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "G2Mark", Type = SqlDbType.Int, Value = g2Mark });
            parameters.Add(new Parameter() { Name = "D2Mark", Type = SqlDbType.Int, Value = d2Mark });
            parameters.Add(new Parameter() { Name = "G3Mark", Type = SqlDbType.Int, Value = g3Mark });
            parameters.Add(new Parameter() { Name = "G4Mark", Type = SqlDbType.Int, Value = g4Mark });
            parameters.Add(new Parameter() { Name = "G2Number", Type = SqlDbType.NVarChar, Value = g2Number });
            parameters.Add(new Parameter() { Name = "D2Number", Type = SqlDbType.NVarChar, Value = d2Number });
            parameters.Add(new Parameter() { Name = "G3Number", Type = SqlDbType.NVarChar, Value = g3Number });
            parameters.Add(new Parameter() { Name = "G4Number", Type = SqlDbType.NVarChar, Value = g4Number });
            parameters.Add(new Parameter() { Name = "AllMark", Type = SqlDbType.Int, Value = allMark });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryLogicalNumbersPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 修改逻辑号
        /// </summary>
        /// <param name="placeMaintObject">要修改的站点维护对象</param>
        public void SaveLogicalNumber(PlaceMaintObject placeMaintObject)
        {
            if (placeMaintObject.Id != Guid.Empty)
            {
                Place place = placeRepository.FindByKey(placeMaintObject.Id);
                if (place != null)
                {
                    if (placeMaintObject.G2Number != "")
                    {
                        if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G2Number == placeMaintObject.G2Number && entity.Id != place.Id)))
                        {
                            throw new ApplicationFault("系统中已存在该2G逻辑号");
                        }
                    }
                    if (placeMaintObject.D2Number != "")
                    {
                        if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.D2Number == placeMaintObject.D2Number && entity.Id != place.Id)))
                        {
                            throw new ApplicationFault("系统中已存在该2D逻辑号");
                        }
                    }
                    if (placeMaintObject.G3Number != "")
                    {
                        if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G3Number == placeMaintObject.G3Number && entity.Id != place.Id)))
                        {
                            throw new ApplicationFault("系统中已存在该3G逻辑号");
                        }
                    }
                    if (placeMaintObject.G4Number != "")
                    {
                        if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G4Number == placeMaintObject.G4Number && entity.Id != place.Id)))
                        {
                            throw new ApplicationFault("系统中已存在该4G逻辑号");
                        }
                    }

                    place.LogicalNumberMaintain(placeMaintObject.G2Number, placeMaintObject.D2Number, placeMaintObject.G3Number, placeMaintObject.G4Number);
                    placeRepository.Update(place);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据条件获取基站列表(移动端)
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="professionListSql">专业sql</param>
        /// <param name="placeName">站点名称</param>
        /// <returns>分页站点列表的Json字符串</returns>
        public string GetPlacesMobile(int pageIndex, int pageSize, string professionListSql, string placeName, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProfessionListSql", Type = SqlDbType.VarChar, Value = professionListSql });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetPlacesMobile", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取基站列表(移动端)
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="professionListSql">专业sql</param>
        /// <returns>分页站点列表的Json字符串</returns>
        public string GetPlacesPageMobile(int pageIndex, int pageSize, string professionListSql, decimal lng, decimal lat, decimal distance, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(7);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProfessionListSql", Type = SqlDbType.VarChar, Value = professionListSql });
            parameters.Add(new Parameter() { Name = "Lng", Type = SqlDbType.Decimal, Value = lng });
            parameters.Add(new Parameter() { Name = "Lat", Type = SqlDbType.Decimal, Value = lat });
            parameters.Add(new Parameter() { Name = "Distance", Type = SqlDbType.Decimal, Value = distance });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetPlacesPageMobile", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 更新站点方位(移动端)
        /// </summary>
        /// <param name="placeMaintObject">要修改的站点维护对象</param>
        public void SavePlacePositionMobile(PlaceMaintObject placeMaintObject)
        {
            if (placeMaintObject.Id != Guid.Empty)
            {
                Place place = placeRepository.FindByKey(placeMaintObject.Id);
                if (place != null)
                {
                    place.Lng = placeMaintObject.Lng;
                    place.Lat = placeMaintObject.Lat;
                    placeRepository.Update(place);
                }
                else
                {
                    throw new ApplicationFault("该站点在系统中不存在");
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 站点修改(移动端)
        /// </summary>
        /// <param name="placeMaintObject"></param>
        public void SavePlaceMobile(PlaceMaintObject placeMaintObject)
        {
            if (placeMaintObject.Id != Guid.Empty)
            {
                Place place = placeRepository.FindByKey(placeMaintObject.Id);
                if (place != null)
                {
                    Duty duty = Duty.网优人员;
                    DutyUser dutyUser = dutyUserRepository.Find(Specification<DutyUser>.Eval(entity => entity.UserId == placeMaintObject.ModifyUserId && entity.Duty == duty));
                    if (dutyUser == null)
                    {
                        throw new ApplicationFault("只有网优人员可以修改站点信息");
                    }
                    place.PlaceName = placeMaintObject.PlaceName;
                    place.PlaceCategoryId = placeMaintObject.PlaceCategoryId;
                    place.ReseauId = placeMaintObject.ReseauId;
                    place.Lng = placeMaintObject.Lng;
                    place.Lat = placeMaintObject.Lat;
                    place.PlaceOwner = placeMaintObject.PlaceOwner;
                    place.Importance = (Importance)placeMaintObject.Importance;
                    place.AddressingRealName = placeMaintObject.AddressingRealName;
                    place.OwnerName = placeMaintObject.OwnerName;
                    place.OwnerContact = placeMaintObject.OwnerContact;
                    place.OwnerPhoneNumber = placeMaintObject.OwnerPhoneNumber;
                    place.DetailedAddress = placeMaintObject.DetailedAddress;
                    placeRepository.Update(place);
                }
                else
                {
                    throw new ApplicationFault("该站点在系统中不存在");
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
