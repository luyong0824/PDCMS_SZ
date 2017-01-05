USE [PDCMS_SZ]

--删除外键约束--------------------------------------------------------------------------------------------------------------
--删除Department外键约束
ALTER TABLE [dbo].[tbl_Department] DROP CONSTRAINT [FK_dbo.tbl_Department_dbo.tbl_User_ManagerUserId]
--删除Project表外键约束
ALTER TABLE [dbo].[tbl_Project] DROP CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_AccountingEntity_AccountingEntityId]
--删除OperatorsPlanning外键约束
ALTER TABLE [dbo].[tbl_OperatorsPlanning] DROP CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Area_AreaId]
--删除Reseau外键约束
ALTER TABLE [dbo].[tbl_Reseau] DROP CONSTRAINT [FK_dbo.tbl_Reseau_dbo.tbl_Area_AreaId]
--删除Department外键约束
ALTER TABLE [dbo].[tbl_Department] DROP CONSTRAINT [FK_dbo.tbl_Department_dbo.tbl_Company_CompanyId]
--删除User外键约束
ALTER TABLE [dbo].[tbl_User] DROP CONSTRAINT [FK_dbo.tbl_User_dbo.tbl_Department_DepartmentId]
--删除MenuSub外键约束
ALTER TABLE [dbo].[tbl_MenuSub] DROP CONSTRAINT [FK_dbo.tbl_MenuSub_dbo.tbl_Menu_MenuId]
--删除RoleMenuItem外键约束
ALTER TABLE [dbo].[tbl_RoleMenuItem] DROP CONSTRAINT [FK_dbo.tbl_RoleMenuItem_dbo.tbl_MenuItem_MenuItemId]
--删除MenuItem外键约束
ALTER TABLE [dbo].[tbl_MenuItem] DROP CONSTRAINT [FK_dbo.tbl_MenuItem_dbo.tbl_MenuSub_MenuSubId]
--删除OperatorsConfirmDetail外键约束
ALTER TABLE [dbo].[tbl_OperatorsConfirmDetail] DROP CONSTRAINT [FK_dbo.tbl_OperatorsConfirmDetail_dbo.tbl_OperatorsConfirm_OperatorsConfirmId]
--删除OperatorsPlanning外键约束
ALTER TABLE [dbo].[tbl_OperatorsPlanning] DROP CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_PlaceCategory_PlaceCategoryId]
--删除Place外键约束
ALTER TABLE [dbo].[tbl_Place] DROP CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId]
--删除Planning外键约束
ALTER TABLE [dbo].[tbl_Planning] DROP CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId]
--删除Purchase外键约束
ALTER TABLE [dbo].[tbl_Purchase] DROP CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_PlaceCategory_PlaceCategoryId]
--删除Addressing外键约束
ALTER TABLE [dbo].[tbl_Addressing] DROP CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Planning_PlanningId]
--删除OperatorsConfirmDetail外键约束
ALTER TABLE [dbo].[tbl_OperatorsConfirmDetail] DROP CONSTRAINT [FK_dbo.tbl_OperatorsConfirmDetail_dbo.tbl_Planning_PlanningId]
--删除OperatorsPlanning外键约束
ALTER TABLE [dbo].[tbl_OperatorsPlanning] DROP CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Planning_PlanningId]
----删除Addressing外键约束
--ALTER TABLE [dbo].[tbl_Addressing] DROP CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Project_ProjectId]
--删除ProjectProfession外键约束
ALTER TABLE [dbo].[tbl_ProjectProfession] DROP CONSTRAINT [FK_dbo.tbl_ProjectProfession_dbo.tbl_Project_ProjectId]
--删除OperatorsSharing外键约束
ALTER TABLE [dbo].[tbl_OperatorsSharing] DROP CONSTRAINT [FK_dbo.tbl_OperatorsSharing_dbo.tbl_Remodeling_RemodelingId]
--删除Place外键约束
ALTER TABLE [dbo].[tbl_Place] DROP CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId]
--删除PlanningApply外键约束
--ALTER TABLE [dbo].[tbl_PlanningApply] DROP CONSTRAINT [FK_dbo.tbl_PlanningApply_dbo.tbl_Reseau_ReseauId]
--删除Planning外键约束
ALTER TABLE [dbo].[tbl_Planning] DROP CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId]
--删除Purchase外键约束
ALTER TABLE [dbo].[tbl_Purchase] DROP CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_Reseau_ReseauId]
--删除RoleMenuItem外键约束
ALTER TABLE [dbo].[tbl_RoleMenuItem] DROP CONSTRAINT [FK_dbo.tbl_RoleMenuItem_dbo.tbl_Role_RoleId]
--删除RoleUser外键约束
ALTER TABLE [dbo].[tbl_RoleUser] DROP CONSTRAINT [FK_dbo.tbl_RoleUser_dbo.tbl_Role_RoleId]
--删除Addressing外键约束
--ALTER TABLE [dbo].[tbl_Addressing] DROP CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Scene_SceneId]
--删除Place外键约束
--ALTER TABLE [dbo].[tbl_Place] DROP CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Scene_SceneId]
--删除Purchase外键约束
ALTER TABLE [dbo].[tbl_Purchase] DROP CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_Scene_SceneId]
--删除Planning外键约束
ALTER TABLE [dbo].[tbl_Planning] DROP CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_User_AddressingUserId]
--删除PostUser外键约束
ALTER TABLE [dbo].[tbl_PostUser] DROP CONSTRAINT [FK_dbo.tbl_PostUser_dbo.tbl_Post_PostId]
ALTER TABLE [dbo].[tbl_PostUser] DROP CONSTRAINT [FK_dbo.tbl_PostUser_dbo.tbl_User_UserId]
--删除Project外键约束
ALTER TABLE [dbo].[tbl_Project] DROP CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_User_ManagerUserId]
ALTER TABLE [dbo].[tbl_Project] DROP CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_User_ResponsibleUserId]
--删除RoleUser外键约束
ALTER TABLE [dbo].[tbl_RoleUser] DROP CONSTRAINT [FK_dbo.tbl_RoleUser_dbo.tbl_User_UserId]
--删除Remodeling外键约束
ALTER TABLE [dbo].[tbl_Remodeling] DROP CONSTRAINT [FK_dbo.tbl_Remodeling_dbo.tbl_Place_PlaceId]
--删除OperatorsSharing外键约束
ALTER TABLE [dbo].[tbl_OperatorsSharing] DROP CONSTRAINT [FK_dbo.tbl_OperatorsSharing_dbo.tbl_Place_PlaceId]
--删除Remodeling外键约束
--ALTER TABLE [dbo].[tbl_Remodeling] DROP CONSTRAINT [FK_dbo.tbl_Remodeling_dbo.tbl_Project_ProjectId]
--删除WFActivity外键约束
ALTER TABLE [dbo].[tbl_WFActivity] DROP CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_Company_CompanyId]
ALTER TABLE [dbo].[tbl_WFActivity] DROP CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_Department_DepartmentId]
ALTER TABLE [dbo].[tbl_WFActivity] DROP CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_Post_PostId]
ALTER TABLE [dbo].[tbl_WFActivity] DROP CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_User_UserId]
ALTER TABLE [dbo].[tbl_WFActivity] DROP CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_WFActivityEditor_WFActivityEditorId]
ALTER TABLE [dbo].[tbl_WFActivity] DROP CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_WFProcess_WFProcessId]
--删除WFActivityEditor外键约束
ALTER TABLE [dbo].[tbl_WFActivityEditor] DROP CONSTRAINT [FK_dbo.tbl_WFActivityEditor_dbo.tbl_WFCategory_WFCategoryId]
--删除WFActivityInstance外键约束
ALTER TABLE [dbo].[tbl_WFActivityInstance] DROP CONSTRAINT [FK_dbo.tbl_WFActivityInstance_dbo.tbl_User_UserId]
ALTER TABLE [dbo].[tbl_WFActivityInstance] DROP CONSTRAINT [FK_dbo.tbl_WFActivityInstance_dbo.tbl_WFActivityEditor_WFActivityEditorId]
ALTER TABLE [dbo].[tbl_WFActivityInstance] DROP CONSTRAINT [FK_dbo.tbl_WFActivityInstance_dbo.tbl_WFProcessInstance_WFProcessInstanceId]
--删除WFProcess外键约束
ALTER TABLE [dbo].[tbl_WFProcess] DROP CONSTRAINT [FK_dbo.tbl_WFProcess_dbo.tbl_WFCategory_WFCategoryId]
--删除WFProcessInstance外键约束
ALTER TABLE [dbo].[tbl_WFProcessInstance] DROP CONSTRAINT [FK_dbo.tbl_WFProcessInstance_dbo.tbl_WFProcess_WFProcessId]

