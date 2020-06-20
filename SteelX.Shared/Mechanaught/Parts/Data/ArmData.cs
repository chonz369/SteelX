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
}