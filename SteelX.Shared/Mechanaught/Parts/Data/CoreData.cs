namespace SteelX.Shared
{
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
}