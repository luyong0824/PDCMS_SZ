
/****** Object:  StoredProcedure [dbo].[prc_QueryDutyUsers]    Script Date: 2016-12-13 21:17:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryDutyUsers]
							@CompanyId uniqueidentifier,
							@DepartmentId uniqueidentifier,
							@Duty int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_Department.Id,tbl_Department.DepartmentName as Name,tbl_Department.CompanyId as PId,
			'00000000-0000-0000-0000-000000000000' as DutyUserId,tbl_Department.DepartmentCode as Code,'false' as IsLeafStr,'false' as AsyncLoadStr
		from tbl_Department
		where tbl_Department.CompanyId = @CompanyId and tbl_Department.[State] = 1 and
			(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_Department.Id = @DepartmentId)
	union
	select tbl_User.Id,tbl_User.UserName + '(' + tbl_User.FullName + ')',tbl_User.DepartmentId,ISNULL(tbl_DutyUser.Id,'00000000-0000-0000-0000-000000000000'),
			'','true','false'
		from tbl_User left join tbl_Department on tbl_User.DepartmentId = tbl_Department.Id
					left join tbl_DutyUser on tbl_User.Id = tbl_DutyUser.UserId and tbl_DutyUser.Duty = @Duty
		where tbl_Department.CompanyId = @CompanyId and tbl_User.[State] = 1 and tbl_Department.[State] = 1 and
			(@DepartmentId = '00000000-0000-0000-0000-000000000000' or tbl_Department.Id = @DepartmentId)
	union
	select Id,CompanyName,'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','','false','false'
		from tbl_Company
		where Id = @CompanyId and tbl_Company.[State] = 1
	order by Code,Name
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetPlanningAndPlaceByIds]    Script Date: 2016-12-11 22:19:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetPlanningAndPlaceByIds]
					@PlanningIdsSql varchar(max),
					@PlaceIdsSql varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @TempPlanning table(value uniqueidentifier);
	insert @TempPlanning exec(@PlanningIdsSql);

	declare @TempPlace table(value uniqueidentifier);
	insert @TempPlace exec(@PlaceIdsSql);

	declare @temp table(Id uniqueidentifier,PlaceName varchar(50),AreaName varchar(50),ReseauName varchar(50),PlaceCategoryName varchar(50),
		Lng decimal(18,5),Lat decimal(18,5),CompanyName varchar(50),DataType int,Profession int,CompanyId uniqueidentifier,Issued int,AddressingStateName varchar(50),
		PlaceOwner uniqueidentifier,OwnerName varchar(50),OwnerContact varchar(50),OwnerPhoneNumber varchar(50),PlaceOwnerName varchar(50),G2Number varchar(50),
		D2Number varchar(50),G3Number varchar(50),G4Number varchar(50),NetWorks varchar(50),PlaceMapState int)

	insert @temp
		select tbl_Planning.Id,tbl_Planning.PlanningName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Planning.Lng,tbl_Planning.Lat,'苏州联通' as CompanyName,1 as DataType,tbl_Planning.Profession,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' as CompanyId,
			tbl_Planning.Issued,case tbl_Planning.AddressingState when 1 then '未寻址确认' when 2 then '已寻址确认' when 3 then '流转中' when 4 then '流程终止' else '' end as AddressingStateName,
			'00000000-0000-0000-0000-000000000000','','','','','','','','','',0
		from tbl_Planning left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
			left join tbl_Reseau on tbl_Planning.ReseauId=tbl_Reseau.Id
			left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
		where tbl_Planning.Id in (select value from @TempPlanning)
		order by tbl_Planning.PlanningCode

	insert @temp
		select tbl_Place.Id,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Place.Lng,tbl_Place.Lat,'苏州联通' as CompanyName,2 as DataType,tbl_Place.Profession,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' as CompanyId,
			0,'',tbl_Place.PlaceOwner,tbl_Place.OwnerName,tbl_Place.OwnerContact,tbl_Place.OwnerPhoneNumber,tbl_PlaceOwner.PlaceOwnerName,tbl_Place.G2Number,
			tbl_Place.D2Number,tbl_Place.G3Number,tbl_Place.G4Number,'' as NetWorks,tbl_Place.PlaceMapState
			from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_PlaceOwner on tbl_Place.PlaceOwner = tbl_PlaceOwner.Id
			where tbl_Place.Id in (select value from @TempPlace)
			order by tbl_Place.PlaceCode

	select * from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetPlanningsPageMobile]    Script Date: 2016-12-08 13:41:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetPlanningsPageMobile]
					@PageIndex int,
					@PageSize int,
					@ProfessionListSql varchar(max),
					@Lng decimal(18,5),
					@Lat decimal(18,5),
					@Distance decimal(18,5),
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @TempProfessionList table(value int);
	insert @TempProfessionList exec(@ProfessionListSql);

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,ProfessionName nvarchar(50),PlanningName nvarchar(50),Lng decimal(18,5),Lat decimal(18,5),Distance decimal(18,5));
	with cte as
	(
		select tbl_Planning.Id,case tbl_Planning.Profession when 1 then '基站' when 2 then '室分' else '' end as ProfessionName,tbl_Planning.PlanningName,tbl_Planning.Lng,tbl_Planning.Lat,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Planning.Lng,tbl_Planning.Lat) as Distance
			from tbl_Planning left join tbl_User on tbl_Planning.CreateUserId = tbl_User.Id
						left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
						left join @TempProfessionList t on tbl_Planning.Profession = t.value
		where tbl_Department.CompanyId=@CompanyId and
				t.value is not null
	)
	insert @temp
		select cte.Id,cte.ProfessionName,cte.PlanningName,cte.Lng,cte.Lat,cte.Distance
		from cte
		where cte.Distance <= @Distance;


	set @PageIndex=@PageIndex-1

	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select * from @temp t order by t.Distance offset @PageStart row fetch next @PageSize rows only

	select COUNT(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetPlacesPageMobile]    Script Date: 2016-12-08 13:41:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetPlacesPageMobile]
					@PageIndex int,
					@PageSize int,
					@ProfessionListSql varchar(max),
					@Lng decimal(18,5),
					@Lat decimal(18,5),
					@Distance decimal(18,5),
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @TempProfessionList table(value int);
	insert @TempProfessionList exec(@ProfessionListSql);

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,ProfessionName nvarchar(50),PlaceName nvarchar(50),Lng decimal(18,5),Lat decimal(18,5),Distance decimal(18,5));
	with cte as
	(
		select tbl_Place.Id,case tbl_Place.Profession when 1 then '基站' when 2 then '室分' else '' end as ProfessionName,tbl_Place.PlaceName,tbl_Place.Lng,tbl_Place.Lat,
			dbo.func_GetDistance(@Lng,@Lat,tbl_Place.Lng,tbl_Place.Lat) as Distance
		from tbl_Place left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
						left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
						left join @TempProfessionList t on tbl_Place.Profession = t.value
		where tbl_Place.[State] = 1 and tbl_Department.CompanyId=@CompanyId and
				t.value is not null
	)
	insert @temp
		select cte.Id,cte.ProfessionName,cte.PlaceName,cte.Lng,cte.Lat,cte.Distance
		from cte
		where cte.Distance <= @Distance;

	set @PageIndex=@PageIndex-1

	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select * from @temp t order by t.Distance offset @PageStart row fetch next @PageSize rows only

	select COUNT(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetPlanningsMobile]    Script Date: 2016-12-08 09:24:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetPlanningsMobile]
					@PageIndex int,
					@PageSize int,
					@ProfessionListSql varchar(max),
					@PlanningName nvarchar(50),
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @TempProfessionList table(value int);
	insert @TempProfessionList exec(@ProfessionListSql);

	set @PageIndex=@PageIndex-1

	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Planning.Id,case tbl_Planning.Profession when 1 then '基站' when 2 then '室分' else '' end as ProfessionName,tbl_Planning.PlanningName,tbl_Planning.Lng,tbl_Planning.Lat
		from tbl_Planning left join tbl_User on tbl_Planning.CreateUserId = tbl_User.Id
						left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
						left join @TempProfessionList t on tbl_Planning.Profession = t.value
		where CHARINDEX(@PlanningName,tbl_Planning.PlanningName)>0 and
				tbl_Department.CompanyId=@CompanyId and
				t.value is not null
		order by tbl_Planning.PlanningCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Planning left join tbl_User on tbl_Planning.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join @TempProfessionList t on tbl_Planning.Profession = t.value
		where CHARINDEX(@PlanningName,tbl_Planning.PlanningName)>0 and
				tbl_Department.CompanyId=@CompanyId and
				t.value is not null
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetNearbyPlanningsAndPlacesListMobile]    Script Date: 2016-12-04 10:58:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetNearbyPlanningsAndPlacesListMobile]
					@ProfessionListSql varchar(max),
					@Lng decimal(18,5),
					@Lat decimal(18,5),
					@Distance decimal(18,5),
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @CompanyName nvarchar(100)
	select @CompanyName = CompanyName from tbl_Company where Id = @CompanyId;

	declare @TempProfessionList table(value int);
	insert @TempProfessionList exec(@ProfessionListSql);
	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,Profession int,ProfessionName nvarchar(50),PlaceName nvarchar(50),TypeName nvarchar(10),TypeId int,Distance decimal(18,5));

	with cte as
	(
		select tbl_Planning.Id,tbl_Planning.Profession,case tbl_Planning.Profession when 1 then '基站' when 2 then '室分' else '' end as ProfessionName,tbl_Planning.PlanningName as PlaceName,'规划点' as TypeName,1 as TypeId,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Planning.Lng,tbl_Planning.Lat) as Distance
			from tbl_Planning left join tbl_User on tbl_Planning.CreateUserId=tbl_User.Id
				left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id 
			where tbl_Planning.AddressingState <> 2 and tbl_Department.CompanyId=@CompanyId and tbl_Planning.Profession in (select value from @TempProfessionList)
	)
	insert @temp
		select cte.Id,cte.Profession,cte.ProfessionName,cte.PlaceName,cte.TypeName,cte.TypeId,cte.Distance
		from cte
		where cte.Distance <= @Distance;
		
	with cte as
	(
		select tbl_Place.Id,tbl_Place.Profession,case tbl_Place.Profession when 1 then '基站' when 2 then '室分' else '' end as ProfessionName,tbl_Place.PlaceName,'已有点' as TypeName,2 as TypeId,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Place.Lng,tbl_Place.Lat) as Distance
			from tbl_Place left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
				left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
			where tbl_Place.[State] = 1 and tbl_Department.CompanyId=@CompanyId and tbl_Place.Profession in (select value from @TempProfessionList)
	)
	insert @temp
		select cte.Id,cte.Profession,cte.ProfessionName,cte.PlaceName,cte.TypeName,cte.TypeId,cte.Distance
		from cte
		where cte.Distance <= @Distance;

	select * from @temp order by Distance
	select distinct Profession from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetPlacesMobile]    Script Date: 2016-12-03 12:34:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetPlacesMobile]
					@PageIndex int,
					@PageSize int,
					@ProfessionListSql varchar(max),
					@PlaceName nvarchar(50),
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @TempProfessionList table(value int);
	insert @TempProfessionList exec(@ProfessionListSql);

	set @PageIndex=@PageIndex-1

	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Place.Id,case tbl_Place.Profession when 1 then '基站' when 2 then '室分' else '' end as ProfessionName,tbl_Place.PlaceName,tbl_Place.Lng,tbl_Place.Lat
		from tbl_Place left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
						left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
						left join @TempProfessionList t on tbl_Place.Profession = t.value
		where CHARINDEX(@PlaceName,tbl_Place.PlaceName)>0 and
				tbl_Place.[State] = 1 and tbl_Department.CompanyId=@CompanyId and
				t.value is not null
		order by tbl_Place.PlaceCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Place left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join @TempProfessionList t on tbl_Place.Profession = t.value
		where CHARINDEX(@PlaceName,tbl_Place.PlaceName)>0 and
				tbl_Place.[State] = 1 and tbl_Department.CompanyId=@CompanyId and
				t.value is not null
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportAddressingMonthUserExcel]    Script Date: 2016-11-28 19:50:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportAddressingMonthUserExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@DepartmentId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(年月 nvarchar(6),租赁部门 varchar(50),租赁人 varchar(50),
		A类完成数量 int,B类完成数量 int,C类完成数量 int,租赁得分 int)

	declare @DepartmentName varchar(50),
			@FullName varchar(50),
			@AddressingUserId uniqueidentifier,
			@ACount int,
			@BCount int,
			@CCount int,
			@AddressingScore int
	declare cur cursor for
	select distinct tbl_Department.DepartmentName,tbl_User.FullName,tbl_Planning.AddressingUserId 
		from tbl_Planning left join tbl_User on tbl_Planning.AddressingUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where tbl_Planning.AddressingUserId<>'00000000-0000-0000-0000-000000000000' and
				(tbl_Department.Id=@DepartmentId or @DepartmentId='00000000-0000-0000-0000-000000000000') and
				tbl_Planning.AddressingState=2 and tbl_Planning.Profession=@Profession and tbl_Department.CompanyId=@CompanyId
	open cur
	fetch next from cur into @DepartmentName,@FullName,@AddressingUserId
	while @@fetch_status = 0
	begin
		set @ACount=0
		set @BCount=0
		set	@CCount=0
		set	@AddressingScore=0

		select @ACount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=1 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.AddressingUserId=@AddressingUserId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		select @BCount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=2 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.AddressingUserId=@AddressingUserId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		select @CCount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=3 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.AddressingUserId=@AddressingUserId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		set @AddressingScore=@ACount*3+@BCount*2+@CCount

		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@DepartmentName,@FullName,@ACount,@BCount,@CCount,@AddressingScore)

		fetch next from cur into @DepartmentName,@FullName,@AddressingUserId
	end
	close cur
	deallocate cur
	select * from @temp order by 租赁部门,租赁人
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportAddressingMonthReseauExcel]    Script Date: 2016-11-28 19:50:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportAddressingMonthReseauExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(年月 nvarchar(6),区域 varchar(50),网格 varchar(50),
		A类完成数量 int,B类完成数量 int,C类完成数量 int,租赁得分 int)

	declare @AreaName varchar(50),
			@ReseauName varchar(50),
			@ReseauId uniqueidentifier,
			@ACount int,
			@BCount int,
			@CCount int,
			@AddressingScore int
	declare cur cursor for
	select distinct tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Reseau.Id 
		from tbl_Reseau left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
						left join tbl_User on tbl_Reseau.CreateUserId=tbl_User.Id
						left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId=@AreaId) and tbl_Department.CompanyId=@CompanyId
	open cur
	fetch next from cur into @AreaName,@ReseauName,@ReseauId
	while @@fetch_status = 0
	begin
		set @ACount=0
		set @BCount=0
		set	@CCount=0
		set	@AddressingScore=0

		select @ACount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=1 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.ReseauId=@ReseauId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		select @BCount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=2 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.ReseauId=@ReseauId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		select @CCount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=3 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.ReseauId=@ReseauId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		set @AddressingScore=@ACount*3+@BCount*2+@CCount

		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@AreaName,@ReseauName,@ACount,@BCount,@CCount,@AddressingScore)

		fetch next from cur into @AreaName,@ReseauName,@ReseauId
	end
	close cur
	deallocate cur
	select * from @temp order by 区域,网格
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportProjectTaskDepartmentExcel]    Script Date: 2016-11-28 19:34:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportProjectTaskDepartmentExcel]
					@BeginDate datetime,
					@BeginDateYear datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(年月 nvarchar(6),部门名称 varchar(50),
		本月按时完成数量 int,本月超时完成数量 int,年内项目总数 int,年内按时完成数量 int,年内超时完成数量 int,年内未完成数量 int,
		年内完成率 decimal(18,2),月度得分 int,年度得分 decimal(18,2))

	declare @DepartmentId uniqueidentifier,
			@DepartmentName varchar(50),
			@MonthOnTimeCount int,
			@MonthOverTimeCount int,
			@TotalCount int,
			@YearOnTimeCount int,
			@YearOverTimeCount int,
			@YearUnFinishCount int,
			@YearOnTimeRate decimal(18,2),
			@MonthScore int,
			@YearScore decimal(18,2)
	declare cur cursor for
	select distinct tbl_Department.Id,tbl_Department.DepartmentName 
		from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
		where tbl_ProjectTask.AreaManagerId<>'00000000-0000-0000-0000-000000000000' and 
				tbl_ProjectTask.ProjectCode<>'' and tbl_Place.Profession=@Profession and tbl_Department.CompanyId=@CompanyId
	open cur
	fetch next from cur into @DepartmentId,@DepartmentName
	while @@fetch_status = 0
	begin
		set @MonthOnTimeCount=0
		set @MonthOverTimeCount=0
		set	@TotalCount=0
		set	@YearOnTimeCount=0
		set	@YearOverTimeCount=0
		set	@YearUnFinishCount=0
		set	@YearOnTimeRate=0
		set	@MonthScore=0
		set	@YearScore=0

		select @MonthOnTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
																		left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and		
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectDate>@BeginDate and 
			tbl_ProjectTask.ProjectDate<DATEADD(MONTH,1,@BeginDate) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30

		select @MonthOverTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id 
																		left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and 
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectDate>@BeginDate and 
			tbl_ProjectTask.ProjectDate<DATEADD(MONTH,1,@BeginDate) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30

		select @TotalCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
																left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id 
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear)

		select @YearOnTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
																		left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30

		select @YearOverTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
																		left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30

		select @YearUnFinishCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
																		left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate='2000-01-01' and
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear)

		if @TotalCount<>0
		begin
			set @YearOnTimeRate=(@YearOnTimeCount+@YearOverTimeCount)*1.0/@TotalCount*100
		end
		else
		begin
			set @YearOnTimeRate=0
		end

		set @MonthScore=@MonthOnTimeCount*2+@MonthOverTimeCount
		set @YearScore=(@YearOnTimeCount*2+@YearOverTimeCount)*(@YearOnTimeRate)/(100+0.0)

		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@DepartmentName,@MonthOnTimeCount,@MonthOverTimeCount,@TotalCount,@YearOnTimeCount,
			@YearOverTimeCount,@YearUnFinishCount,@YearOnTimeRate,@MonthScore,@YearScore)

		fetch next from cur into @DepartmentId,@DepartmentName
	end
	close cur
	deallocate cur
	select * from @temp order by 部门名称
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportProjectTaskProjectManagerExcel]    Script Date: 2016-11-28 19:26:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportProjectTaskProjectManagerExcel]
					@BeginDate datetime,
					@BeginDateYear datetime,
					@DepartmentId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(年月 nvarchar(6),部门名称 varchar(50),项目经理 varchar(50),
		本月暗示完成数量 int,本月超时完成数量 int,年内项目总数 int,年内按时完成数量 int,年内超时完成数量 int,年内未完成数量 int,
		年内完成率 decimal(18,2),月度得分 int,年度得分 decimal(18,2))

	declare @DepartmentName varchar(50),
			@FullName varchar(50),
			@AreaManagerId uniqueidentifier,
			@MonthOnTimeCount int,
			@MonthOverTimeCount int,
			@TotalCount int,
			@YearOnTimeCount int,
			@YearOverTimeCount int,
			@YearUnFinishCount int,
			@YearOnTimeRate decimal(18,2),
			@MonthScore int,
			@YearScore decimal(18,2)
	declare cur cursor for
	select distinct tbl_Department.DepartmentName,tbl_User.FullName,tbl_ProjectTask.AreaManagerId 
		from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id			 
		where tbl_ProjectTask.AreaManagerId<>'00000000-0000-0000-0000-000000000000' and
				(tbl_Department.Id=@DepartmentId or @DepartmentId='00000000-0000-0000-0000-000000000000') and
				tbl_ProjectTask.ProjectCode<>'' and tbl_Place.Profession=@Profession and tbl_Department.CompanyId=@CompanyId
	open cur
	fetch next from cur into @DepartmentName,@FullName,@AreaManagerId
	while @@fetch_status = 0
	begin
		set @MonthOnTimeCount=0
		set @MonthOverTimeCount=0
		set	@TotalCount=0
		set	@YearOnTimeCount=0
		set	@YearOverTimeCount=0
		set	@YearUnFinishCount=0
		set	@YearOnTimeRate=0
		set	@MonthScore=0
		set	@YearScore=0

		select @MonthOnTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and		
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectDate>@BeginDate and 
			tbl_ProjectTask.ProjectDate<DATEADD(MONTH,1,@BeginDate) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30

		select @MonthOverTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and 
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectDate>@BeginDate and 
			tbl_ProjectTask.ProjectDate<DATEADD(MONTH,1,@BeginDate) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30

		select @TotalCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear)

		select @YearOnTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30

		select @YearOverTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30

		select @YearUnFinishCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate='2000-01-01' and
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear)

		if @TotalCount<>0
		begin
			set @YearOnTimeRate=(@YearOnTimeCount+@YearOverTimeCount)*1.0/@TotalCount*100
		end
		else
		begin
			set @YearOnTimeRate=0
		end

		set @MonthScore=@MonthOnTimeCount*2+@MonthOverTimeCount
		set @YearScore=(@YearOnTimeCount*2+@YearOverTimeCount)*(@YearOnTimeRate)/(100+0.0)

		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@DepartmentName,@FullName,@MonthOnTimeCount,@MonthOverTimeCount,@TotalCount,@YearOnTimeCount,
			@YearOverTimeCount,@YearUnFinishCount,@YearOnTimeRate,@MonthScore,@YearScore)

		fetch next from cur into @DepartmentName,@FullName,@AreaManagerId
	end
	close cur
	deallocate cur
	select * from @temp order by 部门名称,项目经理
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeYearGrowthCompanyExcel]    Script Date: 2016-11-28 16:36:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeYearGrowthCompanyExcel]
					@BeginDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @tempTV table(公司 varchar(50),
		Jan decimal(18,2),Feb decimal(18,2),Mar decimal(18,2),Apr decimal(18,2),May decimal(18,2),
		Jun decimal(18,2),Jul decimal(18,2),Aug decimal(18,2),Sept decimal(18,2),Oct decimal(18,2),
		Nov decimal(18,2),Dec decimal(18,2))

	declare @tempBV table(公司 varchar(50),
		Jan decimal(18,2),Feb decimal(18,2),Mar decimal(18,2),Apr decimal(18,2),May decimal(18,2),
		Jun decimal(18,2),Jul decimal(18,2),Aug decimal(18,2),Sept decimal(18,2),Oct decimal(18,2),
		Nov decimal(18,2),Dec decimal(18,2))

	declare @CPId uniqueidentifier,
			@CompanyName varchar(50),
			@JanTV decimal(18,3),
			@JanBV decimal(18,3),
			@FebTV decimal(18,3),
			@FebBV decimal(18,3),
			@MarTV decimal(18,3),
			@MarBV decimal(18,3),
			@AprTV decimal(18,3),
			@AprBV decimal(18,3),
			@MayTV decimal(18,3),
			@MayBV decimal(18,3),
			@JunTV decimal(18,3),
			@JunBV decimal(18,3),
			@JulTV decimal(18,3),
			@JulBV decimal(18,3),
			@AugTV decimal(18,3),
			@AugBV decimal(18,3),
			@SeptTV decimal(18,3),
			@SeptBV decimal(18,3),
			@OctTV decimal(18,3),
			@OctBV decimal(18,3),
			@NovTV decimal(18,3),
			@NovBV decimal(18,3),
			@DecTV decimal(18,3),
			@DecBV decimal(18,3)
	declare cur cursor for
	select tbl_Company.Id,tbl_Company.CompanyName from tbl_Company
		where tbl_Company.Id=@CompanyId order by tbl_Company.CompanyCode
							
	open cur
	fetch next from cur into @CPId,@CompanyName
	while @@fetch_status = 0
	begin
		set @JanTV=0
		set @JanBV=0
		set @FebTV=0
		set @FebBV=0
		set @MarTV=0
		set @MarBV=0
		set @AprTV=0
		set @AprBV=0
		set @MayTV=0
		set @MayBV=0
		set	@JunTV=0
		set @JunBV=0
		set @JulTV=0
		set @JulBV=0
		set @AugTV=0
		set @AugBV=0
		set @SeptTV=0
		set @SeptBV=0
		set @OctTV=0
		set @OctBV=0
		set @NovTV=0
		set @NovBV=0
		set @DecTV=0
		set @DecBV=0
		select @JanTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JanBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,1,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @FebTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@FebBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,2,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MarTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MarBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,2,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,3,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AprTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AprBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,3,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,4,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MayTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MayBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,4,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,5,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JunTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JunBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,5,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,6,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JulTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JulBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,6,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,7,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AugTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AugBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,7,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,8,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @SeptTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@SeptBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,8,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,9,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @OctTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@OctBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,9,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,10,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @NovTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@NovBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,10,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,11,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @DecTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@DecBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,11,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,12,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		insert @tempTV values(@CompanyName,@JanTV/10000,@FebTV/10000,@MarTV/10000,@AprTV/10000,@MayTV/10000,
			@JunTV/10000,@JulTV/10000,@AugTV/10000,@SeptTV/10000,@OctTV/10000,@NovTV/10000,@DecTV/10000)
		
		insert @tempBV values(@CompanyName,@JanBV/1000,@FebBV/1000,@MarBV/1000,@AprBV/1000,@MayBV/1000,
			@JunBV/1000,@JulBV/1000,@AugBV/1000,@SeptBV/1000,@OctBV/1000,@NovBV/1000,@DecBV/1000)
		
		fetch next from cur into @CPId,@CompanyName
	end
	close cur
	deallocate cur

	select * from @tempTV
	select * from @tempBV
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeYearGrowthAreaExcel]    Script Date: 2016-11-28 16:33:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeYearGrowthAreaExcel]
					@BeginDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @tempTV table(区域 varchar(50),
		Jan decimal(18,2),Feb decimal(18,2),Mar decimal(18,2),Apr decimal(18,2),May decimal(18,2),
		Jun decimal(18,2),Jul decimal(18,2),Aug decimal(18,2),Sept decimal(18,2),Oct decimal(18,2),
		Nov decimal(18,2),Dec decimal(18,2))

	declare @tempBV table(区域 varchar(50),
		Jan decimal(18,2),Feb decimal(18,2),Mar decimal(18,2),Apr decimal(18,2),May decimal(18,2),
		Jun decimal(18,2),Jul decimal(18,2),Aug decimal(18,2),Sept decimal(18,2),Oct decimal(18,2),
		Nov decimal(18,2),Dec decimal(18,2))

	declare @AreaId uniqueidentifier,
			@AreaName varchar(50),
			@JanTV decimal(18,3),
			@JanBV decimal(18,3),
			@FebTV decimal(18,3),
			@FebBV decimal(18,3),
			@MarTV decimal(18,3),
			@MarBV decimal(18,3),
			@AprTV decimal(18,3),
			@AprBV decimal(18,3),
			@MayTV decimal(18,3),
			@MayBV decimal(18,3),
			@JunTV decimal(18,3),
			@JunBV decimal(18,3),
			@JulTV decimal(18,3),
			@JulBV decimal(18,3),
			@AugTV decimal(18,3),
			@AugBV decimal(18,3),
			@SeptTV decimal(18,3),
			@SeptBV decimal(18,3),
			@OctTV decimal(18,3),
			@OctBV decimal(18,3),
			@NovTV decimal(18,3),
			@NovBV decimal(18,3),
			@DecTV decimal(18,3),
			@DecBV decimal(18,3)
	declare cur cursor for
	select tbl_Area.Id,tbl_Area.AreaName from tbl_Area left join tbl_User on tbl_Area.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where tbl_Department.CompanyId=@CompanyId order by tbl_Area.AreaCode
							
	open cur
	fetch next from cur into @AreaId,@AreaName
	while @@fetch_status = 0
	begin
		set @JanTV=0
		set @JanBV=0
		set @FebTV=0
		set @FebBV=0
		set @MarTV=0
		set @MarBV=0
		set @AprTV=0
		set @AprBV=0
		set @MayTV=0
		set @MayBV=0
		set	@JunTV=0
		set @JunBV=0
		set @JulTV=0
		set @JulBV=0
		set @AugTV=0
		set @AugBV=0
		set @SeptTV=0
		set @SeptBV=0
		set @OctTV=0
		set @OctBV=0
		set @NovTV=0
		set @NovBV=0
		set @DecTV=0
		set @DecBV=0
		select @JanTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JanBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,1,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @FebTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@FebBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,2,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MarTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MarBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,2,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,3,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AprTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AprBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,3,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,4,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MayTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MayBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,4,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,5,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JunTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JunBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,5,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,6,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JulTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JulBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,6,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,7,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AugTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AugBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,7,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,8,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @SeptTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@SeptBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,8,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,9,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @OctTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@OctBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,9,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,10,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @NovTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@NovBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,10,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,11,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @DecTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@DecBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,11,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,12,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		insert @tempTV values(@AreaName,@JanTV/10000,@FebTV/10000,@MarTV/10000,@AprTV/10000,@MayTV/10000,
			@JunTV/10000,@JulTV/10000,@AugTV/10000,@SeptTV/10000,@OctTV/10000,@NovTV/10000,@DecTV/10000)
		
		insert @tempBV values(@AreaName,@JanBV/1000,@FebBV/1000,@MarBV/1000,@AprBV/1000,@MayBV/1000,
			@JunBV/1000,@JulBV/1000,@AugBV/1000,@SeptBV/1000,@OctBV/1000,@NovBV/1000,@DecBV/1000)
		
		fetch next from cur into @AreaId,@AreaName
	end
	close cur
	deallocate cur

	select * from @tempTV
	select * from @tempBV
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeYearGrowthReseauExcel]    Script Date: 2016-11-28 16:26:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeYearGrowthReseauExcel]
					@BeginDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @tempTV table(区域 varchar(50),网格 varchar(50),
		Jan decimal(18,2),Feb decimal(18,2),Mar decimal(18,2),Apr decimal(18,2),May decimal(18,2),
		Jun decimal(18,2),Jul decimal(18,2),Aug decimal(18,2),Sept decimal(18,2),Oct decimal(18,2),
		Nov decimal(18,2),Dec decimal(18,2))

	declare @tempBV table(区域 varchar(50),网格 varchar(50),
		Jan decimal(18,2),Feb decimal(18,2),Mar decimal(18,2),Apr decimal(18,2),May decimal(18,2),
		Jun decimal(18,2),Jul decimal(18,2),Aug decimal(18,2),Sept decimal(18,2),Oct decimal(18,2),
		Nov decimal(18,2),Dec decimal(18,2))

	declare @ReseauId uniqueidentifier,
			@ReseauName varchar(50),
			@AreaName varchar(50),
			@JanTV decimal(18,3),
			@JanBV decimal(18,3),
			@FebTV decimal(18,3),
			@FebBV decimal(18,3),
			@MarTV decimal(18,3),
			@MarBV decimal(18,3),
			@AprTV decimal(18,3),
			@AprBV decimal(18,3),
			@MayTV decimal(18,3),
			@MayBV decimal(18,3),
			@JunTV decimal(18,3),
			@JunBV decimal(18,3),
			@JulTV decimal(18,3),
			@JulBV decimal(18,3),
			@AugTV decimal(18,3),
			@AugBV decimal(18,3),
			@SeptTV decimal(18,3),
			@SeptBV decimal(18,3),
			@OctTV decimal(18,3),
			@OctBV decimal(18,3),
			@NovTV decimal(18,3),
			@NovBV decimal(18,3),
			@DecTV decimal(18,3),
			@DecBV decimal(18,3)
	declare cur cursor for
	select tbl_Reseau.Id,tbl_Reseau.ReseauName,tbl_Area.AreaName from tbl_Reseau left join tbl_User on tbl_Reseau.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
		where tbl_Department.CompanyId=@CompanyId order by tbl_Reseau.ReseauCode
							
	open cur
	fetch next from cur into @ReseauId,@ReseauName,@AreaName
	while @@fetch_status = 0
	begin
		set @JanTV=0
		set @JanBV=0
		set @FebTV=0
		set @FebBV=0
		set @MarTV=0
		set @MarBV=0
		set @AprTV=0
		set @AprBV=0
		set @MayTV=0
		set @MayBV=0
		set	@JunTV=0
		set @JunBV=0
		set @JulTV=0
		set @JulBV=0
		set @AugTV=0
		set @AugBV=0
		set @SeptTV=0
		set @SeptBV=0
		set @OctTV=0
		set @OctBV=0
		set @NovTV=0
		set @NovBV=0
		set @DecTV=0
		set @DecBV=0
		select @JanTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JanBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,1,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @FebTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@FebBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,2,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MarTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MarBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,2,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,3,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AprTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AprBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,3,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,4,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MayTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MayBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,4,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,5,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JunTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JunBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,5,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,6,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JulTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JulBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,6,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,7,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AugTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AugBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,7,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,8,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @SeptTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@SeptBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,8,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,9,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @OctTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@OctBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,9,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,10,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @NovTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@NovBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,10,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,11,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @DecTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@DecBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,11,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,12,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		insert @tempTV values(@AreaName,@ReseauName,@JanTV/10000,@FebTV/10000,@MarTV/10000,@AprTV/10000,@MayTV/10000,
			@JunTV/10000,@JulTV/10000,@AugTV/10000,@SeptTV/10000,@OctTV/10000,@NovTV/10000,@DecTV/10000)
		
		insert @tempBV values(@AreaName,@ReseauName,@JanBV/1000,@FebBV/1000,@MarBV/1000,@AprBV/1000,@MayBV/1000,
			@JunBV/1000,@JulBV/1000,@AugBV/1000,@SeptBV/1000,@OctBV/1000,@NovBV/1000,@DecBV/1000)
		
		fetch next from cur into @ReseauId,@ReseauName,@AreaName
	end
	close cur
	deallocate cur

	select * from @tempTV
	select * from @tempBV
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeMonthCompanyExcel]    Script Date: 2016-11-28 15:59:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeMonthCompanyExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),CompanyName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),
		G3BV decimal(18,2),G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select CONVERT(nvarchar(6),tbl_PlaceBusinessVolume.CreateDate,112),tbl_Company.CompanyName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_Company on tbl_Department.CompanyId=tbl_Company.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		order by tbl_PlaceBusinessVolume.CreateDate desc,tbl_Place.PlaceCode desc

		select CreateDate as '登记月份',CompanyName as '公司',SUM(G2TV) as '2G话务量(万Erl)',SUM(G2BV) as '2G业务量(TB)',SUM(D2TV) as '2D话务量(万Erl)',
		SUM(D2BV) as '2D业务量(TB)',SUM(G3TV) as '3G话务量(万Erl)',SUM(G3BV) as '3G业务量(TB)',SUM(G4BV) as '4G业务量(TB)',SUM(TotalTV) as '合计话务量(万Erl)',SUM(TotalBV) as '合计业务量(TB)'  
			from @temp group by CreateDate,CompanyName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeMonthAreaExcel]    Script Date: 2016-11-28 15:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeMonthAreaExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),
		G3BV decimal(18,2),G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select CONVERT(nvarchar(6),tbl_PlaceBusinessVolume.CreateDate,112),tbl_Area.AreaName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

	select CreateDate as '登记月份',AreaName as '区域',SUM(G2TV) as '2G话务量(万Erl)',SUM(G2BV) as '2G业务量(TB)',SUM(D2TV) as '2D话务量(万Erl)',
		SUM(D2BV) as '2D业务量(TB)',SUM(G3TV) as '3G话务量(万Erl)',SUM(G3BV) as '3G业务量(TB)',SUM(G4BV) as '4G业务量(TB)',SUM(TotalTV) as '合计话务量(万Erl)',SUM(TotalBV) as '合计业务量(TB)' 
		from @temp group by CreateDate,AreaName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeMonthReseauExcel]    Script Date: 2016-11-28 15:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeMonthReseauExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),ReseauName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),G3BV decimal(18,2),
		G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select CONVERT(nvarchar(6),tbl_PlaceBusinessVolume.CreateDate,112),tbl_Area.AreaName,tbl_Reseau.ReseauName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

	select CreateDate as '登记月份',AreaName as '区域',ReseauName as '网格',SUM(G2TV) as '2G话务量(万Erl)',SUM(G2BV) as '2G业务量(TB)',SUM(D2TV) as '2D话务量(万Erl)',
		SUM(D2BV) as '2D业务量(TB)',SUM(G3TV) as '3G话务量(万Erl)',SUM(G3BV) as '3G业务量(TB)',SUM(G4BV) as '4G业务量(TB)',SUM(TotalTV) as '合计话务量(万Erl)',SUM(TotalBV) as '合计业务量(TB)' 
		from @temp group by CreateDate,AreaName,ReseauName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeMonthPlaceExcel]    Script Date: 2016-11-28 15:53:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeMonthPlaceExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@PlaceName varchar(100),
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),ReseauName varchar(50),PlaceName varchar(100),
		G2TV decimal(18,3),G2BV decimal(18,3),D2TV decimal(18,3),D2BV decimal(18,3),G3TV decimal(18,3),G3BV decimal(18,3),
		G4BV decimal(18,3),TotalTV decimal(18,3),TotalBV decimal(18,3))

	insert @temp
	select CONVERT(nvarchar(6),tbl_PlaceBusinessVolume.CreateDate,112),tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.PlaceName,
	SUM(ISNULL(g2.TrafficVolumes,0)) as G2TV,
	SUM(ISNULL(g2.BusinessVolumes,0)) as G2BV,
	SUM(ISNULL(d2.TrafficVolumes,0)) as D2TV,
	SUM(ISNULL(d2.BusinessVolumes,0)) as D2BV,
	SUM(ISNULL(g3.TrafficVolumes,0)) as G3TV,
	SUM(ISNULL(g3.BusinessVolumes,0)) as G3BV,
	SUM(ISNULL(g4.BusinessVolumes,0)) as G4BV,
	SUM((ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))) as TotalTV,
	SUM((ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))) as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
			(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		group by CONVERT(nvarchar(6),tbl_PlaceBusinessVolume.CreateDate,112),AreaName,ReseauName,PlaceName

	select CreateDate as '登记月份',AreaName as '区域',ReseauName as '网格',PlaceName as '站点名称',G2TV as '2G话务量',G2BV as '2G业务量',D2TV as '2D话务量',D2BV as '2D业务量',
		G3TV as '3G话务量',G3BV as '3G业务量',G4BV as '4G业务量',TotalTV as '合计话务量',TotalBV as '合计业务量'
		from @temp order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeCompanyExcel]    Script Date: 2016-11-28 15:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeCompanyExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate datetime,CompanyName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),G3BV decimal(18,2),
		G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select tbl_PlaceBusinessVolume.CreateDate,tbl_Company.CompanyName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_Company on tbl_Department.CompanyId=tbl_Company.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		order by tbl_PlaceBusinessVolume.CreateDate desc,tbl_Place.PlaceCode desc

		select CreateDate as '登记日期',CompanyName as '公司',SUM(G2TV) as '2G话务量(万Erl)',SUM(G2BV) as '2G业务量(TB)',SUM(D2TV) as '2D话务量(万Erl)',
			SUM(D2BV) as '2D业务量(TB)',SUM(G3TV) as '3G话务量(万Erl)',SUM(G3BV) as '3G业务量(TB)',SUM(G4BV) as '4G业务量(TB)',SUM(TotalTV) as '合计话务量(万Erl)',SUM(TotalBV) as '合计业务量(TB)'  
			from @temp group by CreateDate,CompanyName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeAreaExcel]    Script Date: 2016-11-28 15:19:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeAreaExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate datetime,AreaId uniqueidentifier,AreaName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),G3BV decimal(18,2),
		G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select tbl_PlaceBusinessVolume.CreateDate,tbl_Area.Id,tbl_Area.AreaName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		order by tbl_PlaceBusinessVolume.CreateDate desc,tbl_Place.PlaceCode desc

		select CreateDate as '登记日期',AreaName as '区域',SUM(G2TV) as '2G话务量(万Erl)',SUM(G2BV) as '2G业务量(TB)',SUM(D2TV) as '2D话务量(万Erl)',
			SUM(D2BV) as '2D业务量(TB)',SUM(G3TV) as '3G话务量(万Erl)',SUM(G3BV) as '3G业务量(TB)',SUM(G4BV) as '4G业务量(TB)',SUM(TotalTV) as '合计话务量(万Erl)',SUM(TotalBV) as '合计业务量(TB)' 
			from @temp group by CreateDate,AreaName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeReseauExcel]    Script Date: 2016-11-28 15:03:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeReseauExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate datetime,AreaName varchar(50),ReseauName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),G3BV decimal(18,2),
		G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select tbl_PlaceBusinessVolume.CreateDate,tbl_Area.AreaName,tbl_Reseau.ReseauName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		order by tbl_PlaceBusinessVolume.CreateDate desc,tbl_Place.PlaceCode desc


		select CreateDate as '登记日期',AreaName as '区域',ReseauName as '网格',SUM(G2TV) as '2G话务量(万Erl)',SUM(G2BV) as '2G业务量(TB)',SUM(D2TV) as '2D话务量(万Erl)',
			SUM(D2BV) as '2D业务量(TB)',SUM(G3TV) as '3G话务量(万Erl)',SUM(G3BV) as '3G业务量(TB)',SUM(G4BV) as '4G业务量(TB)',SUM(TotalTV) as '合计话务量(万Erl)',SUM(TotalBV) as '合计业务量(TB)' 
			from @temp group by CreateDate,AreaName,ReseauName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportBusinessVolumeExcel]    Script Date: 2016-11-28 14:37:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportBusinessVolumeExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_PlaceBusinessVolume.CreateDate as '登记日期',tbl_Place.PlaceName as '站点名称',tbl_Area.AreaName as '区域',tbl_Reseau.ReseauName as '网格',
	ISNULL(g2.TrafficVolumes,0) as '2G话务量',
	ISNULL(g2.BusinessVolumes,0) as '2G业务量',
	ISNULL(d2.TrafficVolumes,0) as '2D话务量',
	ISNULL(d2.BusinessVolumes,0) as '2D业务量',
	ISNULL(g3.TrafficVolumes,0) as '3G话务量',
	ISNULL(g3.BusinessVolumes,0) as '3G业务量',
	ISNULL(g4.BusinessVolumes,0) as '4G业务量',
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0)) as '合计话务量',
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0)) as '合计业务量'
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
			(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		order by tbl_PlaceBusinessVolume.CreateDate desc,tbl_Place.PlaceCode desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportLogicalBusinessVolumeExcel]    Script Date: 2016-11-28 14:28:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportLogicalBusinessVolumeExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@LogicalType int,
					@LogicalNumber nvarchar(150),
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select case tbl_BusinessVolume.LogicalType when 1 then '2G逻辑号' when 2 then '2D逻辑号' when 3 then '3G逻辑号' when 4 then '4G逻辑号' else '' end as '逻辑号类型',
		tbl_BusinessVolume.LogicalNumber as '逻辑号',tbl_BusinessVolume.TrafficVolumes as '话务量',tbl_BusinessVolume.BusinessVolumes as '业务量',tbl_User.FullName as '登记人',
		tbl_BusinessVolume.CreateDate as '登记日期'
		from tbl_BusinessVolume left join tbl_User on tbl_BusinessVolume.CreateUserId = tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where (tbl_BusinessVolume.CreateDate >= @BeginDate and tbl_BusinessVolume.CreateDate < @EndDate) and
				(@LogicalType = 0 or tbl_BusinessVolume.LogicalType = @LogicalType) and
				(@LogicalNumber = '' or CHARINDEX(@LogicalNumber,tbl_BusinessVolume.LogicalNumber) > 0) and
				tbl_BusinessVolume.Profession=@Profession and tbl_Department.CompanyId = @CompanyId
		order by tbl_BusinessVolume.LogicalType,tbl_BusinessVolume.LogicalNumber asc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeYearRisePlace]    Script Date: 2016-11-28 10:17:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeYearRisePlace]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),ReseauName varchar(50),PlaceName varchar(100),
		TotalTV decimal(18,3),TVRiseYear decimal(18,3),TotalBV decimal(18,3),BVRiseYear decimal(18,3))

	declare @PlaceId uniqueidentifier,
			@AreaName varchar(50),
			@ReseauName varchar(50),
			@PlaceName varchar(100),
			@TotalTV decimal(18,3),
			@TotalTVYear decimal(18,3),
			@TotalBV decimal(18,3),
			@TotalBVYear decimal(18,3),
			@TotalTVYearRate decimal(18,3),
			@TotalBVYearRate decimal(18,3)

	declare cur cursor for
	select tbl_Place.Id,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.PlaceName from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id 
							left join tbl_User on tbl_Reseau.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
		where tbl_Department.CompanyId=@CompanyId and (@Profession=0 or tbl_Place.Profession=@Profession) order by tbl_Place.PlaceCode
	open cur
	fetch next from cur into @PlaceId,@AreaName,@ReseauName,@PlaceName
	while @@fetch_status = 0
	begin
		set @TotalTV=0
		set @TotalTVYear=0
		set @TotalBV=0
		set @TotalBVYear=0
		set	@TotalTVYearRate=0
		set	@TotalBVYearRate=0

		select @TotalTV=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBV=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_PlaceBusinessVolume.PlaceId = @PlaceId and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVYear=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVYear=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_PlaceBusinessVolume.PlaceId = @PlaceId and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(YEAR,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(YEAR,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		if @TotalTV=0
		begin
			set @TotalTVYearRate=0
		end
		else
		begin
			set @TotalTV=@TotalTV
			if @TotalTVYear=0
			begin
				set @TotalTVYearRate=0
			end
			else
			begin
				set @TotalTVYearRate=(@TotalTV*1.0/@TotalTVYear-1)*100
			end
		end

		if @TotalBV=0
		begin
			set @TotalBVYearRate=0
		end
		else
		begin
			set @TotalBV=@TotalBV
			if @TotalBVYear=0
			begin
				set @TotalBVYearRate=0
			end
			else
			begin
				set @TotalBVYearRate=(@TotalBV*1.0/@TotalBVYear-1)*100
			end
		end
		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@AreaName,@ReseauName,@PlaceName,@TotalTV,@TotalTVYearRate,
			@TotalBV,@TotalBVYearRate)
		
		fetch next from cur into @PlaceId,@AreaName,@ReseauName,@PlaceName
	end
	close cur
	deallocate cur

	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select * from @temp order by AreaName,ReseauName,PlaceName offset @PageStart row fetch next @PageSize rows only

	select COUNT(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeMonthRisePlace]    Script Date: 2016-11-28 10:08:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeMonthRisePlace]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),ReseauName varchar(50),PlaceName varchar(100),
		TotalTV decimal(18,2),TVRiseMonth decimal(18,2),TVRiseYear decimal(18,2),TotalBV decimal(18,2),BVRiseMonth decimal(18,2),BVRiseYear decimal(18,2))

	declare @PlaceId uniqueidentifier,
			@AreaName varchar(50),
			@ReseauName varchar(50),
			@PlaceName varchar(100),
			@TotalTV decimal(18,3),
			@TotalTVMonth decimal(18,3),
			@TotalTVYear decimal(18,3),
			@TotalBV decimal(18,3),
			@TotalBVMonth decimal(18,3),
			@TotalBVYear decimal(18,3),
			@TotalTVMonthRate decimal(18,3),
			@TotalTVYearRate decimal(18,3),
			@TotalBVMonthRate decimal(18,3),
			@TotalBVYearRate decimal(18,3)

	declare cur cursor for
	select tbl_Place.Id,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.PlaceName from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id 
							left join tbl_User on tbl_Reseau.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
		where tbl_Department.CompanyId=@CompanyId and (@Profession=0 or tbl_Place.Profession=@Profession) order by tbl_Place.PlaceCode
	open cur
	fetch next from cur into @PlaceId,@AreaName,@ReseauName,@PlaceName
	while @@fetch_status = 0
	begin
		set @TotalTV=0
		set @TotalTVMonth=0
		set @TotalTVYear=0
		set @TotalBV=0
		set @TotalBVMonth=0
		set @TotalBVYear=0
		set @TotalTVMonthRate=0
		set	@TotalTVYearRate=0
		set	@TotalBVMonthRate=0
		set	@TotalBVYearRate=0

		select @TotalTV=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBV=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_PlaceBusinessVolume.PlaceId = @PlaceId and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVMonth=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVMonth=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_PlaceBusinessVolume.PlaceId = @PlaceId and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVYear=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVYear=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_PlaceBusinessVolume.PlaceId = @PlaceId and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(YEAR,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(YEAR,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		if @TotalTV=0
		begin
			set @TotalTVMonthRate=0
			set @TotalTVYearRate=0
		end
		else
		begin
			set @TotalTV=@TotalTV
			if @TotalTVMonth=0
			begin			
				set @TotalTVMonthRate=0
			end
			else
			begin			
				set @TotalTVMonthRate=(@TotalTV*1.0/@TotalTVMonth-1)*100
			end
			if @TotalTVYear=0
			begin
				set @TotalTVYearRate=0
			end
			else
			begin
				set @TotalTVYearRate=(@TotalTV*1.0/@TotalTVYear-1)*100
			end
		end

		if @TotalBV=0
		begin
			set @TotalBVMonthRate=0
			set @TotalBVYearRate=0
		end
		else
		begin
			set @TotalBV=@TotalBV
			if @TotalBVMonth=0
			begin			
				set @TotalBVMonthRate=0
			end
			else
			begin			
				set @TotalBVMonthRate=(@TotalBV*1.0/@TotalBVMonth-1)*100
			end
			if @TotalBVYear=0
			begin
				set @TotalBVYearRate=0
			end
			else
			begin
				set @TotalBVYearRate=(@TotalBV*1.0/@TotalBVYear-1)*100
			end
		end
		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@AreaName,@ReseauName,@PlaceName,@TotalTV,@TotalTVMonthRate,@TotalTVYearRate,
			@TotalBV,@TotalBVMonthRate,@TotalBVYearRate)
		
		fetch next from cur into @PlaceId,@AreaName,@ReseauName,@PlaceName
	end
	close cur
	deallocate cur

	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select * from @temp order by AreaName,ReseauName,PlaceName offset @PageStart row fetch next @PageSize rows only

	select COUNT(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeMonthPlace]    Script Date: 2016-11-28 09:24:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeMonthPlace]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@PlaceName varchar(100),
					@Profession int,
					@CompanyId uniqueidentifier,
					@SortField varchar(50),
					@SortOrder varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,CreateDate nvarchar(6),AreaName varchar(50),ReseauName varchar(50),PlaceName varchar(100),
		G2TV decimal(18,3),G2BV decimal(18,3),D2TV decimal(18,3),D2BV decimal(18,3),G3TV decimal(18,3),G3BV decimal(18,3),
		G4BV decimal(18,3),TotalTV decimal(18,3),TotalBV decimal(18,3))

	insert @temp
	select tbl_Place.Id,CONVERT(nvarchar(6),tbl_PlaceBusinessVolume.CreateDate,112),tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.PlaceName,
	SUM(ISNULL(g2.TrafficVolumes,0)) as G2TV,
	SUM(ISNULL(g2.BusinessVolumes,0)) as G2BV,
	SUM(ISNULL(d2.TrafficVolumes,0)) as D2TV,
	SUM(ISNULL(d2.BusinessVolumes,0)) as D2BV,
	SUM(ISNULL(g3.TrafficVolumes,0)) as G3TV,
	SUM(ISNULL(g3.BusinessVolumes,0)) as G3BV,
	SUM(ISNULL(g4.BusinessVolumes,0)) as G4BV,
	SUM((ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))) as TotalTV,
	SUM((ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))) as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
			(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		group by tbl_Place.Id,CONVERT(nvarchar(6),tbl_PlaceBusinessVolume.CreateDate,112),AreaName,ReseauName,PlaceName

	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select Id,CreateDate,AreaName,ReseauName,PlaceName,G2TV,G2BV,D2TV,D2BV,G3TV,G3BV,
		G4BV,TotalTV,TotalBV 
		from @temp order by case when @SortField = N'G2TV' and @SortOrder = 'asc' then G2TV end,
							case when @SortField = N'G2BV' and @SortOrder = 'asc' then G2BV end,							
							case when @SortField = N'D2TV' and @SortOrder = 'asc' then D2TV end,
							case when @SortField = N'D2BV' and @SortOrder = 'asc' then D2BV end,
							case when @SortField = N'G3TV' and @SortOrder = 'asc' then G3TV end,
							case when @SortField = N'G3BV' and @SortOrder = 'asc' then G3BV end,
							case when @SortField = N'G4BV' and @SortOrder = 'asc' then G4BV end,
							case when @SortField = N'TotalTV' and @SortOrder = 'asc' then TotalTV end,
							case when @SortField = N'TotalBV' and @SortOrder = 'asc' then TotalBV end,
							case when @SortField = N'G2TV' and @SortOrder = 'desc' then G2TV end desc,
							case when @SortField = N'G2BV' and @SortOrder = 'desc' then G2BV end desc,							
							case when @SortField = N'D2TV' and @SortOrder = 'desc' then D2TV end desc,
							case when @SortField = N'D2BV' and @SortOrder = 'desc' then D2BV end desc,
							case when @SortField = N'G3TV' and @SortOrder = 'desc' then G3TV end desc,
							case when @SortField = N'G3BV' and @SortOrder = 'desc' then G3BV end desc,
							case when @SortField = N'G4BV' and @SortOrder = 'desc' then G4BV end desc,
							case when @SortField = N'TotalTV' and @SortOrder = 'desc' then TotalTV end desc,
							case when @SortField = N'TotalBV' and @SortOrder = 'desc' then TotalBV end desc,
							case when @SortField = '' and @SortOrder = '' then CreateDate end desc
		offset @PageStart row fetch next @PageSize rows only

	select COUNT(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetNearbyPlanningsAndPlacesAll]    Script Date: 2016-11-27 20:53:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetNearbyPlanningsAndPlacesAll]
					@Lng decimal(18,5),
					@Lat decimal(18,5),
					@PlanningId uniqueidentifier,
					@PlaceId uniqueidentifier,
					@PlanningProfessionsSql varchar(max),
					@PlaceProfessionsSql varchar(max),
					@Distance decimal(18,5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @CompanyName nvarchar(100)
	select @CompanyName = CompanyName from tbl_Company where Id = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600';

	declare @TempPlanningProfession table(value int);
	insert @TempPlanningProfession exec(@PlanningProfessionsSql);

	declare @TempPlaceProfession table(value int);
	insert @TempPlaceProfession exec(@PlaceProfessionsSql);

	with cte as
	(
		select tbl_Planning.Id,tbl_Planning.PlanningName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,
				tbl_Reseau.ReseauName,tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.Profession,tbl_Planning.Issued,
				case tbl_Planning.AddressingState when 1 then '未寻址确认' when 2 then '已寻址确认' when 3 then '流转中' when 4 then '流程终止' else '' end as AddressingStateName,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Planning.Lng,tbl_Planning.Lat) as Distance
			from tbl_Planning left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
								left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
								left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
								left join @TempPlanningProfession t on tbl_Planning.Profession = t.value
			where tbl_Planning.Id <> @PlanningId and
					tbl_Planning.AddressingState <> 2 and 
					t.value is not null
	)
	select cte.Id,cte.PlanningName,cte.PlaceCategoryName,cte.AreaName,cte.ReseauName,cte.Lng,cte.Lat,cte.Profession,cte.Issued,cte.AddressingStateName,
			'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' as CompanyId,@CompanyName as CompanyName,1 as DataType
		from cte
		where cte.Distance <= @Distance;
		
	with cte as
	(
		select tbl_Place.Id,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,
				tbl_Reseau.ReseauName,tbl_Place.Lng,tbl_Place.Lat,
				tbl_Place.OwnerName,tbl_Place.OwnerContact,
				tbl_Place.OwnerPhoneNumber,tbl_Place.Profession,tbl_PlaceOwner.PlaceOwnerName,
				tbl_Place.G2Number,tbl_Place.D2Number,tbl_Place.G3Number,tbl_Place.G4Number,'' as NetWorks,tbl_Place.PlaceMapState,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Place.Lng,tbl_Place.Lat) as Distance
			from tbl_Place left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceOwner on tbl_Place.PlaceOwner = tbl_PlaceOwner.Id
								left join @TempPlaceProfession t on tbl_Place.Profession = t.value
			where tbl_Place.[State] = 1 and
					tbl_Place.Id <> @PlaceId and 
					t.value is not null
	)
	select cte.Id,cte.PlaceName,cte.PlaceCategoryName,cte.AreaName,cte.ReseauName,cte.Lng,cte.Lat,
			cte.OwnerName,cte.OwnerContact,cte.OwnerPhoneNumber,cte.Profession,cte.PlaceOwnerName,cte.G2Number,cte.D2Number,cte.G3Number,cte.G4Number,'' as NetWorks,cte.PlaceMapState,2 as DataType
		from cte
		where cte.Distance <= @Distance;
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetNearbyPlanningsAndPlacesMobile]    Script Date: 2016-11-27 12:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetNearbyPlanningsAndPlacesMobile]
					@Lng decimal(18,5),
					@Lat decimal(18,5),
					@Distance decimal(18,5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @CompanyName nvarchar(100)
	select @CompanyName = CompanyName from tbl_Company where Id = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600';

	with cte as
	(
		select tbl_Planning.Id,tbl_Planning.PlanningName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,
				tbl_Reseau.ReseauName,tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.Profession,tbl_Planning.Issued,
				case tbl_Planning.AddressingState when 1 then '未寻址确认' when 2 then '已寻址确认' when 3 then '流转中' when 4 then '流程终止' else '' end as AddressingStateName,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Planning.Lng,tbl_Planning.Lat) as Distance
			from tbl_Planning left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
								left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
								left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
			where tbl_Planning.AddressingState <> 2
	)
	select cte.Id,cte.PlanningName,cte.PlaceCategoryName,cte.AreaName,cte.ReseauName,cte.Lng,cte.Lat,cte.Profession,cte.Issued,cte.AddressingStateName,
			'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' as CompanyId,@CompanyName as CompanyName,1 as DataType
		from cte
		where cte.Distance <= @Distance;
		
	with cte as
	(
		select tbl_Place.Id,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,
				tbl_Reseau.ReseauName,tbl_Place.Lng,tbl_Place.Lat,
				tbl_Place.OwnerName,tbl_Place.OwnerContact,
				tbl_Place.OwnerPhoneNumber,tbl_Place.Profession,tbl_PlaceOwner.PlaceOwnerName,
				tbl_Place.G2Number,tbl_Place.D2Number,tbl_Place.G3Number,tbl_Place.G4Number,'' as NetWorks,tbl_Place.PlaceMapState,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Place.Lng,tbl_Place.Lat) as Distance
			from tbl_Place left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceOwner on tbl_Place.PlaceOwner = tbl_PlaceOwner.Id
			where tbl_Place.[State] = 1
	)
	select cte.Id,cte.PlaceName,cte.PlaceCategoryName,cte.AreaName,cte.ReseauName,cte.Lng,cte.Lat,
			cte.OwnerName,cte.OwnerContact,cte.OwnerPhoneNumber,cte.Profession,cte.PlaceOwnerName,cte.G2Number,cte.D2Number,cte.G3Number,cte.G4Number,'' as NetWorks,cte.PlaceMapState,2 as DataType
		from cte
		where cte.Distance <= @Distance;
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryEngineeringProgresssPageMobile]    Script Date: 2016-11-26 23:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryEngineeringProgresssPageMobile]
					@Profession int,
					@ProjectCode nvarchar(100),
					@PlaceName nvarchar(100),
					@TaskModel int,
					@EngineeringProgress int,
					@CurrentUserId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,PlaceId uniqueidentifier,Profession int,PlaceName nvarchar(100),TaskModel int,EngineeringProgress int,
		ProjectCode varchar(50),ProfessionName nvarchar(50))

	insert @temp
		select tbl_EngineeringTask.Id,tbl_ProjectTask.PlaceId,tbl_Place.Profession,tbl_Place.PlaceName,tbl_EngineeringTask.TaskModel,tbl_EngineeringTask.EngineeringProgress,
		tbl_ProjectTask.ProjectCode,case tbl_Place.Profession when 1 then '基站' when 2 then '室分' else '' end as ProfessionName
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
							left join tbl_Customer design on tbl_EngineeringTask.DesignCustomerId=design.Id
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@EngineeringProgress = 0 or (tbl_EngineeringTask.EngineeringProgress=@EngineeringProgress and @EngineeringProgress<>6) or (@EngineeringProgress=6 and (tbl_EngineeringTask.EngineeringProgress=1 or tbl_EngineeringTask.EngineeringProgress=2 or tbl_EngineeringTask.EngineeringProgress=4))) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_EngineeringTask.[State]=1 and
				(tbl_EngineeringTask.ProjectManagerId=@CurrentUserId or 
				construction.CustomerUserId=@CurrentUserId or
				supervision.CustomerUserId=@CurrentUserId or
				tbl_EngineeringTask.ProjectManagerId=@CurrentUserId)
		union
		select tbl_EngineeringTask.Id,tbl_ProjectTask.PlaceId,tbl_Place.Profession,tbl_Place.PlaceName,tbl_EngineeringTask.TaskModel,tbl_EngineeringTask.EngineeringProgress,
		tbl_ProjectTask.ProjectCode,case tbl_Place.Profession when 1 then '基站' when 2 then '室分' else '' end as ProfessionName
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
							left join tbl_Customer design on tbl_EngineeringTask.DesignCustomerId=design.Id
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@EngineeringProgress = 0 or (tbl_EngineeringTask.EngineeringProgress=@EngineeringProgress and @EngineeringProgress<>6) or (@EngineeringProgress=6 and (tbl_EngineeringTask.EngineeringProgress=1 or tbl_EngineeringTask.EngineeringProgress=2 or tbl_EngineeringTask.EngineeringProgress=4))) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType<>1 and tbl_EngineeringTask.[State]=1 and
				(tbl_EngineeringTask.ProjectManagerId=@CurrentUserId or 
				construction.CustomerUserId=@CurrentUserId or
				supervision.CustomerUserId=@CurrentUserId or
				tbl_EngineeringTask.ProjectManagerId=@CurrentUserId)

		select * from @temp t
		order by t.TaskModel,t.PlaceName

	select count(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryProjectProgresssPageMobile]    Script Date: 2016-11-26 22:46:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectProgresssPageMobile]
					@Profession int,
					@ProjectCode nvarchar(100),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@ProjectProgress int,
					@AreaManagerId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,PlaceId uniqueidentifier,Profession int,PlaceName nvarchar(100),ProjectProgress int,ProfessionName nvarchar(50),ProjectCode varchar(50))

	insert @temp
		select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,tbl_Place.Profession,tbl_Place.PlaceName,tbl_ProjectTask.ProjectProgress,
		case tbl_Place.Profession when 1 then '基站' when 2 then '室分' else '' end as ProfessionName,tbl_ProjectTask.ProjectCode
		from tbl_ProjectTask left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectProgress = 0 or (tbl_ProjectTask.ProjectProgress = @ProjectProgress and @ProjectProgress<>7) or (@ProjectProgress=7 and (tbl_ProjectTask.ProjectProgress=1 or tbl_ProjectTask.ProjectProgress=2))) and
				(@Profession=0 or tbl_Place.Profession = @Profession) and
				 tbl_ProjectTask.ProjectType=1 and
				tbl_ProjectTask.AreaManagerId = @AreaManagerId
		union
		select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,tbl_Place.Profession,tbl_Place.PlaceName,tbl_ProjectTask.ProjectProgress,
		case tbl_Place.Profession when 1 then '基站' when 2 then '室分' else '' end as ProfessionName,tbl_ProjectTask.ProjectCode
		from tbl_ProjectTask left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectProgress = 0 or (tbl_ProjectTask.ProjectProgress = @ProjectProgress and @ProjectProgress<>7) or (@ProjectProgress=7 and (tbl_ProjectTask.ProjectProgress=1 or tbl_ProjectTask.ProjectProgress=2))) and
				(@Profession=0 or tbl_Place.Profession = @Profession) and
				tbl_ProjectTask.ProjectType<>1 and
				tbl_ProjectTask.AreaManagerId = @AreaManagerId

		select * from @temp t
		order by t.Profession,t.PlaceName

	select count(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetTaskToDoMobile]    Script Date: 2016-11-26 21:46:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetTaskToDoMobile]
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
	declare @TempTask table(ProfessionName nvarchar(10),Profession int,TaskTypeName nvarchar(20),TaskTypeId int,TaskCount int,PageUrl varchar(100),TaskIndex int)

	--项目进度登记
	insert into @TempTask 
		select '基站',1,'项目进度登记',1,count(tbl_ProjectTask.Id),'BaseStationBM/ProjectProgress',2
			from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_Place.Profession=1 and tbl_ProjectTask.AreaManagerId=@UserId and
				(tbl_ProjectTask.ProjectProgress = 1 or tbl_ProjectTask.ProjectProgress = 2)

	--工程进度登记
	insert into @TempTask 
		select '基站',1,'工程进度登记',2,count(tbl_EngineeringTask.Id),'BaseStationBM/EngineeringProgress',3
			from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
									left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
									left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
									left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
			where tbl_Place.Profession=1 and  (construction.CustomerUserId=@UserId or supervision.CustomerUserId=@UserId or tbl_EngineeringTask.ProjectManagerId=@UserId) and
				(tbl_EngineeringTask.EngineeringProgress = 1 or tbl_EngineeringTask.EngineeringProgress = 2 or tbl_EngineeringTask.EngineeringProgress = 4)

	--项目进度登记
	insert into @TempTask 
		select '室分',2,'项目进度登记',3,count(tbl_ProjectTask.Id),'IndoorDistributionBM/ProjectProgress',3
			from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_Place.Profession=2 and tbl_ProjectTask.AreaManagerId=@UserId and
				(tbl_ProjectTask.ProjectProgress = 1 or tbl_ProjectTask.ProjectProgress = 2)

	--工程进度登记
	insert into @TempTask 
		select '室分',2,'工程进度登记',4,count(tbl_EngineeringTask.Id),'IndoorDistributionBM/EngineeringProgress',4
			from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
									left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
									left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
									left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
			where tbl_Place.Profession=2 and  (construction.CustomerUserId=@UserId or supervision.CustomerUserId=@UserId or tbl_EngineeringTask.ProjectManagerId=@UserId) and
				(tbl_EngineeringTask.EngineeringProgress = 1 or tbl_EngineeringTask.EngineeringProgress = 2 or tbl_EngineeringTask.EngineeringProgress = 4)

	select * from @TempTask where TaskCount>0 order by TaskIndex
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetNearbyPlanningsAndPlacesID]    Script Date: 2016-11-18 14:22:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetNearbyPlanningsAndPlacesID]
					@Lng decimal(18,5),
					@Lat decimal(18,5),
					@PlanningId uniqueidentifier,
					@PlaceId uniqueidentifier,
					@IDPlanningPlaceCategorySql varchar(max),
					@IDPlaceCategorySql varchar(max),
					@Distance decimal(18,5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @CompanyName nvarchar(100)
	select @CompanyName = CompanyName from tbl_Company where Id = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600'

	declare @TempIDPlanningPlaceCategory table(value uniqueidentifier);
	insert @TempIDPlanningPlaceCategory exec(@IDPlanningPlaceCategorySql);

	declare @TempIDPlaceCategory table(value uniqueidentifier);
	insert @TempIDPlaceCategory exec(@IDPlaceCategorySql);

	with cte as
	(
		select tbl_Planning.Id,tbl_Planning.PlanningName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,
				tbl_Reseau.ReseauName,tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.Profession,tbl_Planning.Issued,
				case tbl_Planning.AddressingState when 1 then '未寻址确认' when 2 then '已寻址确认' when 3 then '流转中' when 4 then '流程终止' else '' end as AddressingStateName,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Planning.Lng,tbl_Planning.Lat) as Distance
			from tbl_Planning left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
								left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
								left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
								left join @TempIDPlanningPlaceCategory t on tbl_Planning.PlaceCategoryId = t.value
			where tbl_Planning.Profession = 2 and
					tbl_Planning.Id <> @PlanningId and
					tbl_Planning.AddressingState <> 2 and
					t.value is not null
	)
	select cte.Id,cte.PlanningName,cte.PlaceCategoryName,cte.AreaName,cte.ReseauName,cte.Lng,cte.Lat,cte.Profession,cte.Issued,cte.AddressingStateName,
			'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' as CompanyId,@CompanyName as CompanyName,1 as DataType
		from cte
		where cte.Distance <= @Distance;
		
	with cte as
	(
		select tbl_Place.Id,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,
				tbl_Reseau.ReseauName,tbl_Place.Lng,tbl_Place.Lat,
				tbl_Place.OwnerName,tbl_Place.OwnerContact,
				tbl_Place.OwnerPhoneNumber,tbl_Place.Profession,tbl_PlaceOwner.PlaceOwnerName,
				tbl_Place.G2Number,tbl_Place.D2Number,tbl_Place.G3Number,tbl_Place.G4Number,'' as NetWorks,tbl_Place.PlaceMapState,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Place.Lng,tbl_Place.Lat) as Distance
			from tbl_Place left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceOwner on tbl_Place.PlaceOwner = tbl_PlaceOwner.Id
							left join @TempIDPlaceCategory t on tbl_Place.PlaceCategoryId = t.value
			where tbl_Place.Profession = 2 and
					tbl_Place.[State] = 1 and
					tbl_Place.Id <> @PlaceId and
					t.value is not null
	)
	select cte.Id,cte.PlaceName,cte.PlaceCategoryName,cte.AreaName,cte.ReseauName,cte.Lng,cte.Lat,
			cte.OwnerName,cte.OwnerContact,cte.OwnerPhoneNumber,cte.Profession,cte.PlaceOwnerName,cte.G2Number,cte.D2Number,cte.G3Number,cte.G4Number,'' as NetWorks,cte.PlaceMapState,2 as DataType
		from cte
		where cte.Distance <= @Distance;
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryPlanningApplysWaitPage]    Script Date: 2016-11-07 14:50:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryPlanningApplysWaitPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlanningName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@DoState int,
					@PlanningUserId uniqueidentifier,
					@CreateUserId uniqueidentifier,
					@Profession int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_PlanningApply.Id,tbl_PlanningApply.PlanningCode,tbl_PlanningApply.PlanningName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlanningApply.Lng,tbl_PlanningApply.Lat,
			tbl_PlanningApply.DetailedAddress,tbl_PlanningApply.Remarks,tbl_PlanningApply.DoState,tbl_PlanningApply.Importance,tbl_PlanningApply.PlanningAdvice,
			tbl_User.FullName,tbl_Department.DepartmentName,tbl_PlanningApply.CreateDate
		from tbl_PlanningApply left join tbl_Reseau on tbl_PlanningApply.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_User on tbl_PlanningApply.CreateUserId = tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where (tbl_PlanningApply.CreateDate >= @BeginDate and tbl_PlanningApply.CreateDate < @EndDate) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_PlanningApply.PlanningName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_PlanningApply.ReseauId = @ReseauId) and
				(@PlanningUserId = '00000000-0000-0000-0000-000000000000' or tbl_PlanningApply.PlanningUserId = @PlanningUserId) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_PlanningApply.CreateUserId = @CreateUserId) and
				(@DoState=0 or tbl_PlanningApply.DoState=@DoState) and
				tbl_PlanningApply.Profession = @Profession
		order by tbl_PlanningApply.PlanningCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_PlanningApply left join tbl_Reseau on tbl_PlanningApply.ReseauId = tbl_Reseau.Id
		where (tbl_PlanningApply.CreateDate >= @BeginDate and tbl_PlanningApply.CreateDate < @EndDate) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_PlanningApply.PlanningName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_PlanningApply.ReseauId = @ReseauId) and
				(@PlanningUserId = '00000000-0000-0000-0000-000000000000' or tbl_PlanningApply.PlanningUserId = @PlanningUserId) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_PlanningApply.CreateUserId = @CreateUserId) and
				(@DoState=0 or tbl_PlanningApply.DoState=@DoState) and
				tbl_PlanningApply.Profession = @Profession
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetPlanningApplyByIds]    Script Date: 2016-11-04 16:43:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetPlanningApplyByIds]
					@PlanningApplyIdsSql varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @Temp table(value uniqueidentifier);
	insert @Temp exec(@PlanningApplyIdsSql);

	select tbl_PlanningApply.Id,tbl_PlanningApply.PlanningName,tbl_Area.AreaName,'' as PlaceCategoryName,
		tbl_PlanningApply.Lng,tbl_PlanningApply.Lat,'苏州联通' as CompanyName,1 as DataType,tbl_PlanningApply.Profession,
		'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' as CompanyId,tbl_Reseau.ReseauName,2 as Issued,
		'' as AddressingStateName
	from tbl_PlanningApply left join tbl_Reseau on tbl_PlanningApply.ReseauId=tbl_Reseau.Id
		left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
	where tbl_PlanningApply.Id in (select value from @Temp)
	order by tbl_PlanningApply.PlanningCode
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryAddressingMonthUser]    Script Date: 2016-10-31 13:23:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryAddressingMonthUser]
					@BeginDate datetime,
					@EndDate datetime,
					@DepartmentId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),DepartmentName varchar(50),FullName varchar(50),
		ACount int,BCount int,CCount int,AddressingScore int)

	declare @DepartmentName varchar(50),
			@FullName varchar(50),
			@AddressingUserId uniqueidentifier,
			@ACount int,
			@BCount int,
			@CCount int,
			@AddressingScore int
	declare cur cursor for
	select distinct tbl_Department.DepartmentName,tbl_User.FullName,tbl_Planning.AddressingUserId 
		from tbl_Planning left join tbl_User on tbl_Planning.AddressingUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where tbl_Planning.AddressingUserId<>'00000000-0000-0000-0000-000000000000' and
				(tbl_Department.Id=@DepartmentId or @DepartmentId='00000000-0000-0000-0000-000000000000') and
				tbl_Planning.AddressingState=2 and tbl_Planning.Profession=@Profession and tbl_Department.CompanyId=@CompanyId
	open cur
	fetch next from cur into @DepartmentName,@FullName,@AddressingUserId
	while @@fetch_status = 0
	begin
		set @ACount=0
		set @BCount=0
		set	@CCount=0
		set	@AddressingScore=0

		select @ACount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=1 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.AddressingUserId=@AddressingUserId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		select @BCount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=2 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.AddressingUserId=@AddressingUserId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		select @CCount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=3 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.AddressingUserId=@AddressingUserId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		set @AddressingScore=@ACount*3+@BCount*2+@CCount

		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@DepartmentName,@FullName,@ACount,@BCount,@CCount,@AddressingScore)

		fetch next from cur into @DepartmentName,@FullName,@AddressingUserId
	end
	close cur
	deallocate cur
	select * from @temp order by DepartmentName,FullName
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryAddressingMonthReseau]    Script Date: 2016-10-31 13:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryAddressingMonthReseau]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),ReseauName varchar(50),
		ACount int,BCount int,CCount int,AddressingScore int)

	declare @AreaName varchar(50),
			@ReseauName varchar(50),
			@ReseauId uniqueidentifier,
			@ACount int,
			@BCount int,
			@CCount int,
			@AddressingScore int
	declare cur cursor for
	select distinct tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Reseau.Id 
		from tbl_Reseau left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
						left join tbl_User on tbl_Reseau.CreateUserId=tbl_User.Id
						left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId=@AreaId) and tbl_Department.CompanyId=@CompanyId
	open cur
	fetch next from cur into @AreaName,@ReseauName,@ReseauId
	while @@fetch_status = 0
	begin
		set @ACount=0
		set @BCount=0
		set	@CCount=0
		set	@AddressingScore=0

		select @ACount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=1 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.ReseauId=@ReseauId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		select @BCount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=2 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.ReseauId=@ReseauId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		select @CCount=ISNULL(COUNT(*),0) from tbl_Planning
			where tbl_Planning.Importance=3 and
			tbl_Planning.AddressingState=2 and
			tbl_Planning.Profession=@Profession and		
			tbl_Planning.ReseauId=@ReseauId and 
			tbl_Planning.AddressingDate>=@BeginDate and 
			tbl_Planning.AddressingDate<DATEADD(MONTH,1,@EndDate)

		set @AddressingScore=@ACount*3+@BCount*2+@CCount

		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@AreaName,@ReseauName,@ACount,@BCount,@CCount,@AddressingScore)

		fetch next from cur into @AreaName,@ReseauName,@ReseauId
	end
	close cur
	deallocate cur
	select * from @temp order by AreaName,ReseauName
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBlindSpotFeedBacksPage]    Script Date: 2016-10-29 09:09:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBlindSpotFeedBacksPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@PlaceName nvarchar(100),
					@DoState int,
					@CreateUserId uniqueidentifier,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_BlindSpotFeedBack.Id,tbl_BlindSpotFeedBack.PlaceName,tbl_Area.AreaName,tbl_BlindSpotFeedBack.Lng,tbl_BlindSpotFeedBack.Lat,
			tbl_BlindSpotFeedBack.FeedBackContent,u1.FullName as DoFullName,tbl_BlindSpotFeedBack.DoState,tbl_BlindSpotFeedBack.FeedBackResult,
			u2.FullName,tbl_Department.DepartmentName,tbl_BlindSpotFeedBack.CreateDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_BlindSpotFeedBack left join tbl_Area on tbl_BlindSpotFeedBack.AreaId = tbl_Area.Id
							left join tbl_User u1 on tbl_BlindSpotFeedBack.DoUserId = u1.Id
							left join tbl_User u2 on tbl_BlindSpotFeedBack.CreateUserId = u2.Id
							left join tbl_Department on u2.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_BlindSpotFeedBack.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'BlindSpotFeedBack'
		where (tbl_BlindSpotFeedBack.CreateDate >= @BeginDate and tbl_BlindSpotFeedBack.CreateDate < @EndDate) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_BlindSpotFeedBack.AreaId = @AreaId) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_BlindSpotFeedBack.PlaceName) > 0) and
				(@DoState = 0 or tbl_BlindSpotFeedBack.DoState = @DoState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_BlindSpotFeedBack.CreateUserId = @CreateUserId)
		order by tbl_BlindSpotFeedBack.CreateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_BlindSpotFeedBack left join tbl_Area on tbl_BlindSpotFeedBack.AreaId = tbl_Area.Id
							left join tbl_User u1 on tbl_BlindSpotFeedBack.DoUserId = u1.Id
							left join tbl_User u2 on tbl_BlindSpotFeedBack.CreateUserId = u2.Id
							left join tbl_Department on u2.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_BlindSpotFeedBack.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'BlindSpotFeedBack'
		where (tbl_BlindSpotFeedBack.CreateDate >= @BeginDate and tbl_BlindSpotFeedBack.CreateDate < @EndDate) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_BlindSpotFeedBack.AreaId = @AreaId) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_BlindSpotFeedBack.PlaceName) > 0) and
				(@DoState = 0 or tbl_BlindSpotFeedBack.DoState = @DoState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_BlindSpotFeedBack.CreateUserId = @CreateUserId)
END
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectTaskDepartment]
					@BeginDate datetime,
					@BeginDateYear datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),DepartmentName varchar(50),
		MonthOnTimeCount int,MonthOverTimeCount int,TotalCount int,YearOnTimeCount int,YearOverTimeCount int,YearUnFinishCount int,
		YearOnTimeRate decimal(18,2),MonthScore int,YearScore decimal(18,2))

	declare @DepartmentId uniqueidentifier,
			@DepartmentName varchar(50),
			@MonthOnTimeCount int,
			@MonthOverTimeCount int,
			@TotalCount int,
			@YearOnTimeCount int,
			@YearOverTimeCount int,
			@YearUnFinishCount int,
			@YearOnTimeRate decimal(18,2),
			@MonthScore int,
			@YearScore decimal(18,2)
	declare cur cursor for
	select distinct tbl_Department.Id,tbl_Department.DepartmentName 
		from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
		where tbl_ProjectTask.AreaManagerId<>'00000000-0000-0000-0000-000000000000' and 
				tbl_ProjectTask.ProjectCode<>'' and tbl_Place.Profession=@Profession and tbl_Department.CompanyId=@CompanyId
	open cur
	fetch next from cur into @DepartmentId,@DepartmentName
	while @@fetch_status = 0
	begin
		set @MonthOnTimeCount=0
		set @MonthOverTimeCount=0
		set	@TotalCount=0
		set	@YearOnTimeCount=0
		set	@YearOverTimeCount=0
		set	@YearUnFinishCount=0
		set	@YearOnTimeRate=0
		set	@MonthScore=0
		set	@YearScore=0

		select @MonthOnTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
																		left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and		
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectDate>@BeginDate and 
			tbl_ProjectTask.ProjectDate<DATEADD(MONTH,1,@BeginDate) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30

		select @MonthOverTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id 
																		left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and 
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectDate>@BeginDate and 
			tbl_ProjectTask.ProjectDate<DATEADD(MONTH,1,@BeginDate) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30

		select @TotalCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
																left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id 
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear)

		select @YearOnTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
																		left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30

		select @YearOverTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
																		left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30

		select @YearUnFinishCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
																		left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate='2000-01-01' and
			tbl_User.DepartmentId=@DepartmentId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear)

		if @TotalCount<>0
		begin
			set @YearOnTimeRate=(@YearOnTimeCount+@YearOverTimeCount)*1.0/@TotalCount*100
		end
		else
		begin
			set @YearOnTimeRate=0
		end

		set @MonthScore=@MonthOnTimeCount*2+@MonthOverTimeCount
		set @YearScore=(@YearOnTimeCount*2+@YearOverTimeCount)*(@YearOnTimeRate)/(100+0.0)

		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@DepartmentName,@MonthOnTimeCount,@MonthOverTimeCount,@TotalCount,@YearOnTimeCount,
			@YearOverTimeCount,@YearUnFinishCount,@YearOnTimeRate,@MonthScore,@YearScore)

		fetch next from cur into @DepartmentId,@DepartmentName
	end
	close cur
	deallocate cur
	select * from @temp order by DepartmentName
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryProjectTaskProjectManager]    Script Date: 2016-10-27 15:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectTaskProjectManager]
					@BeginDate datetime,
					@BeginDateYear datetime,
					@DepartmentId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),DepartmentName varchar(50),FullName varchar(50),
		MonthOnTimeCount int,MonthOverTimeCount int,TotalCount int,YearOnTimeCount int,YearOverTimeCount int,YearUnFinishCount int,
		YearOnTimeRate decimal(18,2),MonthScore int,YearScore decimal(18,2))

	declare @DepartmentName varchar(50),
			@FullName varchar(50),
			@AreaManagerId uniqueidentifier,
			@MonthOnTimeCount int,
			@MonthOverTimeCount int,
			@TotalCount int,
			@YearOnTimeCount int,
			@YearOverTimeCount int,
			@YearUnFinishCount int,
			@YearOnTimeRate decimal(18,2),
			@MonthScore int,
			@YearScore decimal(18,2)
	declare cur cursor for
	select distinct tbl_Department.DepartmentName,tbl_User.FullName,tbl_ProjectTask.AreaManagerId 
		from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id			 
		where tbl_ProjectTask.AreaManagerId<>'00000000-0000-0000-0000-000000000000' and
				(tbl_Department.Id=@DepartmentId or @DepartmentId='00000000-0000-0000-0000-000000000000') and
				tbl_ProjectTask.ProjectCode<>'' and tbl_Place.Profession=@Profession and tbl_Department.CompanyId=@CompanyId
	open cur
	fetch next from cur into @DepartmentName,@FullName,@AreaManagerId
	while @@fetch_status = 0
	begin
		set @MonthOnTimeCount=0
		set @MonthOverTimeCount=0
		set	@TotalCount=0
		set	@YearOnTimeCount=0
		set	@YearOverTimeCount=0
		set	@YearUnFinishCount=0
		set	@YearOnTimeRate=0
		set	@MonthScore=0
		set	@YearScore=0

		select @MonthOnTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and		
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectDate>@BeginDate and 
			tbl_ProjectTask.ProjectDate<DATEADD(MONTH,1,@BeginDate) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30

		select @MonthOverTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and 
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectDate>@BeginDate and 
			tbl_ProjectTask.ProjectDate<DATEADD(MONTH,1,@BeginDate) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30

		select @TotalCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear)

		select @YearOnTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30

		select @YearOverTimeCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate<>'2000-01-01' and	
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear) and
			DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30

		select @YearUnFinishCount=ISNULL(COUNT(*),0) from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_ProjectTask.ProjectCode<>'' and
			tbl_Place.Profession=@Profession and
			tbl_ProjectTask.ProjectBeginDate<>'2000-01-01' and
			tbl_ProjectTask.ProjectDate='2000-01-01' and
			tbl_ProjectTask.AreaManagerId=@AreaManagerId and 
			tbl_ProjectTask.ProjectBeginDate>@BeginDateYear and 
			tbl_ProjectTask.ProjectBeginDate<DATEADD(YEAR,1,@BeginDateYear)

		if @TotalCount<>0
		begin
			set @YearOnTimeRate=(@YearOnTimeCount+@YearOverTimeCount)*1.0/@TotalCount*100
		end
		else
		begin
			set @YearOnTimeRate=0
		end

		set @MonthScore=@MonthOnTimeCount*2+@MonthOverTimeCount
		set @YearScore=(@YearOnTimeCount*2+@YearOverTimeCount)*(@YearOnTimeRate)/(100+0.0)

		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@DepartmentName,@FullName,@MonthOnTimeCount,@MonthOverTimeCount,@TotalCount,@YearOnTimeCount,
			@YearOverTimeCount,@YearUnFinishCount,@YearOnTimeRate,@MonthScore,@YearScore)

		fetch next from cur into @DepartmentName,@FullName,@AreaManagerId
	end
	close cur
	deallocate cur
	select * from @temp order by DepartmentName,FullName
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportPlacesBaseStationExcel]    Script Date: 2016-10-24 14:14:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportPlacesBaseStationExcel]
					@PlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@PlaceOwner uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Importance int,
					@State int,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_Place.PlaceCode as '站点编码',tbl_Place.PlaceName as '站点名称',tbl_PlaceCategory.PlaceCategoryName as '站点类型',tbl_Area.AreaName as '区域',
			tbl_Reseau.ReseauName as '网格',tbl_Place.Lng as '经度',tbl_Place.Lat as '纬度',case tbl_Place.Importance when 1 then 'A' when 2 then 'B' when 3 then 'C' else '' end as '重要性程度',
			tbl_PlaceOwner.PlaceOwnerName as '产权',tbl_Place.OwnerName as '业主名称',tbl_Place.OwnerContact as '联系人',tbl_Place.OwnerPhoneNumber as '联系方式',
			case tbl_Place.[State] when 1 then '使用' when 2 then '停用' else '' end as '状态',tbl_User.FullName as '登记人',tbl_Place.CreateDate as '登记日期'
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
						left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
						left join tbl_PlaceOwner on tbl_Place.PlaceOwner=tbl_PlaceOwner.Id
						left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
						left join tbl_Company on tbl_Department.CompanyId=tbl_Company.Id
		where (@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PlaceOwner = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceOwner = @PlaceOwner) and
				(@Importance = 0 or tbl_Place.Importance = @Importance) and
				(@State = 0 or tbl_Place.[State] = @State) and
				tbl_Department.CompanyId = @CompanyId
		order by tbl_Place.PlaceCode desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportPlaceAllExcel]    Script Date: 2016-10-24 13:56:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportPlaceAllExcel]
					@Profession int,
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@PlaceOwner uniqueidentifier,
					@State int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_Place.PlaceCode as '站点编号',case tbl_Place.Profession when 1 then '基站' else '' end as '专业',tbl_Place.PlaceName as '站点名称',
			tbl_PlaceCategory.PlaceCategoryName as '站点类型',tbl_Area.AreaName as '区域',tbl_Reseau.ReseauName as '网格',tbl_Place.Lng as '经度',
			tbl_Place.Lat as '纬度',case tbl_Place.Importance when 1 then 'A' when 2 then 'B' when 3 then 'C' else '' end as '重要性程度',
			tbl_PlaceOwner.PlaceOwnerName as '产权',tbl_Place.OwnerName as '业主名称',tbl_Place.OwnerContact as '联系人',tbl_Place.OwnerPhoneNumber as '联系方式',
			tbl_Place.G2Number as '2G逻辑号',tbl_Place.D2Number as '2D逻辑号',tbl_Place.G3Number as '3G逻辑号',tbl_Place.G4Number as '4G逻辑号',
			tbl_Place.AddressingRealName as '实际租赁人',case tbl_Place.[State] when 1 then '使用' when 2 then '停用' else '' end as '状态',tbl_User.FullName as '登记人',
			tbl_Place.CreateDate as '登记日期'
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
						left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
						left join tbl_PlaceOwner on tbl_Place.PlaceOwner=tbl_PlaceOwner.Id
		where (@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and				
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PlaceOwner = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceOwner = @PlaceOwner) and
				(@State = 0 or tbl_Place.[State] = @State)
		order by tbl_Place.PlaceCode desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetReports]    Script Date: 2016-10-17 21:11:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetReports]
				@UserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),TaskTypeName varchar(50),PageUrl varchar(1000),SortId int)

	insert @temp values('网格业务月清单','BaseStationReport/BusinessVolumeMonthReseau',1)
	insert @temp values('区域业务月清单','BaseStationReport/BusinessVolumeMonthArea',2)
	insert @temp values('公司业务月清单','BaseStationReport/BusinessVolumeMonthCompany',3)

	select TaskTypeName,PageUrl from @temp order by SortId
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeYearGrowthCompany]    Script Date: 2016-10-17 21:10:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeYearGrowthCompany]
					@BeginDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CompanyName varchar(50),
		JanTV decimal(18,2),JanBV decimal(18,2),FebTV decimal(18,2),FebBV decimal(18,2),MarTV decimal(18,2),MarBV decimal(18,2),AprTV decimal(18,2),AprBV decimal(18,2),MayTV decimal(18,2),MayBV decimal(18,2),
		JunTV decimal(18,2),JunBV decimal(18,2),JulTV decimal(18,2),JulBV decimal(18,2),AugTV decimal(18,2),AugBV decimal(18,2),SeptTV decimal(18,2),SeptBV decimal(18,2),OctTV decimal(18,2),OctBV decimal(18,2),
		NovTV decimal(18,2),NovBV decimal(18,2),DecTV decimal(18,2),DecBV decimal(18,2))

	declare @CPId uniqueidentifier,
			@CompanyName varchar(50),
			@JanTV decimal(18,3),
			@JanBV decimal(18,3),
			@FebTV decimal(18,3),
			@FebBV decimal(18,3),
			@MarTV decimal(18,3),
			@MarBV decimal(18,3),
			@AprTV decimal(18,3),
			@AprBV decimal(18,3),
			@MayTV decimal(18,3),
			@MayBV decimal(18,3),
			@JunTV decimal(18,3),
			@JunBV decimal(18,3),
			@JulTV decimal(18,3),
			@JulBV decimal(18,3),
			@AugTV decimal(18,3),
			@AugBV decimal(18,3),
			@SeptTV decimal(18,3),
			@SeptBV decimal(18,3),
			@OctTV decimal(18,3),
			@OctBV decimal(18,3),
			@NovTV decimal(18,3),
			@NovBV decimal(18,3),
			@DecTV decimal(18,3),
			@DecBV decimal(18,3)
	declare cur cursor for
	select tbl_Company.Id,tbl_Company.CompanyName from tbl_Company
		where tbl_Company.Id=@CompanyId order by tbl_Company.CompanyCode
							
	open cur
	fetch next from cur into @CPId,@CompanyName
	while @@fetch_status = 0
	begin
		set @JanTV=0
		set @JanBV=0
		set @FebTV=0
		set @FebBV=0
		set @MarTV=0
		set @MarBV=0
		set @AprTV=0
		set @AprBV=0
		set @MayTV=0
		set @MayBV=0
		set	@JunTV=0
		set @JunBV=0
		set @JulTV=0
		set @JulBV=0
		set @AugTV=0
		set @AugBV=0
		set @SeptTV=0
		set @SeptBV=0
		set @OctTV=0
		set @OctBV=0
		set @NovTV=0
		set @NovBV=0
		set @DecTV=0
		set @DecBV=0
		select @JanTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JanBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,1,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @FebTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@FebBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,2,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MarTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MarBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,2,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,3,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AprTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AprBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,3,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,4,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MayTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MayBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,4,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,5,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JunTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JunBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,5,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,6,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JulTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JulBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,6,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,7,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AugTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AugBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,7,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,8,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @SeptTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@SeptBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,8,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,9,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @OctTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@OctBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,9,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,10,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @NovTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@NovBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,10,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,11,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @DecTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@DecBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId=tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,11,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,12,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		insert @temp values(@CompanyName,@JanTV/10000,@JanBV/1000,@FebTV/10000,@FebBV/1000,@MarTV/10000,@MarBV/1000,@AprTV/10000,@AprBV/1000,@MayTV/10000,@MayBV/1000,
			@JunTV/10000,@JunBV/1000,@JulTV/10000,@JulBV/1000,@AugTV/10000,@AugBV/1000,@SeptTV/10000,@SeptBV/1000,@OctTV/10000,@OctBV/1000,@NovTV/10000,@NovBV/1000,@DecTV/10000,@DecBV/1000)
		fetch next from cur into @CPId,@CompanyName
	end
	close cur
	deallocate cur

	select * from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeYearGrowthArea]    Script Date: 2016-10-17 21:10:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeYearGrowthArea]
					@BeginDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),AreaName varchar(50),
		JanTV decimal(18,2),JanBV decimal(18,2),FebTV decimal(18,2),FebBV decimal(18,2),MarTV decimal(18,2),MarBV decimal(18,2),AprTV decimal(18,2),AprBV decimal(18,2),MayTV decimal(18,2),MayBV decimal(18,2),
		JunTV decimal(18,2),JunBV decimal(18,2),JulTV decimal(18,2),JulBV decimal(18,2),AugTV decimal(18,2),AugBV decimal(18,2),SeptTV decimal(18,2),SeptBV decimal(18,2),OctTV decimal(18,2),OctBV decimal(18,2),
		NovTV decimal(18,2),NovBV decimal(18,2),DecTV decimal(18,2),DecBV decimal(18,2))

	declare @AreaId uniqueidentifier,
			@AreaName varchar(50),
			@JanTV decimal(18,3),
			@JanBV decimal(18,3),
			@FebTV decimal(18,3),
			@FebBV decimal(18,3),
			@MarTV decimal(18,3),
			@MarBV decimal(18,3),
			@AprTV decimal(18,3),
			@AprBV decimal(18,3),
			@MayTV decimal(18,3),
			@MayBV decimal(18,3),
			@JunTV decimal(18,3),
			@JunBV decimal(18,3),
			@JulTV decimal(18,3),
			@JulBV decimal(18,3),
			@AugTV decimal(18,3),
			@AugBV decimal(18,3),
			@SeptTV decimal(18,3),
			@SeptBV decimal(18,3),
			@OctTV decimal(18,3),
			@OctBV decimal(18,3),
			@NovTV decimal(18,3),
			@NovBV decimal(18,3),
			@DecTV decimal(18,3),
			@DecBV decimal(18,3)
	declare cur cursor for
	select tbl_Area.Id,tbl_Area.AreaName from tbl_Area left join tbl_User on tbl_Area.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where tbl_Department.CompanyId=@CompanyId order by tbl_Area.AreaCode
							
	open cur
	fetch next from cur into @AreaId,@AreaName
	while @@fetch_status = 0
	begin
		set @JanTV=0
		set @JanBV=0
		set @FebTV=0
		set @FebBV=0
		set @MarTV=0
		set @MarBV=0
		set @AprTV=0
		set @AprBV=0
		set @MayTV=0
		set @MayBV=0
		set	@JunTV=0
		set @JunBV=0
		set @JulTV=0
		set @JulBV=0
		set @AugTV=0
		set @AugBV=0
		set @SeptTV=0
		set @SeptBV=0
		set @OctTV=0
		set @OctBV=0
		set @NovTV=0
		set @NovBV=0
		set @DecTV=0
		set @DecBV=0
		select @JanTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JanBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,1,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @FebTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@FebBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,2,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MarTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MarBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,2,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,3,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AprTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AprBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,3,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,4,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MayTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MayBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,4,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,5,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JunTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JunBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,5,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,6,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JulTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JulBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,6,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,7,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AugTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AugBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,7,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,8,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @SeptTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@SeptBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,8,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,9,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @OctTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@OctBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,9,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,10,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @NovTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@NovBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,10,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,11,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @DecTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@DecBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,11,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,12,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		insert @temp values(@AreaName,@JanTV/10000,@JanBV/1000,@FebTV/10000,@FebBV/1000,@MarTV/10000,@MarBV/1000,@AprTV/10000,@AprBV/1000,@MayTV/10000,@MayBV/1000,
			@JunTV/10000,@JunBV/1000,@JulTV/10000,@JulBV/1000,@AugTV/10000,@AugBV/1000,@SeptTV/10000,@SeptBV/1000,@OctTV/10000,@OctBV/1000,@NovTV/10000,@NovBV/1000,@DecTV/10000,@DecBV/1000)
		fetch next from cur into @AreaId,@AreaName
	end
	close cur
	deallocate cur

	select * from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeYearGrowthReseau]    Script Date: 2016-10-17 21:09:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeYearGrowthReseau]
					@BeginDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),AreaName varchar(50),ReseauName varchar(50),
		JanTV decimal(18,2),JanBV decimal(18,2),FebTV decimal(18,2),FebBV decimal(18,2),MarTV decimal(18,2),MarBV decimal(18,2),AprTV decimal(18,2),AprBV decimal(18,2),MayTV decimal(18,2),MayBV decimal(18,2),
		JunTV decimal(18,2),JunBV decimal(18,2),JulTV decimal(18,2),JulBV decimal(18,2),AugTV decimal(18,2),AugBV decimal(18,2),SeptTV decimal(18,2),SeptBV decimal(18,2),OctTV decimal(18,2),OctBV decimal(18,2),
		NovTV decimal(18,2),NovBV decimal(18,2),DecTV decimal(18,2),DecBV decimal(18,2))

	declare @ReseauId uniqueidentifier,
			@ReseauName varchar(50),
			@AreaName varchar(50),
			@JanTV decimal(18,3),
			@JanBV decimal(18,3),
			@FebTV decimal(18,3),
			@FebBV decimal(18,3),
			@MarTV decimal(18,3),
			@MarBV decimal(18,3),
			@AprTV decimal(18,3),
			@AprBV decimal(18,3),
			@MayTV decimal(18,3),
			@MayBV decimal(18,3),
			@JunTV decimal(18,3),
			@JunBV decimal(18,3),
			@JulTV decimal(18,3),
			@JulBV decimal(18,3),
			@AugTV decimal(18,3),
			@AugBV decimal(18,3),
			@SeptTV decimal(18,3),
			@SeptBV decimal(18,3),
			@OctTV decimal(18,3),
			@OctBV decimal(18,3),
			@NovTV decimal(18,3),
			@NovBV decimal(18,3),
			@DecTV decimal(18,3),
			@DecBV decimal(18,3)
	declare cur cursor for
	select tbl_Reseau.Id,tbl_Reseau.ReseauName,tbl_Area.AreaName from tbl_Reseau left join tbl_User on tbl_Reseau.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
		where tbl_Department.CompanyId=@CompanyId order by tbl_Reseau.ReseauCode
							
	open cur
	fetch next from cur into @ReseauId,@ReseauName,@AreaName
	while @@fetch_status = 0
	begin
		set @JanTV=0
		set @JanBV=0
		set @FebTV=0
		set @FebBV=0
		set @MarTV=0
		set @MarBV=0
		set @AprTV=0
		set @AprBV=0
		set @MayTV=0
		set @MayBV=0
		set	@JunTV=0
		set @JunBV=0
		set @JulTV=0
		set @JulBV=0
		set @AugTV=0
		set @AugBV=0
		set @SeptTV=0
		set @SeptBV=0
		set @OctTV=0
		set @OctBV=0
		set @NovTV=0
		set @NovBV=0
		set @DecTV=0
		set @DecBV=0
		select @JanTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JanBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,1,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @FebTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@FebBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,2,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MarTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MarBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,2,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,3,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AprTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AprBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,3,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,4,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @MayTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@MayBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,4,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,5,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JunTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JunBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,5,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,6,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @JulTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@JulBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,6,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,7,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @AugTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@AugBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,7,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,8,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @SeptTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@SeptBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,8,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,9,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @OctTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@OctBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,9,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,10,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @NovTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@NovBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,10,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,11,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @DecTV=ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0),
			@DecBV=ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,11,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,12,@BeginDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		insert @temp values(@AreaName,@ReseauName,@JanTV/10000,@JanBV/1000,@FebTV/10000,@FebBV/1000,@MarTV/10000,@MarBV/1000,@AprTV/10000,@AprBV/1000,@MayTV/10000,@MayBV/1000,
			@JunTV/10000,@JunBV/1000,@JulTV/10000,@JulBV/1000,@AugTV/10000,@AugBV/1000,@SeptTV/10000,@SeptBV/1000,@OctTV/10000,@OctBV/1000,@NovTV/10000,@NovBV/1000,@DecTV/10000,@DecBV/1000)
		fetch next from cur into @ReseauId,@ReseauName,@AreaName
	end
	close cur
	deallocate cur

	select * from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeYearRiseCompany]    Script Date: 2016-10-14 23:02:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeYearRiseCompany]
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),CompanyName varchar(50),
		TotalTV decimal(18,2),TVRiseYear decimal(18,2),TotalBV decimal(18,2),BVRiseYear decimal(18,2))

	declare @CPId uniqueidentifier,
			@CompanyName varchar(50),
			@TotalTV decimal(18,3),
			@TotalTVYear decimal(18,3),
			@TotalBV decimal(18,3),
			@TotalBVYear decimal(18,3),
			@TotalTVYearRate decimal(18,3),
			@TotalBVYearRate decimal(18,3)

	declare cur cursor for
	select tbl_Company.Id,tbl_Company.CompanyName from tbl_Company
		where tbl_Company.Id=@CompanyId
	open cur
	fetch next from cur into @CPId,@CompanyName
	while @@fetch_status = 0
	begin
		set @TotalTV=0
		set @TotalTVYear=0
		set @TotalBV=0
		set @TotalBVYear=0
		set	@TotalTVYearRate=0
		set	@TotalBVYearRate=0

		select @TotalTV=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBV=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVYear=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVYear=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(YEAR,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(YEAR,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		if @TotalTV=0
		begin
			set @TotalTVYearRate=0
		end
		else
		begin
			if @TotalTVYear=0
			begin
				set @TotalTVYearRate=0
			end
			else
			begin
				set @TotalTVYearRate=(@TotalTV*1.0/@TotalTVYear-1)*100
			end
			set @TotalTV=@TotalTV/10000
		end

		if @TotalBV=0
		begin
			set @TotalBVYearRate=0
		end
		else
		begin
			if @TotalBVYear=0
			begin
				set @TotalBVYearRate=0
			end
			else
			begin
				set @TotalBVYearRate=(@TotalBV*1.0/@TotalBVYear-1)*100
			end
			set @TotalBV=@TotalBV/1000
		end
		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@CompanyName,@TotalTV,@TotalTVYearRate,
			@TotalBV,@TotalBVYearRate)
		
		fetch next from cur into @CPId,@CompanyName
	end
	close cur
	deallocate cur
	select * from @temp order by CompanyName
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeMonthRiseCompany]    Script Date: 2016-10-14 23:02:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeMonthRiseCompany]
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),CompanyName varchar(50),
		TotalTV decimal(18,2),TVRiseMonth decimal(18,2),TVRiseYear decimal(18,2),TotalBV decimal(18,2),BVRiseMonth decimal(18,2),BVRiseYear decimal(18,2))

	declare @CPId uniqueidentifier,
			@CompanyName varchar(50),
			@TotalTV decimal(18,3),
			@TotalTVMonth decimal(18,3),
			@TotalTVYear decimal(18,3),
			@TotalBV decimal(18,3),
			@TotalBVMonth decimal(18,3),
			@TotalBVYear decimal(18,3),
			@TotalTVMonthRate decimal(18,3),
			@TotalTVYearRate decimal(18,3),
			@TotalBVMonthRate decimal(18,3),
			@TotalBVYearRate decimal(18,3)

	declare cur cursor for
	select tbl_Company.Id,tbl_Company.CompanyName from tbl_Company
		where tbl_Company.Id=@CompanyId
	open cur
	fetch next from cur into @CPId,@CompanyName
	while @@fetch_status = 0
	begin
		set @TotalTV=0
		set @TotalTVMonth=0
		set @TotalTVYear=0
		set @TotalBV=0
		set @TotalBVMonth=0
		set @TotalBVYear=0
		set @TotalTVMonthRate=0
		set	@TotalTVYearRate=0
		set	@TotalBVMonthRate=0
		set	@TotalBVYearRate=0

		select @TotalTV=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBV=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVMonth=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVMonth=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVYear=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVYear=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Department.CompanyId = @CPId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(YEAR,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(YEAR,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		if @TotalTV=0
		begin
			set @TotalTVMonthRate=0
			set @TotalTVYearRate=0
		end
		else
		begin
			if @TotalTVMonth=0
			begin			
				set @TotalTVMonthRate=0
			end
			else
			begin			
				set @TotalTVMonthRate=(@TotalTV*1.0/@TotalTVMonth-1)*100
			end
			if @TotalTVYear=0
			begin
				set @TotalTVYearRate=0
			end
			else
			begin
				set @TotalTVYearRate=(@TotalTV*1.0/@TotalTVYear-1)*100
			end
			set @TotalTV=@TotalTV/10000
		end

		if @TotalBV=0
		begin
			set @TotalBVMonthRate=0
			set @TotalBVYearRate=0
		end
		else
		begin
			if @TotalBVMonth=0
			begin			
				set @TotalBVMonthRate=0
			end
			else
			begin			
				set @TotalBVMonthRate=(@TotalBV*1.0/@TotalBVMonth-1)*100
			end
			if @TotalBVYear=0
			begin
				set @TotalBVYearRate=0
			end
			else
			begin
				set @TotalBVYearRate=(@TotalBV*1.0/@TotalBVYear-1)*100
			end
			set @TotalBV=@TotalBV/1000
		end
		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@CompanyName,@TotalTV,@TotalTVMonthRate,@TotalTVYearRate,
			@TotalBV,@TotalBVMonthRate,@TotalBVYearRate)
		
		fetch next from cur into @CPId,@CompanyName
	end
	close cur
	deallocate cur
	select * from @temp order by CompanyName
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeYearRiseArea]    Script Date: 2016-10-14 23:01:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeYearRiseArea]
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),
		TotalTV decimal(18,2),TVRiseYear decimal(18,2),TotalBV decimal(18,2),BVRiseYear decimal(18,2))

	declare @AreaId uniqueidentifier,
			@AreaName varchar(50),
			@TotalTV decimal(18,3),
			@TotalTVYear decimal(18,3),
			@TotalBV decimal(18,3),
			@TotalBVYear decimal(18,3),
			@TotalTVYearRate decimal(18,3),
			@TotalBVYearRate decimal(18,3)

	declare cur cursor for
	select tbl_Area.Id,tbl_Area.AreaName from tbl_Area left join tbl_User on tbl_Area.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where tbl_Department.CompanyId=@CompanyId order by tbl_Area.AreaCode
	open cur
	fetch next from cur into @AreaId,@AreaName
	while @@fetch_status = 0
	begin
		set @TotalTV=0
		set @TotalTVYear=0
		set @TotalBV=0
		set @TotalBVYear=0
		set	@TotalTVYearRate=0
		set	@TotalBVYearRate=0

		select @TotalTV=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBV=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVYear=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVYear=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(YEAR,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(YEAR,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		
		if @TotalTV=0
		begin
			set @TotalTVYearRate=0
		end
		else
		begin
			if @TotalTVYear=0
			begin
				set @TotalTVYearRate=0
			end
			else
			begin
				set @TotalTVYearRate=(@TotalTV*1.0/@TotalTVYear-1)*100
			end
			set @TotalTV=@TotalTV/10000
		end

		if @TotalBV=0
		begin
			set @TotalBVYearRate=0
		end
		else
		begin
			if @TotalBVYear=0
			begin
				set @TotalBVYearRate=0
			end
			else
			begin
				set @TotalBVYearRate=(@TotalBV*1.0/@TotalBVYear-1)*100
			end
			set @TotalBV=@TotalBV/1000
		end
		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@AreaName,@TotalTV,@TotalTVYearRate,
			@TotalBV,@TotalBVYearRate)
		
		fetch next from cur into @AreaId,@AreaName
	end
	close cur
	deallocate cur
	select * from @temp order by AreaName
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeMonthRiseArea]    Script Date: 2016-10-14 23:01:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeMonthRiseArea]
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),
		TotalTV decimal(18,2),TVRiseMonth decimal(18,2),TVRiseYear decimal(18,2),TotalBV decimal(18,2),BVRiseMonth decimal(18,2),BVRiseYear decimal(18,2))

	declare @AreaId uniqueidentifier,
			@AreaName varchar(50),
			@TotalTV decimal(18,3),
			@TotalTVMonth decimal(18,3),
			@TotalTVYear decimal(18,3),
			@TotalBV decimal(18,3),
			@TotalBVMonth decimal(18,3),
			@TotalBVYear decimal(18,3),
			@TotalTVMonthRate decimal(18,3),
			@TotalTVYearRate decimal(18,3),
			@TotalBVMonthRate decimal(18,3),
			@TotalBVYearRate decimal(18,3)

	declare cur cursor for
	select tbl_Area.Id,tbl_Area.AreaName from tbl_Area left join tbl_User on tbl_Area.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where tbl_Department.CompanyId=@CompanyId order by tbl_Area.AreaCode
	open cur
	fetch next from cur into @AreaId,@AreaName
	while @@fetch_status = 0
	begin
		set @TotalTV=0
		set @TotalTVMonth=0
		set @TotalTVYear=0
		set @TotalBV=0
		set @TotalBVMonth=0
		set @TotalBVYear=0
		set @TotalTVMonthRate=0
		set	@TotalTVYearRate=0
		set	@TotalBVMonthRate=0
		set	@TotalBVYearRate=0

		select @TotalTV=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBV=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVMonth=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVMonth=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVYear=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVYear=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Reseau.AreaId = @AreaId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(YEAR,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(YEAR,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		if @TotalTV=0
		begin
			set @TotalTVMonthRate=0
			set @TotalTVYearRate=0
		end
		else
		begin
			if @TotalTVMonth=0
			begin			
				set @TotalTVMonthRate=0
			end
			else
			begin			
				set @TotalTVMonthRate=(@TotalTV*1.0/@TotalTVMonth-1)*100
			end
			if @TotalTVYear=0
			begin
				set @TotalTVYearRate=0
			end
			else
			begin
				set @TotalTVYearRate=(@TotalTV*1.0/@TotalTVYear-1)*100
			end
			set @TotalTV=@TotalTV/10000
		end

		if @TotalBV=0
		begin
			set @TotalBVMonthRate=0
			set @TotalBVYearRate=0
		end
		else
		begin
			if @TotalBVMonth=0
			begin			
				set @TotalBVMonthRate=0
			end
			else
			begin			
				set @TotalBVMonthRate=(@TotalBV*1.0/@TotalBVMonth-1)*100
			end
			if @TotalBVYear=0
			begin
				set @TotalBVYearRate=0
			end
			else
			begin
				set @TotalBVYearRate=(@TotalBV*1.0/@TotalBVYear-1)*100
			end
			set @TotalBV=@TotalBV/1000
		end
		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@AreaName,@TotalTV,@TotalTVMonthRate,@TotalTVYearRate,
			@TotalBV,@TotalBVMonthRate,@TotalBVYearRate)
		
		fetch next from cur into @AreaId,@AreaName
	end
	close cur
	deallocate cur
	select * from @temp order by AreaName
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeYearRiseReseau]    Script Date: 2016-10-14 22:40:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeYearRiseReseau]
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),ReseauName varchar(50),
		TotalTV decimal(18,2),TVRiseYear decimal(18,2),TotalBV decimal(18,2),BVRiseYear decimal(18,2))

	declare @ReseauId uniqueidentifier,
			@AreaName varchar(50),
			@ReseauName varchar(50),
			@TotalTV decimal(18,3),
			@TotalTVYear decimal(18,3),
			@TotalBV decimal(18,3),
			@TotalBVYear decimal(18,3),
			@TotalTVYearRate decimal(18,3),
			@TotalBVYearRate decimal(18,3)

	declare cur cursor for
	select tbl_Reseau.Id,tbl_Area.AreaName,tbl_Reseau.ReseauName from tbl_Reseau left join tbl_User on tbl_Reseau.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
		where tbl_Department.CompanyId=@CompanyId order by tbl_Reseau.ReseauCode
	open cur
	fetch next from cur into @ReseauId,@AreaName,@ReseauName
	while @@fetch_status = 0
	begin
		set @TotalTV=0
		set @TotalTVYear=0
		set @TotalBV=0
		set @TotalBVYear=0
		set	@TotalTVYearRate=0
		set	@TotalBVYearRate=0

		select @TotalTV=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBV=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVYear=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVYear=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(YEAR,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(YEAR,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		if @TotalTV=0
		begin
			set @TotalTVYearRate=0
		end
		else
		begin
			if @TotalTVYear=0
			begin
				set @TotalTVYearRate=0
			end
			else
			begin
				set @TotalTVYearRate=(@TotalTV*1.0/@TotalTVYear-1)*100
			end
			set @TotalTV=@TotalTV/10000
		end

		if @TotalBV=0
		begin
			set @TotalBVYearRate=0
		end
		else
		begin
			if @TotalBVYear=0
			begin
				set @TotalBVYearRate=0
			end
			else
			begin
				set @TotalBVYearRate=(@TotalBV*1.0/@TotalBVYear-1)*100
			end
			set @TotalBV=@TotalBV/1000
		end
		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@AreaName,@ReseauName,@TotalTV,@TotalTVYearRate,
			@TotalBV,@TotalBVYearRate)
		
		fetch next from cur into @ReseauId,@AreaName,@ReseauName
	end
	close cur
	deallocate cur
	select * from @temp order by AreaName,ReseauName
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeMonthRiseReseau]    Script Date: 2016-10-14 15:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeMonthRiseReseau]
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),ReseauName varchar(50),
		TotalTV decimal(18,2),TVRiseMonth decimal(18,2),TVRiseYear decimal(18,2),TotalBV decimal(18,2),BVRiseMonth decimal(18,2),BVRiseYear decimal(18,2))

	declare @ReseauId uniqueidentifier,
			@AreaName varchar(50),
			@ReseauName varchar(50),
			@TotalTV decimal(18,3),
			@TotalTVMonth decimal(18,3),
			@TotalTVYear decimal(18,3),
			@TotalBV decimal(18,3),
			@TotalBVMonth decimal(18,3),
			@TotalBVYear decimal(18,3),
			@TotalTVMonthRate decimal(18,3),
			@TotalTVYearRate decimal(18,3),
			@TotalBVMonthRate decimal(18,3),
			@TotalBVYearRate decimal(18,3)

	declare cur cursor for
	select tbl_Reseau.Id,tbl_Area.AreaName,tbl_Reseau.ReseauName from tbl_Reseau left join tbl_User on tbl_Reseau.CreateUserId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
		where tbl_Department.CompanyId=@CompanyId order by tbl_Reseau.ReseauCode
	open cur
	fetch next from cur into @ReseauId,@AreaName,@ReseauName
	while @@fetch_status = 0
	begin
		set @TotalTV=0
		set @TotalTVMonth=0
		set @TotalTVYear=0
		set @TotalBV=0
		set @TotalBVMonth=0
		set @TotalBVYear=0
		set @TotalTVMonthRate=0
		set	@TotalTVYearRate=0
		set	@TotalBVMonthRate=0
		set	@TotalBVYearRate=0

		select @TotalTV=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBV=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVMonth=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVMonth=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(MONTH,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(MONTH,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		select @TotalTVYear=(ISNULL(SUM(g2.TrafficVolumes),0)+ISNULL(SUM(d2.TrafficVolumes),0)+ISNULL(SUM(g3.TrafficVolumes),0)),
			@TotalBVYear=(ISNULL(SUM(g2.BusinessVolumes),0)+ISNULL(SUM(d2.BusinessVolumes),0)+ISNULL(SUM(g3.BusinessVolumes),0)+ISNULL(SUM(g4.BusinessVolumes),0))
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where tbl_Place.ReseauId = @ReseauId and (@Profession=0 or tbl_Place.Profession=@Profession) and
			(tbl_PlaceBusinessVolume.CreateDate>=DATEADD(YEAR,-1,@BeginDate) and tbl_PlaceBusinessVolume.CreateDate<DATEADD(YEAR,-1,@EndDate)) and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

		if @TotalTV=0
		begin
			set @TotalTVMonthRate=0
			set @TotalTVYearRate=0
		end
		else
		begin
			if @TotalTVMonth=0
			begin			
				set @TotalTVMonthRate=0
			end
			else
			begin			
				set @TotalTVMonthRate=(@TotalTV*1.0/@TotalTVMonth-1)*100
			end
			if @TotalTVYear=0
			begin
				set @TotalTVYearRate=0
			end
			else
			begin
				set @TotalTVYearRate=(@TotalTV*1.0/@TotalTVYear-1)*100
			end
			set @TotalTV=@TotalTV/10000
		end

		if @TotalBV=0
		begin
			set @TotalBVMonthRate=0
			set @TotalBVYearRate=0
		end
		else
		begin
			if @TotalBVMonth=0
			begin			
				set @TotalBVMonthRate=0
			end
			else
			begin			
				set @TotalBVMonthRate=(@TotalBV*1.0/@TotalBVMonth-1)*100
			end
			if @TotalBVYear=0
			begin
				set @TotalBVYearRate=0
			end
			else
			begin
				set @TotalBVYearRate=(@TotalBV*1.0/@TotalBVYear-1)*100
			end
			set @TotalBV=@TotalBV/1000
		end
		insert @temp values(CONVERT(nvarchar(6),@BeginDate,112),@AreaName,@ReseauName,@TotalTV,@TotalTVMonthRate,@TotalTVYearRate,
			@TotalBV,@TotalBVMonthRate,@TotalBVYearRate)
		
		fetch next from cur into @ReseauId,@AreaName,@ReseauName
	end
	close cur
	deallocate cur
	select * from @temp order by AreaName,ReseauName
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeMonthCompany]    Script Date: 2016-10-14 13:29:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeMonthCompany]
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),CompanyName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),
		G3BV decimal(18,2),G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select CONVERT(nvarchar(6),tbl_PlaceBusinessVolume.CreateDate,112),tbl_Company.CompanyName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_Company on tbl_Department.CompanyId=tbl_Company.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		order by tbl_PlaceBusinessVolume.CreateDate desc,tbl_Place.PlaceCode desc

		select CreateDate,CompanyName,SUM(G2TV) as G2TV,SUM(G2BV) as G2BV,SUM(D2TV) as D2TV,SUM(D2BV) as D2BV,SUM(G3TV) as G3TV,SUM(G3BV) as G3BV,
			SUM(G4BV) as G4BV,SUM(TotalTV) as TotalTV,SUM(TotalBV) as TotalBV 
			from @temp group by CreateDate,CompanyName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeMonthArea]    Script Date: 2016-10-14 13:28:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeMonthArea]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),
		G3BV decimal(18,2),G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select CONVERT(nvarchar(6),tbl_PlaceBusinessVolume.CreateDate,112),tbl_Area.AreaName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

	select CreateDate,AreaName,SUM(G2TV) as G2TV,SUM(G2BV) as G2BV,SUM(D2TV) as D2TV,SUM(D2BV) as D2BV,SUM(G3TV) as G3TV,SUM(G3BV) as G3BV,
		SUM(G4BV) as G4BV,SUM(TotalTV) as TotalTV,SUM(TotalBV) as TotalBV 
		from @temp group by CreateDate,AreaName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeMonthReseau]    Script Date: 2016-10-14 13:24:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeMonthReseau]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate nvarchar(6),AreaName varchar(50),ReseauName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),G3BV decimal(18,2),
		G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select CONVERT(nvarchar(6),tbl_PlaceBusinessVolume.CreateDate,112),tbl_Area.AreaName,tbl_Reseau.ReseauName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')

	select CreateDate,AreaName,ReseauName,SUM(G2TV) as G2TV,SUM(G2BV) as G2BV,SUM(D2TV) as D2TV,SUM(D2BV) as D2BV,SUM(G3TV) as G3TV,SUM(G3BV) as G3BV,
		SUM(G4BV) as G4BV,SUM(TotalTV) as TotalTV,SUM(TotalBV) as TotalBV 
		from @temp group by CreateDate,AreaName,ReseauName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeCompany]    Script Date: 2016-10-14 11:08:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeCompany]
					@BeginDate datetime,
					@EndDate datetime,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate datetime,CompanyName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),G3BV decimal(18,2),
		G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select tbl_PlaceBusinessVolume.CreateDate,tbl_Company.CompanyName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_Company on tbl_Department.CompanyId=tbl_Company.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		order by tbl_PlaceBusinessVolume.CreateDate desc,tbl_Place.PlaceCode desc

		select CreateDate,CompanyName,SUM(G2TV) as G2TV,SUM(G2BV) as G2BV,SUM(D2TV) as D2TV,SUM(D2BV) as D2BV,SUM(G3TV) as G3TV,SUM(G3BV) as G3BV,
			SUM(G4BV) as G4BV,SUM(TotalTV) as TotalTV,SUM(TotalBV) as TotalBV 
			from @temp group by CreateDate,CompanyName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeArea]    Script Date: 2016-10-14 11:07:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeArea]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate datetime,AreaId uniqueidentifier,AreaName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),G3BV decimal(18,2),
		G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select tbl_PlaceBusinessVolume.CreateDate,tbl_Area.Id,tbl_Area.AreaName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		order by tbl_PlaceBusinessVolume.CreateDate desc,tbl_Place.PlaceCode desc

		select CreateDate,AreaName,SUM(G2TV) as G2TV,SUM(G2BV) as G2BV,SUM(D2TV) as D2TV,SUM(D2BV) as D2BV,SUM(G3TV) as G3TV,SUM(G3BV) as G3BV,
			SUM(G4BV) as G4BV,SUM(TotalTV) as TotalTV,SUM(TotalBV) as TotalBV 
			from @temp group by CreateDate,AreaName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumeReseau]    Script Date: 2016-10-14 09:59:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeReseau]
					@BeginDate datetime,
					@EndDate datetime,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @temp table(indexid int identity(1,1),CreateDate datetime,AreaName varchar(50),ReseauName varchar(50),
		G2TV decimal(18,2),G2BV decimal(18,2),D2TV decimal(18,2),D2BV decimal(18,2),G3TV decimal(18,2),G3BV decimal(18,2),
		G4BV decimal(18,2),TotalTV decimal(18,2),TotalBV decimal(18,2))

	insert @temp
	select tbl_PlaceBusinessVolume.CreateDate,tbl_Area.AreaName,tbl_Reseau.ReseauName,
	ISNULL(g2.TrafficVolumes,0)/10000 as G2TV,
	ISNULL(g2.BusinessVolumes,0)/1000 as G2BV,
	ISNULL(d2.TrafficVolumes,0)/10000 as D2TV,
	ISNULL(d2.BusinessVolumes,0)/1000 as D2BV,
	ISNULL(g3.TrafficVolumes,0)/10000 as G3TV,
	ISNULL(g3.BusinessVolumes,0)/1000 as G3BV,
	ISNULL(g4.BusinessVolumes,0)/1000 as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0))/10000 as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0))/1000 as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId and
			(tbl_PlaceBusinessVolume.G2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.D2BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or 
			tbl_PlaceBusinessVolume.G3BusinessVolumeId<>'00000000-0000-0000-0000-000000000000' or
			tbl_PlaceBusinessVolume.G4BusinessVolumeId<>'00000000-0000-0000-0000-000000000000')
		order by tbl_PlaceBusinessVolume.CreateDate desc,tbl_Place.PlaceCode desc


		select CreateDate,AreaName,ReseauName,SUM(G2TV) as G2TV,SUM(G2BV) as G2BV,SUM(D2TV) as D2TV,SUM(D2BV) as D2BV,SUM(G3TV) as G3TV,SUM(G3BV) as G3BV,
			SUM(G4BV) as G4BV,SUM(TotalTV) as TotalTV,SUM(TotalBV) as TotalBV 
			from @temp group by CreateDate,AreaName,ReseauName order by CreateDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryNoticesPage]    Script Date: 2016-10-09 11:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryNoticesPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@NoticeContent nvarchar(150),
					@NoticeState int,
					@ReceiveUserId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Notice.Id,tbl_Notice.NoticeState,tbl_Notice.CreateDate,tbl_User.FullName as CreateFullName,tbl_Notice.NoticeType,tbl_Notice.NoticeContent,
		'' as NoticeStateName,tbl_Notice.ModifyDate
		from tbl_Notice left join tbl_User on tbl_Notice.CreateUserId = tbl_User.Id
		where (tbl_Notice.CreateDate >= @BeginDate and tbl_Notice.CreateDate < @EndDate) and
				(@NoticeContent = '' or CHARINDEX(@NoticeContent,tbl_Notice.NoticeContent) > 0) and
				(@NoticeState = 0 or tbl_Notice.NoticeState = @NoticeState) and
				(@ReceiveUserId = '00000000-0000-0000-0000-000000000000' or tbl_Notice.ReceiveUserId = @ReceiveUserId)
		order by tbl_Notice.CreateDate desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Notice left join tbl_User on tbl_Notice.CreateUserId = tbl_User.Id
		where (tbl_Notice.CreateDate >= @BeginDate and tbl_Notice.CreateDate < @EndDate) and
				(@NoticeContent = '' or CHARINDEX(@NoticeContent,tbl_Notice.NoticeContent) > 0) and
				(@NoticeState = 0 or tbl_Notice.NoticeState = @NoticeState) and
				(@ReceiveUserId = '00000000-0000-0000-0000-000000000000' or tbl_Notice.ReceiveUserId = @ReceiveUserId)
END
GO
/****** Object:  StoredProcedure [dbo].[[prc_QueryBusinessVolumeReportPage]]    Script Date: 2016-09-29 20:36:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumeReportPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_PlaceBusinessVolume.CreateDate,tbl_Place.Id,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
	ISNULL(g2.TrafficVolumes,0) as G2TV,
	ISNULL(g2.BusinessVolumes,0) as G2BV,
	ISNULL(d2.TrafficVolumes,0) as D2TV,
	ISNULL(d2.BusinessVolumes,0) as D2BV,
	ISNULL(g3.TrafficVolumes,0) as G3TV,
	ISNULL(g3.BusinessVolumes,0) as G3BV,
	ISNULL(g4.BusinessVolumes,0) as G4BV,
	(ISNULL(g2.TrafficVolumes,0)+ISNULL(d2.TrafficVolumes,0)+ISNULL(g3.TrafficVolumes,0)) as TotalTV,
	(ISNULL(g2.BusinessVolumes,0)+ISNULL(d2.BusinessVolumes,0)+ISNULL(g3.BusinessVolumes,0)+ISNULL(g4.BusinessVolumes,0)) as TotalBV
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
			(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId
		order by tbl_PlaceBusinessVolume.CreateDate desc,tbl_Place.PlaceCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_PlaceBusinessVolume left join tbl_Place on tbl_PlaceBusinessVolume.PlaceId=tbl_Place.Id 
					left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
					left join tbl_Area on tbl_Reseau.AreaId=tbl_Area.Id
					left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
					left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
					left join tbl_BusinessVolume g2 on tbl_PlaceBusinessVolume.G2BusinessVolumeId=g2.Id
					left join tbl_BusinessVolume d2 on tbl_PlaceBusinessVolume.D2BusinessVolumeId=d2.Id
					left join tbl_BusinessVolume g3 on tbl_PlaceBusinessVolume.G3BusinessVolumeId=g3.Id
					left join tbl_BusinessVolume g4 on tbl_PlaceBusinessVolume.G4BusinessVolumeId=g4.Id
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
			(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
			(tbl_PlaceBusinessVolume.CreateDate>=@BeginDate and tbl_PlaceBusinessVolume.CreateDate<@EndDate) and
			(@Profession=0 or tbl_Place.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryBusinessVolumesPage]    Script Date: 2016-09-29 15:55:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryBusinessVolumesPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@LogicalType int,
					@LogicalNumber nvarchar(150),
					@Profession int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_BusinessVolume.Id,case tbl_BusinessVolume.LogicalType when 1 then '2G逻辑号' when 2 then '2D逻辑号' when 3 then '3G逻辑号' when 4 then '4G逻辑号' else '' end as LogicalType,
		tbl_BusinessVolume.LogicalNumber,tbl_BusinessVolume.TrafficVolumes,tbl_BusinessVolume.BusinessVolumes,tbl_User.FullName as CreateFullName,tbl_BusinessVolume.CreateDate
		from tbl_BusinessVolume left join tbl_User on tbl_BusinessVolume.CreateUserId = tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where (tbl_BusinessVolume.CreateDate >= @BeginDate and tbl_BusinessVolume.CreateDate < @EndDate) and
				(@LogicalType = 0 or tbl_BusinessVolume.LogicalType = @LogicalType) and
				(@LogicalNumber = '' or CHARINDEX(@LogicalNumber,tbl_BusinessVolume.LogicalNumber) > 0) and
				(@Profession=0 or tbl_BusinessVolume.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId
		order by tbl_BusinessVolume.LogicalType,tbl_BusinessVolume.LogicalNumber asc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_BusinessVolume left join tbl_User on tbl_BusinessVolume.CreateUserId = tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where (tbl_BusinessVolume.CreateDate >= @BeginDate and tbl_BusinessVolume.CreateDate < @EndDate) and
				(@LogicalType = 0 or tbl_BusinessVolume.LogicalType = @LogicalType) and
				(@LogicalNumber = '' or CHARINDEX(@LogicalNumber,tbl_BusinessVolume.LogicalNumber) > 0) and
				(@Profession=0 or tbl_BusinessVolume.Profession=@Profession) and tbl_Department.CompanyId = @CompanyId
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportEngineeringDesignReportExcel]    Script Date: 2016-09-28 14:33:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportEngineeringDesignReportExcel]
					@ProjectCode nvarchar(50),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@TaskModel int,
					@DesignRealName nvarchar(50),
					@DesignCustomerId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @temp table(项目编码 nvarchar(50),站点名称 nvarchar(100),区域 nvarchar(100),网格 nvarchar(100),拟建网络 nvarchar(250),建设方式 nvarchar(10),
		工程名称 nvarchar(10),总设简述 nvarchar(150),设计单位 nvarchar(100),设计人 nvarchar(50),设计日期 datetime)

	insert @temp
	select tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Planning.ProposedNetwork,
			case tbl_ProjectTask.ProjectType when 1 then '新增' when 2 then '改造' when 3 then '部分拆除' when 4 then '全部拆除' else '' end,
			case tbl_EngineeringTask.TaskModel when 1 then '天桅' when 2 then '天桅基础' when 3 then '机房' when 4 then '外电引入' when 5 then '设备安装' when 6 then '线路' else '' end,
			tbl_EngineeringTask.DesignMemos,tbl_Customer.CustomerName as DesignCustomerName,tbl_EngineeringTask.DesignRealName,tbl_EngineeringTask.DesignDate
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_EngineeringTask.DesignCustomerId=tbl_Customer.Id
							left join tbl_User on tbl_EngineeringTask.ProjectManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_EngineeringTask.DesignRealName) > 0) and
				(@DesignCustomerId='00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.DesignCustomerId=@DesignCustomerId) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_EngineeringTask.[State]=1 and tbl_Department.CompanyId=@CompanyId
		union
		select tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Remodeling.ProposedNetwork,
			case tbl_ProjectTask.ProjectType when 1 then '新增' when 2 then '改造' when 3 then '部分拆除' when 4 then '全部拆除' else '' end,
			case tbl_EngineeringTask.TaskModel when 1 then '天桅' when 2 then '天桅基础' when 3 then '机房' when 4 then '外电引入' when 5 then '设备安装' when 6 then '线路' else '' end,
			tbl_EngineeringTask.DesignMemos,tbl_Customer.CustomerName as DesignCustomerName,tbl_EngineeringTask.DesignRealName,tbl_EngineeringTask.DesignDate
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_EngineeringTask.DesignCustomerId=tbl_Customer.Id
							left join tbl_User on tbl_EngineeringTask.ProjectManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_EngineeringTask.DesignRealName) > 0) and
				(@DesignCustomerId='00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.DesignCustomerId=@DesignCustomerId) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType<>1 and tbl_EngineeringTask.[State]=1 and tbl_Department.CompanyId=@CompanyId

		select * from @temp t
		order by t.建设方式 asc,t.项目编码 desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryEngineeringDesignReportPage]    Script Date: 2016-09-28 14:33:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryEngineeringDesignReportPage]
					@PageIndex int,
					@PageSize int,
					@ProjectCode nvarchar(50),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@TaskModel int,
					@DesignRealName nvarchar(50),
					@DesignCustomerId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,ProjectTaskId uniqueidentifier,PlaceId uniqueidentifier,ProjectCode nvarchar(50),PlaceName nvarchar(100),
		PlaceCategoryName varchar(50),AreaName nvarchar(100),ReseauName nvarchar(100),ProposedNetwork nvarchar(250),ProjectType int,TaskModel int,DesignMemos nvarchar(150),
		DesignCustomerName nvarchar(100),DesignRealName nvarchar(50),DesignDate datetime,IsFile nvarchar(50),IsSGFile nvarchar(50))

	insert @temp
	select tbl_EngineeringTask.Id,tbl_ProjectTask.Id as ProjectTaskId,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Planning.ProposedNetwork,tbl_ProjectTask.ProjectType,tbl_EngineeringTask.TaskModel,tbl_EngineeringTask.DesignMemos,
			tbl_Customer.CustomerName as DesignCustomerName,tbl_EngineeringTask.DesignRealName,tbl_EngineeringTask.DesignDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile,isnull(fileSG.EntityName,'') as IsSGFile			
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_EngineeringTask.DesignCustomerId=tbl_Customer.Id
							left join tbl_User on tbl_EngineeringTask.ProjectManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
							left join tbl_FileAssociation fileSG on tbl_EngineeringTask.Id = fileSG.EntityId and fileSG.EntityName = 'EngineeringDesign'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_EngineeringTask.DesignRealName) > 0) and
				(@DesignCustomerId='00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.DesignCustomerId=@DesignCustomerId) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_EngineeringTask.[State]=1 and tbl_Department.CompanyId=@CompanyId
		union
		select tbl_EngineeringTask.Id,tbl_ProjectTask.Id as ProjectTaskId,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Remodeling.ProposedNetwork,tbl_ProjectTask.ProjectType,tbl_EngineeringTask.TaskModel,tbl_EngineeringTask.DesignMemos,
			tbl_Customer.CustomerName as DesignCustomerName,tbl_EngineeringTask.DesignRealName,tbl_EngineeringTask.DesignDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile,isnull(fileSG.EntityName,'') as IsSGFile
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_EngineeringTask.DesignCustomerId=tbl_Customer.Id
							left join tbl_User on tbl_EngineeringTask.ProjectManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
							left join tbl_FileAssociation fileSG on tbl_EngineeringTask.Id = fileSG.EntityId and fileSG.EntityName = 'EngineeringDesign'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_EngineeringTask.DesignRealName) > 0) and
				(@DesignCustomerId='00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.DesignCustomerId=@DesignCustomerId) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType<>1 and tbl_EngineeringTask.[State]=1 and tbl_Department.CompanyId=@CompanyId

		select * from @temp t
		order by t.ProjectType asc,t.ProjectCode desc offset @PageStart row fetch next @PageSize rows only

	select count(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportEngineeringProgressReportExcel]    Script Date: 2016-09-28 13:42:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportEngineeringProgressReportExcel]
					@ProjectCode nvarchar(50),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@TaskModel int,
					@EngineeringProgress int,
					@ProjectType int,
					@ProjectManagerId uniqueidentifier,
					@ConstructionCustomerId uniqueidentifier,
					@SupervisionCustomerId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @temp table(项目编码 nvarchar(50),站点名称 nvarchar(100),区域 nvarchar(100),网格 nvarchar(100),建设方式 nvarchar(10),工程名称 nvarchar(10),工程进度 nvarchar(10),
		进度简述 nvarchar(150),工程经理 nvarchar(50),施工单位 nvarchar(100),监理单位 nvarchar(100),设计单位 varchar(100),设计简述 varchar(150),登记日期 datetime)

	insert @temp
	select tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			case tbl_ProjectTask.ProjectType when 1 then '新增' when 2 then '改造' when 3 then '部分拆除' when 4 then '全部拆除' else '' end,
			case tbl_EngineeringTask.TaskModel when 1 then '天桅' when 2 then '天桅基础' when 3 then '机房' when 4 then '外电引入' when 5 then '设备安装' when 6 then '线路' else '' end,
			case tbl_EngineeringTask.EngineeringProgress when 1 then '未开工' when 2 then '进行中' when 3 then '已完工' when 4 then '暂缓' when 5 then '取消' else '' end,
			tbl_EngineeringTask.ProgressMemos,tbl_User.FullName as ProjectManagerName,construction.CustomerFullName as ConstructionFullName,
			supervision.CustomerFullName as SupervisionFullName,design.CustomerFullName as DesignFullName,tbl_EngineeringTask.DesignMemos,
			tbl_EngineeringTask.ModifyDate		
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_User on tbl_EngineeringTask.ProjectManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
							left join tbl_Customer design on tbl_EngineeringTask.DesignCustomerId=design.Id
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@EngineeringProgress = 0 or (tbl_EngineeringTask.EngineeringProgress=@EngineeringProgress and @EngineeringProgress<>6) or (@EngineeringProgress=6 and (tbl_EngineeringTask.EngineeringProgress=1 or tbl_EngineeringTask.EngineeringProgress=2))) and
				(@ProjectType=0 or (@ProjectType=1 and tbl_ProjectTask.ProjectType=1)) and
				(@ProjectManagerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.ProjectManagerId=@ProjectManagerId) and
				(@ConstructionCustomerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.ConstructionCustomerId=@ConstructionCustomerId) and
				(@SupervisionCustomerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.SupervisionCustomerId=@SupervisionCustomerId) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_EngineeringTask.[State]=1 and tbl_Department.CompanyId=@CompanyId
		union
		select tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			case tbl_ProjectTask.ProjectType when 1 then '新增' when 2 then '改造' when 3 then '部分拆除' when 4 then '全部拆除' else '' end,
			case tbl_EngineeringTask.TaskModel when 1 then '天桅' when 2 then '天桅基础' when 3 then '机房' when 4 then '外电引入' when 5 then '设备安装' when 6 then '线路' else '' end,
			case tbl_EngineeringTask.EngineeringProgress when 1 then '未开工' when 2 then '进行中' when 3 then '已完工' when 4 then '暂缓' when 5 then '取消' else '' end,
			tbl_EngineeringTask.ProgressMemos,tbl_User.FullName as ProjectManagerName,construction.CustomerFullName as ConstructionFullName,
			supervision.CustomerFullName as SupervisionFullName,design.CustomerFullName as DesignFullName,tbl_EngineeringTask.DesignMemos,
			tbl_EngineeringTask.ModifyDate			
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_User on tbl_EngineeringTask.ProjectManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
							left join tbl_Customer design on tbl_EngineeringTask.DesignCustomerId=design.Id
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@EngineeringProgress = 0 or (tbl_EngineeringTask.EngineeringProgress=@EngineeringProgress and @EngineeringProgress<>6) or (@EngineeringProgress=6 and (tbl_EngineeringTask.EngineeringProgress=1 or tbl_EngineeringTask.EngineeringProgress=2))) and
				(@ProjectType=0 or (@ProjectType<>1 and tbl_ProjectTask.ProjectType=@ProjectType)) and
				(@ProjectManagerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.ProjectManagerId=@ProjectManagerId) and
				(@ConstructionCustomerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.ConstructionCustomerId=@ConstructionCustomerId) and
				(@SupervisionCustomerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.SupervisionCustomerId=@SupervisionCustomerId) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType<>1 and tbl_EngineeringTask.[State]=1 and tbl_Department.CompanyId=@CompanyId

		select * from @temp t
		order by t.建设方式 asc,t.项目编码 desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryEngineeringProgressReportPage]    Script Date: 2016-09-28 13:42:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryEngineeringProgressReportPage]
					@PageIndex int,
					@PageSize int,
					@ProjectCode nvarchar(50),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@TaskModel int,
					@EngineeringProgress int,
					@ProjectType int,
					@ProjectManagerId uniqueidentifier,
					@ConstructionCustomerId uniqueidentifier,
					@SupervisionCustomerId uniqueidentifier,
					@Profession int,
					@CompanyId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,PlaceId uniqueidentifier,ProjectTaskId uniqueidentifier,ProjectCode nvarchar(50),PlaceName nvarchar(100),
		AreaName nvarchar(100),ReseauName nvarchar(100),ProjectType int,TaskModel int,EngineeringProgress int,ProgressMemos nvarchar(150),ProjectManagerName nvarchar(50),
		ConstructionFullName nvarchar(100),SupervisionFullName nvarchar(100),DesignFullName varchar(100),DesignMemos varchar(150),ModifyDate datetime,IsFile nvarchar(50),
		IsSGFile nvarchar(50))

	insert @temp
	select tbl_EngineeringTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.Id as ProjectTaskId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,
			tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_ProjectTask.ProjectType,tbl_EngineeringTask.TaskModel,tbl_EngineeringTask.EngineeringProgress,
			tbl_EngineeringTask.ProgressMemos,tbl_User.FullName as ProjectManagerName,construction.CustomerFullName as ConstructionFullName,
			supervision.CustomerFullName as SupervisionFullName,design.CustomerFullName as DesignFullName,tbl_EngineeringTask.DesignMemos,
			tbl_EngineeringTask.ModifyDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile,isnull(fileSG.EntityName,'') as IsSGFile			
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_User on tbl_EngineeringTask.ProjectManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
							left join tbl_Customer design on tbl_EngineeringTask.DesignCustomerId=design.Id
							left join tbl_FileAssociation on tbl_EngineeringTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'EngineeringProgress'
							left join tbl_FileAssociation fileSG on tbl_EngineeringTask.Id = fileSG.EntityId and fileSG.EntityName = 'EngineeringDesign'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@EngineeringProgress = 0 or (tbl_EngineeringTask.EngineeringProgress=@EngineeringProgress and @EngineeringProgress<>6) or (@EngineeringProgress=6 and (tbl_EngineeringTask.EngineeringProgress=1 or tbl_EngineeringTask.EngineeringProgress=2))) and
				(@ProjectType=0 or (@ProjectType=1 and tbl_ProjectTask.ProjectType=1)) and
				(@ProjectManagerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.ProjectManagerId=@ProjectManagerId) and
				(@ConstructionCustomerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.ConstructionCustomerId=@ConstructionCustomerId) and
				(@SupervisionCustomerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.SupervisionCustomerId=@SupervisionCustomerId) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_EngineeringTask.[State]=1 and tbl_Department.CompanyId=@CompanyId
		union
		select tbl_EngineeringTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.Id as ProjectTaskId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,
			tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_ProjectTask.ProjectType,tbl_EngineeringTask.TaskModel,tbl_EngineeringTask.EngineeringProgress,
			tbl_EngineeringTask.ProgressMemos,tbl_User.FullName as ProjectManagerName,construction.CustomerFullName as ConstructionFullName,
			supervision.CustomerFullName as SupervisionFullName,design.CustomerFullName as DesignFullName,tbl_EngineeringTask.DesignMemos,
			tbl_EngineeringTask.ModifyDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile,isnull(fileSG.EntityName,'') as IsSGFile			
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_User on tbl_EngineeringTask.ProjectManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
							left join tbl_Customer design on tbl_EngineeringTask.DesignCustomerId=design.Id
							left join tbl_FileAssociation on tbl_EngineeringTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'EngineeringProgress'
							left join tbl_FileAssociation fileSG on tbl_EngineeringTask.Id = fileSG.EntityId and fileSG.EntityName = 'EngineeringDesign'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@EngineeringProgress = 0 or (tbl_EngineeringTask.EngineeringProgress=@EngineeringProgress and @EngineeringProgress<>6) or (@EngineeringProgress=6 and (tbl_EngineeringTask.EngineeringProgress=1 or tbl_EngineeringTask.EngineeringProgress=2))) and
				(@ProjectType=0 or (@ProjectType<>1 and tbl_ProjectTask.ProjectType=@ProjectType)) and
				(@ProjectManagerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.ProjectManagerId=@ProjectManagerId) and
				(@ConstructionCustomerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.ConstructionCustomerId=@ConstructionCustomerId) and
				(@SupervisionCustomerId= '00000000-0000-0000-0000-000000000000' or tbl_EngineeringTask.SupervisionCustomerId=@SupervisionCustomerId) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType<>1 and tbl_EngineeringTask.[State]=1 and tbl_Department.CompanyId=@CompanyId

		select * from @temp t
		order by t.ProjectType asc,t.ProjectCode desc offset @PageStart row fetch next @PageSize rows only

	select count(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportProjectDesignReportExcel]    Script Date: 2016-09-27 20:39:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportProjectDesignReportExcel]
					@ProjectCode nvarchar(50),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@GeneralDesignId uniqueidentifier,
					@DesignRealName nvarchar(50),
					@Profession int,
					@CompanyId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @temp table(项目编码 nvarchar(50),站点名称 nvarchar(100),区域 nvarchar(100),网格 nvarchar(100),拟建网络 nvarchar(250),建设方式 nvarchar(10),总设单位 nvarchar(100),设计人 nvarchar(50),设计日期 datetime)

	insert @temp
	select tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Planning.ProposedNetwork,
			case tbl_ProjectTask.ProjectType when 1 then '新建' when 2 then '改造' when 3 then '部分拆除' when 4 then '全部拆除' else '' end,
			tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.DesignRealName,tbl_ProjectTask.DesignDate			
		from tbl_ProjectTask left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
							left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and	
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and	
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and		
				(@GeneralDesignId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.GeneralDesignId = @GeneralDesignId) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_ProjectTask.DesignRealName) > 0) and	
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_Department.CompanyId=@CompanyId
		union
		select tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Remodeling.ProposedNetwork,
			case tbl_ProjectTask.ProjectType when 1 then '新建' when 2 then '改造' when 3 then '部分拆除' when 4 then '全部拆除' else '' end,
			tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.DesignRealName,tbl_ProjectTask.DesignDate			
		from tbl_ProjectTask left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
							left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and	
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and	
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and		
				(@GeneralDesignId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.GeneralDesignId = @GeneralDesignId) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_ProjectTask.DesignRealName) > 0) and					
				tbl_Place.Profession = @Profession  and tbl_ProjectTask.ProjectType<>1 and tbl_Department.CompanyId=@CompanyId
		
		select * from @temp t
			order by t.建设方式 asc,t.项目编码 desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryProjectDesignReportPage]    Script Date: 2016-09-27 20:39:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectDesignReportPage]
					@PageIndex int,
					@PageSize int,
					@ProjectCode nvarchar(50),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@GeneralDesignId uniqueidentifier,
					@DesignRealName nvarchar(50),
					@Profession int,
					@CompanyId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,PlaceId uniqueidentifier,IsFile nvarchar(50),ProjectCode nvarchar(50),PlaceName nvarchar(100),AreaName nvarchar(100),
		ReseauName nvarchar(100),ProposedNetwork nvarchar(250),ProjectType int,GeneralDesignName nvarchar(100),DesignRealName nvarchar(50),DesignDate datetime)

	insert @temp
	select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,isnull(tbl_FileAssociation.EntityName,'') as IsFile,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			tbl_Planning.ProposedNetwork,tbl_ProjectTask.ProjectType,tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.DesignRealName,tbl_ProjectTask.DesignDate			
		from tbl_ProjectTask left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
							left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and	
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and	
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and		
				(@GeneralDesignId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.GeneralDesignId = @GeneralDesignId) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_ProjectTask.DesignRealName) > 0) and	
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_Department.CompanyId=@CompanyId
		union
		select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,isnull(tbl_FileAssociation.EntityName,'') as IsFile,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			tbl_Remodeling.ProposedNetwork,tbl_ProjectTask.ProjectType,tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.DesignRealName,tbl_ProjectTask.DesignDate			
		from tbl_ProjectTask left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
							left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and	
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and	
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and		
				(@GeneralDesignId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.GeneralDesignId = @GeneralDesignId) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_ProjectTask.DesignRealName) > 0) and					
				tbl_Place.Profession = @Profession  and tbl_ProjectTask.ProjectType<>1 and tbl_Department.CompanyId=@CompanyId
		
		select * from @temp t
			order by t.ProjectType asc,t.ProjectCode desc offset @PageStart row fetch next @PageSize rows only

	select count(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportProjectProgressReportExcel]    Script Date: 2016-09-27 17:36:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportProjectProgressReportExcel]
					@ProjectCode nvarchar(50),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@ProjectType int,
					@ProjectProgress int,
					@ProjectManagerId uniqueidentifier,
					@IsOverTime int,
					@Profession int,
					@CompanyId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @temp table(项目编码 nvarchar(50),站点名称 nvarchar(100),
		区域 nvarchar(100),网格 nvarchar(100),建设方式 nvarchar(10),拟建网络 nvarchar(250),项目进度 nvarchar(10),进度简述 nvarchar(150),项目经理 nvarchar(50),
		项目启动日期 datetime,项目开通日期 datetime,建设时长 nvarchar(10),是否超时 nvarchar(10))

	insert @temp
	select tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
		case tbl_ProjectTask.ProjectType when 1 then '新增' when 2 then '改造' when 3 then '部分拆除' when 4 then '全部拆除' else '' end,tbl_Planning.ProposedNetwork,
		case tbl_ProjectTask.ProjectProgress when 1 then '未开工' when 2 then '进行中' when 3 then '完工' when 4 then '开通' when 5 then '暂缓' when 6 then '撤销' else '' end,
		tbl_ProjectTask.ProgressMemos,tbl_User.FullName,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate,DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate),
			case when DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30 then '是' else '否' end
		from tbl_ProjectTask left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'ProjectProgress'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectType=0 or (@ProjectType=1 and tbl_ProjectTask.ProjectType=1)) and
				(@ProjectProgress = 0 or (tbl_ProjectTask.ProjectProgress = @ProjectProgress and @ProjectProgress<>7) or (@ProjectProgress=7 and (tbl_ProjectTask.ProjectProgress=1 or tbl_ProjectTask.ProjectProgress=2))) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.AreaManagerId = @ProjectManagerId) and
				(@IsOverTime=0 or (@IsOverTime=1 and DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30) or ((@IsOverTime=2 and DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30))) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_Department.CompanyId=@CompanyId
		union
		select tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
		case tbl_ProjectTask.ProjectType when 1 then '新增' when 2 then '改造' when 3 then '部分拆除' when 4 then '全部拆除' else '' end,tbl_Remodeling.ProposedNetwork,
		case tbl_ProjectTask.ProjectProgress when 1 then '未开工' when 2 then '进行中' when 3 then '完工' when 4 then '开通' when 5 then '暂缓' when 6 then '撤销' else '' end,
		tbl_ProjectTask.ProgressMemos,tbl_User.FullName,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate,DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate),
			case when DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30 then '是' else '否' end
		from tbl_ProjectTask left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'ProjectProgress'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectType=0 or (@ProjectType<>1 and tbl_ProjectTask.ProjectType=@ProjectType)) and
				(@ProjectProgress = 0 or (tbl_ProjectTask.ProjectProgress = @ProjectProgress and @ProjectProgress<>7) or (@ProjectProgress=7 and (tbl_ProjectTask.ProjectProgress=1 or tbl_ProjectTask.ProjectProgress=2))) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.AreaManagerId = @ProjectManagerId) and
				(@IsOverTime=0 or (@IsOverTime=1 and DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30) or ((@IsOverTime=2 and DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30))) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType<>1 and tbl_Department.CompanyId=@CompanyId

		select * from @temp t
		order by t.建设方式 asc,t.项目编码 desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryProjectProgresssReportPage]    Script Date: 2016-09-27 17:36:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectProgresssReportPage]
					@PageIndex int,
					@PageSize int,
					@ProjectCode nvarchar(50),
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@ProjectType int,
					@ProjectProgress int,
					@ProjectManagerId uniqueidentifier,
					@IsOverTime int,
					@Profession int,
					@CompanyId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,PlaceId uniqueidentifier,IsFile nvarchar(50),ProjectCode nvarchar(50),PlaceName nvarchar(100),
		AreaName nvarchar(100),ReseauName nvarchar(100),ProjectType int,ProposedNetwork nvarchar(250),ProjectProgress int,ProgressMemos nvarchar(150),ProjectManagerName nvarchar(50),
		ProjectBeginDate datetime,ProjectDate datetime,ConstructionDays int,IsOverTime nvarchar(10))

	insert @temp
	select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,isnull(tbl_FileAssociation.EntityName,'') as IsFile,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,
		tbl_Reseau.ReseauName,tbl_ProjectTask.ProjectType,tbl_Planning.ProposedNetwork,tbl_ProjectTask.ProjectProgress,tbl_ProjectTask.ProgressMemos,tbl_User.FullName,
			tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate,case tbl_ProjectTask.ProjectDate when '2000-01-01' then DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,GETDATE()) else DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate) end as ConstructionDays,
			case when DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30 then '是' else '否' end
		from tbl_ProjectTask left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'ProjectProgress'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectType=0 or (@ProjectType=1 and tbl_ProjectTask.ProjectType=1)) and
				(@ProjectProgress = 0 or (tbl_ProjectTask.ProjectProgress = @ProjectProgress and @ProjectProgress<>7) or (@ProjectProgress=7 and (tbl_ProjectTask.ProjectProgress=1 or tbl_ProjectTask.ProjectProgress=2))) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.AreaManagerId = @ProjectManagerId) and
				(@IsOverTime=0 or (@IsOverTime=1 and DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30) or ((@IsOverTime=2 and DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30))) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_Department.CompanyId=@CompanyId
		union
		select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,isnull(tbl_FileAssociation.EntityName,'') as IsFile,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_Area.AreaName,
		tbl_Reseau.ReseauName,tbl_ProjectTask.ProjectType,tbl_Remodeling.ProposedNetwork,tbl_ProjectTask.ProjectProgress,tbl_ProjectTask.ProgressMemos,tbl_User.FullName,
			tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate,case tbl_ProjectTask.ProjectDate when '2000-01-01' then DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,GETDATE()) else DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate) end as ConstructionDays,
			case when DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30 then '是' else '否' end
		from tbl_ProjectTask left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'ProjectProgress'
		where (@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectType=0 or (@ProjectType<>1 and tbl_ProjectTask.ProjectType=@ProjectType)) and
				(@ProjectProgress = 0 or (tbl_ProjectTask.ProjectProgress = @ProjectProgress and @ProjectProgress<>7) or (@ProjectProgress=7 and (tbl_ProjectTask.ProjectProgress=1 or tbl_ProjectTask.ProjectProgress=2))) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.AreaManagerId = @ProjectManagerId) and
				(@IsOverTime=0 or (@IsOverTime=1 and DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)>30) or ((@IsOverTime=2 and DATEDIFF(DAY,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate)<=30))) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType<>1 and tbl_Department.CompanyId=@CompanyId

		select * from @temp t
		order by t.ProjectType asc,t.ProjectCode desc offset @PageStart row fetch next @PageSize rows only

	select count(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportAddressingReportExcel]    Script Date: 2016-09-26 20:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportAddressingReportExcel]
					@BeginDate datetime,
					@EndDate datetime,
					@PlanningCode nvarchar(50),
					@PlanningName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Importance int,
					@AddressingState int,
					@AddressingDepartmentId uniqueidentifier,
					@AddressingUserId uniqueidentifier,
					@IsAppoint int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_Planning.PlanningCode as '规划编码',tbl_Planning.PlanningName as '规划名称',ISNULL(tbl_Addressing.PlaceName,'') as '基站名称',ISNULL(tbl_User.FullName,'') as '租赁人',
		DATEDIFF(DAY,tbl_Planning.AddressingUserDate,tbl_Planning.AddressingDate) as '租赁时长',tbl_Area.AreaName as '区域',
		tbl_Reseau.ReseauName as '网格',tbl_PlaceCategory.PlaceCategoryName as '站点类型',tbl_Planning.Lng as '经度',tbl_Planning.Lat as '纬度',tbl_Planning.ProposedNetwork as '拟建网络',
		case tbl_Planning.Importance when 1 then 'A' when 2 then 'B' when 3 then 'C' else '' end as '重要性程度',tbl_PlaceOwner.PlaceOwnerName as '产权',tbl_Planning.OptionalAddress as '可选地址',
		tbl_Addressing.OwnerName as '业主名称',tbl_Addressing.OwnerContact as '联系人',tbl_Addressing.OwnerPhoneNumber as '联系方式',
		case tbl_Planning.AddressingState when 1 then '未寻址确认' when 2 then '已寻址确认' when 3 then '流转中' when 4 then '流程终止' else '' end as '寻址状态',
		case tbl_Planning.AddressingState when 2 then tbl_Planning.AddressingDate else '2000-01-01' end as '租赁完成日期',ISNULL(tbl_Department.DepartmentName,'') as '租赁部门',tbl_Addressing.AddressingRealName as '实际租赁人',
		ISNULL(tbl_Addressing.Remarks,'') as '备注',ISNULL(u2.FullName,'') as '规划人',tbl_Planning.CreateDate as '规划日期' 
		from tbl_Planning left join tbl_Addressing on tbl_Planning.Id = tbl_Addressing.PlanningId
							left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_PlaceOwner on tbl_Planning.PlaceOwner = tbl_PlaceOwner.Id
							left join tbl_User on tbl_Planning.AddressingUserId = tbl_User.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
							left join tbl_Department on tbl_Addressing.AddressingDepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_Addressing.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Addressing'
		where tbl_Planning.Issued = 1 and
				(tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Importance = 0 or tbl_Planning.Importance = @Importance) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@AddressingDepartmentId= '00000000-0000-0000-0000-000000000000' or tbl_Addressing.AddressingDepartmentId=@AddressingDepartmentId) and
				(@AddressingUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.AddressingUserId = @AddressingUserId) and
				(@IsAppoint=0 or (@IsAppoint=1 and tbl_Planning.AddressingUserId<> '00000000-0000-0000-0000-000000000000') or (@IsAppoint=2 and tbl_Planning.AddressingUserId= '00000000-0000-0000-0000-000000000000')) and
				tbl_Department.CompanyId=@CompanyId
		order by tbl_Planning.PlanningCode desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryAddressingReportPage]    Script Date: 2016-09-26 20:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryAddressingReportPage]
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
					@Importance int,
					@AddressingState int,
					@AddressingDepartmentId uniqueidentifier,
					@AddressingUserId uniqueidentifier,
					@IsAppoint int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select ISNULL(tbl_Addressing.Id,'00000000-0000-0000-0000-000000000000') as Id,tbl_Planning.Id as PlanningId,tbl_Planning.PlaceId,tbl_Planning.PlanningCode,tbl_Planning.PlanningName,
		ISNULL(tbl_Addressing.PlaceName,'') as PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.ProposedNetwork,
		ISNULL(tbl_Planning.Importance,0) as Importance,tbl_PlaceOwner.PlaceOwnerName,tbl_Planning.OptionalAddress,tbl_Addressing.OwnerName,tbl_Addressing.OwnerContact,tbl_Addressing.OwnerPhoneNumber,
		case tbl_Planning.AddressingState when 2 then tbl_Planning.AddressingDate else '2000-01-01' end as AddressingDate,tbl_Planning.AddressingState,ISNULL(tbl_Department.DepartmentName,'') as AddressingDepartmentName,tbl_Addressing.AddressingRealName,
		case tbl_Planning.AddressingState when 2 then DATEDIFF(DAY,tbl_Planning.AddressingUserDate,tbl_Planning.AddressingDate) when 1 then DATEDIFF(DAY,tbl_Planning.AddressingUserDate,GETDATE()) else 0 end as AddressingDays,
		ISNULL(tbl_Addressing.Remarks,'') as Remarks,ISNULL(tbl_User.FullName,'') as AddressingUserFullName,ISNULL(u2.FullName,'') as FullName,tbl_Planning.CreateDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile 
		from tbl_Planning left join tbl_Addressing on tbl_Planning.Id = tbl_Addressing.PlanningId
							left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_PlaceOwner on tbl_Planning.PlaceOwner = tbl_PlaceOwner.Id
							left join tbl_User on tbl_Planning.AddressingUserId = tbl_User.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
							left join tbl_Department on tbl_Addressing.AddressingDepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_Addressing.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Addressing'
		where tbl_Planning.Issued = 1 and
				(tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and tbl_Department.CompanyId=@CompanyId and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Importance = 0 or tbl_Planning.Importance = @Importance) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@AddressingDepartmentId= '00000000-0000-0000-0000-000000000000' or tbl_Addressing.AddressingDepartmentId=@AddressingDepartmentId) and
				(@AddressingUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.AddressingUserId = @AddressingUserId) and
				(@IsAppoint=0 or (@IsAppoint=1 and tbl_Planning.AddressingUserId<> '00000000-0000-0000-0000-000000000000') or (@IsAppoint=2 and tbl_Planning.AddressingUserId= '00000000-0000-0000-0000-000000000000'))
		order by tbl_Planning.PlanningCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Planning left join tbl_Addressing on tbl_Planning.Id = tbl_Addressing.PlanningId
							left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Department on tbl_Addressing.AddressingDepartmentId=tbl_Department.Id
		where tbl_Planning.Issued = 1 and
				(tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and tbl_Department.CompanyId=@CompanyId and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Importance = 0 or tbl_Planning.Importance = @Importance) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				(@AddressingDepartmentId= '00000000-0000-0000-0000-000000000000' or tbl_Addressing.AddressingDepartmentId=@AddressingDepartmentId) and
				(@AddressingUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.AddressingUserId = @AddressingUserId) and
				(@IsAppoint=0 or (@IsAppoint=1 and tbl_Planning.AddressingUserId<> '00000000-0000-0000-0000-000000000000') or (@IsAppoint=2 and tbl_Planning.AddressingUserId= '00000000-0000-0000-0000-000000000000'))
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryProjectTaskHistory]    Script Date: 2016-09-24 22:13:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectTaskHistory]
				@PlaceId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectType,tbl_ProjectTask.ProjectCode,tbl_ProjectTask.ProjectProgress,
		tbl_User.FullName as AreaManagerName,tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ProjectDate
		from tbl_ProjectTask left join tbl_User on tbl_ProjectTask.AreaManagerId=tbl_User.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
		where PlaceId=@PlaceId order by tbl_ProjectTask.CreateDate
END
GO
/****** Object:  StoredProcedure [dbo].[prc_ExportLogicalNumbersExcel]    Script Date: 2016-09-23 21:06:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_ExportLogicalNumbersExcel]
					@PlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@G2Mark int,
					@D2Mark int,
					@G3Mark int,
					@G4Mark int,
					@G2Number nvarchar(150),
					@D2Number nvarchar(150),
					@G3Number nvarchar(150),
					@G4Number nvarchar(150),					
					@AllMark int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select tbl_Place.PlaceCode as '站点编码',tbl_Place.PlaceName as '站点名称',tbl_Area.AreaName as '区域',tbl_Reseau.ReseauName as '网格',tbl_Place.G2Number as '2G逻辑号',
		tbl_Place.D2Number as '2D逻辑号',tbl_Place.G3Number as '3G逻辑号',tbl_Place.G4Number as '4G逻辑号'
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
		where (@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				((@AllMark=0 and
				((@G2Mark=0 and tbl_Place.G2Number = '') or (@G2Mark=1 and tbl_Place.G2Number<>'')) and
				((@D2Mark=0 and tbl_Place.D2Number = '') or (@D2Mark=1 and tbl_Place.D2Number<>'')) and
				((@G3Mark=0 and tbl_Place.G3Number = '') or (@G3Mark=1 and tbl_Place.G3Number<>'')) and
				((@G4Mark=0 and tbl_Place.G4Number = '') or (@G4Mark=1 and tbl_Place.G4Number<>''))) or @AllMark=1) and
				(@G2Number = '' or CHARINDEX(@G2Number,tbl_Place.G2Number) > 0) and
				(@D2Number = '' or CHARINDEX(@D2Number,tbl_Place.D2Number) > 0) and
				(@G3Number = '' or CHARINDEX(@G3Number,tbl_Place.G3Number) > 0) and
				(@G4Number = '' or CHARINDEX(@G4Number,tbl_Place.G4Number) > 0)
		order by tbl_Place.PlaceCode desc
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryLogicalNumbersPage]    Script Date: 2016-09-23 14:55:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryLogicalNumbersPage]
					@PageIndex int,
					@PageSize int,
					@PlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@G2Mark int,
					@D2Mark int,
					@G3Mark int,
					@G4Mark int,
					@G2Number nvarchar(150),
					@D2Number nvarchar(150),
					@G3Number nvarchar(150),
					@G4Number nvarchar(150),					
					@AllMark int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Place.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.G2Number,tbl_Place.D2Number,
			tbl_Place.G3Number,tbl_Place.G4Number
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
		where (@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				((@AllMark=0 and (((@G2Mark=0 and tbl_Place.G2Number = '') or (@G2Mark=1 and tbl_Place.G2Number<>'')) and
				((@D2Mark=0 and tbl_Place.D2Number = '') or (@D2Mark=1 and tbl_Place.D2Number<>'')) and
				((@G3Mark=0 and tbl_Place.G3Number = '') or (@G3Mark=1 and tbl_Place.G3Number<>'')) and
				((@G4Mark=0 and tbl_Place.G4Number = '') or (@G4Mark=1 and tbl_Place.G4Number<>''))))
				or 
				(@AllMark=1 and ((((@G2Mark=0) or (@G2Mark=1 and tbl_Place.G2Number<>'')) and
				((@D2Mark=0) or (@D2Mark=1 and tbl_Place.D2Number<>'')) and
				((@G3Mark=0) or (@G3Mark=1 and tbl_Place.G3Number<>'')) and
				((@G4Mark=0) or (@G4Mark=1 and tbl_Place.G4Number<>'')))))) and
				(@G2Number = '' or CHARINDEX(@G2Number,tbl_Place.G2Number) > 0) and
				(@D2Number = '' or CHARINDEX(@D2Number,tbl_Place.D2Number) > 0) and
				(@G3Number = '' or CHARINDEX(@G3Number,tbl_Place.G3Number) > 0) and
				(@G4Number = '' or CHARINDEX(@G4Number,tbl_Place.G4Number) > 0)
		order by tbl_Place.PlaceCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
		where (@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				((@AllMark=0 and (((@G2Mark=0 and tbl_Place.G2Number = '') or (@G2Mark=1 and tbl_Place.G2Number<>'')) and
				((@D2Mark=0 and tbl_Place.D2Number = '') or (@D2Mark=1 and tbl_Place.D2Number<>'')) and
				((@G3Mark=0 and tbl_Place.G3Number = '') or (@G3Mark=1 and tbl_Place.G3Number<>'')) and
				((@G4Mark=0 and tbl_Place.G4Number = '') or (@G4Mark=1 and tbl_Place.G4Number<>''))))
				or 
				(@AllMark=1 and ((((@G2Mark=0) or (@G2Mark=1 and tbl_Place.G2Number<>'')) and
				((@D2Mark=0) or (@D2Mark=1 and tbl_Place.D2Number<>'')) and
				((@G3Mark=0) or (@G3Mark=1 and tbl_Place.G3Number<>'')) and
				((@G4Mark=0) or (@G4Mark=1 and tbl_Place.G4Number<>'')))))) and
				(@G2Number = '' or CHARINDEX(@G2Number,tbl_Place.G2Number) > 0) and
				(@D2Number = '' or CHARINDEX(@D2Number,tbl_Place.D2Number) > 0) and
				(@G3Number = '' or CHARINDEX(@G3Number,tbl_Place.G3Number) > 0) and
				(@G4Number = '' or CHARINDEX(@G4Number,tbl_Place.G4Number) > 0)
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryPlaceImportsPage]    Script Date: 2016-09-13 13:50:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryPlaceImportsPage]
					@PageIndex int,
					@PageSize int,
					@PlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@PlaceOwner uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Importance int,
					@State int,
					@CompanyId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Place.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.Lng,tbl_Place.Lat,
			tbl_Place.Importance,tbl_PlaceOwner.PlaceOwnerName,tbl_Place.OwnerName,tbl_Place.OwnerContact,tbl_Place.OwnerPhoneNumber,tbl_Place.[State],tbl_User.FullName,
			tbl_Place.CreateDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
						left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
						left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
						left join tbl_PlaceOwner on tbl_Place.PlaceOwner=tbl_PlaceOwner.Id
						left join tbl_FileAssociation on tbl_Place.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Place'
		where (@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PlaceOwner = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceOwner = @PlaceOwner) and
				(@Importance = 0 or tbl_Place.Importance = @Importance) and
				(@State = 0 or tbl_Place.[State] = @State) and tbl_Department.CompanyId=@CompanyId
		order by tbl_Place.PlaceCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
						left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
		where (@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PlaceOwner = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceOwner = @PlaceOwner) and
				(@Importance = 0 or tbl_Place.Importance = @Importance) and
				(@State = 0 or tbl_Place.[State] = @State) and tbl_Department.CompanyId=@CompanyId
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryEngineeringProgresssPage]    Script Date: 2016-09-04 00:31:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryEngineeringProgresssPage]
					@PageIndex int,
					@PageSize int,
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@TaskModel int,
					@EngineeringProgress int,
					@Profession int,
					@CurrentUserId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,PlaceId uniqueidentifier,ProjectTaskId uniqueidentifier,ProjectCode nvarchar(50),PlaceName nvarchar(100),PlaceCategoryName nvarchar(100),
		AreaName nvarchar(100),ReseauName nvarchar(100),ProjectType int,TaskModel int,EngineeringProgress int,ProgressMemos nvarchar(150),ProjectManagerName nvarchar(50),
		ConstructionFullName nvarchar(100),SupervisionFullName nvarchar(100),DesignFullName varchar(100),DesignMemos varchar(150),ModifyDate datetime,IsFile nvarchar(50),
		IsSGFile nvarchar(50))

	insert @temp
	select tbl_EngineeringTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.Id as ProjectTaskId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_ProjectTask.ProjectType,tbl_EngineeringTask.TaskModel,tbl_EngineeringTask.EngineeringProgress,
			tbl_EngineeringTask.ProgressMemos,tbl_User.FullName as ProjectManagerName,construction.CustomerFullName as ConstructionFullName,
			supervision.CustomerFullName as SupervisionFullName,design.CustomerFullName as DesignFullName,tbl_EngineeringTask.DesignMemos,
			tbl_EngineeringTask.ModifyDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile,isnull(fileSG.EntityName,'') as IsSGFile			
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User on tbl_EngineeringTask.ProjectManagerId=tbl_User.Id
							left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
							left join tbl_Customer design on tbl_EngineeringTask.DesignCustomerId=design.Id
							left join tbl_FileAssociation on tbl_EngineeringTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'EngineeringProgress'
							left join tbl_FileAssociation fileSG on tbl_EngineeringTask.Id = fileSG.EntityId and fileSG.EntityName = 'EngineeringDesign'
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@EngineeringProgress = 0 or (tbl_EngineeringTask.EngineeringProgress=@EngineeringProgress and @EngineeringProgress<>6) or (@EngineeringProgress=6 and (tbl_EngineeringTask.EngineeringProgress=1 or tbl_EngineeringTask.EngineeringProgress=2 or tbl_EngineeringTask.EngineeringProgress=4))) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_EngineeringTask.[State]=1 and
				(tbl_EngineeringTask.ProjectManagerId=@CurrentUserId or 
				construction.CustomerUserId=@CurrentUserId or
				supervision.CustomerUserId=@CurrentUserId or
				tbl_EngineeringTask.ProjectManagerId=@CurrentUserId)
		union
		select tbl_EngineeringTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.Id as ProjectTaskId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_ProjectTask.ProjectType,tbl_EngineeringTask.TaskModel,tbl_EngineeringTask.EngineeringProgress,
			tbl_EngineeringTask.ProgressMemos,tbl_User.FullName as ProjectManagerName,construction.CustomerFullName as ConstructionFullName,
			supervision.CustomerFullName as SupervisionFullName,design.CustomerFullName as DesignFullName,tbl_EngineeringTask.DesignMemos,
			tbl_EngineeringTask.ModifyDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile,isnull(fileSG.EntityName,'') as IsSGFile			
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User on tbl_EngineeringTask.ProjectManagerId=tbl_User.Id
							left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
							left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
							left join tbl_Customer design on tbl_EngineeringTask.DesignCustomerId=design.Id
							left join tbl_FileAssociation on tbl_EngineeringTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'EngineeringProgress'
							left join tbl_FileAssociation fileSG on tbl_EngineeringTask.Id = fileSG.EntityId and fileSG.EntityName = 'EngineeringDesign'
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@EngineeringProgress = 0 or (tbl_EngineeringTask.EngineeringProgress=@EngineeringProgress and @EngineeringProgress<>6) or (@EngineeringProgress=6 and (tbl_EngineeringTask.EngineeringProgress=1 or tbl_EngineeringTask.EngineeringProgress=2 or tbl_EngineeringTask.EngineeringProgress=4))) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType<>1 and tbl_EngineeringTask.[State]=1 and
				(tbl_EngineeringTask.ProjectManagerId=@CurrentUserId or 
				construction.CustomerUserId=@CurrentUserId or
				supervision.CustomerUserId=@CurrentUserId or
				tbl_EngineeringTask.ProjectManagerId=@CurrentUserId)

		select * from @temp t
		order by t.ProjectType asc,t.ProjectCode desc offset @PageStart row fetch next @PageSize rows only

	select count(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryProjectProgresssPage]    Script Date: 2016-09-03 22:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectProgresssPage]
					@PageIndex int,
					@PageSize int,
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@ProjectProgress int,
					@Profession int,
					@AreaManagerId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,PlaceId uniqueidentifier,ProjectCode nvarchar(50),PlaceName nvarchar(100),PlaceCategoryName nvarchar(100),
		AreaName nvarchar(100),ReseauName nvarchar(100),ProjectType int,ProjectProgress int,ProgressMemos nvarchar(150),ProjectBeginDate datetime,ModifyDate datetime,IsFile nvarchar(50),
		IsDesignFile nvarchar(50),GeneralDesignName nvarchar(100),DesignRealName nvarchar(50),DesignDate datetime)

	insert @temp
	select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			tbl_ProjectTask.ProjectType,tbl_ProjectTask.ProjectProgress,tbl_ProjectTask.ProgressMemos,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ModifyDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile,
			isnull(designFile.EntityName,'') as IsDesignFile,tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.DesignRealName,tbl_ProjectTask.DesignDate
		from tbl_ProjectTask left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'ProjectProgress'
							left join tbl_FileAssociation designFile on tbl_ProjectTask.Id = designFile.EntityId and designFile.EntityName = 'GeneralDesign'
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectProgress = 0 or (tbl_ProjectTask.ProjectProgress = @ProjectProgress and @ProjectProgress<>7) or (@ProjectProgress=7 and (tbl_ProjectTask.ProjectProgress=1 or tbl_ProjectTask.ProjectProgress=2))) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and
				(@AreaManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.AreaManagerId = @AreaManagerId)
		union
		select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			tbl_ProjectTask.ProjectType,tbl_ProjectTask.ProjectProgress,tbl_ProjectTask.ProgressMemos,tbl_ProjectTask.ProjectBeginDate,tbl_ProjectTask.ModifyDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile,
			isnull(designFile.EntityName,'') as IsDesignFile,tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.DesignRealName,tbl_ProjectTask.DesignDate
		from tbl_ProjectTask left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'ProjectProgress'
							left join tbl_FileAssociation designFile on tbl_ProjectTask.Id = designFile.EntityId and designFile.EntityName = 'GeneralDesign'
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectProgress = 0 or (tbl_ProjectTask.ProjectProgress = @ProjectProgress and @ProjectProgress<>7) or (@ProjectProgress=7 and (tbl_ProjectTask.ProjectProgress=1 or tbl_ProjectTask.ProjectProgress=2))) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType<>1 and
				(@AreaManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.AreaManagerId = @AreaManagerId)

		select * from @temp t
		order by t.ProjectType asc,t.ProjectCode desc offset @PageStart row fetch next @PageSize rows only

	select count(*) from @temp
		--from tbl_ProjectTask left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
		--						left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
		--						left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
		--						left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
		--	where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Addressing.PlaceName) > 0) and
		--			(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
		--			(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
		--			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
		--		(@ProjectProgress = 0 or (tbl_ProjectTask.ProjectProgress = @ProjectProgress and @ProjectProgress<>7) or (@ProjectProgress=7 and (tbl_ProjectTask.ProjectProgress=1 or tbl_ProjectTask.ProjectProgress=2))) and
		--			tbl_Planning.Profession = @Profession and
		--			(@AreaManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.AreaManagerId = @AreaManagerId)
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryEngineeringDesignsPage]    Script Date: 2016-09-02 22:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryEngineeringDesignsPage]
					@PageIndex int,
					@PageSize int,
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@TaskModel int,
					@DesignRealName nvarchar(50),
					@DesignState int,
					@Profession int,
					@CustomerUserId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,ProjectTaskId uniqueidentifier,PlaceId uniqueidentifier,ProjectCode nvarchar(50),PlaceName nvarchar(100),PlaceCategoryName nvarchar(100),
		AreaName nvarchar(100),ReseauName nvarchar(100),ProposedNetwork nvarchar(250),ProjectType int,TaskModel int,DesignMemos nvarchar(150),DesignRealName nvarchar(50),
		DesignDate datetime,IsFile nvarchar(50),IsSGFile nvarchar(50))

	insert @temp
	select tbl_EngineeringTask.Id,tbl_ProjectTask.Id as ProjectTaskId,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Planning.ProposedNetwork,tbl_ProjectTask.ProjectType,tbl_EngineeringTask.TaskModel,tbl_EngineeringTask.DesignMemos,tbl_EngineeringTask.DesignRealName,
			tbl_EngineeringTask.DesignDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile,isnull(fileSG.EntityName,'') as IsSGFile			
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_EngineeringTask.DesignCustomerId=tbl_Customer.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
							left join tbl_FileAssociation fileSG on tbl_EngineeringTask.Id = fileSG.EntityId and fileSG.EntityName = 'EngineeringDesign'
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_EngineeringTask.DesignRealName) > 0) and
				(@DesignState=0 or tbl_EngineeringTask.DesignState=@DesignState) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and tbl_EngineeringTask.[State]=1 and
				(@CustomerUserId = '00000000-0000-0000-0000-000000000000' or tbl_Customer.CustomerUserId = @CustomerUserId)
		union
		select tbl_EngineeringTask.Id,tbl_ProjectTask.Id as ProjectTaskId,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Remodeling.ProposedNetwork,tbl_ProjectTask.ProjectType,tbl_EngineeringTask.TaskModel,tbl_EngineeringTask.DesignMemos,tbl_EngineeringTask.DesignRealName,
			tbl_EngineeringTask.DesignDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile,isnull(fileSG.EntityName,'') as IsSGFile			
		from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
							left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_EngineeringTask.DesignCustomerId=tbl_Customer.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
							left join tbl_FileAssociation fileSG on tbl_EngineeringTask.Id = fileSG.EntityId and fileSG.EntityName = 'EngineeringDesign'
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@TaskModel = 0 or tbl_EngineeringTask.TaskModel = @TaskModel) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_EngineeringTask.DesignRealName) > 0) and
				(@DesignState=0 or tbl_EngineeringTask.DesignState=@DesignState) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType<>1 and tbl_EngineeringTask.[State]=1 and
				(@CustomerUserId = '00000000-0000-0000-0000-000000000000' or tbl_Customer.CustomerUserId = @CustomerUserId)

		select * from @temp t
		order by t.ProjectType asc,t.ProjectCode desc offset @PageStart row fetch next @PageSize rows only

	select count(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryDesignDrawingsPage]    Script Date: 2016-09-19 19:49:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryDesignDrawingsPage]
					@PageIndex int,
					@PageSize int,
					@PlaceName nvarchar(100),
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@DesignRealName nvarchar(50),
					@Profession int,
					@UserId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,PlaceId uniqueidentifier,ProjectCode nvarchar(50),PlaceName nvarchar(100),PlaceCategoryName nvarchar(100),
		AreaName nvarchar(100),ReseauName nvarchar(100),ProposedNetwork nvarchar(250),ProjectType int,GeneralDesignName nvarchar(100),DesignRealName nvarchar(50),DesignDate datetime,
		IsFile nvarchar(50))

	insert @temp
	select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			tbl_Planning.ProposedNetwork,tbl_ProjectTask.ProjectType,tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.DesignRealName,tbl_ProjectTask.DesignDate,
			isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_ProjectTask left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_ProjectTask.DesignRealName) > 0) and
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and
				(@UserId = '00000000-0000-0000-0000-000000000000' or tbl_Customer.CustomerUserId = @UserId)
		union
		select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			tbl_Remodeling.ProposedNetwork,tbl_ProjectTask.ProjectType,tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.DesignRealName,tbl_ProjectTask.DesignDate,
			isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_ProjectTask left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
		where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@DesignRealName = '' or CHARINDEX(@DesignRealName,tbl_ProjectTask.DesignRealName) > 0) and
				tbl_Place.Profession = @Profession  and tbl_ProjectTask.ProjectType<>1 and
				(@UserId = '00000000-0000-0000-0000-000000000000' or tbl_Customer.CustomerUserId = @UserId)
		
		select * from @temp t
			order by t.ProjectType asc,t.ProjectCode desc offset @PageStart row fetch next @PageSize rows only

	select count(*) from @temp
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryProjectDesignsPage]    Script Date: 2016-09-02 22:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryProjectDesignsPage]
					@PageIndex int,
					@PageSize int,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@PlaceName nvarchar(100),
					@ProjectCode nvarchar(50),
					@Profession int,
					@AreaManagerId uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	declare @temp table(indexid int identity(1,1),Id uniqueidentifier,PlaceId uniqueidentifier,ProjectCode nvarchar(50),PlaceName nvarchar(100),PlaceCategoryName nvarchar(100),
		AreaName nvarchar(100),ReseauName nvarchar(100),ProposedNetwork nvarchar(250),ProjectType int,GeneralDesignName nvarchar(100),DesignRealName nvarchar(50),DesignDate datetime,
		IsFile nvarchar(50))

	insert @temp
	select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			tbl_Planning.ProposedNetwork,tbl_ProjectTask.ProjectType,tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.DesignRealName,tbl_ProjectTask.DesignDate,
			isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_ProjectTask left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
							left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and				
				tbl_Place.Profession = @Profession and tbl_ProjectTask.ProjectType=1 and
				(@AreaManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.AreaManagerId = @AreaManagerId)
		union
		select tbl_ProjectTask.Id,tbl_ProjectTask.PlaceId,tbl_ProjectTask.ProjectCode,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
			tbl_Remodeling.ProposedNetwork,tbl_ProjectTask.ProjectType,tbl_Customer.CustomerName as GeneralDesignName,tbl_ProjectTask.DesignRealName,tbl_ProjectTask.DesignDate,
			isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_ProjectTask left join tbl_Remodeling on tbl_ProjectTask.ParentId=tbl_Remodeling.Id
							left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
							left join tbl_FileAssociation on tbl_ProjectTask.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'GeneralDesign'
		where (@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@ProjectCode = '' or CHARINDEX(@ProjectCode,tbl_ProjectTask.ProjectCode) > 0) and				
				tbl_Place.Profession = @Profession  and tbl_ProjectTask.ProjectType<>1 and
				(@AreaManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.AreaManagerId = @AreaManagerId)
		
		select * from @temp t
			order by t.ProjectType asc,t.ProjectCode desc offset @PageStart row fetch next @PageSize rows only

	select count(*) from @temp
		--from tbl_ProjectTask left join tbl_Addressing on tbl_ProjectTask.ParentId=tbl_Addressing.Id
		--						left join tbl_Planning on tbl_Addressing.PlanningId=tbl_Planning.Id
		--						left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
		--						left join tbl_Customer on tbl_ProjectTask.GeneralDesignId=tbl_Customer.Id
		--	where (@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Addressing.PlaceName) > 0) and
		--			(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
		--			(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
		--			(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
		--			(@GeneralDesignId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.GeneralDesignId = @GeneralDesignId) and
		--			(@GesignRealName = '' or CHARINDEX(@GesignRealName,tbl_ProjectTask.DesignRealName) > 0) and
		--			tbl_Planning.Profession = @Profession and
		--			(@AreaManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ProjectTask.AreaManagerId = @AreaManagerId)
END
GO
/****** Object:  StoredProcedure [dbo].[[prc_GenerateProjectCode]]    Script Date: 2016-08-30 15:14:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GenerateProjectCode]
					@Profession int,
					@GenerateDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @ProfessionName varchar(10),
			@HeadCode varchar(10),
			@SeedCode varchar(5),
			@SqlStatement nvarchar(max)

	if @Profession=1
	begin
		set @ProfessionName='JZ'
	end
	else if @Profession=2
	begin
		set @ProfessionName='SF'
	end
	else if @Profession=3
	begin
		set @ProfessionName='GZ'
	end
	else if @Profession=4
	begin
		set @ProfessionName='JK'
	end
	else if @Profession=5
	begin
		set @ProfessionName='JF'
	end
	else if @Profession=6
	begin
		set @ProfessionName='XL'
	end
	else if @Profession=7
	begin
		set @ProfessionName='GJ'
	end
	else
	begin
		set @ProfessionName='QT'
	end
	set @HeadCode='XM'+SUBSTRING(CAST(YEAR(@GenerateDate) as varchar(4)),3,2)+@ProfessionName
	set @SqlStatement = N'select top 1 @SeedCode = tbl_OrderCodeSeed.Seed
								from tbl_OrderCodeSeed left join tbl_ProjectTask on ''' + @HeadCode + ''' + tbl_OrderCodeSeed.Seed = tbl_ProjectTask.ProjectCode
								where tbl_ProjectTask.ProjectCode is null
								order by tbl_OrderCodeSeed.Seed'
	exec sp_executesql @SqlStatement,N'@SeedCode varchar(5) output',@SeedCode output
	select @HeadCode+@SeedCode
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryAddressingUsersPage]    Script Date: 2016-08-30 10:44:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryAddressingUsersPage]
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
					@Importance int,
					@IsAppoint int,
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
			tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.DetailedAddress,tbl_Planning.Remarks,tbl_Planning.ProposedNetwork,tbl_Planning.Importance,tbl_PlaceOwner.PlaceOwnerName,
			tbl_Planning.OptionalAddress,u2.FullName,tbl_Planning.CreateDate,ISNULL(u1.FullName,'') as AddressingUserFullName,
			case when u1.FullName is null then 0 
				else case tbl_Planning.AddressingState when 2 then DATEDIFF(DAY,tbl_Planning.AddressingUserDate,tbl_Planning.AddressingDate) 
				else DATEDIFF(DAY,tbl_Planning.AddressingUserDate,GETDATE()) end end as AddressingDays,
			tbl_Planning.Issued,tbl_Planning.AddressingState,tbl_Planning.PlaceId,isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_Planning left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_PlaceOwner on tbl_Planning.PlaceOwner=tbl_PlaceOwner.Id
							left join tbl_User u1 on tbl_Planning.AddressingUserId = u1.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
							left join tbl_FileAssociation on tbl_Planning.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Planning'
		where (tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Importance = 0 or tbl_Planning.Importance = @Importance) and
				(@IsAppoint = 0 or ((@IsAppoint=1 and tbl_Planning.AddressingUserId!='00000000-0000-0000-0000-000000000000') or (@IsAppoint=2 and tbl_Planning.AddressingUserId='00000000-0000-0000-0000-000000000000'))) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and tbl_Planning.Issued=1 and
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
				(@Importance = 0 or tbl_Planning.Importance = @Importance) and
				(@IsAppoint = 0 or ((@IsAppoint=1 and tbl_Planning.AddressingUserId!='00000000-0000-0000-0000-000000000000') or (@IsAppoint=2 and tbl_Planning.AddressingUserId='00000000-0000-0000-0000-000000000000'))) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and tbl_Planning.Issued=1 and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.CreateUserId = @CreateUserId)
END
GO
/****** Object:  StoredProcedure [dbo].[prc_QueryPlanningApplysPage]    Script Date: 2016-08-23 22:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_QueryPlanningApplysPage]
					@PageIndex int,
					@PageSize int,
					@BeginDate datetime,
					@EndDate datetime,
					@PlanningName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@Issued int,
					@CreateUserId uniqueidentifier,
					@Profession int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_PlanningApply.Id,tbl_PlanningApply.PlanningCode,tbl_PlanningApply.PlanningName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlanningApply.Lng,tbl_PlanningApply.Lat,
			tbl_PlanningApply.DetailedAddress,tbl_PlanningApply.Remarks,tbl_PlanningApply.Issued,planningUser.FullName as PlanningFullName,tbl_PlanningApply.DoState,
			tbl_PlanningApply.Importance,tbl_PlanningApply.PlanningAdvice,tbl_User.FullName,tbl_Department.DepartmentName,tbl_PlanningApply.CreateDate
		from tbl_PlanningApply left join tbl_Reseau on tbl_PlanningApply.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_User on tbl_PlanningApply.CreateUserId = tbl_User.Id
							left join tbl_Department on tbl_User.DepartmentId=tbl_Department.Id
							left join tbl_User planningUser on tbl_PlanningApply.PlanningUserId=planningUser.Id
		where (tbl_PlanningApply.CreateDate >= @BeginDate and tbl_PlanningApply.CreateDate < @EndDate) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_PlanningApply.PlanningName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_PlanningApply.ReseauId = @ReseauId) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_PlanningApply.CreateUserId = @CreateUserId) and
				(@Issued=0 or tbl_PlanningApply.Issued=@Issued) and
				tbl_PlanningApply.Profession = @Profession
		order by tbl_PlanningApply.PlanningCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_PlanningApply left join tbl_Reseau on tbl_PlanningApply.ReseauId = tbl_Reseau.Id
		where (tbl_PlanningApply.CreateDate >= @BeginDate and tbl_PlanningApply.CreateDate < @EndDate) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_PlanningApply.PlanningName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_PlanningApply.ReseauId = @ReseauId) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_PlanningApply.CreateUserId = @CreateUserId) and
				(@Issued=0 or tbl_PlanningApply.Issued=@Issued) and
				tbl_PlanningApply.Profession = @Profession
END
GO
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
					@Importance int,
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
			tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.DetailedAddress,tbl_Planning.Remarks,tbl_Planning.ProposedNetwork,tbl_Planning.Importance,tbl_PlaceOwner.PlaceOwnerName,
			tbl_Planning.OptionalAddress,u2.FullName,tbl_Planning.CreateDate,ISNULL(u1.FullName,'') as AddressingUserFullName,
			case when u1.FullName is null then 0 
				else case tbl_Planning.AddressingState when 2 then DATEDIFF(DAY,tbl_Planning.AddressingUserDate,tbl_Planning.AddressingDate) 
				else DATEDIFF(DAY,tbl_Planning.AddressingUserDate,GETDATE()) end end as AddressingDays,
			tbl_Planning.Issued,tbl_Planning.AddressingState,tbl_Planning.PlaceId,isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_Planning left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_PlaceOwner on tbl_Planning.PlaceOwner=tbl_PlaceOwner.Id
							left join tbl_User u1 on tbl_Planning.AddressingUserId = u1.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
							left join tbl_FileAssociation on tbl_Planning.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Planning'
		where (tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Importance = 0 or tbl_Planning.Importance = @Importance) and
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
				(@Importance = 0 or tbl_Planning.Importance = @Importance) and
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
					@GenerateDate datetime,
					@Profession int
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

	select @CodePrefix = tbl_WFCategory.CodePrefix from tbl_WFCategory where tbl_WFCategory.EntityName = @EntityName and tbl_WFCategory.Profession = @Profession
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
					@Importance int,
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

	select ISNULL(tbl_Addressing.Id,'00000000-0000-0000-0000-000000000000') as Id,tbl_Planning.Id as PlanningId,tbl_Planning.PlaceId,tbl_Planning.PlanningCode,tbl_Planning.PlanningName,
		ISNULL(tbl_Addressing.PlaceName,'') as PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.ProposedNetwork,
		ISNULL(tbl_Planning.Importance,0) as Importance,tbl_PlaceOwner.PlaceOwnerName,tbl_Planning.OptionalAddress,tbl_Addressing.OwnerName,tbl_Addressing.OwnerContact,tbl_Addressing.OwnerPhoneNumber,
		tbl_Planning.AddressingState,ISNULL(tbl_Department.DepartmentName,'') as AddressingDepartmentName,tbl_Addressing.AddressingRealName,addressingUser.FullName as AddressingUserName,
		case tbl_Planning.AddressingState when 2 then DATEDIFF(DAY,tbl_Planning.AddressingUserDate,tbl_Planning.AddressingDate) when 1 then DATEDIFF(DAY,tbl_Planning.AddressingUserDate,GETDATE()) else 0 end as AddressingDays,
		ISNULL(tbl_Addressing.Remarks,'') as Remarks,ISNULL(tbl_User.FullName,'') as AddressingUserFullName,ISNULL(u2.FullName,'') as FullName,tbl_Planning.CreateDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile 
		from tbl_Planning left join tbl_Addressing on tbl_Planning.Id = tbl_Addressing.PlanningId
							left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_PlaceOwner on tbl_Planning.PlaceOwner = tbl_PlaceOwner.Id
							left join tbl_User on tbl_Planning.AddressingUserId = tbl_User.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
							left join tbl_User addressingUser on tbl_Planning.AddressingUserId=addressingUser.Id
							left join tbl_Department on tbl_Addressing.AddressingDepartmentId=tbl_Department.Id
							left join tbl_FileAssociation on tbl_Addressing.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Addressing'
		where tbl_Planning.Issued = 1 and
				(tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
				(@Importance = 0 or tbl_Planning.Importance = @Importance) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				((@AddressingUserId = '00000000-0000-0000-0000-000000000000' and tbl_Planning.AddressingUserId='00000000-0000-0000-0000-000000000000') or tbl_Planning.AddressingUserId = @AddressingUserId)
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
				(@Importance = 0 or tbl_Planning.Importance = @Importance) and
				(@AddressingState = 0 or tbl_Planning.AddressingState = @AddressingState) and
				((@AddressingUserId = '00000000-0000-0000-0000-000000000000' and tbl_Planning.AddressingUserId='00000000-0000-0000-0000-000000000000') or tbl_Planning.AddressingUserId = @AddressingUserId)
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
					@Profession int,
					@PlaceName nvarchar(100),
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@PlaceOwner uniqueidentifier,
					@State int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Place.Id,tbl_Place.PlaceCode,tbl_Place.Profession,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.Lng,tbl_Place.Lat,tbl_Place.Importance,tbl_PlaceOwner.PlaceOwnerName,
			tbl_Place.OwnerName,tbl_Place.OwnerContact,tbl_Place.OwnerPhoneNumber,tbl_Place.G2Number,tbl_Place.D2Number,tbl_Place.G3Number,
			tbl_Place.AddressingRealName,tbl_Place.G4Number,tbl_Place.[State],tbl_User.FullName,tbl_Place.CreateDate,isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
						left join tbl_User on tbl_Place.CreateUserId = tbl_User.Id
						left join tbl_PlaceOwner on tbl_Place.PlaceOwner=tbl_PlaceOwner.Id
						left join tbl_FileAssociation on tbl_Place.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Place'
		where (@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and				
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PlaceOwner = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceOwner = @PlaceOwner) and
				(@State = 0 or tbl_Place.[State] = @State)
		order by tbl_Place.PlaceCode desc offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
		where (@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and				
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@PlaceOwner = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceOwner = @PlaceOwner) and
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
					@PlaceName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier,
					@ProjectType int,
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

	select tbl_Remodeling.Id,tbl_Remodeling.PlaceId,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
			tbl_Place.Lng,tbl_Place.Lat,tbl_PlaceOwner.PlaceOwnerName,tbl_ProjectTask.ProjectType,tbl_Remodeling.ProposedNetwork,
			tbl_Remodeling.Remarks,tbl_Remodeling.OrderState,tbl_User.FullName,tbl_Remodeling.CreateDate
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_PlaceOwner on tbl_Place.PlaceOwner=tbl_PlaceOwner.Id
							left join tbl_User on tbl_Remodeling.CreateUserId = tbl_User.Id
							left join tbl_ProjectTask on tbl_Remodeling.Id=tbl_ProjectTask.ParentId
		where (tbl_Remodeling.CreateDate >= @BeginDate and tbl_Remodeling.CreateDate < @EndDate) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_Remodeling.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectType = 0 or tbl_ProjectTask.ProjectType = @ProjectType) and
				(@OrderState = 0 or tbl_Remodeling.OrderState = @OrderState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Remodeling.CreateUserId = @CreateUserId) and
				tbl_ProjectTask.ProjectType<>1
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_ProjectTask on tbl_Remodeling.Id=tbl_ProjectTask.ParentId
		where (tbl_Remodeling.CreateDate >= @BeginDate and tbl_Remodeling.CreateDate < @EndDate) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_Remodeling.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ProjectType = 0 or tbl_ProjectTask.ProjectType = @ProjectType) and
				(@OrderState = 0 or tbl_Remodeling.OrderState = @OrderState) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Remodeling.CreateUserId = @CreateUserId) and
				tbl_ProjectTask.ProjectType<>1
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
			tbl_Place.Lng,tbl_Place.Lat,tbl_OperatorsSharing.PowerUsed,tbl_OperatorsSharing.PoleNumber,tbl_OperatorsSharing.CabinetNumber,
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
					@PlaceCode nvarchar(50),
					@PlaceName nvarchar(100),
					@Profession int,
					@PlaceCategoryId uniqueidentifier,
					@AreaId uniqueidentifier,
					@ReseauId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Place.Id,tbl_Place.PlaceCode,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.Profession,
			tbl_PlaceCategory.PlaceCategoryName,tbl_Place.Lng,tbl_Place.Lat,tbl_PlaceOwner.PlaceOwnerName,
			tbl_Reseau.AreaId,tbl_Place.ReseauId,tbl_Place.PlaceCategoryId
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
						left join tbl_PlaceOwner on tbl_Place.PlaceOwner=tbl_PlaceOwner.Id
		where tbl_Place.[State] = 1 and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId)
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
		where tbl_Place.[State] = 1 and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@Profession = 0 or tbl_Place.Profession = @Profession) and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId)
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
	select @CompanyName = CompanyName from tbl_Company where Id = '0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600'

	declare @TempBSPlanningPlaceCategory table(value uniqueidentifier);
	insert @TempBSPlanningPlaceCategory exec(@BSPlanningPlaceCategorySql);

	declare @TempBSPlaceCategory table(value uniqueidentifier);
	insert @TempBSPlaceCategory exec(@BSPlaceCategorySql);

	with cte as
	(
		select tbl_Planning.Id,tbl_Planning.PlanningName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,
				tbl_Reseau.ReseauName,tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.Profession,tbl_Planning.Issued,
				case tbl_Planning.AddressingState when 1 then '未寻址确认' when 2 then '已寻址确认' when 3 then '流转中' when 4 then '流程终止' else '' end as AddressingStateName,
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
	select cte.Id,cte.PlanningName,cte.PlaceCategoryName,cte.AreaName,cte.ReseauName,cte.Lng,cte.Lat,cte.Profession,cte.Issued,cte.AddressingStateName,
			'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' as CompanyId,@CompanyName as CompanyName,1 as DataType
		from cte
		where cte.Distance <= @Distance;
		
	with cte as
	(
		select tbl_Place.Id,tbl_Place.PlaceName,tbl_PlaceCategory.PlaceCategoryName,tbl_Area.AreaName,
				tbl_Reseau.ReseauName,tbl_Place.Lng,tbl_Place.Lat,
				tbl_Place.OwnerName,tbl_Place.OwnerContact,
				tbl_Place.OwnerPhoneNumber,tbl_Place.Profession,tbl_PlaceOwner.PlaceOwnerName,
				tbl_Place.G2Number,tbl_Place.D2Number,tbl_Place.G3Number,tbl_Place.G4Number,'' as NetWorks,tbl_Place.PlaceMapState,
				dbo.func_GetDistance(@Lng,@Lat,tbl_Place.Lng,tbl_Place.Lat) as Distance
			from tbl_Place left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceOwner on tbl_Place.PlaceOwner = tbl_PlaceOwner.Id
							left join @TempBSPlaceCategory t on tbl_Place.PlaceCategoryId = t.value
			where tbl_Place.Profession = 1 and
					tbl_Place.[State] = 1 and
					tbl_Place.Id <> @PlaceId and
					t.value is not null
	)
	select cte.Id,cte.PlaceName,cte.PlaceCategoryName,cte.AreaName,cte.ReseauName,cte.Lng,cte.Lat,
			cte.OwnerName,cte.OwnerContact,cte.OwnerPhoneNumber,cte.Profession,cte.PlaceOwnerName,cte.G2Number,cte.D2Number,cte.G3Number,cte.G4Number,'' as NetWorks,cte.PlaceMapState,2 as DataType
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

	select tbl_PlaceProperty.Id,tbl_PlaceProperty.Id as PlacePropertyId,tbl_ConstructionTask.Id as ConstructionTaskId,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
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

	select tbl_PlaceProperty.Id,tbl_PlaceProperty.Id as PlacePropertyId,tbl_ConstructionTask.Id as ConstructionTaskId,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
		tbl_PlaceCategory.PlaceCategoryName,ISNULL(tbl_Place.Importance,0) as Importance,tbl_ConstructionTask.ConstructionProgress,tbl_ConstructionTask.ProgressMemos,tbl_Project.ProjectName,
		tbl_PlaceProperty.MobilePoleNumber,tbl_PlaceProperty.MobileCabinetNumber,tbl_PlaceProperty.MobilePowerUsed,tbl_ConstructionTask.IsFinishMobile,ISNULL(Mobile.FullName,'') as MobileFullName,tbl_PlaceProperty.MobileCreateDate,
		tbl_PlaceProperty.TelecomPoleNumber,tbl_PlaceProperty.TelecomCabinetNumber,tbl_PlaceProperty.TelecomPowerUsed,tbl_ConstructionTask.IsFinishTelecom,ISNULL(Telecom.FullName,'') as TelecomFullName,tbl_PlaceProperty.TelecomCreateDate,
		tbl_PlaceProperty.UnicomPoleNumber,tbl_PlaceProperty.UnicomCabinetNumber,tbl_PlaceProperty.UnicomPowerUsed,tbl_ConstructionTask.IsFinishUnicom,ISNULL(Unicom.FullName,'') as UnicomFullName,tbl_PlaceProperty.UnicomCreateDate
		from tbl_ConstructionTask left join tbl_Remodeling on tbl_ConstructionTask.PlaceId=tbl_Remodeling.PlaceId
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
		from tbl_ConstructionTask left join tbl_Remodeling on tbl_ConstructionTask.PlaceId=tbl_Remodeling.PlaceId
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

	select tbl_Place.Id,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_PlaceCategory.PlaceCategoryName,tbl_Place.Lng,tbl_Place.Lat,'苏州联通' as CompanyName,
		2 as DataType,'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' as CompanyId,tbl_Reseau.ReseauName,tbl_Place.Lng,tbl_Place.Lat,tbl_Place.PlaceOwner,
		tbl_Place.OwnerName,tbl_Place.OwnerContact,tbl_Place.OwnerPhoneNumber,tbl_Place.Profession,tbl_PlaceOwner.PlaceOwnerName,tbl_Place.G2Number,
		tbl_Place.D2Number,tbl_Place.G3Number,tbl_Place.G4Number,'' as NetWorks,tbl_Place.PlaceMapState
		from tbl_Place left join tbl_Reseau on tbl_Place.ReseauId=tbl_Reseau.Id
						left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
						left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
						left join tbl_PlaceOwner on tbl_Place.PlaceOwner = tbl_PlaceOwner.Id
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
			tbl_Planning.Lng,tbl_Planning.Lat,tbl_Planning.Importance,0,0,0,0,
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
				(@Urgency = 0 or tbl_Planning.Importance = @Urgency) and
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
				(@Urgency = 0 or tbl_Planning.Importance = @Urgency) and
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
			tbl_Planning.Lng as '经度',tbl_Planning.Lat as '纬度','' as '紧要程度','' as '移动需求',
			'' as '电信需求','' as '联通需求',
			'' as '确认状态','' as '是否下达',
			'' as '寻址状态',ISNULL(u1.FullName,'') as '租赁人',tbl_Planning.Remarks as '备注',
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
			tbl_Place.Lng as '经度',tbl_Place.Lat as '纬度',--case tbl_Place.PropertyRight when 1 then '铁塔' when 2 then '购移动' when 3 then '购电信' when 4 then '购联通' else '' end as '产权',
			'' as '紧要程度',
			'' as '是否下达',tbl_Remodeling.Remarks as '备注',u2.FullName as '创建人',tbl_Remodeling.CreateDate as '创建日期',
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
			tbl_Place.Lng,tbl_Place.Lat,tbl_Place.PlaceOwner,
			tbl_Remodeling.Remarks,tbl_User.FullName,tbl_Remodeling.CreateDate,
			tbl_Remodeling.PlaceId,ISNULL(tbl_Project.ProjectName,'') as ProjectName,
			ISNULL(u3.FullName,'') as ProjectManagerName,ISNULL(tbl_ConstructionTask.ConstructionProgress,0) as ConstructionProgress,ISNULL(tbl_ConstructionTask.ProgressMemos,'') as ProgressMemos,
			ISNULL(tbl_ConstructionTask.Id,'00000000-0000-0000-0000-000000000000') as ConstructionTaskId
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User on tbl_Remodeling.CreateUserId = tbl_User.Id
							left join tbl_ConstructionTask on tbl_Remodeling.PlaceId=tbl_ConstructionTask.PlaceId and tbl_ConstructionTask.ConstructionMethod=2
							left join tbl_Project on tbl_ConstructionTask.ProjectId=tbl_Project.Id and tbl_ConstructionTask.ConstructionMethod=2
							left join tbl_User u3 on tbl_ConstructionTask.ProjectManagerId = u3.Id
		where (tbl_Remodeling.CreateDate >= @BeginDate and tbl_Remodeling.CreateDate < @EndDate) and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_Remodeling.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Remodeling.CreateUserId = @CreateUserId) and
				(@ProjectId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectId = @ProjectId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectManagerId = @ProjectManagerId) and
				(@ConstructionProgress = 0 or tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress) and
				tbl_Remodeling.OrderState=3
		order by tbl_Place.PlaceCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_ConstructionTask on tbl_Remodeling.PlaceId=tbl_ConstructionTask.PlaceId and tbl_ConstructionTask.ConstructionMethod=2
		where (tbl_Remodeling.CreateDate >= @BeginDate and tbl_Remodeling.CreateDate < @EndDate) and
				(@PlaceCode = '' or CHARINDEX(@PlaceCode,tbl_Place.PlaceCode) > 0) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				tbl_Remodeling.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Place.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@CreateUserId = '00000000-0000-0000-0000-000000000000' or tbl_Remodeling.CreateUserId = @CreateUserId) and
				(@ProjectId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectId = @ProjectId) and
				(@ProjectManagerId = '00000000-0000-0000-0000-000000000000' or tbl_ConstructionTask.ProjectManagerId = @ProjectManagerId) and
				(@ConstructionProgress = 0 or tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress) and
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
		tbl_Planning.Lng,tbl_Planning.Lat,'苏州联通' as CompanyName,1 as DataType,tbl_Planning.Profession,
		'0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600' as CompanyId,tbl_Reseau.ReseauName,tbl_Planning.Issued,
		case tbl_Planning.AddressingState when 1 then '未寻址确认' when 2 then '已寻址确认' when 3 then '流转中' when 4 then '流程终止' else '' end as AddressingStateName
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
					@CustomerFullName nvarchar(100),
					@CustomerType int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @PageStart int
	set @PageStart = @PageIndex * @PageSize

	select tbl_Customer.Id,tbl_Customer.CustomerType,tbl_Customer.CustomerCode,tbl_Customer.CustomerName,tbl_Customer.CustomerFullName,tbl_Customer.ContactMan,tbl_Customer.ContactTel,
			tbl_Customer.ContactAddr,tbl_Customer.[State],tbl_Customer.Remarks
		from tbl_Customer
		where (@CustomerCode = '' or CHARINDEX(@CustomerCode,tbl_Customer.CustomerCode) > 0) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_Customer.CustomerName) > 0) and
				(@CustomerFullName = '' or CHARINDEX(@CustomerFullName,tbl_Customer.CustomerFullName) > 0) and
				(@CustomerType=0 or tbl_Customer.CustomerType=@CustomerType)
		order by tbl_Customer.CustomerCode offset @PageStart row fetch next @PageSize rows only

	select COUNT(*)
		from tbl_Customer
		where (@CustomerCode = '' or CHARINDEX(@CustomerCode,tbl_Customer.CustomerCode) > 0) and
				(@CustomerName = '' or CHARINDEX(@CustomerName,tbl_Customer.CustomerName) > 0) and
				(@CustomerFullName = '' or CHARINDEX(@CustomerFullName,tbl_Customer.CustomerFullName) > 0) and
				(@CustomerType=0 or tbl_Customer.CustomerType=@CustomerType)
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
							left join tbl_Project on (tbl_MaterialList.PropertyType=1) or (tbl_MaterialList.PropertyType=2)
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
							left join tbl_Project on (tbl_MaterialList.PropertyType=1) or (tbl_MaterialList.PropertyType=2)
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

	select '' as '项目名称',tbl_Area.AreaName as '区域',tbl_Reseau.ReseauName as '网格',tbl_PlaceCategory.PlaceCategoryName as '基站类型',tbl_Place.PlaceName as '基站名称',
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

	select tbl_ConstructionTask.Id,tbl_Place.Id as PlaceId,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
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

	select tbl_TaskProperty.Id,tbl_Place.Id as PlaceId,tbl_TaskProperty.TaskModel as TaskModelId,tbl_ConstructionTask.Id as ConstructionTaskId,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,
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

	select tbl_Place.Id,tbl_Place.PlaceName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_Place.Profession,
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
				--(@PropertyRight = 0 or tbl_Place.PropertyRight = @PropertyRight) and
				(@Importance = 0 or tbl_Place.Importance = @Importance) and
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
				--(@PropertyRight = 0 or tbl_Place.PropertyRight = @PropertyRight) and
				(@Importance = 0 or tbl_Place.Importance = @Importance) and
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
		tbl_Place.PlaceName,tbl_Project.ProjectCode,tbl_Project.ProjectName,tbl_Area.AreaName,tbl_Reseau.ReseauName,tbl_PlaceCategory.PlaceCategoryName,
		tbl_Place.PlaceOwner,tbl_ConstructionTask.ConstructionMethod,tbl_ConstructionTask.ConstructionProgress,isnull(tbl_Tower.TowerType,0) as TowerType,isnull(tbl_Tower.TowerHeight,0) as TowerHeight,
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
							--left join @TempPropertyRight t on tbl_Place.PropertyRight = t.value
		where (tbl_ConstructionTask.CreateDate >= @BeginDate and tbl_ConstructionTask.CreateDate < @EndDate) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionMethod=0 or tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 5) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 5))
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
							--left join @TempPropertyRight t on tbl_Place.PropertyRight = t.value
		where (tbl_ConstructionTask.CreateDate >= @BeginDate and tbl_ConstructionTask.CreateDate < @EndDate) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionMethod=0 or tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 5) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 5))
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

	select tbl_Place.PlaceName as '基站名称',tbl_Project.ProjectCode as '项目编码',tbl_Project.ProjectName as '建设项目',tbl_Area.AreaName as '区域',
		tbl_Reseau.ReseauName as '网格',tbl_PlaceCategory.PlaceCategoryName as '基站类型',--case tbl_Place.PropertyRight when 1 then '铁塔' when 2 then '购移动' when 3 then '购电信' when 4 then '购联通' else '' end as '产权',
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
							--left join @TempPropertyRight t on tbl_Place.PropertyRight = t.value
		where (tbl_ConstructionTask.CreateDate >= @BeginDate and tbl_ConstructionTask.CreateDate < DATEADD(DAY,1,@EndDate)) and
				(@PlaceName = '' or CHARINDEX(@PlaceName,tbl_Place.PlaceName) > 0) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Place.ReseauId = @ReseauId) and
				(@ConstructionMethod=0 or tbl_ConstructionTask.ConstructionMethod = @ConstructionMethod) and
				(@ConstructionProgress = 0 or (tbl_ConstructionTask.ConstructionProgress = @ConstructionProgress and @ConstructionProgress <> 5) or ((tbl_ConstructionTask.ConstructionProgress = 1 or tbl_ConstructionTask.ConstructionProgress = 2) and @ConstructionProgress = 5))
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
			tbl_Planning.Lng,tbl_Planning.Lat,0,0,0,'' as SceneName,
			tbl_Addressing.OwnerName,tbl_Addressing.OwnerContact,tbl_Addressing.OwnerPhoneNumber,'' as ProjectName,tbl_Planning.AddressingState,
			tbl_Planning.Remarks,u2.FullName,tbl_Planning.CreateDate,tbl_Planning.PlaceId,isnull(tbl_FileAssociation.EntityName,'') as IsFile
		from tbl_Planning left join tbl_Reseau on tbl_Planning.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Planning.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User u2 on tbl_Planning.CreateUserId = u2.Id
							left join tbl_Addressing on tbl_Planning.Id = tbl_Addressing.PlanningId
							left join tbl_FileAssociation on tbl_Addressing.Id = tbl_FileAssociation.EntityId and tbl_FileAssociation.EntityName = 'Addressing'
		where (tbl_Planning.CreateDate >= @BeginDate and tbl_Planning.CreateDate < @EndDate) and
				(@PlanningCode = '' or CHARINDEX(@PlanningCode,tbl_Planning.PlanningCode) > 0) and
				(@PlanningName = '' or CHARINDEX(@PlanningName,tbl_Planning.PlanningName) > 0) and
				tbl_Planning.Profession = @Profession and
				(@PlaceCategoryId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.PlaceCategoryId = @PlaceCategoryId) and
				(@AreaId = '00000000-0000-0000-0000-000000000000' or tbl_Reseau.AreaId = @AreaId) and
				(@ReseauId = '00000000-0000-0000-0000-000000000000' or tbl_Planning.ReseauId = @ReseauId) and
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
			tbl_Place.Lng,tbl_Place.Lat,tbl_Place.PlaceOwner,case when osMobile.Id is null then '不需要' else '需要' end as MobileDemand1,
			case when osTelecom.Id is null then '不需要' else '需要' end as TelecomDemand,case when osUnicom.Id is null then '不需要' else '需要' end as UnicomDemand,
			tbl_Remodeling.OrderState,tbl_Remodeling.Remarks,tbl_User.FullName,tbl_Remodeling.CreateDate,
			tbl_Remodeling.PlaceId
		from tbl_Remodeling left join tbl_Place on tbl_Remodeling.PlaceId = tbl_Place.Id
							left join tbl_Reseau on tbl_Place.ReseauId = tbl_Reseau.Id
							left join tbl_Area on tbl_Reseau.AreaId = tbl_Area.Id
							left join tbl_PlaceCategory on tbl_Place.PlaceCategoryId = tbl_PlaceCategory.Id
							left join tbl_User on tbl_Remodeling.CreateUserId = tbl_User.Id
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

	--待处理基站建设申请
	--insert into @TempTask 
	--	select '','基站','基站建设申请',count(tbl_PlanningApply.Id),'BaseStationBM/PlanningApplyWait',1,1 
	--		from tbl_PlanningApply
	--		where Profession=1 and DoState=2 and Issued=1 and PlanningUserId=@UserId

	--寻址确认
	insert into @TempTask 
		select '','基站','寻址确认',count(tbl_Planning.Id),'BaseStationBM/Addressing',1,2 
			from tbl_Planning
			where Profession=1 and AddressingState=1 and Issued=1 and AddressingUserId=@UserId

	--施工设计
	insert into @TempTask 
		select '' as ProjectCode,'基站','施工设计',count(tbl_EngineeringTask.Id),'BaseStationBM/EngineeringDesign',1,3
			from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
									left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
									left join tbl_Customer on tbl_EngineeringTask.DesignCustomerId=tbl_Customer.Id
			where tbl_Place.Profession=1 and tbl_Customer.CustomerUserId=@UserId and tbl_EngineeringTask.DesignState=2 and
				(tbl_EngineeringTask.EngineeringProgress = 1 or tbl_EngineeringTask.EngineeringProgress = 2 or tbl_EngineeringTask.EngineeringProgress = 4)

	--项目进度登记
	insert into @TempTask 
		select '' as ProjectCode,'基站','项目进度登记',count(tbl_ProjectTask.Id),'BaseStationBM/ProjectProgress',1,4
			from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_Place.Profession=1 and tbl_ProjectTask.AreaManagerId=@UserId and
				(tbl_ProjectTask.ProjectProgress = 1 or tbl_ProjectTask.ProjectProgress = 2 or tbl_ProjectTask.ProjectProgress = 5)

	--工程进度登记
	insert into @TempTask 
		select '' as ProjectCode,'基站','工程进度登记',count(tbl_EngineeringTask.Id),'BaseStationBM/EngineeringProgress',1,5
			from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
									left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
									left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
									left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
			where tbl_Place.Profession=1 and  (construction.CustomerUserId=@UserId or supervision.CustomerUserId=@UserId or tbl_EngineeringTask.ProjectManagerId=@UserId) and
				(tbl_EngineeringTask.EngineeringProgress = 1 or tbl_EngineeringTask.EngineeringProgress = 2 or tbl_EngineeringTask.EngineeringProgress = 4)

	--待阅通知
	insert into @TempTask 
		select '' as ProjectCode,'基站',case tbl_Notice.NoticeType when 1 then '规划变更通知' else '' end,count(tbl_Notice.Id),'BaseStationBM/Notice',1,6
			from tbl_Notice
			where tbl_Notice.ReceiveUserId=@UserId and
				tbl_Notice.NoticeState = 1
			group by tbl_Notice.NoticeType

	--待处理室分建设申请
	--insert into @TempTask 
	--	select '','室分','室分建设申请',count(tbl_PlanningApply.Id),'IndoorDistributionBM/PlanningApplyWait',2,1 
	--		from tbl_PlanningApply
	--		where Profession=2 and DoState=2 and Issued=1 and PlanningUserId=@UserId

	--寻址确认
	insert into @TempTask 
		select '','室分','寻址确认',count(tbl_Planning.Id),'IndoorDistributionBM/Addressing',2,2 
			from tbl_Planning
			where Profession=2 and AddressingState=1 and Issued=1 and AddressingUserId=@UserId

	--施工设计
	insert into @TempTask 
		select '' as ProjectCode,'室分','施工设计',count(tbl_EngineeringTask.Id),'IndoorDistributionBM/EngineeringDesign',2,3
			from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
									left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
									left join tbl_Customer on tbl_EngineeringTask.DesignCustomerId=tbl_Customer.Id
			where tbl_Place.Profession=2 and tbl_Customer.CustomerUserId=@UserId and tbl_EngineeringTask.DesignState=2 and
				(tbl_EngineeringTask.EngineeringProgress = 1 or tbl_EngineeringTask.EngineeringProgress = 2 or tbl_EngineeringTask.EngineeringProgress = 4)

	--项目进度登记
	insert into @TempTask 
		select '' as ProjectCode,'室分','项目进度登记',count(tbl_ProjectTask.Id),'IndoorDistributionBM/ProjectProgress',2,4
			from tbl_ProjectTask left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
			where tbl_Place.Profession=2 and tbl_ProjectTask.AreaManagerId=@UserId and
				(tbl_ProjectTask.ProjectProgress = 1 or tbl_ProjectTask.ProjectProgress = 2 or tbl_ProjectTask.ProjectProgress = 5)

	--工程进度登记
	insert into @TempTask 
		select '' as ProjectCode,'室分','工程进度登记',count(tbl_EngineeringTask.Id),'IndoorDistributionBM/EngineeringProgress',2,5
			from tbl_EngineeringTask left join tbl_ProjectTask on tbl_EngineeringTask.ProjectTaskId=tbl_ProjectTask.Id
									left join tbl_Place on tbl_ProjectTask.PlaceId=tbl_Place.Id
									left join tbl_Customer construction on tbl_EngineeringTask.ConstructionCustomerId=construction.Id
									left join tbl_Customer supervision on tbl_EngineeringTask.SupervisionCustomerId=supervision.Id
			where tbl_Place.Profession=2 and  (construction.CustomerUserId=@UserId or supervision.CustomerUserId=@UserId or tbl_EngineeringTask.ProjectManagerId=@UserId) and
				(tbl_EngineeringTask.EngineeringProgress = 1 or tbl_EngineeringTask.EngineeringProgress = 2 or tbl_EngineeringTask.EngineeringProgress = 4)

	select * from @TempTask where TaskCount>0
END
GO