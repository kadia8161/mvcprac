SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[prcStudentCourseInsert]
@StudentId bigint output,
@Name nvarchar(300),
@DateOfBirth datetime,
@Gender TINYINT,
@CourseIds VARCHAR(max)
as
BEGIN
    if @StudentId > 0
    BEGIN
        update Student 
        set [Name] = @Name,
        DateOfBirth = @DateOfBirth,
        Gender = @Gender
        where StudentId = @StudentId
    end
    else
    BEGIN
        insert into Student ([Name],DateOfBirth,Gender)
        values (@Name,@DateOfBirth,@Gender)

        select @StudentId = SCOPE_IDENTITY();
    end

    --Split Comma Separated String
    create TABLE #tmpSplit (val varchar(max))
    declare @str varchar(max)
    set @str = isnull(@CourseIds,'')
    declare @tmpvar varchar(max) 
    while len(@str) > 0 
    BEGIN
        if(CHARINDEX(',',@str) > 0)
        BEGIN
            select @tmpvar = SUBSTRING(@str,1,CHARINDEX(',',@str)-1)
            set @str = substring(@str,(len(@tmpvar) + 2),(len(@str) - (len(@tmpvar)))) 
        END
        else
        begin
            select @tmpvar = @str   
            set @str  = ''
        end    
        insert into #tmpSplit VALUES (@tmpvar)        
    end 

    --Remove Un selected course 
    DELETE from StudentCourse
    where 
    StudentCourse.StudentId = @StudentId 
    and not EXISTS
    (select 1 from StudentCourse sc
    inner join #tmpSplit t on convert(int,t.val) = sc.CourseId
    where StudentId = @StudentId) 

    declare @CourseId int
    declare curtemp cursor 
    for 
    select val from #tmpSplit
    open curtemp
    FETCH next from curtemp into @CourseId
    WHILE @@FETCH_status = 0
    BEGIN         
        if not exists(select 1 from StudentCourse where StudentId = @StudentId and CourseId = @CourseId)
        BEGIN
            insert into StudentCourse (CourseId,StudentId)
            select @CourseId,@StudentId 
        END
        FETCH next from curtemp into @CourseId
    END
    CLOSE curtemp
    DEALLOCATE curtemp 

    drop table #tmpSplit
    
END

GO
