using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Events.WorkFlow;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Services;
using PDBM.Domain.Specifications;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.EventHandlers.WorkFlow
{
    /// <summary>
    /// 工作流活动实例处理事件处理器
    /// </summary>
    public class WFActivityInstanceDoingEventHandler : IDomainEventHandler<WFActivityInstanceDoingEvent>
    {
        private readonly IRepository<WFActivityInstance> wfActivityInstanceRepository;
        private readonly IRepository<WFProcessInstance> wfProcessInstanceRepository;
        private readonly IRepository<WFProcess> wfProcessRepository;
        private readonly IRepository<WFCategory> wfCategoryRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Remodeling> remodelingRepository;
        private readonly IRepository<PlanningApply> planningApplyRepository;
        private readonly IRepository<PlanningApplyHeader> planningApplyHeaderRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<ProjectTask> projectTaskRepository;
        private readonly IRepository<EngineeringTask> engineeringTaskRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<ConstructionTask> constructionTaskRepository;
        private readonly IRepository<PlaceProperty> placePropertyRepository;
        private readonly IRepository<PlacePropertyLog> placePropertyLogRepository;
        private readonly IRepository<WFActivityEditor> wfActivityEditorRepository;
        private readonly IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository;
        private readonly IRepository<PlaceDesign> placeDesignRepository;
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
        private readonly IRepository<Project> projectRepository;
        private readonly IRepository<TaskProperty> taskPropertyRepository;
        private readonly IRepository<OperatorsSharing> operatorsSharingRepository;
        private readonly IRepository<WorkApply> workApplyRepository;
        private readonly IRepository<WorkOrder> workOrderRepository;
        private readonly ICodeSeedRepository codeSeedRepository;
        private readonly IBMMgmtService bmMgmtService;

        public WFActivityInstanceDoingEventHandler(IRepository<WFActivityInstance> wfActivityInstanceRepository,
            IRepository<WFProcessInstance> wfProcessInstanceRepository,
            IRepository<WFProcess> wfProcessRepository,
            IRepository<WFCategory> wfCategoryRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<Remodeling> remodelingRepository,
            IRepository<PlanningApply> planningApplyRepository,
            IRepository<PlanningApplyHeader> planningApplyHeaderRepository,
            IRepository<Planning> planningRepository,
            IRepository<Place> placeRepository,
            IRepository<ProjectTask> projectTaskRepository,
            IRepository<EngineeringTask> engineeringTaskRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<ConstructionTask> constructionTaskRepository,
            IRepository<PlaceProperty> placePropertyRepository,
            IRepository<PlacePropertyLog> placePropertyLogRepository,
            IRepository<WFActivityEditor> wfActivityEditorRepository,
            IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository,
            IRepository<PlaceDesign> placeDesignRepository,
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
            IRepository<Project> projectRepository,
            IRepository<TaskProperty> taskPropertyRepository,
            IRepository<OperatorsSharing> operatorsSharingRepository,
            IRepository<WorkApply> workApplyRepository,
            IRepository<WorkOrder> workOrderRepository,
            ICodeSeedRepository codeSeedRepository,
            IBMMgmtService bmMgmtService)
        {
            this.wfActivityInstanceRepository = wfActivityInstanceRepository;
            this.wfProcessInstanceRepository = wfProcessInstanceRepository;
            this.wfProcessRepository = wfProcessRepository;
            this.wfCategoryRepository = wfCategoryRepository;
            this.addressingRepository = addressingRepository;
            this.remodelingRepository = remodelingRepository;
            this.planningApplyRepository = planningApplyRepository;
            this.planningApplyHeaderRepository = planningApplyHeaderRepository;
            this.planningRepository = planningRepository;
            this.placeRepository = placeRepository;
            this.projectTaskRepository = projectTaskRepository;
            this.engineeringTaskRepository = engineeringTaskRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.constructionTaskRepository = constructionTaskRepository;
            this.placePropertyRepository = placePropertyRepository;
            this.placePropertyLogRepository = placePropertyLogRepository;
            this.wfActivityEditorRepository = wfActivityEditorRepository;
            this.wfActivityInstanceEditorRepository = wfActivityInstanceEditorRepository;
            this.placeDesignRepository = placeDesignRepository;
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
            this.projectRepository = projectRepository;
            this.taskPropertyRepository = taskPropertyRepository;
            this.operatorsSharingRepository = operatorsSharingRepository;
            this.workApplyRepository = workApplyRepository;
            this.workOrderRepository = workOrderRepository;
            this.codeSeedRepository = codeSeedRepository;
            this.bmMgmtService = bmMgmtService;
        }

        public void Handle(WFActivityInstanceDoingEvent evt)
        {
            WFActivityInstance wfActivityInstance = evt.Source as WFActivityInstance;
            IList<WFActivityInstance> forwardWFActivityInstances = evt.forwardWFActivityInstances;

            WFProcessInstance wfProcessInstance = wfProcessInstanceRepository.GetByKey(wfActivityInstance.WFProcessInstanceId);
            if (wfProcessInstance.WFProcessInstanceState != WFProcessInstanceState.流转中)
            {
                throw new DomainFault("{0}<br>公文已经流转结束", wfProcessInstance.WFProcessInstanceCode);
            }

            if (wfActivityInstance.WFActivityInstanceFlow == WFActivityInstanceFlow.终止流转)
            {
                wfProcessInstance.WFProcessInstanceState = WFProcessInstanceState.流程终止;
                this.AfterNotPassed(wfProcessInstance);
            }
            else if (wfActivityInstance.WFActivityInstanceFlow == WFActivityInstanceFlow.正常流转)
            {
                if (wfActivityInstance.WFActivityEditorId != null)
                {
                    //WFActivityEditor wfActivityEditor = wfActivityEditorRepository.GetByKey(wfActivityInstance.WFActivityEditorId.Value);
                    if (wfActivityInstance.IsMustEdit == Bool.是)
                    {
                        WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == wfActivityInstance.Id));
                        if (wfActivityInstanceEditors == null)
                        {
                            throw new DomainFault("请先编辑单据内容");
                        }
                    }
                }

                IEnumerable<WFActivityInstance> wfActivityInstances = wfActivityInstanceRepository.FindAll(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfActivityInstance.WFProcessInstanceId &&
                    entity.SerialId >= wfActivityInstance.SerialId && entity.Id != wfActivityInstance.Id && entity.WFActivityInstanceState != WFActivityInstanceState.已处理));
                if (wfActivityInstances.Where(entity => entity.WFActivityOperate != WFActivityOperate.阅).Count() == 0)
                {
                    wfProcessInstance.WFProcessInstanceState = WFProcessInstanceState.流程通过;
                    foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances.Where(entity => entity.WFActivityOperate == WFActivityOperate.阅))
                    {
                        modifyWFActivityInstance.WFActivityInstanceState = WFActivityInstanceState.待处理;
                        modifyWFActivityInstance.ReceivedDate = DateTime.Now;
                        wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                    }
                    this.AfterPassed(wfProcessInstance);
                }
                else
                {
                    int minSerialId = wfActivityInstances.Where(entity => entity.WFActivityOperate != WFActivityOperate.阅).Min(entity => entity.SerialId);
                    foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances.Where(entity => entity.SerialId <= minSerialId && entity.WFActivityInstanceState != WFActivityInstanceState.待处理))
                    {
                        modifyWFActivityInstance.WFActivityInstanceState = WFActivityInstanceState.待处理;
                        modifyWFActivityInstance.ReceivedDate = DateTime.Now;
                        wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                    }
                }
            }
            else if (wfActivityInstance.WFActivityInstanceFlow == WFActivityInstanceFlow.转发他人)
            {
                int forwardRowId;
                int count = forwardWFActivityInstances.Count();
                if (count == 0)
                {
                    throw new DomainFault("请添加转发步骤");
                }

                IEnumerable<WFActivityInstance> wfActivityInstances = wfActivityInstanceRepository.FindAll(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfActivityInstance.WFProcessInstanceId &&
                    entity.SerialId >= wfActivityInstance.SerialId && entity.Id != wfActivityInstance.Id));

                if (wfActivityInstances.Where(entity => entity.SerialId == wfActivityInstance.SerialId && entity.RowId > wfActivityInstance.RowId).Count() > 0)
                {
                    WFActivityInstance maxRowWFActivityInstance = wfActivityInstances.Where(entity => entity.SerialId == wfActivityInstance.SerialId).OrderByDescending(entity => entity.RowId).First();
                    maxRowWFActivityInstance.WFActivityOrder = WFActivityOrder.并发;
                    wfActivityInstanceRepository.Update(maxRowWFActivityInstance);
                    forwardRowId = maxRowWFActivityInstance.RowId;
                }
                else
                {
                    wfActivityInstance.WFActivityOrder = WFActivityOrder.并发;
                    forwardRowId = wfActivityInstance.RowId;
                }

                for (int i = 0; i < count; i++)
                {
                    forwardWFActivityInstances[i].RowId += forwardRowId;
                    forwardWFActivityInstances[i].WFActivityInstanceState = WFActivityInstanceState.待处理;
                    forwardWFActivityInstances[i].ReceivedDate = DateTime.Now;
                    if (i == count - 1)
                    {
                        forwardWFActivityInstances[i].WFActivityOrder = WFActivityOrder.顺序;
                    }
                    wfActivityInstanceRepository.Add(forwardWFActivityInstances[i]);
                }

                if (forwardWFActivityInstances.Where(entity => entity.WFActivityOperate != WFActivityOperate.阅).Count() > 0 ||
                    wfActivityInstances.Where(entity => entity.SerialId == wfActivityInstance.SerialId && entity.WFActivityInstanceState != WFActivityInstanceState.已处理 && entity.WFActivityOperate != WFActivityOperate.阅).Count() > 0)
                {
                    foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances.Where(entity => entity.SerialId > wfActivityInstance.SerialId))
                    {
                        modifyWFActivityInstance.RowId += count;
                        wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                    }
                }
                else
                {
                    if (wfActivityInstances.Where(entity => entity.SerialId > wfActivityInstance.SerialId && entity.WFActivityOperate != WFActivityOperate.阅).Count() == 0)
                    {
                        wfProcessInstance.WFProcessInstanceState = WFProcessInstanceState.流程通过;
                        foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances.Where(entity => entity.SerialId > wfActivityInstance.SerialId && entity.WFActivityOperate == WFActivityOperate.阅))
                        {
                            modifyWFActivityInstance.WFActivityInstanceState = WFActivityInstanceState.待处理;
                            modifyWFActivityInstance.ReceivedDate = DateTime.Now;
                            modifyWFActivityInstance.RowId += count;
                            wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                        }
                        this.AfterPassed(wfProcessInstance);
                    }
                    else
                    {
                        int minSerialId = wfActivityInstances.Where(entity => entity.SerialId > wfActivityInstance.SerialId && entity.WFActivityOperate != WFActivityOperate.阅).Min(entity => entity.SerialId);
                        foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances.Where(entity => entity.SerialId <= minSerialId && entity.SerialId > wfActivityInstance.SerialId))
                        {
                            modifyWFActivityInstance.WFActivityInstanceState = WFActivityInstanceState.待处理;
                            modifyWFActivityInstance.ReceivedDate = DateTime.Now;
                            modifyWFActivityInstance.RowId += count;
                            wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                        }
                        foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances.Where(entity => entity.SerialId > minSerialId))
                        {
                            modifyWFActivityInstance.RowId += count;
                            wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                        }
                    }
                }
            }
            else if (wfActivityInstance.WFActivityInstanceFlow == WFActivityInstanceFlow.退回并转至自己)
            {
                int backRowId;
                WFActivityInstanceState backWFActivityInstanceState;

                IEnumerable<WFActivityInstance> wfActivityInstances = wfActivityInstanceRepository.FindAll(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfActivityInstance.WFProcessInstanceId &&
                    entity.SerialId >= wfActivityInstance.SerialId && entity.Id != wfActivityInstance.Id));

                if (wfActivityInstances.Where(entity => entity.SerialId == wfActivityInstance.SerialId &&
                            entity.WFActivityOperate != WFActivityOperate.阅 &&
                            entity.WFActivityInstanceState != WFActivityInstanceState.已处理).Count() == 0)
                {
                    backWFActivityInstanceState = WFActivityInstanceState.待处理;
                }
                else
                {
                    backWFActivityInstanceState = WFActivityInstanceState.未处理;
                }

                if (wfActivityInstances.Where(entity => entity.SerialId == wfActivityInstance.SerialId).Count() > 0)
                {
                    int maxRowId = wfActivityInstances.Where(entity => entity.SerialId == wfActivityInstance.SerialId).Max(entity => entity.RowId);
                    backRowId = maxRowId + 1;
                }
                else
                {
                    backRowId = wfActivityInstance.RowId + 1;
                }

                foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances.Where(entity => entity.SerialId > wfActivityInstance.SerialId))
                {
                    modifyWFActivityInstance.SerialId += 2;
                    modifyWFActivityInstance.RowId += 2;
                    wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                }

                WFActivityInstance backWFActivityInstance = AggregateFactory.CreateWFActivityInstance(wfProcessInstance.Id, "退回发起人", WFActivityOperate.审批, Guid.Empty, WFActivityOrder.顺序,
                    wfActivityInstance.SerialId + 1, backRowId, 24, wfProcessInstance.CreateUserId, Bool.否, wfActivityInstance.UserId);
                backWFActivityInstance.WFActivityInstanceState = backWFActivityInstanceState;
                WFActivityInstance selfWFActivityInstance = AggregateFactory.CreateWFActivityInstance(wfProcessInstance.Id, wfActivityInstance.WFActivityInstanceName, wfActivityInstance.WFActivityOperate, wfActivityInstance.WFActivityEditorId ?? Guid.Empty, WFActivityOrder.顺序,
                    wfActivityInstance.SerialId + 2, backRowId + 1, 24, wfActivityInstance.UserId, wfActivityInstance.IsMustEdit, wfActivityInstance.UserId);
                wfActivityInstanceRepository.Add(backWFActivityInstance);
                wfActivityInstanceRepository.Add(selfWFActivityInstance);
            }

            wfProcessInstanceRepository.Update(wfProcessInstance);
        }

        /// <summary>
        /// 流程通过后的处理
        /// </summary>
        /// <param name="wfProcessInstance">工作流过程实例</param>
        private void AfterPassed(WFProcessInstance wfProcessInstance)
        {
            WFProcess wfProcess = wfProcessRepository.GetByKey(wfProcessInstance.WFProcessId);
            WFCategory wfCategory = wfCategoryRepository.GetByKey(wfProcess.WFCategoryId);
            if (wfCategory.CodePrefix == "JZXZQR") //基站寻址确认
            {
                Addressing addressing = addressingRepository.GetByKey(wfProcessInstance.EntityId);
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);

                addressing.OrderState = WFProcessInstanceState.流程通过;
                addressingRepository.Update(addressing);

                ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == addressing.Id && entity.ProjectType == ProjectType.新建));
                if (projectTask != null)
                {
                    FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                    if (generalDesignFileAssociation != null)
                    {
                        FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planning.PlaceId && entity.EntityName == "Place"));
                        if (placeFileAssociation != null)
                        {
                            generalDesignFileAssociation.FileIdList = placeFileAssociation.FileIdList + "," + generalDesignFileAssociation.FileIdList;
                            placeFileAssociation.Modify(generalDesignFileAssociation.FileIdList, addressing.CreateUserId);
                            fileAssociationRepository.Update(placeFileAssociation);
                        }
                        else
                        {
                            FileAssociation newPlaceFileAssociation = AggregateFactory.CreateFileAssociation("Place", planning.PlaceId, generalDesignFileAssociation.FileIdList, addressing.CreateUserId);
                            fileAssociationRepository.Add(newPlaceFileAssociation);
                        }
                    }
                }
            }
            else if (wfCategory.CodePrefix == "JZGZQR") //基站改造确认
            {
                Remodeling remodeling = remodelingRepository.GetByKey(wfProcessInstance.EntityId);

                remodeling.OrderState = WFProcessInstanceState.流程通过;
                remodelingRepository.Update(remodeling);

                ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == remodeling.Id && entity.ProjectType != ProjectType.新建));
                if (projectTask != null)
                {
                    FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                    if (generalDesignFileAssociation != null)
                    {
                        FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == remodeling.PlaceId && entity.EntityName == "Place"));
                        if (placeFileAssociation != null)
                        {
                            generalDesignFileAssociation.FileIdList = placeFileAssociation.FileIdList + "," + generalDesignFileAssociation.FileIdList;
                            placeFileAssociation.Modify(generalDesignFileAssociation.FileIdList, remodeling.CreateUserId);
                            fileAssociationRepository.Update(placeFileAssociation);
                        }
                        else
                        {
                            FileAssociation newPlaceFileAssociation = AggregateFactory.CreateFileAssociation("Place", remodeling.PlaceId, generalDesignFileAssociation.FileIdList, remodeling.CreateUserId);
                            fileAssociationRepository.Add(newPlaceFileAssociation);
                        }
                    }
                }
            }
            else if (wfCategory.CodePrefix == "SFXZQR") //室分寻址确认
            {
                Addressing addressing = addressingRepository.GetByKey(wfProcessInstance.EntityId);
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);

                addressing.OrderState = WFProcessInstanceState.流程通过;
                addressingRepository.Update(addressing);

                ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == addressing.Id && entity.ProjectType == ProjectType.新建));
                if (projectTask != null)
                {
                    FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                    if (generalDesignFileAssociation != null)
                    {
                        FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planning.PlaceId && entity.EntityName == "Place"));
                        if (placeFileAssociation != null)
                        {
                            generalDesignFileAssociation.FileIdList = placeFileAssociation.FileIdList + "," + generalDesignFileAssociation.FileIdList;
                            placeFileAssociation.Modify(generalDesignFileAssociation.FileIdList, addressing.CreateUserId);
                            fileAssociationRepository.Update(placeFileAssociation);
                        }
                        else
                        {
                            FileAssociation newPlaceFileAssociation = AggregateFactory.CreateFileAssociation("Place", planning.PlaceId, generalDesignFileAssociation.FileIdList, addressing.CreateUserId);
                            fileAssociationRepository.Add(newPlaceFileAssociation);
                        }
                    }
                }
            }
            else if (wfCategory.CodePrefix == "SFGZQR") //室分改造确认
            {
                Remodeling remodeling = remodelingRepository.GetByKey(wfProcessInstance.EntityId);

                remodeling.OrderState = WFProcessInstanceState.流程通过;
                remodelingRepository.Update(remodeling);

                ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == remodeling.Id && entity.ProjectType != ProjectType.新建));
                if (projectTask != null)
                {
                    FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                    if (generalDesignFileAssociation != null)
                    {
                        FileAssociation placeFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == remodeling.PlaceId && entity.EntityName == "Place"));
                        if (placeFileAssociation != null)
                        {
                            generalDesignFileAssociation.FileIdList = placeFileAssociation.FileIdList + "," + generalDesignFileAssociation.FileIdList;
                            placeFileAssociation.Modify(generalDesignFileAssociation.FileIdList, remodeling.CreateUserId);
                            fileAssociationRepository.Update(placeFileAssociation);
                        }
                        else
                        {
                            FileAssociation newPlaceFileAssociation = AggregateFactory.CreateFileAssociation("Place", remodeling.PlaceId, generalDesignFileAssociation.FileIdList, remodeling.CreateUserId);
                            fileAssociationRepository.Add(newPlaceFileAssociation);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 流程未通过后的处理
        /// </summary>
        /// <param name="wfProcessInstance">工作流过程实例</param>
        private void AfterNotPassed(WFProcessInstance wfProcessInstance)
        {
            WFProcess wfProcess = wfProcessRepository.GetByKey(wfProcessInstance.WFProcessId);
            WFCategory wfCategory = wfCategoryRepository.GetByKey(wfProcess.WFCategoryId);
            if (wfCategory.CodePrefix == "JZXZQR") //基站寻址确认
            {
                Addressing addressing = addressingRepository.GetByKey(wfProcessInstance.EntityId);
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);
                if (planning.AddressingState == AddressingState.流转中)
                {
                    planning.AddressingState = AddressingState.流程终止;
                    planningRepository.Update(planning);
                }
                addressing.OrderState = WFProcessInstanceState.流程终止;
                addressingRepository.Update(addressing);
            }
            else if (wfCategory.CodePrefix == "JZGZQR") //基站改造确认
            {
                Remodeling remodeling = remodelingRepository.GetByKey(wfProcessInstance.EntityId);
                remodeling.OrderState = WFProcessInstanceState.流程终止;
                remodelingRepository.Update(remodeling);
            }
            else if (wfCategory.CodePrefix == "SFXZQR") //室分寻址确认
            {
                Addressing addressing = addressingRepository.GetByKey(wfProcessInstance.EntityId);
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);
                if (planning.AddressingState == AddressingState.流转中)
                {
                    planning.AddressingState = AddressingState.流程终止;
                    planningRepository.Update(planning);
                }
                addressing.OrderState = WFProcessInstanceState.流程终止;
                addressingRepository.Update(addressing);
            }
            else if (wfCategory.CodePrefix == "SFGZQR") //室分改造确认
            {
                Remodeling remodeling = remodelingRepository.GetByKey(wfProcessInstance.EntityId);
                remodeling.OrderState = WFProcessInstanceState.流程终止;
                remodelingRepository.Update(remodeling);
            }
        }
    }
}
