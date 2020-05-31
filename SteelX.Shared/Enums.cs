namespace SteelX.Shared
{
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

	public enum WeightClass
	{
		Light,
		Standard,
		Heavy
	}

	public enum Skills
	{

	}

	public enum Scenes
	{
		Login,
		Lobby,
		Shop,
		Settings,
		Inventory
	}
}