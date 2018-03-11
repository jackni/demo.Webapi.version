if NOT EXISTS(SELECT name FROM sys.server_principals WHERE name = 'demo') 
begin
    -- create accound if does not exists. 
	CREATE LOGIN [demo] WITH PASSWORD = '$userpassword$'

	CREATE USER [demo] 
	FOR LOGIN [demo]
	WITH DEFAULT_SCHEMA=[Demo]

	ALTER ROLE [db_owner] ADD MEMBER [demo]
	
end