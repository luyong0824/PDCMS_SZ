USE [PDBM]

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
--删除Addressing外键约束
ALTER TABLE [dbo].[tbl_Addressing] DROP CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Project_ProjectId]
--删除ProjectProfession外键约束
ALTER TABLE [dbo].[tbl_ProjectProfession] DROP CONSTRAINT [FK_dbo.tbl_ProjectProfession_dbo.tbl_Project_ProjectId]
--删除OperatorsSharing外键约束
ALTER TABLE [dbo].[tbl_OperatorsSharing] DROP CONSTRAINT [FK_dbo.tbl_OperatorsSharing_dbo.tbl_Remodeling_RemodelingId]
--删除Place外键约束
ALTER TABLE [dbo].[tbl_Place] DROP CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId]
--删除Planning外键约束
ALTER TABLE [dbo].[tbl_Planning] DROP CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId]
--删除Purchase外键约束
ALTER TABLE [dbo].[tbl_Purchase] DROP CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_Reseau_ReseauId]
--删除RoleMenuItem外键约束
ALTER TABLE [dbo].[tbl_RoleMenuItem] DROP CONSTRAINT [FK_dbo.tbl_RoleMenuItem_dbo.tbl_Role_RoleId]
--删除RoleUser外键约束
ALTER TABLE [dbo].[tbl_RoleUser] DROP CONSTRAINT [FK_dbo.tbl_RoleUser_dbo.tbl_Role_RoleId]
--删除Addressing外键约束
ALTER TABLE [dbo].[tbl_Addressing] DROP CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Scene_SceneId]
--删除Place外键约束
ALTER TABLE [dbo].[tbl_Place] DROP CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Scene_SceneId]
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
ALTER TABLE [dbo].[tbl_Remodeling] DROP CONSTRAINT [FK_dbo.tbl_Remodeling_dbo.tbl_Project_ProjectId]
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

