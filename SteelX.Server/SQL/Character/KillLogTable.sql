CREATE TABLE [KillLog] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[AttackerCID] [int] NULL ,
	[VictimCID] [int] NULL ,
	[Time] [datetime] NULL ,
	CONSTRAINT [KillLog_PK] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO