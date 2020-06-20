CREATE TABLE [Mechanaught]  (
 	[MID] bigint identity not null   	    	-- Item id created when purchased by user account
, 	[AID] varchar(450)                          -- GUID, FK; Account associated with this item
, 	[Name] int not null                         -- Name of assembled and completed mech
, 	[Position] tinyint not null                 -- Rank/slot position number in hanger of this mech
, 	[DeleteFlag] bit not null default('false')  --
, 	[HeadID] int not null                       --
, 	[CoreID] int not null                       --
, 	[ArmID] int not null                        --
, 	[LegID] int not null                        --
, 	[BoosterID] int not null                    --
, 	[WeaponL1ID] int not null                   --
, 	[WeaponR1ID] int not null                   --
, 	[WeaponL2ID] int not null                   --
, 	[WeaponR2ID] int not null                   --
, 	[Skill1ID] int null                         --
, 	[Skill2ID] int null                         --
, 	[Skill3ID] int null                         --
, 	[Skill4ID] int null                         --
, 	[HeadColor] tinyint not null                --
, 	[CoreColor] tinyint not null                --
, 	[ArmColor] tinyint not null                 --
, 	[LegColor] tinyint not null                 --
, 	[BoosterColor] tinyint not null             --
,	CONSTRAINT [Mechanaught_PK] PRIMARY KEY  CLUSTERED 
	(
		[MID]
	)  ON [PRIMARY] 
,	CONSTRAINT [Player_Mechanaught_FK] FOREIGN KEY 
	(
		[AID]
	) REFERENCES [Player] ( --Might need to reference account instead... not sure if this will work
		[AID]
	)
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_Mechanaught_AID 
ON [Mechanaught]( [AID], [Position] )
GO

CREATE NONCLUSTERED INDEX IX_Mechanaught_AID_DeleteFlag
ON [Mechanaught]( [AID], [DeleteFlag] )
GO

CREATE NONCLUSTERED INDEX IX_Mechanaught_DeleteFlag
ON [Mechanaught]( [DeleteFlag] )
GO

CREATE NONCLUSTERED INDEX IX_Mechanaught_Name
ON [Mechanaught]( [Name] )
GO