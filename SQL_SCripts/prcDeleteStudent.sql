SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[prcDeleteStudent]
@StudentId bigint
AS
BEGIN
    Delete from StudentCourse
    where StudentId = @StudentId

    Delete From Student
    where StudentId = @StudentId
END


GO
