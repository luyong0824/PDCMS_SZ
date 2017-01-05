using PDBM.ServiceContracts.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models.FileMgmt;

namespace PDBM.ApplicationService.Services.BaseData
{
    public class PlacePropertyService : DataService, IPlacePropertyService
    {
        private readonly IRepository<PlaceProperty> placePropertyRepository;
        private readonly IRepository<PlacePropertyLog> placePropertyLogRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FoundationTest> foundationTestRepository;
        private readonly IRepository<TowerLog> towerLogRepository;
        private readonly IRepository<TowerBaseLog> towerBaseLogRepository;
        private readonly IRepository<MachineRoomLog> machineRoomLogRepository;
        private readonly IRepository<ExternalElectricPowerLog> externalElectricPowerLogRepository;
        private readonly IRepository<EquipmentInstallLog> equipmentInstallLogRepository;
        private readonly IRepository<AddressExplorLog> addressExplorLogRepository;
        private readonly IRepository<FoundationTestLog> foundationTestLogRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public PlacePropertyService(IRepositoryContext context,
            IRepository<PlaceProperty> placePropertyRepository,
            IRepository<PlacePropertyLog> placePropertyLogRepository,
            IRepository<Place> placeRepository,
            IRepository<User> userRepository,
            IRepository<Tower> towerRepository,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FoundationTest> foundationTestRepository,
            IRepository<TowerLog> towerLogRepository,
            IRepository<TowerBaseLog> towerBaseLogRepository,
            IRepository<MachineRoomLog> machineRoomLogRepository,
            IRepository<ExternalElectricPowerLog> externalElectricPowerLogRepository,
            IRepository<EquipmentInstallLog> equipmentInstallLogRepository,
            IRepository<AddressExplorLog> addressExplorLogRepository,
            IRepository<FoundationTestLog> foundationTestLogRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.placePropertyRepository = placePropertyRepository;
            this.placePropertyLogRepository = placePropertyLogRepository;
            this.placeRepository = placeRepository;
            this.userRepository = userRepository;
            this.towerRepository = towerRepository;
            this.towerBaseRepository = towerBaseRepository;
            this.machineRoomRepository = machineRoomRepository;
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.addressExplorRepository = addressExplorRepository;
            this.foundationTestRepository = foundationTestRepository;
            this.towerLogRepository = towerLogRepository;
            this.towerBaseLogRepository = towerBaseLogRepository;
            this.machineRoomLogRepository = machineRoomLogRepository;
            this.externalElectricPowerLogRepository = externalElectricPowerLogRepository;
            this.equipmentInstallLogRepository = equipmentInstallLogRepository;
            this.addressExplorLogRepository = addressExplorLogRepository;
            this.foundationTestLogRepository = foundationTestLogRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        public PlacePropertyMaintObject GetPlacePropertyById(Guid id)
        {
            PlaceProperty placeProperty = placePropertyRepository.FindByKey(id);
            if (placeProperty != null)
            {
                PlacePropertyMaintObject placePropertyMaintObject = MapperHelper.Map<PlaceProperty, PlacePropertyMaintObject>(placeProperty);
                //Place place = placeRepository.GetByKey(placeProperty.PlaceId);
                //User yd = userRepository.GetByKey(placeProperty.YDRegistUserId);
                //User dx = userRepository.GetByKey(placeProperty.DXRegistUserId);
                //User lt = userRepository.GetByKey(placeProperty.LTRegistUserId);
                //placePropertyMaintObject.PlaceName = place.PlaceName;
                //placePropertyMaintObject.YDRegistMan = yd.FullName;
                //placePropertyMaintObject.DXRegistMan = dx.FullName;
                //placePropertyMaintObject.LTRegistMan = lt.FullName;
                return placePropertyMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的站点属性在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改站点属性
        /// </summary>
        /// <param name="placePropertyMaintObject">要新增或者修改的站点属性对象</param>
        public void AddOrUpdatePlaceProperty(PlacePropertyMaintObject placePropertyMaintObject)
        {
            if (placePropertyMaintObject.Id == Guid.Empty)
            {
                PlaceProperty placeProperty = AggregateFactory.CreatePlaceProperty(placePropertyMaintObject.ParentId, (PropertyType)placePropertyMaintObject.PropertyType, Bool.否,
                    placePropertyMaintObject.MobilePoleNumber, placePropertyMaintObject.MobileCabinetNumber, placePropertyMaintObject.MobilePowerUsed, placePropertyMaintObject.MobileCreateUserId,
                    Bool.否, placePropertyMaintObject.TelecomPoleNumber, placePropertyMaintObject.TelecomCabinetNumber, placePropertyMaintObject.TelecomPowerUsed, placePropertyMaintObject.TelecomCreateUserId,
                    Bool.否, placePropertyMaintObject.UnicomPoleNumber, placePropertyMaintObject.UnicomCabinetNumber, placePropertyMaintObject.UnicomPowerUsed, placePropertyMaintObject.UnicomCreateUserId);
                placePropertyRepository.Add(placeProperty);
            }
            else
            {
                PlaceProperty placeProperty = placePropertyRepository.FindByKey(placePropertyMaintObject.Id);
                if (placeProperty != null)
                {
                    //placeProperty.Modify(placePropertyMaintObject.MobilePoleNumber, placePropertyMaintObject.MobileCabinetNumber, placePropertyMaintObject.MobilePowerUsed, placePropertyMaintObject.MobileCreateUserId, placePropertyMaintObject.MobileCreateDate,
                    //    placePropertyMaintObject.TelecomPoleNumber, placePropertyMaintObject.TelecomCabinetNumber, placePropertyMaintObject.TelecomPowerUsed, placePropertyMaintObject.TelecomCreateUserId,
                    //    placePropertyMaintObject.TelecomCreateDate, placePropertyMaintObject.UnicomPoleNumber, placePropertyMaintObject.UnicomCabinetNumber, placePropertyMaintObject.UnicomPowerUsed,
                    //    placePropertyMaintObject.UnicomCreateUserId, placePropertyMaintObject.UnicomCreateDate);
                    //placePropertyRepository.Update(placeProperty);
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
        /// 删除站点属性
        /// </summary>
        /// <param name="placePropertyMaintObjects"></param>
        public void RemovePlaceProperty(IList<PlacePropertyMaintObject> placePropertyMaintObjects)
        {
            foreach (PlacePropertyMaintObject placePropertyMaintObject in placePropertyMaintObjects)
            {
                PlaceProperty placeProperty = placePropertyRepository.FindByKey(placePropertyMaintObject.Id);
                if (placeProperty != null)
                {
                    placePropertyRepository.Remove(placeProperty);
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

        public ResourceMaintObject GetResourceMaintenanceById(Guid id)
        {
            Place place = placeRepository.FindByKey(id);
            if (place != null)
            {
                ResourceMaintObject resourceMaintObject = new ResourceMaintObject();
                PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));

                resourceMaintObject.Id = place.Id;

                Tower tower = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                if (tower != null)
                {
                    resourceMaintObject.TowerMark = 1;
                    resourceMaintObject.TowerId = tower.Id;
                    resourceMaintObject.TowerType = (int)tower.TowerType;
                    resourceMaintObject.TowerHeight = tower.TowerHeight;
                    resourceMaintObject.PlatFormNumber = tower.PlatFormNumber;
                    resourceMaintObject.PoleNumber = tower.PoleNumber;
                    if (tower.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(tower.ModifyUserId);
                        resourceMaintObject.TowerFullName = user.FullName;
                    }
                    else
                    {
                        resourceMaintObject.TowerFullName = "";
                    }
                    resourceMaintObject.TowerModifyDate = tower.ModifyDate.ToShortDateString();

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
                        resourceMaintObject.TowerCount = count;
                        resourceMaintObject.TowerFileIdList = towerFileAssociation.FileIdList;
                    }
                    else
                    {
                        resourceMaintObject.TowerCount = 0;
                        resourceMaintObject.TowerFileIdList = "";
                    }
                }
                else
                {
                    resourceMaintObject.TowerId = Guid.Empty;
                    resourceMaintObject.TowerMark = 0;
                    resourceMaintObject.TowerType = 0;
                    resourceMaintObject.TowerHeight = 0;
                    resourceMaintObject.PlatFormNumber = 0;
                    resourceMaintObject.PoleNumber = 0;
                    resourceMaintObject.TowerCount = 0;
                    resourceMaintObject.TowerFullName = "";
                    resourceMaintObject.TowerModifyDate = "";
                }

                TowerBase towerBase = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                if (towerBase != null)
                {
                    resourceMaintObject.TowerBaseMark = 1;
                    resourceMaintObject.TowerBaseId = towerBase.Id;
                    resourceMaintObject.TowerBaseType = (int)towerBase.TowerBaseType;
                    if (towerBase.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(towerBase.ModifyUserId);
                        resourceMaintObject.TowerBaseFullName = user.FullName;
                    }
                    else
                    {
                        resourceMaintObject.TowerBaseFullName = "";
                    }
                    resourceMaintObject.TowerBaseModifyDate = towerBase.ModifyDate.ToShortDateString();

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
                        resourceMaintObject.TowerBaseCount = count;
                        resourceMaintObject.TowerBaseFileIdList = towerBaseFileAssociation.FileIdList;
                    }
                    else
                    {
                        resourceMaintObject.TowerBaseCount = 0;
                        resourceMaintObject.TowerBaseFileIdList = "";
                    }
                }
                else
                {
                    resourceMaintObject.TowerBaseId = Guid.Empty;
                    resourceMaintObject.TowerBaseMark = 0;
                    resourceMaintObject.TowerBaseType = 0;
                    resourceMaintObject.TowerBaseCount = 0;
                    resourceMaintObject.TowerBaseFullName = "";
                    resourceMaintObject.TowerBaseModifyDate = "";
                }

                MachineRoom machineRoom = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                if (machineRoom != null)
                {
                    resourceMaintObject.MachineRoomMark = 1;
                    resourceMaintObject.MachineRoomId = machineRoom.Id;
                    resourceMaintObject.MachineRoomType = (int)machineRoom.MachineRoomType;
                    resourceMaintObject.MachineRoomArea = machineRoom.MachineRoomArea;
                    if (machineRoom.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(machineRoom.ModifyUserId);
                        resourceMaintObject.MachineRoomFullName = user.FullName;
                    }
                    else
                    {
                        resourceMaintObject.MachineRoomFullName = "";
                    }
                    resourceMaintObject.MachineRoomModifyDate = machineRoom.ModifyDate.ToShortDateString();

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
                        resourceMaintObject.MachineRoomCount = count;
                        resourceMaintObject.MachineRoomFileIdList = machineRoomFileAssociation.FileIdList;
                    }
                    else
                    {
                        resourceMaintObject.MachineRoomCount = 0;
                        resourceMaintObject.MachineRoomFileIdList = "";
                    }
                }
                else
                {
                    resourceMaintObject.MachineRoomId = Guid.Empty;
                    resourceMaintObject.MachineRoomMark = 0;
                    resourceMaintObject.MachineRoomType = 0;
                    resourceMaintObject.MachineRoomArea = 0;
                    resourceMaintObject.MachineRoomCount = 0;
                    resourceMaintObject.MachineRoomFullName = "";
                    resourceMaintObject.MachineRoomModifyDate = "";
                }

                ExternalElectricPower externalElectricPower = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                if (externalElectricPower != null)
                {
                    resourceMaintObject.ExternalElectricPowerMark = 1;
                    resourceMaintObject.ExternalElectricPowerId = externalElectricPower.Id;
                    resourceMaintObject.ExternalElectric = (int)externalElectricPower.ExternalElectric;
                    if (externalElectricPower.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(externalElectricPower.ModifyUserId);
                        resourceMaintObject.ExternalFullName = user.FullName;
                    }
                    else
                    {
                        resourceMaintObject.ExternalFullName = "";
                    }
                    resourceMaintObject.ExternalModifyDate = externalElectricPower.ModifyDate.ToShortDateString();

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
                        resourceMaintObject.ExternalCount = count;
                        resourceMaintObject.ExternalFileIdList = externalElectricPowerFileAssociation.FileIdList;
                    }
                    else
                    {
                        resourceMaintObject.ExternalCount = 0;
                        resourceMaintObject.ExternalFileIdList = "";
                    }
                }
                else
                {
                    resourceMaintObject.ExternalElectricPowerId = Guid.Empty;
                    resourceMaintObject.ExternalElectricPowerMark = 0;
                    resourceMaintObject.ExternalElectric = 0;
                    resourceMaintObject.ExternalCount = 0;
                    resourceMaintObject.ExternalFullName = "";
                    resourceMaintObject.ExternalModifyDate = "";
                }

                EquipmentInstall equipmentInstall = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                if (equipmentInstall != null)
                {
                    resourceMaintObject.EquipmentInstallMark = 1;
                    resourceMaintObject.EquipmentInstallId = equipmentInstall.Id;
                    resourceMaintObject.SwitchPower = equipmentInstall.SwitchPower;
                    resourceMaintObject.Battery = equipmentInstall.Battery;
                    resourceMaintObject.CabinetNumber = equipmentInstall.CabinetNumber;
                    if (equipmentInstall.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(equipmentInstall.ModifyUserId);
                        resourceMaintObject.EquipmentFullName = user.FullName;
                    }
                    else
                    {
                        resourceMaintObject.EquipmentFullName = "";
                    }
                    resourceMaintObject.EquipmentModifyDate = equipmentInstall.ModifyDate.ToShortDateString();

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
                        resourceMaintObject.EquipmentInstallCount = count;
                        resourceMaintObject.EquipmentInstallFileIdList = EquipmentInstallFileAssociation.FileIdList;
                    }
                    else
                    {
                        resourceMaintObject.EquipmentInstallCount = 0;
                        resourceMaintObject.EquipmentInstallFileIdList = "";
                    }
                }
                else
                {
                    resourceMaintObject.EquipmentInstallId = Guid.Empty;
                    resourceMaintObject.EquipmentInstallMark = 0;
                    resourceMaintObject.SwitchPower = 0;
                    resourceMaintObject.Battery = 0;
                    resourceMaintObject.CabinetNumber = 0;
                    resourceMaintObject.EquipmentInstallCount = 0;
                    resourceMaintObject.EquipmentFullName = "";
                    resourceMaintObject.EquipmentModifyDate = "";
                }

                AddressExplor addressExplor = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                if (addressExplor != null)
                {
                    resourceMaintObject.AddressExplorMark = 1;
                    resourceMaintObject.AddressExplorId = addressExplor.Id;
                    if (addressExplor.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(addressExplor.ModifyUserId);
                        resourceMaintObject.AddressFullName = user.FullName;
                    }
                    else
                    {
                        resourceMaintObject.AddressFullName = "";
                    }
                    resourceMaintObject.AddressModifyDate = addressExplor.ModifyDate.ToShortDateString();

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
                        resourceMaintObject.AddressCount = count;
                        resourceMaintObject.AddressFileIdList = AddressExplorFileAssociation.FileIdList;
                    }
                    else
                    {
                        resourceMaintObject.AddressCount = 0;
                        resourceMaintObject.AddressFileIdList = "";
                    }
                }
                else
                {
                    resourceMaintObject.AddressExplorId = Guid.Empty;
                    resourceMaintObject.AddressExplorMark = 0;
                    resourceMaintObject.AddressCount = 0;
                    resourceMaintObject.AddressFullName = "";
                    resourceMaintObject.AddressModifyDate = "";
                }

                FoundationTest foundationTest = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                if (foundationTest != null)
                {
                    resourceMaintObject.FoundationTestMark = 1;
                    resourceMaintObject.FoundationTestId = foundationTest.Id;
                    if (foundationTest.ModifyUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(foundationTest.ModifyUserId);
                        resourceMaintObject.FoundationFullName = user.FullName;
                    }
                    else
                    {
                        resourceMaintObject.FoundationFullName = "";
                    }
                    resourceMaintObject.FoundationModifyDate = foundationTest.ModifyDate.ToShortDateString();

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
                        resourceMaintObject.FoundationCount = count;
                        resourceMaintObject.FoundationFileIdList = foundationTestFileAssociation.FileIdList;
                    }
                    else
                    {
                        resourceMaintObject.FoundationCount = 0;
                        resourceMaintObject.FoundationFileIdList = "";
                    }
                }
                else
                {
                    resourceMaintObject.FoundationTestId = Guid.Empty;
                    resourceMaintObject.FoundationTestMark = 0;
                    resourceMaintObject.FoundationCount = 0;
                    resourceMaintObject.FoundationFullName = "";
                    resourceMaintObject.FoundationModifyDate = "";
                }

                //resourceMaintObject.MobileShare = (int)place.MobileShare;
                //resourceMaintObject.TelecomShare = (int)place.TelecomShare;
                //resourceMaintObject.UnicomShare = (int)place.UnicomShare;
                if (placeProperty != null)
                {
                    resourceMaintObject.TelecomPoleNumber = placeProperty.TelecomPoleNumber;
                    resourceMaintObject.TelecomCabinetNumber = placeProperty.TelecomCabinetNumber;
                    resourceMaintObject.TelecomPowerUsed = placeProperty.TelecomPowerUsed;
                    if (placeProperty.TelecomCreateUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(placeProperty.TelecomCreateUserId.Value);
                        resourceMaintObject.TelecomFullName = user.FullName;
                        resourceMaintObject.TelecomModifyDate = placeProperty.TelecomCreateDate.ToShortDateString();
                    }
                    else
                    {
                        resourceMaintObject.TelecomFullName = "";
                        resourceMaintObject.TelecomModifyDate = "";
                    }

                    resourceMaintObject.MobilePoleNumber = placeProperty.MobilePoleNumber;
                    resourceMaintObject.MobileCabinetNumber = placeProperty.MobileCabinetNumber;
                    resourceMaintObject.MobilePowerUsed = placeProperty.MobilePowerUsed;
                    if (placeProperty.MobileCreateUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(placeProperty.MobileCreateUserId.Value);
                        resourceMaintObject.MobileFullName = user.FullName;
                        resourceMaintObject.MobileModifyDate = placeProperty.MobileCreateDate.ToShortDateString();
                    }
                    else
                    {
                        resourceMaintObject.MobileFullName = "";
                        resourceMaintObject.MobileModifyDate = "";
                    }

                    resourceMaintObject.UnicomPoleNumber = placeProperty.UnicomPoleNumber;
                    resourceMaintObject.UnicomCabinetNumber = placeProperty.UnicomCabinetNumber;
                    resourceMaintObject.UnicomPowerUsed = placeProperty.UnicomPowerUsed;
                    if (placeProperty.UnicomCreateUserId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(placeProperty.UnicomCreateUserId.Value);
                        resourceMaintObject.UnicomFullName = user.FullName;
                        resourceMaintObject.UnicomModifyDate = placeProperty.UnicomCreateDate.ToShortDateString();
                    }
                    else
                    {
                        resourceMaintObject.UnicomFullName = "";
                        resourceMaintObject.UnicomModifyDate = "";
                    }
                }
                else
                {
                    resourceMaintObject.TelecomPoleNumber = 0;
                    resourceMaintObject.TelecomCabinetNumber = 0;
                    resourceMaintObject.TelecomPowerUsed = 0;
                    resourceMaintObject.TelecomFullName = "";
                    resourceMaintObject.TelecomModifyDate = "";
                    resourceMaintObject.MobilePoleNumber = 0;
                    resourceMaintObject.MobileCabinetNumber = 0;
                    resourceMaintObject.MobilePowerUsed = 0;
                    resourceMaintObject.MobileFullName = "";
                    resourceMaintObject.MobileModifyDate = "";
                    resourceMaintObject.UnicomPoleNumber = 0;
                    resourceMaintObject.UnicomCabinetNumber = 0;
                    resourceMaintObject.UnicomPowerUsed = 0;
                    resourceMaintObject.UnicomFullName = "";
                    resourceMaintObject.UnicomModifyDate = "";
                }
                return resourceMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的站点在系统中不存在");
            }
        }

