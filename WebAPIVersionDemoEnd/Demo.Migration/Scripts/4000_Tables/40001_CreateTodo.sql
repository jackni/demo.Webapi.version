IF OBJECT_ID(N'Demo.Todo', N'U') IS NULL
BEGIN
	CREATE TABLE [Demo].[Todo](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[CreatedDateTime] [datetimeoffset](7) NOT NULL,
	[RowVersion] [timestamp] NOT NULL
 CONSTRAINT [PK_Todo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END