--删除外键------------------------------------------------------------------------------------------------------------------
ALTER TABLE [dbo].[tbl_AccountingEntity] DROP CONSTRAINT [PK_dbo.tbl_AccountingEntity]
ALTER TABLE [dbo].[tbl_Addressing] DROP CONSTRAINT [PK_dbo.tbl_Addressing]
ALTER TABLE [dbo].[tbl_Area] DROP CONSTRAINT [PK_dbo.tbl_Area]
ALTER TABLE [dbo].[tbl_CodeSeed] DROP CONSTRAINT [PK_dbo.tbl_CodeSeed]
ALTER TABLE [dbo].[tbl_Company] DROP CONSTRAINT [PK_dbo.tbl_Company]
ALTER TABLE [dbo].[tbl_Department] DROP CONSTRAINT [PK_dbo.tbl_Department]
ALTER TABLE [dbo].[tbl_File] DROP CONSTRAINT [PK_dbo.tbl_File]
ALTER TABLE [dbo].[tbl_FileAssociation] DROP CONSTRAINT [PK_dbo.tbl_FileAssociation]
ALTER TABLE [dbo].[tbl_Menu] DROP CONSTRAINT [PK_dbo.tbl_Menu]
ALTER TABLE [dbo].[tbl_MenuItem] DROP CONSTRAINT [PK_dbo.tbl_MenuItem]
ALTER TABLE [dbo].[tbl_MenuSub] DROP CONSTRAINT [PK_dbo.tbl_MenuSub]
ALTER TABLE [dbo].[tbl_OperatorsConfirm] DROP CONSTRAINT [PK_dbo.tbl_OperatorsConfirm]
ALTER TABLE [dbo].[tbl_OperatorsConfirmDetail] DROP CONSTRAINT [PK_dbo.tbl_OperatorsConfirmDetail]
ALTER TABLE [dbo].[tbl_OperatorsPlanning] DROP CONSTRAINT [PK_dbo.tbl_OperatorsPlanning]
ALTER TABLE [dbo].[tbl_OperatorsSharing] DROP CONSTRAINT [PK_dbo.tbl_OperatorsSharing]
ALTER TABLE [dbo].[tbl_Place] DROP CONSTRAINT [PK_dbo.tbl_Place]
ALTER TABLE [dbo].[tbl_PlaceCategory] DROP CONSTRAINT [PK_dbo.tbl_PlaceCategory]
ALTER TABLE [dbo].[tbl_Planning] DROP CONSTRAINT [PK_dbo.tbl_Planning]
ALTER TABLE [dbo].[tbl_PlanningApply] DROP CONSTRAINT [PK_dbo.tbl_PlanningApply]
ALTER TABLE [dbo].[tbl_Post] DROP CONSTRAINT [PK_dbo.tbl_Post]
ALTER TABLE [dbo].[tbl_PostUser] DROP CONSTRAINT [PK_dbo.tbl_PostUser]
ALTER TABLE [dbo].[tbl_Project] DROP CONSTRAINT [PK_dbo.tbl_Project]
ALTER TABLE [dbo].[tbl_ProjectProfession] DROP CONSTRAINT [PK_dbo.tbl_ProjectProfession]
ALTER TABLE [dbo].[tbl_Purchase] DROP CONSTRAINT [PK_dbo.tbl_Purchase]
ALTER TABLE [dbo].[tbl_Remodeling] DROP CONSTRAINT [PK_dbo.tbl_Remodeling]
ALTER TABLE [dbo].[tbl_Reseau] DROP CONSTRAINT [PK_dbo.tbl_Reseau]
ALTER TABLE [dbo].[tbl_Role] DROP CONSTRAINT [PK_dbo.tbl_Role]
ALTER TABLE [dbo].[tbl_RoleMenuItem] DROP CONSTRAINT [PK_dbo.tbl_RoleMenuItem]
ALTER TABLE [dbo].[tbl_RoleUser] DROP CONSTRAINT [PK_dbo.tbl_RoleUser]
ALTER TABLE [dbo].[tbl_Scene] DROP CONSTRAINT [PK_dbo.tbl_Scene]
ALTER TABLE [dbo].[tbl_User] DROP CONSTRAINT [PK_dbo.tbl_User]
ALTER TABLE [dbo].[tbl_WFActivity] DROP CONSTRAINT [PK_dbo.tbl_WFActivity]
ALTER TABLE [dbo].[tbl_WFActivityEditor] DROP CONSTRAINT [PK_dbo.tbl_WFActivityEditor]
ALTER TABLE [dbo].[tbl_WFActivityInstance] DROP CONSTRAINT [PK_dbo.tbl_WFActivityInstance]
ALTER TABLE [dbo].[tbl_WFCategory] DROP CONSTRAINT [PK_dbo.tbl_WFCategory]
ALTER TABLE [dbo].[tbl_WFProcess] DROP CONSTRAINT [PK_dbo.tbl_WFProcess]
ALTER TABLE [dbo].[tbl_WFProcessInstance] DROP CONSTRAINT [PK_dbo.tbl_WFProcessInstance]
ALTER TABLE [dbo].[tbl_PlaceProperty] DROP CONSTRAINT [PK_dbo.tbl_PlaceProperty]
ALTER TABLE [dbo].[tbl_PlacePropertyLog] DROP CONSTRAINT [PK_dbo.tbl_PlacePropertyLog]
ALTER TABLE [dbo].[tbl_ConstructionTask] DROP CONSTRAINT [PK_dbo.tbl_ConstructionTask]
ALTER TABLE [dbo].[tbl_WFActivityInstanceEditor] DROP CONSTRAINT [PK_dbo.tbl_WFActivityInstanceEditor]
ALTER TABLE [dbo].[tbl_Unit] DROP CONSTRAINT [PK_dbo.tbl_Unit]
ALTER TABLE [dbo].[tbl_MaterialCategory] DROP CONSTRAINT [PK_dbo.tbl_MaterialCategory]
ALTER TABLE [dbo].[tbl_Material] DROP CONSTRAINT [PK_dbo.tbl_Material]
ALTER TABLE [dbo].[tbl_MaterialSpec] DROP CONSTRAINT [PK_dbo.tbl_MaterialSpec]
ALTER TABLE [dbo].[tbl_Customer] DROP CONSTRAINT [PK_dbo.tbl_Customer]
ALTER TABLE [dbo].[tbl_OperatorsPlanningDemand] DROP CONSTRAINT [PK_dbo.tbl_OperatorsPlanningDemand]
ALTER TABLE [dbo].[tbl_Tower] DROP CONSTRAINT [PK_dbo.tbl_Tower]
ALTER TABLE [dbo].[tbl_TowerBase] DROP CONSTRAINT [PK_dbo.tbl_TowerBase]
ALTER TABLE [dbo].[tbl_MachineRoom] DROP CONSTRAINT [PK_dbo.tbl_MachineRoom]
ALTER TABLE [dbo].[tbl_ExternalElectricPower] DROP CONSTRAINT [PK_dbo.tbl_ExternalElectricPower]
ALTER TABLE [dbo].[tbl_EquipmentInstall] DROP CONSTRAINT [PK_dbo.tbl_EquipmentInstall]
ALTER TABLE [dbo].[tbl_AddressExplor] DROP CONSTRAINT [PK_dbo.tbl_AddressExplor]
ALTER TABLE [dbo].[tbl_FoundationTest] DROP CONSTRAINT [PK_dbo.tbl_FoundationTest]
ALTER TABLE [dbo].[tbl_PlaceDesign] DROP CONSTRAINT [PK_dbo.tbl_PlaceDesign]
ALTER TABLE [dbo].[tbl_MaterialList] DROP CONSTRAINT [PK_dbo.tbl_MaterialList]
ALTER TABLE [dbo].[tbl_TaskProperty] DROP CONSTRAINT [PK_dbo.tbl_TaskProperty]
ALTER TABLE [dbo].[tbl_TowerLog] DROP CONSTRAINT [PK_dbo.tbl_TowerLog]
ALTER TABLE [dbo].[tbl_TowerBaseLog] DROP CONSTRAINT [PK_dbo.tbl_TowerBaseLog]
ALTER TABLE [dbo].[tbl_MachineRoomLog] DROP CONSTRAINT [PK_dbo.tbl_MachineRoomLog]
ALTER TABLE [dbo].[tbl_ExternalElectricPowerLog] DROP CONSTRAINT [PK_dbo.tbl_ExternalElectricPowerLog]
ALTER TABLE [dbo].[tbl_EquipmentInstallLog] DROP CONSTRAINT [PK_dbo.tbl_EquipmentInstallLog]
ALTER TABLE [dbo].[tbl_AddressExplorLog] DROP CONSTRAINT [PK_dbo.tbl_AddressExplorLog]
ALTER TABLE [dbo].[tbl_FoundationTestLog] DROP CONSTRAINT [PK_dbo.tbl_FoundationTestLog]
ALTER TABLE [dbo].[tbl_TaskPropertyLog] DROP CONSTRAINT [PK_dbo.tbl_TaskPropertyLog]
ALTER TABLE [dbo].[tbl_WorkBigClass] DROP CONSTRAINT [PK_dbo.tbl_WorkBigClass]
ALTER TABLE [dbo].[tbl_WorkSmallClass] DROP CONSTRAINT [PK_dbo.tbl_WorkSmallClass]
ALTER TABLE [dbo].[tbl_WorkApply] DROP CONSTRAINT [PK_dbo.tbl_WorkApply]
ALTER TABLE [dbo].[tbl_WorkOrder] DROP CONSTRAINT [PK_dbo.tbl_WorkOrder]
ALTER TABLE [dbo].[tbl_WorkOrderDetail] DROP CONSTRAINT [PK_dbo.tbl_WorkOrderDetail]
ALTER TABLE [dbo].[tbl_CustomerUser] DROP CONSTRAINT [PK_dbo.tbl_CustomerUser]
ALTER TABLE [dbo].[tbl_DelayApply] DROP CONSTRAINT [PK_dbo.tbl_DelayApply]
ALTER TABLE [dbo].[tbl_ProjectCodeList] DROP CONSTRAINT [PK_dbo.tbl_ProjectCodeList]
ALTER TABLE [dbo].[tbl_MaterialSpecList] DROP CONSTRAINT [PK_dbo.tbl_MaterialSpecList]
ALTER TABLE [dbo].[tbl_PlaceOwner] DROP CONSTRAINT [PK_dbo.tbl_PlaceOwner]
ALTER TABLE [dbo].[tbl_ProjectTask] DROP CONSTRAINT [PK_dbo.tbl_ProjectTask]
ALTER TABLE [dbo].[tbl_EngineeringTask] DROP CONSTRAINT [PK_dbo.tbl_EngineeringTask]
ALTER TABLE [dbo].[tbl_BusinessVolume] DROP CONSTRAINT [PK_dbo.tbl_BusinessVolume]
ALTER TABLE [dbo].[tbl_Notice] DROP CONSTRAINT [PK_dbo.tbl_Notice]
ALTER TABLE [dbo].[tbl_PlaceBusinessVolume] DROP CONSTRAINT [PK_dbo.tbl_PlaceBusinessVolume]
ALTER TABLE [dbo].[tbl_BlindSpotFeedBack] DROP CONSTRAINT [PK_dbo.tbl_BlindSpotFeedBack]
ALTER TABLE [dbo].[tbl_PlanningApplyHeader] DROP CONSTRAINT [PK_dbo.tbl_PlanningApplyHeader]
ALTER TABLE [dbo].[tbl_DutyUser] DROP CONSTRAINT [PK_dbo.tbl_DutyUser]

