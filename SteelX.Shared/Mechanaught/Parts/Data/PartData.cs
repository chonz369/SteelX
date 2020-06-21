namespace SteelX.Shared
{
	public struct PartData
	{
		/// <summary>
		/// Name of the set, this part belongs to
		/// </summary>
		public string SetName { get; set; }
		/// <summary>
		/// Internal name of this individual part
		/// </summary>
		public Parts PartName { get; private set; }
		public MechSlots PartType { get; private set; }
		/// <summary>
		/// Name of this this part when viewed in-game from shop or inventory
		/// </summary>
		public string DisplayName { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public WeightClass WeightSeries { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int Weight { get; private set; }
		/// <summary>
		/// An Array of this Unit's Price;
		/// If value is null, then option does not exist.
		/// Index[1] : Credits
		/// Index[2] : Coins
		/// </summary>
		public int?[] Price { get; private set; }
		/// <summary>
		/// Rank required to purchase or equip part
		/// </summary>
		public byte RankRequired { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int Durability { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int Size { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int HP { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int EnergyDrain { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int RecoveryEN { get; private set; }
		/// <summary>
		/// </summary>
		public int Description { get; private set; }
	}
}