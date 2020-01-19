CREATE TABLE [FileLog] ( -- Game files needed to download for play
	[FileID] int IDENTITY (1, 1) NOT NULL 
--,	[ServerID] smallint NULL 
,	[Filename] varchar(32) not NULL 
,	[FileType] varchar(32) not NULL 
,	[FileSize] int not NULL 
--,	[FileHash] varchar(450) not NULL 
,	[FileSalt] varchar(128) NULL 
,	[DirectoryPath] varchar(128) not NULL 
--,	[DownloadUrl] varchar(128) not NULL 
,	[DateCreated] datetime NULL
--,	[LastModified] datetime NULL
--,	[IsDeleted] bit not NULL default ('false') 
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_ServerLog_ID_Time
ON [FileLog]( [FileType] )
GO

CREATE NONCLUSTERED INDEX IX_ServerLog_ID_Time
ON [FileLog]( [ServerID], [FileID], [IsDeleted] )
GO

--CREATE CLUSTERED INDEX IX_ServerLog_Time
--ON [FileLog]( [Filename], [FileHash] )
--GO

--------------------------------
CREATE TABLE [ServerFileData] (
	[FileID] int /*IDENTITY (1, 1)*/ NOT NULL 
,	[ServerID] smallint NULL 
--,	[Filename] varchar(32) not NULL 
--,	[FileType] varchar(32) not NULL 
--,	[FileSize] int not NULL 
,	[FileHash] varchar(450) not NULL 
--,	[FileSalt] varchar(128) NULL 
--,	[DirectoryPath] varchar(128) not NULL 
,	[DownloadUrl] varchar(128) not NULL 
--,	[DateCreated] datetime NULL
,	[LastModified] datetime NULL
,	[IsDeleted] bit not NULL default ('false') 
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_ServerLog_ID_Time
ON [ServerFileData]( [FileID], [LastModified] )
GO

CREATE NONCLUSTERED INDEX IX_ServerLog_Time
ON [ServerFileData]( [ServerID], [FileID], [FileHash] )
GO

CREATE CLUSTERED INDEX IX_ServerLog_ID_Time
ON [ServerFileData]( [ServerID], [FileID] )--, [IsDeleted]
GO