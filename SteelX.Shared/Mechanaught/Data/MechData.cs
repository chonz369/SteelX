namespace SteelX.Shared
{
	/// <summary>
	/// Use this struct to store and load essential mech properties
	/// between node endpoints
	/// </summary>
	public struct MechData
	{
		public PartMetaData Arms { get; set; }
		public PartMetaData Legs { get; set; }
		public PartMetaData Core { get; set; }
		public PartMetaData Head { get; set; }
		public PartMetaData Booster { get; set; }
		public MechWeapon[] WeaponSet { get; set; }
		public Skills[] Skills { get; set; }
	}
}