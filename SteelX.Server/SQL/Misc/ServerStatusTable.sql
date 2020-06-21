CREATE TABLE [ServerStatus] (
	[ServerID] int NOT NULL ,
	[CurrPlayer] smallint NULL ,
	[MaxPlayer] smallint NULL ,
	[Time] datetime NULL ,
	[IP] varchar (32) NULL ,
	[Port] smallint NULL ,
	[ServerName] varchar (64) NULL ,
	[Opened] tinyint NULL ,
	[Type] int null,
	CONSTRAINT [ServerStatus_PK] PRIMARY KEY CLUSTERED 
	(
		[ServerID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

/*
create table ServerType(Type int primary key, Description varchar(128))
GO

insert into ServerType(Type, Description) values(2, 'normal');
insert into ServerType(Type, Description) values(3, 'clan');
insert into ServerType(Type, Description) values(4, 'laststand');
insert into ServerType(Type, Description) values(6, 'debug & test');
GO
*/