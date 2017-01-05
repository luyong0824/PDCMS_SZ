using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class PlaceDesignService : DataService, IPlaceDesignService
    {
        private readonly IRepository<PlaceDesign> placeDesignRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Remodeling> remodelingRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository;
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
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<WFActivityInstance> wfActivityInstanceRepository;

        public PlaceDesignService(IRepositoryContext context,
            IRepository<PlaceDesign> placeDesignRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<Remodeling> remodelingRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository,
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
            IRepository<Customer> customerRepository,
            IRepository<WFActivityInstance> wfActivityInstanceRepository)
            : base(context)
        {
            this.placeDesignRepository = placeDesignRepository;
            this.addressingRepository = addressingRepository;
            this.remodelingRepository = remodelingRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.wfActivityInstanceEditorRepository = wfActivityInstanceEditorRepository;
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
            this.customerRepository = customerRepository;
            this.wfActivityInstanceRepository = wfActivityInstanceRepository;
        }

        /// <summary>
        /// 根据站点设计信息Id获取站点设计信息
        /// </summary>
        /// <param name="id">站点设计信息Id</param>
        /// <returns>站点设计信息维护对象</returns>
        public PlaceDesignMaintObject GetPlaceDesignById(Guid id)
        {
            PlaceDesign placeDesign = placeDesignRepository.FindByKey(id);
            if (placeDesign != null)
            {
                PlaceDesignMaintObject placeDesignMaintObject = MapperHelper.Map<PlaceDesign, PlaceDesignMaintObject>(placeDesign);
                return placeDesignMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的站点设计信息在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改站点设计信息
        /// </summary>
        /// <param name="placeDesignMaintObject">要新增或者修改的站点设计信息对象</param>
        public void AddOrUpdatePlaceDesign(PlaceDesignMaintObject placeDesignMaintObject)
        {
            if (placeDesignMaintObject.Id == Guid.Empty)
            {
                PlaceDesign placeDesign = AggregateFactory.CreatePlaceDesign(placeDesignMaintObject.ParentId, (PropertyType)placeDesignMaintObject.PropertyType, placeDesignMaintObject.CreateUserId);
                placeDesignRepository.Add(placeDesign);
            }
            else
            {
                PlaceDesign placeDesign = placeDesignRepository.FindByKey(placeDesignMaintObject.Id);
                if (placeDesign != null)
                {
                    placeDesign.Modify((State)placeDesignMaintObject.State, placeDesignMaintObject.ModifyUserId);
                    placeDesignRepository.Update(placeDesign);
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

        public void RemovePlaceDesign(IList<PlaceDesignMaintObject> placeDesignMaintObjects)
        {
            foreach (PlaceDesignMaintObject placeDesignMaintObject in placeDesignMaintObjects)
            {
                PlaceDesign placeDesign = placeDesignRepository.FindByKey(placeDesignMaintObject.Id);
                if (placeDesign != null)
                {
                    placeDesignRepository.Remove(placeDesign);
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
        /// 指定设计单位
        /// </summary>
        /// <param name="placeDesignMaintObject"></param>
        public void SaveAppointDesign(PlaceDesignMaintObject placeDesignMaintObject)
        {
            PlaceDesign placeDesign = placeDesignRepository.FindByKey(placeDesignMaintObject.Id);
            Addressing addressing = addressingRepository.FindByKey(placeDesign.ParentId);
            if (addressing != null)
            {
                Customer customer = customerRepository.FindByKey(placeDesignMaintObject.DesignCustomerId.Value);
                if (customer != null)
                {
                    if (customer.CustomerUserId == Guid.Empty)
                    {
                        throw new ApplicationFault("请选择已关联登陆人的设计单位");
                    }
                }
                placeDesign.AppointDesign(placeDesignMaintObject.DesignCustomerId.Value);
                placeDesignRepository.Update(placeDesign);

                WFActivityInstance wfActivityInstance = wfActivityInstanceRepository.FindByKey(placeDesignMaintObject.WFActivityInstanceId);

                IEnumerable<WFActivityInstance> wfActivityInstances = wfActivityInstanceRepository.FindAll(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfActivityInstance.WFProcessInstanceId &&
                    entity.SerialId >= wfActivityInstance.SerialId && entity.Id != wfActivityInstance.Id && entity.WFActivityInstanceState == WFActivityInstanceState.未处理 && entity.WFActivityOperate == WFActivityOperate.单据编辑 && entity.WFActivityEditorId.ToString() == "90c81c32-b84e-46ed-9041-a00cb9b2c04e"));
                if (wfActivityInstances != null)
                {
                    foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances)
                    {
                        modifyWFActivityInstance.UserId = customer.CustomerUserId;
                        wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                    }
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == placeDesignMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(placeDesignMaintObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("FK_dbo.tbl_PlaceDesign_dbo.tbl_Customer_DesignCustomerId"))
                //{
                //    throw new ApplicationFault("选择的设计单位在系统中不存在");
                //}
                //if (ex.Message.Contains("FK_dbo.tbl_PlaceDesign_dbo.tbl_User_DesignUserId"))
                //{
                //    throw new ApplicationFault("选择的设计人员在系统中不存在");
                //}
                throw ex;
            }
        }

        /// <summary>
        /// 指定设计人员
        /// </summary>
        /// <param name="placeDesignMaintObject"></param>
        public void SaveAppointDesignUser(PlaceDesignMaintObject placeDesignMaintObject)
        {
            PlaceDesign placeDesign = placeDesignRepository.FindByKey(placeDesignMaintObject.Id);
            Addressing addressing = addressingRepository.FindByKey(placeDesign.ParentId);
            if (addressing != null)
            {
                placeDesign.AppointDesignUser(placeDesignMaintObject.DesignUserId.Value);
                placeDesignRepository.Update(placeDesign);

                WFActivityInstance wfActivityInstance = wfActivityInstanceRepository.FindByKey(placeDesignMaintObject.WFActivityInstanceId);

                IEnumerable<WFActivityInstance> wfActivityInstances = wfActivityInstanceRepository.FindAll(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfActivityInstance.WFProcessInstanceId &&
                    entity.SerialId >= wfActivityInstance.SerialId && entity.Id != wfActivityInstance.Id && entity.WFActivityInstanceState == WFActivityInstanceState.未处理 && entity.WFActivityOperate == WFActivityOperate.单据编辑 && entity.WFActivityEditorId.ToString() == "97f9aae8-bae1-4fb4-a5d9-061bca9831e4"));
                if (wfActivityInstances != null)
                {
                    foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances)
                    {
                        modifyWFActivityInstance.UserId = placeDesignMaintObject.DesignUserId.Value;
                        wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                    }
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == placeDesignMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(placeDesignMaintObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("FK_dbo.tbl_PlaceDesign_dbo.tbl_Customer_DesignCustomerId"))
                //{
                //    throw new ApplicationFault("选择的设计单位在系统中不存在");
                //}
                //if (ex.Message.Contains("FK_dbo.tbl_PlaceDesign_dbo.tbl_User_DesignUserId"))
                //{
                //    throw new ApplicationFault("选择的设计人员在系统中不存在");
                //}
                throw ex;
            }
        }

        /// <summary>
        /// 保存施工设计
        /// </summary>
        /// <param name="addressingEditorObject"></param>
        public void SaveConstructionDesign(AddressingEditorObject addressingEditorObject)
        {
            Addressing addressing = addressingRepository.FindByKey(addressingEditorObject.Id);
            PlaceDesign placeDesign = placeDesignRepository.Find(Specification<PlaceDesign>.Eval(Entity => Entity.ParentId == addressing.Id && Entity.PropertyType == PropertyType.寻址设计));
            if (addressing != null)
            {
                if (addressingEditorObject.TowerMark == 1)
                {
                    Tower towerCheck = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (towerCheck != null)
                    {
                        Tower tower = towerRepository.FindByKey(towerCheck.Id);
                        FileAssociation towerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"));
                        if ((towerFileAssociation == null && addressingEditorObject.TowerFileIdList != "") || (towerFileAssociation != null && addressingEditorObject.TowerFileIdList != towerFileAssociation.FileIdList) || addressingEditorObject.TowerType != (int)tower.TowerType || addressingEditorObject.TowerHeight != tower.TowerHeight || addressingEditorObject.PlatFormNumber != tower.PlatFormNumber || addressingEditorObject.PoleNumber != tower.PoleNumber || addressingEditorObject.TowerBudget != tower.BudgetPrice || addressingEditorObject.TowerTimeLimit != tower.TimeLimit)
                        {
                            tower.Modify((TowerType)addressingEditorObject.TowerType, addressingEditorObject.TowerHeight, addressingEditorObject.PlatFormNumber, addressingEditorObject.PoleNumber, addressingEditorObject.TowerBudget, addressingEditorObject.TowerTimeLimit, "", addressingEditorObject.ModifyUserId);
                            towerRepository.Update(tower);

                            if (towerFileAssociation == null && addressingEditorObject.TowerFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("Tower", tower.Id, addressingEditorObject.TowerFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (towerFileAssociation != null && addressingEditorObject.TowerFileIdList != towerFileAssociation.FileIdList)
                            {
                                towerFileAssociation.Modify(addressingEditorObject.TowerFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(towerFileAssociation);
                            }

                            TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.修改, tower.ParentId, tower.PropertyType, (TowerType)addressingEditorObject.TowerType, addressingEditorObject.TowerHeight,
                                addressingEditorObject.PlatFormNumber, addressingEditorObject.PoleNumber, addressingEditorObject.TowerBudget, addressingEditorObject.TowerTimeLimit, "", addressingEditorObject.ModifyUserId);
                            towerLogRepository.Add(towerLog);
                            FileAssociation towerLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerLog", towerLog.Id, addressingEditorObject.TowerFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(towerLogFileAssociation);
                        }
                    }
                    else
                    {
                        Tower tower = AggregateFactory.CreateTower(addressing.Id, PropertyType.寻址设计, (TowerType)addressingEditorObject.TowerType, addressingEditorObject.TowerHeight, addressingEditorObject.PlatFormNumber, addressingEditorObject.PoleNumber, addressingEditorObject.TowerBudget, addressingEditorObject.TowerTimeLimit, "", addressingEditorObject.ModifyUserId);
                        towerRepository.Add(tower);
                        FileAssociation towerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"));
                        if (towerFileAssociation == null && addressingEditorObject.TowerFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("Tower", tower.Id, addressingEditorObject.TowerFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (towerFileAssociation != null && addressingEditorObject.TowerFileIdList != towerFileAssociation.FileIdList)
                        {
                            towerFileAssociation.Modify(addressingEditorObject.TowerFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(towerFileAssociation);
                        }

                        TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.新增, tower.ParentId, tower.PropertyType, (TowerType)addressingEditorObject.TowerType, addressingEditorObject.TowerHeight,
                                addressingEditorObject.PlatFormNumber, addressingEditorObject.PoleNumber, addressingEditorObject.TowerBudget, addressingEditorObject.TowerTimeLimit, "", addressingEditorObject.ModifyUserId);
                        towerLogRepository.Add(towerLog);
                        FileAssociation towerLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerLog", towerLog.Id, addressingEditorObject.TowerFileIdList, addressingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(towerLogFileAssociation);
                    }
                }
                else
                {
                    Tower towerCheck = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (towerCheck != null)
                    {
                        Tower tower = towerRepository.FindByKey(towerCheck.Id);
                        if (tower != null)
                        {
                            TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.删除, tower.ParentId, tower.PropertyType, tower.TowerType, tower.TowerHeight,
                                tower.PlatFormNumber, tower.PoleNumber, tower.BudgetPrice, tower.TimeLimit, "", addressingEditorObject.ModifyUserId);
                            towerLogRepository.Add(towerLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation towerLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerLog", towerLog.Id, fileAssociation.FileIdList, addressingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(towerLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            towerRepository.Remove(tower);
                        }
                    }
                }

                if (addressingEditorObject.TowerBaseMark == 1)
                {
                    TowerBase towerBaseCheck = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (towerBaseCheck != null)
                    {
                        TowerBase towerBase = towerBaseRepository.FindByKey(towerBaseCheck.Id);
                        FileAssociation towerBaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"));
                        if ((towerBaseFileAssociation == null && addressingEditorObject.TowerBaseFileIdList != "") || (towerBaseFileAssociation != null && addressingEditorObject.TowerBaseFileIdList != towerBaseFileAssociation.FileIdList) || addressingEditorObject.TowerBaseType != (int)towerBase.TowerBaseType || addressingEditorObject.TowerBaseBudget != towerBase.BudgetPrice || addressingEditorObject.TowerBaseTimeLimit != towerBase.TimeLimit)
                        {
                            towerBase.Modify((TowerBaseType)addressingEditorObject.TowerBaseType, addressingEditorObject.TowerBaseBudget, addressingEditorObject.TowerBaseTimeLimit, "", addressingEditorObject.ModifyUserId);
                            towerBaseRepository.Update(towerBase);

                            if (towerBaseFileAssociation == null && addressingEditorObject.TowerBaseFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("TowerBase", towerBase.Id, addressingEditorObject.TowerBaseFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (towerBaseFileAssociation != null && addressingEditorObject.TowerBaseFileIdList != towerBaseFileAssociation.FileIdList)
                            {
                                towerBaseFileAssociation.Modify(addressingEditorObject.TowerBaseFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(towerBaseFileAssociation);
                            }

                            TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.修改, towerBase.ParentId, towerBase.PropertyType, (TowerBaseType)addressingEditorObject.TowerBaseType, addressingEditorObject.TowerBaseBudget, addressingEditorObject.TowerBaseTimeLimit, "", addressingEditorObject.ModifyUserId);
                            towerBaseLogRepository.Add(towerBaseLog);
                            FileAssociation towerBaseLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerBaseLog", towerBaseLog.Id, addressingEditorObject.TowerBaseFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(towerBaseLogFileAssociation);
                        }
                    }
                    else
                    {
                        TowerBase towerBase = AggregateFactory.CreateTowerBase(addressing.Id, PropertyType.寻址设计, (TowerBaseType)addressingEditorObject.TowerBaseType, addressingEditorObject.TowerBaseBudget, addressingEditorObject.TowerBaseTimeLimit, "", addressingEditorObject.ModifyUserId);
                        towerBaseRepository.Add(towerBase);
                        FileAssociation towerBaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"));
                        if (towerBaseFileAssociation == null && addressingEditorObject.TowerBaseFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("TowerBase", towerBase.Id, addressingEditorObject.TowerBaseFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (towerBaseFileAssociation != null && addressingEditorObject.TowerBaseFileIdList != towerBaseFileAssociation.FileIdList)
                        {
                            towerBaseFileAssociation.Modify(addressingEditorObject.TowerBaseFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(towerBaseFileAssociation);
                        }

                        TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.新增, towerBase.ParentId, towerBase.PropertyType, (TowerBaseType)addressingEditorObject.TowerBaseType, addressingEditorObject.TowerBaseBudget, addressingEditorObject.TowerBaseTimeLimit, "", addressingEditorObject.ModifyUserId);
                        towerBaseLogRepository.Add(towerBaseLog);
                        FileAssociation towerBaseLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerBaseLog", towerBaseLog.Id, addressingEditorObject.TowerBaseFileIdList, addressingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(towerBaseLogFileAssociation);
                    }
                }
                else
                {
                    TowerBase towerBaseCheck = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (towerBaseCheck != null)
                    {
                        TowerBase towerBase = towerBaseRepository.FindByKey(towerBaseCheck.Id);
                        if (towerBase != null)
                        {
                            TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.删除, towerBase.ParentId, towerBase.PropertyType, towerBase.TowerBaseType, towerBase.BudgetPrice, towerBase.TimeLimit, "", addressingEditorObject.ModifyUserId);
                            towerBaseLogRepository.Add(towerBaseLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation towerBaseLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerBaseLog", towerBaseLog.Id, fileAssociation.FileIdList, addressingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(towerBaseLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            towerBaseRepository.Remove(towerBase);
                        }
                    }
                }

                if (addressingEditorObject.MachineRoomMark == 1)
                {
                    MachineRoom machineRoomCheck = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (machineRoomCheck != null)
                    {
                        MachineRoom machineRoom = machineRoomRepository.FindByKey(machineRoomCheck.Id);
                        FileAssociation machineRoomFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"));
                        if ((machineRoomFileAssociation == null && addressingEditorObject.MachineRoomFileIdList != "") || (machineRoomFileAssociation != null && addressingEditorObject.MachineRoomFileIdList != machineRoomFileAssociation.FileIdList) || (MachineRoomType)addressingEditorObject.MachineRoomType != machineRoom.MachineRoomType || addressingEditorObject.MachineRoomArea != machineRoom.MachineRoomArea || addressingEditorObject.MachineRoomBudget != machineRoom.BudgetPrice || addressingEditorObject.MachineRoomTimeLimit != machineRoom.TimeLimit)
                        {
                            machineRoom.Modify((MachineRoomType)addressingEditorObject.MachineRoomType, addressingEditorObject.MachineRoomArea, addressingEditorObject.MachineRoomBudget, addressingEditorObject.MachineRoomTimeLimit, "", addressingEditorObject.ModifyUserId);
                            machineRoomRepository.Update(machineRoom);

                            if (machineRoomFileAssociation == null && addressingEditorObject.MachineRoomFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoom", machineRoom.Id, addressingEditorObject.MachineRoomFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (machineRoomFileAssociation != null && addressingEditorObject.MachineRoomFileIdList != machineRoomFileAssociation.FileIdList)
                            {
                                machineRoomFileAssociation.Modify(addressingEditorObject.MachineRoomFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(machineRoomFileAssociation);
                            }

                            MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.修改, machineRoom.ParentId, machineRoom.PropertyType, (MachineRoomType)addressingEditorObject.MachineRoomType, addressingEditorObject.MachineRoomArea, addressingEditorObject.MachineRoomBudget, addressingEditorObject.MachineRoomTimeLimit, "", addressingEditorObject.ModifyUserId);
                            machineRoomLogRepository.Add(machineRoomLog);
                            FileAssociation machineRoomLogFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoomLog", machineRoomLog.Id, addressingEditorObject.MachineRoomFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(machineRoomLogFileAssociation);
                        }
                    }
                    else
                    {
                        MachineRoom machineRoom = AggregateFactory.CreateMachineRoom(addressing.Id, PropertyType.寻址设计, (MachineRoomType)addressingEditorObject.MachineRoomType, addressingEditorObject.MachineRoomArea, addressingEditorObject.MachineRoomBudget, addressingEditorObject.MachineRoomTimeLimit, "", addressingEditorObject.ModifyUserId);
                        machineRoomRepository.Add(machineRoom);
                        FileAssociation machineRoomFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"));
                        if (machineRoomFileAssociation == null && addressingEditorObject.MachineRoomFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoom", machineRoom.Id, addressingEditorObject.MachineRoomFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (machineRoomFileAssociation != null && addressingEditorObject.MachineRoomFileIdList != machineRoomFileAssociation.FileIdList)
                        {
                            machineRoomFileAssociation.Modify(addressingEditorObject.MachineRoomFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(machineRoomFileAssociation);
                        }

                        MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.新增, machineRoom.ParentId, machineRoom.PropertyType, (MachineRoomType)addressingEditorObject.MachineRoomType, addressingEditorObject.MachineRoomArea, addressingEditorObject.MachineRoomBudget, addressingEditorObject.MachineRoomTimeLimit, "", addressingEditorObject.ModifyUserId);
                        machineRoomLogRepository.Add(machineRoomLog);
                        FileAssociation machineRoomLogFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoomLog", machineRoomLog.Id, addressingEditorObject.MachineRoomFileIdList, addressingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(machineRoomLogFileAssociation);

                        placeDesign.ModifyPropertyMark(TaskModel.机房, Bool.是);
                        placeDesignRepository.Update(placeDesign);
                    }
                }
                else
                {
                    MachineRoom machineRoomCheck = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (machineRoomCheck != null)
                    {
                        MachineRoom machineRoom = machineRoomRepository.FindByKey(machineRoomCheck.Id);
                        if (machineRoom != null)
                        {
                            MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.删除, machineRoom.ParentId, machineRoom.PropertyType, machineRoom.MachineRoomType, machineRoom.MachineRoomArea, machineRoom.BudgetPrice, machineRoom.TimeLimit, "", addressingEditorObject.ModifyUserId);
                            machineRoomLogRepository.Add(machineRoomLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation machineRoomLogFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoomLog", machineRoomLog.Id, fileAssociation.FileIdList, addressingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(machineRoomLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            machineRoomRepository.Remove(machineRoom);
                        }
                        placeDesign.ModifyPropertyMark(TaskModel.机房, Bool.否);
                        placeDesignRepository.Update(placeDesign);
                    }
                }

                if (addressingEditorObject.ExternalElectricPowerMark == 1)
                {
                    ExternalElectricPower externalElectricPowerCheck = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (externalElectricPowerCheck != null)
                    {
                        ExternalElectricPower externalElectricPower = externalElectricPowerRepository.FindByKey(externalElectricPowerCheck.Id);
                        FileAssociation externalElectricPowerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"));
                        if ((externalElectricPowerFileAssociation == null && addressingEditorObject.ExternalFileIdList != "") || (externalElectricPowerFileAssociation != null && addressingEditorObject.ExternalFileIdList != externalElectricPowerFileAssociation.FileIdList) || (ExternalElectric)addressingEditorObject.ExternalElectric != externalElectricPower.ExternalElectric || addressingEditorObject.ExternalBudget != externalElectricPower.BudgetPrice || addressingEditorObject.ExternalTimeLimit != externalElectricPower.TimeLimit)
                        {
                            externalElectricPower.Modify((ExternalElectric)addressingEditorObject.ExternalElectric, addressingEditorObject.ExternalBudget, addressingEditorObject.ExternalTimeLimit, "", addressingEditorObject.ModifyUserId);
                            externalElectricPowerRepository.Update(externalElectricPower);

                            if (externalElectricPowerFileAssociation == null && addressingEditorObject.ExternalFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPower", externalElectricPower.Id, addressingEditorObject.ExternalFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (externalElectricPowerFileAssociation != null && addressingEditorObject.ExternalFileIdList != externalElectricPowerFileAssociation.FileIdList)
                            {
                                externalElectricPowerFileAssociation.Modify(addressingEditorObject.ExternalFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(externalElectricPowerFileAssociation);
                            }

                            ExternalElectricPowerLog externalElectricPowerLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.修改, externalElectricPower.ParentId, externalElectricPower.PropertyType, (ExternalElectric)addressingEditorObject.ExternalElectric, addressingEditorObject.ExternalBudget, addressingEditorObject.ExternalTimeLimit, "", addressingEditorObject.ModifyUserId);
                            externalElectricPowerLogRepository.Add(externalElectricPowerLog);
                            FileAssociation externalElectricPowerLogFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPowerLog", externalElectricPowerLog.Id, addressingEditorObject.ExternalFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(externalElectricPowerLogFileAssociation);
                        }
                    }
                    else
                    {
                        ExternalElectricPower externalElectricPower = AggregateFactory.CreateExternalElectricPower(addressing.Id, PropertyType.寻址设计, (ExternalElectric)addressingEditorObject.ExternalElectric, addressingEditorObject.ExternalBudget, addressingEditorObject.ExternalTimeLimit, "", addressingEditorObject.ModifyUserId);
                        externalElectricPowerRepository.Add(externalElectricPower);
                        FileAssociation externalElectricPowerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"));
                        if (externalElectricPowerFileAssociation == null && addressingEditorObject.ExternalFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPower", externalElectricPower.Id, addressingEditorObject.ExternalFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (externalElectricPowerFileAssociation != null && addressingEditorObject.ExternalFileIdList != externalElectricPowerFileAssociation.FileIdList)
                        {
                            externalElectricPowerFileAssociation.Modify(addressingEditorObject.ExternalFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(externalElectricPowerFileAssociation);
                        }

                        ExternalElectricPowerLog externalElectricPowerLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.新增, externalElectricPower.ParentId, externalElectricPower.PropertyType, (ExternalElectric)addressingEditorObject.ExternalElectric, addressingEditorObject.ExternalBudget, addressingEditorObject.ExternalTimeLimit, "", addressingEditorObject.ModifyUserId);
                        externalElectricPowerLogRepository.Add(externalElectricPowerLog);
                        FileAssociation externalElectricPowerLogFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPowerLog", externalElectricPowerLog.Id, addressingEditorObject.ExternalFileIdList, addressingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(externalElectricPowerLogFileAssociation);
                    }
                }
                else
                {
                    ExternalElectricPower externalElectricPowerCheck = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (externalElectricPowerCheck != null)
                    {
                        ExternalElectricPower externalElectricPower = externalElectricPowerRepository.FindByKey(externalElectricPowerCheck.Id);
                        if (externalElectricPower != null)
                        {
                            ExternalElectricPowerLog externalElectricPowerLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.删除, externalElectricPower.ParentId, externalElectricPower.PropertyType, externalElectricPower.ExternalElectric, externalElectricPower.BudgetPrice, externalElectricPower.TimeLimit, "", addressingEditorObject.ModifyUserId);
                            externalElectricPowerLogRepository.Add(externalElectricPowerLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation externalElectricPowerLogFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPowerLog", externalElectricPowerLog.Id, fileAssociation.FileIdList, addressingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(externalElectricPowerLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            externalElectricPowerRepository.Remove(externalElectricPower);
                        }
                    }
                }

                if (addressingEditorObject.EquipmentInstallMark == 1)
                {
                    EquipmentInstall equipmentInstallCheck = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (equipmentInstallCheck != null)
                    {
                        EquipmentInstall equipmentInstall = equipmentInstallRepository.FindByKey(equipmentInstallCheck.Id);
                        FileAssociation equipmentInstallFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"));
                        if ((equipmentInstallFileAssociation == null && addressingEditorObject.EquipmentInstallFileIdList != "") || (equipmentInstallFileAssociation != null && addressingEditorObject.EquipmentInstallFileIdList != equipmentInstallFileAssociation.FileIdList) || addressingEditorObject.SwitchPower != equipmentInstall.SwitchPower || addressingEditorObject.Battery != equipmentInstall.Battery || addressingEditorObject.CabinetNumber != equipmentInstall.CabinetNumber || addressingEditorObject.EquipmentBudget != equipmentInstall.BudgetPrice || addressingEditorObject.EquipmentTimeLimit != equipmentInstall.TimeLimit)
                        {
                            equipmentInstall.Modify(addressingEditorObject.SwitchPower, addressingEditorObject.Battery, addressingEditorObject.CabinetNumber, addressingEditorObject.EquipmentBudget, addressingEditorObject.EquipmentTimeLimit, "", addressingEditorObject.ModifyUserId);
                            equipmentInstallRepository.Update(equipmentInstall);

                            if (equipmentInstallFileAssociation == null && addressingEditorObject.EquipmentInstallFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstall", equipmentInstall.Id, addressingEditorObject.EquipmentInstallFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (equipmentInstallFileAssociation != null && addressingEditorObject.EquipmentInstallFileIdList != equipmentInstallFileAssociation.FileIdList)
                            {
                                equipmentInstallFileAssociation.Modify(addressingEditorObject.EquipmentInstallFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(equipmentInstallFileAssociation);
                            }

                            EquipmentInstallLog equipmentInstallLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.修改, equipmentInstall.ParentId, equipmentInstall.PropertyType, addressingEditorObject.SwitchPower, addressingEditorObject.Battery, addressingEditorObject.CabinetNumber, addressingEditorObject.EquipmentBudget, addressingEditorObject.EquipmentTimeLimit, "", addressingEditorObject.ModifyUserId);
                            equipmentInstallLogRepository.Add(equipmentInstallLog);
                            FileAssociation equipmentInstallLogFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstallLog", equipmentInstallLog.Id, addressingEditorObject.EquipmentInstallFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(equipmentInstallLogFileAssociation);
                        }
                    }
                    else
                    {
                        EquipmentInstall equipmentInstall = AggregateFactory.CreateEquipmentInstall(addressing.Id, PropertyType.寻址设计, addressingEditorObject.SwitchPower, addressingEditorObject.Battery, addressingEditorObject.CabinetNumber, addressingEditorObject.EquipmentBudget, addressingEditorObject.EquipmentTimeLimit, "", addressingEditorObject.ModifyUserId);
                        equipmentInstallRepository.Add(equipmentInstall);
                        FileAssociation equipmentInstallFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"));
                        if (equipmentInstallFileAssociation == null && addressingEditorObject.EquipmentInstallFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstall", equipmentInstall.Id, addressingEditorObject.EquipmentInstallFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (equipmentInstallFileAssociation != null && addressingEditorObject.EquipmentInstallFileIdList != equipmentInstallFileAssociation.FileIdList)
                        {
                            equipmentInstallFileAssociation.Modify(addressingEditorObject.EquipmentInstallFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(equipmentInstallFileAssociation);
                        }

                        EquipmentInstallLog equipmentInstallLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.新增, equipmentInstall.ParentId, equipmentInstall.PropertyType, addressingEditorObject.SwitchPower, addressingEditorObject.Battery, addressingEditorObject.CabinetNumber, addressingEditorObject.EquipmentBudget, addressingEditorObject.EquipmentTimeLimit, "", addressingEditorObject.ModifyUserId);
                        equipmentInstallLogRepository.Add(equipmentInstallLog);
                        FileAssociation equipmentInstallLogFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstallLog", equipmentInstallLog.Id, addressingEditorObject.EquipmentInstallFileIdList, addressingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(equipmentInstallLogFileAssociation);
                    }
                }
                else
                {
                    EquipmentInstall equipmentInstallCheck = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (equipmentInstallCheck != null)
                    {
                        EquipmentInstall equipmentInstall = equipmentInstallRepository.FindByKey(equipmentInstallCheck.Id);
                        if (equipmentInstall != null)
                        {
                            EquipmentInstallLog equipmentInstallLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.删除, equipmentInstall.ParentId, equipmentInstall.PropertyType, equipmentInstall.SwitchPower, equipmentInstall.Battery, equipmentInstall.CabinetNumber, equipmentInstall.BudgetPrice, addressingEditorObject.EquipmentTimeLimit, "", addressingEditorObject.ModifyUserId);
                            equipmentInstallLogRepository.Add(equipmentInstallLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation equipmentInstallLogFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstallLog", equipmentInstallLog.Id, fileAssociation.FileIdList, addressingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(equipmentInstallLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                        }
                        equipmentInstallRepository.Remove(equipmentInstall);
                    }
                }

                if (addressingEditorObject.AddressExplorMark == 1)
                {
                    AddressExplor addressExplorCheck = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (addressExplorCheck != null)
                    {
                        AddressExplor addressExplor = addressExplorRepository.FindByKey(addressExplorCheck.Id);
                        FileAssociation addressExplorFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"));
                        if ((addressExplorFileAssociation == null && addressingEditorObject.AddressFileIdList != "") || (addressExplorFileAssociation != null && addressingEditorObject.AddressFileIdList != addressExplorFileAssociation.FileIdList) || addressingEditorObject.AddressBudget != addressExplor.BudgetPrice || addressingEditorObject.AddressTimeLimit != addressExplor.TimeLimit)
                        {
                            addressExplor.Modify(addressingEditorObject.AddressBudget, addressingEditorObject.AddressTimeLimit, "", addressingEditorObject.ModifyUserId);
                            addressExplorRepository.Update(addressExplor);

                            if (addressExplorFileAssociation == null && addressingEditorObject.AddressFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplor", addressExplor.Id, addressingEditorObject.AddressFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (addressExplorFileAssociation != null && addressingEditorObject.AddressFileIdList != addressExplorFileAssociation.FileIdList)
                            {
                                addressExplorFileAssociation.Modify(addressingEditorObject.AddressFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(addressExplorFileAssociation);
                            }

                            AddressExplorLog addressExplorLog = AggregateFactory.CreateAddressExplorLog(OperationType.修改, addressExplor.ParentId, addressExplor.PropertyType, addressingEditorObject.AddressBudget, addressingEditorObject.AddressTimeLimit, "", addressingEditorObject.ModifyUserId);
                            addressExplorLogRepository.Add(addressExplorLog);
                            FileAssociation addressExplorLogFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplorLog", addressExplorLog.Id, addressingEditorObject.AddressFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(addressExplorLogFileAssociation);
                        }
                    }
                    else
                    {
                        AddressExplor addressExplor = AggregateFactory.CreateAddressExplor(addressing.Id, PropertyType.寻址设计, addressingEditorObject.AddressBudget, addressingEditorObject.AddressTimeLimit, "", addressingEditorObject.ModifyUserId);
                        addressExplorRepository.Add(addressExplor);
                        FileAssociation addressExplorFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"));
                        if (addressExplorFileAssociation == null && addressingEditorObject.AddressFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplor", addressExplor.Id, addressingEditorObject.AddressFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (addressExplorFileAssociation != null && addressingEditorObject.AddressFileIdList != addressExplorFileAssociation.FileIdList)
                        {
                            addressExplorFileAssociation.Modify(addressingEditorObject.AddressFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(addressExplorFileAssociation);
                        }

                        AddressExplorLog addressExplorLog = AggregateFactory.CreateAddressExplorLog(OperationType.新增, addressExplor.ParentId, addressExplor.PropertyType, addressingEditorObject.AddressBudget, addressingEditorObject.AddressTimeLimit, "", addressingEditorObject.ModifyUserId);
                        addressExplorLogRepository.Add(addressExplorLog);
                        FileAssociation addressExplorLogFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplorLog", addressExplorLog.Id, addressingEditorObject.AddressFileIdList, addressingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(addressExplorLogFileAssociation);
                    }
                }
                else
                {
                    AddressExplor addressExplorCheck = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (addressExplorCheck != null)
                    {
                        AddressExplor addressExplor = addressExplorRepository.FindByKey(addressExplorCheck.Id);
                        if (addressExplor != null)
                        {
                            AddressExplorLog addressExplorLog = AggregateFactory.CreateAddressExplorLog(OperationType.删除, addressExplor.ParentId, addressExplor.PropertyType, addressExplor.BudgetPrice, addressingEditorObject.AddressTimeLimit, "", addressingEditorObject.ModifyUserId);
                            addressExplorLogRepository.Add(addressExplorLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation addressExplorLogFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplorLog", addressExplorLog.Id, fileAssociation.FileIdList, addressingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(addressExplorLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            addressExplorRepository.Remove(addressExplor);
                        }
                    }
                }

                if (addressingEditorObject.FoundationTestMark == 1)
                {
                    FoundationTest foundationTestCheck = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (foundationTestCheck != null)
                    {
                        FoundationTest foundationTest = foundationTestRepository.FindByKey(foundationTestCheck.Id);
                        FileAssociation foundationTestFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"));
                        if ((foundationTestFileAssociation == null && addressingEditorObject.FoundationFileIdList != "") || (foundationTestFileAssociation != null && addressingEditorObject.FoundationFileIdList != foundationTestFileAssociation.FileIdList) || addressingEditorObject.FoundationBudget != foundationTest.BudgetPrice || addressingEditorObject.FoundationTimeLimit != foundationTest.TimeLimit)
                        {
                            foundationTest.Modify(addressingEditorObject.FoundationBudget, addressingEditorObject.FoundationTimeLimit, "", addressingEditorObject.ModifyUserId);
                            foundationTestRepository.Update(foundationTest);

                            if (foundationTestFileAssociation == null && addressingEditorObject.FoundationFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTest", foundationTest.Id, addressingEditorObject.FoundationFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (foundationTestFileAssociation != null && addressingEditorObject.FoundationFileIdList != foundationTestFileAssociation.FileIdList)
                            {
                                foundationTestFileAssociation.Modify(addressingEditorObject.FoundationFileIdList, addressingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(foundationTestFileAssociation);
                            }

                            FoundationTestLog foundationTestLog = AggregateFactory.CreateFoundationTestLog(OperationType.修改, foundationTest.ParentId, foundationTest.PropertyType, addressingEditorObject.FoundationBudget, addressingEditorObject.FoundationTimeLimit, "", addressingEditorObject.ModifyUserId);
                            foundationTestLogRepository.Add(foundationTestLog);
                            FileAssociation foundationTestLogFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTestLog", foundationTestLog.Id, addressingEditorObject.FoundationFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(foundationTestLogFileAssociation);
                        }
                    }
                    else
                    {
                        FoundationTest foundationTest = AggregateFactory.CreateFoundationTest(addressing.Id, PropertyType.寻址设计, addressingEditorObject.FoundationBudget, addressingEditorObject.FoundationTimeLimit, "", addressingEditorObject.ModifyUserId);
                        foundationTestRepository.Add(foundationTest);
                        FileAssociation foundationTestFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"));
                        if (foundationTestFileAssociation == null && addressingEditorObject.FoundationFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTest", foundationTest.Id, addressingEditorObject.FoundationFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (foundationTestFileAssociation != null && addressingEditorObject.FoundationFileIdList != foundationTestFileAssociation.FileIdList)
                        {
                            foundationTestFileAssociation.Modify(addressingEditorObject.FoundationFileIdList, addressingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(foundationTestFileAssociation);
                        }

                        FoundationTestLog foundationTestLog = AggregateFactory.CreateFoundationTestLog(OperationType.新增, foundationTest.ParentId, foundationTest.PropertyType, addressingEditorObject.FoundationBudget, addressingEditorObject.FoundationTimeLimit, "", addressingEditorObject.ModifyUserId);
                        foundationTestLogRepository.Add(foundationTestLog);
                        FileAssociation foundationTestLogFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTestLog", foundationTestLog.Id, addressingEditorObject.FoundationFileIdList, addressingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(foundationTestLogFileAssociation);
                    }
                }
                else
                {
                    FoundationTest foundationTestCheck = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    if (foundationTestCheck != null)
                    {
                        FoundationTest foundationTest = foundationTestRepository.FindByKey(foundationTestCheck.Id);
                        if (foundationTest != null)
                        {
                            FoundationTestLog foundationTestLog = AggregateFactory.CreateFoundationTestLog(OperationType.删除, foundationTest.ParentId, foundationTest.PropertyType, foundationTest.BudgetPrice, addressingEditorObject.FoundationTimeLimit, "", addressingEditorObject.ModifyUserId);
                            foundationTestLogRepository.Add(foundationTestLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation foundationTestLogFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTestLog", foundationTestLog.Id, fileAssociation.FileIdList, addressingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(foundationTestLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            foundationTestRepository.Remove(foundationTest);
                        }
                    }
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == addressingEditorObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(addressingEditorObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("FK_dbo.tbl_PlaceDesign_dbo.tbl_Customer_DesignCustomerId"))
                //{
                //    throw new ApplicationFault("选择的设计单位在系统中不存在");
                //}
                //if (ex.Message.Contains("FK_dbo.tbl_PlaceDesign_dbo.tbl_User_DesignUserId"))
                //{
                //    throw new ApplicationFault("选择的设计人员在系统中不存在");
                //}
                throw ex;
            }
        }

        /// <summary>
        /// 指定施工单位
        /// </summary>
        /// <param name="addressingEditorObject"></param>
        public void SaveCustomer(AddressingEditorObject addressingEditorObject)
        {
            Addressing addressing = addressingRepository.FindByKey(addressingEditorObject.Id);
            PlaceDesign placeDesign = placeDesignRepository.Find(Specification<PlaceDesign>.Eval(Entity => Entity.ParentId == addressing.Id && Entity.PropertyType == PropertyType.寻址设计));
            if (addressing != null)
            {
                if (placeDesign.TowerMark == Bool.是)
                {
                    Tower tower = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    tower.ModifyCustomer(addressingEditorObject.TowerCustomerId);
                    towerRepository.Update(tower);
                }

                if (placeDesign.TowerBaseMark == Bool.是)
                {
                    TowerBase towerBase = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    towerBase.ModifyCustomer(addressingEditorObject.TowerBaseCustomerId);
                    towerBaseRepository.Update(towerBase);
                }

                if (placeDesign.MachineRoomMark == Bool.是)
                {
                    MachineRoom machineRoom = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    machineRoom.ModifyCustomer(addressingEditorObject.MachineRoomCustomerId);
                    machineRoomRepository.Update(machineRoom);
                }

                if (placeDesign.ExternalElectricPowerMark == Bool.是)
                {
                    ExternalElectricPower externalElectricPower = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    externalElectricPower.ModifyCustomer(addressingEditorObject.ExternalCustomerId);
                    externalElectricPowerRepository.Update(externalElectricPower);
                }

                if (placeDesign.EquipmentInstallMark == Bool.是)
                {
                    EquipmentInstall equipmentInstall = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    equipmentInstall.ModifyCustomer(addressingEditorObject.EquipmentCustomerId);
                    equipmentInstallRepository.Update(equipmentInstall);
                }

                if (placeDesign.AddressExplorMark == Bool.是)
                {
                    AddressExplor addressExplor = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    addressExplor.ModifyCustomer(addressingEditorObject.AddressCustomerId);
                    addressExplorRepository.Update(addressExplor);
                }

                if (placeDesign.FoundationTestMark == Bool.是)
                {
                    FoundationTest foundationTest = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == addressingEditorObject.Id && entity.PropertyType == PropertyType.寻址设计));
                    foundationTest.ModifyCustomer(addressingEditorObject.FoundationCustomerId);
                    foundationTestRepository.Update(foundationTest);
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == addressingEditorObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(addressingEditorObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
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
        /// 指定工程经理及监理单位
        /// </summary>
        /// <param name="placeDesignMaintObject"></param>
        public void SaveManagerAndSupervisor(PlaceDesignMaintObject placeDesignMaintObject)
        {
            PlaceDesign placeDesign = placeDesignRepository.FindByKey(placeDesignMaintObject.Id);
            Addressing addressing = addressingRepository.FindByKey(placeDesign.ParentId);
            if (addressing != null)
            {
                Customer customer = customerRepository.FindByKey(placeDesignMaintObject.SupervisorCustomerId.Value);
                if (customer != null)
                {
                    if (customer.CustomerUserId == Guid.Empty)
                    {
                        throw new ApplicationFault("请选择已关联登陆人的监理单位");
                    }
                }
                placeDesign.AppointSupervisor(placeDesignMaintObject.SupervisorCustomerId.Value, customer.CustomerUserId);
                placeDesignRepository.Update(placeDesign);

                //addressing.AppointProjectManagerId(placeDesignMaintObject.ProjectManagerId.Value, placeDesignMaintObject.ModifyUserId);
                //addressingRepository.Update(addressing);

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == placeDesignMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(placeDesignMaintObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
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
        /// 改造站指定设计单位
        /// </summary>
        /// <param name="placeDesignMaintObject"></param>
        public void SaveAppointDesignR(PlaceDesignMaintObject placeDesignMaintObject)
        {
            PlaceDesign placeDesign = placeDesignRepository.FindByKey(placeDesignMaintObject.Id);
            Remodeling remodeling = remodelingRepository.FindByKey(placeDesign.ParentId);
            if (remodeling != null)
            {
                Customer customer = customerRepository.FindByKey(placeDesignMaintObject.DesignCustomerId.Value);
                if (customer != null)
                {
                    if (customer.CustomerUserId == Guid.Empty)
                    {
                        throw new ApplicationFault("请选择已关联登陆人的设计单位");
                    }
                }
                placeDesign.AppointDesign(placeDesignMaintObject.DesignCustomerId.Value);
                placeDesignRepository.Update(placeDesign);

                WFActivityInstance wfActivityInstance = wfActivityInstanceRepository.FindByKey(placeDesignMaintObject.WFActivityInstanceId);

                IEnumerable<WFActivityInstance> wfActivityInstances = wfActivityInstanceRepository.FindAll(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfActivityInstance.WFProcessInstanceId &&
                    entity.SerialId >= wfActivityInstance.SerialId && entity.Id != wfActivityInstance.Id && entity.WFActivityInstanceState == WFActivityInstanceState.未处理 && entity.WFActivityOperate == WFActivityOperate.单据编辑 && entity.WFActivityEditorId.ToString() == "95154645-69f3-4c49-95b1-dc77b8c4c962"));
                if (wfActivityInstances != null)
                {
                    foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances)
                    {
                        modifyWFActivityInstance.UserId = customer.CustomerUserId;
                        wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                    }
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == placeDesignMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(placeDesignMaintObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("FK_dbo.tbl_PlaceDesign_dbo.tbl_Customer_DesignCustomerId"))
                //{
                //    throw new ApplicationFault("选择的设计单位在系统中不存在");
                //}
                //if (ex.Message.Contains("FK_dbo.tbl_PlaceDesign_dbo.tbl_User_DesignUserId"))
                //{
                //    throw new ApplicationFault("选择的设计人员在系统中不存在");
                //}
                throw ex;
            }
        }

        /// <summary>
        /// 改造站指定设计人员
        /// </summary>
        /// <param name="placeDesignMaintObject"></param>
        public void SaveAppointDesignUserR(PlaceDesignMaintObject placeDesignMaintObject)
        {
            PlaceDesign placeDesign = placeDesignRepository.FindByKey(placeDesignMaintObject.Id);
            Remodeling remodeling = remodelingRepository.FindByKey(placeDesign.ParentId);
            if (remodeling != null)
            {
                placeDesign.AppointDesignUser(placeDesignMaintObject.DesignUserId.Value);
                placeDesignRepository.Update(placeDesign);

                WFActivityInstance wfActivityInstance = wfActivityInstanceRepository.FindByKey(placeDesignMaintObject.WFActivityInstanceId);

                IEnumerable<WFActivityInstance> wfActivityInstances = wfActivityInstanceRepository.FindAll(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfActivityInstance.WFProcessInstanceId &&
                    entity.SerialId >= wfActivityInstance.SerialId && entity.Id != wfActivityInstance.Id && entity.WFActivityInstanceState == WFActivityInstanceState.未处理 && entity.WFActivityOperate == WFActivityOperate.单据编辑 && entity.WFActivityEditorId.ToString() == "f11adb5a-ed98-4320-80b1-d000f60c9bcf"));
                if (wfActivityInstances != null)
                {
                    foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances)
                    {
                        modifyWFActivityInstance.UserId = placeDesignMaintObject.DesignUserId.Value;
                        wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                    }
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == placeDesignMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(placeDesignMaintObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("FK_dbo.tbl_PlaceDesign_dbo.tbl_Customer_DesignCustomerId"))
                //{
                //    throw new ApplicationFault("选择的设计单位在系统中不存在");
                //}
                //if (ex.Message.Contains("FK_dbo.tbl_PlaceDesign_dbo.tbl_User_DesignUserId"))
                //{
                //    throw new ApplicationFault("选择的设计人员在系统中不存在");
                //}
                throw ex;
            }
        }

        /// <summary>
        /// 改造站保存施工设计
        /// </summary>
        /// <param name="remodelingEditorObject"></param>
        public void SaveConstructionDesignR(RemodelingEditorObject remodelingEditorObject)
        {
            Remodeling remodeling = remodelingRepository.FindByKey(remodelingEditorObject.Id);
            PlaceDesign placeDesign = placeDesignRepository.Find(Specification<PlaceDesign>.Eval(Entity => Entity.ParentId == remodeling.Id && Entity.PropertyType == PropertyType.改造设计));
            if (remodeling != null)
            {
                if (remodelingEditorObject.TowerMark == 1)
                {
                    Tower towerCheck = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (towerCheck != null)
                    {
                        Tower tower = towerRepository.FindByKey(towerCheck.Id);
                        FileAssociation towerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"));
                        if ((towerFileAssociation == null && remodelingEditorObject.TowerFileIdList != "") || (towerFileAssociation != null && remodelingEditorObject.TowerFileIdList != towerFileAssociation.FileIdList) || (TowerType)remodelingEditorObject.TowerType != tower.TowerType || remodelingEditorObject.TowerHeight != tower.TowerHeight || remodelingEditorObject.PlatFormNumber != tower.PlatFormNumber || remodelingEditorObject.PoleNumber != tower.PoleNumber || remodelingEditorObject.TowerBudget != tower.BudgetPrice || remodelingEditorObject.TowerTimeLimit != tower.TimeLimit)
                        {
                            tower.Modify((TowerType)remodelingEditorObject.TowerType, remodelingEditorObject.TowerHeight, remodelingEditorObject.PlatFormNumber, remodelingEditorObject.PoleNumber, remodelingEditorObject.TowerBudget, remodelingEditorObject.TowerTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            towerRepository.Update(tower);

                            if (towerFileAssociation == null && remodelingEditorObject.TowerFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("Tower", tower.Id, remodelingEditorObject.TowerFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (towerFileAssociation != null && remodelingEditorObject.TowerFileIdList != towerFileAssociation.FileIdList)
                            {
                                towerFileAssociation.Modify(remodelingEditorObject.TowerFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(towerFileAssociation);
                            }

                            TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.修改, tower.ParentId, tower.PropertyType, (TowerType)remodelingEditorObject.TowerType, remodelingEditorObject.TowerHeight, remodelingEditorObject.PlatFormNumber, remodelingEditorObject.PoleNumber, remodelingEditorObject.TowerBudget, remodelingEditorObject.TowerTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            towerLogRepository.Add(towerLog);
                            FileAssociation towerLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerLog", towerLog.Id, remodelingEditorObject.TowerFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(towerLogFileAssociation);
                        }
                    }
                    else
                    {
                        Tower tower = AggregateFactory.CreateTower(remodeling.Id, PropertyType.改造设计, (TowerType)remodelingEditorObject.TowerType, remodelingEditorObject.TowerHeight, remodelingEditorObject.PlatFormNumber, remodelingEditorObject.PoleNumber, remodelingEditorObject.TowerBudget, remodelingEditorObject.TowerTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        towerRepository.Add(tower);
                        FileAssociation towerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"));
                        if (towerFileAssociation == null && remodelingEditorObject.TowerFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("Tower", tower.Id, remodelingEditorObject.TowerFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (towerFileAssociation != null && remodelingEditorObject.TowerFileIdList != towerFileAssociation.FileIdList)
                        {
                            towerFileAssociation.Modify(remodelingEditorObject.TowerFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(towerFileAssociation);
                        }

                        TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.新增, tower.ParentId, tower.PropertyType, (TowerType)remodelingEditorObject.TowerType, remodelingEditorObject.TowerHeight, remodelingEditorObject.PlatFormNumber, remodelingEditorObject.PoleNumber, remodelingEditorObject.TowerBudget, remodelingEditorObject.TowerTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        towerLogRepository.Add(towerLog);
                        FileAssociation towerLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerLog", towerLog.Id, remodelingEditorObject.TowerFileIdList, remodelingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(towerLogFileAssociation);
                    }
                }
                else
                {
                    Tower towerCheck = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (towerCheck != null)
                    {
                        Tower tower = towerRepository.FindByKey(towerCheck.Id);
                        if (tower != null)
                        {
                            TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.删除, tower.ParentId, tower.PropertyType, tower.TowerType, tower.TowerHeight, tower.PlatFormNumber, tower.PoleNumber, tower.BudgetPrice, tower.TimeLimit, "", remodelingEditorObject.ModifyUserId);
                            towerLogRepository.Add(towerLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation towerLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerLog", towerLog.Id, fileAssociation.FileIdList, remodelingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(towerLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            towerRepository.Remove(tower);
                        }
                    }
                }

                if (remodelingEditorObject.TowerBaseMark == 1)
                {
                    TowerBase towerBaseCheck = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (towerBaseCheck != null)
                    {
                        TowerBase towerBase = towerBaseRepository.FindByKey(towerBaseCheck.Id);
                        FileAssociation towerBaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"));
                        if ((towerBaseFileAssociation == null && remodelingEditorObject.TowerBaseFileIdList != "") || (towerBaseFileAssociation != null && remodelingEditorObject.TowerBaseFileIdList != towerBaseFileAssociation.FileIdList) || (TowerBaseType)remodelingEditorObject.TowerBaseType != towerBase.TowerBaseType || remodelingEditorObject.TowerBaseBudget != towerBase.BudgetPrice || remodelingEditorObject.TowerBaseTimeLimit != towerBase.TimeLimit)
                        {
                            towerBase.Modify((TowerBaseType)remodelingEditorObject.TowerBaseType, remodelingEditorObject.TowerBaseBudget, remodelingEditorObject.TowerBaseTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            towerBaseRepository.Update(towerBase);

                            if (towerBaseFileAssociation == null && remodelingEditorObject.TowerBaseFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("TowerBase", towerBase.Id, remodelingEditorObject.TowerBaseFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (towerBaseFileAssociation != null && remodelingEditorObject.TowerBaseFileIdList != towerBaseFileAssociation.FileIdList)
                            {
                                towerBaseFileAssociation.Modify(remodelingEditorObject.TowerBaseFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(towerBaseFileAssociation);
                            }

                            TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.修改, towerBase.ParentId, towerBase.PropertyType, (TowerBaseType)remodelingEditorObject.TowerBaseType, remodelingEditorObject.TowerBaseBudget, remodelingEditorObject.TowerBaseTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            towerBaseLogRepository.Add(towerBaseLog);
                            FileAssociation towerBaseLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerBaseLog", towerBaseLog.Id, remodelingEditorObject.TowerBaseFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(towerBaseLogFileAssociation);
                        }
                    }
                    else
                    {
                        TowerBase towerBase = AggregateFactory.CreateTowerBase(remodeling.Id, PropertyType.改造设计, (TowerBaseType)remodelingEditorObject.TowerBaseType, remodelingEditorObject.TowerBaseBudget, remodelingEditorObject.TowerBaseTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        towerBaseRepository.Add(towerBase);
                        FileAssociation towerBaseFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"));
                        if (towerBaseFileAssociation == null && remodelingEditorObject.TowerBaseFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("TowerBase", towerBase.Id, remodelingEditorObject.TowerBaseFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (towerBaseFileAssociation != null && remodelingEditorObject.TowerBaseFileIdList != towerBaseFileAssociation.FileIdList)
                        {
                            towerBaseFileAssociation.Modify(remodelingEditorObject.TowerBaseFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(towerBaseFileAssociation);
                        }

                        TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.新增, towerBase.ParentId, towerBase.PropertyType, (TowerBaseType)remodelingEditorObject.TowerBaseType, remodelingEditorObject.TowerBaseBudget, remodelingEditorObject.TowerBaseTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        towerBaseLogRepository.Add(towerBaseLog);
                        FileAssociation towerBaseLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerBaseLog", towerBaseLog.Id, remodelingEditorObject.TowerBaseFileIdList, remodelingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(towerBaseLogFileAssociation);
                    }
                }
                else
                {
                    TowerBase towerBaseCheck = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (towerBaseCheck != null)
                    {
                        TowerBase towerBase = towerBaseRepository.FindByKey(towerBaseCheck.Id);
                        if (towerBase != null)
                        {
                            TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.删除, towerBase.ParentId, towerBase.PropertyType, towerBase.TowerBaseType, towerBase.BudgetPrice, towerBase.TimeLimit, "", remodelingEditorObject.ModifyUserId);
                            towerBaseLogRepository.Add(towerBaseLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation towerBaseLogFileAssociation = AggregateFactory.CreateFileAssociation("TowerBaseLog", towerBaseLog.Id, fileAssociation.FileIdList, remodelingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(towerBaseLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            towerBaseRepository.Remove(towerBase);
                        }
                    }
                }

                if (remodelingEditorObject.MachineRoomMark == 1)
                {
                    MachineRoom machineRoomCheck = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (machineRoomCheck != null)
                    {
                        MachineRoom machineRoom = machineRoomRepository.FindByKey(machineRoomCheck.Id);
                        FileAssociation machineRoomFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"));
                        if ((machineRoomFileAssociation == null && remodelingEditorObject.MachineRoomFileIdList != "") || (machineRoomFileAssociation != null && remodelingEditorObject.MachineRoomFileIdList != machineRoomFileAssociation.FileIdList) || (MachineRoomType)remodelingEditorObject.MachineRoomType != machineRoom.MachineRoomType || remodelingEditorObject.MachineRoomArea != machineRoom.MachineRoomArea || remodelingEditorObject.MachineRoomBudget != machineRoom.BudgetPrice || remodelingEditorObject.MachineRoomTimeLimit != machineRoom.TimeLimit)
                        {
                            machineRoom.Modify((MachineRoomType)remodelingEditorObject.MachineRoomType, remodelingEditorObject.MachineRoomArea, remodelingEditorObject.MachineRoomBudget, remodelingEditorObject.MachineRoomTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            machineRoomRepository.Update(machineRoom);

                            if (machineRoomFileAssociation == null && remodelingEditorObject.MachineRoomFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoom", machineRoom.Id, remodelingEditorObject.MachineRoomFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (machineRoomFileAssociation != null && remodelingEditorObject.MachineRoomFileIdList != machineRoomFileAssociation.FileIdList)
                            {
                                machineRoomFileAssociation.Modify(remodelingEditorObject.MachineRoomFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(machineRoomFileAssociation);
                            }

                            MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.修改, machineRoom.ParentId, machineRoom.PropertyType, (MachineRoomType)remodelingEditorObject.MachineRoomType, remodelingEditorObject.MachineRoomArea, remodelingEditorObject.MachineRoomBudget, remodelingEditorObject.MachineRoomTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            machineRoomLogRepository.Add(machineRoomLog);
                            FileAssociation machineRoomLogFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoomLog", machineRoomLog.Id, remodelingEditorObject.MachineRoomFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(machineRoomLogFileAssociation);
                        }
                    }
                    else
                    {
                        MachineRoom machineRoom = AggregateFactory.CreateMachineRoom(remodeling.Id, PropertyType.改造设计, (MachineRoomType)remodelingEditorObject.MachineRoomType, remodelingEditorObject.MachineRoomArea, remodelingEditorObject.MachineRoomBudget, remodelingEditorObject.MachineRoomTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        machineRoomRepository.Add(machineRoom);
                        FileAssociation machineRoomFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"));
                        if (machineRoomFileAssociation == null && remodelingEditorObject.MachineRoomFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoom", machineRoom.Id, remodelingEditorObject.MachineRoomFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (machineRoomFileAssociation != null && remodelingEditorObject.MachineRoomFileIdList != machineRoomFileAssociation.FileIdList)
                        {
                            machineRoomFileAssociation.Modify(remodelingEditorObject.MachineRoomFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(machineRoomFileAssociation);
                        }

                        MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.新增, machineRoom.ParentId, machineRoom.PropertyType, (MachineRoomType)remodelingEditorObject.MachineRoomType, remodelingEditorObject.MachineRoomArea, remodelingEditorObject.MachineRoomBudget, remodelingEditorObject.MachineRoomTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        machineRoomLogRepository.Add(machineRoomLog);
                        FileAssociation machineRoomLogFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoomLog", machineRoomLog.Id, remodelingEditorObject.MachineRoomFileIdList, remodelingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(machineRoomLogFileAssociation);
                    }
                }
                else
                {
                    MachineRoom machineRoomCheck = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (machineRoomCheck != null)
                    {
                        MachineRoom machineRoom = machineRoomRepository.FindByKey(machineRoomCheck.Id);
                        if (machineRoom != null)
                        {
                            MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.删除, machineRoom.ParentId, machineRoom.PropertyType, machineRoom.MachineRoomType, machineRoom.MachineRoomArea, machineRoom.BudgetPrice, machineRoom.TimeLimit, "", remodelingEditorObject.ModifyUserId);
                            machineRoomLogRepository.Add(machineRoomLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation machineRoomLogFileAssociation = AggregateFactory.CreateFileAssociation("MachineRoomLog", machineRoomLog.Id, fileAssociation.FileIdList, remodelingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(machineRoomLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            machineRoomRepository.Remove(machineRoom);
                        }
                    }
                }

                if (remodelingEditorObject.ExternalElectricPowerMark == 1)
                {
                    ExternalElectricPower externalElectricPowerCheck = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (externalElectricPowerCheck != null)
                    {
                        ExternalElectricPower externalElectricPower = externalElectricPowerRepository.FindByKey(externalElectricPowerCheck.Id);
                        FileAssociation externalElectricPowerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"));
                        if ((externalElectricPowerFileAssociation == null && remodelingEditorObject.ExternalFileIdList != "") || (externalElectricPowerFileAssociation != null && remodelingEditorObject.ExternalFileIdList != externalElectricPowerFileAssociation.FileIdList) || (ExternalElectric)remodelingEditorObject.ExternalElectric != externalElectricPower.ExternalElectric || remodelingEditorObject.ExternalBudget != externalElectricPower.BudgetPrice || remodelingEditorObject.ExternalTimeLimit != externalElectricPower.TimeLimit)
                        {
                            externalElectricPower.Modify((ExternalElectric)remodelingEditorObject.ExternalElectric, remodelingEditorObject.ExternalBudget, 0, "", remodelingEditorObject.ModifyUserId);
                            externalElectricPowerRepository.Update(externalElectricPower);

                            if (externalElectricPowerFileAssociation == null && remodelingEditorObject.ExternalFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPower", externalElectricPower.Id, remodelingEditorObject.ExternalFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (externalElectricPowerFileAssociation != null && remodelingEditorObject.ExternalFileIdList != externalElectricPowerFileAssociation.FileIdList)
                            {
                                externalElectricPowerFileAssociation.Modify(remodelingEditorObject.ExternalFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(externalElectricPowerFileAssociation);
                            }

                            ExternalElectricPowerLog externalElectricPowerLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.修改, externalElectricPower.ParentId, externalElectricPower.PropertyType, (ExternalElectric)remodelingEditorObject.ExternalElectric, remodelingEditorObject.ExternalBudget, remodelingEditorObject.ExternalTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            externalElectricPowerLogRepository.Add(externalElectricPowerLog);
                            FileAssociation externalElectricPowerLogFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPowerLog", externalElectricPowerLog.Id, remodelingEditorObject.ExternalFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(externalElectricPowerLogFileAssociation);
                        }
                    }
                    else
                    {
                        ExternalElectricPower externalElectricPower = AggregateFactory.CreateExternalElectricPower(remodeling.Id, PropertyType.改造设计, (ExternalElectric)remodelingEditorObject.ExternalElectric, remodelingEditorObject.ExternalBudget, remodelingEditorObject.ExternalTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        externalElectricPowerRepository.Add(externalElectricPower);
                        FileAssociation externalElectricPowerFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"));
                        if (externalElectricPowerFileAssociation == null && remodelingEditorObject.ExternalFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPower", externalElectricPower.Id, remodelingEditorObject.ExternalFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (externalElectricPowerFileAssociation != null && remodelingEditorObject.ExternalFileIdList != externalElectricPowerFileAssociation.FileIdList)
                        {
                            externalElectricPowerFileAssociation.Modify(remodelingEditorObject.ExternalFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(externalElectricPowerFileAssociation);
                        }

                        ExternalElectricPowerLog externalElectricPowerLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.新增, externalElectricPower.ParentId, externalElectricPower.PropertyType, (ExternalElectric)remodelingEditorObject.ExternalElectric, remodelingEditorObject.ExternalBudget, remodelingEditorObject.ExternalTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        externalElectricPowerLogRepository.Add(externalElectricPowerLog);
                        FileAssociation externalElectricPowerLogFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPowerLog", externalElectricPowerLog.Id, remodelingEditorObject.ExternalFileIdList, remodelingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(externalElectricPowerLogFileAssociation);
                    }
                }
                else
                {
                    ExternalElectricPower externalElectricPowerCheck = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (externalElectricPowerCheck != null)
                    {
                        ExternalElectricPower externalElectricPower = externalElectricPowerRepository.FindByKey(externalElectricPowerCheck.Id);
                        if (externalElectricPower != null)
                        {
                            ExternalElectricPowerLog externalElectricPowerLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.删除, externalElectricPower.ParentId, externalElectricPower.PropertyType, externalElectricPower.ExternalElectric, externalElectricPower.BudgetPrice, externalElectricPower.TimeLimit, "", remodelingEditorObject.ModifyUserId);
                            externalElectricPowerLogRepository.Add(externalElectricPowerLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation externalElectricPowerLogFileAssociation = AggregateFactory.CreateFileAssociation("ExternalElectricPowerLog", externalElectricPowerLog.Id, fileAssociation.FileIdList, remodelingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(externalElectricPowerLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            externalElectricPowerRepository.Remove(externalElectricPower);
                        }
                    }
                }

                if (remodelingEditorObject.EquipmentInstallMark == 1)
                {
                    EquipmentInstall equipmentInstallCheck = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (equipmentInstallCheck != null)
                    {
                        EquipmentInstall equipmentInstall = equipmentInstallRepository.FindByKey(equipmentInstallCheck.Id);
                        FileAssociation equipmentInstallFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"));
                        if ((equipmentInstallFileAssociation == null && remodelingEditorObject.EquipmentInstallFileIdList != "") || (equipmentInstallFileAssociation != null && remodelingEditorObject.EquipmentInstallFileIdList != equipmentInstallFileAssociation.FileIdList) || remodelingEditorObject.SwitchPower != equipmentInstall.SwitchPower || remodelingEditorObject.Battery != equipmentInstall.Battery || remodelingEditorObject.CabinetNumber != equipmentInstall.CabinetNumber || remodelingEditorObject.EquipmentBudget != equipmentInstall.BudgetPrice || remodelingEditorObject.EquipmentTimeLimit != equipmentInstall.TimeLimit)
                        {
                            equipmentInstall.Modify(remodelingEditorObject.SwitchPower, remodelingEditorObject.Battery, remodelingEditorObject.CabinetNumber, remodelingEditorObject.EquipmentBudget, remodelingEditorObject.EquipmentTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            equipmentInstallRepository.Update(equipmentInstall);

                            if (equipmentInstallFileAssociation == null && remodelingEditorObject.EquipmentInstallFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstall", equipmentInstall.Id, remodelingEditorObject.EquipmentInstallFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (equipmentInstallFileAssociation != null && remodelingEditorObject.EquipmentInstallFileIdList != equipmentInstallFileAssociation.FileIdList)
                            {
                                equipmentInstallFileAssociation.Modify(remodelingEditorObject.EquipmentInstallFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(equipmentInstallFileAssociation);
                            }

                            EquipmentInstallLog equipmentInstallLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.修改, equipmentInstall.ParentId, equipmentInstall.PropertyType, remodelingEditorObject.SwitchPower, remodelingEditorObject.Battery, remodelingEditorObject.CabinetNumber, remodelingEditorObject.EquipmentBudget, remodelingEditorObject.EquipmentTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            equipmentInstallLogRepository.Add(equipmentInstallLog);
                            FileAssociation equipmentInstallLogFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstallLog", equipmentInstallLog.Id, remodelingEditorObject.EquipmentInstallFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(equipmentInstallLogFileAssociation);
                        }
                    }
                    else
                    {
                        EquipmentInstall equipmentInstall = AggregateFactory.CreateEquipmentInstall(remodeling.Id, PropertyType.改造设计, remodelingEditorObject.SwitchPower, remodelingEditorObject.Battery, remodelingEditorObject.CabinetNumber, remodelingEditorObject.EquipmentBudget, remodelingEditorObject.EquipmentTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        equipmentInstallRepository.Add(equipmentInstall);
                        FileAssociation equipmentInstallFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"));
                        if (equipmentInstallFileAssociation == null && remodelingEditorObject.EquipmentInstallFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstall", equipmentInstall.Id, remodelingEditorObject.EquipmentInstallFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (equipmentInstallFileAssociation != null && remodelingEditorObject.EquipmentInstallFileIdList != equipmentInstallFileAssociation.FileIdList)
                        {
                            equipmentInstallFileAssociation.Modify(remodelingEditorObject.EquipmentInstallFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(equipmentInstallFileAssociation);
                        }

                        EquipmentInstallLog equipmentInstallLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.新增, equipmentInstall.ParentId, equipmentInstall.PropertyType, remodelingEditorObject.SwitchPower, remodelingEditorObject.Battery, remodelingEditorObject.CabinetNumber, remodelingEditorObject.EquipmentBudget, remodelingEditorObject.EquipmentTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        equipmentInstallLogRepository.Add(equipmentInstallLog);
                        FileAssociation equipmentInstallLogFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstallLog", equipmentInstallLog.Id, remodelingEditorObject.EquipmentInstallFileIdList, remodelingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(equipmentInstallLogFileAssociation);
                    }
                }
                else
                {
                    EquipmentInstall equipmentInstallCheck = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (equipmentInstallCheck != null)
                    {
                        EquipmentInstall equipmentInstall = equipmentInstallRepository.FindByKey(equipmentInstallCheck.Id);
                        if (equipmentInstall != null)
                        {
                            EquipmentInstallLog equipmentInstallLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.删除, equipmentInstall.ParentId, equipmentInstall.PropertyType, equipmentInstall.SwitchPower, equipmentInstall.Battery, equipmentInstall.CabinetNumber, equipmentInstall.BudgetPrice, equipmentInstall.TimeLimit, "", remodelingEditorObject.ModifyUserId);
                            equipmentInstallLogRepository.Add(equipmentInstallLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation equipmentInstallLogFileAssociation = AggregateFactory.CreateFileAssociation("EquipmentInstallLog", equipmentInstallLog.Id, fileAssociation.FileIdList, remodelingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(equipmentInstallLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            equipmentInstallRepository.Remove(equipmentInstall);
                        }
                    }
                }

                if (remodelingEditorObject.AddressExplorMark == 1)
                {
                    AddressExplor addressExplorCheck = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (addressExplorCheck != null)
                    {
                        AddressExplor addressExplor = addressExplorRepository.FindByKey(addressExplorCheck.Id);
                        FileAssociation addressExplorFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"));
                        if ((addressExplorFileAssociation == null && remodelingEditorObject.AddressFileIdList != "") || (addressExplorFileAssociation != null && remodelingEditorObject.AddressFileIdList != addressExplorFileAssociation.FileIdList) || remodelingEditorObject.AddressBudget != addressExplor.BudgetPrice || remodelingEditorObject.AddressTimeLimit != addressExplor.TimeLimit)
                        {
                            addressExplor.Modify(remodelingEditorObject.AddressBudget, remodelingEditorObject.AddressTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            addressExplorRepository.Update(addressExplor);

                            if (addressExplorFileAssociation == null && remodelingEditorObject.AddressFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplor", addressExplor.Id, remodelingEditorObject.AddressFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (addressExplorFileAssociation != null && remodelingEditorObject.AddressFileIdList != addressExplorFileAssociation.FileIdList)
                            {
                                addressExplorFileAssociation.Modify(remodelingEditorObject.AddressFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(addressExplorFileAssociation);
                            }

                            AddressExplorLog addressExplorLog = AggregateFactory.CreateAddressExplorLog(OperationType.修改, addressExplor.ParentId, addressExplor.PropertyType, remodelingEditorObject.AddressBudget, remodelingEditorObject.AddressTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            addressExplorLogRepository.Add(addressExplorLog);
                            FileAssociation addressExplorLogFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplorLog", addressExplorLog.Id, remodelingEditorObject.AddressFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(addressExplorLogFileAssociation);
                        }
                    }
                    else
                    {
                        AddressExplor addressExplor = AggregateFactory.CreateAddressExplor(remodeling.Id, PropertyType.改造设计, remodelingEditorObject.AddressBudget, remodelingEditorObject.AddressTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        addressExplorRepository.Add(addressExplor);
                        FileAssociation addressExplorFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"));
                        if (addressExplorFileAssociation == null && remodelingEditorObject.AddressFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplor", addressExplor.Id, remodelingEditorObject.AddressFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (addressExplorFileAssociation != null && remodelingEditorObject.AddressFileIdList != addressExplorFileAssociation.FileIdList)
                        {
                            addressExplorFileAssociation.Modify(remodelingEditorObject.AddressFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(addressExplorFileAssociation);
                        }

                        AddressExplorLog addressExplorLog = AggregateFactory.CreateAddressExplorLog(OperationType.新增, addressExplor.ParentId, addressExplor.PropertyType, remodelingEditorObject.AddressBudget, remodelingEditorObject.AddressTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        addressExplorLogRepository.Add(addressExplorLog);
                        FileAssociation addressExplorLogFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplorLog", addressExplorLog.Id, remodelingEditorObject.AddressFileIdList, remodelingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(addressExplorLogFileAssociation);
                    }
                }
                else
                {
                    AddressExplor addressExplorCheck = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (addressExplorCheck != null)
                    {
                        AddressExplor addressExplor = addressExplorRepository.FindByKey(addressExplorCheck.Id);
                        if (addressExplor != null)
                        {
                            AddressExplorLog addressExplorLog = AggregateFactory.CreateAddressExplorLog(OperationType.删除, addressExplor.ParentId, addressExplor.PropertyType, addressExplor.BudgetPrice, addressExplor.TimeLimit, "", remodelingEditorObject.ModifyUserId);
                            addressExplorLogRepository.Add(addressExplorLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation addressExplorLogFileAssociation = AggregateFactory.CreateFileAssociation("AddressExplorLog", addressExplorLog.Id, fileAssociation.FileIdList, remodelingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(addressExplorLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            addressExplorRepository.Remove(addressExplor);
                        }
                    }
                }

                if (remodelingEditorObject.FoundationTestMark == 1)
                {
                    FoundationTest foundationTestCheck = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (foundationTestCheck != null)
                    {
                        FoundationTest foundationTest = foundationTestRepository.FindByKey(foundationTestCheck.Id);
                        FileAssociation foundationTestFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"));
                        if ((foundationTestFileAssociation == null && remodelingEditorObject.FoundationFileIdList != "") || (foundationTestFileAssociation != null && remodelingEditorObject.FoundationFileIdList != foundationTestFileAssociation.FileIdList) || remodelingEditorObject.FoundationBudget != foundationTest.BudgetPrice || remodelingEditorObject.FoundationTimeLimit != foundationTest.TimeLimit)
                        {
                            foundationTest.Modify(remodelingEditorObject.FoundationBudget, remodelingEditorObject.FoundationTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            foundationTestRepository.Update(foundationTest);

                            if (foundationTestFileAssociation == null && remodelingEditorObject.FoundationFileIdList != "")
                            {
                                FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTest", foundationTest.Id, remodelingEditorObject.FoundationFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Add(newAddressingFileAssociation);
                            }
                            else if (foundationTestFileAssociation != null && remodelingEditorObject.FoundationFileIdList != foundationTestFileAssociation.FileIdList)
                            {
                                foundationTestFileAssociation.Modify(remodelingEditorObject.FoundationFileIdList, remodelingEditorObject.ModifyUserId);
                                fileAssociationRepository.Update(foundationTestFileAssociation);
                            }

                            FoundationTestLog foundationTestLog = AggregateFactory.CreateFoundationTestLog(OperationType.修改, foundationTest.ParentId, foundationTest.PropertyType, remodelingEditorObject.FoundationBudget, remodelingEditorObject.FoundationTimeLimit, "", remodelingEditorObject.ModifyUserId);
                            foundationTestLogRepository.Add(foundationTestLog);
                            FileAssociation foundationTestLogFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTestLog", foundationTestLog.Id, remodelingEditorObject.FoundationFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(foundationTestLogFileAssociation);
                        }
                    }
                    else
                    {
                        FoundationTest foundationTest = AggregateFactory.CreateFoundationTest(remodeling.Id, PropertyType.改造设计, remodelingEditorObject.FoundationBudget, remodelingEditorObject.FoundationTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        foundationTestRepository.Add(foundationTest);
                        FileAssociation foundationTestFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"));
                        if (foundationTestFileAssociation == null && remodelingEditorObject.FoundationFileIdList != "")
                        {
                            FileAssociation newAddressingFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTest", foundationTest.Id, remodelingEditorObject.FoundationFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Add(newAddressingFileAssociation);
                        }
                        else if (foundationTestFileAssociation != null && remodelingEditorObject.FoundationFileIdList != foundationTestFileAssociation.FileIdList)
                        {
                            foundationTestFileAssociation.Modify(remodelingEditorObject.FoundationFileIdList, remodelingEditorObject.ModifyUserId);
                            fileAssociationRepository.Update(foundationTestFileAssociation);
                        }

                        FoundationTestLog foundationTestLog = AggregateFactory.CreateFoundationTestLog(OperationType.新增, foundationTest.ParentId, foundationTest.PropertyType, remodelingEditorObject.FoundationBudget, remodelingEditorObject.FoundationTimeLimit, "", remodelingEditorObject.ModifyUserId);
                        foundationTestLogRepository.Add(foundationTestLog);
                        FileAssociation foundationTestLogFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTestLog", foundationTestLog.Id, remodelingEditorObject.FoundationFileIdList, remodelingEditorObject.ModifyUserId);
                        fileAssociationRepository.Add(foundationTestLogFileAssociation);
                    }
                }
                else
                {
                    FoundationTest foundationTestCheck = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    if (foundationTestCheck != null)
                    {
                        FoundationTest foundationTest = foundationTestRepository.FindByKey(foundationTestCheck.Id);
                        if (foundationTest != null)
                        {
                            FoundationTestLog foundationTestLog = AggregateFactory.CreateFoundationTestLog(OperationType.删除, foundationTest.ParentId, foundationTest.PropertyType, foundationTest.BudgetPrice, foundationTest.TimeLimit, "", remodelingEditorObject.ModifyUserId);
                            foundationTestLogRepository.Add(foundationTestLog);
                            IEnumerable<FileAssociation> fileAssociations = fileAssociationRepository.FindAll(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"), "EntityName");
                            if (fileAssociations != null)
                            {
                                foreach (var fileAssociation in fileAssociations)
                                {
                                    FileAssociation foundationTestLogFileAssociation = AggregateFactory.CreateFileAssociation("FoundationTestLog", foundationTestLog.Id, fileAssociation.FileIdList, remodelingEditorObject.ModifyUserId);
                                    fileAssociationRepository.Add(foundationTestLogFileAssociation);
                                    fileAssociationRepository.Remove(fileAssociation);
                                }
                            }
                            foundationTestRepository.Remove(foundationTest);
                        }
                    }
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == remodelingEditorObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(remodelingEditorObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
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
        /// 改造站指定施工单位
        /// </summary>
        /// <param name="remodelingEditorObject"></param>
        public void SaveCustomerR(RemodelingEditorObject remodelingEditorObject)
        {
            Remodeling remodeling = remodelingRepository.FindByKey(remodelingEditorObject.Id);
            PlaceDesign placeDesign = placeDesignRepository.Find(Specification<PlaceDesign>.Eval(Entity => Entity.ParentId == remodeling.Id && Entity.PropertyType == PropertyType.改造设计));
            if (remodeling != null)
            {
                if (placeDesign.TowerMark == Bool.是)
                {
                    Tower tower = towerRepository.Find(Specification<Tower>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    tower.ModifyCustomer(remodelingEditorObject.TowerCustomerId);
                    towerRepository.Update(tower);
                }

                if (placeDesign.TowerBaseMark == Bool.是)
                {
                    TowerBase towerBase = towerBaseRepository.Find(Specification<TowerBase>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    towerBase.ModifyCustomer(remodelingEditorObject.TowerBaseCustomerId);
                    towerBaseRepository.Update(towerBase);
                }

                if (placeDesign.MachineRoomMark == Bool.是)
                {
                    MachineRoom machineRoom = machineRoomRepository.Find(Specification<MachineRoom>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    machineRoom.ModifyCustomer(remodelingEditorObject.MachineRoomCustomerId);
                    machineRoomRepository.Update(machineRoom);
                }

                if (placeDesign.ExternalElectricPowerMark == Bool.是)
                {
                    ExternalElectricPower externalElectricPower = externalElectricPowerRepository.Find(Specification<ExternalElectricPower>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    externalElectricPower.ModifyCustomer(remodelingEditorObject.ExternalCustomerId);
                    externalElectricPowerRepository.Update(externalElectricPower);
                }

                if (placeDesign.EquipmentInstallMark == Bool.是)
                {
                    EquipmentInstall equipmentInstall = equipmentInstallRepository.Find(Specification<EquipmentInstall>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    equipmentInstall.ModifyCustomer(remodelingEditorObject.EquipmentCustomerId);
                    equipmentInstallRepository.Update(equipmentInstall);
                }

                if (placeDesign.AddressExplorMark == Bool.是)
                {
                    AddressExplor addressExplor = addressExplorRepository.Find(Specification<AddressExplor>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    addressExplor.ModifyCustomer(remodelingEditorObject.AddressCustomerId);
                    addressExplorRepository.Update(addressExplor);
                }

                if (placeDesign.FoundationTestMark == Bool.是)
                {
                    FoundationTest foundationTest = foundationTestRepository.Find(Specification<FoundationTest>.Eval(entity => entity.ParentId == remodelingEditorObject.Id && entity.PropertyType == PropertyType.改造设计));
                    foundationTest.ModifyCustomer(remodelingEditorObject.FoundationCustomerId);
                    foundationTestRepository.Update(foundationTest);
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == remodelingEditorObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(remodelingEditorObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
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
        /// 改造站指定工程经理及监理单位
        /// </summary>
        /// <param name="placeDesignMaintObject"></param>
        public void SaveManagerAndSupervisorR(PlaceDesignMaintObject placeDesignMaintObject)
        {
            PlaceDesign placeDesign = placeDesignRepository.FindByKey(placeDesignMaintObject.Id);
            Remodeling remodeling = remodelingRepository.FindByKey(placeDesign.ParentId);
            if (remodeling != null)
            {
                Customer customer = customerRepository.FindByKey(placeDesignMaintObject.SupervisorCustomerId.Value);
                if (customer != null)
                {
                    if (customer.CustomerUserId == Guid.Empty)
                    {
                        throw new ApplicationFault("请选择已关联登陆人的监理单位");
                    }
                }
                placeDesign.AppointSupervisor(placeDesignMaintObject.SupervisorCustomerId.Value, customer.CustomerUserId);
                placeDesignRepository.Update(placeDesign);

                //remodeling.AppointProjectManagerId(placeDesignMaintObject.ProjectManagerId.Value, placeDesignMaintObject.ModifyUserId);
                //remodelingRepository.Update(remodeling);

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == placeDesignMaintObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(placeDesignMaintObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
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
