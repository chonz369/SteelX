CREATE TABLE [GameLog]  (						
 	[ID] bigint identity(1,1) primary key       -- 
, 	[MasterPID] bigint                         	-- FK of player
, 	[MapID] tinyint                     		-- string or FK of map
, 	[GameName] varchar(64)                      -- 
, 	[GameTypeID] tinyint                        -- 
, 	[StartTime] datetime null                   -- if null == waiting in lobby
, 	[Round] tinyint                         	-- Score/Duration?...
, 	[PlayerCount] tinyint                       -- 
, 	[PlayerList] varchar(1000)                  -- Array of Player IDs 
);	

CREATE CLUSTERED INDEX IX_GameLog_StartTime
ON [GameLog]( [StartTime] )
GO