--创建外键约束--------------------------------------------------------------------------------------------------------------
--新建Department外键约束
ALTER TABLE [dbo].[tbl_Department]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Department_dbo.tbl_User_ManagerUserId] FOREIGN KEY([ManagerUserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_Department] CHECK CONSTRAINT [FK_dbo.tbl_Department_dbo.tbl_User_ManagerUserId]
--新建Project表外键约束
ALTER TABLE [dbo].[tbl_Project]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_AccountingEntity_AccountingEntityId] FOREIGN KEY([AccountingEntityId])
REFERENCES [dbo].[tbl_AccountingEntity] ([Id])
ALTER TABLE [dbo].[tbl_Project] CHECK CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_AccountingEntity_AccountingEntityId]
--新建OperatorsPlanning外键约束
ALTER TABLE [dbo].[tbl_OperatorsPlanning]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Area_AreaId] FOREIGN KEY([AreaId])
REFERENCES [dbo].[tbl_Area] ([Id])
ALTER TABLE [dbo].[tbl_OperatorsPlanning] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Area_AreaId]
--新建Reseau外键约束
ALTER TABLE [dbo].[tbl_Reseau]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Reseau_dbo.tbl_Area_AreaId] FOREIGN KEY([AreaId])
REFERENCES [dbo].[tbl_Area] ([Id])
ALTER TABLE [dbo].[tbl_Reseau] CHECK CONSTRAINT [FK_dbo.tbl_Reseau_dbo.tbl_Area_AreaId]
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
ALTER TABLE [dbo].[tbl_Planning]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId] FOREIGN KEY([PlaceCategoryId])
REFERENCES [dbo].[tbl_PlaceCategory] ([Id])
ALTER TABLE [dbo].[tbl_Planning] CHECK CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId]
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
ALTER TABLE [dbo].[tbl_Addressing]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Project_ProjectId] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[tbl_Project] ([Id])
ALTER TABLE [dbo].[tbl_Addressing] CHECK CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Project_ProjectId]
--新建ProjectProfession外键约束
ALTER TABLE [dbo].[tbl_ProjectProfession]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_ProjectProfession_dbo.tbl_Project_ProjectId] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[tbl_Project] ([Id])
ALTER TABLE [dbo].[tbl_ProjectProfession] CHECK CONSTRAINT [FK_dbo.tbl_ProjectProfession_dbo.tbl_Project_ProjectId]
--新建OperatorsSharing外键约束
ALTER TABLE [dbo].[tbl_OperatorsSharing]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_OperatorsSharing_dbo.tbl_Remodeling_RemodelingId] FOREIGN KEY([RemodelingId])
REFERENCES [dbo].[tbl_Remodeling] ([Id])
ALTER TABLE [dbo].[tbl_OperatorsSharing] CHECK CONSTRAINT [FK_dbo.tbl_OperatorsSharing_dbo.tbl_Remodeling_RemodelingId]
--新建Place外键约束
ALTER TABLE [dbo].[tbl_Place]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId] FOREIGN KEY([ReseauId])
REFERENCES [dbo].[tbl_Reseau] ([Id])
ALTER TABLE [dbo].[tbl_Place] CHECK CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId]
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
ALTER TABLE [dbo].[tbl_Addressing]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Scene_SceneId] FOREIGN KEY([SceneId])
REFERENCES [dbo].[tbl_Scene] ([Id])
ALTER TABLE [dbo].[tbl_Addressing] CHECK CONSTRAINT [FK_dbo.tbl_Addressing_dbo.tbl_Scene_SceneId]
--新建Place外键约束
ALTER TABLE [dbo].[tbl_Place]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Scene_SceneId] FOREIGN KEY([SceneId])
REFERENCES [dbo].[tbl_Scene] ([Id])
ALTER TABLE [dbo].[tbl_Place] CHECK CONSTRAINT [FK_dbo.tbl_Place_dbo.tbl_Scene_SceneId]
--新建Purchase外键约束
ALTER TABLE [dbo].[tbl_Purchase]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_Scene_SceneId] FOREIGN KEY([SceneId])
REFERENCES [dbo].[tbl_Scene] ([Id])
ALTER TABLE [dbo].[tbl_Purchase] CHECK CONSTRAINT [FK_dbo.tbl_Purchase_dbo.tbl_Scene_SceneId]
--新建Planning外键约束
ALTER TABLE [dbo].[tbl_Planning]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_User_AddressingUserId] FOREIGN KEY([AddressingUserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_Planning] CHECK CONSTRAINT [FK_dbo.tbl_Planning_dbo.tbl_User_AddressingUserId]
--新建PostUser外键约束
ALTER TABLE [dbo].[tbl_PostUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_PostUser_dbo.tbl_Post_PostId] FOREIGN KEY([PostId])
REFERENCES [dbo].[tbl_Post] ([Id])
ALTER TABLE [dbo].[tbl_PostUser] CHECK CONSTRAINT [FK_dbo.tbl_PostUser_dbo.tbl_Post_PostId]
ALTER TABLE [dbo].[tbl_PostUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_PostUser_dbo.tbl_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_PostUser] CHECK CONSTRAINT [FK_dbo.tbl_PostUser_dbo.tbl_User_UserId]
--新建Project外键约束
ALTER TABLE [dbo].[tbl_Project]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_User_ManagerUserId] FOREIGN KEY([ManagerUserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_Project] CHECK CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_User_ManagerUserId]
ALTER TABLE [dbo].[tbl_Project]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_User_ResponsibleUserId] FOREIGN KEY([ResponsibleUserId])
REFERENCES [dbo].[tbl_User] ([Id])
ALTER TABLE [dbo].[tbl_Project] CHECK CONSTRAINT [FK_dbo.tbl_Project_dbo.tbl_User_ResponsibleUserId]
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
ALTER TABLE [dbo].[tbl_Remodeling]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_Remodeling_dbo.tbl_Project_ProjectId] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[tbl_Project] ([Id])
ALTER TABLE [dbo].[tbl_Remodeling] CHECK CONSTRAINT [FK_dbo.tbl_Remodeling_dbo.tbl_Project_ProjectId]
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
ALTER TABLE [dbo].[tbl_WFActivity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_WFActivityEditor_WFActivityEditorId] FOREIGN KEY([WFActivityEditorId])
REFERENCES [dbo].[tbl_WFActivityEditor] ([Id])
ALTER TABLE [dbo].[tbl_WFActivity] CHECK CONSTRAINT [FK_dbo.tbl_WFActivity_dbo.tbl_WFActivityEditor_WFActivityEditorId]
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