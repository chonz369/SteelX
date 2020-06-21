namespace SteelX.Shared
{
	public struct HeadData
	{
		#region Variables
		public Parts Part { get; set; }
		public int Id { get; private set; }
		public int NPCPart { get; private set; }
		public string Model { get; private set; }
		public string Name { get; private set; }
		public int Weight { get; private set; }
		public int HitPoint { get; private set; }
		public int Size { get; private set; }
		public int EnergyDrain { get; private set; }
		/// <summary>
		/// MPU sets how many <see cref="Skills"/> can be equipped on the mechanaught
		/// </summary>
		public int MPU { get; private set; }
		/// <summary>
		/// SP sets the minimum Skill Points of the Mechanaught
		/// </summary>
		public int SPCache { get; private set; }
		/// <summary>
		/// Scan Range sets the minimum scan range of the mechanaught.
		/// </summary>
		public int ScanRange { get; private set; }
		public int IFOSize { get; private set; }
		public int BoneType { get; private set; }
		public float RepairTime { get; private set; }
		public int Grade { get; private set; }
		public string Description { get; private set; }
		#endregion
	}
}