CREATE TABLE [Clan] (
	[CLID] int IDENTITY (1, 1) NOT NULL 
,	[Name] varchar (24) NULL 
,	[Exp] int NOT NULL DEFAULT(0)
,	[Level] tinyint NOT DEFAULT (1)
,	[Point] int NOT NULL DEFAULT (1000)
,	[MasterAID] int NULL 
,	[Wins] int NOT NULL DEFAULT (0)
,	[MarkWebImg] varchar (48) NULL 
,	[Introduction] varchar (1024) NULL 
,	[RegDate] datetime NOT NULL 
,	[DeleteFlag] tinyint NULL DEFAULT (0)
,	[DeleteName] varchar (24) NULL 
,	[Homepage] varchar (128) NULL 
,	[Losses] int NOT NULL DEFAULT (0)
,	[Draws] int NOT NULL DEFAULT (0)
,	[Ranking] int NOT NULL DEFAULT (0)
,	[TotalPoint] int NOT NULL DEFAULT (0)
,	[Cafe_Url] varchar (20) NULL 
,	[Email] varchar (70) NULL 
,	[EmblemUrl] varchar (256) NULL 
,	[RankIncrease] int NOT NULL DEFAULT (0)
,	[EmblemChecksum] int NOT NULL DEFAULT (0)
,	[LastDayRanking] int NOT NULL DEFAULT (0)
,	[LastMonthRanking] int NOT NULL DEFAULT (0)
,	[DeleteDate] DATETIME
,	CONSTRAINT [Clan_PK] PRIMARY KEY CLUSTERED 
	(
		[CLID]
	)  ON [PRIMARY] 
,	CONSTRAINT [Clan_Account_FK] FOREIGN KEY 
	(
		[MasterAID]
	) REFERENCES [Account] (
		[AID]
	)
) ON [PRIMARY] 
GO

CREATE NONCLUSTERED INDEX IX_Clan_DeleteFlag
ON [Clan]( [DeleteFlag] )
GO

CREATE NONCLUSTERED INDEX IX_Clan_MasterAID
ON [Clan]( [MasterAID] )
GO

CREATE NONCLUSTERED INDEX IX_Clan_Name
ON [Clan]( [Name] )
GO

CREATE NONCLUSTERED INDEX IX_Clan_Ranking
ON [Clan]( [Ranking] )
GO

CREATE NONCLUSTERED INDEX IX_Clan_RegDate
ON [Clan]( [RegDate] DESC )
GO