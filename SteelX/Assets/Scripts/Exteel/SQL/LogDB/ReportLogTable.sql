CREATE TABLE report_abuse_info
(
	rid				int DEFAULT '0' NOT NULL identity primary key,
	violater	    varchar(64),
    reporter		varchar(24),
	gamesceneID	    int,
	objectid		int,
    reason	        tinyint,
	comment			varchar(300),		
	admin_comment	text,		
	[state]			tinyint,	 --resolved?	
	regdate			datetime
);

--primary key violator, reporter, reason (maybe date too?.. one report for reason per day/hour? or per game scene...)