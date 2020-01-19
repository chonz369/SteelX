CREATE TABLE [ServerLog] (
	[id] int IDENTITY (1, 1) NOT NULL 
,	[ServerID] smallint NULL 
,	[PlayerCount] smallint NULL 
,	[GameCount] smallint NULL 
,	[Time] smalldatetime NULL
,   [BlockCount] int not null default (0)
,   [NonBlockCount] int not null default (0)
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_ServerLog_ID_Time
ON [ServerLog]( [ServerID], [Time] )
GO

CREATE CLUSTERED INDEX IX_ServerLog_Time
ON [ServerLog]( [Time] )
GO