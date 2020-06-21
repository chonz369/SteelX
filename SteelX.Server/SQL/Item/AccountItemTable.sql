CREATE TABLE [AccountItem]  ( 					-- Can rename to [PlayerItem] instead (Player Id is based on Account Id)
 	[AIID] bigint identity    	    			-- Item id created when purchased by user account
, 	[AID] varchar(450) not null                 -- GUID, FK; Account associated with this item
, 	[ItemID] int not null                       -- FOREIGN KEY
, 	[Durability] int null default (0)          	-- Used for tracking repair points on mech
, 	[RentDate] datetime null                    -- Date registered for rent with player
, 	[RentHourPeriod] smallint not null          -- Duration of hours before item expires from date of purchase 
, 	[Cnt] smallint not null default (0)         -- Count/Quantity of amount purchased...
,	CONSTRAINT [AccountItem_PK] PRIMARY KEY  CLUSTERED 
	(
		[AIID] --Clustered on AID,ItemID,AIID?
	)  ON [PRIMARY] 
,	CONSTRAINT [Account_AccountItem_FK] FOREIGN KEY 
	(
		[AID]
	) REFERENCES [Account] (
		[AID]
	)
,	CONSTRAINT [Item_AccountItem_FK] FOREIGN KEY 
	(
		[ItemID]
	) REFERENCES [Item] (
		[ItemID]
	)
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_AccountItem_AID
ON [AccountItem]( [AID] )
GO

CREATE NONCLUSTERED INDEX IX_AccountItem_Item
ON [AccountItem]( [AID], [ItemID] )
GO

/* Table isnt needed, i'm just putting this here for the days to hour math...
CREATE TABLE RentPeriodDay( 
 	[Day] int PRIMARY KEY CLUSTERED NOT NULL
, 	[Hour] int NOT NULL UNIQUE 
);
GO

INSERT INTO RentPeriodDay( [Day], [Hour] )
VALUES (0, 0);
INSERT INTO RentPeriodDay( [Day], [Hour] )
VALUES (7, 168);
INSERT INTO RentPeriodDay( [Day], [Hour] )
VALUES (15, 360);
INSERT INTO RentPeriodDay( [Day], [Hour] )
VALUES (30, 720);
INSERT INTO RentPeriodDay( [Day], [Hour] )
VALUES (90, 2160);
INSERT INTO RentPeriodDay( [Day], [Hour] )
VALUES (180, 4320);
INSERT INTO RentPeriodDay( [Day], [Hour] )
VALUES (365, 8760);
GO
*/