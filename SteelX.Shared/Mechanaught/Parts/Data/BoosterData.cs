namespace SteelX.Shared
{
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