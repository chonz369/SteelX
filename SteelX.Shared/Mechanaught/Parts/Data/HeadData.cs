namespace SteelX.Shared
{
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
}