--创建聚集索引--------------------------------------------------------------------------------------------------------------
CREATE CLUSTERED INDEX IX_AccountingEntityCreateDate ON tbl_AccountingEntity (CreateDate)
CREATE CLUSTERED INDEX IX_AddressingCreateDate ON tbl_Addressing (CreateDate)
CREATE CLUSTERED INDEX IX_AreaCreateDate ON tbl_Area (CreateDate)
CREATE CLUSTERED INDEX IX_CodeSeedCreateDate ON tbl_CodeSeed (CreateDate)
CREATE CLUSTERED INDEX IX_CompanyCreateDate ON tbl_Company (CreateDate)
CREATE CLUSTERED INDEX IX_DepartmentCreateDate ON tbl_Department (CreateDate)
CREATE CLUSTERED INDEX IX_FileUploadDate ON tbl_File (UploadDate)
CREATE CLUSTERED INDEX IX_FileAssociationCreateDate ON tbl_FileAssociation (CreateDate)
CREATE CLUSTERED INDEX IX_MenuCreateDate ON tbl_Menu (CreateDate)
CREATE CLUSTERED INDEX IX_MenuItemCreateDate ON tbl_MenuItem (CreateDate)
CREATE CLUSTERED INDEX IX_MenuSubCreateDate ON tbl_MenuSub (CreateDate)
CREATE CLUSTERED INDEX IX_OperatorsConfirmCreateDate ON tbl_OperatorsConfirm (CreateDate)
CREATE CLUSTERED INDEX IX_OperatorsConfirmDetailCreateDate ON tbl_OperatorsConfirmDetail (CreateDate)
CREATE CLUSTERED INDEX IX_OperatorsPlanningCreateDate ON tbl_OperatorsPlanning (CreateDate)
CREATE CLUSTERED INDEX IX_OperatorsSharingCreateDate ON tbl_OperatorsSharing (CreateDate)
CREATE CLUSTERED INDEX IX_PlaceCreateDate ON tbl_Place (CreateDate)
CREATE CLUSTERED INDEX IX_PlaceCategoryCreateDate ON tbl_PlaceCategory (CreateDate)
CREATE CLUSTERED INDEX IX_PlanningCreateDate ON tbl_Planning (CreateDate)
CREATE CLUSTERED INDEX IX_PlanningApplyCreateDate ON tbl_PlanningApply (CreateDate)
CREATE CLUSTERED INDEX IX_PostCreateDate ON tbl_Post (CreateDate)
CREATE CLUSTERED INDEX IX_PostUserCreateDate ON tbl_PostUser (CreateDate)
CREATE CLUSTERED INDEX IX_ProjectCreateDate ON tbl_Project (CreateDate)
CREATE CLUSTERED INDEX IX_ProjectProfessionCreateDate ON tbl_ProjectProfession (CreateDate)
CREATE CLUSTERED INDEX IX_PurchaseCreateDate ON tbl_Purchase (CreateDate)
CREATE CLUSTERED INDEX IX_RemodelingCreateDate ON tbl_Remodeling (CreateDate)
CREATE CLUSTERED INDEX IX_ReseauCreateDate ON tbl_Reseau (CreateDate)
CREATE CLUSTERED INDEX IX_RoleCreateDate ON tbl_Role (CreateDate)
CREATE CLUSTERED INDEX IX_RoleMenuItemCreateDate ON tbl_RoleMenuItem (CreateDate)
CREATE CLUSTERED INDEX IX_RoleUserCreateDate ON tbl_RoleUser (CreateDate)
CREATE CLUSTERED INDEX IX_SceneCreateDate ON tbl_Scene (CreateDate)
CREATE CLUSTERED INDEX IX_UserCreateDate ON tbl_User (CreateDate)
CREATE CLUSTERED INDEX IX_WFActivityCreateDate ON tbl_WFActivity (CreateDate)
CREATE CLUSTERED INDEX IX_WFActivityEditorCreateDate ON tbl_WFActivityEditor (CreateDate)
CREATE CLUSTERED INDEX IX_WFActivityInstanceCreateDate ON tbl_WFActivityInstance (CreateDate)
CREATE CLUSTERED INDEX IX_WFCategoryCreateDate ON tbl_WFCategory (CreateDate)
CREATE CLUSTERED INDEX IX_WFProcessCreateDate ON tbl_WFProcess (CreateDate)
CREATE CLUSTERED INDEX IX_WFProcessInstanceCreateDate ON tbl_WFProcessInstance (CreateDate)
CREATE CLUSTERED INDEX IX_PlacePropertyCreateDate ON tbl_PlaceProperty (CreateDate)
CREATE CLUSTERED INDEX IX_PlacePropertyLogCreateDate ON tbl_PlacePropertyLog (CreateDate)
CREATE CLUSTERED INDEX IX_ConstructionTaskCreateDate ON tbl_ConstructionTask (CreateDate)
CREATE CLUSTERED INDEX IX_WFActivityInstanceEditorCreateDate ON tbl_WFActivityInstanceEditor (CreateDate)
CREATE CLUSTERED INDEX IX_UnitCreateDate ON tbl_Unit (CreateDate)
CREATE CLUSTERED INDEX IX_MaterialCategoryCreateDate ON tbl_MaterialCategory (CreateDate)
CREATE CLUSTERED INDEX IX_MaterialCreateDate ON tbl_Material (CreateDate)
CREATE CLUSTERED INDEX IX_MaterialSpecCreateDate ON tbl_MaterialSpec (CreateDate)
CREATE CLUSTERED INDEX IX_CustomerCreateDate ON tbl_Customer (CreateDate)
CREATE CLUSTERED INDEX IX_OperatorsPlanningDemandCreateDate ON tbl_OperatorsPlanningDemand (CreateDate)
CREATE CLUSTERED INDEX IX_TowerCreateDate ON tbl_Tower (CreateDate)
CREATE CLUSTERED INDEX IX_TowerBaseCreateDate ON tbl_TowerBase (CreateDate)
CREATE CLUSTERED INDEX IX_MachineRoomCreateDate ON tbl_MachineRoom (CreateDate)
CREATE CLUSTERED INDEX IX_ExternalElectricPowerCreateDate ON tbl_ExternalElectricPower (CreateDate)
CREATE CLUSTERED INDEX IX_EquipmentInstallCreateDate ON tbl_EquipmentInstall (CreateDate)
CREATE CLUSTERED INDEX IX_AddressExplorCreateDate ON tbl_AddressExplor (CreateDate)
CREATE CLUSTERED INDEX IX_FoundationTestCreateDate ON tbl_FoundationTest (CreateDate)
CREATE CLUSTERED INDEX IX_PlaceDesignCreateDate ON tbl_PlaceDesign (CreateDate)
CREATE CLUSTERED INDEX IX_MaterialListCreateDate ON tbl_MaterialList (CreateDate)
CREATE CLUSTERED INDEX IX_TaskPropertyCreateDate ON tbl_TaskProperty (CreateDate)
CREATE CLUSTERED INDEX IX_TowerLogCreateDate ON tbl_TowerLog (CreateDate)
CREATE CLUSTERED INDEX IX_TowerBaseLogCreateDate ON tbl_TowerBaseLog (CreateDate)
CREATE CLUSTERED INDEX IX_MachineRoomLogCreateDate ON tbl_MachineRoomLog (CreateDate)
CREATE CLUSTERED INDEX IX_ExternalElectricPowerLogCreateDate ON tbl_ExternalElectricPowerLog (CreateDate)
CREATE CLUSTERED INDEX IX_EquipmentInstallLogCreateDate ON tbl_EquipmentInstallLog (CreateDate)
CREATE CLUSTERED INDEX IX_AddressExplorLogCreateDate ON tbl_AddressExplorLog (CreateDate)
CREATE CLUSTERED INDEX IX_FoundationTestLogCreateDate ON tbl_FoundationTestLog (CreateDate)
CREATE CLUSTERED INDEX IX_TaskPropertyLogCreateDate ON tbl_TaskPropertyLog (CreateDate)
CREATE CLUSTERED INDEX IX_WorkBigClassCreateDate ON tbl_WorkBigClass (CreateDate)
CREATE CLUSTERED INDEX IX_WorkSmallClassCreateDate ON tbl_WorkSmallClass (CreateDate)
CREATE CLUSTERED INDEX IX_WorkApplyCreateDate ON tbl_WorkApply (CreateDate)
CREATE CLUSTERED INDEX IX_WorkOrderCreateDate ON tbl_WorkOrder (CreateDate)
CREATE CLUSTERED INDEX IX_WorkOrderDetailCreateDate ON tbl_WorkOrderDetail (CreateDate)
CREATE CLUSTERED INDEX IX_CustomerUserCreateDate ON tbl_CustomerUser (CreateDate)
CREATE CLUSTERED INDEX IX_DelayApplyCreateDate ON tbl_DelayApply (CreateDate)
CREATE CLUSTERED INDEX IX_ProjectCodeListCreateDate ON tbl_ProjectCodeList (CreateDate)
CREATE CLUSTERED INDEX IX_MaterialSpecListCreateDate ON tbl_MaterialSpecList (CreateDate)
CREATE CLUSTERED INDEX IX_PlaceOwnerCreateDate ON tbl_PlaceOwner (CreateDate)
CREATE CLUSTERED INDEX IX_ProjectTaskCreateDate ON tbl_ProjectTask (CreateDate)
CREATE CLUSTERED INDEX IX_EngineeringTaskCreateDate ON tbl_EngineeringTask (CreateDate)
CREATE CLUSTERED INDEX IX_BusinessVolumeCreateDate ON tbl_BusinessVolume (CreateDate)
CREATE CLUSTERED INDEX IX_NoticeCreateDate ON tbl_Notice (CreateDate)
CREATE CLUSTERED INDEX IX_PlaceBusinessVolumeCreateDate ON tbl_PlaceBusinessVolume (CreateDate)
CREATE CLUSTERED INDEX IX_BlindSpotFeedBackCreateDate ON tbl_BlindSpotFeedBack (CreateDate)
CREATE CLUSTERED INDEX IX_PlanningApplyHeaderCreateDate ON tbl_PlanningApplyHeader (CreateDate)
CREATE CLUSTERED INDEX IX_DutyUserCreateDate ON tbl_DutyUser (CreateDate)

