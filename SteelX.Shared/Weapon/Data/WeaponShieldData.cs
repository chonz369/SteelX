namespace SteelX.Shared
{
	public struct WeaponShieldData : IWeaponData
	{
		public int Id { get; private set; }
		public bool NPCPart { get; private set; }
		public string Model { get; private set; }
		public string Name { get; private set; }
		public WeaponTypes WeaponType { get; private set; }
		public int Weight { get; private set; }
		public int Size { get; private set; }
		public int IFOSize { get; private set; }
		public int EnergyDrain { get; private set; }
		public bool TwoHanded { get; private set; }
		public int DfnMelee { get; private set; }
		public int DfnRange { get; private set; }
		public int DfnSiege { get; private set; }
		public int DfnAngle { get; private set; }
		public float OverheatPoint { get; private set; }
		public float OverheatRecovery { get; private set; }
		public float RepairTime { get; private set; }
		public int Grade { get; private set; }
		public string Description { get; private set; }
	}
}