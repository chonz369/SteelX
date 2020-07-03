namespace SteelX.Shared
{
	public enum Ranks
	{
		/// <summary>
		/// N/A
		/// </summary>
		Trainee = 0,
		/// <summary>
		/// 3 Ability Points
		/// 2500 Credits
		/// </summary>
		Private		= 3,
		/// <summary>
		/// 3 Ability Points
		/// 2500 Credits
		/// </summary>
		Private_First_Class= 5,
		/// <summary>
		/// 3 Ability Points
		/// 2500 Credits
		/// </summary>
		Corporal= 7,
		/// <summary>
		/// 3 Ability Points
		/// 2500 Credits
		/// 1 Additional Mechanaught Slot
		/// </summary>
		Sergeant= 10,
		/// <summary>
		/// 3 Ability Points
		/// 15,000 Credits
		/// 10 Extra Hangar Slots
		/// </summary>
		Staff_Sergeant= 15,
		/// <summary>
		/// 3 Ability Points
		/// 3000 Credits
		/// </summary>
		Technical_Sergeant= 20,
		/// <summary>
		/// 3 Ability Points
		/// 3000 Credits
		/// </summary>
		Master_Sergeant= 25,
		/// <summary>
		/// 3 Ability Points
		/// 3000 Credits
		/// 1 Additional Mechanaught Slot
		/// </summary>
		Chief_Master_Sergeant= 30,
		/// <summary>
		/// 3 Ability Points
		/// 25000 Credits
		/// </summary>
		Second_Lieutenant= 36,
		/// <summary>
		/// 3 Ability Points
		/// 3000 Credits
		/// </summary>
		First_Lieutenant= 41,
		/// <summary>
		/// 3 Ability Points
		/// 3000 Credits
		/// 15 Extra Hanger Slots
		/// 1 Additional Mechanaught Slot
		/// </summary>	
		Captain= 46,
		/// <summary>
		/// 3 Ability Points
		/// 3000 Credits
		/// 20 Extra Hanger Slots
		/// </summary>		
		Major= 51,
	}

	#region Weapon Enums
	public enum Weaponz
	{
		NONE = 0
	}
	public enum WeaponTypes
	{
		NONE = 0,
		Shields,
		Rectifiers,
		Rifles,
		Rockets,
		SMGs,
		Shotguns,
		Spears,
		Blades,
		Cannons
	}

	public enum BlowTypes
	{
		NONE
	}
	#endregion

	#region Mech Parts
	public enum Parts
	{
		/// <summary>Jaywalker</summary>
		HDJ606,
		/// <summary>Hellfire</summary>
		HDM709,
		/// <summary>Spazmok</summary>
		HDS707,
		/// <summary>Ballistika</summary>
		HDM806,
		/// <summary>Mekhi</summary>
		HDM402,
		/// <summary>Frontliner</summary>
		HDS308,
		/// <summary>Rushnik</summary>
		HDS003,
		/// <summary>Trooper</summary>
		HDM002,
		/// <summary>Boomrocker</summary>
		HDH004,
		/// <summary>Stallion</summary>
		HDS008,
		/// <summary>Stark</summary>
		HDM007,
		/// <summary>Haskell</summary>
		HDH009,
		/// <summary>Scootwing</summary>
		HDS107,
		/// <summary>Davenstar</summary>
		HDM505,
		/// <summary>Zeeker</summary>
		HDM803,
		/// <summary>Blitzker</summary>
		HDH701,
		/// <summary>Pinkett</summary>
		HDP010,
		/// <summary>Hellbent Butcher</summary>
		HDH802,
		/// <summary>Bastion</summary>
		HDW201,
		/// <summary>Aerobolt</summary>
		HDS808,
		/// <summary>Vigilant</summary>
		HDH810,
		/// <summary>Sidewinder</summary>
		HDM809,
		/// <summary>GameMaster</summary>
		HGM001,
		/// <summary>Vindicator</summary>
		HDM819,
		/// <summary>Guardian</summary>
		HDH820,
		/// <summary>Centurion</summary>
		HDS818,
		/// <summary>Valkyrie</summary>
		HGS001,
		/// <summary>Sting X</summary>
		HSM002,
		/// <summary>Aerene</summary>
		HGH003,
		/// <summary>Gamma Ray</summary>
		HSS001,
		/// <summary>Luciferiel</summary>
		HGM002,
		/// <summary>Big Mammoth</summary>
		HB,
		/// <summary>Jaywalker</summary>
		CEJ606,
		/// <summary>Hellfire</summary>
		CEM709,
		/// <summary>Spazmok</summary>
		CES707,
		/// <summary>Ballistika</summary>
		CEM806,
		/// <summary>Mekhi</summary>
		CEM412,
		/// <summary>Frontliner</summary>
		CES301,
		/// <summary>Rushnik</summary>
		CES003,
		/// <summary>Trooper</summary>
		CNM002,
		/// <summary>Boomrocker</summary>
		CEH007,
		/// <summary>Stallion</summary>
		CES008,
		/// <summary>Stark</summary>
		CNM007,
		/// <summary>Haskell</summary>
		CEH009,
		/// <summary>Scootwing</summary>
		CES103,
		/// <summary>Davenstar</summary>
		CEM571,
		/// <summary>Zeeker</summary>
		CEM803,
		/// <summary>Blitzker</summary>
		CEH737,
		/// <summary>Hellbent Butcher</summary>
		CEH807,
		/// <summary>Bastion</summary>
		CEW201,
		/// <summary>Aerobolt</summary>
		CES210,
		/// <summary>Vigitant</summary>
		CEH410,
		/// <summary>Sidewinder</summary>
		CEM310,
		/// <summary>GameMaster</summary>
		CGM001,
		/// <summary>Vindicator</summary>
		CEM320,
		/// <summary>Guardian</summary>
		CEH420,
		/// <summary>Centurion</summary>
		CES220,
		/// <summary>Valkyrie</summary>
		CSS001,
		/// <summary>Sting X</summary>
		CSM020,
		/// <summary>Gamma Ray</summary>
		CSS010,
		/// <summary>Luciferiel</summary>
		CSM002,
		/// <summary>Aerene</summary>
		CSH003,
		/// <summary>Big Mammoth</summary>
		CB,
		/// <summary>Pinkett</summary>
		CNP010,
		/// <summary>Jaywalker</summary>
		AEJ606,
		/// <summary>Hellfire</summary>
		AEM709,
		/// <summary>Spazmok</summary>
		AES707,
		/// <summary>Ballistika</summary>
		AEM806,
		/// <summary>Mekhi</summary>
		AEM482,
		/// <summary>Frontliner</summary>
		AES308,
		/// <summary>Rushnik</summary>
		AES005,
		/// <summary>Trooper</summary>
		ANM002,
		/// <summary>Boomrocker</summary>
		AEH010,
		/// <summary>Stallion</summary>
		AES008,
		/// <summary>Stark</summary>
		ANM007,
		/// <summary>Haskell</summary>
		AEH009,
		/// <summary>Scootwing</summary>
		AES104,
		/// <summary>Davenstar</summary>
		AEM504,
		/// <summary>Zeeker</summary>
		AEM803,
		/// <summary>Blitzker</summary>
		AEH747,
		/// <summary>Pinkett</summary>
		ANP025,
		/// <summary>Vigilant</summary>
		AEH909,
		/// <summary>Hellbent Butcher</summary>
		AEH808,
		/// <summary>Bastion</summary>
		AEW201,
		/// <summary>Aerobolt</summary>
		AES709,
		/// <summary>Sidewinder</summary>
		AEM809,
		/// <summary>GameMaster</summary>
		AGM001,
		/// <summary>Vindicator</summary>
		AEM819,
		/// <summary>Guardian</summary>
		AEH919,
		/// <summary>Centurion</summary>
		AES719,
		/// <summary>Valkyrie</summary>
		AGS001,
		/// <summary>Sting X</summary>
		ASM002,
		/// <summary>Aerene</summary>
		AGH003,
		/// <summary>Gamma Ray</summary>
		ASS001,
		/// <summary>Luciferiel</summary>
		AGM002,
		/// <summary>Big Mammoth</summary>
		ASH003,
		/// <summary>Jaywalker</summary>
		LTJ606,
		/// <summary>Hellfire</summary>
		LTN709,
		/// <summary>Spazmok</summary>
		LTS707,
		/// <summary>Ballistika</summary>
		LTN806,
		/// <summary>Mekhi</summary>
		LTN411,
		/// <summary>Frontliner</summary>
		LTS222,
		/// <summary>Rushnik</summary>
		LTS003,
		/// <summary>Trooper</summary>
		LTM002,
		/// <summary>Boomrocker</summary>
		LTH009,
		/// <summary>Stallion</summary>
		LTS008,
		/// <summary>Stark</summary>
		LTM007,
		/// <summary>Haskell</summary>
		LTH019,
		/// <summary>Scootwing</summary>
		LTS134,
		/// <summary>Davenstar</summary>
		LTN543,
		/// <summary>Zeeker</summary>
		LTN803,
		/// <summary>Blitzker</summary>
		LTH712,
		/// <summary>Pinkett</summary>
		LTP007,
		/// <summary>Hellbent Butcher</summary>
		LTH840,
		/// <summary>Bastion</summary>
		LTW201,
		/// <summary>Aerobolt</summary>
		LTS719,
		/// <summary>Sidewinder</summary>
		LTH919,
		/// <summary>Vigilant</summary>
		LTM819,
		/// <summary>GameMaster</summary>
		LGM001,
		/// <summary>Vindicator</summary>
		LTM829,
		/// <summary>Guardian</summary>
		LTH929,
		/// <summary>Centurion</summary>
		LTS729,
		/// <summary>Valkyrie</summary>
		LGS001,
		/// <summary>Luciferiel</summary>
		LGM002,
		/// <summary>Aerene</summary>
		LGH003,
		/// <summary>Gamma Ray</summary>
		LSS001,
		/// <summary>Sting X</summary>
		LS,
		/// <summary>Big Mammoth</summary>
		LB,
		/// <summary>SonicBoomer</summary>
		PBS003,
		/// <summary>LaserWing</summary>
		PRC009,
		/// <summary>Booster</summary>
		PBM002,
		/// <summary>Tomahawk-S</summary>
		PRC008,
		/// <summary>MissileLauncher</summary>
		PRC012,
		/// <summary>Dispenser</summary>
		PBS004,
		/// <summary>StormMover</summary>
		PSD005,
		/// <summary>Sentinel</summary>
		PSD004,
		/// <summary>Fever Rain</summary>
		PRC017,
		/// <summary>Cyclone</summary>
		PBS006,
		/// <summary>Legionnaire Wing</summary>
		PGM002,
		/// <summary>Dispenser-S</summary>
		PSH003,
		/// <summary>GameMaster</summary>
		PGM001,
		/// <summary>Drift Tail</summary>
		PBS018,
		/// <summary>Strike Force</summary>
		PSM002,
		/// <summary>Steel Heart</summary>
		PGS001,
		/// <summary>Prowler</summary>
		PBS017,
		/// <summary>Maverick</summary>
		PBS007,
		/// <summary>Flanker</summary>
		PBS016,
		/// <summary>Argus</summary>
		PBS008,
		/// <summary>P-Booster</summary>
		PBS000,
		/// <summary>Scorpion-S</summary>
		PSS001,
		/// <summary>Tonel Boomer</summary>
		PGH003
	}
	#endregion

	public enum Operators
	{

	}

	public enum OperatorStats
	{
		HP,
		EN,
		SP,
		MPU,
		Size,
		Weight,
		ENOutputRate,
		MinEN_Required,
		DashENDrain,
		JumpENDrain,
		MaxHeat,
		CooldownRate,
		ScanRange,
		Marksmanship
	}

	public enum MechSlots
	{
		Booster,
		Core,
		Head,
		Arm,
		Leg
	}

	public enum WeightClass : byte
	{
		/// <summary>
		/// Speed
		/// </summary>
		Light,
		/// <summary>
		/// Normal
		/// </summary>
		Standard,
		Heavy
	}

	public enum Scenes
	{
		Login,
		Lobby,
		Shop,
		Settings,
		Inventory
	}

	#region Shop

	/// <summary>
	/// The contract type for this item
	/// </summary>
	public enum ContractTypes : byte
	{
		/// <summary>
		/// Qty is amount of usable units
		/// </summary>
		FixedQty = 0,
		/// <summary>
		/// Durability is durability item is created with
		/// </summary>
		Durability = 1,
		/// <summary>
		/// Time is duration (in hours) the item has before it expires from inventory 
		/// </summary>
		Time = 2
	}

	public enum ProductTypes : byte
	{
		Part = 0,
		Code = 1,
		Etc = 2,
		UnitSet = 3
	}
	#endregion

	#region Skills
	public enum Skills
	{
		BLADEBLADEA
		, BLADEBLADEA1
		, BLADESHIELDC
		, BLADESHIELDC1
		, BLADESPEARC
		, CANNONA
		, CANNONA1
		, CANNONB
		, CANNONB1
		, CANNONC1
		, ENGBLADEC
		, ENGBLADEC1
		, ENGSHIELDC
		, ENGSHIELDC1
		, ENGSPEARC
		, ENGSPEARC1
		, HOMINGLASERA
		, MISSILEA
		, MOVEMISSILEA
		, RIFLEBLADEC
		, RIFLEBLADEC1
		, RIFLERIFLEC
		, RIFLERIFLEC1
		, RIFLESHIELDC
		, RIFLESHIELDC1
		, RIFLESPEARC
		, RIFLESPEARC1
		, ROCKETA
		, ROCKETA1
		, ROCKETC
		, ROCKETC1
		, SHOTGUNBLADEC
		, SHOTGUNBLADEC1
		, SHOTGUNSHIELDC
		, SHOTGUNSHIELDC1
		, SHOTGUNSHOTGUNC
		, SHOTGUNSHOTGUNC1
		, SHOTGUNSPEARC
		, SHOTGUNSPEARC1
		, SMGBLADEC
		, SMGBLADEC1
		, SMGRIFLEC
		, SMGRIFLEC1
		, SMGSHIELDC
		, SMGSHIELDC1
		, SMGSHOTGUNC
		, SMGSHOTGUNC1
		, SMGSMGA
		, SMGSMGA1
		, SMGSPEARC
		, SMGSPEARC1
		, SPEARSHIELDC
		, SPEARSHIELDC1
		, SPEARSPEARC
		, STOPMISSILEA
	}

	public enum CodeTypes
	{
		SKILL, PASSIVE, TUNNING
	}

	public enum CodeCategorys
	{
		CONTROL,
		OVERCLOCK,
		HACKING,
		/// <summary>
		/// exclusive
		/// </summary>
		COMPUTING
	}

	public enum CodeEquipTypes
	{
		WEAPON,
		BOOSTER
	}

	public enum CodeWeaponTypes
	{

	}

	public enum CodeActivationTargets
	{
		SELF,
		ENEMY,
		/// <summary>
		/// exclusive
		/// </summary>
		ALLY
	}
	#endregion

	public enum GameTypes : byte
	{
		Training = 0,
		Deathmatch = 1,
		TeamDeathmatch = 2,
		/// <summary>
		/// TC?
		/// </summary>
		TeamBattle = 3,
		ClanBattle = 4,
		/// <summary>
		/// Last stand
		/// </summary>
		DefensiveBattle = 5,
		Tutorial = 6,
		/// <summary>
		/// story mode campaign
		/// </summary>
		Mission = 7,
		FreeMode = 8,
		/// <summary>
		/// CTF
		/// </summary>
		FlagCompetition = 9
	}

	public enum NPCTypes : byte
	{
		NORMAL = 1,
		SPY = 2,
		TERRORIST = 3,
		GUARD = 4,
		BERSERKER = 5,
		HP_GUARD = 6,
		SUPER_HP_GUARD = 7,
		GUNNER = 8,
		SUPPORTER = 9,
		TUTORIAL = 10,
		DUDLEY_FIGHTER = 80,
		DUDLEY_BOMBER = 81,
		DUDLEY_ANNIHILATOR = 90
	}

	public enum AbilityGrowthTypes
	{
		PA_UNKNOWN = 0,
		PA_HP = 1,
		PA_MV = 2,
		PA_EN = 3,
		PA_SCAN_MPU = 4,
		PA_SP_OHE = 5,
		PA_AIM_OHR = 6
	}

	/// <summary>
	/// Projectile object that's used when weapon is fired.
	/// </summary>
	public enum IfoTypes
	{
		NONE = 0,
		ROCKET = 1,
		MISSLE = 2,
		SENTINEL_DRIVER = 3,
		SIMPLE = 4
	}

	public enum PacketTypes : byte
	{
		/// <summary>
		/// Called when the user logs in for the first time
		/// </summary>
		CONNECT_CLIENT,
		/// <summary>
		/// Sent when the client re-connects after switching to a room server
		/// </summary>
		/// ToDo: One connection (ping) for lobby, one connection (ping) for game?
		/// It should ping the server once, for info it needs, dispose the rest
		CONNECT_SWITCH,
		/// <summary>
		/// Sent when the user wants to switch to a room server
		/// </summary>
		/// Uses room id
		//SWITCH_SERVER,
		/// <summary>
		/// Logging packet from client
		/// </summary>
		LOG,
		/// <summary>
		/// Packet when user reports their protocol version
		/// </summary>
		VERSION,
		/// <summary>
		/// Called when the client needs to download shop data
		/// </summary>
		//REQ_GOODS_DATA,
		REQ_SHOP_DATA,
		/// <summary>
		/// Sent when the client wishes to select their operator
		/// </summary>
		SELECT_OPERATOR,
		/// <summary>
		/// Client packet to request the server time
		/// </summary>
		//SERVERTIME,
		/// <summary>
		/// Called when the user needs to sync their money
		/// </summary>
		/// appears to only be used in the shop
		SYNC_MONEY,
		/// <summary>
		/// Dummy packet for unknown client packets
		/// </summary>
		UNKNOWN,
		/// <summary>
		/// Sent when the client first connects to do an encryption handshake?
		/// </summary>
		VALIDATE_CLIENT,
		/// <summary>
		/// Called when the user visits their main lobby window and we need their account stats and info
		/// </summary>
		BRIDGE_REQ_AVATAR_INFO,
		/// <summary>
		/// Looks to be a packet request user stats (kills, deaths, wins, etc)
		/// </summary>
		BRIDGE_REQ_BEST_INFO,
		/// <summary>
		/// When the user requests their stats
		/// </summary>
		BRIDGE_REQ_STAT_INFO,
		/// <summary>
		/// Sent when a user aims on a unit
		/// </summary>
		/// Used to give them a "locked" notification
		//AIM_UNIT,
		/// <summary>
		/// Called when the client enters sniper mode
		/// </summary>
		//MODE_SNIPER,
		/// <summary>
		/// Sent frequently by the user to indicate their current position
		/// </summary>
		GAME_MOVE_UNIT,
		/// <summary>
		/// Sent when a user has sucessfully loaded into the game and is ready to play
		/// </summary>
		GAME_READY_GAME,
		/// <summary>
		/// Sent when the client wants to swap weapons
		/// </summary>
		REQ_CHANGE_WEAPONSET,
		/// <summary>
		/// Seems to be sent by client to request palettes for units
		/// </summary>
		/// Maybe for preloading skills?
		/// Or showing in bar?
		/// Assuming palette is skills currently
		GAME_REQ_PALETTE,
		/// <summary>
		/// Called when the user wishes to respawn
		/// </summary>
		REQ_REGAIN,
		/// <summary>
		/// Sent when the user first spawns. Not sure exactly how it works
		/// </summary>
		GAME_SELECT_BASE,
		/// <summary>
		/// Sent when a user begins attacking
		/// </summary>
		START_ATTACK,
		/// <summary>
		/// Sent when the user stops attacking with an automatic weapon
		/// </summary>
		STOP_ATTACK,
		/// <summary>
		/// Sent when the user stops aiming at a unit
		/// </summary>
		UN_AIM_UNIT,
		/// <summary>
		/// Called when a client requests their inventory
		/// </summary>
		INV_REQ_INVENTORY,
		/// <summary>
		/// Sent when the client requests their current operators or... all operators?
		/// </summary>
		INV_REQ_OPERATOR,
		/// <summary>
		/// Called when a user enters a game after server switch
		/// </summary>
		LOBBY_ENTER_GAME,
		/// <summary>
		/// Called when the user refreshes their lobby games list
		/// </summary>
		LOBBY_REQ_SEARCH_GAME,
		/// <summary>
		/// Called when the user enters a room and wants a list of the current users
		/// </summary>
		ROOM_LIST_USER,
		/// <summary>
		/// Sent when a user toggles their ready status
		/// </summary>
		ROOM_READY,
		/// <summary>
		/// Sent when the user wishes to start the game
		/// </summary>
		ROOM_START_GAME,
		/// <summary>
		/// Sent to the client when they are validated?
		/// </summary>
		CLIENT_VALIDATED = 0x01,
		/// <summary>
		/// Response to a users login request
		/// </summary>
		SERVER_CONNECT_RESULT = 0x02,
		/// <summary>
		/// Probably sent as a response to buying an operator or selecting one
		/// </summary>
		SELECT_OPERATOR_INFO= 0xe6,
		/// <summary>
		/// Ping info for the client
		/// </summary>
		SERVERTIME= 0x04,
		/// <summary>
		/// The server sends its version as a response to the client
		/// </summary>
		SERVER_VERSION= 0x00,
		/// <summary>
		/// Sent to the user to allow them to switch servers
		/// </summary>
		SWITCH_SERVER= 0x0e,
		/// <summary>
		/// Sends the users stats and info to the client
		/// </summary>
		BRIDGE_SEND_AVATAR_INFO = 0x97,
		/// <summary>
		/// A packet containing stats for a particular mode of play
		/// </summary>
		BRIDGE_SEND_BEST_INFO = 0x9f,
		/// <summary>
		/// A packet containing stats for survival (deathmatch)
		/// </summary>
		BRIDGE_SEND_CLANBATTLE_INFO = 0x9d,
		/// <summary>
		/// A packet containing stats for team battle (territory control)
		/// </summary>
		BRIDGE_SEND_CTF_INFO = 0x9c,
		/// <summary>
		/// A packet containing stats for survival (deathmatch)
		/// </summary>
		BRIDGE_SEND_DEFENSIVEBATTLE_INFO = 0x9e,
		/// <summary>
		/// A packet containing stats for survival (deathmatch)
		/// </summary>
		BRIDGE_SEND_SURVIVAL_INFO = 0x99,
		/// <summary>
		/// A packet containing stats for team battle (territory control)
		/// </summary>
		BRIDGE_SEND_TEAMBATTLE_INFO = 0x9b,
		/// <summary>
		/// A packet containing stats for team survival (team deathmatch)
		/// </summary>
		BRIDGE_SEND_TEAMSURVIVAL_INFO = 0x9a,
		/// <summary>
		/// A packet containing stats for survival (deathmatch)
		/// </summary>
		BRIDGE_SEND_TRAINING_INFO= 0x98,
		/// <summary>
		/// Player Position and Aim Rotation
		/// </summary>
		AIM_UNIT= 0x65,
		/// <summary>
		/// Testing this - 69 (nice) seems to be single target attacks
		/// </summary>
		ATTACK= 0x67,
		/// <summary>
		/// Testing this - 69 (nice) seems to be single target attacks
		/// </summary>
		ATTACK_A = 0x6f,
		/// <summary>
		/// Sent to confirm projectile hits? 
		/// </summary>
		ATTACK_A_RESULT= 0x72,
		/// <summary>
		/// Sent when a user performs a melee attack
		/// </summary>
		ATTACK_BLADE= 0x6d,
		/// <summary>
		/// Sent as a reponse to users when they select a base
		/// </summary>
		/// User spawn point?
		GAME_BASE_SELECTED = 0x55,
		/// <summary>
		/// Sent when all units are loaded in and the game is ready to begin
		/// </summary>
		GAME_STARTED = 0x4e,
		/// <summary>
		/// Sent when a user enters sniper mode
		/// </summary>
		MODE_SNIPER = 0x7d,
		/// <summary>
		/// Looks to include data about the overheat status for a users weapon
		/// </summary>
		OVERHEAT_STATUS = 0x83,
		/// <summary>
		/// Seems to be sent with a list of "palette"
		/// </summary>
		/// Maybe skills?
		GAME_PALETTE_LIST = 0x4b,
		/// <summary>
		/// Sent when the user sucessfully or unsucessfully respawns i think
		/// </summary>
		GAME_REGAIN_RESULT = 0x57,
		/// <summary>
		/// Called when the game wants to spawn a unit
		/// </summary>
		GAME_SPAWN_UNIT= 0x4d,
		/// <summary>
		/// Looks to update unit status, maybe for HP or death
		/// </summary>
		STATUS_CHANGED= 0x7a,
		/// <summary>
		/// I think this is the aim unit packet, but it has a lot of data so i could be wrong
		/// </summary>
		//UN_AIM_UNIT= 0x66,
		/// <summary>
		/// Sent when someone DIES
		/// </summary>
		UNIT_DESTROYED= 0x62,
		/// <summary>
		/// Sent in game with data about users units
		/// </summary>
		GAME_UNIT_INFO= 0x4c,
		/// <summary>
		/// Sent to indicate that a unit has moved
		/// </summary>
		GAME_UNIT_MOVED= 0x63,
		/// <summary>
		/// Sent when a unit stops? Not sure the purpose
		/// </summary>
		STOP_UNIT= 0x64,
		/// <summary>
		/// Sent to all clients in a room to give a users info
		/// </summary>
		GAME_USER_INFO= 0x49,
		/// <summary>
		/// Sent when a user switches weapon sets
		/// </summary>
		WEAPONSET_CHANGED = 0x84,
		/// <summary>
		/// This looks like a packet for skills in the users inventory
		/// </summary>
		INV_CODE_LIST= 0x1f,
		/// <summary>
		/// Sends the ID of the users default unit
		/// </summary>
		/// Maybe for lobby screen?
		INV_SEND_DEFAULT_UNIT= 0x1e,
		/// <summary>
		/// Sends when all other inventory packets have been transmitted
		/// </summary>
		INV_END= 0x26,
		/// <summary>
		/// Sends the users money and coins
		/// </summary>
		INV_SEND_MONEY= 0x1d,
		/// <summary>
		/// Sends a list of operators... that the user has? or for sale. not sure
		/// </summary>
		INV_OP_ITEM_LIST= 0x24,
		/// <summary>
		/// Sends the Palette?? for a unit
		/// </summary>
		/// Maybe equiped skills?
		INV_SEND_PALETTE= 0x23,
		/// <summary>
		/// Sends all the parts (and other things?) in the users inventory
		/// </summary>
		INV_SEND_PARTS = 0x20,
		/// <summary>
		/// Sends special items
		/// </summary>
		/// Need to investigate further
		INV_SPECIAL_ITEM_LIST= 0x25,
		/// <summary>
		/// Sends a single units info packet
		/// </summary>
		INV_SEND_UNIT_INFO = 0x22,
		/// <summary>
		/// Send the number of unit slots this user has
		/// </summary>
		INV_SEND_UNIT_SLOTS = 0x21,
		/// <summary>
		/// Sent when the user enters a game
		/// </summary>
		LOBBY_GAME_ENTERED = 0x33,
		/// <summary>
		/// Response to a game search request
		/// </summary>
		LOBBY_GAME_SEARCHED = 0x2f,
		/// <summary>
		/// Sent when the user trys to start the game
		/// </summary>
		ROOM_GAME_START = 0x3d,
		/// <summary>
		/// Contains unit info for units in a room
		/// </summary>
		ROOM_UNIT_INFO = 0x3e,
		/// <summary>
		/// Sent to players in a room when a user joins
		/// </summary>
		ROOM_USER_ENTER = 0x0a,
		/// <summary>
		/// Contains the public user info for users in a room
		/// </summary>
		ROOM_USER_INFO = 0x35,
		/// <summary>
		/// Represents a single "chunk" or group of goods items
		/// </summary>
		/// This is pretty much all unknown
		SEND_GOODS_DATA = 0x91,
		/// <summary>
		/// Empty packet used to determine end of data stream
		/// </summary>
		SEND_GOODS_DATA_END = 0x92,
		/// <summary>
		/// Sends the start indicator for the goods data download, along with the number of goods
		/// </summary>
		SEND_GOODS_DATA_START = 0x90,
		/// <summary>
		/// Sends the users money and coins
		/// </summary>
		MONEY_SYNCED = 0x10
	}
}