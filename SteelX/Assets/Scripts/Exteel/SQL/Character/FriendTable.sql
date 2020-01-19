CREATE TABLE [Friend]  (						
 	[ID] bigint identity(1,1) primary key       -- index isnt needed if both FKs are Primary key...
, 	[PID] bigint                         		-- FK of player
, 	[FriendID] bigint                     		-- FK of player
, 	[Type] tinyint                         		-- 
, 	[Favorite] bit                         		-- 
, 	[DeleteFlag] bit                         	--
);	