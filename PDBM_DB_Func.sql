----创建函数
CREATE FUNCTION [dbo].[func_GetDistance]
(
	-- Add the parameters for the function here
	@SourceLng float,
	@SourceLat float,
	@TargetLng float,
	@TargetLat float
)
RETURNS float
AS
BEGIN
	declare @Distance float
	if(@SourceLng = @TargetLng and @SourceLat = @TargetLat)
	begin
		set @Distance = 0;
	end
	else
	begin
		select @Distance = 6371.004 * ACOS(SIN(@SourceLat / 180 * PI()) * SIN(@TargetLat / 180 * PI()) + COS(@SourceLat / 180 * PI()) * COS(@TargetLat / 180 * PI()) * COS((@SourceLng - @TargetLng) / 180 * PI()))		
	end

	return @Distance
END

