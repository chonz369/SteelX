CREATE TABLE [Player]  (						-- Extension of Account Table but focused around GAME experience (Only one PLAYER per ACCOUNT... for now)
 	[AID] varchar(450) not null           		-- GUID, FK; Account associated with this item
, 	[Name] varchar(20) not null					-- Player IGN is different from User Login Id
, 	[PilotId] tinyint not null                  -- FK of active operator pilot for mech
, 	[LastMechId] bigint null                    -- FK of active mech in hanger
, 	[TotalExpPoints] bigint not null            -- Points used to display level and rank
, 	[AbilityPoints] smallint not null           -- Points available to improving player stats
, 	[RepairPoints] int not null                 -- Repair damage made to mechs/items
, 	[RechargePoints] int not null				-- Amount of times you can bypass spawn timer
, 	[Credits] int not null                      -- Regular Currency
, 	[Coins] smallint not null                   -- Premium Currency
, 	[RegDate] datetime                         	-- Date registered with game 
, 	[PlayTime] int not null                     --
, 	[GameCount] int not null                    --
, 	[KillCount] int not null                    --
, 	[AssistCount] int not null                  --
, 	[DeathCount] int not null                   --
, 	[Stat_HP] tinyint not null                  --
, 	[Stat_EN] tinyint not null                  --
, 	[Stat_SP] tinyint not null                  --
, 	[Stat_Move] tinyint not null                --
, 	[Stat_Scan] tinyint not null                --
, 	[Stat_Mark] tinyint not null                --
,	CONSTRAINT [Player_PK] PRIMARY KEY  CLUSTERED 
	(
		[Name]
	)  ON [PRIMARY] 
,	CONSTRAINT [Player_ActiveMech_FK] FOREIGN KEY 
	(
		[AID]
	) REFERENCES [Account] (
		[AID]
	)
) ON [PRIMARY]
GO


CREATE UNIQUE INDEX UQ_Player_AID ON [Player] ([AID]) WHERE [AID] IS NOT NULL;
GO