--创建主键非聚集索引--------------------------------------------------------------------------------------------------------
ALTER TABLE [dbo].[tbl_AccountingEntity] ADD CONSTRAINT [PK_dbo.tbl_AccountingEntity] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Addressing] ADD CONSTRAINT [PK_dbo.tbl_Addressing] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Area] ADD CONSTRAINT [PK_dbo.tbl_Area] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_CodeSeed] ADD CONSTRAINT [PK_dbo.tbl_CodeSeed] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Company] ADD CONSTRAINT [PK_dbo.tbl_Company] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Department] ADD CONSTRAINT [PK_dbo.tbl_Department] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_File] ADD CONSTRAINT [PK_dbo.tbl_File] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_FileAssociation] ADD CONSTRAINT [PK_dbo.tbl_FileAssociation] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Menu] ADD CONSTRAINT [PK_dbo.tbl_Menu] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_MenuItem] ADD CONSTRAINT [PK_dbo.tbl_MenuItem] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_MenuSub] ADD CONSTRAINT [PK_dbo.tbl_MenuSub] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_OperatorsConfirm] ADD CONSTRAINT [PK_dbo.tbl_OperatorsConfirm] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_OperatorsConfirmDetail] ADD CONSTRAINT [PK_dbo.tbl_OperatorsConfirmDetail] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_OperatorsPlanning] ADD CONSTRAINT [PK_dbo.tbl_OperatorsPlanning] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_OperatorsSharing] ADD CONSTRAINT [PK_dbo.tbl_OperatorsSharing] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Place] ADD CONSTRAINT [PK_dbo.tbl_Place] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_PlaceCategory] ADD CONSTRAINT [PK_dbo.tbl_PlaceCategory] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Planning] ADD CONSTRAINT [PK_dbo.tbl_Planning] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_PlanningApply] ADD CONSTRAINT [PK_dbo.tbl_PlanningApply] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Post] ADD CONSTRAINT [PK_dbo.tbl_Post] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_PostUser] ADD CONSTRAINT [PK_dbo.tbl_PostUser] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Project] ADD CONSTRAINT [PK_dbo.tbl_Project] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_ProjectProfession] ADD CONSTRAINT [PK_dbo.tbl_ProjectProfession] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Purchase] ADD CONSTRAINT [PK_dbo.tbl_Purchase] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Remodeling] ADD CONSTRAINT [PK_dbo.tbl_Remodeling] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Reseau] ADD CONSTRAINT [PK_dbo.tbl_Reseau] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Role] ADD CONSTRAINT [PK_dbo.tbl_Role] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_RoleMenuItem] ADD CONSTRAINT [PK_dbo.tbl_RoleMenuItem] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_RoleUser] ADD CONSTRAINT [PK_dbo.tbl_RoleUser] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Scene] ADD CONSTRAINT [PK_dbo.tbl_Scene] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_User] ADD CONSTRAINT [PK_dbo.tbl_User] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WFActivity] ADD CONSTRAINT [PK_dbo.tbl_WFActivity] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WFActivityEditor] ADD CONSTRAINT [PK_dbo.tbl_WFActivityEditor] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WFActivityInstance] ADD CONSTRAINT [PK_dbo.tbl_WFActivityInstance] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WFCategory] ADD CONSTRAINT [PK_dbo.tbl_WFCategory] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WFProcess] ADD CONSTRAINT [PK_dbo.tbl_WFProcess] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WFProcessInstance] ADD CONSTRAINT [PK_dbo.tbl_WFProcessInstance] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_PlaceProperty] ADD CONSTRAINT [PK_dbo.tbl_PlaceProperty] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_PlacePropertyLog] ADD CONSTRAINT [PK_dbo.tbl_PlacePropertyLog] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_ConstructionTask] ADD CONSTRAINT [PK_dbo.tbl_ConstructionTask] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WFActivityInstanceEditor] ADD CONSTRAINT [PK_dbo.tbl_WFActivityInstanceEditor] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Unit] ADD CONSTRAINT [PK_dbo.tbl_Unit] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_MaterialCategory] ADD CONSTRAINT [PK_dbo.tbl_MaterialCategory] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Material] ADD CONSTRAINT [PK_dbo.tbl_Material] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_MaterialSpec] ADD CONSTRAINT [PK_dbo.tbl_MaterialSpec] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Customer] ADD CONSTRAINT [PK_dbo.tbl_Customer] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_OperatorsPlanningDemand] ADD CONSTRAINT [PK_dbo.tbl_OperatorsPlanningDemand] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Tower] ADD CONSTRAINT [PK_dbo.tbl_Tower] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_TowerBase] ADD CONSTRAINT [PK_dbo.tbl_TowerBase] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_MachineRoom] ADD CONSTRAINT [PK_dbo.tbl_MachineRoom] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_ExternalElectricPower] ADD CONSTRAINT [PK_dbo.tbl_ExternalElectricPower] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_EquipmentInstall] ADD CONSTRAINT [PK_dbo.tbl_EquipmentInstall] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_AddressExplor] ADD CONSTRAINT [PK_dbo.tbl_AddressExplor] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_FoundationTest] ADD CONSTRAINT [PK_dbo.tbl_FoundationTest] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_PlaceDesign] ADD CONSTRAINT [PK_dbo.tbl_PlaceDesign] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_MaterialList] ADD CONSTRAINT [PK_dbo.tbl_MaterialList] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_TaskProperty] ADD CONSTRAINT [PK_dbo.tbl_TaskProperty] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_TowerLog] ADD CONSTRAINT [PK_dbo.tbl_TowerLog] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_TowerBaseLog] ADD CONSTRAINT [PK_dbo.tbl_TowerBaseLog] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_MachineRoomLog] ADD CONSTRAINT [PK_dbo.tbl_MachineRoomLog] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_ExternalElectricPowerLog] ADD CONSTRAINT [PK_dbo.tbl_ExternalElectricPowerLog] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_EquipmentInstallLog] ADD CONSTRAINT [PK_dbo.tbl_EquipmentInstallLog] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_AddressExplorLog] ADD CONSTRAINT [PK_dbo.tbl_AddressExplorLog] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_FoundationTestLog] ADD CONSTRAINT [PK_dbo.tbl_FoundationTestLog] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_TaskPropertyLog] ADD CONSTRAINT [PK_dbo.tbl_TaskPropertyLog] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WorkBigClass] ADD CONSTRAINT [PK_dbo.tbl_WorkBigClass] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WorkSmallClass] ADD CONSTRAINT [PK_dbo.tbl_WorkSmallClass] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WorkApply] ADD CONSTRAINT [PK_dbo.tbl_WorkApply] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WorkOrder] ADD CONSTRAINT [PK_dbo.tbl_WorkOrder] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_WorkOrderDetail] ADD CONSTRAINT [PK_dbo.tbl_WorkOrderDetail] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_CustomerUser] ADD CONSTRAINT [PK_dbo.tbl_CustomerUser] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_DelayApply] ADD CONSTRAINT [PK_dbo.tbl_DelayApply] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_ProjectCodeList] ADD CONSTRAINT [PK_dbo.tbl_ProjectCodeList] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_MaterialSpecList] ADD CONSTRAINT [PK_dbo.tbl_MaterialSpecList] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_PlaceOwner] ADD CONSTRAINT [PK_dbo.tbl_PlaceOwner] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_ProjectTask] ADD CONSTRAINT [PK_dbo.tbl_ProjectTask] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_EngineeringTask] ADD CONSTRAINT [PK_dbo.tbl_EngineeringTask] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_BusinessVolume] ADD CONSTRAINT [PK_dbo.tbl_BusinessVolume] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_Notice] ADD CONSTRAINT [PK_dbo.tbl_Notice] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_PlaceBusinessVolume] ADD CONSTRAINT [PK_dbo.tbl_PlaceBusinessVolume] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_BlindSpotFeedBack] ADD CONSTRAINT [PK_dbo.tbl_BlindSpotFeedBack] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_PlanningApplyHeader] ADD CONSTRAINT [PK_dbo.tbl_PlanningApplyHeader] PRIMARY KEY NONCLUSTERED ([Id])
ALTER TABLE [dbo].[tbl_DutyUser] ADD CONSTRAINT [PK_dbo.tbl_DutyUser] PRIMARY KEY NONCLUSTERED ([Id])

