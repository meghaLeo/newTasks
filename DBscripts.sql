create database TasksDB
go
use TasksDB
go


Create table Parent_Task
(
Parent_ID int identity(1,1) Primary Key,
Parent_Task varchar(500) 
)
go


Create table Task
(
Task_ID int identity(1,1),
Parent_ID int null foreign key references Parent_Task(Parent_ID),
Task_Description varchar(500),
Start_Date datetime,
End_Date datetime,
[Priority] int ,
CONSTRAINT chk_Task_Priority CHECK ([Priority] BETWEEN 1 AND 30)
)

go