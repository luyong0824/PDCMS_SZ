using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System.Data;
using PDBM.Domain.Models.FileMgmt;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class ConstructionTaskService : DataService, IConstructionTaskService
    {
        private readonly IRepository<ConstructionTask> constructionTaskRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<PlaceProperty> placePropertyRepository;
        private readonly IRepository<PlacePropertyLog> placePropertyLogRepository;
        private readonly IRepository<Project> projectRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FoundationTest> foundationTestRepository;
        private readonly IRepository<TaskProperty> taskPropertyRepository;
        private readonly IRepository<TaskPropertyLog> taskPropertyLogRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<Area> areaRepository;
        private readonly IRepository<Scene> sceneRepository;
        private readonly IRepository<PlaceCategory> placeCategoryRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Remodeling> remodelingRepository;
        private readonly IRepository<OperatorsSharing> operatorsSharingRepository;
        private readonly IRepository<Customer> customerRepository;

        public ConstructionTaskService(IRepositoryContext context,
            IRepository<ConstructionTask> constructionTaskRepository,
            IRepository<Place> placeRepository,
            IRepository<PlaceProperty> placePropertyRepository,
            IRepository<PlacePropertyLog> placePropertyLogRepository,
            IRepository<Project> projectRepository,
            IRepository<User> userRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<Tower> towerRepository,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FoundationTest> foundationTestRepository,
            IRepository<TaskProperty> taskPropertyRepository,
            IRepository<TaskPropertyLog> taskPropertyLogRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<Area> areaRepository,
            IRepository<Scene> sceneRepository,
            IRepository<PlaceCategory> placeCategoryRepository,
            IRepository<Planning> planningRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<Remodeling> remodelingRepository,
            IRepository<OperatorsSharing> operatorsSharingRepository,
            IRepository<Customer> customerRepository)
            : base(context)
        {
            this.constructionTaskRepository = constructionTaskRepository;
            this.placeRepository = placeRepository;
            this.placePropertyRepository = placePropertyRepository;
            this.placePropertyLogRepository = placePropertyLogRepository;
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.towerRepository = towerRepository;
            this.towerBaseRepository = towerBaseRepository;
            this.machineRoomRepository = machineRoomRepository;
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.addressExplorRepository = addressExplorRepository;
            this.foundationTestRepository = foundationTestRepository;
            this.taskPropertyRepository = taskPropertyRepository;
            this.taskPropertyLogRepository = taskPropertyLogRepository;
            this.reseauRepository = reseauRepository;
            this.areaRepository = areaRepository;
            this.sceneRepository = sceneRepository;
            this.placeCategoryRepository = placeCategoryRepository;
            this.planningRepository = planningRepository;
            this.addressingRepository = addressingRepository;
            this.remodelingRepository = remodelingRepository;
            this.operatorsSharingRepository = operatorsSharingRepository;
            this.customerRepository = customerRepository;
        }

        /// <summary>
        /// 根据任务Id获取任务
        /// </summary>
        /// <param name="id">任务Id</param>
        /// <returns>任务维护对象</returns>
        public ConstructionTaskMaintObject GetConstructionTaskById(Guid id)
        {
            ConstructionTask constructionTask = constructionTaskRepository.FindByKey(id);
            if (constructionTask != null)
            {
                ConstructionTaskMaintObject constructionTaskMaintObject = MapperHelper.Map<ConstructionTask, ConstructionTaskMaintObject>(constructionTask);
                Place place = placeRepository.GetByKey(constructionTask.PlaceId);
                constructionTaskMaintObject.PlaceName = place.PlaceName;
                constructionTaskMaintObject.ConstructionProgress = (int)constructionTask.ConstructionProgress;
                constructionTaskMaintObject.ProgressMemos = constructionTask.ProgressMemos;
                return constructionTaskMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的任务在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改任务
        /// </summary>
        /// <param name="constructionTaskMaintObject">要新增或者修改的任务对象</param>
        public void AddOrUpdateConstructionTask(ConstructionTaskMaintObject constructionTaskMaintObject)
        {
            if (constructionTaskMaintObject.Id == Guid.Empty)
            {
                ConstructionTask constructionTask = AggregateFactory.CreateConstructionTask((ConstructionMethod)constructionTaskMaintObject.ConstructionMethod, constructionTaskMaintObject.PlaceId, constructionTaskMaintObject.ProjectId,
                    constructionTaskMaintObject.ProjectManagerId, constructionTaskMaintObject.SupervisorCustomerId, constructionTaskMaintObject.SupervisorUserId, (EngineeringProgress)constructionTaskMaintObject.ConstructionProgress, constructionTaskMaintObject.ProgressMemos);
                constructionTaskRepository.Add(constructionTask);
            }
            else
            {
                ConstructionTask constructionTask = constructionTaskRepository.FindByKey(constructionTaskMaintObject.Id);
                if (constructionTask != null)
                {
                    constructionTask.Modify((EngineeringProgress)constructionTaskMaintObject.ConstructionProgress, constructionTaskMaintObject.ProgressMemos);
                    constructionTaskRepository.Update(constructionTask);
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

        public void RemoveConstructionTask(IList<ConstructionTaskMaintObject> constructionTaskMaintObjects)
        {
            foreach (ConstructionTaskMaintObject constructionTaskMaintObject in constructionTaskMaintObjects)
            {
                ConstructionTask constructionTask = constructionTaskRepository.FindByKey(constructionTaskMaintObject.Id);
                if (constructionTask != null)
                {
                    //constructionTask.CheckByRemove(constructionTaskMaintObject.ModifyUserId);
                    constructionTaskRepository.Remove(constructionTask);
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

        public string GetConstructionPlanningsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid projectId, int constructionProgress, Guid projectManagerId, int constructionMethod)
        {
            List<Parameter> parameters = new List<Parameter>(13);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ProjectId", Type = SqlDbType.UniqueIdentifier, Value = projectId });
            parameters.Add(new Parameter() { Name = "ConstructionProgress", Type = SqlDbType.Int, Value = constructionProgress });
            parameters.Add(new Parameter() { Name = "ProjectManagerId", Type = SqlDbType.UniqueIdentifier, Value = projectManagerId });
            parameters.Add(new Parameter() { Name = "ConstructionMethod", Type = SqlDbType.Int, Value = constructionMethod });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryConstructionPlanningsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        public ConstructionTaskEditorObject GetConstructionPlanningById(Guid id, Guid placeId)
        {
            ConstructionTask constructionTask = constructionTaskRepository.FindByKey(id);
            Place place = placeRepository.FindByKey(placeId);
            PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == placeId));
            if (constructionTask != null)
            {
                ConstructionTaskEditorObject constructionTaskEditorObject = MapperHelper.Map<ConstructionTask, ConstructionTaskEditorObject>(constructionTask);
                //constructionTaskEditorObject.Id = id;
                //constructionTaskEditorObject.PlaceId = placeId;
                //constructionTaskEditorObject.PlaceName = place.PlaceName;
                //constructionTaskEditorObject.ConstructionProgress = (int)constructionTask.ConstructionProgress;
                //constructionTaskEditorObject.ProgressMemos = constructionTask.ProgressMemos;
                //constructionTaskEditorObject.TowerType = (int)placeProperty.TowerType;
                //constructionTaskEditorObject.TowerHeight = placeProperty.TowerHeight;
                //constructionTaskEditorObject.PlatFormNumber = placeProperty.PlatFormNumber;
                //constructionTaskEditorObject.PoleNumber = placeProperty.PoleNumber;
                //constructionTaskEditorObject.MachineRoomType = (int)placeProperty.MachineRoomType;
                //constructionTaskEditorObject.MachineRoomArea = placeProperty.MachineRoomArea;
                //constructionTaskEditorObject.ExternalElectric = (int)placeProperty.ExternalElectric;
                //constructionTaskEditorObject.SwitchPower = placeProperty.SwitchPower;
                //constructionTaskEditorObject.Battery = placeProperty.Battery;
                //constructionTaskEditorObject.AirConditioner = placeProperty.AirConditioner;
                //constructionTaskEditorObject.FireControl = (int)placeProperty.FireControl;
                //constructionTaskEditorObject.FileIdList = "";
                //FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == placeId && entity.EntityName == "Place"));
                //if (placeFileAssociation != null)
                //{
                //    constructionTaskEditorObject.FileIdList = placeFileAssociation.FileIdList;
                //    string[] strFileList = constructionTaskEditorObject.FileIdList.Split(',');
                //    int count = 0;
                //    foreach (string i in strFileList)
                //    {
                //        count += 1;
                //    }
                //    constructionTaskEditorObject.Count = count;
                //}
                //else
                //{
                //    constructionTaskEditorObject.Count = 0;
                //}
                return constructionTaskEditorObject;
            }
            else
            {
                throw new ApplicationFault("选择的建设任务在系统中不存在");
            }
        }

        public void SaveConstructionPlanning(ConstructionTaskEditorObject constructionTaskEditorObject)
        {
            ConstructionTask constructionTask = constructionTaskRepository.FindByKey(constructionTaskEditorObject.Id);
            PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == constructionTaskEditorObject.PlaceId));

            if (constructionTask != null)
            {
                constructionTask.Modify((EngineeringProgress)constructionTaskEditorObject.ConstructionProgress, constructionTaskEditorObject.ProgressMemos);
                constructionTaskRepository.Update(constructionTask);
            }
            //if (placeProperty != null)
            //{
            //    placeProperty.ModifyConstruction((TowerType)constructionTaskEditorObject.TowerType, constructionTaskEditorObject.TowerHeight,constructionTaskEditorObject.PlatFormNumber,
            //        constructionTaskEditorObject.PoleNumber,(MachineRoomType)constructionTaskEditorObject.MachineRoomType,constructionTaskEditorObject.MachineRoomArea,
            //        (ExternalElectric)constructionTaskEditorObject.ExternalElectric,constructionTaskEditorObject.SwitchPower,constructionTaskEditorObject.Battery,constructionTaskEditorObject.AirConditioner,
            //        (FireControl)constructionTaskEditorObject.FireControl);
            //    placePropertyRepository.Update(placeProperty);

            //    FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == constructionTaskEditorObject.PlaceId && entity.EntityName == "Place"));
            //    if (placeFileAssociation == null && constructionTaskEditorObject.FileIdList != "")
            //    {
            //        FileAssociation newPlaceFileAssociation = AggregateFactory.CreateFileAssociation("Place", constructionTaskEditorObject.PlaceId, constructionTaskEditorObject.FileIdList, constructionTaskEditorObject.ModifyUserId);
            //        fileAssociationRepository.Add(newPlaceFileAssociation);
            //    }
            //    else if (placeFileAssociation != null && constructionTaskEditorObject.FileIdList != placeFileAssociation.FileIdList)
            //    {
            //        placeFileAssociation.Modify(constructionTaskEditorObject.FileIdList, constructionTaskEditorObject.ModifyUserId);
            //        fileAssociationRepository.Update(placeFileAssociation);
            //    }
            //}
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetRegisterPlanningsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, int constructionProgress, int isFinish, Guid companyId, int constructionMethod)
        {
            List<Parameter> parameters = new List<Parameter>(13);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ConstructionProgress", Type = SqlDbType.Int, Value = constructionProgress });
            parameters.Add(new Parameter() { Name = "IsFinish", Type = SqlDbType.Int, Value = isFinish });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "ConstructionMethod", Type = SqlDbType.Int, Value = constructionMethod });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryRegisterPlanningsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        public string GetRegisterRemodeingsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, int constructionProgress, int isFinish, Guid companyId, int constructionMethod)
        {
            List<Parameter> parameters = new List<Parameter>(13);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ConstructionProgress", Type = SqlDbType.Int, Value = constructionProgress });
            parameters.Add(new Parameter() { Name = "IsFinish", Type = SqlDbType.Int, Value = isFinish });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "ConstructionMethod", Type = SqlDbType.Int, Value = constructionMethod });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryRegisterRemodeingsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        public PlacePropertyEditorObject GetRegisterPlanningById(Guid id, Guid constructionTaskId, Guid companyId)
        {
            ConstructionTask constructionTask = constructionTaskRepository.FindByKey(constructionTaskId);
            PlaceProperty placeProperty = placePropertyRepository.FindByKey(id);
            if (constructionTask != null)
            {
                PlacePropertyEditorObject placePropertyEditorObject = MapperHelper.Map<PlaceProperty, PlacePropertyEditorObject>(placeProperty);
                placePropertyEditorObject.Id = placeProperty.Id;
                placePropertyEditorObject.ConstructionTaskId = constructionTaskId;
                if (companyId.ToString() == "6365f3de-0fc5-4930-a321-2350ee6269bb")
                {
                    if (placeProperty.MobileCreateUserId.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        User user = userRepository.GetByKey(placeProperty.MobileCreateUserId.Value);
                        placePropertyEditorObject.CreateFullName = user.FullName;
                    }
                    else
                    {
                        placePropertyEditorObject.CreateFullName = "";
                    }
                    placePropertyEditorObject.PoleNumber = placeProperty.MobilePoleNumber;
                    placePropertyEditorObject.CabinetNumber = placeProperty.MobileCabinetNumber;
                    placePropertyEditorObject.PowerUsed = placeProperty.MobilePowerUsed;
                    placePropertyEditorObject.IsFinish = (int)constructionTask.IsFinishMobile;
                    placePropertyEditorObject.CreateDate = placeProperty.MobileCreateDate.ToShortDateString();
                }
                else if (companyId.ToString() == "2e0ffe5f-c03a-4767-9915-9683f0db0b53")
                {
                    if (placeProperty.TelecomCreateUserId.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        User user = userRepository.GetByKey(placeProperty.TelecomCreateUserId.Value);
                        placePropertyEditorObject.CreateFullName = user.FullName;
                    }
                    else
                    {
                        placePropertyEditorObject.CreateFullName = "";
                    }
                    placePropertyEditorObject.PoleNumber = placeProperty.TelecomPoleNumber;
                    placePropertyEditorObject.CabinetNumber = placeProperty.TelecomCabinetNumber;
                    placePropertyEditorObject.PowerUsed = placeProperty.TelecomPowerUsed;
                    placePropertyEditorObject.IsFinish = (int)constructionTask.IsFinishTelecom;
                    placePropertyEditorObject.CreateDate = placeProperty.TelecomCreateDate.ToShortDateString();
                }
                else if (companyId.ToString() == "0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600")
                {
                    if (placeProperty.UnicomCreateUserId.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        User user = userRepository.GetByKey(placeProperty.UnicomCreateUserId.Value);
                        placePropertyEditorObject.CreateFullName = user.FullName;
                    }
                    else
                    {
                        placePropertyEditorObject.CreateFullName = "";
                    }
                    placePropertyEditorObject.PoleNumber = placeProperty.UnicomPoleNumber;
                    placePropertyEditorObject.CabinetNumber = placeProperty.UnicomCabinetNumber;
                    placePropertyEditorObject.PowerUsed = placeProperty.UnicomPowerUsed;
                    placePropertyEditorObject.IsFinish = (int)constructionTask.IsFinishUnicom;
                    placePropertyEditorObject.CreateDate = placeProperty.UnicomCreateDate.ToShortDateString();
                }
                return placePropertyEditorObject;
            }
            else
            {
                throw new ApplicationFault("选择的任务在系统中不存在");
            }
        }

        public void SaveRegisterPlanning(PlacePropertyEditorObject placePropertyEditorObject)
        {
            PlaceProperty placeProperty = placePropertyRepository.FindByKey(placePropertyEditorObject.Id);
            ConstructionTask constructionTask = constructionTaskRepository.FindByKey(placePropertyEditorObject.ConstructionTaskId);
            Place place = placeRepository.FindByKey(constructionTask.PlaceId);
            if (placeProperty != null)
            {
                if (placePropertyEditorObject.CompanyId.ToString() == "6365f3de-0fc5-4930-a321-2350ee6269bb")
                {
                    constructionTask.ModifyMobile((Bool)placePropertyEditorObject.IsFinish);
                    constructionTaskRepository.Update(constructionTask);
                    if (placePropertyEditorObject.IsFinish == 1)
                    {
                        place.OperatorShared(1, Bool.是);
                        placeRepository.Update(place);
                    }
                    placeProperty.ModifyMobile(placeProperty.MobileShare, placePropertyEditorObject.PoleNumber, placePropertyEditorObject.CabinetNumber, placePropertyEditorObject.PowerUsed, placePropertyEditorObject.CreateUserId);
                    PlacePropertyLog placePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.移动, placeProperty.MobileShare, placePropertyEditorObject.PoleNumber, placePropertyEditorObject.CabinetNumber, placePropertyEditorObject.PowerUsed, placePropertyEditorObject.CreateUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                    placePropertyLogRepository.Add(placePropertyLog);
                }
                else if (placePropertyEditorObject.CompanyId.ToString() == "2e0ffe5f-c03a-4767-9915-9683f0db0b53")
                {
                    constructionTask.ModifyTelecom((Bool)placePropertyEditorObject.IsFinish);
                    constructionTaskRepository.Update(constructionTask);
                    if (placePropertyEditorObject.IsFinish == 1)
                    {
                        place.OperatorShared(2, Bool.是);
                        placeRepository.Update(place);
                    }
                    placeProperty.ModifyTelecom(placeProperty.TelecomShare, placePropertyEditorObject.PoleNumber, placePropertyEditorObject.CabinetNumber, placePropertyEditorObject.PowerUsed, placePropertyEditorObject.CreateUserId);
                    PlacePropertyLog placePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.电信, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, placeProperty.TelecomShare, placePropertyEditorObject.PoleNumber, placePropertyEditorObject.CabinetNumber, placePropertyEditorObject.PowerUsed, placePropertyEditorObject.CreateUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                    placePropertyLogRepository.Add(placePropertyLog);
                }
                else if (placePropertyEditorObject.CompanyId.ToString() == "0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600")
                {
                    constructionTask.ModifyUnicom((Bool)placePropertyEditorObject.IsFinish);
                    constructionTaskRepository.Update(constructionTask);
                    if (placePropertyEditorObject.IsFinish == 1)
                    {
                        place.OperatorShared(3, Bool.是);
                        placeRepository.Update(place);
                    }
                    placeProperty.ModifyUnicom(placeProperty.UnicomShare, placePropertyEditorObject.PoleNumber, placePropertyEditorObject.CabinetNumber, placePropertyEditorObject.PowerUsed, placePropertyEditorObject.CreateUserId);
                    PlacePropertyLog placePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.联通, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, placeProperty.UnicomShare, placePropertyEditorObject.PoleNumber, placePropertyEditorObject.CabinetNumber, placePropertyEditorObject.PowerUsed, placePropertyEditorObject.CreateUserId);
                    placePropertyLogRepository.Add(placePropertyLog);
                }
                placePropertyRepository.Update(placeProperty);
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

        public string GetConstructionTasksPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid customerId, int constructionProgress, Guid userId)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "CustomerId", Type = SqlDbType.UniqueIdentifier, Value = customerId });
            parameters.Add(new Parameter() { Name = "ConstructionProgress", Type = SqlDbType.Int, Value = constructionProgress });
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryConstructionTasksPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        public void SaveConstructionTaskProgress(ConstructionTaskEditorObject constructionTaskEditorObject)
        {
            ConstructionTask constructionTask = constructionTaskRepository.FindByKey(constructionTaskEditorObject.Id);
            if (constructionTask != null)
            {
                Project project = projectRepository.FindByKey(constructionTask.ProjectId);
                if (constructionTaskEditorObject.ConstructionProgress != (int)constructionTask.ConstructionProgress)
                {
                    if (constructionTaskEditorObject.ConstructionProgress == (int)EngineeringProgress.已完工)
                    {
                        project.ModifyProjectProgress(1);
                    }
                    else
                    {
                        project.ModifyProjectProgress(2);
                    }
                    projectRepository.Update(project);
                }
                constructionTask.Modify((EngineeringProgress)constructionTaskEditorObject.ConstructionProgress, constructionTaskEditorObject.ProgressMemos);
                constructionTaskRepository.Update(constructionTask);
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

        public string GetTaskPropertysPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, Guid customerId, int constructionProgress, int taskModel, Guid supervisorCustomerId, Guid userId)
        {
            List<Parameter> parameters = new List<Parameter>(11);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "CustomerId", Type = SqlDbType.UniqueIdentifier, Value = customerId });
            parameters.Add(new Parameter() { Name = "ConstructionProgress", Type = SqlDbType.Int, Value = constructionProgress });
            parameters.Add(new Parameter() { Name = "TaskModel", Type = SqlDbType.Int, Value = taskModel });
            parameters.Add(new Parameter() { Name = "SupervisorCustomerId", Type = SqlDbType.UniqueIdentifier, Value = supervisorCustomerId });
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryTaskPropertysPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        public ConstructionTaskPrintObject GetConstructionTaskCardById(Guid id)
        {
            ConstructionTask constructionTask = constructionTaskRepository.FindByKey(id);
            if (constructionTask != null)
            {
                ConstructionTaskPrintObject constructionTaskPrintObject = new ConstructionTaskPrintObject();
                Place place = placeRepository.FindByKey(constructionTask.PlaceId);
                PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == constructionTask.Id && entity.PropertyType == PropertyType.任务设计));
                Reseau reseau = reseauRepository.GetByKey(place.ReseauId);
                Area area = areaRepository.GetByKey(reseau.AreaId);
                PlaceCategory placeCategory = placeCategoryRepository.GetByKey(place.PlaceCategoryId);
                User userPlace = userRepository.FindByKey(place.CreateUserId);

                constructionTaskPrintObject.PlaceId = place.Id;
                constructionTaskPrintObject.PlaceName = place.PlaceName;
                constructionTaskPrintObject.AreaName = area.AreaName;
                constructionTaskPrintObject.ReseauName = reseau.ReseauName;
                constructionTaskPrintObject.PlaceCategoryName = placeCategory.PlaceCategoryName;
                constructionTaskPrintObject.ImportanceName = EnumHelper.GetEnumText(typeof(Importance), place.Importance);
                constructionTaskPrintObject.Lat = place.Lat;
                constructionTaskPrintObject.Lng = place.Lng;
                //constructionTaskPrintObject.SceneName = scene.SceneName;
                //constructionTaskPrintObject.PropertyRightName = EnumHelper.GetEnumText(typeof(PlaceOwner), place.PropertyRight);
                constructionTaskPrintObject.OwnerName = place.OwnerName;
                constructionTaskPrintObject.OwnerContact = place.OwnerContact;
                constructionTaskPrintObject.OwnerPhoneNumber = place.OwnerPhoneNumber;
                //constructionTaskPrintObject.TelecomDemandName = EnumHelper.GetEnumText(typeof(Bool), place.TelecomShare);
                //constructionTaskPrintObject.MobileDemandName = EnumHelper.GetEnumText(typeof(Bool), place.MobileShare);
                //constructionTaskPrintObject.UnicomDemandName = EnumHelper.GetEnumText(typeof(Bool), place.UnicomShare);
                constructionTaskPrintObject.DetailedAddress = place.DetailedAddress;
                constructionTaskPrintObject.Remarks = place.Remarks;
                constructionTaskPrintObject.CreateUserName = userPlace.FullName;
                constructionTaskPrintObject.CreateDate = place.CreateDate.ToShortDateString();

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
                    constructionTaskPrintObject.Count = count;
                }
                else
                {
                    constructionTaskPrintObject.Count = 0;
                }
                constructionTaskPrintObject.Id = constructionTask.Id;

                constructionTaskPrintObject.MobileShare = 2;
                constructionTaskPrintObject.TelecomShare = 2;
                constructionTaskPrintObject.UnicomShare = 2;
                if (constructionTask.ConstructionMethod == ConstructionMethod.新建)
                {
                    Planning planning = planningRepository.Find(Specification<Planning>.Eval(entity => entity.PlaceId == constructionTask.PlaceId));
                    Addressing addressing = addressingRepository.Find(Specification<Addressing>.Eval(entity => entity.PlanningId == planning.Id));
                    constructionTaskPrintObject.MobileShare = 1;
                    constructionTaskPrintObject.TelecomShare = 1;
                    constructionTaskPrintObject.UnicomShare = 1;
                }
                else
                {
                    Remodeling remodeling = remodelingRepository.Find(Specification<Remodeling>.Eval(entity => entity.PlaceId == constructionTask.PlaceId));
                    IEnumerable<OperatorsSharing> operatorsSharings = operatorsSharingRepository.FindAll(Specification<OperatorsSharing>.Eval(entity => entity.RemodelingId == remodeling.Id));
                    if (operatorsSharings != null)
                    {
                        foreach (var operatorsSharing in operatorsSharings)
                        {
                            if (operatorsSharing.CompanyId.ToString() == "6365f3de-0fc5-4930-a321-2350ee6269bb")
                            {
                                constructionTaskPrintObject.MobileShare = 1;
                            }
                            if (operatorsSharing.CompanyId.ToString() == "2e0ffe5f-c03a-4767-9915-9683f0db0b53")
                            {
                                constructionTaskPrintObject.TelecomShare = 1;
                            }
                            if (operatorsSharing.CompanyId.ToString() == "0b2a7f2d-b623-4ef2-a89d-ff4eab0a1600")
                            {
                                constructionTaskPrintObject.UnicomShare = 1;
                            }
                        }
                    }
                }
                if (placeProperty != null)
                {
                    constructionTaskPrintObject.TelecomPoleNumber = placeProperty.TelecomPoleNumber;
                    constructionTaskPrintObject.TelecomCabinetNumber = placeProperty.TelecomCabinetNumber;
                    constructionTaskPrintObject.TelecomPowerUsed = placeProperty.TelecomPowerUsed;
                    constructionTaskPrintObject.IsFinishMobile = EnumHelper.GetEnumText(typeof(Bool), constructionTask.IsFinishMobile);
                    if (placeProperty.MobileCreateUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(placeProperty.MobileCreateUserId.Value);
                        constructionTaskPrintObject.MobileFullName = user.FullName;
                        constructionTaskPrintObject.MobileModifyDate = placeProperty.MobileCreateDate.ToShortDateString();
                    }
                    else
                    {
                        constructionTaskPrintObject.MobileFullName = "";
                        constructionTaskPrintObject.MobileModifyDate = "";
                    }
                    constructionTaskPrintObject.MobilePoleNumber = placeProperty.MobilePoleNumber;
                    constructionTaskPrintObject.MobileCabinetNumber = placeProperty.MobileCabinetNumber;
                    constructionTaskPrintObject.MobilePowerUsed = placeProperty.MobilePowerUsed;
                    constructionTaskPrintObject.IsFinishTelecom = EnumHelper.GetEnumText(typeof(Bool), constructionTask.IsFinishTelecom);
                    if (placeProperty.TelecomCreateUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(placeProperty.TelecomCreateUserId.Value);
                        constructionTaskPrintObject.TelecomFullName = user.FullName;
                        constructionTaskPrintObject.TelecomModifyDate = placeProperty.TelecomCreateDate.ToShortDateString();
                    }
                    else
                    {
                        constructionTaskPrintObject.TelecomFullName = "";
                        constructionTaskPrintObject.TelecomModifyDate = "";
                    }
                    constructionTaskPrintObject.UnicomPoleNumber = placeProperty.UnicomPoleNumber;
                    constructionTaskPrintObject.UnicomCabinetNumber = placeProperty.UnicomCabinetNumber;
                    constructionTaskPrintObject.UnicomPowerUsed = placeProperty.UnicomPowerUsed;
                    constructionTaskPrintObject.IsFinishUnicom = EnumHelper.GetEnumText(typeof(Bool), constructionTask.IsFinishUnicom);
                    if (placeProperty.UnicomCreateUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(placeProperty.UnicomCreateUserId.Value);
                        constructionTaskPrintObject.UnicomFullName = user.FullName;
                        constructionTaskPrintObject.UnicomModifyDate = placeProperty.UnicomCreateDate.ToShortDateString();
                    }
                    else
                    {
                        constructionTaskPrintObject.UnicomFullName = "";
                        constructionTaskPrintObject.UnicomModifyDate = "";
                    }
                }
                else
                {
                    constructionTaskPrintObject.TelecomPoleNumber = 0;
                    constructionTaskPrintObject.TelecomCabinetNumber = 0;
                    constructionTaskPrintObject.TelecomPowerUsed = 0;
                    constructionTaskPrintObject.IsFinishTelecom = "";
                    constructionTaskPrintObject.TelecomFullName = "";
                    constructionTaskPrintObject.TelecomModifyDate = "";
                    constructionTaskPrintObject.MobilePoleNumber = 0;
                    constructionTaskPrintObject.MobileCabinetNumber = 0;
                    constructionTaskPrintObject.MobilePowerUsed = 0;
                    constructionTaskPrintObject.IsFinishMobile = "";
                    constructionTaskPrintObject.MobileFullName = "";
                    constructionTaskPrintObject.MobileModifyDate = "";
                    constructionTaskPrintObject.UnicomPoleNumber = 0;
                    constructionTaskPrintObject.UnicomCabinetNumber = 0;
                    constructionTaskPrintObject.UnicomPowerUsed = 0;
                    constructionTaskPrintObject.IsFinishUnicom = "";
                    constructionTaskPrintObject.UnicomFullName = "";
                    constructionTaskPrintObject.UnicomModifyDate = "";
                }
                return constructionTaskPrintObject;
            }
            else
            {
                throw new ApplicationFault("选择的任务在系统中不存在");
            }
        }

        public ResourceUpdateObject GetResourceUpdatePrint(Guid id)
        {
            ConstructionTask constructionTask = constructionTaskRepository.FindByKey(id);
            if (constructionTask != null)
            {
                ResourceUpdateObject resourceUpdateObject = new ResourceUpdateObject();
                Place place = placeRepository.FindByKey(constructionTask.PlaceId);
                PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == constructionTask.Id && entity.PropertyType == PropertyType.任务设计));

                Tower tower = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == constructionTask.Id && entity.PropertyType == PropertyType.任务设计));
                if (tower != null)
                {
                    resourceUpdateObject.TowerIdPrint = tower.Id;
                    resourceUpdateObject.TowerTypePrint = EnumHelper.GetEnumText(typeof(TowerType), tower.TowerType);
                    resourceUpdateObject.TowerHeightPrint = tower.TowerHeight;
                    resourceUpdateObject.PlatFormNumberPrint = tower.PlatFormNumber;
                    resourceUpdateObject.PoleNumberPrint = tower.PoleNumber;
                    if (tower.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(tower.ModifyUserId);
                        resourceUpdateObject.TowerFullNamePrint = user.FullName;
                    }
                    else
                    {
                        resourceUpdateObject.TowerFullNamePrint = "";
                    }
                    resourceUpdateObject.TowerModifyDatePrint = tower.ModifyDate.ToShortDateString();

                    FileAssociation towerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"));
                    if (towerFileAssociation != null)
                    {
                        int count = 0;
                        if (towerFileAssociation.FileIdList != "")
                        {
                            if (towerFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = towerFileAssociation.FileIdList.Split(',');
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
                        resourceUpdateObject.TowerCountPrint = count;
                    }
                    else
                    {
                        resourceUpdateObject.TowerCountPrint = 0;
                    }
                }
                else
                {
                    resourceUpdateObject.TowerIdPrint = Guid.Empty;
                    resourceUpdateObject.TowerTypePrint = "";
                    resourceUpdateObject.TowerHeightPrint = 0;
                    resourceUpdateObject.PlatFormNumberPrint = 0;
                    resourceUpdateObject.PoleNumberPrint = 0;
                    resourceUpdateObject.TowerCountPrint = 0;
                    resourceUpdateObject.TowerFullNamePrint = "";
                    resourceUpdateObject.TowerModifyDatePrint = "";
                }

                TowerBase towerBase = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == constructionTask.Id && entity.PropertyType == PropertyType.任务设计));
                if (towerBase != null)
                {
                    resourceUpdateObject.TowerBaseIdPrint = towerBase.Id;
                    resourceUpdateObject.TowerBaseTypePrint = EnumHelper.GetEnumText(typeof(TowerBaseType), towerBase.TowerBaseType);
                    if (towerBase.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(towerBase.ModifyUserId);
                        resourceUpdateObject.TowerBaseFullNamePrint = user.FullName;
                    }
                    else
                    {
                        resourceUpdateObject.TowerBaseFullNamePrint = "";
                    }
                    resourceUpdateObject.TowerBaseModifyDatePrint = towerBase.ModifyDate.ToShortDateString();

                    FileAssociation towerBaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"));
                    if (towerBaseFileAssociation != null)
                    {
                        int count = 0;
                        if (towerBaseFileAssociation.FileIdList != "")
                        {
                            if (towerBaseFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = towerBaseFileAssociation.FileIdList.Split(',');
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
                        resourceUpdateObject.TowerBaseCountPrint = count;
                    }
                    else
                    {
                        resourceUpdateObject.TowerBaseCountPrint = 0;
                    }
                }
                else
                {
                    resourceUpdateObject.TowerBaseIdPrint = Guid.Empty;
                    resourceUpdateObject.TowerBaseTypePrint = "";
                    resourceUpdateObject.TowerBaseCountPrint = 0;
                    resourceUpdateObject.TowerBaseFullNamePrint = "";
                    resourceUpdateObject.TowerBaseModifyDatePrint = "";
                }

                MachineRoom machineRoom = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == constructionTask.Id && entity.PropertyType == PropertyType.任务设计));
                if (machineRoom != null)
                {
                    resourceUpdateObject.MachineRoomIdPrint = machineRoom.Id;
                    resourceUpdateObject.MachineRoomTypePrint = EnumHelper.GetEnumText(typeof(MachineRoomType), machineRoom.MachineRoomType);
                    resourceUpdateObject.MachineRoomAreaPrint = machineRoom.MachineRoomArea;
                    if (machineRoom.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(machineRoom.ModifyUserId);
                        resourceUpdateObject.MachineRoomFullNamePrint = user.FullName;
                    }
                    else
                    {
                        resourceUpdateObject.MachineRoomFullNamePrint = "";
                    }
                    resourceUpdateObject.MachineRoomModifyDatePrint = machineRoom.ModifyDate.ToShortDateString();

                    FileAssociation machineRoomFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"));
                    if (machineRoomFileAssociation != null)
                    {
                        int count = 0;
                        if (machineRoomFileAssociation.FileIdList != "")
                        {
                            if (machineRoomFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = machineRoomFileAssociation.FileIdList.Split(',');
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
                        resourceUpdateObject.MachineRoomCountPrint = count;
                    }
                    else
                    {
                        resourceUpdateObject.MachineRoomCountPrint = 0;
                    }
                }
                else
                {
                    resourceUpdateObject.MachineRoomIdPrint = Guid.Empty;
                    resourceUpdateObject.MachineRoomTypePrint = "";
                    resourceUpdateObject.MachineRoomAreaPrint = 0;
                    resourceUpdateObject.MachineRoomCountPrint = 0;
                    resourceUpdateObject.MachineRoomFullNamePrint = "";
                    resourceUpdateObject.MachineRoomModifyDatePrint = "";
                }

                ExternalElectricPower externalElectricPower = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == constructionTask.Id && entity.PropertyType == PropertyType.任务设计));
                if (externalElectricPower != null)
                {
                    resourceUpdateObject.ExternalElectricPowerIdPrint = externalElectricPower.Id;
                    resourceUpdateObject.ExternalElectricPrint = EnumHelper.GetEnumText(typeof(ExternalElectric), externalElectricPower.ExternalElectric);
                    if (externalElectricPower.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(externalElectricPower.ModifyUserId);
                        resourceUpdateObject.ExternalFullNamePrint = user.FullName;
                    }
                    else
                    {
                        resourceUpdateObject.ExternalFullNamePrint = "";
                    }
                    resourceUpdateObject.ExternalModifyDatePrint = externalElectricPower.ModifyDate.ToShortDateString();

                    FileAssociation externalElectricPowerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"));
                    if (externalElectricPowerFileAssociation != null)
                    {
                        int count = 0;
                        if (externalElectricPowerFileAssociation.FileIdList != "")
                        {
                            if (externalElectricPowerFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = externalElectricPowerFileAssociation.FileIdList.Split(',');
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
                        resourceUpdateObject.ExternalCountPrint = count;
                    }
                    else
                    {
                        resourceUpdateObject.ExternalCountPrint = 0;
                    }
                }
                else
                {
                    resourceUpdateObject.ExternalElectricPowerIdPrint = Guid.Empty;
                    resourceUpdateObject.ExternalElectricPrint = "";
                    resourceUpdateObject.ExternalCountPrint = 0;
                    resourceUpdateObject.ExternalFullNamePrint = "";
                    resourceUpdateObject.ExternalModifyDatePrint = "";
                }

                EquipmentInstall equipmentInstall = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == constructionTask.Id && entity.PropertyType == PropertyType.任务设计));
                if (equipmentInstall != null)
                {
                    resourceUpdateObject.EquipmentInstallIdPrint = equipmentInstall.Id;
                    resourceUpdateObject.SwitchPowerPrint = equipmentInstall.SwitchPower;
                    resourceUpdateObject.BatteryPrint = equipmentInstall.Battery;
                    resourceUpdateObject.CabinetNumberPrint = equipmentInstall.CabinetNumber;
                    if (equipmentInstall.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(equipmentInstall.ModifyUserId);
                        resourceUpdateObject.EquipmentFullNamePrint = user.FullName;
                    }
                    else
                    {
                        resourceUpdateObject.EquipmentFullNamePrint = "";
                    }
                    resourceUpdateObject.EquipmentModifyDatePrint = equipmentInstall.ModifyDate.ToShortDateString();

                    FileAssociation EquipmentInstallFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"));
                    if (EquipmentInstallFileAssociation != null)
                    {
                        int count = 0;
                        if (EquipmentInstallFileAssociation.FileIdList != "")
                        {
                            if (EquipmentInstallFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = EquipmentInstallFileAssociation.FileIdList.Split(',');
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
                        resourceUpdateObject.EquipmentInstallCountPrint = count;
                    }
                    else
                    {
                        resourceUpdateObject.EquipmentInstallCountPrint = 0;
                    }
                }
                else
                {
                    resourceUpdateObject.EquipmentInstallIdPrint = Guid.Empty;
                    resourceUpdateObject.SwitchPowerPrint = 0;
                    resourceUpdateObject.BatteryPrint = 0;
                    resourceUpdateObject.CabinetNumberPrint = 0;
                    resourceUpdateObject.EquipmentInstallCountPrint = 0;
                    resourceUpdateObject.EquipmentFullNamePrint = "";
                    resourceUpdateObject.EquipmentModifyDatePrint = "";
                }

                AddressExplor addressExplor = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == constructionTask.Id && entity.PropertyType == PropertyType.任务设计));
                if (addressExplor != null)
                {
                    resourceUpdateObject.AddressExplorIdPrint = addressExplor.Id;
                    if (addressExplor.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(addressExplor.ModifyUserId);
                        resourceUpdateObject.AddressFullNamePrint = user.FullName;
                    }
                    else
                    {
                        resourceUpdateObject.AddressFullNamePrint = "";
                    }
                    resourceUpdateObject.AddressModifyDatePrint = addressExplor.ModifyDate.ToShortDateString();

                    FileAssociation AddressExplorFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"));
                    if (AddressExplorFileAssociation != null)
                    {
                        int count = 0;
                        if (AddressExplorFileAssociation.FileIdList != "")
                        {
                            if (AddressExplorFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = AddressExplorFileAssociation.FileIdList.Split(',');
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
                        resourceUpdateObject.AddressCountPrint = count;
                    }
                    else
                    {
                        resourceUpdateObject.AddressCountPrint = 0;
                    }
                }
                else
                {
                    resourceUpdateObject.AddressExplorIdPrint = Guid.Empty;
                    resourceUpdateObject.AddressCountPrint = 0;
                    resourceUpdateObject.AddressFullNamePrint = "";
                    resourceUpdateObject.AddressModifyDatePrint = "";
                }

                FoundationTest foundationTest = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == constructionTask.Id && entity.PropertyType == PropertyType.任务设计));
                if (foundationTest != null)
                {
                    resourceUpdateObject.FoundationTestIdPrint = foundationTest.Id;
                    if (foundationTest.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(foundationTest.ModifyUserId);
                        resourceUpdateObject.FoundationFullNamePrint = user.FullName;
                    }
                    else
                    {
                        resourceUpdateObject.FoundationFullNamePrint = "";
                    }
                    resourceUpdateObject.FoundationModifyDatePrint = foundationTest.ModifyDate.ToShortDateString();

                    FileAssociation foundationTestFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"));
                    if (foundationTestFileAssociation != null)
                    {
                        int count = 0;
                        if (foundationTestFileAssociation.FileIdList != "")
                        {
                            if (foundationTestFileAssociation.FileIdList.Contains(","))
                            {
                                string[] strFileList = foundationTestFileAssociation.FileIdList.Split(',');
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
                        resourceUpdateObject.FoundationCountPrint = count;
                    }
                    else
                    {
                        resourceUpdateObject.FoundationCountPrint = 0;
                    }
                }
                else
                {
                    resourceUpdateObject.FoundationTestIdPrint = Guid.Empty;
                    resourceUpdateObject.FoundationCountPrint = 0;
                    resourceUpdateObject.FoundationFullNamePrint = "";
                    resourceUpdateObject.FoundationModifyDatePrint = "";
                }

                if (placeProperty != null)
                {
                    resourceUpdateObject.TelecomSharePrint = (int)placeProperty.TelecomShare;
                    resourceUpdateObject.TelecomPoleNumberPrint = placeProperty.TelecomPoleNumber;
                    resourceUpdateObject.TelecomCabinetNumberPrint = placeProperty.TelecomCabinetNumber;
                    resourceUpdateObject.TelecomPowerUsedPrint = placeProperty.TelecomPowerUsed;
                    resourceUpdateObject.IsFinishTelecomPrint = EnumHelper.GetEnumText(typeof(Bool), constructionTask.IsFinishTelecom);
                    if (placeProperty.TelecomCreateUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(placeProperty.TelecomCreateUserId.Value);
                        resourceUpdateObject.TelecomFullNamePrint = user.FullName;
                        resourceUpdateObject.TelecomModifyDatePrint = placeProperty.TelecomCreateDate.ToShortDateString();
                    }
                    else
                    {
                        resourceUpdateObject.TelecomFullNamePrint = "";
                        resourceUpdateObject.TelecomModifyDatePrint = "";
                    }

                    resourceUpdateObject.MobileSharePrint = (int)placeProperty.MobileShare;
                    resourceUpdateObject.MobilePoleNumberPrint = placeProperty.MobilePoleNumber;
                    resourceUpdateObject.MobileCabinetNumberPrint = placeProperty.MobileCabinetNumber;
                    resourceUpdateObject.MobilePowerUsedPrint = placeProperty.MobilePowerUsed;
                    resourceUpdateObject.IsFinishMobilePrint = EnumHelper.GetEnumText(typeof(Bool), constructionTask.IsFinishMobile);
                    if (placeProperty.MobileCreateUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(placeProperty.MobileCreateUserId.Value);
                        resourceUpdateObject.MobileFullNamePrint = user.FullName;
                        resourceUpdateObject.MobileModifyDatePrint = placeProperty.MobileCreateDate.ToShortDateString();
                    }
                    else
                    {
                        resourceUpdateObject.MobileFullNamePrint = "";
                        resourceUpdateObject.MobileModifyDatePrint = "";
                    }

                    resourceUpdateObject.UnicomSharePrint = (int)placeProperty.UnicomShare;
                    resourceUpdateObject.UnicomPoleNumberPrint = placeProperty.UnicomPoleNumber;
                    resourceUpdateObject.UnicomCabinetNumberPrint = placeProperty.UnicomCabinetNumber;
                    resourceUpdateObject.UnicomPowerUsedPrint = placeProperty.UnicomPowerUsed;
                    resourceUpdateObject.IsFinishUnicomPrint = EnumHelper.GetEnumText(typeof(Bool), constructionTask.IsFinishUnicom);
                    if (placeProperty.UnicomCreateUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(placeProperty.UnicomCreateUserId.Value);
                        resourceUpdateObject.UnicomFullNamePrint = user.FullName;
                        resourceUpdateObject.UnicomModifyDatePrint = placeProperty.UnicomCreateDate.ToShortDateString();
                    }
                    else
                    {
                        resourceUpdateObject.UnicomFullNamePrint = "";
                        resourceUpdateObject.UnicomModifyDatePrint = "";
                    }
                }
                else
                {
                    resourceUpdateObject.TelecomSharePrint = 2;
                    resourceUpdateObject.TelecomPoleNumberPrint = 0;
                    resourceUpdateObject.TelecomCabinetNumberPrint = 0;
                    resourceUpdateObject.TelecomPowerUsedPrint = 0;
                    resourceUpdateObject.IsFinishTelecomPrint = "";
                    resourceUpdateObject.TelecomFullNamePrint = "";
                    resourceUpdateObject.TelecomModifyDatePrint = "";
                    resourceUpdateObject.MobileSharePrint = 2;
                    resourceUpdateObject.MobilePoleNumberPrint = 0;
                    resourceUpdateObject.MobileCabinetNumberPrint = 0;
                    resourceUpdateObject.MobilePowerUsedPrint = 0;
                    resourceUpdateObject.IsFinishMobilePrint = "";
                    resourceUpdateObject.MobileFullNamePrint = "";
                    resourceUpdateObject.MobileModifyDatePrint = "";
                    resourceUpdateObject.UnicomSharePrint = 2;
                    resourceUpdateObject.UnicomPoleNumberPrint = 0;
                    resourceUpdateObject.UnicomCabinetNumberPrint = 0;
                    resourceUpdateObject.UnicomPowerUsedPrint = 0;
                    resourceUpdateObject.IsFinishUnicomPrint = "";
                    resourceUpdateObject.UnicomFullNamePrint = "";
                    resourceUpdateObject.UnicomModifyDatePrint = "";
                }
                return resourceUpdateObject;
            }
            else
            {
                throw new ApplicationFault("选择的任务在系统中不存在");
            }
        }

        public string GetProjectInformationPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string propertyRightSql, string groupPlaceCode, string placeName, Guid areaId, Guid reseauId, int constructionMethod, int constructionProgress)
        {
            List<Parameter> parameters = new List<Parameter>(11);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PropertyRightSql", Type = SqlDbType.NVarChar, Value = propertyRightSql });
            parameters.Add(new Parameter() { Name = "GroupPlaceCode", Type = SqlDbType.NVarChar, Value = groupPlaceCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ConstructionMethod", Type = SqlDbType.Int, Value = constructionMethod });
            parameters.Add(new Parameter() { Name = "ConstructionProgress", Type = SqlDbType.Int, Value = constructionProgress });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryProjectInformationPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 设置工程经理
        /// </summary>
        /// <param name="constructionTaskMaintObjects">要设置工程经理的任务维护对象列表</param>
        public void SettingProjectManager(IList<ConstructionTaskMaintObject> constructionTaskMaintObjects)
        {
            foreach (ConstructionTaskMaintObject constructionTaskMaintObject in constructionTaskMaintObjects)
            {
                ConstructionTask constructionTask = constructionTaskRepository.FindByKey(constructionTaskMaintObject.Id);
                if (constructionTask != null)
                {
                    constructionTask.SettingProjectManager(constructionTaskMaintObject.ProjectManagerId);
                    constructionTaskRepository.Update(constructionTask);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_User_AddressingUserId"))
                //{
                //    throw new ApplicationFault("选择的租赁人在系统中不存在");
                //}
                throw ex;
            }
        }

        /// <summary>
        /// 设置监理单位
        /// </summary>
        /// <param name="constructionTaskMaintObjects">要设置监理单位的任务维护对象列表</param>
        public void SettingSupervisorCustomer(IList<ConstructionTaskMaintObject> constructionTaskMaintObjects)
        {
            foreach (ConstructionTaskMaintObject constructionTaskMaintObject in constructionTaskMaintObjects)
            {
                Customer supervisorCustomer = customerRepository.FindByKey(constructionTaskMaintObject.SupervisorCustomerId);
                if (supervisorCustomer != null)
                {
                    if (supervisorCustomer.CustomerUserId == Guid.Empty)
                    {
                        throw new ApplicationFault("选择的监理单位还未关联登陆人");
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的监理单位在系统中不存在");
                }
                ConstructionTask constructionTask = constructionTaskRepository.FindByKey(constructionTaskMaintObject.Id);
                if (constructionTask != null && supervisorCustomer != null)
                {
                    constructionTask.SettingSupervisorCustomer(constructionTaskMaintObject.SupervisorCustomerId, supervisorCustomer.CustomerUserId);
                    constructionTaskRepository.Update(constructionTask);
                    IEnumerable<TaskProperty> taskPropertys = taskPropertyRepository.FindAll(Specification<TaskProperty>.Eval(entity => entity.ConstructionTaskId == constructionTask.Id));
                    if (taskPropertys != null)
                    {
                        foreach (var taskProperty in taskPropertys)
                        {
                            taskProperty.SupervisorCustomerId = supervisorCustomer.Id;
                            taskPropertyRepository.Update(taskProperty);
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
                //if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_User_AddressingUserId"))
                //{
                //    throw new ApplicationFault("选择的租赁人在系统中不存在");
                //}
                throw ex;
            }
        }
    }
}
