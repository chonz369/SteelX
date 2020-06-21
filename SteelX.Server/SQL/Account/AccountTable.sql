CREATE TABLE [Account]  (
 	[AID] varchar(450) identity    				-- GUID 
, 	[UserID] varchar(64) not null				-- Username for logging in; rename to [Username]?
, 	[Fullname] varchar(128) null                -- Identification/correspondence (player behind monitor)
, 	[Age] tinyint null							--  
, 	[Sex] bit null                              -- Player behind monitor, not in-game mech
, 	[UStatusID] tinyint not null                -- FOREIGN KEY, But not really necessary
,	[CreateDate] datetime2 null					-- Date account created (different from date player was made)
, 	[PasswordQuestionID] tinyint null	        -- 
, 	[PasswordQuestionAnswer] varchar(30) null   -- 
, 	[Country] char(2) null                      -- Player region where they're logging in from
, 	[Email] varchar(256) not null              	-- Can be used for logging in or password recovery
,   [EmailConfirmed] bit NOT NULL default ('false')
, 	[DeleteFlag] bit not null default ('false') --  
, 	[HackingType] tinyint null                  -- Type of hacking you are being punished for
, 	[HackingRegTime] datetime null              -- Date registered for hacking punishment 
, 	[EndHackingBlockTime] datetime null         -- 
, 	[LastLoginTime] datetime null               -- Updates on successful sign-ins
, 	[LastLogoutTime] datetime null              -- 
, 	[ServerID]  tinyint null                  	-- 
,	CONSTRAINT [Account_PK] PRIMARY KEY  CLUSTERED 
	(
		[AID]
	)  ON [PRIMARY] 
	/*,CONSTRAINT [UserStatus_Account_FK] FOREIGN KEY 
	(
		[UStatusID]
	) REFERENCES [UserStatus] (
		[UStatusID]
	)*/
) ON [PRIMARY]
GO


CREATE UNIQUE INDEX UQ_Account_UserID ON [Account] ([UserID]) WHERE [UserID] IS NOT NULL;
CREATE NONCLUSTERED INDEX IX_Account_RegDate ON [Account]( [RegDate] );
CREATE NONCLUSTERED INDEX IX_Account_UserID ON [Account]( [UserID] );
CREATE NONCLUSTERED INDEX IX_Account_Email ON [Account]( [Email] );
CREATE NONCLUSTERED INDEX IX_Account_LastLoginTime ON [Account]( [LastLoginTime] );
CREATE NONCLUSTERED INDEX IX_Account_LastLogoutTime ON [Account]( [LastLogoutTime] );



/* Values for User Account Status... Not really needed for database, will be added into backend code though
CREATE TABLE [UserStatus] (
	[UStatusID] [int] NOT NULL ,
	[Name] [varchar] (128) NOT NULL ,
	CONSTRAINT [UserStatus_PK] PRIMARY KEY  CLUSTERED 
	(
		[UStatusID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

insert into userstatus(ustatusid, name) values(0,'free')
insert into userstatus(ustatusid, name) values(1,'Subscription Account') --not needed if no user contribution
insert into userstatus(ustatusid, name) values(2,'Best Gamer')
insert into userstatus(ustatusid, name) values(100,'Marked Man') --Flagged for Report
insert into userstatus(ustatusid, name) values(101,'First Warning')
insert into userstatus(ustatusid, name) values(102,'Second Warning')
insert into userstatus(ustatusid, name) values(103,'Third Warning')
insert into userstatus(ustatusid, name) values(104,'Chatting Forbidden')
insert into userstatus(ustatusid, name) values(105,'Time Block')
insert into userstatus(ustatusid, name) values(252,'Event Manager')
insert into userstatus(ustatusid, name) values(253,'Permanent Block')
insert into userstatus(ustatusid, name) values(254,'Developer')
insert into userstatus(ustatusid, name) values(255,'Manager')
GO
*/