namespace SteelX.Shared
{
	public struct ArmData
	{
		#region Variables
		public Parts Part { get; set; }
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
		public ArmData(Parts? name = null, string display = null, byte rank = 0, int hp = 0, int size = 0, int weight = 0, int endrain = 0, int heat = 0, int cooldown = 0, int mark = 0, WeightClass? type = null)
		{
			Part = name.HasValue ? name.Value : Parts.AES008;
			//PartName = name.HasValue ? name.Value : Parts.AGM001;
			//DisplayName = display ?? "GameMaster";
			//RankRequired = rank;
			//HP = hp;
			//Size = size;
			//Weight = weight;
			//EnergyDrain = endrain;
			MaxHeat = heat;
			CooldownRate = cooldown;
			Marksmanship = mark;
			//WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion
	}
	public struct LegData
	{
		#region Variables
		public Parts Part { get; set; }
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
		public LegData(Parts? name = null, string display = null, byte rank = 0, int hp = 0, int size = 0, int weight = 0, int speed = 0, int endrain = 0, int capacity = 0, int deceleration = 0, WeightClass? type = null)
		{
			Part = name.HasValue ? name.Value : Parts.LTS008;
			//PartName = name.HasValue ? name.Value : Parts.LGM001;
			//DisplayName = display ?? "GameMaster";
			//RankRequired = rank;
			//HP = hp;
			//Size = size;
			//Weight = weight;
			BasicSpeed = speed;
			//EnergyDrain = endrain;
			Capacity = capacity;
			Deceleration = deceleration;
			//WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion
	}
	public struct CoreData
	{
		#region Variables
		public Parts Part { get; set; }
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
		public CoreData(Parts? name = null, string display = null, byte rank = 0, int hp = 0, int en = 0, int size = 0, int weight = 0, int enoutput = 0, int minimumen = 0, int endrain = 0, WeightClass? type = null)
		{
			Part = name.HasValue ? name.Value : Parts.CES008;
			//PartName = name.HasValue ? name.Value : Parts.CGM001;
			//DisplayName = display ?? "GameMaster";
			//RankRequired = rank;
			//HP = hp;
			EN = en;
			//Size = size;
			//Weight = weight;
			OutputRate = enoutput;
			MinEN = minimumen;
			//EnergyDrain = endrain;
			//WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion
	}
	public struct HeadData
	{
		#region Variables
		public Parts Part { get; set; }
		/// <summary>
		/// SP sets the minimum Skill Points of the Mechanaught
		/// </summary>
		public int SP { get; set; }
		/// <summary>
		/// MPU sets how many <see cref="Skills"/> can be equipped on the mechanaught
		/// </summary>
		public int MPU { get; set; }
		/// <summary>
		/// Scan Range sets the minimum scan range of the mechanaught.
		/// </summary>
		public int ScanRange { get; set; }
		#endregion

		#region Constructor
		public HeadData(Parts? name = null, string display = null, byte rank = 0, int hp = 0, int sp = 0, int mpu = 0, int size = 0, int weight = 0, int endrain = 0, int scan = 0, WeightClass? type = null)
		{
			Part = name.HasValue ? name.Value : Parts.HDS008;
			//PartName = name.HasValue ? name.Value : Parts.HGM001;
			//DisplayName = display ?? "GameMaster";
			//RankRequired = rank;
			//HP = hp;
			SP = sp;
			MPU = mpu;
			//Size = size;
			//Weight = weight;
			//EnergyDrain = endrain;
			ScanRange = scan;
			//WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion
	}
	public struct BoosterData
	{
		#region Variables
		public Parts Part { get; set; }
		/// <summary>
		/// Rate of speed that is increased when dashing.
		/// </summary>
		public int DashOutput { get; set; }
		/// <summary>
		/// How fast Energy is drained when dashing.
		/// </summary>
		public int DashDrainEN { get; set; }
		/// <summary>
		/// How fast Energy is drained when jumping.
		/// </summary>
		public int JumpDrainEN { get; set; }
		#endregion

		#region Constructor
		public BoosterData(Parts? name = null, string display = null, byte rank = 0, int hp = 0, int size = 0, int weight = 0, int endrain = 0, int dashout = 0, int dashdrain = 0, int jumpout = 0, WeightClass? type = null)
		{
			Part = name.HasValue ? name.Value : Parts.PRC008;
			//PartName = name.HasValue ? name.Value : Parts.PGM001;
			//DisplayName = display ?? "GameMaster";
			//RankRequired = rank;
			//HP = hp;
			//Size = size;
			//Weight = weight;
			//EnergyDrain = endrain;
			DashOutput = dashout;
			DashDrainEN = dashdrain;
			JumpDrainEN = jumpout;
			//WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion
	}
}