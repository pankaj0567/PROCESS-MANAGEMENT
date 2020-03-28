Create database ProcessManagementDb
use ProcessManagementDb

CREATE SCHEMA PM;

create table PM.RoleMatrix
(
	Id int primary key identity,
	RoleName varchar(50)
)

insert into PM.RoleMatrix 
	select 'Admin'
	union all
	select 'Supervisor'
	union all 
	select 'Agent'

select * from PM.RoleMatrix

create table PM.UserDetails
(
	UserName Varchar(100) primary key,
	UserPassword VARchar(max),
	RoleMatrixId int references PM.RoleMatrix(Id)
)


create proc PM.CreateUserDetails
@UserName Varchar(100),
@UserPassword VARchar(max),
@RoleMatrixId int 
as
begin
		INSERT into PM.UserDetails
			select @UserName,@UserPassword,@RoleMatrixId
end

select * from PM.UserDetails


create proc PM.GetAllUserDetails 
as
begin
		select * from  PM.UserDetails
end





