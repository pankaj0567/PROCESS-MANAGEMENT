Create database ProcessManagementDb
GO
use ProcessManagementDb
GO
CREATE SCHEMA PM;
GO
create table PM.RoleMatrix
(
	Id int primary key identity,
	RoleName varchar(50)
)
GO
insert into PM.RoleMatrix 
	select 'Admin'
	union all
	select 'Supervisor'
	union all 
	select 'Agent'

select * from PM.RoleMatrix

GO
create table PM.UserDetails
(
	UserName Varchar(100) primary key,
	UserPassword VARchar(max),
	RoleMatrixId int references PM.RoleMatrix(Id)
)

GO
insert into PM.UserDetails
	select 'PT001' as UserName,'123456' as UserPassword,1 as RoleMatrixId

GO
create proc PM.CreateUserDetails
@UserName Varchar(100),
@UserPassword VARchar(max),
@RoleMatrixId int 
as
begin
		INSERT into PM.UserDetails
			select @UserName,@UserPassword,@RoleMatrixId
end
GO
select * from PM.UserDetails

GO
create proc PM.GetAllUserDetails 
as
begin
		select * from  PM.UserDetails
end

GO
create proc PM.ValidateUser 
@UserName Varchar(100),
@UserPassword VARchar(max)
as
begin
		select * from PM.UserDetails where UserName = @UserName and UserPassword= @UserPassword;
end

GO
drop table PM.UserProfile
GO
create table PM.UserProfile
(
	Id int primary key identity,
	FullName varchar(50),
	Email varchar(50),
    Phone varchar(50),
    CommunicationAddress varchar(50),
	Status bit,
	UserName varchar(100) references PM.userdetails(UserName)
)

GO
drop proc PM.CreateUserProfile
GO
create proc PM.CreateUserProfile
@FullName varchar(50),
@Email varchar(50) = null,
@Phone varchar(50) = null,
@CommunicationAddress varchar(50) = null,
@Status bit,
@UserName varchar(100) 
as 
begin
if(@UserName='1')
begin
	Select @FullName + Cast(floor(RAND() * 100) as varchar(max)) as UserName,floor(RAND() * 10000) as UserPassword,3 as RoleMatrixId 
		into #TempUserPassword

	INSERT INTO	PM.UserDetails
			Select * from #TempUserPassword

	INSERT INTO PM.UserProfile
		SELECT @FullName,@Email,@Phone, @CommunicationAddress, @Status ,(Select UserName from #TempUserPassword) as UserName 

	select UD.UserName,UserPassword, RoleMatrixId,FullName,Email,Phone, CommunicationAddress, [Status] 
	from #TempUserPassword UD
	inner join PM.UserProfile UP
	on UD.UserName = UP.UserName
end
else 
begin
	INSERT INTO PM.UserProfile
		SELECT @FullName,@Email,@Phone, @CommunicationAddress, @Status,@UserName
end
end

GO
select * from PM.UserDetails
select * from PM.UserProfile

GO
CREATE FUNCTION SplitString
(    
      @Input NVARCHAR(MAX),
      @Character CHAR(1)
)
RETURNS @Output TABLE (
      Item NVARCHAR(1000)
)
AS
BEGIN
      DECLARE @StartIndex INT, @EndIndex INT
 
      SET @StartIndex = 1
      IF SUBSTRING(@Input, LEN(@Input) - 1, LEN(@Input)) <> @Character
      BEGIN
            SET @Input = @Input + @Character
      END
 
      WHILE CHARINDEX(@Character, @Input) > 0
      BEGIN
            SET @EndIndex = CHARINDEX(@Character, @Input)
           
            INSERT INTO @Output(Item)
            SELECT SUBSTRING(@Input, @StartIndex, @EndIndex - 1)
           
            SET @Input = SUBSTRING(@Input, @EndIndex + 1, LEN(@Input))
      END
 
      RETURN
END
GO

drop proc PM.GetAllUser
GO
create proc PM.GetAllUser
@Role int
as
--exec PM.GetAllUser 2
begin
		select UD.UserName,UserPassword, RoleMatrixId,FullName,Email,Phone, CommunicationAddress, [Status] 
		from PM.UserDetails UD
		inner join PM.UserProfile UP
		on UD.UserName = UP.UserName
		where UD.RoleMatrixId IN (select Item from SplitString((case when @Role=1 then '1,2,3'
										when  @Role=2 then '2,3'
										else '3' end) 
										,',')
										)
end
GO

select * from PM.UserDetails UD
select * from PM.UserProfile UP