--创建外键约束--------------------------------------------------------------------------------------------------------------
--新建Department外键约束
ALTER TABLE [dbo].[tbl_Department]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Department_dbo.tbl_User_ManagerUserId] FOREIGN KEY([ManagerUserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_Department] CHECK CONSTRAINT [FK_dbo.tbl_Department_dbo.tbl_User_ManagerUserId]
--新建Project表外键约束
--ALTER TABLE [dbo].[tbl_Project]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_AccountingEntity_AccountingEntityId] FOREIGN KEY([AccountingEntityId])
--REFERENCES [dbo].[tbl_AccountingEntity] ([Id])
--ALTER TABLE [dbo].[tbl_Project] CHECK CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_AccountingEntity_AccountingEntityId]
--新建OperatorsPlanning外键约束
ALTER TABLE [dbo].[tbl_OperatorsPlanning]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Area_AreaId] FOREIGN KEY([AreaId])
REFERENCES [dbo].[tbl_Area] ([Id])
ALTER TABLE [dbo].[tbl_OperatorsPlanning] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Area_AreaId]
--新建Reseau外键约束
ALTER TABLE [dbo].[tbl_Reseau]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Reseau_dbo.tbl_Area_AreaId] FOREIGN KEY([AreaId])
REFERENCES [dbo].[tbl_Area] ([Id])
ALTER TABLE [dbo].[tbl_Reseau] CHECK CONSTRAINT [FK_dbo.tbl_Reseau_dbo.tbl_Area_AreaId]
--ALTER TABLE [dbo].[tbl_Reseau]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Reseau_dbo.tbl_User_ReseauManagerId] FOREIGN KEY([ReseauManagerId])
--REFERENCES [dbo].[tbl_User] ([Id])
--ALTER TABLE [dbo].[tbl_Reseau] CHECK CONSTRAINT [FK_dbo.tbl_Reseau_dbo.tbl_User_ReseauManagerId]
--新建Department外键约束
ALTER TABLE [dbo].[tbl_Department]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Department_dbo.tbl_Company_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[tbl_Company] ([Id])
ALTER TABLE [dbo].[tbl_Department] CHECK CONSTRAINT [FK_dbo.tbl_Department_dbo.tbl_Company_CompanyId]
--新建User外键约束
ALTER TABLE [dbo].[tbl_User]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_User_dbo.tbl_Department_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[tbl_Department] ([Id])
ALTER TABLE [dbo].[tbl_User] CHECK CONSTRAINT [FK_dbo.tbl_User_dbo.tbl_Department_DepartmentId]
--新建MenuSub外键约束
ALTER TABLE [dbo].[tbl_MenuSub]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_MenuSub_dbo.tbl_Menu_MenuId] FOREIGN KEY([MenuId])
REFERENCES [dbo].[tbl_Menu] ([Id])
ALTER TABLE [dbo].[tbl_MenuSub] CHECK CONSTRAINT [FK_dbo.tbl_MenuSub_dbo.tbl_Menu_MenuId]
--新建RoleMenuItem外键约束
ALTER TABLE [dbo].[tbl_RoleMenuItem]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_RoleMenuItem_dbo.tbl_MenuItem_MenuItemId] FOREIGN KEY([MenuItemId])
REFERENCES [dbo].[tbl_MenuItem] ([Id])
ALTER TABLE [dbo].[tbl_RoleMenuItem] CHECK CONSTRAINT [FK_dbo.tbl_RoleMenuItem_dbo.tbl_MenuItem_MenuItemId]
--新建MenuItem外键约束
ALTER TABLE [dbo].[tbl_MenuItem]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_MenuItem_dbo.tbl_MenuSub_MenuSubId] FOREIGN KEY([MenuSubId])
REFERENCES [dbo].[tbl_MenuSub] ([Id])
ALTER TABLE [dbo].[tbl_MenuItem] CHECK CONSTRAINT [FK_dbo.tbl_MenuItem_dbo.tbl_MenuSub_MenuSubId]
--新建OperatorsConfirmDetail外键约束
ALTER TABLE [dbo].[tbl_OperatorsConfirmDetail]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsConfirmDetail_dbo.tbl_OperatorsConfirm_OperatorsConfirmId] FOREIGN KEY([OperatorsConfirmId])
REFERENCES [dbo].[tbl_OperatorsConfirm] ([Id])
ALTER TABLE [dbo].[tbl_OperatorsConfirmDetail] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsConfirmDetail_dbo.tbl_OperatorsConfirm_OperatorsConfirmId]
--新建OperatorsPlanning外键约束
ALTER TABLE [dbo].[tbl_OperatorsPlanning]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_PlaceCategory_PlaceCategoryId] FOREIGN KEY([PlaceCategoryId])
REFERENCES [dbo].[tbl_PlaceCategory] ([Id])
ALTER TABLE [dbo].[tbl_OperatorsPlanning] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_PlaceCategory_PlaceCategoryId]
--新建Place外键约束
ALTER TABLE [dbo].[tbl_Place]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId] FOREIGN KEY([PlaceCategoryId])
REFERENCES [dbo].[tbl_PlaceCategory] ([Id])
ALTER TABLE [dbo].[tbl_Place] CHECK CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId]
--新建Planning外键约束
--ALTER TABLE [dbo].[tbl_Planning]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId] FOREIGN KEY([PlaceCategoryId])
--REFERENCES [dbo].[tbl_PlaceCategory] ([Id])
--ALTER TABLE [dbo].[tbl_Planning] CHECK CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId]
--新建Purchase外键约束
ALTER TABLE [dbo].[tbl_Purchase]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_PlaceCategory_PlaceCategoryId] FOREIGN KEY([PlaceCategoryId])
REFERENCES [dbo].[tbl_PlaceCategory] ([Id])
ALTER TABLE [dbo].[tbl_Purchase] CHECK CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_PlaceCategory_PlaceCategoryId]
--新建Addressing外键约束
ALTER TABLE [dbo].[tbl_Addressing]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Planning_PlanningId] FOREIGN KEY([PlanningId])
REFERENCES [dbo].[tbl_Planning] ([Id])
ALTER TABLE [dbo].[tbl_Addressing] CHECK CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Planning_PlanningId]
--新建OperatorsConfirmDetail外键约束
ALTER TABLE [dbo].[tbl_OperatorsConfirmDetail]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsConfirmDetail_dbo.tbl_Planning_PlanningId] FOREIGN KEY([PlanningId])
REFERENCES [dbo].[tbl_Planning] ([Id])
ALTER TABLE [dbo].[tbl_OperatorsConfirmDetail] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsConfirmDetail_dbo.tbl_Planning_PlanningId]
--新建OperatorsPlanning外键约束
ALTER TABLE [dbo].[tbl_OperatorsPlanning]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Planning_PlanningId] FOREIGN KEY([PlanningId])
REFERENCES [dbo].[tbl_Planning] ([Id])
ALTER TABLE [dbo].[tbl_OperatorsPlanning] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Planning_PlanningId]
--新建Addressing外键约束
--ALTER TABLE [dbo].[tbl_Addressing]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Project_ProjectId] FOREIGN KEY([ProjectId])
--REFERENCES [dbo].[tbl_Project] ([Id])
--ALTER TABLE [dbo].[tbl_Addressing] CHECK CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Project_ProjectId]
--新建ProjectProfession外键约束
ALTER TABLE [dbo].[tbl_ProjectProfession]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ProjectProfession_dbo.tbl_Project_ProjectId] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[tbl_Project] ([Id])
ALTER TABLE [dbo].[tbl_ProjectProfession] CHECK CONSTRAINT [FK_dbo.tbl_ProjectProfession_dbo.tbl_Project_ProjectId]
--新建OperatorsSharing外键约束
--ALTER TABLE [dbo].[tbl_OperatorsSharing]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsSharing_dbo.tbl_Remodeling_RemodelingId] FOREIGN KEY([RemodelingId])
--REFERENCES [dbo].[tbl_Remodeling] ([Id])
--ALTER TABLE [dbo].[tbl_OperatorsSharing] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsSharing_dbo.tbl_Remodeling_RemodelingId]
--新建Place外键约束
ALTER TABLE [dbo].[tbl_Place]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId] FOREIGN KEY([ReseauId])
REFERENCES [dbo].[tbl_Reseau] ([Id])
ALTER TABLE [dbo].[tbl_Place] CHECK CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId]
--新建PlanningApply外键约束
ALTER TABLE [dbo].[tbl_PlanningApply]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_PlanningApply_dbo.tbl_Reseau_ReseauId] FOREIGN KEY([ReseauId])
REFERENCES [dbo].[tbl_Reseau] ([Id])
ALTER TABLE [dbo].[tbl_PlanningApply] CHECK CONSTRAINT [FK_dbo.tbl_PlanningApply_dbo.tbl_Reseau_ReseauId]
--新建Planning外键约束
ALTER TABLE [dbo].[tbl_Planning]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId] FOREIGN KEY([ReseauId])
REFERENCES [dbo].[tbl_Reseau] ([Id])
ALTER TABLE [dbo].[tbl_Planning] CHECK CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId]
--新建Purchase外键约束
ALTER TABLE [dbo].[tbl_Purchase]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_Reseau_ReseauId] FOREIGN KEY([ReseauId])
REFERENCES [dbo].[tbl_Reseau] ([Id])
ALTER TABLE [dbo].[tbl_Purchase] CHECK CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_Reseau_ReseauId]
--新建RoleMenuItem外键约束
ALTER TABLE [dbo].[tbl_RoleMenuItem]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_RoleMenuItem_dbo.tbl_Role_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[tbl_Role] ([Id])
ALTER TABLE [dbo].[tbl_RoleMenuItem] CHECK CONSTRAINT [FK_dbo.tbl_RoleMenuItem_dbo.tbl_Role_RoleId]
--新建RoleUser外键约束
ALTER TABLE [dbo].[tbl_RoleUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_RoleUser_dbo.tbl_Role_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[tbl_Role] ([Id])
ALTER TABLE [dbo].[tbl_RoleUser] CHECK CONSTRAINT [FK_dbo.tbl_RoleUser_dbo.tbl_Role_RoleId]
--新建Addressing外键约束
--ALTER TABLE [dbo].[tbl_Addressing]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Scene_SceneId] FOREIGN KEY([SceneId])
--REFERENCES [dbo].[tbl_Scene] ([Id])
--ALTER TABLE [dbo].[tbl_Addressing] CHECK CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Scene_SceneId]
--新建Place外键约束
--ALTER TABLE [dbo].[tbl_Place]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Scene_SceneId] FOREIGN KEY([SceneId])
--REFERENCES [dbo].[tbl_Scene] ([Id])
--ALTER TABLE [dbo].[tbl_Place] CHECK CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Scene_SceneId]
--新建Purchase外键约束
ALTER TABLE [dbo].[tbl_Purchase]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_Scene_SceneId] FOREIGN KEY([SceneId])
REFERENCES [dbo].[tbl_Scene] ([Id])
ALTER TABLE [dbo].[tbl_Purchase] CHECK CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_Scene_SceneId]
--新建Planning外键约束
--ALTER TABLE [dbo].[tbl_Planning]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_User_AddressingUserId] FOREIGN KEY([AddressingUserId])
--REFERENCES [dbo].[tbl_User] ([Id])
--ALTER TABLE [dbo].[tbl_Planning] CHECK CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_User_AddressingUserId]
--新建PostUser外键约束
ALTER TABLE [dbo].[tbl_PostUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_PostUser_dbo.tbl_Post_PostId] FOREIGN KEY([PostId])
REFERENCES [dbo].[tbl_Post] ([Id])
ALTER TABLE [dbo].[tbl_PostUser] CHECK CONSTRAINT [FK_dbo.tbl_PostUser_dbo.tbl_Post_PostId]
ALTER TABLE [dbo].[tbl_PostUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_PostUser_dbo.tbl_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_PostUser] CHECK CONSTRAINT [FK_dbo.tbl_PostUser_dbo.tbl_User_UserId]
--新建Project外键约束
--ALTER TABLE [dbo].[tbl_Project]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_User_ManagerUserId] FOREIGN KEY([ManagerUserId])
--REFERENCES [dbo].[tbl_User] ([Id])
--ALTER TABLE [dbo].[tbl_Project] CHECK CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_User_ManagerUserId]
--ALTER TABLE [dbo].[tbl_Project]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_User_ResponsibleUserId] FOREIGN KEY([ResponsibleUserId])
--REFERENCES [dbo].[tbl_User] ([Id])
--ALTER TABLE [dbo].[tbl_Project] CHECK CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_User_ResponsibleUserId]
--新建RoleUser外键约束
ALTER TABLE [dbo].[tbl_RoleUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_RoleUser_dbo.tbl_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_RoleUser] CHECK CONSTRAINT [FK_dbo.tbl_RoleUser_dbo.tbl_User_UserId]
--新建Remodeling外键约束
ALTER TABLE [dbo].[tbl_Remodeling]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Remodeling_dbo.tbl_Place_PlaceId] FOREIGN KEY([PlaceId])
REFERENCES [dbo].[tbl_Place] ([Id])
ALTER TABLE [dbo].[tbl_Remodeling] CHECK CONSTRAINT [FK_dbo.tbl_Remodeling_dbo.tbl_Place_PlaceId]
--新建OperatorsSharing外键约束
ALTER TABLE [dbo].[tbl_OperatorsSharing]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsSharing_dbo.tbl_Place_PlaceId] FOREIGN KEY([PlaceId])
REFERENCES [dbo].[tbl_Place] ([Id])
ALTER TABLE [dbo].[tbl_OperatorsSharing] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsSharing_dbo.tbl_Place_PlaceId]
--新建Remodeling外键约束
--ALTER TABLE [dbo].[tbl_Remodeling]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Remodeling_dbo.tbl_Project_ProjectId] FOREIGN KEY([ProjectId])
--REFERENCES [dbo].[tbl_Project] ([Id])
--ALTER TABLE [dbo].[tbl_Remodeling] CHECK CONSTRAINT [FK_dbo.tbl_Remodeling_dbo.tbl_Project_ProjectId]
--新建WFActivity外键约束
ALTER TABLE [dbo].[tbl_WFActivity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_Company_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[tbl_Company] ([Id])
ALTER TABLE [dbo].[tbl_WFActivity] CHECK CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_Company_CompanyId]
ALTER TABLE [dbo].[tbl_WFActivity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_Department_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[tbl_Department] ([Id])
ALTER TABLE [dbo].[tbl_WFActivity] CHECK CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_Department_DepartmentId]
ALTER TABLE [dbo].[tbl_WFActivity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_Post_PostId] FOREIGN KEY([PostId])
REFERENCES [dbo].[tbl_Post] ([Id])
ALTER TABLE [dbo].[tbl_WFActivity] CHECK CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_Post_PostId]
ALTER TABLE [dbo].[tbl_WFActivity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_WFActivity] CHECK CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_User_UserId]
--ALTER TABLE [dbo].[tbl_WFActivity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_WFActivityEditor_WFActivityEditorId] FOREIGN KEY([WFActivityEditorId])
--REFERENCES [dbo].[tbl_WFActivityEditor] ([Id])
--ALTER TABLE [dbo].[tbl_WFActivity] CHECK CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_WFActivityEditor_WFActivityEditorId]
ALTER TABLE [dbo].[tbl_WFActivity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_WFProcess_WFProcessId] FOREIGN KEY([WFProcessId])
REFERENCES [dbo].[tbl_WFProcess] ([Id])
ALTER TABLE [dbo].[tbl_WFActivity] CHECK CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_WFProcess_WFProcessId]
--新建WFActivityEditor外键约束
ALTER TABLE [dbo].[tbl_WFActivityEditor]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivityEditor_dbo.tbl_WFCategory_WFCategoryId] FOREIGN KEY([WFCategoryId])
REFERENCES [dbo].[tbl_WFCategory] ([Id])
ALTER TABLE [dbo].[tbl_WFActivityEditor] CHECK CONSTRAINT [FK_dbo.tbl_WFActivityEditor_dbo.tbl_WFCategory_WFCategoryId]
--新建WFActivityInstance外键约束
ALTER TABLE [dbo].[tbl_WFActivityInstance]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivityInstance_dbo.tbl_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_WFActivityInstance] CHECK CONSTRAINT [FK_dbo.tbl_WFActivityInstance_dbo.tbl_User_UserId]
ALTER TABLE [dbo].[tbl_WFActivityInstance]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivityInstance_dbo.tbl_WFActivityEditor_WFActivityEditorId] FOREIGN KEY([WFActivityEditorId])
REFERENCES [dbo].[tbl_WFActivityEditor] ([Id])
ALTER TABLE [dbo].[tbl_WFActivityInstance] CHECK CONSTRAINT [FK_dbo.tbl_WFActivityInstance_dbo.tbl_WFActivityEditor_WFActivityEditorId]
ALTER TABLE [dbo].[tbl_WFActivityInstance]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivityInstance_dbo.tbl_WFProcessInstance_WFProcessInstanceId] FOREIGN KEY([WFProcessInstanceId])
REFERENCES [dbo].[tbl_WFProcessInstance] ([Id])
ALTER TABLE [dbo].[tbl_WFActivityInstance] CHECK CONSTRAINT [FK_dbo.tbl_WFActivityInstance_dbo.tbl_WFProcessInstance_WFProcessInstanceId]
--新建WFProcess外键约束
ALTER TABLE [dbo].[tbl_WFProcess]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFProcess_dbo.tbl_WFCategory_WFCategoryId] FOREIGN KEY([WFCategoryId])
REFERENCES [dbo].[tbl_WFCategory] ([Id])
ALTER TABLE [dbo].[tbl_WFProcess] CHECK CONSTRAINT [FK_dbo.tbl_WFProcess_dbo.tbl_WFCategory_WFCategoryId]
--新增WFProcessInstance外键约束
ALTER TABLE [dbo].[tbl_WFProcessInstance]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFProcessInstance_dbo.tbl_WFProcess_WFProcessId] FOREIGN KEY([WFProcessId])
REFERENCES [dbo].[tbl_WFProcess] ([Id])
ALTER TABLE [dbo].[tbl_WFProcessInstance] CHECK CONSTRAINT [FK_dbo.tbl_WFProcessInstance_dbo.tbl_WFProcess_WFProcessId]
--新增Material外键约束
ALTER TABLE [dbo].[tbl_Material]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Material_dbo.tbl_MaterialCategory_MaterialCategoryId] FOREIGN KEY([MaterialCategoryId])
REFERENCES [dbo].[tbl_MaterialCategory] ([Id])
ALTER TABLE [dbo].[tbl_Material] CHECK CONSTRAINT [FK_dbo.tbl_Material_dbo.tbl_MaterialCategory_MaterialCategoryId]
--新增MaterialSpec外键约束
ALTER TABLE [dbo].[tbl_MaterialSpec]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_MaterialSpec_dbo.tbl_Material_MaterialId] FOREIGN KEY([MaterialId])
REFERENCES [dbo].[tbl_Material] ([Id])
ALTER TABLE [dbo].[tbl_MaterialSpec] CHECK CONSTRAINT [FK_dbo.tbl_MaterialSpec_dbo.tbl_Material_MaterialId]
--新增MaterialSpec外键约束
ALTER TABLE [dbo].[tbl_MaterialSpec]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_MaterialSpec_dbo.tbl_Unit_UnitId] FOREIGN KEY([UnitId])
REFERENCES [dbo].[tbl_Unit] ([Id])
ALTER TABLE [dbo].[tbl_MaterialSpec] CHECK CONSTRAINT [FK_dbo.tbl_MaterialSpec_dbo.tbl_Unit_UnitId]
--新增MaterialList外键约束
ALTER TABLE [dbo].[tbl_MaterialList]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_MaterialList_dbo.tbl_MaterialSpec_MaterialSpecId] FOREIGN KEY([MaterialSpecId])
REFERENCES [dbo].[tbl_MaterialSpec] ([Id])
ALTER TABLE [dbo].[tbl_MaterialList] CHECK CONSTRAINT [FK_dbo.tbl_MaterialList_dbo.tbl_MaterialSpec_MaterialSpecId]
--新增OperatorsPlanningDemand外键约束
ALTER TABLE [dbo].[tbl_OperatorsPlanningDemand]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsPlanningDemand_dbo.tbl_OperatorsPlanning_OperatorsPlanningId] FOREIGN KEY([OperatorsPlanningId])
REFERENCES [dbo].[tbl_OperatorsPlanning] ([Id])
ALTER TABLE [dbo].[tbl_OperatorsPlanningDemand] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsPlanningDemand_dbo.tbl_OperatorsPlanning_OperatorsPlanningId]
--新增OperatorsPlanningDemand外键约束
--ALTER TABLE [dbo].[tbl_OperatorsPlanningDemand]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsPlanningDemand_dbo.tbl_Place_PlaceId] FOREIGN KEY([PlaceId])
--REFERENCES [dbo].[tbl_Place] ([Id])
--ALTER TABLE [dbo].[tbl_OperatorsPlanningDemand] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsPlanningDemand_dbo.tbl_Place_PlaceId]
--新建WorkSmallClass外键约束
ALTER TABLE [dbo].[tbl_WorkSmallClass]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WorkSmallClass_dbo.tbl_WorkBigClass_WorkBigClassId] FOREIGN KEY([WorkBigClassId])
REFERENCES [dbo].[tbl_WorkBigClass] ([Id])
ALTER TABLE [dbo].[tbl_WorkSmallClass] CHECK CONSTRAINT [FK_dbo.tbl_WorkSmallClass_dbo.tbl_WorkBigClass_WorkBigClassId]
--新建WorkApply外键约束
ALTER TABLE [dbo].[tbl_WorkApply]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WorkApply_dbo.tbl_Reseau_ReseauId] FOREIGN KEY([ReseauId])
REFERENCES [dbo].[tbl_Reseau] ([Id])
ALTER TABLE [dbo].[tbl_WorkApply] CHECK CONSTRAINT [FK_dbo.tbl_WorkApply_dbo.tbl_Reseau_ReseauId]
ALTER TABLE [dbo].[tbl_WorkApply]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WorkApply_dbo.tbl_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tbl_Customer] ([Id])
ALTER TABLE [dbo].[tbl_WorkApply] CHECK CONSTRAINT [FK_dbo.tbl_WorkApply_dbo.tbl_Customer_CustomerId]
--新建WorkOrder外键约束
ALTER TABLE [dbo].[tbl_WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WorkOrder_dbo.tbl_Reseau_ReseauId] FOREIGN KEY([ReseauId])
REFERENCES [dbo].[tbl_Reseau] ([Id])
ALTER TABLE [dbo].[tbl_WorkOrder] CHECK CONSTRAINT [FK_dbo.tbl_WorkOrder_dbo.tbl_Reseau_ReseauId]
ALTER TABLE [dbo].[tbl_WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WorkOrder_dbo.tbl_WorkSmallClass_WorkSmallClassId] FOREIGN KEY([WorkSmallClassId])
REFERENCES [dbo].[tbl_WorkSmallClass] ([Id])
ALTER TABLE [dbo].[tbl_WorkOrder] CHECK CONSTRAINT [FK_dbo.tbl_WorkOrder_dbo.tbl_WorkSmallClass_WorkSmallClassId]
ALTER TABLE [dbo].[tbl_WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WorkOrder_dbo.tbl_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tbl_Customer] ([Id])
ALTER TABLE [dbo].[tbl_WorkOrder] CHECK CONSTRAINT [FK_dbo.tbl_WorkOrder_dbo.tbl_Customer_CustomerId]
--新建WorkOrderDetail外键约束
ALTER TABLE [dbo].[tbl_WorkOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WorkOrderDetail_dbo.tbl_WorkOrder_WorkOrderId] FOREIGN KEY([WorkOrderId])
REFERENCES [dbo].[tbl_WorkOrder] ([Id])
ALTER TABLE [dbo].[tbl_WorkOrderDetail] CHECK CONSTRAINT [FK_dbo.tbl_WorkOrderDetail_dbo.tbl_WorkOrder_WorkOrderId]
--新建CustomerUser外键约束
ALTER TABLE [dbo].[tbl_CustomerUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerUser_dbo.tbl_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tbl_Customer] ([Id])
ALTER TABLE [dbo].[tbl_CustomerUser] CHECK CONSTRAINT [FK_dbo.tbl_CustomerUser_dbo.tbl_Customer_CustomerId]
ALTER TABLE [dbo].[tbl_CustomerUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_CustomerUser_dbo.tbl_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_CustomerUser] CHECK CONSTRAINT [FK_dbo.tbl_CustomerUser_dbo.tbl_User_UserId]
--新建ProjectCodeList外键约束
ALTER TABLE [dbo].[tbl_ProjectCodeList]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ProjectCodeList_dbo.tbl_Reseau_ReseauId] FOREIGN KEY([ReseauId])
REFERENCES [dbo].[tbl_Reseau] ([Id])
ALTER TABLE [dbo].[tbl_ProjectCodeList] CHECK CONSTRAINT [FK_dbo.tbl_ProjectCodeList_dbo.tbl_Reseau_ReseauId]
--新建ProjectCodeList外键约束
ALTER TABLE [dbo].[tbl_ProjectCodeList]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ProjectCodeList_dbo.tbl_User_ProjectManagerId] FOREIGN KEY([ProjectManagerId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_ProjectCodeList] CHECK CONSTRAINT [FK_dbo.tbl_ProjectCodeList_dbo.tbl_User_ProjectManagerId]
--新建MaterialSpecList外键约束
--ALTER TABLE [dbo].[tbl_MaterialSpecList]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_MaterialSpecList_dbo.tbl_ProjectCodeList_ProjectCode] FOREIGN KEY([ProjectCode])
--REFERENCES [dbo].[tbl_ProjectCodeList] ([ProjectCode])
--ALTER TABLE [dbo].[tbl_MaterialSpecList] CHECK CONSTRAINT [FK_dbo.tbl_MaterialSpecList_dbo.tbl_ProjectCodeList_ProjectCode]
--新增PlaceDesign外键约束
--ALTER TABLE [dbo].[tbl_PlaceDesign]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_PlaceDesign_dbo.tbl_Customer_DesignCustomerId] FOREIGN KEY([DesignCustomerId])
--REFERENCES [dbo].[tbl_Customer] ([Id])
--ALTER TABLE [dbo].[tbl_PlaceDesign] CHECK CONSTRAINT [FK_dbo.tbl_PlaceDesign_dbo.tbl_Customer_DesignCustomerId]
--ALTER TABLE [dbo].[tbl_PlaceDesign]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_PlaceDesign_dbo.tbl_Customer_SupervisorCustomerId] FOREIGN KEY([SupervisorCustomerId])
--REFERENCES [dbo].[tbl_Customer] ([Id])
--ALTER TABLE [dbo].[tbl_PlaceDesign] CHECK CONSTRAINT [FK_dbo.tbl_PlaceDesign_dbo.tbl_Customer_SupervisorCustomerId]
--ALTER TABLE [dbo].[tbl_PlaceDesign]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_PlaceDesign_dbo.tbl_User_DesignUserId] FOREIGN KEY([DesignUserId])
--REFERENCES [dbo].[tbl_User] ([Id])
--ALTER TABLE [dbo].[tbl_PlaceDesign] CHECK CONSTRAINT [FK_dbo.tbl_PlaceDesign_dbo.tbl_User_DesignUserId]
--ALTER TABLE [dbo].[tbl_PlaceDesign]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_PlaceDesign_dbo.tbl_User_SupervisorUserId] FOREIGN KEY([SupervisorUserId])
--REFERENCES [dbo].[tbl_User] ([Id])
--ALTER TABLE [dbo].[tbl_PlaceDesign] CHECK CONSTRAINT [FK_dbo.tbl_PlaceDesign_dbo.tbl_User_SupervisorUserId]
