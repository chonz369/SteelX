CREATE TABLE [AccountPenaltyLog]  (
 	[PenaltyLogID] bigint identity   			-- GUID 
, 	[AID] varchar(450) not null					-- FORIEGN KEY GUID
, 	[UStatusID] tinyint not null                -- FOREIGN KEY
, 	[DaysLeft] tinyint not null                 -- Days remaining for punishment
, 	[RegDate] datetime not null            		-- Date registered for punishment 
, 	[GMID]  tinyint not null              		-- FK; Admin responsible for punishment
,	CONSTRAINT [AccountPenaltyLog_PK] PRIMARY KEY  CLUSTERED 
	(
		[PenaltyLogID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_AccountPenaltyLog_AID
ON [AccountPenaltyLog]( [AID] )
GO

CREATE NONCLUSTERED INDEX IX_AccountPenaltyLog_RegisteredDate
ON [AccountPenaltyLog]( [RegDate] )
GO