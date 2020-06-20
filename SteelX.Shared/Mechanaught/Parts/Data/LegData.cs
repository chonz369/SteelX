namespace SteelX.Shared
{
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
}