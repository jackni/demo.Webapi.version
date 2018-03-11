IF NOT EXISTS(select * from sys.schemas where name='Demo') 
begin
	EXEC sp_executesql N'CREATE SCHEMA Demo' --AUTHORIZATION [dbo]
end