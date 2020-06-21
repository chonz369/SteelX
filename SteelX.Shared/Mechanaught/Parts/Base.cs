namespace SteelX.Shared
{
	#region Nested Classes
	//ToDo: Rank Required is missing from Parts database 
	public class Arms : Part
	{
		#region Variables
		/// <summary>
		/// 
		/// </summary>
		public int MaxHeat { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int CooldownRate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Marksmanship { get; set; }
		#endregion

		#region Constructor
		public Arms(Parts arm)
		{
			Arms Arm = GetPart(arm);
			PartName = Arm.PartName;
			DisplayName = Arm.DisplayName;
			RankRequired = Arm.RankRequired;
			HP = Arm.HP;
			Size = Arm.Size;
			Weight = Arm.Weight;
			EnergyDrain = Arm.EnergyDrain;
			MaxHeat = Arm.MaxHeat;
			CooldownRate = Arm.CooldownRate;
			Marksmanship = Arm.Marksmanship;
			WeightSeries = Arm.WeightSeries;
		}
		public Arms(Parts? name = null, string display = null, byte rank = 0, int hp = 0, int size = 0, int weight = 0, int endrain = 0, int heat = 0, int cooldown = 0, int mark = 0, WeightClass? type = null)
		{
			PartName = name.HasValue ? name.Value : Parts.AGM001;
			DisplayName = display ?? "GameMaster";
			RankRequired = rank;
			HP = hp;
			Size = size;
			Weight = weight;
			EnergyDrain = endrain;
			MaxHeat = heat;
			CooldownRate = cooldown;
			Marksmanship = mark;
			WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion

		#region Methods
		internal Arms GetPart(Parts ID)
		{
			foreach (Arms part in Database)
			{
				if (part.PartName == ID) return part;
			}
			throw new System.Exception("Part ID doesnt exist in the database. Please check Arms constructor.");
		}
		#endregion

		#region Database
		private static readonly Arms[] Database;
		static Arms()
		{
			Database = new Arms[] {
//(\w+)\s+(\w+[\s]?\w+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\w+)
//Name    DisplayName HP  EN  SP  MPU Size    Weight  BasicSpeed  EN_Output   MinENRequired   ENDrain Scan    MaxHeat CoolDown    Mark    Capacity    Deceleration    DashOutput  DashENDrain JumpENDrain WeightType
//new Arms(\n\tname: Parts.$1,\n\tdisplay: "$2",\n\thp: $3,\n\tsize: $7,\n\tweight: $8,\n\tendrain: $12,\n\theat: $14,\n\tcooldown: $15,\n\tmark: $16,\n\ttype: WeightClass.$22),
new Arms(
	name: Parts.AGM001,
	display: "GameMaster",
	hp: 0,
	size: 0,
	weight: 0,
	endrain: 0,
	heat: 0,
	cooldown: 0,
	mark: 0,
	type: WeightClass.Standard),
new Arms(
	name: Parts.AES008,
	display: "Stallion",
	hp: 359,
	size: 2486,
	weight: 13290,
	endrain: 70,
	heat: 109,
	cooldown: 40,
	mark: 5,
	type: WeightClass.Light),
new Arms(
	name: Parts.AEM482,
	display: "Mekhi",
	hp: 450,
	size: 2486,
	weight: 13298,
	endrain: 77,
	heat: 102,
	cooldown: 39,
	mark: 14,
	type: WeightClass.Light),
new Arms(
	name: Parts.ANP025,
	display: "Pinkett",
	hp: 450,
	size: 3910,
	weight: 15300,
	endrain: 68,
	heat: 102,
	cooldown: 22,
	mark: 5,
	type: WeightClass.Standard),
new Arms(
	name: Parts.AES709,
	display: "Aerobolt",
	hp: 450,
	size: 2410,
	weight: 12210,
	endrain: 95,
	heat: 105,
	cooldown: 47,
	mark: 28,
	type: WeightClass.Light),
new Arms(
	name: Parts.AES005,
	display: "Rushnik",
	hp: 466,
	size: 2702,
	weight: 13295,
	endrain: 83,
	heat: 110,
	cooldown: 28,
	mark: 19,
	type: WeightClass.Light),
new Arms(
	name: Parts.AES308,
	display: "Frontliner",
	hp: 481,
	size: 3430,
	weight: 15296,
	endrain: 102,
	heat: 155,
	cooldown: 26,
	mark: 7,
	type: WeightClass.Standard),
new Arms(
	name: Parts.ANM002,
	display: "Trooper",
	hp: 499,
	size: 3910,
	weight: 15297,
	endrain: 96,
	heat: 123,
	cooldown: 29,
	mark: 24,
	type: WeightClass.Standard),
new Arms(
	name: Parts.AGS001,
	display: "Valkyrie",
	hp: 500,
	size: 2300,
	weight: 12000,
	endrain: 100,
	heat: 120,
	cooldown: 48,
	mark: 25,
	type: WeightClass.Light),
new Arms(
	name: Parts.ASS001,
	display: "Gamma Ray",
	hp: 500,
	size: 2800,
	weight: 12600,
	endrain: 86,
	heat: 130,
	cooldown: 40,
	mark: 30,
	type: WeightClass.Light),
new Arms(
	name: Parts.AES707,
	display: "Spazmok",
	hp: 517,
	size: 2654,
	weight: 13282,
	endrain: 134,
	heat: 137,
	cooldown: 51,
	mark: 19,
	type: WeightClass.Light),
new Arms(
	name: Parts.AES104,
	display: "Scootwing",
	hp: 517,
	size: 2726,
	weight: 13288,
	endrain: 141,
	heat: 172,
	cooldown: 25,
	mark: 24,
	type: WeightClass.Light),
new Arms(
	name: Parts.AEM803,
	display: "Zeeker",
	hp: 517,
	size: 2885,
	weight: 14103,
	endrain: 115,
	heat: 148,
	cooldown: 32,
	mark: 31,
	type: WeightClass.Light),
new Arms(
	name: Parts.AEH010,
	display: "Boomrocker",
	hp: 532,
	size: 4902,
	weight: 18295,
	endrain: 83,
	heat: 127,
	cooldown: 30,
	mark: 12,
	type: WeightClass.Heavy),
new Arms(
	name: Parts.ANM007,
	display: "Stark",
	hp: 540,
	size: 4222,
	weight: 15291,
	endrain: 96,
	heat: 162,
	cooldown: 25,
	mark: 12,
	type: WeightClass.Standard),
new Arms(
	name: Parts.AES719,
	display: "Centurion",
	hp: 550,
	size: 3115,
	weight: 13120,
	endrain: 110,
	heat: 137,
	cooldown: 42,
	mark: 29,
	type: WeightClass.Light),
new Arms(
	name: Parts.AEM709,
	display: "Hellfire",
	hp: 553,
	size: 3350,
	weight: 13287,
	endrain: 109,
	heat: 155,
	cooldown: 36,
	mark: 14,
	type: WeightClass.Light),
new Arms(
	name: Parts.AEM819,
	display: "Vindicator",
	hp: 570,
	size: 4355,
	weight: 17056,
	endrain: 125,
	heat: 185,
	cooldown: 34,
	mark: 35,
	type: WeightClass.Standard),
new Arms(
	name: Parts.AEW201,
	display: "Bastion",
	hp: 572,
	size: 4534,
	weight: 15285,
	endrain: 96,
	heat: 141,
	cooldown: 30,
	mark: 34,
	type: WeightClass.Standard),
new Arms(
	name: Parts.AEJ606,
	display: "Jaywalker",
	hp: 588,
	size: 4734,
	weight: 14550,
	endrain: 141,
	heat: 176,
	cooldown: 30,
	mark: 19,
	type: WeightClass.Light),
new Arms(
	name: Parts.AEH747,
	display: "Blitzker",
	hp: 590,
	size: 4926,
	weight: 18283,
	endrain: 90,
	heat: 158,
	cooldown: 33,
	mark: 43,
	type: WeightClass.Heavy),
new Arms(
	name: Parts.AEH919,
	display: "Guardian",
	hp: 590,
	size: 5229,
	weight: 19144,
	endrain: 145,
	heat: 189,
	cooldown: 30,
	mark: 42,
	type: WeightClass.Heavy),
new Arms(
	name: Parts.AGM002,
	display: "Luciferiel",
	hp: 600,
	size: 3510,
	weight: 15000,
	endrain: 130,
	heat: 175,
	cooldown: 38,
	mark: 32,
	type: WeightClass.Standard),
new Arms(
	name: Parts.AEM806,
	display: "Ballistika",
	hp: 617,
	size: 3934,
	weight: 15283,
	endrain: 154,
	heat: 197,
	cooldown: 33,
	mark: 31,
	type: WeightClass.Standard),
new Arms(
	name: Parts.AEM504,
	display: "Davenstar",
	hp: 621,
	size: 4630,
	weight: 15287,
	endrain: 160,
	heat: 134,
	cooldown: 52,
	mark: 10,
	type: WeightClass.Standard),
new Arms(
	name: Parts.AEM809,
	display: "Sidewinder",
	hp: 630,
	size: 3815,
	weight: 15100,
	endrain: 105,
	heat: 150,
	cooldown: 38,
	mark: 33,
	type: WeightClass.Standard),
new Arms(
	name: Parts.AEH009,
	display: "Haskell",
	hp: 640,
	size: 4854,
	weight: 18293,
	endrain: 102,
	heat: 137,
	cooldown: 32,
	mark: 22,
	type: WeightClass.Heavy),
new Arms(
	name: Parts.AEH808,
	display: "Hellbent Butcher",
	hp: 659,
	size: 5878,
	weight: 20280,
	endrain: 122,
	heat: 144,
	cooldown: 29,
	mark: 55,
	type: WeightClass.Heavy),
new Arms(
	name: Parts.ASM002,
	display: "Sting X",
	hp: 680,
	size: 4000,
	weight: 15310,
	endrain: 130,
	heat: 185,
	cooldown: 35,
	mark: 33,
	type: WeightClass.Standard),
new Arms(
	name: Parts.ASH003,
	display: "Big Mammoth",
	hp: 710,
	size: 6000,
	weight: 20000,
	endrain: 124,
	heat: 175,
	cooldown: 28,
	mark: 46,
	type: WeightClass.Heavy),
new Arms(
	name: Parts.AEH909,
	display: "Vigilant",
	hp: 750,
	size: 6012,
	weight: 22570,
	endrain: 129,
	heat: 172,
	cooldown: 31,
	mark: 50,
	type: WeightClass.Heavy),
new Arms(
	name: Parts.AGH003,
	display: "Aerene",
	hp: 780,
	size: 5560,
	weight: 20000,
	endrain: 120,
	heat: 183,
	cooldown: 32,
	mark: 30,
	type: WeightClass.Heavy)
				};
		}
		#endregion
	}
	public class Legs : Part
	{
		#region Variables
		/// <summary>
		/// 
		/// </summary>
		public int BasicSpeed { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Capacity { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Deceleration { get; set; }
		#endregion

		#region Constructor
		public Legs(Parts leg)
		{
			Legs Leg = GetPart(leg);
			PartName = Leg.PartName;
			DisplayName = Leg.DisplayName;
			RankRequired = Leg.RankRequired;
			HP = Leg.HP;
			Size = Leg.Size;
			Weight = Leg.Weight;
			BasicSpeed = Leg.BasicSpeed;
			EnergyDrain = Leg.EnergyDrain;
			Capacity = Leg.Capacity;
			Deceleration = Leg.Deceleration;
			WeightSeries = Leg.WeightSeries;
		}
		public Legs(Parts? name = null, string display = null, byte rank = 0, int hp = 0, int size = 0, int weight = 0, int speed = 0, int endrain = 0, int capacity = 0, int deceleration = 0, WeightClass? type = null)
		{
			PartName = name.HasValue ? name.Value : Parts.AGM001;
			DisplayName = display ?? "GameMaster";
			RankRequired = rank;
			HP = hp;
			Size = size;
			Weight = weight;
			BasicSpeed = speed;
			EnergyDrain = endrain;
			Capacity = capacity;
			Deceleration = deceleration;
			WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion

		#region Methods
		internal Legs GetPart(Parts ID)
		{
			foreach (Legs part in Database)
			{
				if (part.PartName == ID) return part;
			}
			throw new System.Exception("Part ID doesnt exist in the database. Please check Legs constructor.");
		}
		#endregion

		#region Database
		private static readonly Legs[] Database;
		static Legs()
		{
			Database = new Legs[] { 
//(\w+)\s+(\w+[\s]?\w+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\w+)
//Name    DisplayName HP  EN  SP  MPU Size    Weight  BasicSpeed  EN_Output   MinENRequired   ENDrain Scan    MaxHeat CoolDown    Mark    Capacity    Deceleration    DashOutput  DashENDrain JumpENDrain WeightType
//new Legs(\n\tname: Parts.$1,\n\tdisplay: "$2",\n\thp: $3,\n\tsize: $7,\n\tweight: $8,\n\tspeed: $9,\n\tendrain: $12,\n\tcapacity: $17,\n\tdeceleration: $18,\n\ttype: WeightClass.$22),
new Legs(
	name: Parts.LGM001,
	display: "GameMaster",
	hp: 0,
	size: 0,
	weight: 0,
	speed: 0,
	endrain: 0,
	capacity: 0,
	deceleration: 0,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LTJ606,
	display: "Jaywalker",
	hp: 701,
	size: 6120,
	weight: 22975,
	speed: 610,
	endrain: 43,
	capacity: 60000,
	deceleration: 92800,
	type: WeightClass.Light),
new Legs(
	name: Parts.LTN709,
	display: "Hellfire",
	hp: 738,
	size: 4300,
	weight: 19981,
	speed: 608,
	endrain: 35,
	capacity: 60000,
	deceleration: 60800,
	type: WeightClass.Light),
new Legs(
	name: Parts.LTS707,
	display: "Spazmok",
	hp: 645,
	size: 4540,
	weight: 19970,
	speed: 613,
	endrain: 15,
	capacity: 60000,
	deceleration: 92800,
	type: WeightClass.Light),
new Legs(
	name: Parts.LTN806,
	display: "Ballistika",
	hp: 763,
	size: 5780,
	weight: 21469,
	speed: 464,
	endrain: 33,
	capacity: 195000,
	deceleration: 112000,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LTN411,
	display: "Mekhi",
	hp: 600,
	size: 4420,
	weight: 19994,
	speed: 601,
	endrain: 45,
	capacity: 60000,
	deceleration: 48000,
	type: WeightClass.Light),
new Legs(
	name: Parts.LTS222,
	display: "Frontliner",
	hp: 650,
	size: 5660,
	weight: 21493,
	speed: 453,
	endrain: 50,
	capacity: 195000,
	deceleration: 67200,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LTS003,
	display: "Rushnik",
	hp: 665,
	size: 4420,
	weight: 19992,
	speed: 600,
	endrain: 43,
	capacity: 60000,
	deceleration: 52857,
	type: WeightClass.Light),
new Legs(
	name: Parts.LTM002,
	display: "Trooper",
	hp: 650,
	size: 6140,
	weight: 21496,
	speed: 457,
	endrain: 50,
	capacity: 195000,
	deceleration: 64000,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LTH009,
	display: "Boomrocker",
	hp: 818,
	size: 7020,
	weight: 22995,
	speed: 415,
	endrain: 146,
	capacity: 390000,
	deceleration: 70400,
	type: WeightClass.Heavy),
new Legs(
	name: Parts.LTS008,
	display: "Stallion",
	hp: 565,
	size: 4420,
	weight: 19985,
	speed: 603,
	endrain: 47,
	capacity: 60000,
	deceleration: 54400,
	type: WeightClass.Light),
new Legs(
	name: Parts.LTM007,
	display: "Stark",
	hp: 685,
	size: 5780,
	weight: 21487,
	speed: 460,
	endrain: 30,
	capacity: 195000,
	deceleration: 64000,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LTH019,
	display: "Haskell",
	hp: 765,
	size: 7020,
	weight: 22988,
	speed: 417,
	endrain: 39,
	capacity: 390000,
	deceleration: 67200,
	type: WeightClass.Heavy),
new Legs(
	name: Parts.LTS134,
	display: "Scootwing",
	hp: 677,
	size: 4540,
	weight: 19976,
	speed: 609,
	endrain: 25,
	capacity: 60000,
	deceleration: 73600,
	type: WeightClass.Light),
new Legs(
	name: Parts.LTN543,
	display: "Davenstar",
	hp: 748,
	size: 5660,
	weight: 21481,
	speed: 462,
	endrain: 50,
	capacity: 195000,
	deceleration: 64000,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LTN803,
	display: "Zeeker",
	hp: 664,
	size: 4180,
	weight: 21553,
	speed: 611,
	endrain: 20,
	capacity: 60000,
	deceleration: 80000,
	type: WeightClass.Light),
new Legs(
	name: Parts.LTH712,
	display: "Blitzker",
	hp: 811,
	size: 6900,
	weight: 22972,
	speed: 426,
	endrain: 50,
	capacity: 390000,
	deceleration: 76800,
	type: WeightClass.Heavy),
new Legs(
	name: Parts.LTP007,
	display: "Pinkett",
	hp: 620,
	size: 5900,
	weight: 20613,
	speed: 465,
	endrain: 35,
	capacity: 195000,
	deceleration: 25600,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LTH840,
	display: "Hellbent Butcher",
	hp: 878,
	size: 8900,
	weight: 25018,
	speed: 425,
	endrain: 50,
	capacity: 390000,
	deceleration: 89600,
	type: WeightClass.Heavy),
new Legs(
	name: Parts.LTW201,
	display: "Bastion",
	hp: 774,
	size: 5660,
	weight: 21476,
	speed: 461,
	endrain: 28,
	capacity: 195000,
	deceleration: 80000,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LTS719,
	display: "Aerobolt",
	hp: 550,
	size: 4210,
	weight: 18665,
	speed: 620,
	endrain: 23,
	capacity: 62000,
	deceleration: 93300,
	type: WeightClass.Light),
new Legs(
	name: Parts.LTH919,
	display: "Sidewinder",
	hp: 800,
	size: 5810,
	weight: 21230,
	speed: 480,
	endrain: 42,
	capacity: 195000,
	deceleration: 115000,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LTM819,
	display: "Vigilant",
	hp: 900,
	size: 9000,
	weight: 26208,
	speed: 410,
	endrain: 75,
	capacity: 390000,
	deceleration: 90000,
	type: WeightClass.Heavy),
new Legs(
	name: Parts.LTM829,
	display: "Vindicator",
	hp: 820,
	size: 5900,
	weight: 22699,
	speed: 469,
	endrain: 30,
	capacity: 215000,
	deceleration: 110000,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LTH929,
	display: "Guardian",
	hp: 865,
	size: 7380,
	weight: 24550,
	speed: 440,
	endrain: 51,
	capacity: 350000,
	deceleration: 90500,
	type: WeightClass.Heavy),
new Legs(
	name: Parts.LTS729,
	display: "Centurion",
	hp: 650,
	size: 4820,
	weight: 19392,
	speed: 590,
	endrain: 28,
	capacity: 73000,
	deceleration: 94500,
	type: WeightClass.Light),
new Legs(
	name: Parts.LGS001,
	display: "Valkyrie",
	hp: 600,
	size: 4380,
	weight: 19000,
	speed: 600,
	endrain: 25,
	capacity: 70000,
	deceleration: 95000,
	type: WeightClass.Light),
new Legs(
	name: Parts.LGM002,
	display: "Luciferiel",
	hp: 830,
	size: 5500,
	weight: 21546,
	speed: 470,
	endrain: 45,
	capacity: 195000,
	deceleration: 10000,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LGH003,
	display: "Aerene",
	hp: 880,
	size: 8500,
	weight: 24511,
	speed: 425,
	endrain: 75,
	capacity: 380000,
	deceleration: 90000,
	type: WeightClass.Heavy),
new Legs(
	name: Parts.LSS001,
	display: "Gamma Ray",
	hp: 620,
	size: 4350,
	weight: 19000,
	speed: 600,
	endrain: 20,
	capacity: 70000,
	deceleration: 93300,
	type: WeightClass.Light),
new Legs(
	name: Parts.LS,
	display: "Sting X",
	hp: 835,
	size: 6000,
	weight: 22000,
	speed: 473,
	endrain: 38,
	capacity: 200000,
	deceleration: 114000,
	type: WeightClass.Standard),
new Legs(
	name: Parts.LB,
	display: "Big Mammoth",
	hp: 880,
	size: 8000,
	weight: 25650,
	speed: 420,
	endrain: 50,
	capacity: 400000,
	deceleration: 90000,
	type: WeightClass.Heavy)
				};
		}
		#endregion
	}
	public class Cores : Part
	{
		#region Variables
		/// <summary>
		/// EN sets the minimum EN of the Mechanaught
		/// </summary>
		public int EN { get; set; }
		/// <summary>
		/// Minimum EN Required
		/// </summary>
		public int MinEN { get; set; }
		/// <summary>
		/// EN Output Rate sets the rate of EN regeneration of a mechanaught
		/// </summary>
		public int OutputRate { get; set; }
		#endregion

		#region Constructor
		public Cores(Parts core)
		{
			Cores Core = GetPart(core);
			PartName = Core.PartName;
			DisplayName = Core.DisplayName;
			RankRequired = Core.RankRequired;
			HP = Core.HP;
			Size = Core.Size;
			Weight = Core.Weight;
			OutputRate = Core.OutputRate;
			MinEN = Core.MinEN;
			EnergyDrain = Core.EnergyDrain;
			WeightSeries = Core.WeightSeries;
		}
		public Cores(Parts? name = null, string display = null, byte rank = 0, int hp = 0, int en = 0, int size = 0, int weight = 0, int enoutput = 0, int minimumen = 0, int endrain = 0, WeightClass? type = null)
		{
			PartName = name.HasValue ? name.Value : Parts.AGM001;
			DisplayName = display ?? "GameMaster";
			RankRequired = rank;
			HP = hp;
			Size = size;
			Weight = weight;
			OutputRate = enoutput;
			MinEN = minimumen;
			EnergyDrain = endrain;
			WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion

		#region Methods
		internal Cores GetPart(Parts ID)
		{
			foreach (Cores part in Database)
			{
				if (part.PartName == ID) return part;
			}
			throw new System.Exception("Part ID doesnt exist in the database. Please check Cores constructor.");
		}
		#endregion

		#region Database
		private static readonly Cores[] Database;
		static Cores()
		{
			Database = new Cores[] { 
//(\w+)\s+(\w+[\s]?\w+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\w+)
//Name    DisplayName HP  EN  SP  MPU Size    Weight  BasicSpeed  EN_Output   MinENRequired   ENDrain Scan    MaxHeat CoolDown    Mark    Capacity    Deceleration    DashOutput  DashENDrain JumpENDrain WeightType
//new Cores(\n\tname: Parts.$1,\n\tdisplay: "$2",\n\thp: $3,\n\ten: $4,\n\tsize: $7,\n\tweight: $8,\n\tenoutput: $10,\n\tminimumen: $11,\n\tendrain: $12,\n\ttype: WeightClass.$22),
new Cores(
	name: Parts.CGM001,
	display: "GameMaster",
	hp: 0,
	en: 0,
	size: 0,
	weight: 0,
	enoutput: 0,
	minimumen: 0,
	endrain: 0,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CEJ606,
	display: "Jaywalker",
	hp: 965,
	en: 3072,
	size: 4855,
	weight: 21860,
	enoutput: 768,
	minimumen: 388,
	endrain: 124,
	type: WeightClass.Light),
new Cores(
	name: Parts.CEM709,
	display: "Hellfire",
	hp: 1258,
	en: 2355,
	size: 4710,
	weight: 22146,
	enoutput: 790,
	minimumen: 806,
	endrain: 160,
	type: WeightClass.Light),
new Cores(
	name: Parts.CES707,
	display: "Spazmok",
	hp: 960,
	en: 2183,
	size: 5190,
	weight: 22136,
	enoutput: 1022,
	minimumen: 851,
	endrain: 90,
	type: WeightClass.Light),
new Cores(
	name: Parts.CEM806,
	display: "Ballistika",
	hp: 1018,
	en: 3026,
	size: 7230,
	weight: 25468,
	enoutput: 886,
	minimumen: 717,
	endrain: 174,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CEM412,
	display: "Mekhi",
	hp: 800,
	en: 2150,
	size: 5310,
	weight: 22160,
	enoutput: 669,
	minimumen: 314,
	endrain: 98,
	type: WeightClass.Light),
new Cores(
	name: Parts.CES301,
	display: "Frontliner",
	hp: 854,
	en: 2147,
	size: 6630,
	weight: 25493,
	enoutput: 650,
	minimumen: 269,
	endrain: 92,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CES003,
	display: "Rushnik",
	hp: 832,
	en: 2286,
	size: 5430,
	weight: 22162,
	enoutput: 787,
	minimumen: 448,
	endrain: 106,
	type: WeightClass.Light),
new Cores(
	name: Parts.CNM002,
	display: "Trooper",
	hp: 887,
	en: 2080,
	size: 6510,
	weight: 25492,
	enoutput: 708,
	minimumen: 403,
	endrain: 100,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CEH007,
	display: "Boomrocker",
	hp: 942,
	en: 2261,
	size: 8070,
	weight: 30497,
	enoutput: 663,
	minimumen: 358,
	endrain: 102,
	type: WeightClass.Heavy),
new Cores(
	name: Parts.CES008,
	display: "Stallion",
	hp: 732,
	en: 2216,
	size: 5070,
	weight: 22151,
	enoutput: 878,
	minimumen: 538,
	endrain: 170,
	type: WeightClass.Light),
new Cores(
	name: Parts.CNM007,
	display: "Stark",
	hp: 806,
	en: 2366,
	size: 6870,
	weight: 25486,
	enoutput: 838,
	minimumen: 762,
	endrain: 168,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CEH009,
	display: "Haskell",
	hp: 969,
	en: 2419,
	size: 5190,
	weight: 30488,
	enoutput: 770,
	minimumen: 582,
	endrain: 174,
	type: WeightClass.Heavy),
new Cores(
	name: Parts.CES103,
	display: "Scootwing",
	hp: 920,
	en: 2508,
	size: 5190,
	weight: 22142,
	enoutput: 785,
	minimumen: 762,
	endrain: 160,
	type: WeightClass.Light),
new Cores(
	name: Parts.CEM571,
	display: "Davenstar",
	hp: 1258,
	en: 2594,
	size: 6630,
	weight: 25481,
	enoutput: 790,
	minimumen: 896,
	endrain: 160,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CEM803,
	display: "Zeeker",
	hp: 980,
	en: 2764,
	size: 5350,
	weight: 23851,
	enoutput: 916,
	minimumen: 672,
	endrain: 181,
	type: WeightClass.Light),
new Cores(
	name: Parts.CEH737,
	display: "Blitzker",
	hp: 1124,
	en: 2778,
	size: 7950,
	weight: 30470,
	enoutput: 904,
	minimumen: 403,
	endrain: 165,
	type: WeightClass.Heavy),
new Cores(
	name: Parts.CEH807,
	display: "Hellbent Butcher",
	hp: 1335,
	en: 3126,
	size: 8790,
	weight: 35468,
	enoutput: 856,
	minimumen: 941,
	endrain: 161,
	type: WeightClass.Heavy),
new Cores(
	name: Parts.CEW201,
	display: "Bastion",
	hp: 977,
	en: 2506,
	size: 6390,
	weight: 25477,
	enoutput: 896,
	minimumen: 582,
	endrain: 173,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CES210,
	display: "Aerobolt",
	hp: 780,
	en: 2800,
	size: 4980,
	weight: 21260,
	enoutput: 941,
	minimumen: 650,
	endrain: 112,
	type: WeightClass.Light),
new Cores(
	name: Parts.CEH410,
	display: "Vigitant",
	hp: 1550,
	en: 3250,
	size: 9200,
	weight: 38625,
	enoutput: 862,
	minimumen: 900,
	endrain: 168,
	type: WeightClass.Heavy),
new Cores(
	name: Parts.CEM310,
	display: "Sidewinder",
	hp: 1150,
	en: 3120,
	size: 7450,
	weight: 25890,
	enoutput: 910,
	minimumen: 695,
	endrain: 150,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CEM320,
	display: "Vindicator",
	hp: 1300,
	en: 3000,
	size: 7850,
	weight: 27480,
	enoutput: 883,
	minimumen: 750,
	endrain: 140,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CEH420,
	display: "Guardian",
	hp: 1350,
	en: 3100,
	size: 8358,
	weight: 33520,
	enoutput: 870,
	minimumen: 790,
	endrain: 161,
	type: WeightClass.Heavy),
new Cores(
	name: Parts.CES220,
	display: "Centurion",
	hp: 920,
	en: 2900,
	size: 6015,
	weight: 22000,
	enoutput: 900,
	minimumen: 680,
	endrain: 112,
	type: WeightClass.Light),
new Cores(
	name: Parts.CSS001,
	display: "Valkyrie",
	hp: 810,
	en: 2700,
	size: 4825,
	weight: 20500,
	enoutput: 935,
	minimumen: 600,
	endrain: 93,
	type: WeightClass.Light),
new Cores(
	name: Parts.CSM020,
	display: "Sting X",
	hp: 1180,
	en: 3000,
	size: 7200,
	weight: 25200,
	enoutput: 890,
	minimumen: 750,
	endrain: 145,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CSS010,
	display: "Gamma Ray",
	hp: 850,
	en: 2800,
	size: 5858,
	weight: 20000,
	enoutput: 920,
	minimumen: 650,
	endrain: 90,
	type: WeightClass.Light),
new Cores(
	name: Parts.CSM002,
	display: "Luciferiel",
	hp: 1110,
	en: 2850,
	size: 6310,
	weight: 23650,
	enoutput: 895,
	minimumen: 680,
	endrain: 108,
	type: WeightClass.Standard),
new Cores(
	name: Parts.CSH003,
	display: "Aerene",
	hp: 1400,
	en: 3000,
	size: 8208,
	weight: 31450,
	enoutput: 875,
	minimumen: 800,
	endrain: 128,
	type: WeightClass.Heavy),
new Cores(
	name: Parts.CB,
	display: "Big Mammoth",
	hp: 1450,
	en: 3150,
	size: 8500,
	weight: 35000,
	enoutput: 808,
	minimumen: 850,
	endrain: 155,
	type: WeightClass.Heavy),
new Cores(
	name: Parts.CNP010,
	display: "Pinkett",
	hp: 860,
	en: 2000,
	size: 6990,
	weight: 25500,
	enoutput: 630,
	minimumen: 430,
	endrain: 90,
	type: WeightClass.Standard)
				};
		}
		#endregion
	}
	public class Heads : Part
	{
		#region Variables
		/// <summary>
		/// SP sets the minimum Skill Points of the Mechanaught
		/// </summary>
		public int SP { get; set; }
		/// <summary>
		///MPU sets how many <see cref="Skills"/> can be equipped on the mechanaught
		/// </summary>
		public int MPU { get; set; }
		/// <summary>
		/// Scan Range sets the minimum scan range of the mechanaught.
		/// </summary>
		public int ScanRange { get; set; }
		#endregion

		#region Constructor
		public Heads(Parts head)
		{
			Heads Head = GetPart(head);
			PartName = Head.PartName;
			DisplayName = Head.DisplayName;
			RankRequired = Head.RankRequired;
			HP = Head.HP;
			SP = Head.SP;
			MPU = Head.MPU;
			Size = Head.Size;
			Weight = Head.Weight;
			EnergyDrain = Head.EnergyDrain;
			ScanRange = Head.ScanRange;
			WeightSeries = Head.WeightSeries;
		}
		public Heads(Parts? name = null, string display = null, byte rank = 0, int hp = 0, int sp = 0, int mpu = 0, int size = 0, int weight = 0, int endrain = 0, int scan = 0, WeightClass? type = null)
		{
			PartName = name.HasValue ? name.Value : Parts.AGM001;
			DisplayName = display ?? "GameMaster";
			RankRequired = rank;
			HP = hp;
			SP = sp;
			MPU = mpu;
			Size = size;
			Weight = weight;
			EnergyDrain = endrain;
			ScanRange = scan;
			WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion

		#region Methods
		internal Heads GetPart(Parts ID)
		{
			foreach (Heads part in Database)
			{
				if (part.PartName == ID) return part;
			}
			throw new System.Exception("Part ID doesnt exist in the database. Please check Heads constructor.");
		}
		#endregion

		#region Database
		private static readonly Heads[] Database;
		static Heads()
		{
			Database = new Heads[] { 
//(\w+)\s+(\w+[\s]?\w+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\w+)
//Name    DisplayName HP  EN  SP  MPU Size    Weight  BasicSpeed  EN_Output   MinENRequired   ENDrain Scan    MaxHeat CoolDown    Mark    Capacity    Deceleration    DashOutput  DashENDrain JumpENDrain WeightType
//new Heads(\n\tname: "$1",\n\tdisplay: "$2",\n\thp: $3,\n\tsp: $5,\n\tmpu: $6,\n\tsize: $7,\n\tweight: $8,\n\tendrain: $12,\n\tscan: $13,\n\ttype: WeightClass.$22),
new Heads(
	name: Parts.HGM001,
	display: "GameMaster",
	hp: 0,
	sp: 0,
	mpu: 0,
	size: 0,
	weight: 0,
	endrain: 0,
	scan: 0,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HDM402,
	display: "Mekhi",
	hp: 160,
	sp: 960,
	mpu: 2,
	size: 568,
	weight: 4430,
	endrain: 45,
	scan: 1360,
	type: WeightClass.Light),
new Heads(
	name: Parts.HDP010,
	display: "Pinkett",
	hp: 160,
	sp: 1050,
	mpu: 1,
	size: 296,
	weight: 4580,
	endrain: 51,
	scan: 1450,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HDS008,
	display: "Stallion",
	hp: 160,
	sp: 1400,
	mpu: 2,
	size: 232,
	weight: 4423,
	endrain: 102,
	scan: 1480,
	type: WeightClass.Light),
new Heads(
	name: Parts.HDS808,
	display: "Aerobolt",
	hp: 160,
	sp: 2150,
	mpu: 2,
	size: 225,
	weight: 4100,
	endrain: 85,
	scan: 1800,
	type: WeightClass.Light),
new Heads(
	name: Parts.HDS003,
	display: "Rushnik",
	hp: 165,
	sp: 1406,
	mpu: 1,
	size: 496,
	weight: 4428,
	endrain: 109,
	scan: 1520,
	type: WeightClass.Light),
new Heads(
	name: Parts.HDM002,
	display: "Trooper",
	hp: 170,
	sp: 1200,
	mpu: 2,
	size: 152,
	weight: 5099,
	endrain: 115,
	scan: 1680,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HGS001,
	display: "Valkyrie",
	hp: 170,
	sp: 2200,
	mpu: 2,
	size: 250,
	weight: 4000,
	endrain: 87,
	scan: 1950,
	type: WeightClass.Light),

new Heads(
	name: Parts.HSS001,
	display: "Gamma Ray",
	hp: 170,
	sp: 2000,
	mpu: 0,
	size: 300,
	weight: 4300,
	endrain: 80,
	scan: 1700,
	type: WeightClass.Light),
new Heads(
	name: Parts.HDS818,
	display: "Centurion",
	hp: 180,
	sp: 2275,
	mpu: 3,
	size: 355,
	weight: 4350,
	endrain: 92,
	scan: 1850,
	type: WeightClass.Light),
new Heads(
	name: Parts.HDS107,
	display: "Scootwing",
	hp: 182,
	sp: 1635,
	mpu: 3,
	size: 640,
	weight: 4421,
	endrain: 128,
	scan: 1600,
	type: WeightClass.Light),
new Heads(
	name: Parts.HDM007,
	display: "Stark",
	hp: 185,
	sp: 1300,
	mpu: 2,
	size: 368,
	weight: 5091,
	endrain: 115,
	scan: 1520,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HDM709,
	display: "Hellfire",
	hp: 194,
	sp: 2360,
	mpu: 3,
	size: 616,
	weight: 4419,
	endrain: 115,
	scan: 1880,
	type: WeightClass.Light),
new Heads(
	name: Parts.HDS308,
	display: "Frontliner",
	hp: 195,
	sp: 1109,
	mpu: 1,
	size: 512,
	weight: 5096,
	endrain: 77,
	scan: 1200,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HDH004,
	display: "Boomrocker",
	hp: 210,
	sp: 960,
	mpu: 2,
	size: 264,
	weight: 6096,
	endrain: 102,
	scan: 1160,
	type: WeightClass.Heavy),
new Heads(
	name: Parts.HGM002,
	display: "Luciferiel",
	hp: 220,
	sp: 2450,
	mpu: 3,
	size: 550,
	weight: 4000,
	endrain: 107,
	scan: 2050,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HDM505,
	display: "Davenstar",
	hp: 224,
	sp: 1700,
	mpu: 2,
	size: 656,
	weight: 5087,
	endrain: 102,
	scan: 1350,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HDM803,
	display: "Zeeker",
	hp: 225,
	sp: 2400,
	mpu: 2,
	size: 450,
	weight: 4360,
	endrain: 122,
	scan: 1850,
	type: WeightClass.Light),
new Heads(
	name: Parts.HDM809,
	display: "Sidewinder",
	hp: 225,
	sp: 2400,
	mpu: 0,
	size: 485,
	weight: 5000,
	endrain: 110,
	scan: 1900,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HDS707,
	display: "Spazmok",
	hp: 225,
	sp: 2560,
	mpu: 4,
	size: 928,
	weight: 4415,
	endrain: 83,
	scan: 1920,
	type: WeightClass.Light),
new Heads(
	name: Parts.HDH009,
	display: "Haskell",
	hp: 226,
	sp: 1630,
	mpu: 3,
	size: 1584,
	weight: 6093,
	endrain: 58,
	scan: 1300,
	type: WeightClass.Heavy),
new Heads(
	name: Parts.HDH701,
	display: "Blitzker",
	hp: 231,
	sp: 1825,
	mpu: 4,
	size: 384,
	weight: 6083,
	endrain: 166,
	scan: 1550,
	type: WeightClass.Heavy),
new Heads(
	name: Parts.HSM002,
	display: "Sting X",
	hp: 235,
	sp: 2250,
	mpu: 3,
	size: 455,
	weight: 4955,
	endrain: 105,
	scan: 1900,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HDM819,
	display: "Vindicator",
	hp: 240,
	sp: 2450,
	mpu: 4,
	size: 698,
	weight: 5321,
	endrain: 100,
	scan: 1900,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HDJ606,
	display: "Jaywalker",
	hp: 243,
	sp: 2496,
	mpu: 2,
	size: 480,
	weight: 4260,
	endrain: 78,
	scan: 1840,
	type: WeightClass.Light),
new Heads(
	name: Parts.HGH003,
	display: "Aerene",
	hp: 250,
	sp: 2600,
	mpu: 4,
	size: 880,
	weight: 5250,
	endrain: 153,
	scan: 1900,
	type: WeightClass.Heavy),
new Heads(
	name: Parts.HDM806,
	display: "Ballistika",
	hp: 267,
	sp: 1632,
	mpu: 2,
	size: 440,
	weight: 5080,
	endrain: 147,
	scan: 3000,
	type: WeightClass.Standard),
new Heads(
	name: Parts.HDH820,
	display: "Guardian",
	hp: 275,
	sp: 2550,
	mpu: 3,
	size: 988,
	weight: 5600,
	endrain: 138,
	scan: 1950,
	type: WeightClass.Heavy),
new Heads(
	name: Parts.HDH802,
	display: "Hellbent Butcher",
	hp: 277,
	sp: 2000,
	mpu: 4,
	size: 1008,
	weight: 6081,
	endrain: 172,
	scan: 1650,
	type: WeightClass.Heavy),
new Heads(
	name: Parts.HB,
	display: "Big Mammoth",
	hp: 280,
	sp: 2300,
	mpu: 3,
	size: 930,
	weight: 5300,
	endrain: 130,
	scan: 2200,
	type: WeightClass.Heavy),
new Heads(
	name: Parts.HDH810,
	display: "Vigilant",
	hp: 300,
	sp: 2660,
	mpu: 3,
	size: 1308,
	weight: 6380,
	endrain: 180,
	scan: 1850,
	type: WeightClass.Heavy),
new Heads(
	name: Parts.HDW201,
	display: "Bastion",
	hp: 420,
	sp: 1825,
	mpu: 1,
	size: 152,
	weight: 5086,
	endrain: 166,
	scan: 1450,
	type: WeightClass.Standard)
				};
		}
		#endregion
	}
	public class Boosters : Part
	{
		#region Variables
		/// <summary>
		/// 
		/// </summary>
		public int DashOutput { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int DashDrainEN { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int JumpDrainEN { get; set; }
		#endregion

		#region Constructor
		public Boosters(Parts booster)
		{
			Boosters Booster = GetPart(booster);
			PartName = Booster.PartName;
			DisplayName = Booster.DisplayName;
			RankRequired = Booster.RankRequired;
			HP = Booster.HP;
			Size = Booster.Size;
			Weight = Booster.Weight;
			EnergyDrain = Booster.EnergyDrain;
			DashOutput = Booster.DashOutput;
			DashDrainEN = Booster.DashDrainEN;
			JumpDrainEN = Booster.JumpDrainEN;
			WeightSeries = Booster.WeightSeries;
		}
		public Boosters(Parts? name = null, string display = null, byte rank = 0, int hp = 0, int size = 0, int weight = 0, int endrain = 0, int dashout = 0, int dashdrain = 0, int jumpout = 0, WeightClass? type = null)
		{
			PartName = name.HasValue ? name.Value : Parts.AGM001;
			DisplayName = display ?? "GameMaster";
			RankRequired = rank;
			HP = hp;
			Size = size;
			Weight = weight;
			EnergyDrain = endrain;
			DashOutput = dashout;
			DashDrainEN = dashdrain;
			JumpDrainEN = jumpout;
			WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion

		#region Methods
		internal Boosters GetPart(Parts ID)
		{
			foreach (Boosters part in Database)
			{
				if (part.PartName == ID) return part;
			}
			throw new System.Exception("Part ID doesnt exist in the database. Please check Boosters constructor.");
		}
		#endregion

		#region Database
		private static readonly Boosters[] Database;
		static Boosters()
		{
			Database = new Boosters[] { 
//(\w+)\s+(\w+[\s]?\w+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\w+)
//Name    DisplayName HP  EN  SP  MPU Size    Weight  BasicSpeed  EN_Output   MinENRequired   ENDrain Scan    MaxHeat CoolDown    Mark    Capacity    Deceleration    DashOutput  DashENDrain JumpENDrain WeightType
//new Boosters(\n\tname: Parts.$1,\n\tdisplay: "$2",\n\thp: $3,\n\tsize: $7,\n\tweight: $8,\n\tendrain: $12,\n\tdashout: $19,\n\tdashdrain: $20,\n\tjumpout: $21,\n\ttype: WeightClass.$22),
new Boosters(
	name: Parts.PGM001,
	display: "GameMaster",
	hp: 0,
	size: 0,
	weight: 0,
	endrain: 0,
	dashout: 0,
	dashdrain: 0,
	jumpout: 0,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PSS001,
	display: "Scorpion-S",
	hp: 40,
	size: 1500,
	weight: 5250,
	endrain: 45,
	dashout: 490,
	dashdrain: 370,
	jumpout: 270,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PSD005,
	display: "StormMover",
	hp: 50,
	size: 1570,
	weight: 5110,
	endrain: 71,
	dashout: 485,
	dashdrain: 366,
	jumpout: 350,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PBS006,
	display: "Cyclone",
	hp: 60,
	size: 1150,
	weight: 4980,
	endrain: 43,
	dashout: 538,
	dashdrain: 375,
	jumpout: 240,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PGS001,
	display: "Steel Heart",
	hp: 65,
	size: 1050,
	weight: 4850,
	endrain: 40,
	dashout: 520,
	dashdrain: 370,
	jumpout: 240,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PSH003,
	display: "Dispenser-S",
	hp: 70,
	size: 1500,
	weight: 5500,
	endrain: 33,
	dashout: 485,
	dashdrain: 350,
	jumpout: 260,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PBS016,
	display: "Flanker",
	hp: 70,
	size: 1265,
	weight: 5100,
	endrain: 48,
	dashout: 510,
	dashdrain: 355,
	jumpout: 240,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PRC017,
	display: "Fever Rain",
	hp: 75,
	size: 1530,
	weight: 5165,
	endrain: 47,
	dashout: 493,
	dashdrain: 370,
	jumpout: 241,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PBS003,
	display: "SonicBoomer",
	hp: 78,
	size: 1230,
	weight: 5030,
	endrain: 55,
	dashout: 533,
	dashdrain: 386,
	jumpout: 247,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PSD004,
	display: "Sentinel",
	hp: 78,
	size: 1230,
	weight: 5050,
	endrain: 55,
	dashout: 533,
	dashdrain: 390,
	jumpout: 290,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PRC009,
	display: "LaserWing",
	hp: 80,
	size: 1650,
	weight: 5190,
	endrain: 45,
	dashout: 488,
	dashdrain: 389,
	jumpout: 280,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PRC008,
	display: "Tomahawk-S",
	hp: 80,
	size: 1650,
	weight: 5140,
	endrain: 49,
	dashout: 425,
	dashdrain: 385,
	jumpout: 294,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PBM002,
	display: "Booster",
	hp: 81,
	size: 1450,
	weight: 5100,
	endrain: 32,
	dashout: 489,
	dashdrain: 325,
	jumpout: 285,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PRC012,
	display: "MissileLauncher",
	hp: 81,
	size: 1620,
	weight: 5120,
	endrain: 44,
	dashout: 440,
	dashdrain: 351,
	jumpout: 237,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PBS000,
	display: "P-Booster",
	hp: 81,
	size: 1450,
	weight: 5210,
	endrain: 53,
	dashout: 488,
	dashdrain: 388,
	jumpout: 300,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PBS004,
	display: "Dispenser",
	hp: 83,
	size: 1420,
	weight: 5090,
	endrain: 30,
	dashout: 501,
	dashdrain: 320,
	jumpout: 240,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PGM002,
	display: "Legionnaire Wing",
	hp: 85,
	size: 1150,
	weight: 4900,
	endrain: 32,
	dashout: 495,
	dashdrain: 330,
	jumpout: 260,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PBS017,
	display: "Prowler",
	hp: 90,
	size: 1480,
	weight: 5100,
	endrain: 38,
	dashout: 500,
	dashdrain: 315,
	jumpout: 240,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PBS007,
	display: "Maverick",
	hp: 95,
	size: 1580,
	weight: 5180,
	endrain: 45,
	dashout: 518,
	dashdrain: 330,
	jumpout: 240,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PSM002,
	display: "Strike Force",
	hp: 130,
	size: 1750,
	weight: 5450,
	endrain: 44,
	dashout: 490,
	dashdrain: 375,
	jumpout: 240,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PBS008,
	display: "Argus",
	hp: 180,
	size: 1700,
	weight: 5300,
	endrain: 58,
	dashout: 485,
	dashdrain: 355,
	jumpout: 265,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PBS018,
	display: "Drift Tail",
	hp: 250,
	size: 1765,
	weight: 5396,
	endrain: 53,
	dashout: 468,
	dashdrain: 390,
	jumpout: 275,
	type: WeightClass.Standard),
new Boosters(
	name: Parts.PGH003,
	display: "Tonel Boomer",
	hp: 320,
	size: 1310,
	weight: 4998,
	endrain: 58,
	dashout: 462,
	dashdrain: 401,
	jumpout: 287,
	type: WeightClass.Standard)
				};
		}
		#endregion
	}
	public abstract class Part
	{
		/// <summary>
		/// Name of the set, this part belongs to
		/// </summary>
		public string SetName { get; set; }
		/// <summary>
		/// Internal name of this individual part
		/// </summary>
		public Parts PartName { get; protected set; }
		/// <summary>
		/// Name of this this part when viewed in-game from shop or inventory
		/// </summary>
		public string DisplayName { get; protected set; }
		/// <summary>
		/// 
		/// </summary>
		public WeightClass WeightSeries { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Weight { get; set; }
		/// <summary>
		/// An Array of this Unit's Price
		/// If value is null, then option does not exist
		/// Index[1] : Credits
		/// Index[2] : Coins
		/// </summary>
		public int?[] Price { get; set; }
		/// <summary>
		/// Rank required to purchase or equip part
		/// 
		/// </summary>
		public byte RankRequired { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Durability { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Size { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int HP { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int EnergyDrain { get; set; }
		//public int MoveSpeed { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int RecoveryEN { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Description { get; set; }
	}
	#endregion
}