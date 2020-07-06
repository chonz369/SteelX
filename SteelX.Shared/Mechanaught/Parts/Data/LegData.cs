namespace SteelX.Shared
{
	public struct LegData : IPartData
	{
		#region Variables
		public Parts Part { get; set; }
		public int Id { get; private set; }
		public bool NPCPart { get; private set; }
		public string Model { get; private set; }
		public string Name { get; private set; }
		public int Weight { get; private set; }
		public int HitPoint { get; private set; }
		public int LoadAbility { get; private set; }
		public int MoveSpeed { get; private set; }
		public int Size { get; private set; }
		public int EnergyDrain { get; private set; }
		public int BreakAbility { get; private set; }
		public int IFOSize { get; private set; }
		public int BoneType { get; private set; }
		public float RepairTime { get; private set; }
		public int Grade { get; private set; }
		public string Description { get; private set; }
		#endregion
	}
}