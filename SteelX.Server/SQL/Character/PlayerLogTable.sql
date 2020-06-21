CREATE TABLE [PlayerLog]  (						-- Log amd track performance in between game sessions
 	[AID] varchar(450)                     		-- GUID, FK; Account associated with this item
, 	[ExpPoints] int not null                   	-- Points gained during play session
, 	[TotalExpPoints] bigint not null            -- Points at the end of play session
, 	[PlayTime] int not null                     --
, 	[AssistCount] int not null                  --
, 	[DeathCount] int not null                   --
-- Not sure if to move everything below to [PlayerTable] or create an [OperatorTable]...
-- Was the amount of times a player died while using a particular weapon also recorded?
-- would assist tracking assist values per every weapon be a waste of data/space? 
, 	[KillCount_SMGS] int not null               --
, 	[KillCount_RIFLES] int not null             --
, 	[KillCount_ROCKETS] int not null            --
, 	[KillCount_SHOTGUNS] int not null           --
, 	[KillCount_SPEARS] int not null             --
, 	[KillCount_BLADES] int not null             --
, 	[KillCount_CANNONS] int not null            --
, 	[AssistCount_RECTIFIERS] int not null       --
, 	[DamageCount_SMGS] int not null             --
, 	[DamageCount_RIFLES] int not null           --
, 	[DamageCount_ROCKETS] int not null          --
, 	[DamageCount_SHOTGUNS] int not null         --
, 	[DamageCount_SPEARS] int not null           --
, 	[DamageCount_BLADES] int not null           --
, 	[DamageCount_CANNONS] int not null          --
, 	[HealthHealCount_RECTIFIERS] int not null   --
, 	[DamageBlockCount_SHIELD] int not null      --
,	CONSTRAINT [PlayerLog_PK] PRIMARY KEY  CLUSTERED 
	(
		[AID] 									-- Maybe an ID for identity column?
	)  ON [PRIMARY] 
,	CONSTRAINT [PlayerLog_Account_FK] FOREIGN KEY 
	(
		[AID]
	) REFERENCES [Account] (
		[AID]
	)
);	


--CREATE UNIQUE INDEX UQ_PlayerLog_AID 			-- Dont think unique and clustered PK is needed
--ON [PlayerLog] ([AID]) 
--WHERE [AID] IS NOT NULL
--GO

--CREATE NONCLUSTERED INDEX IX_PlayerLog_AID 
--ON [PlayerLog]( [AID] )
--GO

CREATE NONCLUSTERED INDEX IX_PlayerLog_DisTime
ON [PlayerLog]( [DisTime] )
GO