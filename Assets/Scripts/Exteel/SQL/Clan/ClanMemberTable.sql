CREATE TABLE [ClanMember] (
	[CMID] int IDENTITY (1, 1) NOT NULL  --Dont really need an index, since only one Acc per Clan
,	[CLID] int NULL 
,	[AID] int NULL 
,	[Grade] tinyint NOT NULL 
,	[RegDate] datetime NOT NULL 
,	[ContPoint] int NOT NULL DEFAULT (0)
,	CONSTRAINT [ClanMember_PK] PRIMARY KEY CLUSTERED --remove column & index?
	(
		[CMID]
	)  ON [PRIMARY] 
,	CONSTRAINT [ClanMember_Clan_FK] FOREIGN KEY 
	(
		[CLID]
	) REFERENCES [Clan] (
		[CLID]
	)
,	CONSTRAINT [ClanMember_Account_FK] FOREIGN KEY --I would make this primary key
	(
		[AID]
	) REFERENCES [Account] (
		[AID]
	)
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_ClanMember_AID
ON [ClanMember]( [AID] )
GO

CREATE NONCLUSTERED INDEX IX_ClanMember_CLID --I would cluster this index
ON [ClanMember]( [CLID] )
GO

/*
create table ClanMemberGrade(
    GradeID int primary key clustered
,   Grade varchar(24) 
);
go

insert into ClanMemberGrade(gradeid, grade)
values( 0, 'None' );
insert into ClanMemberGrade(gradeid, grade)
values( 1, 'Master' );
insert into ClanMemberGrade(gradeid, grade)
values( 2, 'Admin' );
insert into ClanMemberGrade(gradeid, grade)
values( 9, 'Member' );
GO
*/