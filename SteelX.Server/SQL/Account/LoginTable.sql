CREATE TABLE [Login]  (
 	[AID] varchar(450)               			-- GUID 
, 	[PasswordHash] varchar(20)                  -- One-way hash
, 	[LastConnDate] datetime null                -- Updates on successful sign-ins
, 	[LastIP] varchar(20)                        -- Last computer logged in from
-- Protect against multiple users logging in and modifying account
,    [SecurityStamp] varchar(max) NULL			-- if multiple endpoints are signed in and this doesnt match, they're kicked out
,    [ConcurrencyStamp] varchar(max) NULL		-- if multiple endpoints are accessing and changing, this protects against that
-- Added some protection against brute force entries...
,   [TwoFactorEnabled] bit NOT NULL
,   [LockoutEnd] datetimeoffset NULL
,   [LockoutEnabled] bit NOT NULL
,   [AccessFailedCount] tinyint NOT NULL
,	[ConfirmationToken] varchar(128) NULL
,	[LastPasswordFailureDate] datetime NULL
,	[PasswordFailuresSinceLastSuccess] tinyint NOT NULL DEFAULT(0),
	--,[PasswordChangedDate] datetime2 NULL
	--,[PasswordVerificationToken] nvarchar(128) NULL
	--,[PasswordVerificationTokenExpirationDate] datetime2 NULL
,	CONSTRAINT [Login_PK] PRIMARY KEY  CLUSTERED 
	(
		[AID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_Login_AID
ON [Login]( [AID] )
GO

--For Two Factor Auth Logins?...
CREATE TABLE [dbo].[UserTokens] (
    [UserId] varchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] varchar(max) NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);
GO