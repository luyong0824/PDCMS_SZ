/****** Object:  StoredProcedure [dbo].[prc_QueryWFInstancesDoingPage]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWFInstancesDoingPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@WFProcessInstanceCode nvarchar(50),
					@WFProcessInstanceName nvarchar(100),
					@WFProcessId uniqueidentifier,
					@UserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_WFActivityInstance.Id,tbl_WFActivityInstance.WFProcessInstanceId,tbl_WFProcessInstance.WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceName,
			tbl_WFProcess.WFProcessName,tbl_WFActivityInstance.ReceivedDate,tbl_User.FullName,tbl_WFProcessInstance.CreateDate
		from tbl_WFActivityInstance left join tbl_WFProcessInstance on tbl_WFActivityInstance.WFProcessInstanceId = tbl_WFProcessInstance.Id
									left join tbl_WFProcess on tbl_WFProcessInstance.WFProcessId = tbl_WFProcess.Id
									left join tbl_User on tbl_WFProcessInstance.CreateUserId = tbl_User.Id
		where (tbl_WFProcessInstance.CreateDate >= @BeginDate and tbl_WFProcessInstance.CreateDate < @EndDate) and
				(@WFProcessInstanceCode = '' or CHARINDEX(@WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceCode) > 0) and
				(@WFProcessInstanceName = '' or CHARINDEX(@WFProcessInstanceName,tbl_WFProcessInstance.WFProcessInstanceName) > 0) and
				(@WFProcessId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.WFProcessId = @WFProcessId) and
				(tbl_WFActivityInstance.WFActivityOperate = 2 or tbl_WFProcessInstance.WFProcessInstanceState = 2) and
				tbl_WFActivityInstance.WFActivityInstanceState = 2 and
				tbl_WFActivityInstance.UserId = @UserId
		order by tbl_WFActivityInstance.ReceivedDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_WFActivityInstance left join tbl_WFProcessInstance on tbl_WFActivityInstance.WFProcessInstanceId = tbl_WFProcessInstance.Id
		where (tbl_WFProcessInstance.CreateDate >= @BeginDate and tbl_WFProcessInstance.CreateDate < @EndDate) and
				(@WFProcessInstanceCode = '' or CHARINDEX(@WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceCode) > 0) and
				(@WFProcessInstanceName = '' or CHARINDEX(@WFProcessInstanceName,tbl_WFProcessInstance.WFProcessInstanceName) > 0) and
				(@WFProcessId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.WFProcessId = @WFProcessId) and
				(tbl_WFActivityInstance.WFActivityOperate = 2 or tbl_WFProcessInstance.WFProcessInstanceState = 2) and
				tbl_WFActivityInstance.WFActivityInstanceState = 2 and
				tbl_WFActivityInstance.UserId = @UserId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetWFInstancesToDo]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetWFInstancesToDo]
					@UserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_WFActivityInstance.Id,tbl_WFProcessInstance.WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceName,tbl_WFProcess.WFProcessName,
			tbl_WFActivityInstance.ReceivedDate,tbl_User.FullName,tbl_WFProcessInstance.CreateDate
		from tbl_WFActivityInstance left join tbl_WFProcessInstance on tbl_WFActivityInstance.WFProcessInstanceId = tbl_WFProcessInstance.Id
									left join tbl_WFProcess on tbl_WFProcessInstance.WFProcessId = tbl_WFProcess.Id
									left join tbl_User on tbl_WFProcessInstance.CreateUserId = tbl_User.Id
		where (tbl_WFActivityInstance.WFActivityOperate = 2 or tbl_WFProcessInstance.WFProcessInstanceState = 2) and
				tbl_WFActivityInstance.WFActivityInstanceState = 2 and
				tbl_WFActivityInstance.UserId = @UserId
		order by tbl_WFActivityInstance.ReceivedDate desc
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryPlanningsPage]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryPlanningsPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlanningCode nvarchar(50),
					@PlanningName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Urgency int,
					@TelecomDemand int,
					@MobileDemand int,
					@UnicomDemand int,
					@DemandState int,
					@Issued int,
					@AddressingState int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Planning.Id,tbl_Planning.PlanningCode,tbl_Planning.PlanningName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.Urgency,tbl_Planning.MobileDemand,tbl_Planning.TelecomDemand,tbl_Planning.UnicomDemand,tbl_Planning.DemandState,
			tbl_Planning.Issued,tbl_Planning.AddressingState,ISNULL(u1.FullName,'') as AddressingUserFullName,tbl_Planning.Remarks,u2.FullName,tbl_Planning.CreateDate,
			tbl_Planning.PlaceId
		from tbl_Planning left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User u1 on tbl_Planning.AddressingUserId = u1.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
		where (tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Planning.Urgency = @Urgency) and
				(@TelecomDemand = 0 or tbl_Planning.TelecomDemand = @TelecomDemand) and
				(@MobileDemand = 0 or tbl_Planning.MobileDemand = @MobileDemand) and
				(@UnicomDemand = 0 or tbl_Planning.UnicomDemand = @UnicomDemand) and
				(@DemandState = 0 or tbl_Planning.DemandState = @DemandState) and
				(@Issued = 0 or tbl_Planning.Issued = @Issued) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.CreateUserId = @CreateUserId)
		order by tbl_Planning.PlanningCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Planning left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
		where (tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Planning.Urgency = @Urgency) and
				(@TelecomDemand = 0 or tbl_Planning.TelecomDemand = @TelecomDemand) and
				(@MobileDemand = 0 or tbl_Planning.MobileDemand = @MobileDemand) and
				(@UnicomDemand = 0 or tbl_Planning.UnicomDemand = @UnicomDemand) and
				(@DemandState = 0 or tbl_Planning.DemandState = @DemandState) and
				(@Issued = 0 or tbl_Planning.Issued = @Issued) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.CreateUserId = @CreateUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GenerateOrderCode]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GenerateOrderCode]
					@EntityName varchar(50),
					@GenerateDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @CodePrefix nvarchar(50),
			@DateCode varchar(50),
			@SeedCode varchar(5),
			@SqlStatement nvarchar(max)

	select @CodePrefix = tbl_WFCategory.CodePrefix from tbl_WFCategory where tbl_WFCategory.EntityName = @EntityName
	set @DateCode = cast(year(@GenerateDate) as varchar(4)) + iif(len(month(@GenerateDate)) = 1,'0' + cast(month(@GenerateDate) as varchar(1)),cast(month(@GenerateDate) as varchar(2)))
	set @SqlStatement = N'select top 1 @SeedCode = tbl_OrderCodeSeed.Seed
							from tbl_OrderCodeSeed left join tbl_' + @EntityName + ' on ''' + @CodePrefix + @DateCode + ''' + tbl_OrderCodeSeed.Seed = tbl_' + @EntityName + '.OrderCode
							where tbl_' + @EntityName + '.OrderCode is null
							order by tbl_OrderCodeSeed.Seed'
	exec sp_executesql @SqlStatement,N'@SeedCode varchar(5) output',@SeedCode output
	select @CodePrefix + @DateCode + @SeedCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetUsedUsersBySend]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetUsedUsersBySend]
					@DepartmentId uniqueidentifier,
					@PostId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select '00000000-0000-0000-0000-000000000000' as Id,'请选择' as FullName
	union
	select tbl_User.Id,tbl_User.FullName
		from tbl_User left join tbl_PostUser on tbl_User.Id = tbl_PostUser.UserId
		where tbl_User.DepartmentId = @DepartmentId and
			(@PostId = '00000000-0000-0000-0000-000000000000' or tbl_PostUser.PostId = @PostId)
		order by FullName
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetUsedDepartmentsBySend]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetUsedDepartmentsBySend]
					@CompanyId uniqueidentifier,
					@PostId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	select '00000000-0000-0000-0000-000000000000' as Id,'' as DepartmentCode,'请选择' as DepartmentName
	union
	select distinct tbl_Department.Id,tbl_Department.DepartmentCode,tbl_Department.DepartmentName
		from tbl_Department left join tbl_User on tbl_Department.Id = tbl_User.DepartmentId
							left join tbl_PostUser on tbl_User.Id = tbl_PostUser.UserId
		where tbl_Department.CompanyId = @CompanyId and
			(@PostId = '00000000-0000-0000-0000-000000000000' or tbl_PostUser.PostId = @PostId)
		order by DepartmentCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryWFActivitys]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWFActivitys]
					@WFProcessId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_WFActivity.Id,tbl_WFActivity.WFActivityName,tbl_WFActivity.WFActivityOperate,ISNULL(tbl_WFActivity.WFActivityEditorId,'00000000-0000-0000-0000-000000000000') as WFActivityEditorId,
			tbl_WFActivity.WFActivityOrder,tbl_WFActivity.SerialId,tbl_WFActivity.RowId,tbl_WFActivity.Timelimit,tbl_WFActivity.CompanyId,
			ISNULL(tbl_WFActivity.DepartmentId,'00000000-0000-0000-0000-000000000000') as DepartmentId,ISNULL(tbl_WFActivity.UserId,'00000000-0000-0000-0000-000000000000') as UserId,
			ISNULL(tbl_WFActivity.PostId,'00000000-0000-0000-0000-000000000000') as PostId,tbl_Company.CompanyName,ISNULL(tbl_Department.DepartmentName,'请选择') as DepartmentName,ISNULL(tbl_User.FullName,'请选择') as FullName,
			ISNULL(tbl_WFActivityEditor.WFActivityEditorName,'无') as WFActivityEditorName,tbl_WFActivity.IsMustEdit,case tbl_WFActivity.IsMustEdit when 1 then '是' when 2 then '否' else '' end as MustEditName,tbl_WFActivity.IsNecessaryStep
		from tbl_WFActivity left join tbl_Company on tbl_WFActivity.CompanyId = tbl_Company.Id
							left join tbl_Department on tbl_WFActivity.DepartmentId = tbl_Department.Id
							left join tbl_User on tbl_WFActivity.UserId = tbl_User.Id
							left join tbl_WFActivityEditor on tbl_WFActivity.WFActivityEditorId = tbl_WFActivityEditor.Id
		where tbl_WFActivity.WFProcessId = @WFProcessId
		order by tbl_WFActivity.SerialId,tbl_WFActivity.RowId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryRoleUsers]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryRoleUsers]
							@CompanyId uniqueidentifier,
							@DepartmentId uniqueidentifier,
							@RoleId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_Department.Id,tbl_Department.DepartmentName as Name,tbl_Department.CompanyId as PId,
			'00000000-0000-0000-0000-000000000000' as RoleUserId,tbl_Department.DepartmentCode as Code,'false' as IsLeafStr,'false' as AsyncLoadStr
		from tbl_Department
		where tbl_Department.CompanyId = @CompanyId and tbl_Department.[State] = 1 and
			(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_Department.Id = @DepartmentId)
	union
	select tbl_User.Id,tbl_User.UserName + '(' + tbl_User.FullName + ')',tbl_User.DepartmentId,ISNULL(tbl_RoleUser.Id,'00000000-0000-0000-0000-000000000000'),
			'','true','false'
		from tbl_User left join tbl_Department on tbl_User.DepartmentId = tbl_Department.Id
					left join tbl_RoleUser on tbl_User.Id = tbl_RoleUser.UserId and tbl_RoleUser.RoleId = @RoleId
		where tbl_Department.CompanyId = @CompanyId and tbl_User.[State] = 1 and tbl_Department.[State] = 1 and
			(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_Department.Id = @DepartmentId)
	union
	select Id,CompanyName,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','','false','false'
		from tbl_Company
		where Id = @CompanyId and tbl_Company.[State] = 1
	order by Code,Name
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryPostUsers]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryPostUsers]
							@CompanyId uniqueidentifier,
							@DepartmentId uniqueidentifier,
							@PostId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_Department.Id,tbl_Department.DepartmentName as Name,tbl_Department.CompanyId as PId,
			'00000000-0000-0000-0000-000000000000' as PostUserId,tbl_Department.DepartmentCode as Code,'false' as IsLeafStr,'false' as AsyncLoadStr
		from tbl_Department
		where tbl_Department.CompanyId = @CompanyId and tbl_Department.[State] = 1 and
			(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_Department.Id = @DepartmentId)
	union
	select tbl_User.Id,tbl_User.UserName + '(' + tbl_User.FullName + ')',tbl_User.DepartmentId,ISNULL(tbl_PostUser.Id,'00000000-0000-0000-0000-000000000000'),
			'','true','false'
		from tbl_User left join tbl_Department on tbl_User.DepartmentId = tbl_Department.Id
					left join tbl_PostUser on tbl_User.Id = tbl_PostUser.UserId and tbl_PostUser.PostId = @PostId
		where tbl_Department.CompanyId = @CompanyId and tbl_User.[State] = 1 and tbl_Department.[State] = 1 and
			(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_Department.Id = @DepartmentId)
	union
	select Id,CompanyName,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','','false','false'
		from tbl_Company
		where Id = @CompanyId and tbl_Company.[State] = 1
	order by Code,Name
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetPostNamesByUserId]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetPostNamesByUserId]
					@UserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_Post.PostName
		from tbl_PostUser left join tbl_Post on tbl_PostUser.PostId = tbl_Post.Id
		where tbl_PostUser.UserId = @UserId and tbl_Post.[State] = 1
		order by tbl_Post.PostCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetRoleNamesByUserId]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetRoleNamesByUserId]
					@UserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_Role.RoleName
		from tbl_RoleUser left join tbl_Role on tbl_RoleUser.RoleId = tbl_Role.Id
		where tbl_RoleUser.UserId = @UserId and tbl_Role.[State] = 1
		order by tbl_Role.RoleCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryAddressingsPage]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryAddressingsPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlanningCode nvarchar(50),
					@PlanningName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Urgency int,
					@AddressingState int,
					@AddressingUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select ISNULL(tbl_Addressing.Id,'00000000-0000-0000-0000-000000000000') as Id,tbl_Planning.Id as PlanningId,tbl_Planning.PlanningCode,tbl_Planning.PlanningName,ISNULL(tbl_Addressing.PlaceName,'') as PlaceName,
			tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.Urgency,ISNULL(tbl_Addressing.Importance,0) as Importance,
			tbl_Planning.TelecomDemand,tbl_Planning.MobileDemand,tbl_Planning.UnicomDemand,ISNULL(tbl_Scene.SceneName,'') as SceneName,
			tbl_Addressing.OwnerName,tbl_Addressing.OwnerContact,tbl_Addressing.OwnerPhoneNumber,ISNULL(tbl_Project.ProjectName,'') as ProjectName,tbl_Planning.AddressingState,ISNULL(tbl_Addressing.Remarks,'') as Remarks,
			ISNULL(tbl_User.FullName,'') as AddressingUserFullName,ISNULL(u2.FullName,'') as FullName,tbl_Planning.CreateDate,tbl_Planning.PlaceId,isnull(tbl_FileAssociation.EntityName,'') as IsFile 
		from tbl_Planning left join tbl_Addressing on tbl_Planning.Id = tbl_Addressing.PlanningId
							left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Scene on tbl_Addressing.SceneId = tbl_Scene.Id
							left join tbl_Project on tbl_Addressing.ProjectId = tbl_Project.Id
							left join tbl_User on tbl_Planning.AddressingUserId = tbl_User.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
							left join tbl_FileAssociation on tbl_Addressing.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Addressing'
		where tbl_Planning.Issued = 1 and
				(tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Planning.Urgency = @Urgency) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@AddressingUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.AddressingUserId = @AddressingUserId)
		order by tbl_Planning.PlanningCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Planning left join tbl_Addressing on tbl_Planning.Id = tbl_Addressing.PlanningId
							left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
		where tbl_Planning.Issued = 1 and
				(tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Planning.Urgency = @Urgency) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@AddressingUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.AddressingUserId = @AddressingUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryPurchasesPage]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryPurchasesPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@GroupPlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@PropertyRightSql varchar(100),
					@Importance int,
					@TelecomShare int,
					@MobileShare int,
					@UnicomShare int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @TempPropertyRight table(value int)
	insert @TempPropertyRight exec(@PropertyRightSql)

	select tbl_Purchase.Id,tbl_Purchase.GroupPlaceCode,tbl_Purchase.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			tbl_PlaceCategory.PlaceCategoryName,tbl_Purchase.Lng,tbl_Purchase.Lat,tbl_Purchase.PropertyRight,tbl_Purchase.Importance,
			tbl_Purchase.TelecomShare,tbl_Purchase.MobileShare,tbl_Purchase.UnicomShare,tbl_Scene.SceneName,tbl_Purchase.OwnerName,
			tbl_Purchase.OwnerContact,tbl_Purchase.OwnerPhoneNumber,tbl_Purchase.Remarks,tbl_User.FullName,tbl_Purchase.PurchaseDate,
			tbl_Purchase.PlaceId,isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_Purchase left join tbl_Reseau on tbl_Purchase.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Purchase.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Scene on tbl_Purchase.SceneId = tbl_Scene.Id
							left join tbl_User on tbl_Purchase.CreateUserId = tbl_User.Id
							left join tbl_FileAssociation on tbl_Purchase.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Purchase'
							left join @TempPropertyRight t on tbl_Purchase.PropertyRight = t.value
		where (tbl_Purchase.PurchaseDate >= @BeginDate and tbl_Purchase.PurchaseDate < @EndDate) and
				(@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Purchase.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Purchase.PlaceName) > 0) and
				tbl_Purchase.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Purchase.PlaceCategoryId = @PlaceCategoryId) and
				(@Importance = 0 or tbl_Purchase.Importance = @Importance) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Purchase.ReseauId = @ReseauId) and
				(@TelecomShare = 0 or tbl_Purchase.TelecomShare = @TelecomShare) and
				(@MobileShare = 0 or tbl_Purchase.MobileShare = @MobileShare) and
				(@UnicomShare = 0 or tbl_Purchase.UnicomShare = @UnicomShare) and
				t.value is not null
		order by tbl_Purchase.PlaceCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Purchase left join tbl_Reseau on tbl_Purchase.ReseauId = tbl_Reseau.Id
							left join @TempPropertyRight t on tbl_Purchase.PropertyRight = t.value
		where (tbl_Purchase.PurchaseDate >= @BeginDate and tbl_Purchase.PurchaseDate < @EndDate) and
				(@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Purchase.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Purchase.PlaceName) > 0) and
				tbl_Purchase.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Purchase.PlaceCategoryId = @PlaceCategoryId) and
				(@Importance = 0 or tbl_Purchase.Importance = @Importance) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Purchase.ReseauId = @ReseauId) and
				(@TelecomShare = 0 or tbl_Purchase.TelecomShare = @TelecomShare) and
				(@MobileShare = 0 or tbl_Purchase.MobileShare = @MobileShare) and
				(@UnicomShare = 0 or tbl_Purchase.UnicomShare = @UnicomShare) and
				t.value is not null
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryPlacesPage]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryPlacesPage]
					@PageIndex int,
					@PageSize int,
					@GroupPlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@PropertyRight int,
					@Importance int,
					@TelecomShare int,
					@MobileShare int,
					@UnicomShare int,
					@State int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Place.Id,tbl_Place.GroupPlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.Profession,
			tbl_PlaceCategory.PlaceCategoryName,tbl_Place.Lng,tbl_Place.Lat,tbl_Place.Importance,tbl_Place.PropertyRight,
			tbl_Place.TelecomShare,tbl_Place.MobileShare,tbl_Place.UnicomShare,tbl_Scene.SceneName,tbl_Place.OwnerName,
			tbl_Place.OwnerContact,tbl_Place.OwnerPhoneNumber,tbl_Place.[State],tbl_User.FullName,tbl_Place.CreateDate,
			isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
						left join tbl_Scene on tbl_Place.SceneId = tbl_Scene.Id
						left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
						left join tbl_FileAssociation on tbl_Place.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Place'
		where (@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Place.GroupPlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PropertyRight = 0 or tbl_Place.PropertyRight = @PropertyRight) and
				(@Importance = 0 or tbl_Place.Importance = @Importance) and
				(@TelecomShare = 0 or tbl_Place.TelecomShare = @TelecomShare) and
				(@MobileShare = 0 or tbl_Place.MobileShare = @MobileShare) and
				(@UnicomShare = 0 or tbl_Place.UnicomShare = @UnicomShare) and
				(@State = 0 or tbl_Place.[State] = @State)
		order by tbl_Place.PlaceCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
		where (@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Place.GroupPlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PropertyRight = 0 or tbl_Place.PropertyRight = @PropertyRight) and
				(@Importance = 0 or tbl_Place.Importance = @Importance) and
				(@TelecomShare = 0 or tbl_Place.TelecomShare = @TelecomShare) and
				(@MobileShare = 0 or tbl_Place.MobileShare = @MobileShare) and
				(@UnicomShare = 0 or tbl_Place.UnicomShare = @UnicomShare) and
				(@State = 0 or tbl_Place.[State] = @State)
END

GO

/****** Object:  StoredProcedure [dbo].[prc_QueryOperatorsSharingsPageBySelect]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryOperatorsSharingsPageBySelect]
					@PageIndex int,
					@PageSize int,
					@PlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@CompanyId uniqueidentifier,
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Urgency int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_OperatorsSharing.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Place.Lng,tbl_Place.Lat,tbl_OperatorsSharing.PowerUsed,tbl_OperatorsSharing.PoleNumber,tbl_OperatorsSharing.CabinetNumber,
			tbl_OperatorsSharing.Urgency,tbl_OperatorsSharing.Remarks,tbl_Company.CompanyName,tbl_User.FullName,tbl_OperatorsSharing.CreateDate,
			tbl_OperatorsSharing.PlaceId
		from tbl_OperatorsSharing left join tbl_Place on tbl_OperatorsSharing.PlaceId = tbl_Place.Id
									left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
									left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
									left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
									left join tbl_Company on tbl_OperatorsSharing.CompanyId = tbl_Company.Id
									left join tbl_User on tbl_OperatorsSharing.CreateUserId = tbl_User.Id
		where tbl_OperatorsSharing.Solved = 2 and 
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@CompanyId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsSharing.CompanyId = @CompanyId) and
				tbl_OperatorsSharing.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_OperatorsSharing.Urgency = @Urgency)
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_OperatorsSharing left join tbl_Place on tbl_OperatorsSharing.PlaceId = tbl_Place.Id
									left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
		where tbl_OperatorsSharing.Solved = 2 and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@CompanyId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsSharing.CompanyId = @CompanyId) and
				tbl_OperatorsSharing.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_OperatorsSharing.Urgency = @Urgency)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryRemodelingsPage]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryRemodelingsPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Urgency int,
					@OrderState int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Remodeling.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Place.Lng,tbl_Place.Lat,tbl_Place.PropertyRight,tbl_Place.TelecomShare,tbl_Place.MobileShare,tbl_Place.UnicomShare,
			tbl_Remodeling.Urgency,isnull(tbl_Project.ProjectName,'') as ProjectName,tbl_Remodeling.OrderState,tbl_Remodeling.Remarks,tbl_User.FullName,tbl_Remodeling.CreateDate,
			tbl_Remodeling.PlaceId,isnull(pm.FullName,'') as ProjectManagerName,tbl_Remodeling.ProjectId,tbl_Remodeling.ProjectManagerId
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User on tbl_Remodeling.CreateUserId = tbl_User.Id
							left join tbl_Project on tbl_Remodeling.ProjectId = tbl_Project.Id
							left join tbl_User pm on tbl_Remodeling.ProjectManagerId = pm.Id
		where (tbl_Remodeling.CreateDate >= @BeginDate and tbl_Remodeling.CreateDate < @EndDate) and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_Remodeling.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Remodeling.Urgency = @Urgency) and
				(@OrderState = 0 or tbl_Remodeling.OrderState = @OrderState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Remodeling.CreateUserId = @CreateUserId)
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
		where (tbl_Remodeling.CreateDate >= @BeginDate and tbl_Remodeling.CreateDate < @EndDate) and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_Remodeling.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Remodeling.Urgency = @Urgency) and
				(@OrderState = 0 or tbl_Remodeling.OrderState = @OrderState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Remodeling.CreateUserId = @CreateUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryOperatorsSharingsPage]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryOperatorsSharingsPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@CompanyId uniqueidentifier,
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Urgency int,
					@Solved int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_OperatorsSharing.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Place.Lng,tbl_Place.Lat,tbl_Place.PropertyRight,tbl_OperatorsSharing.PowerUsed,tbl_OperatorsSharing.PoleNumber,tbl_OperatorsSharing.CabinetNumber,
			tbl_OperatorsSharing.Urgency,tbl_OperatorsSharing.Solved,tbl_OperatorsSharing.Remarks,tbl_Company.CompanyName,tbl_User.FullName,tbl_OperatorsSharing.CreateDate,
			tbl_OperatorsSharing.PlaceId
		from tbl_OperatorsSharing left join tbl_Place on tbl_OperatorsSharing.PlaceId = tbl_Place.Id
									left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
									left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
									left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
									left join tbl_Company on tbl_OperatorsSharing.CompanyId = tbl_Company.Id
									left join tbl_User on tbl_OperatorsSharing.CreateUserId = tbl_User.Id
		where (tbl_OperatorsSharing.CreateDate >= @BeginDate and tbl_OperatorsSharing.CreateDate < @EndDate) and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_OperatorsSharing.CompanyId = @CompanyId and
				tbl_OperatorsSharing.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_OperatorsSharing.Urgency = @Urgency) and
				(@Solved = 0 or tbl_OperatorsSharing.Solved = @Solved)
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_OperatorsSharing left join tbl_Place on tbl_OperatorsSharing.PlaceId = tbl_Place.Id
									left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
		where (tbl_OperatorsSharing.CreateDate >= @BeginDate and tbl_OperatorsSharing.CreateDate < @EndDate) and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_OperatorsSharing.CompanyId = @CompanyId and
				tbl_OperatorsSharing.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_OperatorsSharing.Urgency = @Urgency) and
				(@Solved = 0 or tbl_OperatorsSharing.Solved = @Solved)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryOperatorsConfirmDetailsPage]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryOperatorsConfirmDetailsPage]
					@PageIndex int,
					@PageSize int,
					@PlanningCode nvarchar(50),
					@PlanningName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Demand int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_OperatorsConfirmDetail.Id,tbl_Planning.PlanningCode,tbl_Planning.PlanningName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Planning.Lng,tbl_Planning.Lat,tbl_OperatorsConfirmDetail.MobileDemand,tbl_OperatorsConfirmDetail.TelecomDemand,tbl_OperatorsConfirmDetail.UnicomDemand,
			tbl_OperatorsConfirmDetail.CreateDate as ConfirmDate,tbl_User.FullName,tbl_Planning.CreateDate,tbl_OperatorsConfirmDetail.PlanningId
		from tbl_OperatorsConfirmDetail left join tbl_Planning on tbl_OperatorsConfirmDetail.PlanningId = tbl_Planning.Id
										left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
										left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
										left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
										left join tbl_User on tbl_Planning.CreateUserId = tbl_User.Id
		where (@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Demand = 0 or
					(@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and tbl_OperatorsConfirmDetail.MobileDemand = @Demand) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and tbl_OperatorsConfirmDetail.TelecomDemand = @Demand) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and tbl_OperatorsConfirmDetail.UnicomDemand = @Demand))
		order by tbl_Planning.PlanningCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_OperatorsConfirmDetail left join tbl_Planning on tbl_OperatorsConfirmDetail.PlanningId = tbl_Planning.Id
										left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
		where (@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Demand = 0 or
					(@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and tbl_OperatorsConfirmDetail.MobileDemand = @Demand) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and tbl_OperatorsConfirmDetail.TelecomDemand = @Demand) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and tbl_OperatorsConfirmDetail.UnicomDemand = @Demand))
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryOperatorsPlanningsPageBySelect]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryOperatorsPlanningsPageBySelect]
						@PageIndex int,
						@PageSize int,
						@PlanningCode nvarchar(50),
						@PlanningName nvarchar(100),
						@CompanyId uniqueidentifier,
						@Profession int,
						@PlaceCategoryId uniqueidentifier,
						@AreaId uniqueidentifier,
						@Urgency int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_OperatorsPlanning.Id,tbl_OperatorsPlanning.PlanningCode,tbl_OperatorsPlanning.PlanningName,tbl_Area.AreaName,
			tbl_PlaceCategory.PlaceCategoryName,tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat,tbl_OperatorsPlanning.AntennaHeight,
			tbl_OperatorsPlanning.PoleNumber,tbl_OperatorsPlanning.CabinetNumber,tbl_OperatorsPlanning.Urgency,tbl_OperatorsPlanning.Remarks,
			tbl_Company.CompanyName,tbl_User.FullName,tbl_OperatorsPlanning.CreateDate
		from tbl_OperatorsPlanning left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
									left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
									left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
									left join tbl_User on tbl_OperatorsPlanning.CreateUserId = tbl_User.Id
		where tbl_OperatorsPlanning.Solved = 2 and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_OperatorsPlanning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_OperatorsPlanning.PlanningName) > 0) and
				(@CompanyId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.CompanyId = @CompanyId) and
				tbl_OperatorsPlanning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.AreaId = @AreaId) and
				(@Urgency = 0 or tbl_OperatorsPlanning.Urgency = @Urgency)
		order by tbl_OperatorsPlanning.PlanningCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_OperatorsPlanning
		where tbl_OperatorsPlanning.Solved = 2 and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_OperatorsPlanning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_OperatorsPlanning.PlanningName) > 0) and
				(@CompanyId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.CompanyId = @CompanyId) and
				tbl_OperatorsPlanning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.AreaId = @AreaId) and
				(@Urgency = 0 or tbl_OperatorsPlanning.Urgency = @Urgency)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryOperatorsPlanningsPage]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryOperatorsPlanningsPage]
						@PageIndex int,
						@PageSize int,
						@BeginDate datetime,
						@EndDate datetime,
						@PlanningCode nvarchar(50),
						@PlanningName nvarchar(100),
						@CompanyId uniqueidentifier,
						@Profession int,
						@PlaceCategoryId uniqueidentifier,
						@AreaId uniqueidentifier,
						@Urgency int,
						@Solved int,
						@ToShared int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_OperatorsPlanning.Id,tbl_OperatorsPlanning.PlanningCode,tbl_OperatorsPlanning.PlanningName,tbl_Area.AreaName,
			tbl_PlaceCategory.PlaceCategoryName,tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat,tbl_OperatorsPlanning.AntennaHeight,
			tbl_OperatorsPlanning.PoleNumber,tbl_OperatorsPlanning.CabinetNumber,tbl_OperatorsPlanning.Urgency,tbl_OperatorsPlanning.Solved,
			tbl_OperatorsPlanning.ToShared,tbl_OperatorsPlanning.Remarks,tbl_Company.CompanyName,tbl_User.FullName,tbl_OperatorsPlanning.CreateDate
		from tbl_OperatorsPlanning left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
									left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
									left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
									left join tbl_User on tbl_OperatorsPlanning.CreateUserId = tbl_User.Id
		where (tbl_OperatorsPlanning.CreateDate >= @BeginDate and tbl_OperatorsPlanning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_OperatorsPlanning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_OperatorsPlanning.PlanningName) > 0) and
				(@CompanyId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.CompanyId = @CompanyId) and
				tbl_OperatorsPlanning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.AreaId = @AreaId) and
				(@Urgency = 0 or tbl_OperatorsPlanning.Urgency = @Urgency) and
				(@Solved = 0 or tbl_OperatorsPlanning.Solved = @Solved) and
				(@ToShared = 0 or tbl_OperatorsPlanning.ToShared = @ToShared)
		order by tbl_OperatorsPlanning.PlanningCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_OperatorsPlanning
		where (tbl_OperatorsPlanning.CreateDate >= @BeginDate and tbl_OperatorsPlanning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_OperatorsPlanning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_OperatorsPlanning.PlanningName) > 0) and
				(@CompanyId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.CompanyId = @CompanyId) and
				tbl_OperatorsPlanning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_OperatorsPlanning.AreaId = @AreaId) and
				(@Urgency = 0 or tbl_OperatorsPlanning.Urgency = @Urgency) and
				(@Solved = 0 or tbl_OperatorsPlanning.Solved = @Solved) and
				(@ToShared = 0 or tbl_OperatorsPlanning.ToShared = @ToShared)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryRoleMenuItems]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryRoleMenuItems]
						@RoleId uniqueidentifier,
						@MenuId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_Menu.Id,tbl_Menu.MenuName as Name,tbl_Menu.IndexId,'00000000-0000-0000-0000-000000000000' as PId,
			'00000000-0000-0000-0000-000000000000' as RoleMenuItemId,'false' as IsLeafStr,'false' as AsyncLoadStr
		from tbl_Menu
		where @MenuId = '00000000-0000-0000-0000-000000000000' or tbl_Menu.Id = @MenuId
	union
	select tbl_MenuSub.Id,tbl_MenuSub.MenuSubName,tbl_MenuSub.IndexId,tbl_MenuSub.MenuId,'00000000-0000-0000-0000-000000000000',
			'false','false'
		from tbl_MenuSub
		where @MenuId = '00000000-0000-0000-0000-000000000000' or tbl_MenuSub.MenuId = @MenuId
	union
	select tbl_MenuItem.Id,tbl_MenuItem.MenuItemName,tbl_MenuItem.IndexId,tbl_MenuItem.MenuSubId,ISNULL(tbl_RoleMenuItem.Id,'00000000-0000-0000-0000-000000000000'),
			'true','false'
		from tbl_MenuItem left join tbl_RoleMenuItem on tbl_MenuItem.Id = tbl_RoleMenuItem.MenuItemId and tbl_RoleMenuItem.RoleId = @RoleId
						left join tbl_MenuSub on tbl_MenuItem.MenuSubId = tbl_MenuSub.Id
		where (@MenuId = '00000000-0000-0000-0000-000000000000' or tbl_MenuSub.MenuId = @MenuId) and
				tbl_MenuItem.IsDisplay = 1
	union
	select '00000000-0000-0000-0000-000000000000','菜单',0,null,'00000000-0000-0000-0000-000000000000','false','false'
	order by IndexId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryUsedUsersPageBySelect]    Script Date: 2015/5/28 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryUsedUsersPageBySelect]
					@PageIndex int,
					@PageSize int,
					@CompanyId uniqueidentifier,
					@DepartmentId uniqueidentifier,
					@FullName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_User.Id,tbl_User.UserName,tbl_User.FullName,tbl_Department.DepartmentName
		from tbl_User left join tbl_Department on tbl_User.DepartmentId = tbl_Department.Id
		where tbl_Department.CompanyId = @CompanyId and
				(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_User.DepartmentId = @DepartmentId) and
				(@FullName = '' or CHARINDEX(@FullName,tbl_User.FullName) > 0) and
				tbl_Department.[State] = 1 and tbl_User.[State] = 1
		order by tbl_Department.DepartmentCode,tbl_User.UserName offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_User left join tbl_Department on tbl_User.DepartmentId = tbl_Department.Id
		where tbl_Department.CompanyId = @CompanyId and
				(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_User.DepartmentId = @DepartmentId) and
				(@FullName = '' or CHARINDEX(@FullName,tbl_User.FullName) > 0) and
				tbl_Department.[State] = 1 and tbl_User.[State] = 1
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetMenuInfoByUserId]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetMenuInfoByUserId]
						@UserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @TempTable table(MenuId uniqueidentifier,MenuName nvarchar(50),MenuIndexId int,MenuSubId uniqueidentifier,MenuSubName nvarchar(50),
								MenuSubIndexId int,MenuItemId uniqueidentifier,MenuItemName nvarchar(50),MenuItemPath varchar(100),MenuItemIndexId int)
	insert @TempTable
	select tbl_Menu.Id,tbl_Menu.MenuName,tbl_Menu.IndexId,tbl_MenuSub.Id,tbl_MenuSub.MenuSubName,tbl_MenuSub.IndexId,tbl_MenuItem.Id,
			tbl_MenuItem.MenuItemName,tbl_MenuItem.MenuItemPath,tbl_MenuItem.IndexId
		from tbl_Menu inner join tbl_MenuSub on tbl_Menu.Id = tbl_MenuSub.MenuId
					inner join tbl_MenuItem on tbl_MenuSub.Id = tbl_MenuItem.MenuSubId
					inner join tbl_RoleMenuItem on tbl_MenuItem.Id = tbl_RoleMenuItem.MenuItemId
					inner join tbl_Role on tbl_RoleMenuItem.RoleId = tbl_Role.Id
					inner join tbl_RoleUser on tbl_Role.Id = tbl_RoleUser.RoleId
		where tbl_Role.[State] = 1 and tbl_RoleUser.UserId = @UserId and tbl_MenuItem.IsDisplay = 1
		group by tbl_Menu.Id,tbl_Menu.MenuName,tbl_Menu.IndexId,tbl_MenuSub.Id,tbl_MenuSub.MenuSubName,tbl_MenuSub.IndexId,
					tbl_MenuItem.Id,tbl_MenuItem.MenuItemName,tbl_MenuItem.MenuItemPath,tbl_MenuItem.IndexId
		order by tbl_Menu.IndexId,tbl_MenuSub.IndexId,tbl_MenuItem.IndexId

	select t.MenuId as Id,t.MenuName as Name,null as PId,'' as Url,t.MenuIndexId as IndexId
		from @TempTable t
		group by t.MenuId,t.MenuName,t.MenuIndexId
	union
	select t.MenuSubId,t.MenuSubName,t.MenuId,'',t.MenuSubIndexId
		from @TempTable t
		group by t.MenuSubId,t.MenuSubName,t.MenuId,t.MenuSubIndexId
	union
	select t.MenuItemId,t.MenuItemName,t.MenuSubId,t.MenuItemPath,t.MenuItemIndexId
		from @TempTable t
		group by t.MenuItemId,t.MenuItemName,t.MenuSubId,t.MenuItemPath,t.MenuName,t.MenuSubName,t.MenuItemName,t.MenuItemIndexId
	order by IndexId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryUsersPage]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryUsersPage]
					@PageIndex int,
					@PageSize int,
					@CompanyId uniqueidentifier,
					@DepartmentId uniqueidentifier,
					@UserName nvarchar(50),
					@FullName nvarchar(50),
					@Email nvarchar(100),
					@PhoneNumber nvarchar(50),
					@State int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_User.Id,tbl_User.UserName,tbl_User.FullName,tbl_User.Email,tbl_User.PhoneNumber,tbl_Company.CompanyName,
			tbl_Department.DepartmentName,tbl_User.[State]
		from tbl_User left join tbl_Department on tbl_User.DepartmentId = tbl_Department.Id
					left join tbl_Company on tbl_Department.CompanyId = tbl_Company.Id
		where tbl_Department.CompanyId = @CompanyId and
				(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_User.DepartmentId = @DepartmentId) and
				(@UserName = '' or CHARINDEX(@UserName,tbl_User.UserName) > 0) and
				(@FullName = '' or CHARINDEX(@FullName,tbl_User.FullName) > 0) and
				(@Email = '' or CHARINDEX(@Email,tbl_User.Email) > 0) and
				(@PhoneNumber = '' or CHARINDEX(@PhoneNumber,tbl_User.PhoneNumber) > 0) and
				(@State = 0 or tbl_User.[State] = @State) and
				tbl_User.IsCurrentUsed = 1
		order by tbl_Department.DepartmentCode,tbl_User.UserName offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_User left join tbl_Department on tbl_User.DepartmentId = tbl_Department.Id
		where tbl_Department.CompanyId = @CompanyId and
				(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_User.DepartmentId = @DepartmentId) and
				(@UserName = '' or CHARINDEX(@UserName,tbl_User.UserName) > 0) and
				(@FullName = '' or CHARINDEX(@FullName,tbl_User.FullName) > 0) and
				(@Email = '' or CHARINDEX(@Email,tbl_User.Email) > 0) and
				(@PhoneNumber = '' or CHARINDEX(@PhoneNumber,tbl_User.PhoneNumber) > 0) and
				(@State = 0 or tbl_User.[State] = @State) and
				tbl_User.IsCurrentUsed = 1
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryProjectsPage]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectsPage]
					@PageIndex int,
					@PageSize int,
					@ProjectCode nvarchar(50),
					@ProjectName nvarchar(100),
					@ProjectFullName nvarchar(100),
					@ProjectCategory int,
					@AccountingEntityId uniqueidentifier,
					@ProjectProgress int,
					@State int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Project.Id,tbl_Project.ProjectCode,tbl_Project.ProjectName,tbl_Project.ProjectFullName,tbl_Project.ProjectCategory,
			tbl_AccountingEntity.AccountingEntityName,tbl_Project.ProjectProgress,tbl_Project.[State],tbl_Project.Remarks
		from tbl_Project left join tbl_AccountingEntity on tbl_Project.AccountingEntityId = tbl_AccountingEntity.Id
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_Project.ProjectCode) > 0) and
				(@ProjectName = '' or CHARINDEX(@ProjectName,tbl_Project.ProjectName) > 0) and
				(@ProjectFullName = '' or CHARINDEX(@ProjectFullName,tbl_Project.ProjectFullName) > 0) and
				(@ProjectCategory = 0 or tbl_Project.ProjectCategory = @ProjectCategory) and
				(@AccountingEntityId = '00000000-0000-0000-0000-000000000000' or tbl_Project.AccountingEntityId = @AccountingEntityId) and
				(@ProjectProgress = 0 or tbl_Project.ProjectProgress = @ProjectProgress) and
				(@State = 0 or tbl_Project.[State] = @State)
		order by tbl_Project.ProjectCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Project
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_Project.ProjectCode) > 0) and
				(@ProjectName = '' or CHARINDEX(@ProjectName,tbl_Project.ProjectName) > 0) and
				(@ProjectFullName = '' or CHARINDEX(@ProjectFullName,tbl_Project.ProjectFullName) > 0) and
				(@ProjectCategory = 0 or tbl_Project.ProjectCategory = @ProjectCategory) and
				(@AccountingEntityId = '00000000-0000-0000-0000-000000000000' or tbl_Project.AccountingEntityId = @AccountingEntityId) and
				(@ProjectProgress = 0 or tbl_Project.ProjectProgress = @ProjectProgress) and
				(@State = 0 or tbl_Project.[State] = @State)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GenerateCode]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GenerateCode]
					@EntityName varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @Id uniqueidentifier,
			@Digit int,
			@Prefix varchar(50),
			@Seed int,
			@RowVersion timestamp,
			@NewSeed int,
			@Differ int,
			@Counter int,
			@PlaceHolder varchar(max)

	begin tran tran_GenerateCode
		if exists (select * from tbl_CodeSeed where EntityName = @EntityName)
		begin
			select @Id = Id,@Digit = Digit,@Prefix = Prefix,@Seed = Seed,@RowVersion = [RowVersion]
				from tbl_CodeSeed
				where EntityName = @EntityName

			set @NewSeed = @Seed + 1

			update tbl_CodeSeed set Seed = @NewSeed where Id = @Id and [RowVersion] = @RowVersion

			if @@ROWCOUNT > 0
			begin
				set @Counter = 1
				set @PlaceHolder = ''
				if @Digit > 0 and LEN(@NewSeed) < @Digit
				begin
					set @Differ = @Digit - LEN(@NewSeed)
					while @Counter <= @Differ
					begin
						set @PlaceHolder += '0'
						set @Counter += 1
					end
				end

				select @Prefix + @PlaceHolder + CAST(@NewSeed as varchar(max))
			end
			else
			begin
				select ''
			end
		end
		else
		begin
			select ''
		end
	commit tran tran_GenerateCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryProjectsPageBySelect]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectsPageBySelect]
					@PageIndex int,
					@PageSize int,
					@ProjectCode nvarchar(50),
					@ProjectName nvarchar(100),
					@ProjectFullName nvarchar(100),
					@AccountingEntityId uniqueidentifier,
					@IsCheckedProjectProgress1 int,
					@IsCheckedProjectProgress2 int,
					@IsCheckedState1 int,
					@IsCheckedState2 int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Project.Id,tbl_Project.ProjectCode,tbl_Project.ProjectName,tbl_Project.ProjectFullName,
			tbl_AccountingEntity.AccountingEntityName,tbl_Project.ProjectProgress,tbl_Project.[State]
		from tbl_Project left join tbl_AccountingEntity on tbl_Project.AccountingEntityId = tbl_AccountingEntity.Id
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_Project.ProjectCode) > 0) and
				(@ProjectName = '' or CHARINDEX(@ProjectName,tbl_Project.ProjectName) > 0) and
				(@ProjectFullName = '' or CHARINDEX(@ProjectFullName,tbl_Project.ProjectFullName) > 0) and
				(@AccountingEntityId = '00000000-0000-0000-0000-000000000000' or tbl_Project.AccountingEntityId = @AccountingEntityId) and
				((@IsCheckedProjectProgress1 = 1 and tbl_Project.ProjectProgress = 1) or (@IsCheckedProjectProgress2 = 1 and tbl_Project.ProjectProgress = 2)) and 
				((@IsCheckedState1 = 1 and tbl_Project.[State] = 1) or (@IsCheckedState2 = 1 and tbl_Project.[State] = 2))
		order by tbl_Project.ProjectCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Project left join tbl_AccountingEntity on tbl_Project.AccountingEntityId = tbl_AccountingEntity.Id
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_Project.ProjectCode) > 0) and
				(@ProjectName = '' or CHARINDEX(@ProjectName,tbl_Project.ProjectName) > 0) and
				(@ProjectFullName = '' or CHARINDEX(@ProjectFullName,tbl_Project.ProjectFullName) > 0) and
				(@AccountingEntityId = '00000000-0000-0000-0000-000000000000' or tbl_Project.AccountingEntityId = @AccountingEntityId) and
				((@IsCheckedProjectProgress1 = 1 and tbl_Project.ProjectProgress = 1) or (@IsCheckedProjectProgress2 = 1 and tbl_Project.ProjectProgress = 2)) and 
				((@IsCheckedState1 = 1 and tbl_Project.[State] = 1) or (@IsCheckedState2 = 1 and tbl_Project.[State] = 2))
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryPlacesPageBySelect]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryPlacesPageBySelect]
					@PageIndex int,
					@PageSize int,
					@GroupPlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@PropertyRight int,
					@TelecomShare int,
					@MobileShare int,
					@UnicomShare int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Place.Id,tbl_Place.GroupPlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.Profession,
			tbl_PlaceCategory.PlaceCategoryName,tbl_Place.Lng,tbl_Place.Lat,tbl_Place.PropertyRight,
			tbl_Place.TelecomShare,tbl_Place.MobileShare,tbl_Place.UnicomShare,tbl_Reseau.AreaId,tbl_Place.ReseauId,tbl_Place.PlaceCategoryId
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
		where tbl_Place.[State] = 1 and
				(@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PropertyRight = 0 or tbl_Place.PropertyRight = @PropertyRight) and
				(@TelecomShare = 0 or tbl_Place.TelecomShare = @TelecomShare) and
				(@MobileShare = 0 or tbl_Place.MobileShare = @MobileShare) and
				(@UnicomShare = 0 or tbl_Place.UnicomShare = @UnicomShare)
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
		where tbl_Place.[State] = 1 and
				(@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PropertyRight = 0 or tbl_Place.PropertyRight = @PropertyRight) and
				(@TelecomShare = 0 or tbl_Place.TelecomShare = @TelecomShare) and
				(@MobileShare = 0 or tbl_Place.MobileShare = @MobileShare) and
				(@UnicomShare = 0 or tbl_Place.UnicomShare = @UnicomShare)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryOperatorsPlanningsByDistance]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryOperatorsPlanningsByDistance]
					@OperatorsPlanningId uniqueidentifier,
					@PlanningId uniqueidentifier,
					@Profession int,
					@Distance decimal(18,5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @Lng decimal(18,5),
			@Lat decimal(18,5)

	if @PlanningId = '00000000-0000-0000-0000-000000000000'
	begin
		select @Lng = Lng,@Lat = Lat from tbl_OperatorsPlanning where Id = @OperatorsPlanningId;
		with cte as
		(
			select tbl_OperatorsPlanning.Id,tbl_OperatorsPlanning.PlanningCode,tbl_OperatorsPlanning.PlanningName,tbl_Area.AreaName,tbl_PlaceCategory.PlaceCategoryName,
					tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat,tbl_Company.CompanyName,tbl_OperatorsPlanning.AntennaHeight,
					tbl_OperatorsPlanning.PoleNumber,tbl_OperatorsPlanning.CabinetNumber,tbl_OperatorsPlanning.Urgency,tbl_User.FullName,tbl_OperatorsPlanning.CreateDate,
					cast(dbo.func_GetDistance(@Lng,@Lat,tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat) as decimal(18,3)) as Distance,
					ISNULL(tbl_OperatorsPlanning.PlanningId,'00000000-0000-0000-0000-000000000000') as PlanningId
				from tbl_OperatorsPlanning left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
											left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
											left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
											left join tbl_User on tbl_OperatorsPlanning.CreateUserId = tbl_User.Id
				where tbl_OperatorsPlanning.Solved = 2 and
						tbl_OperatorsPlanning.Profession = @Profession
		)
		select * from cte where cte.Distance <= @Distance order by cte.Distance
	end
	else
	begin
		select @Lng = Lng,@Lat = Lat from tbl_Planning where Id = @PlanningId;
		with cte as
		(
			select tbl_OperatorsPlanning.Id,tbl_OperatorsPlanning.PlanningCode,tbl_OperatorsPlanning.PlanningName,tbl_Area.AreaName,tbl_PlaceCategory.PlaceCategoryName,
					tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat,tbl_Company.CompanyName,tbl_OperatorsPlanning.AntennaHeight,
					tbl_OperatorsPlanning.PoleNumber,tbl_OperatorsPlanning.CabinetNumber,tbl_OperatorsPlanning.Urgency,tbl_User.FullName,tbl_OperatorsPlanning.CreateDate,
					cast(dbo.func_GetDistance(@Lng,@Lat,tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat) as decimal(18,3)) as Distance,
					'00000000-0000-0000-0000-000000000000' as PlanningId
				from tbl_OperatorsPlanning left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
											left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
											left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
											left join tbl_User on tbl_OperatorsPlanning.CreateUserId = tbl_User.Id
				where tbl_OperatorsPlanning.Solved = 2 and
						tbl_OperatorsPlanning.Profession = @Profession
		)
		select * from cte where cte.Distance <= @Distance
		union
		select tbl_OperatorsPlanning.Id,tbl_OperatorsPlanning.PlanningCode,tbl_OperatorsPlanning.PlanningName,tbl_Area.AreaName,tbl_PlaceCategory.PlaceCategoryName,
					tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat,tbl_Company.CompanyName,tbl_OperatorsPlanning.AntennaHeight,
					tbl_OperatorsPlanning.PoleNumber,tbl_OperatorsPlanning.CabinetNumber,tbl_OperatorsPlanning.Urgency,tbl_User.FullName,tbl_OperatorsPlanning.CreateDate,
					cast(dbo.func_GetDistance(@Lng,@Lat,tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat) as decimal(18,3)) as Distance,
					tbl_OperatorsPlanning.PlanningId
				from tbl_OperatorsPlanning left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
											left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
											left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
											left join tbl_User on tbl_OperatorsPlanning.CreateUserId = tbl_User.Id
				where tbl_OperatorsPlanning.Solved = 1 and
						tbl_OperatorsPlanning.PlanningId = @PlanningId
		order by Distance
	end
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryOperatorsSharingsByPlace]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryOperatorsSharingsByPlace]
					@OperatorsSharingId uniqueidentifier,
					@RemodelingId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PlaceId uniqueidentifier

	if @RemodelingId = '00000000-0000-0000-0000-000000000000'
	begin
		select @PlaceId = PlaceId from tbl_OperatorsSharing where Id = @OperatorsSharingId

		select tbl_OperatorsSharing.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
				tbl_PlaceCategory.PlaceCategoryName,tbl_Place.Lng,tbl_Place.Lat,tbl_OperatorsSharing.Urgency,tbl_Company.CompanyName,
				tbl_OperatorsSharing.PowerUsed,tbl_OperatorsSharing.PoleNumber,tbl_OperatorsSharing.CabinetNumber,
				tbl_User.FullName,tbl_OperatorsSharing.CreateDate,tbl_OperatorsSharing.PlaceId
			from tbl_OperatorsSharing left join tbl_Place on tbl_OperatorsSharing.PlaceId = tbl_Place.Id
										left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
										left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
										left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
										left join tbl_Company on tbl_OperatorsSharing.CompanyId = tbl_Company.Id
										left join tbl_User on tbl_OperatorsSharing.CreateUserId = tbl_User.Id
			where tbl_OperatorsSharing.Solved = 2 and
					tbl_OperatorsSharing.PlaceId = @PlaceId
			order by tbl_OperatorsSharing.PlaceCode
	end
	else
	begin
		select @PlaceId = PlaceId from tbl_Remodeling where Id = @RemodelingId

		select tbl_OperatorsSharing.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
				tbl_PlaceCategory.PlaceCategoryName,tbl_Place.Lng,tbl_Place.Lat,tbl_OperatorsSharing.Urgency,tbl_Company.CompanyName,
				tbl_OperatorsSharing.PowerUsed,tbl_OperatorsSharing.PoleNumber,tbl_OperatorsSharing.CabinetNumber,
				tbl_User.FullName,tbl_OperatorsSharing.CreateDate,'00000000-0000-0000-0000-000000000000' as RemodelingId,tbl_OperatorsSharing.PlaceId
			from tbl_OperatorsSharing left join tbl_Place on tbl_OperatorsSharing.PlaceId = tbl_Place.Id
										left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
										left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
										left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
										left join tbl_Company on tbl_OperatorsSharing.CompanyId = tbl_Company.Id
										left join tbl_User on tbl_OperatorsSharing.CreateUserId = tbl_User.Id
			where tbl_OperatorsSharing.Solved = 2 and
					tbl_OperatorsSharing.PlaceId = @PlaceId
			union
		select tbl_OperatorsSharing.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
				tbl_PlaceCategory.PlaceCategoryName,tbl_Place.Lng,tbl_Place.Lat,tbl_OperatorsSharing.Urgency,tbl_Company.CompanyName,
				tbl_OperatorsSharing.PowerUsed,tbl_OperatorsSharing.PoleNumber,tbl_OperatorsSharing.CabinetNumber,
				tbl_User.FullName,tbl_OperatorsSharing.CreateDate,tbl_OperatorsSharing.RemodelingId,tbl_OperatorsSharing.PlaceId
			from tbl_OperatorsSharing left join tbl_Place on tbl_OperatorsSharing.PlaceId = tbl_Place.Id
										left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
										left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
										left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
										left join tbl_Company on tbl_OperatorsSharing.CompanyId = tbl_Company.Id
										left join tbl_User on tbl_OperatorsSharing.CreateUserId = tbl_User.Id
			where tbl_OperatorsSharing.Solved = 1 and
					tbl_OperatorsSharing.RemodelingId = @RemodelingId
		order by PlaceCode
	end
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetNearbyPlanningsAndPlaces]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetNearbyPlanningsAndPlaces]
					@Lng decimal(18,5),
					@Lat decimal(18,5),
					@PlanningId uniqueidentifier,
					@PlaceId uniqueidentifier,
					@BSPlanningPlaceCategorySql varchar(max),
					@BSPlaceCategorySql varchar(max),
					@Distance decimal(18,5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @CompanyName nvarchar(100)
	select @CompanyName = CompanyName from tbl_Company where Id = '9D4A4487-2AD6-4C19-8633-00742E8F1D28'

	declare @TempBSPlanningPlaceCategory table(value uniqueidentifier);
	insert @TempBSPlanningPlaceCategory exec(@BSPlanningPlaceCategorySql);

	declare @TempBSPlaceCategory table(value uniqueidentifier);
	insert @TempBSPlaceCategory exec(@BSPlaceCategorySql);

	with cte as
	(
		select tbl_Planning.Id,tbl_Planning.PlanningName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,
				tbl_Reseau.ReseauName,tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.Profession,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Planning.Lng,tbl_Planning.Lat) as Distance
			from tbl_Planning left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
								left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
								left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
								left join @TempBSPlanningPlaceCategory t on tbl_Planning.PlaceCategoryId = t.value
			where tbl_Planning.Profession = 1 and
					tbl_Planning.Id <> @PlanningId and
					tbl_Planning.AddressingState <> 2 and
					t.value is not null
	)
	select cte.Id,cte.PlanningName,cte.PlaceCategoryName,cte.AreaName,cte.ReseauName,cte.Lng,cte.Lat,cte.Profession,
			'9D4A4487-2AD6-4C19-8633-00742E8F1D28' as CompanyId,@CompanyName as CompanyName,1 as DataType
		from cte
		where cte.Distance <= @Distance;
		
	with cte as
	(
		select tbl_Place.Id,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,
				tbl_Reseau.ReseauName,tbl_Place.Lng,tbl_Place.Lat,tbl_Place.PropertyRight,tbl_Place.TelecomShare,
				tbl_Place.MobileShare,tbl_Place.UnicomShare,tbl_Place.OwnerName,tbl_Place.OwnerContact,
				tbl_Place.OwnerPhoneNumber,tbl_Place.Profession,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Place.Lng,tbl_Place.Lat) as Distance
			from tbl_Place left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join @TempBSPlaceCategory t on tbl_Place.PlaceCategoryId = t.value
			where tbl_Place.Profession = 1 and
					tbl_Place.[State] = 1 and
					tbl_Place.Id <> @PlaceId and
					t.value is not null
	)
	select cte.Id,cte.PlaceName,cte.PlaceCategoryName,cte.AreaName,cte.ReseauName,cte.Lng,cte.Lat,cte.PropertyRight,
			cte.TelecomShare,cte.MobileShare,cte.UnicomShare,cte.OwnerName,cte.OwnerContact,cte.OwnerPhoneNumber,cte.Profession,2 as DataType
		from cte
		where cte.Distance <= @Distance;
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetOperatorsPlanningsByPlanning]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetOperatorsPlanningsByPlanning]
					@PlanningId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_OperatorsPlanning.Id,tbl_OperatorsPlanning.PlanningCode,tbl_OperatorsPlanning.PlanningName,tbl_Area.AreaName,
			tbl_PlaceCategory.PlaceCategoryName,tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat,
			tbl_Company.CompanyName,tbl_User.FullName,tbl_OperatorsPlanning.CreateDate,1 as DataType,tbl_OperatorsPlanning.Profession,
			tbl_OperatorsPlanning.CompanyId,'' as ReseauName
		from tbl_OperatorsPlanning left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
									left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
									left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
									left join tbl_User on tbl_OperatorsPlanning.CreateUserId = tbl_User.Id
		where tbl_OperatorsPlanning.PlanningId = @PlanningId
		order by tbl_OperatorsPlanning.PlanningCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryDepartments]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryDepartments]
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_Department.Id,tbl_Department.DepartmentCode,tbl_Department.DepartmentName,ISNULL(tbl_User.FullName,'无') as FullName,
			tbl_Department.[State],tbl_Department.Remarks
		from tbl_Department left join tbl_User on tbl_Department.ManagerUserId = tbl_User.Id 
		where tbl_Department.CompanyId = @CompanyId
		order by tbl_Department.DepartmentCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetWFActivitysBySend]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetWFActivitysBySend]
					@WFProcessId uniqueidentifier,
					@UserId uniqueidentifier,
					@EntityId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @IsApprovedByManager int,
			@ManagerUserId uniqueidentifier,
			@FullName nvarchar(100),
			@DepartmentId uniqueidentifier,
			@DepartmentName nvarchar(100),
			@CompanyId uniqueidentifier,
			@CompanyName nvarchar(100),
			@WFProcessName varchar(20),
			@AreaCompanyId uniqueidentifier,
			@AreaDepartmentId uniqueidentifier,
			@AreaPostId uniqueidentifier,
			@AreaCompanyName varchar(20),
			@AreaDepartmentName varchar(100),
			@AreaFullName varchar(100),
			@AreaUserId uniqueidentifier,
			@CustomerCompanyId uniqueidentifier,
			@CustomerDepartmentId uniqueidentifier,
			@CustomerPostId uniqueidentifier,
			@CustomerCompanyName varchar(20),
			@CustomerDepartmentName varchar(100),
			@CustomerFullName varchar(100),
			@CustomerUserId uniqueidentifier

	
	select @IsApprovedByManager = IsApprovedByManager,@WFProcessName=WFProcessName from tbl_WFProcess where Id = @WFProcessId
	if @WFProcessName='零星派工'
	begin
		declare @temp table(indexid int identity(1,1),Id uniqueidentifier,WFActivityName varchar(50),WFActivityOperate int,WFActivityEditorId uniqueidentifier,
		WFActivityOrder int,SerialId int,RowId int,Timelimit int,CompanyId uniqueidentifier,DepartmentId uniqueidentifier,UserId uniqueidentifier,
		PostId uniqueidentifier,CompanyName varchar(20),DepartmentName varchar(100),FullName varchar(100),WFActivityEditorName varchar(20),PredefinedDepartmentId uniqueidentifier,
		PredefinedUserId uniqueidentifier,PredefinedIsManager int,IsMustEdit int,MustEditName varchar(20),IsNecessaryStep int)
		
		select @AreaCompanyId=tbl_Department.CompanyId,@AreaDepartmentId=tbl_User.DepartmentId,@AreaUserId=tbl_User.Id,@AreaCompanyName=tbl_Company.CompanyName,
			@AreaDepartmentName=tbl_Department.DepartmentName,@AreaFullName=tbl_User.FullName 
				from tbl_WorkOrder left join tbl_Reseau on tbl_WorkOrder.ReseauId=tbl_Reseau.Id
									left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
									left join tbl_User on tbl_Area.AreaManagerId=tbl_User.Id
									left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
									left join tbl_Company on tbl_Department.CompanyId=tbl_Company.Id where tbl_WorkOrder.Id=@EntityId

		select @CustomerCompanyId=tbl_Department.CompanyId,@CustomerDepartmentId=tbl_User.DepartmentId,@CustomerUserId=tbl_User.Id,@CustomerCompanyName=tbl_Company.CompanyName,
			@CustomerDepartmentName=tbl_Department.DepartmentName,@CustomerFullName=tbl_User.FullName 
				from tbl_WorkOrder left join tbl_User on tbl_WorkOrder.CustomerUserId=tbl_User.Id
									left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
									left join tbl_Company on tbl_Department.CompanyId=tbl_Company.Id where tbl_WorkOrder.Id=@EntityId

		if(ISNULL(@IsApprovedByManager,2) = 1)
		begin
			select @DepartmentId = DepartmentId from tbl_User where Id = @UserId
			select @DepartmentName = DepartmentName,@CompanyId = CompanyId,@ManagerUserId = ManagerUserId from tbl_Department where Id = @DepartmentId
			select @CompanyName = CompanyName from tbl_Company where Id = @CompanyId
			if(@ManagerUserId is null)
			begin
				set @ManagerUserId = '00000000-0000-0000-0000-000000000000'
				set @FullName = '请选择'
			end
			else
			begin
				select @FullName = FullName from tbl_User where Id = @ManagerUserId
			end

			insert @temp
			select tbl_WFActivity.Id,tbl_WFActivity.WFActivityName,tbl_WFActivity.WFActivityOperate,ISNULL(tbl_WFActivity.WFActivityEditorId,'00000000-0000-0000-0000-000000000000') as WFActivityEditorId,
					tbl_WFActivity.WFActivityOrder,tbl_WFActivity.SerialId + 1 as SerialId,tbl_WFActivity.RowId,tbl_WFActivity.Timelimit,tbl_WFActivity.CompanyId,
					ISNULL(tbl_WFActivity.DepartmentId,'00000000-0000-0000-0000-000000000000') as DepartmentId,ISNULL(tbl_WFActivity.UserId,'00000000-0000-0000-0000-000000000000') as UserId,
					ISNULL(tbl_WFActivity.PostId,'00000000-0000-0000-0000-000000000000') as PostId,tbl_Company.CompanyName,ISNULL(tbl_Department.DepartmentName,'请选择') as DepartmentName,ISNULL(tbl_User.FullName,'请选择') as FullName,
					ISNULL(tbl_WFActivityEditor.WFActivityEditorName,'无') as WFActivityEditorName,ISNULL(tbl_WFActivity.DepartmentId,'00000000-0000-0000-0000-000000000000') as PredefinedDepartmentId,
					ISNULL(tbl_WFActivity.UserId,'00000000-0000-0000-0000-000000000000') as PredefinedUserId,2 as PredefinedIsManager,tbl_WFActivity.IsMustEdit,case tbl_WFActivity.IsMustEdit when 1 then '是' when 2 then '否' else '' end as MustEditName,tbl_WFActivity.IsNecessaryStep
				from tbl_WFActivity left join tbl_Company on tbl_WFActivity.CompanyId = tbl_Company.Id
									left join tbl_Department on tbl_WFActivity.DepartmentId = tbl_Department.Id
									left join tbl_User on tbl_WFActivity.UserId = tbl_User.Id
									left join tbl_WFActivityEditor on tbl_WFActivity.WFActivityEditorId = tbl_WFActivityEditor.Id
				where tbl_WFActivity.WFProcessId = @WFProcessId
			union
			select NEWID(),'部门经理',1,'00000000-0000-0000-0000-000000000000',1,1,0,24,@CompanyId,@DepartmentId,@ManagerUserId,'00000000-0000-0000-0000-000000000000',@CompanyName,@DepartmentName,
				@FullName,'无',@DepartmentId,@ManagerUserId,1,2,'否',1
			order by SerialId,RowId
		end
		else
		begin
			insert @temp
			select tbl_WFActivity.Id,tbl_WFActivity.WFActivityName,tbl_WFActivity.WFActivityOperate,ISNULL(tbl_WFActivity.WFActivityEditorId,'00000000-0000-0000-0000-000000000000') as WFActivityEditorId,
					tbl_WFActivity.WFActivityOrder,tbl_WFActivity.SerialId,tbl_WFActivity.RowId,tbl_WFActivity.Timelimit,tbl_WFActivity.CompanyId,
					ISNULL(tbl_WFActivity.DepartmentId,'00000000-0000-0000-0000-000000000000') as DepartmentId,ISNULL(tbl_WFActivity.UserId,'00000000-0000-0000-0000-000000000000') as UserId,
					ISNULL(tbl_WFActivity.PostId,'00000000-0000-0000-0000-000000000000') as PostId,tbl_Company.CompanyName,ISNULL(tbl_Department.DepartmentName,'请选择') as DepartmentName,ISNULL(tbl_User.FullName,'请选择') as FullName,
					ISNULL(tbl_WFActivityEditor.WFActivityEditorName,'无') as WFActivityEditorName,ISNULL(tbl_WFActivity.DepartmentId,'00000000-0000-0000-0000-000000000000') as PredefinedDepartmentId,
					ISNULL(tbl_WFActivity.UserId,'00000000-0000-0000-0000-000000000000') as PredefinedUserId,2 as PredefinedIsManager,tbl_WFActivity.IsMustEdit,case tbl_WFActivity.IsMustEdit when 1 then '是' when 2 then '否' else '' end as MustEditName,tbl_WFActivity.IsNecessaryStep
				from tbl_WFActivity left join tbl_Company on tbl_WFActivity.CompanyId = tbl_Company.Id
									left join tbl_Department on tbl_WFActivity.DepartmentId = tbl_Department.Id
									left join tbl_User on tbl_WFActivity.UserId = tbl_User.Id
									left join tbl_WFActivityEditor on tbl_WFActivity.WFActivityEditorId = tbl_WFActivityEditor.Id
				where tbl_WFActivity.WFProcessId = @WFProcessId
				order by tbl_WFActivity.SerialId,tbl_WFActivity.RowId
		end

		declare @Id uniqueidentifier,
				@WFActivityName varchar(20)
		declare cur cursor for
		select distinct Id,WFActivityName from @temp
		open cur
		fetch next from cur into @Id,@WFActivityName
		while @@fetch_status = 0
		begin
			if @WFActivityName='区域经理'
			begin
				update @temp set CompanyId=@AreaCompanyId,DepartmentId=@AreaDepartmentId,@UserId=@AreaUserId,CompanyName=@AreaCompanyName,
					PredefinedDepartmentId=@AreaDepartmentId,PredefinedUserId=@AreaUserId,DepartmentName=@AreaDepartmentName,UserId=@AreaUserId,FullName=@AreaFullName 
					where Id=@Id
			end
			if @WFActivityName='派工联系人'
			begin
				update @temp set CompanyId=@CustomerCompanyId,DepartmentId=@CustomerDepartmentId,@UserId=@CustomerUserId,CompanyName=@CustomerCompanyName,
					PredefinedDepartmentId=@CustomerDepartmentId,PredefinedUserId=@CustomerUserId,DepartmentName=@CustomerDepartmentName,UserId=@CustomerUserId,FullName=@CustomerFullName 
					where Id=@Id
			end
			fetch next from cur into @Id,@WFActivityName
		end
		close cur
		deallocate cur
		select * from @temp
	end
	else
	begin
		if(ISNULL(@IsApprovedByManager,2) = 1)
		begin
			select @DepartmentId = DepartmentId from tbl_User where Id = @UserId
			select @DepartmentName = DepartmentName,@CompanyId = CompanyId,@ManagerUserId = ManagerUserId from tbl_Department where Id = @DepartmentId
			select @CompanyName = CompanyName from tbl_Company where Id = @CompanyId
			if(@ManagerUserId is null)
			begin
				set @ManagerUserId = '00000000-0000-0000-0000-000000000000'
				set @FullName = '请选择'
			end
			else
			begin
				select @FullName = FullName from tbl_User where Id = @ManagerUserId
			end

			select tbl_WFActivity.Id,tbl_WFActivity.WFActivityName,tbl_WFActivity.WFActivityOperate,ISNULL(tbl_WFActivity.WFActivityEditorId,'00000000-0000-0000-0000-000000000000') as WFActivityEditorId,
					tbl_WFActivity.WFActivityOrder,tbl_WFActivity.SerialId + 1 as SerialId,tbl_WFActivity.RowId,tbl_WFActivity.Timelimit,tbl_WFActivity.CompanyId,
					ISNULL(tbl_WFActivity.DepartmentId,'00000000-0000-0000-0000-000000000000') as DepartmentId,ISNULL(tbl_WFActivity.UserId,'00000000-0000-0000-0000-000000000000') as UserId,
					ISNULL(tbl_WFActivity.PostId,'00000000-0000-0000-0000-000000000000') as PostId,tbl_Company.CompanyName,ISNULL(tbl_Department.DepartmentName,'请选择') as DepartmentName,ISNULL(tbl_User.FullName,'请选择') as FullName,
					ISNULL(tbl_WFActivityEditor.WFActivityEditorName,'无') as WFActivityEditorName,ISNULL(tbl_WFActivity.DepartmentId,'00000000-0000-0000-0000-000000000000') as PredefinedDepartmentId,
					ISNULL(tbl_WFActivity.UserId,'00000000-0000-0000-0000-000000000000') as PredefinedUserId,2 as PredefinedIsManager,tbl_WFActivity.IsMustEdit,case tbl_WFActivity.IsMustEdit when 1 then '是' when 2 then '否' else '' end as MustEditName,tbl_WFActivity.IsNecessaryStep
				from tbl_WFActivity left join tbl_Company on tbl_WFActivity.CompanyId = tbl_Company.Id
									left join tbl_Department on tbl_WFActivity.DepartmentId = tbl_Department.Id
									left join tbl_User on tbl_WFActivity.UserId = tbl_User.Id
									left join tbl_WFActivityEditor on tbl_WFActivity.WFActivityEditorId = tbl_WFActivityEditor.Id
				where tbl_WFActivity.WFProcessId = @WFProcessId
			union
			select NEWID(),'部门经理',1,'00000000-0000-0000-0000-000000000000',1,1,0,24,@CompanyId,@DepartmentId,@ManagerUserId,'00000000-0000-0000-0000-000000000000',@CompanyName,@DepartmentName,
				@FullName,'无',@DepartmentId,@ManagerUserId,1,2,'否',1
			order by SerialId,RowId
		end
		else
		begin
			select tbl_WFActivity.Id,tbl_WFActivity.WFActivityName,tbl_WFActivity.WFActivityOperate,ISNULL(tbl_WFActivity.WFActivityEditorId,'00000000-0000-0000-0000-000000000000') as WFActivityEditorId,
					tbl_WFActivity.WFActivityOrder,tbl_WFActivity.SerialId,tbl_WFActivity.RowId,tbl_WFActivity.Timelimit,tbl_WFActivity.CompanyId,
					ISNULL(tbl_WFActivity.DepartmentId,'00000000-0000-0000-0000-000000000000') as DepartmentId,ISNULL(tbl_WFActivity.UserId,'00000000-0000-0000-0000-000000000000') as UserId,
					ISNULL(tbl_WFActivity.PostId,'00000000-0000-0000-0000-000000000000') as PostId,tbl_Company.CompanyName,ISNULL(tbl_Department.DepartmentName,'请选择') as DepartmentName,ISNULL(tbl_User.FullName,'请选择') as FullName,
					ISNULL(tbl_WFActivityEditor.WFActivityEditorName,'无') as WFActivityEditorName,ISNULL(tbl_WFActivity.DepartmentId,'00000000-0000-0000-0000-000000000000') as PredefinedDepartmentId,
					ISNULL(tbl_WFActivity.UserId,'00000000-0000-0000-0000-000000000000') as PredefinedUserId,2 as PredefinedIsManager,tbl_WFActivity.IsMustEdit,case tbl_WFActivity.IsMustEdit when 1 then '是' when 2 then '否' else '' end as MustEditName,tbl_WFActivity.IsNecessaryStep
				from tbl_WFActivity left join tbl_Company on tbl_WFActivity.CompanyId = tbl_Company.Id
									left join tbl_Department on tbl_WFActivity.DepartmentId = tbl_Department.Id
									left join tbl_User on tbl_WFActivity.UserId = tbl_User.Id
									left join tbl_WFActivityEditor on tbl_WFActivity.WFActivityEditorId = tbl_WFActivityEditor.Id
				where tbl_WFActivity.WFProcessId = @WFProcessId
				order by tbl_WFActivity.SerialId,tbl_WFActivity.RowId
		end
	end
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetOperatorsPlanningsByIds]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetOperatorsPlanningsByIds]
					@OperatorsPlanningIdsSql varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @TempOperatorsPlanning table(value uniqueidentifier);
	insert @TempOperatorsPlanning exec(@OperatorsPlanningIdsSql);

	select tbl_OperatorsPlanning.Id,tbl_OperatorsPlanning.PlanningName,tbl_Area.AreaName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat,tbl_Company.CompanyName,1 as DataType,tbl_OperatorsPlanning.Profession,
			tbl_OperatorsPlanning.CompanyId,'' as ReseauName
		from tbl_OperatorsPlanning left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
									left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
									left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
		where tbl_OperatorsPlanning.Id in (select value from @TempOperatorsPlanning)
		order by tbl_OperatorsPlanning.PlanningCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryWFProcesses]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWFProcesses]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_WFProcess.Id,tbl_WFProcess.WFProcessCode,tbl_WFProcess.WFProcessName,tbl_WFCategory.WFCategoryName,
			tbl_WFProcess.IsApprovedByManager,tbl_WFProcess.[State],tbl_WFProcess.Remarks
		from tbl_WFProcess left join tbl_WFCategory on tbl_WFProcess.WFCategoryId = tbl_WFCategory.Id
		order by tbl_WFCategory.WFCategoryCode,tbl_WFProcess.WFProcessCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetWFActivityInstances]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetWFActivityInstances]
					@WFProcessInstanceId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_WFActivityInstance.Id,tbl_WFActivityInstance.SerialId,tbl_WFActivityInstance.WFActivityInstanceName,tbl_User.FullName,
			tbl_WFActivityInstance.WFActivityOperate,tbl_WFActivityInstance.WFActivityInstanceResult,tbl_WFActivityInstance.Content,
			isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_WFActivityInstance left join tbl_User on tbl_WFActivityInstance.UserId = tbl_User.Id
									left join tbl_FileAssociation on tbl_WFActivityInstance.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'WFActivityInstance'
		where tbl_WFActivityInstance.WFProcessInstanceId = @WFProcessInstanceId
		order by tbl_WFActivityInstance.SerialId,tbl_WFActivityInstance.RowId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetWFActivityInstancesInfo]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetWFActivityInstancesInfo]
					@WFProcessInstanceCode nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @WFProcessInstanceId uniqueidentifier,
			@CreateUserId uniqueidentifier,
			@CreateDate datetime

	select @WFProcessInstanceId = Id,@CreateUserId = CreateUserId,@CreateDate = CreateDate from tbl_WFProcessInstance where WFProcessInstanceCode = @WFProcessInstanceCode

	select tbl_Department.DepartmentName + '/' + tbl_User.FullName as FullName,@CreateDate
		from tbl_User left join tbl_Department on tbl_User.DepartmentId = tbl_Department.Id
		where tbl_User.Id = @CreateUserId

	select tbl_WFActivityInstance.SerialId,tbl_WFActivityInstance.WFActivityInstanceName,
			tbl_Department.DepartmentName + '/' + tbl_User.FullName as FullName,
			tbl_WFActivityInstance.Content,tbl_WFActivityInstance.WFActivityInstanceResult,tbl_WFActivityInstance.OperateDate,
			tbl_WFActivityInstance.WFActivityOperate
		from tbl_WFActivityInstance left join tbl_User on tbl_WFActivityInstance.UserId = tbl_User.Id
									left join tbl_Department on tbl_User.DepartmentId = tbl_Department.Id
		where tbl_WFActivityInstance.WFProcessInstanceId = @WFProcessInstanceId
		order by tbl_WFActivityInstance.SerialId,tbl_WFActivityInstance.RowId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryWFProcessInstancesPage]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWFProcessInstancesPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@WFProcessInstanceCode nvarchar(50),
					@WFProcessInstanceName nvarchar(100),
					@WFProcessId uniqueidentifier,
					@WFProcessInstanceState int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_WFProcessInstance.Id,tbl_WFProcessInstance.WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceName,tbl_WFProcess.WFProcessName,
			tbl_WFProcessInstance.WFProcessInstanceState,tbl_User.FullName,tbl_WFProcessInstance.CreateDate,
			tbl_WFCategory.PrintUrl + '/' + cast(tbl_WFProcessInstance.EntityId as varchar(50)) as PrintUrl
		from tbl_WFProcessInstance left join tbl_WFProcess on tbl_WFProcessInstance.WFProcessId = tbl_WFProcess.Id
									left join tbl_WFCategory on tbl_WFProcess.WFCategoryId = tbl_WFCategory.Id
									left join tbl_User on tbl_WFProcessInstance.CreateUserId = tbl_User.Id
		where (tbl_WFProcessInstance.CreateDate >= @BeginDate and tbl_WFProcessInstance.CreateDate < @EndDate) and
				(@WFProcessInstanceCode = '' or CHARINDEX(@WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceCode) > 0) and
				(@WFProcessInstanceName = '' or CHARINDEX(@WFProcessInstanceName,tbl_WFProcessInstance.WFProcessInstanceName) > 0) and
				(@WFProcessId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.WFProcessId = @WFProcessId) and
				(@WFProcessInstanceState = 0 or tbl_WFProcessInstance.WFProcessInstanceState = @WFProcessInstanceState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.CreateUserId = @CreateUserId)
		order by tbl_WFProcessInstance.CreateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_WFProcessInstance
		where (tbl_WFProcessInstance.CreateDate >= @BeginDate and tbl_WFProcessInstance.CreateDate < @EndDate) and
				(@WFProcessInstanceCode = '' or CHARINDEX(@WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceCode) > 0) and
				(@WFProcessInstanceName = '' or CHARINDEX(@WFProcessInstanceName,tbl_WFProcessInstance.WFProcessInstanceName) > 0) and
				(@WFProcessId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.WFProcessId = @WFProcessId) and
				(@WFProcessInstanceState = 0 or tbl_WFProcessInstance.WFProcessInstanceState = @WFProcessInstanceState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.CreateUserId = @CreateUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryWFInstancesSendedToDoingPage]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWFInstancesSendedToDoingPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@WFProcessInstanceCode nvarchar(50),
					@WFProcessInstanceName nvarchar(100),
					@WFProcessId uniqueidentifier,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_WFProcessInstance.Id,tbl_WFProcessInstance.WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceName,
			tbl_WFProcess.WFProcessName,tbl_WFProcessInstance.CreateDate,tbl_WFProcessInstance.WFProcessInstanceState,
			tbl_WFCategory.PrintUrl + '/' + cast(tbl_WFProcessInstance.EntityId as varchar(50)) as PrintUrl
		from tbl_WFProcessInstance left join tbl_WFProcess on tbl_WFProcessInstance.WFProcessId = tbl_WFProcess.Id
									left join tbl_WFCategory on tbl_WFProcess.WFCategoryId = tbl_WFCategory.Id
		where (tbl_WFProcessInstance.CreateDate >= @BeginDate and tbl_WFProcessInstance.CreateDate < @EndDate) and
				(@WFProcessInstanceCode = '' or CHARINDEX(@WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceCode) > 0) and
				(@WFProcessInstanceName = '' or CHARINDEX(@WFProcessInstanceName,tbl_WFProcessInstance.WFProcessInstanceName) > 0) and
				(@WFProcessId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.WFProcessId = @WFProcessId) and
				tbl_WFProcessInstance.WFProcessInstanceState = 2 and
				tbl_WFProcessInstance.CreateUserId = @CreateUserId
		order by tbl_WFProcessInstance.CreateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_WFProcessInstance
		where (tbl_WFProcessInstance.CreateDate >= @BeginDate and tbl_WFProcessInstance.CreateDate < @EndDate) and
				(@WFProcessInstanceCode = '' or CHARINDEX(@WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceCode) > 0) and
				(@WFProcessInstanceName = '' or CHARINDEX(@WFProcessInstanceName,tbl_WFProcessInstance.WFProcessInstanceName) > 0) and
				(@WFProcessId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.WFProcessId = @WFProcessId) and
				tbl_WFProcessInstance.WFProcessInstanceState = 2 and
				tbl_WFProcessInstance.CreateUserId = @CreateUserId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryWFInstancesSendedToDoedPage]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWFInstancesSendedToDoedPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@WFProcessInstanceCode nvarchar(50),
					@WFProcessInstanceName nvarchar(100),
					@WFProcessId uniqueidentifier,
					@WFProcessInstanceState int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_WFProcessInstance.Id,tbl_WFProcessInstance.WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceName,
			tbl_WFProcess.WFProcessName,tbl_WFProcessInstance.CreateDate,tbl_WFProcessInstance.WFProcessInstanceState,
			tbl_WFCategory.PrintUrl + '/' + cast(tbl_WFProcessInstance.EntityId as varchar(50)) as PrintUrl
		from tbl_WFProcessInstance left join tbl_WFProcess on tbl_WFProcessInstance.WFProcessId = tbl_WFProcess.Id
									left join tbl_WFCategory on tbl_WFProcess.WFCategoryId = tbl_WFCategory.Id
		where (tbl_WFProcessInstance.CreateDate >= @BeginDate and tbl_WFProcessInstance.CreateDate < @EndDate) and
				(@WFProcessInstanceCode = '' or CHARINDEX(@WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceCode) > 0) and
				(@WFProcessInstanceName = '' or CHARINDEX(@WFProcessInstanceName,tbl_WFProcessInstance.WFProcessInstanceName) > 0) and
				(@WFProcessId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.WFProcessId = @WFProcessId) and
				(tbl_WFProcessInstance.WFProcessInstanceState = 3 or tbl_WFProcessInstance.WFProcessInstanceState = 4) and
				(@WFProcessInstanceState = 0 or tbl_WFProcessInstance.WFProcessInstanceState = @WFProcessInstanceState) and
				tbl_WFProcessInstance.CreateUserId = @CreateUserId
		order by tbl_WFProcessInstance.CreateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_WFProcessInstance
		where (tbl_WFProcessInstance.CreateDate >= @BeginDate and tbl_WFProcessInstance.CreateDate < @EndDate) and
				(@WFProcessInstanceCode = '' or CHARINDEX(@WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceCode) > 0) and
				(@WFProcessInstanceName = '' or CHARINDEX(@WFProcessInstanceName,tbl_WFProcessInstance.WFProcessInstanceName) > 0) and
				(@WFProcessId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.WFProcessId = @WFProcessId) and
				(tbl_WFProcessInstance.WFProcessInstanceState = 3 or tbl_WFProcessInstance.WFProcessInstanceState = 4) and
				(@WFProcessInstanceState = 0 or tbl_WFProcessInstance.WFProcessInstanceState = @WFProcessInstanceState) and
				tbl_WFProcessInstance.CreateUserId = @CreateUserId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryWFInstancesDoedPage]    Script Date: 2015/5/28 11:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWFInstancesDoedPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@WFProcessInstanceCode nvarchar(50),
					@WFProcessInstanceName nvarchar(100),
					@WFProcessId uniqueidentifier,
					@UserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_WFActivityInstance.Id,tbl_WFActivityInstance.WFProcessInstanceId,tbl_WFProcessInstance.WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceName,
			tbl_WFProcess.WFProcessName,tbl_WFActivityInstance.ReceivedDate,tbl_WFActivityInstance.OperateDate,tbl_User.FullName,tbl_WFProcessInstance.CreateDate,tbl_WFActivityInstance.WFActivityOperate,
			tbl_WFActivityInstance.WFActivityInstanceResult,tbl_WFActivityInstance.Content,
			tbl_WFCategory.PrintUrl + '/' + cast(tbl_WFProcessInstance.EntityId as varchar(50)) as PrintUrl
		from tbl_WFActivityInstance left join tbl_WFProcessInstance on tbl_WFActivityInstance.WFProcessInstanceId = tbl_WFProcessInstance.Id
									left join tbl_WFProcess on tbl_WFProcessInstance.WFProcessId = tbl_WFProcess.Id
									left join tbl_WFCategory on tbl_WFProcess.WFCategoryId = tbl_WFCategory.Id
									left join tbl_User on tbl_WFProcessInstance.CreateUserId = tbl_User.Id
		where (tbl_WFProcessInstance.CreateDate >= @BeginDate and tbl_WFProcessInstance.CreateDate < @EndDate) and
				(@WFProcessInstanceCode = '' or CHARINDEX(@WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceCode) > 0) and
				(@WFProcessInstanceName = '' or CHARINDEX(@WFProcessInstanceName,tbl_WFProcessInstance.WFProcessInstanceName) > 0) and
				(@WFProcessId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.WFProcessId = @WFProcessId) and
				tbl_WFActivityInstance.WFActivityInstanceState = 3 and
				tbl_WFActivityInstance.UserId = @UserId
		order by tbl_WFActivityInstance.OperateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_WFActivityInstance left join tbl_WFProcessInstance on tbl_WFActivityInstance.WFProcessInstanceId = tbl_WFProcessInstance.Id
		where (tbl_WFProcessInstance.CreateDate >= @BeginDate and tbl_WFProcessInstance.CreateDate < @EndDate) and
				(@WFProcessInstanceCode = '' or CHARINDEX(@WFProcessInstanceCode,tbl_WFProcessInstance.WFProcessInstanceCode) > 0) and
				(@WFProcessInstanceName = '' or CHARINDEX(@WFProcessInstanceName,tbl_WFProcessInstance.WFProcessInstanceName) > 0) and
				(@WFProcessId = '00000000-0000-0000-0000-000000000000' or tbl_WFProcessInstance.WFProcessId = @WFProcessId) and
				tbl_WFActivityInstance.WFActivityInstanceState = 3 and
				tbl_WFActivityInstance.UserId = @UserId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryConstructionPlanningsPage]    Script Date: 2015/7/8 星期三 13:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryConstructionPlanningsPage]
					@PageIndex int,
					@PageSize int,
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@ProjectId uniqueidentifier,
					@ConstructionProgress int,
					@ProjectManagerId uniqueidentifier,
					@ConstructionMethod int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_ConstructionTask.Id,tbl_Place.Id as PlaceId,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
		tbl_PlaceCategory.PlaceCategoryName,ISNULL(tbl_Place.Importance,0) as Importance,tbl_ConstructionTask.ConstructionProgress,tbl_ConstructionTask.ProgressMemos,
		tbl_ConstructionTask.ConstructionMethod,tbl_Project.ProjectName,tbl_User.FullName as ProjectManagerName,isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_ConstructionTask left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId = tbl_Project.Id
							left join tbl_User on tbl_ConstructionTask.ProjectManagerId = tbl_User.Id
							left join tbl_FileAssociation on tbl_Place.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Place'
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectId = @ProjectId) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 5) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 5)) and
				tbl_ConstructionTask.ProjectManagerId=@ProjectManagerId and
				tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod and
				tbl_Place.Profession = 1
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_ConstructionTask left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId = tbl_Project.Id
							left join tbl_User on tbl_ConstructionTask.ProjectManagerId = tbl_User.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 5) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 5)) and
				tbl_ConstructionTask.ProjectManagerId=@ProjectManagerId and
				tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod and
				tbl_Place.Profession = 1
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryRegisterPlanningsPage]    Script Date: 2015/10/8 星期四 10:17:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryRegisterPlanningsPage]
					@PageIndex int,
					@PageSize int,
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@ConstructionProgress int,
					@IsFinish int,
					@CompanyId uniqueidentifier,
					@ConstructionMethod int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_PlaceProperty.Id,tbl_PlaceProperty.Id as PlacePropertyId,tbl_ConstructionTask.Id as ConstructionTaskId,tbl_Place.GroupPlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
		tbl_PlaceCategory.PlaceCategoryName,ISNULL(tbl_Place.Importance,0) as Importance,tbl_ConstructionTask.ConstructionProgress,tbl_ConstructionTask.ProgressMemos,tbl_Project.ProjectName,
		tbl_PlaceProperty.MobilePoleNumber,tbl_PlaceProperty.MobileCabinetNumber,tbl_PlaceProperty.MobilePowerUsed,tbl_ConstructionTask.IsFinishMobile,ISNULL(Mobile.FullName,'') as MobileFullName,tbl_PlaceProperty.MobileCreateDate,
		tbl_PlaceProperty.TelecomPoleNumber,tbl_PlaceProperty.TelecomCabinetNumber,tbl_PlaceProperty.TelecomPowerUsed,tbl_ConstructionTask.IsFinishTelecom,ISNULL(Telecom.FullName,'') as TelecomFullName,tbl_PlaceProperty.TelecomCreateDate,
		tbl_PlaceProperty.UnicomPoleNumber,tbl_PlaceProperty.UnicomCabinetNumber,tbl_PlaceProperty.UnicomPowerUsed,tbl_ConstructionTask.IsFinishUnicom,ISNULL(Unicom.FullName,'') as UnicomFullName,tbl_PlaceProperty.UnicomCreateDate
		from tbl_ConstructionTask left join tbl_Planning on tbl_ConstructionTask.PlaceId=tbl_Planning.PlaceId
							left join tbl_Addressing on tbl_Planning.Id=tbl_Addressing.PlanningId
							left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_PlaceProperty on tbl_ConstructionTask.Id=tbl_PlaceProperty.ParentId and tbl_PlaceProperty.PropertyType=3
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User Mobile on tbl_PlaceProperty.MobileCreateUserId = Mobile.Id
							left join tbl_User Telecom on tbl_PlaceProperty.TelecomCreateUserId = Telecom.Id
							left join tbl_User Unicom on tbl_PlaceProperty.UnicomCreateUserId = Unicom.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 6) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 6)) and
				((@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and tbl_Addressing.MobileShare=1) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and tbl_Addressing.TelecomShare=1) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and tbl_Addressing.UnicomShare=1)) and
				((@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishMobile=@IsFinish)) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishTelecom=@IsFinish)) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishUnicom=@IsFinish))) and
				tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod and
				tbl_Place.Profession = 1
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_ConstructionTask left join tbl_Planning on tbl_ConstructionTask.PlaceId=tbl_Planning.PlaceId
							left join tbl_Addressing on tbl_Planning.Id=tbl_Addressing.PlanningId
							left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_PlaceProperty on tbl_ConstructionTask.Id=tbl_PlaceProperty.ParentId and tbl_PlaceProperty.PropertyType=3
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User Mobile on tbl_PlaceProperty.MobileCreateUserId = Mobile.Id
							left join tbl_User Telcom on tbl_PlaceProperty.TelecomCreateUserId = Telcom.Id
							left join tbl_User Unicom on tbl_PlaceProperty.UnicomCreateUserId = Unicom.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 6) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 6)) and
				((@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and tbl_Addressing.MobileShare=1) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and tbl_Addressing.TelecomShare=1) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and tbl_Addressing.UnicomShare=1)) and
				((@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishMobile=@IsFinish)) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishTelecom=@IsFinish)) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishUnicom=@IsFinish))) and
				tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod and
				tbl_Place.Profession = 1
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryRegisterRemodeingsPage]    Script Date: 2015/10/8 星期四 10:17:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryRegisterRemodeingsPage]
					@PageIndex int,
					@PageSize int,
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@ConstructionProgress int,
					@IsFinish int,
					@CompanyId uniqueidentifier,
					@ConstructionMethod int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_PlaceProperty.Id,tbl_PlaceProperty.Id as PlacePropertyId,tbl_ConstructionTask.Id as ConstructionTaskId,tbl_Place.GroupPlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
		tbl_PlaceCategory.PlaceCategoryName,ISNULL(tbl_Place.Importance,0) as Importance,tbl_ConstructionTask.ConstructionProgress,tbl_ConstructionTask.ProgressMemos,tbl_Project.ProjectName,
		tbl_PlaceProperty.MobilePoleNumber,tbl_PlaceProperty.MobileCabinetNumber,tbl_PlaceProperty.MobilePowerUsed,tbl_ConstructionTask.IsFinishMobile,ISNULL(Mobile.FullName,'') as MobileFullName,tbl_PlaceProperty.MobileCreateDate,
		tbl_PlaceProperty.TelecomPoleNumber,tbl_PlaceProperty.TelecomCabinetNumber,tbl_PlaceProperty.TelecomPowerUsed,tbl_ConstructionTask.IsFinishTelecom,ISNULL(Telecom.FullName,'') as TelecomFullName,tbl_PlaceProperty.TelecomCreateDate,
		tbl_PlaceProperty.UnicomPoleNumber,tbl_PlaceProperty.UnicomCabinetNumber,tbl_PlaceProperty.UnicomPowerUsed,tbl_ConstructionTask.IsFinishUnicom,ISNULL(Unicom.FullName,'') as UnicomFullName,tbl_PlaceProperty.UnicomCreateDate
		from tbl_ConstructionTask left join tbl_Remodeling on tbl_ConstructionTask.PlaceId=tbl_Remodeling.PlaceId and tbl_ConstructionTask.ProjectId=tbl_Remodeling.ProjectId
							left join tbl_OperatorsSharing on tbl_Remodeling.Id=tbl_OperatorsSharing.RemodelingId
							left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_PlaceProperty on tbl_ConstructionTask.Id=tbl_PlaceProperty.ParentId and tbl_PlaceProperty.PropertyType=3
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User Mobile on tbl_PlaceProperty.MobileCreateUserId = Mobile.Id
							left join tbl_User Telecom on tbl_PlaceProperty.TelecomCreateUserId = Telecom.Id
							left join tbl_User Unicom on tbl_PlaceProperty.UnicomCreateUserId = Unicom.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 6) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 6)) and
				tbl_OperatorsSharing.CompanyId=@CompanyId and
				((@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishMobile=@IsFinish)) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishTelecom=@IsFinish)) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishUnicom=@IsFinish))) and
				tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod and
				tbl_Place.Profession = 1
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_ConstructionTask left join tbl_Remodeling on tbl_ConstructionTask.PlaceId=tbl_Remodeling.PlaceId and tbl_ConstructionTask.ProjectId=tbl_Remodeling.ProjectId
							left join tbl_OperatorsSharing on tbl_Remodeling.Id=tbl_OperatorsSharing.RemodelingId
							left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_PlaceProperty on tbl_ConstructionTask.PlaceId=tbl_PlaceProperty.Id and tbl_PlaceProperty.PropertyType=3
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User Mobile on tbl_PlaceProperty.MobileCreateUserId = Mobile.Id
							left join tbl_User Telecom on tbl_PlaceProperty.TelecomCreateUserId = Telecom.Id
							left join tbl_User Unicom on tbl_PlaceProperty.UnicomCreateUserId = Unicom.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 6) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 6)) and
				tbl_OperatorsSharing.CompanyId=@CompanyId and
				((@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishMobile=@IsFinish)) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishTelecom=@IsFinish)) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and (@IsFinish=0 or tbl_ConstructionTask.IsFinishUnicom=@IsFinish))) and
				tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod and
				tbl_Place.Profession = 1
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetOperatorsPlanningByIds]    Script Date: 2015/6/29 星期一 9:57:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetOperatorsPlanningByIds]
					@OperatorsIdsSql varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @Temp table(value uniqueidentifier);
	insert @Temp exec(@OperatorsIdsSql);

	select tbl_OperatorsPlanning.Id,tbl_OperatorsPlanning.PlanningName,tbl_Area.AreaName,tbl_PlaceCategory.PlaceCategoryName,
		tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat,tbl_Company.CompanyName,1 as DataType,tbl_OperatorsPlanning.Profession,
		tbl_OperatorsPlanning.CompanyId,'' as ReseauName
	from tbl_OperatorsPlanning left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
								left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
								left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
	where tbl_OperatorsPlanning.Id in (select value from @Temp)
	order by tbl_OperatorsPlanning.PlanningCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_ExportPlacesExcel]    Script Date: 2015/7/13 星期一 15:28:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportPlacesExcel]
					@Profession int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_Place.PlaceCode as '基站编码',tbl_Place.PlaceName as '基站名称',tbl_Area.AreaName as '区域',tbl_Reseau.ReseauName as '网格',case tbl_Place.Profession when 1 then '基站' else '' end as '专业',
			tbl_PlaceCategory.PlaceCategoryName as '基站类型',tbl_Place.Lng as '经度',tbl_Place.Lat as '纬度',case tbl_Place.Importance when 1 then '一般' when 2 then '重要' else '' end as '重要性程度',
			case tbl_Place.PropertyRight when 1 then '铁塔' when 2 then '购移动' when 3 then '购电信' when 4 then '购联通' else '' end as '产权',
			case tbl_Place.TelecomShare when 1 then '是' when 2 then '否' else '' end as '电信共享',case tbl_Place.MobileShare when 1 then '是' when 2 then '否' else '' end as '移动共享',
			case tbl_Place.UnicomShare when 1 then '是' when 2 then '否' else '' end as '联通共享',tbl_Scene.SceneName as '周边场景',tbl_Place.OwnerName as '业主名称',
			tbl_Place.OwnerContact as '业主联系人',tbl_Place.OwnerPhoneNumber as '联系方式',case tbl_Place.[State] when 1 then '使用' when 2 then '停用' else '' end as '状态',
			tbl_User.FullName as '登记人',tbl_Place.CreateDate as '登记日期'
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
						left join tbl_Scene on tbl_Place.SceneId = tbl_Scene.Id
						left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
		where (@Profession = 0 or tbl_Place.Profession = @Profession)
		order by tbl_Place.PlaceCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_ExportOperatorsPlanningsExcel]    Script Date: 2015/7/13 星期一 16:14:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportOperatorsPlanningsExcel]
						@Profession int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_OperatorsPlanning.PlanningCode as '规划编码',tbl_OperatorsPlanning.PlanningName as '规划名称',tbl_Area.AreaName as '区域',
			tbl_PlaceCategory.PlaceCategoryName as '基站类型',tbl_OperatorsPlanning.Lng as '经度',tbl_OperatorsPlanning.Lat as '纬度',tbl_OperatorsPlanning.AntennaHeight as '天线挂高',
			tbl_OperatorsPlanning.PoleNumber as '抱杆数量',tbl_OperatorsPlanning.CabinetNumber as '机柜数量', case tbl_OperatorsPlanning.Urgency when 1 then '一级' when 2 then '二级' else '' end as '紧要程度',
			case tbl_OperatorsPlanning.Solved when 1 then '是' when 2 then '否' else '' end as '是否采纳',tbl_OperatorsPlanning.Remarks as '备注',tbl_Company.CompanyName as '规划公司',
			tbl_User.FullName as '规划人',tbl_OperatorsPlanning.CreateDate as '规划日期'
		from tbl_OperatorsPlanning left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
									left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
									left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
									left join tbl_User on tbl_OperatorsPlanning.CreateUserId = tbl_User.Id
		where tbl_OperatorsPlanning.Profession = @Profession
		order by tbl_OperatorsPlanning.PlanningCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetPlaceByIds]    Script Date: 2015/6/29 星期一 10:00:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetPlaceByIds]
					@PlaceIdsSql varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @TempPlace table(value uniqueidentifier);
	insert @TempPlace exec(@PlaceIdsSql);

	select tbl_Place.Id,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_PlaceCategory.PlaceCategoryName,tbl_Place.Lng,tbl_Place.Lat,'苏州铁塔' as CompanyName,
		2 as DataType,'9D4A4487-2AD6-4C19-8633-00742E8F1D28' as CompanyId,tbl_Reseau.ReseauName,tbl_Place.Lng,tbl_Place.Lat,tbl_Place.PropertyRight,tbl_Place.TelecomShare,
		tbl_Place.MobileShare,tbl_Place.UnicomShare,tbl_Place.OwnerName,tbl_Place.OwnerContact,tbl_Place.OwnerPhoneNumber,tbl_Place.Profession
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
									--left join tbl_Company on tbl_Place.CompanyId = tbl_Company.Id
		where tbl_Place.Id in (select value from @TempPlace)
		order by tbl_Place.PlaceCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryConstructionTaskPlanningsPage]    Script Date: 2015/7/16 星期四 15:43:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryConstructionTaskPlanningsPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlanningCode nvarchar(50),
					@PlanningName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Urgency int,
					@TelecomDemand int,
					@MobileDemand int,
					@UnicomDemand int,
					@DemandState int,
					@Issued int,
					@AddressingState int,
					@CreateUserId uniqueidentifier,
					@PlaceName nvarchar(100),
					@AddressingUserId uniqueidentifier,
					@ProjectManagerId uniqueidentifier,
					@ConstructionProgress int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Planning.Id,tbl_Planning.PlanningCode,tbl_Planning.PlanningName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.Urgency,tbl_Planning.MobileDemand,tbl_Planning.TelecomDemand,tbl_Planning.UnicomDemand,tbl_Planning.DemandState,
			tbl_Planning.Issued,tbl_Planning.AddressingState,ISNULL(u1.FullName,'') as AddressingUserFullName,tbl_Planning.Remarks,u2.FullName,tbl_Planning.CreateDate,
			tbl_Planning.PlaceId,ISNULL(tbl_Project.ProjectName,'') as ProjectName,ISNULL(u3.FullName,'') as ProjectManagerName,ISNULL(tbl_ConstructionTask.ConstructionProgress,0) as ConstructionProgress,
			ISNULL(tbl_ConstructionTask.ProgressMemos,'') as ProgressMemos,isnull(tbl_ConstructionTask.CreateDate,'1990-01-01') as ConCreateDate,ISNULL(tbl_ConstructionTask.Id,'00000000-0000-0000-0000-000000000000') as ConstructionTaskId
		from tbl_Planning left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User u1 on tbl_Planning.AddressingUserId = u1.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
							left join tbl_Place on tbl_Planning.PlaceId=tbl_Place.Id
							left join tbl_ConstructionTask on tbl_Planning.PlaceId=tbl_ConstructionTask.PlaceId and tbl_ConstructionTask.ConstructionMethod=1
							left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id and tbl_ConstructionTask.ConstructionMethod=1
							left join tbl_User u3 on tbl_ConstructionTask.ProjectManagerId = u3.Id
		where (tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Planning.Urgency = @Urgency) and
				(@TelecomDemand = 0 or tbl_Planning.TelecomDemand = @TelecomDemand) and
				(@MobileDemand = 0 or tbl_Planning.MobileDemand = @MobileDemand) and
				(@UnicomDemand = 0 or tbl_Planning.UnicomDemand = @UnicomDemand) and
				(@DemandState = 0 or tbl_Planning.DemandState = @DemandState) and
				(@Issued = 0 or tbl_Planning.Issued = @Issued) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.CreateUserId = @CreateUserId) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AddressingUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.AddressingUserId = @AddressingUserId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectManagerId = @ProjectManagerId) and
				(@ConstructionProgress = 0 or tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress)
		order by tbl_Planning.PlanningCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Planning left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
			left join tbl_ConstructionTask on tbl_Planning.PlaceId=tbl_ConstructionTask.PlaceId and tbl_ConstructionTask.ConstructionMethod=1
			left join tbl_Place on tbl_Planning.PlaceId=tbl_Place.Id
		where (tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Planning.Urgency = @Urgency) and
				(@TelecomDemand = 0 or tbl_Planning.TelecomDemand = @TelecomDemand) and
				(@MobileDemand = 0 or tbl_Planning.MobileDemand = @MobileDemand) and
				(@UnicomDemand = 0 or tbl_Planning.UnicomDemand = @UnicomDemand) and
				(@DemandState = 0 or tbl_Planning.DemandState = @DemandState) and
				(@Issued = 0 or tbl_Planning.Issued = @Issued) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.CreateUserId = @CreateUserId) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AddressingUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.AddressingUserId = @AddressingUserId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectManagerId = @ProjectManagerId) and
				(@ConstructionProgress = 0 or tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_ExportConstructionTaskPlanningsExcel]    Script Date: 2015/7/17 星期五 8:53:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportConstructionTaskPlanningsExcel]
					@Profession int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_Planning.PlanningCode as '规划编码',tbl_Planning.PlanningName as '规划名称',tbl_Area.AreaName as '区域',tbl_Reseau.ReseauName as '网格',tbl_PlaceCategory.PlaceCategoryName as '基站类型',
			tbl_Planning.Lng as '经度',tbl_Planning.Lat as '纬度',case tbl_Planning.Urgency when 1 then '一级' when 2 then '二级' end as '紧要程度',case tbl_Planning.MobileDemand when 1 then '未确认' when 2 then '需要' when 3 then '不需要' else '' end as '移动需求',
			case tbl_Planning.TelecomDemand when 1 then '未确认' when 2 then '需要' when 3 then '不需要' else '' end as '电信需求',case tbl_Planning.UnicomDemand when 1 then '未确认' when 2 then '需要' when 3 then '不需要' else '' end as '联通需求',
			case tbl_Planning.DemandState when 1 then '未请求' when 2 then '确认中' when 3 then '确认完成' else '' end as '确认状态',case tbl_Planning.Issued when 1 then '是' when 2 then '否' else '' end as '是否下达',
			case tbl_Planning.AddressingState when 1 then '未寻址确认' when 2 then '已寻址确认' when 3 then '流转中' when 4 then '流程终止' else '' end as '寻址状态',ISNULL(u1.FullName,'') as '租赁人',tbl_Planning.Remarks as '备注',
			u2.FullName as '规划人',tbl_Planning.CreateDate as '规划日期',ISNULL(tbl_Project.ProjectName,'') as '建设项目',ISNULL(u3.FullName,'') as '项目经理',case tbl_ConstructionTask.ConstructionProgress when 1 then '未开工' when 2 then '进行中' when 3 then '已完工' when 4 then '取消' else '' end as '建设进度',
			ISNULL(tbl_ConstructionTask.ProgressMemos,'') as '进度简述',isnull(tbl_ConstructionTask.CreateDate,'1990-01-01') as '登记日期'
		from tbl_Planning left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User u1 on tbl_Planning.AddressingUserId = u1.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
							left join tbl_Place on tbl_Planning.PlaceId=tbl_Place.Id
							left join tbl_ConstructionTask on tbl_Planning.PlaceId=tbl_ConstructionTask.PlaceId and tbl_ConstructionTask.ConstructionMethod=1
							left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id and tbl_ConstructionTask.ConstructionMethod=1
							left join tbl_User u3 on tbl_ConstructionTask.ProjectManagerId = u3.Id
		where tbl_Planning.Profession = @Profession
		order by tbl_Planning.PlanningCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_ExportConstructionTaskRemodeingsExcel]    Script Date: 2015/11/26 15:02:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportConstructionTaskRemodeingsExcel]
					@Profession int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_Place.PlaceCode as '基站编码',tbl_Place.PlaceName as '基站名称',tbl_Area.AreaName as '区域',tbl_Reseau.ReseauName as '网格',tbl_PlaceCategory.PlaceCategoryName as '基站类型',
			tbl_Place.Lng as '经度',tbl_Place.Lat as '纬度',case tbl_Place.PropertyRight when 1 then '铁塔' when 2 then '购移动' when 3 then '购电信' when 4 then '购联通' else '' end as '产权',
			case tbl_Place.MobileShare when 1 then '是' when 2 then '否' else '' end as '移动共享',case tbl_Place.TelecomShare when 1 then '是' when 2 then '否' else '' end as '电信共享',
			case tbl_Place.UnicomShare when 1 then '是' when 2 then '否' else '' end as '联通共享',case tbl_Remodeling.Urgency when 1 then '一级' when 2 then '二级' end as '紧要程度',
			case tbl_Remodeling.Issued when 1 then '是' when 2 then '否' else '' end as '是否下达',tbl_Remodeling.Remarks as '备注',u2.FullName as '创建人',tbl_Remodeling.CreateDate as '创建日期',
			ISNULL(tbl_Project.ProjectName,'') as '建设项目',ISNULL(u3.FullName,'') as '项目经理',case tbl_ConstructionTask.ConstructionProgress when 1 then '未开工' when 2 then '进行中' when 3 then '已完工' when 4 then '取消' else '' end as '建设进度',
			ISNULL(tbl_ConstructionTask.ProgressMemos,'') as '进度简述',isnull(tbl_ConstructionTask.CreateDate,'1990-01-01') as '登记日期'
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User u2 on tbl_Remodeling.CreateUserId = u2.Id
							left join tbl_ConstructionTask on tbl_Remodeling.PlaceId=tbl_ConstructionTask.PlaceId and tbl_ConstructionTask.ConstructionMethod=2
							left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id and tbl_ConstructionTask.ConstructionMethod=2
							left join tbl_User u3 on tbl_ConstructionTask.ProjectManagerId = u3.Id
		where tbl_Remodeling.Profession = @Profession
		order by tbl_Place.PlaceCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryConstructionTaskRemodeingsPage]    Script Date: 2015/7/17 星期五 10:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryConstructionTaskRemodeingsPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Urgency int,
					@Issued int,
					@CreateUserId uniqueidentifier,
					@ProjectId uniqueidentifier,
					@ProjectManagerId uniqueidentifier,
					@ConstructionProgress int,
					@TelecomShare int,
					@MobileShare int,
					@UnicomShare int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Remodeling.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Place.Lng,tbl_Place.Lat,tbl_Place.PropertyRight,tbl_Place.TelecomShare,tbl_Place.MobileShare,tbl_Place.UnicomShare,
			tbl_Remodeling.Urgency,tbl_Remodeling.Issued,tbl_Remodeling.Remarks,tbl_User.FullName,tbl_Remodeling.CreateDate,
			tbl_Remodeling.PlaceId,ISNULL(tbl_Project.ProjectName,'') as ProjectName,
			ISNULL(u3.FullName,'') as ProjectManagerName,ISNULL(tbl_ConstructionTask.ConstructionProgress,0) as ConstructionProgress,ISNULL(tbl_ConstructionTask.ProgressMemos,'') as ProgressMemos,
			ISNULL(tbl_ConstructionTask.Id,'00000000-0000-0000-0000-000000000000') as ConstructionTaskId
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User on tbl_Remodeling.CreateUserId = tbl_User.Id
							left join tbl_ConstructionTask on tbl_Remodeling.PlaceId=tbl_ConstructionTask.PlaceId and tbl_ConstructionTask.ConstructionMethod=2 and tbl_Remodeling.ProjectId=tbl_ConstructionTask.ProjectId
							left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id and tbl_ConstructionTask.ConstructionMethod=2
							left join tbl_User u3 on tbl_ConstructionTask.ProjectManagerId = u3.Id
		where (tbl_Remodeling.CreateDate >= @BeginDate and tbl_Remodeling.CreateDate < @EndDate) and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_Remodeling.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Remodeling.Urgency = @Urgency) and
				(@Issued = 0 or tbl_Remodeling.Issued = @Issued) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Remodeling.CreateUserId = @CreateUserId) and
				(@ProjectId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectId = @ProjectId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectManagerId = @ProjectManagerId) and
				(@ConstructionProgress = 0 or tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress) and
				(@TelecomShare = 0 or tbl_Place.TelecomShare = @TelecomShare) and
				(@MobileShare = 0 or tbl_Place.MobileShare = @MobileShare) and
				(@UnicomShare = 0 or tbl_Place.UnicomShare = @UnicomShare) and
				tbl_Remodeling.OrderState=3
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_ConstructionTask on tbl_Remodeling.PlaceId=tbl_ConstructionTask.PlaceId and tbl_ConstructionTask.ConstructionMethod=2 and tbl_Remodeling.ProjectId=tbl_ConstructionTask.ProjectId
		where (tbl_Remodeling.CreateDate >= @BeginDate and tbl_Remodeling.CreateDate < @EndDate) and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_Remodeling.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Remodeling.Urgency = @Urgency) and
				(@Issued = 0 or tbl_Remodeling.Issued = @Issued) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Remodeling.CreateUserId = @CreateUserId) and
				(@ProjectId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectId = @ProjectId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectManagerId = @ProjectManagerId) and
				(@ConstructionProgress = 0 or tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress) and
				(@TelecomShare = 0 or tbl_Place.TelecomShare = @TelecomShare) and
				(@MobileShare = 0 or tbl_Place.MobileShare = @MobileShare) and
				(@UnicomShare = 0 or tbl_Place.UnicomShare = @UnicomShare) and
				tbl_Remodeling.OrderState=3
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetPlanningByIds]    Script Date: 2015/7/17 星期五 10:59:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetPlanningByIds]
					@PlanningIdsSql varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @Temp table(value uniqueidentifier);
	insert @Temp exec(@PlanningIdsSql);

	select tbl_Planning.Id,tbl_Planning.PlanningName,tbl_Area.AreaName,tbl_PlaceCategory.PlaceCategoryName,
		tbl_Planning.Lng,tbl_Planning.Lat,'苏州铁塔' as CompanyName,1 as DataType,tbl_Planning.Profession,
		'9D4A4487-2AD6-4C19-8633-00742E8F1D28' as CompanyId,tbl_Reseau.ReseauName
	from tbl_Planning left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
		left join tbl_Reseau on tbl_Planning.ReseauId=tbl_Reseau.Id
		left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
	where tbl_Planning.Id in (select value from @Temp)
	order by tbl_Planning.PlanningCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetAllUsedMaterials]    Script Date: 2015/8/18 星期二 15:44:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetAllUsedMaterials]
						@State int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_MaterialCategory.Id,tbl_MaterialCategory.MaterialCategoryName as Name,'00000000-0000-0000-0000-000000000000' as PId,'false' as IsLeafStr
		from tbl_MaterialCategory
		where tbl_MaterialCategory.State=@State
	union
	select tbl_Material.Id,tbl_Material.MaterialName,tbl_Material.MaterialCategoryId,'true'
		from tbl_Material 
		where tbl_Material.State=@State
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryCustomersPage]    Script Date: 2015/8/19 星期三 14:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryCustomersPage]
					@PageIndex int,
					@PageSize int,
					@CustomerType int,
					@CustomerCode nvarchar(50),
					@CustomerName nvarchar(100),
					@CustomerFullName nvarchar(100),
					@State int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Customer.Id,tbl_Customer.CustomerType,tbl_Customer.CustomerCode,tbl_Customer.CustomerName,tbl_Customer.CustomerFullName,isnull(tbl_User.FullName,'') as FullName,
		tbl_Customer.ContactMan,tbl_Customer.ContactTel,tbl_Customer.ContactAddr,tbl_Customer.[State],tbl_Customer.Remarks
		from tbl_Customer left join tbl_User on tbl_Customer.CustomerUserId=tbl_User.Id
		where (@CustomerType = 0 or tbl_Customer.CustomerType = @CustomerType) and
				(@CustomerCode = '' or CHARINDEX(@CustomerCode,tbl_Customer.CustomerCode) > 0) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_Customer.CustomerName) > 0) and
				(@CustomerFullName = '' or CHARINDEX(@CustomerFullName,tbl_Customer.CustomerFullName) > 0) and
				(@State = 0 or tbl_Customer.[State] = @State)
		order by tbl_Customer.CustomerCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Customer
		where (@CustomerType = 0 or tbl_Customer.CustomerType = @CustomerType) and
				(@CustomerCode = '' or CHARINDEX(@CustomerCode,tbl_Customer.CustomerCode) > 0) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_Customer.CustomerName) > 0) and
				(@CustomerFullName = '' or CHARINDEX(@CustomerFullName,tbl_Customer.CustomerFullName) > 0) and
				(@State = 0 or tbl_Customer.[State] = @State)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetMaterialSpecs]    Script Date: 2015/8/21 星期五 9:15:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetMaterialSpecs]
					@MaterialId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_MaterialSpec.Id,tbl_MaterialSpec.MaterialSpecCode,tbl_MaterialSpec.MaterialSpecName,tbl_Unit.UnitName,tbl_MaterialSpec.Price,tbl_Customer.CustomerName,
			tbl_MaterialSpec.[State],tbl_MaterialSpec.Remarks
		from tbl_MaterialSpec left join tbl_Unit on tbl_MaterialSpec.UnitId=tbl_Unit.Id
						left join tbl_Customer on tbl_MaterialSpec.CustomerId=tbl_Customer.Id
		where tbl_MaterialSpec.MaterialId=@MaterialId
		order by tbl_MaterialSpec.MaterialSpecCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryCustomersPageBySelect]    Script Date: 2015/8/21 星期五 9:16:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryCustomersPageBySelect]
					@PageIndex int,
					@PageSize int,
					@CustomerCode nvarchar(50),
					@CustomerName nvarchar(100),
					@CustomerFullName nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Customer.Id,tbl_Customer.CustomerCode,tbl_Customer.CustomerName,tbl_Customer.CustomerFullName,tbl_Customer.ContactMan,tbl_Customer.ContactTel,
			tbl_Customer.ContactAddr,tbl_Customer.[State],tbl_Customer.Remarks
		from tbl_Customer
		where (@CustomerCode = '' or CHARINDEX(@CustomerCode,tbl_Customer.CustomerCode) > 0) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_Customer.CustomerName) > 0) and
				(@CustomerFullName = '' or CHARINDEX(@CustomerFullName,tbl_Customer.CustomerFullName) > 0)
		order by tbl_Customer.CustomerCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Customer
		where (@CustomerCode = '' or CHARINDEX(@CustomerCode,tbl_Customer.CustomerCode) > 0) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_Customer.CustomerName) > 0) and
				(@CustomerFullName = '' or CHARINDEX(@CustomerFullName,tbl_Customer.CustomerFullName) > 0)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetOperatorsPlanningsByOperatorsPlanning]    Script Date: 2015/8/24 星期一 9:23:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetOperatorsPlanningsByOperatorsPlanningDemandId]
					@OperatorsPlanningDemandId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_OperatorsPlanning.Id,tbl_OperatorsPlanning.PlanningCode,tbl_OperatorsPlanning.PlanningName,tbl_Area.AreaName,
			tbl_PlaceCategory.PlaceCategoryName,tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat,
			tbl_Company.CompanyName,tbl_User.FullName,tbl_OperatorsPlanning.CreateDate,1 as DataType,tbl_OperatorsPlanning.Profession,
			tbl_OperatorsPlanning.CompanyId,'' as ReseauName
		from tbl_OperatorsPlanningDemand left join tbl_OperatorsPlanning on tbl_OperatorsPlanningDemand.OperatorsPlanningId=tbl_OperatorsPlanning.Id
									left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
									left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
									left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
									left join tbl_User on tbl_OperatorsPlanning.CreateUserId = tbl_User.Id
		where tbl_OperatorsPlanningDemand.Id = @OperatorsPlanningDemandId
		order by tbl_OperatorsPlanning.PlanningCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetOperatorsPlanningsByOperatorsPlanningDemand]    Script Date: 2015/8/24 星期一 9:24:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetOperatorsPlanningsByOperatorsPlanningDemand]
					@OperatorsPlanningDemandId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_OperatorsPlanning.Id,tbl_OperatorsPlanning.PlanningCode,tbl_OperatorsPlanning.PlanningName,tbl_Area.AreaName,
			tbl_PlaceCategory.PlaceCategoryName,tbl_OperatorsPlanning.Lng,tbl_OperatorsPlanning.Lat,
			tbl_Company.CompanyName,tbl_User.FullName,tbl_OperatorsPlanning.CreateDate,1 as DataType,tbl_OperatorsPlanning.Profession,
			tbl_OperatorsPlanning.CompanyId,'' as ReseauName
		from tbl_OperatorsPlanningDemand left join tbl_OperatorsPlanning on tbl_OperatorsPlanningDemand.OperatorsPlanningId=tbl_OperatorsPlanning.Id
									left join tbl_Area on tbl_OperatorsPlanning.AreaId = tbl_Area.Id
									left join tbl_PlaceCategory on tbl_OperatorsPlanning.PlaceCategoryId = tbl_PlaceCategory.Id
									left join tbl_Company on tbl_OperatorsPlanning.CompanyId = tbl_Company.Id
									left join tbl_User on tbl_OperatorsPlanning.CreateUserId = tbl_User.Id
		where tbl_OperatorsPlanningDemand.Id = @OperatorsPlanningDemandId
		order by tbl_OperatorsPlanning.PlanningCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryOperatorsPlanningDemandsPage]    Script Date: 2015/8/24 星期一 9:24:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryOperatorsPlanningDemandsPage]
					@PageIndex int,
					@PageSize int,
					@PlanningCode nvarchar(50),
					@PlanningName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Demand int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_OperatorsPlanningDemand.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,tbl_Place.Lng,tbl_Place.Lat,
	tbl_OperatorsPlanningDemand.Demand,tbl_OperatorsPlanningDemand.CreateDate as ConfirmDate,tbl_User.FullName,tbl_OperatorsPlanningDemand.OperatorsPlanningId,
	tbl_OperatorsPlanningDemand.PlaceId
		from tbl_OperatorsPlanningDemand left join tbl_OperatorsPlanning on tbl_OperatorsPlanningDemand.OperatorsPlanningId = tbl_OperatorsPlanning.Id
										left join tbl_Place on tbl_OperatorsPlanningDemand.PlaceId=tbl_Place.Id
										left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
										left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
										left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
										left join tbl_User on tbl_OperatorsPlanningDemand.CreateUserId = tbl_User.Id
		where (@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_OperatorsPlanning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_OperatorsPlanning.PlanningName) > 0) and
				tbl_OperatorsPlanning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Demand = 0 or tbl_OperatorsPlanningDemand.Demand = @Demand) and
				tbl_OperatorsPlanning.CompanyId=@CompanyId
		order by tbl_OperatorsPlanning.PlanningCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_OperatorsPlanningDemand left join tbl_OperatorsPlanning on tbl_OperatorsPlanningDemand.OperatorsPlanningId = tbl_OperatorsPlanning.Id
										left join tbl_Place on tbl_OperatorsPlanningDemand.PlaceId=tbl_Place.Id
										left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
		where (@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_OperatorsPlanning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_OperatorsPlanning.PlanningName) > 0) and
				tbl_OperatorsPlanning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Demand = 0 or tbl_OperatorsPlanningDemand.Demand = @Demand) and
				tbl_OperatorsPlanning.CompanyId=@CompanyId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetMaterialList]    Script Date: 2015/9/6 星期日 12:25:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetMaterialList]
					@ParentId uniqueidentifier,
					@PropertyType int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_MaterialList.Id as MaterialListId,tbl_MaterialCategory.MaterialCategoryName,tbl_Material.MaterialName,tbl_MaterialList.SpecNumber,tbl_MaterialList.BudgetPrice,tbl_MaterialList.Memos,
	isnull(tbl_MaterialSpec.MaterialSpecName,'') as MaterialSpecName,isnull(tbl_Customer.CustomerName,'') as CustomerName,tbl_MaterialSpec.Price,tbl_MaterialList.SpecNumber*tbl_MaterialSpec.Price as TotalPrice
		from tbl_MaterialList left join tbl_Material on tbl_MaterialList.MaterialId=tbl_Material.Id
							left join tbl_MaterialCategory on tbl_Material.MaterialCategoryId=tbl_MaterialCategory.Id
							left join tbl_MaterialSpec on tbl_MaterialList.MaterialSpecId=tbl_MaterialSpec.Id
							left join tbl_Customer on tbl_MaterialSpec.CustomerId=tbl_Customer.Id
		where tbl_MaterialList.ParentId=@ParentId and
				tbl_MaterialList.[PropertyType] = @PropertyType
		order by tbl_MaterialCategory.MaterialCategoryName,tbl_Material.MaterialName
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryMaterialPurchasePage]    Script Date: 2015/9/11 星期五 20:52:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryMaterialPurchasePage]
					@PageIndex int,
					@PageSize int,
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@MaterialName nvarchar(100),
					@DoState int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_MaterialList.Id,tbl_Project.ProjectName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,tbl_Place.PlaceName,
		tbl_Material.MaterialName,tbl_MaterialSpec.MaterialSpecName,tbl_Unit.UnitName,tbl_MaterialList.SpecNumber,supplier.CustomerName as SupplierCustomerName,
		tbl_MaterialList.[DoState],tbl_MaterialList.CreateDate,design.CustomerName as DesignCustomerName,tbl_User.FullName,tbl_MaterialSpec.Price,tbl_MaterialList.SpecNumber*tbl_MaterialSpec.Price as TotalPrice
		from tbl_MaterialList left join tbl_Addressing on tbl_MaterialList.ParentId=tbl_Addressing.Id and tbl_MaterialList.PropertyType=1
							left join tbl_Remodeling on tbl_MaterialList.ParentId=tbl_Remodeling.Id and tbl_MaterialList.PropertyType=2
							left join tbl_PlaceDesign on tbl_MaterialList.ParentId=tbl_PlaceDesign.ParentId
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on (tbl_Planning.PlaceId = tbl_Place.Id and tbl_MaterialList.PropertyType=1) or (tbl_Remodeling.PlaceId=tbl_Place.Id and tbl_MaterialList.PropertyType=2)
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_MaterialSpec on tbl_MaterialList.MaterialSpecId=tbl_MaterialSpec.Id
							left join tbl_Material on tbl_MaterialSpec.MaterialId=tbl_Material.Id
							left join tbl_User on tbl_MaterialList.CreateUserId = tbl_User.Id
							left join tbl_Customer design on tbl_PlaceDesign.DesignCustomerId=design.Id
							left join tbl_Customer supplier on tbl_MaterialList.SupplierId=supplier.Id
							left join tbl_Project on (tbl_Addressing.ProjectId=tbl_Project.Id and tbl_MaterialList.PropertyType=1) or (tbl_Remodeling.ProjectId=tbl_Project.Id and tbl_MaterialList.PropertyType=2)
							left join tbl_Unit on tbl_MaterialSpec.UnitId=tbl_Unit.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@MaterialName = '' or CHARINDEX(@MaterialName,tbl_Material.MaterialName) > 0) and
				(@DoState = 0 or tbl_MaterialList.[DoState]=@DoState) and
				tbl_Place.Profession = 1
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_MaterialList left join tbl_Addressing on tbl_MaterialList.ParentId=tbl_Addressing.Id and tbl_MaterialList.PropertyType=1
							left join tbl_Remodeling on tbl_MaterialList.ParentId=tbl_Remodeling.Id and tbl_MaterialList.PropertyType=2
							left join tbl_PlaceDesign on tbl_MaterialList.ParentId=tbl_PlaceDesign.ParentId
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on (tbl_Planning.PlaceId = tbl_Place.Id and tbl_MaterialList.PropertyType=1) or (tbl_Remodeling.PlaceId=tbl_Place.Id and tbl_MaterialList.PropertyType=2)
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_MaterialSpec on tbl_MaterialList.MaterialSpecId=tbl_MaterialSpec.Id
							left join tbl_Material on tbl_MaterialSpec.MaterialId=tbl_Material.Id
							left join tbl_User on tbl_MaterialList.CreateUserId = tbl_User.Id
							left join tbl_Customer design on tbl_PlaceDesign.DesignCustomerId=design.Id
							left join tbl_Customer supplier on tbl_MaterialList.SupplierId=supplier.Id
							left join tbl_Project on (tbl_Addressing.ProjectId=tbl_Project.Id and tbl_MaterialList.PropertyType=1) or (tbl_Remodeling.ProjectId=tbl_Project.Id and tbl_MaterialList.PropertyType=2)
							left join tbl_Unit on tbl_MaterialSpec.UnitId=tbl_Unit.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@MaterialName = '' or CHARINDEX(@MaterialName,tbl_Material.MaterialName) > 0) and
				(@DoState = 0 or tbl_MaterialList.[DoState]=@DoState) and
				tbl_Place.Profession = 1
END
GO

/****** Object:  StoredProcedure [dbo].[prc_ExportMaterialPurchaseExcel]    Script Date: 2015/9/12 星期六 7:47:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportMaterialPurchaseExcel]
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@MaterialName nvarchar(100),
					@DoState int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_Project.ProjectName as '项目名称',tbl_Area.AreaName as '区域',tbl_Reseau.ReseauName as '网格',tbl_PlaceCategory.PlaceCategoryName as '基站类型',tbl_Place.PlaceName as '基站名称',
		tbl_Material.MaterialName as '物资名称',tbl_MaterialSpec.MaterialSpecName as '物资规格',tbl_Unit.UnitName as '计量单位',tbl_MaterialList.SpecNumber as '数量',supplier.CustomerName as '供应商',
		case tbl_MaterialList.[DoState] when 1 then '未采购' when 2 then '已采购' else '' end as '采购状态',tbl_MaterialList.CreateDate as '编制日期',design.CustomerName as '设计单位',tbl_User.FullName as '设计人'
		from tbl_MaterialList left join tbl_Addressing on tbl_MaterialList.ParentId=tbl_Addressing.Id and tbl_MaterialList.PropertyType=1
							left join tbl_PlaceDesign on tbl_MaterialList.ParentId=tbl_PlaceDesign.ParentId
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_Planning.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_MaterialSpec on tbl_MaterialList.MaterialSpecId=tbl_MaterialSpec.Id
							left join tbl_Material on tbl_MaterialSpec.MaterialId=tbl_Material.Id
							left join tbl_User on tbl_MaterialList.CreateUserId = tbl_User.Id
							left join tbl_Customer design on tbl_PlaceDesign.DesignCustomerId=design.Id
							left join tbl_Customer supplier on tbl_MaterialList.SupplierId=supplier.Id
							left join tbl_Project on tbl_Addressing.ProjectId=tbl_Project.Id
							left join tbl_Unit on tbl_MaterialSpec.UnitId=tbl_Unit.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@MaterialName = '' or CHARINDEX(@MaterialName,tbl_Material.MaterialName) > 0) and
				(@DoState = 0 or tbl_MaterialList.[DoState]=@DoState) and
				tbl_Addressing.OrderState=3 and tbl_Place.Profession = 1
		order by tbl_Place.PlaceCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryConstructionTasksPage]    Script Date: 2015/10/8 星期四 10:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryConstructionTasksPage]
					@PageIndex int,
					@PageSize int,
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@CustomerId uniqueidentifier,
					@ConstructionProgress int,
					@UserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_ConstructionTask.Id,tbl_Place.Id as PlaceId,tbl_Place.GroupPlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
		tbl_PlaceCategory.PlaceCategoryName,ISNULL(tbl_Place.Importance,0) as Importance,tbl_ConstructionTask.ConstructionProgress,tbl_ConstructionTask.ProgressMemos,
		tbl_ConstructionTask.ConstructionMethod,tbl_Project.ProjectName,tbl_User.FullName,tbl_Customer.CustomerName
		from tbl_ConstructionTask left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId = tbl_Project.Id
							left join tbl_User on tbl_ConstructionTask.ProjectManagerId = tbl_User.Id
							left join tbl_Customer on tbl_ConstructionTask.SupervisorCustomerId=tbl_Customer.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@CustomerId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.SupervisorCustomerId = @CustomerId) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 6) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 6)) and
				(tbl_ConstructionTask.ProjectManagerId=@UserId or tbl_ConstructionTask.SupervisorUserId=@UserId or @UserId='00000000-0000-0000-0000-000000000000') and
				tbl_Place.Profession = 1
		order by tbl_ConstructionTask.CreateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_ConstructionTask left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId = tbl_Project.Id
							left join tbl_User on tbl_ConstructionTask.ProjectManagerId = tbl_User.Id
							left join tbl_Customer on tbl_ConstructionTask.SupervisorCustomerId=tbl_Customer.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@CustomerId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.SupervisorCustomerId = @CustomerId) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 6) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 6)) and
				(tbl_ConstructionTask.ProjectManagerId=@UserId or tbl_ConstructionTask.SupervisorUserId=@UserId or @UserId='00000000-0000-0000-0000-000000000000') and
				tbl_Place.Profession = 1
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryTaskPropertysPage]    Script Date: 2015/10/8 星期四 10:10:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryTaskPropertysPage]
					@PageIndex int,
					@PageSize int,
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@CustomerId uniqueidentifier,
					@ConstructionProgress int,
					@TaskModel int,
					@SupervisorCustomerId uniqueidentifier,
					@UserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_TaskProperty.Id,tbl_Place.Id as PlaceId,tbl_TaskProperty.TaskModel as TaskModelId,tbl_ConstructionTask.Id as ConstructionTaskId,tbl_Place.GroupPlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
		tbl_PlaceCategory.PlaceCategoryName,tbl_TaskProperty.ConstructionProgress,tbl_TaskProperty.ProgressMemos,tbl_ConstructionTask.ConstructionMethod,tbl_Project.ProjectName,
		tbl_User.FullName,construction.CustomerName,supervisor.CustomerName as SupervisorCustomerName,tbl_TaskProperty.SubmitState,tbl_TaskProperty.TaskModel,DATEDIFF(DAY,GETDATE(),DATEADD(DAY,(tbl_TaskProperty.TimeLimit),tbl_TaskProperty.CreateDate)) as RestDays
		from tbl_TaskProperty left join tbl_ConstructionTask on tbl_TaskProperty.ConstructionTaskId=tbl_ConstructionTask.Id 
							left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId = tbl_Project.Id
							left join tbl_User on tbl_ConstructionTask.ProjectManagerId = tbl_User.Id
							left join tbl_Customer construction on tbl_TaskProperty.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervisor on tbl_TaskProperty.SupervisorCustomerId=supervisor.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@CustomerId = '00000000-0000-0000-0000-000000000000' or tbl_TaskProperty.ConstructionCustomerId = @CustomerId) and
				(@SupervisorCustomerId = '00000000-0000-0000-0000-000000000000' or tbl_TaskProperty.SupervisorCustomerId = @SupervisorCustomerId) and
				(@TaskModel=0 or tbl_TaskProperty.TaskModel=@TaskModel) and
				(@ConstructionProgress = 0 or (tbl_TaskProperty.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 6) or ((tbl_TaskProperty.ConstructionProgress = 1 or tbl_TaskProperty.ConstructionProgress = 2) and @ConstructionProgress = 6)) and
				(tbl_ConstructionTask.ProjectManagerId=@UserId or supervisor.CustomerUserId=@UserId or construction.CustomerUserId=@UserId  or @UserId='00000000-0000-0000-0000-000000000000') and
				tbl_Place.Profession = 1
		order by tbl_TaskProperty.CreateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_TaskProperty left join tbl_ConstructionTask on tbl_TaskProperty.ConstructionTaskId=tbl_ConstructionTask.Id 
							left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId = tbl_Project.Id
							left join tbl_User on tbl_ConstructionTask.ProjectManagerId = tbl_User.Id
							left join tbl_Customer construction on tbl_TaskProperty.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervisor on tbl_TaskProperty.SupervisorCustomerId=supervisor.Id
		where  (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@CustomerId = '00000000-0000-0000-0000-000000000000' or tbl_TaskProperty.ConstructionCustomerId = @CustomerId) and
				(@SupervisorCustomerId = '00000000-0000-0000-0000-000000000000' or tbl_TaskProperty.SupervisorCustomerId = @SupervisorCustomerId) and
				(@TaskModel=0 or tbl_TaskProperty.TaskModel=@TaskModel) and
				(@ConstructionProgress = 0 or (tbl_TaskProperty.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 6) or ((tbl_TaskProperty.ConstructionProgress = 1 or tbl_TaskProperty.ConstructionProgress = 2) and @ConstructionProgress = 6)) and
				(tbl_ConstructionTask.ProjectManagerId=@UserId or supervisor.CustomerUserId=@UserId or construction.CustomerUserId=@UserId  or @UserId='00000000-0000-0000-0000-000000000000') and
				tbl_Place.Profession = 1
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryResourcePlacesPage]    Script Date: 2015/10/16 星期五 15:24:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryResourcePlacesPage]
					@PageIndex int,
					@PageSize int,
					@GroupPlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@PropertyRight int,
					@Importance int,
					@TelecomShare int,
					@MobileShare int,
					@UnicomShare int,
					@State int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Place.Id,tbl_Place.GroupPlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.Profession,
			tbl_PlaceCategory.PlaceCategoryName,isnull(tbl_Tower.TowerType,0) as TowerType,isnull(tbl_Tower.TowerHeight,0) as TowerHeight,
			isnull(tbl_Tower.PlatFormNumber,0) as PlatFormNumber,isnull(tbl_Tower.PoleNumber,0) as PoleNumber,isnull(tbl_TowerBase.TowerBaseType,0) as TowerBaseType,
			isnull(tbl_MachineRoom.MachineRoomType,0) as MachineRoomType,isnull(tbl_MachineRoom.MachineRoomArea,0) as MachineRoomArea,
			isnull(tbl_ExternalElectricPower.ExternalElectric,0) as ExternalElectric,isnull(tbl_EquipmentInstall.SwitchPower,0) as SwitchPower,
			isnull(tbl_EquipmentInstall.Battery,0) as Battery,isnull(tbl_EquipmentInstall.CabinetNumber,0) as CabinetNumber
		from tbl_Purchase left join tbl_Place on tbl_Purchase.PlaceId=tbl_Place.Id
						left join tbl_Remodeling on tbl_Purchase.PlaceId=tbl_Remodeling.PlaceId
						left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
						left join tbl_Tower on tbl_Place.Id=tbl_Tower.ParentId and tbl_Tower.PropertyType=4
						left join tbl_TowerBase on tbl_Place.Id=tbl_TowerBase.ParentId and tbl_TowerBase.PropertyType=4
						left join tbl_MachineRoom on tbl_Place.Id=tbl_MachineRoom.ParentId and tbl_MachineRoom.PropertyType=4
						left join tbl_ExternalElectricPower on tbl_Place.Id=tbl_ExternalElectricPower.ParentId and tbl_ExternalElectricPower.PropertyType=4
						left join tbl_EquipmentInstall on tbl_Place.Id=tbl_EquipmentInstall.ParentId and tbl_EquipmentInstall.PropertyType=4
		where (@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PropertyRight = 0 or tbl_Place.PropertyRight = @PropertyRight) and
				(@Importance = 0 or tbl_Place.Importance = @Importance) and
				(@TelecomShare = 0 or tbl_Place.TelecomShare = @TelecomShare) and
				(@MobileShare = 0 or tbl_Place.MobileShare = @MobileShare) and
				(@UnicomShare = 0 or tbl_Place.UnicomShare = @UnicomShare) and
				(@State = 0 or tbl_Place.[State] = @State) and
				tbl_Remodeling.Id is null
		order by tbl_Place.PlaceCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Purchase left join tbl_Place on tbl_Purchase.PlaceId=tbl_Place.Id
						left join tbl_Remodeling on tbl_Purchase.PlaceId=tbl_Remodeling.PlaceId
						left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
						left join tbl_Tower on tbl_Place.Id=tbl_Tower.ParentId and tbl_Tower.PropertyType=4
						left join tbl_TowerBase on tbl_Place.Id=tbl_TowerBase.ParentId and tbl_TowerBase.PropertyType=4
						left join tbl_MachineRoom on tbl_Place.Id=tbl_MachineRoom.ParentId and tbl_MachineRoom.PropertyType=4
						left join tbl_ExternalElectricPower on tbl_Place.Id=tbl_ExternalElectricPower.ParentId and tbl_ExternalElectricPower.PropertyType=4
						left join tbl_EquipmentInstall on tbl_Place.Id=tbl_EquipmentInstall.ParentId and tbl_EquipmentInstall.PropertyType=4
		where (@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PropertyRight = 0 or tbl_Place.PropertyRight = @PropertyRight) and
				(@Importance = 0 or tbl_Place.Importance = @Importance) and
				(@TelecomShare = 0 or tbl_Place.TelecomShare = @TelecomShare) and
				(@MobileShare = 0 or tbl_Place.MobileShare = @MobileShare) and
				(@UnicomShare = 0 or tbl_Place.UnicomShare = @UnicomShare) and
				(@State = 0 or tbl_Place.[State] = @State) and
				tbl_Remodeling.Id is null
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryProjectInformationPage]    Script Date: 2015/10/27 星期二 14:00:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectInformationPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PropertyRightSql varchar(100),
					@GroupPlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@ConstructionMethod int,
					@ConstructionProgress int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @TempPropertyRight table(value int)
	insert @TempPropertyRight exec(@PropertyRightSql)

	select tbl_ConstructionTask.Id,tbl_ConstructionTask.PlaceId,isnull(tower.FileIdList,'') as IsFileTower,isnull(towerBase.FileIdList,'') as IsFileTowerBase,isnull(machineRoom.FileIdList,'') as IsFileMachineRoom,
		isnull(externalElectricPower.FileIdList,'') as IsFileExternal,isnull(equipmentInstall.FileIdList,'') as IsFileEquipment,isnull(addressExplor.FileIdList,'') as IsFileAddress,
		isnull(foundationTest.FileIdList,'') as IsFileFoundation,isnull(tbl_Tower.Id,'00000000-0000-0000-0000-000000000000') as TowerId,isnull(tbl_TowerBase.Id,'00000000-0000-0000-0000-000000000000') as TowerBaseId,isnull(tbl_MachineRoom.Id,'00000000-0000-0000-0000-000000000000') as MachineRoomId,
		isnull(tbl_ExternalElectricPower.Id,'00000000-0000-0000-0000-000000000000') as ExternalId,isnull(tbl_EquipmentInstall.Id,'00000000-0000-0000-0000-000000000000') as EquipmentId,isnull(tbl_AddressExplor.Id,'00000000-0000-0000-0000-000000000000') as AddressId,isnull(tbl_FoundationTest.Id,'00000000-0000-0000-0000-000000000000') as FoundationId,
		tbl_Place.GroupPlaceCode,tbl_Place.PlaceName,tbl_Project.ProjectCode,tbl_Project.ProjectName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
		tbl_Place.PropertyRight,tbl_ConstructionTask.ConstructionMethod,tbl_ConstructionTask.ConstructionProgress,isnull(tbl_Tower.TowerType,0) as TowerType,isnull(tbl_Tower.TowerHeight,0) as TowerHeight,
		isnull(tbl_Tower.PlatFormNumber,0) as PlatFormNumber,isnull(tbl_Tower.PoleNumber,0) as PoleNumber,isnull(tbl_TowerBase.TowerBaseType,0) as TowerBaseType,
		isnull(tbl_MachineRoom.MachineRoomType,0) as MachineRoomType,isnull(tbl_MachineRoom.MachineRoomArea,0) as MachineRoomArea,isnull(tbl_ExternalElectricPower.ExternalElectric,0) as ExternalElectric,
		isnull(tbl_EquipmentInstall.SwitchPower,0) as SwitchPower,isnull(tbl_EquipmentInstall.Battery,0) as Battery,isnull(tbl_EquipmentInstall.CabinetNumber,0) as CabinetNumber,tbl_PlaceProperty.MobileShare,
		tbl_PlaceProperty.MobilePoleNumber,tbl_PlaceProperty.MobileCabinetNumber,tbl_PlaceProperty.MobilePowerUsed,tbl_PlaceProperty.TelecomShare,tbl_PlaceProperty.TelecomPoleNumber,tbl_PlaceProperty.TelecomCabinetNumber,
		tbl_PlaceProperty.TelecomPowerUsed,tbl_PlaceProperty.UnicomShare,tbl_PlaceProperty.UnicomPoleNumber,tbl_PlaceProperty.UnicomCabinetNumber,tbl_PlaceProperty.UnicomPowerUsed
		from tbl_ConstructionTask left join tbl_Place on tbl_ConstructionTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId = tbl_Project.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Tower on tbl_ConstructionTask.Id=tbl_Tower.ParentId and tbl_Tower.PropertyType=3
							left join tbl_TowerBase on tbl_ConstructionTask.Id=tbl_TowerBase.ParentId and tbl_TowerBase.PropertyType=3
							left join tbl_MachineRoom on tbl_ConstructionTask.Id=tbl_MachineRoom.ParentId and tbl_MachineRoom.PropertyType=3
							left join tbl_ExternalElectricPower on tbl_ConstructionTask.Id=tbl_ExternalElectricPower.ParentId and tbl_ExternalElectricPower.PropertyType=3
							left join tbl_EquipmentInstall on tbl_ConstructionTask.Id=tbl_EquipmentInstall.ParentId and tbl_EquipmentInstall.PropertyType=3
							left join tbl_AddressExplor on tbl_ConstructionTask.Id=tbl_AddressExplor.ParentId and tbl_AddressExplor.PropertyType=3
							left join tbl_FoundationTest on tbl_ConstructionTask.Id=tbl_FoundationTest.ParentId and tbl_FoundationTest.PropertyType=3
							left join tbl_FileAssociation tower on tbl_Tower.Id = tower.EntityId and tower.EntityName = 'Tower'
							left join tbl_FileAssociation towerBase on tbl_TowerBase.Id = towerBase.EntityId and towerBase.EntityName = 'TowerBase'
							left join tbl_FileAssociation machineRoom on tbl_MachineRoom.Id = machineRoom.EntityId and machineRoom.EntityName = 'MachineRoom'
							left join tbl_FileAssociation externalElectricPower on tbl_ExternalElectricPower.Id = externalElectricPower.EntityId and externalElectricPower.EntityName = 'ExternalElectricPower'
							left join tbl_FileAssociation equipmentInstall on tbl_EquipmentInstall.Id = equipmentInstall.EntityId and equipmentInstall.EntityName = 'EquipmentInstall'
							left join tbl_FileAssociation addressExplor on tbl_AddressExplor.Id = addressExplor.EntityId and addressExplor.EntityName = 'AddressExplor'
							left join tbl_FileAssociation foundationTest on tbl_FoundationTest.Id = foundationTest.EntityId and foundationTest.EntityName = 'FoundationTest'
							left join tbl_PlaceProperty on tbl_ConstructionTask.Id=tbl_PlaceProperty.ParentId and tbl_PlaceProperty.PropertyType=3
							left join @TempPropertyRight t on tbl_Place.PropertyRight = t.value
		where (tbl_ConstructionTask.CreateDate >= @BeginDate and tbl_ConstructionTask.CreateDate < @EndDate) and
				(@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionMethod=0 or tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 5) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 5)) and
				t.value is not null
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_ConstructionTask left join tbl_Place on tbl_ConstructionTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId = tbl_Project.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Tower on tbl_ConstructionTask.Id=tbl_Tower.ParentId and tbl_Tower.PropertyType=3
							left join tbl_TowerBase on tbl_ConstructionTask.Id=tbl_TowerBase.ParentId and tbl_TowerBase.PropertyType=3
							left join tbl_MachineRoom on tbl_ConstructionTask.Id=tbl_MachineRoom.ParentId and tbl_MachineRoom.PropertyType=3
							left join tbl_ExternalElectricPower on tbl_ConstructionTask.Id=tbl_ExternalElectricPower.ParentId and tbl_ExternalElectricPower.PropertyType=3
							left join tbl_EquipmentInstall on tbl_ConstructionTask.Id=tbl_EquipmentInstall.ParentId and tbl_EquipmentInstall.PropertyType=3
							left join tbl_AddressExplor on tbl_ConstructionTask.Id=tbl_AddressExplor.ParentId and tbl_AddressExplor.PropertyType=3
							left join tbl_FoundationTest on tbl_ConstructionTask.Id=tbl_FoundationTest.ParentId and tbl_FoundationTest.PropertyType=3
							left join tbl_FileAssociation tower on tbl_Tower.Id = tower.EntityId and tower.EntityName = 'Tower'
							left join tbl_FileAssociation towerBase on tbl_TowerBase.Id = towerBase.EntityId and towerBase.EntityName = 'TowerBase'
							left join tbl_FileAssociation machineRoom on tbl_MachineRoom.Id = machineRoom.EntityId and machineRoom.EntityName = 'MachineRoom'
							left join tbl_FileAssociation externalElectricPower on tbl_ExternalElectricPower.Id = externalElectricPower.EntityId and externalElectricPower.EntityName = 'ExternalElectricPower'
							left join tbl_FileAssociation equipmentInstall on tbl_EquipmentInstall.Id = equipmentInstall.EntityId and equipmentInstall.EntityName = 'EquipmentInstall'
							left join tbl_FileAssociation addressExplor on tbl_AddressExplor.Id = addressExplor.EntityId and addressExplor.EntityName = 'AddressExplor'
							left join tbl_FileAssociation foundationTest on tbl_FoundationTest.Id = foundationTest.EntityId and foundationTest.EntityName = 'FoundationTest'
							left join tbl_PlaceProperty on tbl_ConstructionTask.Id=tbl_PlaceProperty.ParentId and tbl_PlaceProperty.PropertyType=3
							left join @TempPropertyRight t on tbl_Place.PropertyRight = t.value
		where (tbl_ConstructionTask.CreateDate >= @BeginDate and tbl_ConstructionTask.CreateDate < @EndDate) and
				(@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionMethod=0 or tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 5) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 5)) and
				t.value is not null
END
GO

/****** Object:  StoredProcedure [dbo].[prc_ExportProjectInformationExcel]    Script Date: 2015/10/27 星期二 14:00:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportProjectInformationExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@PropertyRightSql varchar(100),
					@GroupPlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@ConstructionMethod int,
					@ConstructionProgress int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @TempPropertyRight table(value int)
	insert @TempPropertyRight exec(@PropertyRightSql)

	select tbl_Place.GroupPlaceCode as '集团编码',tbl_Place.PlaceName as '基站名称',tbl_Project.ProjectCode as '项目编码',tbl_Project.ProjectName as '建设项目',tbl_Area.AreaName as '区域',
		tbl_Reseau.ReseauName as '网格',tbl_PlaceCategory.PlaceCategoryName as '基站类型',case tbl_Place.PropertyRight when 1 then '铁塔' when 2 then '购移动' when 3 then '购电信' when 4 then '购联通' else '' end as '产权',
		case tbl_ConstructionTask.ConstructionMethod when 1 then '新建' when 2 then '改造' else '' end as '建设方式',case tbl_ConstructionTask.ConstructionProgress when 1 then '未开工' when 2 then '进行中' when 3 then '已完工' when 4 then '暂缓' when 5 then '取消' else '' end as '建设进度',
		case tbl_Tower.TowerType when 1 then '路灯杆塔' when 2 then '双轮景观塔' when 3 then '灯杆景观塔' when 4 then '三管塔' when 5 then '插接式单管塔' when 6 then '角钢塔' when 7 then '落地拉线塔' when 8 then '屋顶拉线塔' when 9 then '增高架' when 10 then '支撑杆' when 11 then '抱杆' when 12 then '仿生树' else '' end as '铁塔类型',isnull(tbl_Tower.TowerHeight,0) as '铁塔高度',
		isnull(tbl_Tower.PlatFormNumber,0) as '平台数量',isnull(tbl_Tower.PoleNumber,0) as '抱杆数量',case tbl_TowerBase.TowerBaseType when 1 then '独立桩基' when 2 then '开挖式基础' when 3 then '楼顶塔基础' when 4 then '拉线塔基础' end as '基础类型',
		case tbl_MachineRoom.MachineRoomType when 1 then '自建砖混机房' when 2 then '租用砖混机房' when 3 then '自建彩钢板机房' when 4 then '一体化机柜' when 5 then '其他' end as '机房类型',isnull(tbl_MachineRoom.MachineRoomArea,0) as '机房面积',
		case tbl_ExternalElectricPower.ExternalElectric when 1 then '专线' when 2 then '专变' when 3 then '转供' end as '引入方式',isnull(tbl_EquipmentInstall.SwitchPower,0) as '开关电源',isnull(tbl_EquipmentInstall.Battery,0) as '蓄电池',isnull(tbl_EquipmentInstall.CabinetNumber,0) as '机柜数量',
		case tbl_PlaceProperty.MobileShare when 1 then '是' when 2 then '否' end as '移动共享',tbl_PlaceProperty.MobilePoleNumber as '移动抱杆数量',tbl_PlaceProperty.MobileCabinetNumber as '移动机柜数量',tbl_PlaceProperty.MobilePowerUsed as '移动用电量',
		case tbl_PlaceProperty.TelecomShare when 1 then '是' when 2 then '否' end as '电信共享',tbl_PlaceProperty.TelecomPoleNumber as '电信抱杆数量',tbl_PlaceProperty.TelecomCabinetNumber as '电信机柜数量',tbl_PlaceProperty.TelecomPowerUsed as '电信用电量',
		case tbl_PlaceProperty.UnicomShare when 1 then '是' when 2 then '否' end as '联通共享',tbl_PlaceProperty.UnicomPoleNumber as '联通抱杆数量',tbl_PlaceProperty.UnicomCabinetNumber as '联通机柜数量',tbl_PlaceProperty.UnicomPowerUsed as '联通用电量'
		from tbl_ConstructionTask left join tbl_Place on tbl_ConstructionTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_Project on tbl_ConstructionTask.ProjectId = tbl_Project.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Tower on tbl_ConstructionTask.Id=tbl_Tower.ParentId and tbl_Tower.PropertyType=3
							left join tbl_TowerBase on tbl_ConstructionTask.Id=tbl_TowerBase.ParentId and tbl_TowerBase.PropertyType=3
							left join tbl_MachineRoom on tbl_ConstructionTask.Id=tbl_MachineRoom.ParentId and tbl_MachineRoom.PropertyType=3
							left join tbl_ExternalElectricPower on tbl_ConstructionTask.Id=tbl_ExternalElectricPower.ParentId and tbl_ExternalElectricPower.PropertyType=3
							left join tbl_EquipmentInstall on tbl_ConstructionTask.Id=tbl_EquipmentInstall.ParentId and tbl_EquipmentInstall.PropertyType=3
							left join tbl_AddressExplor on tbl_ConstructionTask.Id=tbl_AddressExplor.ParentId and tbl_AddressExplor.PropertyType=3
							left join tbl_FoundationTest on tbl_ConstructionTask.Id=tbl_FoundationTest.ParentId and tbl_FoundationTest.PropertyType=3
							left join tbl_FileAssociation tower on tbl_Tower.Id = tower.EntityId and tower.EntityName = 'Tower'
							left join tbl_FileAssociation towerBase on tbl_TowerBase.Id = towerBase.EntityId and towerBase.EntityName = 'TowerBase'
							left join tbl_FileAssociation machineRoom on tbl_MachineRoom.Id = machineRoom.EntityId and machineRoom.EntityName = 'MachineRoom'
							left join tbl_FileAssociation externalElectricPower on tbl_ExternalElectricPower.Id = externalElectricPower.EntityId and externalElectricPower.EntityName = 'ExternalElectricPower'
							left join tbl_FileAssociation equipmentInstall on tbl_EquipmentInstall.Id = equipmentInstall.EntityId and equipmentInstall.EntityName = 'EquipmentInstall'
							left join tbl_FileAssociation addressExplor on tbl_AddressExplor.Id = addressExplor.EntityId and addressExplor.EntityName = 'AddressExplor'
							left join tbl_FileAssociation foundationTest on tbl_FoundationTest.Id = foundationTest.EntityId and foundationTest.EntityName = 'FoundationTest'
							left join tbl_PlaceProperty on tbl_ConstructionTask.Id=tbl_PlaceProperty.ParentId and tbl_PlaceProperty.PropertyType=3
							left join @TempPropertyRight t on tbl_Place.PropertyRight = t.value
		where (tbl_ConstructionTask.CreateDate >= @BeginDate and tbl_ConstructionTask.CreateDate < DATEADD(DAY,1,@EndDate)) and
				(@GroupPlaceCode = '' or CHARINDEX(@GroupPlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionMethod=0 or tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 5) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 5)) and
				t.value is not null
		order by tbl_Place.PlaceCode
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetTowerLog]    Script Date: 2015/10/28 星期三 13:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetTowerLog]
					@PropertyType int,
					@ParentId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_TowerLog.Id,isnull(tbl_FileAssociation.FileIdList,'') as IsFile,tbl_TowerLog.OperationType,tbl_TowerLog.TowerType,tbl_TowerLog.TowerHeight,
		tbl_TowerLog.PlatFormNumber,tbl_TowerLog.PoleNumber,tbl_User.FullName,tbl_TowerLog.CreateDate
		from tbl_TowerLog left join tbl_FileAssociation on tbl_TowerLog.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'TowerLog'
						left join tbl_User on tbl_TowerLog.CreateUserId=tbl_User.Id
		where tbl_TowerLog.PropertyType=@PropertyType and
				tbl_TowerLog.ParentId=@ParentId
		order by tbl_TowerLog.CreateDate

END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetTowerBaseLog]    Script Date: 2015/10/28 星期三 13:32:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetTowerBaseLog]
					@PropertyType int,
					@ParentId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_TowerBaseLog.Id,isnull(tbl_FileAssociation.FileIdList,'') as IsFile,tbl_TowerBaseLog.OperationType,tbl_TowerBaseLog.TowerBaseType,
	tbl_User.FullName,tbl_TowerBaseLog.CreateDate
		from tbl_TowerBaseLog left join tbl_FileAssociation on tbl_TowerBaseLog.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'TowerBaseLog'
						left join tbl_User on tbl_TowerBaseLog.CreateUserId=tbl_User.Id
		where tbl_TowerBaseLog.PropertyType=@PropertyType and
				tbl_TowerBaseLog.ParentId=@ParentId
		order by tbl_TowerBaseLog.CreateDate

END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetMachineRoomLog]    Script Date: 2015/10/28 星期三 13:32:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetMachineRoomLog]
					@PropertyType int,
					@ParentId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_MachineRoomLog.Id,isnull(tbl_FileAssociation.FileIdList,'') as IsFile,tbl_MachineRoomLog.OperationType,tbl_MachineRoomLog.MachineRoomType,
	tbl_MachineRoomLog.MachineRoomArea,tbl_User.FullName,tbl_MachineRoomLog.CreateDate
		from tbl_MachineRoomLog left join tbl_FileAssociation on tbl_MachineRoomLog.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'MachineRoomLog'
						left join tbl_User on tbl_MachineRoomLog.CreateUserId=tbl_User.Id
		where tbl_MachineRoomLog.PropertyType=@PropertyType and
				tbl_MachineRoomLog.ParentId=@ParentId
		order by tbl_MachineRoomLog.CreateDate

END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetExternalElectricPowerLog]    Script Date: 2015/10/28 星期三 13:32:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetExternalElectricPowerLog]
					@PropertyType int,
					@ParentId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_ExternalElectricPowerLog.Id,isnull(tbl_FileAssociation.FileIdList,'') as IsFile,tbl_ExternalElectricPowerLog.OperationType,tbl_ExternalElectricPowerLog.ExternalElectric,
		tbl_User.FullName,tbl_ExternalElectricPowerLog.CreateDate
		from tbl_ExternalElectricPowerLog left join tbl_FileAssociation on tbl_ExternalElectricPowerLog.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'ExternalElectricPowerLog'
						left join tbl_User on tbl_ExternalElectricPowerLog.CreateUserId=tbl_User.Id
		where tbl_ExternalElectricPowerLog.PropertyType=@PropertyType and
				tbl_ExternalElectricPowerLog.ParentId=@ParentId
		order by tbl_ExternalElectricPowerLog.CreateDate

END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetEquipmentInstallLog]    Script Date: 2015/10/28 星期三 13:33:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetEquipmentInstallLog]
					@PropertyType int,
					@ParentId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_EquipmentInstallLog.Id,isnull(tbl_FileAssociation.FileIdList,'') as IsFile,tbl_EquipmentInstallLog.OperationType,tbl_EquipmentInstallLog.SwitchPower,
		tbl_EquipmentInstallLog.Battery,tbl_EquipmentInstallLog.CabinetNumber,tbl_User.FullName,tbl_EquipmentInstallLog.CreateDate
		from tbl_EquipmentInstallLog left join tbl_FileAssociation on tbl_EquipmentInstallLog.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'EquipmentInstallLog'
						left join tbl_User on tbl_EquipmentInstallLog.CreateUserId=tbl_User.Id
		where tbl_EquipmentInstallLog.PropertyType=@PropertyType and
				tbl_EquipmentInstallLog.ParentId=@ParentId
		order by tbl_EquipmentInstallLog.CreateDate

END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetAddressExplorLog]    Script Date: 2015/10/28 星期三 13:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetAddressExplorLog]
					@PropertyType int,
					@ParentId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_AddressExplorLog.Id,isnull(tbl_FileAssociation.FileIdList,'') as IsFile,tbl_AddressExplorLog.OperationType,tbl_User.FullName,tbl_AddressExplorLog.CreateDate
		from tbl_AddressExplorLog left join tbl_FileAssociation on tbl_AddressExplorLog.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'AddressExplorLog'
						left join tbl_User on tbl_AddressExplorLog.CreateUserId=tbl_User.Id
		where tbl_AddressExplorLog.PropertyType=@PropertyType and
				tbl_AddressExplorLog.ParentId=@ParentId
		order by tbl_AddressExplorLog.CreateDate

END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetFoundationTestLog]    Script Date: 2015/10/28 星期三 13:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetFoundationTestLog]
					@PropertyType int,
					@ParentId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_FoundationTestLog.Id,isnull(tbl_FileAssociation.FileIdList,'') as IsFile,tbl_FoundationTestLog.OperationType,tbl_User.FullName,tbl_FoundationTestLog.CreateDate
		from tbl_FoundationTestLog left join tbl_FileAssociation on tbl_FoundationTestLog.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'FoundationTestLog'
						left join tbl_User on tbl_FoundationTestLog.CreateUserId=tbl_User.Id
		where tbl_FoundationTestLog.PropertyType=@PropertyType and
				tbl_FoundationTestLog.ParentId=@ParentId
		order by tbl_FoundationTestLog.CreateDate

END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetPlacePropertyLog]    Script Date: 2015/10/28 星期三 13:34:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetPlacePropertyLog]
					@PropertyType int,
					@ParentId uniqueidentifier,
					@CompanyNameId int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	if(@CompanyNameId=1)
	begin
		select tbl_PlacePropertyLog.MobileShare,tbl_PlacePropertyLog.MobilePoleNumber,tbl_PlacePropertyLog.MobileCabinetNumber,tbl_PlacePropertyLog.MobilePowerUsed,tbl_User.FullName,tbl_PlacePropertyLog.CreateDate
			from tbl_PlacePropertyLog left join tbl_User on tbl_PlacePropertyLog.MobileCreateUserId=tbl_User.Id
			where tbl_PlacePropertyLog.PropertyType=@PropertyType and
					tbl_PlacePropertyLog.ParentId=@ParentId and
					tbl_PlacePropertyLog.CompanyNameId=@CompanyNameId
			order by tbl_PlacePropertyLog.CreateDate
	end
	if(@CompanyNameId=2)
	begin
		select tbl_PlacePropertyLog.TelecomShare,tbl_PlacePropertyLog.TelecomPoleNumber,tbl_PlacePropertyLog.TelecomCabinetNumber,tbl_PlacePropertyLog.TelecomPowerUsed,tbl_User.FullName,tbl_PlacePropertyLog.CreateDate
			from tbl_PlacePropertyLog left join tbl_User on tbl_PlacePropertyLog.TelecomCreateUserId=tbl_User.Id
			where tbl_PlacePropertyLog.PropertyType=@PropertyType and
					tbl_PlacePropertyLog.ParentId=@ParentId and
					tbl_PlacePropertyLog.CompanyNameId=@CompanyNameId
			order by tbl_PlacePropertyLog.CreateDate
	end
	if(@CompanyNameId=3)
	begin
		select tbl_PlacePropertyLog.UnicomShare,tbl_PlacePropertyLog.UnicomPoleNumber,tbl_PlacePropertyLog.UnicomCabinetNumber,tbl_PlacePropertyLog.UnicomPowerUsed,tbl_User.FullName,tbl_PlacePropertyLog.CreateDate
			from tbl_PlacePropertyLog left join tbl_User on tbl_PlacePropertyLog.UnicomCreateUserId=tbl_User.Id
			where tbl_PlacePropertyLog.PropertyType=@PropertyType and
					tbl_PlacePropertyLog.ParentId=@ParentId and
					tbl_PlacePropertyLog.CompanyNameId=@CompanyNameId
			order by tbl_PlacePropertyLog.CreateDate
	end
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetWorkOrderDetail]    Script Date: 2015/11/23 17:08:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetWorkOrderDetail]
					@WorkOrderId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_WorkOrderDetail.Id,DATEADD(MINUTE,tbl_WorkOrderDetail.BeginMinute,(DATEADD(HOUR,tbl_WorkOrderDetail.BeginHour,tbl_WorkOrderDetail.WorkBeginDate))) as WorkBeginDate,
		DATEADD(MINUTE,tbl_WorkOrderDetail.EndMinute,(DATEADD(HOUR,tbl_WorkOrderDetail.EndHour,tbl_WorkOrderDetail.WorkEndDate))) as WorkEndDate,tbl_WorkOrderDetail.ExecuteSituation,
		tbl_WorkOrderDetail.MaterialConsumption,tbl_WorkOrderDetail.PersonnelNumber,tbl_WorkOrderDetail.CarType,tbl_WorkOrderDetail.IsFinish,case tbl_WorkOrderDetail.IsFinish when 1 then '是' else '否' end as IsFinishName
		from tbl_WorkOrderDetail
		where tbl_WorkOrderDetail.WorkOrderId=@WorkOrderId
		order by tbl_WorkOrderDetail.CreateDate
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryWorkApplysPage]    Script Date: 2015/11/23 17:09:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWorkApplysPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@Title nvarchar(50),
					@ReseauId uniqueidentifier,
					@OrderState int,
					@IsSoved int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_WorkApply.Id,tbl_WorkApply.OrderCode,tbl_WorkApply.CreateDate,tbl_WorkApply.Title,tbl_Reseau.ReseauName,isnull(tbl_User.FullName,'') as FullName,tbl_WorkApply.ApplyReason,
		tbl_WorkApply.OrderState,isnull(tbl_WorkOrder.OrderCode,'') as PGOrderCode,tbl_WorkApply.IsSoved,tbl_WorkApply.WorkOrderId,isnull(tbl_FileAssociation.FileIdList,'') as IsFile,
		tbl_WorkApply.ReturnReason
		from tbl_WorkApply left join tbl_Reseau on tbl_WorkApply.ReseauId=tbl_Reseau.Id
							left join tbl_User on tbl_WorkApply.ReseauManagerId=tbl_User.Id
							left join tbl_WorkOrder on tbl_WorkApply.WorkOrderId=tbl_WorkOrder.Id
							left join tbl_FileAssociation on tbl_WorkApply.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'WorkApply'
		where (tbl_WorkApply.CreateDate >= @BeginDate and tbl_WorkApply.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkApply.Title) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.ReseauId = @ReseauId) and
				(@OrderState = 0 or tbl_WorkApply.OrderState = @OrderState) and
				(@IsSoved = 0 or tbl_WorkApply.IsSoved = @IsSoved) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.CreateUserId = @CreateUserId)
		order by tbl_WorkApply.OrderCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_WorkApply left join tbl_User on tbl_WorkApply.ReseauManagerId=tbl_User.Id
							left join tbl_WorkOrder on tbl_WorkApply.WorkOrderId=tbl_WorkOrder.Id
		where (tbl_WorkApply.CreateDate >= @BeginDate and tbl_WorkApply.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkApply.Title) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.ReseauId = @ReseauId) and
				(@OrderState = 0 or tbl_WorkApply.OrderState = @OrderState) and
				(@IsSoved = 0 or tbl_WorkApply.IsSoved = @IsSoved) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.CreateUserId = @CreateUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryWorkApplyWaitPage]    Script Date: 2015/11/23 17:10:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWorkApplyWaitPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@Title nvarchar(50),
					@IsSoved int,
					@SendUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_WorkApply.Id,tbl_WorkApply.OrderCode,tbl_WorkApply.CreateDate,tbl_WorkApply.Title,tbl_WorkApply.ApplyReason,
		isnull(tbl_WorkOrder.OrderCode,'') as PGOrderCode,tbl_WorkApply.IsSoved,(tbl_User.FullName+'/'+tbl_Department.DepartmentName) as CreateFullName,
		isnull(tbl_FileAssociation.FileIdList,'') as IsFile
		from tbl_WorkApply left join tbl_User on tbl_WorkApply.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_WorkOrder on tbl_WorkApply.WorkOrderId=tbl_WorkOrder.Id
							left join tbl_FileAssociation on tbl_WorkApply.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'WorkApply'
		where (tbl_WorkApply.CreateDate >= @BeginDate and tbl_WorkApply.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkApply.Title) > 0) and
				(@IsSoved = 0 or tbl_WorkApply.IsSoved = @IsSoved) and
				tbl_WorkApply.OrderState = 3 and
				tbl_WorkApply.WorkOrderId='00000000-0000-0000-0000-000000000000' and
				tbl_WorkApply.ReseauManagerId = @SendUserId
		order by tbl_WorkApply.OrderCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_WorkApply
		where (tbl_WorkApply.CreateDate >= @BeginDate and tbl_WorkApply.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkApply.Title) > 0) and
				(@IsSoved = 0 or tbl_WorkApply.IsSoved = @IsSoved) and
				tbl_WorkApply.OrderState = 3 and
				tbl_WorkApply.WorkOrderId='00000000-0000-0000-0000-000000000000' and
				tbl_WorkApply.ReseauManagerId = @SendUserId
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryWorkOrdersPage]    Script Date: 2015/11/23 17:10:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWorkOrdersPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@Title nvarchar(50),
					@ReseauId uniqueidentifier,
					@WorkBigClassId uniqueidentifier,
					@WorkSmallClassId uniqueidentifier,
					@CustomerId uniqueidentifier,
					@MaintainContactMan nvarchar(50),
					@SendUserId uniqueidentifier,
					@IsFinish int,
					@OrderState int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_WorkOrder.Id,tbl_WorkOrder.OrderCode,tbl_WorkOrder.CreateDate,tbl_WorkOrder.RequireSendDate,tbl_WorkOrder.Title,(tbl_WorkBigClass.BigClassName+'/'+tbl_WorkSmallClass.SmallClassName) as ClassName,
		tbl_Reseau.ReseauName,tbl_User.FullName,isnull(tbl_Customer.CustomerName,'') as CustomerName,tbl_WorkOrder.SceneContactMan as MaintainContactMan,tbl_WorkOrder.WorkContent,tbl_WorkOrder.IsFinish,tbl_WorkOrder.OrderState,
		isnull(tbl_FileAssociation.FileIdList,'') as IsFile,createUser.FullName as CreateFullName
		from tbl_WorkOrder left join tbl_WorkSmallClass on tbl_WorkOrder.WorkSmallClassId=tbl_WorkSmallClass.Id
							left join tbl_WorkBigClass on tbl_WorkSmallClass.WorkBigClassId=tbl_WorkBigClass.Id
							left join tbl_Customer on tbl_WorkOrder.CustomerId=tbl_Customer.Id
							left join tbl_Reseau on tbl_WorkOrder.ReseauId=tbl_Reseau.Id
							left join tbl_User on tbl_Reseau.ReseauManagerId=tbl_User.Id
							left join tbl_User createUser on tbl_WorkOrder.CreateUserId=createUser.Id
							left join tbl_User customerUser on tbl_WorkOrder.CustomerUserId=customerUser.Id
							left join tbl_FileAssociation on tbl_WorkOrder.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'WorkOrder'
		where (tbl_WorkOrder.CreateDate >= @BeginDate and tbl_WorkOrder.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkOrder.Title) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.ReseauId = @ReseauId) and
				(@WorkBigClassId = '00000000-0000-0000-0000-000000000000' or tbl_WorkSmallClass.WorkBigClassId = @WorkBigClassId) and
				(@WorkSmallClassId = '00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.WorkSmallClassId = @WorkSmallClassId) and
				(@CustomerId = '00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.CustomerId = @CustomerId) and
				(@MaintainContactMan = '' or CHARINDEX(@MaintainContactMan,tbl_WorkOrder.SceneContactMan) > 0) and
				(@SendUserId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.ReseauManagerId = @SendUserId) and
				(@IsFinish = 0 or tbl_WorkOrder.IsFinish = @IsFinish) and
				(@OrderState = 0 or tbl_WorkOrder.OrderState = @OrderState) and
				(@CreateUserId='00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.CreateUserId = @CreateUserId)
		order by tbl_WorkOrder.OrderCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_WorkOrder left join tbl_WorkSmallClass on tbl_WorkOrder.WorkSmallClassId=tbl_WorkSmallClass.Id
							left join tbl_WorkBigClass on tbl_WorkSmallClass.WorkBigClassId=tbl_WorkSmallClass.Id
							left join tbl_Customer on tbl_WorkOrder.CustomerId=tbl_Customer.Id
		where (tbl_WorkOrder.CreateDate >= @BeginDate and tbl_WorkOrder.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkOrder.Title) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.ReseauId = @ReseauId) and
				(@WorkBigClassId = '00000000-0000-0000-0000-000000000000' or tbl_WorkSmallClass.WorkBigClassId = @WorkBigClassId) and
				(@WorkSmallClassId = '00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.WorkSmallClassId = @WorkSmallClassId) and
				(@CustomerId = '00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.CustomerId = @CustomerId) and
				(@MaintainContactMan = '' or CHARINDEX(@MaintainContactMan,tbl_WorkOrder.SceneContactMan) > 0) and
				(@SendUserId = '00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.CreateUserId = @SendUserId) and
				(@IsFinish = 0 or tbl_WorkOrder.IsFinish = @IsFinish) and
				(@OrderState = 0 or tbl_WorkOrder.OrderState = @OrderState) and
				(@CreateUserId='00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.CreateUserId = @CreateUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryNewPlanningsPage]    Script Date: 2016-03-05 11:56:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryNewPlanningsPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlanningCode nvarchar(50),
					@PlanningName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@SceneId uniqueidentifier,
					@TelecomDemand int,
					@MobileDemand int,
					@UnicomDemand int,
					@AddressingState int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Planning.Id,tbl_Addressing.Id as AddressingId,tbl_Planning.PlanningCode,tbl_Planning.PlanningName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.MobileDemand,tbl_Planning.TelecomDemand,tbl_Planning.UnicomDemand,ISNULL(tbl_Scene.SceneName,'') as SceneName,
			tbl_Addressing.OwnerName,tbl_Addressing.OwnerContact,tbl_Addressing.OwnerPhoneNumber,ISNULL(tbl_Project.ProjectName,'') as ProjectName,tbl_Planning.AddressingState,
			tbl_Planning.Remarks,u2.FullName,tbl_Planning.CreateDate,tbl_Planning.PlaceId,isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_Planning left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
							left join tbl_Addressing on tbl_Planning.Id = tbl_Addressing.PlanningId
							left join tbl_Scene on tbl_Addressing.SceneId = tbl_Scene.Id
							left join tbl_Project on tbl_Addressing.ProjectId = tbl_Project.Id
							left join tbl_FileAssociation on tbl_Addressing.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Addressing'
		where (tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@SceneId = '00000000-0000-0000-0000-000000000000' or tbl_Addressing.SceneId = @SceneId) and
				(@TelecomDemand = 0 or tbl_Planning.TelecomDemand = @TelecomDemand) and
				(@MobileDemand = 0 or tbl_Planning.MobileDemand = @MobileDemand) and
				(@UnicomDemand = 0 or tbl_Planning.UnicomDemand = @UnicomDemand) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.CreateUserId = @CreateUserId)
		order by tbl_Planning.PlanningCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Planning left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
						left join tbl_Addressing on tbl_Planning.Id = tbl_Addressing.PlanningId
		where (tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@SceneId = '00000000-0000-0000-0000-000000000000' or tbl_Addressing.SceneId = @SceneId) and
				(@TelecomDemand = 0 or tbl_Planning.TelecomDemand = @TelecomDemand) and
				(@MobileDemand = 0 or tbl_Planning.MobileDemand = @MobileDemand) and
				(@UnicomDemand = 0 or tbl_Planning.UnicomDemand = @UnicomDemand) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.CreateUserId = @CreateUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryNewRemodelingsPage]    Script Date: 2016-03-08 15:32:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryNewRemodelingsPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Urgency int,
					@OrderState int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Remodeling.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Place.Lng,tbl_Place.Lat,tbl_Place.PropertyRight,case when osMobile.Id is null then '不需要' else '需要' end as MobileDemand1,
			case when osTelecom.Id is null then '不需要' else '需要' end as TelecomDemand,case when osUnicom.Id is null then '不需要' else '需要' end as UnicomDemand,
			tbl_Remodeling.OrderState,tbl_Remodeling.Remarks,tbl_User.FullName,tbl_Remodeling.CreateDate,
			tbl_Remodeling.PlaceId,tbl_Place.TelecomShare,tbl_Place.MobileShare,tbl_Place.UnicomShare
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User on tbl_Remodeling.CreateUserId = tbl_User.Id
							left join tbl_Project on tbl_Remodeling.ProjectId = tbl_Project.Id
							left join tbl_User pm on tbl_Remodeling.ProjectManagerId = pm.Id
							left join tbl_OperatorsSharing osMobile on tbl_Remodeling.Id=osMobile.RemodelingId and osMobile.CompanyId='6365F3DE-0FC5-4930-A321-2350EE6269BB'
							left join tbl_OperatorsSharing osTelecom on tbl_Remodeling.Id=osTelecom.RemodelingId and osTelecom.CompanyId='2E0FFE5F-C03A-4767-9915-9683F0DB0B53'
							left join tbl_OperatorsSharing osUnicom on tbl_Remodeling.Id=osUnicom.RemodelingId and osUnicom.CompanyId='0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600'
		where (tbl_Remodeling.CreateDate >= @BeginDate and tbl_Remodeling.CreateDate < @EndDate) and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_Remodeling.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Remodeling.Urgency = @Urgency) and
				(@OrderState = 0 or tbl_Remodeling.OrderState = @OrderState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Remodeling.CreateUserId = @CreateUserId)
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
		where (tbl_Remodeling.CreateDate >= @BeginDate and tbl_Remodeling.CreateDate < @EndDate) and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_Remodeling.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@Urgency = 0 or tbl_Remodeling.Urgency = @Urgency) and
				(@OrderState = 0 or tbl_Remodeling.OrderState = @OrderState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Remodeling.CreateUserId = @CreateUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[[prc_QueryReseaus]]    Script Date: 2016-03-08 15:32:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryReseaus]
					@AreaId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_Reseau.Id,tbl_Reseau.ReseauCode,tbl_Reseau.ReseauName,ISNULL(tbl_User.FullName,'无') as FullName,
			tbl_Reseau.[State],tbl_Reseau.Remarks
		from tbl_Reseau left join tbl_User on tbl_Reseau.ReseauManagerId = tbl_User.Id 
		where tbl_Reseau.AreaId = @AreaId
		order by tbl_Reseau.ReseauCode
END
GO

/****** Object:  StoredProcedure [dbo].[[prc_QueryWorkApplysReport]]    Script Date: 2016-03-26 10:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWorkApplysReport]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@Title nvarchar(50),
					@ReseauId uniqueidentifier,
					@CustomerId uniqueidentifier,
					@OrderState int,
					@IsSoved int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_WorkApply.Id,tbl_WorkApply.OrderCode,tbl_WorkApply.CreateDate,tbl_WorkApply.Title,tbl_Reseau.ReseauName,isnull(tbl_User.FullName,'') as FullName,tbl_WorkApply.ApplyReason,createUser.FullName as CreateFullName,
		tbl_Customer.CustomerName,tbl_WorkApply.OrderState,isnull(tbl_WorkOrder.OrderCode,'') as PGOrderCode,tbl_WorkApply.IsSoved,tbl_WorkApply.WorkOrderId,isnull(tbl_FileAssociation.FileIdList,'') as IsFile
		from tbl_WorkApply left join tbl_Reseau on tbl_WorkApply.ReseauId=tbl_Reseau.Id
							left join tbl_User on tbl_WorkApply.ReseauManagerId=tbl_User.Id
							left join tbl_User createUser on tbl_WorkApply.CreateUserId=createUser.Id
							left join tbl_WorkOrder on tbl_WorkApply.WorkOrderId=tbl_WorkOrder.Id
							left join tbl_Customer on tbl_WorkApply.CustomerId=tbl_Customer.Id
							left join tbl_FileAssociation on tbl_WorkApply.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'WorkApply'
		where (tbl_WorkApply.CreateDate >= @BeginDate and tbl_WorkApply.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkApply.Title) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.ReseauId = @ReseauId) and
				(@CustomerId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.CustomerId = @CustomerId) and
				(@OrderState = 0 or tbl_WorkApply.OrderState = @OrderState) and
				(@IsSoved = 0 or tbl_WorkApply.IsSoved = @IsSoved) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.CreateUserId = @CreateUserId)
		order by tbl_WorkApply.OrderCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_WorkApply left join tbl_User on tbl_WorkApply.ReseauManagerId=tbl_User.Id
							left join tbl_WorkOrder on tbl_WorkApply.WorkOrderId=tbl_WorkOrder.Id
		where (tbl_WorkApply.CreateDate >= @BeginDate and tbl_WorkApply.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkApply.Title) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.ReseauId = @ReseauId) and
				(@CustomerId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.CustomerId = @CustomerId) and
				(@OrderState = 0 or tbl_WorkApply.OrderState = @OrderState) and
				(@IsSoved = 0 or tbl_WorkApply.IsSoved = @IsSoved) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.CreateUserId = @CreateUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetTaskPropertyLog]    Script Date: 2016-03-29 10:56:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetTaskPropertyLog]
					@TaskModel int,
					@ParentId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_TaskPropertyLog.Id,tbl_TaskPropertyLog.ConstructionProgress,isnull(tbl_FileAssociation.FileIdList,'') as IsFile,tbl_TaskPropertyLog.ProgressMemos,tbl_User.FullName,tbl_TaskPropertyLog.CreateDate
		from tbl_TaskPropertyLog left join tbl_FileAssociation on tbl_TaskPropertyLog.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'ProgressLog'
			left join tbl_User on tbl_TaskPropertyLog.ProgressUserId=tbl_User.Id
		where tbl_TaskPropertyLog.ConstructionTaskId=@ParentId and tbl_TaskPropertyLog.TaskModel=@TaskModel and tbl_TaskPropertyLog.RegisterType=1
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetAllUsedCustomers]    Script Date: 2016-04-06 11:02:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetAllUsedCustomers]
						@State int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,TypeId int,CustomerTypeName varchar(50))
	insert @temp values('6CDCD80B-17CE-45FE-837E-1B26EA5DF252',1,'铁塔单位')
	insert @temp values('A2282995-110D-498D-BA0F-6371667CBB7C',2,'土建单位')
	insert @temp values('212631E4-9C5D-460C-9095-B28B575EC3BE',3,'市电单位')
	insert @temp values('05654F2E-7E3F-48B2-8CE0-8D536BBE130E',4,'配套单位')
	insert @temp values('955845BF-D64B-439B-ABE0-ECF2D2E03F15',5,'检测单位')
	insert @temp values('A71986CF-42F3-43CA-8BF0-933EDC6809E8',6,'监理单位')
	insert @temp values('BAA784A7-0EF0-47A4-8971-60951F8E44EC',7,'设计单位')
	insert @temp values('F6BAF797-E6F3-4EAE-AB7A-FAA4ADE82E5A',8,'维护单位')

	select t.Id,t.CustomerTypeName as Name,'00000000-0000-0000-0000-000000000000' as PId,'false' as IsLeafStr,TypeId as CustomerType
		from @temp t
	union
	select tbl_Customer.Id,tbl_Customer.CustomerName,t.Id,'true',tbl_Customer.CustomerType
		from tbl_Customer left join @temp t on tbl_Customer.CustomerType=t.TypeId
		where tbl_Customer.State=@State
	order by CustomerType,Name
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetUsersByCustomer]    Script Date: 2016-04-06 15:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetUsersByCustomer]
					@CustomerId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select '00000000-0000-0000-0000-000000000000' as Id,'请选择' as FullName
	union
	select tbl_User.Id,tbl_User.FullName
		from tbl_User left join tbl_CustomerUser on tbl_User.Id = tbl_CustomerUser.UserId
		where tbl_CustomerUser.CustomerId = @CustomerId
		order by FullName
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryCustomerUsers]    Script Date: 2016-04-07 20:47:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryCustomerUsers]
							@CompanyId uniqueidentifier,
							@DepartmentId uniqueidentifier,
							@CustomerId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_Department.Id,tbl_Department.DepartmentName as Name,tbl_Department.CompanyId as PId,
			'00000000-0000-0000-0000-000000000000' as CustomerUserId,tbl_Department.DepartmentCode as Code,'false' as IsLeafStr,'false' as AsyncLoadStr
		from tbl_Department
		where tbl_Department.CompanyId = @CompanyId and tbl_Department.[State] = 1 and
			(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_Department.Id = @DepartmentId)
	union
	select tbl_User.Id,tbl_User.UserName + '(' + tbl_User.FullName + ')',tbl_User.DepartmentId,ISNULL(tbl_CustomerUser.Id,'00000000-0000-0000-0000-000000000000'),
			'','true','false'
		from tbl_User left join tbl_Department on tbl_User.DepartmentId = tbl_Department.Id
					left join tbl_CustomerUser on tbl_User.Id = tbl_CustomerUser.UserId and tbl_CustomerUser.CustomerId = @CustomerId
		where tbl_Department.CompanyId = @CompanyId and tbl_User.[State] = 1 and tbl_Department.[State] = 1 and
			(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_Department.Id = @DepartmentId)
	union
	select Id,CompanyName,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','','false','false'
		from tbl_Company
		where Id = @CompanyId and tbl_Company.[State] = 1
	order by Code,Name
END
GO

/****** Object:  StoredProcedure [dbo].[[prc_QueryAreas]]    Script Date: 2016-04-11 13:59:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryAreas]
					@State int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_Area.Id,tbl_Area.AreaCode,tbl_Area.AreaName,tbl_Area.Lat,tbl_Area.Lng,ISNULL(tbl_User.FullName,'无') as FullName,
			tbl_Area.[State],tbl_Area.Remarks
		from tbl_Area left join tbl_User on tbl_Area.AreaManagerId = tbl_User.Id 
		where (@State = 0 or tbl_Area.[State] = @State)
		order by tbl_Area.AreaCode
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryWorkApplyProjectPage]    Script Date: 2016-04-14 10:40:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryWorkApplyProjectPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@Title nvarchar(50),
					@ProjectCode nvarchar(50),
					@IsProject int,
					@SendUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_WorkApply.Id,tbl_WorkApply.OrderCode,tbl_WorkApply.CreateDate,tbl_WorkApply.Title,tbl_WorkApply.ApplyReason,
		isnull(tbl_WorkOrder.OrderCode,'') as PGOrderCode,tbl_WorkApply.IsProject,(tbl_User.FullName+'/'+tbl_Department.DepartmentName) as CreateFullName,
		isnull(tbl_FileAssociation.FileIdList,'') as IsFile,tbl_WorkApply.ProjectCode,reseauUser.FullName as ReseauFullName
		from tbl_WorkApply left join tbl_User on tbl_WorkApply.CreateUserId=tbl_User.Id
							left join tbl_User reseauUser on tbl_WorkApply.ReseauManagerId=reseauUser.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_WorkOrder on tbl_WorkApply.WorkOrderId=tbl_WorkOrder.Id
							left join tbl_FileAssociation on tbl_WorkApply.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'WorkApply'
		where (tbl_WorkApply.CreateDate >= @BeginDate and tbl_WorkApply.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkApply.Title) > 0) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_WorkApply.ProjectCode) > 0) and
				(@IsProject = 0 or tbl_WorkApply.IsProject = @IsProject) and
				tbl_WorkApply.OrderState = 3 and
				(tbl_WorkApply.ReseauManagerId = @SendUserId or @SendUserId='00000000-0000-0000-0000-000000000000')
		order by tbl_WorkApply.OrderCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_WorkApply
		where (tbl_WorkApply.CreateDate >= @BeginDate and tbl_WorkApply.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkApply.Title) > 0) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_WorkApply.ProjectCode) > 0) and
				(@IsProject = 0 or tbl_WorkApply.IsProject = @IsProject) and
				tbl_WorkApply.OrderState = 3 and
				(tbl_WorkApply.ReseauManagerId = @SendUserId or @SendUserId='00000000-0000-0000-0000-000000000000')
END
GO

/****** Object:  StoredProcedure [dbo].[prc_ExportWorkApplysExcel]    Script Date: 2016-04-24 10:08:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportWorkApplysExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@Title nvarchar(50),
					@ReseauId uniqueidentifier,
					@CustomerId uniqueidentifier,
					@OrderState int,
					@IsSoved int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_WorkApply.CreateDate as '申请日期',tbl_WorkApply.Title as '标题',createUser.FullName as '申请人',tbl_Reseau.ReseauName as '网格',isnull(tbl_User.FullName,'') as '网格经理',tbl_WorkApply.ApplyReason as '申请事由',
		tbl_Customer.CustomerName as '申请单位',case tbl_WorkApply.OrderState when 1 then '未发送' when 2 then '流转中' when 3 then '流程通过' when 4 then '流程终止' else '' end as '申请状态',
		isnull(tbl_WorkOrder.OrderCode,'') as '派工单号',case tbl_WorkApply.IsSoved when 1 then '是' when 2 then '否' else '' end as '是否解决'
		from tbl_WorkApply left join tbl_Reseau on tbl_WorkApply.ReseauId=tbl_Reseau.Id
							left join tbl_User on tbl_WorkApply.ReseauManagerId=tbl_User.Id
							left join tbl_User createUser on tbl_WorkApply.CreateUserId=createUser.Id
							left join tbl_WorkOrder on tbl_WorkApply.WorkOrderId=tbl_WorkOrder.Id
							left join tbl_Customer on tbl_WorkApply.CustomerId=tbl_Customer.Id
							left join tbl_FileAssociation on tbl_WorkApply.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'WorkApply'
		where (tbl_WorkApply.CreateDate >= @BeginDate and tbl_WorkApply.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkApply.Title) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.ReseauId = @ReseauId) and
				(@CustomerId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.CustomerId = @CustomerId) and
				(@OrderState = 0 or tbl_WorkApply.OrderState = @OrderState) and
				(@IsSoved = 0 or tbl_WorkApply.IsSoved = @IsSoved) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_WorkApply.CreateUserId = @CreateUserId)
		order by tbl_WorkApply.OrderCode desc
END
GO

/****** Object:  StoredProcedure [dbo].[prc_ExportWorkOrdersExcel]    Script Date: 2016-04-24 10:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportWorkOrdersExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@Title nvarchar(50),
					@ReseauId uniqueidentifier,
					@WorkBigClassId uniqueidentifier,
					@WorkSmallClassId uniqueidentifier,
					@CustomerId uniqueidentifier,
					@MaintainContactMan nvarchar(50),
					@SendUserId uniqueidentifier,
					@IsFinish int,
					@OrderState int,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_WorkOrder.OrderCode as '单据编号',tbl_WorkOrder.CreateDate as '派工日期',tbl_WorkOrder.RequireSendDate as '要求派工日期',tbl_WorkOrder.Title as '标题',(tbl_WorkBigClass.BigClassName+'/'+tbl_WorkSmallClass.SmallClassName) as '工单类型',
		tbl_Reseau.ReseauName as '网格',tbl_User.FullName as '网格经理',isnull(tbl_Customer.CustomerName,'') as '派工单位',tbl_WorkOrder.SceneContactMan as '派工联系人',tbl_WorkOrder.WorkContent as '工作内容',case tbl_WorkOrder.IsFinish when 1 then '是' when 2 then '否' else '' end as '是否完成',
		case tbl_WorkOrder.OrderState when 1 then '未发送' when 2 then '流转中' when 3 then '流程通过' when 4 then '流程终止' else '' end as '申请状态',createUser.FullName as '申请人'
		from tbl_WorkOrder left join tbl_WorkSmallClass on tbl_WorkOrder.WorkSmallClassId=tbl_WorkSmallClass.Id
							left join tbl_WorkBigClass on tbl_WorkSmallClass.WorkBigClassId=tbl_WorkBigClass.Id
							left join tbl_Customer on tbl_WorkOrder.CustomerId=tbl_Customer.Id
							left join tbl_Reseau on tbl_WorkOrder.ReseauId=tbl_Reseau.Id
							left join tbl_User on tbl_Reseau.ReseauManagerId=tbl_User.Id
							left join tbl_User createUser on tbl_WorkOrder.CreateUserId=createUser.Id
							left join tbl_User customerUser on tbl_WorkOrder.CustomerUserId=customerUser.Id
							left join tbl_FileAssociation on tbl_WorkOrder.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'WorkOrder'
		where (tbl_WorkOrder.CreateDate >= @BeginDate and tbl_WorkOrder.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkOrder.Title) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.ReseauId = @ReseauId) and
				(@WorkBigClassId = '00000000-0000-0000-0000-000000000000' or tbl_WorkSmallClass.WorkBigClassId = @WorkBigClassId) and
				(@WorkSmallClassId = '00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.WorkSmallClassId = @WorkSmallClassId) and
				(@CustomerId = '00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.CustomerId = @CustomerId) and
				(@MaintainContactMan = '' or CHARINDEX(@MaintainContactMan,tbl_WorkOrder.SceneContactMan) > 0) and
				(@SendUserId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.ReseauManagerId = @SendUserId) and
				(@IsFinish = 0 or tbl_WorkOrder.IsFinish = @IsFinish) and
				(@OrderState = 0 or tbl_WorkOrder.OrderState = @OrderState) and
				(@CreateUserId='00000000-0000-0000-0000-000000000000' or tbl_WorkOrder.CreateUserId = @CreateUserId)
		order by tbl_WorkOrder.OrderCode desc
END
GO

/****** Object:  StoredProcedure [dbo].[prc_ExportWorkApplyProjectsExcel]    Script Date: 2016-04-24 11:39:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportWorkApplyProjectsExcel]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@Title nvarchar(50),
					@ProjectCode nvarchar(50),
					@IsProject int,
					@SendUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_WorkApply.OrderCode as '单据编号',tbl_WorkApply.CreateDate as '申请日期',tbl_WorkApply.Title as '标题',tbl_WorkApply.ProjectCode as '项目编码',case tbl_WorkApply.IsProject when 1 then '是' when 2 then '否' else '' end as '是否立项',
	tbl_WorkApply.ApplyReason as '申请事由',reseauUser.FullName as '网格经理',(tbl_User.FullName+'/'+tbl_Department.DepartmentName) as '申请人'
		from tbl_WorkApply left join tbl_User on tbl_WorkApply.CreateUserId=tbl_User.Id
							left join tbl_User reseauUser on tbl_WorkApply.ReseauManagerId=reseauUser.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where (tbl_WorkApply.CreateDate >= @BeginDate and tbl_WorkApply.CreateDate < @EndDate) and
				(@Title = '' or CHARINDEX(@Title,tbl_WorkApply.Title) > 0) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_WorkApply.ProjectCode) > 0) and
				(@IsProject = 0 or tbl_WorkApply.IsProject = @IsProject) and
				tbl_WorkApply.OrderState = 3 and
				(tbl_WorkApply.ReseauManagerId = @SendUserId or @SendUserId='00000000-0000-0000-0000-000000000000')
		order by tbl_WorkApply.OrderCode desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryProjectCodeListPage]    Script Date: 2016-04-26 21:01:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectCodeListPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@ProjectCode nvarchar(50),
					@ProjectType int,
					@PlaceName nvarchar(50),
					@ReseauId uniqueidentifier,
					@ProjectManagerId uniqueidentifier,
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_ProjectCodeList.Id,tbl_ProjectCodeList.ProjectCode,tbl_ProjectCodeList.ProjectType,tbl_ProjectCodeList.ProjectDate,tbl_ProjectCodeList.PlaceName,tbl_Reseau.ReseauName,isnull(tbl_User.FullName,'') as FullName
		from tbl_ProjectCodeList left join tbl_Reseau on tbl_ProjectCodeList.ReseauId=tbl_Reseau.Id
							left join tbl_User on tbl_ProjectCodeList.ProjectManagerId=tbl_User.Id
		where (tbl_ProjectCodeList.ProjectDate >= @BeginDate and tbl_ProjectCodeList.ProjectDate < @EndDate) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectCodeList.ProjectCode) > 0) and
				(@ProjectType = 0 or tbl_ProjectCodeList.ProjectType = @ProjectType) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_ProjectCodeList.PlaceName) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.ReseauId = @ReseauId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.ProjectManagerId = @ProjectManagerId) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.CreateUserId = @CreateUserId)
		order by tbl_ProjectCodeList.CreateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_ProjectCodeList left join tbl_Reseau on tbl_ProjectCodeList.ReseauId=tbl_Reseau.Id
							left join tbl_User on tbl_ProjectCodeList.ProjectManagerId=tbl_User.Id
		where (tbl_ProjectCodeList.ProjectDate >= @BeginDate and tbl_ProjectCodeList.ProjectDate < @EndDate) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectCodeList.ProjectCode) > 0) and
				(@ProjectType = 0 or tbl_ProjectCodeList.ProjectType = @ProjectType) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_ProjectCodeList.PlaceName) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.ReseauId = @ReseauId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.ProjectManagerId = @ProjectManagerId) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.CreateUserId = @CreateUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryMaterialSpecListPage]    Script Date: 2016-04-26 21:02:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryMaterialSpecListPage]
					@PageIndex int,
					@PageSize int,
					@ProjectCode nvarchar(50),
					@CustomerName nvarchar(50),
					@MaterialSpecType int,
					@MaterialSpecName nvarchar(50),
					@OrderCode nvarchar(50),
					@CreateUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_MaterialSpecList.Id,tbl_MaterialSpecList.ProjectCode,tbl_MaterialSpecList.CustomerName,tbl_MaterialSpecList.MaterialSpecType,tbl_MaterialSpecList.MaterialSpecName,tbl_MaterialSpecList.UnitPrice,
		tbl_MaterialSpecList.SpecNumber,tbl_MaterialSpecList.TotalPrice,tbl_MaterialSpecList.OrderCode
		from tbl_MaterialSpecList 
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_MaterialSpecList.ProjectCode) > 0) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_MaterialSpecList.CustomerName) > 0) and
				(@MaterialSpecType = 0 or tbl_MaterialSpecList.MaterialSpecType = @MaterialSpecType) and
				(@MaterialSpecName = '' or CHARINDEX(@MaterialSpecName,tbl_MaterialSpecList.MaterialSpecName) > 0) and
				(@OrderCode = '' or CHARINDEX(@OrderCode,tbl_MaterialSpecList.OrderCode) > 0) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_MaterialSpecList.CreateUserId = @CreateUserId)
		order by tbl_MaterialSpecList.CreateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_MaterialSpecList 
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_MaterialSpecList.ProjectCode) > 0) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_MaterialSpecList.CustomerName) > 0) and
				(@MaterialSpecType = 0 or tbl_MaterialSpecList.MaterialSpecType = @MaterialSpecType) and
				(@MaterialSpecName = '' or CHARINDEX(@MaterialSpecName,tbl_MaterialSpecList.MaterialSpecName) > 0) and
				(@OrderCode = '' or CHARINDEX(@OrderCode,tbl_MaterialSpecList.OrderCode) > 0) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_MaterialSpecList.CreateUserId = @CreateUserId)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_QueryProjectCodeListAndMaterialSpecListPage]    Script Date: 2016-04-26 21:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectCodeListAndMaterialSpecListPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@ProjectCode nvarchar(50),
					@ProjectType int,
					@PlaceName nvarchar(50),
					@ReseauId uniqueidentifier,
					@ProjectManagerId uniqueidentifier,
					@CustomerName nvarchar(50),
					@MaterialSpecType int,
					@MaterialSpecName nvarchar(50),
					@OrderCode nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_ProjectCodeList.Id,tbl_ProjectCodeList.ProjectCode,tbl_ProjectCodeList.ProjectType,tbl_ProjectCodeList.ProjectDate,tbl_ProjectCodeList.PlaceName,tbl_Area.AreaName,
		tbl_Reseau.ReseauName,isnull(tbl_User.FullName,'') as FullName,tbl_MaterialSpecList.Id,tbl_MaterialSpecList.ProjectCode,tbl_MaterialSpecList.CustomerName,tbl_MaterialSpecList.MaterialSpecType,
		tbl_MaterialSpecList.MaterialSpecName,tbl_MaterialSpecList.UnitPrice,tbl_MaterialSpecList.SpecNumber,tbl_MaterialSpecList.TotalPrice,tbl_MaterialSpecList.OrderCode
		from tbl_MaterialSpecList left join tbl_ProjectCodeList on tbl_MaterialSpecList.ProjectCode=tbl_ProjectCodeList.ProjectCode
								left join tbl_Reseau on tbl_ProjectCodeList.ReseauId=tbl_Reseau.Id
								left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
								left join tbl_User on tbl_ProjectCodeList.ProjectManagerId=tbl_User.Id
		where (tbl_ProjectCodeList.ProjectDate >= @BeginDate and tbl_ProjectCodeList.ProjectDate < @EndDate) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectCodeList.ProjectCode) > 0) and
				(@ProjectType = 0 or tbl_ProjectCodeList.ProjectType = @ProjectType) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_ProjectCodeList.PlaceName) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.ReseauId = @ReseauId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.ProjectManagerId = @ProjectManagerId) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_MaterialSpecList.CustomerName) > 0) and
				(@MaterialSpecType = 0 or tbl_MaterialSpecList.MaterialSpecType = @MaterialSpecType) and
				(@MaterialSpecName = '' or CHARINDEX(@MaterialSpecName,tbl_MaterialSpecList.MaterialSpecName) > 0) and
				(@OrderCode = '' or CHARINDEX(@OrderCode,tbl_MaterialSpecList.OrderCode) > 0)
		order by tbl_MaterialSpecList.CreateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_MaterialSpecList left join tbl_ProjectCodeList on tbl_MaterialSpecList.ProjectCode=tbl_ProjectCodeList.ProjectCode
		where (tbl_ProjectCodeList.ProjectDate >= @BeginDate and tbl_ProjectCodeList.ProjectDate < @EndDate) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectCodeList.ProjectCode) > 0) and
				(@ProjectType = 0 or tbl_ProjectCodeList.ProjectType = @ProjectType) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_ProjectCodeList.PlaceName) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.ReseauId = @ReseauId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.ProjectManagerId = @ProjectManagerId) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_MaterialSpecList.CustomerName) > 0) and
				(@MaterialSpecType = 0 or tbl_MaterialSpecList.MaterialSpecType = @MaterialSpecType) and
				(@MaterialSpecName = '' or CHARINDEX(@MaterialSpecName,tbl_MaterialSpecList.MaterialSpecName) > 0) and
				(@OrderCode = '' or CHARINDEX(@OrderCode,tbl_MaterialSpecList.OrderCode) > 0)
END
GO

/****** Object:  StoredProcedure [dbo].[prc_ExportProjectMaterial]    Script Date: 2016-04-26 21:03:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportProjectMaterial]
					@BeginDate datetime,
					@EndDate datetime,
					@ProjectCode nvarchar(50),
					@ProjectType int,
					@PlaceName nvarchar(50),
					@ReseauId uniqueidentifier,
					@ProjectManagerId uniqueidentifier,
					@CustomerName nvarchar(50),
					@MaterialSpecType int,
					@MaterialSpecName nvarchar(50),
					@OrderCode nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_ProjectCodeList.ProjectCode as '立项编号',case tbl_ProjectCodeList.ProjectType when 1 then '新建' when 2 then '改造' when 3 then '维护改造' else '' end as '建设方式',tbl_ProjectCodeList.ProjectDate as '立项时间',
		tbl_ProjectCodeList.PlaceName as '站点名称',tbl_Area.AreaName as '区域',tbl_Reseau.ReseauName as '网格',isnull(tbl_User.FullName,'') as '项目经理',tbl_MaterialSpecList.CustomerName as '供应商',
		case tbl_MaterialSpecList.MaterialSpecType when 1 then '铁塔' when 2 then '美化外罩' when 3 then '电力线缆' when 4 then '室外机柜' when 5 then '蓄电池' when 6 then '开关电源' when 7 then '设计' when 8 then '监理' when 9 then '地勘' 
		when 10 then '桩基检测' when 11 then '土建' when 12 then '外电引入' when 13 then '施工' else '' end as '型号类别',tbl_MaterialSpecList.MaterialSpecName as '规格型号',tbl_MaterialSpecList.UnitPrice as '单价',
		tbl_MaterialSpecList.SpecNumber as '数量',tbl_MaterialSpecList.TotalPrice as '金额',tbl_MaterialSpecList.OrderCode as '订单编号'
		from tbl_MaterialSpecList left join tbl_ProjectCodeList on tbl_MaterialSpecList.ProjectCode=tbl_ProjectCodeList.ProjectCode
								left join tbl_Reseau on tbl_ProjectCodeList.ReseauId=tbl_Reseau.Id
								left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
								left join tbl_User on tbl_ProjectCodeList.ProjectManagerId=tbl_User.Id
		where (tbl_ProjectCodeList.ProjectDate >= @BeginDate and tbl_ProjectCodeList.ProjectDate < @EndDate) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectCodeList.ProjectCode) > 0) and
				(@ProjectType = 0 or tbl_ProjectCodeList.ProjectType = @ProjectType) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_ProjectCodeList.PlaceName) > 0) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.ReseauId = @ReseauId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectCodeList.ProjectManagerId = @ProjectManagerId) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_MaterialSpecList.CustomerName) > 0) and
				(@MaterialSpecType = 0 or tbl_MaterialSpecList.MaterialSpecType = @MaterialSpecType) and
				(@MaterialSpecName = '' or CHARINDEX(@MaterialSpecName,tbl_MaterialSpecList.MaterialSpecName) > 0) and
				(@OrderCode = '' or CHARINDEX(@OrderCode,tbl_MaterialSpecList.OrderCode) > 0)
		order by tbl_MaterialSpecList.CreateDate desc
END
GO

/****** Object:  StoredProcedure [dbo].[prc_GetTaskToDo]    Script Date: 2015/6/29 星期一 10:00:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetTaskToDo]
					@UserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @CompanyId uniqueidentifier,
			@DeptmentId uniqueidentifier
	select @DeptmentId=DepartmentId from tbl_User where Id=@UserId
	select @CompanyId=CompanyId from tbl_Department where Id=@DeptmentId
	
    -- Insert statements for procedure here
	declare @TempTask table(ProjectName nvarchar(100),ProfessionName nvarchar(10),TaskTypeName nvarchar(20),TaskCount int,PageUrl varchar(100),TypeIndex int,TaskIndex int)

	--运营商确认
	insert into @TempTask 
		select '','基站','运营商需求确认',count(tbl_OperatorsConfirmDetail.Id),'BaseStationBM/OperatorsConfirm',1,1 
			from tbl_OperatorsConfirmDetail
			where (@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and tbl_OperatorsConfirmDetail.MobileDemand = 1) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and tbl_OperatorsConfirmDetail.TelecomDemand = 1) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and tbl_OperatorsConfirmDetail.UnicomDemand = 1)

	--改造站运营商确认
	insert into @TempTask 
		select '','基站','改造站需求确认',count(tbl_OperatorsPlanningDemand.Id),'BaseStationBM/OperatorsPlanningDemand',1,2 
			from tbl_OperatorsPlanningDemand left join tbl_OperatorsPlanning on tbl_OperatorsPlanningDemand.OperatorsPlanningId=tbl_OperatorsPlanning.Id
			where tbl_OperatorsPlanning.CompanyId = @CompanyId and
					tbl_OperatorsPlanningDemand.Demand = 1

	--寻址确认
	insert into @TempTask 
		select '','基站','寻址确认',count(tbl_Planning.Id),'BaseStationBM/Addressing',1,3 
			from tbl_Planning
			where AddressingState=1 and
					Issued=1 and
					AddressingUserId=@UserId

	----新增基站建设
	--insert into @TempTask 
	--	select tbl_Project.ProjectName,'基站','新增基站建设',count(tbl_ConstructionTask.Id),'BaseStationBM/ConstructionPlanning',1,4 
	--		from tbl_ConstructionTask left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id
	--			left join tbl_Place on tbl_ConstructionTask.PlaceId=tbl_Place.Id
	--		where tbl_ConstructionTask.ProjectManagerId=@UserId and
	--			tbl_ConstructionTask.ConstructionMethod = 1 and
	--			tbl_ConstructionTask.ConstructionProgress <> 3 and
	--			tbl_ConstructionTask.ConstructionProgress <> 4 and
	--			tbl_Place.Profession = 1
	--			group by tbl_Project.ProjectName

	----改造基站建设
	--insert into @TempTask 
	--	select tbl_Project.ProjectName,'基站','改造基站建设',count(tbl_ConstructionTask.Id),'BaseStationBM/ConstructionRemodeing',1,5 
	--		from tbl_ConstructionTask left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id
	--			left join tbl_Place on tbl_ConstructionTask.PlaceId=tbl_Place.Id
	--		where tbl_ConstructionTask.ProjectManagerId=@UserId and
	--			tbl_ConstructionTask.ConstructionMethod = 2 and
	--			tbl_ConstructionTask.ConstructionProgress <> 3 and
	--			tbl_ConstructionTask.ConstructionProgress <> 4 and
	--			tbl_Place.Profession = 1
	--			group by tbl_Project.ProjectName

	--新增站安装登记
	insert into @TempTask 
		select '','基站','新增站安装登记',count(tbl_ConstructionTask.Id),'BaseStationBM/RegisterPlanning',1,6 
			from tbl_ConstructionTask left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id
				left join tbl_Planning on tbl_ConstructionTask.PlaceId=tbl_Planning.PlaceId
				left join tbl_Addressing on tbl_Planning.Id=tbl_Addressing.PlanningId
				left join tbl_Place on tbl_ConstructionTask.PlaceId=tbl_Place.Id
				--left join tbl_PlaceProperty on tbl_Place.Id=tbl_PlaceProperty.PlaceId
			where tbl_ConstructionTask.ConstructionProgress = 3 and
					((@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and tbl_Addressing.MobileShare=1) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and tbl_Addressing.TelecomShare=1) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and tbl_Addressing.UnicomShare=1)) and
					((@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and (tbl_ConstructionTask.IsFinishMobile=2)) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and (tbl_ConstructionTask.IsFinishTelecom=2)) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and (tbl_ConstructionTask.IsFinishUnicom=2))) and
					tbl_ConstructionTask.ConstructionMethod =1 and
					tbl_Place.Profession = 1

	--改造站安装登记
	insert into @TempTask 
		select '','基站','改造站安装登记',count(tbl_ConstructionTask.Id),'BaseStationBM/RegisterRemodeing',1,7
			from tbl_ConstructionTask left join tbl_Remodeling on tbl_ConstructionTask.PlaceId=tbl_Remodeling.PlaceId and tbl_ConstructionTask.ProjectId=tbl_Remodeling.ProjectId
				left join tbl_OperatorsSharing on tbl_Remodeling.Id=tbl_OperatorsSharing.RemodelingId
				left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id
				left join tbl_Place on tbl_ConstructionTask.PlaceId=tbl_Place.Id
				--left join tbl_PlaceProperty on tbl_Place.Id=tbl_PlaceProperty.PlaceId
			where tbl_OperatorsSharing.CompanyId=@CompanyId and
					tbl_ConstructionTask.ConstructionProgress = 3 and
					((@CompanyId = '6365F3DE-0FC5-4930-A321-2350EE6269BB' and (tbl_ConstructionTask.IsFinishMobile=2)) or
					(@CompanyId = '2E0FFE5F-C03A-4767-9915-9683F0DB0B53' and (tbl_ConstructionTask.IsFinishTelecom=2)) or
					(@CompanyId = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' and (tbl_ConstructionTask.IsFinishUnicom=2))) and
					tbl_ConstructionTask.ConstructionMethod =2 and
					tbl_Place.Profession = 1

	--项目管理
	insert into @TempTask
	select '','基站','项目管理',count(tbl_ConstructionTask.Id),'BaseStationBM/ProjectManagement',1,8
		from tbl_ConstructionTask left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
		where (tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and
				(tbl_ConstructionTask.SupervisorUserId=@UserId) and
				tbl_Place.Profession = 1

	--工程管理
	insert into @TempTask
	select '','基站','工程管理',count(tbl_TaskProperty.Id),'BaseStationBM/EngineeringManagement',1,9
		from tbl_TaskProperty left join tbl_ConstructionTask on tbl_TaskProperty.ConstructionTaskId=tbl_ConstructionTask.Id 
							left join tbl_Place on tbl_ConstructionTask.PlaceId = tbl_Place.Id
							left join tbl_Customer construction on tbl_TaskProperty.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervisor on tbl_TaskProperty.SupervisorCustomerId=supervisor.Id
		where  (tbl_TaskProperty.ConstructionProgress = 1 or tbl_TaskProperty.ConstructionProgress = 2) and
				(supervisor.CustomerUserId=@UserId or construction.CustomerUserId=@UserId) and
				tbl_Place.Profession = 1

	--派工申请
	insert into @TempTask
	select '','' as ProfessionName,'待处理隐患上报单',count(tbl_WorkApply.Id),'BaseStationBM/WorkApplyWait',1,10
		from tbl_WorkApply left join tbl_User on tbl_WorkApply.ReseauManagerId=tbl_User.Id
							left join tbl_WorkOrder on tbl_WorkApply.WorkOrderId=tbl_WorkOrder.Id
		where tbl_WorkApply.ReseauManagerId = @UserId and
				tbl_WorkApply.OrderState = 3 and
				tbl_WorkApply.IsSoved=2 and
				tbl_WorkApply.WorkOrderId='00000000-0000-0000-0000-000000000000'

	--待立项隐患上报单
	insert into @TempTask
	select '','' as ProfessionName,'待立项隐患上报单',count(tbl_WorkApply.Id),'BaseStationBM/WorkApplyProject',1,11
		from tbl_WorkApply left join tbl_User on tbl_WorkApply.ReseauManagerId=tbl_User.Id
							left join tbl_WorkOrder on tbl_WorkApply.WorkOrderId=tbl_WorkOrder.Id
		where tbl_WorkApply.ReseauManagerId = @UserId and
				tbl_WorkApply.OrderState = 3 and
				tbl_WorkApply.IsProject=2

	select * from @TempTask where TaskCount>0
END
GO