        public void SaveResourceMaintenance(ResourceMaintObject resourceMaintObject)
        {
            Place place = placeRepository.FindByKey(resourceMaintObject.Id);
            PlaceProperty placeProperty = placePropertyRepository.Find(Specification<PlaceProperty>.Eval(Entity => Entity.ParentId == place.Id && Entity.PropertyType == PropertyType.站点参数));
            if (place != null)
            {
                if (resourceMaintObject.TowerMark == 1)
                {
                    Tower towerCheck = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (towerCheck != null)
                    {
                        Tower tower = towerRepository.FindByKey(towerCheck.Id);
                        FileAssociation towerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"));
                        if ((towerFileAssociation == null && resourceMaintObject.TowerFileIdList != "") || (towerFileAssociation != null && resourceMaintObject.TowerFileIdList != towerFileAssociation.FileIdList) || resourceMaintObject.TowerType != (int)tower.TowerType || resourceMaintObject.TowerHeight != tower.TowerHeight || resourceMaintObject.PlatFormNumber != tower.PlatFormNumber || resourceMaintObject.PoleNumber != tower.PoleNumber)
                        {
                            tower.Modify((TowerType)resourceMaintObject.TowerType, resourceMaintObject.TowerHeight, resourceMaintObject.PlatFormNumber, resourceMaintObject.PoleNumber, towerCheck.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            towerRepository.Update(tower);

                            if (towerFileAssociation == null && resourceMaintObject.TowerFileIdList != "")
                            {
                                FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("Tower", tower.Id, resourceMaintObject.TowerFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Add(newFileAssociation);
                            }
                            else if (towerFileAssociation != null && resourceMaintObject.TowerFileIdList != towerFileAssociation.FileIdList)
                            {
                                towerFileAssociation.Modify(resourceMaintObject.TowerFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Update(towerFileAssociation);
                            }

                            TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.修改, tower.ParentId, tower.PropertyType, (TowerType)resourceMaintObject.TowerType, resourceMaintObject.TowerHeight,
                                resourceMaintObject.PlatFormNumber, resourceMaintObject.PoleNumber, tower.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            towerLogRepository.Add(towerLog);
                            FileAssociation towerLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerLog", towerLog.Id, resourceMaintObject.TowerFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(towerLogFileAssociation);
                        }
                    }
                    else
                    {
                        Tower tower = AggregateFactory.CreateTower(place.Id, PropertyType.站点参数, (TowerType)resourceMaintObject.TowerType, resourceMaintObject.TowerHeight, resourceMaintObject.PlatFormNumber, resourceMaintObject.PoleNumber, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        towerRepository.Add(tower);
                        FileAssociation towerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"));
                        if (towerFileAssociation == null && resourceMaintObject.TowerFileIdList != "")
                        {
                            FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("Tower", tower.Id, resourceMaintObject.TowerFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(newFileAssociation);
                        }

                        TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.新增, tower.ParentId, tower.PropertyType, (TowerType)resourceMaintObject.TowerType, resourceMaintObject.TowerHeight,
                                resourceMaintObject.PlatFormNumber, resourceMaintObject.PoleNumber, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        towerLogRepository.Add(towerLog);
                        FileAssociation towerLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerLog", towerLog.Id, resourceMaintObject.TowerFileIdList, resourceMaintObject.ModifyUserId.Value);
                        fileAssociationRepository.Add(towerLogFileAssociation);
                    }
                }
                else
                {
                    Tower towerCheck = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (towerCheck != null)
                    {
                        Tower tower = towerRepository.FindByKey(towerCheck.Id);
                        if (tower != null)
                        {
                            TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.删除, tower.ParentId, tower.PropertyType, tower.TowerType, tower.TowerHeight,
                                tower.PlatFormNumber, tower.PoleNumber, tower.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            towerLogRepository.Add(towerLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation towerLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerLog", towerLog.Id, fileAssociation.FileIdList, resourceMaintObject.ModifyUserId.Value);
                                    fileAssociationRepository.Add(towerLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            towerRepository.Remove(tower);
                        }
                    }
                }

                if (resourceMaintObject.TowerBaseMark == 1)
                {
                    TowerBase towerBaseCheck = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (towerBaseCheck != null)
                    {
                        TowerBase towerBase = towerBaseRepository.FindByKey(towerBaseCheck.Id);
                        FileAssociation towerBaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"));
                        if ((towerBaseFileAssociation == null && resourceMaintObject.TowerBaseFileIdList != "") || (towerBaseFileAssociation != null && resourceMaintObject.TowerBaseFileIdList != towerBaseFileAssociation.FileIdList) || resourceMaintObject.TowerBaseType != (int)towerBase.TowerBaseType)
                        {
                            towerBase.Modify((TowerBaseType)resourceMaintObject.TowerBaseType, towerBase.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            towerBaseRepository.Update(towerBase);

                            if (towerBaseFileAssociation == null && resourceMaintObject.TowerBaseFileIdList != "")
                            {
                                FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("TowerBase", towerBase.Id, resourceMaintObject.TowerBaseFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Add(newFileAssociation);
                            }
                            else if (towerBaseFileAssociation != null && resourceMaintObject.TowerBaseFileIdList != towerBaseFileAssociation.FileIdList)
                            {
                                towerBaseFileAssociation.Modify(resourceMaintObject.TowerBaseFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Update(towerBaseFileAssociation);
                            }

                            TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.修改, towerBase.ParentId, towerBase.PropertyType, (TowerBaseType)resourceMaintObject.TowerBaseType, towerBase.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            towerBaseLogRepository.Add(towerBaseLog);
                            FileAssociation towerBaseLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerBaseLog", towerBaseLog.Id, resourceMaintObject.TowerBaseFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(towerBaseLogFileAssociation);
                        }
                    }
                    else
                    {
                        TowerBase towerBase = AggregateFactory.CreateTowerBase(place.Id, PropertyType.站点参数, (TowerBaseType)resourceMaintObject.TowerBaseType, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        towerBaseRepository.Add(towerBase);
                        FileAssociation towerBaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"));
                        if (towerBaseFileAssociation == null && resourceMaintObject.TowerBaseFileIdList != "")
                        {
                            FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("TowerBase", towerBase.Id, resourceMaintObject.TowerBaseFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(newFileAssociation);
                        }

                        TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.新增, towerBase.ParentId, towerBase.PropertyType, (TowerBaseType)resourceMaintObject.TowerBaseType, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        towerBaseLogRepository.Add(towerBaseLog);
                        FileAssociation towerBaseLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerBaseLog", towerBaseLog.Id, resourceMaintObject.TowerBaseFileIdList, resourceMaintObject.ModifyUserId.Value);
                        fileAssociationRepository.Add(towerBaseLogFileAssociation);
                    }
                }
                else
                {
                    TowerBase towerBaseCheck = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (towerBaseCheck != null)
                    {
                        TowerBase towerBase = towerBaseRepository.FindByKey(towerBaseCheck.Id);
                        if (towerBase != null)
                        {
                            TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.删除, towerBase.ParentId, towerBase.PropertyType, towerBase.TowerBaseType, towerBase.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            towerBaseLogRepository.Add(towerBaseLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation towerBaseLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerBaseLog", towerBaseLog.Id, fileAssociation.FileIdList, resourceMaintObject.ModifyUserId.Value);
                                    fileAssociationRepository.Add(towerBaseLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            towerBaseRepository.Remove(towerBase);
                        }
                    }
                }

                if (resourceMaintObject.MachineRoomMark == 1)
                {
                    MachineRoom machineRoomCheck = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (machineRoomCheck != null)
                    {
                        MachineRoom machineRoom = machineRoomRepository.FindByKey(machineRoomCheck.Id);
                        FileAssociation machineRoomFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"));
                        if ((machineRoomFileAssociation == null && resourceMaintObject.MachineRoomFileIdList != "") || (machineRoomFileAssociation != null && resourceMaintObject.MachineRoomFileIdList != machineRoomFileAssociation.FileIdList) || (MachineRoomType)resourceMaintObject.MachineRoomType != machineRoom.MachineRoomType || resourceMaintObject.MachineRoomArea != machineRoom.MachineRoomArea)
                        {
                            machineRoom.Modify((MachineRoomType)resourceMaintObject.MachineRoomType, resourceMaintObject.MachineRoomArea, machineRoom.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            machineRoomRepository.Update(machineRoom);

                            if (machineRoomFileAssociation == null && resourceMaintObject.MachineRoomFileIdList != "")
                            {
                                FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoom", machineRoom.Id, resourceMaintObject.MachineRoomFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Add(newFileAssociation);
                            }
                            else if (machineRoomFileAssociation != null && resourceMaintObject.MachineRoomFileIdList != machineRoomFileAssociation.FileIdList)
                            {
                                machineRoomFileAssociation.Modify(resourceMaintObject.MachineRoomFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Update(machineRoomFileAssociation);
                            }

                            MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.修改, machineRoom.ParentId, machineRoom.PropertyType, (MachineRoomType)resourceMaintObject.MachineRoomType, resourceMaintObject.MachineRoomArea, machineRoom.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            machineRoomLogRepository.Add(machineRoomLog);
                            FileAssociation machineRoomLogFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoomLog", machineRoomLog.Id, resourceMaintObject.MachineRoomFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(machineRoomLogFileAssociation);
                        }
                    }
                    else
                    {
                        MachineRoom machineRoom = AggregateFactory.CreateMachineRoom(place.Id, PropertyType.站点参数, (MachineRoomType)resourceMaintObject.MachineRoomType, resourceMaintObject.MachineRoomArea, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        machineRoomRepository.Add(machineRoom);
                        FileAssociation machineRoomFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"));
                        if (machineRoomFileAssociation == null && resourceMaintObject.MachineRoomFileIdList != "")
                        {
                            FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoom", machineRoom.Id, resourceMaintObject.MachineRoomFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(newFileAssociation);
                        }

                        MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.新增, machineRoom.ParentId, machineRoom.PropertyType, (MachineRoomType)resourceMaintObject.MachineRoomType, resourceMaintObject.MachineRoomArea, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        machineRoomLogRepository.Add(machineRoomLog);
                        FileAssociation machineRoomLogFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoomLog", machineRoomLog.Id, resourceMaintObject.MachineRoomFileIdList, resourceMaintObject.ModifyUserId.Value);
                        fileAssociationRepository.Add(machineRoomLogFileAssociation);
                    }
                }
                else
                {
                    MachineRoom machineRoomCheck = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (machineRoomCheck != null)
                    {
                        MachineRoom machineRoom = machineRoomRepository.FindByKey(machineRoomCheck.Id);
                        if (machineRoom != null)
                        {
                            MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.删除, machineRoom.ParentId, machineRoom.PropertyType, machineRoom.MachineRoomType, machineRoom.MachineRoomArea, machineRoom.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            machineRoomLogRepository.Add(machineRoomLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation machineRoomLogFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoomLog", machineRoomLog.Id, fileAssociation.FileIdList, resourceMaintObject.ModifyUserId.Value);
                                    fileAssociationRepository.Add(machineRoomLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            machineRoomRepository.Remove(machineRoom);
                        }
                    }
                }

                if (resourceMaintObject.ExternalElectricPowerMark == 1)
                {
                    ExternalElectricPower externalElectricPowerCheck = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (externalElectricPowerCheck != null)
                    {
                        ExternalElectricPower externalElectricPower = externalElectricPowerRepository.FindByKey(externalElectricPowerCheck.Id);
                        FileAssociation externalElectricPowerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"));
                        if ((externalElectricPowerFileAssociation == null && resourceMaintObject.ExternalFileIdList != "") || (externalElectricPowerFileAssociation != null && resourceMaintObject.ExternalFileIdList != externalElectricPowerFileAssociation.FileIdList) || (ExternalElectric)resourceMaintObject.ExternalElectric != externalElectricPower.ExternalElectric)
                        {
                            externalElectricPower.Modify((ExternalElectric)resourceMaintObject.ExternalElectric, externalElectricPower.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            externalElectricPowerRepository.Update(externalElectricPower);

                            if (externalElectricPowerFileAssociation == null && resourceMaintObject.ExternalFileIdList != "")
                            {
                                FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPower", externalElectricPower.Id, resourceMaintObject.ExternalFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Add(newFileAssociation);
                            }
                            else if (externalElectricPowerFileAssociation != null && resourceMaintObject.ExternalFileIdList != externalElectricPowerFileAssociation.FileIdList)
                            {
                                externalElectricPowerFileAssociation.Modify(resourceMaintObject.ExternalFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Update(externalElectricPowerFileAssociation);
                            }

                            ExternalElectricPowerLog externalElectricPowerLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.修改, externalElectricPower.ParentId, externalElectricPower.PropertyType, (ExternalElectric)resourceMaintObject.ExternalElectric, externalElectricPower.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            externalElectricPowerLogRepository.Add(externalElectricPowerLog);
                            FileAssociation externalElectricPowerLogFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPowerLog", externalElectricPowerLog.Id, resourceMaintObject.ExternalFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(externalElectricPowerLogFileAssociation);
                        }
                    }
                    else
                    {
                        ExternalElectricPower externalElectricPower = AggregateFactory.CreateExternalElectricPower(place.Id, PropertyType.站点参数, (ExternalElectric)resourceMaintObject.ExternalElectric, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        externalElectricPowerRepository.Add(externalElectricPower);
                        FileAssociation externalElectricPowerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"));
                        if (externalElectricPowerFileAssociation == null && resourceMaintObject.ExternalFileIdList != "")
                        {
                            FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPower", externalElectricPower.Id, resourceMaintObject.ExternalFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(newFileAssociation);
                        }

                        ExternalElectricPowerLog externalElectricPowerLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.新增, externalElectricPower.ParentId, externalElectricPower.PropertyType, (ExternalElectric)resourceMaintObject.ExternalElectric, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        externalElectricPowerLogRepository.Add(externalElectricPowerLog);
                        FileAssociation externalElectricPowerLogFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPowerLog", externalElectricPowerLog.Id, resourceMaintObject.ExternalFileIdList, resourceMaintObject.ModifyUserId.Value);
                        fileAssociationRepository.Add(externalElectricPowerLogFileAssociation);
                    }
                }
                else
                {
                    ExternalElectricPower externalElectricPowerCheck = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (externalElectricPowerCheck != null)
                    {
                        ExternalElectricPower externalElectricPower = externalElectricPowerRepository.FindByKey(externalElectricPowerCheck.Id);
                        if (externalElectricPower != null)
                        {
                            ExternalElectricPowerLog externalElectricPowerLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.删除, externalElectricPower.ParentId, externalElectricPower.PropertyType, externalElectricPower.ExternalElectric, externalElectricPower.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            externalElectricPowerLogRepository.Add(externalElectricPowerLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation externalElectricPowerLogFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPowerLog", externalElectricPowerLog.Id, fileAssociation.FileIdList, resourceMaintObject.ModifyUserId.Value);
                                    fileAssociationRepository.Add(externalElectricPowerLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            externalElectricPowerRepository.Remove(externalElectricPower);
                        }
                    }
                }

                if (resourceMaintObject.EquipmentInstallMark == 1)
                {
                    EquipmentInstall equipmentInstallCheck = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (equipmentInstallCheck != null)
                    {
                        EquipmentInstall equipmentInstall = equipmentInstallRepository.FindByKey(equipmentInstallCheck.Id);
                        FileAssociation equipmentInstallFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"));
                        if ((equipmentInstallFileAssociation == null && resourceMaintObject.EquipmentInstallFileIdList != "") || (equipmentInstallFileAssociation != null && resourceMaintObject.EquipmentInstallFileIdList != equipmentInstallFileAssociation.FileIdList) || resourceMaintObject.SwitchPower != equipmentInstall.SwitchPower || resourceMaintObject.Battery != equipmentInstall.Battery || resourceMaintObject.CabinetNumber != equipmentInstall.CabinetNumber)
                        {
                            equipmentInstall.Modify(resourceMaintObject.SwitchPower, resourceMaintObject.Battery, resourceMaintObject.CabinetNumber, equipmentInstall.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            equipmentInstallRepository.Update(equipmentInstall);

                            if (equipmentInstallFileAssociation == null && resourceMaintObject.EquipmentInstallFileIdList != "")
                            {
                                FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstall", equipmentInstall.Id, resourceMaintObject.EquipmentInstallFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Add(newFileAssociation);
                            }
                            else if (equipmentInstallFileAssociation != null && resourceMaintObject.EquipmentInstallFileIdList != equipmentInstallFileAssociation.FileIdList)
                            {
                                equipmentInstallFileAssociation.Modify(resourceMaintObject.EquipmentInstallFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Update(equipmentInstallFileAssociation);
                            }

                            EquipmentInstallLog equipmentInstallLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.修改, equipmentInstall.ParentId, equipmentInstall.PropertyType, resourceMaintObject.SwitchPower, resourceMaintObject.Battery, resourceMaintObject.CabinetNumber, equipmentInstall.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            equipmentInstallLogRepository.Add(equipmentInstallLog);
                            FileAssociation equipmentInstallLogFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstallLog", equipmentInstallLog.Id, resourceMaintObject.EquipmentInstallFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(equipmentInstallLogFileAssociation);
                        }
                    }
                    else
                    {
                        EquipmentInstall equipmentInstall = AggregateFactory.CreateEquipmentInstall(place.Id, PropertyType.站点参数, resourceMaintObject.SwitchPower, resourceMaintObject.Battery, resourceMaintObject.CabinetNumber, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        equipmentInstallRepository.Add(equipmentInstall);
                        FileAssociation equipmentInstallFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"));
                        if (equipmentInstallFileAssociation == null && resourceMaintObject.EquipmentInstallFileIdList != "")
                        {
                            FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstall", equipmentInstall.Id, resourceMaintObject.EquipmentInstallFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(newFileAssociation);
                        }

                        EquipmentInstallLog equipmentInstallLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.新增, equipmentInstall.ParentId, equipmentInstall.PropertyType, resourceMaintObject.SwitchPower, resourceMaintObject.Battery, resourceMaintObject.CabinetNumber, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        equipmentInstallLogRepository.Add(equipmentInstallLog);
                        FileAssociation equipmentInstallLogFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstallLog", equipmentInstallLog.Id, resourceMaintObject.EquipmentInstallFileIdList, resourceMaintObject.ModifyUserId.Value);
                        fileAssociationRepository.Add(equipmentInstallLogFileAssociation);
                    }
                }
                else
                {
                    EquipmentInstall equipmentInstallCheck = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (equipmentInstallCheck != null)
                    {
                        EquipmentInstall equipmentInstall = equipmentInstallRepository.FindByKey(equipmentInstallCheck.Id);
                        if (equipmentInstall != null)
                        {
                            EquipmentInstallLog equipmentInstallLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.删除, equipmentInstall.ParentId, equipmentInstall.PropertyType, equipmentInstall.SwitchPower, equipmentInstall.Battery, equipmentInstall.CabinetNumber, equipmentInstall.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            equipmentInstallLogRepository.Add(equipmentInstallLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation equipmentInstallLogFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstallLog", equipmentInstallLog.Id, fileAssociation.FileIdList, resourceMaintObject.ModifyUserId.Value);
                                    fileAssociationRepository.Add(equipmentInstallLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                        }
                        equipmentInstallRepository.Remove(equipmentInstall);
                    }
                }

                if (resourceMaintObject.AddressExplorMark == 1)
                {
                    AddressExplor addressExplorCheck = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (addressExplorCheck != null)
                    {
                        AddressExplor addressExplor = addressExplorRepository.FindByKey(addressExplorCheck.Id);
                        FileAssociation addressExplorFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"));
                        if ((addressExplorFileAssociation == null && resourceMaintObject.AddressFileIdList != "") || (addressExplorFileAssociation != null && resourceMaintObject.AddressFileIdList != addressExplorFileAssociation.FileIdList))
                        {
                            addressExplor.Modify(addressExplor.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            addressExplorRepository.Update(addressExplor);

                            if (addressExplorFileAssociation == null && resourceMaintObject.AddressFileIdList != "")
                            {
                                FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplor", addressExplor.Id, resourceMaintObject.AddressFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Add(newFileAssociation);
                            }
                            else if (addressExplorFileAssociation != null && resourceMaintObject.AddressFileIdList != addressExplorFileAssociation.FileIdList)
                            {
                                addressExplorFileAssociation.Modify(resourceMaintObject.AddressFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Update(addressExplorFileAssociation);
                            }

                            AddressExplorLog addressExplorLog = AggregateFactory.CreateAddressExplorLog(OperationType.修改, addressExplor.ParentId, addressExplor.PropertyType, addressExplor.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            addressExplorLogRepository.Add(addressExplorLog);
                            FileAssociation addressExplorLogFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplorLog", addressExplorLog.Id, resourceMaintObject.AddressFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(addressExplorLogFileAssociation);
                        }
                    }
                    else
                    {
                        AddressExplor addressExplor = AggregateFactory.CreateAddressExplor(place.Id, PropertyType.站点参数, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        addressExplorRepository.Add(addressExplor);
                        FileAssociation addressExplorFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"));
                        if (addressExplorFileAssociation == null && resourceMaintObject.AddressFileIdList != "")
                        {
                            FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplor", addressExplor.Id, resourceMaintObject.AddressFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(newFileAssociation);
                        }

                        AddressExplorLog addressExplorLog = AggregateFactory.CreateAddressExplorLog(OperationType.新增, addressExplor.ParentId, addressExplor.PropertyType, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        addressExplorLogRepository.Add(addressExplorLog);
                        FileAssociation addressExplorLogFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplorLog", addressExplorLog.Id, resourceMaintObject.AddressFileIdList, resourceMaintObject.ModifyUserId.Value);
                        fileAssociationRepository.Add(addressExplorLogFileAssociation);
                    }
                }
                else
                {
                    AddressExplor addressExplorCheck = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (addressExplorCheck != null)
                    {
                        AddressExplor addressExplor = addressExplorRepository.FindByKey(addressExplorCheck.Id);
                        if (addressExplor != null)
                        {
                            AddressExplorLog addressExplorLog = AggregateFactory.CreateAddressExplorLog(OperationType.删除, addressExplor.ParentId, addressExplor.PropertyType, addressExplor.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            addressExplorLogRepository.Add(addressExplorLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation addressExplorLogFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplorLog", addressExplorLog.Id, fileAssociation.FileIdList, resourceMaintObject.ModifyUserId.Value);
                                    fileAssociationRepository.Add(addressExplorLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            addressExplorRepository.Remove(addressExplor);
                        }
                    }
                }

                if (resourceMaintObject.FoundationTestMark == 1)
                {
                    FoundationTest foundationTestCheck = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (foundationTestCheck != null)
                    {
                        FoundationTest foundationTest = foundationTestRepository.FindByKey(foundationTestCheck.Id);
                        FileAssociation foundationTestFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"));
                        if ((foundationTestFileAssociation == null && resourceMaintObject.FoundationFileIdList != "") || (foundationTestFileAssociation != null && resourceMaintObject.FoundationFileIdList != foundationTestFileAssociation.FileIdList))
                        {
                            foundationTest.Modify(foundationTest.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            foundationTestRepository.Update(foundationTest);

                            if (foundationTestFileAssociation == null && resourceMaintObject.FoundationFileIdList != "")
                            {
                                FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTest", foundationTest.Id, resourceMaintObject.FoundationFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Add(newFileAssociation);
                            }
                            else if (foundationTestFileAssociation != null && resourceMaintObject.FoundationFileIdList != foundationTestFileAssociation.FileIdList)
                            {
                                foundationTestFileAssociation.Modify(resourceMaintObject.FoundationFileIdList, resourceMaintObject.ModifyUserId.Value);
                                fileAssociationRepository.Update(foundationTestFileAssociation);
                            }

                            FoundationTestLog foundationTestLog = AggregateFactory.CreateFoundationTestLog(OperationType.修改, foundationTest.ParentId, foundationTest.PropertyType, foundationTest.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            foundationTestLogRepository.Add(foundationTestLog);
                            FileAssociation foundationTestLogFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTestLog", foundationTestLog.Id, resourceMaintObject.FoundationFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(foundationTestLogFileAssociation);
                        }
                    }
                    else
                    {
                        FoundationTest foundationTest = AggregateFactory.CreateFoundationTest(place.Id, PropertyType.站点参数, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        foundationTestRepository.Add(foundationTest);
                        FileAssociation foundationTestFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"));
                        if (foundationTestFileAssociation == null && resourceMaintObject.FoundationFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTest", foundationTest.Id, resourceMaintObject.FoundationFileIdList, resourceMaintObject.ModifyUserId.Value);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }

                        FoundationTestLog foundationTestLog = AggregateFactory.CreateFoundationTestLog(OperationType.新增, foundationTest.ParentId, foundationTest.PropertyType, 0, 0, "", resourceMaintObject.ModifyUserId.Value);
                        foundationTestLogRepository.Add(foundationTestLog);
                        FileAssociation foundationTestLogFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTestLog", foundationTestLog.Id, resourceMaintObject.FoundationFileIdList, resourceMaintObject.ModifyUserId.Value);
                        fileAssociationRepository.Add(foundationTestLogFileAssociation);
                    }
                }
                else
                {
                    FoundationTest foundationTestCheck = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == place.Id && entity.PropertyType == PropertyType.站点参数));
                    if (foundationTestCheck != null)
                    {
                        FoundationTest foundationTest = foundationTestRepository.FindByKey(foundationTestCheck.Id);
                        if (foundationTest != null)
                        {
                            FoundationTestLog foundationTestLog = AggregateFactory.CreateFoundationTestLog(OperationType.删除, foundationTest.ParentId, foundationTest.PropertyType, foundationTest.BudgetPrice, 0, "", resourceMaintObject.ModifyUserId.Value);
                            foundationTestLogRepository.Add(foundationTestLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation foundationTestLogFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTestLog", foundationTestLog.Id, fileAssociation.FileIdList, resourceMaintObject.ModifyUserId.Value);
                                    fileAssociationRepository.Add(foundationTestLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            foundationTestRepository.Remove(foundationTest);
                        }
                    }
                }

                if (placeProperty != null)
                {
                    if (resourceMaintObject.MobileShare != (int)placeProperty.MobileShare || resourceMaintObject.MobilePoleNumber != placeProperty.MobilePoleNumber || resourceMaintObject.MobileCabinetNumber != placeProperty.MobileCabinetNumber || resourceMaintObject.MobilePowerUsed != placeProperty.MobilePowerUsed)
                    {
                        placeProperty.ModifyMobile((Bool)resourceMaintObject.MobileShare, resourceMaintObject.MobilePoleNumber, resourceMaintObject.MobileCabinetNumber, resourceMaintObject.MobilePowerUsed, resourceMaintObject.ModifyUserId.Value);
                        placePropertyRepository.Update(placeProperty);
                        PlacePropertyLog mobilePlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.移动, (Bool)resourceMaintObject.MobileShare, resourceMaintObject.MobilePoleNumber, resourceMaintObject.MobileCabinetNumber, resourceMaintObject.MobilePowerUsed, resourceMaintObject.ModifyUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId.Value, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                        placePropertyLogRepository.Add(mobilePlacePropertyLog);
                        place.OperatorShared(1, (Bool)resourceMaintObject.MobileShare);
                        placeRepository.Update(place);
                    }
                    if (resourceMaintObject.TelecomShare != (int)placeProperty.TelecomShare || resourceMaintObject.TelecomPoleNumber != placeProperty.TelecomPoleNumber || resourceMaintObject.TelecomCabinetNumber != placeProperty.TelecomCabinetNumber || resourceMaintObject.TelecomPowerUsed != placeProperty.TelecomPowerUsed)
                    {
                        placeProperty.ModifyTelecom((Bool)resourceMaintObject.TelecomShare, resourceMaintObject.TelecomPoleNumber, resourceMaintObject.TelecomCabinetNumber, resourceMaintObject.TelecomPowerUsed, resourceMaintObject.ModifyUserId.Value);
                        placePropertyRepository.Update(placeProperty);
                        PlacePropertyLog telecomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.电信, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, (Bool)resourceMaintObject.TelecomShare, resourceMaintObject.TelecomPoleNumber, resourceMaintObject.TelecomCabinetNumber, resourceMaintObject.TelecomPowerUsed, resourceMaintObject.ModifyUserId, placeProperty.UnicomShare, placeProperty.UnicomPoleNumber, placeProperty.UnicomCabinetNumber, placeProperty.UnicomPowerUsed, placeProperty.UnicomCreateUserId);
                        placePropertyLogRepository.Add(telecomPlacePropertyLog);
                        place.OperatorShared(2, (Bool)resourceMaintObject.TelecomShare);
                        placeRepository.Update(place);
                    }
                    if (resourceMaintObject.UnicomShare != (int)placeProperty.UnicomShare || resourceMaintObject.UnicomPoleNumber != placeProperty.UnicomPoleNumber || resourceMaintObject.UnicomCabinetNumber != placeProperty.UnicomCabinetNumber || resourceMaintObject.UnicomPowerUsed != placeProperty.UnicomPowerUsed)
                    {
                        placeProperty.ModifyUnicom((Bool)resourceMaintObject.UnicomShare, resourceMaintObject.UnicomPoleNumber, resourceMaintObject.UnicomCabinetNumber, resourceMaintObject.UnicomPowerUsed, resourceMaintObject.ModifyUserId.Value);
                        placePropertyRepository.Update(placeProperty);
                        PlacePropertyLog unicomPlacePropertyLog = AggregateFactory.CreatePlacePropertyLog(OperationType.修改, placeProperty.ParentId, placeProperty.PropertyType, CompanyNameId.联通, placeProperty.MobileShare, placeProperty.MobilePoleNumber, placeProperty.MobileCabinetNumber, placeProperty.MobilePowerUsed, placeProperty.MobileCreateUserId, placeProperty.TelecomShare, placeProperty.TelecomPoleNumber, placeProperty.TelecomCabinetNumber, placeProperty.TelecomPowerUsed, placeProperty.TelecomCreateUserId, (Bool)resourceMaintObject.UnicomShare, resourceMaintObject.UnicomPoleNumber, resourceMaintObject.UnicomCabinetNumber, resourceMaintObject.UnicomPowerUsed, resourceMaintObject.ModifyUserId);
                        placePropertyLogRepository.Add(unicomPlacePropertyLog);
                        place.OperatorShared(3, (Bool)resourceMaintObject.UnicomShare);
                        placeRepository.Update(place);
                    }
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
