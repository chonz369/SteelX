CREATE TABLE [ItemPurchaseLogByCredit] (
	[IPLCreditID] bigint IDENTITY (1, 1) NOT NULL 
,	[ItemID] int NOT NULL 
,	[AID] int not NULL 
,	[Date] datetime NULL 
,	[Credits] int NULL 
,	[PlayerCredits] int NULL 
,	[Type] varchar (20) NULL 
,	CONSTRAINT [Account_PurchaseItemByCreditHistory] FOREIGN KEY 
	(
		[AID]
	) REFERENCES [Account] (
		[AID]
	)
,	CONSTRAINT [Item_PurchaseItemByCreditHistory] FOREIGN KEY 
	(
		[ItemID]
	) REFERENCES [Item] (
		[ItemID]
	)
) ON [PRIMARY]
GO

--CREATE NONCLUSTERED INDEX IX_ItemPurchaseLogByCredit_AID
--ON [ItemPurchaseLogByCredit]( [AID] )
--GO

CREATE CLUSTERED INDEX IX_ItemPurchaseLogByCredit_Date
ON [ItemPurchaseLogByCredit]( [Date] )
GO