CREATE TABLE [ClanGameLog] (
	[CGLID] int IDENTITY (1, 1) NOT NULL ,
	[WinnerCLID] int NOT NULL ,
	[LoserCLID] int NOT NULL ,
	[WinnerClanName] varchar (24) NULL ,
	[LoserClanName] varchar (24) NULL ,
	[WinnerMembers] varchar (110) NULL ,
	[LoserMembers] varchar (110) NULL ,
	[RoundWins] tinyint NOT NULL ,
	[RoundLosses] tinyint NOT NULL ,
	[MapID] tinyint NOT NULL ,
	[GameType] tinyint NOT NULL ,
	[RegDate] datetime NOT NULL ,
	[WinnerPoint] int NULL ,
	[LoserPoint] int NULL ,
	CONSTRAINT [ClanGameLog_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[CGLID]
	)  ON [PRIMARY] ,
	CONSTRAINT [ClanGameLog_LoserCLID_FK] FOREIGN KEY 
	(
		[LoserCLID]
	) REFERENCES [Clan] (
		[CLID]
	),
	CONSTRAINT [ClanGameLog_WinnerCLID_FK] FOREIGN KEY 
	(
		[WinnerCLID]
	) REFERENCES [Clan] (
		[CLID]
	)
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_ClanGameLog_LoserCLID
ON [ClanGameLog]( [LoserCLID] )
GO

CREATE CLUSTERED INDEX IX_ClanGameLog_RegDate
ON [ClanGameLog]( [RegDate] )
GO

CREATE NONCLUSTERED INDEX IX_ClanGameLog_WinnerCLID
ON [ClanGameLog]( [WinnerCLID] )
GO